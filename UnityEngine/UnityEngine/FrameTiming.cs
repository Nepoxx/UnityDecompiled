// Decompiled with JetBrains decompiler
// Type: UnityEngine.FrameTiming
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Bindings;

namespace UnityEngine
{
  public struct FrameTiming
  {
    [NativeName("m_CPUTimePresentCalled")]
    public ulong cpuTimePresentCalled;
    [NativeName("m_CPUFrameTime")]
    public double cpuFrameTime;
    [NativeName("m_CPUTimeFrameComplete")]
    public ulong cpuTimeFrameComplete;
    [NativeName("m_GPUFrameTime")]
    public double gpuFrameTime;
    [NativeName("m_HeightScale")]
    public float heightScale;
    [NativeName("m_WidthScale")]
    public float widthScale;
    [NativeName("m_SyncInterval")]
    public uint syncInterval;
  }
}
