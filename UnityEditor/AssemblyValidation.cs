// Decompiled with JetBrains decompiler
// Type: AssemblyValidation
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

internal class AssemblyValidation
{
  private static Dictionary<RuntimePlatform, List<System.Type>> _rulesByPlatform;

  public static ValidationResult Validate(RuntimePlatform platform, IEnumerable<string> userAssemblies, params object[] options)
  {
    AssemblyValidation.WarmUpRulesCache();
    string[] strArray = userAssemblies as string[] ?? userAssemblies.ToArray<string>();
    if (strArray.Length != 0)
    {
      foreach (IValidationRule validationRule in AssemblyValidation.ValidationRulesFor(platform, options))
      {
        ValidationResult validationResult = validationRule.Validate((IEnumerable<string>) strArray, options);
        if (!validationResult.Success)
          return validationResult;
      }
    }
    return new ValidationResult() { Success = true };
  }

  private static void WarmUpRulesCache()
  {
    if (AssemblyValidation._rulesByPlatform != null)
      return;
    AssemblyValidation._rulesByPlatform = new Dictionary<RuntimePlatform, List<System.Type>>();
    System.Type[] types = typeof (AssemblyValidation).Assembly.GetTypes();
    // ISSUE: reference to a compiler-generated field
    if (AssemblyValidation.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      AssemblyValidation.\u003C\u003Ef__mg\u0024cache0 = new Func<System.Type, bool>(AssemblyValidation.IsValidationRule);
    }
    // ISSUE: reference to a compiler-generated field
    Func<System.Type, bool> fMgCache0 = AssemblyValidation.\u003C\u003Ef__mg\u0024cache0;
    foreach (System.Type type in ((IEnumerable<System.Type>) types).Where<System.Type>(fMgCache0))
      AssemblyValidation.RegisterValidationRule(type);
  }

  private static bool IsValidationRule(System.Type type)
  {
    return AssemblyValidation.ValidationRuleAttributesFor(type).Any<AssemblyValidationRule>();
  }

  private static IEnumerable<IValidationRule> ValidationRulesFor(RuntimePlatform platform, params object[] options)
  {
    return AssemblyValidation.ValidationRuleTypesFor(platform).Select<System.Type, IValidationRule>((Func<System.Type, IValidationRule>) (t => AssemblyValidation.CreateValidationRuleWithOptions(t, options))).Where<IValidationRule>((Func<IValidationRule, bool>) (v => v != null));
  }

  [DebuggerHidden]
  private static IEnumerable<System.Type> ValidationRuleTypesFor(RuntimePlatform platform)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    AssemblyValidation.\u003CValidationRuleTypesFor\u003Ec__Iterator0 typesForCIterator0 = new AssemblyValidation.\u003CValidationRuleTypesFor\u003Ec__Iterator0() { platform = platform };
    // ISSUE: reference to a compiler-generated field
    typesForCIterator0.\u0024PC = -2;
    return (IEnumerable<System.Type>) typesForCIterator0;
  }

  private static IValidationRule CreateValidationRuleWithOptions(System.Type type, params object[] options)
  {
    List<object> objectList = new List<object>((IEnumerable<object>) options);
    object[] array;
    ConstructorInfo constructorInfo;
    while (true)
    {
      array = objectList.ToArray();
      constructorInfo = AssemblyValidation.ConstructorFor(type, (IEnumerable<object>) array);
      if (constructorInfo == null)
      {
        if (objectList.Count != 0)
          objectList.RemoveAt(objectList.Count - 1);
        else
          goto label_4;
      }
      else
        break;
    }
    return (IValidationRule) constructorInfo.Invoke(array);
label_4:
    return (IValidationRule) null;
  }

  private static ConstructorInfo ConstructorFor(System.Type type, IEnumerable<object> options)
  {
    System.Type[] array = options.Select<object, System.Type>((Func<object, System.Type>) (o => o.GetType())).ToArray<System.Type>();
    return type.GetConstructor(array);
  }

  internal static void RegisterValidationRule(System.Type type)
  {
    foreach (AssemblyValidationRule assemblyValidationRule in AssemblyValidation.ValidationRuleAttributesFor(type))
      AssemblyValidation.RegisterValidationRuleForPlatform(assemblyValidationRule.Platform, type);
  }

  internal static void RegisterValidationRuleForPlatform(RuntimePlatform platform, System.Type type)
  {
    if (!AssemblyValidation._rulesByPlatform.ContainsKey(platform))
      AssemblyValidation._rulesByPlatform[platform] = new List<System.Type>();
    if (AssemblyValidation._rulesByPlatform[platform].IndexOf(type) == -1)
      AssemblyValidation._rulesByPlatform[platform].Add(type);
    AssemblyValidation._rulesByPlatform[platform].Sort((Comparison<System.Type>) ((a, b) => AssemblyValidation.CompareValidationRulesByPriority(a, b, platform)));
  }

  internal static int CompareValidationRulesByPriority(System.Type a, System.Type b, RuntimePlatform platform)
  {
    int num1 = AssemblyValidation.PriorityFor(a, platform);
    int num2 = AssemblyValidation.PriorityFor(b, platform);
    if (num1 == num2)
      return 0;
    return num1 >= num2 ? 1 : -1;
  }

  private static int PriorityFor(System.Type type, RuntimePlatform platform)
  {
    return AssemblyValidation.ValidationRuleAttributesFor(type).Where<AssemblyValidationRule>((Func<AssemblyValidationRule, bool>) (attr => attr.Platform == platform)).Select<AssemblyValidationRule, int>((Func<AssemblyValidationRule, int>) (attr => attr.Priority)).FirstOrDefault<int>();
  }

  private static IEnumerable<AssemblyValidationRule> ValidationRuleAttributesFor(System.Type type)
  {
    return type.GetCustomAttributes(true).OfType<AssemblyValidationRule>();
  }
}
