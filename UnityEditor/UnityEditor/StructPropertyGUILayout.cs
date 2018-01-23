// Decompiled with JetBrains decompiler
// Type: UnityEditor.StructPropertyGUILayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class StructPropertyGUILayout
  {
    internal static void GenericStruct(SerializedProperty property, params GUILayoutOption[] options)
    {
      float num = (float) (16.0 + 16.0 * (double) StructPropertyGUILayout.GetChildrenCount(property));
      StructPropertyGUI.GenericStruct(GUILayoutUtility.GetRect(EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, num, num, EditorStyles.layerMaskField, options), property);
    }

    internal static int GetChildrenCount(SerializedProperty property)
    {
      int num = 0;
      SerializedProperty x = property.Copy();
      SerializedProperty endProperty = x.GetEndProperty();
      while (!SerializedProperty.EqualContents(x, endProperty))
      {
        ++num;
        x.NextVisible(true);
      }
      return num;
    }
  }
}
