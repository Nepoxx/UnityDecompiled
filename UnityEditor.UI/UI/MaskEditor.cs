// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.MaskEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DDF3271B-B3CA-41C7-8FD7-FD00990E91BF
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\Editor\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the Mask component.</para>
  /// </summary>
  [CustomEditor(typeof (Mask), true)]
  [CanEditMultipleObjects]
  public class MaskEditor : Editor
  {
    private SerializedProperty m_ShowMaskGraphic;

    protected virtual void OnEnable()
    {
      this.m_ShowMaskGraphic = this.serializedObject.FindProperty("m_ShowMaskGraphic");
    }

    public override void OnInspectorGUI()
    {
      Graphic component = (this.target as Mask).GetComponent<Graphic>();
      if ((bool) ((Object) component) && !component.IsActive())
        EditorGUILayout.HelpBox("Masking disabled due to Graphic component being disabled.", MessageType.Warning);
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_ShowMaskGraphic);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
