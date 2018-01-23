// Decompiled with JetBrains decompiler
// Type: UnityEditor.TimeCursorManipulator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class TimeCursorManipulator : AnimationWindowManipulator
  {
    public TimeCursorManipulator.Alignment alignment;
    public Color headColor;
    public Color lineColor;
    public bool dottedLine;
    public bool drawLine;
    public bool drawHead;
    public string tooltip;
    private GUIStyle m_Style;

    public TimeCursorManipulator(GUIStyle style)
    {
      this.m_Style = style;
      this.dottedLine = false;
      this.headColor = Color.white;
      this.lineColor = style.normal.textColor;
      this.drawLine = true;
      this.drawHead = true;
      this.tooltip = string.Empty;
      this.alignment = TimeCursorManipulator.Alignment.Center;
    }

    public void OnGUI(Rect windowRect, float pixelTime)
    {
      float fixedWidth = this.m_Style.fixedWidth;
      float fixedHeight = this.m_Style.fixedHeight;
      Vector2 vector2 = new Vector2(pixelTime, windowRect.yMin);
      switch (this.alignment)
      {
        case TimeCursorManipulator.Alignment.Center:
          this.rect = new Rect(vector2.x - fixedWidth / 2f, vector2.y, fixedWidth, fixedHeight);
          break;
        case TimeCursorManipulator.Alignment.Left:
          this.rect = new Rect(vector2.x - fixedWidth, vector2.y, fixedWidth, fixedHeight);
          break;
        case TimeCursorManipulator.Alignment.Right:
          this.rect = new Rect(vector2.x, vector2.y, fixedWidth, fixedHeight);
          break;
      }
      Vector3 p1 = new Vector3(vector2.x, vector2.y + fixedHeight, 0.0f);
      Vector3 p2 = new Vector3(vector2.x, windowRect.height, 0.0f);
      if (this.drawLine)
      {
        Handles.color = this.lineColor;
        if (this.dottedLine)
          Handles.DrawDottedLine(p1, p2, 5f);
        else
          Handles.DrawLine(p1, p2);
      }
      if (!this.drawHead)
        return;
      Color color = GUI.color;
      GUI.color = this.headColor;
      GUI.Box(this.rect, GUIContent.none, this.m_Style);
      GUI.color = color;
    }

    public enum Alignment
    {
      Center,
      Left,
      Right,
    }
  }
}
