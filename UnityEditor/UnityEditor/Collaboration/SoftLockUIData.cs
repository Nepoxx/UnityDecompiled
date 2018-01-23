// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.SoftLockUIData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor.Collaboration
{
  internal static class SoftLockUIData
  {
    private static Dictionary<string, Texture> s_ImageCache = new Dictionary<string, Texture>();
    private static Dictionary<SoftLockUIData.SectionEnum, string> s_ImageNameCache = new Dictionary<SoftLockUIData.SectionEnum, string>();
    private const string kIconMipSuffix = " Icon";

    public static List<string> GetLocksNamesOnAsset(string assetGuid)
    {
      List<SoftLock> softLocks = (List<SoftLock>) null;
      List<string> stringList = new List<string>();
      if (SoftLockData.TryGetLocksOnAssetGUID(assetGuid, out softLocks))
      {
        foreach (SoftLock softLock in softLocks)
          stringList.Add(softLock.displayName);
      }
      return stringList;
    }

    public static List<string> GetLocksNamesOnScene(Scene scene)
    {
      return SoftLockUIData.GetLockNamesOnScenePath(scene.path);
    }

    public static List<string> GetLockNamesOnScenePath(string scenePath)
    {
      return SoftLockUIData.GetLocksNamesOnAsset(AssetDatabase.AssetPathToGUID(scenePath));
    }

    public static string GetSceneNameFromPath(string scenePath)
    {
      string str = "";
      if (scenePath != null)
        str = scenePath;
      return str;
    }

    public static List<List<string>> GetLockNamesOnScenes(List<Scene> scenes)
    {
      List<List<string>> stringListList = new List<List<string>>();
      if (scenes == null)
        return stringListList;
      foreach (Scene scene in scenes)
      {
        List<string> locksNamesOnScene = SoftLockUIData.GetLocksNamesOnScene(scene);
        stringListList.Add(locksNamesOnScene);
      }
      return stringListList;
    }

    [DebuggerHidden]
    public static IEnumerable<KeyValuePair<string, List<string>>> GetLockNamesOnOpenScenes()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SoftLockUIData.\u003CGetLockNamesOnOpenScenes\u003Ec__Iterator0 scenesCIterator0_1 = new SoftLockUIData.\u003CGetLockNamesOnOpenScenes\u003Ec__Iterator0();
      // ISSUE: variable of a compiler-generated type
      SoftLockUIData.\u003CGetLockNamesOnOpenScenes\u003Ec__Iterator0 scenesCIterator0_2 = scenesCIterator0_1;
      // ISSUE: reference to a compiler-generated field
      scenesCIterator0_2.\u0024PC = -2;
      return (IEnumerable<KeyValuePair<string, List<string>>>) scenesCIterator0_2;
    }

    public static int CountOfLocksOnOpenScenes()
    {
      int num = 0;
      foreach (KeyValuePair<string, List<string>> namesOnOpenScene in SoftLockUIData.GetLockNamesOnOpenScenes())
        num += namesOnOpenScene.Value.Count;
      return num;
    }

    public static List<string> GetLockNamesOnObject(Object objectWithGUID)
    {
      string assetGUID = (string) null;
      AssetAccess.TryGetAssetGUIDFromObject(objectWithGUID, out assetGUID);
      return SoftLockUIData.GetLocksNamesOnAsset(assetGUID);
    }

    public static Texture GetIconForSection(SoftLockUIData.SectionEnum section)
    {
      return SoftLockUIData.GetIconForName(SoftLockUIData.IconNameForSection(section));
    }

    private static string IconNameForSection(SoftLockUIData.SectionEnum section)
    {
      string str;
      if (!SoftLockUIData.s_ImageNameCache.TryGetValue(section, out str))
      {
        switch (section)
        {
          case SoftLockUIData.SectionEnum.Inspector:
          case SoftLockUIData.SectionEnum.Scene:
            str = "SoftlockInline.png";
            break;
          case SoftLockUIData.SectionEnum.ProjectBrowser:
            str = string.Format("SoftlockProjectBrowser{0}", (object) " Icon");
            break;
          default:
            return (string) null;
        }
        SoftLockUIData.s_ImageNameCache.Add(section, str);
      }
      return str;
    }

    private static Texture GetIconForName(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
        return (Texture) null;
      Texture texture;
      if (!SoftLockUIData.s_ImageCache.TryGetValue(fileName, out texture) || (Object) texture == (Object) null)
      {
        texture = !fileName.EndsWith(" Icon") ? (Texture) EditorGUIUtility.LoadIconRequired(fileName) : (Texture) EditorGUIUtility.FindTexture(fileName);
        SoftLockUIData.s_ImageCache.Remove(fileName);
        SoftLockUIData.s_ImageCache.Add(fileName, texture);
      }
      return texture;
    }

    public enum SectionEnum
    {
      None,
      Inspector,
      Scene,
      ProjectBrowser,
    }
  }
}
