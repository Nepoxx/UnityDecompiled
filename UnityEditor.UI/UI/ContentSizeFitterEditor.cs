// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.ContentSizeFitterEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DDF3271B-B3CA-41C7-8FD7-FD00990E91BF
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\Editor\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///         <para>Custom Editor for the ContentSizeFitter Component.
  /// </para>
  ///       </summary>
  [CustomEditor(typeof (ContentSizeFitter), true)]
  [CanEditMultipleObjects]
  public class ContentSizeFitterEditor : SelfControllerEditor
  {
    private SerializedProperty m_HorizontalFit;
    private SerializedProperty m_VerticalFit;

    protected virtual void OnEnable()
    {
      this.m_HorizontalFit = this.serializedObject.FindProperty("m_HorizontalFit");
      this.m_VerticalFit = this.serializedObject.FindProperty("m_VerticalFit");
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_HorizontalFit, true, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_VerticalFit, true, new GUILayoutOption[0]);
      this.serializedObject.ApplyModifiedProperties();
      base.OnInspectorGUI();
    }
  }
}
