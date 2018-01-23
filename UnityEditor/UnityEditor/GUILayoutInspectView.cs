// Decompiled with JetBrains decompiler
// Type: UnityEditor.GUILayoutInspectView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class GUILayoutInspectView : BaseInspectView
  {
    private Vector2 m_StacktraceScrollPos = new Vector2();
    private readonly List<IMGUILayoutInstruction> m_LayoutInstructions = new List<IMGUILayoutInstruction>();
    private GUIStyle m_FakeMarginStyleForOverlay = new GUIStyle();

    public GUILayoutInspectView(GUIViewDebuggerWindow guiViewDebuggerWindow)
      : base(guiViewDebuggerWindow)
    {
    }

    public override void UpdateInstructions()
    {
      this.m_LayoutInstructions.Clear();
      GUIViewDebuggerHelper.GetLayoutInstructions(this.m_LayoutInstructions);
    }

    public override void ShowOverlay()
    {
      if (!this.isInstructionSelected)
      {
        this.debuggerWindow.ClearInstructionHighlighter();
      }
      else
      {
        IMGUILayoutInstruction layoutInstruction = this.m_LayoutInstructions[this.listViewState.row];
        RectOffset rectOffset = new RectOffset();
        rectOffset.left = layoutInstruction.marginLeft;
        rectOffset.right = layoutInstruction.marginRight;
        rectOffset.top = layoutInstruction.marginTop;
        rectOffset.bottom = layoutInstruction.marginBottom;
        this.m_FakeMarginStyleForOverlay.padding = rectOffset;
        Rect unclippedRect = layoutInstruction.unclippedRect;
        this.debuggerWindow.HighlightInstruction(this.debuggerWindow.inspected, rectOffset.Add(unclippedRect), this.m_FakeMarginStyleForOverlay);
      }
    }

    protected override int GetInstructionCount()
    {
      return this.m_LayoutInstructions.Count;
    }

    protected override void DoDrawInstruction(ListViewElement el, int id)
    {
      IMGUILayoutInstruction layoutInstruction = this.m_LayoutInstructions[el.row];
      GUIContent content = GUIContent.Temp(this.GetInstructionListName(el.row));
      Rect position = el.position;
      position.xMin += (float) (layoutInstruction.level * 10);
      GUIViewDebuggerWindow.Styles.listItemBackground.Draw(position, false, false, this.listViewState.row == el.row, false);
      GUIViewDebuggerWindow.Styles.listItem.Draw(position, content, id, this.listViewState.row == el.row);
    }

    protected override void DrawInspectedStacktrace()
    {
      IMGUILayoutInstruction layoutInstruction = this.m_LayoutInstructions[this.listViewState.row];
      this.m_StacktraceScrollPos = EditorGUILayout.BeginScrollView(this.m_StacktraceScrollPos, GUIViewDebuggerWindow.Styles.stacktraceBackground, GUILayout.ExpandHeight(false));
      this.DrawStackFrameList(layoutInstruction.stack);
      EditorGUILayout.EndScrollView();
    }

    internal override void DoDrawSelectedInstructionDetails(int selectedInstructionIndex)
    {
      IMGUILayoutInstruction layoutInstruction = this.m_LayoutInstructions[selectedInstructionIndex];
      using (new EditorGUI.DisabledScope(true))
        this.DrawInspectedRect(layoutInstruction.unclippedRect);
      this.DoSelectableInstructionDataField("margin.left", layoutInstruction.marginLeft.ToString());
      this.DoSelectableInstructionDataField("margin.top", layoutInstruction.marginTop.ToString());
      this.DoSelectableInstructionDataField("margin.right", layoutInstruction.marginRight.ToString());
      this.DoSelectableInstructionDataField("margin.bottom", layoutInstruction.marginBottom.ToString());
      if (layoutInstruction.style != null)
        this.DoSelectableInstructionDataField("Style Name", layoutInstruction.style.name);
      if (layoutInstruction.isGroup == 1)
        return;
      this.DoSelectableInstructionDataField("IsVertical", (layoutInstruction.isVertical == 1).ToString());
    }

    internal override string GetInstructionListName(int index)
    {
      StackFrame[] stack = this.m_LayoutInstructions[index].stack;
      int interestingFrameIndex = this.GetInterestingFrameIndex(stack);
      if (interestingFrameIndex > 0)
        --interestingFrameIndex;
      return stack[interestingFrameIndex].methodName;
    }

    internal override void OnDoubleClickInstruction(int index)
    {
      throw new NotImplementedException();
    }

    internal override void OnSelectedInstructionChanged(int index)
    {
      this.listViewState.row = index;
      this.ShowOverlay();
    }

    private int GetInterestingFrameIndex(StackFrame[] stacktrace)
    {
      string dataPath = Application.dataPath;
      int num = -1;
      for (int index = 0; index < stacktrace.Length; ++index)
      {
        StackFrame stackFrame = stacktrace[index];
        if (!string.IsNullOrEmpty(stackFrame.sourceFile) && !stackFrame.signature.StartsWith("UnityEngine.GUIDebugger") && !stackFrame.signature.StartsWith("UnityEngine.GUILayoutUtility"))
        {
          if (num == -1)
            num = index;
          if (stackFrame.sourceFile.StartsWith(dataPath))
            return index;
        }
      }
      if (num != -1)
        return num;
      return stacktrace.Length - 1;
    }
  }
}
