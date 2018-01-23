// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerClipTreeViewState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AudioProfilerClipTreeViewState : TreeViewState
  {
    [SerializeField]
    public int selectedColumn = 2;
    [SerializeField]
    public int prevSelectedColumn = 1;
    [SerializeField]
    public bool sortByDescendingOrder = true;
    [SerializeField]
    public float[] columnWidths;

    public void SetSelectedColumn(int index)
    {
      if (index != this.selectedColumn)
        this.prevSelectedColumn = this.selectedColumn;
      else
        this.sortByDescendingOrder = !this.sortByDescendingOrder;
      this.selectedColumn = index;
    }
  }
}
