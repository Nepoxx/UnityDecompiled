// Decompiled with JetBrains decompiler
// Type: UnityEditor.AttributeHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal class AttributeHelper
  {
    private static Dictionary<System.Type, AttributeHelper.MethodInfoSorter> m_Cache = new Dictionary<System.Type, AttributeHelper.MethodInfoSorter>();

    [RequiredByNativeCode]
    private static AttributeHelper.MonoGizmoMethod[] ExtractGizmos(Assembly assembly)
    {
      List<AttributeHelper.MonoGizmoMethod> monoGizmoMethodList = new List<AttributeHelper.MonoGizmoMethod>();
      foreach (System.Type type in AssemblyHelper.GetTypesFromAssembly(assembly))
      {
        MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        for (int index = 0; index < methods.GetLength(0); ++index)
        {
          MethodInfo methodInfo = methods[index];
          foreach (DrawGizmo customAttribute in methodInfo.GetCustomAttributes(typeof (DrawGizmo), false))
          {
            System.Reflection.ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters.Length != 2)
              UnityEngine.Debug.LogWarning((object) string.Format("Method {0}.{1} is marked with the DrawGizmo attribute but does not take parameters (ComponentType, GizmoType) so will be ignored.", (object) methodInfo.DeclaringType.FullName, (object) methodInfo.Name));
            else if (methodInfo.DeclaringType != null && methodInfo.DeclaringType.IsGenericTypeDefinition)
            {
              UnityEngine.Debug.LogWarning((object) string.Format("Method {0}.{1} is marked with the DrawGizmo attribute but is defined on a generic type definition, so will be ignored.", (object) methodInfo.DeclaringType.FullName, (object) methodInfo.Name));
            }
            else
            {
              AttributeHelper.MonoGizmoMethod monoGizmoMethod = new AttributeHelper.MonoGizmoMethod();
              if (customAttribute.drawnType == null)
                monoGizmoMethod.drawnType = parameters[0].ParameterType;
              else if (parameters[0].ParameterType.IsAssignableFrom(customAttribute.drawnType))
              {
                monoGizmoMethod.drawnType = customAttribute.drawnType;
              }
              else
              {
                UnityEngine.Debug.LogWarning((object) string.Format("Method {0}.{1} is marked with the DrawGizmo attribute but the component type it applies to could not be determined.", (object) methodInfo.DeclaringType.FullName, (object) methodInfo.Name));
                continue;
              }
              if (parameters[1].ParameterType != typeof (GizmoType) && parameters[1].ParameterType != typeof (int))
              {
                UnityEngine.Debug.LogWarning((object) string.Format("Method {0}.{1} is marked with the DrawGizmo attribute but does not take a second parameter of type GizmoType so will be ignored.", (object) methodInfo.DeclaringType.FullName, (object) methodInfo.Name));
              }
              else
              {
                monoGizmoMethod.drawGizmo = methodInfo;
                monoGizmoMethod.options = (int) customAttribute.drawOptions;
                monoGizmoMethodList.Add(monoGizmoMethod);
              }
            }
          }
        }
      }
      return monoGizmoMethodList.ToArray();
    }

    [RequiredByNativeCode]
    private static string GetComponentMenuName(System.Type type)
    {
      object[] customAttributes = type.GetCustomAttributes(typeof (AddComponentMenu), false);
      if (customAttributes.Length > 0)
        return ((AddComponentMenu) customAttributes[0]).componentMenu;
      return (string) null;
    }

    [RequiredByNativeCode]
    private static int GetComponentMenuOrdering(System.Type type)
    {
      object[] customAttributes = type.GetCustomAttributes(typeof (AddComponentMenu), false);
      if (customAttributes.Length > 0)
        return ((AddComponentMenu) customAttributes[0]).componentOrder;
      return 0;
    }

    [RequiredByNativeCode]
    private static AttributeHelper.MonoCreateAssetItem[] ExtractCreateAssetMenuItems(Assembly assembly)
    {
      List<AttributeHelper.MonoCreateAssetItem> monoCreateAssetItemList = new List<AttributeHelper.MonoCreateAssetItem>();
      foreach (System.Type type in AssemblyHelper.GetTypesFromAssembly(assembly))
      {
        CreateAssetMenuAttribute customAttribute = (CreateAssetMenuAttribute) Attribute.GetCustomAttribute((MemberInfo) type, typeof (CreateAssetMenuAttribute));
        if (customAttribute != null)
        {
          if (!type.IsSubclassOf(typeof (ScriptableObject)))
          {
            UnityEngine.Debug.LogWarningFormat("CreateAssetMenu attribute on {0} will be ignored as {0} is not derived from ScriptableObject.", (object) type.FullName);
          }
          else
          {
            string str = !string.IsNullOrEmpty(customAttribute.menuName) ? customAttribute.menuName : ObjectNames.NicifyVariableName(type.Name);
            string path = !string.IsNullOrEmpty(customAttribute.fileName) ? customAttribute.fileName : "New " + ObjectNames.NicifyVariableName(type.Name) + ".asset";
            if (!Path.HasExtension(path))
              path += ".asset";
            monoCreateAssetItemList.Add(new AttributeHelper.MonoCreateAssetItem()
            {
              menuItem = str,
              fileName = path,
              order = customAttribute.order,
              type = type
            });
          }
        }
      }
      return monoCreateAssetItemList.ToArray();
    }

    internal static ArrayList FindEditorClassesWithAttribute(System.Type attribType)
    {
      ArrayList arrayList = new ArrayList();
      foreach (System.Type loadedType in EditorAssemblies.loadedTypes)
      {
        if (loadedType.GetCustomAttributes(attribType, false).Length != 0)
          arrayList.Add((object) loadedType);
      }
      return arrayList;
    }

    internal static object InvokeMemberIfAvailable(object target, string methodName, object[] args)
    {
      MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (method != null)
        return method.Invoke(target, args);
      return (object) null;
    }

    internal static bool GameObjectContainsAttribute(GameObject go, System.Type attributeType)
    {
      foreach (Component component in go.GetComponents(typeof (Component)))
      {
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.GetType().GetCustomAttributes(attributeType, true).Length > 0)
          return true;
      }
      return false;
    }

    [DebuggerHidden]
    internal static IEnumerable<T> CallMethodsWithAttribute<T>(System.Type attributeType, params object[] arguments)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AttributeHelper.\u003CCallMethodsWithAttribute\u003Ec__Iterator0<T> attributeCIterator0 = new AttributeHelper.\u003CCallMethodsWithAttribute\u003Ec__Iterator0<T>() { attributeType = attributeType, arguments = arguments };
      // ISSUE: reference to a compiler-generated field
      attributeCIterator0.\u0024PC = -2;
      return (IEnumerable<T>) attributeCIterator0;
    }

    private static bool AreSignaturesMatching(MethodInfo left, MethodInfo right)
    {
      if (left.IsStatic != right.IsStatic || left.ReturnType != right.ReturnType)
        return false;
      System.Reflection.ParameterInfo[] parameters1 = left.GetParameters();
      System.Reflection.ParameterInfo[] parameters2 = right.GetParameters();
      if (parameters1.Length != parameters2.Length)
        return false;
      for (int index = 0; index < parameters1.Length; ++index)
      {
        if (parameters1[index].ParameterType != parameters2[index].ParameterType)
          return false;
      }
      return true;
    }

    internal static string MethodToString(MethodInfo method)
    {
      return (!method.IsStatic ? "" : "static ") + method.ToString();
    }

    internal static bool MethodMatchesAnyRequiredSignatureOfAttribute(MethodInfo method, System.Type attributeType)
    {
      List<MethodInfo> source = new List<MethodInfo>();
      foreach (MethodInfo method1 in attributeType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic))
      {
        if (method1.GetCustomAttributes(typeof (RequiredSignatureAttribute), false).Length > 0)
        {
          if (AttributeHelper.AreSignaturesMatching(method, method1))
            return true;
          source.Add(method1);
        }
      }
      if (source.Count == 0)
        UnityEngine.Debug.LogError((object) (AttributeHelper.MethodToString(method) + " has an invalid attribute : " + (object) attributeType + ". " + (object) attributeType + " must have at least one required signature declaration"));
      else if (source.Count == 1)
        UnityEngine.Debug.LogError((object) (AttributeHelper.MethodToString(method) + " does not match " + (object) attributeType + " expected signature.\n Use " + AttributeHelper.MethodToString(source[0])));
      else
        UnityEngine.Debug.LogError((object) (AttributeHelper.MethodToString(method) + " does not match any of " + (object) attributeType + " expected signatures.\n Valid signatures are: " + string.Join(" , ", source.Select<MethodInfo, string>((Func<MethodInfo, string>) (a => AttributeHelper.MethodToString(a))).ToArray<string>())));
      return false;
    }

    internal static AttributeHelper.MethodInfoSorter GetMethodsWithAttribute<T>(BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) where T : Attribute
    {
      if (!AttributeHelper.m_Cache.ContainsKey(typeof (T)))
      {
        List<AttributeHelper.MethodWithAttribute> methodsWithAttributes = new List<AttributeHelper.MethodWithAttribute>();
        foreach (Assembly loadedAssembly in EditorAssemblies.loadedAssemblies)
        {
          foreach (System.Type type in AssemblyHelper.GetTypesFromAssembly(loadedAssembly))
          {
            foreach (MethodInfo method in type.GetMethods(flags))
            {
              object[] customAttributes = method.GetCustomAttributes(typeof (T), false);
              if (customAttributes.Length > 0)
              {
                if (method.IsGenericMethod)
                {
                  UnityEngine.Debug.LogError((object) (AttributeHelper.MethodToString(method) + " is a generic method. " + typeof (T).ToString() + " can't be applied to it."));
                }
                else
                {
                  foreach (T obj in customAttributes)
                  {
                    if (AttributeHelper.MethodMatchesAnyRequiredSignatureOfAttribute(method, typeof (T)))
                      methodsWithAttributes.Add(new AttributeHelper.MethodWithAttribute()
                      {
                        info = method,
                        attribute = (Attribute) obj
                      });
                  }
                }
              }
            }
          }
        }
        AttributeHelper.m_Cache.Add(typeof (T), new AttributeHelper.MethodInfoSorter(methodsWithAttributes));
      }
      return AttributeHelper.m_Cache[typeof (T)];
    }

    private struct MonoGizmoMethod
    {
      public MethodInfo drawGizmo;
      public System.Type drawnType;
      public int options;
    }

    private struct MonoCreateAssetItem
    {
      public string menuItem;
      public string fileName;
      public int order;
      public System.Type type;
    }

    internal class MethodWithAttribute
    {
      public MethodInfo info;
      public Attribute attribute;
    }

    internal class MethodInfoSorter
    {
      internal MethodInfoSorter(List<AttributeHelper.MethodWithAttribute> methodsWithAttributes)
      {
        this.MethodsWithAttributes = methodsWithAttributes;
      }

      public IEnumerable<MethodInfo> FilterAndSortOnAttribute<T>(Func<T, bool> filter, Func<T, IComparable> sorter) where T : Attribute
      {
        return this.MethodsWithAttributes.Where<AttributeHelper.MethodWithAttribute>((Func<AttributeHelper.MethodWithAttribute, bool>) (a => filter((T) a.attribute))).OrderBy<AttributeHelper.MethodWithAttribute, IComparable>((Func<AttributeHelper.MethodWithAttribute, IComparable>) (c => sorter((T) c.attribute))).Select<AttributeHelper.MethodWithAttribute, MethodInfo>((Func<AttributeHelper.MethodWithAttribute, MethodInfo>) (o => o.info));
      }

      public List<AttributeHelper.MethodWithAttribute> MethodsWithAttributes { get; }
    }
  }
}
