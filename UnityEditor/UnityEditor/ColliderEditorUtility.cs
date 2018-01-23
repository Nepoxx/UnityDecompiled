// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColliderEditorUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ColliderEditorUtility
  {
    private static GUIStyle s_EditColliderButtonStyle;
    private const float k_EditColliderbuttonWidth = 22f;
    private const float k_EditColliderbuttonHeight = 22f;
    private const float k_SpaceBetweenLabelAndButton = 5f;

    public static bool InspectorEditButtonGUI(bool editing)
    {
      if (ColliderEditorUtility.s_EditColliderButtonStyle == null)
      {
        ColliderEditorUtility.s_EditColliderButtonStyle = new GUIStyle((GUIStyle) "Button");
        ColliderEditorUtility.s_EditColliderButtonStyle.padding = new RectOffset(0, 0, 0, 0);
        ColliderEditorUtility.s_EditColliderButtonStyle.margin = new RectOffset(0, 0, 0, 0);
      }
      EditorGUI.BeginChangeCheck();
      Rect controlRect = EditorGUILayout.GetControlRect(true, 22f, new GUILayoutOption[0]);
      Rect position1 = new Rect(controlRect.xMin + EditorGUIUtility.labelWidth, controlRect.yMin, 22f, 22f);
      Vector2 vector2 = GUI.skin.label.CalcSize(new GUIContent("Edit Collider"));
      Rect position2 = new Rect(position1.xMax + 5f, controlRect.yMin + (float) (((double) controlRect.height - (double) vector2.y) * 0.5), vector2.x, controlRect.height);
      GUILayout.Space(2f);
      bool flag = GUI.Toggle(position1, editing, EditorGUIUtility.IconContent("EditCollider"), ColliderEditorUtility.s_EditColliderButtonStyle);
      GUI.Label(position2, "Edit Collider");
      if (EditorGUI.EndChangeCheck())
        SceneView.RepaintAll();
      return flag;
    }
  }
}
