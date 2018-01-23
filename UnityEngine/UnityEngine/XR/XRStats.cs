// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.XRStats
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
  /// <summary>
  ///   <para>Timing and other statistics from the XR subsystem.</para>
  /// </summary>
  public static class XRStats
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool TryGetGPUTimeLastFrame(out float gpuTimeLastFrame);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool TryGetDroppedFrameCount(out int droppedFrameCount);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool TryGetFramePresentCount(out int framePresentCount);

    /// <summary>
    ///   <para>Total GPU time utilized last frame as measured by the XR subsystem.</para>
    /// </summary>
    [Obsolete("gpuTimeLastFrame is deprecated. Use XRStats.TryGetGPUTimeLastFrame instead.")]
    public static float gpuTimeLastFrame
    {
      get
      {
        float gpuTimeLastFrame;
        if (XRStats.TryGetGPUTimeLastFrame(out gpuTimeLastFrame))
          return gpuTimeLastFrame;
        return 0.0f;
      }
    }
  }
}
