// Decompiled with JetBrains decompiler
// Type: UnityEditor.FlowLayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class FlowLayout : GUILayoutGroup
  {
    private int m_Lines;
    private FlowLayout.LineInfo[] m_LineInfo;

    public override void CalcWidth()
    {
      bool flag = (double) this.minWidth != 0.0;
      base.CalcWidth();
      if (this.isVertical || flag)
        return;
      this.minWidth = 0.0f;
      foreach (GUILayoutEntry entry in this.entries)
        this.minWidth = Mathf.Max(this.m_ChildMinWidth, entry.minWidth);
    }

    public override void SetHorizontal(float x, float width)
    {
      base.SetHorizontal(x, width);
      if (this.resetCoords)
        x = 0.0f;
      if (this.isVertical)
      {
        Debug.LogError((object) "Wordwrapped vertical group. Don't. Just Don't");
      }
      else
      {
        this.m_Lines = 0;
        float num = 0.0f;
        foreach (GUILayoutEntry entry in this.entries)
        {
          if ((double) entry.rect.xMax - (double) num > (double) x + (double) width)
          {
            num = entry.rect.x - (float) entry.margin.left;
            ++this.m_Lines;
          }
          entry.SetHorizontal(entry.rect.x - num, entry.rect.width);
          entry.rect.y = (float) this.m_Lines;
        }
        ++this.m_Lines;
      }
    }

    public override void CalcHeight()
    {
      if (this.entries.Count == 0)
      {
        this.maxHeight = this.minHeight = 0.0f;
      }
      else
      {
        this.m_ChildMinHeight = this.m_ChildMaxHeight = 0.0f;
        int num1 = 0;
        int num2 = 0;
        this.m_StretchableCountY = 0;
        if (!this.isVertical)
        {
          this.m_LineInfo = new FlowLayout.LineInfo[this.m_Lines];
          for (int index = 0; index < this.m_Lines; ++index)
          {
            this.m_LineInfo[index].topBorder = 10000;
            this.m_LineInfo[index].bottomBorder = 10000;
          }
          foreach (GUILayoutEntry entry in this.entries)
          {
            entry.CalcHeight();
            int y = (int) entry.rect.y;
            this.m_LineInfo[y].minSize = Mathf.Max(entry.minHeight, this.m_LineInfo[y].minSize);
            this.m_LineInfo[y].maxSize = Mathf.Max(entry.maxHeight, this.m_LineInfo[y].maxSize);
            this.m_LineInfo[y].topBorder = Mathf.Min(entry.margin.top, this.m_LineInfo[y].topBorder);
            this.m_LineInfo[y].bottomBorder = Mathf.Min(entry.margin.bottom, this.m_LineInfo[y].bottomBorder);
          }
          for (int index = 0; index < this.m_Lines; ++index)
          {
            this.m_ChildMinHeight += this.m_LineInfo[index].minSize;
            this.m_ChildMaxHeight += this.m_LineInfo[index].maxSize;
          }
          for (int index = 1; index < this.m_Lines; ++index)
          {
            float num3 = (float) Mathf.Max(this.m_LineInfo[index - 1].bottomBorder, this.m_LineInfo[index].topBorder);
            this.m_ChildMinHeight += num3;
            this.m_ChildMaxHeight += num3;
          }
          num1 = this.m_LineInfo[0].topBorder;
          num2 = this.m_LineInfo[this.m_LineInfo.Length - 1].bottomBorder;
        }
        this.margin.top = num1;
        this.margin.bottom = num2;
        float num4;
        float num5 = num4 = 0.0f;
        this.minHeight = Mathf.Max(this.minHeight, this.m_ChildMinHeight + num5 + num4);
        if ((double) this.maxHeight == 0.0)
        {
          this.stretchHeight += this.m_StretchableCountY + (!this.style.stretchHeight ? 0 : 1);
          this.maxHeight = this.m_ChildMaxHeight + num5 + num4;
        }
        else
          this.stretchHeight = 0;
        this.maxHeight = Mathf.Max(this.maxHeight, this.minHeight);
      }
    }

    public override void SetVertical(float y, float height)
    {
      if (this.entries.Count == 0)
        base.SetVertical(y, height);
      else if (this.isVertical)
      {
        base.SetVertical(y, height);
      }
      else
      {
        if (this.resetCoords)
          y = 0.0f;
        float num1 = y - (float) this.margin.top;
        float num2 = y + (float) this.margin.vertical - this.spacing * (float) (this.m_Lines - 1);
        float t = 0.0f;
        if ((double) this.m_ChildMinHeight != (double) this.m_ChildMaxHeight)
          t = Mathf.Clamp((float) (((double) num2 - (double) this.m_ChildMinHeight) / ((double) this.m_ChildMaxHeight - (double) this.m_ChildMinHeight)), 0.0f, 1f);
        float num3 = num1;
        for (int index = 0; index < this.m_Lines; ++index)
        {
          if (index > 0)
            num3 += (float) Mathf.Max(this.m_LineInfo[index].topBorder, this.m_LineInfo[index - 1].bottomBorder);
          this.m_LineInfo[index].start = num3;
          this.m_LineInfo[index].size = Mathf.Lerp(this.m_LineInfo[index].minSize, this.m_LineInfo[index].maxSize, t);
          num3 += this.m_LineInfo[index].size + this.spacing;
        }
        foreach (GUILayoutEntry entry in this.entries)
        {
          FlowLayout.LineInfo lineInfo = this.m_LineInfo[(int) entry.rect.y];
          if (entry.stretchHeight != 0)
            entry.SetVertical(lineInfo.start + (float) entry.margin.top, lineInfo.size - (float) entry.margin.vertical);
          else
            entry.SetVertical(lineInfo.start + (float) entry.margin.top, Mathf.Clamp(lineInfo.size - (float) entry.margin.vertical, entry.minHeight, entry.maxHeight));
        }
      }
    }

    private struct LineInfo
    {
      public float minSize;
      public float maxSize;
      public float start;
      public float size;
      public int topBorder;
      public int bottomBorder;
    }
  }
}
