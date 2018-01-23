// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.TreeViewDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  internal abstract class TreeViewDragging : ITreeViewDragging
  {
    protected TreeViewDragging.DropData m_DropData = new TreeViewDragging.DropData();
    protected TreeViewController m_TreeView;
    private const double k_DropExpandTimeout = 0.7;

    public TreeViewDragging(TreeViewController treeView)
    {
      this.m_TreeView = treeView;
    }

    public virtual void OnInitialize()
    {
    }

    public int GetDropTargetControlID()
    {
      return this.m_DropData.dropTargetControlID;
    }

    public int GetRowMarkerControlID()
    {
      return this.m_DropData.rowMarkerControlID;
    }

    public bool drawRowMarkerAbove { get; set; }

    public virtual bool CanStartDrag(TreeViewItem targetItem, List<int> draggedItemIDs, Vector2 mouseDownPosition)
    {
      return true;
    }

    public abstract void StartDrag(TreeViewItem draggedItem, List<int> draggedItemIDs);

    public abstract DragAndDropVisualMode DoDrag(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPosition);

    protected float GetDropBetweenHalfHeight(TreeViewItem item, Rect itemRect)
    {
      return !this.m_TreeView.data.CanBeParent(item) ? itemRect.height * 0.5f : this.m_TreeView.gui.halfDropBetweenHeight;
    }

    protected bool TryGetDropPosition(TreeViewItem item, Rect itemRect, int row, out TreeViewDragging.DropPosition dropPosition)
    {
      Vector2 mousePosition = Event.current.mousePosition;
      if (itemRect.Contains(mousePosition))
      {
        float betweenHalfHeight = this.GetDropBetweenHalfHeight(item, itemRect);
        dropPosition = (double) mousePosition.y < (double) itemRect.yMax - (double) betweenHalfHeight ? ((double) mousePosition.y > (double) itemRect.yMin + (double) betweenHalfHeight ? TreeViewDragging.DropPosition.Upon : TreeViewDragging.DropPosition.Above) : TreeViewDragging.DropPosition.Below;
        return true;
      }
      float num = this.m_TreeView.gui.halfDropBetweenHeight;
      int row1 = row + 1;
      if (row1 < this.m_TreeView.data.rowCount)
      {
        Rect rowRect = this.m_TreeView.gui.GetRowRect(row1, itemRect.width);
        num = !this.m_TreeView.data.CanBeParent(this.m_TreeView.data.GetItem(row1)) ? rowRect.height * 0.5f : this.m_TreeView.gui.halfDropBetweenHeight;
      }
      Rect rect1 = itemRect;
      rect1.y = itemRect.yMax;
      rect1.height = num;
      if (rect1.Contains(mousePosition))
      {
        dropPosition = TreeViewDragging.DropPosition.Below;
        return true;
      }
      if (row == 0)
      {
        Rect rect2 = itemRect;
        rect2.yMin -= this.m_TreeView.gui.halfDropBetweenHeight;
        rect2.height = this.m_TreeView.gui.halfDropBetweenHeight;
        if (rect2.Contains(mousePosition))
        {
          dropPosition = TreeViewDragging.DropPosition.Above;
          return true;
        }
      }
      dropPosition = TreeViewDragging.DropPosition.Below;
      return false;
    }

    public virtual bool DragElement(TreeViewItem targetItem, Rect targetItemRect, int row)
    {
      bool perform = Event.current.type == EventType.DragPerform;
      if (targetItem == null)
      {
        if (this.m_DropData != null)
        {
          this.m_DropData.dropTargetControlID = 0;
          this.m_DropData.rowMarkerControlID = 0;
        }
        DragAndDrop.visualMode = this.DoDrag((TreeViewItem) null, (TreeViewItem) null, perform, TreeViewDragging.DropPosition.Below);
        if (DragAndDrop.visualMode != DragAndDropVisualMode.None && perform)
          this.FinalizeDragPerformed(true);
        return false;
      }
      TreeViewDragging.DropPosition dropPosition;
      if (!this.TryGetDropPosition(targetItem, targetItemRect, row, out dropPosition))
        return false;
      TreeViewItem parentItem = (TreeViewItem) null;
      switch (dropPosition)
      {
        case TreeViewDragging.DropPosition.Upon:
          parentItem = targetItem;
          break;
        case TreeViewDragging.DropPosition.Below:
          if (this.m_TreeView.data.IsExpanded(targetItem) && targetItem.hasChildren)
          {
            parentItem = targetItem;
            targetItem = targetItem.children[0];
            dropPosition = TreeViewDragging.DropPosition.Above;
            break;
          }
          parentItem = targetItem.parent;
          break;
        case TreeViewDragging.DropPosition.Above:
          parentItem = targetItem.parent;
          break;
      }
      DragAndDropVisualMode andDropVisualMode1 = DragAndDropVisualMode.None;
      if (perform)
      {
        if (dropPosition == TreeViewDragging.DropPosition.Upon)
          andDropVisualMode1 = this.DoDrag(targetItem, targetItem, true, dropPosition);
        if (andDropVisualMode1 == DragAndDropVisualMode.None && parentItem != null)
          andDropVisualMode1 = this.DoDrag(parentItem, targetItem, true, dropPosition);
        if (andDropVisualMode1 != DragAndDropVisualMode.None)
        {
          this.FinalizeDragPerformed(false);
        }
        else
        {
          this.DragCleanup(true);
          this.m_TreeView.NotifyListenersThatDragEnded((int[]) null, false);
        }
      }
      else
      {
        if (this.m_DropData == null)
          this.m_DropData = new TreeViewDragging.DropData();
        this.m_DropData.dropTargetControlID = 0;
        this.m_DropData.rowMarkerControlID = 0;
        int itemControlId = TreeViewController.GetItemControlID(targetItem);
        this.HandleAutoExpansion(itemControlId, targetItem, targetItemRect);
        if (dropPosition == TreeViewDragging.DropPosition.Upon)
          andDropVisualMode1 = this.DoDrag(targetItem, targetItem, false, dropPosition);
        if (andDropVisualMode1 != DragAndDropVisualMode.None)
        {
          this.m_DropData.dropTargetControlID = itemControlId;
          DragAndDrop.visualMode = andDropVisualMode1;
        }
        else if (targetItem != null && parentItem != null)
        {
          DragAndDropVisualMode andDropVisualMode2 = this.DoDrag(parentItem, targetItem, false, dropPosition);
          if (andDropVisualMode2 != DragAndDropVisualMode.None)
          {
            this.drawRowMarkerAbove = dropPosition == TreeViewDragging.DropPosition.Above;
            this.m_DropData.rowMarkerControlID = itemControlId;
            DragAndDrop.visualMode = andDropVisualMode2;
          }
        }
      }
      Event.current.Use();
      return true;
    }

    private void FinalizeDragPerformed(bool revertExpanded)
    {
      this.DragCleanup(revertExpanded);
      DragAndDrop.AcceptDrag();
      List<UnityEngine.Object> objectList = new List<UnityEngine.Object>((IEnumerable<UnityEngine.Object>) DragAndDrop.objectReferences);
      bool draggedItemsFromOwnTreeView = true;
      if (objectList.Count > 0 && objectList[0] != (UnityEngine.Object) null && TreeViewUtility.FindItemInList<TreeViewItem>(objectList[0].GetInstanceID(), this.m_TreeView.data.GetRows()) == null)
        draggedItemsFromOwnTreeView = false;
      int[] draggedIDs = new int[objectList.Count];
      for (int index = 0; index < objectList.Count; ++index)
      {
        if (!(objectList[index] == (UnityEngine.Object) null))
          draggedIDs[index] = objectList[index].GetInstanceID();
      }
      this.m_TreeView.NotifyListenersThatDragEnded(draggedIDs, draggedItemsFromOwnTreeView);
    }

    protected virtual void HandleAutoExpansion(int itemControlID, TreeViewItem targetItem, Rect targetItemRect)
    {
      Vector2 mousePosition = Event.current.mousePosition;
      float contentIndent = this.m_TreeView.gui.GetContentIndent(targetItem);
      float betweenHalfHeight = this.GetDropBetweenHalfHeight(targetItem, targetItemRect);
      bool flag1 = new Rect(targetItemRect.x + contentIndent, targetItemRect.y + betweenHalfHeight, targetItemRect.width - contentIndent, targetItemRect.height - betweenHalfHeight * 2f).Contains(mousePosition);
      if (itemControlID != this.m_DropData.lastControlID || !flag1 || this.m_DropData.expandItemBeginPosition != mousePosition)
      {
        this.m_DropData.lastControlID = itemControlID;
        this.m_DropData.expandItemBeginTimer = (double) Time.realtimeSinceStartup;
        this.m_DropData.expandItemBeginPosition = mousePosition;
      }
      bool flag2 = (double) Time.realtimeSinceStartup - this.m_DropData.expandItemBeginTimer > 0.7;
      bool flag3 = flag1 && flag2;
      if (targetItem == null || !flag3 || (!targetItem.hasChildren || this.m_TreeView.data.IsExpanded(targetItem)))
        return;
      if (this.m_DropData.expandedArrayBeforeDrag == null)
        this.m_DropData.expandedArrayBeforeDrag = this.GetCurrentExpanded().ToArray();
      this.m_TreeView.data.SetExpanded(targetItem, true);
      this.m_DropData.expandItemBeginTimer = (double) Time.realtimeSinceStartup;
      this.m_DropData.lastControlID = 0;
    }

    public virtual void DragCleanup(bool revertExpanded)
    {
      if (this.m_DropData == null)
        return;
      if (this.m_DropData.expandedArrayBeforeDrag != null && revertExpanded)
        this.RestoreExpanded(new List<int>((IEnumerable<int>) this.m_DropData.expandedArrayBeforeDrag));
      this.m_DropData = new TreeViewDragging.DropData();
    }

    public List<int> GetCurrentExpanded()
    {
      return this.m_TreeView.data.GetRows().Where<TreeViewItem>((Func<TreeViewItem, bool>) (item => this.m_TreeView.data.IsExpanded(item))).Select<TreeViewItem, int>((Func<TreeViewItem, int>) (item => item.id)).ToList<int>();
    }

    public void RestoreExpanded(List<int> ids)
    {
      foreach (TreeViewItem row in (IEnumerable<TreeViewItem>) this.m_TreeView.data.GetRows())
        this.m_TreeView.data.SetExpanded(row, ids.Contains(row.id));
    }

    internal static int GetInsertionIndex(TreeViewItem parentItem, TreeViewItem targetItem, TreeViewDragging.DropPosition dropPosition)
    {
      if (parentItem == null)
        return -1;
      int num1;
      if (parentItem == targetItem)
      {
        num1 = -1;
      }
      else
      {
        int num2 = parentItem.children.IndexOf(targetItem);
        if (num2 >= 0)
        {
          num1 = dropPosition != TreeViewDragging.DropPosition.Below ? num2 : num2 + 1;
        }
        else
        {
          Debug.LogError((object) "Did not find targetItem,; should be a child of parentItem");
          num1 = -1;
        }
      }
      return num1;
    }

    protected class DropData
    {
      public int lastControlID = -1;
      public int dropTargetControlID = -1;
      public int rowMarkerControlID = -1;
      public int[] expandedArrayBeforeDrag;
      public double expandItemBeginTimer;
      public Vector2 expandItemBeginPosition;
    }

    public enum DropPosition
    {
      Upon,
      Below,
      Above,
    }
  }
}
