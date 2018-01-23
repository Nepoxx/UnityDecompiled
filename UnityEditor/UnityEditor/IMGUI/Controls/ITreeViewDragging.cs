// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.ITreeViewDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  internal interface ITreeViewDragging
  {
    void OnInitialize();

    bool CanStartDrag(TreeViewItem targetItem, List<int> draggedItemIDs, Vector2 mouseDownPosition);

    void StartDrag(TreeViewItem draggedItem, List<int> draggedItemIDs);

    bool DragElement(TreeViewItem targetItem, Rect targetItemRect, int row);

    void DragCleanup(bool revertExpanded);

    int GetDropTargetControlID();

    int GetRowMarkerControlID();

    bool drawRowMarkerAbove { get; set; }
  }
}
