// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.SoftLockData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace UnityEditor.Collaboration
{
  internal static class SoftLockData
  {
    internal static SoftLockData.OnSoftlockUpdate SoftlockSubscriber = (SoftLockData.OnSoftlockUpdate) null;

    public static void SetSoftlockChanges(string[] assetGUIDs)
    {
      if (SoftLockData.SoftlockSubscriber == null)
        return;
      SoftLockData.SoftlockSubscriber(assetGUIDs);
    }

    public static bool AllowsSoftLocks(UnityEngine.Object unityObject)
    {
      if (unityObject == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (unityObject));
      return unityObject.GetType().Equals(typeof (SceneAsset)) || SoftLockData.IsPrefab(unityObject);
    }

    public static bool IsPrefab(UnityEngine.Object unityObject)
    {
      PrefabType prefabType = PrefabUtility.GetPrefabType(unityObject);
      return prefabType == PrefabType.PrefabInstance || prefabType == PrefabType.Prefab;
    }

    public static bool IsPrefab(string assetGUID)
    {
      bool flag = false;
      UnityEngine.Object asset;
      if (AssetAccess.TryGetAssetFromGUID(assetGUID, out asset))
        flag = SoftLockData.IsPrefab(asset);
      return flag;
    }

    private static bool TryHasSoftLocks(Scene scene, out bool hasSoftLocks)
    {
      return SoftLockData.TryHasSoftLocks(AssetDatabase.AssetPathToGUID(scene.path), out hasSoftLocks);
    }

    public static bool TryHasSoftLocks(UnityEngine.Object objectWithGUID, out bool hasSoftLocks)
    {
      string assetGUID = (string) null;
      AssetAccess.TryGetAssetGUIDFromObject(objectWithGUID, out assetGUID);
      return SoftLockData.TryHasSoftLocks(assetGUID, out hasSoftLocks);
    }

    public static bool TryHasSoftLocks(string assetGuid, out bool hasSoftLocks)
    {
      hasSoftLocks = false;
      bool flag = false;
      int count = 0;
      if (SoftLockData.TryGetSoftlockCount(assetGuid, out count))
      {
        flag = true;
        hasSoftLocks = count > 0;
      }
      return flag;
    }

    public static bool TryGetSoftlockCount(Scene scene, out int count)
    {
      count = 0;
      if (!scene.IsValid())
        return false;
      return SoftLockData.TryGetSoftlockCount(AssetDatabase.AssetPathToGUID(scene.path), out count);
    }

    public static bool TryGetSoftlockCount(UnityEngine.Object objectWithGUID, out int count)
    {
      string assetGUID = (string) null;
      AssetAccess.TryGetAssetGUIDFromObject(objectWithGUID, out assetGUID);
      return SoftLockData.TryGetSoftlockCount(assetGUID, out count);
    }

    public static bool TryGetSoftlockCount(string assetGuid, out int count)
    {
      bool flag = false;
      count = 0;
      List<SoftLock> softLocks = (List<SoftLock>) null;
      if (SoftLockData.TryGetLocksOnAssetGUID(assetGuid, out softLocks))
      {
        count = softLocks.Count;
        flag = true;
      }
      return flag;
    }

    private static bool TryGetLocksOnObject(UnityEngine.Object objectWithGUID, out List<SoftLock> softLocks)
    {
      bool flag = false;
      string assetGUID = (string) null;
      if (AssetAccess.TryGetAssetGUIDFromObject(objectWithGUID, out assetGUID))
        flag = SoftLockData.TryGetLocksOnAssetGUID(assetGUID, out softLocks);
      else
        softLocks = new List<SoftLock>();
      return flag;
    }

    public static bool TryGetLocksOnAssetGUID(string assetGuid, out List<SoftLock> softLocks)
    {
      if (assetGuid == null)
        throw new ArgumentNullException(nameof (assetGuid));
      if (!Collab.instance.IsCollabEnabledForCurrentProject() || assetGuid.Length == 0)
      {
        softLocks = new List<SoftLock>();
        return false;
      }
      SoftLock[] softLocks1 = Collab.instance.GetSoftLocks(assetGuid);
      softLocks = new List<SoftLock>();
      for (int index = 0; index < softLocks1.Length; ++index)
        softLocks.Add(softLocks1[index]);
      return true;
    }

    internal delegate void OnSoftlockUpdate(string[] assetGUIDs);
  }
}
