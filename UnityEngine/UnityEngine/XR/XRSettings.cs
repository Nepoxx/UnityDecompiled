// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.XRSettings
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
  /// <summary>
  ///   <para>Global XR related settings.</para>
  /// </summary>
  public static class XRSettings
  {
    /// <summary>
    ///   <para>Globally enables or disables XR for the application.</para>
    /// </summary>
    public static extern bool enabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Read-only value that can be used to determine if the XR device is active.</para>
    /// </summary>
    public static extern bool isDeviceActive { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Mirror what is shown on the device to the main display, if possible.</para>
    /// </summary>
    public static extern bool showDeviceView { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>This field has been deprecated. Use XRSettings.eyeTextureResolutionScale instead.</para>
    /// </summary>
    [Obsolete("renderScale is deprecated, use XRSettings.eyeTextureResolutionScale instead (UnityUpgradable) -> eyeTextureResolutionScale")]
    public static extern float renderScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Controls the actual size of eye textures as a multiplier of the device's default resolution.</para>
    /// </summary>
    public static extern float eyeTextureResolutionScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The current width of an eye texture for the loaded device.</para>
    /// </summary>
    public static extern int eyeTextureWidth { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The current height of an eye texture for the loaded device.</para>
    /// </summary>
    public static extern int eyeTextureHeight { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Fetch the eye texture RenderTextureDescriptor from the active stereo device.</para>
    /// </summary>
    public static RenderTextureDescriptor eyeTextureDesc
    {
      get
      {
        RenderTextureDescriptor textureDescriptor;
        XRSettings.INTERNAL_get_eyeTextureDesc(out textureDescriptor);
        return textureDescriptor;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_eyeTextureDesc(out RenderTextureDescriptor value);

    /// <summary>
    ///   <para>Controls how much of the allocated eye texture should be used for rendering.</para>
    /// </summary>
    public static float renderViewportScale
    {
      get
      {
        return XRSettings.renderViewportScaleInternal;
      }
      set
      {
        if ((double) value < 0.0 || (double) value > 1.0)
          throw new ArgumentOutOfRangeException(nameof (value), "Render viewport scale should be between 0 and 1.");
        XRSettings.renderViewportScaleInternal = value;
      }
    }

    internal static extern float renderViewportScaleInternal { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A scale applied to the standard occulsion mask for each platform.</para>
    /// </summary>
    public static extern float occlusionMaskScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Specifies whether or not the occlusion mesh should be used when rendering. Enabled by default.</para>
    /// </summary>
    public static extern bool useOcclusionMesh { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Type of XR device that is currently loaded.</para>
    /// </summary>
    public static extern string loadedDeviceName { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Loads the requested device at the beginning of the next frame.</para>
    /// </summary>
    /// <param name="deviceName">Name of the device from XRSettings.supportedDevices.</param>
    /// <param name="prioritizedDeviceNameList">Prioritized list of device names from XRSettings.supportedDevices.</param>
    public static void LoadDeviceByName(string deviceName)
    {
      XRSettings.LoadDeviceByName(new string[1]
      {
        deviceName
      });
    }

    /// <summary>
    ///   <para>Loads the requested device at the beginning of the next frame.</para>
    /// </summary>
    /// <param name="deviceName">Name of the device from XRSettings.supportedDevices.</param>
    /// <param name="prioritizedDeviceNameList">Prioritized list of device names from XRSettings.supportedDevices.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void LoadDeviceByName(string[] prioritizedDeviceNameList);

    /// <summary>
    ///   <para>Returns a list of supported XR devices that were included at build time.</para>
    /// </summary>
    public static extern string[] supportedDevices { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
