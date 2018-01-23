// Decompiled with JetBrains decompiler
// Type: UnityEditor.UnifiedInspectView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class UnifiedInspectView : BaseInspectView
  {
    private Vector2 m_StacktraceScrollPos = new Vector2();
    private readonly List<IMGUIInstruction> m_Instructions = new List<IMGUIInstruction>();
    private BaseInspectView m_InstructionClipView;
    private BaseInspectView m_InstructionStyleView;
    private BaseInspectView m_InstructionPropertyView;
    private BaseInspectView m_InstructionLayoutView;
    private BaseInspectView m_InstructionNamedControlView;

    public UnifiedInspectView(GUIViewDebuggerWindow guiViewDebuggerWindow)
      : base(guiViewDebuggerWindow)
    {
      this.m_InstructionClipView = (BaseInspectView) new GUIClipInspectView(guiViewDebuggerWindow);
      this.m_InstructionStyleView = (BaseInspectView) new StyleDrawInspectView(guiViewDebuggerWindow);
      this.m_InstructionLayoutView = (BaseInspectView) new GUILayoutInspectView(guiViewDebuggerWindow);
      this.m_InstructionPropertyView = (BaseInspectView) new GUIPropertyInspectView(guiViewDebuggerWindow);
      this.m_InstructionNamedControlView = (BaseInspectView) new GUINamedControlInspectView(guiViewDebuggerWindow);
    }

    public override void UpdateInstructions()
    {
      this.m_InstructionClipView.UpdateInstructions();
      this.m_InstructionStyleView.UpdateInstructions();
      this.m_InstructionLayoutView.UpdateInstructions();
      this.m_InstructionPropertyView.UpdateInstructions();
      this.m_InstructionNamedControlView.UpdateInstructions();
      this.m_Instructions.Clear();
      GUIViewDebuggerHelper.GetUnifiedInstructions(this.m_Instructions);
    }

    public override void ShowOverlay()
    {
      if (!this.isInstructionSelected)
        this.debuggerWindow.ClearInstructionHighlighter();
      else
        this.GetInspectViewForType(this.m_Instructions[this.listViewState.row].type).ShowOverlay();
    }

    protected override int GetInstructionCount()
    {
      return this.m_Instructions.Count;
    }

    protected override void DoDrawInstruction(ListViewElement el, int controlId)
    {
      IMGUIInstruction instruction = this.m_Instructions[el.row];
      GUIContent content = GUIContent.Temp(this.GetInstructionListName(el.row));
      Rect position = el.position;
      position.xMin += (float) (instruction.level * 10);
      GUIViewDebuggerWindow.Styles.listItemBackground.Draw(position, false, false, this.listViewState.row == el.row, false);
      GUIViewDebuggerWindow.Styles.listItem.Draw(position, content, controlId, this.listViewState.row == el.row);
    }

    protected override void DrawInspectedStacktrace()
    {
      IMGUIInstruction instruction = this.m_Instructions[this.listViewState.row];
      this.m_StacktraceScrollPos = EditorGUILayout.BeginScrollView(this.m_StacktraceScrollPos, GUIViewDebuggerWindow.Styles.stacktraceBackground, GUILayout.ExpandHeight(false));
      this.DrawStackFrameList(instruction.stack);
      EditorGUILayout.EndScrollView();
    }

    internal override void DoDrawSelectedInstructionDetails(int selectedInstructionIndex)
    {
      IMGUIInstruction instruction = this.m_Instructions[selectedInstructionIndex];
      this.GetInspectViewForType(instruction.type).DoDrawSelectedInstructionDetails(instruction.typeInstructionIndex);
    }

    internal override string GetInstructionListName(int index)
    {
      IMGUIInstruction instruction = this.m_Instructions[index];
      return this.GetInspectViewForType(instruction.type).GetInstructionListName(instruction.typeInstructionIndex);
    }

    internal override void OnSelectedInstructionChanged(int index)
    {
      this.listViewState.row = index;
      if (this.listViewState.row >= 0)
      {
        IMGUIInstruction instruction = this.m_Instructions[this.listViewState.row];
        this.GetInspectViewForType(instruction.type).OnSelectedInstructionChanged(instruction.typeInstructionIndex);
        this.ShowOverlay();
      }
      else
        this.debuggerWindow.ClearInstructionHighlighter();
    }

    internal override void OnDoubleClickInstruction(int index)
    {
      IMGUIInstruction instruction = this.m_Instructions[index];
      this.GetInspectViewForType(instruction.type).OnDoubleClickInstruction(instruction.typeInstructionIndex);
    }

    private BaseInspectView GetInspectViewForType(InstructionType type)
    {
      switch (type)
      {
        case InstructionType.kStyleDraw:
          return this.m_InstructionStyleView;
        case InstructionType.kClipPush:
        case InstructionType.kClipPop:
          return this.m_InstructionClipView;
        case InstructionType.kLayoutBeginGroup:
        case InstructionType.kLayoutEndGroup:
        case InstructionType.kLayoutEntry:
          return this.m_InstructionLayoutView;
        case InstructionType.kPropertyBegin:
        case InstructionType.kPropertyEnd:
          return this.m_InstructionPropertyView;
        case InstructionType.kLayoutNamedControl:
          return this.m_InstructionNamedControlView;
        default:
          throw new NotImplementedException("Unhandled InstructionType");
      }
    }
  }
}
