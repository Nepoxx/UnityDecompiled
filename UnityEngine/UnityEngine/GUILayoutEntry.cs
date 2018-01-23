// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUILayoutEntry
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  internal class GUILayoutEntry
  {
    internal static Rect kDummyRect = new Rect(0.0f, 0.0f, 1f, 1f);
    protected static int indent = 0;
    public Rect rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    private GUIStyle m_Style = GUIStyle.none;
    public float minWidth;
    public float maxWidth;
    public float minHeight;
    public float maxHeight;
    public int stretchWidth;
    public int stretchHeight;

    public GUILayoutEntry(float _minWidth, float _maxWidth, float _minHeight, float _maxHeight, GUIStyle _style)
    {
      this.minWidth = _minWidth;
      this.maxWidth = _maxWidth;
      this.minHeight = _minHeight;
      this.maxHeight = _maxHeight;
      if (_style == null)
        _style = GUIStyle.none;
      this.style = _style;
    }

    public GUILayoutEntry(float _minWidth, float _maxWidth, float _minHeight, float _maxHeight, GUIStyle _style, GUILayoutOption[] options)
    {
      this.minWidth = _minWidth;
      this.maxWidth = _maxWidth;
      this.minHeight = _minHeight;
      this.maxHeight = _maxHeight;
      this.style = _style;
      this.ApplyOptions(options);
    }

    public GUIStyle style
    {
      get
      {
        return this.m_Style;
      }
      set
      {
        this.m_Style = value;
        this.ApplyStyleSettings(value);
      }
    }

    public virtual RectOffset margin
    {
      get
      {
        return this.style.margin;
      }
    }

    public virtual void CalcWidth()
    {
    }

    public virtual void CalcHeight()
    {
    }

    public virtual void SetHorizontal(float x, float width)
    {
      this.rect.x = x;
      this.rect.width = width;
    }

    public virtual void SetVertical(float y, float height)
    {
      this.rect.y = y;
      this.rect.height = height;
    }

    protected virtual void ApplyStyleSettings(GUIStyle style)
    {
      this.stretchWidth = (double) style.fixedWidth != 0.0 || !style.stretchWidth ? 0 : 1;
      this.stretchHeight = (double) style.fixedHeight != 0.0 || !style.stretchHeight ? 0 : 1;
      this.m_Style = style;
    }

    public virtual void ApplyOptions(GUILayoutOption[] options)
    {
      if (options == null)
        return;
      foreach (GUILayoutOption option in options)
      {
        switch (option.type)
        {
          case GUILayoutOption.Type.fixedWidth:
            this.minWidth = this.maxWidth = (float) option.value;
            this.stretchWidth = 0;
            break;
          case GUILayoutOption.Type.fixedHeight:
            this.minHeight = this.maxHeight = (float) option.value;
            this.stretchHeight = 0;
            break;
          case GUILayoutOption.Type.minWidth:
            this.minWidth = (float) option.value;
            if ((double) this.maxWidth < (double) this.minWidth)
            {
              this.maxWidth = this.minWidth;
              break;
            }
            break;
          case GUILayoutOption.Type.maxWidth:
            this.maxWidth = (float) option.value;
            if ((double) this.minWidth > (double) this.maxWidth)
              this.minWidth = this.maxWidth;
            this.stretchWidth = 0;
            break;
          case GUILayoutOption.Type.minHeight:
            this.minHeight = (float) option.value;
            if ((double) this.maxHeight < (double) this.minHeight)
            {
              this.maxHeight = this.minHeight;
              break;
            }
            break;
          case GUILayoutOption.Type.maxHeight:
            this.maxHeight = (float) option.value;
            if ((double) this.minHeight > (double) this.maxHeight)
              this.minHeight = this.maxHeight;
            this.stretchHeight = 0;
            break;
          case GUILayoutOption.Type.stretchWidth:
            this.stretchWidth = (int) option.value;
            break;
          case GUILayoutOption.Type.stretchHeight:
            this.stretchHeight = (int) option.value;
            break;
        }
      }
      if ((double) this.maxWidth != 0.0 && (double) this.maxWidth < (double) this.minWidth)
        this.maxWidth = this.minWidth;
      if ((double) this.maxHeight == 0.0 || (double) this.maxHeight >= (double) this.minHeight)
        return;
      this.maxHeight = this.minHeight;
    }

    public override string ToString()
    {
      string str = "";
      for (int index = 0; index < GUILayoutEntry.indent; ++index)
        str += " ";
      return str + UnityString.Format("{1}-{0} (x:{2}-{3}, y:{4}-{5})", this.style == null ? (object) "NULL" : (object) this.style.name, (object) this.GetType(), (object) this.rect.x, (object) this.rect.xMax, (object) this.rect.y, (object) this.rect.yMax) + "   -   W: " + (object) this.minWidth + "-" + (object) this.maxWidth + (this.stretchWidth == 0 ? (object) "" : (object) "+") + ", H: " + (object) this.minHeight + "-" + (object) this.maxHeight + (this.stretchHeight == 0 ? (object) "" : (object) "+");
    }
  }
}
