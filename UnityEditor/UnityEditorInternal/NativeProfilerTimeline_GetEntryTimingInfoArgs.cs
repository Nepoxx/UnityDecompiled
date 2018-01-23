// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.NativeProfilerTimeline_GetEntryTimingInfoArgs
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditorInternal
{
  public struct NativeProfilerTimeline_GetEntryTimingInfoArgs
  {
    public int frameIndex;
    public int threadIndex;
    public int entryIndex;
    public bool calculateFrameData;
    public float out_LocalStartTime;
    public float out_Duration;
    public float out_TotalDurationForFrame;
    public int out_InstanceCountForFrame;

    public void Reset()
    {
      this.frameIndex = -1;
      this.threadIndex = -1;
      this.entryIndex = -1;
      this.calculateFrameData = false;
      this.out_LocalStartTime = -1f;
      this.out_Duration = -1f;
      this.out_TotalDurationForFrame = -1f;
      this.out_InstanceCountForFrame = -1;
    }
  }
}
