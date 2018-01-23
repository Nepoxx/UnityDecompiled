// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.NativeProfilerTimeline_DrawArgs
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditorInternal
{
  public struct NativeProfilerTimeline_DrawArgs
  {
    public int frameIndex;
    public int threadIndex;
    public float timeOffset;
    public Rect threadRect;
    public Rect shownAreaRect;
    public int selectedEntryIndex;
    public int mousedOverEntryIndex;

    public void Reset()
    {
      this.frameIndex = -1;
      this.threadIndex = -1;
      this.timeOffset = 0.0f;
      this.threadRect = Rect.zero;
      this.shownAreaRect = Rect.zero;
      this.selectedEntryIndex = -1;
      this.mousedOverEntryIndex = -1;
    }
  }
}
