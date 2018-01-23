// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.UIElementsUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
  internal class UIElementsUtility
  {
    private static Stack<IMGUIContainer> s_ContainerStack = new Stack<IMGUIContainer>();
    private static Dictionary<int, Panel> s_UIElementsCache = new Dictionary<int, Panel>();
    private static Event s_EventInstance = new Event();
    private static EventDispatcher s_EventDispatcher;
    internal static Action<IMGUIContainer> s_BeginContainerCallback;
    internal static Action<IMGUIContainer> s_EndContainerCallback;

    static UIElementsUtility()
    {
      Action takeCapture = GUIUtility.takeCapture;
      // ISSUE: reference to a compiler-generated field
      if (UIElementsUtility.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        UIElementsUtility.\u003C\u003Ef__mg\u0024cache0 = new Action(UIElementsUtility.TakeCapture);
      }
      // ISSUE: reference to a compiler-generated field
      Action fMgCache0 = UIElementsUtility.\u003C\u003Ef__mg\u0024cache0;
      GUIUtility.takeCapture = takeCapture + fMgCache0;
      Action releaseCapture = GUIUtility.releaseCapture;
      // ISSUE: reference to a compiler-generated field
      if (UIElementsUtility.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        UIElementsUtility.\u003C\u003Ef__mg\u0024cache1 = new Action(UIElementsUtility.ReleaseCapture);
      }
      // ISSUE: reference to a compiler-generated field
      Action fMgCache1 = UIElementsUtility.\u003C\u003Ef__mg\u0024cache1;
      GUIUtility.releaseCapture = releaseCapture + fMgCache1;
      Func<int, IntPtr, bool> processEvent = GUIUtility.processEvent;
      // ISSUE: reference to a compiler-generated field
      if (UIElementsUtility.\u003C\u003Ef__mg\u0024cache2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        UIElementsUtility.\u003C\u003Ef__mg\u0024cache2 = new Func<int, IntPtr, bool>(UIElementsUtility.ProcessEvent);
      }
      // ISSUE: reference to a compiler-generated field
      Func<int, IntPtr, bool> fMgCache2 = UIElementsUtility.\u003C\u003Ef__mg\u0024cache2;
      GUIUtility.processEvent = processEvent + fMgCache2;
      Action cleanupRoots = GUIUtility.cleanupRoots;
      // ISSUE: reference to a compiler-generated field
      if (UIElementsUtility.\u003C\u003Ef__mg\u0024cache3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        UIElementsUtility.\u003C\u003Ef__mg\u0024cache3 = new Action(UIElementsUtility.CleanupRoots);
      }
      // ISSUE: reference to a compiler-generated field
      Action fMgCache3 = UIElementsUtility.\u003C\u003Ef__mg\u0024cache3;
      GUIUtility.cleanupRoots = cleanupRoots + fMgCache3;
      Func<Exception, bool> guiFromException = GUIUtility.endContainerGUIFromException;
      // ISSUE: reference to a compiler-generated field
      if (UIElementsUtility.\u003C\u003Ef__mg\u0024cache4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        UIElementsUtility.\u003C\u003Ef__mg\u0024cache4 = new Func<Exception, bool>(UIElementsUtility.EndContainerGUIFromException);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Exception, bool> fMgCache4 = UIElementsUtility.\u003C\u003Ef__mg\u0024cache4;
      GUIUtility.endContainerGUIFromException = guiFromException + fMgCache4;
    }

    internal static IEventDispatcher eventDispatcher
    {
      get
      {
        if (UIElementsUtility.s_EventDispatcher == null)
          UIElementsUtility.s_EventDispatcher = new EventDispatcher();
        return (IEventDispatcher) UIElementsUtility.s_EventDispatcher;
      }
    }

    internal static void ClearDispatcher()
    {
      UIElementsUtility.s_EventDispatcher = (EventDispatcher) null;
    }

    private static void TakeCapture()
    {
      if (UIElementsUtility.s_ContainerStack.Count <= 0)
        return;
      IMGUIContainer imguiContainer = UIElementsUtility.s_ContainerStack.Peek();
      if (imguiContainer.GUIDepth != GUIUtility.Internal_GetGUIDepth())
        return;
      if (UIElementsUtility.eventDispatcher.capture != null && UIElementsUtility.eventDispatcher.capture != imguiContainer)
        Debug.Log((object) string.Format("Should not grab hot control with an active capture (current={0} new={1}", (object) UIElementsUtility.eventDispatcher.capture, (object) imguiContainer));
      UIElementsUtility.eventDispatcher.TakeCapture((IEventHandler) imguiContainer);
    }

    private static void ReleaseCapture()
    {
      UIElementsUtility.eventDispatcher.RemoveCapture();
    }

    private static bool ProcessEvent(int instanceID, IntPtr nativeEventPtr)
    {
      Panel panel;
      if (!(nativeEventPtr != IntPtr.Zero) || !UIElementsUtility.s_UIElementsCache.TryGetValue(instanceID, out panel))
        return false;
      UIElementsUtility.s_EventInstance.CopyFromPtr(nativeEventPtr);
      return UIElementsUtility.DoDispatch((BaseVisualElementPanel) panel);
    }

    private static void CleanupRoots()
    {
      UIElementsUtility.s_EventInstance = (Event) null;
      UIElementsUtility.s_EventDispatcher = (EventDispatcher) null;
      UIElementsUtility.s_UIElementsCache = (Dictionary<int, Panel>) null;
      UIElementsUtility.s_ContainerStack = (Stack<IMGUIContainer>) null;
      UIElementsUtility.s_BeginContainerCallback = (Action<IMGUIContainer>) null;
      UIElementsUtility.s_EndContainerCallback = (Action<IMGUIContainer>) null;
    }

    private static bool EndContainerGUIFromException(Exception exception)
    {
      if (UIElementsUtility.s_ContainerStack.Count > 0)
      {
        GUIUtility.EndContainer();
        UIElementsUtility.s_ContainerStack.Pop();
      }
      return GUIUtility.ShouldRethrowException(exception);
    }

    internal static void BeginContainerGUI(GUILayoutUtility.LayoutCache cache, Event evt, IMGUIContainer container)
    {
      if (container.useOwnerObjectGUIState)
        GUIUtility.BeginContainerFromOwner(container.elementPanel.ownerObject);
      else
        GUIUtility.BeginContainer(container.guiState);
      UIElementsUtility.s_ContainerStack.Push(container);
      GUIUtility.s_SkinMode = (int) container.contextType;
      GUIUtility.s_OriginalID = container.elementPanel.ownerObject.GetInstanceID();
      Event.current = evt;
      if (UIElementsUtility.s_BeginContainerCallback != null)
        UIElementsUtility.s_BeginContainerCallback(container);
      GUI.enabled = container.enabledInHierarchy;
      GUILayoutUtility.BeginContainer(cache);
      GUIUtility.ResetGlobalState();
      Rect clipRect = container.lastWorldClip;
      if ((double) clipRect.width == 0.0 || (double) clipRect.height == 0.0)
        clipRect = container.worldBound;
      Matrix4x4 matrix4x4 = container.worldTransform;
      if (evt.type == EventType.Repaint && container.elementPanel != null && container.elementPanel.stylePainter != null)
        matrix4x4 = container.elementPanel.stylePainter.currentTransform;
      GUIClip.SetTransform(matrix4x4 * Matrix4x4.Translate((Vector3) container.layout.position), clipRect);
    }

    internal static void EndContainerGUI()
    {
      if (Event.current.type == EventType.Layout && UIElementsUtility.s_ContainerStack.Count > 0)
      {
        Rect layout = UIElementsUtility.s_ContainerStack.Peek().layout;
        GUILayoutUtility.LayoutFromContainer(layout.width, layout.height);
      }
      GUILayoutUtility.SelectIDList(GUIUtility.s_OriginalID, false);
      GUIContent.ClearStaticCache();
      if (UIElementsUtility.s_ContainerStack.Count <= 0)
        return;
      IMGUIContainer imguiContainer = UIElementsUtility.s_ContainerStack.Peek();
      if (UIElementsUtility.s_EndContainerCallback != null)
        UIElementsUtility.s_EndContainerCallback(imguiContainer);
      GUIUtility.EndContainer();
      UIElementsUtility.s_ContainerStack.Pop();
    }

    internal static ContextType GetGUIContextType()
    {
      return GUIUtility.s_SkinMode != 0 ? ContextType.Editor : ContextType.Player;
    }

    internal static EventBase CreateEvent(Event systemEvent)
    {
      switch (systemEvent.type)
      {
        case EventType.MouseDown:
          return (EventBase) MouseEventBase<MouseDownEvent>.GetPooled(systemEvent);
        case EventType.MouseUp:
          return (EventBase) MouseEventBase<MouseUpEvent>.GetPooled(systemEvent);
        case EventType.MouseMove:
          return (EventBase) MouseEventBase<MouseMoveEvent>.GetPooled(systemEvent);
        case EventType.MouseDrag:
          return (EventBase) MouseEventBase<MouseMoveEvent>.GetPooled(systemEvent);
        case EventType.KeyDown:
          return (EventBase) KeyboardEventBase<KeyDownEvent>.GetPooled(systemEvent);
        case EventType.KeyUp:
          return (EventBase) KeyboardEventBase<KeyUpEvent>.GetPooled(systemEvent);
        case EventType.ScrollWheel:
          return (EventBase) WheelEvent.GetPooled(systemEvent);
        default:
          return (EventBase) IMGUIEvent.GetPooled(systemEvent);
      }
    }

    internal static void ReleaseEvent(EventBase evt)
    {
      long eventTypeId = evt.GetEventTypeId();
      if (eventTypeId == EventBase<MouseMoveEvent>.TypeId())
        EventBase<MouseMoveEvent>.ReleasePooled((MouseMoveEvent) evt);
      else if (eventTypeId == EventBase<MouseDownEvent>.TypeId())
        EventBase<MouseDownEvent>.ReleasePooled((MouseDownEvent) evt);
      else if (eventTypeId == EventBase<MouseUpEvent>.TypeId())
        EventBase<MouseUpEvent>.ReleasePooled((MouseUpEvent) evt);
      else if (eventTypeId == EventBase<WheelEvent>.TypeId())
        EventBase<WheelEvent>.ReleasePooled((WheelEvent) evt);
      else if (eventTypeId == EventBase<KeyDownEvent>.TypeId())
        EventBase<KeyDownEvent>.ReleasePooled((KeyDownEvent) evt);
      else if (eventTypeId == EventBase<KeyUpEvent>.TypeId())
      {
        EventBase<KeyUpEvent>.ReleasePooled((KeyUpEvent) evt);
      }
      else
      {
        if (eventTypeId != EventBase<IMGUIEvent>.TypeId())
          return;
        EventBase<IMGUIEvent>.ReleasePooled((IMGUIEvent) evt);
      }
    }

    private static bool DoDispatch(BaseVisualElementPanel panel)
    {
      bool flag;
      if (UIElementsUtility.s_EventInstance.type == EventType.Repaint)
      {
        panel.Repaint(UIElementsUtility.s_EventInstance);
        flag = panel.IMGUIContainersCount > 0;
      }
      else
      {
        panel.ValidateLayout();
        EventBase evt = UIElementsUtility.CreateEvent(UIElementsUtility.s_EventInstance);
        Vector2 mousePosition = UIElementsUtility.s_EventInstance.mousePosition;
        UIElementsUtility.s_EventDispatcher.DispatchEvent(evt, (IPanel) panel);
        UIElementsUtility.s_EventInstance.mousePosition = mousePosition;
        if (evt.isPropagationStopped)
          panel.visualTree.Dirty(ChangeType.Repaint);
        flag = evt.isPropagationStopped;
        UIElementsUtility.ReleaseEvent(evt);
      }
      return flag;
    }

    internal static Dictionary<int, Panel>.Enumerator GetPanelsIterator()
    {
      return UIElementsUtility.s_UIElementsCache.GetEnumerator();
    }

    internal static Panel FindOrCreatePanel(ScriptableObject ownerObject, ContextType contextType, IDataWatchService dataWatch = null)
    {
      Panel panel;
      if (!UIElementsUtility.s_UIElementsCache.TryGetValue(ownerObject.GetInstanceID(), out panel))
      {
        panel = new Panel(ownerObject, contextType, dataWatch, UIElementsUtility.eventDispatcher);
        UIElementsUtility.s_UIElementsCache.Add(ownerObject.GetInstanceID(), panel);
      }
      else
        Debug.Assert(contextType == panel.contextType, "Context type mismatch");
      return panel;
    }

    internal static Panel FindOrCreatePanel(ScriptableObject ownerObject)
    {
      return UIElementsUtility.FindOrCreatePanel(ownerObject, UIElementsUtility.GetGUIContextType(), (IDataWatchService) null);
    }
  }
}
