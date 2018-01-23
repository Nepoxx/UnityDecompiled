// Decompiled with JetBrains decompiler
// Type: UnityEditor.SortingLayerEditorUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SortingLayerEditorUtility
  {
    public static void RenderSortingLayerFields(SerializedProperty sortingOrder, SerializedProperty sortingLayer)
    {
      EditorGUILayout.SortingLayerField(SortingLayerEditorUtility.Styles.m_SortingLayerStyle, sortingLayer, EditorStyles.popup, EditorStyles.label);
      EditorGUILayout.PropertyField(sortingOrder, SortingLayerEditorUtility.Styles.m_SortingOrderStyle, new GUILayoutOption[0]);
    }

    public static void RenderSortingLayerFields(Rect r, SerializedProperty sortingOrder, SerializedProperty sortingLayer)
    {
      EditorGUI.SortingLayerField(r, SortingLayerEditorUtility.Styles.m_SortingLayerStyle, sortingLayer, EditorStyles.popup, EditorStyles.label);
      r.y += EditorGUIUtility.singleLineHeight;
      EditorGUI.PropertyField(r, sortingOrder, SortingLayerEditorUtility.Styles.m_SortingOrderStyle);
    }

    private static class Styles
    {
      public static GUIContent m_SortingLayerStyle = EditorGUIUtility.TextContent("Sorting Layer|Name of the Renderer's sorting layer");
      public static GUIContent m_SortingOrderStyle = EditorGUIUtility.TextContent("Order in Layer|Renderer's order within a sorting layer");
    }
  }
}
