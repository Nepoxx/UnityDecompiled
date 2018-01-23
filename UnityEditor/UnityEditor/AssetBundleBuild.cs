// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetBundleBuild
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>AssetBundle building map entry.</para>
  /// </summary>
  public struct AssetBundleBuild
  {
    /// <summary>
    ///   <para>AssetBundle name.</para>
    /// </summary>
    public string assetBundleName;
    /// <summary>
    ///   <para>AssetBundle variant.</para>
    /// </summary>
    public string assetBundleVariant;
    /// <summary>
    ///   <para>Asset names which belong to the given AssetBundle.</para>
    /// </summary>
    public string[] assetNames;
    /// <summary>
    ///   <para>Addressable name used to load an asset.</para>
    /// </summary>
    public string[] addressableNames;
  }
}
