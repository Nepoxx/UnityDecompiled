// Decompiled with JetBrains decompiler
// Type: UnityEditor.AreaManipulator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class AreaManipulator : AnimationWindowManipulator
  {
    private GUIStyle m_Style;
    private MouseCursor m_Cursor;

    public AreaManipulator(GUIStyle style, MouseCursor cursor)
    {
      this.m_Style = style;
      this.m_Cursor = cursor;
    }

    public AreaManipulator(GUIStyle style)
    {
      this.m_Style = style;
      this.m_Cursor = MouseCursor.Arrow;
    }

    public void OnGUI(Rect widgetRect)
    {
      if (this.m_Style == null)
        return;
      this.rect = widgetRect;
      if (Mathf.Approximately(widgetRect.width * widgetRect.height, 0.0f))
        return;
      GUI.Label(widgetRect, GUIContent.none, this.m_Style);
      if (GUIUtility.hotControl == 0 && this.m_Cursor != MouseCursor.Arrow)
      {
        EditorGUIUtility.AddCursorRect(widgetRect, this.m_Cursor);
      }
      else
      {
        if (GUIUtility.hotControl != this.controlID)
          return;
        Vector2 mousePosition = Event.current.mousePosition;
        EditorGUIUtility.AddCursorRect(new Rect(mousePosition.x - 10f, mousePosition.y - 10f, 20f, 20f), this.m_Cursor);
      }
    }
  }
}
