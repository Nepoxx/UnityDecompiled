// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.ColorBlockDrawer
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DDF3271B-B3CA-41C7-8FD7-FD00990E91BF
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\Editor\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>PropertyDrawer for ColorBlock.</para>
  /// </summary>
  [CustomPropertyDrawer(typeof (ColorBlock), true)]
  public class ColorBlockDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
    {
      Rect position = rect;
      position.height = EditorGUIUtility.singleLineHeight;
      SerializedProperty propertyRelative1 = prop.FindPropertyRelative("m_NormalColor");
      SerializedProperty propertyRelative2 = prop.FindPropertyRelative("m_HighlightedColor");
      SerializedProperty propertyRelative3 = prop.FindPropertyRelative("m_PressedColor");
      SerializedProperty propertyRelative4 = prop.FindPropertyRelative("m_DisabledColor");
      SerializedProperty propertyRelative5 = prop.FindPropertyRelative("m_ColorMultiplier");
      SerializedProperty propertyRelative6 = prop.FindPropertyRelative("m_FadeDuration");
      EditorGUI.PropertyField(position, propertyRelative1);
      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      EditorGUI.PropertyField(position, propertyRelative2);
      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      EditorGUI.PropertyField(position, propertyRelative3);
      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      EditorGUI.PropertyField(position, propertyRelative4);
      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      EditorGUI.PropertyField(position, propertyRelative5);
      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      EditorGUI.PropertyField(position, propertyRelative6);
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
      return (float) (6.0 * (double) EditorGUIUtility.singleLineHeight + 5.0 * (double) EditorGUIUtility.standardVerticalSpacing);
    }
  }
}
