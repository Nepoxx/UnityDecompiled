// Decompiled with JetBrains decompiler
// Type: UnityEditor.GUIClipInspectView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class GUIClipInspectView : BaseInspectView
  {
    private Vector2 m_StacktraceScrollPos = new Vector2();
    private List<IMGUIClipInstruction> m_ClipList = new List<IMGUIClipInstruction>();

    public GUIClipInspectView(GUIViewDebuggerWindow guiViewDebuggerWindow)
      : base(guiViewDebuggerWindow)
    {
    }

    public override void UpdateInstructions()
    {
      this.m_ClipList.Clear();
      GUIViewDebuggerHelper.GetClipInstructions(this.m_ClipList);
    }

    public override void ShowOverlay()
    {
      if (!this.isInstructionSelected)
        this.debuggerWindow.ClearInstructionHighlighter();
      else
        this.debuggerWindow.HighlightInstruction(this.debuggerWindow.inspected, this.m_ClipList[this.listViewState.row].unclippedScreenRect, GUIStyle.none);
    }

    protected override int GetInstructionCount()
    {
      return this.m_ClipList.Count;
    }

    protected override void DoDrawInstruction(ListViewElement el, int id)
    {
      IMGUIClipInstruction clip = this.m_ClipList[el.row];
      GUIContent content = GUIContent.Temp(this.GetInstructionListName(el.row));
      Rect position = el.position;
      position.xMin += (float) (clip.level * 12);
      GUIViewDebuggerWindow.Styles.listItemBackground.Draw(el.position, false, false, this.listViewState.row == el.row, false);
      GUIViewDebuggerWindow.Styles.listItem.Draw(position, content, id, this.listViewState.row == el.row);
    }

    protected override void DrawInspectedStacktrace()
    {
      IMGUIClipInstruction clip = this.m_ClipList[this.listViewState.row];
      this.m_StacktraceScrollPos = EditorGUILayout.BeginScrollView(this.m_StacktraceScrollPos, GUIViewDebuggerWindow.Styles.stacktraceBackground, GUILayout.ExpandHeight(false));
      this.DrawStackFrameList(clip.pushStacktrace);
      EditorGUILayout.EndScrollView();
    }

    internal override void DoDrawSelectedInstructionDetails(int selectedInstructionIndex)
    {
      IMGUIClipInstruction clip = this.m_ClipList[selectedInstructionIndex];
      this.DoSelectableInstructionDataField("RenderOffset", clip.renderOffset.ToString());
      this.DoSelectableInstructionDataField("ResetOffset", clip.resetOffset.ToString());
      this.DoSelectableInstructionDataField("screenRect", clip.screenRect.ToString());
      this.DoSelectableInstructionDataField("scrollOffset", clip.scrollOffset.ToString());
    }

    internal override string GetInstructionListName(int index)
    {
      StackFrame[] pushStacktrace = this.m_ClipList[index].pushStacktrace;
      if (pushStacktrace.Length == 0)
        return "Empty";
      int interestingFrameIndex = this.GetInterestingFrameIndex(pushStacktrace);
      return pushStacktrace[interestingFrameIndex].methodName;
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
        if (!string.IsNullOrEmpty(stackFrame.sourceFile) && !stackFrame.signature.StartsWith("UnityEngine.GUIClip"))
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
