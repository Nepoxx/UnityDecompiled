// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.RectUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class RectUtils
  {
    public static bool IntersectsSegment(Rect rect, Vector2 p1, Vector2 p2)
    {
      float num1 = Mathf.Min(p1.x, p2.x);
      float num2 = Mathf.Max(p1.x, p2.x);
      if ((double) num2 > (double) rect.xMax)
        num2 = rect.xMax;
      if ((double) num1 < (double) rect.xMin)
        num1 = rect.xMin;
      if ((double) num1 > (double) num2)
        return false;
      float num3 = Mathf.Min(p1.y, p2.y);
      float num4 = Mathf.Max(p1.y, p2.y);
      float f = p2.x - p1.x;
      if ((double) Mathf.Abs(f) > 1.40129846432482E-45)
      {
        float num5 = (p2.y - p1.y) / f;
        float num6 = p1.y - num5 * p1.x;
        num3 = num5 * num1 + num6;
        num4 = num5 * num2 + num6;
      }
      if ((double) num3 > (double) num4)
      {
        float num5 = num4;
        num4 = num3;
        num3 = num5;
      }
      if ((double) num4 > (double) rect.yMax)
        num4 = rect.yMax;
      if ((double) num3 < (double) rect.yMin)
        num3 = rect.yMin;
      return (double) num3 <= (double) num4;
    }

    public static Rect Encompass(Rect a, Rect b)
    {
      return new Rect() { xMin = Math.Min(a.xMin, b.xMin), yMin = Math.Min(a.yMin, b.yMin), xMax = Math.Max(a.xMax, b.xMax), yMax = Math.Max(a.yMax, b.yMax) };
    }
  }
}
