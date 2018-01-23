// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.ToggleEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DDF3271B-B3CA-41C7-8FD7-FD00990E91BF
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\Editor\UnityEditor.UI.dll

using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the Toggle Component.</para>
  /// </summary>
  [CustomEditor(typeof (Toggle), true)]
  [CanEditMultipleObjects]
  public class ToggleEditor : SelectableEditor
  {
    private SerializedProperty m_OnValueChangedProperty;
    private SerializedProperty m_TransitionProperty;
    private SerializedProperty m_GraphicProperty;
    private SerializedProperty m_GroupProperty;
    private SerializedProperty m_IsOnProperty;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.m_TransitionProperty = this.serializedObject.FindProperty("toggleTransition");
      this.m_GraphicProperty = this.serializedObject.FindProperty("graphic");
      this.m_GroupProperty = this.serializedObject.FindProperty("m_Group");
      this.m_IsOnProperty = this.serializedObject.FindProperty("m_IsOn");
      this.m_OnValueChangedProperty = this.serializedObject.FindProperty("onValueChanged");
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      EditorGUILayout.Space();
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_IsOnProperty);
      EditorGUILayout.PropertyField(this.m_TransitionProperty);
      EditorGUILayout.PropertyField(this.m_GraphicProperty);
      EditorGUILayout.PropertyField(this.m_GroupProperty);
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_OnValueChangedProperty);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
