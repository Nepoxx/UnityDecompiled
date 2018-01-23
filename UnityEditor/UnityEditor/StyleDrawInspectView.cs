// Decompiled with JetBrains decompiler
// Type: UnityEditor.StyleDrawInspectView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class StyleDrawInspectView : BaseInspectView
  {
    private Vector2 m_StacktraceScrollPos = new Vector2();
    [NonSerialized]
    private List<IMGUIDrawInstruction> m_Instructions = new List<IMGUIDrawInstruction>();
    [NonSerialized]
    private IMGUIDrawInstruction m_Instruction;
    [NonSerialized]
    private StyleDrawInspectView.CachedInstructionInfo m_CachedInstructionInfo;

    public StyleDrawInspectView(GUIViewDebuggerWindow guiViewDebuggerWindow)
      : base(guiViewDebuggerWindow)
    {
    }

    public override void UpdateInstructions()
    {
      this.m_Instructions.Clear();
      GUIViewDebuggerHelper.GetDrawInstructions(this.m_Instructions);
    }

    public override void ClearRowSelection()
    {
      base.ClearRowSelection();
      this.m_CachedInstructionInfo = (StyleDrawInspectView.CachedInstructionInfo) null;
    }

    public override void ShowOverlay()
    {
      if (this.m_CachedInstructionInfo == null)
        return;
      this.debuggerWindow.HighlightInstruction(this.debuggerWindow.inspected, this.m_Instruction.rect, this.m_Instruction.usedGUIStyle);
    }

    protected override int GetInstructionCount()
    {
      return this.m_Instructions.Count;
    }

    protected override void DoDrawInstruction(ListViewElement el, int id)
    {
      GUIContent content = GUIContent.Temp(this.GetInstructionListName(el.row));
      GUIViewDebuggerWindow.Styles.listItemBackground.Draw(el.position, false, false, this.listViewState.row == el.row, false);
      GUIViewDebuggerWindow.Styles.listItem.Draw(el.position, content, id, this.listViewState.row == el.row);
    }

    protected override void DrawInspectedStacktrace()
    {
      this.m_StacktraceScrollPos = EditorGUILayout.BeginScrollView(this.m_StacktraceScrollPos, GUIViewDebuggerWindow.Styles.stacktraceBackground, GUILayout.ExpandHeight(false));
      this.DrawStackFrameList(this.m_Instruction.stackframes);
      EditorGUILayout.EndScrollView();
    }

    protected override bool isInstructionSelected
    {
      get
      {
        return this.m_CachedInstructionInfo != null;
      }
    }

    internal override void DoDrawSelectedInstructionDetails(int selectedInstructionIndex)
    {
      using (new EditorGUI.DisabledScope(true))
        this.DrawInspectedRect(this.m_Instruction.rect);
      this.DoSelectableInstructionDataField("VisibleRect", this.m_Instruction.visibleRect.ToString());
      EditorGUILayout.Space();
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_CachedInstructionInfo.styleSerializedProperty, GUIContent.Temp("Style"), true, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_CachedInstructionInfo.styleContainerSerializedObject.ApplyModifiedPropertiesWithoutUndo();
        this.debuggerWindow.inspected.Repaint();
      }
      GUILayout.Label(GUIContent.Temp("GUIContent"));
      using (new EditorGUI.IndentLevelScope())
      {
        this.DoSelectableInstructionDataField("Text", this.m_Instruction.usedGUIContent.text);
        this.DoSelectableInstructionDataField("Tooltip", this.m_Instruction.usedGUIContent.tooltip);
        using (new EditorGUI.DisabledScope(true))
          EditorGUILayout.ObjectField("Icon", (UnityEngine.Object) this.m_Instruction.usedGUIContent.image, typeof (Texture2D), false, new GUILayoutOption[0]);
      }
    }

    internal override string GetInstructionListName(int index)
    {
      string instructionListName = this.GetInstructionListName(this.m_Instructions[index].stackframes);
      return string.Format("{0}. {1}", (object) index, (object) instructionListName);
    }

    private string GetInstructionListName(StackFrame[] stacktrace)
    {
      int interestingFrameIndex = this.GetInterestingFrameIndex(stacktrace);
      if (interestingFrameIndex > 0)
        --interestingFrameIndex;
      return stacktrace[interestingFrameIndex].methodName;
    }

    internal override void OnDoubleClickInstruction(int index)
    {
      this.ShowInstructionInExternalEditor(this.m_Instructions[index].stackframes);
    }

    internal override void OnSelectedInstructionChanged(int index)
    {
      this.listViewState.row = index;
      if (this.listViewState.row >= 0)
      {
        if (this.m_CachedInstructionInfo == null)
          this.m_CachedInstructionInfo = new StyleDrawInspectView.CachedInstructionInfo();
        this.m_Instruction = this.m_Instructions[this.listViewState.row];
        this.m_CachedInstructionInfo.styleContainer.inspectedStyle = this.m_Instruction.usedGUIStyle;
        this.m_CachedInstructionInfo.styleContainerSerializedObject = (SerializedObject) null;
        this.m_CachedInstructionInfo.styleSerializedProperty = (SerializedProperty) null;
        this.GetSelectedStyleProperty(out this.m_CachedInstructionInfo.styleContainerSerializedObject, out this.m_CachedInstructionInfo.styleSerializedProperty);
        this.debuggerWindow.HighlightInstruction(this.debuggerWindow.inspected, this.m_Instruction.rect, this.m_Instruction.usedGUIStyle);
      }
      else
      {
        this.m_CachedInstructionInfo = (StyleDrawInspectView.CachedInstructionInfo) null;
        this.debuggerWindow.ClearInstructionHighlighter();
      }
    }

    private int GetInterestingFrameIndex(StackFrame[] stacktrace)
    {
      string dataPath = Application.dataPath;
      int num = -1;
      for (int index = 0; index < stacktrace.Length; ++index)
      {
        StackFrame stackFrame = stacktrace[index];
        if (!string.IsNullOrEmpty(stackFrame.sourceFile) && !stackFrame.signature.StartsWith("UnityEngine.GUI") && !stackFrame.signature.StartsWith("UnityEditor.EditorGUI"))
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

    private void GetSelectedStyleProperty(out SerializedObject serializedObject, out SerializedProperty styleProperty)
    {
      GUISkin guiSkin = (GUISkin) null;
      GUISkin current = GUISkin.current;
      GUIStyle style = current.FindStyle(this.m_Instruction.usedGUIStyle.name);
      if (style != null && style == this.m_Instruction.usedGUIStyle)
        guiSkin = current;
      styleProperty = (SerializedProperty) null;
      if ((UnityEngine.Object) guiSkin != (UnityEngine.Object) null)
      {
        serializedObject = new SerializedObject((UnityEngine.Object) guiSkin);
        SerializedProperty iterator = serializedObject.GetIterator();
        bool enterChildren = true;
        while (iterator.NextVisible(enterChildren))
        {
          if (iterator.type == "GUIStyle")
          {
            enterChildren = false;
            if (iterator.FindPropertyRelative("m_Name").stringValue == this.m_Instruction.usedGUIStyle.name)
            {
              styleProperty = iterator;
              return;
            }
          }
          else
            enterChildren = true;
        }
        Debug.Log((object) string.Format("Showing editable Style from GUISkin: {0}, IsPersistant: {1}", (object) guiSkin.name, (object) EditorUtility.IsPersistent((UnityEngine.Object) guiSkin)));
      }
      serializedObject = new SerializedObject((UnityEngine.Object) this.m_CachedInstructionInfo.styleContainer);
      styleProperty = serializedObject.FindProperty("inspectedStyle");
    }

    private void ShowInstructionInExternalEditor(StackFrame[] frames)
    {
      int interestingFrameIndex = this.GetInterestingFrameIndex(frames);
      StackFrame frame = frames[interestingFrameIndex];
      InternalEditorUtility.OpenFileAtLineExternal(frame.sourceFile, (int) frame.lineNumber);
    }

    [Serializable]
    private class CachedInstructionInfo
    {
      public SerializedObject styleContainerSerializedObject = (SerializedObject) null;
      public SerializedProperty styleSerializedProperty = (SerializedProperty) null;
      public readonly GUIStyleHolder styleContainer;

      public CachedInstructionInfo()
      {
        this.styleContainer = ScriptableObject.CreateInstance<GUIStyleHolder>();
      }
    }
  }
}
