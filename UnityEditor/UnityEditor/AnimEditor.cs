// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AnimEditor : ScriptableObject
  {
    private static List<AnimEditor> s_AnimationWindows = new List<AnimEditor>();
    internal static PrefColor kEulerXColor = new PrefColor("Animation/EulerX", 1f, 0.0f, 1f, 1f);
    internal static PrefColor kEulerYColor = new PrefColor("Animation/EulerY", 1f, 1f, 0.0f, 1f);
    internal static PrefColor kEulerZColor = new PrefColor("Animation/EulerZ", 0.0f, 1f, 1f, 1f);
    private static Color s_SelectionRangeColorLight = (Color) new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 90);
    private static Color s_SelectionRangeColorDark = (Color) new Color32((byte) 200, (byte) 200, (byte) 200, (byte) 40);
    private static Color s_OutOfRangeColorLight = (Color) new Color32((byte) 160, (byte) 160, (byte) 160, (byte) 127);
    private static Color s_OutOfRangeColorDark = (Color) new Color32((byte) 40, (byte) 40, (byte) 40, (byte) 127);
    private static Color s_InRangeColorLight = (Color) new Color32((byte) 211, (byte) 211, (byte) 211, byte.MaxValue);
    private static Color s_InRangeColorDark = (Color) new Color32((byte) 75, (byte) 75, (byte) 75, byte.MaxValue);
    internal static PrefKey kAnimationPlayToggle = new PrefKey("Animation/Play Animation", " ");
    internal static PrefKey kAnimationPrevFrame = new PrefKey("Animation/Previous Frame", ",");
    internal static PrefKey kAnimationNextFrame = new PrefKey("Animation/Next Frame", ".");
    internal static PrefKey kAnimationPrevKeyframe = new PrefKey("Animation/Previous Keyframe", "&,");
    internal static PrefKey kAnimationNextKeyframe = new PrefKey("Animation/Next Keyframe", "&.");
    internal static PrefKey kAnimationFirstKey = new PrefKey("Animation/First Keyframe", "#,");
    internal static PrefKey kAnimationLastKey = new PrefKey("Animation/Last Keyframe", "#.");
    internal static PrefKey kAnimationRecordKeyframeSelected = new PrefKey("Animation/Key Selected", "k");
    internal static PrefKey kAnimationRecordKeyframeModified = new PrefKey("Animation/Key Modified", "#k");
    internal static PrefKey kAnimationShowCurvesToggle = new PrefKey("Animation/Show Curves", "c");
    [SerializeField]
    private SplitterState m_HorizontalSplitter;
    [SerializeField]
    private AnimationWindowState m_State;
    [SerializeField]
    private DopeSheetEditor m_DopeSheet;
    [SerializeField]
    private AnimationWindowHierarchy m_Hierarchy;
    [SerializeField]
    private AnimationWindowClipPopup m_ClipPopup;
    [SerializeField]
    private AnimationEventTimeLine m_Events;
    [SerializeField]
    private CurveEditor m_CurveEditor;
    [SerializeField]
    private AnimEditorOverlay m_Overlay;
    [SerializeField]
    private EditorWindow m_OwnerWindow;
    [NonSerialized]
    private Rect m_Position;
    [NonSerialized]
    private bool m_TriggerFraming;
    [NonSerialized]
    private bool m_Initialized;
    internal const int kSliderThickness = 15;
    internal const int kLayoutRowHeight = 18;
    internal const int kIntFieldWidth = 35;
    internal const int kHierarchyMinWidth = 300;
    internal const int kToggleButtonWidth = 80;
    internal const float kDisabledRulerAlpha = 0.12f;

    public static List<AnimEditor> GetAllAnimationWindows()
    {
      return AnimEditor.s_AnimationWindows;
    }

    public bool stateDisabled
    {
      get
      {
        return this.m_State.disabled;
      }
    }

    private float hierarchyWidth
    {
      get
      {
        return (float) this.m_HorizontalSplitter.realSizes[0];
      }
    }

    private float contentWidth
    {
      get
      {
        return (float) this.m_HorizontalSplitter.realSizes[1];
      }
    }

    private static Color selectionRangeColor
    {
      get
      {
        return !EditorGUIUtility.isProSkin ? AnimEditor.s_SelectionRangeColorLight : AnimEditor.s_SelectionRangeColorDark;
      }
    }

    private static Color outOfRangeColor
    {
      get
      {
        return !EditorGUIUtility.isProSkin ? AnimEditor.s_OutOfRangeColorLight : AnimEditor.s_OutOfRangeColorDark;
      }
    }

    private static Color inRangeColor
    {
      get
      {
        return !EditorGUIUtility.isProSkin ? AnimEditor.s_InRangeColorLight : AnimEditor.s_InRangeColorDark;
      }
    }

    public AnimationWindowState state
    {
      get
      {
        return this.m_State;
      }
    }

    public AnimationWindowSelection selection
    {
      get
      {
        return this.m_State.selection;
      }
    }

    public AnimationWindowSelectionItem selectedItem
    {
      get
      {
        return this.m_State.selectedItem;
      }
      set
      {
        this.m_State.selectedItem = value;
      }
    }

    public IAnimationWindowControl controlInterface
    {
      get
      {
        return this.state.controlInterface;
      }
    }

    public IAnimationWindowControl overrideControlInterface
    {
      get
      {
        return this.state.overrideControlInterface;
      }
      set
      {
        this.state.overrideControlInterface = value;
      }
    }

    private bool triggerFraming
    {
      set
      {
        this.m_TriggerFraming = value;
      }
      get
      {
        return this.m_TriggerFraming;
      }
    }

    internal CurveEditor curveEditor
    {
      get
      {
        return this.m_CurveEditor;
      }
    }

    internal DopeSheetEditor dopeSheetEditor
    {
      get
      {
        return this.m_DopeSheet;
      }
    }

    public void OnAnimEditorGUI(EditorWindow parent, Rect position)
    {
      this.m_DopeSheet.m_Owner = parent;
      this.m_OwnerWindow = parent;
      this.m_Position = position;
      if (!this.m_Initialized)
        this.Initialize();
      this.m_State.OnGUI();
      if (this.m_State.disabled && this.controlInterface.recording)
        this.m_State.StopRecording();
      this.SynchronizeLayout();
      using (new EditorGUI.DisabledScope(this.m_State.disabled || this.m_State.animatorIsOptimized))
      {
        this.OverlayEventOnGUI();
        GUILayout.BeginHorizontal();
        SplitterGUILayout.BeginHorizontalSplit(this.m_HorizontalSplitter);
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal(EditorStyles.toolbarButton, new GUILayoutOption[0]);
        this.PlayControlsOnGUI();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(EditorStyles.toolbarButton, new GUILayoutOption[0]);
        this.LinkOptionsOnGUI();
        this.ClipSelectionDropDownOnGUI();
        GUILayout.FlexibleSpace();
        this.FrameRateInputFieldOnGUI();
        this.AddKeyframeButtonOnGUI();
        this.AddEventButtonOnGUI();
        GUILayout.EndHorizontal();
        this.HierarchyOnGUI();
        GUILayout.BeginHorizontal(AnimationWindowStyles.miniToolbar, new GUILayoutOption[0]);
        this.TabSelectionOnGUI();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        Rect rect1 = GUILayoutUtility.GetRect(this.contentWidth, 18f);
        Rect rect2 = GUILayoutUtility.GetRect(this.contentWidth, 18f);
        Rect rect3 = GUILayoutUtility.GetRect(this.contentWidth, this.contentWidth, 0.0f, float.MaxValue, new GUILayoutOption[1]{ GUILayout.ExpandHeight(true) });
        this.MainContentOnGUI(rect3);
        this.TimeRulerOnGUI(rect1);
        this.EventLineOnGUI(rect2);
        GUILayout.EndVertical();
        SplitterGUILayout.EndHorizontalSplit();
        GUILayout.EndHorizontal();
        this.OverlayOnGUI(rect3);
        this.RenderEventTooltip();
        this.HandleHotKeys();
      }
    }

    private void MainContentOnGUI(Rect contentLayoutRect)
    {
      if (this.m_State.animatorIsOptimized)
      {
        Vector2 vector2 = GUI.skin.label.CalcSize(AnimationWindowStyles.animatorOptimizedText);
        GUI.Label(new Rect((float) ((double) contentLayoutRect.x + (double) contentLayoutRect.width * 0.5 - (double) vector2.x * 0.5), (float) ((double) contentLayoutRect.y + (double) contentLayoutRect.height * 0.5 - (double) vector2.y * 0.5), vector2.x, vector2.y), AnimationWindowStyles.animatorOptimizedText);
      }
      else
      {
        if (this.m_State.disabled)
        {
          this.SetupWizardOnGUI(contentLayoutRect);
        }
        else
        {
          Event current = Event.current;
          if (current.type == EventType.MouseDown && contentLayoutRect.Contains(current.mousePosition))
            this.m_Events.ClearSelection();
          if (this.triggerFraming && current.type == EventType.Repaint)
          {
            this.m_DopeSheet.FrameClip();
            this.m_CurveEditor.FrameClip(true, true);
            this.triggerFraming = false;
          }
          if (this.m_State.showCurveEditor)
            this.CurveEditorOnGUI(contentLayoutRect);
          else
            this.DopeSheetOnGUI(contentLayoutRect);
        }
        this.HandleCopyPaste();
      }
    }

    private void OverlayEventOnGUI()
    {
      if (this.m_State.animatorIsOptimized || this.m_State.disabled)
        return;
      GUI.BeginGroup(new Rect(this.hierarchyWidth - 1f, 0.0f, this.contentWidth - 15f, this.m_Position.height - 15f));
      this.m_Overlay.HandleEvents();
      GUI.EndGroup();
    }

    private void OverlayOnGUI(Rect contentRect)
    {
      if (this.m_State.animatorIsOptimized || this.m_State.disabled || Event.current.type != EventType.Repaint)
        return;
      Rect rect1 = new Rect(contentRect.xMin, contentRect.yMin, contentRect.width - 15f, contentRect.height - 15f);
      Rect position = new Rect(this.hierarchyWidth - 1f, 0.0f, this.contentWidth - 15f, this.m_Position.height - 15f);
      GUI.BeginGroup(position);
      Rect rect2 = new Rect(0.0f, 0.0f, position.width, position.height);
      Rect contentRect1 = rect1;
      contentRect1.position -= position.min;
      this.m_Overlay.OnGUI(rect2, contentRect1);
      GUI.EndGroup();
    }

    public void Update()
    {
      if ((UnityEngine.Object) this.m_State == (UnityEngine.Object) null)
        return;
      this.PlaybackUpdate();
    }

    public void OnEnable()
    {
      this.hideFlags = HideFlags.HideAndDontSave;
      AnimEditor.s_AnimationWindows.Add(this);
      if ((UnityEngine.Object) this.m_State == (UnityEngine.Object) null)
      {
        this.m_State = ScriptableObject.CreateInstance(typeof (AnimationWindowState)) as AnimationWindowState;
        this.m_State.hideFlags = HideFlags.HideAndDontSave;
        this.m_State.animEditor = this;
        this.InitializeHorizontalSplitter();
        this.InitializeClipSelection();
        this.InitializeDopeSheet();
        this.InitializeEvents();
        this.InitializeCurveEditor();
        this.InitializeOverlay();
      }
      this.InitializeNonserializedValues();
      this.m_State.timeArea = !this.m_State.showCurveEditor ? (TimeArea) this.m_DopeSheet : (TimeArea) this.m_CurveEditor;
      this.m_DopeSheet.state = this.m_State;
      this.m_ClipPopup.state = this.m_State;
      this.m_Overlay.state = this.m_State;
      this.m_CurveEditor.curvesUpdated += new CurveEditor.CallbackFunction(this.SaveChangedCurvesFromCurveEditor);
      this.m_CurveEditor.OnEnable();
      EditorApplication.globalEventHandler += new EditorApplication.CallbackFunction(this.HandleGlobalHotkeys);
    }

    public void OnDisable()
    {
      AnimEditor.s_AnimationWindows.Remove(this);
      if (this.m_CurveEditor != null)
      {
        this.m_CurveEditor.curvesUpdated -= new CurveEditor.CallbackFunction(this.SaveChangedCurvesFromCurveEditor);
        this.m_CurveEditor.OnDisable();
      }
      if (this.m_DopeSheet != null)
        this.m_DopeSheet.OnDisable();
      this.m_State.OnDisable();
      EditorApplication.globalEventHandler -= new EditorApplication.CallbackFunction(this.HandleGlobalHotkeys);
    }

    public void OnDestroy()
    {
      if (this.m_CurveEditor != null)
        this.m_CurveEditor.OnDestroy();
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_State);
    }

    public void OnSelectionChanged()
    {
      this.m_State.OnSelectionChanged();
      this.triggerFraming = true;
      this.Repaint();
    }

    public void OnStartLiveEdit()
    {
      this.SaveCurveEditorKeySelection();
    }

    public void OnEndLiveEdit()
    {
      this.UpdateSelectedKeysToCurveEditor();
      this.controlInterface.ResampleAnimation();
    }

    public void OnLostFocus()
    {
      if (this.m_Hierarchy != null)
        this.m_Hierarchy.EndNameEditing(true);
      EditorGUI.EndEditingActiveTextField();
    }

    private void PlaybackUpdate()
    {
      if (this.m_State.disabled && this.controlInterface.playing)
        this.controlInterface.StopPlayback();
      if (!this.controlInterface.PlaybackUpdate())
        return;
      this.Repaint();
    }

    private void SetupWizardOnGUI(Rect position)
    {
      GUI.Label(position, GUIContent.none, AnimationWindowStyles.dopeSheetBackground);
      Rect position1 = new Rect(position.x, position.y, position.width - 15f, position.height - 15f);
      GUI.BeginClip(position1);
      GUI.enabled = true;
      this.m_State.showCurveEditor = false;
      this.m_State.timeArea = (TimeArea) this.m_DopeSheet;
      this.m_State.timeArea.SetShownHRangeInsideMargins(0.0f, 1f);
      if ((bool) ((UnityEngine.Object) this.m_State.activeGameObject) && !EditorUtility.IsPersistent((UnityEngine.Object) this.m_State.activeGameObject))
      {
        string str = (bool) ((UnityEngine.Object) this.m_State.activeRootGameObject) || (bool) ((UnityEngine.Object) this.m_State.activeAnimationClip) ? AnimationWindowStyles.animationClip.text : AnimationWindowStyles.animatorAndAnimationClip.text;
        GUIContent content = GUIContent.Temp(string.Format(AnimationWindowStyles.formatIsMissing.text, (object) this.m_State.activeGameObject.name, (object) str));
        Vector2 vector2 = GUI.skin.label.CalcSize(content);
        Rect position2 = new Rect((float) ((double) position1.width * 0.5 - (double) vector2.x * 0.5), (float) ((double) position1.height * 0.5 - (double) vector2.y * 0.5), vector2.x, vector2.y);
        GUI.Label(position2, content);
        if (GUI.Button(new Rect((float) ((double) position1.width * 0.5 - 35.0), position2.yMax + 3f, 70f, 20f), AnimationWindowStyles.create) && AnimationWindowUtility.InitializeGameobjectForAnimation(this.m_State.activeGameObject))
        {
          this.m_State.selection.UpdateClip(this.m_State.selectedItem, AnimationUtility.GetAnimationClips(AnimationWindowUtility.GetClosestAnimationPlayerComponentInParents(this.m_State.activeGameObject.transform).gameObject)[0]);
          GUIUtility.ExitGUI();
        }
      }
      else
      {
        Color color = GUI.color;
        GUI.color = Color.gray;
        Vector2 vector2 = GUI.skin.label.CalcSize(AnimationWindowStyles.noAnimatableObjectSelectedText);
        GUI.Label(new Rect((float) ((double) position1.width * 0.5 - (double) vector2.x * 0.5), (float) ((double) position1.height * 0.5 - (double) vector2.y * 0.5), vector2.x, vector2.y), AnimationWindowStyles.noAnimatableObjectSelectedText);
        GUI.color = color;
      }
      GUI.EndClip();
      GUI.enabled = false;
    }

    private void EventLineOnGUI(Rect eventsRect)
    {
      eventsRect.width -= 15f;
      GUI.Label(eventsRect, GUIContent.none, AnimationWindowStyles.eventBackground);
      using (new EditorGUI.DisabledScope((UnityEngine.Object) this.m_State.selectedItem == (UnityEngine.Object) null || !this.m_State.selectedItem.animationIsEditable))
        this.m_Events.EventLineGUI(eventsRect, this.m_State);
    }

    private void RenderEventTooltip()
    {
      this.m_Events.DrawInstantTooltip(this.m_Position);
    }

    private void TabSelectionOnGUI()
    {
      GUILayout.FlexibleSpace();
      EditorGUI.BeginChangeCheck();
      GUILayout.Toggle((!this.m_State.showCurveEditor ? 1 : 0) != 0, AnimationWindowStyles.dopesheet, AnimationWindowStyles.miniToolbarButton, new GUILayoutOption[1]
      {
        GUILayout.Width(80f)
      });
      GUILayout.Toggle((this.m_State.showCurveEditor ? 1 : 0) != 0, AnimationWindowStyles.curves, AnimationWindowStyles.miniToolbarButton, new GUILayoutOption[1]
      {
        GUILayout.Width(80f)
      });
      if (EditorGUI.EndChangeCheck())
      {
        this.SwitchBetweenCurvesAndDopesheet();
      }
      else
      {
        if (!AnimEditor.kAnimationShowCurvesToggle.activated)
          return;
        this.SwitchBetweenCurvesAndDopesheet();
        Event.current.Use();
      }
    }

    private void HierarchyOnGUI()
    {
      Rect rect = GUILayoutUtility.GetRect(this.hierarchyWidth, this.hierarchyWidth, 0.0f, float.MaxValue, new GUILayoutOption[1]{ GUILayout.ExpandHeight(true) });
      if (this.m_State.disabled)
        return;
      this.m_Hierarchy.OnGUI(rect);
    }

    private void FrameRateInputFieldOnGUI()
    {
      AnimationWindowSelectionItem selectedItem = this.m_State.selectedItem;
      using (new EditorGUI.DisabledScope((UnityEngine.Object) selectedItem == (UnityEngine.Object) null || !selectedItem.animationIsEditable))
      {
        GUILayout.Label(AnimationWindowStyles.samples, AnimationWindowStyles.toolbarLabel, new GUILayoutOption[0]);
        EditorGUI.BeginChangeCheck();
        int num = EditorGUILayout.DelayedIntField((int) this.m_State.clipFrameRate, EditorStyles.toolbarTextField, new GUILayoutOption[1]{ GUILayout.Width(35f) });
        if (!EditorGUI.EndChangeCheck())
          return;
        this.m_State.clipFrameRate = (float) num;
        this.UpdateSelectedKeysToCurveEditor();
      }
    }

    private void ClipSelectionDropDownOnGUI()
    {
      this.m_ClipPopup.OnGUI();
    }

    private void DopeSheetOnGUI(Rect position)
    {
      Rect rect = new Rect(position.xMin, position.yMin, position.width - 15f, position.height);
      if (Event.current.type == EventType.Repaint)
      {
        this.m_DopeSheet.rect = rect;
        this.m_DopeSheet.SetTickMarkerRanges();
        this.m_DopeSheet.RecalculateBounds();
      }
      if (this.m_State.showCurveEditor)
        return;
      Rect position1 = new Rect(position.xMin, position.yMin, position.width - 15f, position.height - 15f);
      Rect position2 = new Rect(position1.xMin, position1.yMin, position1.width, 16f);
      this.m_DopeSheet.BeginViewGUI();
      GUI.Label(position, GUIContent.none, AnimationWindowStyles.dopeSheetBackground);
      if (!this.m_State.disabled)
      {
        this.m_DopeSheet.TimeRuler(position1, this.m_State.frameRate, false, true, 0.12f, this.m_State.timeFormat);
        this.m_DopeSheet.DrawMasterDopelineBackground(position2);
      }
      this.m_DopeSheet.OnGUI(position1, this.m_State.hierarchyState.scrollPos * -1f);
      this.m_DopeSheet.EndViewGUI();
      Rect position3 = new Rect(rect.xMax, rect.yMin, 15f, position1.height);
      float height = this.m_Hierarchy.GetTotalRect().height;
      float bottomValue = Mathf.Max(height, this.m_Hierarchy.GetContentSize().y);
      this.m_State.hierarchyState.scrollPos.y = GUI.VerticalScrollbar(position3, this.m_State.hierarchyState.scrollPos.y, height, 0.0f, bottomValue);
      if (!this.m_DopeSheet.spritePreviewLoading)
        return;
      this.Repaint();
    }

    private void CurveEditorOnGUI(Rect position)
    {
      if (Event.current.type == EventType.Repaint)
      {
        this.m_CurveEditor.rect = position;
        this.m_CurveEditor.SetTickMarkerRanges();
      }
      Rect position1 = new Rect(position.xMin, position.yMin, position.width - 15f, position.height - 15f);
      this.m_CurveEditor.vSlider = this.m_State.showCurveEditor;
      this.m_CurveEditor.hSlider = this.m_State.showCurveEditor;
      if (Event.current.type == EventType.Layout)
        this.UpdateCurveEditorData();
      this.m_CurveEditor.BeginViewGUI();
      if (!this.m_State.disabled)
      {
        GUI.Box(position1, GUIContent.none, AnimationWindowStyles.curveEditorBackground);
        this.m_CurveEditor.GridGUI();
      }
      EditorGUI.BeginChangeCheck();
      this.m_CurveEditor.CurveGUI();
      if (EditorGUI.EndChangeCheck())
        this.SaveChangedCurvesFromCurveEditor();
      this.m_CurveEditor.EndViewGUI();
    }

    private void TimeRulerOnGUI(Rect timeRulerRect)
    {
      Rect rect = new Rect(timeRulerRect.xMin, timeRulerRect.yMin, timeRulerRect.width - 15f, timeRulerRect.height);
      GUI.Box(timeRulerRect, GUIContent.none, EditorStyles.toolbarButton);
      if (!this.m_State.disabled)
      {
        this.RenderInRangeOverlay(rect);
        this.RenderSelectionOverlay(rect);
      }
      this.m_State.timeArea.TimeRuler(rect, this.m_State.frameRate, true, false, 1f, this.m_State.timeFormat);
      if (this.m_State.disabled)
        return;
      this.RenderOutOfRangeOverlay(rect);
    }

    private void AddEventButtonOnGUI()
    {
      AnimationWindowSelectionItem selectedItem = this.m_State.selectedItem;
      if (!((UnityEngine.Object) selectedItem != (UnityEngine.Object) null))
        return;
      using (new EditorGUI.DisabledScope(!selectedItem.animationIsEditable))
      {
        if (GUILayout.Button(AnimationWindowStyles.addEventContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
          this.m_Events.AddEvent(this.m_State.currentTime - selectedItem.timeOffset, selectedItem.rootGameObject, selectedItem.animationClip);
      }
    }

    private void AddKeyframeButtonOnGUI()
    {
      using (new EditorGUI.DisabledScope(!(bool) ((UnityEngine.Object) this.m_State.selection.Find((Predicate<AnimationWindowSelectionItem>) (selectedItem => selectedItem.animationIsEditable)))))
      {
        if (!GUILayout.Button(AnimationWindowStyles.addKeyframeContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
          return;
        this.SaveCurveEditorKeySelection();
        AnimationWindowUtility.AddSelectedKeyframes(this.m_State, AnimationKeyTime.Time(this.m_State.currentTime, this.m_State.frameRate));
        this.UpdateSelectedKeysToCurveEditor();
        GUIUtility.ExitGUI();
      }
    }

    private void PlayControlsOnGUI()
    {
      using (new EditorGUI.DisabledScope(!this.controlInterface.canPreview))
        this.PreviewButtonOnGUI();
      using (new EditorGUI.DisabledScope(!this.controlInterface.canRecord))
        this.RecordButtonOnGUI();
      if (GUILayout.Button(AnimationWindowStyles.firstKeyContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
      {
        this.controlInterface.GoToFirstKeyframe();
        EditorGUI.EndEditingActiveTextField();
      }
      if (GUILayout.Button(AnimationWindowStyles.prevKeyContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
      {
        this.controlInterface.GoToPreviousKeyframe();
        EditorGUI.EndEditingActiveTextField();
      }
      using (new EditorGUI.DisabledScope(!this.controlInterface.canPlay))
        this.PlayButtonOnGUI();
      if (GUILayout.Button(AnimationWindowStyles.nextKeyContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
      {
        this.controlInterface.GoToNextKeyframe();
        EditorGUI.EndEditingActiveTextField();
      }
      if (GUILayout.Button(AnimationWindowStyles.lastKeyContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
      {
        this.controlInterface.GoToLastKeyframe();
        EditorGUI.EndEditingActiveTextField();
      }
      GUILayout.FlexibleSpace();
      EditorGUI.BeginChangeCheck();
      int frame = EditorGUILayout.DelayedIntField(this.m_State.currentFrame, EditorStyles.toolbarTextField, new GUILayoutOption[1]{ GUILayout.Width(35f) });
      if (!EditorGUI.EndChangeCheck())
        return;
      this.controlInterface.GoToFrame(frame);
    }

    private void LinkOptionsOnGUI()
    {
      if (!this.m_State.linkedWithSequencer || GUILayout.Toggle(true, AnimationWindowStyles.sequencerLinkContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        return;
      this.m_State.linkedWithSequencer = false;
      this.m_State.selection.Clear();
      GUIUtility.ExitGUI();
    }

    private void HandleHotKeys()
    {
      if (!GUI.enabled || this.m_State.disabled)
        return;
      bool flag = false;
      if (AnimEditor.kAnimationPrevKeyframe.activated)
      {
        this.controlInterface.GoToPreviousKeyframe();
        flag = true;
      }
      if (AnimEditor.kAnimationNextKeyframe.activated)
      {
        this.controlInterface.GoToNextKeyframe();
        flag = true;
      }
      if (AnimEditor.kAnimationNextFrame.activated)
      {
        this.controlInterface.GoToNextFrame();
        flag = true;
      }
      if (AnimEditor.kAnimationPrevFrame.activated)
      {
        this.controlInterface.GoToPreviousFrame();
        flag = true;
      }
      if (AnimEditor.kAnimationFirstKey.activated)
      {
        this.controlInterface.GoToFirstKeyframe();
        flag = true;
      }
      if (AnimEditor.kAnimationLastKey.activated)
      {
        this.controlInterface.GoToLastKeyframe();
        flag = true;
      }
      if (flag)
      {
        Event.current.Use();
        this.Repaint();
      }
      if (AnimEditor.kAnimationPlayToggle.activated)
      {
        if (this.controlInterface.playing)
          this.controlInterface.StopPlayback();
        else
          this.controlInterface.StartPlayback();
        Event.current.Use();
      }
      if (AnimEditor.kAnimationRecordKeyframeSelected.activated)
      {
        this.SaveCurveEditorKeySelection();
        AnimationWindowUtility.AddSelectedKeyframes(this.m_State, this.controlInterface.time);
        this.UpdateSelectedKeysToCurveEditor();
        Event.current.Use();
      }
      if (!AnimEditor.kAnimationRecordKeyframeModified.activated)
        return;
      this.SaveCurveEditorKeySelection();
      this.controlInterface.ProcessCandidates();
      this.UpdateSelectedKeysToCurveEditor();
      Event.current.Use();
    }

    public void HandleGlobalHotkeys()
    {
      if (!this.m_State.previewing || !GUI.enabled || this.m_State.disabled)
        return;
      if (AnimEditor.kAnimationRecordKeyframeSelected.activated)
      {
        this.SaveCurveEditorKeySelection();
        AnimationWindowUtility.AddSelectedKeyframes(this.m_State, this.controlInterface.time);
        this.controlInterface.ClearCandidates();
        this.UpdateSelectedKeysToCurveEditor();
        Event.current.Use();
      }
      if (!AnimEditor.kAnimationRecordKeyframeModified.activated)
        return;
      this.SaveCurveEditorKeySelection();
      this.controlInterface.ProcessCandidates();
      this.UpdateSelectedKeysToCurveEditor();
      Event.current.Use();
    }

    private void PlayButtonOnGUI()
    {
      EditorGUI.BeginChangeCheck();
      bool flag = GUILayout.Toggle(this.controlInterface.playing, AnimationWindowStyles.playContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      if (flag)
        this.controlInterface.StartPlayback();
      else
        this.controlInterface.StopPlayback();
      EditorGUI.EndEditingActiveTextField();
    }

    private void PreviewButtonOnGUI()
    {
      EditorGUI.BeginChangeCheck();
      bool flag = GUILayout.Toggle(this.controlInterface.previewing, AnimationWindowStyles.previewContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      if (flag)
        this.m_State.StartPreview();
      else
        this.m_State.StopPreview();
    }

    private void RecordButtonOnGUI()
    {
      EditorGUI.BeginChangeCheck();
      Color color = GUI.color;
      if (this.controlInterface.recording)
      {
        Color recordedPropertyColor = AnimationMode.recordedPropertyColor;
        recordedPropertyColor.a *= GUI.color.a;
        GUI.color = recordedPropertyColor;
      }
      bool flag = GUILayout.Toggle(this.controlInterface.recording, AnimationWindowStyles.recordContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        if (flag)
        {
          this.m_State.StartRecording();
        }
        else
        {
          this.m_State.StopRecording();
          InspectorWindow.RepaintAllInspectors();
        }
      }
      GUI.color = color;
    }

    private void SwitchBetweenCurvesAndDopesheet()
    {
      if (!this.m_State.showCurveEditor)
        this.SwitchToCurveEditor();
      else
        this.SwitchToDopeSheetEditor();
    }

    internal void SwitchToCurveEditor()
    {
      this.m_State.showCurveEditor = true;
      this.UpdateSelectedKeysToCurveEditor();
      AnimationWindowUtility.SyncTimeArea((TimeArea) this.m_DopeSheet, (TimeArea) this.m_CurveEditor);
      this.m_State.timeArea = (TimeArea) this.m_CurveEditor;
    }

    internal void SwitchToDopeSheetEditor()
    {
      this.m_State.showCurveEditor = false;
      this.UpdateSelectedKeysFromCurveEditor();
      AnimationWindowUtility.SyncTimeArea((TimeArea) this.m_CurveEditor, (TimeArea) this.m_DopeSheet);
      this.m_State.timeArea = (TimeArea) this.m_DopeSheet;
    }

    private void RenderSelectionOverlay(Rect rect)
    {
      if (this.m_State.showCurveEditor && !this.m_CurveEditor.hasSelection || !this.m_State.showCurveEditor && this.m_State.selectedKeys.Count == 0)
        return;
      Bounds bounds = !this.m_State.showCurveEditor ? this.m_State.selectionBounds : this.m_CurveEditor.selectionBounds;
      float startPixel = this.m_State.TimeToPixel(bounds.min.x) + rect.xMin;
      float endPixel = this.m_State.TimeToPixel(bounds.max.x) + rect.xMin;
      if ((double) endPixel - (double) startPixel < 14.0)
      {
        float num = (float) (((double) startPixel + (double) endPixel) * 0.5);
        startPixel = num - 7f;
        endPixel = num + 7f;
      }
      AnimationWindowUtility.DrawSelectionOverlay(rect, AnimEditor.selectionRangeColor, startPixel, endPixel);
    }

    private void RenderInRangeOverlay(Rect rect)
    {
      Color inRangeColor = AnimEditor.inRangeColor;
      Color color = !this.controlInterface.recording ? (!this.controlInterface.previewing ? Color.clear : inRangeColor * AnimationMode.animatedPropertyColor) : inRangeColor * AnimationMode.recordedPropertyColor;
      Vector2 timeRange = this.m_State.timeRange;
      AnimationWindowUtility.DrawInRangeOverlay(rect, color, this.m_State.TimeToPixel(timeRange.x) + rect.xMin, this.m_State.TimeToPixel(timeRange.y) + rect.xMin);
    }

    private void RenderOutOfRangeOverlay(Rect rect)
    {
      Color outOfRangeColor = AnimEditor.outOfRangeColor;
      if (this.controlInterface.recording)
        outOfRangeColor *= AnimationMode.recordedPropertyColor;
      else if (this.controlInterface.previewing)
        outOfRangeColor *= AnimationMode.animatedPropertyColor;
      Vector2 timeRange = this.m_State.timeRange;
      AnimationWindowUtility.DrawOutOfRangeOverlay(rect, outOfRangeColor, this.m_State.TimeToPixel(timeRange.x) + rect.xMin, this.m_State.TimeToPixel(timeRange.y) + rect.xMin);
    }

    private void SynchronizeLayout()
    {
      this.m_HorizontalSplitter.realSizes[1] = (int) Mathf.Min(this.m_Position.width - (float) this.m_HorizontalSplitter.realSizes[0], (float) this.m_HorizontalSplitter.realSizes[1]);
      if ((UnityEngine.Object) this.selectedItem != (UnityEngine.Object) null && (UnityEngine.Object) this.selectedItem.animationClip != (UnityEngine.Object) null)
        this.m_State.frameRate = this.selectedItem.animationClip.frameRate;
      else
        this.m_State.frameRate = 60f;
    }

    private void SaveChangedCurvesFromCurveEditor()
    {
      this.m_State.SaveKeySelection("Edit Curve");
      Dictionary<AnimationClip, AnimEditor.ChangedCurvesPerClip> dictionary = new Dictionary<AnimationClip, AnimEditor.ChangedCurvesPerClip>();
      AnimEditor.ChangedCurvesPerClip changedCurvesPerClip = new AnimEditor.ChangedCurvesPerClip();
      for (int index = 0; index < this.m_CurveEditor.animationCurves.Length; ++index)
      {
        CurveWrapper animationCurve = this.m_CurveEditor.animationCurves[index];
        if (animationCurve.changed)
        {
          if (!animationCurve.animationIsEditable)
            Debug.LogError((object) "Curve is not editable and shouldn't be saved.");
          if ((UnityEngine.Object) animationCurve.animationClip != (UnityEngine.Object) null)
          {
            if (dictionary.TryGetValue(animationCurve.animationClip, out changedCurvesPerClip))
            {
              changedCurvesPerClip.bindings.Add(animationCurve.binding);
              changedCurvesPerClip.curves.Add(animationCurve.curve.length <= 0 ? (AnimationCurve) null : animationCurve.curve);
            }
            else
            {
              changedCurvesPerClip.bindings = new List<EditorCurveBinding>();
              changedCurvesPerClip.curves = new List<AnimationCurve>();
              changedCurvesPerClip.bindings.Add(animationCurve.binding);
              changedCurvesPerClip.curves.Add(animationCurve.curve.length <= 0 ? (AnimationCurve) null : animationCurve.curve);
              dictionary.Add(animationCurve.animationClip, changedCurvesPerClip);
            }
          }
          animationCurve.changed = false;
        }
      }
      if (dictionary.Count <= 0)
        return;
      foreach (KeyValuePair<AnimationClip, AnimEditor.ChangedCurvesPerClip> keyValuePair in dictionary)
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) keyValuePair.Key, "Edit Curve");
        AnimationUtility.SetEditorCurves(keyValuePair.Key, keyValuePair.Value.bindings.ToArray(), keyValuePair.Value.curves.ToArray());
      }
      this.m_State.ResampleAnimation();
    }

    private void UpdateSelectedKeysFromCurveEditor()
    {
      this.m_State.ClearKeySelections();
      foreach (CurveSelection selectedCurve in this.m_CurveEditor.selectedCurves)
      {
        AnimationWindowKeyframe animationWindowKeyframe = AnimationWindowUtility.CurveSelectionToAnimationWindowKeyframe(selectedCurve, this.m_State.allCurves);
        if (animationWindowKeyframe != null)
          this.m_State.SelectKey(animationWindowKeyframe);
      }
    }

    private void UpdateSelectedKeysToCurveEditor()
    {
      this.UpdateCurveEditorData();
      this.m_CurveEditor.ClearSelection();
      this.m_CurveEditor.BeginRangeSelection();
      foreach (AnimationWindowKeyframe selectedKey in this.m_State.selectedKeys)
      {
        CurveSelection curveSelection = AnimationWindowUtility.AnimationWindowKeyframeToCurveSelection(selectedKey, this.m_CurveEditor);
        if (curveSelection != null)
          this.m_CurveEditor.AddSelection(curveSelection);
      }
      this.m_CurveEditor.EndRangeSelection();
    }

    private void SaveCurveEditorKeySelection()
    {
      if (this.m_State.showCurveEditor)
        this.UpdateSelectedKeysFromCurveEditor();
      else
        this.UpdateSelectedKeysToCurveEditor();
      this.m_CurveEditor.SaveKeySelection("Edit Curve");
    }

    public void BeginKeyModification()
    {
      this.SaveCurveEditorKeySelection();
      this.m_State.SaveKeySelection("Edit Curve");
      this.m_State.ClearKeySelections();
    }

    public void EndKeyModification()
    {
      this.UpdateSelectedKeysToCurveEditor();
    }

    private void HandleCopyPaste()
    {
      if (Event.current.type != EventType.ValidateCommand && Event.current.type != EventType.ExecuteCommand)
        return;
      switch (Event.current.commandName)
      {
        case "Copy":
          if (Event.current.type == EventType.ExecuteCommand)
          {
            if (this.m_State.showCurveEditor)
              this.UpdateSelectedKeysFromCurveEditor();
            this.m_State.CopyKeys();
          }
          Event.current.Use();
          break;
        case "Paste":
          if (Event.current.type == EventType.ExecuteCommand)
          {
            this.SaveCurveEditorKeySelection();
            this.m_State.PasteKeys();
            this.UpdateSelectedKeysToCurveEditor();
            GUIUtility.ExitGUI();
          }
          Event.current.Use();
          break;
      }
    }

    internal void UpdateCurveEditorData()
    {
      this.m_CurveEditor.animationCurves = this.m_State.activeCurveWrappers;
    }

    public void Repaint()
    {
      if (!((UnityEngine.Object) this.m_OwnerWindow != (UnityEngine.Object) null))
        return;
      this.m_OwnerWindow.Repaint();
    }

    private void Initialize()
    {
      AnimationWindowStyles.Initialize();
      this.InitializeHierarchy();
      this.m_CurveEditor.state = (ICurveEditorState) this.m_State;
      this.m_HorizontalSplitter.realSizes[0] = 300;
      this.m_HorizontalSplitter.realSizes[1] = (int) Mathf.Max(this.m_Position.width - 300f, 300f);
      this.m_DopeSheet.rect = new Rect(0.0f, 0.0f, this.contentWidth, 100f);
      this.m_Initialized = true;
    }

    private void InitializeClipSelection()
    {
      this.m_ClipPopup = new AnimationWindowClipPopup();
    }

    private void InitializeHierarchy()
    {
      this.m_Hierarchy = new AnimationWindowHierarchy(this.m_State, this.m_OwnerWindow, new Rect(0.0f, 0.0f, this.hierarchyWidth, 100f));
    }

    private void InitializeDopeSheet()
    {
      this.m_DopeSheet = new DopeSheetEditor(this.m_OwnerWindow);
      this.m_DopeSheet.SetTickMarkerRanges();
      this.m_DopeSheet.hSlider = true;
      this.m_DopeSheet.shownArea = new Rect(1f, 1f, 1f, 1f);
      this.m_DopeSheet.rect = new Rect(0.0f, 0.0f, this.contentWidth, 100f);
      this.m_DopeSheet.hTicks.SetTickModulosForFrameRate(this.m_State.frameRate);
    }

    private void InitializeEvents()
    {
      this.m_Events = new AnimationEventTimeLine(this.m_OwnerWindow);
    }

    private void InitializeCurveEditor()
    {
      this.m_CurveEditor = new CurveEditor(new Rect(0.0f, 0.0f, this.contentWidth, 100f), new CurveWrapper[0], false);
      CurveEditorSettings curveEditorSettings = new CurveEditorSettings();
      curveEditorSettings.hTickStyle.distMin = 30;
      curveEditorSettings.hTickStyle.distFull = 80;
      curveEditorSettings.hTickStyle.distLabel = 0;
      if (EditorGUIUtility.isProSkin)
      {
        curveEditorSettings.vTickStyle.tickColor.color = new Color(1f, 1f, 1f, curveEditorSettings.vTickStyle.tickColor.color.a);
        curveEditorSettings.vTickStyle.labelColor.color = new Color(1f, 1f, 1f, curveEditorSettings.vTickStyle.labelColor.color.a);
      }
      curveEditorSettings.vTickStyle.distMin = 15;
      curveEditorSettings.vTickStyle.distFull = 40;
      curveEditorSettings.vTickStyle.distLabel = 30;
      curveEditorSettings.vTickStyle.stubs = true;
      curveEditorSettings.hRangeMin = 0.0f;
      curveEditorSettings.hRangeLocked = false;
      curveEditorSettings.vRangeLocked = false;
      curveEditorSettings.hSlider = true;
      curveEditorSettings.vSlider = true;
      curveEditorSettings.allowDeleteLastKeyInCurve = true;
      curveEditorSettings.rectangleToolFlags = CurveEditorSettings.RectangleToolFlags.FullRectangleTool;
      curveEditorSettings.undoRedoSelection = true;
      this.m_CurveEditor.shownArea = new Rect(1f, 1f, 1f, 1f);
      this.m_CurveEditor.settings = curveEditorSettings;
      this.m_CurveEditor.state = (ICurveEditorState) this.m_State;
    }

    private void InitializeHorizontalSplitter()
    {
      this.m_HorizontalSplitter = new SplitterState(new float[2]
      {
        300f,
        900f
      }, new int[2]{ 300, 300 }, (int[]) null);
      this.m_HorizontalSplitter.realSizes[0] = 300;
      this.m_HorizontalSplitter.realSizes[1] = 300;
    }

    private void InitializeOverlay()
    {
      this.m_Overlay = new AnimEditorOverlay();
    }

    private void InitializeNonserializedValues()
    {
      this.m_State.onFrameRateChange += (Action<float>) (newFrameRate =>
      {
        this.m_CurveEditor.invSnap = newFrameRate;
        this.m_CurveEditor.hTicks.SetTickModulosForFrameRate(newFrameRate);
      });
      this.m_State.onStartLiveEdit += new Action(this.OnStartLiveEdit);
      this.m_State.onEndLiveEdit += new Action(this.OnEndLiveEdit);
      this.m_State.selection.onSelectionChanged += new Action(this.OnSelectionChanged);
    }

    private struct ChangedCurvesPerClip
    {
      public List<EditorCurveBinding> bindings;
      public List<AnimationCurve> curves;
    }
  }
}
