// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewExamples.TestDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace UnityEditor.TreeViewExamples
{
  internal class TestDataSource : TreeViewDataSource
  {
    private BackendData m_Backend;

    public TestDataSource(TreeViewController treeView, BackendData data)
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
      this.AddChildrenRecursive(this.m_Backend.root, this.m_RootItem);
      this.m_NeedRefreshRows = true;
    }

    private void AddChildrenRecursive(BackendData.Foo source, TreeViewItem dest)
    {
      if (!source.hasChildren)
        return;
      dest.children = new List<TreeViewItem>(source.children.Count);
      for (int index = 0; index < source.children.Count; ++index)
      {
        BackendData.Foo child = source.children[index];
        dest.children.Add((TreeViewItem) new FooTreeViewItem(child.id, dest.depth + 1, dest, child.name, child));
        ++this.itemCounter;
        this.AddChildrenRecursive(child, dest.children[index]);
      }
    }

    public override bool CanBeParent(TreeViewItem item)
    {
      return true;
    }
  }
}
