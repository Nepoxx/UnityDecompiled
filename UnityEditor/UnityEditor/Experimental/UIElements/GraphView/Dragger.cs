// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.Dragger
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class Dragger : MouseManipulator
  {
    private Vector2 m_Start;
    protected bool m_Active;

    public Dragger()
    {
      this.activators.Add(new ManipulatorActivationFilter()
      {
        button = MouseButton.LeftMouse
      });
      this.panSpeed = new Vector2(1f, 1f);
      this.clampToParentEdges = false;
      this.m_Active = false;
    }

    public Vector2 panSpeed { get; set; }

    public GraphElementPresenter presenter { get; set; }

    public bool clampToParentEdges { get; set; }

    protected Rect CalculatePosition(float x, float y, float width, float height)
    {
      Rect rect = new Rect(x, y, width, height);
      if (this.clampToParentEdges)
      {
        if ((double) rect.x < (double) this.target.parent.layout.xMin)
          rect.x = this.target.parent.layout.xMin;
        else if ((double) rect.xMax > (double) this.target.parent.layout.xMax)
          rect.x = this.target.parent.layout.xMax - rect.width;
        if ((double) rect.y < (double) this.target.parent.layout.yMin)
          rect.y = this.target.parent.layout.yMin;
        else if ((double) rect.yMax > (double) this.target.parent.layout.yMax)
          rect.y = this.target.parent.layout.yMax - rect.height;
        rect.width = width;
        rect.height = height;
      }
      return rect;
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

    protected void OnMouseDown(MouseDownEvent e)
    {
      GraphElement target1 = e.target as GraphElement;
      if (target1 != null)
      {
        GraphElementPresenter presenter = target1.presenter;
        if ((Object) presenter != (Object) null && (presenter.capabilities & Capabilities.Movable) != Capabilities.Movable)
          return;
      }
      if (!this.CanStartManipulation((IMouseEvent) e))
        return;
      GraphElement target2 = this.target as GraphElement;
      if (target2 != null)
        this.presenter = target2.presenter;
      this.m_Start = e.localMousePosition;
      this.m_Active = true;
      this.target.TakeCapture();
      e.StopPropagation();
    }

    protected void OnMouseMove(MouseMoveEvent e)
    {
      GraphElement target = e.target as GraphElement;
      if (target != null)
      {
        GraphElementPresenter presenter = target.presenter;
        if ((Object) presenter != (Object) null && (presenter.capabilities & Capabilities.Movable) != Capabilities.Movable)
          return;
      }
      if (!this.m_Active)
        return;
      if ((PositionType) this.target.style.positionType == PositionType.Manual)
      {
        Vector2 vector2 = e.localMousePosition - this.m_Start;
        this.target.layout = this.CalculatePosition(this.target.layout.x + vector2.x, this.target.layout.y + vector2.y, this.target.layout.width, this.target.layout.height);
      }
      e.StopPropagation();
    }

    protected void OnMouseUp(MouseUpEvent e)
    {
      GraphElement target = e.target as GraphElement;
      if (target != null)
      {
        GraphElementPresenter presenter = target.presenter;
        if ((Object) presenter != (Object) null && (presenter.capabilities & Capabilities.Movable) != Capabilities.Movable)
          return;
      }
      if (!this.m_Active || !this.CanStopManipulation((IMouseEvent) e))
        return;
      this.presenter.position = this.target.layout;
      this.presenter.CommitChanges();
      this.presenter = (GraphElementPresenter) null;
      this.m_Active = false;
      this.target.ReleaseCapture();
      e.StopPropagation();
    }
  }
}
