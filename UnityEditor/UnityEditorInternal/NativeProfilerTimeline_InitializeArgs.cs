// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.NativeProfilerTimeline_InitializeArgs
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditorInternal
{
  public struct NativeProfilerTimeline_InitializeArgs
  {
    public Color[] profilerColors;
    public Color allocationSampleColor;
    public Color internalSampleColor;
    public float ghostAlpha;
    public float nonSelectedAlpha;
    public IntPtr guiStyle;
    public float lineHeight;
    public float textFadeOutWidth;
    public float textFadeStartWidth;

    public void Reset()
    {
      this.profilerColors = (Color[]) null;
      this.allocationSampleColor = Color.clear;
      this.internalSampleColor = Color.clear;
      this.ghostAlpha = 0.0f;
      this.nonSelectedAlpha = 0.0f;
      this.guiStyle = (IntPtr) 0;
      this.lineHeight = 0.0f;
      this.textFadeOutWidth = 0.0f;
      this.textFadeStartWidth = 0.0f;
    }
  }
}
