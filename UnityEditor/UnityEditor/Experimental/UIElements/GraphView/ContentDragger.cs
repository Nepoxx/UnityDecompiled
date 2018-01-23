// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.ContentDragger
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class ContentDragger : MouseManipulator
  {
    private Vector2 m_Start;
    private bool m_Active;

    public ContentDragger()
    {
      this.m_Active = false;
      this.activators.Add(new ManipulatorActivationFilter()
      {
        button = MouseButton.LeftMouse,
        modifiers = EventModifiers.Alt
      });
      this.activators.Add(new ManipulatorActivationFilter()
      {
        button = MouseButton.MiddleMouse
      });
      this.panSpeed = new Vector2(1f, 1f);
      this.clampToParentEdges = false;
    }

    public Vector2 panSpeed { get; set; }

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
      if (!(this.target is UnityEditor.Experimental.UIElements.GraphView.GraphView))
        throw new InvalidOperationException("Manipulator can only be added to a GraphView");
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
      if (!this.CanStartManipulation((IMouseEvent) e))
        return;
      UnityEditor.Experimental.UIElements.GraphView.GraphView target = this.target as UnityEditor.Experimental.UIElements.GraphView.GraphView;
      if (target == null)
        return;
      this.m_Start = target.ChangeCoordinatesTo(target.contentViewContainer, e.localMousePosition);
      this.m_Active = true;
      this.target.TakeCapture();
      e.StopPropagation();
    }

    protected void OnMouseMove(MouseMoveEvent e)
    {
      if (!this.m_Active || !this.target.HasCapture())
        return;
      UnityEditor.Experimental.UIElements.GraphView.GraphView target = this.target as UnityEditor.Experimental.UIElements.GraphView.GraphView;
      if (target == null)
        return;
      Vector2 vector2 = target.ChangeCoordinatesTo(target.contentViewContainer, e.localMousePosition) - this.m_Start;
      Vector3 scale = target.contentViewContainer.transform.scale;
      target.viewTransform.position += Vector3.Scale((Vector3) vector2, scale);
      e.StopPropagation();
    }

    protected void OnMouseUp(MouseUpEvent e)
    {
      if (!this.m_Active || !this.CanStopManipulation((IMouseEvent) e))
        return;
      UnityEditor.Experimental.UIElements.GraphView.GraphView target = this.target as UnityEditor.Experimental.UIElements.GraphView.GraphView;
      if (target == null)
        return;
      Vector3 position = target.contentViewContainer.transform.position;
      Vector3 scale = target.contentViewContainer.transform.scale;
      target.UpdateViewTransform(position, scale);
      this.m_Active = false;
      this.target.ReleaseCapture();
      e.StopPropagation();
    }
  }
}
