// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewForAudioMixerGroup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.Audio;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal static class TreeViewForAudioMixerGroup
  {
    private static readonly int kNoneItemID = 0;
    private static string s_NoneText = "None";

    public static void CreateAndSetTreeView(ObjectTreeForSelector.TreeSelectorData data)
    {
      AudioMixerController objectFromInstanceId = InternalEditorUtility.GetObjectFromInstanceID(data.userData) as AudioMixerController;
      TreeViewController treeView = new TreeViewController(data.editorWindow, data.state);
      TreeViewForAudioMixerGroup.GroupTreeViewGUI groupTreeViewGui = new TreeViewForAudioMixerGroup.GroupTreeViewGUI(treeView);
      TreeViewForAudioMixerGroup.TreeViewDataSourceForMixers dataSourceForMixers1 = new TreeViewForAudioMixerGroup.TreeViewDataSourceForMixers(treeView, objectFromInstanceId);
      TreeViewForAudioMixerGroup.TreeViewDataSourceForMixers dataSourceForMixers2 = dataSourceForMixers1;
      dataSourceForMixers2.onVisibleRowsChanged = dataSourceForMixers2.onVisibleRowsChanged + new Action(groupTreeViewGui.CalculateRowRects);
      treeView.deselectOnUnhandledMouseDown = false;
      treeView.Init(data.treeViewRect, (ITreeViewDataSource) dataSourceForMixers1, (ITreeViewGUI) groupTreeViewGui, (ITreeViewDragging) null);
      data.objectTreeForSelector.SetTreeView(treeView);
    }

    private class GroupTreeViewGUI : TreeViewGUI
    {
      private readonly Texture2D k_AudioGroupIcon = EditorGUIUtility.FindTexture("AudioMixerGroup Icon");
      private readonly Texture2D k_AudioListenerIcon = EditorGUIUtility.FindTexture("AudioListener Icon");
      private List<Rect> m_RowRects = new List<Rect>();
      private const float k_SpaceBetween = 25f;
      private const float k_HeaderHeight = 20f;

      public GroupTreeViewGUI(TreeViewController treeView)
        : base(treeView)
      {
      }

      public override Rect GetRowRect(int row, float rowWidth)
      {
        if (this.m_TreeView.isSearching)
          return base.GetRowRect(row, rowWidth);
        if (this.m_TreeView.data.rowCount != this.m_RowRects.Count)
          this.CalculateRowRects();
        return this.m_RowRects[row];
      }

      public override void OnRowGUI(Rect rowRect, TreeViewItem item, int row, bool selected, bool focused)
      {
        if (this.m_TreeView.isSearching)
        {
          base.OnRowGUI(rowRect, item, row, selected, focused);
        }
        else
        {
          this.DoItemGUI(rowRect, row, item, selected, focused, false);
          if (item.parent != this.m_TreeView.data.root || item.id == TreeViewForAudioMixerGroup.kNoneItemID)
            return;
          AudioMixerController controller = ((TreeViewForAudioMixerGroup.MixerTreeViewItem) item).group.controller;
          GUI.Label(new Rect(rowRect.x + 2f, rowRect.y - 18f, rowRect.width, 18f), GUIContent.Temp(controller.name), EditorStyles.boldLabel);
        }
      }

      protected override Texture GetIconForItem(TreeViewItem item)
      {
        if (item != null && (UnityEngine.Object) item.icon != (UnityEngine.Object) null)
          return (Texture) item.icon;
        if (item.id == TreeViewForAudioMixerGroup.kNoneItemID)
          return (Texture) this.k_AudioListenerIcon;
        return (Texture) this.k_AudioGroupIcon;
      }

      protected override void SyncFakeItem()
      {
      }

      protected override void RenameEnded()
      {
      }

      private bool IsController(TreeViewItem item)
      {
        return item.parent == this.m_TreeView.data.root && item.id != TreeViewForAudioMixerGroup.kNoneItemID;
      }

      public void CalculateRowRects()
      {
        if (this.m_TreeView.isSearching)
          return;
        float width = GUIClip.visibleRect.width;
        IList<TreeViewItem> rows = this.m_TreeView.data.GetRows();
        this.m_RowRects = new List<Rect>(rows.Count);
        float num1 = 2f;
        for (int index = 0; index < rows.Count; ++index)
        {
          float num2 = !this.IsController(rows[index]) ? 0.0f : 25f;
          float y = num1 + num2;
          float kLineHeight = this.k_LineHeight;
          this.m_RowRects.Add(new Rect(0.0f, y, width, kLineHeight));
          num1 = y + kLineHeight;
        }
      }

      public override Vector2 GetTotalSize()
      {
        if (this.m_TreeView.isSearching)
        {
          Vector2 totalSize = base.GetTotalSize();
          totalSize.x = 1f;
          return totalSize;
        }
        if (this.m_RowRects.Count == 0)
          return new Vector2(1f, 1f);
        return new Vector2(1f, this.m_RowRects[this.m_RowRects.Count - 1].yMax);
      }

      public override int GetNumRowsOnPageUpDown(TreeViewItem fromItem, bool pageUp, float heightOfTreeView)
      {
        if (this.m_TreeView.isSearching)
          return base.GetNumRowsOnPageUpDown(fromItem, pageUp, heightOfTreeView);
        return (int) Mathf.Floor(heightOfTreeView / this.k_LineHeight);
      }

      public override void GetFirstAndLastRowVisible(out int firstRowVisible, out int lastRowVisible)
      {
        if (this.m_TreeView.isSearching)
        {
          base.GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible);
        }
        else
        {
          int rowCount = this.m_TreeView.data.rowCount;
          if (rowCount != this.m_RowRects.Count)
            Debug.LogError((object) "Mismatch in state: rows vs cached rects");
          int num1 = -1;
          int num2 = -1;
          float y = this.m_TreeView.state.scrollPos.y;
          float height = this.m_TreeView.GetTotalRect().height;
          for (int index = 0; index < this.m_RowRects.Count; ++index)
          {
            if ((double) this.m_RowRects[index].y > (double) y && (double) this.m_RowRects[index].y < (double) y + (double) height || (double) this.m_RowRects[index].yMax > (double) y && (double) this.m_RowRects[index].yMax < (double) y + (double) height)
            {
              if (num1 == -1)
                num1 = index;
              num2 = index;
            }
          }
          if (num1 != -1 && num2 != -1)
          {
            firstRowVisible = num1;
            lastRowVisible = num2;
          }
          else
          {
            firstRowVisible = 0;
            lastRowVisible = rowCount - 1;
          }
        }
      }
    }

    private class MixerTreeViewItem : TreeViewItem
    {
      public MixerTreeViewItem(int id, int depth, TreeViewItem parent, string displayName, AudioMixerGroupController groupController)
        : base(id, depth, parent, displayName)
      {
        this.group = groupController;
      }

      public AudioMixerGroupController group { get; set; }
    }

    private class TreeViewDataSourceForMixers : TreeViewDataSource
    {
      public TreeViewDataSourceForMixers(TreeViewController treeView, AudioMixerController ignoreController)
        : base(treeView)
      {
        this.showRootItem = false;
        this.rootIsCollapsable = false;
        this.ignoreThisController = ignoreController;
        this.alwaysAddFirstItemToSearchResult = true;
      }

      public AudioMixerController ignoreThisController { get; private set; }

      private bool ShouldShowController(AudioMixerController controller, List<int> allowedInstanceIDs)
      {
        if (!(bool) ((UnityEngine.Object) controller))
          return false;
        if (allowedInstanceIDs != null && allowedInstanceIDs.Count > 0)
          return allowedInstanceIDs.Contains(controller.GetInstanceID());
        return true;
      }

      public override void FetchData()
      {
        this.m_RootItem = new TreeViewItem(1010101010, -1, (TreeViewItem) null, "InvisibleRoot");
        this.SetExpanded(this.m_RootItem.id, true);
        List<int> allowedInstanceIds = ObjectSelector.get.allowedInstanceIDs;
        HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
        hierarchyProperty.SetSearchFilter(new SearchFilter()
        {
          classNames = new string[1]
          {
            "AudioMixerController"
          }
        });
        List<AudioMixerController> audioMixerControllerList = new List<AudioMixerController>();
        while (hierarchyProperty.Next((int[]) null))
        {
          AudioMixerController pptrValue = hierarchyProperty.pptrValue as AudioMixerController;
          if (this.ShouldShowController(pptrValue, allowedInstanceIds))
            audioMixerControllerList.Add(pptrValue);
        }
        List<TreeViewItem> treeViewItemList = new List<TreeViewItem>();
        treeViewItemList.Add(new TreeViewItem(TreeViewForAudioMixerGroup.kNoneItemID, 0, this.m_RootItem, TreeViewForAudioMixerGroup.s_NoneText));
        foreach (AudioMixerController controller in audioMixerControllerList)
          treeViewItemList.Add(this.BuildSubTree(controller));
        this.m_RootItem.children = treeViewItemList;
        if (audioMixerControllerList.Count == 1)
          this.m_TreeView.data.SetExpandedWithChildren(this.m_RootItem, true);
        this.m_NeedRefreshRows = true;
      }

      private TreeViewItem BuildSubTree(AudioMixerController controller)
      {
        AudioMixerGroupController masterGroup = controller.masterGroup;
        TreeViewForAudioMixerGroup.MixerTreeViewItem mixerTreeViewItem = new TreeViewForAudioMixerGroup.MixerTreeViewItem(masterGroup.GetInstanceID(), 0, this.m_RootItem, masterGroup.name, masterGroup);
        this.AddChildrenRecursive(masterGroup, (TreeViewItem) mixerTreeViewItem);
        return (TreeViewItem) mixerTreeViewItem;
      }

      private void AddChildrenRecursive(AudioMixerGroupController group, TreeViewItem item)
      {
        item.children = new List<TreeViewItem>(group.children.Length);
        for (int index = 0; index < group.children.Length; ++index)
        {
          item.children.Add((TreeViewItem) new TreeViewForAudioMixerGroup.MixerTreeViewItem(group.children[index].GetInstanceID(), item.depth + 1, item, group.children[index].name, group.children[index]));
          this.AddChildrenRecursive(group.children[index], item.children[index]);
        }
      }

      public override bool CanBeMultiSelected(TreeViewItem item)
      {
        return false;
      }

      public override bool IsRenamingItemAllowed(TreeViewItem item)
      {
        return false;
      }
    }
  }
}
