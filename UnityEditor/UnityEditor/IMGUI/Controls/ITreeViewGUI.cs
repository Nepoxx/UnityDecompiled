// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.ITreeViewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  internal interface ITreeViewGUI
  {
    void OnInitialize();

    Vector2 GetTotalSize();

    void GetFirstAndLastRowVisible(out int firstRowVisible, out int lastRowVisible);

    Rect GetRowRect(int row, float rowWidth);

    Rect GetRectForFraming(int row);

    int GetNumRowsOnPageUpDown(TreeViewItem fromItem, bool pageUp, float heightOfTreeView);

    void OnRowGUI(Rect rowRect, TreeViewItem item, int row, bool selected, bool focused);

    void BeginRowGUI();

    void EndRowGUI();

    void BeginPingItem(TreeViewItem item, float topPixelOfRow, float availableWidth);

    void EndPingItem();

    bool BeginRename(TreeViewItem item, float delay);

    void EndRename();

    Rect GetRenameRect(Rect rowRect, int row, TreeViewItem item);

    float GetContentIndent(TreeViewItem item);

    float halfDropBetweenHeight { get; }

    float topRowMargin { get; }

    float bottomRowMargin { get; }
  }
}
