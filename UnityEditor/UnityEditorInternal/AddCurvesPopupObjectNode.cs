// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AddCurvesPopupObjectNode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;

namespace UnityEditorInternal
{
  internal class AddCurvesPopupObjectNode : TreeViewItem
  {
    public AddCurvesPopupObjectNode(TreeViewItem parent, string path, string className)
      : base((path + className).GetHashCode(), parent == null ? -1 : parent.depth + 1, parent, className)
    {
    }
  }
}
