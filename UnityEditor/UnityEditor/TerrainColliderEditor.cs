// Decompiled with JetBrains decompiler
// Type: UnityEditor.TerrainColliderEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (TerrainCollider))]
  [CanEditMultipleObjects]
  internal class TerrainColliderEditor : Collider3DEditorBase
  {
    protected GUIContent terrainContent = EditorGUIUtility.TextContent("Terrain Data|The TerrainData asset that stores heightmaps, terrain textures, detail meshes and trees.");
    protected GUIContent treeColliderContent = EditorGUIUtility.TextContent("Enable Tree Colliders|When selected, Tree Colliders will be enabled.");
    private SerializedProperty m_TerrainData;
    private SerializedProperty m_EnableTreeColliders;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_TerrainData = this.serializedObject.FindProperty("m_TerrainData");
      this.m_EnableTreeColliders = this.serializedObject.FindProperty("m_EnableTreeColliders");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Material, this.materialContent, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_TerrainData, this.terrainContent, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_EnableTreeColliders, this.treeColliderContent, new GUILayoutOption[0]);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
