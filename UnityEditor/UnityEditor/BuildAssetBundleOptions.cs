// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildAssetBundleOptions
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Asset Bundle building options.</para>
  /// </summary>
  [System.Flags]
  public enum BuildAssetBundleOptions
  {
    None = 0,
    UncompressedAssetBundle = 1,
    [Obsolete("This has been made obsolete. It is always enabled in the new AssetBundle build system introduced in 5.0.")] CollectDependencies = 2,
    [Obsolete("This has been made obsolete. It is always enabled in the new AssetBundle build system introduced in 5.0.")] CompleteAssets = 4,
    DisableWriteTypeTree = 8,
    DeterministicAssetBundle = 16, // 0x00000010
    ForceRebuildAssetBundle = 32, // 0x00000020
    IgnoreTypeTreeChanges = 64, // 0x00000040
    AppendHashToAssetBundleName = 128, // 0x00000080
    ChunkBasedCompression = 256, // 0x00000100
    StrictMode = 512, // 0x00000200
    DryRunBuild = 1024, // 0x00000400
    DisableLoadAssetByFileName = 4096, // 0x00001000
    DisableLoadAssetByFileNameWithExtension = 8192, // 0x00002000
  }
}
