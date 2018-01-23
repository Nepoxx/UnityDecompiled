// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.EdgeControl
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class EdgeControl : VisualElement
  {
    private float m_CapRadius = 5f;
    private int m_EdgeWidth = 2;
    private float m_InterceptWidth = 5f;
    private Orientation m_Orientation;
    private LineType m_LineType;
    private Color m_EdgeColor;
    private Color m_StartCapColor;
    private Color m_EndCapColor;
    private Vector2 m_From;
    private Vector2 m_To;
    private bool m_TangentsDirty;
    private Vector3[] m_ControlPoints;
    private Vector3[] m_RenderPoints;

    public Orientation orientation
    {
      get
      {
        return this.m_Orientation;
      }
      set
      {
        if (this.m_Orientation == value)
          return;
        this.m_Orientation = value;
        this.Dirty(ChangeType.Repaint);
      }
    }

    public LineType lineType
    {
      get
      {
        return this.m_LineType;
      }
      set
      {
        if (this.m_LineType == value)
          return;
        this.m_LineType = value;
        this.PointsChanged();
      }
    }

    public Color edgeColor
    {
      get
      {
        return this.m_EdgeColor;
      }
      set
      {
        if (this.m_EdgeColor == value)
          return;
        this.m_EdgeColor = value;
        this.Dirty(ChangeType.Repaint);
      }
    }

    public Color startCapColor
    {
      get
      {
        return this.m_StartCapColor;
      }
      set
      {
        if (this.m_StartCapColor == value)
          return;
        this.m_StartCapColor = value;
        this.Dirty(ChangeType.Repaint);
      }
    }

    public Color endCapColor
    {
      get
      {
        return this.m_EndCapColor;
      }
      set
      {
        if (this.m_EndCapColor == value)
          return;
        this.m_EndCapColor = value;
        this.Dirty(ChangeType.Repaint);
      }
    }

    public float capRadius
    {
      get
      {
        return this.m_CapRadius;
      }
      set
      {
        if ((double) this.m_CapRadius == (double) value)
          return;
        this.m_CapRadius = value;
        this.Dirty(ChangeType.Repaint);
      }
    }

    public int edgeWidth
    {
      get
      {
        return this.m_EdgeWidth;
      }
      set
      {
        if (this.m_EdgeWidth == value)
          return;
        this.m_EdgeWidth = value;
        this.Dirty(ChangeType.Repaint);
      }
    }

    public float interceptWidth
    {
      get
      {
        return this.m_InterceptWidth;
      }
      set
      {
        this.m_InterceptWidth = value;
      }
    }

    public Vector2 from
    {
      get
      {
        return this.m_From;
      }
      set
      {
        if (!(this.m_From != value))
          return;
        this.m_From = value;
        this.PointsChanged();
      }
    }

    public Vector2 to
    {
      get
      {
        return this.m_To;
      }
      set
      {
        if (!(this.m_To != value))
          return;
        this.m_To = value;
        this.PointsChanged();
      }
    }

    public Vector3[] controlPoints
    {
      get
      {
        if (this.m_TangentsDirty || this.m_ControlPoints == null)
        {
          this.CacheLineData();
          this.m_TangentsDirty = false;
        }
        return this.m_ControlPoints;
      }
    }

    public Vector3[] renderPoints
    {
      get
      {
        if (this.m_TangentsDirty || this.m_RenderPoints == null)
        {
          this.CacheLineData();
          this.m_TangentsDirty = false;
        }
        return this.m_RenderPoints;
      }
    }

    protected virtual void DrawEdge()
    {
      Vector3[] controlPoints = this.controlPoints;
      switch (this.lineType)
      {
        case LineType.Bezier:
          Handles.DrawBezier(controlPoints[0], controlPoints[3], controlPoints[1], controlPoints[2], this.edgeColor, (Texture2D) null, (float) this.edgeWidth);
          break;
        case LineType.PolyLine:
        case LineType.StraightLine:
          Handles.color = this.edgeColor;
          Handles.DrawAAPolyLine((float) this.edgeWidth, this.renderPoints);
          break;
        default:
          throw new ArgumentOutOfRangeException("Unsupported LineType: " + (object) this.lineType);
      }
    }

    protected virtual void DrawEndpoint(Vector2 pos)
    {
      Handles.DrawSolidDisc((Vector3) pos, new Vector3(0.0f, 0.0f, -1f), this.capRadius);
    }

    public override void DoRepaint()
    {
      Color color = Handles.color;
      this.DrawEdge();
      Handles.color = this.startCapColor;
      this.DrawEndpoint(this.from);
      Handles.color = this.endCapColor;
      this.DrawEndpoint(this.to);
      Handles.color = color;
    }

    public override bool ContainsPoint(Vector2 localPoint)
    {
      if ((double) Vector2.Distance(this.from, localPoint) <= 2.0 * (double) this.capRadius || (double) Vector2.Distance(this.to, localPoint) <= 2.0 * (double) this.capRadius)
        return false;
      Vector3[] renderPoints = this.renderPoints;
      float a = float.PositiveInfinity;
      for (int index = 0; index < renderPoints.Length; ++index)
      {
        float b = Vector3.Distance(renderPoints[index], (Vector3) localPoint);
        a = Mathf.Min(a, b);
        if ((double) a < (double) this.interceptWidth)
          return true;
      }
      return false;
    }

    public override bool Overlaps(Rect rect)
    {
      Vector3[] renderPoints = this.renderPoints;
      for (int index = 0; index < renderPoints.Length - 1; ++index)
      {
        Vector2 p1 = new Vector2(renderPoints[index].x, renderPoints[index].y);
        Vector2 p2 = new Vector2(renderPoints[index + 1].x, renderPoints[index + 1].y);
        if (RectUtils.IntersectsSegment(rect, p1, p2))
          return true;
      }
      return false;
    }

    private void PointsChanged()
    {
      this.m_TangentsDirty = true;
      this.layout = new Rect(Vector2.Min(this.m_To, this.m_From), new Vector2(Mathf.Abs(this.m_From.x - this.m_To.x), Mathf.Abs(this.m_From.y - this.m_To.y)));
      this.Dirty(ChangeType.Repaint);
    }

    private void CacheLineData()
    {
      Vector2 vector2_1 = this.to;
      Vector2 vector2_2 = this.from;
      if (this.orientation == Orientation.Horizontal && (double) this.from.x < (double) this.to.x)
      {
        vector2_1 = this.from;
        vector2_2 = this.to;
      }
      if (this.lineType == LineType.StraightLine)
      {
        if (this.m_ControlPoints == null || this.m_ControlPoints.Length != 2)
          this.m_ControlPoints = new Vector3[2];
        this.m_ControlPoints[0] = (Vector3) vector2_1;
        this.m_ControlPoints[1] = (Vector3) vector2_2;
        this.m_RenderPoints = this.m_ControlPoints;
      }
      else
      {
        if (this.m_ControlPoints == null || this.m_ControlPoints.Length != 4)
          this.m_ControlPoints = new Vector3[4];
        this.m_ControlPoints[0] = (Vector3) vector2_1;
        this.m_ControlPoints[3] = (Vector3) vector2_2;
        switch (this.lineType)
        {
          case LineType.Bezier:
            float num1 = 0.5f;
            float num2 = 1f - num1;
            float y = 0.0f;
            float num3 = Mathf.Clamp01((float) (((double) (vector2_1 - vector2_2).magnitude - 10.0) / 50.0));
            if (this.orientation == Orientation.Horizontal)
            {
              this.m_ControlPoints[1] = (Vector3) (vector2_1 + new Vector2((float) (((double) vector2_2.x - (double) vector2_1.x) * (double) num1 + 30.0), y) * num3);
              this.m_ControlPoints[2] = (Vector3) (vector2_2 + new Vector2((float) (((double) vector2_2.x - (double) vector2_1.x) * -(double) num2 - 30.0), -y) * num3);
            }
            else
            {
              float b = (float) ((double) vector2_1.y - (double) vector2_2.y + 100.0);
              float num4 = Mathf.Min((vector2_1 - vector2_2).magnitude, b);
              if ((double) num4 < 0.0)
                num4 = -num4;
              this.m_ControlPoints[1] = (Vector3) (vector2_1 + new Vector2(0.0f, num4 * -0.5f));
              this.m_ControlPoints[2] = (Vector3) (vector2_2 + new Vector2(0.0f, num4 * 0.5f));
            }
            this.m_RenderPoints = Handles.MakeBezierPoints(this.m_ControlPoints[0], this.m_ControlPoints[3], this.m_ControlPoints[1], this.m_ControlPoints[2], 20);
            break;
          case LineType.PolyLine:
            if (this.orientation == Orientation.Horizontal)
            {
              this.m_ControlPoints[2] = (Vector3) new Vector2((float) (((double) vector2_1.x + (double) vector2_2.x) / 2.0), vector2_2.y);
              this.m_ControlPoints[1] = (Vector3) new Vector2((float) (((double) vector2_1.x + (double) vector2_2.x) / 2.0), vector2_1.y);
            }
            else
            {
              this.m_ControlPoints[2] = (Vector3) new Vector2(vector2_2.x, (float) (((double) vector2_1.y + (double) vector2_2.y) / 2.0));
              this.m_ControlPoints[1] = (Vector3) new Vector2(vector2_1.x, (float) (((double) vector2_1.y + (double) vector2_2.y) / 2.0));
            }
            this.m_RenderPoints = this.m_ControlPoints;
            break;
          case LineType.StraightLine:
            break;
          default:
            throw new ArgumentOutOfRangeException("Unsupported LineType: " + (object) this.lineType);
        }
      }
    }
  }
}
