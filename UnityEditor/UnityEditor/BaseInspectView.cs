// Decompiled with JetBrains decompiler
// Type: UnityEditor.BaseInspectView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class BaseInspectView : IBaseInspectView
  {
    [NonSerialized]
    private readonly ListViewState m_ListViewState = new ListViewState();
    private Vector2 m_InstructionDetailsScrollPos = new Vector2();
    private readonly SplitterState m_InstructionDetailStacktraceSplitter = new SplitterState(new float[2]{ 80f, 20f }, new int[2]{ 100, 100 }, (int[]) null);
    private GUIViewDebuggerWindow m_DebuggerWindow;

    public BaseInspectView(GUIViewDebuggerWindow guiViewDebuggerWindow)
    {
      this.m_DebuggerWindow = guiViewDebuggerWindow;
    }

    protected ListViewState listViewState
    {
      get
      {
        return this.m_ListViewState;
      }
    }

    protected GUIViewDebuggerWindow debuggerWindow
    {
      get
      {
        return this.m_DebuggerWindow;
      }
    }

    public abstract void UpdateInstructions();

    public virtual void DrawInstructionList()
    {
      Event current1 = Event.current;
      this.m_ListViewState.totalRows = this.GetInstructionCount();
      EditorGUILayout.BeginVertical(GUIViewDebuggerWindow.Styles.listBackgroundStyle, new GUILayoutOption[0]);
      GUILayout.Label(BaseInspectView.Styles.instructionsLabel);
      int controlId = GUIUtility.GetControlID(FocusType.Keyboard);
      IEnumerator enumerator = ListViewGUI.ListView(this.m_ListViewState, GUIViewDebuggerWindow.Styles.listBackgroundStyle, new GUILayoutOption[0]).GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          ListViewElement current2 = (ListViewElement) enumerator.Current;
          if (current1.type == EventType.MouseDown && current1.button == 0 && (current2.position.Contains(current1.mousePosition) && current1.clickCount == 2))
            this.OnDoubleClickInstruction(current2.row);
          if (current1.type == EventType.Repaint && current2.row < this.GetInstructionCount())
            this.DoDrawInstruction(current2, controlId);
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      EditorGUILayout.EndVertical();
    }

    public virtual void DrawSelectedInstructionDetails()
    {
      if (this.m_ListViewState.selectionChanged)
        this.OnSelectedInstructionChanged(this.m_ListViewState.row);
      else if (this.m_ListViewState.row >= this.GetInstructionCount())
        this.OnSelectedInstructionChanged(-1);
      if (!this.isInstructionSelected)
      {
        this.DoDrawNothingSelected();
      }
      else
      {
        SplitterGUILayout.BeginVerticalSplit(this.m_InstructionDetailStacktraceSplitter);
        this.m_InstructionDetailsScrollPos = EditorGUILayout.BeginScrollView(this.m_InstructionDetailsScrollPos, GUIViewDebuggerWindow.Styles.boxStyle, new GUILayoutOption[0]);
        this.DoDrawSelectedInstructionDetails(this.m_ListViewState.row);
        EditorGUILayout.EndScrollView();
        this.DrawInspectedStacktrace();
        SplitterGUILayout.EndVerticalSplit();
      }
    }

    public abstract void ShowOverlay();

    public virtual void SelectRow(int index)
    {
      this.m_ListViewState.row = index;
      this.m_ListViewState.selectionChanged = true;
    }

    public virtual void ClearRowSelection()
    {
      this.m_ListViewState.row = -1;
      this.m_ListViewState.selectionChanged = true;
    }

    protected abstract int GetInstructionCount();

    protected abstract void DoDrawInstruction(ListViewElement el, int controlId);

    protected abstract void DrawInspectedStacktrace();

    protected virtual bool isInstructionSelected
    {
      get
      {
        return this.m_ListViewState.row >= 0 && this.m_ListViewState.row < this.GetInstructionCount();
      }
    }

    protected void DrawStackFrameList(StackFrame[] stackframes)
    {
      if (stackframes == null)
        return;
      foreach (StackFrame stackframe in stackframes)
      {
        if (!string.IsNullOrEmpty(stackframe.sourceFile))
          GUILayout.Label(string.Format("{0} [{1}:{2}]", (object) stackframe.signature, (object) stackframe.sourceFile, (object) stackframe.lineNumber), GUIViewDebuggerWindow.Styles.stackframeStyle, new GUILayoutOption[0]);
      }
    }

    protected void DrawInspectedRect(Rect instructionRect)
    {
      Rect rect1 = GUILayoutUtility.GetRect(0.0f, 100f);
      RectOffset rectOffset = new RectOffset(50, 100, Mathf.CeilToInt(34f), Mathf.CeilToInt(16f));
      Rect position1 = rectOffset.Remove(rect1);
      float imageAspect = instructionRect.width / instructionRect.height;
      Rect outScreenRect = new Rect();
      Rect outSourceRect = new Rect();
      GUI.CalculateScaledTextureRects(position1, ScaleMode.ScaleToFit, imageAspect, ref outScreenRect, ref outSourceRect);
      Rect position2 = outScreenRect;
      position2.width = Mathf.Max(80f, position2.width);
      position2.height = Mathf.Max(26f, position2.height);
      Rect position3 = new Rect();
      position3.height = 16f;
      position3.width = (float) (rectOffset.left * 2);
      position3.y = position2.y - (float) rectOffset.top;
      position3.x = position2.x - position3.width / 2f;
      Rect position4 = new Rect() { height = 16f, width = (float) (rectOffset.right * 2), y = position2.yMax };
      position4.x = position2.xMax - position4.width / 2f;
      Rect rect2 = new Rect() { x = position2.x, y = position3.yMax + 2f, width = position2.width, height = 16f };
      Rect position5 = rect2;
      position5.width = rect2.width / 3f;
      position5.x = rect2.x + (float) (((double) rect2.width - (double) position5.width) / 2.0);
      Rect rect3 = position2;
      rect3.x = position2.xMax;
      rect3.width = 16f;
      Rect position6 = rect3;
      position6.height = 16f;
      position6.width = (float) rectOffset.right;
      position6.y += (float) (((double) rect3.height - (double) position6.height) / 2.0);
      GUI.Label(position3, string.Format("({0},{1})", (object) instructionRect.x, (object) instructionRect.y), BaseInspectView.Styles.centeredLabel);
      Handles.color = new Color(1f, 1f, 1f, 0.5f);
      Vector3 p1 = new Vector3(rect2.x, position5.y);
      Vector3 p2 = new Vector3(rect2.x, position5.yMax);
      Handles.DrawLine(p1, p2);
      p1.x = p2.x = rect2.xMax;
      Handles.DrawLine(p1, p2);
      p1.x = rect2.x;
      p1.y = p2.y = Mathf.Lerp(p1.y, p2.y, 0.5f);
      p2.x = position5.x;
      Handles.DrawLine(p1, p2);
      p1.x = position5.xMax;
      p2.x = rect2.xMax;
      Handles.DrawLine(p1, p2);
      GUI.Label(position5, instructionRect.width.ToString(), BaseInspectView.Styles.centeredLabel);
      p1 = new Vector3(rect3.x, rect3.y);
      p2 = new Vector3(rect3.xMax, rect3.y);
      Handles.DrawLine(p1, p2);
      p1.y = p2.y = rect3.yMax;
      Handles.DrawLine(p1, p2);
      p1.x = p2.x = Mathf.Lerp(p1.x, p2.x, 0.5f);
      p1.y = rect3.y;
      p2.y = position6.y;
      Handles.DrawLine(p1, p2);
      p1.y = position6.yMax;
      p2.y = rect3.yMax;
      Handles.DrawLine(p1, p2);
      GUI.Label(position6, instructionRect.height.ToString());
      GUI.Label(position4, string.Format("({0},{1})", (object) instructionRect.xMax, (object) instructionRect.yMax), BaseInspectView.Styles.centeredLabel);
      GUI.Box(position2, GUIContent.none);
    }

    protected void DoSelectableInstructionDataField(string label, string instructionData)
    {
      Rect controlRect = EditorGUILayout.GetControlRect(true, new GUILayoutOption[0]);
      EditorGUI.LabelField(controlRect, label);
      controlRect.xMin += EditorGUIUtility.labelWidth;
      EditorGUI.SelectableLabel(controlRect, instructionData);
    }

    internal abstract void DoDrawSelectedInstructionDetails(int selectedInstructionIndex);

    internal abstract string GetInstructionListName(int index);

    internal abstract void OnDoubleClickInstruction(int index);

    internal abstract void OnSelectedInstructionChanged(int newSelectionIndex);

    private void DoDrawNothingSelected()
    {
      EditorGUILayout.BeginVertical();
      GUILayout.FlexibleSpace();
      GUILayout.Label(BaseInspectView.Styles.emptyViewLabel, GUIViewDebuggerWindow.Styles.centeredText, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndVertical();
    }

    protected static class Styles
    {
      public static readonly GUIContent instructionsLabel = new GUIContent("Instructions");
      public static readonly GUIContent emptyViewLabel = new GUIContent("Select an Instruction on the left to see details");
      public static readonly GUIStyle centeredLabel = new GUIStyle((GUIStyle) "PR Label");

      static Styles()
      {
        BaseInspectView.Styles.centeredLabel.alignment = TextAnchor.MiddleCenter;
        BaseInspectView.Styles.centeredLabel.padding.right = 0;
        BaseInspectView.Styles.centeredLabel.padding.left = 0;
      }
    }
  }
}
