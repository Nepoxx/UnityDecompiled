// Decompiled with JetBrains decompiler
// Type: UnityEngine.LightProbeProxyVolume
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The Light Probe Proxy Volume component offers the possibility to use higher resolution lighting for large non-static GameObjects.</para>
  /// </summary>
  public sealed class LightProbeProxyVolume : Behaviour
  {
    /// <summary>
    ///   <para>The world-space bounding box in which the 3D grid of interpolated Light Probes is generated.</para>
    /// </summary>
    public Bounds boundsGlobal
    {
      get
      {
        Bounds bounds;
        this.INTERNAL_get_boundsGlobal(out bounds);
        return bounds;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_boundsGlobal(out Bounds value);

    /// <summary>
    ///   <para>The size of the bounding box in which the 3D grid of interpolated Light Probes is generated.</para>
    /// </summary>
    public Vector3 sizeCustom
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_sizeCustom(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_sizeCustom(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_sizeCustom(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_sizeCustom(ref Vector3 value);

    /// <summary>
    ///   <para>The local-space origin of the bounding box in which the 3D grid of interpolated Light Probes is generated.</para>
    /// </summary>
    public Vector3 originCustom
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_originCustom(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_originCustom(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_originCustom(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_originCustom(ref Vector3 value);

    /// <summary>
    ///   <para>The bounding box mode for generating the 3D grid of interpolated Light Probes.</para>
    /// </summary>
    public extern LightProbeProxyVolume.BoundingBoxMode boundingBoxMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The resolution mode for generating the grid of interpolated Light Probes.</para>
    /// </summary>
    public extern LightProbeProxyVolume.ResolutionMode resolutionMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The mode in which the interpolated Light Probe positions are generated.</para>
    /// </summary>
    public extern LightProbeProxyVolume.ProbePositionMode probePositionMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Sets the way the Light Probe Proxy Volume refreshes.</para>
    /// </summary>
    public extern LightProbeProxyVolume.RefreshMode refreshMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Interpolated Light Probe density.</para>
    /// </summary>
    public extern float probeDensity { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The 3D grid resolution on the z-axis.</para>
    /// </summary>
    public extern int gridResolutionX { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The 3D grid resolution on the y-axis.</para>
    /// </summary>
    public extern int gridResolutionY { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The 3D grid resolution on the z-axis.</para>
    /// </summary>
    public extern int gridResolutionZ { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Triggers an update of the Light Probe Proxy Volume.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Update();

    /// <summary>
    ///   <para>Checks if Light Probe Proxy Volumes are supported.</para>
    /// </summary>
    public static extern bool isFeatureSupported { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The resolution mode for generating a grid of interpolated Light Probes.</para>
    /// </summary>
    public enum ResolutionMode
    {
      Automatic,
      Custom,
    }

    /// <summary>
    ///   <para>The bounding box mode for generating a grid of interpolated Light Probes.</para>
    /// </summary>
    public enum BoundingBoxMode
    {
      AutomaticLocal,
      AutomaticWorld,
      Custom,
    }

    /// <summary>
    ///   <para>The mode in which the interpolated Light Probe positions are generated.</para>
    /// </summary>
    public enum ProbePositionMode
    {
      CellCorner,
      CellCenter,
    }

    /// <summary>
    ///   <para>An enum describing the way a Light Probe Proxy Volume refreshes in the Player.</para>
    /// </summary>
    public enum RefreshMode
    {
      Automatic,
      EveryFrame,
      ViaScripting,
    }
  }
}
