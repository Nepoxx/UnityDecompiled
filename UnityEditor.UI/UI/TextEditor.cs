// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.TextEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DDF3271B-B3CA-41C7-8FD7-FD00990E91BF
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\Editor\UnityEditor.UI.dll

using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the Text Component.</para>
  /// </summary>
  [CustomEditor(typeof (Text), true)]
  [CanEditMultipleObjects]
  public class TextEditor : GraphicEditor
  {
    private SerializedProperty m_Text;
    private SerializedProperty m_FontData;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.m_Text = this.serializedObject.FindProperty("m_Text");
      this.m_FontData = this.serializedObject.FindProperty("m_FontData");
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Text);
      EditorGUILayout.PropertyField(this.m_FontData);
      this.AppearanceControlsGUI();
      this.RaycastControlsGUI();
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
