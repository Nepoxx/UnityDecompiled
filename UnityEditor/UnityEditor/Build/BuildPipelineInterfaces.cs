// Decompiled with JetBrains decompiler
// Type: UnityEditor.Build.BuildPipelineInterfaces
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace UnityEditor.Build
{
  internal static class BuildPipelineInterfaces
  {
    private static BuildPipelineInterfaces.BuildCallbacks previousFlags = BuildPipelineInterfaces.BuildCallbacks.None;
    private static List<IPreprocessBuild> buildPreprocessors;
    private static List<IPostprocessBuild> buildPostprocessors;
    private static List<IProcessScene> sceneProcessors;
    private static List<IActiveBuildTargetChanged> buildTargetProcessors;

    private static int CompareICallbackOrder(IOrderedCallback a, IOrderedCallback b)
    {
      return a.callbackOrder - b.callbackOrder;
    }

    private static void AddToList<T>(object o, ref List<T> list) where T : class
    {
      T obj = o as T;
      if ((object) obj == null)
        return;
      if (list == null)
        list = new List<T>();
      list.Add(obj);
    }

    [RequiredByNativeCode]
    internal static void InitializeBuildCallbacks(BuildPipelineInterfaces.BuildCallbacks findFlags)
    {
      if (findFlags == BuildPipelineInterfaces.previousFlags)
        return;
      BuildPipelineInterfaces.previousFlags = findFlags;
      BuildPipelineInterfaces.CleanupBuildCallbacks();
      HashSet<string> stringSet = new HashSet<string>();
      stringSet.Add("UnityEditor");
      stringSet.Add("UnityEngine.UI");
      stringSet.Add("Unity.PackageManager");
      stringSet.Add("UnityEngine.Networking");
      stringSet.Add("nunit.framework");
      stringSet.Add("UnityEditor.TreeEditor");
      stringSet.Add("UnityEditor.Graphs");
      stringSet.Add("UnityEditor.UI");
      stringSet.Add("UnityEditor.TestRunner");
      stringSet.Add("UnityEngine.TestRunner");
      stringSet.Add("UnityEngine.HoloLens");
      stringSet.Add("SyntaxTree.VisualStudio.Unity.Bridge");
      stringSet.Add("UnityEditor.Android.Extensions");
      bool flag1 = (findFlags & BuildPipelineInterfaces.BuildCallbacks.BuildProcessors) == BuildPipelineInterfaces.BuildCallbacks.BuildProcessors;
      bool flag2 = (findFlags & BuildPipelineInterfaces.BuildCallbacks.SceneProcessors) == BuildPipelineInterfaces.BuildCallbacks.SceneProcessors;
      bool flag3 = (findFlags & BuildPipelineInterfaces.BuildCallbacks.BuildTargetProcessors) == BuildPipelineInterfaces.BuildCallbacks.BuildTargetProcessors;
      BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
      System.Type[] expectedArguments = new System.Type[2]{ typeof (BuildTarget), typeof (string) };
      for (int index1 = 0; index1 < EditorAssemblies.loadedAssemblies.Length; ++index1)
      {
        Assembly loadedAssembly = EditorAssemblies.loadedAssemblies[index1];
        bool flag4 = !stringSet.Contains(loadedAssembly.FullName.Substring(0, loadedAssembly.FullName.IndexOf(',')));
        System.Type[] types;
        try
        {
          types = loadedAssembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
          types = ex.Types;
        }
        for (int index2 = 0; index2 < types.Length; ++index2)
        {
          System.Type type = types[index2];
          if (type != null)
          {
            object o = (object) null;
            bool flag5 = false;
            if (flag1)
            {
              flag5 = typeof (IOrderedCallback).IsAssignableFrom(type);
              if (flag5)
              {
                if (BuildPipelineInterfaces.ValidateType<IPreprocessBuild>(type))
                {
                  o = Activator.CreateInstance(type);
                  BuildPipelineInterfaces.AddToList<IPreprocessBuild>(o, ref BuildPipelineInterfaces.buildPreprocessors);
                }
                if (BuildPipelineInterfaces.ValidateType<IPostprocessBuild>(type))
                {
                  o = o ?? Activator.CreateInstance(type);
                  BuildPipelineInterfaces.AddToList<IPostprocessBuild>(o, ref BuildPipelineInterfaces.buildPostprocessors);
                }
              }
            }
            if (flag2 && (!flag1 || flag5) && BuildPipelineInterfaces.ValidateType<IProcessScene>(type))
            {
              o = o ?? Activator.CreateInstance(type);
              BuildPipelineInterfaces.AddToList<IProcessScene>(o, ref BuildPipelineInterfaces.sceneProcessors);
            }
            if (flag3 && (!flag1 || flag5) && BuildPipelineInterfaces.ValidateType<IActiveBuildTargetChanged>(type))
              BuildPipelineInterfaces.AddToList<IActiveBuildTargetChanged>(o ?? Activator.CreateInstance(type), ref BuildPipelineInterfaces.buildTargetProcessors);
            if (flag4)
            {
              foreach (MethodInfo method in type.GetMethods(bindingAttr))
              {
                if (!method.IsSpecialName)
                {
                  if (flag1 && BuildPipelineInterfaces.ValidateMethod<PostProcessBuildAttribute>(method, expectedArguments))
                    BuildPipelineInterfaces.AddToList<IPostprocessBuild>((object) new BuildPipelineInterfaces.AttributeCallbackWrapper(method), ref BuildPipelineInterfaces.buildPostprocessors);
                  if (flag2 && BuildPipelineInterfaces.ValidateMethod<PostProcessSceneAttribute>(method, System.Type.EmptyTypes))
                    BuildPipelineInterfaces.AddToList<IProcessScene>((object) new BuildPipelineInterfaces.AttributeCallbackWrapper(method), ref BuildPipelineInterfaces.sceneProcessors);
                }
              }
            }
          }
        }
      }
      if (BuildPipelineInterfaces.buildPreprocessors != null)
      {
        List<IPreprocessBuild> buildPreprocessors = BuildPipelineInterfaces.buildPreprocessors;
        // ISSUE: reference to a compiler-generated field
        if (BuildPipelineInterfaces.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          BuildPipelineInterfaces.\u003C\u003Ef__mg\u0024cache0 = new Comparison<IPreprocessBuild>(BuildPipelineInterfaces.CompareICallbackOrder);
        }
        // ISSUE: reference to a compiler-generated field
        Comparison<IPreprocessBuild> fMgCache0 = BuildPipelineInterfaces.\u003C\u003Ef__mg\u0024cache0;
        buildPreprocessors.Sort(fMgCache0);
      }
      if (BuildPipelineInterfaces.buildPostprocessors != null)
      {
        List<IPostprocessBuild> buildPostprocessors = BuildPipelineInterfaces.buildPostprocessors;
        // ISSUE: reference to a compiler-generated field
        if (BuildPipelineInterfaces.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          BuildPipelineInterfaces.\u003C\u003Ef__mg\u0024cache1 = new Comparison<IPostprocessBuild>(BuildPipelineInterfaces.CompareICallbackOrder);
        }
        // ISSUE: reference to a compiler-generated field
        Comparison<IPostprocessBuild> fMgCache1 = BuildPipelineInterfaces.\u003C\u003Ef__mg\u0024cache1;
        buildPostprocessors.Sort(fMgCache1);
      }
      if (BuildPipelineInterfaces.buildTargetProcessors != null)
      {
        List<IActiveBuildTargetChanged> targetProcessors = BuildPipelineInterfaces.buildTargetProcessors;
        // ISSUE: reference to a compiler-generated field
        if (BuildPipelineInterfaces.\u003C\u003Ef__mg\u0024cache2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          BuildPipelineInterfaces.\u003C\u003Ef__mg\u0024cache2 = new Comparison<IActiveBuildTargetChanged>(BuildPipelineInterfaces.CompareICallbackOrder);
        }
        // ISSUE: reference to a compiler-generated field
        Comparison<IActiveBuildTargetChanged> fMgCache2 = BuildPipelineInterfaces.\u003C\u003Ef__mg\u0024cache2;
        targetProcessors.Sort(fMgCache2);
      }
      if (BuildPipelineInterfaces.sceneProcessors == null)
        return;
      List<IProcessScene> sceneProcessors = BuildPipelineInterfaces.sceneProcessors;
      // ISSUE: reference to a compiler-generated field
      if (BuildPipelineInterfaces.\u003C\u003Ef__mg\u0024cache3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        BuildPipelineInterfaces.\u003C\u003Ef__mg\u0024cache3 = new Comparison<IProcessScene>(BuildPipelineInterfaces.CompareICallbackOrder);
      }
      // ISSUE: reference to a compiler-generated field
      Comparison<IProcessScene> fMgCache3 = BuildPipelineInterfaces.\u003C\u003Ef__mg\u0024cache3;
      sceneProcessors.Sort(fMgCache3);
    }

    internal static bool ValidateType<T>(System.Type t)
    {
      return !t.IsInterface && !t.IsAbstract && typeof (T).IsAssignableFrom(t) && t != typeof (BuildPipelineInterfaces.AttributeCallbackWrapper);
    }

    private static bool ValidateMethod<T>(MethodInfo method, System.Type[] expectedArguments)
    {
      System.Type attributeType = typeof (T);
      if (!method.IsDefined(attributeType, false))
        return false;
      if (!method.IsStatic)
      {
        string str = attributeType.Name.Replace("Attribute", "");
        Debug.LogErrorFormat("Method {0} with {1} attribute must be static.", new object[2]
        {
          (object) method.Name,
          (object) str
        });
        return false;
      }
      if (method.IsGenericMethod || method.IsGenericMethodDefinition)
      {
        string str = attributeType.Name.Replace("Attribute", "");
        Debug.LogErrorFormat("Method {0} with {1} attribute cannot be generic.", new object[2]
        {
          (object) method.Name,
          (object) str
        });
        return false;
      }
      System.Reflection.ParameterInfo[] parameters = method.GetParameters();
      bool flag = parameters.Length == expectedArguments.Length;
      if (flag)
      {
        for (int index = 0; index < parameters.Length; ++index)
        {
          if (parameters[index].ParameterType != expectedArguments[index])
          {
            flag = false;
            break;
          }
        }
      }
      if (flag)
        return true;
      string str1 = attributeType.Name.Replace("Attribute", "");
      string str2 = "static void " + method.Name + "(";
      for (int index = 0; index < expectedArguments.Length; ++index)
      {
        str2 += expectedArguments[index].Name;
        if (index != expectedArguments.Length - 1)
          str2 += ", ";
      }
      string str3 = str2 + ")";
      Debug.LogErrorFormat("Method {0} with {1} attribute does not have the correct signature, expected: {2}.", (object) method.Name, (object) str1, (object) str3);
      return false;
    }

    [RequiredByNativeCode]
    internal static void OnBuildPreProcess(BuildTarget platform, string path, bool strict)
    {
      if (BuildPipelineInterfaces.buildPreprocessors == null)
        return;
      foreach (IPreprocessBuild buildPreprocessor in BuildPipelineInterfaces.buildPreprocessors)
      {
        try
        {
          buildPreprocessor.OnPreprocessBuild(platform, path);
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
          if (strict)
            break;
        }
      }
    }

    [RequiredByNativeCode]
    internal static void OnSceneProcess(Scene scene, bool strict)
    {
      if (BuildPipelineInterfaces.sceneProcessors == null)
        return;
      foreach (IProcessScene sceneProcessor in BuildPipelineInterfaces.sceneProcessors)
      {
        try
        {
          sceneProcessor.OnProcessScene(scene);
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
          if (strict)
            break;
        }
      }
    }

    [RequiredByNativeCode]
    internal static void OnBuildPostProcess(BuildTarget platform, string path, bool strict)
    {
      if (BuildPipelineInterfaces.buildPostprocessors == null)
        return;
      foreach (IPostprocessBuild buildPostprocessor in BuildPipelineInterfaces.buildPostprocessors)
      {
        try
        {
          buildPostprocessor.OnPostprocessBuild(platform, path);
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
          if (strict)
            break;
        }
      }
    }

    [RequiredByNativeCode]
    internal static void OnActiveBuildTargetChanged(BuildTarget previousPlatform, BuildTarget newPlatform)
    {
      if (BuildPipelineInterfaces.buildTargetProcessors == null)
        return;
      foreach (IActiveBuildTargetChanged buildTargetProcessor in BuildPipelineInterfaces.buildTargetProcessors)
      {
        try
        {
          buildTargetProcessor.OnActiveBuildTargetChanged(previousPlatform, newPlatform);
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
        }
      }
    }

    [RequiredByNativeCode]
    internal static void CleanupBuildCallbacks()
    {
      BuildPipelineInterfaces.buildTargetProcessors = (List<IActiveBuildTargetChanged>) null;
      BuildPipelineInterfaces.buildPreprocessors = (List<IPreprocessBuild>) null;
      BuildPipelineInterfaces.buildPostprocessors = (List<IPostprocessBuild>) null;
      BuildPipelineInterfaces.sceneProcessors = (List<IProcessScene>) null;
      BuildPipelineInterfaces.previousFlags = BuildPipelineInterfaces.BuildCallbacks.None;
    }

    [System.Flags]
    internal enum BuildCallbacks
    {
      None = 0,
      BuildProcessors = 1,
      SceneProcessors = 2,
      BuildTargetProcessors = 4,
    }

    private class AttributeCallbackWrapper : IPostprocessBuild, IProcessScene, IActiveBuildTargetChanged, IOrderedCallback
    {
      private int m_callbackOrder;
      private MethodInfo m_method;

      public AttributeCallbackWrapper(MethodInfo m)
      {
        this.m_callbackOrder = ((CallbackOrderAttribute) Attribute.GetCustomAttribute((MemberInfo) m, typeof (CallbackOrderAttribute))).callbackOrder;
        this.m_method = m;
      }

      public int callbackOrder
      {
        get
        {
          return this.m_callbackOrder;
        }
      }

      public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
      {
        this.m_method.Invoke((object) null, new object[2]
        {
          (object) previousTarget,
          (object) newTarget
        });
      }

      public void OnPostprocessBuild(BuildTarget target, string path)
      {
        this.m_method.Invoke((object) null, new object[2]
        {
          (object) target,
          (object) path
        });
      }

      public void OnProcessScene(Scene scene)
      {
        this.m_method.Invoke((object) null, (object[]) null);
      }
    }
  }
}
