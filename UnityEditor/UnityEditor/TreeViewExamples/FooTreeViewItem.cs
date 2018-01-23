// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewExamples.FooTreeViewItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;

namespace UnityEditor.TreeViewExamples
{
  internal class FooTreeViewItem : TreeViewItem
  {
    public FooTreeViewItem(int id, int depth, TreeViewItem parent, string displayName, BackendData.Foo foo)
      : base(id, depth, parent, displayName)
    {
      this.foo = foo;
    }

    public BackendData.Foo foo { get; private set; }
  }
}
