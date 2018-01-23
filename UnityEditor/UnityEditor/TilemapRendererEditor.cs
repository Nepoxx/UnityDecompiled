// Decompiled with JetBrains decompiler
// Type: UnityEditor.TilemapRendererEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (TilemapRenderer))]
  internal class TilemapRendererEditor : RendererEditorBase
  {
    private SerializedProperty m_Material;
    private SerializedProperty m_SortOrder;
    private SerializedProperty m_MaskInteraction;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Material = this.serializedObject.FindProperty("m_Materials.Array");
      this.m_SortOrder = this.serializedObject.FindProperty("m_SortOrder");
      this.m_MaskInteraction = this.serializedObject.FindProperty("m_MaskInteraction");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Material.GetArrayElementAtIndex(0), TilemapRendererEditor.Styles.materialLabel, true, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_SortOrder);
      this.RenderSortingLayerFields();
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_MaskInteraction);
      this.serializedObject.ApplyModifiedProperties();
    }

    private static class Styles
    {
      public static readonly GUIContent materialLabel = EditorGUIUtility.TextContent("Material");
    }
  }
}
