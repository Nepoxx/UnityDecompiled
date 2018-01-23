// Decompiled with JetBrains decompiler
// Type: UnityEngine.VR.VRDevice
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.VR
{
  [Obsolete("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead (UnityUpgradable) -> UnityEngine.XR.XRDevice", true)]
  public static class VRDevice
  {
    public static bool isPresent
    {
      get
      {
        throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
      }
    }

    public static UnityEngine.XR.UserPresenceState userPresence
    {
      get
      {
        throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
      }
    }

    [Obsolete("family is deprecated.  Use XRSettings.loadedDeviceName instead.", true)]
    public static string family
    {
      get
      {
        throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
      }
    }

    public static string model
    {
      get
      {
        throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
      }
    }

    public static float refreshRate
    {
      get
      {
        throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
      }
    }

    public static UnityEngine.XR.TrackingSpaceType GetTrackingSpaceType()
    {
      throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
    }

    public static bool SetTrackingSpaceType(UnityEngine.XR.TrackingSpaceType trackingSpaceType)
    {
      throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
    }

    public static IntPtr GetNativePtr()
    {
      throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
    }

    [Obsolete("DisableAutoVRCameraTracking has been moved and renamed.  Use UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking instead (UnityUpgradable) -> UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking(*)", true)]
    public static void DisableAutoVRCameraTracking(Camera camera, bool disabled)
    {
      throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
    }
  }
}
