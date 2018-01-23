// Decompiled with JetBrains decompiler
// Type: UnityEditor.GUIViewDebuggerWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor
{
  internal class GUIViewDebuggerWindow : EditorWindow
  {
    [SerializeField]
    private GUIViewDebuggerWindow.InstructionType m_InstructionType = GUIViewDebuggerWindow.InstructionType.Draw;
    private bool m_ShowHighlighter = true;
    [NonSerialized]
    private bool m_QueuedPointInspection = false;
    private readonly SplitterState m_InstructionListDetailSplitter = new SplitterState(new float[2]{ 30f, 70f }, new int[2]{ 32, 32 }, (int[]) null);
    private static GUIViewDebuggerWindow s_ActiveInspector;
    [SerializeField]
    private GUIView m_Inspected;
    private EditorWindow m_InspectedEditorWindow;
    private IBaseInspectView m_InstructionModeView;
    private VisualElement m_ContentHighlighter;
    private VisualElement m_PaddingHighlighter;
    [NonSerialized]
    private Vector2 m_PointToInspect;

    protected GUIViewDebuggerWindow()
    {
      this.m_InstructionModeView = (IBaseInspectView) new StyleDrawInspectView(this);
    }

    private static EditorWindow GetEditorWindow(GUIView view)
    {
      HostView hostView = view as HostView;
      if ((UnityEngine.Object) hostView != (UnityEngine.Object) null)
        return hostView.actualView;
      return (EditorWindow) null;
    }

    private static string GetViewName(GUIView view)
    {
      EditorWindow editorWindow = GUIViewDebuggerWindow.GetEditorWindow(view);
      if ((UnityEngine.Object) editorWindow != (UnityEngine.Object) null)
        return editorWindow.titleContent.text;
      return view.GetType().Name;
    }

    public GUIView inspected
    {
      get
      {
        if ((UnityEngine.Object) this.m_Inspected != (UnityEngine.Object) null || (UnityEngine.Object) this.m_InspectedEditorWindow == (UnityEngine.Object) null)
          return this.m_Inspected;
        GUIView parent = (GUIView) this.m_InspectedEditorWindow.m_Parent;
        this.inspected = parent;
        return parent;
      }
      private set
      {
        if (!((UnityEngine.Object) this.m_Inspected != (UnityEngine.Object) value))
          return;
        this.ClearInstructionHighlighter();
        this.m_Inspected = value;
        if ((UnityEngine.Object) this.m_Inspected != (UnityEngine.Object) null)
        {
          this.m_InspectedEditorWindow = !(this.m_Inspected is HostView) ? (EditorWindow) null : ((HostView) this.m_Inspected).actualView;
          GUIViewDebuggerHelper.DebugWindow(this.m_Inspected);
          this.m_Inspected.Repaint();
        }
        else
          GUIViewDebuggerHelper.StopDebugging();
        if (this.instructionModeView != null)
          this.instructionModeView.ClearRowSelection();
        this.OnInspectedViewChanged();
      }
    }

    public IBaseInspectView instructionModeView
    {
      get
      {
        return this.m_InstructionModeView;
      }
    }

    public void ClearInstructionHighlighter()
    {
      if (this.m_PaddingHighlighter == null || this.m_PaddingHighlighter.shadow.parent == null)
        return;
      VisualElement parent = this.m_PaddingHighlighter.shadow.parent;
      this.m_PaddingHighlighter.RemoveFromHierarchy();
      this.m_ContentHighlighter.RemoveFromHierarchy();
      parent.Dirty(ChangeType.Repaint);
    }

    public void HighlightInstruction(GUIView view, Rect instructionRect, GUIStyle style)
    {
      if (!this.m_ShowHighlighter)
        return;
      this.ClearInstructionHighlighter();
      if (this.m_PaddingHighlighter == null)
      {
        this.m_PaddingHighlighter = new VisualElement();
        this.m_PaddingHighlighter.style.backgroundColor = (StyleValue<Color>) GUIViewDebuggerWindow.Styles.paddingHighlighterColor;
        this.m_ContentHighlighter = new VisualElement();
        this.m_ContentHighlighter.style.backgroundColor = (StyleValue<Color>) GUIViewDebuggerWindow.Styles.contentHighlighterColor;
      }
      this.m_PaddingHighlighter.layout = instructionRect;
      view.visualTree.Add(this.m_PaddingHighlighter);
      if (style != null)
        instructionRect = style.padding.Remove(instructionRect);
      this.m_ContentHighlighter.layout = instructionRect;
      view.visualTree.Add(this.m_ContentHighlighter);
    }

    private GUIViewDebuggerWindow.InstructionType instructionType
    {
      get
      {
        return this.m_InstructionType;
      }
      set
      {
        if (this.m_InstructionType == value && this.m_InstructionModeView != null)
          return;
        this.m_InstructionType = value;
        switch (this.m_InstructionType)
        {
          case GUIViewDebuggerWindow.InstructionType.Draw:
            this.m_InstructionModeView = (IBaseInspectView) new StyleDrawInspectView(this);
            break;
          case GUIViewDebuggerWindow.InstructionType.Clip:
            this.m_InstructionModeView = (IBaseInspectView) new GUIClipInspectView(this);
            break;
          case GUIViewDebuggerWindow.InstructionType.Layout:
            this.m_InstructionModeView = (IBaseInspectView) new GUILayoutInspectView(this);
            break;
          case GUIViewDebuggerWindow.InstructionType.NamedControl:
            this.m_InstructionModeView = (IBaseInspectView) new GUINamedControlInspectView(this);
            break;
          case GUIViewDebuggerWindow.InstructionType.Property:
            this.m_InstructionModeView = (IBaseInspectView) new GUIPropertyInspectView(this);
            break;
          case GUIViewDebuggerWindow.InstructionType.Unified:
            this.m_InstructionModeView = (IBaseInspectView) new UnifiedInspectView(this);
            break;
        }
        this.m_InstructionModeView.UpdateInstructions();
      }
    }

    private static void Init()
    {
      if ((UnityEngine.Object) GUIViewDebuggerWindow.s_ActiveInspector == (UnityEngine.Object) null)
        GUIViewDebuggerWindow.s_ActiveInspector = (GUIViewDebuggerWindow) EditorWindow.GetWindow(typeof (GUIViewDebuggerWindow));
      GUIViewDebuggerWindow.s_ActiveInspector.Show();
    }

    private void OnEnable()
    {
      this.titleContent = new GUIContent("GUI Inspector");
      GUIViewDebuggerHelper.onViewInstructionsChanged += new Action(this.OnInspectedViewChanged);
      GUIView inspected = this.m_Inspected;
      this.inspected = (GUIView) null;
      this.inspected = inspected;
      this.m_InstructionModeView = (IBaseInspectView) null;
      this.instructionType = this.m_InstructionType;
    }

    private void OnDisable()
    {
      GUIViewDebuggerHelper.onViewInstructionsChanged -= new Action(this.OnInspectedViewChanged);
      GUIViewDebuggerHelper.StopDebugging();
      this.ClearInstructionHighlighter();
    }

    private void OnBecameVisible()
    {
      this.OnShowOverlayChanged();
    }

    private void OnBecameInvisible()
    {
      this.ClearInstructionHighlighter();
    }

    private void OnGUI()
    {
      this.DoToolbar();
      this.ShowDrawInstructions();
    }

    private void OnInspectedViewChanged()
    {
      this.RefreshData();
      this.Repaint();
    }

    private void DoToolbar()
    {
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      this.DoWindowPopup();
      this.DoInspectTypePopup();
      this.DoInstructionOverlayToggle();
      GUILayout.EndHorizontal();
    }

    private bool CanInspectView(GUIView view)
    {
      if ((UnityEngine.Object) view == (UnityEngine.Object) null)
        return false;
      EditorWindow editorWindow = GUIViewDebuggerWindow.GetEditorWindow(view);
      return (UnityEngine.Object) editorWindow == (UnityEngine.Object) null || !((UnityEngine.Object) editorWindow == (UnityEngine.Object) this);
    }

    private void DoWindowPopup()
    {
      string t = !((UnityEngine.Object) this.inspected == (UnityEngine.Object) null) ? GUIViewDebuggerWindow.GetViewName(this.inspected) : GUIViewDebuggerWindow.Styles.defaultWindowPopupText;
      GUILayout.Label(GUIViewDebuggerWindow.Styles.inspectedWindowLabel, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      Rect rect = GUILayoutUtility.GetRect(GUIContent.Temp(t), EditorStyles.toolbarDropDown, new GUILayoutOption[1]{ GUILayout.ExpandWidth(true) });
      if (!GUI.Button(rect, GUIContent.Temp(t), EditorStyles.toolbarDropDown))
        return;
      List<GUIView> views = new List<GUIView>();
      GUIViewDebuggerHelper.GetViews(views);
      List<GUIContent> guiContentList = new List<GUIContent>(views.Count + 1);
      guiContentList.Add(new GUIContent("None"));
      int selected = 0;
      List<GUIView> guiViewList = new List<GUIView>(views.Count + 1);
      for (int index = 0; index < views.Count; ++index)
      {
        GUIView view = views[index];
        if (this.CanInspectView(view))
        {
          GUIContent guiContent = new GUIContent(string.Format("{0}. {1}", (object) guiContentList.Count, (object) GUIViewDebuggerWindow.GetViewName(view)));
          guiContentList.Add(guiContent);
          guiViewList.Add(view);
          if ((UnityEngine.Object) view == (UnityEngine.Object) this.inspected)
            selected = guiViewList.Count;
        }
      }
      EditorUtility.DisplayCustomMenu(rect, guiContentList.ToArray(), selected, new EditorUtility.SelectMenuItemFunction(this.OnWindowSelected), (object) guiViewList);
    }

    private void DoInspectTypePopup()
    {
      EditorGUI.BeginChangeCheck();
      GUIViewDebuggerWindow.InstructionType instructionType = (GUIViewDebuggerWindow.InstructionType) EditorGUILayout.EnumPopup((Enum) this.m_InstructionType, EditorStyles.toolbarDropDown, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.instructionType = instructionType;
    }

    private void DoInstructionOverlayToggle()
    {
      EditorGUI.BeginChangeCheck();
      this.m_ShowHighlighter = GUILayout.Toggle(this.m_ShowHighlighter, GUIContent.Temp("Show overlay"), EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.OnShowOverlayChanged();
    }

    private void OnShowOverlayChanged()
    {
      if (!this.m_ShowHighlighter)
        this.ClearInstructionHighlighter();
      else if ((UnityEngine.Object) this.inspected != (UnityEngine.Object) null)
        this.instructionModeView.ShowOverlay();
    }

    private void OnWindowSelected(object userdata, string[] options, int selected)
    {
      --selected;
      this.inspected = selected >= 0 ? ((List<GUIView>) userdata)[selected] : (GUIView) null;
    }

    private void RefreshData()
    {
      this.instructionModeView.UpdateInstructions();
    }

    private void ShowDrawInstructions()
    {
      if ((UnityEngine.Object) this.inspected == (UnityEngine.Object) null)
      {
        this.ClearInstructionHighlighter();
      }
      else
      {
        if (this.m_QueuedPointInspection)
        {
          this.instructionModeView.ClearRowSelection();
          this.instructionModeView.SelectRow(this.FindInstructionUnderPoint(this.m_PointToInspect));
          this.m_QueuedPointInspection = false;
        }
        SplitterGUILayout.BeginHorizontalSplit(this.m_InstructionListDetailSplitter);
        this.instructionModeView.DrawInstructionList();
        EditorGUILayout.BeginVertical();
        this.instructionModeView.DrawSelectedInstructionDetails();
        EditorGUILayout.EndVertical();
        SplitterGUILayout.EndHorizontalSplit();
      }
    }

    private void InspectPointAt(Vector2 point)
    {
      this.m_PointToInspect = point;
      this.m_QueuedPointInspection = true;
      this.inspected.Repaint();
      this.Repaint();
    }

    private int FindInstructionUnderPoint(Vector2 point)
    {
      List<IMGUIDrawInstruction> drawInstructions = new List<IMGUIDrawInstruction>();
      GUIViewDebuggerHelper.GetDrawInstructions(drawInstructions);
      for (int index = 0; index < drawInstructions.Count; ++index)
      {
        if (drawInstructions[index].rect.Contains(point))
          return index;
      }
      return -1;
    }

    private enum InstructionType
    {
      Draw,
      Clip,
      Layout,
      NamedControl,
      Property,
      Unified,
    }

    internal static class Styles
    {
      public static readonly string defaultWindowPopupText = "<Please Select>";
      public static readonly GUIContent inspectedWindowLabel = new GUIContent("Inspected View: ");
      public static readonly GUIStyle listItem = new GUIStyle((GUIStyle) "PR Label");
      public static readonly GUIStyle listItemBackground = new GUIStyle((GUIStyle) "CN EntryBackOdd");
      public static readonly GUIStyle listBackgroundStyle = new GUIStyle((GUIStyle) "CN Box");
      public static readonly GUIStyle boxStyle = new GUIStyle((GUIStyle) "CN Box");
      public static readonly GUIStyle stackframeStyle = new GUIStyle(EditorStyles.label);
      public static readonly GUIStyle stacktraceBackground = new GUIStyle((GUIStyle) "CN Box");
      public static readonly GUIStyle centeredText = new GUIStyle((GUIStyle) "PR Label");
      public static readonly Color contentHighlighterColor = new Color(0.62f, 0.77f, 0.9f, 0.5f);
      public static readonly Color paddingHighlighterColor = new Color(0.76f, 0.87f, 0.71f, 0.5f);

      static Styles()
      {
        GUIViewDebuggerWindow.Styles.stackframeStyle.margin = new RectOffset(0, 0, 0, 0);
        GUIViewDebuggerWindow.Styles.stackframeStyle.padding = new RectOffset(0, 0, 0, 0);
        GUIViewDebuggerWindow.Styles.stacktraceBackground.padding = new RectOffset(5, 5, 5, 5);
        GUIViewDebuggerWindow.Styles.centeredText.alignment = TextAnchor.MiddleCenter;
        GUIViewDebuggerWindow.Styles.centeredText.stretchHeight = true;
        GUIViewDebuggerWindow.Styles.centeredText.stretchWidth = true;
      }
    }
  }
}
