// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorAssemblies
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal static class EditorAssemblies
  {
    internal static List<RuntimeInitializeClassInfo> m_RuntimeInitializeClassInfoList;
    internal static int m_TotalNumRuntimeInitializeMethods;

    internal static Assembly[] loadedAssemblies { get; private set; }

    internal static IEnumerable<System.Type> loadedTypes
    {
      get
      {
        return ((IEnumerable<Assembly>) EditorAssemblies.loadedAssemblies).SelectMany<Assembly, System.Type>((Func<Assembly, IEnumerable<System.Type>>) (assembly => (IEnumerable<System.Type>) AssemblyHelper.GetTypesFromAssembly(assembly)));
      }
    }

    internal static IEnumerable<System.Type> SubclassesOf(System.Type parent)
    {
      return EditorAssemblies.loadedTypes.Where<System.Type>((Func<System.Type, bool>) (klass => klass.IsSubclassOf(parent)));
    }

    [RequiredByNativeCode]
    private static void SetLoadedEditorAssemblies(Assembly[] assemblies)
    {
      EditorAssemblies.loadedAssemblies = assemblies;
    }

    internal static void FindClassesThatImplementAnyInterface(List<System.Type> results, params System.Type[] interfaces)
    {
      results.AddRange(EditorAssemblies.loadedTypes.Where<System.Type>((Func<System.Type, bool>) (x => ((IEnumerable<System.Type>) interfaces).Any<System.Type>((Func<System.Type, bool>) (i => i.IsAssignableFrom(x) && i != x)))));
    }

    [RequiredByNativeCode]
    private static RuntimeInitializeClassInfo[] GetRuntimeInitializeClassInfos()
    {
      if (EditorAssemblies.m_RuntimeInitializeClassInfoList == null)
        return (RuntimeInitializeClassInfo[]) null;
      return EditorAssemblies.m_RuntimeInitializeClassInfoList.ToArray();
    }

    [RequiredByNativeCode]
    private static int GetTotalNumRuntimeInitializeMethods()
    {
      return EditorAssemblies.m_TotalNumRuntimeInitializeMethods;
    }

    private static void StoreRuntimeInitializeClassInfo(System.Type type, List<string> methodNames, List<RuntimeInitializeLoadType> loadTypes)
    {
      EditorAssemblies.m_RuntimeInitializeClassInfoList.Add(new RuntimeInitializeClassInfo()
      {
        assemblyName = type.Assembly.GetName().Name.ToString(),
        className = type.ToString(),
        methodNames = methodNames.ToArray(),
        loadTypes = loadTypes.ToArray()
      });
      EditorAssemblies.m_TotalNumRuntimeInitializeMethods += methodNames.Count;
    }

    private static void ProcessStaticMethodAttributes(System.Type type)
    {
      List<string> methodNames = (List<string>) null;
      List<RuntimeInitializeLoadType> loadTypes = (List<RuntimeInitializeLoadType>) null;
      MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      for (int index = 0; index < methods.GetLength(0); ++index)
      {
        MethodInfo methodInfo = methods[index];
        if (Attribute.IsDefined((MemberInfo) methodInfo, typeof (RuntimeInitializeOnLoadMethodAttribute)))
        {
          RuntimeInitializeLoadType initializeLoadType = RuntimeInitializeLoadType.AfterSceneLoad;
          object[] customAttributes = methodInfo.GetCustomAttributes(typeof (RuntimeInitializeOnLoadMethodAttribute), false);
          if (customAttributes != null && customAttributes.Length > 0)
            initializeLoadType = ((RuntimeInitializeOnLoadMethodAttribute) customAttributes[0]).loadType;
          if (methodNames == null)
          {
            methodNames = new List<string>();
            loadTypes = new List<RuntimeInitializeLoadType>();
          }
          methodNames.Add(methodInfo.Name);
          loadTypes.Add(initializeLoadType);
        }
        if (Attribute.IsDefined((MemberInfo) methodInfo, typeof (InitializeOnLoadMethodAttribute)))
        {
          try
          {
            methodInfo.Invoke((object) null, (object[]) null);
          }
          catch (TargetInvocationException ex)
          {
            Debug.LogError((object) ex.InnerException);
          }
        }
      }
      if (methodNames == null)
        return;
      EditorAssemblies.StoreRuntimeInitializeClassInfo(type, methodNames, loadTypes);
    }

    private static void ProcessEditorInitializeOnLoad(System.Type type)
    {
      try
      {
        RuntimeHelpers.RunClassConstructor(type.TypeHandle);
      }
      catch (TypeInitializationException ex)
      {
        Debug.LogError((object) ex.InnerException);
      }
    }

    [RequiredByNativeCode]
    private static int[] ProcessInitializeOnLoadAttributes()
    {
      List<int> intList = (List<int>) null;
      Assembly[] loadedAssemblies = EditorAssemblies.loadedAssemblies;
      EditorAssemblies.m_TotalNumRuntimeInitializeMethods = 0;
      EditorAssemblies.m_RuntimeInitializeClassInfoList = new List<RuntimeInitializeClassInfo>();
      for (int index = 0; index < loadedAssemblies.Length; ++index)
      {
        int initializeMethods = EditorAssemblies.m_TotalNumRuntimeInitializeMethods;
        int count = EditorAssemblies.m_RuntimeInitializeClassInfoList.Count;
        try
        {
          foreach (System.Type type in AssemblyHelper.GetTypesFromAssembly(loadedAssemblies[index]))
          {
            if (type.IsDefined(typeof (InitializeOnLoadAttribute), false))
              EditorAssemblies.ProcessEditorInitializeOnLoad(type);
            EditorAssemblies.ProcessStaticMethodAttributes(type);
          }
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
          if (intList == null)
            intList = new List<int>();
          if (initializeMethods != EditorAssemblies.m_TotalNumRuntimeInitializeMethods)
            EditorAssemblies.m_TotalNumRuntimeInitializeMethods = initializeMethods;
          if (count != EditorAssemblies.m_RuntimeInitializeClassInfoList.Count)
            EditorAssemblies.m_RuntimeInitializeClassInfoList.RemoveRange(count, EditorAssemblies.m_RuntimeInitializeClassInfoList.Count - count);
          intList.Add(index);
        }
      }
      if (intList == null)
        return (int[]) null;
      return intList.ToArray();
    }
  }
}
