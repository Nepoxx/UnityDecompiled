// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collider3DEditorBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class Collider3DEditorBase : ColliderEditorBase
  {
    protected GUIContent materialContent = EditorGUIUtility.TextContent("Material|Reference to the Physic Material that determines how this Collider interacts with others.");
    protected GUIContent triggerContent = EditorGUIUtility.TextContent("Is Trigger|If enabled, this Collider is used for triggering events and is ignored by the physics engine.");
    protected SerializedProperty m_Material;
    protected SerializedProperty m_IsTrigger;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Material = this.serializedObject.FindProperty("m_Material");
      this.m_IsTrigger = this.serializedObject.FindProperty("m_IsTrigger");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_IsTrigger, this.triggerContent, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Material, this.materialContent, new GUILayoutOption[0]);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
