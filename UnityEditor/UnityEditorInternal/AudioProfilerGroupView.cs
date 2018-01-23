// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerGroupView
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
  internal class AudioProfilerGroupView
  {
    private TreeViewController m_TreeView;
    private AudioProfilerGroupTreeViewState m_TreeViewState;
    private EditorWindow m_EditorWindow;
    private AudioProfilerGroupView.AudioProfilerGroupViewColumnHeader m_ColumnHeader;
    private AudioProfilerGroupViewBackend m_Backend;
    private GUIStyle m_HeaderStyle;
    private int delayedPingObject;

    public AudioProfilerGroupView(EditorWindow editorWindow, AudioProfilerGroupTreeViewState state)
    {
      this.m_EditorWindow = editorWindow;
      this.m_TreeViewState = state;
    }

    public int GetNumItemsInData()
    {
      return this.m_Backend.items.Count;
    }

    public void Init(Rect rect, AudioProfilerGroupViewBackend backend)
    {
      if (this.m_HeaderStyle == null)
        this.m_HeaderStyle = new GUIStyle((GUIStyle) "OL title");
      this.m_HeaderStyle.alignment = TextAnchor.MiddleLeft;
      if (this.m_TreeView != null)
        return;
      this.m_Backend = backend;
      if (this.m_TreeViewState.columnWidths == null || this.m_TreeViewState.columnWidths.Length == 0)
      {
        int length = AudioProfilerGroupInfoHelper.GetLastColumnIndex() + 1;
        this.m_TreeViewState.columnWidths = new float[length];
        for (int index = 2; index < length; ++index)
          this.m_TreeViewState.columnWidths[index] = index == 2 || index == 3 || index >= 11 && index <= 16 ? 75f : 60f;
        this.m_TreeViewState.columnWidths[0] = 140f;
        this.m_TreeViewState.columnWidths[1] = 140f;
      }
      this.m_TreeView = new TreeViewController(this.m_EditorWindow, (TreeViewState) this.m_TreeViewState);
      ITreeViewGUI gui = (ITreeViewGUI) new AudioProfilerGroupView.AudioProfilerGroupViewGUI(this.m_TreeView);
      ITreeViewDataSource data = (ITreeViewDataSource) new AudioProfilerGroupView.AudioProfilerDataSource(this.m_TreeView, this.m_Backend);
      this.m_TreeView.Init(rect, data, gui, (ITreeViewDragging) null);
      this.m_ColumnHeader = new AudioProfilerGroupView.AudioProfilerGroupViewColumnHeader(this.m_TreeViewState, this.m_Backend);
      this.m_ColumnHeader.columnWidths = this.m_TreeViewState.columnWidths;
      this.m_ColumnHeader.minColumnWidth = 30f;
      this.m_TreeView.selectionChangedCallback += new Action<int[]>(this.OnTreeSelectionChanged);
    }

    private void PingObjectDelayed()
    {
      EditorGUIUtility.PingObject(this.delayedPingObject);
    }

    public void OnTreeSelectionChanged(int[] selection)
    {
      if (selection.Length != 1)
        return;
      AudioProfilerGroupView.AudioProfilerGroupTreeViewItem groupTreeViewItem = this.m_TreeView.FindItem(selection[0]) as AudioProfilerGroupView.AudioProfilerGroupTreeViewItem;
      if (groupTreeViewItem != null)
      {
        EditorGUIUtility.PingObject(groupTreeViewItem.info.info.assetInstanceId);
        this.delayedPingObject = groupTreeViewItem.info.info.objectInstanceId;
        EditorApplication.CallDelayed(new EditorApplication.CallbackFunction(this.PingObjectDelayed), 1f);
      }
    }

    public void OnGUI(Rect rect, bool allowSorting)
    {
      int controlId = GUIUtility.GetControlID(FocusType.Keyboard, rect);
      Rect rect1 = new Rect(rect.x, rect.y, rect.width, this.m_HeaderStyle.fixedHeight);
      GUI.Label(rect1, "", this.m_HeaderStyle);
      this.m_ColumnHeader.OnGUI(rect1, allowSorting, this.m_HeaderStyle);
      rect.y += rect1.height;
      rect.height -= rect1.height;
      this.m_TreeView.OnEvent();
      this.m_TreeView.OnGUI(rect, controlId);
    }

    internal class AudioProfilerGroupTreeViewItem : TreeViewItem
    {
      public AudioProfilerGroupTreeViewItem(int id, int depth, TreeViewItem parent, string displayName, AudioProfilerGroupInfoWrapper info)
        : base(id, depth, parent, displayName)
      {
        this.info = info;
      }

      public AudioProfilerGroupInfoWrapper info { get; set; }
    }

    internal class AudioProfilerDataSource : TreeViewDataSource
    {
      private AudioProfilerGroupViewBackend m_Backend;

      public AudioProfilerDataSource(TreeViewController treeView, AudioProfilerGroupViewBackend backend)
        : base(treeView)
      {
        this.m_Backend = backend;
        this.m_Backend.OnUpdate = new AudioProfilerGroupViewBackend.DataUpdateDelegate(((TreeViewDataSource) this).FetchData);
        this.showRootItem = false;
        this.rootIsCollapsable = false;
        this.FetchData();
      }

      private void FillTreeItems(AudioProfilerGroupView.AudioProfilerGroupTreeViewItem parentNode, int depth, int parentId, List<AudioProfilerGroupInfoWrapper> items)
      {
        int capacity = 0;
        foreach (AudioProfilerGroupInfoWrapper groupInfoWrapper in items)
        {
          if (parentId == (!groupInfoWrapper.addToRoot ? groupInfoWrapper.info.parentId : 0))
            ++capacity;
        }
        if (capacity <= 0)
          return;
        parentNode.children = new List<TreeViewItem>(capacity);
        foreach (AudioProfilerGroupInfoWrapper info in items)
        {
          if (parentId == (!info.addToRoot ? info.info.parentId : 0))
          {
            AudioProfilerGroupView.AudioProfilerGroupTreeViewItem parentNode1 = new AudioProfilerGroupView.AudioProfilerGroupTreeViewItem(info.info.uniqueId, !info.addToRoot ? depth : 1, (TreeViewItem) parentNode, info.objectName, info);
            parentNode.children.Add((TreeViewItem) parentNode1);
            this.FillTreeItems(parentNode1, depth + 1, info.info.uniqueId, items);
          }
        }
      }

      public override void FetchData()
      {
        AudioProfilerGroupView.AudioProfilerGroupTreeViewItem parentNode = new AudioProfilerGroupView.AudioProfilerGroupTreeViewItem(1, 0, (TreeViewItem) null, "ROOT", new AudioProfilerGroupInfoWrapper(new AudioProfilerGroupInfo(), "ROOT", "ROOT", false));
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

    internal class AudioProfilerGroupViewColumnHeader
    {
      private string[] headers = new string[23]{ "Object", "Asset", "Volume", "Audibility", "Plays", "3D", "Paused", "Muted", "Virtual", "OneShot", "Looped", "Distance", "MinDist", "MaxDist", "Time", "Duration", "Frequency", "Stream", "Compressed", "NonBlocking", "User", "Memory", "MemoryPoint" };
      private AudioProfilerGroupTreeViewState m_TreeViewState;
      private AudioProfilerGroupViewBackend m_Backend;

      public AudioProfilerGroupViewColumnHeader(AudioProfilerGroupTreeViewState state, AudioProfilerGroupViewBackend backend)
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
        int lastColumnIndex = AudioProfilerGroupInfoHelper.GetLastColumnIndex();
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

    internal class AudioProfilerGroupViewGUI : TreeViewGUI
    {
      public AudioProfilerGroupViewGUI(TreeViewController treeView)
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
        TextAnchor alignment = guiStyle.alignment;
        guiStyle.alignment = TextAnchor.MiddleLeft;
        guiStyle.padding.left = 0;
        int num = 2;
        base.OnContentGUI(new Rect(rect.x, rect.y, this.columnWidths[0] - (float) num, rect.height), row, item, label, selected, focused, useBoldFont, isPinging);
        rect.x += this.columnWidths[0] + (float) num;
        AudioProfilerGroupView.AudioProfilerGroupTreeViewItem groupTreeViewItem = item as AudioProfilerGroupView.AudioProfilerGroupTreeViewItem;
        for (int index = 1; index < this.columnWidths.Length; ++index)
        {
          rect.width = this.columnWidths[index] - (float) (2 * num);
          guiStyle.Draw(rect, AudioProfilerGroupInfoHelper.GetColumnString(groupTreeViewItem.info, (AudioProfilerGroupInfoHelper.ColumnIndices) index), false, false, selected, focused);
          Handles.color = Color.black;
          Handles.DrawLine(new Vector3((float) ((double) rect.x - (double) num + 1.0), rect.y, 0.0f), new Vector3((float) ((double) rect.x - (double) num + 1.0), rect.y + rect.height, 0.0f));
          rect.x += this.columnWidths[index];
          guiStyle.alignment = TextAnchor.MiddleRight;
        }
        guiStyle.alignment = alignment;
      }

      private float[] columnWidths
      {
        get
        {
          return ((AudioProfilerGroupTreeViewState) this.m_TreeView.state).columnWidths;
        }
      }
    }
  }
}
