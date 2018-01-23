// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightmapEditorSettings
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
  ///   <para>Various settings for the bake.</para>
  /// </summary>
  public sealed class LightmapEditorSettings
  {
    /// <summary>
    ///   <para>Determines which backend to use for baking lightmaps.</para>
    /// </summary>
    public static extern LightmapEditorSettings.Lightmapper lightmapper { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Determines which sampling strategy to use for baking lightmaps with the Progressive Lightmapper.</para>
    /// </summary>
    public static extern LightmapEditorSettings.Sampling sampling { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Configure a filter kernel for the direct light target.</para>
    /// </summary>
    public static extern LightmapEditorSettings.FilterType filterTypeDirect { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Configure a filter kernel for the indirect light target.</para>
    /// </summary>
    public static extern LightmapEditorSettings.FilterType filterTypeIndirect { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Configure a filter kernel for the ambient occlusion target.</para>
    /// </summary>
    public static extern LightmapEditorSettings.FilterType filterTypeAO { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Reset();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsLightmappedOrDynamicLightmappedForRendering(Renderer renderer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasZeroAreaMesh(Renderer renderer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasClampedResolution(Renderer renderer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool GetSystemResolution(Renderer renderer, out int width, out int height);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool GetTerrainSystemResolution(Terrain terrain, out int width, out int height, out int numChunksInX, out int numChunksInY);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool GetInstanceResolution(Renderer renderer, out int width, out int height);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool GetInputSystemHash(Renderer renderer, out Hash128 inputSystemHash);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool GetInstanceHash(Renderer renderer, out Hash128 instanceHash);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool GetPVRInstanceHash(int instanceID, out Hash128 instanceHash);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool GetPVRAtlasHash(int instanceID, out Hash128 atlasHash);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool GetPVRAtlasInstanceOffset(int instanceID, out int atlasInstanceOffset);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool GetGeometryHash(Renderer renderer, out Hash128 geometryHash);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void AnalyzeLighting(out LightingStats enabled, out LightingStats active, out LightingStats inactive);

    /// <summary>
    ///   <para>The maximum width of an individual lightmap texture.</para>
    /// </summary>
    public static extern int maxAtlasWidth { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum height of an individual lightmap texture.</para>
    /// </summary>
    public static extern int maxAtlasHeight { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Lightmap resolution in texels per world unit. Defines the resolution of Realtime GI if enabled. If Baked GI is enabled, this defines the resolution used for indirect lighting. Higher resolution may take a long time to bake.</para>
    /// </summary>
    public static extern float realtimeResolution { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern float bakeResolution { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Whether to use texture compression on the generated lightmaps.</para>
    /// </summary>
    public static extern bool textureCompression { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Determines how Unity will compress baked reflection cubemap.</para>
    /// </summary>
    public static extern ReflectionCubemapCompression reflectionCubemapCompression { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable baked ambient occlusion (AO).</para>
    /// </summary>
    public static extern bool enableAmbientOcclusion { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Beyond this distance a ray is considered to be unoccluded.</para>
    /// </summary>
    public static extern float aoMaxDistance { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Ambient occlusion (AO) for indirect lighting.</para>
    /// </summary>
    public static extern float aoExponentIndirect { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Ambient occlusion (AO) for direct lighting.</para>
    /// </summary>
    public static extern float aoExponentDirect { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Texel separation between shapes.</para>
    /// </summary>
    public static extern int padding { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern UnityEngine.Object GetLightmapSettings();

    [Obsolete("LightmapEditorSettings.aoContrast has been deprecated.", false)]
    public static float aoContrast
    {
      get
      {
        return 0.0f;
      }
      set
      {
      }
    }

    [Obsolete("LightmapEditorSettings.aoAmount has been deprecated.", false)]
    public static float aoAmount
    {
      get
      {
        return 0.0f;
      }
      set
      {
      }
    }

    [Obsolete("LightmapEditorSettings.lockAtlas has been deprecated.", false)]
    public static bool lockAtlas
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    [Obsolete("LightmapEditorSettings.skyLightColor has been deprecated.", false)]
    public static Color skyLightColor
    {
      get
      {
        return Color.black;
      }
      set
      {
      }
    }

    [Obsolete("LightmapEditorSettings.skyLightIntensity has been deprecated.", false)]
    public static float skyLightIntensity
    {
      get
      {
        return 0.0f;
      }
      set
      {
      }
    }

    [Obsolete("LightmapEditorSettings.quality has been deprecated.", false)]
    public static LightmapBakeQuality quality
    {
      get
      {
        return LightmapBakeQuality.High;
      }
      set
      {
      }
    }

    [Obsolete("LightmapEditorSettings.bounceBoost has been deprecated.", false)]
    public static float bounceBoost
    {
      get
      {
        return 0.0f;
      }
      set
      {
      }
    }

    [Obsolete("LightmapEditorSettings.finalGatherRays has been deprecated.", false)]
    public static int finalGatherRays
    {
      get
      {
        return 0;
      }
      set
      {
      }
    }

    [Obsolete("LightmapEditorSettings.finalGatherContrastThreshold has been deprecated.", false)]
    public static float finalGatherContrastThreshold
    {
      get
      {
        return 0.0f;
      }
      set
      {
      }
    }

    [Obsolete("LightmapEditorSettings.finalGatherGradientThreshold has been deprecated.", false)]
    public static float finalGatherGradientThreshold
    {
      get
      {
        return 0.0f;
      }
      set
      {
      }
    }

    [Obsolete("LightmapEditorSettings.finalGatherInterpolationPoints has been deprecated.", false)]
    public static int finalGatherInterpolationPoints
    {
      get
      {
        return 0;
      }
      set
      {
      }
    }

    [Obsolete("LightmapEditorSettings.lastUsedResolution has been deprecated.", false)]
    public static float lastUsedResolution
    {
      get
      {
        return 0.0f;
      }
      set
      {
      }
    }

    [Obsolete("LightmapEditorSettings.bounces has been deprecated.", false)]
    public static int bounces
    {
      get
      {
        return 0;
      }
      set
      {
      }
    }

    [Obsolete("LightmapEditorSettings.bounceIntensity has been deprecated.", false)]
    public static float bounceIntensity
    {
      get
      {
        return 0.0f;
      }
      set
      {
      }
    }

    [Obsolete("resolution is now called realtimeResolution (UnityUpgradable) -> realtimeResolution", false)]
    public static float resolution
    {
      get
      {
        return LightmapEditorSettings.realtimeResolution;
      }
      set
      {
        LightmapEditorSettings.realtimeResolution = value;
      }
    }

    [Obsolete("The giBakeBackend property has been renamed to lightmapper. (UnityUpgradable) -> lightmapper", false)]
    public static LightmapEditorSettings.GIBakeBackend giBakeBackend
    {
      get
      {
        return LightmapEditorSettings.lightmapper == LightmapEditorSettings.Lightmapper.PathTracer ? LightmapEditorSettings.GIBakeBackend.PathTracer : LightmapEditorSettings.GIBakeBackend.Radiosity;
      }
      set
      {
        if (value == LightmapEditorSettings.GIBakeBackend.PathTracer)
          LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.PathTracer;
        else
          LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.Radiosity;
      }
    }

    [Obsolete("The giPathTracerSampling property has been renamed to sampling. (UnityUpgradable) -> sampling", false)]
    public static LightmapEditorSettings.PathTracerSampling giPathTracerSampling
    {
      get
      {
        return LightmapEditorSettings.sampling == LightmapEditorSettings.Sampling.Auto ? LightmapEditorSettings.PathTracerSampling.Auto : LightmapEditorSettings.PathTracerSampling.Fixed;
      }
      set
      {
        if (value == LightmapEditorSettings.PathTracerSampling.Auto)
          LightmapEditorSettings.sampling = LightmapEditorSettings.Sampling.Auto;
        else
          LightmapEditorSettings.sampling = LightmapEditorSettings.Sampling.Fixed;
      }
    }

    /// <summary>
    ///   <para>Determines the filtering kernel for the Progressive Lightmapper.</para>
    /// </summary>
    [Obsolete("The giPathTracerFilter property has been deprecated. There are three independent properties to set individual filter types for direct, indirect and AO GI textures: filterTypeDirect, filterTypeIndirect and filterTypeAO.")]
    public static LightmapEditorSettings.PathTracerFilter giPathTracerFilter
    {
      get
      {
        return LightmapEditorSettings.filterTypeDirect == LightmapEditorSettings.FilterType.Gaussian ? LightmapEditorSettings.PathTracerFilter.Gaussian : LightmapEditorSettings.PathTracerFilter.ATrous;
      }
      set
      {
        LightmapEditorSettings.filterTypeDirect = LightmapEditorSettings.FilterType.Gaussian;
        LightmapEditorSettings.filterTypeIndirect = LightmapEditorSettings.FilterType.Gaussian;
        LightmapEditorSettings.filterTypeAO = LightmapEditorSettings.FilterType.Gaussian;
      }
    }

    /// <summary>
    ///   <para>Backends available for baking lightmaps.</para>
    /// </summary>
    public enum Lightmapper
    {
      Enlighten = 0,
      [Obsolete("Use Lightmapper.Enlighten instead. (UnityUpgradable) -> UnityEditor.LightmapEditorSettings/Lightmapper.Enlighten", true)] Radiosity = 0,
      [Obsolete("Use Lightmapper.ProgressiveCPU instead. (UnityUpgradable) -> UnityEditor.LightmapEditorSettings/Lightmapper.ProgressiveCPU", true)] PathTracer = 1,
      ProgressiveCPU = 1,
    }

    /// <summary>
    ///   <para>Available sampling strategies for baking lightmaps with the Progressive Lightmapper.</para>
    /// </summary>
    public enum Sampling
    {
      Auto,
      Fixed,
    }

    /// <summary>
    ///   <para>The available filtering modes for the Progressive Lightmapper.</para>
    /// </summary>
    public enum FilterMode
    {
      None,
      Auto,
      Advanced,
    }

    /// <summary>
    ///   <para>The available filter kernels for the Progressive Lightmapper.</para>
    /// </summary>
    public enum FilterType
    {
      Gaussian,
      ATrous,
      None,
    }

    [Obsolete("GIBakeBackend has been renamed to Lightmapper. (UnityUpgradable)", true)]
    public enum GIBakeBackend
    {
      Radiosity,
      PathTracer,
    }

    [Obsolete("PathTracerSampling has been renamed to Sampling. (UnityUpgradable) -> UnityEditor.LightmapEditorSettings/Sampling", false)]
    public enum PathTracerSampling
    {
      Auto,
      Fixed,
    }

    [Obsolete("PathTracerFilter has been renamed to FilterType. (UnityUpgradable) -> UnityEditor.LightmapEditorSettings/FilterType", false)]
    public enum PathTracerFilter
    {
      Gaussian,
      ATrous,
    }
  }
}
