// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.TreeViewAnimationInput
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  internal class TreeViewAnimationInput
  {
    public Action<TreeViewAnimationInput> animationEnded;

    public TreeViewAnimationInput()
    {
      double timeSinceStartup = EditorApplication.timeSinceStartup;
      this.timeCaptured = timeSinceStartup;
      this.startTime = timeSinceStartup;
    }

    public void CaptureTime()
    {
      this.timeCaptured = EditorApplication.timeSinceStartup;
    }

    public float elapsedTimeNormalized
    {
      get
      {
        return Mathf.Clamp01((float) this.elapsedTime / (float) this.animationDuration);
      }
    }

    public double elapsedTime
    {
      get
      {
        return this.timeCaptured - this.startTime;
      }
      set
      {
        this.startTime = this.timeCaptured - value;
      }
    }

    public int startRow { get; set; }

    public int endRow { get; set; }

    public Rect rowsRect { get; set; }

    public Rect startRowRect { get; set; }

    public double startTime { get; set; }

    public double timeCaptured { get; set; }

    public double animationDuration { get; set; }

    public bool expanding { get; set; }

    public bool includeChildren { get; set; }

    public TreeViewItem item { get; set; }

    public TreeViewController treeView { get; set; }

    public void FireAnimationEndedEvent()
    {
      if (this.animationEnded == null)
        return;
      this.animationEnded(this);
    }

    public override string ToString()
    {
      return "Input: startRow " + (object) this.startRow + " endRow " + (object) this.endRow + " rowsRect " + (object) this.rowsRect + " startTime " + (object) this.startTime + " anitmationDuration" + (object) this.animationDuration + " " + (object) this.expanding + " " + this.item.displayName;
    }
  }
}
