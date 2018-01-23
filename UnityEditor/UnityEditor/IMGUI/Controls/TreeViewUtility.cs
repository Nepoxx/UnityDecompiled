// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.TreeViewUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.IMGUI.Controls
{
  internal static class TreeViewUtility
  {
    internal static void SetParentAndChildrenForItems(IList<TreeViewItem> rows, TreeViewItem root)
    {
      TreeViewUtility.SetChildParentReferences(rows, root);
    }

    internal static void SetDepthValuesForItems(TreeViewItem root)
    {
      if (root == null)
        throw new ArgumentNullException(nameof (root), "The root is null");
      Stack<TreeViewItem> treeViewItemStack = new Stack<TreeViewItem>();
      treeViewItemStack.Push(root);
      while (treeViewItemStack.Count > 0)
      {
        TreeViewItem treeViewItem = treeViewItemStack.Pop();
        if (treeViewItem.children != null)
        {
          foreach (TreeViewItem child in treeViewItem.children)
          {
            if (child != null)
            {
              child.depth = treeViewItem.depth + 1;
              treeViewItemStack.Push(child);
            }
          }
        }
      }
    }

    internal static List<TreeViewItem> FindItemsInList(IEnumerable<int> itemIDs, IList<TreeViewItem> treeViewItems)
    {
      return treeViewItems.Where<TreeViewItem>((Func<TreeViewItem, bool>) (x => itemIDs.Contains<int>(x.id))).ToList<TreeViewItem>();
    }

    internal static TreeViewItem FindItemInList<T>(int id, IList<T> treeViewItems) where T : TreeViewItem
    {
      return (TreeViewItem) treeViewItems.FirstOrDefault<T>((Func<T, bool>) (t => t.id == id));
    }

    internal static TreeViewItem FindItem(int id, TreeViewItem searchFromThisItem)
    {
      return TreeViewUtility.FindItemRecursive(id, searchFromThisItem);
    }

    private static TreeViewItem FindItemRecursive(int id, TreeViewItem item)
    {
      if (item == null)
        return (TreeViewItem) null;
      if (item.id == id)
        return item;
      if (!item.hasChildren)
        return (TreeViewItem) null;
      foreach (TreeViewItem child in item.children)
      {
        TreeViewItem itemRecursive = TreeViewUtility.FindItemRecursive(id, child);
        if (itemRecursive != null)
          return itemRecursive;
      }
      return (TreeViewItem) null;
    }

    internal static HashSet<int> GetParentsAboveItem(TreeViewItem fromItem)
    {
      if (fromItem == null)
        throw new ArgumentNullException(nameof (fromItem));
      HashSet<int> intSet = new HashSet<int>();
      for (TreeViewItem parent = fromItem.parent; parent != null; parent = parent.parent)
        intSet.Add(parent.id);
      return intSet;
    }

    internal static HashSet<int> GetParentsBelowItem(TreeViewItem fromItem)
    {
      if (fromItem == null)
        throw new ArgumentNullException(nameof (fromItem));
      Stack<TreeViewItem> treeViewItemStack = new Stack<TreeViewItem>();
      treeViewItemStack.Push(fromItem);
      HashSet<int> intSet = new HashSet<int>();
      while (treeViewItemStack.Count > 0)
      {
        TreeViewItem treeViewItem = treeViewItemStack.Pop();
        if (treeViewItem.hasChildren)
        {
          intSet.Add(treeViewItem.id);
          if (LazyTreeViewDataSource.IsChildListForACollapsedParent((IList<TreeViewItem>) treeViewItem.children))
            throw new InvalidOperationException("Invalid tree for finding descendants: Ensure a complete tree when using this utillity method.");
          foreach (TreeViewItem child in treeViewItem.children)
            treeViewItemStack.Push(child);
        }
      }
      return intSet;
    }

    internal static void DebugPrintToEditorLogRecursive(TreeViewItem item)
    {
      if (item == null)
        return;
      Console.WriteLine(new string(' ', item.depth * 3) + item.displayName);
      if (!item.hasChildren)
        return;
      foreach (TreeViewItem child in item.children)
        TreeViewUtility.DebugPrintToEditorLogRecursive(child);
    }

    internal static void SetChildParentReferences(IList<TreeViewItem> visibleItems, TreeViewItem root)
    {
      for (int index = 0; index < visibleItems.Count; ++index)
        visibleItems[index].parent = (TreeViewItem) null;
      int capacity = 0;
      for (int parentIndex = 0; parentIndex < visibleItems.Count; ++parentIndex)
      {
        TreeViewUtility.SetChildParentReferences(parentIndex, visibleItems);
        if (visibleItems[parentIndex].parent == null)
          ++capacity;
      }
      if (capacity > 0)
      {
        List<TreeViewItem> treeViewItemList = new List<TreeViewItem>(capacity);
        for (int index = 0; index < visibleItems.Count; ++index)
        {
          if (visibleItems[index].parent == null)
          {
            treeViewItemList.Add(visibleItems[index]);
            visibleItems[index].parent = root;
          }
        }
        root.children = treeViewItemList;
      }
      else
        root.children = new List<TreeViewItem>();
    }

    private static void SetChildren(TreeViewItem item, List<TreeViewItem> newChildList)
    {
      if (LazyTreeViewDataSource.IsChildListForACollapsedParent((IList<TreeViewItem>) item.children) && newChildList == null)
        return;
      item.children = newChildList;
    }

    private static void SetChildParentReferences(int parentIndex, IList<TreeViewItem> visibleItems)
    {
      TreeViewItem visibleItem = visibleItems[parentIndex];
      if (visibleItem.children != null && visibleItem.children.Count > 0 && visibleItem.children[0] != null)
        return;
      int depth = visibleItem.depth;
      int capacity = 0;
      for (int index = parentIndex + 1; index < visibleItems.Count; ++index)
      {
        if (visibleItems[index].depth == depth + 1)
          ++capacity;
        if (visibleItems[index].depth <= depth)
          break;
      }
      List<TreeViewItem> newChildList = (List<TreeViewItem>) null;
      if (capacity != 0)
      {
        newChildList = new List<TreeViewItem>(capacity);
        int num = 0;
        for (int index = parentIndex + 1; index < visibleItems.Count; ++index)
        {
          if (visibleItems[index].depth == depth + 1)
          {
            visibleItems[index].parent = visibleItem;
            newChildList.Add(visibleItems[index]);
            ++num;
          }
          if (visibleItems[index].depth <= depth)
            break;
        }
      }
      TreeViewUtility.SetChildren(visibleItem, newChildList);
    }
  }
}
