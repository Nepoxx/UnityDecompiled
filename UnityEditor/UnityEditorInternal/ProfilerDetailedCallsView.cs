// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProfilerDetailedCallsView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class ProfilerDetailedCallsView : ProfilerDetailedView
  {
    [NonSerialized]
    private bool m_Initialized = false;
    [NonSerialized]
    private GUIContent m_TotalSelectedPropertyTimeLabel = new GUIContent("", "Total time of all calls of the selected function in the frame.");
    [SerializeField]
    private SplitterState m_VertSplit = new SplitterState(new float[2]{ 40f, 60f }, new int[2]{ 50, 50 }, (int[]) null);
    [NonSerialized]
    private float m_TotalSelectedPropertyTime;
    [SerializeField]
    private ProfilerDetailedCallsView.CallsTreeViewController m_CallersTreeView;
    [SerializeField]
    private ProfilerDetailedCallsView.CallsTreeViewController m_CalleesTreeView;

    public ProfilerDetailedCallsView(ProfilerHierarchyGUI mainProfilerHierarchyGUI)
      : base(mainProfilerHierarchyGUI)
    {
    }

    private void InitIfNeeded()
    {
      if (this.m_Initialized)
        return;
      if (this.m_CallersTreeView == null)
        this.m_CallersTreeView = new ProfilerDetailedCallsView.CallsTreeViewController(ProfilerDetailedCallsView.CallsTreeView.Type.Callers);
      this.m_CallersTreeView.callSelected += new ProfilerDetailedCallsView.CallsTreeViewController.CallSelectedCallback(this.OnCallSelected);
      if (this.m_CalleesTreeView == null)
        this.m_CalleesTreeView = new ProfilerDetailedCallsView.CallsTreeViewController(ProfilerDetailedCallsView.CallsTreeView.Type.Callees);
      this.m_CalleesTreeView.callSelected += new ProfilerDetailedCallsView.CallsTreeViewController.CallSelectedCallback(this.OnCallSelected);
      this.m_Initialized = true;
    }

    public void DoGUI(GUIStyle headerStyle, int frameIndex, ProfilerViewType viewType)
    {
      string selectedPropertyPath = ProfilerDriver.selectedPropertyPath;
      if (string.IsNullOrEmpty(selectedPropertyPath))
      {
        this.DrawEmptyPane(headerStyle);
      }
      else
      {
        this.InitIfNeeded();
        this.UpdateIfNeeded(frameIndex, viewType, selectedPropertyPath);
        GUILayout.BeginVertical();
        GUILayout.Label(this.m_TotalSelectedPropertyTimeLabel, EditorStyles.label, new GUILayoutOption[0]);
        SplitterGUILayout.BeginVerticalSplit(this.m_VertSplit, new GUILayoutOption[2]
        {
          GUILayout.ExpandWidth(true),
          GUILayout.ExpandHeight(true)
        });
        this.m_CalleesTreeView.OnGUI(EditorGUILayout.BeginVertical());
        EditorGUILayout.EndVertical();
        this.m_CallersTreeView.OnGUI(EditorGUILayout.BeginVertical());
        EditorGUILayout.EndVertical();
        SplitterGUILayout.EndHorizontalSplit();
        GUILayout.EndVertical();
      }
    }

    private void UpdateIfNeeded(int frameIndex, ProfilerViewType viewType, string selectedPropertyPath)
    {
      if (this.m_CachedProfilerPropertyConfig.EqualsTo(frameIndex, viewType, ProfilerColumn.DontSort))
        return;
      ProfilerProperty rootProperty = this.m_MainProfilerHierarchyGUI.GetRootProperty();
      string profilerPropertyName = ProfilerDetailedCallsView.GetProfilerPropertyName(selectedPropertyPath);
      this.m_TotalSelectedPropertyTime = 0.0f;
      Dictionary<string, ProfilerDetailedCallsView.CallInformation> dictionary1 = new Dictionary<string, ProfilerDetailedCallsView.CallInformation>();
      Dictionary<string, ProfilerDetailedCallsView.CallInformation> dictionary2 = new Dictionary<string, ProfilerDetailedCallsView.CallInformation>();
      Stack<ProfilerDetailedCallsView.ParentCallInfo> parentCallInfoStack = new Stack<ProfilerDetailedCallsView.ParentCallInfo>();
      bool flag = false;
      while (rootProperty.Next(true))
      {
        string propertyName = rootProperty.propertyName;
        int depth = rootProperty.depth;
        if (parentCallInfoStack.Count + 1 != depth)
        {
          while (parentCallInfoStack.Count + 1 > depth)
            parentCallInfoStack.Pop();
          flag = parentCallInfoStack.Count != 0 && profilerPropertyName == parentCallInfoStack.Peek().name;
        }
        if (parentCallInfoStack.Count != 0)
        {
          ProfilerDetailedCallsView.ParentCallInfo parentCallInfo = parentCallInfoStack.Peek();
          ProfilerDetailedCallsView.CallInformation callInformation;
          if (profilerPropertyName == propertyName)
          {
            float columnAsSingle1 = rootProperty.GetColumnAsSingle(ProfilerColumn.TotalTime);
            int columnAsSingle2 = (int) rootProperty.GetColumnAsSingle(ProfilerColumn.Calls);
            int columnAsSingle3 = (int) rootProperty.GetColumnAsSingle(ProfilerColumn.GCMemory);
            if (!dictionary1.TryGetValue(parentCallInfo.name, out callInformation))
            {
              dictionary1.Add(parentCallInfo.name, new ProfilerDetailedCallsView.CallInformation()
              {
                name = parentCallInfo.name,
                path = parentCallInfo.path,
                callsCount = columnAsSingle2,
                gcAllocBytes = columnAsSingle3,
                totalCallTimeMs = (double) parentCallInfo.timeMs,
                totalSelfTimeMs = (double) columnAsSingle1
              });
            }
            else
            {
              callInformation.callsCount += columnAsSingle2;
              callInformation.gcAllocBytes += columnAsSingle3;
              callInformation.totalCallTimeMs += (double) parentCallInfo.timeMs;
              callInformation.totalSelfTimeMs += (double) columnAsSingle1;
            }
            this.m_TotalSelectedPropertyTime += columnAsSingle1;
          }
          if (flag)
          {
            float columnAsSingle1 = rootProperty.GetColumnAsSingle(ProfilerColumn.TotalTime);
            int columnAsSingle2 = (int) rootProperty.GetColumnAsSingle(ProfilerColumn.Calls);
            int columnAsSingle3 = (int) rootProperty.GetColumnAsSingle(ProfilerColumn.GCMemory);
            if (!dictionary2.TryGetValue(propertyName, out callInformation))
            {
              dictionary2.Add(propertyName, new ProfilerDetailedCallsView.CallInformation()
              {
                name = propertyName,
                path = rootProperty.propertyPath,
                callsCount = columnAsSingle2,
                gcAllocBytes = columnAsSingle3,
                totalCallTimeMs = (double) columnAsSingle1,
                totalSelfTimeMs = 0.0
              });
            }
            else
            {
              callInformation.callsCount += columnAsSingle2;
              callInformation.gcAllocBytes += columnAsSingle3;
              callInformation.totalCallTimeMs += (double) columnAsSingle1;
            }
          }
        }
        else if (profilerPropertyName == propertyName)
          this.m_TotalSelectedPropertyTime += rootProperty.GetColumnAsSingle(ProfilerColumn.TotalTime);
        if (rootProperty.HasChildren)
        {
          float columnAsSingle = rootProperty.GetColumnAsSingle(ProfilerColumn.TotalTime);
          parentCallInfoStack.Push(new ProfilerDetailedCallsView.ParentCallInfo()
          {
            name = propertyName,
            path = rootProperty.propertyPath,
            timeMs = columnAsSingle
          });
          flag = profilerPropertyName == propertyName;
        }
      }
      this.m_CallersTreeView.SetCallsData(new ProfilerDetailedCallsView.CallsData()
      {
        calls = dictionary1.Values.ToList<ProfilerDetailedCallsView.CallInformation>(),
        totalSelectedPropertyTime = this.m_TotalSelectedPropertyTime
      });
      this.m_CalleesTreeView.SetCallsData(new ProfilerDetailedCallsView.CallsData()
      {
        calls = dictionary2.Values.ToList<ProfilerDetailedCallsView.CallInformation>(),
        totalSelectedPropertyTime = this.m_TotalSelectedPropertyTime
      });
      this.m_TotalSelectedPropertyTimeLabel.text = profilerPropertyName + string.Format(" - Total time: {0:f2} ms", (object) this.m_TotalSelectedPropertyTime);
      this.m_CachedProfilerPropertyConfig.Set(frameIndex, viewType, ProfilerColumn.TotalTime);
    }

    private static string GetProfilerPropertyName(string propertyPath)
    {
      int num = propertyPath.LastIndexOf('/');
      return num != -1 ? propertyPath.Substring(num + 1) : propertyPath;
    }

    private void OnCallSelected(string path, Event evt)
    {
      EventType type = evt.type;
      switch (type)
      {
        case EventType.ValidateCommand:
        case EventType.ExecuteCommand:
          if (evt.commandName != "FrameSelected")
            break;
          if (type == EventType.ExecuteCommand)
            this.m_MainProfilerHierarchyGUI.SelectPath(path);
          evt.Use();
          break;
      }
    }

    private struct CallsData
    {
      public List<ProfilerDetailedCallsView.CallInformation> calls;
      public float totalSelectedPropertyTime;
    }

    private class CallInformation
    {
      public string name;
      public string path;
      public int callsCount;
      public int gcAllocBytes;
      public double totalCallTimeMs;
      public double totalSelfTimeMs;
      public double timePercent;
    }

    private class CallsTreeView : TreeView
    {
      private static string s_NoneText = LocalizationDatabase.GetLocalizedString("None");
      internal ProfilerDetailedCallsView.CallsData m_CallsData;
      private ProfilerDetailedCallsView.CallsTreeView.Type m_Type;

      public CallsTreeView(ProfilerDetailedCallsView.CallsTreeView.Type type, TreeViewState treeViewState, MultiColumnHeader multicolumnHeader)
        : base(treeViewState, multicolumnHeader)
      {
        this.m_Type = type;
        this.showBorder = true;
        this.showAlternatingRowBackgrounds = true;
        multicolumnHeader.sortingChanged += new MultiColumnHeader.HeaderCallback(this.OnSortingChanged);
        this.Reload();
      }

      public void SetCallsData(ProfilerDetailedCallsView.CallsData callsData)
      {
        this.m_CallsData = callsData;
        foreach (ProfilerDetailedCallsView.CallInformation call in this.m_CallsData.calls)
          call.timePercent = this.m_Type != ProfilerDetailedCallsView.CallsTreeView.Type.Callees ? call.totalSelfTimeMs / call.totalCallTimeMs : call.totalCallTimeMs / (double) this.m_CallsData.totalSelectedPropertyTime;
        this.OnSortingChanged(this.multiColumnHeader);
      }

      protected override TreeViewItem BuildRoot()
      {
        TreeViewItem root = new TreeViewItem() { id = 0, depth = -1, displayName = "Root" };
        List<TreeViewItem> treeViewItemList = new List<TreeViewItem>();
        if (this.m_CallsData.calls != null && this.m_CallsData.calls.Count != 0)
        {
          treeViewItemList.Capacity = this.m_CallsData.calls.Count;
          for (int index = 0; index < this.m_CallsData.calls.Count; ++index)
            treeViewItemList.Add(new TreeViewItem()
            {
              id = index + 1,
              depth = 0,
              displayName = this.m_CallsData.calls[index].name
            });
        }
        else
          treeViewItemList.Add(new TreeViewItem()
          {
            id = 1,
            depth = 0,
            displayName = ProfilerDetailedCallsView.CallsTreeView.s_NoneText
          });
        TreeView.SetupParentsAndChildrenFromDepths(root, (IList<TreeViewItem>) treeViewItemList);
        return root;
      }

      protected override void RowGUI(TreeView.RowGUIArgs args)
      {
        TreeViewItem treeViewItem = args.item;
        for (int visibleColumnIndex = 0; visibleColumnIndex < args.GetNumVisibleColumns(); ++visibleColumnIndex)
          this.CellGUI(args.GetCellRect(visibleColumnIndex), treeViewItem, (ProfilerDetailedCallsView.CallsTreeView.Column) args.GetColumn(visibleColumnIndex), ref args);
      }

      private void CellGUI(Rect cellRect, TreeViewItem item, ProfilerDetailedCallsView.CallsTreeView.Column column, ref TreeView.RowGUIArgs args)
      {
        if (this.m_CallsData.calls.Count == 0)
        {
          base.RowGUI(args);
        }
        else
        {
          ProfilerDetailedCallsView.CallInformation call = this.m_CallsData.calls[args.item.id - 1];
          this.CenterRectUsingSingleLineHeight(ref cellRect);
          switch (column)
          {
            case ProfilerDetailedCallsView.CallsTreeView.Column.Name:
              TreeView.DefaultGUI.Label(cellRect, call.name, args.selected, args.focused);
              break;
            case ProfilerDetailedCallsView.CallsTreeView.Column.Calls:
              string label = call.callsCount.ToString();
              TreeView.DefaultGUI.LabelRightAligned(cellRect, label, args.selected, args.focused);
              break;
            case ProfilerDetailedCallsView.CallsTreeView.Column.GcAlloc:
              int gcAllocBytes = call.gcAllocBytes;
              TreeView.DefaultGUI.LabelRightAligned(cellRect, gcAllocBytes.ToString(), args.selected, args.focused);
              break;
            case ProfilerDetailedCallsView.CallsTreeView.Column.TimeMs:
              double num = this.m_Type != ProfilerDetailedCallsView.CallsTreeView.Type.Callees ? call.totalSelfTimeMs : call.totalCallTimeMs;
              TreeView.DefaultGUI.LabelRightAligned(cellRect, num.ToString("f2"), args.selected, args.focused);
              break;
            case ProfilerDetailedCallsView.CallsTreeView.Column.TimePercent:
              TreeView.DefaultGUI.LabelRightAligned(cellRect, (call.timePercent * 100.0).ToString("f2"), args.selected, args.focused);
              break;
          }
        }
      }

      private void OnSortingChanged(MultiColumnHeader header)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ProfilerDetailedCallsView.CallsTreeView.\u003COnSortingChanged\u003Ec__AnonStorey0 changedCAnonStorey0 = new ProfilerDetailedCallsView.CallsTreeView.\u003COnSortingChanged\u003Ec__AnonStorey0();
        if (header.sortedColumnIndex == -1)
          return;
        // ISSUE: reference to a compiler-generated field
        changedCAnonStorey0.orderMultiplier = !header.IsSortedAscending(header.sortedColumnIndex) ? -1 : 1;
        Comparison<ProfilerDetailedCallsView.CallInformation> comparison;
        switch (header.sortedColumnIndex)
        {
          case 0:
            // ISSUE: reference to a compiler-generated method
            comparison = new Comparison<ProfilerDetailedCallsView.CallInformation>(changedCAnonStorey0.\u003C\u003Em__0);
            break;
          case 1:
            // ISSUE: reference to a compiler-generated method
            comparison = new Comparison<ProfilerDetailedCallsView.CallInformation>(changedCAnonStorey0.\u003C\u003Em__1);
            break;
          case 2:
            // ISSUE: reference to a compiler-generated method
            comparison = new Comparison<ProfilerDetailedCallsView.CallInformation>(changedCAnonStorey0.\u003C\u003Em__2);
            break;
          case 3:
            // ISSUE: reference to a compiler-generated method
            comparison = new Comparison<ProfilerDetailedCallsView.CallInformation>(changedCAnonStorey0.\u003C\u003Em__3);
            break;
          case 4:
            // ISSUE: reference to a compiler-generated method
            comparison = new Comparison<ProfilerDetailedCallsView.CallInformation>(changedCAnonStorey0.\u003C\u003Em__4);
            break;
          case 5:
            // ISSUE: reference to a compiler-generated method
            comparison = new Comparison<ProfilerDetailedCallsView.CallInformation>(changedCAnonStorey0.\u003C\u003Em__5);
            break;
          default:
            return;
        }
        this.m_CallsData.calls.Sort(comparison);
        this.Reload();
      }

      public enum Type
      {
        Callers,
        Callees,
      }

      public enum Column
      {
        Name,
        Calls,
        GcAlloc,
        TimeMs,
        TimePercent,
        Count,
      }
    }

    [Serializable]
    private class CallsTreeViewController
    {
      [NonSerialized]
      private ProfilerDetailedCallsView.CallsTreeView m_View;
      [NonSerialized]
      private bool m_Initialized;
      [SerializeField]
      private TreeViewState m_ViewState;
      [SerializeField]
      private MultiColumnHeaderState m_ViewHeaderState;
      [SerializeField]
      private ProfilerDetailedCallsView.CallsTreeView.Type m_Type;

      public CallsTreeViewController(ProfilerDetailedCallsView.CallsTreeView.Type type)
      {
        this.m_Type = type;
      }

      public event ProfilerDetailedCallsView.CallsTreeViewController.CallSelectedCallback callSelected;

      private void InitIfNeeded()
      {
        if (this.m_Initialized)
          return;
        if (this.m_ViewState == null)
          this.m_ViewState = new TreeViewState();
        bool flag = this.m_ViewHeaderState == null;
        MultiColumnHeaderState columnHeaderState = this.CreateDefaultMultiColumnHeaderState();
        if (MultiColumnHeaderState.CanOverwriteSerializedFields(this.m_ViewHeaderState, columnHeaderState))
          MultiColumnHeaderState.OverwriteSerializedFields(this.m_ViewHeaderState, columnHeaderState);
        this.m_ViewHeaderState = columnHeaderState;
        MultiColumnHeader multicolumnHeader = new MultiColumnHeader(this.m_ViewHeaderState) { height = 25f };
        if (flag)
        {
          multicolumnHeader.state.visibleColumns = new int[4]
          {
            0,
            1,
            3,
            4
          };
          multicolumnHeader.ResizeToFit();
        }
        this.m_View = new ProfilerDetailedCallsView.CallsTreeView(this.m_Type, this.m_ViewState, multicolumnHeader);
        this.m_Initialized = true;
      }

      private MultiColumnHeaderState CreateDefaultMultiColumnHeaderState()
      {
        return new MultiColumnHeaderState(new MultiColumnHeaderState.Column[5]{ new MultiColumnHeaderState.Column() { headerContent = this.m_Type != ProfilerDetailedCallsView.CallsTreeView.Type.Callers ? ProfilerDetailedCallsView.CallsTreeViewController.Styles.calleesLabel : ProfilerDetailedCallsView.CallsTreeViewController.Styles.callersLabel, headerTextAlignment = TextAlignment.Left, sortedAscending = true, sortingArrowAlignment = TextAlignment.Center, width = 150f, minWidth = 150f, autoResize = true, allowToggleVisibility = false }, new MultiColumnHeaderState.Column() { headerContent = ProfilerDetailedCallsView.CallsTreeViewController.Styles.callsLabel, headerTextAlignment = TextAlignment.Right, sortedAscending = false, sortingArrowAlignment = TextAlignment.Center, width = 60f, minWidth = 60f, autoResize = false, allowToggleVisibility = true }, new MultiColumnHeaderState.Column() { headerContent = ProfilerDetailedCallsView.CallsTreeViewController.Styles.gcAllocLabel, headerTextAlignment = TextAlignment.Right, sortedAscending = false, sortingArrowAlignment = TextAlignment.Center, width = 60f, minWidth = 60f, autoResize = false, allowToggleVisibility = true }, new MultiColumnHeaderState.Column() { headerContent = this.m_Type != ProfilerDetailedCallsView.CallsTreeView.Type.Callers ? ProfilerDetailedCallsView.CallsTreeViewController.Styles.timeMsCalleesLabel : ProfilerDetailedCallsView.CallsTreeViewController.Styles.timeMsCallersLabel, headerTextAlignment = TextAlignment.Right, sortedAscending = false, sortingArrowAlignment = TextAlignment.Center, width = 60f, minWidth = 60f, autoResize = false, allowToggleVisibility = true }, new MultiColumnHeaderState.Column() { headerContent = this.m_Type != ProfilerDetailedCallsView.CallsTreeView.Type.Callers ? ProfilerDetailedCallsView.CallsTreeViewController.Styles.timePctCalleesLabel : ProfilerDetailedCallsView.CallsTreeViewController.Styles.timePctCallersLabel, headerTextAlignment = TextAlignment.Right, sortedAscending = false, sortingArrowAlignment = TextAlignment.Center, width = 60f, minWidth = 60f, autoResize = false, allowToggleVisibility = true } }) { sortedColumnIndex = 3 };
      }

      public void SetCallsData(ProfilerDetailedCallsView.CallsData callsData)
      {
        this.InitIfNeeded();
        this.m_View.SetCallsData(callsData);
      }

      public void OnGUI(Rect r)
      {
        this.InitIfNeeded();
        this.m_View.OnGUI(r);
        this.HandleCommandEvents();
      }

      private void HandleCommandEvents()
      {
        if (GUIUtility.keyboardControl != this.m_View.treeViewControlID || this.m_ViewState.selectedIDs.Count == 0)
          return;
        int index = this.m_ViewState.selectedIDs.First<int>() - 1;
        if (index >= this.m_View.m_CallsData.calls.Count)
          return;
        ProfilerDetailedCallsView.CallInformation call = this.m_View.m_CallsData.calls[index];
        // ISSUE: reference to a compiler-generated field
        if (this.callSelected == null)
          return;
        Event current = Event.current;
        // ISSUE: reference to a compiler-generated field
        this.callSelected(call.path, current);
      }

      private static class Styles
      {
        public static GUIContent callersLabel = new GUIContent("Called From", "Parents the selected function is called from\n\n(Press 'F' for frame selection)");
        public static GUIContent calleesLabel = new GUIContent("Calls To", "Functions which are called from the selected function\n\n(Press 'F' for frame selection)");
        public static GUIContent callsLabel = new GUIContent("Calls", "Total number of calls in a selected frame");
        public static GUIContent gcAllocLabel = new GUIContent("GC Alloc");
        public static GUIContent timeMsCallersLabel = new GUIContent("Time ms", "Total time the selected function spend within a parent");
        public static GUIContent timeMsCalleesLabel = new GUIContent("Time ms", "Total time the child call spend within selected function");
        public static GUIContent timePctCallersLabel = new GUIContent("Time %", "Shows how often the selected function was called from the parent call");
        public static GUIContent timePctCalleesLabel = new GUIContent("Time %", "Shows how often child call was called from the selected function");
      }

      public delegate void CallSelectedCallback(string path, Event evt);
    }

    private struct ParentCallInfo
    {
      public string name;
      public string path;
      public float timeMs;
    }
  }
}
