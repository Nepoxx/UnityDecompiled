// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.Debugger.VisualTreeTreeView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.Debugger
{
  internal class VisualTreeTreeView : TreeView
  {
    public Panel panel;
    public bool includeShadowHierarchy;

    public VisualTreeTreeView(TreeViewState state)
      : base(state)
    {
    }

    protected override TreeViewItem BuildRoot()
    {
      TreeViewItem tree = new TreeViewItem(0, -1);
      this.Recurse(tree, this.panel.visualTree);
      return tree;
    }

    private void Recurse(TreeViewItem tree, VisualElement elt)
    {
      VisualTreeItem visualTreeItem = new VisualTreeItem(elt, tree.depth + 1);
      tree.AddChild((TreeViewItem) visualTreeItem);
      foreach (VisualElement elt1 in !this.includeShadowHierarchy ? elt.Children() : elt.shadow.Children())
        this.Recurse((TreeViewItem) visualTreeItem, elt1);
    }

    public VisualTreeItem GetNodeFor(int selectedId)
    {
      return this.FindRows((IList<int>) new List<int>() { selectedId }).FirstOrDefault<TreeViewItem>() as VisualTreeItem;
    }
  }
}
