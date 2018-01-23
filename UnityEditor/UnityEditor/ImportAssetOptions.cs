// Decompiled with JetBrains decompiler
// Type: UnityEditor.ImportAssetOptions
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Asset importing options.</para>
  /// </summary>
  [System.Flags]
  public enum ImportAssetOptions
  {
    Default = 0,
    ForceUpdate = 1,
    ForceSynchronousImport = 8,
    ImportRecursive = 256, // 0x00000100
    DontDownloadFromCacheServer = 8192, // 0x00002000
    ForceUncompressedImport = 16384, // 0x00004000
  }
}
