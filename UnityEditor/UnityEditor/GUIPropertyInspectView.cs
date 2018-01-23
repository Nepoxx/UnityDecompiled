// Decompiled with JetBrains decompiler
// Type: UnityEditor.GUIPropertyInspectView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class GUIPropertyInspectView : BaseInspectView
  {
    private Vector2 m_StacktraceScrollPos = new Vector2();
    private GUIStyle m_FakeMargingStyleForOverlay = new GUIStyle();
    private List<IMGUIPropertyInstruction> m_PropertyList = new List<IMGUIPropertyInstruction>();

    public GUIPropertyInspectView(GUIViewDebuggerWindow guiViewDebuggerWindow)
      : base(guiViewDebuggerWindow)
    {
    }

    public override void UpdateInstructions()
    {
      this.m_PropertyList.Clear();
      GUIViewDebuggerHelper.GetPropertyInstructions(this.m_PropertyList);
    }

    public override void ShowOverlay()
    {
      if (!this.isInstructionSelected)
        this.debuggerWindow.ClearInstructionHighlighter();
      else
        this.debuggerWindow.HighlightInstruction(this.debuggerWindow.inspected, this.m_PropertyList[this.listViewState.row].rect, this.m_FakeMargingStyleForOverlay);
    }

    protected override int GetInstructionCount()
    {
      return this.m_PropertyList.Count;
    }

    protected override void DoDrawInstruction(ListViewElement el, int id)
    {
      GUIContent content = GUIContent.Temp(this.GetInstructionListName(el.row));
      Rect position = el.position;
      GUIViewDebuggerWindow.Styles.listItemBackground.Draw(position, false, false, this.listViewState.row == el.row, false);
      GUIViewDebuggerWindow.Styles.listItem.Draw(position, content, id, this.listViewState.row == el.row);
    }

    protected override void DrawInspectedStacktrace()
    {
      IMGUIPropertyInstruction property = this.m_PropertyList[this.listViewState.row];
      this.m_StacktraceScrollPos = EditorGUILayout.BeginScrollView(this.m_StacktraceScrollPos, GUIViewDebuggerWindow.Styles.stacktraceBackground, GUILayout.ExpandHeight(false));
      this.DrawStackFrameList(property.beginStacktrace);
      EditorGUILayout.EndScrollView();
    }

    internal override void DoDrawSelectedInstructionDetails(int selectedInstructionIndex)
    {
      IMGUIPropertyInstruction property = this.m_PropertyList[this.listViewState.row];
      using (new EditorGUI.DisabledScope(true))
        this.DrawInspectedRect(property.rect);
      this.DoSelectableInstructionDataField("Target Type Name", property.targetTypeName);
      this.DoSelectableInstructionDataField("Path", property.path);
    }

    internal override string GetInstructionListName(int index)
    {
      return this.m_PropertyList[index].path;
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
  }
}
