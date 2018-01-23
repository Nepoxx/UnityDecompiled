// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerClipView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AudioProfilerClipView
  {
    private TreeViewController m_TreeView;
    private AudioProfilerClipTreeViewState m_TreeViewState;
    private EditorWindow m_EditorWindow;
    private AudioProfilerClipView.AudioProfilerClipViewColumnHeader m_ColumnHeader;
    private AudioProfilerClipViewBackend m_Backend;
    private GUIStyle m_HeaderStyle;

    public AudioProfilerClipView(EditorWindow editorWindow, AudioProfilerClipTreeViewState state)
    {
      this.m_EditorWindow = editorWindow;
      this.m_TreeViewState = state;
    }

    public int GetNumItemsInData()
    {
      return this.m_Backend.items.Count;
    }

    public void Init(Rect rect, AudioProfilerClipViewBackend backend)
    {
      if (this.m_HeaderStyle == null)
        this.m_HeaderStyle = new GUIStyle((GUIStyle) "OL title");
      this.m_HeaderStyle.alignment = TextAnchor.MiddleLeft;
      if (this.m_TreeView != null)
        return;
      this.m_Backend = backend;
      if (this.m_TreeViewState.columnWidths == null || this.m_TreeViewState.columnWidths.Length == 0)
      {
        int length = AudioProfilerClipInfoHelper.GetLastColumnIndex() + 1;
        this.m_TreeViewState.columnWidths = new float[length];
        for (int index1 = 0; index1 < length; ++index1)
        {
          float[] columnWidths = this.m_TreeViewState.columnWidths;
          int index2 = index1;
          int num1;
          switch (index1)
          {
            case 0:
              num1 = 300;
              break;
            case 2:
              num1 = 110;
              break;
            default:
              num1 = 80;
              break;
          }
          double num2 = (double) num1;
          columnWidths[index2] = (float) num2;
        }
      }
      this.m_TreeView = new TreeViewController(this.m_EditorWindow, (TreeViewState) this.m_TreeViewState);
      ITreeViewGUI gui = (ITreeViewGUI) new AudioProfilerClipView.AudioProfilerClipViewGUI(this.m_TreeView);
      ITreeViewDataSource data = (ITreeViewDataSource) new AudioProfilerClipView.AudioProfilerDataSource(this.m_TreeView, this.m_Backend);
      this.m_TreeView.Init(rect, data, gui, (ITreeViewDragging) null);
      this.m_ColumnHeader = new AudioProfilerClipView.AudioProfilerClipViewColumnHeader(this.m_TreeViewState, this.m_Backend);
      this.m_ColumnHeader.columnWidths = this.m_TreeViewState.columnWidths;
      this.m_ColumnHeader.minColumnWidth = 30f;
      this.m_TreeView.selectionChangedCallback += new Action<int[]>(this.OnTreeSelectionChanged);
    }

    public void OnTreeSelectionChanged(int[] selection)
    {
      if (selection.Length != 1)
        return;
      AudioProfilerClipView.AudioProfilerClipTreeViewItem clipTreeViewItem = this.m_TreeView.FindItem(selection[0]) as AudioProfilerClipView.AudioProfilerClipTreeViewItem;
      if (clipTreeViewItem != null)
        EditorGUIUtility.PingObject(clipTreeViewItem.info.info.assetInstanceId);
    }

    public void OnGUI(Rect rect)
    {
      int controlId = GUIUtility.GetControlID(FocusType.Keyboard, rect);
      Rect rect1 = new Rect(rect.x, rect.y, rect.width, this.m_HeaderStyle.fixedHeight);
      GUI.Label(rect1, "", this.m_HeaderStyle);
      this.m_ColumnHeader.OnGUI(rect1, true, this.m_HeaderStyle);
      rect.y += rect1.height;
      rect.height -= rect1.height;
      this.m_TreeView.OnEvent();
      this.m_TreeView.OnGUI(rect, controlId);
    }

    internal class AudioProfilerClipTreeViewItem : TreeViewItem
    {
      public AudioProfilerClipTreeViewItem(int id, int depth, TreeViewItem parent, string displayName, AudioProfilerClipInfoWrapper info)
        : base(id, depth, parent, displayName)
      {
        this.info = info;
      }

      public AudioProfilerClipInfoWrapper info { get; set; }
    }

    internal class AudioProfilerDataSource : TreeViewDataSource
    {
      private AudioProfilerClipViewBackend m_Backend;

      public AudioProfilerDataSource(TreeViewController treeView, AudioProfilerClipViewBackend backend)
        : base(treeView)
      {
        this.m_Backend = backend;
        this.m_Backend.OnUpdate = new AudioProfilerClipViewBackend.DataUpdateDelegate(((TreeViewDataSource) this).FetchData);
        this.showRootItem = false;
        this.rootIsCollapsable = false;
        this.FetchData();
      }

      private void FillTreeItems(AudioProfilerClipView.AudioProfilerClipTreeViewItem parentNode, int depth, int parentId, List<AudioProfilerClipInfoWrapper> items)
      {
        parentNode.children = new List<TreeViewItem>(items.Count);
        int num = 1;
        foreach (AudioProfilerClipInfoWrapper info in items)
        {
          AudioProfilerClipView.AudioProfilerClipTreeViewItem clipTreeViewItem = new AudioProfilerClipView.AudioProfilerClipTreeViewItem(++num, 1, (TreeViewItem) parentNode, info.assetName, info);
          parentNode.children.Add((TreeViewItem) clipTreeViewItem);
        }
      }

      public override void FetchData()
      {
        AudioProfilerClipView.AudioProfilerClipTreeViewItem parentNode = new AudioProfilerClipView.AudioProfilerClipTreeViewItem(1, 0, (TreeViewItem) null, "ROOT", new AudioProfilerClipInfoWrapper(new AudioProfilerClipInfo(), "ROOT"));
        this.FillTreeItems(parentNode, 1, 0, this.m_Backend.items);
        this.m_RootItem = (TreeViewItem) parentNode;
        this.SetExpandedWithChildren(this.m_RootItem, true);
        this.m_NeedRefreshRows = true;
      }

      public override bool CanBeParent(TreeViewItem item)
      {
        return item.hasChildren;
      }

      public override bool IsRenamingItemAllowed(TreeViewItem item)
      {
        return false;
      }
    }

    internal class AudioProfilerClipViewColumnHeader
    {
      private string[] headers = new string[6]{ "Asset", "Load State", "Internal Load State", "Age", "Disposed", "Num Voices" };
      private AudioProfilerClipTreeViewState m_TreeViewState;
      private AudioProfilerClipViewBackend m_Backend;

      public AudioProfilerClipViewColumnHeader(AudioProfilerClipTreeViewState state, AudioProfilerClipViewBackend backend)
      {
        this.m_TreeViewState = state;
        this.m_Backend = backend;
        this.minColumnWidth = 10f;
        this.dragWidth = 6f;
      }

      public float[] columnWidths { get; set; }

      public float minColumnWidth { get; set; }

      public float dragWidth { get; set; }

      public void OnGUI(Rect rect, bool allowSorting, GUIStyle headerStyle)
      {
        GUI.BeginClip(rect);
        float x1 = -this.m_TreeViewState.scrollPos.x;
        int lastColumnIndex = AudioProfilerClipInfoHelper.GetLastColumnIndex();
        for (int index = 0; index <= lastColumnIndex; ++index)
        {
          Rect position1 = new Rect(x1, 0.0f, this.columnWidths[index], rect.height - 1f);
          x1 += this.columnWidths[index];
          Rect position2 = new Rect(x1 - this.dragWidth / 2f, 0.0f, 3f, rect.height);
          float x2 = EditorGUI.MouseDeltaReader(position2, true).x;
          if ((double) x2 != 0.0)
          {
            this.columnWidths[index] += x2;
            this.columnWidths[index] = Mathf.Max(this.columnWidths[index], this.minColumnWidth);
          }
          string header = this.headers[index];
          if (allowSorting && index == this.m_TreeViewState.selectedColumn)
            header += !this.m_TreeViewState.sortByDescendingOrder ? " ▲" : " ▼";
          GUI.Box(position1, header, headerStyle);
          if (allowSorting && Event.current.type == EventType.MouseDown && position1.Contains(Event.current.mousePosition))
          {
            this.m_TreeViewState.SetSelectedColumn(index);
            this.m_Backend.UpdateSorting();
          }
          if (Event.current.type == EventType.Repaint)
            EditorGUIUtility.AddCursorRect(position2, MouseCursor.SplitResizeLeftRight);
        }
        GUI.EndClip();
      }
    }

    internal class AudioProfilerClipViewGUI : TreeViewGUI
    {
      public AudioProfilerClipViewGUI(TreeViewController treeView)
        : base(treeView)
      {
        this.k_IconWidth = 0.0f;
      }

      protected override Texture GetIconForItem(TreeViewItem item)
      {
        return (Texture) null;
      }

      protected override void RenameEnded()
      {
      }

      protected override void SyncFakeItem()
      {
      }

      public override Vector2 GetTotalSize()
      {
        Vector2 totalSize = base.GetTotalSize();
        totalSize.x = 0.0f;
        foreach (float columnWidth in this.columnWidths)
          totalSize.x += columnWidth;
        return totalSize;
      }

      protected override void OnContentGUI(Rect rect, int row, TreeViewItem item, string label, bool selected, bool focused, bool useBoldFont, bool isPinging)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        GUIStyle guiStyle = !useBoldFont ? TreeViewGUI.Styles.lineStyle : TreeViewGUI.Styles.lineBoldStyle;
        guiStyle.alignment = TextAnchor.MiddleLeft;
        guiStyle.padding.left = 0;
        int num = 2;
        base.OnContentGUI(new Rect(rect.x, rect.y, this.columnWidths[0] - (float) num, rect.height), row, item, label, selected, focused, useBoldFont, isPinging);
        rect.x += this.columnWidths[0] + (float) num;
        AudioProfilerClipView.AudioProfilerClipTreeViewItem clipTreeViewItem = item as AudioProfilerClipView.AudioProfilerClipTreeViewItem;
        for (int index = 1; index < this.columnWidths.Length; ++index)
        {
          rect.width = this.columnWidths[index] - (float) (2 * num);
          guiStyle.alignment = TextAnchor.MiddleRight;
          guiStyle.Draw(rect, AudioProfilerClipInfoHelper.GetColumnString(clipTreeViewItem.info, (AudioProfilerClipInfoHelper.ColumnIndices) index), false, false, selected, focused);
          Handles.color = Color.black;
          Handles.DrawLine(new Vector3((float) ((double) rect.x - (double) num + 1.0), rect.y, 0.0f), new Vector3((float) ((double) rect.x - (double) num + 1.0), rect.y + rect.height, 0.0f));
          rect.x += this.columnWidths[index];
        }
        guiStyle.alignment = TextAnchor.MiddleLeft;
      }

      private float[] columnWidths
      {
        get
        {
          return ((AudioProfilerClipTreeViewState) this.m_TreeView.state).columnWidths;
        }
      }
    }
  }
}
