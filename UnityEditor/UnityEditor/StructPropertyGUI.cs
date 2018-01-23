// Decompiled with JetBrains decompiler
// Type: UnityEditor.StructPropertyGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class StructPropertyGUI
  {
    internal static void GenericStruct(Rect position, SerializedProperty property)
    {
      GUI.Label(EditorGUI.IndentedRect(position), property.displayName, EditorStyles.label);
      position.y += 16f;
      StructPropertyGUI.DoChildren(position, property);
    }

    private static void DoChildren(Rect position, SerializedProperty property)
    {
      position.height = 16f;
      ++EditorGUI.indentLevel;
      SerializedProperty serializedProperty = property.Copy();
      SerializedProperty endProperty = serializedProperty.GetEndProperty();
      serializedProperty.NextVisible(true);
      while (!SerializedProperty.EqualContents(serializedProperty, endProperty))
      {
        EditorGUI.PropertyField(position, serializedProperty);
        position.y += 16f;
        if (!serializedProperty.NextVisible(false))
          break;
      }
      --EditorGUI.indentLevel;
      EditorGUILayout.Space();
    }
  }
}
