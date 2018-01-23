// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewExamples.BackendData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.TreeViewExamples
{
  internal class BackendData
  {
    public bool m_RecursiveFindParentsBelow = true;
    private int m_MaxItems = 10000;
    private BackendData.Foo m_Root;
    private const int k_MinChildren = 3;
    private const int k_MaxChildren = 15;
    private const float k_ProbOfLastDescendent = 0.5f;
    private const int k_MaxDepth = 12;

    public BackendData.Foo root
    {
      get
      {
        return this.m_Root;
      }
    }

    public int IDCounter { get; private set; }

    public void GenerateData(int maxNumItems)
    {
      this.m_MaxItems = maxNumItems;
      this.IDCounter = 1;
      this.m_Root = new BackendData.Foo("Root", 0, 0);
      for (int index = 0; index < 10; ++index)
        this.AddChildrenRecursive(this.m_Root, UnityEngine.Random.Range(3, 15), true);
    }

    public BackendData.Foo Find(int id)
    {
      return this.FindRecursive(id, this.m_Root);
    }

    public BackendData.Foo FindRecursive(int id, BackendData.Foo parent)
    {
      if (!parent.hasChildren)
        return (BackendData.Foo) null;
      foreach (BackendData.Foo child in parent.children)
      {
        if (child.id == id)
          return child;
        BackendData.Foo recursive = this.FindRecursive(id, child);
        if (recursive != null)
          return recursive;
      }
      return (BackendData.Foo) null;
    }

    public HashSet<int> GetParentsBelow(int id)
    {
      BackendData.Foo itemRecursive = BackendData.FindItemRecursive(this.root, id);
      if (itemRecursive == null)
        return new HashSet<int>();
      if (this.m_RecursiveFindParentsBelow)
        return this.GetParentsBelowRecursive(itemRecursive);
      return this.GetParentsBelowStackBased(itemRecursive);
    }

    private HashSet<int> GetParentsBelowStackBased(BackendData.Foo searchFromThis)
    {
      Stack<BackendData.Foo> fooStack = new Stack<BackendData.Foo>();
      fooStack.Push(searchFromThis);
      HashSet<int> intSet = new HashSet<int>();
      while (fooStack.Count > 0)
      {
        BackendData.Foo foo = fooStack.Pop();
        if (foo.hasChildren)
        {
          intSet.Add(foo.id);
          foreach (BackendData.Foo child in foo.children)
            fooStack.Push(child);
        }
      }
      return intSet;
    }

    private HashSet<int> GetParentsBelowRecursive(BackendData.Foo searchFromThis)
    {
      HashSet<int> parentIDs = new HashSet<int>();
      BackendData.GetParentsBelowRecursive(searchFromThis, parentIDs);
      return parentIDs;
    }

    private static void GetParentsBelowRecursive(BackendData.Foo item, HashSet<int> parentIDs)
    {
      if (!item.hasChildren)
        return;
      parentIDs.Add(item.id);
      foreach (BackendData.Foo child in item.children)
        BackendData.GetParentsBelowRecursive(child, parentIDs);
    }

    public void ReparentSelection(BackendData.Foo parentItem, int insertionIndex, List<BackendData.Foo> draggedItems)
    {
      if (parentItem == null)
        return;
      if (insertionIndex > 0)
        insertionIndex -= parentItem.children.GetRange(0, insertionIndex).Count<BackendData.Foo>(new Func<BackendData.Foo, bool>(draggedItems.Contains));
      foreach (BackendData.Foo draggedItem in draggedItems)
      {
        draggedItem.parent.children.Remove(draggedItem);
        draggedItem.parent = parentItem;
      }
      if (!parentItem.hasChildren)
        parentItem.children = new List<BackendData.Foo>();
      List<BackendData.Foo> fooList = new List<BackendData.Foo>((IEnumerable<BackendData.Foo>) parentItem.children);
      if (insertionIndex == -1)
        insertionIndex = 0;
      fooList.InsertRange(insertionIndex, (IEnumerable<BackendData.Foo>) draggedItems);
      parentItem.children = fooList;
    }

    private void AddChildrenRecursive(BackendData.Foo foo, int numChildren, bool force)
    {
      if (this.IDCounter > this.m_MaxItems || foo.depth >= 12 || !force && (double) UnityEngine.Random.value < 0.5)
        return;
      if (foo.children == null)
        foo.children = new List<BackendData.Foo>(numChildren);
      for (int index = 0; index < numChildren; ++index)
        foo.children.Add(new BackendData.Foo("Tud" + (object) this.IDCounter, foo.depth + 1, ++this.IDCounter)
        {
          parent = foo
        });
      if (this.IDCounter > this.m_MaxItems)
        return;
      foreach (BackendData.Foo child in foo.children)
        this.AddChildrenRecursive(child, UnityEngine.Random.Range(3, 15), false);
    }

    public static BackendData.Foo FindItemRecursive(BackendData.Foo item, int id)
    {
      if (item == null)
        return (BackendData.Foo) null;
      if (item.id == id)
        return item;
      if (item.children == null)
        return (BackendData.Foo) null;
      foreach (BackendData.Foo child in item.children)
      {
        BackendData.Foo itemRecursive = BackendData.FindItemRecursive(child, id);
        if (itemRecursive != null)
          return itemRecursive;
      }
      return (BackendData.Foo) null;
    }

    public class Foo
    {
      public Foo(string name, int depth, int id)
      {
        this.name = name;
        this.depth = depth;
        this.id = id;
      }

      public string name { get; set; }

      public int id { get; set; }

      public int depth { get; set; }

      public BackendData.Foo parent { get; set; }

      public List<BackendData.Foo> children { get; set; }

      public bool hasChildren
      {
        get
        {
          return this.children != null && this.children.Count > 0;
        }
      }
    }
  }
}
