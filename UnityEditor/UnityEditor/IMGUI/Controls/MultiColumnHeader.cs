// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.MultiColumnHeader
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  /// <summary>
  ///   <para>The MultiColumnHeader is a general purpose class that e.g can be used with the TreeView to create multi-column tree views and list views.</para>
  /// </summary>
  public class MultiColumnHeader
  {
    private float m_Height = MultiColumnHeader.DefaultGUI.defaultHeight;
    private float m_DividerWidth = 6f;
    private bool m_ResizeToFit = false;
    private bool m_CanSort = true;
    private MultiColumnHeaderState m_State;
    private Rect m_PreviousRect;
    private GUIView m_GUIView;
    private Rect[] m_ColumnRects;

    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    /// <param name="state">Column header state and Column state.</param>
    public MultiColumnHeader(MultiColumnHeaderState state)
    {
      this.m_State = state;
    }

    public event MultiColumnHeader.HeaderCallback sortingChanged;

    public event MultiColumnHeader.HeaderCallback visibleColumnsChanged;

    /// <summary>
    ///   <para>The index of the column that is set to be the primary sorting column. This is the column that shows the sorting arrow above the header text.</para>
    /// </summary>
    public int sortedColumnIndex
    {
      get
      {
        return this.state.sortedColumnIndex;
      }
      set
      {
        if (value == this.state.sortedColumnIndex)
          return;
        this.state.sortedColumnIndex = value;
        this.OnSortingChanged();
      }
    }

    /// <summary>
    ///   <para>Sets multiple sorting columns and the associated sorting orders.</para>
    /// </summary>
    /// <param name="columnIndices">Column indices of the sorted columns.</param>
    /// <param name="sortAscending">Sorting order for the column indices specified.</param>
    public void SetSortingColumns(int[] columnIndices, bool[] sortAscending)
    {
      if (columnIndices == null)
        throw new ArgumentNullException(nameof (columnIndices));
      if (sortAscending == null)
        throw new ArgumentNullException(nameof (sortAscending));
      if (columnIndices.Length != sortAscending.Length)
        throw new ArgumentException("Input arrays should have same length");
      if (columnIndices.Length > this.state.maximumNumberOfSortedColumns)
        throw new ArgumentException("The maximum number of sorted columns is " + (object) this.state.maximumNumberOfSortedColumns + ". Trying to set " + (object) columnIndices.Length + " columns.");
      if (columnIndices.Length != ((IEnumerable<int>) columnIndices).Distinct<int>().Count<int>())
        throw new ArgumentException("Duplicate column indices are not allowed", nameof (columnIndices));
      bool flag = false;
      if (!((IEnumerable<int>) columnIndices).SequenceEqual<int>((IEnumerable<int>) this.state.sortedColumns))
      {
        this.state.sortedColumns = columnIndices;
        flag = true;
      }
      for (int index = 0; index < columnIndices.Length; ++index)
      {
        MultiColumnHeaderState.Column column = this.GetColumn(columnIndices[index]);
        if (column.sortedAscending != sortAscending[index])
        {
          column.sortedAscending = sortAscending[index];
          flag = true;
        }
      }
      if (!flag)
        return;
      this.OnSortingChanged();
    }

    /// <summary>
    ///   <para>Sets the primary sorting column and its sorting order.</para>
    /// </summary>
    /// <param name="columnIndex">Column to sort.</param>
    /// <param name="sortAscending">Sorting order for the column specified.</param>
    public void SetSorting(int columnIndex, bool sortAscending)
    {
      bool flag = false;
      if (this.state.sortedColumnIndex != columnIndex)
      {
        this.state.sortedColumnIndex = columnIndex;
        flag = true;
      }
      MultiColumnHeaderState.Column column = this.GetColumn(columnIndex);
      if (column.sortedAscending != sortAscending)
      {
        column.sortedAscending = sortAscending;
        flag = true;
      }
      if (!flag)
        return;
      this.OnSortingChanged();
    }

    /// <summary>
    ///   <para>Change sort direction for a given column.</para>
    /// </summary>
    /// <param name="columnIndex">Column index.</param>
    /// <param name="sortAscending">Direction of the sorting.</param>
    public void SetSortDirection(int columnIndex, bool sortAscending)
    {
      MultiColumnHeaderState.Column column = this.GetColumn(columnIndex);
      if (column.sortedAscending == sortAscending)
        return;
      column.sortedAscending = sortAscending;
      this.OnSortingChanged();
    }

    /// <summary>
    ///   <para>Check the sorting order state for a column.</para>
    /// </summary>
    /// <param name="columnIndex">Column index.</param>
    /// <returns>
    ///   <para>True if sorted ascending.</para>
    /// </returns>
    public bool IsSortedAscending(int columnIndex)
    {
      return this.GetColumn(columnIndex).sortedAscending;
    }

    /// <summary>
    ///   <para>Returns the column data for a given column index.</para>
    /// </summary>
    /// <param name="columnIndex">Column index.</param>
    /// <returns>
    ///   <para>Column data.</para>
    /// </returns>
    public MultiColumnHeaderState.Column GetColumn(int columnIndex)
    {
      if (columnIndex < 0 || columnIndex >= this.state.columns.Length)
        throw new ArgumentOutOfRangeException(nameof (columnIndex), string.Format("columnIndex {0} is not valid when the current column count is {1}", (object) columnIndex, (object) this.state.columns.Length));
      return this.state.columns[columnIndex];
    }

    /// <summary>
    ///   <para>This is the state of the MultiColumnHeader.</para>
    /// </summary>
    public MultiColumnHeaderState state
    {
      get
      {
        return this.m_State;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (state), "MultiColumnHeader state is not allowed to be null");
        this.m_State = value;
      }
    }

    /// <summary>
    ///   <para>Customizable height of the multi column header.</para>
    /// </summary>
    public float height
    {
      get
      {
        return this.m_Height;
      }
      set
      {
        this.m_Height = value;
      }
    }

    /// <summary>
    ///   <para>Use this property to control whether sorting is enabled for all the columns.</para>
    /// </summary>
    public bool canSort
    {
      get
      {
        return this.m_CanSort;
      }
      set
      {
        this.m_CanSort = value;
        this.height = this.m_Height;
      }
    }

    /// <summary>
    ///   <para>Check if a column is currently visible in the MultiColumnHeader.</para>
    /// </summary>
    /// <param name="columnIndex">Column index.</param>
    public bool IsColumnVisible(int columnIndex)
    {
      return ((IEnumerable<int>) this.state.visibleColumns).Any<int>((Func<int, bool>) (t => t == columnIndex));
    }

    /// <summary>
    ///   <para>Convert from column index to visible column index.</para>
    /// </summary>
    /// <param name="columnIndex">Column index.</param>
    /// <returns>
    ///   <para>Visible column index.</para>
    /// </returns>
    public int GetVisibleColumnIndex(int columnIndex)
    {
      for (int index = 0; index < this.state.visibleColumns.Length; ++index)
      {
        if (this.state.visibleColumns[index] == columnIndex)
          return index;
      }
      string str = string.Join(", ", ((IEnumerable<int>) this.state.visibleColumns).Select<int, string>((Func<int, string>) (t => t.ToString())).ToArray<string>());
      throw new ArgumentException(string.Format("Invalid columnIndex: {0}. The index is not part of the current visible columns: {1}", (object) columnIndex, (object) str), nameof (columnIndex));
    }

    /// <summary>
    ///   <para>Calculates a cell rect for a column and row using the visibleColumnIndex and rowRect parameters.</para>
    /// </summary>
    /// <param name="visibleColumnIndex"></param>
    /// <param name="rowRect"></param>
    public Rect GetCellRect(int visibleColumnIndex, Rect rowRect)
    {
      Rect columnRect = this.GetColumnRect(visibleColumnIndex);
      columnRect.y = rowRect.y;
      columnRect.height = rowRect.height;
      return columnRect;
    }

    /// <summary>
    ///   <para>Returns the header column Rect for a given visible column index.</para>
    /// </summary>
    /// <param name="visibleColumnIndex">Index of a visible column.</param>
    public Rect GetColumnRect(int visibleColumnIndex)
    {
      if (visibleColumnIndex < 0 || visibleColumnIndex >= this.m_ColumnRects.Length)
        throw new ArgumentException(string.Format("The provided visibleColumnIndex is invalid. Ensure the index ({0}) is within the number of visible columns ({1})", (object) visibleColumnIndex, (object) this.m_ColumnRects.Length), nameof (visibleColumnIndex));
      return this.m_ColumnRects[visibleColumnIndex];
    }

    /// <summary>
    ///   <para>Resizes the column widths of the columns that have auto-resize enabled to make all the columns fit to the width of the MultiColumnHeader render rect.</para>
    /// </summary>
    public void ResizeToFit()
    {
      this.m_ResizeToFit = true;
      this.Repaint();
    }

    private void UpdateColumnHeaderRects(Rect totalHeaderRect)
    {
      if (this.m_ColumnRects == null || this.m_ColumnRects.Length != this.state.visibleColumns.Length)
        this.m_ColumnRects = new Rect[this.state.visibleColumns.Length];
      Rect rect = totalHeaderRect;
      for (int index = 0; index < this.state.visibleColumns.Length; ++index)
      {
        MultiColumnHeaderState.Column column = this.state.columns[this.state.visibleColumns[index]];
        if (index > 0)
          rect.x += rect.width;
        rect.width = column.width;
        this.m_ColumnRects[index] = rect;
      }
    }

    /// <summary>
    ///   <para>Render and handle input for the MultiColumnHeader at the given rect.</para>
    /// </summary>
    /// <param name="xScroll">Horizontal scroll offset.</param>
    /// <param name="rect">Rect where the MultiColumnHeader is drawn in.</param>
    public virtual void OnGUI(Rect rect, float xScroll)
    {
      Event current = Event.current;
      if ((UnityEngine.Object) this.m_GUIView == (UnityEngine.Object) null)
        this.m_GUIView = GUIView.current;
      this.DetectSizeChanges(rect);
      if (this.m_ResizeToFit && current.type == EventType.Repaint)
      {
        this.m_ResizeToFit = false;
        this.ResizeColumnsWidthsProportionally(rect.width - GUI.skin.verticalScrollbar.fixedWidth - this.state.widthOfAllVisibleColumns);
      }
      GUIClip.Push(rect, new Vector2(-xScroll, 0.0f), Vector2.zero, false);
      Rect totalHeaderRect = new Rect(0.0f, 0.0f, rect.width, rect.height);
      float allVisibleColumns = this.state.widthOfAllVisibleColumns;
      Rect position1 = new Rect(0.0f, 0.0f, ((double) totalHeaderRect.width <= (double) allVisibleColumns ? allVisibleColumns : totalHeaderRect.width) + GUI.skin.verticalScrollbar.fixedWidth, totalHeaderRect.height);
      GUI.Label(position1, GUIContent.none, MultiColumnHeader.DefaultStyles.background);
      if (current.type == EventType.ContextClick && position1.Contains(current.mousePosition))
      {
        current.Use();
        this.DoContextMenu();
      }
      this.UpdateColumnHeaderRects(totalHeaderRect);
      for (int index = 0; index < this.state.visibleColumns.Length; ++index)
      {
        int visibleColumn = this.state.visibleColumns[index];
        MultiColumnHeaderState.Column column = this.state.columns[visibleColumn];
        Rect columnRect = this.m_ColumnRects[index];
        Rect dividerRect = new Rect(columnRect.xMax - 1f, columnRect.y + 4f, 1f, columnRect.height - 8f);
        Rect position2 = new Rect(dividerRect.x - this.m_DividerWidth * 0.5f, totalHeaderRect.y, this.m_DividerWidth, totalHeaderRect.height);
        bool hasControl;
        column.width = EditorGUI.WidthResizer(position2, column.width, column.minWidth, column.maxWidth, out hasControl);
        if (hasControl && current.type == EventType.Repaint)
          this.DrawColumnResizing(columnRect, column);
        this.DrawDivider(dividerRect, column);
        this.ColumnHeaderGUI(column, columnRect, visibleColumn);
      }
      GUIClip.Pop();
    }

    internal virtual void DrawColumnResizing(Rect headerRect, MultiColumnHeaderState.Column column)
    {
      ++headerRect.y;
      --headerRect.width;
      headerRect.height -= 2f;
      EditorGUI.DrawRect(headerRect, new Color(0.5f, 0.5f, 0.5f, 0.1f));
    }

    internal virtual void DrawDivider(Rect dividerRect, MultiColumnHeaderState.Column column)
    {
      EditorGUI.DrawRect(dividerRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));
    }

    protected virtual void ColumnHeaderClicked(MultiColumnHeaderState.Column column, int columnIndex)
    {
      if (this.state.sortedColumnIndex == columnIndex)
        column.sortedAscending = !column.sortedAscending;
      else
        this.state.sortedColumnIndex = columnIndex;
      this.OnSortingChanged();
    }

    /// <summary>
    ///   <para>Called when sorting changes and dispatches the sortingChanged event.</para>
    /// </summary>
    protected virtual void OnSortingChanged()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.sortingChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.sortingChanged(this);
    }

    protected virtual void ColumnHeaderGUI(MultiColumnHeaderState.Column column, Rect headerRect, int columnIndex)
    {
      if (this.canSort && column.canSort)
        this.SortingButton(column, headerRect, columnIndex);
      GUIStyle style = this.GetStyle(column.headerTextAlignment);
      float singleLineHeight = EditorGUIUtility.singleLineHeight;
      GUI.Label(new Rect(headerRect.x, headerRect.yMax - singleLineHeight - MultiColumnHeader.DefaultGUI.labelSpaceFromBottom, headerRect.width, singleLineHeight), column.headerContent, style);
    }

    protected void SortingButton(MultiColumnHeaderState.Column column, Rect headerRect, int columnIndex)
    {
      if (EditorGUI.Button(headerRect, GUIContent.none, GUIStyle.none))
        this.ColumnHeaderClicked(column, columnIndex);
      if (columnIndex != this.state.sortedColumnIndex || Event.current.type != EventType.Repaint)
        return;
      Rect arrowRect = this.GetArrowRect(column, headerRect);
      Matrix4x4 matrix = GUI.matrix;
      if (column.sortedAscending)
        GUIUtility.RotateAroundPivot(180f, arrowRect.center - new Vector2(0.0f, 1f));
      GUI.Label(arrowRect, "▾", MultiColumnHeader.DefaultStyles.arrowStyle);
      if (column.sortedAscending)
        GUI.matrix = matrix;
    }

    internal virtual Rect GetArrowRect(MultiColumnHeaderState.Column column, Rect headerRect)
    {
      float fixedWidth = MultiColumnHeader.DefaultStyles.arrowStyle.fixedWidth;
      float y = headerRect.y;
      float f = 0.0f;
      switch (column.sortingArrowAlignment)
      {
        case TextAlignment.Left:
          f = headerRect.x + (float) MultiColumnHeader.DefaultStyles.columnHeader.padding.left;
          break;
        case TextAlignment.Center:
          f = (float) ((double) headerRect.x + (double) headerRect.width * 0.5 - (double) fixedWidth * 0.5);
          break;
        case TextAlignment.Right:
          f = headerRect.xMax - (float) MultiColumnHeader.DefaultStyles.columnHeader.padding.right - fixedWidth;
          break;
        default:
          Debug.LogError((object) "Unhandled enum");
          break;
      }
      return new Rect(Mathf.Round(f), y, fixedWidth, 16f);
    }

    private GUIStyle GetStyle(TextAlignment alignment)
    {
      switch (alignment)
      {
        case TextAlignment.Left:
          return MultiColumnHeader.DefaultStyles.columnHeader;
        case TextAlignment.Center:
          return MultiColumnHeader.DefaultStyles.columnHeaderCenterAligned;
        case TextAlignment.Right:
          return MultiColumnHeader.DefaultStyles.columnHeaderRightAligned;
        default:
          return MultiColumnHeader.DefaultStyles.columnHeader;
      }
    }

    private void DoContextMenu()
    {
      GenericMenu menu = new GenericMenu();
      this.AddColumnHeaderContextMenuItems(menu);
      menu.ShowAsContext();
    }

    /// <summary>
    ///   <para>Override this method to extend the default context menu items shown when context clicking the header area.</para>
    /// </summary>
    /// <param name="menu">Context menu shown.</param>
    protected virtual void AddColumnHeaderContextMenuItems(GenericMenu menu)
    {
      menu.AddItem(new GUIContent("Resize to Fit"), false, new GenericMenu.MenuFunction(this.ResizeToFit));
      menu.AddSeparator("");
      for (int index = 0; index < this.state.columns.Length; ++index)
      {
        MultiColumnHeaderState.Column column = this.state.columns[index];
        string text = string.IsNullOrEmpty(column.contextMenuText) ? column.headerContent.text : column.contextMenuText;
        if (column.allowToggleVisibility)
          menu.AddItem(new GUIContent(text), ((IEnumerable<int>) this.state.visibleColumns).Contains<int>(index), new GenericMenu.MenuFunction2(this.ToggleVisibility), (object) index);
        else
          menu.AddDisabledItem(new GUIContent(text));
      }
    }

    /// <summary>
    ///   <para>Called when the number of visible column changes and dispatches the visibleColumnsChanged event.</para>
    /// </summary>
    protected virtual void OnVisibleColumnsChanged()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.visibleColumnsChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.visibleColumnsChanged(this);
    }

    private void ToggleVisibility(object userData)
    {
      this.ToggleVisibility((int) userData);
    }

    /// <summary>
    ///   <para>Method for toggling the visibility of a column.</para>
    /// </summary>
    /// <param name="columnIndex">Toggle visibility for this column.</param>
    protected virtual void ToggleVisibility(int columnIndex)
    {
      List<int> intList = new List<int>((IEnumerable<int>) this.state.visibleColumns);
      if (intList.Contains(columnIndex))
      {
        intList.Remove(columnIndex);
      }
      else
      {
        intList.Add(columnIndex);
        intList.Sort();
      }
      this.state.visibleColumns = intList.ToArray();
      this.Repaint();
      this.OnVisibleColumnsChanged();
    }

    /// <summary>
    ///   <para>Requests the window which contains the MultiColumnHeader to repaint.</para>
    /// </summary>
    public void Repaint()
    {
      if (!((UnityEngine.Object) this.m_GUIView != (UnityEngine.Object) null))
        return;
      this.m_GUIView.Repaint();
    }

    private void DetectSizeChanges(Rect rect)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      if ((double) this.m_PreviousRect.width > 0.0)
      {
        float deltaWidth = Mathf.Round(rect.width - this.m_PreviousRect.width);
        if ((double) deltaWidth != 0.0)
        {
          float fixedWidth = GUI.skin.verticalScrollbar.fixedWidth;
          if ((double) rect.width - (double) fixedWidth > (double) this.state.widthOfAllVisibleColumns || (double) deltaWidth < 0.0)
            this.ResizeColumnsWidthsProportionally(deltaWidth);
        }
      }
      this.m_PreviousRect = rect;
    }

    private void ResizeColumnsWidthsProportionally(float deltaWidth)
    {
      List<MultiColumnHeaderState.Column> source = (List<MultiColumnHeaderState.Column>) null;
      foreach (int visibleColumn in this.state.visibleColumns)
      {
        MultiColumnHeaderState.Column column = this.state.columns[visibleColumn];
        if (column.autoResize && ((double) deltaWidth <= 0.0 || (double) column.width < (double) column.maxWidth) && ((double) deltaWidth >= 0.0 || (double) column.width > (double) column.minWidth))
        {
          if (source == null)
            source = new List<MultiColumnHeaderState.Column>();
          source.Add(column);
        }
      }
      if (source == null)
        return;
      float num = source.Sum<MultiColumnHeaderState.Column>((Func<MultiColumnHeaderState.Column, float>) (x => x.width));
      foreach (MultiColumnHeaderState.Column column in source)
      {
        column.width += deltaWidth * (column.width / num);
        column.width = Mathf.Clamp(column.width, column.minWidth, column.maxWidth);
      }
    }

    /// <summary>
    ///   <para>Delegate used for events from the MultiColumnHeader.</para>
    /// </summary>
    /// <param name="multiColumnHeader">The MultiColumnHeader that dispatched this event.</param>
    public delegate void HeaderCallback(MultiColumnHeader multiColumnHeader);

    /// <summary>
    ///   <para>Default GUI methods and properties for the MultiColumnHeader class.</para>
    /// </summary>
    public static class DefaultGUI
    {
      /// <summary>
      ///   <para>Default height of the header.</para>
      /// </summary>
      public static float defaultHeight
      {
        get
        {
          return 27f;
        }
      }

      /// <summary>
      ///   <para>This height is the minium height the header can have and can only be used if sorting is disabled.</para>
      /// </summary>
      public static float minimumHeight
      {
        get
        {
          return 20f;
        }
      }

      /// <summary>
      ///   <para>Margin that can be used by clients of the MultiColumnHeader to control spacing between content in multiple columns.</para>
      /// </summary>
      public static float columnContentMargin
      {
        get
        {
          return 3f;
        }
      }

      internal static float labelSpaceFromBottom
      {
        get
        {
          return 3f;
        }
      }
    }

    /// <summary>
    ///   <para>Default styles used by the MultiColumnHeader class.</para>
    /// </summary>
    public static class DefaultStyles
    {
      /// <summary>
      ///   <para>Style used for rendering the background of the header.</para>
      /// </summary>
      public static GUIStyle background = new GUIStyle((GUIStyle) "ProjectBrowserTopBarBg");
      /// <summary>
      ///   <para>Style used for left aligned header text.</para>
      /// </summary>
      public static GUIStyle columnHeader;
      /// <summary>
      ///   <para>Style used for right aligned header text.</para>
      /// </summary>
      public static GUIStyle columnHeaderRightAligned;
      /// <summary>
      ///   <para>Style used for centered header text.</para>
      /// </summary>
      public static GUIStyle columnHeaderCenterAligned;
      internal static GUIStyle arrowStyle;

      static DefaultStyles()
      {
        MultiColumnHeader.DefaultStyles.background.fixedHeight = 0.0f;
        MultiColumnHeader.DefaultStyles.background.border = new RectOffset(3, 3, 3, 3);
        MultiColumnHeader.DefaultStyles.columnHeader = new GUIStyle(EditorStyles.label);
        MultiColumnHeader.DefaultStyles.columnHeader.alignment = TextAnchor.MiddleLeft;
        MultiColumnHeader.DefaultStyles.columnHeader.padding = new RectOffset(4, 4, 0, 0);
        Color textColor = MultiColumnHeader.DefaultStyles.columnHeader.normal.textColor;
        textColor.a = 0.8f;
        MultiColumnHeader.DefaultStyles.columnHeader.normal.textColor = textColor;
        MultiColumnHeader.DefaultStyles.columnHeaderRightAligned = new GUIStyle(MultiColumnHeader.DefaultStyles.columnHeader);
        MultiColumnHeader.DefaultStyles.columnHeaderRightAligned.alignment = TextAnchor.MiddleRight;
        MultiColumnHeader.DefaultStyles.columnHeaderCenterAligned = new GUIStyle(MultiColumnHeader.DefaultStyles.columnHeader);
        MultiColumnHeader.DefaultStyles.columnHeaderCenterAligned.alignment = TextAnchor.MiddleCenter;
        MultiColumnHeader.DefaultStyles.arrowStyle = new GUIStyle(EditorStyles.label);
        MultiColumnHeader.DefaultStyles.arrowStyle.padding = new RectOffset();
        MultiColumnHeader.DefaultStyles.arrowStyle.fixedWidth = 13f;
        MultiColumnHeader.DefaultStyles.arrowStyle.alignment = TextAnchor.UpperCenter;
      }
    }
  }
}
