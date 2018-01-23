// Decompiled with JetBrains decompiler
// Type: UnityEngine.VR.VRSettings
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.VR
{
  [Obsolete("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead (UnityUpgradable)", true)]
  public static class VRSettings
  {
    public static bool enabled
    {
      get
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
      set
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
    }

    public static bool isDeviceActive
    {
      get
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
    }

    public static bool showDeviceView
    {
      get
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
      set
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
    }

    public static float renderScale
    {
      get
      {
        throw new NotSupportedException("VRSettings.renderScale has been moved and renamed.  Use UnityEngine.XR.XRSettings.eyeTextureResolutionScale instead.");
      }
      set
      {
        throw new NotSupportedException("VRSettings.renderScale has been moved and renamed.  Use UnityEngine.XR.XRSettings.eyeTextureResolutionScale instead.");
      }
    }

    public static int eyeTextureWidth
    {
      get
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
    }

    public static int eyeTextureHeight
    {
      get
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
    }

    public static float renderViewportScale
    {
      get
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
      set
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
    }

    public static float occlusionMaskScale
    {
      get
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
      set
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
    }

    [Obsolete("loadedDevice is deprecated.  Use loadedDeviceName and LoadDeviceByName instead.", true)]
    public static VRDeviceType loadedDevice
    {
      get
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
      set
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
    }

    public static string loadedDeviceName
    {
      get
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
    }

    public static void LoadDeviceByName(string deviceName)
    {
      throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
    }

    public static void LoadDeviceByName(string[] prioritizedDeviceNameList)
    {
      throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
    }

    public static string[] supportedDevices
    {
      get
      {
        throw new NotSupportedException("VRSettings has been moved and renamed.  Use UnityEngine.XR.XRSettings instead.");
      }
    }
  }
}
