// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.GridBackground
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class GridBackground : VisualElement
  {
    private const string k_SpacingProperty = "spacing";
    private const string k_ThickLinesProperty = "thick-lines";
    private const string k_LineColorProperty = "line-color";
    private const string k_ThickLineColorProperty = "thick-line-color";
    private const string k_GridBackgroundColorProperty = "grid-background-color";
    private StyleValue<float> m_Spacing;
    private StyleValue<int> m_ThickLines;
    private StyleValue<Color> m_LineColor;
    private StyleValue<Color> m_ThickLineColor;
    private StyleValue<Color> m_GridBackgroundColor;
    private VisualElement m_Container;

    public GridBackground()
    {
      this.pickingMode = PickingMode.Ignore;
      this.StretchToParentSize();
    }

    private float spacing
    {
      get
      {
        return this.m_Spacing.GetSpecifiedValueOrDefault(50f);
      }
    }

    private int thickLines
    {
      get
      {
        return this.m_ThickLines.GetSpecifiedValueOrDefault(10);
      }
    }

    private Color lineColor
    {
      get
      {
        return this.m_LineColor.GetSpecifiedValueOrDefault(new Color(0.0f, 0.0f, 0.0f, 0.18f));
      }
    }

    private Color thickLineColor
    {
      get
      {
        return this.m_ThickLineColor.GetSpecifiedValueOrDefault(new Color(0.0f, 0.0f, 0.0f, 0.38f));
      }
    }

    private Color gridBackgroundColor
    {
      get
      {
        return this.m_GridBackgroundColor.GetSpecifiedValueOrDefault(new Color(0.17f, 0.17f, 0.17f, 1f));
      }
    }

    private Vector3 Clip(Rect clipRect, Vector3 _in)
    {
      if ((double) _in.x < (double) clipRect.xMin)
        _in.x = clipRect.xMin;
      if ((double) _in.x > (double) clipRect.xMax)
        _in.x = clipRect.xMax;
      if ((double) _in.y < (double) clipRect.yMin)
        _in.y = clipRect.yMin;
      if ((double) _in.y > (double) clipRect.yMax)
        _in.y = clipRect.yMax;
      return _in;
    }

    public override void OnStyleResolved(ICustomStyle elementStyle)
    {
      base.OnStyleResolved(elementStyle);
      elementStyle.ApplyCustomProperty("spacing", ref this.m_Spacing);
      elementStyle.ApplyCustomProperty("thick-lines", ref this.m_ThickLines);
      elementStyle.ApplyCustomProperty("thick-line-color", ref this.m_ThickLineColor);
      elementStyle.ApplyCustomProperty("line-color", ref this.m_LineColor);
      elementStyle.ApplyCustomProperty("grid-background-color", ref this.m_GridBackgroundColor);
    }

    public override void DoRepaint()
    {
      UnityEditor.Experimental.UIElements.GraphView.GraphView parent = this.parent as UnityEditor.Experimental.UIElements.GraphView.GraphView;
      if (parent == null)
        throw new InvalidOperationException("GridBackground can only be added to a GraphView");
      this.m_Container = parent.contentViewContainer;
      Rect layout1 = parent.layout;
      layout1.x = 0.0f;
      layout1.y = 0.0f;
      Vector3 vector3_1 = new Vector3(this.m_Container.transform.matrix.GetColumn(0).magnitude, this.m_Container.transform.matrix.GetColumn(1).magnitude, this.m_Container.transform.matrix.GetColumn(2).magnitude);
      Vector4 column = this.m_Container.transform.matrix.GetColumn(3);
      Rect layout2 = this.m_Container.layout;
      HandleUtility.ApplyWireMaterial();
      GL.Begin(7);
      GL.Color(this.gridBackgroundColor);
      GL.Vertex(new Vector3(layout1.x, layout1.y));
      GL.Vertex(new Vector3(layout1.xMax, layout1.y));
      GL.Vertex(new Vector3(layout1.xMax, layout1.yMax));
      GL.Vertex(new Vector3(layout1.x, layout1.yMax));
      GL.End();
      Vector3 point1 = new Vector3(layout1.x, layout1.y, 0.0f);
      Vector3 point2 = new Vector3(layout1.x, layout1.height, 0.0f);
      Matrix4x4 matrix4x4 = Matrix4x4.TRS((Vector3) column, Quaternion.identity, Vector3.one);
      Vector3 vector3_2 = matrix4x4.MultiplyPoint(point1);
      Vector3 vector3_3 = matrix4x4.MultiplyPoint(point2);
      vector3_2.x += layout2.x * vector3_1.x;
      vector3_2.y += layout2.y * vector3_1.y;
      vector3_3.x += layout2.x * vector3_1.x;
      vector3_3.y += layout2.y * vector3_1.y;
      Handles.DrawWireDisc(vector3_2, new Vector3(0.0f, 0.0f, -1f), 6f);
      float x = vector3_2.x;
      float y = vector3_2.y;
      vector3_2.x = (float) ((double) vector3_2.x % ((double) this.spacing * (double) vector3_1.x) - (double) this.spacing * (double) vector3_1.x);
      vector3_3.x = vector3_2.x;
      vector3_2.y = layout1.y;
      vector3_3.y = layout1.y + layout1.height;
      while ((double) vector3_2.x < (double) layout1.width)
      {
        vector3_2.x += this.spacing * vector3_1.x;
        vector3_3.x += this.spacing * vector3_1.x;
        GL.Begin(1);
        GL.Color(this.lineColor);
        GL.Vertex(this.Clip(layout1, vector3_2));
        GL.Vertex(this.Clip(layout1, vector3_3));
        GL.End();
      }
      float num1 = this.spacing * (float) this.thickLines;
      vector3_2.x = vector3_3.x = (float) ((double) x % ((double) num1 * (double) vector3_1.x) - (double) num1 * (double) vector3_1.x);
      while ((double) vector3_2.x < (double) layout1.width + (double) num1)
      {
        GL.Begin(1);
        GL.Color(this.thickLineColor);
        GL.Vertex(this.Clip(layout1, vector3_2));
        GL.Vertex(this.Clip(layout1, vector3_3));
        GL.End();
        vector3_2.x += this.spacing * vector3_1.x * (float) this.thickLines;
        vector3_3.x += this.spacing * vector3_1.x * (float) this.thickLines;
      }
      vector3_2 = new Vector3(layout1.x, layout1.y, 0.0f);
      vector3_3 = new Vector3(layout1.x + layout1.width, layout1.y, 0.0f);
      vector3_2.x += layout2.x * vector3_1.x;
      vector3_2.y += layout2.y * vector3_1.y;
      vector3_3.x += layout2.x * vector3_1.x;
      vector3_3.y += layout2.y * vector3_1.y;
      vector3_2 = matrix4x4.MultiplyPoint(vector3_2);
      vector3_3 = matrix4x4.MultiplyPoint(vector3_3);
      vector3_2.y = vector3_3.y = (float) ((double) vector3_2.y % ((double) this.spacing * (double) vector3_1.y) - (double) this.spacing * (double) vector3_1.y);
      vector3_2.x = layout1.x;
      vector3_3.x = layout1.width;
      while ((double) vector3_2.y < (double) layout1.height)
      {
        vector3_2.y += this.spacing * vector3_1.y;
        vector3_3.y += this.spacing * vector3_1.y;
        GL.Begin(1);
        GL.Color(this.lineColor);
        GL.Vertex(this.Clip(layout1, vector3_2));
        GL.Vertex(this.Clip(layout1, vector3_3));
        GL.End();
      }
      float num2 = this.spacing * (float) this.thickLines;
      vector3_2.y = vector3_3.y = (float) ((double) y % ((double) num2 * (double) vector3_1.y) - (double) num2 * (double) vector3_1.y);
      while ((double) vector3_2.y < (double) layout1.height + (double) num2)
      {
        GL.Begin(1);
        GL.Color(this.thickLineColor);
        GL.Vertex(this.Clip(layout1, vector3_2));
        GL.Vertex(this.Clip(layout1, vector3_3));
        GL.End();
        vector3_2.y += this.spacing * vector3_1.y * (float) this.thickLines;
        vector3_3.y += this.spacing * vector3_1.y * (float) this.thickLines;
      }
    }
  }
}
