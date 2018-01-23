// Decompiled with JetBrains decompiler
// Type: UnityEngine.VR.VRStats
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.VR
{
  [Obsolete("VRStats has been moved and renamed.  Use UnityEngine.XR.XRStats instead (UnityUpgradable) -> UnityEngine.XR.XRStats", true)]
  public static class VRStats
  {
    public static bool TryGetGPUTimeLastFrame(out float gpuTimeLastFrame)
    {
      gpuTimeLastFrame = 0.0f;
      throw new NotSupportedException("VRStats has been moved and renamed.  Use UnityEngine.XR.XRStats instead.");
    }

    public static bool TryGetDroppedFrameCount(out int droppedFrameCount)
    {
      droppedFrameCount = 0;
      throw new NotSupportedException("VRStats has been moved and renamed.  Use UnityEngine.XR.XRStats instead.");
    }

    public static bool TryGetFramePresentCount(out int framePresentCount)
    {
      framePresentCount = 0;
      throw new NotSupportedException("VRStats has been moved and renamed.  Use UnityEngine.XR.XRStats instead.");
    }

    [Obsolete("gpuTimeLastFrame is deprecated. Use XRStats.TryGetGPUTimeLastFrame instead.", true)]
    public static float gpuTimeLastFrame
    {
      get
      {
        throw new NotSupportedException("VRStats has been moved and renamed.  Use UnityEngine.XR.XRStats instead.");
      }
    }
  }
}
