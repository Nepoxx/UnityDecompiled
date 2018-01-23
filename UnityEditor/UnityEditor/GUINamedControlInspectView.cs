// Decompiled with JetBrains decompiler
// Type: UnityEditor.GUINamedControlInspectView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class GUINamedControlInspectView : BaseInspectView
  {
    private readonly List<IMGUINamedControlInstruction> m_NamedControlInstructions = new List<IMGUINamedControlInstruction>();
    private GUIStyle m_FakeMargingStyleForOverlay = new GUIStyle();

    public GUINamedControlInspectView(GUIViewDebuggerWindow guiViewDebuggerWindow)
      : base(guiViewDebuggerWindow)
    {
    }

    public override void UpdateInstructions()
    {
      this.m_NamedControlInstructions.Clear();
      GUIViewDebuggerHelper.GetNamedControlInstructions(this.m_NamedControlInstructions);
    }

    protected override int GetInstructionCount()
    {
      return this.m_NamedControlInstructions.Count;
    }

    protected override void DoDrawInstruction(ListViewElement el, int id)
    {
      GUIContent content = GUIContent.Temp(this.GetInstructionListName(el.row));
      Rect position = el.position;
      GUIViewDebuggerWindow.Styles.listItemBackground.Draw(position, false, false, this.listViewState.row == el.row, false);
      GUIViewDebuggerWindow.Styles.listItem.Draw(position, content, id, this.listViewState.row == el.row);
    }

    internal override string GetInstructionListName(int index)
    {
      return "\"" + this.m_NamedControlInstructions[index].name + "\"";
    }

    internal override void OnDoubleClickInstruction(int index)
    {
    }

    protected override void DrawInspectedStacktrace()
    {
    }

    internal override void DoDrawSelectedInstructionDetails(int index)
    {
      IMGUINamedControlInstruction controlInstruction = this.m_NamedControlInstructions[this.listViewState.row];
      using (new EditorGUI.DisabledScope(true))
        this.DrawInspectedRect(controlInstruction.rect);
      this.DoSelectableInstructionDataField("Name", controlInstruction.name);
      this.DoSelectableInstructionDataField("ID", controlInstruction.id.ToString());
    }

    internal override void OnSelectedInstructionChanged(int index)
    {
      this.listViewState.row = index;
      this.ShowOverlay();
    }

    public override void ShowOverlay()
    {
      if (!this.isInstructionSelected)
        this.debuggerWindow.ClearInstructionHighlighter();
      else
        this.debuggerWindow.HighlightInstruction(this.debuggerWindow.inspected, this.m_NamedControlInstructions[this.listViewState.row].rect, this.m_FakeMargingStyleForOverlay);
    }
  }
}
