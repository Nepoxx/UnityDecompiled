// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.TreeViewItemAlphaNumericSort
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor.IMGUI.Controls
{
  internal class TreeViewItemAlphaNumericSort : IComparer<TreeViewItem>
  {
    public int Compare(TreeViewItem lhs, TreeViewItem rhs)
    {
      if (lhs == rhs)
        return 0;
      if (lhs == null)
        return -1;
      if (rhs == null)
        return 1;
      return EditorUtility.NaturalCompare(lhs.displayName, rhs.displayName);
    }
  }
}
