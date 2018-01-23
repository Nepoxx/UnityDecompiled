// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.MemoryInfoGCReason
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditorInternal
{
  public enum MemoryInfoGCReason
  {
    SceneObject = 0,
    BuiltinResource = 1,
    MarkedDontSave = 2,
    AssetMarkedDirtyInEditor = 3,
    SceneAssetReferencedByNativeCodeOnly = 5,
    SceneAssetReferenced = 6,
    AssetReferencedByNativeCodeOnly = 8,
    AssetReferenced = 9,
    NotApplicable = 10, // 0x0000000A
  }
}
