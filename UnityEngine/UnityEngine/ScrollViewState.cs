// Decompiled with JetBrains decompiler
// Type: UnityEngine.ScrollViewState
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  internal class ScrollViewState
  {
    public Rect position;
    public Rect visibleRect;
    public Rect viewRect;
    public Vector2 scrollPosition;
    public bool apply;

    [RequiredByNativeCode]
    public ScrollViewState()
    {
    }

    public void ScrollTo(Rect pos)
    {
      this.ScrollTowards(pos, float.PositiveInfinity);
    }

    public bool ScrollTowards(Rect pos, float maxDelta)
    {
      Vector2 vector2 = this.ScrollNeeded(pos);
      if ((double) vector2.sqrMagnitude < 9.99999974737875E-05)
        return false;
      if ((double) maxDelta == 0.0)
        return true;
      if ((double) vector2.magnitude > (double) maxDelta)
        vector2 = vector2.normalized * maxDelta;
      this.scrollPosition += vector2;
      this.apply = true;
      return true;
    }

    private Vector2 ScrollNeeded(Rect pos)
    {
      Rect visibleRect = this.visibleRect;
      visibleRect.x += this.scrollPosition.x;
      visibleRect.y += this.scrollPosition.y;
      float num1 = pos.width - this.visibleRect.width;
      if ((double) num1 > 0.0)
      {
        pos.width -= num1;
        pos.x += num1 * 0.5f;
      }
      float num2 = pos.height - this.visibleRect.height;
      if ((double) num2 > 0.0)
      {
        pos.height -= num2;
        pos.y += num2 * 0.5f;
      }
      Vector2 zero = Vector2.zero;
      if ((double) pos.xMax > (double) visibleRect.xMax)
        zero.x += pos.xMax - visibleRect.xMax;
      else if ((double) pos.xMin < (double) visibleRect.xMin)
        zero.x -= visibleRect.xMin - pos.xMin;
      if ((double) pos.yMax > (double) visibleRect.yMax)
        zero.y += pos.yMax - visibleRect.yMax;
      else if ((double) pos.yMin < (double) visibleRect.yMin)
        zero.y -= visibleRect.yMin - pos.yMin;
      Rect viewRect = this.viewRect;
      viewRect.width = Mathf.Max(viewRect.width, this.visibleRect.width);
      viewRect.height = Mathf.Max(viewRect.height, this.visibleRect.height);
      zero.x = Mathf.Clamp(zero.x, viewRect.xMin - this.scrollPosition.x, viewRect.xMax - this.visibleRect.width - this.scrollPosition.x);
      zero.y = Mathf.Clamp(zero.y, viewRect.yMin - this.scrollPosition.y, viewRect.yMax - this.visibleRect.height - this.scrollPosition.y);
      return zero;
    }
  }
}
