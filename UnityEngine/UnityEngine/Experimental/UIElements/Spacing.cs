// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.Spacing
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  public struct Spacing
  {
    public float left;
    public float top;
    public float right;
    public float bottom;

    public Spacing(float left, float top, float right, float bottom)
    {
      this.left = left;
      this.top = top;
      this.right = right;
      this.bottom = bottom;
    }

    public float horizontal
    {
      get
      {
        return this.left + this.right;
      }
    }

    public float vertical
    {
      get
      {
        return this.top + this.bottom;
      }
    }

    public static Rect operator +(Rect r, Spacing a)
    {
      r.x -= a.left;
      r.y -= a.top;
      r.width += a.horizontal;
      r.height += a.vertical;
      return r;
    }

    public static Rect operator -(Rect r, Spacing a)
    {
      r.x += a.left;
      r.y += a.top;
      r.width -= a.horizontal;
      r.height -= a.vertical;
      return r;
    }
  }
}
