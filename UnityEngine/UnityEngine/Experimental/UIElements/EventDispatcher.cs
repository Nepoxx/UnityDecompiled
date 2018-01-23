// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.EventDispatcher
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
  internal class EventDispatcher : IEventDispatcher
  {
    private VisualElement m_TopElementUnderMouse;

    public IEventHandler capture { get; set; }

    public void ReleaseCapture(IEventHandler handler)
    {
      Debug.Assert(handler == this.capture, "Element releasing capture does not have capture");
      this.capture = (IEventHandler) null;
    }

    public void RemoveCapture()
    {
      if (this.capture != null)
        this.capture.OnLostCapture();
      this.capture = (IEventHandler) null;
    }

    public void TakeCapture(IEventHandler handler)
    {
      if (this.capture == handler)
        return;
      if (GUIUtility.hotControl != 0)
      {
        Debug.Log((object) "Should not be capturing when there is a hotcontrol");
      }
      else
      {
        this.RemoveCapture();
        this.capture = handler;
      }
    }

    private void DispatchMouseEnterMouseLeave(VisualElement previousTopElementUnderMouse, VisualElement currentTopElementUnderMouse, IMouseEvent triggerEvent)
    {
      if (previousTopElementUnderMouse == currentTopElementUnderMouse)
        return;
      int num = 0;
      for (VisualElement visualElement = previousTopElementUnderMouse; visualElement != null; visualElement = visualElement.shadow.parent)
        ++num;
      int capacity = 0;
      for (VisualElement visualElement = currentTopElementUnderMouse; visualElement != null; visualElement = visualElement.shadow.parent)
        ++capacity;
      VisualElement visualElement1 = previousTopElementUnderMouse;
      VisualElement visualElement2 = currentTopElementUnderMouse;
      while (num > capacity)
      {
        MouseLeaveEvent pooled = MouseEventBase<MouseLeaveEvent>.GetPooled(triggerEvent);
        pooled.target = (IEventHandler) visualElement1;
        this.DispatchEvent((EventBase) pooled, visualElement1.panel);
        EventBase<MouseLeaveEvent>.ReleasePooled(pooled);
        --num;
        visualElement1 = visualElement1.shadow.parent;
      }
      List<VisualElement> visualElementList = new List<VisualElement>(capacity);
      while (capacity > num)
      {
        visualElementList.Add(visualElement2);
        --capacity;
        visualElement2 = visualElement2.shadow.parent;
      }
      for (; visualElement1 != visualElement2; visualElement2 = visualElement2.shadow.parent)
      {
        MouseLeaveEvent pooled = MouseEventBase<MouseLeaveEvent>.GetPooled(triggerEvent);
        pooled.target = (IEventHandler) visualElement1;
        this.DispatchEvent((EventBase) pooled, visualElement1.panel);
        EventBase<MouseLeaveEvent>.ReleasePooled(pooled);
        visualElementList.Add(visualElement2);
        visualElement1 = visualElement1.shadow.parent;
      }
      for (int index = visualElementList.Count - 1; index >= 0; --index)
      {
        MouseEnterEvent pooled = MouseEventBase<MouseEnterEvent>.GetPooled(triggerEvent);
        pooled.target = (IEventHandler) visualElementList[index];
        this.DispatchEvent((EventBase) pooled, visualElementList[index].panel);
        EventBase<MouseEnterEvent>.ReleasePooled(pooled);
      }
    }

    private void DispatchMouseOverMouseOut(VisualElement previousTopElementUnderMouse, VisualElement currentTopElementUnderMouse, IMouseEvent triggerEvent)
    {
      if (previousTopElementUnderMouse == currentTopElementUnderMouse)
        return;
      if (previousTopElementUnderMouse != null)
      {
        MouseOutEvent pooled = MouseEventBase<MouseOutEvent>.GetPooled(triggerEvent);
        pooled.target = (IEventHandler) previousTopElementUnderMouse;
        this.DispatchEvent((EventBase) pooled, previousTopElementUnderMouse.panel);
        EventBase<MouseOutEvent>.ReleasePooled(pooled);
      }
      if (currentTopElementUnderMouse == null)
        return;
      MouseOverEvent pooled1 = MouseEventBase<MouseOverEvent>.GetPooled(triggerEvent);
      pooled1.target = (IEventHandler) currentTopElementUnderMouse;
      this.DispatchEvent((EventBase) pooled1, currentTopElementUnderMouse.panel);
      EventBase<MouseOverEvent>.ReleasePooled(pooled1);
    }

    public void DispatchEvent(EventBase evt, IPanel panel)
    {
      Event imguiEvent = evt.imguiEvent;
      if (imguiEvent != null && imguiEvent.type == EventType.Repaint)
        return;
      bool flag = false;
      VisualElement capture = this.capture as VisualElement;
      if (panel != null && panel.panelDebug != null && (panel.panelDebug.enabled && panel.panelDebug.interceptEvents != null) && panel.panelDebug.interceptEvents(imguiEvent))
      {
        evt.StopPropagation();
      }
      else
      {
        if (capture != null && capture.panel == null)
        {
          Debug.Log((object) string.Format("Capture has no panel, forcing removal (capture={0} eventType={1})", (object) this.capture, imguiEvent == null ? (object) "null" : (object) imguiEvent.type.ToString()));
          this.RemoveCapture();
          capture = (VisualElement) null;
        }
        if ((evt is IMouseEvent || imguiEvent != null) && this.capture != null)
        {
          if (panel != null && capture != null && capture.panel.contextType != panel.contextType)
            return;
          flag = true;
          evt.dispatch = true;
          evt.target = this.capture;
          evt.currentTarget = this.capture;
          evt.propagationPhase = PropagationPhase.AtTarget;
          this.capture.HandleEvent(evt);
          evt.propagationPhase = PropagationPhase.None;
          evt.currentTarget = (IEventHandler) null;
          evt.dispatch = false;
        }
        if (!evt.isPropagationStopped)
        {
          if (evt is IKeyboardEvent)
          {
            if (panel.focusController.focusedElement != null)
            {
              IMGUIContainer focusedElement = panel.focusController.focusedElement as IMGUIContainer;
              flag = true;
              if (focusedElement != null)
              {
                if (focusedElement.HandleIMGUIEvent(evt.imguiEvent))
                {
                  evt.StopPropagation();
                  evt.PreventDefault();
                }
              }
              else
              {
                evt.target = (IEventHandler) panel.focusController.focusedElement;
                EventDispatcher.PropagateEvent(evt);
              }
            }
            else
            {
              evt.target = (IEventHandler) panel.visualTree;
              EventDispatcher.PropagateEvent(evt);
              flag = false;
            }
          }
          else if (evt.GetEventTypeId() == EventBase<MouseEnterEvent>.TypeId() || evt.GetEventTypeId() == EventBase<MouseLeaveEvent>.TypeId())
          {
            Debug.Assert(evt.target != null);
            flag = true;
            EventDispatcher.PropagateEvent(evt);
          }
          else if (evt is IMouseEvent || imguiEvent != null && (imguiEvent.type == EventType.ContextClick || imguiEvent.type == EventType.MouseEnterWindow || (imguiEvent.type == EventType.MouseLeaveWindow || imguiEvent.type == EventType.DragUpdated) || (imguiEvent.type == EventType.DragPerform || imguiEvent.type == EventType.DragExited)))
          {
            VisualElement elementUnderMouse = this.m_TopElementUnderMouse;
            if (imguiEvent != null && imguiEvent.type == EventType.MouseLeaveWindow)
            {
              this.m_TopElementUnderMouse = (VisualElement) null;
              this.DispatchMouseEnterMouseLeave(elementUnderMouse, this.m_TopElementUnderMouse, evt as IMouseEvent);
              this.DispatchMouseOverMouseOut(elementUnderMouse, this.m_TopElementUnderMouse, evt as IMouseEvent);
            }
            else if (evt is IMouseEvent || imguiEvent != null)
            {
              if (evt.target == null)
              {
                if (evt is IMouseEvent)
                  this.m_TopElementUnderMouse = panel.Pick((evt as IMouseEvent).localMousePosition);
                else if (imguiEvent != null)
                  this.m_TopElementUnderMouse = panel.Pick(imguiEvent.mousePosition);
                evt.target = (IEventHandler) this.m_TopElementUnderMouse;
              }
              if (evt.target != null)
              {
                flag = true;
                EventDispatcher.PropagateEvent(evt);
              }
              if (evt.GetEventTypeId() == EventBase<MouseMoveEvent>.TypeId())
              {
                this.DispatchMouseEnterMouseLeave(elementUnderMouse, this.m_TopElementUnderMouse, evt as IMouseEvent);
                this.DispatchMouseOverMouseOut(elementUnderMouse, this.m_TopElementUnderMouse, evt as IMouseEvent);
              }
            }
          }
          else if (imguiEvent != null && (imguiEvent.type == EventType.ExecuteCommand || imguiEvent.type == EventType.ValidateCommand))
          {
            IMGUIContainer focusedElement = panel.focusController.focusedElement as IMGUIContainer;
            if (focusedElement != null)
            {
              flag = true;
              if (focusedElement.HandleIMGUIEvent(evt.imguiEvent))
              {
                evt.StopPropagation();
                evt.PreventDefault();
              }
            }
            else if (panel.focusController.focusedElement != null)
            {
              flag = true;
              evt.target = (IEventHandler) panel.focusController.focusedElement;
              EventDispatcher.PropagateEvent(evt);
            }
          }
          else if (evt is IPropagatableEvent)
          {
            Debug.Assert(evt.target != null);
            flag = true;
            EventDispatcher.PropagateEvent(evt);
          }
        }
        if (!evt.isPropagationStopped && imguiEvent != null && (!flag || imguiEvent != null && (imguiEvent.type == EventType.MouseEnterWindow || imguiEvent.type == EventType.MouseLeaveWindow || imguiEvent.type == EventType.Used)))
          EventDispatcher.PropagateToIMGUIContainer(panel.visualTree, evt, capture);
        if (evt.target == null)
          evt.target = (IEventHandler) panel.visualTree;
        EventDispatcher.ExecuteDefaultAction(evt);
      }
    }

    private static void PropagateToIMGUIContainer(VisualElement root, EventBase evt, VisualElement capture)
    {
      IMGUIContainer imguiContainer = root as IMGUIContainer;
      if (imguiContainer != null && (evt.imguiEvent.type == EventType.Used || root != capture))
      {
        if (!imguiContainer.HandleIMGUIEvent(evt.imguiEvent))
          return;
        evt.StopPropagation();
        evt.PreventDefault();
      }
      else if (root != null)
      {
        for (int index = 0; index < root.shadow.childCount; ++index)
        {
          EventDispatcher.PropagateToIMGUIContainer(root.shadow[index], evt, capture);
          if (evt.isPropagationStopped)
            break;
        }
      }
    }

    private static void PropagateEvent(EventBase evt)
    {
      if (evt.dispatch)
        return;
      EventDispatcher.PropagationPaths propagationPaths = EventDispatcher.BuildPropagationPath(evt.target as VisualElement);
      evt.dispatch = true;
      if (evt.capturable && propagationPaths.capturePath.Count > 0)
      {
        evt.propagationPhase = PropagationPhase.Capture;
        for (int index = propagationPaths.capturePath.Count - 1; index >= 0 && !evt.isPropagationStopped; --index)
        {
          evt.currentTarget = (IEventHandler) propagationPaths.capturePath[index];
          evt.currentTarget.HandleEvent(evt);
        }
      }
      if (!evt.isPropagationStopped)
      {
        evt.propagationPhase = PropagationPhase.AtTarget;
        evt.currentTarget = evt.target;
        evt.currentTarget.HandleEvent(evt);
      }
      if (evt.bubbles && propagationPaths.bubblePath.Count > 0)
      {
        evt.propagationPhase = PropagationPhase.BubbleUp;
        for (int index = 0; index < propagationPaths.bubblePath.Count && !evt.isPropagationStopped; ++index)
        {
          evt.currentTarget = (IEventHandler) propagationPaths.bubblePath[index];
          evt.currentTarget.HandleEvent(evt);
        }
      }
      evt.dispatch = false;
      evt.propagationPhase = PropagationPhase.None;
      evt.currentTarget = (IEventHandler) null;
    }

    private static void ExecuteDefaultAction(EventBase evt)
    {
      if (evt.isDefaultPrevented || evt.target == null)
        return;
      evt.dispatch = true;
      evt.currentTarget = evt.target;
      evt.propagationPhase = PropagationPhase.DefaultAction;
      evt.currentTarget.HandleEvent(evt);
      evt.propagationPhase = PropagationPhase.None;
      evt.currentTarget = (IEventHandler) null;
      evt.dispatch = false;
    }

    private static EventDispatcher.PropagationPaths BuildPropagationPath(VisualElement elem)
    {
      EventDispatcher.PropagationPaths propagationPaths = new EventDispatcher.PropagationPaths(16);
      if (elem == null)
        return propagationPaths;
      for (; elem.shadow.parent != null; elem = elem.shadow.parent)
      {
        if (elem.shadow.parent.enabledInHierarchy)
        {
          if (elem.shadow.parent.HasCaptureHandlers())
            propagationPaths.capturePath.Add(elem.shadow.parent);
          if (elem.shadow.parent.HasBubbleHandlers())
            propagationPaths.bubblePath.Add(elem.shadow.parent);
        }
      }
      return propagationPaths;
    }

    private struct PropagationPaths
    {
      public List<VisualElement> capturePath;
      public List<VisualElement> bubblePath;

      public PropagationPaths(int initialSize)
      {
        this.capturePath = new List<VisualElement>(initialSize);
        this.bubblePath = new List<VisualElement>(initialSize);
      }
    }
  }
}
