// Decompiled with JetBrains decompiler
// Type: UnityEditor.MultilineDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomPropertyDrawer(typeof (MultilineAttribute))]
  internal sealed class MultilineDrawer : PropertyDrawer
  {
    private const int kLineHeight = 13;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      if (property.propertyType == SerializedPropertyType.String)
      {
        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.MultiFieldPrefixLabel(position, 0, label, 1);
        EditorGUI.BeginChangeCheck();
        int indentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        string str = EditorGUI.TextArea(position, property.stringValue);
        EditorGUI.indentLevel = indentLevel;
        if (EditorGUI.EndChangeCheck())
          property.stringValue = str;
        EditorGUI.EndProperty();
      }
      else
        EditorGUI.LabelField(position, label.text, "Use Multiline with string.");
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      return (float) ((!EditorGUIUtility.wideMode ? 16.0 : 0.0) + 16.0) + (float) ((((MultilineAttribute) this.attribute).lines - 1) * 13);
    }
  }
}
