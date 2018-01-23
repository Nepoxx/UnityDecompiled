// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.Debugger.VisualTreeItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.Debugger
{
  internal class VisualTreeItem : TreeViewItem
  {
    public readonly VisualElement elt;

    public VisualTreeItem(VisualElement elt, int depth)
      : base((int) elt.controlid, depth, VisualTreeItem.GetDisplayName(elt))
    {
      this.elt = elt;
    }

    private static string GetDisplayName(VisualElement elt)
    {
      return elt.GetType().Name + " " + elt.name;
    }

    public uint controlId
    {
      get
      {
        return this.elt.controlid;
      }
    }
  }
}
