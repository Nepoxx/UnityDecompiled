// Decompiled with JetBrains decompiler
// Type: UnityEditor.RectangleTool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class RectangleTool
  {
    private TimeArea m_TimeArea;
    private RectangleTool.Styles m_Styles;
    private bool m_RippleTimeClutch;

    public TimeArea timeArea
    {
      get
      {
        return this.m_TimeArea;
      }
    }

    public RectangleTool.Styles styles
    {
      get
      {
        return this.m_Styles;
      }
    }

    public bool rippleTimeClutch
    {
      get
      {
        return this.m_RippleTimeClutch;
      }
    }

    public Rect contentRect
    {
      get
      {
        return new Rect(0.0f, 0.0f, this.m_TimeArea.drawRect.width, this.m_TimeArea.drawRect.height);
      }
    }

    public virtual void Initialize(TimeArea timeArea)
    {
      this.m_TimeArea = timeArea;
      if (this.m_Styles != null)
        return;
      this.m_Styles = new RectangleTool.Styles();
    }

    public Vector2 ToolCoordToPosition(RectangleTool.ToolCoord coord, Bounds bounds)
    {
      switch (coord)
      {
        case RectangleTool.ToolCoord.BottomLeft:
          return (Vector2) bounds.min;
        case RectangleTool.ToolCoord.Bottom:
          return new Vector2(bounds.center.x, bounds.min.y);
        case RectangleTool.ToolCoord.BottomRight:
          return new Vector2(bounds.max.x, bounds.min.y);
        case RectangleTool.ToolCoord.Left:
          return new Vector2(bounds.min.x, bounds.center.y);
        case RectangleTool.ToolCoord.Center:
          return (Vector2) bounds.center;
        case RectangleTool.ToolCoord.Right:
          return new Vector2(bounds.max.x, bounds.center.y);
        case RectangleTool.ToolCoord.TopLeft:
          return new Vector2(bounds.min.x, bounds.max.y);
        case RectangleTool.ToolCoord.Top:
          return new Vector2(bounds.center.x, bounds.max.y);
        case RectangleTool.ToolCoord.TopRight:
          return (Vector2) bounds.max;
        default:
          return Vector2.zero;
      }
    }

    public bool CalculateScaleTimeMatrix(float fromTime, float toTime, float offsetTime, float pivotTime, float frameRate, out Matrix4x4 transform, out bool flipKeys)
    {
      transform = Matrix4x4.identity;
      flipKeys = false;
      float num1 = !Mathf.Approximately(frameRate, 0.0f) ? 1f / frameRate : 1f / 1000f;
      float f = toTime - pivotTime;
      float num2 = fromTime - pivotTime;
      if ((double) Mathf.Abs(f) - (double) offsetTime < 0.0)
        return false;
      float num3 = (double) Mathf.Sign(f) != (double) Mathf.Sign(num2) ? f + offsetTime : f - offsetTime;
      if (Mathf.Approximately(num2, 0.0f))
      {
        transform.SetTRS(new Vector3(num3, 0.0f, 0.0f), Quaternion.identity, Vector3.one);
        flipKeys = false;
        return true;
      }
      if ((double) Mathf.Abs(num3) < (double) num1)
        num3 = (double) num3 >= 0.0 ? num1 : -num1;
      float x = num3 / num2;
      transform.SetTRS(new Vector3(pivotTime, 0.0f, 0.0f), Quaternion.identity, Vector3.one);
      transform *= Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(x, 1f, 1f));
      transform *= Matrix4x4.TRS(new Vector3(-pivotTime, 0.0f), Quaternion.identity, Vector3.one);
      flipKeys = (double) x < 0.0;
      return true;
    }

    public bool CalculateScaleValueMatrix(float fromValue, float toValue, float offsetValue, float pivotValue, out Matrix4x4 transform, out bool flipKeys)
    {
      transform = Matrix4x4.identity;
      flipKeys = false;
      float num1 = 1f / 1000f;
      float f = toValue - pivotValue;
      float num2 = fromValue - pivotValue;
      if ((double) Mathf.Abs(f) - (double) offsetValue < 0.0)
        return false;
      float num3 = (double) Mathf.Sign(f) != (double) Mathf.Sign(num2) ? f + offsetValue : f - offsetValue;
      if (Mathf.Approximately(num2, 0.0f))
      {
        transform.SetTRS(new Vector3(0.0f, num3, 0.0f), Quaternion.identity, Vector3.one);
        flipKeys = false;
        return true;
      }
      if ((double) Mathf.Abs(num3) < (double) num1)
        num3 = (double) num3 >= 0.0 ? num1 : -num1;
      float y = num3 / num2;
      transform.SetTRS(new Vector3(0.0f, pivotValue, 0.0f), Quaternion.identity, Vector3.one);
      transform *= Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, y, 1f));
      transform *= Matrix4x4.TRS(new Vector3(0.0f, -pivotValue, 0.0f), Quaternion.identity, Vector3.one);
      flipKeys = (double) y < 0.0;
      return true;
    }

    public float PixelToTime(float pixelTime, float frameRate)
    {
      float width = this.contentRect.width;
      float num1 = this.m_TimeArea.shownArea.xMax - this.m_TimeArea.shownArea.xMin;
      float xMin = this.m_TimeArea.shownArea.xMin;
      float num2 = pixelTime / width * num1 + xMin;
      if ((double) frameRate != 0.0)
        num2 = Mathf.Round(num2 * frameRate) / frameRate;
      return num2;
    }

    public float PixelToValue(float pixelValue)
    {
      float height = this.contentRect.height;
      float num1 = this.m_TimeArea.m_Scale.y * -1f;
      float num2 = (float) ((double) this.m_TimeArea.shownArea.yMin * (double) num1 * -1.0);
      return (height - pixelValue - num2) / num1;
    }

    public float TimeToPixel(float time)
    {
      float width = this.contentRect.width;
      float num = this.m_TimeArea.shownArea.xMax - this.m_TimeArea.shownArea.xMin;
      float xMin = this.m_TimeArea.shownArea.xMin;
      return (time - xMin) * width / num;
    }

    public float ValueToPixel(float value)
    {
      float height = this.contentRect.height;
      float num1 = this.m_TimeArea.m_Scale.y * -1f;
      float num2 = (float) ((double) this.m_TimeArea.shownArea.yMin * (double) num1 * -1.0);
      return height - (value * num1 + num2);
    }

    public void HandleClutchKeys()
    {
      Event current = Event.current;
      switch (current.type)
      {
        case EventType.KeyDown:
          if (current.keyCode != KeyCode.R)
            break;
          this.m_RippleTimeClutch = true;
          break;
        case EventType.KeyUp:
          if (current.keyCode != KeyCode.R)
            break;
          this.m_RippleTimeClutch = false;
          break;
      }
    }

    internal enum ToolCoord
    {
      BottomLeft,
      Bottom,
      BottomRight,
      Left,
      Center,
      Right,
      TopLeft,
      Top,
      TopRight,
    }

    internal class Styles
    {
      public GUIStyle rectangleToolHBarLeft = (GUIStyle) "RectangleToolHBarLeft";
      public GUIStyle rectangleToolHBarRight = (GUIStyle) "RectangleToolHBarRight";
      public GUIStyle rectangleToolHBar = (GUIStyle) "RectangleToolHBar";
      public GUIStyle rectangleToolVBarBottom = (GUIStyle) "RectangleToolVBarBottom";
      public GUIStyle rectangleToolVBarTop = (GUIStyle) "RectangleToolVBarTop";
      public GUIStyle rectangleToolVBar = (GUIStyle) "RectangleToolVBar";
      public GUIStyle rectangleToolSelection = (GUIStyle) "RectangleToolSelection";
      public GUIStyle rectangleToolHighlight = (GUIStyle) "RectangleToolHighlight";
      public GUIStyle rectangleToolScaleLeft = (GUIStyle) "RectangleToolScaleLeft";
      public GUIStyle rectangleToolScaleRight = (GUIStyle) "RectangleToolScaleRight";
      public GUIStyle rectangleToolScaleBottom = (GUIStyle) "RectangleToolScaleBottom";
      public GUIStyle rectangleToolScaleTop = (GUIStyle) "RectangleToolScaleTop";
      public GUIStyle dopesheetScaleLeft = (GUIStyle) "DopesheetScaleLeft";
      public GUIStyle dopesheetScaleRight = (GUIStyle) "DopesheetScaleRight";
      public GUIStyle dragLabel = (GUIStyle) "ProfilerBadge";
    }
  }
}
