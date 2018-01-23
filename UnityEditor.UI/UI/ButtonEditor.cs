// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.ButtonEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DDF3271B-B3CA-41C7-8FD7-FD00990E91BF
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\Editor\UnityEditor.UI.dll

using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the Button Component.</para>
  /// </summary>
  [CustomEditor(typeof (Button), true)]
  [CanEditMultipleObjects]
  public class ButtonEditor : SelectableEditor
  {
    private SerializedProperty m_OnClickProperty;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.m_OnClickProperty = this.serializedObject.FindProperty("m_OnClick");
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      EditorGUILayout.Space();
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_OnClickProperty);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
