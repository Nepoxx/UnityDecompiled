// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimEditorOverlay
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  [Serializable]
  internal class AnimEditorOverlay
  {
    public AnimationWindowState state;
    private TimeCursorManipulator m_PlayHeadCursor;
    private Rect m_Rect;
    private Rect m_ContentRect;

    public Rect rect
    {
      get
      {
        return this.m_Rect;
      }
    }

    public Rect contentRect
    {
      get
      {
        return this.m_ContentRect;
      }
    }

    public void Initialize()
    {
      if (this.m_PlayHeadCursor != null)
        return;
      this.m_PlayHeadCursor = new TimeCursorManipulator(AnimationWindowStyles.playHead);
      TimeCursorManipulator playHeadCursor1 = this.m_PlayHeadCursor;
      playHeadCursor1.onStartDrag = playHeadCursor1.onStartDrag + (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) =>
      {
        if ((double) evt.mousePosition.y <= (double) this.m_Rect.yMin + 20.0)
          return this.OnStartDragPlayHead(evt);
        return false;
      });
      TimeCursorManipulator playHeadCursor2 = this.m_PlayHeadCursor;
      playHeadCursor2.onDrag = playHeadCursor2.onDrag + (AnimationWindowManipulator.OnDragDelegate) ((manipulator, evt) => this.OnDragPlayHead(evt));
      TimeCursorManipulator playHeadCursor3 = this.m_PlayHeadCursor;
      playHeadCursor3.onEndDrag = playHeadCursor3.onEndDrag + (AnimationWindowManipulator.OnEndDragDelegate) ((manipulator, evt) => this.OnEndDragPlayHead(evt));
    }

    public void OnGUI(Rect rect, Rect contentRect)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      this.m_Rect = rect;
      this.m_ContentRect = contentRect;
      this.Initialize();
      this.m_PlayHeadCursor.OnGUI(this.m_Rect, this.m_Rect.xMin + this.TimeToPixel(this.state.currentTime));
    }

    public void HandleEvents()
    {
      this.Initialize();
      this.m_PlayHeadCursor.HandleEvents();
    }

    private bool OnStartDragPlayHead(Event evt)
    {
      this.state.controlInterface.StopPlayback();
      this.state.controlInterface.StartScrubTime();
      this.state.controlInterface.ScrubTime(this.MousePositionToTime(evt));
      return true;
    }

    private bool OnDragPlayHead(Event evt)
    {
      this.state.controlInterface.ScrubTime(this.MousePositionToTime(evt));
      return true;
    }

    private bool OnEndDragPlayHead(Event evt)
    {
      this.state.controlInterface.EndScrubTime();
      return true;
    }

    public float MousePositionToTime(Event evt)
    {
      float width = this.m_ContentRect.width;
      return this.state.SnapToFrame(Mathf.Max(evt.mousePosition.x / width * this.state.visibleTimeSpan + this.state.minVisibleTime, 0.0f), AnimationWindowState.SnapMode.SnapToFrame);
    }

    public float MousePositionToValue(Event evt)
    {
      float num1 = this.m_ContentRect.height - evt.mousePosition.y;
      TimeArea timeArea = this.state.timeArea;
      float num2 = timeArea.m_Scale.y * -1f;
      float num3 = (float) ((double) timeArea.shownArea.yMin * (double) num2 * -1.0);
      return (num1 - num3) / num2;
    }

    public float TimeToPixel(float time)
    {
      return this.state.TimeToPixel(time);
    }

    public float ValueToPixel(float value)
    {
      TimeArea timeArea = this.state.timeArea;
      float num1 = timeArea.m_Scale.y * -1f;
      float num2 = (float) ((double) timeArea.shownArea.yMin * (double) num1 * -1.0);
      return value * num1 + num2;
    }
  }
}
