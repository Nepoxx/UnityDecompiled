// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightmapParameters
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>A collection of parameters that impact lightmap and realtime GI computations.</para>
  /// </summary>
  public sealed class LightmapParameters : UnityEngine.Object
  {
    public LightmapParameters()
    {
      LightmapParameters.Internal_CreateLightmapParameters(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateLightmapParameters([Writable] LightmapParameters self);

    /// <summary>
    ///   <para>The texel resolution per meter used for realtime lightmaps. This value is multiplied by LightmapEditorSettings.resolution.</para>
    /// </summary>
    public extern float resolution { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Controls the resolution at which Enlighten stores and can transfer input light.</para>
    /// </summary>
    public extern float clusterResolution { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The amount of data used for realtime GI texels. Specifies how detailed view of the scene a texel has. Small values mean more averaged out lighting.</para>
    /// </summary>
    public extern int irradianceBudget { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The number of rays to cast for computing irradiance form factors.</para>
    /// </summary>
    public extern int irradianceQuality { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The percentage of rays shot from a ray origin that must hit front faces to be considered usable.</para>
    /// </summary>
    public extern float backFaceTolerance { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Maximum size of gaps that can be ignored for GI (multiplier on pixel size).</para>
    /// </summary>
    public extern float modellingTolerance { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Whether pairs of edges should be stitched together.</para>
    /// </summary>
    public extern bool stitchEdges { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>System tag is an integer identifier. It lets you force an object into a different Enlighten system even though all the other parameters are the same.</para>
    /// </summary>
    public extern int systemTag { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If enabled, the object appears transparent during GlobalIllumination lighting calculations.</para>
    /// </summary>
    public extern bool isTransparent { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The number of rays to cast for computing ambient occlusion.</para>
    /// </summary>
    public extern int AOQuality { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum number of times to supersample a texel to reduce aliasing in AO.</para>
    /// </summary>
    public extern int AOAntiAliasingSamples { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The radius (in texels) of the post-processing filter that blurs baked direct lighting.</para>
    /// </summary>
    public extern int blurRadius { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The number of rays used for lights with an area. Allows for accurate soft shadowing.</para>
    /// </summary>
    public extern int directLightQuality { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum number of times to supersample a texel to reduce aliasing.</para>
    /// </summary>
    public extern int antiAliasingSamples { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>BakedLightmapTag is an integer that affects the assignment to baked lightmaps. Objects with different values for bakedLightmapTag are guaranteed to not be assigned to the same lightmap even if the other baking parameters are the same.</para>
    /// </summary>
    public extern int bakedLightmapTag { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("edgeStitching is deprecated. Use stitchEdges instead.")]
    public float edgeStitching
    {
      get
      {
        return !this.stitchEdges ? 0.0f : 1f;
      }
      set
      {
        this.stitchEdges = (double) value != 0.0;
      }
    }
  }
}
