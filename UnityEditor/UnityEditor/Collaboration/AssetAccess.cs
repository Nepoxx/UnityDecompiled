// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.AssetAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.Collaboration
{
  internal static class AssetAccess
  {
    public static bool TryGetAssetGUIDFromObject(UnityEngine.Object objectWithGUID, out string assetGUID)
    {
      if (objectWithGUID == (UnityEngine.Object) null)
        throw new ArgumentNullException("objectWithGuid");
      bool flag = false;
      if (objectWithGUID.GetType() == typeof (SceneAsset))
        flag = AssetAccess.TryGetAssetGUIDFromDatabase(objectWithGUID, out assetGUID);
      else if (objectWithGUID.GetType() == typeof (GameObject))
        flag = AssetAccess.TryGetPrefabGUID(objectWithGUID, out assetGUID);
      else
        assetGUID = string.Empty;
      return flag;
    }

    public static bool TryGetAssetFromGUID(string assetGUID, out UnityEngine.Object asset)
    {
      if (assetGUID == null)
        throw new ArgumentNullException(nameof (assetGUID));
      bool flag = false;
      string assetPath = AssetDatabase.GUIDToAssetPath(assetGUID);
      if (assetPath == null)
      {
        asset = (UnityEngine.Object) null;
      }
      else
      {
        asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
        flag = asset != (UnityEngine.Object) null;
      }
      return flag;
    }

    private static bool TryGetPrefabGUID(UnityEngine.Object gameObject, out string assetGUID)
    {
      PrefabType prefabType = PrefabUtility.GetPrefabType(gameObject);
      UnityEngine.Object objectWithGUID = (UnityEngine.Object) null;
      switch (prefabType)
      {
        case PrefabType.Prefab:
          objectWithGUID = gameObject;
          break;
        case PrefabType.PrefabInstance:
          objectWithGUID = PrefabUtility.GetPrefabParent(gameObject);
          break;
      }
      bool flag = false;
      if (objectWithGUID == (UnityEngine.Object) null)
        assetGUID = string.Empty;
      else
        flag = AssetAccess.TryGetAssetGUIDFromDatabase(objectWithGUID, out assetGUID);
      return flag;
    }

    private static bool TryGetAssetGUIDFromDatabase(UnityEngine.Object objectWithGUID, out string assetGUID)
    {
      if (objectWithGUID == (UnityEngine.Object) null)
        throw new ArgumentNullException("objectWithGuid");
      string str = (string) null;
      string assetPath = AssetDatabase.GetAssetPath(objectWithGUID);
      if (!string.IsNullOrEmpty(assetPath))
        str = AssetDatabase.AssetPathToGUID(assetPath);
      bool flag = false;
      if (string.IsNullOrEmpty(str))
      {
        assetGUID = string.Empty;
      }
      else
      {
        assetGUID = str;
        flag = true;
      }
      return flag;
    }
  }
}
