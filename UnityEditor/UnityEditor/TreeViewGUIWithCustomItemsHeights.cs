// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewGUIWithCustomItemsHeights
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class TreeViewGUIWithCustomItemsHeights : ITreeViewGUI
  {
    private List<Rect> m_RowRects = new List<Rect>();
    protected float m_BaseIndent = 2f;
    protected float m_IndentWidth = 14f;
    protected float m_FoldoutWidth = 12f;
    private float m_MaxWidthOfRows;
    protected readonly TreeViewController m_TreeView;

    public TreeViewGUIWithCustomItemsHeights(TreeViewController treeView)
    {
      this.m_TreeView = treeView;
    }

    public virtual void OnInitialize()
    {
    }

    public Rect GetRowRect(int row, float rowWidth)
    {
      if (this.m_RowRects.Count != 0)
        return this.m_RowRects[row];
      Debug.LogError((object) "Ensure precalc rects");
      return new Rect();
    }

    public Rect GetRenameRect(Rect rowRect, int row, TreeViewItem item)
    {
      return new Rect();
    }

    public Rect GetRectForFraming(int row)
    {
      return this.GetRowRect(row, 1f);
    }

    public abstract void OnRowGUI(Rect rowRect, TreeViewItem item, int row, bool selected, bool focused);

    protected virtual float AddSpaceBefore(TreeViewItem item)
    {
      return 0.0f;
    }

    protected virtual Vector2 GetSizeOfRow(TreeViewItem item)
    {
      return new Vector2(this.m_TreeView.GetTotalRect().width, 16f);
    }

    public void CalculateRowRects()
    {
      if (this.m_TreeView.isSearching)
        return;
      IList<TreeViewItem> rows = this.m_TreeView.data.GetRows();
      this.m_RowRects = new List<Rect>(rows.Count);
      float num1 = 2f;
      this.m_MaxWidthOfRows = 1f;
      for (int index = 0; index < rows.Count; ++index)
      {
        TreeViewItem treeViewItem = rows[index];
        float num2 = this.AddSpaceBefore(treeViewItem);
        float y = num1 + num2;
        Vector2 sizeOfRow = this.GetSizeOfRow(treeViewItem);
        this.m_RowRects.Add(new Rect(0.0f, y, sizeOfRow.x, sizeOfRow.y));
        num1 = y + sizeOfRow.y;
        if ((double) sizeOfRow.x > (double) this.m_MaxWidthOfRows)
          this.m_MaxWidthOfRows = sizeOfRow.x;
      }
    }

    public Vector2 GetTotalSize()
    {
      if (this.m_RowRects.Count == 0)
        return new Vector2(0.0f, 0.0f);
      return new Vector2(this.m_MaxWidthOfRows, this.m_RowRects[this.m_RowRects.Count - 1].yMax);
    }

    public int GetNumRowsOnPageUpDown(TreeViewItem fromItem, bool pageUp, float heightOfTreeView)
    {
      Debug.LogError((object) "GetNumRowsOnPageUpDown: Not impemented");
      return (int) Mathf.Floor(heightOfTreeView / 30f);
    }

    public void GetFirstAndLastRowVisible(out int firstRowVisible, out int lastRowVisible)
    {
      float y = this.m_TreeView.state.scrollPos.y;
      float height = this.m_TreeView.GetTotalRect().height;
      int rowCount = this.m_TreeView.data.rowCount;
      if (rowCount != this.m_RowRects.Count)
      {
        Debug.LogError((object) "Mismatch in state: rows vs cached rects. Did you remember to hook up: dataSource.onVisibleRowsChanged += gui.CalculateRowRects ?");
        this.CalculateRowRects();
      }
      int num1 = -1;
      int num2 = -1;
      for (int index = 0; index < this.m_RowRects.Count; ++index)
      {
        if ((double) this.m_RowRects[index].y > (double) y && (double) this.m_RowRects[index].y < (double) y + (double) height || (double) this.m_RowRects[index].yMax > (double) y && (double) this.m_RowRects[index].yMax < (double) y + (double) height)
        {
          if (num1 == -1)
            num1 = index;
          num2 = index;
        }
      }
      if (num1 != -1 && num2 != -1)
      {
        firstRowVisible = num1;
        lastRowVisible = num2;
      }
      else
      {
        firstRowVisible = 0;
        lastRowVisible = rowCount - 1;
      }
    }

    public virtual void BeginRowGUI()
    {
    }

    public virtual void EndRowGUI()
    {
    }

    public virtual void BeginPingItem(TreeViewItem item, float topPixelOfRow, float availableWidth)
    {
      throw new NotImplementedException();
    }

    public virtual void EndPingItem()
    {
      throw new NotImplementedException();
    }

    public virtual bool BeginRename(TreeViewItem item, float delay)
    {
      throw new NotImplementedException();
    }

    public virtual void EndRename()
    {
      throw new NotImplementedException();
    }

    public virtual float halfDropBetweenHeight
    {
      get
      {
        return 8f;
      }
    }

    public virtual float topRowMargin { get; private set; }

    public virtual float bottomRowMargin { get; private set; }

    protected float indentWidth
    {
      get
      {
        return this.m_IndentWidth;
      }
    }

    public virtual float GetFoldoutIndent(TreeViewItem item)
    {
      if (this.m_TreeView.isSearching)
        return this.m_BaseIndent;
      return this.m_BaseIndent + (float) item.depth * this.indentWidth;
    }

    public virtual float GetContentIndent(TreeViewItem item)
    {
      return this.GetFoldoutIndent(item) + this.m_FoldoutWidth;
    }
  }
}
