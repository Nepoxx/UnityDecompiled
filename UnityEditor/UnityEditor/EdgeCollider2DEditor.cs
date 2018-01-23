// Decompiled with JetBrains decompiler
// Type: UnityEditor.EdgeCollider2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (EdgeCollider2D))]
  internal class EdgeCollider2DEditor : Collider2DEditorBase
  {
    private PolygonEditorUtility m_PolyUtility = new PolygonEditorUtility();
    private SerializedProperty m_EdgeRadius;
    private SerializedProperty m_Points;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_EdgeRadius = this.serializedObject.FindProperty("m_EdgeRadius");
      this.m_Points = this.serializedObject.FindProperty("m_Points");
      this.m_Points.isExpanded = false;
    }

    public override void OnInspectorGUI()
    {
      this.BeginColliderInspector();
      base.OnInspectorGUI();
      EditorGUILayout.PropertyField(this.m_EdgeRadius);
      if (this.targets.Length == 1)
      {
        EditorGUI.BeginDisabledGroup(this.editingCollider);
        EditorGUILayout.PropertyField(this.m_Points, true, new GUILayoutOption[0]);
        EditorGUI.EndDisabledGroup();
      }
      this.EndColliderInspector();
      this.FinalizeInspectorGUI();
    }

    protected override void OnEditStart()
    {
      this.m_PolyUtility.StartEditing(this.target as Collider2D);
    }

    protected override void OnEditEnd()
    {
      this.m_PolyUtility.StopEditing();
    }

    public void OnSceneGUI()
    {
      if (!this.editingCollider)
        return;
      this.m_PolyUtility.OnSceneGUI();
    }
  }
}
