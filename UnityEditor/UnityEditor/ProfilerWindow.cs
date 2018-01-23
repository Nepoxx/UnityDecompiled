// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProfilerWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Accessibility;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Profiler", useTypeNameAsIconName = true)]
  internal class ProfilerWindow : EditorWindow, IProfilerWindowController, IHasCustomMenu
  {
    private static readonly ProfilerArea[] ms_StackedAreas = new ProfilerArea[4]{ ProfilerArea.CPU, ProfilerArea.GPU, ProfilerArea.UI, ProfilerArea.GlobalIllumination };
    private static List<ProfilerWindow> m_ProfilerWindows = new List<ProfilerWindow>();
    private static readonly int s_HashControlID = "ProfilerSearchField".GetHashCode();
    private bool m_FocusSearchField = false;
    private string m_SearchString = "";
    private SplitterState m_VertSplit = new SplitterState(new float[2]{ 50f, 50f }, new int[2]{ 50, 50 }, (int[]) null);
    private SplitterState m_ViewSplit = new SplitterState(new float[2]{ 70f, 30f }, new int[2]{ 450, 50 }, (int[]) null);
    private SplitterState m_NetworkSplit = new SplitterState(new float[2]{ 20f, 80f }, new int[2]{ 100, 100 }, (int[]) null);
    private AttachProfilerUI m_AttachProfilerUI = new AttachProfilerUI();
    private Vector2 m_GraphPos = Vector2.zero;
    private Vector2[] m_PaneScroll = new Vector2[13];
    private Vector2 m_PaneScroll_AudioChannels = Vector2.zero;
    private Vector2 m_PaneScroll_AudioDSP = Vector2.zero;
    private Vector2 m_PaneScroll_AudioClips = Vector2.zero;
    [SerializeField]
    private ProfilerViewType m_ViewType = ProfilerViewType.Hierarchy;
    [SerializeField]
    private ProfilerArea m_CurrentArea = ProfilerArea.CPU;
    private ProfilerMemoryView m_ShowDetailedMemoryPane = ProfilerMemoryView.Simple;
    private ProfilerAudioView m_ShowDetailedAudioPane = ProfilerAudioView.Stats;
    [SerializeField]
    private bool m_ShowInactiveDSPChains = false;
    [SerializeField]
    private bool m_HighlightAudibleDSPChains = true;
    [SerializeField]
    private float m_DSPGraphZoomFactor = 1f;
    private int m_CurrentFrame = -1;
    private int m_LastFrameFromTick = -1;
    private int m_PrevLastFrame = -1;
    private int m_LastAudioProfilerFrame = -1;
    private readonly string[] k_CPUProfilerViewTypeNames = new string[3]{ "Hierarchy", "Timeline", "Raw Hierarchy" };
    private readonly int[] k_CPUProfilerViewTypes = new int[3]{ 0, 1, 2 };
    private readonly string[] k_GPUProfilerViewTypeNames = new string[2]{ "Hierarchy", "Raw Hierarchy" };
    private readonly int[] k_GPUProfilerViewTypes = new int[2]{ 0, 2 };
    private readonly int[] k_HierarchyViewDetailPaneTypes = new int[3]{ 0, 1, 2 };
    private float[] m_ChartOldMax = new float[14]{ -1f, -1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1f, 0.0f, 0.0f, 0.0f };
    private float m_ChartMaxClamp = 70000f;
    [SerializeField]
    private ProfilerHierarchyGUI m_CPUHierarchyGUI = new ProfilerHierarchyGUI(ProfilerColumn.TotalTime);
    [SerializeField]
    private ProfilerHierarchyGUI m_GPUHierarchyGUI = new ProfilerHierarchyGUI(ProfilerColumn.TotalGPUTime);
    private bool m_GatherObjectReferences = true;
    private AudioProfilerGroupView m_AudioProfilerGroupView = (AudioProfilerGroupView) null;
    private AudioProfilerClipView m_AudioProfilerClipView = (AudioProfilerClipView) null;
    private ProfilerMemoryRecordMode m_SelectedMemRecordMode = ProfilerMemoryRecordMode.None;
    private readonly char s_CheckMark = '✔';
    [SerializeField]
    private ProfilerWindow.HierarchyViewDetailPaneType m_HierarchyViewDetailPaneType = ProfilerWindow.HierarchyViewDetailPaneType.None;
    private string[] msgNames = new string[16]{ "UserMessage", "ObjectDestroy", "ClientRpc", "ObjectSpawn", "Owner", "Command", "LocalPlayerTransform", "SyncEvent", "SyncVars", "SyncList", "ObjectSpawnScene", "NetworkInfo", "SpawnFinished", "ObjectHide", "CRC", "ClientAuthority" };
    private bool[] msgFoldouts = new bool[15]{ true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
    [SerializeField]
    private bool m_Recording;
    [SerializeField]
    private string m_ActiveNativePlatformSupportModule;
    private ProfilerChart[] m_Charts;
    private const float kRowHeight = 16f;
    private const float kIndentPx = 16f;
    private const float kBaseIndent = 8f;
    private const float kSmallMargin = 4f;
    private const float kNameColumnSize = 350f;
    private const float kColumnSize = 80f;
    private const float kFoldoutSize = 14f;
    private const int kFirst = -999999;
    private const int kLast = 999999;
    private ProfilerTimelineGUI m_CPUTimelineGUI;
    private ProfilerWindow.CachedProfilerPropertyConfig m_CPUOrGPUProfilerPropertyConfig;
    private ProfilerProperty m_CPUOrGPUProfilerProperty;
    [SerializeField]
    private UISystemProfiler m_UISystemProfiler;
    private MemoryTreeList m_ReferenceListView;
    private MemoryTreeListClickable m_MemoryListView;
    [SerializeField]
    private AudioProfilerGroupTreeViewState m_AudioProfilerGroupTreeViewState;
    private AudioProfilerGroupViewBackend m_AudioProfilerGroupViewBackend;
    [SerializeField]
    private AudioProfilerClipTreeViewState m_AudioProfilerClipTreeViewState;
    private AudioProfilerClipViewBackend m_AudioProfilerClipViewBackend;
    private AudioProfilerDSPView m_AudioProfilerDSPView;
    private const string kProfilerColumnSettings = "VisibleProfilerColumnsV2";
    private const string kProfilerDetailColumnSettings = "VisibleProfilerDetailColumns";
    private const string kProfilerGPUColumnSettings = "VisibleProfilerGPUColumns";
    private const string kProfilerGPUDetailColumnSettings = "VisibleProfilerGPUDetailColumns";
    private const string kProfilerVisibleGraphsSettings = "VisibleProfilerGraphs";
    private const string kProfilerRecentSaveLoadProfilePath = "ProfilerRecentSaveLoadProfilePath";
    private const string kProfilerEnabledSessionKey = "ProfilerEnabled";
    private const string kSearchControlName = "ProfilerSearchField";

    private bool wantsMemoryRefresh
    {
      get
      {
        return this.m_MemoryListView.RequiresRefresh;
      }
    }

    private void BuildColumns()
    {
      ProfilerColumn[] profilerColumnArray1 = new ProfilerColumn[8]{ ProfilerColumn.FunctionName, ProfilerColumn.TotalPercent, ProfilerColumn.SelfPercent, ProfilerColumn.Calls, ProfilerColumn.GCMemory, ProfilerColumn.TotalTime, ProfilerColumn.SelfTime, ProfilerColumn.WarningCount };
      ProfilerColumn[] profilerColumnArray2 = new ProfilerColumn[7]{ ProfilerColumn.ObjectName, ProfilerColumn.TotalPercent, ProfilerColumn.SelfPercent, ProfilerColumn.Calls, ProfilerColumn.GCMemory, ProfilerColumn.TotalTime, ProfilerColumn.SelfTime };
      string text = EditorGUIUtility.TextContent("Object").text;
      string[] columnNames1 = ProfilerWindow.ProfilerColumnNames(profilerColumnArray2);
      columnNames1[0] = text;
      this.m_CPUHierarchyGUI.Setup((IProfilerWindowController) this, new ProfilerHierarchyGUI((IProfilerWindowController) this, (ProfilerHierarchyGUI) null, "VisibleProfilerDetailColumns", profilerColumnArray2, columnNames1, true, ProfilerColumn.TotalTime), "VisibleProfilerColumnsV2", profilerColumnArray1, ProfilerWindow.ProfilerColumnNames(profilerColumnArray1), false);
      this.m_CPUTimelineGUI = new ProfilerTimelineGUI((IProfilerWindowController) this);
      ProfilerColumn[] profilerColumnArray3 = new ProfilerColumn[4]{ ProfilerColumn.FunctionName, ProfilerColumn.TotalGPUPercent, ProfilerColumn.DrawCalls, ProfilerColumn.TotalGPUTime };
      ProfilerColumn[] profilerColumnArray4 = new ProfilerColumn[4]{ ProfilerColumn.ObjectName, ProfilerColumn.TotalGPUPercent, ProfilerColumn.DrawCalls, ProfilerColumn.TotalGPUTime };
      string[] columnNames2 = ProfilerWindow.ProfilerColumnNames(profilerColumnArray4);
      columnNames2[0] = text;
      this.m_GPUHierarchyGUI.Setup((IProfilerWindowController) this, new ProfilerHierarchyGUI((IProfilerWindowController) this, (ProfilerHierarchyGUI) null, "VisibleProfilerGPUDetailColumns", profilerColumnArray4, columnNames2, true, ProfilerColumn.TotalGPUTime), "VisibleProfilerGPUColumns", profilerColumnArray3, ProfilerWindow.ProfilerColumnNames(profilerColumnArray3), false);
      this.m_UISystemProfiler = new UISystemProfiler();
    }

    private static string[] ProfilerColumnNames(ProfilerColumn[] columns)
    {
      string[] names = Enum.GetNames(typeof (ProfilerColumn));
      string[] strArray = new string[columns.Length];
      for (int index = 0; index < columns.Length; ++index)
      {
        switch (columns[index])
        {
          case ProfilerColumn.FunctionName:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Overview");
            break;
          case ProfilerColumn.TotalPercent:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Total");
            break;
          case ProfilerColumn.SelfPercent:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Self");
            break;
          case ProfilerColumn.Calls:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Calls");
            break;
          case ProfilerColumn.GCMemory:
            strArray[index] = LocalizationDatabase.GetLocalizedString("GC Alloc");
            break;
          case ProfilerColumn.TotalTime:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Time ms");
            break;
          case ProfilerColumn.SelfTime:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Self ms");
            break;
          case ProfilerColumn.DrawCalls:
            strArray[index] = LocalizationDatabase.GetLocalizedString("DrawCalls");
            break;
          case ProfilerColumn.TotalGPUTime:
            strArray[index] = LocalizationDatabase.GetLocalizedString("GPU ms");
            break;
          case ProfilerColumn.SelfGPUTime:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Self ms");
            break;
          case ProfilerColumn.TotalGPUPercent:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Total");
            break;
          case ProfilerColumn.SelfGPUPercent:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Self");
            break;
          case ProfilerColumn.WarningCount:
            strArray[index] = LocalizationDatabase.GetLocalizedString("|Warnings");
            break;
          case ProfilerColumn.ObjectName:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Name");
            break;
          default:
            strArray[index] = "ProfilerColumn." + names[(int) columns[index]];
            break;
        }
      }
      return strArray;
    }

    public void SetSelectedPropertyPath(string path)
    {
      if (!(ProfilerDriver.selectedPropertyPath != path))
        return;
      ProfilerDriver.selectedPropertyPath = path;
      this.UpdateCharts();
    }

    public void ClearSelectedPropertyPath()
    {
      if (!(ProfilerDriver.selectedPropertyPath != string.Empty))
        return;
      this.m_CPUHierarchyGUI.selectedIndex = -1;
      ProfilerDriver.selectedPropertyPath = string.Empty;
      this.UpdateCharts();
    }

    public ProfilerProperty CreateProperty()
    {
      return this.CreateProperty(this.m_CurrentArea != ProfilerArea.CPU ? this.m_GPUHierarchyGUI.sortType : this.m_CPUHierarchyGUI.sortType);
    }

    public ProfilerProperty CreateProperty(ProfilerColumn sortType)
    {
      ProfilerProperty profilerProperty = new ProfilerProperty();
      profilerProperty.SetRoot(this.GetActiveVisibleFrameIndex(), sortType, this.m_ViewType);
      profilerProperty.onlyShowGPUSamples = this.m_CurrentArea == ProfilerArea.GPU;
      return profilerProperty;
    }

    public int GetActiveVisibleFrameIndex()
    {
      return this.m_CurrentFrame != -1 ? this.m_CurrentFrame : this.m_LastFrameFromTick;
    }

    public void SetSearch(string searchString)
    {
      this.m_SearchString = !string.IsNullOrEmpty(searchString) ? searchString : string.Empty;
    }

    public string GetSearch()
    {
      return this.m_SearchString;
    }

    public bool IsSearching()
    {
      return !string.IsNullOrEmpty(this.m_SearchString) && this.m_SearchString.Length > 0;
    }

    private void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
      ProfilerWindow.m_ProfilerWindows.Add(this);
      UserAccessiblitySettings.colorBlindConditionChanged += new Action(this.Initialize);
      this.Initialize();
    }

    private void Initialize()
    {
      int numDataPoints = ProfilerDriver.maxHistoryLength - 1;
      this.m_Charts = new ProfilerChart[13];
      Color[] currentColors = ProfilerColors.currentColors;
      for (ProfilerArea profilerArea = ProfilerArea.CPU; profilerArea < ProfilerArea.AreaCount; ++profilerArea)
      {
        float scale = 1f;
        Chart.ChartType chartType = Chart.ChartType.Line;
        string[] propertiesForArea = ProfilerDriver.GetGraphStatisticsPropertiesForArea(profilerArea);
        int length = propertiesForArea.Length;
        if (Array.IndexOf<ProfilerArea>(ProfilerWindow.ms_StackedAreas, profilerArea) != -1)
        {
          chartType = Chart.ChartType.StackedFill;
          scale = 1f / 1000f;
        }
        ProfilerChart profilerChart = this.CreateProfilerChart(profilerArea, chartType, scale, length);
        for (int index1 = 0; index1 < length; ++index1)
        {
          profilerChart.m_Series[index1] = new ChartSeriesViewData(propertiesForArea[index1], numDataPoints, currentColors[index1 % currentColors.Length]);
          for (int index2 = 0; index2 < numDataPoints; ++index2)
            profilerChart.m_Series[index1].xValues[index2] = (float) index2;
        }
        this.m_Charts[(int) profilerArea] = profilerChart;
      }
      if (this.m_ReferenceListView == null)
        this.m_ReferenceListView = new MemoryTreeList((EditorWindow) this, (MemoryTreeList) null);
      if (this.m_MemoryListView == null)
        this.m_MemoryListView = new MemoryTreeListClickable((EditorWindow) this, this.m_ReferenceListView);
      this.UpdateCharts();
      this.BuildColumns();
      foreach (ProfilerChart chart in this.m_Charts)
        chart.LoadAndBindSettings();
    }

    private ProfilerChart CreateProfilerChart(ProfilerArea i, Chart.ChartType chartType, float scale, int length)
    {
      ProfilerChart profilerChart = i != ProfilerArea.UIDetails ? new ProfilerChart(i, chartType, scale, length) : (ProfilerChart) new UISystemProfilerChart(chartType, scale, length);
      profilerChart.selected += new Chart.ChangedEventHandler(this.OnChartSelected);
      profilerChart.closed += new Chart.ChangedEventHandler(this.OnChartClosed);
      return profilerChart;
    }

    private void OnChartClosed(Chart sender)
    {
      if (this.m_Charts[(int) this.m_CurrentArea] == sender)
      {
        this.m_CurrentArea = ProfilerArea.AreaCount;
        this.m_UISystemProfiler.CurrentAreaChanged(this.m_CurrentArea);
      }
      ((ProfilerChart) sender).active = false;
    }

    private void OnChartSelected(Chart sender)
    {
      ProfilerArea profilerArea = this.m_CurrentArea;
      int index = 0;
      for (int length = this.m_Charts.Length; index < length; ++index)
      {
        if (this.m_Charts[index] == sender)
        {
          profilerArea = (ProfilerArea) index;
          break;
        }
      }
      if (this.m_CurrentArea == profilerArea)
        return;
      this.m_CurrentArea = profilerArea;
      if (this.m_CurrentArea != ProfilerArea.CPU && this.m_CPUHierarchyGUI.selectedIndex != -1)
        this.ClearSelectedPropertyPath();
      this.m_UISystemProfiler.CurrentAreaChanged(this.m_CurrentArea);
      this.Repaint();
      GUIUtility.ExitGUI();
    }

    private void CheckForPlatformModuleChange()
    {
      if (!(this.m_ActiveNativePlatformSupportModule != EditorUtility.GetActiveNativePlatformSupportModuleName()))
        return;
      ProfilerDriver.ResetHistory();
      this.Initialize();
      this.m_ActiveNativePlatformSupportModule = EditorUtility.GetActiveNativePlatformSupportModuleName();
    }

    private void OnDisable()
    {
      ProfilerWindow.m_ProfilerWindows.Remove(this);
      this.m_UISystemProfiler.CurrentAreaChanged(ProfilerArea.AreaCount);
      UserAccessiblitySettings.colorBlindConditionChanged -= new Action(this.Initialize);
    }

    private void Awake()
    {
      if (!Profiler.supported)
        return;
      this.m_Recording = SessionState.GetBool("ProfilerEnabled", true);
      ProfilerDriver.enabled = this.m_Recording;
      this.m_SelectedMemRecordMode = ProfilerDriver.memoryRecordMode;
    }

    private void OnDestroy()
    {
      if (!Profiler.supported || EditorApplication.isPlayingOrWillChangePlaymode)
        return;
      ProfilerDriver.enabled = false;
    }

    private void OnFocus()
    {
      if (!Profiler.supported)
        return;
      ProfilerDriver.enabled = this.m_Recording;
    }

    private void OnLostFocus()
    {
      if (GUIUtility.hotControl == 0)
        return;
      for (int index = 0; index < this.m_Charts.Length; ++index)
        this.m_Charts[index].OnLostFocus();
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      menu.AddItem(ProfilerWindow.Styles.accessibilityModeLabel, UserAccessiblitySettings.colorBlindCondition != ColorBlindCondition.Default, new GenericMenu.MenuFunction(this.OnToggleColorBlindMode));
    }

    private void OnToggleColorBlindMode()
    {
      UserAccessiblitySettings.colorBlindCondition = UserAccessiblitySettings.colorBlindCondition != ColorBlindCondition.Default ? ColorBlindCondition.Default : ColorBlindCondition.Deuteranopia;
    }

    private static void ShowProfilerWindow()
    {
      EditorWindow.GetWindow<ProfilerWindow>(false);
    }

    [RequiredByNativeCode]
    private static void RepaintAllProfilerWindows()
    {
      foreach (ProfilerWindow profilerWindow in ProfilerWindow.m_ProfilerWindows)
      {
        if (ProfilerDriver.lastFrameIndex != profilerWindow.m_LastFrameFromTick)
        {
          profilerWindow.m_LastFrameFromTick = ProfilerDriver.lastFrameIndex;
          profilerWindow.RepaintImmediately();
        }
      }
    }

    private static void SetMemoryProfilerInfo(ObjectMemoryInfo[] memoryInfo, int[] referencedIndices)
    {
      foreach (ProfilerWindow profilerWindow in ProfilerWindow.m_ProfilerWindows)
      {
        if (profilerWindow.wantsMemoryRefresh)
          profilerWindow.m_MemoryListView.SetRoot(MemoryElementDataManager.GetTreeRoot(memoryInfo, referencedIndices));
      }
    }

    private static void SetProfileDeepScripts(bool deep)
    {
      if (ProfilerDriver.deepProfiling == deep)
        return;
      bool flag = true;
      if (EditorApplication.isPlaying)
        flag = !deep ? EditorUtility.DisplayDialog("Disable deep script profiling", "Disabling deep profiling requires reloading all scripts", "Reload", "Cancel") : EditorUtility.DisplayDialog("Enable deep script profiling", "Enabling deep profiling requires reloading scripts.", "Reload", "Cancel");
      if (!flag)
        return;
      ProfilerDriver.deepProfiling = deep;
      InternalEditorUtility.RequestScriptReload();
    }

    private string PickFrameLabel()
    {
      if (this.m_CurrentFrame == -1)
        return "Current";
      return (this.m_CurrentFrame + 1).ToString() + " / " + (object) (ProfilerDriver.lastFrameIndex + 1);
    }

    private void PrevFrame()
    {
      int previousFrameIndex = ProfilerDriver.GetPreviousFrameIndex(this.m_CurrentFrame);
      if (previousFrameIndex == -1)
        return;
      this.SetCurrentFrame(previousFrameIndex);
    }

    private void NextFrame()
    {
      int nextFrameIndex = ProfilerDriver.GetNextFrameIndex(this.m_CurrentFrame);
      if (nextFrameIndex == -1)
        return;
      this.SetCurrentFrame(nextFrameIndex);
    }

    private void DrawCPUOrGPUHierarchyViewToolbar(ProfilerProperty property, bool showDetailedView, bool hasTimeline)
    {
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      this.DrawCPUorGPUCommonToolbar(property, hasTimeline);
      GUILayout.FlexibleSpace();
      this.SearchFieldGUI();
      if (!showDetailedView)
        this.m_HierarchyViewDetailPaneType = this.DrawCPUOrGPUDetailedViewPopup();
      EditorGUILayout.EndHorizontal();
      this.HandleCommandEvents();
    }

    private void DrawCPUTimelineViewToolbar(ProfilerProperty property)
    {
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      this.DrawCPUorGPUCommonToolbar(property, true);
      GUILayout.FlexibleSpace();
      ProfilerWindow.Styles.memRecord.text = "Allocation Callstacks";
      if (this.m_SelectedMemRecordMode != ProfilerMemoryRecordMode.None)
      {
        GUIContent memRecord = ProfilerWindow.Styles.memRecord;
        memRecord.text = memRecord.text + " [" + (object) this.s_CheckMark + "]";
      }
      Rect rect = GUILayoutUtility.GetRect(ProfilerWindow.Styles.memRecord, EditorStyles.toolbarDropDown, new GUILayoutOption[1]{ GUILayout.Width(170f) });
      if (EditorGUI.DropdownButton(rect, ProfilerWindow.Styles.memRecord, FocusType.Passive, EditorStyles.toolbarDropDown))
      {
        string[] options = new string[2]{ "None", "Managed Allocations" };
        if (Unsupported.IsDeveloperBuild())
          options = new string[4]
          {
            "None",
            "Managed Allocations",
            "All Allocations (fast)",
            "All Allocations (full)"
          };
        bool[] enabled = new bool[options.Length];
        for (int index = 0; index < options.Length; ++index)
          enabled[index] = true;
        int[] selected = new int[1]{ (int) this.m_SelectedMemRecordMode };
        EditorUtility.DisplayCustomMenu(rect, options, enabled, selected, new EditorUtility.SelectMenuItemFunction(this.MemRecordModeClick), (object) null);
      }
      EditorGUILayout.EndHorizontal();
      this.HandleCommandEvents();
    }

    private void DrawCPUorGPUCommonToolbar(ProfilerProperty property, bool hasTimeline)
    {
      if (hasTimeline)
      {
        this.m_ViewType = (ProfilerViewType) EditorGUILayout.IntPopup((int) this.m_ViewType, this.k_CPUProfilerViewTypeNames, this.k_CPUProfilerViewTypes, EditorStyles.toolbarDropDown, new GUILayoutOption[1]
        {
          GUILayout.Width(120f)
        });
      }
      else
      {
        if (this.m_ViewType == ProfilerViewType.Timeline)
          this.m_ViewType = ProfilerViewType.Hierarchy;
        this.m_ViewType = (ProfilerViewType) EditorGUILayout.IntPopup((int) this.m_ViewType, this.k_GPUProfilerViewTypeNames, this.k_GPUProfilerViewTypes, EditorStyles.toolbarDropDown, new GUILayoutOption[1]
        {
          GUILayout.Width(120f)
        });
      }
      GUILayout.FlexibleSpace();
      GUILayout.Label(string.Format("CPU:{0}ms   GPU:{1}ms", (object) property.frameTime, (object) property.frameGpuTime), EditorStyles.miniLabel, new GUILayoutOption[0]);
      if (!hasTimeline)
        return;
      GUI.enabled = true;
      if (ProfilerInstrumentationPopup.InstrumentationEnabled && GUILayout.Button(ProfilerWindow.Styles.profilerInstrumentation, EditorStyles.toolbarDropDown, new GUILayoutOption[0]))
        ProfilerInstrumentationPopup.Show(GUILayoutUtility.topLevel.GetLast());
    }

    private void HandleCommandEvents()
    {
      Event current = Event.current;
      EventType type = current.type;
      switch (type)
      {
        case EventType.ValidateCommand:
        case EventType.ExecuteCommand:
          bool flag = type == EventType.ExecuteCommand;
          if (Event.current.commandName == "Find")
          {
            if (flag)
              this.m_FocusSearchField = true;
            current.Use();
          }
          break;
      }
    }

    internal void SearchFieldGUI()
    {
      Event current = Event.current;
      Rect rect = GUILayoutUtility.GetRect(50f, 300f, 16f, 16f, EditorStyles.toolbarSearchField);
      GUI.SetNextControlName("ProfilerSearchField");
      if (this.m_FocusSearchField)
      {
        EditorGUI.FocusTextInControl("ProfilerSearchField");
        if (Event.current.type == EventType.Repaint)
          this.m_FocusSearchField = false;
      }
      if (current.type == EventType.KeyDown && current.keyCode == KeyCode.Escape && GUI.GetNameOfFocusedControl() == "ProfilerSearchField")
        this.m_SearchString = "";
      if (current.type == EventType.KeyDown && (current.keyCode == KeyCode.DownArrow || current.keyCode == KeyCode.UpArrow) && GUI.GetNameOfFocusedControl() == "ProfilerSearchField")
      {
        this.m_CPUHierarchyGUI.SelectFirstRow();
        this.m_CPUHierarchyGUI.SetKeyboardFocus();
        this.Repaint();
        current.Use();
      }
      bool flag = this.m_CPUHierarchyGUI.selectedIndex != -1;
      EditorGUI.BeginChangeCheck();
      this.m_SearchString = EditorGUI.ToolbarSearchField(GUIUtility.GetControlID(ProfilerWindow.s_HashControlID, FocusType.Keyboard, this.position), rect, this.m_SearchString, false);
      if (!EditorGUI.EndChangeCheck() || this.IsSearching() || (GUIUtility.keyboardControl != 0 || !flag))
        return;
      this.m_CPUHierarchyGUI.FrameSelection();
    }

    private static bool CheckFrameData(ProfilerProperty property)
    {
      return property.frameDataReady;
    }

    private void DrawCPUOrGPUPane(ProfilerHierarchyGUI mainPane, ProfilerTimelineGUI timelinePane)
    {
      bool showDetailedView = this.m_HierarchyViewDetailPaneType != ProfilerWindow.HierarchyViewDetailPaneType.None;
      ProfilerProperty profilerProperty = this.GetRootProfilerProperty();
      if (!ProfilerWindow.CheckFrameData(profilerProperty))
      {
        this.DrawCPUOrGPUHierarchyViewToolbar(profilerProperty, showDetailedView, timelinePane != null);
        GUILayout.Label(ProfilerWindow.Styles.noData, ProfilerWindow.Styles.label, new GUILayoutOption[0]);
      }
      else if (timelinePane != null && this.m_ViewType == ProfilerViewType.Timeline)
      {
        this.DrawCPUTimelineViewToolbar(profilerProperty);
        float height = (float) this.m_VertSplit.realSizes[1] - (EditorStyles.toolbar.CalcHeight(GUIContent.none, 10f) + 2f);
        timelinePane.DoGUI(this.GetActiveVisibleFrameIndex(), this.position.width, this.position.height - height, height);
      }
      else
      {
        if (showDetailedView)
          SplitterGUILayout.BeginHorizontalSplit(this.m_ViewSplit);
        GUILayout.BeginVertical();
        this.DrawCPUOrGPUHierarchyViewToolbar(profilerProperty, showDetailedView, timelinePane != null);
        bool expandAll = false;
        mainPane.DoGUI(profilerProperty, this.m_SearchString, expandAll);
        GUILayout.EndVertical();
        if (showDetailedView)
        {
          GUILayout.BeginVertical();
          EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
          this.m_HierarchyViewDetailPaneType = this.DrawCPUOrGPUDetailedViewPopup();
          GUILayout.FlexibleSpace();
          EditorGUILayout.EndHorizontal();
          switch (this.m_HierarchyViewDetailPaneType)
          {
            case ProfilerWindow.HierarchyViewDetailPaneType.Objects:
              mainPane.detailedObjectsView.DoGUI(ProfilerWindow.Styles.header, this.GetActiveVisibleFrameIndex(), this.m_ViewType);
              break;
            case ProfilerWindow.HierarchyViewDetailPaneType.CallersAndCallees:
              mainPane.detailedCallsView.DoGUI(ProfilerWindow.Styles.header, this.GetActiveVisibleFrameIndex(), this.m_ViewType);
              break;
          }
          GUILayout.EndVertical();
          SplitterGUILayout.EndHorizontalSplit();
        }
      }
    }

    private ProfilerWindow.HierarchyViewDetailPaneType DrawCPUOrGPUDetailedViewPopup()
    {
      return (ProfilerWindow.HierarchyViewDetailPaneType) EditorGUILayout.IntPopup((int) this.m_HierarchyViewDetailPaneType, ProfilerWindow.Styles.detailedPaneTypes, this.k_HierarchyViewDetailPaneTypes, EditorStyles.toolbarDropDown, new GUILayoutOption[1]{ GUILayout.Width(120f) });
    }

    public ProfilerProperty GetRootProfilerProperty()
    {
      return this.GetRootProfilerProperty(this.m_CurrentArea != ProfilerArea.CPU ? this.m_GPUHierarchyGUI.sortType : this.m_CPUHierarchyGUI.sortType);
    }

    public ProfilerProperty GetRootProfilerProperty(ProfilerColumn sortType)
    {
      if (this.m_CPUOrGPUProfilerProperty != null && this.m_CPUOrGPUProfilerPropertyConfig.frameIndex == this.GetActiveVisibleFrameIndex() && (this.m_CPUOrGPUProfilerPropertyConfig.area == this.m_CurrentArea && this.m_CPUOrGPUProfilerPropertyConfig.viewType == this.m_ViewType) && this.m_CPUOrGPUProfilerPropertyConfig.sortType == sortType)
      {
        this.m_CPUOrGPUProfilerProperty.ResetToRoot();
        return this.m_CPUOrGPUProfilerProperty;
      }
      if (this.m_CPUOrGPUProfilerProperty != null)
        this.m_CPUOrGPUProfilerProperty.Cleanup();
      this.m_CPUOrGPUProfilerProperty = this.CreateProperty(sortType);
      this.m_CPUOrGPUProfilerPropertyConfig.frameIndex = this.GetActiveVisibleFrameIndex();
      this.m_CPUOrGPUProfilerPropertyConfig.area = this.m_CurrentArea;
      this.m_CPUOrGPUProfilerPropertyConfig.viewType = this.m_ViewType;
      this.m_CPUOrGPUProfilerPropertyConfig.sortType = sortType;
      return this.m_CPUOrGPUProfilerProperty;
    }

    private void DrawMemoryPane(SplitterState splitter)
    {
      this.DrawMemoryToolbar();
      if (this.m_ShowDetailedMemoryPane == ProfilerMemoryView.Simple)
        this.DrawOverviewText(ProfilerArea.Memory);
      else
        this.DrawDetailedMemoryPane(splitter);
    }

    private void DrawDetailedMemoryPane(SplitterState splitter)
    {
      SplitterGUILayout.BeginHorizontalSplit(splitter);
      this.m_MemoryListView.OnGUI();
      this.m_ReferenceListView.OnGUI();
      SplitterGUILayout.EndHorizontalSplit();
    }

    private static Rect GenerateRect(ref int row, int indent)
    {
      Rect rect = new Rect((float) ((double) indent * 16.0 + 8.0), (float) row * 16f, 0.0f, 16f);
      rect.xMax = 350f;
      ++row;
      return rect;
    }

    private void DrawNetworkOperationsPane()
    {
      SplitterGUILayout.BeginHorizontalSplit(this.m_NetworkSplit);
      GUILayout.Label(ProfilerDriver.GetOverviewText(this.m_CurrentArea, this.GetActiveVisibleFrameIndex()), EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
      this.m_PaneScroll[(int) this.m_CurrentArea] = GUILayout.BeginScrollView(this.m_PaneScroll[(int) this.m_CurrentArea], ProfilerWindow.Styles.background);
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Operation Detail");
      EditorGUILayout.LabelField("Over 5 Ticks");
      EditorGUILayout.LabelField("Over 10 Ticks");
      EditorGUILayout.LabelField("Total");
      EditorGUILayout.EndHorizontal();
      ++EditorGUI.indentLevel;
      for (short key1 = 0; (int) key1 < this.msgNames.Length; ++key1)
      {
        if (NetworkDetailStats.m_NetworkOperations.ContainsKey(key1))
        {
          this.msgFoldouts[(int) key1] = EditorGUILayout.Foldout(this.msgFoldouts[(int) key1], this.msgNames[(int) key1] + ":");
          if (this.msgFoldouts[(int) key1])
          {
            EditorGUILayout.BeginVertical();
            NetworkDetailStats.NetworkOperationDetails networkOperation = NetworkDetailStats.m_NetworkOperations[key1];
            ++EditorGUI.indentLevel;
            foreach (string key2 in networkOperation.m_Entries.Keys)
            {
              int time = (int) Time.time;
              NetworkDetailStats.NetworkOperationEntryDetails entry = networkOperation.m_Entries[key2];
              if (entry.m_IncomingTotal > 0)
              {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("IN:" + key2);
                EditorGUILayout.LabelField(entry.m_IncomingSequence.GetFiveTick(time).ToString());
                EditorGUILayout.LabelField(entry.m_IncomingSequence.GetTenTick(time).ToString());
                EditorGUILayout.LabelField(entry.m_IncomingTotal.ToString());
                EditorGUILayout.EndHorizontal();
              }
              if (entry.m_OutgoingTotal > 0)
              {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("OUT:" + key2);
                EditorGUILayout.LabelField(entry.m_OutgoingSequence.GetFiveTick(time).ToString());
                EditorGUILayout.LabelField(entry.m_OutgoingSequence.GetTenTick(time).ToString());
                EditorGUILayout.LabelField(entry.m_OutgoingTotal.ToString());
                EditorGUILayout.EndHorizontal();
              }
            }
            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();
          }
        }
      }
      --EditorGUI.indentLevel;
      GUILayout.EndScrollView();
      SplitterGUILayout.EndHorizontalSplit();
    }

    private void AudioProfilerToggle(ProfilerCaptureFlags toggleFlag)
    {
      bool flag1 = ((ProfilerCaptureFlags) AudioSettings.profilerCaptureFlags & toggleFlag) != ProfilerCaptureFlags.None;
      bool flag2 = GUILayout.Toggle(flag1, "Record", EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (flag1 == flag2)
        return;
      ProfilerDriver.SetAudioCaptureFlags((int) ((ProfilerCaptureFlags) AudioSettings.profilerCaptureFlags & ~toggleFlag | (!flag2 ? ProfilerCaptureFlags.None : toggleFlag)));
    }

    private void DrawAudioPane()
    {
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      ProfilerAudioView profilerAudioView = this.m_ShowDetailedAudioPane;
      if (GUILayout.Toggle(profilerAudioView == ProfilerAudioView.Stats, "Stats", EditorStyles.toolbarButton, new GUILayoutOption[0]))
        profilerAudioView = ProfilerAudioView.Stats;
      if (GUILayout.Toggle(profilerAudioView == ProfilerAudioView.Channels, "Channels", EditorStyles.toolbarButton, new GUILayoutOption[0]))
        profilerAudioView = ProfilerAudioView.Channels;
      if (GUILayout.Toggle(profilerAudioView == ProfilerAudioView.Groups, "Groups", EditorStyles.toolbarButton, new GUILayoutOption[0]))
        profilerAudioView = ProfilerAudioView.Groups;
      if (GUILayout.Toggle(profilerAudioView == ProfilerAudioView.ChannelsAndGroups, "Channels and groups", EditorStyles.toolbarButton, new GUILayoutOption[0]))
        profilerAudioView = ProfilerAudioView.ChannelsAndGroups;
      if (Unsupported.IsDeveloperBuild() && GUILayout.Toggle(profilerAudioView == ProfilerAudioView.DSPGraph, "DSP Graph", EditorStyles.toolbarButton, new GUILayoutOption[0]))
        profilerAudioView = ProfilerAudioView.DSPGraph;
      if (Unsupported.IsDeveloperBuild() && GUILayout.Toggle(profilerAudioView == ProfilerAudioView.Clips, "Clips", EditorStyles.toolbarButton, new GUILayoutOption[0]))
        profilerAudioView = ProfilerAudioView.Clips;
      if (profilerAudioView != this.m_ShowDetailedAudioPane)
      {
        this.m_ShowDetailedAudioPane = profilerAudioView;
        this.m_LastAudioProfilerFrame = -1;
      }
      if (this.m_ShowDetailedAudioPane == ProfilerAudioView.Stats)
      {
        GUILayout.Space(5f);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        this.DrawOverviewText(this.m_CurrentArea);
      }
      else if (this.m_ShowDetailedAudioPane == ProfilerAudioView.DSPGraph)
      {
        GUILayout.Space(5f);
        this.AudioProfilerToggle(ProfilerCaptureFlags.DSPNodes);
        GUILayout.Space(5f);
        this.m_ShowInactiveDSPChains = GUILayout.Toggle(this.m_ShowInactiveDSPChains, "Show inactive", EditorStyles.toolbarButton, new GUILayoutOption[0]);
        if (this.m_ShowInactiveDSPChains)
          this.m_HighlightAudibleDSPChains = GUILayout.Toggle(this.m_HighlightAudibleDSPChains, "Highlight audible", EditorStyles.toolbarButton, new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        Rect rect = GUILayoutUtility.GetRect(20f, 10000f, 10f, 20000f);
        this.m_PaneScroll_AudioDSP = GUI.BeginScrollView(rect, this.m_PaneScroll_AudioDSP, new Rect(0.0f, 0.0f, 10000f, 20000f));
        Rect clippingRect = new Rect(this.m_PaneScroll_AudioDSP.x, this.m_PaneScroll_AudioDSP.y, rect.width, rect.height);
        if (this.m_AudioProfilerDSPView == null)
          this.m_AudioProfilerDSPView = new AudioProfilerDSPView();
        ProfilerProperty property = this.CreateProperty();
        if (ProfilerWindow.CheckFrameData(property))
          this.m_AudioProfilerDSPView.OnGUI(clippingRect, property, this.m_ShowInactiveDSPChains, this.m_HighlightAudibleDSPChains, ref this.m_DSPGraphZoomFactor, ref this.m_PaneScroll_AudioDSP);
        property.Cleanup();
        GUI.EndScrollView();
        this.Repaint();
      }
      else if (this.m_ShowDetailedAudioPane == ProfilerAudioView.Clips)
      {
        GUILayout.Space(5f);
        this.AudioProfilerToggle(ProfilerCaptureFlags.Clips);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        Rect rect1 = GUILayoutUtility.GetRect(20f, 20000f, 10f, 10000f);
        Rect position = new Rect(rect1.x, rect1.y, 230f, rect1.height);
        Rect rect2 = new Rect(position.xMax, rect1.y, rect1.width - position.width, rect1.height);
        string overviewText = ProfilerDriver.GetOverviewText(this.m_CurrentArea, this.GetActiveVisibleFrameIndex());
        Vector2 vector2 = EditorStyles.wordWrappedLabel.CalcSize(GUIContent.Temp(overviewText));
        this.m_PaneScroll_AudioClips = GUI.BeginScrollView(position, this.m_PaneScroll_AudioClips, new Rect(0.0f, 0.0f, vector2.x, vector2.y));
        GUI.Label(new Rect(3f, 3f, vector2.x, vector2.y), overviewText, EditorStyles.wordWrappedLabel);
        GUI.EndScrollView();
        EditorGUI.DrawRect(new Rect(position.xMax - 1f, position.y, 1f, position.height), Color.black);
        if (this.m_AudioProfilerClipTreeViewState == null)
          this.m_AudioProfilerClipTreeViewState = new AudioProfilerClipTreeViewState();
        if (this.m_AudioProfilerClipViewBackend == null)
          this.m_AudioProfilerClipViewBackend = new AudioProfilerClipViewBackend(this.m_AudioProfilerClipTreeViewState);
        ProfilerProperty property = this.CreateProperty();
        if (ProfilerWindow.CheckFrameData(property))
        {
          if (this.m_CurrentFrame == -1 || this.m_LastAudioProfilerFrame != this.m_CurrentFrame)
          {
            this.m_LastAudioProfilerFrame = this.m_CurrentFrame;
            AudioProfilerClipInfo[] profilerClipInfo = property.GetAudioProfilerClipInfo();
            if (profilerClipInfo != null && profilerClipInfo.Length > 0)
            {
              List<AudioProfilerClipInfoWrapper> data = new List<AudioProfilerClipInfoWrapper>();
              foreach (AudioProfilerClipInfo info in profilerClipInfo)
                data.Add(new AudioProfilerClipInfoWrapper(info, property.GetAudioProfilerNameByOffset(info.assetNameOffset)));
              this.m_AudioProfilerClipViewBackend.SetData(data);
              if (this.m_AudioProfilerClipView == null)
              {
                this.m_AudioProfilerClipView = new AudioProfilerClipView((EditorWindow) this, this.m_AudioProfilerClipTreeViewState);
                this.m_AudioProfilerClipView.Init(rect2, this.m_AudioProfilerClipViewBackend);
              }
            }
          }
          if (this.m_AudioProfilerClipView != null)
            this.m_AudioProfilerClipView.OnGUI(rect2);
        }
        property.Cleanup();
      }
      else
      {
        GUILayout.Space(5f);
        this.AudioProfilerToggle(ProfilerCaptureFlags.Channels);
        GUILayout.Space(5f);
        bool flag1 = GUILayout.Toggle(AudioUtil.resetAllAudioClipPlayCountsOnPlay, "Reset play count on play", EditorStyles.toolbarButton, new GUILayoutOption[0]);
        if (flag1 != AudioUtil.resetAllAudioClipPlayCountsOnPlay)
          AudioUtil.resetAllAudioClipPlayCountsOnPlay = flag1;
        if (Unsupported.IsDeveloperBuild())
        {
          GUILayout.Space(5f);
          bool flag2 = EditorPrefs.GetBool("AudioProfilerShowAllGroups");
          bool flag3 = GUILayout.Toggle(flag2, "Show all groups (dev-builds only)", EditorStyles.toolbarButton, new GUILayoutOption[0]);
          if (flag2 != flag3)
            EditorPrefs.SetBool("AudioProfilerShowAllGroups", flag3);
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        Rect rect1 = GUILayoutUtility.GetRect(20f, 20000f, 10f, 10000f);
        Rect position = new Rect(rect1.x, rect1.y, 230f, rect1.height);
        Rect rect2 = new Rect(position.xMax, rect1.y, rect1.width - position.width, rect1.height);
        string overviewText = ProfilerDriver.GetOverviewText(this.m_CurrentArea, this.GetActiveVisibleFrameIndex());
        Vector2 vector2 = EditorStyles.wordWrappedLabel.CalcSize(GUIContent.Temp(overviewText));
        this.m_PaneScroll_AudioChannels = GUI.BeginScrollView(position, this.m_PaneScroll_AudioChannels, new Rect(0.0f, 0.0f, vector2.x, vector2.y));
        GUI.Label(new Rect(3f, 3f, vector2.x, vector2.y), overviewText, EditorStyles.wordWrappedLabel);
        GUI.EndScrollView();
        EditorGUI.DrawRect(new Rect(position.xMax - 1f, position.y, 1f, position.height), Color.black);
        if (this.m_AudioProfilerGroupTreeViewState == null)
          this.m_AudioProfilerGroupTreeViewState = new AudioProfilerGroupTreeViewState();
        if (this.m_AudioProfilerGroupViewBackend == null)
          this.m_AudioProfilerGroupViewBackend = new AudioProfilerGroupViewBackend(this.m_AudioProfilerGroupTreeViewState);
        ProfilerProperty property = this.CreateProperty();
        if (ProfilerWindow.CheckFrameData(property))
        {
          if (this.m_CurrentFrame == -1 || this.m_LastAudioProfilerFrame != this.m_CurrentFrame)
          {
            this.m_LastAudioProfilerFrame = this.m_CurrentFrame;
            AudioProfilerGroupInfo[] profilerGroupInfo = property.GetAudioProfilerGroupInfo();
            if (profilerGroupInfo != null && profilerGroupInfo.Length > 0)
            {
              List<AudioProfilerGroupInfoWrapper> data = new List<AudioProfilerGroupInfoWrapper>();
              foreach (AudioProfilerGroupInfo info in profilerGroupInfo)
              {
                bool flag2 = (info.flags & 64) != 0;
                if ((this.m_ShowDetailedAudioPane != ProfilerAudioView.Channels || !flag2) && (this.m_ShowDetailedAudioPane != ProfilerAudioView.Groups || flag2))
                  data.Add(new AudioProfilerGroupInfoWrapper(info, property.GetAudioProfilerNameByOffset(info.assetNameOffset), property.GetAudioProfilerNameByOffset(info.objectNameOffset), this.m_ShowDetailedAudioPane == ProfilerAudioView.Channels));
              }
              this.m_AudioProfilerGroupViewBackend.SetData(data);
              if (this.m_AudioProfilerGroupView == null)
              {
                this.m_AudioProfilerGroupView = new AudioProfilerGroupView((EditorWindow) this, this.m_AudioProfilerGroupTreeViewState);
                this.m_AudioProfilerGroupView.Init(rect2, this.m_AudioProfilerGroupViewBackend);
              }
            }
          }
          if (this.m_AudioProfilerGroupView != null)
            this.m_AudioProfilerGroupView.OnGUI(rect2, this.m_ShowDetailedAudioPane == ProfilerAudioView.Channels);
        }
        property.Cleanup();
      }
    }

    private static void DrawBackground(int row, bool selected)
    {
      Rect position = new Rect(1f, 16f * (float) row, GUIClip.visibleRect.width, 16f);
      GUIStyle guiStyle = row % 2 != 0 ? ProfilerWindow.Styles.entryOdd : ProfilerWindow.Styles.entryEven;
      if (Event.current.type != EventType.Repaint)
        return;
      guiStyle.Draw(position, GUIContent.none, false, false, selected, false);
    }

    private void DrawMemoryToolbar()
    {
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      this.m_ShowDetailedMemoryPane = (ProfilerMemoryView) EditorGUILayout.EnumPopup((Enum) this.m_ShowDetailedMemoryPane, EditorStyles.toolbarDropDown, new GUILayoutOption[1]
      {
        GUILayout.Width(70f)
      });
      GUILayout.Space(5f);
      if (this.m_ShowDetailedMemoryPane == ProfilerMemoryView.Detailed)
      {
        if (GUILayout.Button("Take Sample: " + this.m_AttachProfilerUI.GetConnectedProfiler(), EditorStyles.toolbarButton, new GUILayoutOption[0]))
          this.RefreshMemoryData();
        this.m_GatherObjectReferences = GUILayout.Toggle(this.m_GatherObjectReferences, ProfilerWindow.Styles.gatherObjectReferences, EditorStyles.toolbarButton, new GUILayoutOption[0]);
        if (this.m_AttachProfilerUI.IsEditor())
          GUILayout.Label("Memory usage in editor is not as it would be in a player", EditorStyles.toolbarButton, new GUILayoutOption[0]);
      }
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
    }

    private void RefreshMemoryData()
    {
      this.m_MemoryListView.RequiresRefresh = true;
      ProfilerDriver.RequestObjectMemoryInfo(this.m_GatherObjectReferences);
    }

    private static void UpdateChartGrid(float timeMax, ChartViewData data)
    {
      if ((double) timeMax < 1500.0)
        data.SetGrid(new float[3]{ 1000f, 250f, 100f }, new string[3]
        {
          "1ms (1000FPS)",
          "0.25ms (4000FPS)",
          "0.1ms (10000FPS)"
        });
      else if ((double) timeMax < 10000.0)
        data.SetGrid(new float[3]{ 8333f, 4000f, 1000f }, new string[3]
        {
          "8ms (120FPS)",
          "4ms (250FPS)",
          "1ms (1000FPS)"
        });
      else if ((double) timeMax < 30000.0)
        data.SetGrid(new float[3]{ 16667f, 10000f, 5000f }, new string[3]
        {
          "16ms (60FPS)",
          "10ms (100FPS)",
          "5ms (200FPS)"
        });
      else if ((double) timeMax < 100000.0)
        data.SetGrid(new float[3]{ 66667f, 33333f, 16667f }, new string[3]
        {
          "66ms (15FPS)",
          "33ms (30FPS)",
          "16ms (60FPS)"
        });
      else
        data.SetGrid(new float[3]
        {
          500000f,
          200000f,
          66667f
        }, new string[3]
        {
          "500ms (2FPS)",
          "200ms (5FPS)",
          "66ms (15FPS)"
        });
    }

    private void UpdateCharts()
    {
      int historyLength = ProfilerDriver.maxHistoryLength - 1;
      int num = ProfilerDriver.lastFrameIndex - historyLength;
      int firstFrame = Mathf.Max(ProfilerDriver.firstFrameIndex, num);
      foreach (ProfilerChart chart in this.m_Charts)
        ProfilerWindow.UpdateSingleChart(chart, num, firstFrame);
      bool flag = ProfilerDriver.selectedPropertyPath != string.Empty && this.m_CurrentArea == ProfilerArea.CPU;
      ProfilerChart chart1 = this.m_Charts[0];
      if (flag)
      {
        chart1.m_Data.hasOverlay = true;
        int numSeries = chart1.m_Data.numSeries;
        for (int index1 = 0; index1 < numSeries; ++index1)
        {
          ChartSeriesViewData chartSeriesViewData = chart1.m_Data.series[index1];
          chart1.m_Data.overlays[index1] = new ChartSeriesViewData(chartSeriesViewData.name, chartSeriesViewData.yValues.Length, chartSeriesViewData.color);
          for (int index2 = 0; index2 < chartSeriesViewData.yValues.Length; ++index2)
            chart1.m_Data.overlays[index1].xValues[index2] = (float) index2;
          float maxValue;
          ProfilerDriver.GetStatisticsValues(ProfilerDriver.GetStatisticsIdentifier(string.Format("Selected{0}", (object) chart1.m_Data.series[index1].name)), num, chart1.m_DataScale, chart1.m_Data.overlays[index1].yValues, out maxValue);
        }
      }
      else
        chart1.m_Data.hasOverlay = false;
      for (int index = 0; index < ProfilerWindow.ms_StackedAreas.Length; ++index)
        this.ComputeChartScaleValue(ProfilerWindow.ms_StackedAreas[index], historyLength, num, firstFrame);
      string str = (string) null;
      if (!ProfilerDriver.isGPUProfilerSupported)
      {
        str = "GPU profiling is not supported by the graphics card driver. Please update to a newer version if available.";
        if (Application.platform == RuntimePlatform.OSXEditor)
          str = ProfilerDriver.isGPUProfilerSupportedByOS ? "GPU profiling is not supported by the graphics card driver (or it was disabled because of driver bugs)." : "GPU profiling requires Mac OS X 10.7 (Lion) and a capable video card. GPU profiling is currently not supported on mobile.";
      }
      this.m_Charts[1].m_NotSupportedWarning = str;
    }

    private void ComputeChartScaleValue(ProfilerArea i, int historyLength, int firstEmptyFrame, int firstFrame)
    {
      ProfilerChart chart = this.m_Charts[(int) i];
      float val1 = 0.0f;
      float num1 = 0.0f;
      for (int index1 = 0; index1 < historyLength; ++index1)
      {
        float num2 = 0.0f;
        for (int index2 = 0; index2 < chart.m_Series.Length; ++index2)
        {
          if (chart.m_Series[index2].enabled)
            num2 += chart.m_Series[index2].yValues[index1];
        }
        if ((double) num2 > (double) val1)
          val1 = num2;
        if ((double) num2 > (double) num1 && index1 + firstEmptyFrame >= firstFrame + 1)
          num1 = num2;
      }
      if ((double) num1 != 0.0)
        val1 = num1;
      float num3 = Math.Min(val1, this.m_ChartMaxClamp);
      if ((double) this.m_ChartOldMax[(int) i] > 0.0)
        num3 = Mathf.Lerp(this.m_ChartOldMax[(int) i], num3, 0.4f);
      this.m_ChartOldMax[(int) i] = num3;
      for (int index = 0; index < chart.m_Data.numSeries; ++index)
        chart.m_Data.series[index].rangeAxis = new Vector2(0.0f, num3);
      ProfilerWindow.UpdateChartGrid(num3, chart.m_Data);
    }

    internal static void UpdateSingleChart(ProfilerChart chart, int firstEmptyFrame, int firstFrame)
    {
      float num = 1f;
      float[] numArray = new float[chart.m_Series.Length];
      int index1 = 0;
      for (int length = chart.m_Series.Length; index1 < length; ++index1)
      {
        float maxValue;
        ProfilerDriver.GetStatisticsValues(ProfilerDriver.GetStatisticsIdentifier(chart.m_Series[index1].name), firstEmptyFrame, chart.m_DataScale, chart.m_Series[index1].yValues, out maxValue);
        maxValue = Mathf.Max(maxValue, 0.0001f);
        if ((double) maxValue > (double) num)
          num = maxValue;
        if (chart.m_Type == Chart.ChartType.Line)
        {
          numArray[index1] = maxValue * (float) (1.04999995231628 + (double) index1 * 0.0500000007450581);
          chart.m_Series[index1].rangeAxis = new Vector2(0.0f, numArray[index1]);
        }
        else
          numArray[index1] = maxValue;
      }
      if (chart.m_Area == ProfilerArea.NetworkMessages || chart.m_Area == ProfilerArea.NetworkOperations)
      {
        int index2 = 0;
        for (int length = chart.m_Series.Length; index2 < length; ++index2)
          chart.m_Series[index2].rangeAxis = new Vector2(0.0f, 0.9f * num);
        chart.m_Data.maxValue = num;
      }
      chart.m_Data.Assign(chart.m_Series, firstEmptyFrame, firstFrame);
      if (!(chart is UISystemProfilerChart))
        return;
      ((UISystemProfilerChart) chart).Update(firstFrame, ProfilerDriver.maxHistoryLength - 1);
    }

    private void AddAreaClick(object userData, string[] options, int selected)
    {
      this.m_Charts[selected].active = true;
    }

    private void MemRecordModeClick(object userData, string[] options, int selected)
    {
      this.m_SelectedMemRecordMode = (ProfilerMemoryRecordMode) selected;
      ProfilerDriver.memoryRecordMode = this.m_SelectedMemRecordMode;
    }

    private void SaveProfilingData()
    {
      string text = EditorGUIUtility.TempContent("Save profile").text;
      string path = EditorPrefs.GetString("ProfilerRecentSaveLoadProfilePath");
      string directory = !string.IsNullOrEmpty(path) ? Path.GetDirectoryName(path) : "";
      string defaultName = !string.IsNullOrEmpty(path) ? Path.GetFileName(path) : "";
      string filename = EditorUtility.SaveFilePanel(text, directory, defaultName, "data");
      if (filename.Length != 0)
      {
        EditorPrefs.SetString("ProfilerRecentSaveLoadProfilePath", filename);
        ProfilerDriver.SaveProfile(filename);
      }
      GUIUtility.ExitGUI();
    }

    private void LoadProfilingData(bool keepExistingData)
    {
      string filename = EditorUtility.OpenFilePanel(EditorGUIUtility.TempContent("Load profile").text, EditorPrefs.GetString("ProfilerRecentSaveLoadProfilePath"), "data");
      if (filename.Length != 0)
      {
        EditorPrefs.SetString("ProfilerRecentSaveLoadProfilePath", filename);
        if (ProfilerDriver.LoadProfile(filename, keepExistingData))
        {
          ProfilerDriver.enabled = this.m_Recording = false;
          SessionState.SetBool("ProfilerEnabled", this.m_Recording);
          NetworkDetailStats.m_NetworkOperations.Clear();
        }
      }
      GUIUtility.ExitGUI();
    }

    private void DrawMainToolbar()
    {
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      Rect rect = GUILayoutUtility.GetRect(ProfilerWindow.Styles.addArea, EditorStyles.toolbarDropDown, new GUILayoutOption[1]{ GUILayout.Width(120f) });
      if (EditorGUI.DropdownButton(rect, ProfilerWindow.Styles.addArea, FocusType.Passive, EditorStyles.toolbarDropDown))
      {
        int length = this.m_Charts.Length;
        string[] options = new string[length];
        bool[] enabled = new bool[length];
        for (int index = 0; index < length; ++index)
        {
          options[index] = ((ProfilerArea) index).ToString();
          enabled[index] = !this.m_Charts[index].active;
        }
        EditorUtility.DisplayCustomMenu(rect, options, enabled, (int[]) null, new EditorUtility.SelectMenuItemFunction(this.AddAreaClick), (object) null);
      }
      GUILayout.FlexibleSpace();
      bool flag = GUILayout.Toggle(this.m_Recording, ProfilerWindow.Styles.profilerRecord, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (flag != this.m_Recording)
      {
        ProfilerDriver.enabled = flag;
        this.m_Recording = flag;
        SessionState.SetBool("ProfilerEnabled", flag);
      }
      EditorGUI.BeginDisabledGroup(!this.m_AttachProfilerUI.IsEditor());
      ProfilerWindow.SetProfileDeepScripts(GUILayout.Toggle(ProfilerDriver.deepProfiling, ProfilerWindow.Styles.deepProfile, EditorStyles.toolbarButton, new GUILayoutOption[0]));
      ProfilerDriver.profileEditor = GUILayout.Toggle(ProfilerDriver.profileEditor, ProfilerWindow.Styles.profileEditor, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      EditorGUI.EndDisabledGroup();
      this.m_AttachProfilerUI.OnGUILayout((EditorWindow) this);
      GUILayout.FlexibleSpace();
      if (GUILayout.Button(ProfilerWindow.Styles.clearData, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        this.Clear();
      if (GUILayout.Button(ProfilerWindow.Styles.loadProfilingData, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        this.LoadProfilingData(Event.current.shift);
      using (new EditorGUI.DisabledScope(ProfilerDriver.lastFrameIndex == -1))
      {
        if (GUILayout.Button(ProfilerWindow.Styles.saveProfilingData, EditorStyles.toolbarButton, new GUILayoutOption[0]))
          this.SaveProfilingData();
      }
      GUILayout.Space(5f);
      this.FrameNavigationControls();
      GUILayout.EndHorizontal();
    }

    private void Clear()
    {
      if (this.m_CPUOrGPUProfilerProperty != null)
      {
        this.m_CPUOrGPUProfilerProperty.Cleanup();
        this.m_CPUOrGPUProfilerProperty = (ProfilerProperty) null;
      }
      this.m_CPUOrGPUProfilerPropertyConfig.frameIndex = -1;
      this.m_CPUHierarchyGUI.ClearCaches();
      this.m_GPUHierarchyGUI.ClearCaches();
      ProfilerDriver.ClearAllFrames();
      NetworkDetailStats.m_NetworkOperations.Clear();
    }

    private void FrameNavigationControls()
    {
      if (this.m_CurrentFrame > ProfilerDriver.lastFrameIndex)
        this.SetCurrentFrameDontPause(ProfilerDriver.lastFrameIndex);
      GUILayout.Label(ProfilerWindow.Styles.frame, EditorStyles.miniLabel, new GUILayoutOption[0]);
      GUILayout.Label("   " + this.PickFrameLabel(), EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        GUILayout.Width(100f)
      });
      GUI.enabled = ProfilerDriver.GetPreviousFrameIndex(this.m_CurrentFrame) != -1;
      if (GUILayout.Button(ProfilerWindow.Styles.prevFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        this.PrevFrame();
      GUI.enabled = ProfilerDriver.GetNextFrameIndex(this.m_CurrentFrame) != -1;
      if (GUILayout.Button(ProfilerWindow.Styles.nextFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        this.NextFrame();
      GUI.enabled = true;
      GUILayout.Space(10f);
      if (!GUILayout.Button(ProfilerWindow.Styles.currentFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        return;
      this.SetCurrentFrame(-1);
      this.m_LastFrameFromTick = ProfilerDriver.lastFrameIndex;
    }

    private static void DrawOtherToolbar(ProfilerArea area)
    {
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      if (area == ProfilerArea.Rendering && GUILayout.Button(!GUI.enabled ? ProfilerWindow.Styles.noFrameDebugger : ProfilerWindow.Styles.frameDebugger, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        FrameDebuggerWindow.ShowFrameDebuggerWindow().EnableIfNeeded();
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
    }

    private void DrawOverviewText(ProfilerArea area)
    {
      if (area == ProfilerArea.AreaCount)
        return;
      this.m_PaneScroll[(int) area] = GUILayout.BeginScrollView(this.m_PaneScroll[(int) area], ProfilerWindow.Styles.background);
      GUILayout.Label(ProfilerDriver.GetOverviewText(area, this.GetActiveVisibleFrameIndex()), EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
      GUILayout.EndScrollView();
    }

    private void DrawPane(ProfilerArea area)
    {
      ProfilerWindow.DrawOtherToolbar(area);
      this.DrawOverviewText(area);
    }

    private void SetCurrentFrameDontPause(int frame)
    {
      this.m_CurrentFrame = frame;
    }

    private void SetCurrentFrame(int frame)
    {
      if (frame != -1 && ProfilerDriver.enabled && (!ProfilerDriver.profileEditor && this.m_CurrentFrame != frame) && EditorApplication.isPlayingOrWillChangePlaymode)
        EditorApplication.isPaused = true;
      if (ProfilerInstrumentationPopup.InstrumentationEnabled)
        ProfilerInstrumentationPopup.UpdateInstrumentableFunctions();
      this.SetCurrentFrameDontPause(frame);
    }

    private void OnGUI()
    {
      this.CheckForPlatformModuleChange();
      this.DrawMainToolbar();
      SplitterGUILayout.BeginVerticalSplit(this.m_VertSplit);
      this.m_GraphPos = EditorGUILayout.BeginScrollView(this.m_GraphPos, ProfilerWindow.Styles.profilerGraphBackground, new GUILayoutOption[0]);
      if (this.m_PrevLastFrame != ProfilerDriver.lastFrameIndex)
      {
        this.UpdateCharts();
        this.m_PrevLastFrame = ProfilerDriver.lastFrameIndex;
      }
      int num = this.m_CurrentFrame;
      for (int index = 0; index < this.m_Charts.Length; ++index)
      {
        ProfilerChart chart = this.m_Charts[index];
        if (chart.active)
          num = chart.DoChartGUI(num, this.m_CurrentArea);
      }
      if (num != this.m_CurrentFrame)
      {
        this.SetCurrentFrame(num);
        this.Repaint();
        GUIUtility.ExitGUI();
      }
      EditorGUILayout.EndScrollView();
      GUILayout.BeginVertical();
      switch (this.m_CurrentArea)
      {
        case ProfilerArea.CPU:
          this.DrawCPUOrGPUPane(this.m_CPUHierarchyGUI, this.m_CPUTimelineGUI);
          break;
        case ProfilerArea.GPU:
          this.DrawCPUOrGPUPane(this.m_GPUHierarchyGUI, (ProfilerTimelineGUI) null);
          break;
        case ProfilerArea.Memory:
          this.DrawMemoryPane(this.m_ViewSplit);
          break;
        case ProfilerArea.Audio:
          this.DrawAudioPane();
          break;
        case ProfilerArea.NetworkMessages:
          this.DrawPane(this.m_CurrentArea);
          break;
        case ProfilerArea.NetworkOperations:
          this.DrawNetworkOperationsPane();
          break;
        case ProfilerArea.UI:
        case ProfilerArea.UIDetails:
          this.m_UISystemProfiler.DrawUIPane(this, this.m_CurrentArea, (UISystemProfilerChart) this.m_Charts[11]);
          break;
        default:
          this.DrawPane(this.m_CurrentArea);
          break;
      }
      GUILayout.EndVertical();
      SplitterGUILayout.EndVerticalSplit();
    }

    internal static class Styles
    {
      public static readonly GUIContent addArea = EditorGUIUtility.TextContent("Add Profiler|Add a profiler area");
      public static readonly GUIContent deepProfile = EditorGUIUtility.TextContent("Deep Profile|Instrument all mono calls to investigate scripts");
      public static readonly GUIContent profileEditor = EditorGUIUtility.TextContent("Profile Editor|Enable profiling of the editor");
      public static readonly GUIContent noData = EditorGUIUtility.TextContent("No frame data available");
      public static readonly GUIContent frameDebugger = EditorGUIUtility.TextContent("Open Frame Debugger|Frame Debugger for current game view");
      public static readonly GUIContent noFrameDebugger = EditorGUIUtility.TextContent("Frame Debugger|Open Frame Debugger (Current frame needs to be selected)");
      public static readonly GUIContent gatherObjectReferences = EditorGUIUtility.TextContent("Gather object references|Collect reference information to see where objects are referenced from. Disable this to save memory");
      public static readonly GUIContent memRecord = EditorGUIUtility.TextContent("Mem Record|Record activity in the native memory system");
      public static readonly GUIContent profilerRecord = EditorGUIUtility.TextContentWithIcon("Record|Record profiling information", "Profiler.Record");
      public static readonly GUIContent profilerInstrumentation = EditorGUIUtility.TextContent("Instrumentation|Add Profiler Instrumentation to selected functions");
      public static readonly GUIContent prevFrame = EditorGUIUtility.IconContent("Profiler.PrevFrame", "|Go back one frame");
      public static readonly GUIContent nextFrame = EditorGUIUtility.IconContent("Profiler.NextFrame", "|Go one frame forwards");
      public static readonly GUIContent currentFrame = EditorGUIUtility.TextContent("Current|Go to current frame");
      public static readonly GUIContent frame = EditorGUIUtility.TextContent("Frame: ");
      public static readonly GUIContent clearData = EditorGUIUtility.TextContent("Clear");
      public static readonly GUIContent saveProfilingData = EditorGUIUtility.TextContent("Save|Save current profiling information to a binary file");
      public static readonly GUIContent loadProfilingData = EditorGUIUtility.TextContent("Load|Load binary profiling information from a file. Shift click to append to the existing data");
      public static readonly GUIContent[] reasons = ProfilerWindow.Styles.GetLocalizedReasons();
      public static readonly GUIContent[] detailedPaneTypes = ProfilerWindow.Styles.GetLocalizedDetailedPaneTypes();
      public static readonly GUIContent accessibilityModeLabel = EditorGUIUtility.TextContent("Color Blind Mode");
      public static readonly GUIStyle background = (GUIStyle) "OL Box";
      public static readonly GUIStyle header = (GUIStyle) "OL title";
      public static readonly GUIStyle label = (GUIStyle) "OL label";
      public static readonly GUIStyle entryEven = (GUIStyle) "OL EntryBackEven";
      public static readonly GUIStyle entryOdd = (GUIStyle) "OL EntryBackOdd";
      public static readonly GUIStyle profilerGraphBackground = (GUIStyle) "ProfilerScrollviewBackground";

      static Styles()
      {
        ProfilerWindow.Styles.profilerGraphBackground.overflow.left = -180;
      }

      internal static GUIContent[] GetLocalizedReasons()
      {
        return new GUIContent[11]{ EditorGUIUtility.TextContent("Scene object (Unloaded by loading a new scene or destroying it)"), EditorGUIUtility.TextContent("Builtin Resource (Never unloaded)"), EditorGUIUtility.TextContent("Object is marked Don't Save. (Must be explicitly destroyed or it will leak)"), EditorGUIUtility.TextContent("Asset is dirty and must be saved first (Editor only)"), null, EditorGUIUtility.TextContent("Asset type created from code or stored in the scene, referenced from native code."), EditorGUIUtility.TextContent("Asset type created from code or stored in the scene, referenced from scripts and native code."), null, EditorGUIUtility.TextContent("Asset referenced from native code."), EditorGUIUtility.TextContent("Asset referenced from scripts and native code."), EditorGUIUtility.TextContent("Not Applicable") };
      }

      private static GUIContent[] GetLocalizedDetailedPaneTypes()
      {
        return new GUIContent[3]{ EditorGUIUtility.TextContent("No Details"), EditorGUIUtility.TextContent("Show Related Objects"), EditorGUIUtility.TextContent("Show Calls") };
      }
    }

    private struct CachedProfilerPropertyConfig
    {
      public int frameIndex;
      public ProfilerArea area;
      public ProfilerViewType viewType;
      public ProfilerColumn sortType;
    }

    private enum HierarchyViewDetailPaneType
    {
      None,
      Objects,
      CallersAndCallees,
    }
  }
}
