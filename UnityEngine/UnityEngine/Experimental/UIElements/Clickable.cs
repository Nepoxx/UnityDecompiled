// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.Clickable
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  public class Clickable : MouseManipulator
  {
    private readonly long m_Delay;
    private readonly long m_Interval;
    private IVisualElementScheduledItem m_Repeater;

    public Clickable(Action handler, long delay, long interval)
      : this(handler)
    {
      this.m_Delay = delay;
      this.m_Interval = interval;
    }

    public Clickable(Action handler)
    {
      // ISSUE: reference to a compiler-generated field
      this.clicked = handler;
      this.activators.Add(new ManipulatorActivationFilter()
      {
        button = MouseButton.LeftMouse
      });
    }

    public event Action clicked;

    public Vector2 lastMousePosition { get; private set; }

    private void OnTimer(TimerState timerState)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.clicked == null || !this.IsRepeatable())
        return;
      if (this.target.ContainsPoint(this.target.ChangeCoordinatesTo(this.target.shadow.parent, this.lastMousePosition)))
      {
        // ISSUE: reference to a compiler-generated field
        this.clicked();
        this.target.pseudoStates |= PseudoStates.Active;
      }
      else
        this.target.pseudoStates &= ~PseudoStates.Active;
    }

    private bool IsRepeatable()
    {
      return this.m_Delay > 0L || this.m_Interval > 0L;
    }

    protected override void RegisterCallbacksOnTarget()
    {
      this.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
      this.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
      this.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
      this.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
      this.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
      this.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
    }

    protected void OnMouseDown(MouseDownEvent evt)
    {
      if (!this.CanStartManipulation((IMouseEvent) evt))
        return;
      this.target.TakeCapture();
      this.lastMousePosition = evt.localMousePosition;
      if (this.IsRepeatable())
      {
        // ISSUE: reference to a compiler-generated field
        if (this.clicked != null && this.target.ContainsPoint(this.target.ChangeCoordinatesTo(this.target.shadow.parent, evt.localMousePosition)))
        {
          // ISSUE: reference to a compiler-generated field
          this.clicked();
        }
        if (this.m_Repeater == null)
          this.m_Repeater = this.target.schedule.Execute(new Action<TimerState>(this.OnTimer)).Every(this.m_Interval).StartingIn(this.m_Delay);
        else
          this.m_Repeater.ExecuteLater(this.m_Delay);
      }
      this.target.pseudoStates |= PseudoStates.Active;
      evt.StopPropagation();
    }

    protected void OnMouseMove(MouseMoveEvent evt)
    {
      if (!this.target.HasCapture())
        return;
      this.lastMousePosition = evt.localMousePosition;
      evt.StopPropagation();
    }

    protected void OnMouseUp(MouseUpEvent evt)
    {
      if (!this.CanStopManipulation((IMouseEvent) evt))
        return;
      this.target.ReleaseCapture();
      if (this.IsRepeatable())
      {
        if (this.m_Repeater != null)
          this.m_Repeater.Pause();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.clicked != null && this.target.ContainsPoint(this.target.ChangeCoordinatesTo(this.target.shadow.parent, evt.localMousePosition)))
        {
          // ISSUE: reference to a compiler-generated field
          this.clicked();
        }
      }
      this.target.pseudoStates &= ~PseudoStates.Active;
      evt.StopPropagation();
    }
  }
}
