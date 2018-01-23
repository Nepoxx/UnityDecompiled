// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpeedTreeImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>AssetImportor for importing SpeedTree model assets.</para>
  /// </summary>
  public sealed class SpeedTreeImporter : AssetImporter
  {
    /// <summary>
    ///   <para>Gets an array of name strings for wind quality value.</para>
    /// </summary>
    public static readonly string[] windQualityNames = new string[6]{ "None", "Fastest", "Fast", "Better", "Best", "Palm" };

    /// <summary>
    ///   <para>Tells if the SPM file has been previously imported.</para>
    /// </summary>
    public extern bool hasImported { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the folder path where generated materials will be placed in.</para>
    /// </summary>
    public extern string materialFolderPath { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>How much to scale the tree model compared to what is in the .spm file.</para>
    /// </summary>
    public extern float scaleFactor { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets a default main color.</para>
    /// </summary>
    public Color mainColor
    {
      get
      {
        Color color;
        this.INTERNAL_get_mainColor(out color);
        return color;
      }
      set
      {
        this.INTERNAL_set_mainColor(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_mainColor(out Color value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_mainColor(ref Color value);

    /// <summary>
    ///   <para>Gets and sets a default specular color.</para>
    /// </summary>
    [Obsolete("specColor is no longer used and has been deprecated.", true)]
    public Color specColor { get; set; }

    /// <summary>
    ///   <para>Gets and sets a default Shininess value.</para>
    /// </summary>
    [Obsolete("shininess is no longer used and has been deprecated.", true)]
    public float shininess { get; set; }

    /// <summary>
    ///   <para>Gets and sets a default Hue variation color and amount (in alpha).</para>
    /// </summary>
    public Color hueVariation
    {
      get
      {
        Color color;
        this.INTERNAL_get_hueVariation(out color);
        return color;
      }
      set
      {
        this.INTERNAL_set_hueVariation(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_hueVariation(out Color value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_hueVariation(ref Color value);

    /// <summary>
    ///   <para>Gets and sets a default alpha test reference values.</para>
    /// </summary>
    public extern float alphaTestRef { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Tells if there is a billboard LOD.</para>
    /// </summary>
    public extern bool hasBillboard { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Enables smooth LOD transitions.</para>
    /// </summary>
    public extern bool enableSmoothLODTransition { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Indicates if the cross-fade LOD transition, applied to the last mesh LOD and the billboard, should be animated.</para>
    /// </summary>
    public extern bool animateCrossFading { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Proportion of the last 3D mesh LOD region width which is used for cross-fading to billboard tree.</para>
    /// </summary>
    public extern float billboardTransitionCrossFadeWidth { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Proportion of the billboard LOD region width which is used for fading out the billboard.</para>
    /// </summary>
    public extern float fadeOutWidth { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets an array of floats of each LOD's screen height value.</para>
    /// </summary>
    public extern float[] LODHeights { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets an array of booleans to enable shadow casting for each LOD.</para>
    /// </summary>
    public extern bool[] castShadows { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets an array of booleans to enable shadow receiving for each LOD.</para>
    /// </summary>
    public extern bool[] receiveShadows { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets an array of booleans to enable Light Probe lighting for each LOD.</para>
    /// </summary>
    public extern bool[] useLightProbes { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets an array of Reflection Probe usages for each LOD.</para>
    /// </summary>
    public extern ReflectionProbeUsage[] reflectionProbeUsages { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets an array of booleans to enable normal mapping for each LOD.</para>
    /// </summary>
    public extern bool[] enableBump { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets an array of booleans to enable Hue variation effect for each LOD.</para>
    /// </summary>
    public extern bool[] enableHue { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns the best-possible wind quality on this asset (configured in SpeedTree modeler).</para>
    /// </summary>
    public extern int bestWindQuality { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Gets and sets an array of integers of the wind qualities on each LOD. Values will be clampped by BestWindQuality internally.</para>
    /// </summary>
    public extern int[] windQualities { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Generates all necessary materials under materialFolderPath. If Version Control is enabled please first check out the folder.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void GenerateMaterials();

    internal extern bool materialsShouldBeRegenerated { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetMaterialVersionToCurrent();
  }
}
