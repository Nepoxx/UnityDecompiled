// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildPlayerOptions
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Provide various options to control the behavior of BuildPipeline.BuildPlayer.</para>
  /// </summary>
  public struct BuildPlayerOptions
  {
    /// <summary>
    ///   <para>The scenes to be included in the build. If empty, the currently open scene will be built. Paths are relative to the project folder (AssetsMyLevelsMyScene.unity).</para>
    /// </summary>
    public string[] scenes { get; set; }

    /// <summary>
    ///   <para>The path where the application will be built.</para>
    /// </summary>
    public string locationPathName { get; set; }

    /// <summary>
    ///   <para>The path to an manifest file describing all of the asset bundles used in the build (optional).</para>
    /// </summary>
    public string assetBundleManifestPath { get; set; }

    /// <summary>
    ///   <para>The BuildTargetGroup to build.</para>
    /// </summary>
    public BuildTargetGroup targetGroup { get; set; }

    /// <summary>
    ///   <para>The BuildTarget to build.</para>
    /// </summary>
    public BuildTarget target { get; set; }

    /// <summary>
    ///   <para>Additional BuildOptions, like whether to run the built player.</para>
    /// </summary>
    public BuildOptions options { get; set; }
  }
}
