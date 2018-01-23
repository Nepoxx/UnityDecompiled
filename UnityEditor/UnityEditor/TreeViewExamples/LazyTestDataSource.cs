// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewExamples.LazyTestDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace UnityEditor.TreeViewExamples
{
  internal class LazyTestDataSource : LazyTreeViewDataSource
  {
    private BackendData m_Backend;

    public LazyTestDataSource(TreeViewController treeView, BackendData data)
      : base(treeView)
    {
      this.m_Backend = data;
      this.FetchData();
    }

    public int itemCounter { get; private set; }

    public override void FetchData()
    {
      this.itemCounter = 1;
      this.m_RootItem = (TreeViewItem) new FooTreeViewItem(this.m_Backend.root.id, 0, (TreeViewItem) null, this.m_Backend.root.name, this.m_Backend.root);
      this.AddVisibleChildrenRecursive(this.m_Backend.root, this.m_RootItem);
      this.m_Rows = (IList<TreeViewItem>) new List<TreeViewItem>();
      this.GetVisibleItemsRecursive(this.m_RootItem, this.m_Rows);
      this.m_NeedRefreshRows = false;
    }

    private void AddVisibleChildrenRecursive(BackendData.Foo source, TreeViewItem dest)
    {
      if (this.IsExpanded(source.id))
      {
        if (source.children == null || source.children.Count <= 0)
          return;
        dest.children = new List<TreeViewItem>(source.children.Count);
        for (int index = 0; index < source.children.Count; ++index)
        {
          BackendData.Foo child = source.children[index];
          dest.children.Add((TreeViewItem) new FooTreeViewItem(child.id, dest.depth + 1, dest, child.name, child));
          ++this.itemCounter;
          this.AddVisibleChildrenRecursive(child, dest.children[index]);
        }
      }
      else if (source.hasChildren)
        dest.children = LazyTreeViewDataSource.CreateChildListForCollapsedParent();
    }

    public override bool CanBeParent(TreeViewItem item)
    {
      return item.hasChildren;
    }

    protected override HashSet<int> GetParentsAbove(int id)
    {
      HashSet<int> intSet = new HashSet<int>();
      for (BackendData.Foo foo = BackendData.FindItemRecursive(this.m_Backend.root, id); foo != null; foo = foo.parent)
      {
        if (foo.parent != null)
          intSet.Add(foo.parent.id);
      }
      return intSet;
    }

    protected override HashSet<int> GetParentsBelow(int id)
    {
      return this.m_Backend.GetParentsBelow(id);
    }
  }
}
