// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.TreeViewController
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor.IMGUI.Controls
{
  internal class TreeViewController
  {
    private readonly TreeViewItemExpansionAnimator m_ExpansionAnimator = new TreeViewItemExpansionAnimator();
    private List<int> m_DragSelection = new List<int>();
    private bool m_UseScrollView = true;
    private bool m_AllowRenameOnMouseUp = true;
    private bool m_UseExpansionAnimation = EditorPrefs.GetBool("TreeViewExpansionAnimation", true);
    private GUIView m_GUIView;
    private AnimFloat m_FramingAnimFloat;
    private bool m_StopIteratingItems;
    internal const string kExpansionAnimationPrefKey = "TreeViewExpansionAnimation";
    private bool m_GrabKeyboardFocus;
    private Rect m_TotalRect;
    private Rect m_VisibleRect;
    private Rect m_ContentRect;
    private bool m_HadFocusLastEvent;
    private int m_KeyboardControlID;
    private const float kSpaceForScrollBar = 16f;

    public TreeViewController(EditorWindow editorWindow, TreeViewState treeViewState)
    {
      this.m_GUIView = !(bool) ((UnityEngine.Object) editorWindow) ? GUIView.current : (GUIView) editorWindow.m_Parent;
      this.state = treeViewState;
    }

    public Action<int[]> selectionChangedCallback { get; set; }

    public Action<int> itemDoubleClickedCallback { get; set; }

    public Action<int[], bool> dragEndedCallback { get; set; }

    public Action<int> contextClickItemCallback { get; set; }

    public Action contextClickOutsideItemsCallback { get; set; }

    public Action keyboardInputCallback { get; set; }

    public Action expandedStateChanged { get; set; }

    public Action<string> searchChanged { get; set; }

    public Action<Vector2> scrollChanged { get; set; }

    public Action<int, Rect> onGUIRowCallback { get; set; }

    public ITreeViewDataSource data { get; set; }

    public ITreeViewDragging dragging { get; set; }

    public ITreeViewGUI gui { get; set; }

    public TreeViewState state { get; set; }

    public GUIStyle horizontalScrollbarStyle { get; set; }

    public GUIStyle verticalScrollbarStyle { get; set; }

    public TreeViewItemExpansionAnimator expansionAnimator
    {
      get
      {
        return this.m_ExpansionAnimator;
      }
    }

    public bool deselectOnUnhandledMouseDown { get; set; }

    public bool useExpansionAnimation
    {
      get
      {
        return this.m_UseExpansionAnimation;
      }
      set
      {
        this.m_UseExpansionAnimation = value;
      }
    }

    public void Init(Rect rect, ITreeViewDataSource data, ITreeViewGUI gui, ITreeViewDragging dragging)
    {
      this.data = data;
      this.gui = gui;
      this.dragging = dragging;
      this.m_TotalRect = rect;
      data.OnInitialize();
      gui.OnInitialize();
      if (dragging != null)
        dragging.OnInitialize();
      this.expandedStateChanged += new Action(this.ExpandedStateHasChanged);
      this.m_FramingAnimFloat = new AnimFloat(this.state.scrollPos.y, new UnityAction(this.AnimatedScrollChanged));
    }

    private void ExpandedStateHasChanged()
    {
      this.m_StopIteratingItems = true;
    }

    public bool isSearching
    {
      get
      {
        return !string.IsNullOrEmpty(this.state.searchString);
      }
    }

    public bool isDragging
    {
      get
      {
        return this.m_DragSelection != null && this.m_DragSelection.Count > 0;
      }
    }

    public bool showingVerticalScrollBar
    {
      get
      {
        return (double) this.m_ContentRect.height > (double) this.m_VisibleRect.height;
      }
    }

    public bool showingHorizontalScrollBar
    {
      get
      {
        return (double) this.m_ContentRect.width > (double) this.m_VisibleRect.width;
      }
    }

    public string searchString
    {
      get
      {
        return this.state.searchString;
      }
      set
      {
        if (object.ReferenceEquals((object) this.state.searchString, (object) value) || this.state.searchString == value)
          return;
        this.state.searchString = value;
        this.data.OnSearchChanged();
        if (this.searchChanged == null)
          return;
        this.searchChanged(this.state.searchString);
      }
    }

    public bool IsSelected(int id)
    {
      return this.state.selectedIDs.Contains(id);
    }

    public bool HasSelection()
    {
      return this.state.selectedIDs.Count<int>() > 0;
    }

    public int[] GetSelection()
    {
      return this.state.selectedIDs.ToArray();
    }

    public int[] GetRowIDs()
    {
      return this.data.GetRows().Select<TreeViewItem, int>((Func<TreeViewItem, int>) (item => item.id)).ToArray<int>();
    }

    public void SetSelection(int[] selectedIDs, bool revealSelectionAndFrameLastSelected)
    {
      this.SetSelection(selectedIDs, revealSelectionAndFrameLastSelected, false);
    }

    public void SetSelection(int[] selectedIDs, bool revealSelectionAndFrameLastSelected, bool animatedFraming)
    {
      if (selectedIDs.Length > 0)
      {
        if (revealSelectionAndFrameLastSelected)
        {
          foreach (int selectedId in selectedIDs)
            this.data.RevealItem(selectedId);
        }
        this.state.selectedIDs = new List<int>((IEnumerable<int>) selectedIDs);
        bool flag = this.state.selectedIDs.IndexOf(this.state.lastClickedID) >= 0;
        if (!flag)
        {
          int id = ((IEnumerable<int>) selectedIDs).Last<int>();
          if (this.data.GetRow(id) != -1)
          {
            this.state.lastClickedID = id;
            flag = true;
          }
          else
            this.state.lastClickedID = 0;
        }
        if (!revealSelectionAndFrameLastSelected || !flag)
          return;
        this.Frame(this.state.lastClickedID, true, false, animatedFraming);
      }
      else
      {
        this.state.selectedIDs.Clear();
        this.state.lastClickedID = 0;
      }
    }

    public TreeViewItem FindItem(int id)
    {
      return this.data.FindItem(id);
    }

    public void SetUseScrollView(bool useScrollView)
    {
      this.m_UseScrollView = useScrollView;
    }

    public void Repaint()
    {
      if (!((UnityEngine.Object) this.m_GUIView != (UnityEngine.Object) null))
        return;
      this.m_GUIView.Repaint();
    }

    public void ReloadData()
    {
      this.data.ReloadData();
      this.Repaint();
      this.m_StopIteratingItems = true;
    }

    public bool HasFocus()
    {
      return (!((UnityEngine.Object) this.m_GUIView != (UnityEngine.Object) null) ? EditorGUIUtility.HasCurrentWindowKeyFocus() : this.m_GUIView.hasFocus) && GUIUtility.keyboardControl == this.m_KeyboardControlID;
    }

    internal static int GetItemControlID(TreeViewItem item)
    {
      return (item == null ? 0 : item.id) + 10000000;
    }

    public void HandleUnusedMouseEventsForItem(Rect rect, TreeViewItem item, int row)
    {
      int itemControlId = TreeViewController.GetItemControlID(item);
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(itemControlId);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          if (!rect.Contains(Event.current.mousePosition))
            break;
          if (Event.current.button == 0)
          {
            GUIUtility.keyboardControl = this.m_KeyboardControlID;
            this.Repaint();
            if (Event.current.clickCount == 2)
            {
              if (this.itemDoubleClickedCallback != null)
                this.itemDoubleClickedCallback(item.id);
            }
            else
            {
              List<int> newSelection = this.GetNewSelection(item, true, false);
              if (this.dragging != null && newSelection.Count != 0 && this.dragging.CanStartDrag(item, newSelection, Event.current.mousePosition))
              {
                this.m_DragSelection = newSelection;
                ((DragAndDropDelay) GUIUtility.GetStateObject(typeof (DragAndDropDelay), itemControlId)).mouseDownPosition = Event.current.mousePosition;
              }
              else
              {
                this.m_DragSelection.Clear();
                if (this.m_AllowRenameOnMouseUp)
                  this.m_AllowRenameOnMouseUp = this.state.selectedIDs.Count == 1 && this.state.selectedIDs[0] == item.id;
                this.SelectionClick(item, false);
              }
              GUIUtility.hotControl = itemControlId;
            }
            current.Use();
          }
          else if (Event.current.button == 1)
          {
            bool keepMultiSelection = true;
            this.SelectionClick(item, keepMultiSelection);
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != itemControlId)
            break;
          bool flag = this.m_DragSelection.Count > 0;
          GUIUtility.hotControl = 0;
          this.m_DragSelection.Clear();
          current.Use();
          if (rect.Contains(current.mousePosition))
          {
            Rect renameRect = this.gui.GetRenameRect(rect, row, item);
            List<int> selectedIds = this.state.selectedIDs;
            if (this.m_AllowRenameOnMouseUp && selectedIds != null && (selectedIds.Count == 1 && selectedIds[0] == item.id) && (renameRect.Contains(current.mousePosition) && !EditorGUIUtility.HasHolddownKeyModifiers(current)))
              this.BeginNameEditing(0.5f);
            else if (flag)
              this.SelectionClick(item, false);
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != itemControlId || this.dragging == null || this.m_DragSelection.Count <= 0)
            break;
          DragAndDropDelay stateObject = (DragAndDropDelay) GUIUtility.GetStateObject(typeof (DragAndDropDelay), itemControlId);
          if (stateObject.CanStartDrag() && this.dragging.CanStartDrag(item, this.m_DragSelection, stateObject.mouseDownPosition))
          {
            this.dragging.StartDrag(item, this.m_DragSelection);
            GUIUtility.hotControl = 0;
          }
          current.Use();
          break;
        default:
          if (typeForControl != EventType.DragUpdated && typeForControl != EventType.DragPerform)
          {
            if (typeForControl != EventType.ContextClick || !rect.Contains(current.mousePosition) || this.contextClickItemCallback == null)
              break;
            this.contextClickItemCallback(item.id);
            break;
          }
          if (this.dragging == null || !this.dragging.DragElement(item, rect, row))
            break;
          GUIUtility.hotControl = 0;
          break;
      }
    }

    public void GrabKeyboardFocus()
    {
      this.m_GrabKeyboardFocus = true;
    }

    public void NotifyListenersThatSelectionChanged()
    {
      if (this.selectionChangedCallback == null)
        return;
      this.selectionChangedCallback(this.state.selectedIDs.ToArray());
    }

    public void NotifyListenersThatDragEnded(int[] draggedIDs, bool draggedItemsFromOwnTreeView)
    {
      if (this.dragEndedCallback == null)
        return;
      this.dragEndedCallback(draggedIDs, draggedItemsFromOwnTreeView);
    }

    public Vector2 GetContentSize()
    {
      return this.gui.GetTotalSize();
    }

    public Rect GetTotalRect()
    {
      return this.m_TotalRect;
    }

    public void SetTotalRect(Rect rect)
    {
      this.m_TotalRect = rect;
    }

    public bool IsItemDragSelectedOrSelected(TreeViewItem item)
    {
      return this.m_DragSelection.Count <= 0 ? this.state.selectedIDs.Contains(item.id) : this.m_DragSelection.Contains(item.id);
    }

    public bool animatingExpansion
    {
      get
      {
        return this.m_UseExpansionAnimation && this.m_ExpansionAnimator.isAnimating;
      }
    }

    private void DoItemGUI(TreeViewItem item, int row, float rowWidth, bool hasFocus)
    {
      if (row < 0 || row >= this.data.rowCount)
      {
        Debug.LogError((object) ("Invalid. Org row: " + (object) row + " Num rows " + (object) this.data.rowCount));
      }
      else
      {
        bool selected = this.IsItemDragSelectedOrSelected(item);
        Rect rect1 = this.gui.GetRowRect(row, rowWidth);
        if (this.animatingExpansion)
          rect1 = this.m_ExpansionAnimator.OnBeginRowGUI(row, rect1);
        if (this.animatingExpansion)
          this.m_ExpansionAnimator.OnRowGUI(row);
        this.gui.OnRowGUI(rect1, item, row, selected, hasFocus);
        if (this.onGUIRowCallback != null)
        {
          float contentIndent = this.gui.GetContentIndent(item);
          Rect rect2 = new Rect(rect1.x + contentIndent, rect1.y, rect1.width - contentIndent, rect1.height);
          this.onGUIRowCallback(item.id, rect2);
        }
        if (this.animatingExpansion)
          this.m_ExpansionAnimator.OnEndRowGUI(row);
        this.HandleUnusedMouseEventsForItem(rect1, item, row);
      }
    }

    public void OnGUI(Rect rect, int keyboardControlID)
    {
      this.m_KeyboardControlID = keyboardControlID;
      Event current = Event.current;
      if (current.type == EventType.Repaint)
        this.m_TotalRect = rect;
      this.m_GUIView = GUIView.current;
      if ((UnityEngine.Object) this.m_GUIView != (UnityEngine.Object) null && !this.m_GUIView.hasFocus && this.state.renameOverlay.IsRenaming())
        this.EndNameEditing(true);
      if (this.m_GrabKeyboardFocus || current.type == EventType.MouseDown && this.m_TotalRect.Contains(current.mousePosition))
      {
        this.m_GrabKeyboardFocus = false;
        GUIUtility.keyboardControl = this.m_KeyboardControlID;
        this.m_AllowRenameOnMouseUp = true;
        this.Repaint();
      }
      bool hasFocus = this.HasFocus();
      if (hasFocus != this.m_HadFocusLastEvent && current.type != EventType.Layout)
      {
        this.m_HadFocusLastEvent = hasFocus;
        if (hasFocus && current.type == EventType.MouseDown)
          this.m_AllowRenameOnMouseUp = false;
      }
      if (this.animatingExpansion)
        this.m_ExpansionAnimator.OnBeforeAllRowsGUI();
      this.data.InitIfNeeded();
      Vector2 totalSize = this.gui.GetTotalSize();
      this.m_ContentRect = new Rect(0.0f, 0.0f, totalSize.x, totalSize.y);
      if (this.m_UseScrollView)
        this.state.scrollPos = GUI.BeginScrollView(this.m_TotalRect, this.state.scrollPos, this.m_ContentRect, this.horizontalScrollbarStyle == null ? GUI.skin.horizontalScrollbar : this.horizontalScrollbarStyle, this.verticalScrollbarStyle == null ? GUI.skin.verticalScrollbar : this.verticalScrollbarStyle);
      else
        GUI.BeginClip(this.m_TotalRect);
      if (current.type == EventType.Repaint)
        this.m_VisibleRect = !this.m_UseScrollView ? this.m_TotalRect : GUI.GetTopScrollView().visibleRect;
      this.gui.BeginRowGUI();
      int firstRowVisible;
      int lastRowVisible;
      this.gui.GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible);
      if (lastRowVisible >= 0)
      {
        int numVisibleRows = lastRowVisible - firstRowVisible + 1;
        float rowWidth = Mathf.Max(GUIClip.visibleRect.width, this.m_ContentRect.width);
        this.IterateVisibleItems(firstRowVisible, numVisibleRows, rowWidth, hasFocus);
      }
      if (this.animatingExpansion)
        this.m_ExpansionAnimator.OnAfterAllRowsGUI();
      this.gui.EndRowGUI();
      if (this.m_UseScrollView)
        GUI.EndScrollView(this.showingVerticalScrollBar);
      else
        GUI.EndClip();
      this.HandleUnusedEvents();
      this.KeyboardGUI();
      GUIUtility.GetControlID(33243602, FocusType.Passive);
    }

    private void IterateVisibleItems(int firstRow, int numVisibleRows, float rowWidth, bool hasFocus)
    {
      this.m_StopIteratingItems = false;
      int num = 0;
      for (int index = 0; index < numVisibleRows; ++index)
      {
        int row = firstRow + index;
        if (this.animatingExpansion)
        {
          int endRow = this.m_ExpansionAnimator.endRow;
          if (this.m_ExpansionAnimator.CullRow(row, this.gui))
          {
            ++num;
            row = endRow + num;
          }
          else
            row += num;
          if (row >= this.data.rowCount)
            continue;
        }
        else if ((double) (this.gui.GetRowRect(row, rowWidth).y - this.state.scrollPos.y) > (double) this.m_TotalRect.height)
          continue;
        this.DoItemGUI(this.data.GetItem(row), row, rowWidth, hasFocus);
        if (this.m_StopIteratingItems)
          break;
      }
    }

    private List<int> GetVisibleSelectedIds()
    {
      int firstRowVisible;
      int lastRowVisible;
      this.gui.GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible);
      if (lastRowVisible < 0)
        return new List<int>();
      List<int> source = new List<int>(lastRowVisible - firstRowVisible);
      for (int row = firstRowVisible; row < lastRowVisible; ++row)
      {
        TreeViewItem treeViewItem = this.data.GetItem(row);
        source.Add(treeViewItem.id);
      }
      return source.Where<int>((Func<int, bool>) (id => this.state.selectedIDs.Contains(id))).ToList<int>();
    }

    private void ExpansionAnimationEnded(TreeViewAnimationInput setup)
    {
      if (setup.expanding)
        return;
      this.ChangeExpandedState(setup.item, false, setup.includeChildren);
    }

    private float GetAnimationDuration(float height)
    {
      return (double) height <= 60.0 ? (float) ((double) height * 0.0700000002980232 / 60.0) : 0.07f;
    }

    public void UserInputChangedExpandedState(TreeViewItem item, int row, bool expand)
    {
      bool alt = Event.current.alt;
      if (this.useExpansionAnimation)
      {
        if (expand)
          this.ChangeExpandedState(item, true, alt);
        int num = row + 1;
        int lastChildRowUnder = this.GetLastChildRowUnder(row);
        float width = GUIClip.visibleRect.width;
        Rect rectForRows = this.GetRectForRows(num, lastChildRowUnder, width);
        float animationDuration = this.GetAnimationDuration(rectForRows.height);
        this.expansionAnimator.BeginAnimating(new TreeViewAnimationInput()
        {
          animationDuration = (double) animationDuration,
          startRow = num,
          endRow = lastChildRowUnder,
          startRowRect = this.gui.GetRowRect(num, width),
          rowsRect = rectForRows,
          expanding = expand,
          includeChildren = alt,
          animationEnded = new Action<TreeViewAnimationInput>(this.ExpansionAnimationEnded),
          item = item,
          treeView = this
        });
      }
      else
        this.ChangeExpandedState(item, expand, alt);
    }

    private void ChangeExpandedState(TreeViewItem item, bool expand, bool includeChildren)
    {
      if (includeChildren)
        this.data.SetExpandedWithChildren(item, expand);
      else
        this.data.SetExpanded(item, expand);
    }

    private int GetLastChildRowUnder(int row)
    {
      IList<TreeViewItem> rows = this.data.GetRows();
      int depth = rows[row].depth;
      for (int index = row + 1; index < rows.Count; ++index)
      {
        if (rows[index].depth <= depth)
          return index - 1;
      }
      return rows.Count - 1;
    }

    protected virtual Rect GetRectForRows(int startRow, int endRow, float rowWidth)
    {
      Rect rowRect1 = this.gui.GetRowRect(startRow, rowWidth);
      Rect rowRect2 = this.gui.GetRowRect(endRow, rowWidth);
      return new Rect(rowRect1.x, rowRect1.y, rowWidth, rowRect2.yMax - rowRect1.yMin);
    }

    private void HandleUnusedEvents()
    {
      switch (Event.current.type)
      {
        case EventType.MouseDown:
          if (!this.deselectOnUnhandledMouseDown || Event.current.button != 0 || (!this.m_TotalRect.Contains(Event.current.mousePosition) || this.state.selectedIDs.Count <= 0))
            break;
          this.SetSelection(new int[0], false);
          this.NotifyListenersThatSelectionChanged();
          break;
        case EventType.DragUpdated:
          if (this.dragging == null || !this.m_TotalRect.Contains(Event.current.mousePosition))
            break;
          this.dragging.DragElement((TreeViewItem) null, new Rect(), -1);
          this.Repaint();
          Event.current.Use();
          break;
        case EventType.DragPerform:
          if (this.dragging == null || !this.m_TotalRect.Contains(Event.current.mousePosition))
            break;
          this.m_DragSelection.Clear();
          this.dragging.DragElement((TreeViewItem) null, new Rect(), -1);
          this.Repaint();
          Event.current.Use();
          break;
        case EventType.DragExited:
          if (this.dragging == null)
            break;
          this.m_DragSelection.Clear();
          this.dragging.DragCleanup(true);
          this.Repaint();
          break;
        case EventType.ContextClick:
          if (!this.m_TotalRect.Contains(Event.current.mousePosition) || this.contextClickOutsideItemsCallback == null)
            break;
          this.contextClickOutsideItemsCallback();
          break;
      }
    }

    public void OnEvent()
    {
      this.state.renameOverlay.OnEvent();
    }

    public bool BeginNameEditing(float delay)
    {
      if (this.state.selectedIDs.Count == 0)
        return false;
      IList<TreeViewItem> rows = this.data.GetRows();
      TreeViewItem treeViewItem1 = (TreeViewItem) null;
      foreach (int selectedId in this.state.selectedIDs)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated method
        TreeViewItem treeViewItem2 = rows.FirstOrDefault<TreeViewItem>(new Func<TreeViewItem, bool>(new TreeViewController.\u003CBeginNameEditing\u003Ec__AnonStorey0() { id = selectedId }.\u003C\u003Em__0));
        if (treeViewItem1 == null)
          treeViewItem1 = treeViewItem2;
        else if (treeViewItem2 != null)
          return false;
      }
      if (treeViewItem1 != null && this.data.IsRenamingItemAllowed(treeViewItem1))
        return this.gui.BeginRename(treeViewItem1, delay);
      return false;
    }

    public void EndNameEditing(bool acceptChanges)
    {
      if (!this.state.renameOverlay.IsRenaming())
        return;
      this.state.renameOverlay.EndRename(acceptChanges);
      this.gui.EndRename();
    }

    private TreeViewItem GetItemAndRowIndex(int id, out int row)
    {
      row = this.data.GetRow(id);
      if (row == -1)
        return (TreeViewItem) null;
      return this.data.GetItem(row);
    }

    private void HandleFastCollapse(TreeViewItem item, int row)
    {
      if (item.depth == 0)
      {
        for (int row1 = row - 1; row1 >= 0; --row1)
        {
          if (this.data.GetItem(row1).hasChildren)
          {
            this.OffsetSelection(row1 - row);
            break;
          }
        }
      }
      else
      {
        if (item.depth <= 0)
          return;
        for (int row1 = row - 1; row1 >= 0; --row1)
        {
          if (this.data.GetItem(row1).depth < item.depth)
          {
            this.OffsetSelection(row1 - row);
            break;
          }
        }
      }
    }

    private void HandleFastExpand(TreeViewItem item, int row)
    {
      int rowCount = this.data.rowCount;
      for (int row1 = row + 1; row1 < rowCount; ++row1)
      {
        if (this.data.GetItem(row1).hasChildren)
        {
          this.OffsetSelection(row1 - row);
          break;
        }
      }
    }

    private void ChangeFolding(int[] ids, bool expand)
    {
      if (ids.Length == 1)
      {
        this.ChangeFoldingForSingleItem(ids[0], expand);
      }
      else
      {
        if (ids.Length <= 1)
          return;
        this.ChangeFoldingForMultipleItems(ids, expand);
      }
    }

    private void ChangeFoldingForSingleItem(int id, bool expand)
    {
      int row;
      TreeViewItem itemAndRowIndex = this.GetItemAndRowIndex(id, out row);
      if (itemAndRowIndex == null)
        return;
      if (this.data.IsExpandable(itemAndRowIndex) && this.data.IsExpanded(itemAndRowIndex) != expand)
      {
        this.UserInputChangedExpandedState(itemAndRowIndex, row, expand);
      }
      else
      {
        this.expansionAnimator.SkipAnimating();
        if (expand)
          this.HandleFastExpand(itemAndRowIndex, row);
        else
          this.HandleFastCollapse(itemAndRowIndex, row);
      }
    }

    private void ChangeFoldingForMultipleItems(int[] ids, bool expand)
    {
      HashSet<int> intSet = new HashSet<int>();
      foreach (int id in ids)
      {
        int row;
        TreeViewItem itemAndRowIndex = this.GetItemAndRowIndex(id, out row);
        if (itemAndRowIndex != null && this.data.IsExpandable(itemAndRowIndex) && this.data.IsExpanded(itemAndRowIndex) != expand)
          intSet.Add(id);
      }
      if (Event.current.alt)
      {
        foreach (int id in intSet)
          this.data.SetExpandedWithChildren(id, expand);
      }
      else
      {
        HashSet<int> source = new HashSet<int>((IEnumerable<int>) this.data.GetExpandedIDs());
        if (expand)
          source.UnionWith((IEnumerable<int>) intSet);
        else
          source.ExceptWith((IEnumerable<int>) intSet);
        this.data.SetExpandedIDs(source.ToArray<int>());
      }
    }

    private void KeyboardGUI()
    {
      if (this.m_KeyboardControlID != GUIUtility.keyboardControl || !GUI.enabled)
        return;
      if (this.keyboardInputCallback != null)
        this.keyboardInputCallback();
      if (Event.current.type != EventType.KeyDown)
        return;
      KeyCode keyCode = Event.current.keyCode;
      switch (keyCode)
      {
        case KeyCode.KeypadEnter:
          if (Application.platform == RuntimePlatform.OSXEditor && this.BeginNameEditing(0.0f))
          {
            Event.current.Use();
            break;
          }
          break;
        case KeyCode.UpArrow:
          Event.current.Use();
          this.OffsetSelection(-1);
          break;
        case KeyCode.DownArrow:
          Event.current.Use();
          this.OffsetSelection(1);
          break;
        case KeyCode.RightArrow:
          this.ChangeFolding(this.state.selectedIDs.ToArray(), true);
          Event.current.Use();
          break;
        case KeyCode.LeftArrow:
          this.ChangeFolding(this.state.selectedIDs.ToArray(), false);
          Event.current.Use();
          break;
        case KeyCode.Home:
          Event.current.Use();
          this.OffsetSelection(-1000000);
          break;
        case KeyCode.End:
          Event.current.Use();
          this.OffsetSelection(1000000);
          break;
        case KeyCode.PageUp:
          Event.current.Use();
          TreeViewItem fromItem1 = this.data.FindItem(this.state.lastClickedID);
          if (fromItem1 != null)
          {
            this.OffsetSelection(-this.gui.GetNumRowsOnPageUpDown(fromItem1, true, this.m_TotalRect.height));
            break;
          }
          break;
        case KeyCode.PageDown:
          Event.current.Use();
          TreeViewItem fromItem2 = this.data.FindItem(this.state.lastClickedID);
          if (fromItem2 != null)
          {
            this.OffsetSelection(this.gui.GetNumRowsOnPageUpDown(fromItem2, true, this.m_TotalRect.height));
            break;
          }
          break;
        case KeyCode.F2:
          if (Application.platform != RuntimePlatform.OSXEditor && this.BeginNameEditing(0.0f))
          {
            Event.current.Use();
            break;
          }
          break;
        default:
          if (keyCode != KeyCode.Return)
          {
            if (Event.current.keyCode <= KeyCode.A || Event.current.keyCode >= KeyCode.Z)
              break;
            break;
          }
          goto case KeyCode.KeypadEnter;
      }
    }

    internal static int GetIndexOfID(IList<TreeViewItem> items, int id)
    {
      for (int index = 0; index < items.Count; ++index)
      {
        if (items[index].id == id)
          return index;
      }
      return -1;
    }

    public bool IsLastClickedPartOfRows()
    {
      IList<TreeViewItem> rows = this.data.GetRows();
      if (rows.Count == 0)
        return false;
      return TreeViewController.GetIndexOfID(rows, this.state.lastClickedID) >= 0;
    }

    public void OffsetSelection(int offset)
    {
      this.expansionAnimator.SkipAnimating();
      IList<TreeViewItem> rows = this.data.GetRows();
      if (rows.Count == 0)
        return;
      Event.current.Use();
      int row = Mathf.Clamp(TreeViewController.GetIndexOfID(rows, this.state.lastClickedID) + offset, 0, rows.Count - 1);
      this.EnsureRowIsVisible(row, true);
      this.SelectionByKey(rows[row]);
    }

    private bool GetFirstAndLastSelected(List<TreeViewItem> items, out int firstIndex, out int lastIndex)
    {
      firstIndex = -1;
      lastIndex = -1;
      for (int index = 0; index < items.Count; ++index)
      {
        if (this.state.selectedIDs.Contains(items[index].id))
        {
          if (firstIndex == -1)
            firstIndex = index;
          lastIndex = index;
        }
      }
      return firstIndex != -1 && lastIndex != -1;
    }

    private List<int> GetNewSelection(TreeViewItem clickedItem, bool keepMultiSelection, bool useShiftAsActionKey)
    {
      IList<TreeViewItem> rows = this.data.GetRows();
      List<int> allInstanceIDs = new List<int>(rows.Count);
      for (int index = 0; index < rows.Count; ++index)
        allInstanceIDs.Add(rows[index].id);
      List<int> selectedIds = this.state.selectedIDs;
      int lastClickedId = this.state.lastClickedID;
      bool allowMultiSelection = this.data.CanBeMultiSelected(clickedItem);
      return InternalEditorUtility.GetNewSelection(clickedItem.id, allInstanceIDs, selectedIds, lastClickedId, keepMultiSelection, useShiftAsActionKey, allowMultiSelection);
    }

    private void SelectionByKey(TreeViewItem itemSelected)
    {
      this.NewSelectionFromUserInteraction(this.GetNewSelection(itemSelected, false, true), itemSelected.id);
    }

    public void SelectionClick(TreeViewItem itemClicked, bool keepMultiSelection)
    {
      this.NewSelectionFromUserInteraction(this.GetNewSelection(itemClicked, keepMultiSelection, false), itemClicked == null ? 0 : itemClicked.id);
    }

    private void NewSelectionFromUserInteraction(List<int> newSelection, int itemID)
    {
      this.state.lastClickedID = itemID;
      if (this.state.selectedIDs.SequenceEqual<int>((IEnumerable<int>) newSelection))
        return;
      this.state.selectedIDs = newSelection;
      this.NotifyListenersThatSelectionChanged();
    }

    public void RemoveSelection()
    {
      if (this.state.selectedIDs.Count <= 0)
        return;
      this.state.selectedIDs.Clear();
      this.NotifyListenersThatSelectionChanged();
    }

    private float GetTopPixelOfRow(int row)
    {
      return this.gui.GetRowRect(row, 1f).y;
    }

    private void EnsureRowIsVisible(int row, bool animated)
    {
      if (!this.m_UseScrollView || row < 0)
        return;
      float num = (double) this.m_VisibleRect.height <= 0.0 ? this.m_TotalRect.height : this.m_VisibleRect.height;
      Rect rectForFraming = this.gui.GetRectForFraming(row);
      float y = rectForFraming.y;
      float targetScrollPos = rectForFraming.yMax - num;
      if ((double) this.state.scrollPos.y < (double) targetScrollPos)
        this.ChangeScrollValue(targetScrollPos, animated);
      else if ((double) this.state.scrollPos.y > (double) y)
        this.ChangeScrollValue(y, animated);
    }

    private void AnimatedScrollChanged()
    {
      this.Repaint();
      this.state.scrollPos.y = this.m_FramingAnimFloat.value;
    }

    private void ChangeScrollValue(float targetScrollPos, bool animated)
    {
      if (this.m_UseExpansionAnimation && animated)
      {
        this.m_FramingAnimFloat.value = this.state.scrollPos.y;
        this.m_FramingAnimFloat.target = targetScrollPos;
        this.m_FramingAnimFloat.speed = 3f;
      }
      else
        this.state.scrollPos.y = targetScrollPos;
    }

    public void Frame(int id, bool frame, bool ping)
    {
      this.Frame(id, frame, ping, false);
    }

    public void Frame(int id, bool frame, bool ping, bool animated)
    {
      float topPixelOfRow = -1f;
      if (frame)
      {
        this.data.RevealItem(id);
        int row = this.data.GetRow(id);
        if (row >= 0)
        {
          topPixelOfRow = this.GetTopPixelOfRow(row);
          this.EnsureRowIsVisible(row, animated);
        }
      }
      if (!ping)
        return;
      int row1 = this.data.GetRow(id);
      if ((double) topPixelOfRow == -1.0 && row1 >= 0)
        topPixelOfRow = this.GetTopPixelOfRow(row1);
      if ((double) topPixelOfRow >= 0.0 && row1 >= 0 && row1 < this.data.rowCount)
      {
        TreeViewItem treeViewItem = this.data.GetItem(row1);
        float num = (double) this.GetContentSize().y <= (double) this.m_TotalRect.height ? 0.0f : -16f;
        this.gui.BeginPingItem(treeViewItem, topPixelOfRow, this.m_TotalRect.width + num);
      }
    }

    public void EndPing()
    {
      this.gui.EndPingItem();
    }

    public List<int> SortIDsInVisiblityOrder(IList<int> ids)
    {
      if (ids.Count <= 1)
        return ids.ToList<int>();
      IList<TreeViewItem> rows = this.data.GetRows();
      List<int> intList = new List<int>();
      for (int index1 = 0; index1 < rows.Count; ++index1)
      {
        int id = rows[index1].id;
        for (int index2 = 0; index2 < ids.Count; ++index2)
        {
          if (ids[index2] == id)
          {
            intList.Add(id);
            break;
          }
        }
      }
      if (ids.Count != intList.Count)
      {
        intList.AddRange(ids.Except<int>((IEnumerable<int>) intList));
        if (ids.Count != intList.Count)
          Debug.LogError((object) ("SortIDsInVisiblityOrder failed: " + (object) ids.Count + " != " + (object) intList.Count));
      }
      return intList;
    }
  }
}
