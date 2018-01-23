// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.NativeProfilerTimeline_GetEntryAtPositionArgs
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditorInternal
{
  public struct NativeProfilerTimeline_GetEntryAtPositionArgs
  {
    public int frameIndex;
    public int threadIndex;
    public float timeOffset;
    public Rect threadRect;
    public Rect shownAreaRect;
    public Vector2 position;
    public int out_EntryIndex;
    public float out_EntryYMaxPos;
    public string out_EntryName;

    public void Reset()
    {
      this.frameIndex = -1;
      this.threadIndex = -1;
      this.timeOffset = 0.0f;
      this.threadRect = Rect.zero;
      this.shownAreaRect = Rect.zero;
      this.position = Vector2.zero;
      this.out_EntryIndex = -1;
      this.out_EntryYMaxPos = 0.0f;
      this.out_EntryName = string.Empty;
    }
  }
}
