// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.ClampedDragger
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  internal class ClampedDragger : Clickable
  {
    public ClampedDragger(Slider slider, Action clickHandler, Action dragHandler)
      : base(clickHandler, 250L, 30L)
    {
      this.dragDirection = ClampedDragger.DragDirection.None;
      this.slider = slider;
      this.dragging += dragHandler;
    }

    public event Action dragging;

    public ClampedDragger.DragDirection dragDirection { get; set; }

    private Slider slider { get; set; }

    public Vector2 startMousePosition { get; private set; }

    public Vector2 delta
    {
      get
      {
        return this.lastMousePosition - this.startMousePosition;
      }
    }

    protected override void RegisterCallbacksOnTarget()
    {
      this.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
      this.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
      this.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(((Clickable) this).OnMouseUp), Capture.NoCapture);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
      this.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
      this.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
      this.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(((Clickable) this).OnMouseUp), Capture.NoCapture);
    }

    private new void OnMouseDown(MouseDownEvent evt)
    {
      if (!this.CanStartManipulation((IMouseEvent) evt))
        return;
      this.startMousePosition = evt.localMousePosition;
      this.dragDirection = ClampedDragger.DragDirection.None;
      base.OnMouseDown(evt);
    }

    private new void OnMouseMove(MouseMoveEvent evt)
    {
      if (!this.target.HasCapture())
        return;
      base.OnMouseMove(evt);
      if (this.dragDirection == ClampedDragger.DragDirection.None)
        this.dragDirection = ClampedDragger.DragDirection.Free;
      // ISSUE: reference to a compiler-generated field
      if (this.dragDirection == ClampedDragger.DragDirection.Free && this.dragging != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.dragging();
      }
    }

    [Flags]
    public enum DragDirection
    {
      None = 0,
      LowToHigh = 1,
      HighToLow = 2,
      Free = 4,
    }
  }
}
