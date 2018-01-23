// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (Grid))]
  [CanEditMultipleObjects]
  internal class GridEditor : Editor
  {
    private SerializedProperty m_CellSize;
    private SerializedProperty m_CellGap;
    private SerializedProperty m_CellSwizzle;

    private void OnEnable()
    {
      this.m_CellSize = this.serializedObject.FindProperty("m_CellSize");
      this.m_CellGap = this.serializedObject.FindProperty("m_CellGap");
      this.m_CellSwizzle = this.serializedObject.FindProperty("m_CellSwizzle");
      SceneViewGridManager.FlushCachedGridProxy();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_CellSize);
      EditorGUILayout.PropertyField(this.m_CellGap);
      EditorGUILayout.PropertyField(this.m_CellSwizzle);
      if (!this.serializedObject.ApplyModifiedProperties())
        return;
      SceneViewGridManager.FlushCachedGridProxy();
    }
  }
}
