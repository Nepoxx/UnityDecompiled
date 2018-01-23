// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUIScrollGroup
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  internal sealed class GUIScrollGroup : GUILayoutGroup
  {
    public bool allowHorizontalScroll = true;
    public bool allowVerticalScroll = true;
    public float calcMinWidth;
    public float calcMaxWidth;
    public float calcMinHeight;
    public float calcMaxHeight;
    public float clientWidth;
    public float clientHeight;
    public bool needsHorizontalScrollbar;
    public bool needsVerticalScrollbar;
    public GUIStyle horizontalScrollbar;
    public GUIStyle verticalScrollbar;

    [RequiredByNativeCode]
    public GUIScrollGroup()
    {
    }

    public override void CalcWidth()
    {
      float minWidth = this.minWidth;
      float maxWidth = this.maxWidth;
      if (this.allowHorizontalScroll)
      {
        this.minWidth = 0.0f;
        this.maxWidth = 0.0f;
      }
      base.CalcWidth();
      this.calcMinWidth = this.minWidth;
      this.calcMaxWidth = this.maxWidth;
      if (!this.allowHorizontalScroll)
        return;
      if ((double) this.minWidth > 32.0)
        this.minWidth = 32f;
      if ((double) minWidth != 0.0)
        this.minWidth = minWidth;
      if ((double) maxWidth != 0.0)
      {
        this.maxWidth = maxWidth;
        this.stretchWidth = 0;
      }
    }

    public override void SetHorizontal(float x, float width)
    {
      float width1 = !this.needsVerticalScrollbar ? width : width - this.verticalScrollbar.fixedWidth - (float) this.verticalScrollbar.margin.left;
      if (this.allowHorizontalScroll && (double) width1 < (double) this.calcMinWidth)
      {
        this.needsHorizontalScrollbar = true;
        this.minWidth = this.calcMinWidth;
        this.maxWidth = this.calcMaxWidth;
        base.SetHorizontal(x, this.calcMinWidth);
        this.rect.width = width;
        this.clientWidth = this.calcMinWidth;
      }
      else
      {
        this.needsHorizontalScrollbar = false;
        if (this.allowHorizontalScroll)
        {
          this.minWidth = this.calcMinWidth;
          this.maxWidth = this.calcMaxWidth;
        }
        base.SetHorizontal(x, width1);
        this.rect.width = width;
        this.clientWidth = width1;
      }
    }

    public override void CalcHeight()
    {
      float minHeight = this.minHeight;
      float maxHeight = this.maxHeight;
      if (this.allowVerticalScroll)
      {
        this.minHeight = 0.0f;
        this.maxHeight = 0.0f;
      }
      base.CalcHeight();
      this.calcMinHeight = this.minHeight;
      this.calcMaxHeight = this.maxHeight;
      if (this.needsHorizontalScrollbar)
      {
        float num = this.horizontalScrollbar.fixedHeight + (float) this.horizontalScrollbar.margin.top;
        this.minHeight += num;
        this.maxHeight += num;
      }
      if (!this.allowVerticalScroll)
        return;
      if ((double) this.minHeight > 32.0)
        this.minHeight = 32f;
      if ((double) minHeight != 0.0)
        this.minHeight = minHeight;
      if ((double) maxHeight != 0.0)
      {
        this.maxHeight = maxHeight;
        this.stretchHeight = 0;
      }
    }

    public override void SetVertical(float y, float height)
    {
      float height1 = height;
      if (this.needsHorizontalScrollbar)
        height1 -= this.horizontalScrollbar.fixedHeight + (float) this.horizontalScrollbar.margin.top;
      if (this.allowVerticalScroll && (double) height1 < (double) this.calcMinHeight)
      {
        if (!this.needsHorizontalScrollbar && !this.needsVerticalScrollbar)
        {
          this.clientWidth = this.rect.width - this.verticalScrollbar.fixedWidth - (float) this.verticalScrollbar.margin.left;
          if ((double) this.clientWidth < (double) this.calcMinWidth)
            this.clientWidth = this.calcMinWidth;
          float width = this.rect.width;
          this.SetHorizontal(this.rect.x, this.clientWidth);
          this.CalcHeight();
          this.rect.width = width;
        }
        float minHeight = this.minHeight;
        float maxHeight = this.maxHeight;
        this.minHeight = this.calcMinHeight;
        this.maxHeight = this.calcMaxHeight;
        base.SetVertical(y, this.calcMinHeight);
        this.minHeight = minHeight;
        this.maxHeight = maxHeight;
        this.rect.height = height;
        this.clientHeight = this.calcMinHeight;
      }
      else
      {
        if (this.allowVerticalScroll)
        {
          this.minHeight = this.calcMinHeight;
          this.maxHeight = this.calcMaxHeight;
        }
        base.SetVertical(y, height1);
        this.rect.height = height;
        this.clientHeight = height1;
      }
    }
  }
}
