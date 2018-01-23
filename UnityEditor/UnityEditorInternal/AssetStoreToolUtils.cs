// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AssetStoreToolUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditorInternal
{
  public sealed class AssetStoreToolUtils
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool BuildAssetStoreAssetBundle(Object targetObject, string targetPath);

    public static bool PreviewAssetStoreAssetBundleInInspector(AssetBundle bundle, AssetStoreAsset info)
    {
      info.id = 0;
      info.previewAsset = bundle.mainAsset;
      AssetStoreAssetSelection.Clear();
      AssetStoreAssetSelection.AddAssetInternal(info);
      Selection.activeObject = (Object) AssetStoreAssetInspector.Instance;
      AssetStoreAssetInspector.Instance.Repaint();
      return true;
    }

    public static void UpdatePreloadingInternal()
    {
      AssetStoreUtils.UpdatePreloading();
    }
  }
}
