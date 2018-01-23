// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetPostprocessingInternal
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal class AssetPostprocessingInternal
  {
    private static ArrayList m_PostprocessStack = (ArrayList) null;
    private static ArrayList m_ImportProcessors = (ArrayList) null;
    private static ArrayList m_PostprocessorClasses = (ArrayList) null;

    private static void LogPostProcessorMissingDefaultConstructor(System.Type type)
    {
      Debug.LogErrorFormat("{0} requires a default constructor to be used as an asset post processor", (object) type);
    }

    [RequiredByNativeCode]
    private static void PostprocessAllAssets(string[] importedAssets, string[] addedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPathAssets)
    {
      object[] parameters = new object[4]{ (object) importedAssets, (object) deletedAssets, (object) movedAssets, (object) movedFromPathAssets };
      foreach (System.Type type in EditorAssemblies.SubclassesOf(typeof (AssetPostprocessor)))
      {
        MethodInfo method = type.GetMethod("OnPostprocessAllAssets", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (method != null)
          method.Invoke((object) null, parameters);
      }
      SyncVS.PostprocessSyncProject(importedAssets, addedAssets, deletedAssets, movedAssets, movedFromPathAssets);
    }

    [RequiredByNativeCode]
    private static void PreprocessAssembly(string pathName)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPreprocessAssembly", (object[]) new string[1]
          {
            pathName
          });
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    internal static void CallOnGeneratedCSProjectFiles()
    {
      object[] parameters = new object[0];
      foreach (MethodBase methodBase in AssetPostprocessingInternal.AllPostProcessorMethodsNamed("OnGeneratedCSProjectFiles"))
        methodBase.Invoke((object) null, parameters);
    }

    internal static bool OnPreGeneratingCSProjectFiles()
    {
      object[] parameters = new object[0];
      bool flag = false;
      foreach (MethodInfo methodInfo in AssetPostprocessingInternal.AllPostProcessorMethodsNamed(nameof (OnPreGeneratingCSProjectFiles)))
      {
        object obj = methodInfo.Invoke((object) null, parameters);
        if (methodInfo.ReturnType == typeof (bool))
          flag |= (bool) obj;
      }
      return flag;
    }

    private static IEnumerable<MethodInfo> AllPostProcessorMethodsNamed(string callbackName)
    {
      return EditorAssemblies.SubclassesOf(typeof (AssetPostprocessor)).Select<System.Type, MethodInfo>((Func<System.Type, MethodInfo>) (assetPostprocessorClass => assetPostprocessorClass.GetMethod(callbackName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))).Where<MethodInfo>((Func<MethodInfo, bool>) (method => method != null));
    }

    private static ArrayList GetCachedAssetPostprocessorClasses()
    {
      if (AssetPostprocessingInternal.m_PostprocessorClasses == null)
      {
        AssetPostprocessingInternal.m_PostprocessorClasses = new ArrayList();
        foreach (System.Type type in EditorAssemblies.SubclassesOf(typeof (AssetPostprocessor)))
          AssetPostprocessingInternal.m_PostprocessorClasses.Add((object) type);
      }
      return AssetPostprocessingInternal.m_PostprocessorClasses;
    }

    [RequiredByNativeCode]
    private static void InitPostprocessors(string pathName)
    {
      AssetPostprocessingInternal.m_ImportProcessors = new ArrayList();
      IEnumerator enumerator = AssetPostprocessingInternal.GetCachedAssetPostprocessorClasses().GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          System.Type current = (System.Type) enumerator.Current;
          try
          {
            AssetPostprocessor instance = Activator.CreateInstance(current) as AssetPostprocessor;
            instance.assetPath = pathName;
            AssetPostprocessingInternal.m_ImportProcessors.Add((object) instance);
          }
          catch (MissingMethodException ex)
          {
            AssetPostprocessingInternal.LogPostProcessorMissingDefaultConstructor(current);
          }
          catch (Exception ex)
          {
            Debug.LogException(ex);
          }
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      AssetPostprocessingInternal.m_ImportProcessors.Sort((IComparer) new AssetPostprocessingInternal.CompareAssetImportPriority());
      AssetPostprocessingInternal.PostprocessStack postprocessStack = new AssetPostprocessingInternal.PostprocessStack();
      postprocessStack.m_ImportProcessors = AssetPostprocessingInternal.m_ImportProcessors;
      if (AssetPostprocessingInternal.m_PostprocessStack == null)
        AssetPostprocessingInternal.m_PostprocessStack = new ArrayList();
      AssetPostprocessingInternal.m_PostprocessStack.Add((object) postprocessStack);
    }

    [RequiredByNativeCode]
    private static void CleanupPostprocessors()
    {
      if (AssetPostprocessingInternal.m_PostprocessStack == null)
        return;
      AssetPostprocessingInternal.m_PostprocessStack.RemoveAt(AssetPostprocessingInternal.m_PostprocessStack.Count - 1);
      if (AssetPostprocessingInternal.m_PostprocessStack.Count != 0)
        AssetPostprocessingInternal.m_ImportProcessors = ((AssetPostprocessingInternal.PostprocessStack) AssetPostprocessingInternal.m_PostprocessStack[AssetPostprocessingInternal.m_PostprocessStack.Count - 1]).m_ImportProcessors;
    }

    [RequiredByNativeCode]
    private static uint[] GetMeshProcessorVersions()
    {
      List<uint> uintList = new List<uint>();
      foreach (System.Type type1 in EditorAssemblies.SubclassesOf(typeof (AssetPostprocessor)))
      {
        try
        {
          AssetPostprocessor instance = Activator.CreateInstance(type1) as AssetPostprocessor;
          System.Type type2 = instance.GetType();
          bool flag1 = type2.GetMethod("OnPreprocessModel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
          bool flag2 = type2.GetMethod("OnProcessMeshAssingModel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
          bool flag3 = type2.GetMethod("OnPostprocessModel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
          uint version = instance.GetVersion();
          if ((int) version != 0)
          {
            if (!flag1 && !flag2)
            {
              if (!flag3)
                goto label_9;
            }
            uintList.Add(version);
          }
        }
        catch (MissingMethodException ex)
        {
          AssetPostprocessingInternal.LogPostProcessorMissingDefaultConstructor(type1);
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
        }
label_9:;
      }
      return uintList.ToArray();
    }

    [RequiredByNativeCode]
    private static void PreprocessMesh(string pathName)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPreprocessModel", (object[]) null);
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    [RequiredByNativeCode]
    private static void PreprocessSpeedTree(string pathName)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPreprocessSpeedTree", (object[]) null);
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    [RequiredByNativeCode]
    private static void PreprocessAnimation(string pathName)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPreprocessAnimation", (object[]) null);
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    [RequiredByNativeCode]
    private static void PostprocessAnimation(GameObject root, AnimationClip clip)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPostprocessAnimation", new object[2]
          {
            (object) root,
            (object) clip
          });
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    [RequiredByNativeCode]
    private static Material ProcessMeshAssignMaterial(Renderer renderer, Material material)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          object obj = AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnAssignMaterialModel", new object[2]{ (object) material, (object) renderer });
          if ((bool) ((UnityEngine.Object) (obj as Material)))
            return obj as Material;
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      return (Material) null;
    }

    [RequiredByNativeCode]
    private static bool ProcessMeshHasAssignMaterial()
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          if (((AssetPostprocessor) enumerator.Current).GetType().GetMethod("OnAssignMaterialModel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null)
            return true;
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      return false;
    }

    private static void PostprocessMesh(GameObject gameObject)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPostprocessModel", new object[1]
          {
            (object) gameObject
          });
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    private static void PostprocessSpeedTree(GameObject gameObject)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPostprocessSpeedTree", new object[1]
          {
            (object) gameObject
          });
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    [RequiredByNativeCode]
    private static void PostprocessMaterial(Material material)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPostprocessMaterial", new object[1]
          {
            (object) material
          });
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    [RequiredByNativeCode]
    private static void PostprocessGameObjectWithUserProperties(GameObject go, string[] prop_names, object[] prop_values)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPostprocessGameObjectWithUserProperties", new object[3]
          {
            (object) go,
            (object) prop_names,
            (object) prop_values
          });
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    [RequiredByNativeCode]
    private static EditorCurveBinding[] PostprocessGameObjectWithAnimatedUserProperties(GameObject go, EditorCurveBinding[] bindings)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPostprocessGameObjectWithAnimatedUserProperties", new object[2]
          {
            (object) go,
            (object) bindings
          });
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      return bindings;
    }

    [RequiredByNativeCode]
    private static uint[] GetTextureProcessorVersions()
    {
      List<uint> uintList = new List<uint>();
      foreach (System.Type type1 in EditorAssemblies.SubclassesOf(typeof (AssetPostprocessor)))
      {
        try
        {
          AssetPostprocessor instance = Activator.CreateInstance(type1) as AssetPostprocessor;
          System.Type type2 = instance.GetType();
          bool flag1 = type2.GetMethod("OnPreprocessTexture", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
          bool flag2 = type2.GetMethod("OnPostprocessTexture", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
          uint version = instance.GetVersion();
          if ((int) version != 0)
          {
            if (!flag1)
            {
              if (!flag2)
                goto label_9;
            }
            uintList.Add(version);
          }
        }
        catch (MissingMethodException ex)
        {
          AssetPostprocessingInternal.LogPostProcessorMissingDefaultConstructor(type1);
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
        }
label_9:;
      }
      return uintList.ToArray();
    }

    [RequiredByNativeCode]
    private static void PreprocessTexture(string pathName)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPreprocessTexture", (object[]) null);
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    [RequiredByNativeCode]
    private static void PostprocessTexture(Texture2D tex, string pathName)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPostprocessTexture", new object[1]
          {
            (object) tex
          });
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    [RequiredByNativeCode]
    private static void PostprocessSprites(Texture2D tex, string pathName, Sprite[] sprites)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPostprocessSprites", new object[2]
          {
            (object) tex,
            (object) sprites
          });
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    [RequiredByNativeCode]
    private static uint[] GetAudioProcessorVersions()
    {
      List<uint> uintList = new List<uint>();
      foreach (System.Type type1 in EditorAssemblies.SubclassesOf(typeof (AssetPostprocessor)))
      {
        try
        {
          AssetPostprocessor instance = Activator.CreateInstance(type1) as AssetPostprocessor;
          System.Type type2 = instance.GetType();
          bool flag1 = type2.GetMethod("OnPreprocessAudio", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
          bool flag2 = type2.GetMethod("OnPostprocessAudio", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
          uint version = instance.GetVersion();
          if ((int) version != 0)
          {
            if (!flag1)
            {
              if (!flag2)
                goto label_9;
            }
            uintList.Add(version);
          }
        }
        catch (MissingMethodException ex)
        {
          AssetPostprocessingInternal.LogPostProcessorMissingDefaultConstructor(type1);
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
        }
label_9:;
      }
      return uintList.ToArray();
    }

    [RequiredByNativeCode]
    private static void PreprocessAudio(string pathName)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPreprocessAudio", (object[]) null);
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    [RequiredByNativeCode]
    private static void PostprocessAudio(AudioClip tex, string pathName)
    {
      IEnumerator enumerator = AssetPostprocessingInternal.m_ImportProcessors.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AttributeHelper.InvokeMemberIfAvailable((object) (AssetPostprocessor) enumerator.Current, "OnPostprocessAudio", new object[1]
          {
            (object) tex
          });
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    [RequiredByNativeCode]
    private static void PostprocessAssetbundleNameChanged(string assetPAth, string prevoiusAssetBundleName, string newAssetBundleName)
    {
      object[] args = new object[3]{ (object) assetPAth, (object) prevoiusAssetBundleName, (object) newAssetBundleName };
      foreach (System.Type type in EditorAssemblies.SubclassesOf(typeof (AssetPostprocessor)))
        AttributeHelper.InvokeMemberIfAvailable((object) (Activator.CreateInstance(type) as AssetPostprocessor), "OnPostprocessAssetbundleNameChanged", args);
    }

    internal class CompareAssetImportPriority : IComparer
    {
      int IComparer.Compare(object xo, object yo)
      {
        return ((AssetPostprocessor) xo).GetPostprocessOrder().CompareTo(((AssetPostprocessor) yo).GetPostprocessOrder());
      }
    }

    internal class PostprocessStack
    {
      internal ArrayList m_ImportProcessors = (ArrayList) null;
    }
  }
}
