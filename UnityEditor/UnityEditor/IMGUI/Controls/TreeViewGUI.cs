// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.TreeViewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  internal abstract class TreeViewGUI : ITreeViewGUI
  {
    protected PingData m_Ping = new PingData();
    private bool m_AnimateScrollBarOnExpandCollapse = true;
    public float k_LineHeight = 16f;
    public float k_BaseIndent = 2f;
    public float k_IndentWidth = 14f;
    public float k_IconWidth = 16f;
    public float k_SpaceBetweenIconAndText = 2f;
    public float k_TopRowMargin = 0.0f;
    public float k_BottomRowMargin = 0.0f;
    public float k_HalfDropBetweenHeight = 4f;
    public float customFoldoutYOffset = 0.0f;
    public float extraInsertionMarkerIndent = 0.0f;
    protected TreeViewController m_TreeView;
    protected Rect m_DraggingInsertionMarkerRect;
    protected bool m_UseHorizontalScroll;

    public TreeViewGUI(TreeViewController treeView)
    {
      this.m_TreeView = treeView;
    }

    public TreeViewGUI(TreeViewController treeView, bool useHorizontalScroll)
    {
      this.m_TreeView = treeView;
      this.m_UseHorizontalScroll = useHorizontalScroll;
    }

    public float iconLeftPadding { get; set; }

    public float iconRightPadding { get; set; }

    public float iconTotalPadding
    {
      get
      {
        return this.iconLeftPadding + this.iconRightPadding;
      }
    }

    public Action<TreeViewItem, Rect> iconOverlayGUI { get; set; }

    public Action<TreeViewItem, Rect> labelOverlayGUI { get; set; }

    public float indentWidth
    {
      get
      {
        return this.k_IndentWidth + this.iconTotalPadding;
      }
    }

    public float extraSpaceBeforeIconAndLabel { get; set; }

    public float halfDropBetweenHeight
    {
      get
      {
        return this.k_HalfDropBetweenHeight;
      }
    }

    public virtual float topRowMargin
    {
      get
      {
        return this.k_TopRowMargin;
      }
    }

    public virtual float bottomRowMargin
    {
      get
      {
        return this.k_BottomRowMargin;
      }
    }

    public virtual void OnInitialize()
    {
    }

    protected virtual Texture GetIconForItem(TreeViewItem item)
    {
      return (Texture) item.icon;
    }

    public virtual Vector2 GetTotalSize()
    {
      float x = 1f;
      if (this.m_UseHorizontalScroll)
        x = this.GetMaxWidth(this.m_TreeView.data.GetRows());
      float y = (float) this.m_TreeView.data.rowCount * this.k_LineHeight + this.topRowMargin + this.bottomRowMargin;
      if (this.m_AnimateScrollBarOnExpandCollapse && this.m_TreeView.expansionAnimator.isAnimating)
        y -= this.m_TreeView.expansionAnimator.deltaHeight;
      return new Vector2(x, y);
    }

    protected float GetMaxWidth(IList<TreeViewItem> rows)
    {
      float num1 = 1f;
      foreach (TreeViewItem row in (IEnumerable<TreeViewItem>) rows)
      {
        float num2 = 0.0f + this.GetContentIndent(row);
        if ((UnityEngine.Object) row.icon != (UnityEngine.Object) null)
          num2 += this.k_IconWidth;
        float minWidth;
        float maxWidth;
        TreeViewGUI.Styles.lineStyle.CalcMinMaxWidth(GUIContent.Temp(row.displayName), out minWidth, out maxWidth);
        float num3 = num2 + maxWidth + this.k_BaseIndent;
        if ((double) num3 > (double) num1)
          num1 = num3;
      }
      return num1;
    }

    public virtual int GetNumRowsOnPageUpDown(TreeViewItem fromItem, bool pageUp, float heightOfTreeView)
    {
      return (int) Mathf.Floor(heightOfTreeView / this.k_LineHeight);
    }

    public virtual void GetFirstAndLastRowVisible(out int firstRowVisible, out int lastRowVisible)
    {
      if (this.m_TreeView.data.rowCount == 0)
      {
        firstRowVisible = lastRowVisible = -1;
      }
      else
      {
        float y = this.m_TreeView.state.scrollPos.y;
        float height = this.m_TreeView.GetTotalRect().height;
        firstRowVisible = (int) Mathf.Floor((y - this.topRowMargin) / this.k_LineHeight);
        lastRowVisible = firstRowVisible + (int) Mathf.Ceil(height / this.k_LineHeight);
        firstRowVisible = Mathf.Max(firstRowVisible, 0);
        lastRowVisible = Mathf.Min(lastRowVisible, this.m_TreeView.data.rowCount - 1);
        if (firstRowVisible < this.m_TreeView.data.rowCount || firstRowVisible <= 0)
          return;
        this.m_TreeView.state.scrollPos.y = 0.0f;
        this.GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible);
      }
    }

    public virtual void BeginRowGUI()
    {
      this.m_DraggingInsertionMarkerRect.x = -1f;
      this.SyncFakeItem();
      if (Event.current.type == EventType.Repaint)
        return;
      this.DoRenameOverlay();
    }

    public virtual void EndRowGUI()
    {
      if ((double) this.m_DraggingInsertionMarkerRect.x >= 0.0 && Event.current.type == EventType.Repaint)
        TreeViewGUI.Styles.insertion.Draw(this.m_DraggingInsertionMarkerRect, false, false, false, false);
      if (Event.current.type == EventType.Repaint)
        this.DoRenameOverlay();
      this.HandlePing();
    }

    public virtual void OnRowGUI(Rect rowRect, TreeViewItem item, int row, bool selected, bool focused)
    {
      this.DoItemGUI(rowRect, row, item, selected, focused, false);
    }

    protected virtual void DrawItemBackground(Rect rect, int row, TreeViewItem item, bool selected, bool focused)
    {
    }

    public virtual Rect GetRenameRect(Rect rowRect, int row, TreeViewItem item)
    {
      float num = this.GetContentIndent(item) + this.extraSpaceBeforeIconAndLabel;
      if ((UnityEngine.Object) this.GetIconForItem(item) != (UnityEngine.Object) null)
        num += this.k_SpaceBetweenIconAndText + this.k_IconWidth + this.iconTotalPadding;
      return new Rect(rowRect.x + num, rowRect.y, rowRect.width - num, EditorGUIUtility.singleLineHeight);
    }

    protected virtual void DoItemGUI(Rect rect, int row, TreeViewItem item, bool selected, bool focused, bool useBoldFont)
    {
      EditorGUIUtility.SetIconSize(new Vector2(this.k_IconWidth, this.k_IconWidth));
      float foldoutIndent = this.GetFoldoutIndent(item);
      int itemControlId = TreeViewController.GetItemControlID(item);
      bool flag1 = false;
      if (this.m_TreeView.dragging != null)
        flag1 = this.m_TreeView.dragging.GetDropTargetControlID() == itemControlId && this.m_TreeView.data.CanBeParent(item);
      bool flag2 = this.IsRenaming(item.id);
      bool flag3 = this.m_TreeView.data.IsExpandable(item);
      if (flag2 && Event.current.type == EventType.Repaint)
        this.GetRenameOverlay().editFieldRect = this.GetRenameRect(rect, row, item);
      string label = item.displayName;
      if (flag2)
      {
        selected = false;
        label = "";
      }
      if (Event.current.type == EventType.Repaint)
      {
        this.DrawItemBackground(rect, row, item, selected, focused);
        if (selected)
          TreeViewGUI.Styles.selectionStyle.Draw(rect, false, false, true, focused);
        if (flag1)
          TreeViewGUI.Styles.lineStyle.Draw(rect, GUIContent.none, true, true, false, false);
        if (this.m_TreeView.dragging != null && this.m_TreeView.dragging.GetRowMarkerControlID() == itemControlId)
        {
          float y = (float) ((!this.m_TreeView.dragging.drawRowMarkerAbove ? (double) rect.yMax : (double) rect.y) - (double) TreeViewGUI.Styles.insertion.fixedHeight * 0.5);
          this.m_DraggingInsertionMarkerRect = new Rect(rect.x + foldoutIndent + this.extraInsertionMarkerIndent + TreeViewGUI.Styles.foldoutWidth + (float) TreeViewGUI.Styles.lineStyle.margin.left, y, rect.width - foldoutIndent, rect.height);
        }
      }
      this.OnContentGUI(rect, row, item, label, selected, focused, useBoldFont, false);
      if (flag3)
        this.DoFoldout(rect, item, row);
      EditorGUIUtility.SetIconSize(Vector2.zero);
    }

    private float GetTopPixelOfRow(int row)
    {
      return (float) row * this.k_LineHeight + this.topRowMargin;
    }

    public virtual Rect GetRowRect(int row, float rowWidth)
    {
      return new Rect(0.0f, this.GetTopPixelOfRow(row), rowWidth, this.k_LineHeight);
    }

    public virtual Rect GetRectForFraming(int row)
    {
      return this.GetRowRect(row, 1f);
    }

    private float GetFoldoutYPosition(float rectY)
    {
      return rectY + this.customFoldoutYOffset;
    }

    protected virtual Rect DoFoldout(Rect rect, TreeViewItem item, int row)
    {
      float foldoutIndent = this.GetFoldoutIndent(item);
      Rect foldoutRect = new Rect(rect.x + foldoutIndent, this.GetFoldoutYPosition(rect.y), TreeViewGUI.Styles.foldoutWidth, EditorGUIUtility.singleLineHeight);
      this.FoldoutButton(foldoutRect, item, row, TreeViewGUI.Styles.foldout);
      return foldoutRect;
    }

    protected virtual void FoldoutButton(Rect foldoutRect, TreeViewItem item, int row, GUIStyle foldoutStyle)
    {
      TreeViewItemExpansionAnimator expansionAnimator = this.m_TreeView.expansionAnimator;
      EditorGUI.BeginChangeCheck();
      bool flag = !expansionAnimator.IsAnimating(item.id) ? this.m_TreeView.data.IsExpanded(item) : expansionAnimator.isExpanding;
      bool expand = GUI.Toggle(foldoutRect, flag, GUIContent.none, foldoutStyle);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_TreeView.UserInputChangedExpandedState(item, row, expand);
    }

    protected virtual void OnContentGUI(Rect rect, int row, TreeViewItem item, string label, bool selected, bool focused, bool useBoldFont, bool isPinging)
    {
      if (Event.current.rawType != EventType.Repaint)
        return;
      if (!isPinging)
      {
        float num = this.GetContentIndent(item) + this.extraSpaceBeforeIconAndLabel;
        rect.xMin += num;
      }
      GUIStyle guiStyle = !useBoldFont ? TreeViewGUI.Styles.lineStyle : TreeViewGUI.Styles.lineBoldStyle;
      Rect position = rect;
      position.width = this.k_IconWidth;
      position.x += this.iconLeftPadding;
      Texture iconForItem = this.GetIconForItem(item);
      if ((UnityEngine.Object) iconForItem != (UnityEngine.Object) null)
        GUI.DrawTexture(position, iconForItem, ScaleMode.ScaleToFit);
      if (this.iconOverlayGUI != null)
      {
        Rect rect1 = rect;
        rect1.width = this.k_IconWidth + this.iconTotalPadding;
        this.iconOverlayGUI(item, rect1);
      }
      guiStyle.padding.left = 0;
      if ((UnityEngine.Object) iconForItem != (UnityEngine.Object) null)
        rect.xMin += this.k_IconWidth + this.iconTotalPadding + this.k_SpaceBetweenIconAndText;
      guiStyle.Draw(rect, label, false, false, selected, focused);
      if (this.labelOverlayGUI == null)
        return;
      this.labelOverlayGUI(item, rect);
    }

    public virtual void BeginPingItem(TreeViewItem item, float topPixelOfRow, float availableWidth)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TreeViewGUI.\u003CBeginPingItem\u003Ec__AnonStorey1 itemCAnonStorey1 = new TreeViewGUI.\u003CBeginPingItem\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      itemCAnonStorey1.item = item;
      // ISSUE: reference to a compiler-generated field
      itemCAnonStorey1.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      if (itemCAnonStorey1.item == null || (double) topPixelOfRow < 0.0)
        return;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TreeViewGUI.\u003CBeginPingItem\u003Ec__AnonStorey0 itemCAnonStorey0 = new TreeViewGUI.\u003CBeginPingItem\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      itemCAnonStorey0.\u003C\u003Ef__ref\u00241 = itemCAnonStorey1;
      this.m_Ping.m_TimeStart = Time.realtimeSinceStartup;
      this.m_Ping.m_PingStyle = TreeViewGUI.Styles.ping;
      // ISSUE: reference to a compiler-generated field
      Vector2 vector2 = this.m_Ping.m_PingStyle.CalcSize(GUIContent.Temp(itemCAnonStorey1.item.displayName));
      // ISSUE: reference to a compiler-generated field
      this.m_Ping.m_ContentRect = new Rect(this.GetContentIndent(itemCAnonStorey1.item) + this.extraSpaceBeforeIconAndLabel, topPixelOfRow, this.k_IconWidth + this.k_SpaceBetweenIconAndText + vector2.x + this.iconTotalPadding, vector2.y);
      this.m_Ping.m_AvailableWidth = availableWidth;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      itemCAnonStorey0.row = this.m_TreeView.data.GetRow(itemCAnonStorey1.item.id);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      itemCAnonStorey0.useBoldFont = itemCAnonStorey1.item.displayName.Equals("Assets");
      // ISSUE: reference to a compiler-generated method
      this.m_Ping.m_ContentDraw = new Action<Rect>(itemCAnonStorey0.\u003C\u003Em__0);
      this.m_TreeView.Repaint();
    }

    public virtual void EndPingItem()
    {
      this.m_Ping.m_TimeStart = -1f;
    }

    private void HandlePing()
    {
      this.m_Ping.HandlePing();
      if (!this.m_Ping.isPinging)
        return;
      this.m_TreeView.Repaint();
    }

    protected RenameOverlay GetRenameOverlay()
    {
      return this.m_TreeView.state.renameOverlay;
    }

    protected virtual bool IsRenaming(int id)
    {
      return this.GetRenameOverlay().IsRenaming() && this.GetRenameOverlay().userData == id && !this.GetRenameOverlay().isWaitingForDelay;
    }

    public virtual bool BeginRename(TreeViewItem item, float delay)
    {
      return this.GetRenameOverlay().BeginRename(item.displayName, item.id, delay);
    }

    public virtual void EndRename()
    {
      if (this.GetRenameOverlay().HasKeyboardFocus())
        this.m_TreeView.GrabKeyboardFocus();
      this.RenameEnded();
      this.ClearRenameAndNewItemState();
    }

    protected virtual void RenameEnded()
    {
    }

    public virtual void DoRenameOverlay()
    {
      if (!this.GetRenameOverlay().IsRenaming() || this.GetRenameOverlay().OnGUI())
        return;
      this.EndRename();
    }

    protected virtual void SyncFakeItem()
    {
    }

    protected virtual void ClearRenameAndNewItemState()
    {
      this.m_TreeView.data.RemoveFakeItem();
      this.GetRenameOverlay().Clear();
    }

    public virtual float GetFoldoutIndent(TreeViewItem item)
    {
      if (this.m_TreeView.isSearching)
        return this.k_BaseIndent;
      return this.k_BaseIndent + (float) item.depth * this.indentWidth;
    }

    public virtual float GetContentIndent(TreeViewItem item)
    {
      return this.GetFoldoutIndent(item) + TreeViewGUI.Styles.foldoutWidth + (float) TreeViewGUI.Styles.lineStyle.margin.left;
    }

    internal static class Styles
    {
      public static GUIStyle foldout = (GUIStyle) "IN Foldout";
      public static GUIStyle insertion = new GUIStyle((GUIStyle) "PR Insertion");
      public static GUIStyle ping = new GUIStyle((GUIStyle) "PR Ping");
      public static GUIStyle toolbarButton = (GUIStyle) "ToolbarButton";
      public static GUIStyle lineStyle = new GUIStyle((GUIStyle) "PR Label");
      public static GUIStyle selectionStyle = new GUIStyle((GUIStyle) "PR Label");
      public static GUIContent content = new GUIContent((Texture) EditorGUIUtility.FindTexture(EditorResourcesUtility.folderIconName));
      public static GUIStyle lineBoldStyle;

      static Styles()
      {
        Texture2D background = TreeViewGUI.Styles.lineStyle.hover.background;
        TreeViewGUI.Styles.lineStyle.onNormal.background = background;
        TreeViewGUI.Styles.lineStyle.onActive.background = background;
        TreeViewGUI.Styles.lineStyle.onFocused.background = background;
        TreeViewGUI.Styles.lineStyle.alignment = TextAnchor.UpperLeft;
        TreeViewGUI.Styles.lineStyle.padding.top = 2;
        TreeViewGUI.Styles.lineStyle.fixedHeight = 0.0f;
        TreeViewGUI.Styles.lineBoldStyle = new GUIStyle(TreeViewGUI.Styles.lineStyle);
        TreeViewGUI.Styles.lineBoldStyle.font = EditorStyles.boldLabel.font;
        TreeViewGUI.Styles.lineBoldStyle.fontStyle = EditorStyles.boldLabel.fontStyle;
        TreeViewGUI.Styles.ping.padding.left = 16;
        TreeViewGUI.Styles.ping.padding.right = 16;
        TreeViewGUI.Styles.ping.fixedHeight = 16f;
        TreeViewGUI.Styles.selectionStyle.fixedHeight = 0.0f;
        TreeViewGUI.Styles.selectionStyle.border = new RectOffset();
        TreeViewGUI.Styles.insertion.overflow = new RectOffset(4, 0, 0, 0);
      }

      public static float foldoutWidth
      {
        get
        {
          return TreeViewGUI.Styles.foldout.fixedWidth;
        }
      }
    }
  }
}
