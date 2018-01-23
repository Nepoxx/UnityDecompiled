// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.ITreeViewDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  internal interface ITreeViewDataSource
  {
    void OnInitialize();

    TreeViewItem root { get; }

    int rowCount { get; }

    void ReloadData();

    void InitIfNeeded();

    TreeViewItem FindItem(int id);

    int GetRow(int id);

    TreeViewItem GetItem(int row);

    IList<TreeViewItem> GetRows();

    bool IsRevealed(int id);

    void RevealItem(int id);

    void SetExpandedWithChildren(TreeViewItem item, bool expand);

    void SetExpanded(TreeViewItem item, bool expand);

    bool IsExpanded(TreeViewItem item);

    bool IsExpandable(TreeViewItem item);

    void SetExpandedWithChildren(int id, bool expand);

    int[] GetExpandedIDs();

    void SetExpandedIDs(int[] ids);

    bool SetExpanded(int id, bool expand);

    bool IsExpanded(int id);

    bool CanBeMultiSelected(TreeViewItem item);

    bool CanBeParent(TreeViewItem item);

    bool IsRenamingItemAllowed(TreeViewItem item);

    void InsertFakeItem(int id, int parentID, string name, Texture2D icon);

    void RemoveFakeItem();

    bool HasFakeItem();

    void OnSearchChanged();
  }
}
