// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.IMGUIContainer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  public class IMGUIContainer : VisualElement
  {
    private GUILayoutUtility.LayoutCache m_Cache = (GUILayoutUtility.LayoutCache) null;
    private bool lostFocus = false;
    private bool receivedFocus = false;
    private FocusChangeDirection focusChangeDirection = FocusChangeDirection.unspecified;
    private bool hasFocusableControls = false;
    private readonly Action m_OnGUIHandler;
    private ObjectGUIState m_ObjectGUIState;
    internal bool useOwnerObjectGUIState;
    private IMGUIContainer.GUIGlobals m_GUIGlobals;

    public IMGUIContainer(Action onGUIHandler)
    {
      this.m_OnGUIHandler = onGUIHandler;
      this.contextType = ContextType.Editor;
      this.focusIndex = 0;
    }

    internal ObjectGUIState guiState
    {
      get
      {
        Debug.Assert(!this.useOwnerObjectGUIState);
        if (this.m_ObjectGUIState == null)
          this.m_ObjectGUIState = new ObjectGUIState();
        return this.m_ObjectGUIState;
      }
    }

    internal Rect lastWorldClip { get; set; }

    private GUILayoutUtility.LayoutCache cache
    {
      get
      {
        if (this.m_Cache == null)
          this.m_Cache = new GUILayoutUtility.LayoutCache();
        return this.m_Cache;
      }
    }

    public ContextType contextType { get; set; }

    internal int GUIDepth { get; private set; }

    public override bool canGrabFocus
    {
      get
      {
        return base.canGrabFocus && this.hasFocusableControls;
      }
    }

    internal override void DoRepaint(IStylePainter painter)
    {
      this.DoRepaint();
      this.lastWorldClip = painter.currentWorldClip;
      this.HandleIMGUIEvent(painter.repaintEvent);
    }

    internal override void ChangePanel(BaseVisualElementPanel p)
    {
      if (this.elementPanel != null)
        --this.elementPanel.IMGUIContainersCount;
      base.ChangePanel(p);
      if (this.elementPanel == null)
        return;
      ++this.elementPanel.IMGUIContainersCount;
    }

    private void SaveGlobals()
    {
      this.m_GUIGlobals.matrix = GUI.matrix;
      this.m_GUIGlobals.color = GUI.color;
      this.m_GUIGlobals.contentColor = GUI.contentColor;
      this.m_GUIGlobals.backgroundColor = GUI.backgroundColor;
      this.m_GUIGlobals.enabled = GUI.enabled;
      this.m_GUIGlobals.changed = GUI.changed;
      this.m_GUIGlobals.displayIndex = Event.current.displayIndex;
    }

    private void RestoreGlobals()
    {
      GUI.matrix = this.m_GUIGlobals.matrix;
      GUI.color = this.m_GUIGlobals.color;
      GUI.contentColor = this.m_GUIGlobals.contentColor;
      GUI.backgroundColor = this.m_GUIGlobals.backgroundColor;
      GUI.enabled = this.m_GUIGlobals.enabled;
      GUI.changed = this.m_GUIGlobals.changed;
      Event.current.displayIndex = this.m_GUIGlobals.displayIndex;
    }

    private void DoOnGUI(Event evt)
    {
      if (this.m_OnGUIHandler == null || this.panel == null)
        return;
      int count1 = GUIClip.Internal_GetCount();
      this.SaveGlobals();
      UIElementsUtility.BeginContainerGUI(this.cache, evt, this);
      if (this.lostFocus)
      {
        GUIUtility.keyboardControl = 0;
        if (this.focusController != null)
          this.focusController.imguiKeyboardControl = 0;
        this.lostFocus = false;
      }
      if (this.receivedFocus)
      {
        if (this.focusChangeDirection != FocusChangeDirection.unspecified && this.focusChangeDirection != FocusChangeDirection.none)
        {
          if (this.focusChangeDirection == VisualElementFocusChangeDirection.left)
            GUIUtility.SetKeyboardControlToLastControlId();
          else if (this.focusChangeDirection == VisualElementFocusChangeDirection.right)
            GUIUtility.SetKeyboardControlToFirstControlId();
        }
        this.receivedFocus = false;
        this.focusChangeDirection = FocusChangeDirection.unspecified;
        if (this.focusController != null)
          this.focusController.imguiKeyboardControl = GUIUtility.keyboardControl;
      }
      this.GUIDepth = GUIUtility.Internal_GetGUIDepth();
      EventType type1 = Event.current.type;
      bool flag = false;
      try
      {
        this.m_OnGUIHandler();
      }
      catch (Exception ex)
      {
        if (type1 == EventType.Layout)
        {
          flag = GUIUtility.IsExitGUIException(ex);
          if (!flag)
            Debug.LogException(ex);
        }
        else
          throw;
      }
      finally
      {
        int num = GUIUtility.CheckForTabEvent(evt);
        if (this.focusController != null)
        {
          if (num < 0)
          {
            KeyDownEvent evt1 = (KeyDownEvent) null;
            switch (num)
            {
              case -2:
                evt1 = KeyboardEventBase<KeyDownEvent>.GetPooled('\t', KeyCode.Tab, EventModifiers.Shift);
                break;
              case -1:
                evt1 = KeyboardEventBase<KeyDownEvent>.GetPooled('\t', KeyCode.Tab, EventModifiers.None);
                break;
            }
            Focusable focusedElement = this.focusController.focusedElement;
            this.focusController.SwitchFocusOnEvent((EventBase) evt1);
            EventBase<KeyDownEvent>.ReleasePooled(evt1);
            if (focusedElement == this)
            {
              if (this.focusController.focusedElement == this)
              {
                switch (num)
                {
                  case -2:
                    GUIUtility.SetKeyboardControlToLastControlId();
                    break;
                  case -1:
                    GUIUtility.SetKeyboardControlToFirstControlId();
                    break;
                }
                this.focusController.imguiKeyboardControl = GUIUtility.keyboardControl;
              }
              else
              {
                GUIUtility.keyboardControl = 0;
                this.focusController.imguiKeyboardControl = 0;
              }
            }
          }
          else if (num > 0)
            this.focusController.imguiKeyboardControl = GUIUtility.keyboardControl;
          else if (num == 0 && type1 == EventType.MouseDown)
            this.focusController.SyncIMGUIFocus(this);
        }
        this.hasFocusableControls = GUIUtility.HasFocusableControls();
      }
      EventType type2 = Event.current.type;
      UIElementsUtility.EndContainerGUI();
      this.RestoreGlobals();
      if (!flag && type2 != EventType.Ignore && type2 != EventType.Used)
      {
        int count2 = GUIClip.Internal_GetCount();
        if (count2 > count1)
          Debug.LogError((object) "GUI Error: You are pushing more GUIClips than you are popping. Make sure they are balanced)");
        else if (count2 < count1)
          Debug.LogError((object) "GUI Error: You are popping more GUIClips than you are pushing. Make sure they are balanced)");
      }
      while (GUIClip.Internal_GetCount() > count1)
        GUIClip.Internal_Pop();
      if (type2 != EventType.Used)
        return;
      this.Dirty(ChangeType.Repaint);
    }

    public override void HandleEvent(EventBase evt)
    {
      base.HandleEvent(evt);
      if (evt.propagationPhase == PropagationPhase.DefaultAction || evt.imguiEvent == null || (evt.isPropagationStopped || this.m_OnGUIHandler == null) || this.elementPanel == null || (!this.elementPanel.IMGUIEventInterests.WantsEvent(evt.imguiEvent.type) || !this.HandleIMGUIEvent(evt.imguiEvent)))
        return;
      evt.StopPropagation();
      evt.PreventDefault();
    }

    internal bool HandleIMGUIEvent(Event e)
    {
      EventType type = e.type;
      e.type = EventType.Layout;
      this.DoOnGUI(e);
      e.type = type;
      this.DoOnGUI(e);
      if (e.type == EventType.Used)
        return true;
      if (e.type == EventType.MouseUp && this.HasCapture())
        GUIUtility.hotControl = 0;
      if (this.elementPanel == null)
        GUIUtility.ExitGUI();
      return false;
    }

    protected internal override void ExecuteDefaultAction(EventBase evt)
    {
      if (evt.GetEventTypeId() == EventBase<BlurEvent>.TypeId())
      {
        BlurEvent blurEvent = evt as BlurEvent;
        if (blurEvent.relatedTarget != null && blurEvent.relatedTarget.canGrabFocus)
          return;
        this.lostFocus = true;
      }
      else
      {
        if (evt.GetEventTypeId() != EventBase<FocusEvent>.TypeId())
          return;
        FocusEvent focusEvent = evt as FocusEvent;
        this.receivedFocus = true;
        this.focusChangeDirection = focusEvent.direction;
      }
    }

    protected internal override Vector2 DoMeasure(float desiredWidth, VisualElement.MeasureMode widthMode, float desiredHeight, VisualElement.MeasureMode heightMode)
    {
      float num1 = float.NaN;
      float num2 = float.NaN;
      if (widthMode != VisualElement.MeasureMode.Exactly || heightMode != VisualElement.MeasureMode.Exactly)
      {
        this.DoOnGUI(new Event() { type = EventType.Layout });
        num1 = this.m_Cache.topLevel.minWidth;
        num2 = this.m_Cache.topLevel.minHeight;
      }
      switch (widthMode)
      {
        case VisualElement.MeasureMode.Exactly:
          num1 = desiredWidth;
          break;
        case VisualElement.MeasureMode.AtMost:
          num1 = Mathf.Min(num1, desiredWidth);
          break;
      }
      switch (heightMode)
      {
        case VisualElement.MeasureMode.Exactly:
          num2 = desiredHeight;
          break;
        case VisualElement.MeasureMode.AtMost:
          num2 = Mathf.Min(num2, desiredHeight);
          break;
      }
      return new Vector2(num1, num2);
    }

    private struct GUIGlobals
    {
      public Matrix4x4 matrix;
      public Color color;
      public Color contentColor;
      public Color backgroundColor;
      public bool enabled;
      public bool changed;
      public int displayIndex;
    }
  }
}
