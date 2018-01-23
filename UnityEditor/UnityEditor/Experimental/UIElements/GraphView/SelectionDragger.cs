// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.SelectionDragger
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class SelectionDragger : Dragger
  {
    private bool m_AddedOnMouseDown;
    private bool m_Dragged;

    public SelectionDragger()
    {
      this.activators.Add(new ManipulatorActivationFilter()
      {
        button = MouseButton.LeftMouse
      });
      this.panSpeed = new Vector2(1f, 1f);
      this.clampToParentEdges = false;
    }

    private GraphElement selectedElement { get; set; }

    private GraphElement clickedElement { get; set; }

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

    protected new void OnMouseDown(MouseDownEvent e)
    {
      UnityEditor.Experimental.UIElements.GraphView.GraphView target = this.target as UnityEditor.Experimental.UIElements.GraphView.GraphView;
      if (target == null)
        return;
      this.selectedElement = (GraphElement) null;
      this.clickedElement = e.target as GraphElement;
      if (this.clickedElement == null)
      {
        this.clickedElement = (e.target as VisualElement).GetFirstAncestorOfType<GraphElement>();
        if (this.clickedElement == null)
          return;
      }
      if (!target.selection.Contains((ISelectable) this.clickedElement))
      {
        if (!e.ctrlKey)
          target.ClearSelection();
        target.AddToSelection((ISelectable) this.clickedElement);
        this.m_AddedOnMouseDown = true;
      }
      if (!this.CanStartManipulation((IMouseEvent) e))
        return;
      this.selectedElement = this.clickedElement;
      if ((UnityEngine.Object) this.selectedElement.presenter != (UnityEngine.Object) null && (this.selectedElement.presenter.capabilities & Capabilities.Movable) != Capabilities.Movable)
        return;
      this.m_Active = true;
      this.target.TakeCapture();
    }

    protected new void OnMouseMove(MouseMoveEvent e)
    {
      if (!this.m_Active)
        return;
      UnityEditor.Experimental.UIElements.GraphView.GraphView target = this.target as UnityEditor.Experimental.UIElements.GraphView.GraphView;
      if (target == null)
        return;
      foreach (ISelectable selectable in target.selection)
      {
        GraphElement graphElement = selectable as GraphElement;
        if (graphElement != null && !((UnityEngine.Object) graphElement.presenter == (UnityEngine.Object) null) && (graphElement.presenter.capabilities & Capabilities.Movable) == Capabilities.Movable)
        {
          this.m_Dragged = true;
          Matrix4x4 worldTransform = graphElement.worldTransform;
          Vector3 vector3 = new Vector3(worldTransform.m00, worldTransform.m11, worldTransform.m22);
          graphElement.SetPosition(this.CalculatePosition(graphElement.layout.x + e.mouseDelta.x * this.panSpeed.x / vector3.x, graphElement.layout.y + e.mouseDelta.y * this.panSpeed.y / vector3.y, graphElement.layout.width, graphElement.layout.height));
        }
      }
      this.selectedElement = (GraphElement) null;
      e.StopPropagation();
    }

    protected new void OnMouseUp(MouseUpEvent e)
    {
      UnityEditor.Experimental.UIElements.GraphView.GraphView target = this.target as UnityEditor.Experimental.UIElements.GraphView.GraphView;
      if (target == null)
        return;
      if (this.clickedElement != null && !this.m_Dragged && target.selection.Contains((ISelectable) this.clickedElement))
      {
        if (e.ctrlKey)
        {
          if (!this.m_AddedOnMouseDown)
            target.RemoveFromSelection((ISelectable) this.clickedElement);
        }
        else
        {
          target.ClearSelection();
          target.AddToSelection((ISelectable) this.clickedElement);
        }
      }
      if (this.m_Active && this.CanStopManipulation((IMouseEvent) e))
      {
        if (this.selectedElement == null)
        {
          foreach (ISelectable selectable in target.selection)
          {
            GraphElement graphElement = selectable as GraphElement;
            if (graphElement != null && !((UnityEngine.Object) graphElement.presenter == (UnityEngine.Object) null))
            {
              GraphElementPresenter presenter = graphElement.presenter;
              if ((graphElement.presenter.capabilities & Capabilities.Movable) == Capabilities.Movable)
              {
                presenter.position = graphElement.layout;
                presenter.CommitChanges();
              }
            }
          }
        }
        this.target.ReleaseCapture();
        e.StopPropagation();
      }
      this.m_AddedOnMouseDown = false;
      this.m_Dragged = false;
      this.m_Active = false;
    }
  }
}
