// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewExamples.TestDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;

namespace UnityEditor.TreeViewExamples
{
  internal class TestDragging : TreeViewDragging
  {
    private const string k_GenericDragID = "FooDragging";
    private BackendData m_BackendData;

    public TestDragging(TreeViewController treeView, BackendData data)
      : base(treeView)
    {
      this.m_BackendData = data;
    }

    public override void StartDrag(TreeViewItem draggedItem, List<int> draggedItemIDs)
    {
      DragAndDrop.PrepareStartDrag();
      DragAndDrop.SetGenericData("FooDragging", (object) new TestDragging.FooDragData(this.GetItemsFromIDs((IEnumerable<int>) draggedItemIDs)));
      DragAndDrop.StartDrag(draggedItemIDs.Count.ToString() + " Foo" + (draggedItemIDs.Count <= 1 ? (object) "" : (object) "s"));
    }

    public override DragAndDropVisualMode DoDrag(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
    {
      TestDragging.FooDragData genericData = DragAndDrop.GetGenericData("FooDragging") as TestDragging.FooDragData;
      FooTreeViewItem fooTreeViewItem = parentItem as FooTreeViewItem;
      if (fooTreeViewItem == null || genericData == null)
        return DragAndDropVisualMode.None;
      bool flag = this.ValidDrag(parentItem, genericData.m_DraggedItems);
      if (perform && flag)
      {
        List<BackendData.Foo> list = genericData.m_DraggedItems.Where<TreeViewItem>((Func<TreeViewItem, bool>) (x => x is FooTreeViewItem)).Select<TreeViewItem, BackendData.Foo>((Func<TreeViewItem, BackendData.Foo>) (x => ((FooTreeViewItem) x).foo)).ToList<BackendData.Foo>();
        int[] array = genericData.m_DraggedItems.Where<TreeViewItem>((Func<TreeViewItem, bool>) (x => x is FooTreeViewItem)).Select<TreeViewItem, int>((Func<TreeViewItem, int>) (x => x.id)).ToArray<int>();
        int insertionIndex = TreeViewDragging.GetInsertionIndex(parentItem, targetItem, dropPos);
        this.m_BackendData.ReparentSelection(fooTreeViewItem.foo, insertionIndex, list);
        this.m_TreeView.ReloadData();
        this.m_TreeView.SetSelection(array, true);
      }
      return !flag ? DragAndDropVisualMode.None : DragAndDropVisualMode.Move;
    }

    private bool ValidDrag(TreeViewItem parent, List<TreeViewItem> draggedItems)
    {
      for (TreeViewItem treeViewItem = parent; treeViewItem != null; treeViewItem = treeViewItem.parent)
      {
        if (draggedItems.Contains(treeViewItem))
          return false;
      }
      return true;
    }

    private List<TreeViewItem> GetItemsFromIDs(IEnumerable<int> draggedItemIDs)
    {
      return TreeViewUtility.FindItemsInList(draggedItemIDs, this.m_TreeView.data.GetRows());
    }

    private class FooDragData
    {
      public List<TreeViewItem> m_DraggedItems;

      public FooDragData(List<TreeViewItem> draggedItems)
      {
        this.m_DraggedItems = draggedItems;
      }
    }
  }
}
