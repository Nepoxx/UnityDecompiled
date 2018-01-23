// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.LazyTreeViewDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.IMGUI.Controls
{
  internal abstract class LazyTreeViewDataSource : TreeViewDataSource
  {
    public LazyTreeViewDataSource(TreeViewController treeView)
      : base(treeView)
    {
    }

    public static List<TreeViewItem> CreateChildListForCollapsedParent()
    {
      return new List<TreeViewItem>() { (TreeViewItem) null };
    }

    public static bool IsChildListForACollapsedParent(IList<TreeViewItem> childList)
    {
      return childList != null && childList.Count == 1 && childList[0] == null;
    }

    protected abstract HashSet<int> GetParentsAbove(int id);

    protected abstract HashSet<int> GetParentsBelow(int id);

    public override void RevealItem(int itemID)
    {
      HashSet<int> source = new HashSet<int>((IEnumerable<int>) this.expandedIDs);
      int count = source.Count;
      HashSet<int> parentsAbove = this.GetParentsAbove(itemID);
      source.UnionWith((IEnumerable<int>) parentsAbove);
      if (count == source.Count)
        return;
      this.SetExpandedIDs(source.ToArray<int>());
      if (this.m_NeedRefreshRows)
        this.FetchData();
    }

    public override TreeViewItem FindItem(int itemID)
    {
      this.RevealItem(itemID);
      return base.FindItem(itemID);
    }

    public override void SetExpandedWithChildren(TreeViewItem item, bool expand)
    {
      this.SetExpandedWithChildren(item.id, expand);
    }

    public override void SetExpandedWithChildren(int id, bool expand)
    {
      HashSet<int> source = new HashSet<int>((IEnumerable<int>) this.expandedIDs);
      HashSet<int> parentsBelow = this.GetParentsBelow(id);
      if (expand)
        source.UnionWith((IEnumerable<int>) parentsBelow);
      else
        source.ExceptWith((IEnumerable<int>) parentsBelow);
      this.SetExpandedIDs(source.ToArray<int>());
    }

    public override void InitIfNeeded()
    {
      if (this.m_Rows != null && !this.m_NeedRefreshRows)
        return;
      this.FetchData();
      this.m_NeedRefreshRows = false;
      if (this.onVisibleRowsChanged != null)
        this.onVisibleRowsChanged();
      this.m_TreeView.Repaint();
    }

    public override IList<TreeViewItem> GetRows()
    {
      this.InitIfNeeded();
      return this.m_Rows;
    }
  }
}
