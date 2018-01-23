// Decompiled with JetBrains decompiler
// Type: UnityEngine.FrameTimingManager
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public static class FrameTimingManager
  {
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CaptureFrameTimings();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern uint GetLatestTimings(uint numFrames, FrameTiming[] timings);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float GetVSyncsPerSecond();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ulong GetGpuTimerFrequency();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ulong GetCpuTimerFrequency();
  }
}
