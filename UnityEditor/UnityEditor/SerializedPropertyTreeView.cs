// Decompiled with JetBrains decompiler
// Type: UnityEditor.SerializedPropertyTreeView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Profiling;

namespace UnityEditor
{
  internal class SerializedPropertyTreeView : TreeView
  {
    private SerializedPropertyDataStore m_DataStore;
    private SerializedPropertyTreeView.ColumnInternal[] m_ColumnsInternal;
    private List<TreeViewItem> m_Items;
    private int m_ChangedId;
    private bool m_bFilterSelection;
    private int[] m_SelectionFilter;

    public SerializedPropertyTreeView(TreeViewState state, MultiColumnHeader multicolumnHeader, SerializedPropertyDataStore dataStore)
      : base(state, multicolumnHeader)
    {
      this.m_DataStore = dataStore;
      int length = this.multiColumnHeader.state.columns.Length;
      this.m_ColumnsInternal = new SerializedPropertyTreeView.ColumnInternal[length];
      for (int idx = 0; idx < length; ++idx)
      {
        SerializedPropertyTreeView.Column column = this.Col(idx);
        if (column.propertyName != null)
          this.m_ColumnsInternal[idx].dependencyProps = new SerializedProperty[column.propertyName.Length];
      }
      this.multiColumnHeader.sortingChanged += new MultiColumnHeader.HeaderCallback(this.OnSortingChanged);
      this.multiColumnHeader.visibleColumnsChanged += new MultiColumnHeader.HeaderCallback(this.OnVisibleColumnChanged);
      this.showAlternatingRowBackgrounds = true;
      this.showBorder = true;
      this.rowHeight = EditorGUIUtility.singleLineHeight;
    }

    public void SerializeState(string uid)
    {
      SessionState.SetBool(uid + SerializedPropertyTreeView.Styles.serializeFilterSelection, this.m_bFilterSelection);
      for (int idx = 0; idx < this.multiColumnHeader.state.columns.Length; ++idx)
      {
        SerializedPropertyFilters.IFilter filter = this.Col(idx).filter;
        if (filter != null)
        {
          string str = filter.SerializeState();
          if (!string.IsNullOrEmpty(str))
            SessionState.SetString(uid + SerializedPropertyTreeView.Styles.serializeFilter + (object) idx, str);
        }
      }
      SessionState.SetString(uid + SerializedPropertyTreeView.Styles.serializeTreeViewState, JsonUtility.ToJson((object) this.state));
      SessionState.SetString(uid + SerializedPropertyTreeView.Styles.serializeColumnHeaderState, JsonUtility.ToJson((object) this.multiColumnHeader.state));
    }

    public void DeserializeState(string uid)
    {
      this.m_bFilterSelection = SessionState.GetBool(uid + SerializedPropertyTreeView.Styles.serializeFilterSelection, false);
      for (int idx = 0; idx < this.multiColumnHeader.state.columns.Length; ++idx)
      {
        SerializedPropertyFilters.IFilter filter = this.Col(idx).filter;
        if (filter != null)
        {
          string state = SessionState.GetString(uid + SerializedPropertyTreeView.Styles.serializeFilter + (object) idx, (string) null);
          if (!string.IsNullOrEmpty(state))
            filter.DeserializeState(state);
        }
      }
      string json1 = SessionState.GetString(uid + SerializedPropertyTreeView.Styles.serializeTreeViewState, "");
      string json2 = SessionState.GetString(uid + SerializedPropertyTreeView.Styles.serializeColumnHeaderState, "");
      if (!string.IsNullOrEmpty(json1))
        JsonUtility.FromJsonOverwrite(json1, (object) this.state);
      if (string.IsNullOrEmpty(json2))
        return;
      JsonUtility.FromJsonOverwrite(json2, (object) this.multiColumnHeader.state);
    }

    public bool IsFilteredDirty()
    {
      return this.m_ChangedId != 0 && (this.m_ChangedId != GUIUtility.keyboardControl || !EditorGUIUtility.editingTextField);
    }

    public bool Update()
    {
      IList<TreeViewItem> rows = this.GetRows();
      int firstRowVisible;
      int lastRowVisible;
      this.GetFirstAndLastVisibleRows(out firstRowVisible, out lastRowVisible);
      bool flag = false;
      if (lastRowVisible != -1)
      {
        for (int index = firstRowVisible; index <= lastRowVisible; ++index)
          flag = flag || ((SerializedPropertyTreeView.SerializedPropertyItem) rows[index]).GetData().Update();
      }
      return flag;
    }

    public void FullReload()
    {
      this.m_Items = (List<TreeViewItem>) null;
      this.Reload();
    }

    protected override TreeViewItem BuildRoot()
    {
      return (TreeViewItem) new SerializedPropertyTreeView.SerializedPropertyItem(-1, -1, (SerializedPropertyDataStore.Data) null);
    }

    protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
    {
      if (this.m_Items == null)
      {
        SerializedPropertyDataStore.Data[] elements = this.m_DataStore.GetElements();
        this.m_Items = new List<TreeViewItem>(elements.Length);
        for (int index = 0; index < elements.Length; ++index)
          this.m_Items.Add((TreeViewItem) new SerializedPropertyTreeView.SerializedPropertyItem(elements[index].objectId, 0, elements[index]));
      }
      IEnumerable<TreeViewItem> rows = (IEnumerable<TreeViewItem>) this.m_Items;
      if (this.m_bFilterSelection)
      {
        if (this.m_SelectionFilter == null)
          this.m_SelectionFilter = Selection.instanceIDs;
        rows = this.m_Items.Where<TreeViewItem>((Func<TreeViewItem, bool>) (item => ((IEnumerable<int>) this.m_SelectionFilter).Contains<int>(item.id)));
      }
      else
        this.m_SelectionFilter = (int[]) null;
      List<TreeViewItem> list = this.Filter(rows).ToList<TreeViewItem>();
      if (this.multiColumnHeader.sortedColumnIndex >= 0)
        this.Sort((IList<TreeViewItem>) list, this.multiColumnHeader.sortedColumnIndex);
      this.m_ChangedId = 0;
      TreeViewUtility.SetParentAndChildrenForItems((IList<TreeViewItem>) list, root);
      return (IList<TreeViewItem>) list;
    }

    protected override void RowGUI(TreeView.RowGUIArgs args)
    {
      SerializedPropertyTreeView.SerializedPropertyItem serializedPropertyItem = (SerializedPropertyTreeView.SerializedPropertyItem) args.item;
      for (int visibleColumnIndex = 0; visibleColumnIndex < args.GetNumVisibleColumns(); ++visibleColumnIndex)
        this.CellGUI(args.GetCellRect(visibleColumnIndex), serializedPropertyItem, args.GetColumn(visibleColumnIndex), ref args);
    }

    private void CellGUI(Rect cellRect, SerializedPropertyTreeView.SerializedPropertyItem item, int columnIndex, ref TreeView.RowGUIArgs args)
    {
      Profiler.BeginSample("SerializedPropertyTreeView.CellGUI");
      this.CenterRectUsingSingleLineHeight(ref cellRect);
      SerializedPropertyDataStore.Data data1 = item.GetData();
      SerializedPropertyTreeView.Column column = (SerializedPropertyTreeView.Column) this.multiColumnHeader.GetColumn(columnIndex);
      if (column.drawDelegate == SerializedPropertyTreeView.DefaultDelegates.s_DrawName)
      {
        Profiler.BeginSample("SerializedPropertyTreeView.OnItemGUI.LabelField");
        TreeView.DefaultGUI.Label(cellRect, data1.name, this.IsSelected(args.item.id), false);
        Profiler.EndSample();
      }
      else
      {
        if (column.drawDelegate == null)
          return;
        SerializedProperty[] properties = data1.properties;
        int num = column.dependencyIndices == null ? 0 : column.dependencyIndices.Length;
        for (int index = 0; index < num; ++index)
          this.m_ColumnsInternal[columnIndex].dependencyProps[index] = properties[column.dependencyIndices[index]];
        if (args.item.id == this.state.lastClickedID && this.HasFocus() && columnIndex == this.multiColumnHeader.state.visibleColumns[this.multiColumnHeader.state.visibleColumns[0] != 0 ? 0 : 1])
          GUI.SetNextControlName(SerializedPropertyTreeView.Styles.focusHelper);
        SerializedProperty property = data1.properties[columnIndex];
        EditorGUI.BeginChangeCheck();
        Profiler.BeginSample("SerializedPropertyTreeView.OnItemGUI.drawDelegate");
        column.drawDelegate(cellRect, property, this.m_ColumnsInternal[columnIndex].dependencyProps);
        Profiler.EndSample();
        if (EditorGUI.EndChangeCheck())
        {
          this.m_ChangedId = column.filter == null || !column.filter.Active() ? this.m_ChangedId : GUIUtility.keyboardControl;
          data1.Store();
          IList<int> selection = this.GetSelection();
          if (selection.Contains(data1.objectId))
          {
            IList<TreeViewItem> rows = this.FindRows(selection);
            Undo.RecordObjects(rows.Select<TreeViewItem, UnityEngine.Object>((Func<TreeViewItem, UnityEngine.Object>) (r => ((SerializedPropertyTreeView.SerializedPropertyItem) r).GetData().serializedObject.targetObject)).ToArray<UnityEngine.Object>(), "Modify Multiple Properties");
            foreach (TreeViewItem treeViewItem in (IEnumerable<TreeViewItem>) rows)
            {
              if (treeViewItem.id != args.item.id)
              {
                SerializedPropertyDataStore.Data data2 = ((SerializedPropertyTreeView.SerializedPropertyItem) treeViewItem).GetData();
                if (SerializedPropertyTreeView.IsEditable(data2.serializedObject.targetObject))
                {
                  if (column.copyDelegate != null)
                    column.copyDelegate(data2.properties[columnIndex], property);
                  else
                    SerializedPropertyTreeView.DefaultDelegates.s_CopyDefault(data2.properties[columnIndex], property);
                  data2.Store();
                }
              }
            }
          }
        }
        Profiler.EndSample();
      }
    }

    private static bool IsEditable(UnityEngine.Object target)
    {
      return (target.hideFlags & HideFlags.NotEditable) == HideFlags.None;
    }

    protected override void BeforeRowsGUI()
    {
      IList<TreeViewItem> rows = this.GetRows();
      int firstRowVisible;
      int lastRowVisible;
      this.GetFirstAndLastVisibleRows(out firstRowVisible, out lastRowVisible);
      if (lastRowVisible != -1)
      {
        for (int index = firstRowVisible; index <= lastRowVisible; ++index)
          ((SerializedPropertyTreeView.SerializedPropertyItem) rows[index]).GetData().Update();
      }
      foreach (SerializedPropertyTreeView.SerializedPropertyItem row in (IEnumerable<TreeViewItem>) this.FindRows(this.GetSelection()))
        row.GetData().Update();
      base.BeforeRowsGUI();
    }

    public void OnFilterGUI(Rect r)
    {
      EditorGUI.BeginChangeCheck();
      float width = r.width;
      float num = 16f;
      r.width = num;
      this.m_bFilterSelection = EditorGUI.Toggle(r, this.m_bFilterSelection);
      r.x += num;
      r.width = GUI.skin.label.CalcSize(SerializedPropertyTreeView.Styles.filterSelection).x;
      EditorGUI.LabelField(r, SerializedPropertyTreeView.Styles.filterSelection);
      r.width = Mathf.Min(width - (r.x + r.width), 300f);
      r.x = (float) ((double) width - (double) r.width + 10.0);
      for (int idx = 0; idx < this.multiColumnHeader.state.columns.Length; ++idx)
      {
        if (this.IsColumnVisible(idx))
        {
          SerializedPropertyTreeView.Column column = this.Col(idx);
          if (column.filter != null && column.filter.GetType().Equals(typeof (SerializedPropertyFilters.Name)))
            column.filter.OnGUI(r);
        }
      }
      if (!EditorGUI.EndChangeCheck())
        return;
      this.Reload();
    }

    protected override void SelectionChanged(IList<int> selectedIds)
    {
      Selection.instanceIDs = selectedIds.ToArray<int>();
    }

    protected override void KeyEvent()
    {
      if (Event.current.type != EventType.KeyDown || (int) Event.current.character != 9)
        return;
      GUI.FocusControl(SerializedPropertyTreeView.Styles.focusHelper);
      Event.current.Use();
    }

    private void OnVisibleColumnChanged(MultiColumnHeader header)
    {
      this.Reload();
    }

    private void OnSortingChanged(MultiColumnHeader multiColumnHeader)
    {
      this.Sort(this.GetRows(), multiColumnHeader.sortedColumnIndex);
    }

    private void Sort(IList<TreeViewItem> rows, int sortIdx)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SerializedPropertyTreeView.\u003CSort\u003Ec__AnonStorey0 sortCAnonStorey0 = new SerializedPropertyTreeView.\u003CSort\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      sortCAnonStorey0.sortIdx = sortIdx;
      // ISSUE: reference to a compiler-generated field
      bool flag = this.multiColumnHeader.IsSortedAscending(sortCAnonStorey0.sortIdx);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      sortCAnonStorey0.comp = this.Col(sortCAnonStorey0.sortIdx).compareDelegate;
      List<TreeViewItem> treeViewItemList = rows as List<TreeViewItem>;
      // ISSUE: reference to a compiler-generated field
      if (sortCAnonStorey0.comp == null)
        return;
      Comparison<TreeViewItem> comparison1;
      Comparison<TreeViewItem> comparison2;
      // ISSUE: reference to a compiler-generated field
      if (sortCAnonStorey0.comp == SerializedPropertyTreeView.DefaultDelegates.s_CompareName)
      {
        comparison1 = (Comparison<TreeViewItem>) ((lhs, rhs) => EditorUtility.NaturalCompare(((SerializedPropertyTreeView.SerializedPropertyItem) lhs).GetData().name, ((SerializedPropertyTreeView.SerializedPropertyItem) rhs).GetData().name));
        comparison2 = (Comparison<TreeViewItem>) ((lhs, rhs) => -EditorUtility.NaturalCompare(((SerializedPropertyTreeView.SerializedPropertyItem) lhs).GetData().name, ((SerializedPropertyTreeView.SerializedPropertyItem) rhs).GetData().name));
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        comparison1 = new Comparison<TreeViewItem>(sortCAnonStorey0.\u003C\u003Em__0);
        // ISSUE: reference to a compiler-generated method
        comparison2 = new Comparison<TreeViewItem>(sortCAnonStorey0.\u003C\u003Em__1);
      }
      treeViewItemList.Sort(!flag ? comparison2 : comparison1);
    }

    private IEnumerable<TreeViewItem> Filter(IEnumerable<TreeViewItem> rows)
    {
      IEnumerable<TreeViewItem> source = rows;
      int length = this.m_ColumnsInternal.Length;
      for (int idx = 0; idx < length; ++idx)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SerializedPropertyTreeView.\u003CFilter\u003Ec__AnonStorey2 filterCAnonStorey2 = new SerializedPropertyTreeView.\u003CFilter\u003Ec__AnonStorey2();
        if (this.IsColumnVisible(idx))
        {
          // ISSUE: reference to a compiler-generated field
          filterCAnonStorey2.c = this.Col(idx);
          // ISSUE: reference to a compiler-generated field
          filterCAnonStorey2.idx = idx;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (filterCAnonStorey2.c.filter != null && filterCAnonStorey2.c.filter.Active())
          {
            // ISSUE: reference to a compiler-generated field
            if (filterCAnonStorey2.c.filter.GetType().Equals(typeof (SerializedPropertyFilters.Name)))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: reference to a compiler-generated method
              source = source.Where<TreeViewItem>(new Func<TreeViewItem, bool>(new SerializedPropertyTreeView.\u003CFilter\u003Ec__AnonStorey1()
              {
                f = (SerializedPropertyFilters.Name) filterCAnonStorey2.c.filter
              }.\u003C\u003Em__0));
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              source = source.Where<TreeViewItem>(new Func<TreeViewItem, bool>(filterCAnonStorey2.\u003C\u003Em__0));
            }
          }
        }
      }
      return source;
    }

    private bool IsColumnVisible(int idx)
    {
      for (int index = 0; index < this.multiColumnHeader.state.visibleColumns.Length; ++index)
      {
        if (this.multiColumnHeader.state.visibleColumns[index] == idx)
          return true;
      }
      return false;
    }

    private SerializedPropertyTreeView.Column Col(int idx)
    {
      return (SerializedPropertyTreeView.Column) this.multiColumnHeader.state.columns[idx];
    }

    internal static class Styles
    {
      public static readonly GUIStyle entryEven = (GUIStyle) "OL EntryBackEven";
      public static readonly GUIStyle entryOdd = (GUIStyle) "OL EntryBackOdd";
      public static readonly string focusHelper = "SerializedPropertyTreeViewFocusHelper";
      public static readonly string serializeFilterSelection = "_FilterSelection";
      public static readonly string serializeFilterDisable = "_FilterDisable";
      public static readonly string serializeFilterInvert = "_FilterInvert";
      public static readonly string serializeTreeViewState = "_TreeViewState";
      public static readonly string serializeColumnHeaderState = "_ColumnHeaderState";
      public static readonly string serializeFilter = "_Filter_";
      public static readonly GUIContent filterSelection = EditorGUIUtility.TextContent("Lock Selection|Limits the table contents to the active selection.");
      public static readonly GUIContent filterDisable = EditorGUIUtility.TextContent("Disable All|Disables all filters.");
      public static readonly GUIContent filterInvert = EditorGUIUtility.TextContent("Invert Result|Inverts the filtered results.");
    }

    internal class SerializedPropertyItem : TreeViewItem
    {
      private SerializedPropertyDataStore.Data m_Data;

      public SerializedPropertyItem(int id, int depth, SerializedPropertyDataStore.Data ltd)
        : base(id, depth, ltd == null ? "root" : ltd.name)
      {
        this.m_Data = ltd;
      }

      public SerializedPropertyDataStore.Data GetData()
      {
        return this.m_Data;
      }
    }

    internal class Column : MultiColumnHeaderState.Column
    {
      public string propertyName;
      public int[] dependencyIndices;
      public SerializedPropertyTreeView.Column.DrawEntry drawDelegate;
      public SerializedPropertyTreeView.Column.CompareEntry compareDelegate;
      public SerializedPropertyTreeView.Column.CopyDelegate copyDelegate;
      public SerializedPropertyFilters.IFilter filter;

      public delegate void DrawEntry(Rect r, SerializedProperty prop, SerializedProperty[] dependencies);

      public delegate int CompareEntry(SerializedProperty lhs, SerializedProperty rhs);

      public delegate void CopyDelegate(SerializedProperty target, SerializedProperty source);
    }

    private struct ColumnInternal
    {
      public SerializedProperty[] dependencyProps;
    }

    internal class DefaultDelegates
    {
      public static readonly SerializedPropertyTreeView.Column.DrawEntry s_DrawDefault = (SerializedPropertyTreeView.Column.DrawEntry) ((r, prop, dependencies) =>
      {
        Profiler.BeginSample("PropDrawDefault");
        EditorGUI.PropertyField(r, prop, GUIContent.none);
        Profiler.EndSample();
      });
      public static readonly SerializedPropertyTreeView.Column.DrawEntry s_DrawCheckbox = (SerializedPropertyTreeView.Column.DrawEntry) ((r, prop, dependencies) =>
      {
        Profiler.BeginSample("PropDrawCheckbox");
        float num = (float) ((double) r.width / 2.0 - 8.0);
        r.x += (double) num < 0.0 ? 0.0f : num;
        EditorGUI.PropertyField(r, prop, GUIContent.none);
        Profiler.EndSample();
      });
      public static readonly SerializedPropertyTreeView.Column.DrawEntry s_DrawName = (SerializedPropertyTreeView.Column.DrawEntry) ((r, prop, dependencies) => {});
      public static readonly SerializedPropertyTreeView.Column.CompareEntry s_CompareFloat = (SerializedPropertyTreeView.Column.CompareEntry) ((lhs, rhs) => lhs.floatValue.CompareTo(rhs.floatValue));
      public static readonly SerializedPropertyTreeView.Column.CompareEntry s_CompareCheckbox = (SerializedPropertyTreeView.Column.CompareEntry) ((lhs, rhs) => lhs.boolValue.CompareTo(rhs.boolValue));
      public static readonly SerializedPropertyTreeView.Column.CompareEntry s_CompareEnum = (SerializedPropertyTreeView.Column.CompareEntry) ((lhs, rhs) => lhs.enumValueIndex.CompareTo(rhs.enumValueIndex));
      public static readonly SerializedPropertyTreeView.Column.CompareEntry s_CompareInt = (SerializedPropertyTreeView.Column.CompareEntry) ((lhs, rhs) => lhs.intValue.CompareTo(rhs.intValue));
      public static readonly SerializedPropertyTreeView.Column.CompareEntry s_CompareColor = (SerializedPropertyTreeView.Column.CompareEntry) ((lhs, rhs) =>
      {
        float H1;
        float S1;
        float V1;
        Color.RGBToHSV(lhs.colorValue, out H1, out S1, out V1);
        float H2;
        float S2;
        float V2;
        Color.RGBToHSV(rhs.colorValue, out H2, out S2, out V2);
        return H1.CompareTo(H2);
      });
      public static readonly SerializedPropertyTreeView.Column.CompareEntry s_CompareName = (SerializedPropertyTreeView.Column.CompareEntry) ((lhs, rhs) => 0);
      public static readonly SerializedPropertyTreeView.Column.CopyDelegate s_CopyDefault = (SerializedPropertyTreeView.Column.CopyDelegate) ((target, source) => target.serializedObject.CopyFromSerializedProperty(source));
    }
  }
}
