// Decompiled with JetBrains decompiler
// Type: UnityEngine.RenderSettings
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The Render Settings contain values for a range of visual elements in your scene, like fog and ambient light.</para>
  /// </summary>
  public sealed class RenderSettings : Object
  {
    /// <summary>
    ///   <para>Custom or skybox ambient lighting data.</para>
    /// </summary>
    public static SphericalHarmonicsL2 ambientProbe
    {
      get
      {
        SphericalHarmonicsL2 sphericalHarmonicsL2;
        RenderSettings.INTERNAL_get_ambientProbe(out sphericalHarmonicsL2);
        return sphericalHarmonicsL2;
      }
      set
      {
        RenderSettings.INTERNAL_set_ambientProbe(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_ambientProbe(out SphericalHarmonicsL2 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_set_ambientProbe(ref SphericalHarmonicsL2 value);

    /// <summary>
    ///   <para>Custom specular reflection cubemap.</para>
    /// </summary>
    public static extern Cubemap customReflection { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Reset();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Object GetRenderSettings();

    [Obsolete("Use RenderSettings.ambientIntensity instead (UnityUpgradable) -> ambientIntensity", false)]
    public static float ambientSkyboxAmount
    {
      get
      {
        return RenderSettings.ambientIntensity;
      }
      set
      {
        RenderSettings.ambientIntensity = value;
      }
    }

    /// <summary>
    ///   <para>Is fog enabled?</para>
    /// </summary>
    public static extern bool fog { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The starting distance of linear fog.</para>
    /// </summary>
    public static extern float fogStartDistance { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The ending distance of linear fog.</para>
    /// </summary>
    public static extern float fogEndDistance { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Fog mode to use.</para>
    /// </summary>
    public static extern FogMode fogMode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The color of the fog.</para>
    /// </summary>
    public static Color fogColor
    {
      get
      {
        Color ret;
        RenderSettings.get_fogColor_Injected(out ret);
        return ret;
      }
      set
      {
        RenderSettings.set_fogColor_Injected(ref value);
      }
    }

    /// <summary>
    ///   <para>The density of the exponential fog.</para>
    /// </summary>
    public static extern float fogDensity { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Ambient lighting mode.</para>
    /// </summary>
    public static extern AmbientMode ambientMode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Ambient lighting coming from above.</para>
    /// </summary>
    public static Color ambientSkyColor
    {
      get
      {
        Color ret;
        RenderSettings.get_ambientSkyColor_Injected(out ret);
        return ret;
      }
      set
      {
        RenderSettings.set_ambientSkyColor_Injected(ref value);
      }
    }

    /// <summary>
    ///   <para>Ambient lighting coming from the sides.</para>
    /// </summary>
    public static Color ambientEquatorColor
    {
      get
      {
        Color ret;
        RenderSettings.get_ambientEquatorColor_Injected(out ret);
        return ret;
      }
      set
      {
        RenderSettings.set_ambientEquatorColor_Injected(ref value);
      }
    }

    /// <summary>
    ///   <para>Ambient lighting coming from below.</para>
    /// </summary>
    public static Color ambientGroundColor
    {
      get
      {
        Color ret;
        RenderSettings.get_ambientGroundColor_Injected(out ret);
        return ret;
      }
      set
      {
        RenderSettings.set_ambientGroundColor_Injected(ref value);
      }
    }

    /// <summary>
    ///   <para>How much the light from the Ambient Source affects the scene.</para>
    /// </summary>
    public static extern float ambientIntensity { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Flat ambient lighting color.</para>
    /// </summary>
    public static Color ambientLight
    {
      get
      {
        Color ret;
        RenderSettings.get_ambientLight_Injected(out ret);
        return ret;
      }
      set
      {
        RenderSettings.set_ambientLight_Injected(ref value);
      }
    }

    /// <summary>
    ///   <para>The color used for the sun shadows in the Subtractive lightmode.</para>
    /// </summary>
    public static Color subtractiveShadowColor
    {
      get
      {
        Color ret;
        RenderSettings.get_subtractiveShadowColor_Injected(out ret);
        return ret;
      }
      set
      {
        RenderSettings.set_subtractiveShadowColor_Injected(ref value);
      }
    }

    /// <summary>
    ///   <para>The global skybox to use.</para>
    /// </summary>
    public static extern Material skybox { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The light used by the procedural skybox.</para>
    /// </summary>
    public static extern Light sun { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much the skybox / custom cubemap reflection affects the scene.</para>
    /// </summary>
    public static extern float reflectionIntensity { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The number of times a reflection includes other reflections.</para>
    /// </summary>
    public static extern int reflectionBounces { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Default reflection mode.</para>
    /// </summary>
    public static extern DefaultReflectionMode defaultReflectionMode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Cubemap resolution for default reflection.</para>
    /// </summary>
    public static extern int defaultReflectionResolution { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Size of the Light halos.</para>
    /// </summary>
    public static extern float haloStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The intensity of all flares in the scene.</para>
    /// </summary>
    public static extern float flareStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The fade speed of all flares in the scene.</para>
    /// </summary>
    public static extern float flareFadeSpeed { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void get_fogColor_Injected(out Color ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void set_fogColor_Injected(ref Color value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void get_ambientSkyColor_Injected(out Color ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void set_ambientSkyColor_Injected(ref Color value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void get_ambientEquatorColor_Injected(out Color ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void set_ambientEquatorColor_Injected(ref Color value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void get_ambientGroundColor_Injected(out Color ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void set_ambientGroundColor_Injected(ref Color value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void get_ambientLight_Injected(out Color ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void set_ambientLight_Injected(ref Color value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void get_subtractiveShadowColor_Injected(out Color ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void set_subtractiveShadowColor_Injected(ref Color value);
  }
}
