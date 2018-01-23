// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.SelectionDropper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class SelectionDropper : Manipulator
  {
    private readonly DragAndDropDelay m_DragAndDropDelay;
    private bool m_Active;
    private bool m_AddedByMouseDown;
    private bool m_Dragging;
    public IDropTarget prevDropTarget;

    public SelectionDropper(DropEvent handler)
    {
      this.m_Active = true;
      this.OnDrop += handler;
      this.m_DragAndDropDelay = new DragAndDropDelay();
      this.activateButton = MouseButton.LeftMouse;
      this.panSpeed = new Vector2(1f, 1f);
    }

    public event DropEvent OnDrop;

    public Vector2 panSpeed { get; set; }

    public MouseButton activateButton { get; set; }

    public bool clampToParentEdges { get; set; }

    private GraphElement selectedElement { get; set; }

    private ISelection selectionContainer { get; set; }

    protected override void RegisterCallbacksOnTarget()
    {
      this.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
      this.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
      this.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
      this.target.RegisterCallback<IMGUIEvent>(new EventCallback<IMGUIEvent>(this.OnIMGUIEvent), Capture.NoCapture);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
      this.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
      this.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
      this.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
      this.target.UnregisterCallback<IMGUIEvent>(new EventCallback<IMGUIEvent>(this.OnIMGUIEvent), Capture.NoCapture);
    }

    protected void OnMouseDown(MouseDownEvent e)
    {
      this.m_Active = false;
      this.m_Dragging = false;
      this.m_AddedByMouseDown = false;
      if (this.target == null)
        return;
      this.selectionContainer = this.target.GetFirstAncestorOfType<ISelection>();
      if (this.selectionContainer == null)
        return;
      this.selectedElement = this.target.GetFirstOfType<GraphElement>();
      if (this.selectedElement == null)
        return;
      if (!this.selectionContainer.selection.Contains((ISelectable) this.selectedElement))
      {
        if (!e.ctrlKey)
          this.selectionContainer.ClearSelection();
        this.selectionContainer.AddToSelection((ISelectable) this.selectedElement);
        this.m_AddedByMouseDown = true;
      }
      if ((MouseButton) e.button != this.activateButton)
        return;
      GraphElementPresenter presenter = this.selectedElement.presenter;
      if ((Object) presenter != (Object) null && (presenter.capabilities & Capabilities.Droppable) != Capabilities.Droppable)
        return;
      this.m_DragAndDropDelay.Init(e.localMousePosition);
      this.m_Active = true;
      this.target.TakeCapture();
      e.StopPropagation();
    }

    protected void OnMouseMove(MouseMoveEvent e)
    {
      if (!this.m_Active || this.m_Dragging || this.selectionContainer == null)
        return;
      List<ISelectable> list = this.selectionContainer.selection.ToList<ISelectable>();
      if (list.Count > 0)
      {
        bool flag = false;
        GraphElement graphElement = list[0] as GraphElement;
        if (graphElement != null)
        {
          GraphElementPresenter presenter = graphElement.presenter;
          if ((Object) presenter != (Object) null)
            flag = (presenter.capabilities & Capabilities.Droppable) == Capabilities.Droppable;
        }
        if (flag && this.m_DragAndDropDelay.CanStartDrag(e.localMousePosition))
        {
          DragAndDrop.PrepareStartDrag();
          DragAndDrop.objectReferences = new Object[0];
          DragAndDrop.SetGenericData("DragSelection", (object) list);
          this.m_Dragging = true;
          DragAndDrop.StartDrag("");
          DragAndDrop.visualMode = !e.ctrlKey ? DragAndDropVisualMode.Move : DragAndDropVisualMode.Copy;
        }
        e.StopPropagation();
      }
    }

    protected void OnMouseUp(MouseUpEvent e)
    {
      if (!this.m_Active || this.selectionContainer == null)
        return;
      if ((MouseButton) e.button == this.activateButton)
      {
        if (!e.ctrlKey)
        {
          this.selectionContainer.ClearSelection();
          this.selectionContainer.AddToSelection((ISelectable) this.selectedElement);
        }
        else if (!this.m_AddedByMouseDown && !this.m_Dragging)
          this.selectionContainer.RemoveFromSelection((ISelectable) this.selectedElement);
        this.target.ReleaseCapture();
        e.StopPropagation();
      }
      this.m_Active = false;
      this.m_AddedByMouseDown = false;
      this.m_Dragging = false;
    }

    protected void OnIMGUIEvent(IMGUIEvent e)
    {
      if (!this.m_Active || this.selectionContainer == null)
        return;
      List<ISelectable> list = this.selectionContainer.selection.ToList<ISelectable>();
      Event imguiEvent = e.imguiEvent;
      switch (imguiEvent.type)
      {
        case EventType.DragUpdated:
          if (!this.target.HasCapture() || (MouseButton) imguiEvent.button != this.activateButton || list.Count <= 0)
            break;
          this.selectedElement = (GraphElement) null;
          // ISSUE: reference to a compiler-generated field
          if (this.OnDrop != null)
          {
            VisualElement visualElement = this.target.panel.Pick(this.target.LocalToWorld(imguiEvent.mousePosition));
            IDropTarget dropTarget = visualElement == null ? (IDropTarget) null : visualElement.GetFirstAncestorOfType<IDropTarget>();
            if (this.prevDropTarget != dropTarget && this.prevDropTarget != null)
            {
              IMGUIEvent pooled = IMGUIEvent.GetPooled(e.imguiEvent);
              pooled.imguiEvent.type = EventType.DragExited;
              // ISSUE: reference to a compiler-generated field
              this.OnDrop(pooled, list, this.prevDropTarget);
              EventBase<IMGUIEvent>.ReleasePooled(pooled);
            }
            // ISSUE: reference to a compiler-generated field
            this.OnDrop(e, list, dropTarget);
            this.prevDropTarget = dropTarget;
          }
          DragAndDrop.visualMode = !imguiEvent.control ? DragAndDropVisualMode.Move : DragAndDropVisualMode.Copy;
          break;
        case EventType.DragPerform:
          if (this.m_Active && (MouseButton) imguiEvent.button == this.activateButton && (list.Count > 0 && list.Count > 0))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.OnDrop != null)
            {
              VisualElement visualElement = this.target.panel.Pick(this.target.LocalToWorld(imguiEvent.mousePosition));
              IDropTarget dropTarget = visualElement == null ? (IDropTarget) null : visualElement.GetFirstAncestorOfType<IDropTarget>();
              // ISSUE: reference to a compiler-generated field
              this.OnDrop(e, list, dropTarget);
            }
            DragAndDrop.visualMode = DragAndDropVisualMode.None;
            DragAndDrop.SetGenericData("DragSelection", (object) null);
          }
          this.prevDropTarget = (IDropTarget) null;
          this.m_Active = false;
          this.target.ReleaseCapture();
          break;
        case EventType.DragExited:
          // ISSUE: reference to a compiler-generated field
          if (this.OnDrop != null && this.prevDropTarget != null)
          {
            // ISSUE: reference to a compiler-generated field
            this.OnDrop(e, list, this.prevDropTarget);
          }
          DragAndDrop.visualMode = DragAndDropVisualMode.None;
          DragAndDrop.SetGenericData("DragSelection", (object) null);
          this.prevDropTarget = (IDropTarget) null;
          this.m_Active = false;
          this.target.ReleaseCapture();
          break;
      }
    }
  }
}
