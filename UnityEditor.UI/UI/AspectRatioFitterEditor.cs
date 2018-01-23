// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.AspectRatioFitterEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DDF3271B-B3CA-41C7-8FD7-FD00990E91BF
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\Editor\UnityEditor.UI.dll

using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the AspectRatioFitter component.</para>
  /// </summary>
  [CustomEditor(typeof (AspectRatioFitter), true)]
  [CanEditMultipleObjects]
  public class AspectRatioFitterEditor : SelfControllerEditor
  {
    private SerializedProperty m_AspectMode;
    private SerializedProperty m_AspectRatio;

    protected virtual void OnEnable()
    {
      this.m_AspectMode = this.serializedObject.FindProperty("m_AspectMode");
      this.m_AspectRatio = this.serializedObject.FindProperty("m_AspectRatio");
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_AspectMode);
      EditorGUILayout.PropertyField(this.m_AspectRatio);
      this.serializedObject.ApplyModifiedProperties();
      base.OnInspectorGUI();
    }
  }
}
