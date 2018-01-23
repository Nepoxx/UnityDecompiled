// Decompiled with JetBrains decompiler
// Type: UnityEditor.DelayedDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomPropertyDrawer(typeof (DelayedAttribute))]
  internal sealed class DelayedDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      if (property.propertyType == SerializedPropertyType.Float)
        EditorGUI.DelayedFloatField(position, property, label);
      else if (property.propertyType == SerializedPropertyType.Integer)
        EditorGUI.DelayedIntField(position, property, label);
      else if (property.propertyType == SerializedPropertyType.String)
        EditorGUI.DelayedTextField(position, property, label);
      else
        EditorGUI.LabelField(position, label.text, "Use Delayed with float, int, or string.");
    }
  }
}
