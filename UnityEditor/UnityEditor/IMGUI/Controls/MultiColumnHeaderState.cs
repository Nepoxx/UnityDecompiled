// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.MultiColumnHeaderState
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
  ///   <para>State used by the MultiColumnHeader.</para>
  /// </summary>
  [Serializable]
  public class MultiColumnHeaderState
  {
    [NonSerialized]
    private int m_MaxNumberOfSortedColumns = 3;
    [SerializeField]
    private MultiColumnHeaderState.Column[] m_Columns;
    [SerializeField]
    private int[] m_VisibleColumns;
    [SerializeField]
    private List<int> m_SortedColumns;

    public MultiColumnHeaderState(MultiColumnHeaderState.Column[] columns)
    {
      if (columns == null)
        throw new ArgumentException("columns are no allowed to be null", nameof (columns));
      if (columns.Length == 0)
        throw new ArgumentException("columns array should at least have one column: it is empty", nameof (columns));
      this.m_Columns = columns;
      this.m_SortedColumns = new List<int>();
      this.m_VisibleColumns = new int[this.m_Columns.Length];
      for (int index = 0; index < this.m_Columns.Length; ++index)
        this.m_VisibleColumns[index] = index;
    }

    /// <summary>
    ///   <para>Checks if the source state can transfer its serialized data to the destination state.</para>
    /// </summary>
    /// <param name="source">State that have serialized data to be transfered to the destination state.</param>
    /// <param name="destination">Destination state.</param>
    /// <returns>
    ///   <para>Returns true if the source state have the same number of columns as the destination state.</para>
    /// </returns>
    public static bool CanOverwriteSerializedFields(MultiColumnHeaderState source, MultiColumnHeaderState destination)
    {
      if (source == null || destination == null || (source.m_Columns == null || destination.m_Columns == null))
        return false;
      return source.m_Columns.Length == destination.m_Columns.Length;
    }

    /// <summary>
    ///   <para>Overwrites the seralized fields from the source state to the destination state.</para>
    /// </summary>
    /// <param name="source">State that have serialized data to be transfered to the destination state.</param>
    /// <param name="destination">Destination state.</param>
    public static void OverwriteSerializedFields(MultiColumnHeaderState source, MultiColumnHeaderState destination)
    {
      if (!MultiColumnHeaderState.CanOverwriteSerializedFields(source, destination))
      {
        Debug.LogError((object) "MultiColumnHeaderState: Not able to overwrite serialized fields");
      }
      else
      {
        destination.m_VisibleColumns = ((IEnumerable<int>) source.m_VisibleColumns).ToArray<int>();
        destination.m_SortedColumns = new List<int>((IEnumerable<int>) source.m_SortedColumns);
        for (int index = 0; index < destination.m_Columns.Length; ++index)
        {
          destination.m_Columns[index].width = source.m_Columns[index].width;
          destination.m_Columns[index].sortedAscending = source.m_Columns[index].sortedAscending;
        }
      }
    }

    /// <summary>
    ///   <para>This property holds the index to the primary sorted column.</para>
    /// </summary>
    public int sortedColumnIndex
    {
      get
      {
        return this.m_SortedColumns.Count <= 0 ? -1 : this.m_SortedColumns[0];
      }
      set
      {
        int num = this.m_SortedColumns.Count <= 0 ? -1 : this.m_SortedColumns[0];
        if (value == num)
          return;
        if (value >= 0)
        {
          this.m_SortedColumns.Remove(value);
          this.m_SortedColumns.Insert(0, value);
        }
        else
          this.m_SortedColumns.Clear();
      }
    }

    private void RemoveInvalidSortingColumnsIndices()
    {
      this.m_SortedColumns.RemoveAll((Predicate<int>) (x => x >= this.m_Columns.Length));
      if (this.m_SortedColumns.Count <= this.m_MaxNumberOfSortedColumns)
        return;
      this.m_SortedColumns.RemoveRange(this.m_MaxNumberOfSortedColumns, this.m_SortedColumns.Count - this.m_MaxNumberOfSortedColumns);
    }

    /// <summary>
    ///   <para>This property controls the maximum number of columns returned by the sortedColumns property.</para>
    /// </summary>
    public int maximumNumberOfSortedColumns
    {
      get
      {
        return this.m_MaxNumberOfSortedColumns;
      }
      set
      {
        this.m_MaxNumberOfSortedColumns = value;
        this.RemoveInvalidSortingColumnsIndices();
      }
    }

    /// <summary>
    ///   <para>The array of column indices for multiple column sorting.</para>
    /// </summary>
    public int[] sortedColumns
    {
      get
      {
        return this.m_SortedColumns.ToArray();
      }
      set
      {
        this.m_SortedColumns = value != null ? new List<int>((IEnumerable<int>) value) : new List<int>();
        this.RemoveInvalidSortingColumnsIndices();
      }
    }

    /// <summary>
    ///   <para>The array of column states used by the MultiColumnHeader class.</para>
    /// </summary>
    public MultiColumnHeaderState.Column[] columns
    {
      get
      {
        return this.m_Columns;
      }
    }

    /// <summary>
    ///   <para>This is the array of currently visible column indices.</para>
    /// </summary>
    public int[] visibleColumns
    {
      get
      {
        return this.m_VisibleColumns;
      }
      set
      {
        if (value == null)
          throw new ArgumentException("visibleColumns should not be set to null");
        if (value.Length == 0)
          throw new ArgumentException("visibleColumns should should not be set to an empty array. At least one visible column is required.");
        this.m_VisibleColumns = value;
      }
    }

    /// <summary>
    ///   <para>Returns the sum of all the widths of the visible columns in the visibleColumns array.</para>
    /// </summary>
    public float widthOfAllVisibleColumns
    {
      get
      {
        return ((IEnumerable<int>) this.visibleColumns).Sum<int>((Func<int, float>) (t => this.columns[t].width));
      }
    }

    /// <summary>
    ///   <para>Column state.</para>
    /// </summary>
    [Serializable]
    public class Column
    {
      /// <summary>
      ///   <para>The width of the column.</para>
      /// </summary>
      [SerializeField]
      public float width = 50f;
      /// <summary>
      ///   <para>This is the GUIContent that will be rendered in the column header.</para>
      /// </summary>
      [NonSerialized]
      public GUIContent headerContent = new GUIContent();
      /// <summary>
      ///   <para>Alignment of the header content.</para>
      /// </summary>
      [NonSerialized]
      public TextAlignment headerTextAlignment = TextAlignment.Left;
      /// <summary>
      ///   <para>Alignment of the sorting arrow.</para>
      /// </summary>
      [NonSerialized]
      public TextAlignment sortingArrowAlignment = TextAlignment.Center;
      /// <summary>
      ///   <para>Minimum width of the column.</para>
      /// </summary>
      [NonSerialized]
      public float minWidth = 20f;
      /// <summary>
      ///   <para>Maximum width of the column.</para>
      /// </summary>
      [NonSerialized]
      public float maxWidth = 1000000f;
      /// <summary>
      ///   <para>Option to allow the column to resize automatically when resizing the entire MultiColumnHeader.</para>
      /// </summary>
      [NonSerialized]
      public bool autoResize = true;
      /// <summary>
      ///   <para>Option to allow/disallow hiding the column from the context menu.</para>
      /// </summary>
      [NonSerialized]
      public bool allowToggleVisibility = true;
      /// <summary>
      ///   <para>Is sorting enabled for this column. If false, left-clicking this column header has no effect.</para>
      /// </summary>
      [NonSerialized]
      public bool canSort = true;
      /// <summary>
      ///   <para>Value that controls if this column is sorted ascending or descending.</para>
      /// </summary>
      [SerializeField]
      public bool sortedAscending;
      /// <summary>
      ///   <para>If this is set then it is used for the context menu for toggling visibility, if not set then the ::headerContent is used.</para>
      /// </summary>
      [NonSerialized]
      public string contextMenuText;
    }
  }
}
