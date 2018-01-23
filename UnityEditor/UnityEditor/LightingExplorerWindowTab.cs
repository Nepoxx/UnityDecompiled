// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightingExplorerWindowTab
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class LightingExplorerWindowTab
  {
    private SerializedPropertyTable m_LightTable;

    public LightingExplorerWindowTab(SerializedPropertyTable lightTable)
    {
      this.m_LightTable = lightTable;
    }

    public void OnEnable()
    {
      if (this.m_LightTable == null)
        return;
      this.m_LightTable.OnEnable();
    }

    public void OnDisable()
    {
      if (this.m_LightTable == null)
        return;
      this.m_LightTable.OnDisable();
    }

    public void OnInspectorUpdate()
    {
      if (this.m_LightTable == null)
        return;
      this.m_LightTable.OnInspectorUpdate();
    }

    public void OnSelectionChange(int[] instanceIDs)
    {
      if (this.m_LightTable == null)
        return;
      this.m_LightTable.OnSelectionChange(instanceIDs);
    }

    public void OnSelectionChange()
    {
      if (this.m_LightTable == null)
        return;
      this.m_LightTable.OnSelectionChange();
    }

    public void OnHierarchyChange()
    {
      if (this.m_LightTable == null)
        return;
      this.m_LightTable.OnHierarchyChange();
    }

    public void OnGUI()
    {
      ++EditorGUI.indentLevel;
      int indentLevel = EditorGUI.indentLevel;
      float indent = EditorGUI.indent;
      EditorGUI.indentLevel = 0;
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(indent);
      using (new EditorGUILayout.VerticalScope(new GUILayoutOption[0]))
      {
        if (this.m_LightTable != null)
          this.m_LightTable.OnGUI();
      }
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();
      EditorGUI.indentLevel = indentLevel;
      --EditorGUI.indentLevel;
    }
  }
}
