// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.TreeViewDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  internal abstract class TreeViewDataSource : ITreeViewDataSource
  {
    protected bool m_NeedRefreshRows = true;
    protected readonly TreeViewController m_TreeView;
    protected TreeViewItem m_RootItem;
    protected IList<TreeViewItem> m_Rows;
    protected TreeViewItem m_FakeItem;
    public Action onVisibleRowsChanged;

    public TreeViewDataSource(TreeViewController treeView)
    {
      this.m_TreeView = treeView;
      this.showRootItem = true;
      this.rootIsCollapsable = false;
      this.m_RootItem = (TreeViewItem) null;
      this.onVisibleRowsChanged = (Action) null;
    }

    public bool showRootItem { get; set; }

    public bool rootIsCollapsable { get; set; }

    public bool alwaysAddFirstItemToSearchResult { get; set; }

    public TreeViewItem root
    {
      get
      {
        return this.m_RootItem;
      }
    }

    protected List<int> expandedIDs
    {
      get
      {
        return this.m_TreeView.state.expandedIDs;
      }
      set
      {
        this.m_TreeView.state.expandedIDs = value;
      }
    }

    public virtual void OnInitialize()
    {
    }

    public abstract void FetchData();

    public virtual void ReloadData()
    {
      this.m_FakeItem = (TreeViewItem) null;
      this.FetchData();
    }

    public virtual TreeViewItem FindItem(int id)
    {
      return TreeViewUtility.FindItem(id, this.m_RootItem);
    }

    public virtual bool IsRevealed(int id)
    {
      return TreeViewController.GetIndexOfID(this.GetRows(), id) >= 0;
    }

    public virtual void RevealItem(int id)
    {
      if (this.IsRevealed(id))
        return;
      TreeViewItem treeViewItem = this.FindItem(id);
      if (treeViewItem == null)
        return;
      for (TreeViewItem parent = treeViewItem.parent; parent != null; parent = parent.parent)
        this.SetExpanded(parent, true);
    }

    public virtual void OnSearchChanged()
    {
      this.m_NeedRefreshRows = true;
    }

    protected void GetVisibleItemsRecursive(TreeViewItem item, IList<TreeViewItem> items)
    {
      if (item != this.m_RootItem || this.showRootItem)
        items.Add(item);
      if (!item.hasChildren || !this.IsExpanded(item))
        return;
      foreach (TreeViewItem child in item.children)
        this.GetVisibleItemsRecursive(child, items);
    }

    protected void SearchRecursive(TreeViewItem item, string search, IList<TreeViewItem> searchResult)
    {
      if (item.displayName.ToLower().Contains(search))
        searchResult.Add(item);
      if (item.children == null)
        return;
      foreach (TreeViewItem child in item.children)
        this.SearchRecursive(child, search, searchResult);
    }

    protected virtual List<TreeViewItem> ExpandedRows(TreeViewItem root)
    {
      List<TreeViewItem> treeViewItemList = new List<TreeViewItem>();
      this.GetVisibleItemsRecursive(this.m_RootItem, (IList<TreeViewItem>) treeViewItemList);
      return treeViewItemList;
    }

    protected virtual List<TreeViewItem> Search(TreeViewItem root, string search)
    {
      List<TreeViewItem> treeViewItemList = new List<TreeViewItem>();
      if (this.showRootItem)
      {
        this.SearchRecursive(root, search, (IList<TreeViewItem>) treeViewItemList);
        treeViewItemList.Sort((IComparer<TreeViewItem>) new TreeViewItemAlphaNumericSort());
      }
      else
      {
        int num = !this.alwaysAddFirstItemToSearchResult ? 0 : 1;
        if (root.hasChildren)
        {
          for (int index = num; index < root.children.Count; ++index)
            this.SearchRecursive(root.children[index], search, (IList<TreeViewItem>) treeViewItemList);
          treeViewItemList.Sort((IComparer<TreeViewItem>) new TreeViewItemAlphaNumericSort());
          if (this.alwaysAddFirstItemToSearchResult)
            treeViewItemList.Insert(0, root.children[0]);
        }
      }
      return treeViewItemList;
    }

    public virtual int rowCount
    {
      get
      {
        return this.GetRows().Count;
      }
    }

    public virtual int GetRow(int id)
    {
      IList<TreeViewItem> rows = this.GetRows();
      for (int index = 0; index < rows.Count; ++index)
      {
        if (rows[index].id == id)
          return index;
      }
      return -1;
    }

    public virtual TreeViewItem GetItem(int row)
    {
      return this.GetRows()[row];
    }

    public virtual IList<TreeViewItem> GetRows()
    {
      this.InitIfNeeded();
      return this.m_Rows;
    }

    public virtual void InitIfNeeded()
    {
      if (this.m_Rows != null && !this.m_NeedRefreshRows)
        return;
      if (this.m_RootItem != null)
      {
        this.m_Rows = !this.m_TreeView.isSearching ? (IList<TreeViewItem>) this.ExpandedRows(this.m_RootItem) : (IList<TreeViewItem>) this.Search(this.m_RootItem, this.m_TreeView.searchString.ToLower());
      }
      else
      {
        Debug.LogError((object) "TreeView root item is null. Ensure that your TreeViewDataSource sets up at least a root item.");
        this.m_Rows = (IList<TreeViewItem>) new List<TreeViewItem>();
      }
      this.m_NeedRefreshRows = false;
      if (this.onVisibleRowsChanged != null)
        this.onVisibleRowsChanged();
      this.m_TreeView.Repaint();
    }

    public bool isInitialized
    {
      get
      {
        return this.m_RootItem != null && this.m_Rows != null;
      }
    }

    public virtual int[] GetExpandedIDs()
    {
      return this.expandedIDs.ToArray();
    }

    public virtual void SetExpandedIDs(int[] ids)
    {
      this.expandedIDs = new List<int>((IEnumerable<int>) ids);
      this.expandedIDs.Sort();
      this.m_NeedRefreshRows = true;
      this.OnExpandedStateChanged();
    }

    public virtual bool IsExpanded(int id)
    {
      return this.expandedIDs.BinarySearch(id) >= 0;
    }

    public virtual bool SetExpanded(int id, bool expand)
    {
      bool flag = this.IsExpanded(id);
      if (expand == flag)
        return false;
      if (expand)
      {
        this.expandedIDs.Add(id);
        this.expandedIDs.Sort();
      }
      else
        this.expandedIDs.Remove(id);
      this.m_NeedRefreshRows = true;
      this.OnExpandedStateChanged();
      return true;
    }

    public virtual void SetExpandedWithChildren(int id, bool expand)
    {
      this.SetExpandedWithChildren(this.FindItem(id), expand);
    }

    public virtual void SetExpandedWithChildren(TreeViewItem fromItem, bool expand)
    {
      if (fromItem == null)
      {
        Debug.LogError((object) "item is null");
      }
      else
      {
        HashSet<int> parentsBelowItem = TreeViewUtility.GetParentsBelowItem(fromItem);
        HashSet<int> source = new HashSet<int>((IEnumerable<int>) this.expandedIDs);
        if (expand)
          source.UnionWith((IEnumerable<int>) parentsBelowItem);
        else
          source.ExceptWith((IEnumerable<int>) parentsBelowItem);
        this.SetExpandedIDs(source.ToArray<int>());
      }
    }

    public virtual void SetExpanded(TreeViewItem item, bool expand)
    {
      this.SetExpanded(item.id, expand);
    }

    public virtual bool IsExpanded(TreeViewItem item)
    {
      return this.IsExpanded(item.id);
    }

    public virtual bool IsExpandable(TreeViewItem item)
    {
      if (this.m_TreeView.isSearching)
        return false;
      return item.hasChildren;
    }

    public virtual bool CanBeMultiSelected(TreeViewItem item)
    {
      return true;
    }

    public virtual bool CanBeParent(TreeViewItem item)
    {
      return true;
    }

    public virtual void OnExpandedStateChanged()
    {
      if (this.m_TreeView.expandedStateChanged == null)
        return;
      this.m_TreeView.expandedStateChanged();
    }

    public virtual bool IsRenamingItemAllowed(TreeViewItem item)
    {
      return true;
    }

    public virtual void InsertFakeItem(int id, int parentID, string name, Texture2D icon)
    {
      Debug.LogError((object) "InsertFakeItem missing implementation");
    }

    public virtual bool HasFakeItem()
    {
      return this.m_FakeItem != null;
    }

    public virtual void RemoveFakeItem()
    {
      if (!this.HasFakeItem())
        return;
      IList<TreeViewItem> rows = this.GetRows();
      int indexOfId = TreeViewController.GetIndexOfID(rows, this.m_FakeItem.id);
      if (indexOfId != -1)
        rows.RemoveAt(indexOfId);
      this.m_FakeItem = (TreeViewItem) null;
    }
  }
}
