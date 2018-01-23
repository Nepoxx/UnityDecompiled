// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectBrowser
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.IMGUI.Controls;
using UnityEditor.ProjectWindowCallback;
using UnityEditor.TreeViewExamples;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [EditorWindowTitle(icon = "Project", title = "Project")]
  internal class ProjectBrowser : EditorWindow, IHasCustomMenu
  {
    private static List<ProjectBrowser> s_ProjectBrowsers = new List<ProjectBrowser>();
    private static int s_HashForSearchField = "ProjectBrowserSearchField".GetHashCode();
    [NonSerialized]
    private string m_SearchFieldText = "";
    [SerializeField]
    private ProjectBrowser.ViewMode m_ViewMode = ProjectBrowser.ViewMode.TwoColumns;
    [SerializeField]
    private int m_StartGridSize = 64;
    [SerializeField]
    private string[] m_LastFolders = new string[0];
    [SerializeField]
    private float m_LastFoldersGridSize = -1f;
    private bool m_EnableOldAssetTree = true;
    private List<GUIContent> m_SelectedPathSplitted = new List<GUIContent>();
    private bool m_DidSelectSearchResult = false;
    private bool m_ItemSelectedByRightClickThisEvent = false;
    private bool m_InternalSelectionChange = false;
    private SearchFilter.SearchArea m_LastLocalAssetsSearchArea = SearchFilter.SearchArea.AllAssets;
    private bool m_GrabKeyboardFocusForListArea = false;
    private List<KeyValuePair<GUIContent, string>> m_BreadCrumbs = new List<KeyValuePair<GUIContent, string>>();
    private bool m_BreadCrumbLastFolderHasSubFolders = false;
    private float k_MinDirectoriesAreaWidth = 110f;
    [SerializeField]
    private float m_DirectoriesAreaWidth = 115f;
    [NonSerialized]
    private float m_SearchAreaMenuOffset = -1f;
    [NonSerialized]
    private int m_LastFramedID = -1;
    [NonSerialized]
    public GUIContent m_SearchAllAssets = new GUIContent("Assets");
    [NonSerialized]
    public GUIContent m_SearchInFolders = new GUIContent("");
    [NonSerialized]
    public GUIContent m_SearchAssetStore = new GUIContent("Asset Store");
    public static ProjectBrowser s_LastInteractedProjectBrowser;
    private static ProjectBrowser.Styles s_Styles;
    [SerializeField]
    private SearchFilter m_SearchFilter;
    [SerializeField]
    private string m_LastProjectPath;
    [SerializeField]
    private bool m_IsLocked;
    private bool m_FocusSearchField;
    private string m_SelectedPath;
    private float m_LastListWidth;
    private PopupList.InputData m_AssetLabels;
    private PopupList.InputData m_ObjectTypes;
    private bool m_UseTreeViewSelectionInsteadOfMainSelection;
    [SerializeField]
    private TreeViewStateWithAssetUtility m_FolderTreeState;
    private TreeViewController m_FolderTree;
    private int m_TreeViewKeyboardControlID;
    [SerializeField]
    private TreeViewStateWithAssetUtility m_AssetTreeState;
    private TreeViewController m_AssetTree;
    [SerializeField]
    private ObjectListAreaState m_ListAreaState;
    private ObjectListArea m_ListArea;
    private int m_ListKeyboardControlID;
    private ExposablePopupMenu m_SearchAreaMenu;
    private const float k_MinHeight = 250f;
    private const float k_MinWidthOneColumn = 230f;
    private const float k_MinWidthTwoColumns = 230f;
    private float m_ToolbarHeight;
    private const float k_BottomBarHeight = 17f;
    private const float k_ResizerWidth = 5f;
    private const float k_SliderWidth = 55f;
    [NonSerialized]
    private Rect m_ListAreaRect;
    [NonSerialized]
    private Rect m_TreeViewRect;
    [NonSerialized]
    private Rect m_BottomBarRect;
    [NonSerialized]
    private Rect m_ListHeaderRect;

    private ProjectBrowser()
    {
    }

    public static List<ProjectBrowser> GetAllProjectBrowsers()
    {
      return ProjectBrowser.s_ProjectBrowsers;
    }

    private bool useTreeViewSelectionInsteadOfMainSelection
    {
      get
      {
        return this.m_UseTreeViewSelectionInsteadOfMainSelection;
      }
      set
      {
        this.m_UseTreeViewSelectionInsteadOfMainSelection = value;
      }
    }

    private void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
      ProjectBrowser.s_ProjectBrowsers.Add(this);
      EditorApplication.projectWindowChanged += new EditorApplication.CallbackFunction(this.OnProjectChanged);
      EditorApplication.pauseStateChanged += new Action<PauseState>(this.OnPauseStateChanged);
      EditorApplication.playModeStateChanged += new Action<PlayModeStateChange>(this.OnPlayModeStateChanged);
      EditorApplication.assetLabelsChanged += new EditorApplication.CallbackFunction(this.OnAssetLabelsChanged);
      EditorApplication.assetBundleNameChanged += new EditorApplication.CallbackFunction(this.OnAssetBundleNameChanged);
      AssemblyReloadEvents.afterAssemblyReload += new AssemblyReloadEvents.AssemblyReloadCallback(this.OnAfterAssemblyReload);
      ProjectBrowser.s_LastInteractedProjectBrowser = this;
    }

    private void OnAfterAssemblyReload()
    {
      this.Repaint();
    }

    private void OnDisable()
    {
      EditorApplication.pauseStateChanged -= new Action<PauseState>(this.OnPauseStateChanged);
      EditorApplication.playModeStateChanged -= new Action<PlayModeStateChange>(this.OnPlayModeStateChanged);
      EditorApplication.projectWindowChanged -= new EditorApplication.CallbackFunction(this.OnProjectChanged);
      EditorApplication.assetLabelsChanged -= new EditorApplication.CallbackFunction(this.OnAssetLabelsChanged);
      EditorApplication.assetBundleNameChanged -= new EditorApplication.CallbackFunction(this.OnAssetBundleNameChanged);
      AssemblyReloadEvents.afterAssemblyReload -= new AssemblyReloadEvents.AssemblyReloadCallback(this.OnAfterAssemblyReload);
      ProjectBrowser.s_ProjectBrowsers.Remove(this);
    }

    private void OnPauseStateChanged(PauseState state)
    {
      this.EndRenaming();
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
      this.EndRenaming();
    }

    private void OnAssetLabelsChanged()
    {
      if (!this.Initialized())
        return;
      this.SetupAssetLabelList();
      if (this.m_SearchFilter.IsSearching())
        this.InitListArea();
    }

    private void OnAssetBundleNameChanged()
    {
      if (this.m_ListArea == null)
        return;
      this.InitListArea();
    }

    private void Awake()
    {
      if (this.m_ListAreaState != null)
        this.m_ListAreaState.OnAwake();
      if (this.m_FolderTreeState != null)
      {
        this.m_FolderTreeState.OnAwake();
        this.m_FolderTreeState.expandedIDs = new List<int>((IEnumerable<int>) InternalEditorUtility.expandedProjectWindowItems);
      }
      if (this.m_AssetTreeState != null)
      {
        this.m_AssetTreeState.OnAwake();
        this.m_AssetTreeState.expandedIDs = new List<int>((IEnumerable<int>) InternalEditorUtility.expandedProjectWindowItems);
      }
      if (this.m_SearchFilter == null)
        return;
      this.EnsureValidFolders();
    }

    private string GetAnalyticsSizeLabel(float size)
    {
      if ((double) size > 600.0)
        return "Larger than 600 pix";
      return (double) size < 240.0 ? "Less than 240 pix" : "240 - 600 pix";
    }

    internal static ProjectBrowser.ItemType GetItemType(int instanceID)
    {
      return SavedSearchFilters.IsSavedFilter(instanceID) ? ProjectBrowser.ItemType.SavedFilter : ProjectBrowser.ItemType.Asset;
    }

    internal string GetActiveFolderPath()
    {
      if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns && this.m_SearchFilter.GetState() == SearchFilter.State.FolderBrowsing && this.m_SearchFilter.folders.Length > 0)
        return this.m_SearchFilter.folders[0];
      return "Assets";
    }

    private void EnsureValidFolders()
    {
      HashSet<string> source = new HashSet<string>();
      foreach (string folder in this.m_SearchFilter.folders)
      {
        if (AssetDatabase.IsValidFolder(folder))
        {
          source.Add(folder);
        }
        else
        {
          string path = folder;
          for (int index = 0; index < 30 && !string.IsNullOrEmpty(path); ++index)
          {
            path = ProjectWindowUtil.GetContainingFolder(path);
            if (!string.IsNullOrEmpty(path) && AssetDatabase.IsValidFolder(path))
            {
              source.Add(path);
              break;
            }
          }
        }
      }
      this.m_SearchFilter.folders = source.ToArray<string>();
    }

    private void OnProjectChanged()
    {
      if (this.m_AssetTree != null)
      {
        this.m_AssetTree.ReloadData();
        this.SetSearchFoldersFromCurrentSelection();
      }
      if (this.m_FolderTree != null)
      {
        this.m_FolderTree.ReloadData();
        this.SetSearchFolderFromFolderTreeSelection();
      }
      this.EnsureValidFolders();
      if (this.m_ListArea != null)
        this.InitListArea();
      this.RefreshSelectedPath();
      this.m_BreadCrumbs.Clear();
    }

    public bool Initialized()
    {
      return this.m_ListArea != null;
    }

    public void Init()
    {
      if (this.Initialized())
        return;
      this.m_FocusSearchField = false;
      if (this.m_SearchFilter == null)
        this.m_DirectoriesAreaWidth = Mathf.Min(this.position.width / 2f, 200f);
      if (this.m_SearchFilter == null)
        this.m_SearchFilter = new SearchFilter();
      this.m_SearchFieldText = this.m_SearchFilter.FilterToSearchFieldString();
      this.CalculateRects();
      this.RefreshSelectedPath();
      this.SetupDroplists();
      if (this.m_ListAreaState == null)
        this.m_ListAreaState = new ObjectListAreaState();
      this.m_ListAreaState.m_RenameOverlay.isRenamingFilename = true;
      this.m_ListArea = new ObjectListArea(this.m_ListAreaState, (EditorWindow) this, false);
      this.m_ListArea.allowDeselection = true;
      this.m_ListArea.allowDragging = true;
      this.m_ListArea.allowFocusRendering = true;
      this.m_ListArea.allowMultiSelect = true;
      this.m_ListArea.allowRenaming = true;
      this.m_ListArea.allowBuiltinResources = false;
      this.m_ListArea.allowUserRenderingHook = true;
      this.m_ListArea.allowFindNextShortcut = true;
      this.m_ListArea.foldersFirst = this.GetShouldShowFoldersFirst();
      this.m_ListArea.repaintCallback += new Action(((EditorWindow) this).Repaint);
      this.m_ListArea.itemSelectedCallback += new Action<bool>(this.ListAreaItemSelectedCallback);
      this.m_ListArea.keyboardCallback += new Action(this.ListAreaKeyboardCallback);
      this.m_ListArea.gotKeyboardFocus += new Action(this.ListGotKeyboardFocus);
      this.m_ListArea.drawLocalAssetHeader += new Func<Rect, float>(this.DrawLocalAssetHeader);
      this.m_ListArea.assetStoreSearchEnded += new Action(this.AssetStoreSearchEndedCallback);
      this.m_ListArea.gridSize = this.m_StartGridSize;
      this.m_StartGridSize = Mathf.Clamp(this.m_StartGridSize, this.m_ListArea.minGridSize, this.m_ListArea.maxGridSize);
      this.m_LastFoldersGridSize = Mathf.Min(this.m_LastFoldersGridSize, (float) this.m_ListArea.maxGridSize);
      this.m_SearchAreaMenu = new ExposablePopupMenu();
      if (this.m_FolderTreeState == null)
        this.m_FolderTreeState = new TreeViewStateWithAssetUtility();
      this.m_FolderTreeState.renameOverlay.isRenamingFilename = true;
      if (this.m_AssetTreeState == null)
        this.m_AssetTreeState = new TreeViewStateWithAssetUtility();
      this.m_AssetTreeState.renameOverlay.isRenamingFilename = true;
      this.InitViewMode(this.m_ViewMode);
      this.EnsureValidSetup();
      this.RefreshSearchText();
      this.SyncFilterGUI();
      this.InitListArea();
    }

    public void SetSearch(string searchString)
    {
      this.SetSearch(SearchFilter.CreateSearchFilterFromString(searchString));
    }

    public void SetSearch(SearchFilter searchFilter)
    {
      if (!this.Initialized())
        this.Init();
      this.m_SearchFilter = searchFilter;
      this.m_SearchFieldText = searchFilter.FilterToSearchFieldString();
      this.TopBarSearchSettingsChanged();
    }

    internal void RefreshSearchIfFilterContains(string searchString)
    {
      if (!this.Initialized() || !this.m_SearchFilter.IsSearching() || this.m_SearchFieldText.IndexOf(searchString) < 0)
        return;
      this.InitListArea();
    }

    private void SetSearchViewState(ProjectBrowser.SearchViewState state)
    {
      switch (state)
      {
        case ProjectBrowser.SearchViewState.NotSearching:
          Debug.LogError((object) "Invalid search mode as setter");
          break;
        case ProjectBrowser.SearchViewState.AllAssets:
          this.m_SearchFilter.searchArea = SearchFilter.SearchArea.AllAssets;
          break;
        case ProjectBrowser.SearchViewState.SubFolders:
          this.m_SearchFilter.searchArea = SearchFilter.SearchArea.SelectedFolders;
          break;
        case ProjectBrowser.SearchViewState.AssetStore:
          this.m_SearchFilter.searchArea = SearchFilter.SearchArea.AssetStore;
          break;
      }
      this.InitSearchMenu();
      this.InitListArea();
    }

    private ProjectBrowser.SearchViewState GetSearchViewState()
    {
      switch (this.m_SearchFilter.GetState())
      {
        case SearchFilter.State.SearchingInAllAssets:
          return ProjectBrowser.SearchViewState.AllAssets;
        case SearchFilter.State.SearchingInFolders:
          return ProjectBrowser.SearchViewState.SubFolders;
        case SearchFilter.State.SearchingInAssetStore:
          return ProjectBrowser.SearchViewState.AssetStore;
        default:
          return ProjectBrowser.SearchViewState.NotSearching;
      }
    }

    private void SearchButtonClickedCallback(ExposablePopupMenu.ItemData itemClicked)
    {
      if (itemClicked.m_On)
        return;
      this.SetSearchViewState((ProjectBrowser.SearchViewState) itemClicked.m_UserData);
      if (this.m_SearchFilter.searchArea == SearchFilter.SearchArea.AllAssets || this.m_SearchFilter.searchArea == SearchFilter.SearchArea.SelectedFolders)
        this.m_LastLocalAssetsSearchArea = this.m_SearchFilter.searchArea;
    }

    private void InitSearchMenu()
    {
      ProjectBrowser.SearchViewState searchViewState = this.GetSearchViewState();
      if (searchViewState == ProjectBrowser.SearchViewState.NotSearching)
        return;
      List<ExposablePopupMenu.ItemData> items = new List<ExposablePopupMenu.ItemData>();
      GUIStyle guiStyle1 = (GUIStyle) "ExposablePopupItem";
      GUIStyle guiStyle2 = (GUIStyle) "ExposablePopupItem";
      bool enabled = this.m_SearchFilter.folders.Length > 0;
      this.m_SearchAssetStore.text = this.m_ListArea.GetAssetStoreButtonText();
      bool on1 = searchViewState == ProjectBrowser.SearchViewState.AllAssets;
      items.Add(new ExposablePopupMenu.ItemData(this.m_SearchAllAssets, !on1 ? guiStyle2 : guiStyle1, on1, true, (object) 1));
      bool on2 = searchViewState == ProjectBrowser.SearchViewState.SubFolders;
      items.Add(new ExposablePopupMenu.ItemData(this.m_SearchInFolders, !on2 ? guiStyle2 : guiStyle1, on2, enabled, (object) 2));
      bool on3 = searchViewState == ProjectBrowser.SearchViewState.AssetStore;
      items.Add(new ExposablePopupMenu.ItemData(this.m_SearchAssetStore, !on3 ? guiStyle2 : guiStyle1, on3, true, (object) 3));
      GUIContent content = this.m_SearchAllAssets;
      switch (searchViewState)
      {
        case ProjectBrowser.SearchViewState.NotSearching:
        case ProjectBrowser.SearchViewState.AssetStore:
          content = this.m_SearchAssetStore;
          break;
        case ProjectBrowser.SearchViewState.AllAssets:
          content = this.m_SearchAllAssets;
          break;
        case ProjectBrowser.SearchViewState.SubFolders:
          content = this.m_SearchInFolders;
          break;
        default:
          Debug.LogError((object) "Unhandled enum");
          break;
      }
      ExposablePopupMenu.PopupButtonData popupButtonData = new ExposablePopupMenu.PopupButtonData(content, ProjectBrowser.s_Styles.exposablePopup);
      this.m_SearchAreaMenu.Init(items, 10f, 450f, popupButtonData, new Action<ExposablePopupMenu.ItemData>(this.SearchButtonClickedCallback));
    }

    private void AssetStoreSearchEndedCallback()
    {
      this.InitSearchMenu();
    }

    public static void ShowAssetStoreHitsWhileSearchingLocalAssetsChanged()
    {
      foreach (ProjectBrowser projectBrowser in ProjectBrowser.s_ProjectBrowsers)
      {
        projectBrowser.m_ListArea.ShowAssetStoreHitCountWhileSearchingLocalAssetsChanged();
        projectBrowser.InitSearchMenu();
      }
    }

    private void RefreshSearchText()
    {
      if (this.m_SearchFilter.folders.Length > 0)
      {
        string[] baseFolders = ProjectWindowUtil.GetBaseFolders(this.m_SearchFilter.folders);
        string str1 = "";
        string str2 = "";
        int num = 3;
        for (int index = 0; index < baseFolders.Length && index < num; ++index)
        {
          if (index > 0)
            str1 += ", ";
          string withoutExtension = Path.GetFileNameWithoutExtension(baseFolders[index]);
          str1 = str1 + "'" + withoutExtension + "'";
          if (index == 0 && withoutExtension != "Assets")
            str2 = baseFolders[index];
        }
        if (baseFolders.Length > num)
          str1 += " +";
        this.m_SearchInFolders.text = str1;
        this.m_SearchInFolders.tooltip = str2;
      }
      else
      {
        this.m_SearchInFolders.text = "Selected folder";
        this.m_SearchInFolders.tooltip = "";
      }
      this.m_BreadCrumbs.Clear();
      this.InitSearchMenu();
    }

    private void EnsureValidSetup()
    {
      if (this.m_LastProjectPath != Directory.GetCurrentDirectory())
      {
        this.m_SearchFilter = new SearchFilter();
        this.m_LastFolders = new string[0];
        this.SyncFilterGUI();
        if (Selection.activeInstanceID != 0)
          this.FrameObjectPrivate(Selection.activeInstanceID, !this.m_IsLocked, false);
        if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns && !this.IsShowingFolderContents())
          this.SelectAssetsFolder();
        this.m_LastProjectPath = Directory.GetCurrentDirectory();
      }
      if (this.m_ViewMode != ProjectBrowser.ViewMode.TwoColumns || this.m_SearchFilter.GetState() != SearchFilter.State.EmptySearchFilter)
        return;
      this.SelectAssetsFolder();
    }

    private void OnGUIAssetCallback(int instanceID, Rect rect)
    {
      if (EditorApplication.projectWindowItemOnGUI == null)
        return;
      string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(instanceID));
      EditorApplication.projectWindowItemOnGUI(guid, rect);
    }

    private void InitViewMode(ProjectBrowser.ViewMode viewMode)
    {
      this.m_ViewMode = viewMode;
      this.m_FolderTree = (TreeViewController) null;
      this.m_AssetTree = (TreeViewController) null;
      this.useTreeViewSelectionInsteadOfMainSelection = false;
      if (this.m_ViewMode == ProjectBrowser.ViewMode.OneColumn)
      {
        this.m_AssetTree = new TreeViewController((EditorWindow) this, (TreeViewState) this.m_AssetTreeState);
        this.m_AssetTree.deselectOnUnhandledMouseDown = true;
        this.m_AssetTree.selectionChangedCallback += new Action<int[]>(this.AssetTreeSelectionCallback);
        this.m_AssetTree.keyboardInputCallback += new Action(this.AssetTreeKeyboardInputCallback);
        this.m_AssetTree.contextClickItemCallback += new Action<int>(this.AssetTreeViewContextClick);
        this.m_AssetTree.contextClickOutsideItemsCallback += new Action(this.AssetTreeViewContextClickOutsideItems);
        this.m_AssetTree.itemDoubleClickedCallback += new Action<int>(this.AssetTreeItemDoubleClickedCallback);
        this.m_AssetTree.onGUIRowCallback += new Action<int, Rect>(this.OnGUIAssetCallback);
        this.m_AssetTree.dragEndedCallback += new Action<int[], bool>(this.AssetTreeDragEnded);
        this.m_AssetTree.Init(this.m_TreeViewRect, (ITreeViewDataSource) new AssetsTreeViewDataSource(this.m_AssetTree, AssetDatabase.GetInstanceIDFromGUID(AssetDatabase.AssetPathToGUID("Assets")), false, false)
        {
          foldersFirst = this.GetShouldShowFoldersFirst()
        }, (ITreeViewGUI) new AssetsTreeViewGUI(this.m_AssetTree), (ITreeViewDragging) new AssetsTreeViewDragging(this.m_AssetTree));
        this.m_AssetTree.ReloadData();
      }
      else if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns)
      {
        this.m_FolderTree = new TreeViewController((EditorWindow) this, (TreeViewState) this.m_FolderTreeState);
        this.m_FolderTree.deselectOnUnhandledMouseDown = false;
        this.m_FolderTree.selectionChangedCallback += new Action<int[]>(this.FolderTreeSelectionCallback);
        this.m_FolderTree.contextClickItemCallback += new Action<int>(this.FolderTreeViewContextClick);
        this.m_FolderTree.onGUIRowCallback += new Action<int, Rect>(this.OnGUIAssetCallback);
        this.m_FolderTree.dragEndedCallback += new Action<int[], bool>(this.FolderTreeDragEnded);
        this.m_FolderTree.Init(this.m_TreeViewRect, (ITreeViewDataSource) new ProjectBrowserColumnOneTreeViewDataSource(this.m_FolderTree), (ITreeViewGUI) new ProjectBrowserColumnOneTreeViewGUI(this.m_FolderTree), (ITreeViewDragging) new ProjectBrowserColumnOneTreeViewDragging(this.m_FolderTree));
        this.m_FolderTree.ReloadData();
      }
      this.minSize = new Vector2(this.m_ViewMode != ProjectBrowser.ViewMode.OneColumn ? 230f : 230f, 250f);
      this.maxSize = new Vector2(10000f, 10000f);
    }

    private bool GetShouldShowFoldersFirst()
    {
      return Application.platform != RuntimePlatform.OSXEditor;
    }

    private void SetViewMode(ProjectBrowser.ViewMode newViewMode)
    {
      if (this.m_ViewMode == newViewMode)
        return;
      this.EndRenaming();
      this.InitViewMode(this.m_ViewMode != ProjectBrowser.ViewMode.OneColumn ? ProjectBrowser.ViewMode.OneColumn : ProjectBrowser.ViewMode.TwoColumns);
      if (Selection.activeInstanceID != 0)
        this.FrameObjectPrivate(Selection.activeInstanceID, !this.m_IsLocked, false);
      this.RepaintImmediately();
    }

    private void EndRenaming()
    {
      if (this.m_AssetTree != null)
        this.m_AssetTree.EndNameEditing(true);
      if (this.m_FolderTree != null)
        this.m_FolderTree.EndNameEditing(true);
      if (this.m_ListArea == null)
        return;
      this.m_ListArea.EndRename(true);
    }

    private string[] GetTypesDisplayNames()
    {
      return new string[16]{ "AnimationClip", "AudioClip", "AudioMixer", "Font", "GUISkin", "Material", "Mesh", "Model", "PhysicMaterial", "Prefab", "Scene", "Script", "Shader", "Sprite", "Texture", "VideoClip" };
    }

    public void TypeListCallback(PopupList.ListElement element)
    {
      if (!Event.current.control)
      {
        foreach (PopupList.ListElement listElement in this.m_ObjectTypes.m_ListElements)
        {
          if (listElement != element)
            listElement.selected = false;
        }
      }
      element.selected = !element.selected;
      this.m_SearchFilter.classNames = this.m_ObjectTypes.m_ListElements.Where<PopupList.ListElement>((Func<PopupList.ListElement, bool>) (item => item.selected)).Select<PopupList.ListElement, string>((Func<PopupList.ListElement, string>) (item => item.text)).ToArray<string>();
      this.m_SearchFieldText = this.m_SearchFilter.FilterToSearchFieldString();
      this.TopBarSearchSettingsChanged();
      this.Repaint();
    }

    public void AssetLabelListCallback(PopupList.ListElement element)
    {
      if (!Event.current.control)
      {
        foreach (PopupList.ListElement listElement in this.m_AssetLabels.m_ListElements)
        {
          if (listElement != element)
            listElement.selected = false;
        }
      }
      element.selected = !element.selected;
      this.m_SearchFilter.assetLabels = this.m_AssetLabels.m_ListElements.Where<PopupList.ListElement>((Func<PopupList.ListElement, bool>) (item => item.selected)).Select<PopupList.ListElement, string>((Func<PopupList.ListElement, string>) (item => item.text)).ToArray<string>();
      this.m_SearchFieldText = this.m_SearchFilter.FilterToSearchFieldString();
      this.TopBarSearchSettingsChanged();
      this.Repaint();
    }

    private void SetupDroplists()
    {
      this.SetupAssetLabelList();
      this.m_ObjectTypes = new PopupList.InputData();
      this.m_ObjectTypes.m_CloseOnSelection = false;
      this.m_ObjectTypes.m_AllowCustom = false;
      this.m_ObjectTypes.m_OnSelectCallback = new PopupList.OnSelectCallback(this.TypeListCallback);
      this.m_ObjectTypes.m_SortAlphabetically = false;
      this.m_ObjectTypes.m_MaxCount = 0;
      string[] typesDisplayNames = this.GetTypesDisplayNames();
      for (int index = 0; index < typesDisplayNames.Length; ++index)
      {
        PopupList.ListElement listElement = this.m_ObjectTypes.NewOrMatchingElement(typesDisplayNames[index]);
        if (index == 0)
          listElement.selected = true;
      }
    }

    private void SetupAssetLabelList()
    {
      Dictionary<string, float> allLabels = AssetDatabase.GetAllLabels();
      this.m_AssetLabels = new PopupList.InputData();
      this.m_AssetLabels.m_CloseOnSelection = false;
      this.m_AssetLabels.m_AllowCustom = true;
      this.m_AssetLabels.m_OnSelectCallback = new PopupList.OnSelectCallback(this.AssetLabelListCallback);
      this.m_AssetLabels.m_MaxCount = 15;
      this.m_AssetLabels.m_SortAlphabetically = true;
      foreach (KeyValuePair<string, float> keyValuePair in allLabels)
      {
        PopupList.ListElement listElement = this.m_AssetLabels.NewOrMatchingElement(keyValuePair.Key);
        if ((double) listElement.filterScore < (double) keyValuePair.Value)
          listElement.filterScore = keyValuePair.Value;
      }
    }

    private void SyncFilterGUI()
    {
      List<string> stringList1 = new List<string>((IEnumerable<string>) this.m_SearchFilter.assetLabels);
      foreach (PopupList.ListElement listElement in this.m_AssetLabels.m_ListElements)
        listElement.selected = stringList1.Contains(listElement.text);
      List<string> stringList2 = new List<string>((IEnumerable<string>) this.m_SearchFilter.classNames);
      foreach (PopupList.ListElement listElement in this.m_ObjectTypes.m_ListElements)
        listElement.selected = stringList2.Contains(listElement.text);
      this.m_SearchFieldText = this.m_SearchFilter.FilterToSearchFieldString();
    }

    private static int GetParentInstanceID(int objectInstanceID)
    {
      string assetPath = AssetDatabase.GetAssetPath(objectInstanceID);
      int length = assetPath.LastIndexOf("/");
      if (length >= 0)
      {
        UnityEngine.Object @object = AssetDatabase.LoadAssetAtPath(assetPath.Substring(0, length), typeof (UnityEngine.Object));
        if (@object != (UnityEngine.Object) null)
          return @object.GetInstanceID();
      }
      else
        Debug.LogError((object) ("Invalid path: " + assetPath));
      return -1;
    }

    private bool IsShowingFolder(int folderInstanceID)
    {
      return new List<string>((IEnumerable<string>) this.m_SearchFilter.folders).Contains(AssetDatabase.GetAssetPath(folderInstanceID));
    }

    private void ShowFolderContents(int folderInstanceID, bool revealAndFrameInFolderTree)
    {
      if (this.m_ViewMode != ProjectBrowser.ViewMode.TwoColumns)
        Debug.LogError((object) "ShowFolderContents should only be called in two column mode");
      if (folderInstanceID == 0)
        return;
      string assetPath = AssetDatabase.GetAssetPath(folderInstanceID);
      this.m_SearchFilter.ClearSearch();
      this.m_SearchFilter.folders = new string[1]
      {
        assetPath
      };
      this.m_FolderTree.SetSelection(new int[1]
      {
        folderInstanceID
      }, (revealAndFrameInFolderTree ? 1 : 0) != 0);
      this.FolderTreeSelectionChanged(true);
    }

    private bool IsShowingFolderContents()
    {
      return this.m_SearchFilter.folders.Length > 0;
    }

    private void ListGotKeyboardFocus()
    {
    }

    private void ListAreaKeyboardCallback()
    {
      if (Event.current.type != EventType.KeyDown)
        return;
      KeyCode keyCode = Event.current.keyCode;
      switch (keyCode)
      {
        case KeyCode.KeypadEnter:
          if (Application.platform == RuntimePlatform.OSXEditor)
          {
            if (this.m_ListArea.BeginRename(0.0f))
            {
              Event.current.Use();
              break;
            }
            break;
          }
          Event.current.Use();
          this.OpenListAreaSelection();
          break;
        case KeyCode.UpArrow:
          if (Application.platform == RuntimePlatform.OSXEditor && Event.current.command && this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns)
          {
            this.ShowParentFolderOfCurrentlySelected();
            Event.current.Use();
            break;
          }
          break;
        case KeyCode.DownArrow:
          if (Application.platform == RuntimePlatform.OSXEditor && Event.current.command)
          {
            Event.current.Use();
            this.OpenListAreaSelection();
            break;
          }
          break;
        default:
          if (keyCode != KeyCode.Backspace)
          {
            if (keyCode != KeyCode.Return)
            {
              if (keyCode == KeyCode.F2 && Application.platform != RuntimePlatform.OSXEditor && this.m_ListArea.BeginRename(0.0f))
              {
                Event.current.Use();
                break;
              }
              break;
            }
            goto case KeyCode.KeypadEnter;
          }
          else
          {
            if (Application.platform != RuntimePlatform.OSXEditor && this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns)
            {
              this.ShowParentFolderOfCurrentlySelected();
              Event.current.Use();
              break;
            }
            break;
          }
      }
    }

    private void ShowParentFolderOfCurrentlySelected()
    {
      if (!this.IsShowingFolderContents())
        return;
      int[] selection = this.m_FolderTree.GetSelection();
      if (selection.Length == 1)
      {
        TreeViewItem treeViewItem = this.m_FolderTree.FindItem(selection[0]);
        if (treeViewItem != null && treeViewItem.parent != null && treeViewItem.id != ProjectBrowserColumnOneTreeViewDataSource.GetAssetsFolderInstanceID())
        {
          this.SetFolderSelection(new int[1]
          {
            treeViewItem.parent.id
          }, true);
          this.m_ListArea.Frame(treeViewItem.id, true, false);
          Selection.activeInstanceID = treeViewItem.id;
        }
      }
    }

    private void OpenListAreaSelection()
    {
      int[] selection = this.m_ListArea.GetSelection();
      int length = selection.Length;
      if (length <= 0)
        return;
      int num = 0;
      foreach (int instanceID in selection)
      {
        if (ProjectWindowUtil.IsFolder(instanceID))
          ++num;
      }
      if (num == length)
      {
        if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns)
          this.SetFolderSelection(selection, false);
        else if (this.m_ViewMode == ProjectBrowser.ViewMode.OneColumn)
        {
          this.ClearSearch();
          this.m_AssetTree.Frame(selection[0], true, false);
        }
        this.Repaint();
        GUIUtility.ExitGUI();
      }
      else
      {
        ProjectBrowser.OpenAssetSelection(selection);
        this.Repaint();
        GUIUtility.ExitGUI();
      }
    }

    private static void OpenAssetSelection(int[] selectedInstanceIDs)
    {
      foreach (int selectedInstanceId in selectedInstanceIDs)
      {
        if (AssetDatabase.Contains(selectedInstanceId))
          AssetDatabase.OpenAsset(selectedInstanceId);
      }
      GUIUtility.ExitGUI();
    }

    private void SetAsLastInteractedProjectBrowser()
    {
      ProjectBrowser.s_LastInteractedProjectBrowser = this;
    }

    private void RefreshSelectedPath()
    {
      this.m_SelectedPath = !(Selection.activeObject != (UnityEngine.Object) null) ? "" : AssetDatabase.GetAssetPath(Selection.activeObject);
      this.m_SelectedPathSplitted.Clear();
    }

    private void ListAreaItemSelectedCallback(bool doubleClicked)
    {
      this.SetAsLastInteractedProjectBrowser();
      Selection.activeObject = (UnityEngine.Object) null;
      int[] selection = this.m_ListArea.GetSelection();
      if (selection.Length > 0)
      {
        Selection.instanceIDs = selection;
        this.m_SearchFilter.searchArea = this.m_LastLocalAssetsSearchArea;
        this.m_InternalSelectionChange = true;
      }
      else if (AssetStoreAssetSelection.Count > 0)
        Selection.activeObject = (UnityEngine.Object) AssetStoreAssetInspector.Instance;
      this.m_FocusSearchField = false;
      if (Event.current.button == 1 && Event.current.type == EventType.MouseDown)
        this.m_ItemSelectedByRightClickThisEvent = true;
      this.RefreshSelectedPath();
      this.m_DidSelectSearchResult = this.m_SearchFilter.IsSearching();
      if (!doubleClicked)
        return;
      this.OpenListAreaSelection();
    }

    private void OnGotFocus()
    {
    }

    private void OnLostFocus()
    {
      this.EndRenaming();
    }

    private bool ShouldFrameAsset(int instanceID)
    {
      return new HierarchyProperty(HierarchyType.Assets).Find(instanceID, (int[]) null);
    }

    private void OnSelectionChange()
    {
      if (this.m_ListArea == null)
        return;
      this.m_ListArea.InitSelection(Selection.instanceIDs);
      int instanceID = Selection.instanceIDs.Length <= 0 ? 0 : Selection.instanceIDs[Selection.instanceIDs.Length - 1];
      if (this.m_ViewMode == ProjectBrowser.ViewMode.OneColumn)
        this.m_AssetTree.SetSelection(Selection.instanceIDs, !this.m_IsLocked && this.ShouldFrameAsset(instanceID));
      else if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns && !this.m_InternalSelectionChange && (!this.m_IsLocked && Selection.instanceIDs.Length > 0 && this.ShouldFrameAsset(instanceID)))
      {
        if (this.m_SearchFilter.IsSearching())
          this.m_ListArea.Frame(instanceID, true, false);
        else
          this.FrameObjectInTwoColumnMode(instanceID, true, false);
      }
      this.m_InternalSelectionChange = false;
      if (Selection.activeObject != (UnityEngine.Object) null && Selection.activeObject.GetType() != typeof (AssetStoreAssetInspector))
      {
        this.m_ListArea.selectedAssetStoreAsset = false;
        AssetStoreAssetSelection.Clear();
      }
      this.RefreshSelectedPath();
      this.Repaint();
    }

    private void SetFoldersInSearchFilter(int[] selectedInstanceIDs)
    {
      this.m_SearchFilter.folders = ProjectBrowser.GetFolderPathsFromInstanceIDs(selectedInstanceIDs);
      this.EnsureValidFolders();
      if (selectedInstanceIDs.Length <= 0 || (double) this.m_LastFoldersGridSize <= 0.0)
        return;
      this.m_ListArea.gridSize = (int) this.m_LastFoldersGridSize;
    }

    internal void SetFolderSelection(int[] selectedInstanceIDs, bool revealSelectionAndFrameLastSelected)
    {
      this.m_FolderTree.SetSelection(selectedInstanceIDs, revealSelectionAndFrameLastSelected);
      this.SetFoldersInSearchFilter(selectedInstanceIDs);
      this.FolderTreeSelectionChanged(true);
    }

    private void AssetTreeItemDoubleClickedCallback(int instanceID)
    {
      ProjectBrowser.OpenAssetSelection(Selection.instanceIDs);
    }

    private void AssetTreeKeyboardInputCallback()
    {
      if (Event.current.type != EventType.KeyDown)
        return;
      switch (Event.current.keyCode)
      {
        case KeyCode.Return:
        case KeyCode.KeypadEnter:
          if (Application.platform == RuntimePlatform.WindowsEditor)
          {
            Event.current.Use();
            ProjectBrowser.OpenAssetSelection(Selection.instanceIDs);
            break;
          }
          break;
        case KeyCode.DownArrow:
          if (Application.platform == RuntimePlatform.OSXEditor && Event.current.command)
          {
            Event.current.Use();
            ProjectBrowser.OpenAssetSelection(Selection.instanceIDs);
            break;
          }
          break;
      }
    }

    private void AssetTreeSelectionCallback(int[] selectedTreeViewInstanceIDs)
    {
      this.SetAsLastInteractedProjectBrowser();
      Selection.activeObject = (UnityEngine.Object) null;
      if (selectedTreeViewInstanceIDs.Length > 0)
        Selection.instanceIDs = selectedTreeViewInstanceIDs;
      this.RefreshSelectedPath();
      this.SetSearchFoldersFromCurrentSelection();
      this.RefreshSearchText();
    }

    private void SetSearchFoldersFromCurrentSelection()
    {
      HashSet<string> source = new HashSet<string>();
      foreach (int instanceId in Selection.instanceIDs)
      {
        if (AssetDatabase.Contains(instanceId))
        {
          string assetPath = AssetDatabase.GetAssetPath(instanceId);
          if (AssetDatabase.IsValidFolder(assetPath))
          {
            source.Add(assetPath);
          }
          else
          {
            string containingFolder = ProjectWindowUtil.GetContainingFolder(assetPath);
            if (!string.IsNullOrEmpty(containingFolder))
              source.Add(containingFolder);
          }
        }
      }
      this.m_SearchFilter.folders = ProjectWindowUtil.GetBaseFolders(source.ToArray<string>());
    }

    private void SetSearchFolderFromFolderTreeSelection()
    {
      if (this.m_FolderTree == null)
        return;
      this.m_SearchFilter.folders = ProjectBrowser.GetFolderPathsFromInstanceIDs(this.m_FolderTree.GetSelection());
    }

    private void FolderTreeSelectionCallback(int[] selectedTreeViewInstanceIDs)
    {
      this.SetAsLastInteractedProjectBrowser();
      int num = 0;
      if (selectedTreeViewInstanceIDs.Length > 0)
        num = selectedTreeViewInstanceIDs[0];
      bool folderWasSelected = false;
      if (num != 0)
      {
        ProjectBrowser.ItemType itemType = ProjectBrowser.GetItemType(num);
        if (itemType == ProjectBrowser.ItemType.Asset)
        {
          this.SetFoldersInSearchFilter(selectedTreeViewInstanceIDs);
          folderWasSelected = true;
        }
        if (itemType == ProjectBrowser.ItemType.SavedFilter)
        {
          SearchFilter filter = SavedSearchFilters.GetFilter(num);
          if (this.ValidateFilter(num, filter))
          {
            this.m_SearchFilter = filter;
            this.EnsureValidFolders();
            float previewSize = SavedSearchFilters.GetPreviewSize(num);
            if ((double) previewSize > 0.0)
              this.m_ListArea.gridSize = Mathf.Clamp((int) previewSize, this.m_ListArea.minGridSize, this.m_ListArea.maxGridSize);
            this.SyncFilterGUI();
          }
        }
      }
      this.FolderTreeSelectionChanged(folderWasSelected);
    }

    private bool ValidateFilter(int savedFilterID, SearchFilter filter)
    {
      if (filter == null)
        return false;
      switch (filter.GetState())
      {
        case SearchFilter.State.FolderBrowsing:
        case SearchFilter.State.SearchingInFolders:
          foreach (string folder in filter.folders)
          {
            if (AssetDatabase.GetMainAssetInstanceID(folder) == 0)
            {
              if (EditorUtility.DisplayDialog("Folder not found", "The folder '" + folder + "' might have been deleted or belong to another project. Do you want to delete the favorite?", "Delete", "Cancel"))
                SavedSearchFilters.RemoveSavedFilter(savedFilterID);
              return false;
            }
          }
          break;
      }
      return true;
    }

    private void ShowAndHideFolderTreeSelectionAsNeeded()
    {
      if (this.m_ViewMode != ProjectBrowser.ViewMode.TwoColumns || this.m_FolderTree == null)
        return;
      bool flag = false;
      int[] selection = this.m_FolderTree.GetSelection();
      if (selection.Length > 0)
        flag = ProjectBrowser.GetItemType(selection[0]) == ProjectBrowser.ItemType.SavedFilter;
      switch (this.GetSearchViewState())
      {
        case ProjectBrowser.SearchViewState.NotSearching:
        case ProjectBrowser.SearchViewState.SubFolders:
          if (!flag)
          {
            this.m_FolderTree.SetSelection(ProjectBrowser.GetFolderInstanceIDs(this.m_SearchFilter.folders), true);
            break;
          }
          break;
        case ProjectBrowser.SearchViewState.AllAssets:
        case ProjectBrowser.SearchViewState.AssetStore:
          if (!flag)
          {
            this.m_FolderTree.SetSelection(new int[0], false);
            break;
          }
          break;
      }
    }

    public string[] GetCurrentVisibleNames()
    {
      return this.m_ListArea.GetCurrentVisibleNames();
    }

    private void InitListArea()
    {
      this.ShowAndHideFolderTreeSelectionAsNeeded();
      HierarchyType hierarchyType = HierarchyType.Assets;
      string[] folders = this.m_SearchFilter.folders;
      // ISSUE: reference to a compiler-generated field
      if (ProjectBrowser.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ProjectBrowser.\u003C\u003Ef__mg\u0024cache0 = new Func<string, bool>(AssetDatabase.IsPackagedAssetPath);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, bool> fMgCache0 = ProjectBrowser.\u003C\u003Ef__mg\u0024cache0;
      if (((IEnumerable<string>) folders).Any<string>(fMgCache0))
        hierarchyType = HierarchyType.Packages;
      this.m_ListArea.Init(this.m_ListAreaRect, hierarchyType, this.m_SearchFilter, false);
      this.m_ListArea.InitSelection(Selection.instanceIDs);
    }

    private void OnInspectorUpdate()
    {
      if (this.m_ListArea == null)
        return;
      this.m_ListArea.OnInspectorUpdate();
    }

    private void OnDestroy()
    {
      if (this.m_ListArea != null)
        this.m_ListArea.OnDestroy();
      if (!((UnityEngine.Object) this == (UnityEngine.Object) ProjectBrowser.s_LastInteractedProjectBrowser))
        return;
      ProjectBrowser.s_LastInteractedProjectBrowser = (ProjectBrowser) null;
    }

    internal static int[] DuplicateFolders(int[] instanceIDs)
    {
      AssetDatabase.Refresh();
      List<string> stringList = new List<string>();
      bool flag = false;
      int folderInstanceId = ProjectBrowserColumnOneTreeViewDataSource.GetAssetsFolderInstanceID();
      foreach (int instanceId in instanceIDs)
      {
        if (instanceId != folderInstanceId)
        {
          string assetPath = AssetDatabase.GetAssetPath(InternalEditorUtility.GetObjectFromInstanceID(instanceId));
          string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
          if (uniqueAssetPath.Length != 0)
            flag |= !AssetDatabase.CopyAsset(assetPath, uniqueAssetPath);
          else
            flag = ((flag ? 1 : 0) | 1) != 0;
          if (!flag)
            stringList.Add(uniqueAssetPath);
        }
      }
      AssetDatabase.Refresh();
      int[] numArray = new int[stringList.Count];
      for (int index = 0; index < stringList.Count; ++index)
        numArray[index] = AssetDatabase.LoadMainAssetAtPath(stringList[index]).GetInstanceID();
      return numArray;
    }

    private static void DeleteFilter(int filterInstanceID)
    {
      if (SavedSearchFilters.GetRootInstanceID() == filterInstanceID)
        EditorUtility.DisplayDialog("Cannot Delete", "Deleting the 'Filters' root is not allowed", "Ok");
      else if (EditorUtility.DisplayDialog("Delete selected favorite?", "You cannot undo this action.", "Delete", "Cancel"))
        SavedSearchFilters.RemoveSavedFilter(filterInstanceID);
    }

    private bool HandleCommandEventsForTreeView()
    {
      EventType type = Event.current.type;
      switch (type)
      {
        case EventType.ValidateCommand:
        case EventType.ExecuteCommand:
          bool flag = type == EventType.ExecuteCommand;
          int[] selection = this.m_FolderTree.GetSelection();
          if (selection.Length == 0)
            return false;
          ProjectBrowser.ItemType itemType = ProjectBrowser.GetItemType(selection[0]);
          if (Event.current.commandName == "Delete" || Event.current.commandName == "SoftDelete")
          {
            Event.current.Use();
            if (flag)
            {
              if (itemType == ProjectBrowser.ItemType.SavedFilter)
              {
                ProjectBrowser.DeleteFilter(selection[0]);
                GUIUtility.ExitGUI();
              }
              else if (itemType == ProjectBrowser.ItemType.Asset)
              {
                bool askIfSure = Event.current.commandName == "SoftDelete";
                ProjectBrowser.DeleteSelectedAssets(askIfSure);
                if (askIfSure)
                  this.Focus();
              }
            }
            GUIUtility.ExitGUI();
          }
          else if (Event.current.commandName == "Duplicate")
          {
            if (flag)
            {
              if (itemType != ProjectBrowser.ItemType.SavedFilter && itemType == ProjectBrowser.ItemType.Asset)
              {
                Event.current.Use();
                this.SetFolderSelection(ProjectBrowser.DuplicateFolders(selection), true);
                GUIUtility.ExitGUI();
              }
            }
            else
              Event.current.Use();
          }
          break;
      }
      return false;
    }

    private bool HandleCommandEvents()
    {
      EventType type = Event.current.type;
      switch (type)
      {
        case EventType.ValidateCommand:
        case EventType.ExecuteCommand:
          bool flag = type == EventType.ExecuteCommand;
          if (Event.current.commandName == "Delete" || Event.current.commandName == "SoftDelete")
          {
            Event.current.Use();
            if (flag)
            {
              bool askIfSure = Event.current.commandName == "SoftDelete";
              ProjectBrowser.DeleteSelectedAssets(askIfSure);
              if (askIfSure)
                this.Focus();
            }
            GUIUtility.ExitGUI();
          }
          else if (Event.current.commandName == "Duplicate")
          {
            if (flag)
            {
              Event.current.Use();
              ProjectWindowUtil.DuplicateSelectedAssets();
              GUIUtility.ExitGUI();
            }
            else if (Selection.GetFiltered(typeof (UnityEngine.Object), SelectionMode.Assets).Length != 0)
              Event.current.Use();
          }
          else if (Event.current.commandName == "FocusProjectWindow")
          {
            if (flag)
            {
              this.FrameObjectPrivate(Selection.activeInstanceID, true, false);
              Event.current.Use();
              this.Focus();
              GUIUtility.ExitGUI();
            }
            else
              Event.current.Use();
          }
          else if (Event.current.commandName == "SelectAll")
          {
            if (flag)
              this.SelectAll();
            Event.current.Use();
          }
          else if (Event.current.commandName == "FrameSelected")
          {
            if (flag)
            {
              this.FrameObjectPrivate(Selection.activeInstanceID, true, false);
              Event.current.Use();
              GUIUtility.ExitGUI();
            }
            Event.current.Use();
          }
          else if (Event.current.commandName == "Find")
          {
            if (flag)
              this.m_FocusSearchField = true;
            Event.current.Use();
          }
          break;
      }
      return false;
    }

    private void SelectAll()
    {
      if (this.m_ViewMode == ProjectBrowser.ViewMode.OneColumn)
      {
        if (this.m_SearchFilter.IsSearching())
        {
          this.m_ListArea.SelectAll();
        }
        else
        {
          int[] rowIds = this.m_AssetTree.GetRowIDs();
          this.m_AssetTree.SetSelection(rowIds, false);
          this.AssetTreeSelectionCallback(rowIds);
        }
      }
      else if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns)
        this.m_ListArea.SelectAll();
      else
        Debug.LogError((object) ("Missing implementation for ViewMode " + (object) this.m_ViewMode));
    }

    private void RefreshSplittedSelectedPath()
    {
      if (ProjectBrowser.s_Styles == null)
        ProjectBrowser.s_Styles = new ProjectBrowser.Styles();
      this.m_SelectedPathSplitted.Clear();
      if (string.IsNullOrEmpty(this.m_SelectedPath))
      {
        this.m_SelectedPathSplitted.Add(new GUIContent());
      }
      else
      {
        string str1 = this.m_SelectedPath;
        if (this.m_SelectedPath.StartsWith("assets/", StringComparison.CurrentCultureIgnoreCase))
          str1 = this.m_SelectedPath.Substring("assets/".Length);
        if (this.m_SearchFilter.GetState() == SearchFilter.State.FolderBrowsing)
        {
          this.m_SelectedPathSplitted.Add(new GUIContent(Path.GetFileName(this.m_SelectedPath), AssetDatabase.GetCachedIcon(this.m_SelectedPath)));
        }
        else
        {
          float num = (float) ((double) this.position.width - (double) this.m_DirectoriesAreaWidth - 55.0 - 16.0);
          if ((double) ProjectBrowser.s_Styles.selectedPathLabel.CalcSize(GUIContent.Temp(str1)).x + 25.0 > (double) num)
          {
            string[] strArray = str1.Split('/');
            string str2 = "Assets/";
            for (int index = 0; index < strArray.Length; ++index)
            {
              string path = str2 + strArray[index];
              Texture cachedIcon = AssetDatabase.GetCachedIcon(path);
              this.m_SelectedPathSplitted.Add(new GUIContent(strArray[index], cachedIcon));
              str2 = path + "/";
            }
          }
          else
            this.m_SelectedPathSplitted.Add(new GUIContent(str1, AssetDatabase.GetCachedIcon(this.m_SelectedPath)));
        }
      }
    }

    private float GetBottomBarHeight()
    {
      if (this.m_SelectedPathSplitted.Count == 0)
        this.RefreshSplittedSelectedPath();
      if (this.m_ViewMode == ProjectBrowser.ViewMode.OneColumn && !this.m_SearchFilter.IsSearching())
        return 0.0f;
      return 17f * (float) this.m_SelectedPathSplitted.Count;
    }

    private float GetListHeaderHeight()
    {
      return this.m_SearchFilter.GetState() != SearchFilter.State.EmptySearchFilter ? 18f : 0.0f;
    }

    private void CalculateRects()
    {
      float bottomBarHeight = this.GetBottomBarHeight();
      float listHeaderHeight = this.GetListHeaderHeight();
      if (this.m_ViewMode == ProjectBrowser.ViewMode.OneColumn)
      {
        this.m_ListAreaRect = new Rect(0.0f, this.m_ToolbarHeight + listHeaderHeight, this.position.width, this.position.height - this.m_ToolbarHeight - listHeaderHeight - bottomBarHeight);
        this.m_TreeViewRect = new Rect(0.0f, this.m_ToolbarHeight, this.position.width, this.position.height - this.m_ToolbarHeight - bottomBarHeight);
        this.m_BottomBarRect = new Rect(0.0f, this.position.height - bottomBarHeight, this.position.width, bottomBarHeight);
        this.m_ListHeaderRect = new Rect(0.0f, this.m_ToolbarHeight, this.position.width, listHeaderHeight);
      }
      else
      {
        float width = this.position.width - this.m_DirectoriesAreaWidth;
        this.m_ListAreaRect = new Rect(this.m_DirectoriesAreaWidth, this.m_ToolbarHeight + listHeaderHeight, width, this.position.height - this.m_ToolbarHeight - listHeaderHeight - bottomBarHeight);
        this.m_TreeViewRect = new Rect(0.0f, this.m_ToolbarHeight, this.m_DirectoriesAreaWidth, this.position.height - this.m_ToolbarHeight);
        this.m_BottomBarRect = new Rect(this.m_DirectoriesAreaWidth, this.position.height - bottomBarHeight, width, bottomBarHeight);
        this.m_ListHeaderRect = new Rect(this.m_ListAreaRect.x, this.m_ToolbarHeight, this.m_ListAreaRect.width, listHeaderHeight);
      }
    }

    private void EndPing()
    {
      if (this.m_ViewMode == ProjectBrowser.ViewMode.OneColumn)
      {
        this.m_AssetTree.EndPing();
      }
      else
      {
        this.m_FolderTree.EndPing();
        this.m_ListArea.EndPing();
      }
    }

    private void OnEvent()
    {
      if (this.m_AssetTree != null)
        this.m_AssetTree.OnEvent();
      if (this.m_FolderTree != null)
        this.m_FolderTree.OnEvent();
      if (this.m_ListArea == null)
        return;
      this.m_ListArea.OnEvent();
    }

    private void OnGUI()
    {
      if (ProjectBrowser.s_Styles == null)
        ProjectBrowser.s_Styles = new ProjectBrowser.Styles();
      if (!this.Initialized())
        this.Init();
      this.m_ListKeyboardControlID = GUIUtility.GetControlID(FocusType.Keyboard);
      this.m_TreeViewKeyboardControlID = GUIUtility.GetControlID(FocusType.Keyboard);
      this.OnEvent();
      this.m_ToolbarHeight = 18f;
      this.m_ItemSelectedByRightClickThisEvent = false;
      this.ResizeHandling(this.position.width, this.position.height - this.m_ToolbarHeight);
      this.CalculateRects();
      Event current = Event.current;
      Rect position1 = new Rect(0.0f, 0.0f, this.position.width, this.position.height);
      if (current.type == EventType.MouseDown && position1.Contains(current.mousePosition))
      {
        this.EndPing();
        this.SetAsLastInteractedProjectBrowser();
      }
      if (this.m_GrabKeyboardFocusForListArea)
      {
        this.m_GrabKeyboardFocusForListArea = false;
        GUIUtility.keyboardControl = this.m_ListKeyboardControlID;
      }
      GUI.BeginGroup(position1, GUIContent.none);
      this.TopToolbar();
      this.BottomBar();
      if (this.m_ViewMode == ProjectBrowser.ViewMode.OneColumn)
      {
        if (this.m_SearchFilter.IsSearching())
        {
          this.SearchAreaBar();
          if (GUIUtility.keyboardControl == this.m_TreeViewKeyboardControlID)
            GUIUtility.keyboardControl = this.m_ListKeyboardControlID;
          this.m_ListArea.OnGUI(this.m_ListAreaRect, this.m_ListKeyboardControlID);
        }
        else
        {
          if (GUIUtility.keyboardControl == this.m_ListKeyboardControlID)
            GUIUtility.keyboardControl = this.m_TreeViewKeyboardControlID;
          this.m_AssetTree.OnGUI(this.m_TreeViewRect, this.m_TreeViewKeyboardControlID);
        }
      }
      else
      {
        if (this.m_SearchFilter.IsSearching())
          this.SearchAreaBar();
        else
          this.BreadCrumbBar();
        this.m_FolderTree.OnGUI(this.m_TreeViewRect, this.m_TreeViewKeyboardControlID);
        EditorGUIUtility.DrawHorizontalSplitter(new Rect(this.m_ListAreaRect.x, this.m_ToolbarHeight, 1f, this.m_TreeViewRect.height));
        this.m_ListArea.OnGUI(this.m_ListAreaRect, this.m_ListKeyboardControlID);
        if (this.m_SearchFilter.GetState() == SearchFilter.State.FolderBrowsing && this.m_ListArea.numItemsDisplayed == 0)
        {
          Vector2 vector2 = EditorStyles.label.CalcSize(ProjectBrowser.s_Styles.m_EmptyFolderText);
          Rect position2 = new Rect(this.m_ListAreaRect.x + 2f + Mathf.Max(0.0f, (float) (((double) this.m_ListAreaRect.width - (double) vector2.x) * 0.5)), this.m_ListAreaRect.y + 10f, vector2.x, 20f);
          using (new EditorGUI.DisabledScope(true))
            GUI.Label(position2, ProjectBrowser.s_Styles.m_EmptyFolderText, EditorStyles.label);
        }
      }
      this.HandleContextClickInListArea(this.m_ListAreaRect);
      if (this.m_ListArea.gridSize != this.m_StartGridSize)
      {
        this.m_StartGridSize = this.m_ListArea.gridSize;
        if (this.m_SearchFilter.GetState() == SearchFilter.State.FolderBrowsing)
          this.m_LastFoldersGridSize = (float) this.m_ListArea.gridSize;
      }
      GUI.EndGroup();
      if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns)
        this.useTreeViewSelectionInsteadOfMainSelection = GUIUtility.keyboardControl == this.m_TreeViewKeyboardControlID;
      if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns && GUIUtility.keyboardControl == this.m_TreeViewKeyboardControlID)
        this.HandleCommandEventsForTreeView();
      this.HandleCommandEvents();
    }

    private void HandleContextClickInListArea(Rect listRect)
    {
      Event current = Event.current;
      switch (current.type)
      {
        case EventType.MouseDown:
          if (this.m_ViewMode != ProjectBrowser.ViewMode.TwoColumns || this.m_SearchFilter.GetState() != SearchFilter.State.FolderBrowsing || (current.button != 1 || this.m_ItemSelectedByRightClickThisEvent) || (this.m_SearchFilter.folders.Length <= 0 || !listRect.Contains(current.mousePosition)))
            break;
          this.m_InternalSelectionChange = true;
          Selection.instanceIDs = ProjectBrowser.GetFolderInstanceIDs(this.m_SearchFilter.folders);
          break;
        case EventType.ContextClick:
          if (!listRect.Contains(current.mousePosition))
            break;
          GUIUtility.hotControl = 0;
          if (AssetStoreAssetSelection.GetFirstAsset() != null)
            ProjectBrowser.AssetStoreItemContextMenu.Show();
          else
            EditorUtility.DisplayPopupMenu(new Rect(current.mousePosition.x, current.mousePosition.y, 0.0f, 0.0f), "Assets/", (MenuCommand) null);
          current.Use();
          break;
      }
    }

    private void AssetTreeViewContextClick(int clickedItemID)
    {
      Event current = Event.current;
      EditorUtility.DisplayPopupMenu(new Rect(current.mousePosition.x, current.mousePosition.y, 0.0f, 0.0f), "Assets/", (MenuCommand) null);
      current.Use();
    }

    private void AssetTreeViewContextClickOutsideItems()
    {
      Event current = Event.current;
      if (this.m_AssetTree.GetSelection().Length > 0)
      {
        int[] numArray = new int[0];
        this.m_AssetTree.SetSelection(numArray, false);
        this.AssetTreeSelectionCallback(numArray);
      }
      EditorUtility.DisplayPopupMenu(new Rect(current.mousePosition.x, current.mousePosition.y, 0.0f, 0.0f), "Assets/", (MenuCommand) null);
      current.Use();
    }

    private void FolderTreeViewContextClick(int clickedItemID)
    {
      Event current = Event.current;
      if (SavedSearchFilters.IsSavedFilter(clickedItemID))
      {
        if (clickedItemID != SavedSearchFilters.GetRootInstanceID())
          ProjectBrowser.SavedFiltersContextMenu.Show(clickedItemID);
      }
      else
        EditorUtility.DisplayPopupMenu(new Rect(current.mousePosition.x, current.mousePosition.y, 0.0f, 0.0f), "Assets/", (MenuCommand) null);
      current.Use();
    }

    private void AssetTreeDragEnded(int[] draggedInstanceIds, bool draggedItemsFromOwnTreeView)
    {
      if (draggedInstanceIds == null || !draggedItemsFromOwnTreeView)
        return;
      this.m_AssetTree.SetSelection(draggedInstanceIds, true);
      this.m_AssetTree.NotifyListenersThatSelectionChanged();
      this.Repaint();
      GUIUtility.ExitGUI();
    }

    private void FolderTreeDragEnded(int[] draggedInstanceIds, bool draggedItemsFromOwnTreeView)
    {
    }

    private void TopToolbar()
    {
      GUILayout.BeginArea(new Rect(0.0f, 0.0f, this.position.width, this.m_ToolbarHeight));
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      float num = this.position.width - this.m_DirectoriesAreaWidth;
      float pixels = 4f;
      if ((double) num >= 500.0)
        pixels = 10f;
      this.CreateDropdown();
      GUILayout.FlexibleSpace();
      GUILayout.Space(pixels * 2f);
      this.SearchField();
      GUILayout.Space(pixels);
      this.TypeDropDown();
      this.AssetLabelsDropDown();
      if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns)
        this.ButtonSaveFilter();
      GUILayout.EndHorizontal();
      GUILayout.EndArea();
    }

    private void SetOneColumn()
    {
      this.SetViewMode(ProjectBrowser.ViewMode.OneColumn);
    }

    private void SetTwoColumns()
    {
      this.SetViewMode(ProjectBrowser.ViewMode.TwoColumns);
    }

    internal bool IsTwoColumns()
    {
      return this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns;
    }

    private void OpenTreeViewTestWindow()
    {
      EditorWindow.GetWindow<TreeViewTestWindow>();
    }

    private void ToggleExpansionAnimationPreference()
    {
      EditorPrefs.SetBool("TreeViewExpansionAnimation", !EditorPrefs.GetBool("TreeViewExpansionAnimation", false));
      InternalEditorUtility.RequestScriptReload();
    }

    private void ToggleShowPackagesInAssetsFolder()
    {
      EditorPrefs.SetBool("ShowPackagesFolder", !EditorPrefs.GetBool("ShowPackagesFolder", false));
      EditorApplication.projectWindowChanged();
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      if (!this.m_EnableOldAssetTree)
        return;
      GUIContent content1 = new GUIContent("One Column Layout");
      GUIContent content2 = new GUIContent("Two Column Layout");
      menu.AddItem(content1, this.m_ViewMode == ProjectBrowser.ViewMode.OneColumn, new GenericMenu.MenuFunction(this.SetOneColumn));
      if ((double) this.position.width >= 230.0)
        menu.AddItem(content2, this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns, new GenericMenu.MenuFunction(this.SetTwoColumns));
      else
        menu.AddDisabledItem(content2);
      if (Unsupported.IsDeveloperBuild())
      {
        menu.AddItem(new GUIContent("DEVELOPER/Show Packages in Project Window"), EditorPrefs.GetBool("ShowPackagesFolder", false), new GenericMenu.MenuFunction(this.ToggleShowPackagesInAssetsFolder));
        menu.AddItem(new GUIContent("DEVELOPER/Open TreeView Test Window..."), false, new GenericMenu.MenuFunction(this.OpenTreeViewTestWindow));
        menu.AddItem(new GUIContent("DEVELOPER/Use TreeView Expansion Animation"), EditorPrefs.GetBool("TreeViewExpansionAnimation", false), new GenericMenu.MenuFunction(this.ToggleExpansionAnimationPreference));
      }
    }

    private float DrawLocalAssetHeader(Rect r)
    {
      return 0.0f;
    }

    private void ResizeHandling(float width, float height)
    {
      if (this.m_ViewMode == ProjectBrowser.ViewMode.OneColumn)
        return;
      Rect dragRect = new Rect(this.m_DirectoriesAreaWidth, this.m_ToolbarHeight, 5f, height);
      dragRect = EditorGUIUtility.HandleHorizontalSplitter(dragRect, this.position.width, this.k_MinDirectoriesAreaWidth, 230f - this.k_MinDirectoriesAreaWidth);
      this.m_DirectoriesAreaWidth = dragRect.x;
      float num = this.position.width - this.m_DirectoriesAreaWidth;
      if ((double) num != (double) this.m_LastListWidth)
        this.RefreshSplittedSelectedPath();
      this.m_LastListWidth = num;
    }

    private void ButtonSaveFilter()
    {
      using (new EditorGUI.DisabledScope(!this.m_SearchFilter.IsSearching()))
      {
        if (!GUILayout.Button(ProjectBrowser.s_Styles.m_SaveFilterContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
          return;
        ProjectBrowserColumnOneTreeViewGUI gui = this.m_FolderTree.gui as ProjectBrowserColumnOneTreeViewGUI;
        if (gui != null)
        {
          bool flag1 = true;
          int[] selection = this.m_FolderTree.GetSelection();
          if (selection.Length == 1)
          {
            int instanceID = selection[0];
            bool flag2 = SavedSearchFilters.GetRootInstanceID() == instanceID;
            if (SavedSearchFilters.IsSavedFilter(instanceID) && !flag2)
            {
              flag1 = false;
              switch (EditorUtility.DisplayDialogComplex("Overwrite Filter?", "Do you want to overwrite '" + SavedSearchFilters.GetName(instanceID) + "' or create a new filter?", "Overwrite", "Create", "Cancel"))
              {
                case 0:
                  SavedSearchFilters.UpdateExistingSavedFilter(instanceID, this.m_SearchFilter, this.listAreaGridSize);
                  break;
                case 1:
                  flag1 = true;
                  break;
              }
            }
          }
          if (flag1)
          {
            this.Focus();
            gui.BeginCreateSavedFilter(this.m_SearchFilter);
          }
        }
      }
    }

    private void CreateDropdown()
    {
      Rect rect = GUILayoutUtility.GetRect(ProjectBrowser.s_Styles.m_CreateDropdownContent, EditorStyles.toolbarDropDown);
      if (!EditorGUI.DropdownButton(rect, ProjectBrowser.s_Styles.m_CreateDropdownContent, FocusType.Passive, EditorStyles.toolbarDropDown))
        return;
      GUIUtility.hotControl = 0;
      EditorUtility.DisplayPopupMenu(rect, "Assets/Create", (MenuCommand) null);
    }

    private void AssetLabelsDropDown()
    {
      Rect rect = GUILayoutUtility.GetRect(ProjectBrowser.s_Styles.m_FilterByLabel, EditorStyles.toolbarButton);
      if (!EditorGUI.DropdownButton(rect, ProjectBrowser.s_Styles.m_FilterByLabel, FocusType.Passive, EditorStyles.toolbarButton))
        return;
      PopupWindow.Show(rect, (PopupWindowContent) new PopupList(this.m_AssetLabels), (PopupLocationHelper.PopupLocation[]) null, ShowMode.PopupMenuWithKeyboardFocus);
    }

    private void TypeDropDown()
    {
      Rect rect = GUILayoutUtility.GetRect(ProjectBrowser.s_Styles.m_FilterByType, EditorStyles.toolbarButton);
      if (!EditorGUI.DropdownButton(rect, ProjectBrowser.s_Styles.m_FilterByType, FocusType.Passive, EditorStyles.toolbarButton))
        return;
      PopupWindow.Show(rect, (PopupWindowContent) new PopupList(this.m_ObjectTypes));
    }

    private void SearchField()
    {
      Rect rect = GUILayoutUtility.GetRect(0.0f, (float) ((double) EditorGUILayout.kLabelFloatMaxW * 1.5), 16f, 16f, EditorStyles.toolbarSearchField, GUILayout.MinWidth(65f), GUILayout.MaxWidth(300f));
      int controlId = GUIUtility.GetControlID(ProjectBrowser.s_HashForSearchField, FocusType.Passive, rect);
      if (this.m_FocusSearchField)
      {
        GUIUtility.keyboardControl = controlId;
        EditorGUIUtility.editingTextField = true;
        if (Event.current.type == EventType.Repaint)
          this.m_FocusSearchField = false;
      }
      Event current = Event.current;
      if (current.type == EventType.KeyDown && (current.keyCode == KeyCode.DownArrow || current.keyCode == KeyCode.UpArrow) && GUIUtility.keyboardControl == controlId)
      {
        if (!this.m_ListArea.IsLastClickedItemVisible())
          this.m_ListArea.SelectFirst();
        GUIUtility.keyboardControl = this.m_ListKeyboardControlID;
        current.Use();
      }
      string str = EditorGUI.ToolbarSearchField(controlId, rect, this.m_SearchFieldText, false);
      if (!(str != this.m_SearchFieldText) && !this.m_FocusSearchField)
        return;
      this.m_SearchFieldText = str;
      this.m_SearchFilter.SearchFieldStringToFilter(this.m_SearchFieldText);
      this.SyncFilterGUI();
      this.TopBarSearchSettingsChanged();
      this.Repaint();
    }

    private void TopBarSearchSettingsChanged()
    {
      if (!this.m_SearchFilter.IsSearching())
      {
        if (this.m_DidSelectSearchResult)
        {
          this.m_DidSelectSearchResult = false;
          this.FrameObjectPrivate(Selection.activeInstanceID, true, false);
          if (GUIUtility.keyboardControl == 0)
          {
            if (this.m_ViewMode == ProjectBrowser.ViewMode.OneColumn)
              GUIUtility.keyboardControl = this.m_TreeViewKeyboardControlID;
            else if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns)
              GUIUtility.keyboardControl = this.m_ListKeyboardControlID;
          }
        }
        else if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns && GUIUtility.keyboardControl == 0 && (this.m_LastFolders != null && this.m_LastFolders.Length > 0))
        {
          this.m_SearchFilter.folders = this.m_LastFolders;
          this.SetFolderSelection(ProjectBrowser.GetFolderInstanceIDs(this.m_LastFolders), true);
        }
      }
      else
        this.InitSearchMenu();
      this.InitListArea();
    }

    private static int[] GetFolderInstanceIDs(string[] folders)
    {
      int[] numArray = new int[folders.Length];
      for (int index = 0; index < folders.Length; ++index)
        numArray[index] = AssetDatabase.GetMainAssetInstanceID(folders[index]);
      return numArray;
    }

    private static string[] GetFolderPathsFromInstanceIDs(int[] instanceIDs)
    {
      List<string> stringList = new List<string>();
      foreach (int instanceId in instanceIDs)
      {
        string assetPath = AssetDatabase.GetAssetPath(instanceId);
        if (!string.IsNullOrEmpty(assetPath))
          stringList.Add(assetPath);
      }
      return stringList.ToArray();
    }

    private void ClearSearch()
    {
      this.m_SearchFilter.ClearSearch();
      this.m_SearchFieldText = "";
      this.m_AssetLabels.DeselectAll();
      this.m_ObjectTypes.DeselectAll();
      this.m_DidSelectSearchResult = false;
    }

    private void FolderTreeSelectionChanged(bool folderWasSelected)
    {
      if (folderWasSelected)
      {
        switch (this.GetSearchViewState())
        {
          case ProjectBrowser.SearchViewState.AllAssets:
          case ProjectBrowser.SearchViewState.AssetStore:
            string[] folders = this.m_SearchFilter.folders;
            this.ClearSearch();
            this.m_SearchFilter.folders = folders;
            this.m_SearchFilter.searchArea = this.m_LastLocalAssetsSearchArea;
            break;
        }
        this.m_LastFolders = this.m_SearchFilter.folders;
      }
      this.RefreshSearchText();
      this.InitListArea();
    }

    private void IconSizeSlider(Rect r)
    {
      EditorGUI.BeginChangeCheck();
      int num = (int) GUI.HorizontalSlider(r, (float) this.m_ListArea.gridSize, (float) this.m_ListArea.minGridSize, (float) this.m_ListArea.maxGridSize);
      if (!EditorGUI.EndChangeCheck())
        return;
      AssetStorePreviewManager.AbortSize(this.m_ListArea.gridSize);
      this.m_ListArea.gridSize = num;
    }

    private void SearchAreaBar()
    {
      GUI.Label(this.m_ListHeaderRect, GUIContent.none, ProjectBrowser.s_Styles.topBarBg);
      Rect listHeaderRect = this.m_ListHeaderRect;
      listHeaderRect.x += 5f;
      listHeaderRect.width -= 10f;
      ++listHeaderRect.y;
      GUIStyle boldLabel = EditorStyles.boldLabel;
      GUI.Label(listHeaderRect, ProjectBrowser.s_Styles.m_SearchIn, boldLabel);
      if ((double) this.m_SearchAreaMenuOffset < 0.0)
        this.m_SearchAreaMenuOffset = boldLabel.CalcSize(ProjectBrowser.s_Styles.m_SearchIn).x;
      listHeaderRect.x += this.m_SearchAreaMenuOffset + 7f;
      listHeaderRect.width -= this.m_SearchAreaMenuOffset + 7f;
      listHeaderRect.width = this.m_SearchAreaMenu.OnGUI(listHeaderRect);
    }

    private void BreadCrumbBar()
    {
      if ((double) this.m_ListHeaderRect.height <= 0.0 || this.m_SearchFilter.folders.Length == 0)
        return;
      Event current = Event.current;
      if (current.type == EventType.MouseDown && this.m_ListHeaderRect.Contains(current.mousePosition))
      {
        GUIUtility.keyboardControl = this.m_ListKeyboardControlID;
        this.Repaint();
      }
      if (this.m_BreadCrumbs.Count == 0)
      {
        string folder = this.m_SearchFilter.folders[0];
        string[] strArray = folder.Split('/');
        string packagesMountPoint = AssetDatabase.GetPackagesMountPoint();
        if (folder.StartsWith(packagesMountPoint))
          strArray = Regex.Replace(folder, "^" + packagesMountPoint, AssetDatabase.GetPackagesMountPoint()).Split('/');
        string str = "";
        foreach (string text in strArray)
        {
          if (!string.IsNullOrEmpty(str))
            str += "/";
          str += text;
          this.m_BreadCrumbs.Add(new KeyValuePair<GUIContent, string>(new GUIContent(text), str));
        }
        this.m_BreadCrumbLastFolderHasSubFolders = AssetDatabase.GetSubFolders(folder).Length > 0;
      }
      GUI.Label(this.m_ListHeaderRect, GUIContent.none, ProjectBrowser.s_Styles.topBarBg);
      Rect listHeaderRect = this.m_ListHeaderRect;
      ++listHeaderRect.y;
      listHeaderRect.x += 4f;
      if (this.m_SearchFilter.folders.Length == 1)
      {
        for (int index = 0; index < this.m_BreadCrumbs.Count; ++index)
        {
          bool flag = index == this.m_BreadCrumbs.Count - 1;
          GUIStyle style = !flag ? EditorStyles.label : EditorStyles.boldLabel;
          GUIContent key = this.m_BreadCrumbs[index].Key;
          string str = this.m_BreadCrumbs[index].Value;
          Vector2 vector2 = style.CalcSize(key);
          listHeaderRect.width = vector2.x;
          if (GUI.Button(listHeaderRect, key, style))
            this.ShowFolderContents(AssetDatabase.GetMainAssetInstanceID(str), false);
          listHeaderRect.x += vector2.x + 3f;
          if (!flag || this.m_BreadCrumbLastFolderHasSubFolders)
          {
            Rect rect = new Rect(listHeaderRect.x, listHeaderRect.y + 2f, 13f, 13f);
            if (EditorGUI.DropdownButton(rect, GUIContent.none, FocusType.Passive, ProjectBrowser.s_Styles.foldout))
            {
              string currentSubFolder = "";
              if (!flag)
                currentSubFolder = this.m_BreadCrumbs[index + 1].Value;
              ProjectBrowser.BreadCrumbListMenu.Show(str, currentSubFolder, rect, this);
            }
          }
          listHeaderRect.x += 11f;
        }
      }
      else
      {
        if (this.m_SearchFilter.folders.Length <= 1)
          return;
        GUI.Label(listHeaderRect, GUIContent.Temp("Showing multiple folders..."), EditorStyles.miniLabel);
      }
    }

    private void BottomBar()
    {
      if ((double) this.m_BottomBarRect.height == 0.0)
        return;
      Rect bottomBarRect = this.m_BottomBarRect;
      GUI.Label(bottomBarRect, GUIContent.none, ProjectBrowser.s_Styles.bottomBarBg);
      this.IconSizeSlider(new Rect((float) ((double) bottomBarRect.x + (double) bottomBarRect.width - 55.0 - 16.0), (float) ((double) bottomBarRect.y + (double) bottomBarRect.height - 17.0), 55f, 17f));
      EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
      bottomBarRect.width -= 4f;
      bottomBarRect.x += 2f;
      bottomBarRect.height = 17f;
      for (int index = this.m_SelectedPathSplitted.Count - 1; index >= 0; --index)
      {
        if (index == 0)
          bottomBarRect.width = (float) ((double) bottomBarRect.width - 55.0 - 14.0);
        GUI.Label(bottomBarRect, this.m_SelectedPathSplitted[index], ProjectBrowser.s_Styles.selectedPathLabel);
        bottomBarRect.y += 17f;
      }
      EditorGUIUtility.SetIconSize(new Vector2(0.0f, 0.0f));
    }

    private void SelectAssetsFolder()
    {
      this.ShowFolderContents(ProjectBrowserColumnOneTreeViewDataSource.GetAssetsFolderInstanceID(), true);
    }

    private string ValidateCreateNewAssetPath(string pathName)
    {
      if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns && this.m_SearchFilter.GetState() == SearchFilter.State.FolderBrowsing && (this.m_SearchFilter.folders.Length > 0 && !pathName.StartsWith("assets/", StringComparison.CurrentCultureIgnoreCase)) && Selection.GetFiltered(typeof (UnityEngine.Object), SelectionMode.Assets).Length == 0)
      {
        pathName = Path.Combine(this.m_SearchFilter.folders[0], pathName);
        pathName = pathName.Replace("\\", "/");
      }
      return pathName;
    }

    internal void BeginPreimportedNameEditing(int instanceID, EndNameEditAction endAction, string pathName, Texture2D icon, string resourceFile)
    {
      if (!this.Initialized())
        this.Init();
      this.EndRenaming();
      bool isCreatingNewFolder = endAction is DoCreateFolder;
      if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns)
      {
        if (this.m_SearchFilter.GetState() != SearchFilter.State.FolderBrowsing)
          this.SelectAssetsFolder();
        pathName = this.ValidateCreateNewAssetPath(pathName);
        if (!this.m_ListAreaState.m_CreateAssetUtility.BeginNewAssetCreation(instanceID, endAction, pathName, icon, resourceFile))
          return;
        this.ShowFolderContents(AssetDatabase.GetMainAssetInstanceID(this.m_ListAreaState.m_CreateAssetUtility.folder), true);
        this.m_ListArea.BeginNamingNewAsset(this.m_ListAreaState.m_CreateAssetUtility.originalName, instanceID, isCreatingNewFolder);
      }
      else
      {
        if (this.m_ViewMode != ProjectBrowser.ViewMode.OneColumn)
          return;
        if (this.m_SearchFilter.IsSearching())
          this.ClearSearch();
        AssetsTreeViewGUI gui = this.m_AssetTree.gui as AssetsTreeViewGUI;
        if (gui != null)
          gui.BeginCreateNewAsset(instanceID, endAction, pathName, icon, resourceFile);
        else
          Debug.LogError((object) "Not valid defaultTreeViewGUI!");
      }
    }

    public void FrameObject(int instanceID, bool ping)
    {
      bool frame = !this.m_IsLocked && (ping || this.ShouldFrameAsset(instanceID));
      this.FrameObjectPrivate(instanceID, frame, ping);
      if (!((UnityEngine.Object) ProjectBrowser.s_LastInteractedProjectBrowser == (UnityEngine.Object) this))
        return;
      this.m_GrabKeyboardFocusForListArea = true;
    }

    private void FrameObjectPrivate(int instanceID, bool frame, bool ping)
    {
      if (instanceID == 0 || this.m_ListArea == null)
        return;
      if (this.m_LastFramedID != instanceID)
        this.EndPing();
      this.m_LastFramedID = instanceID;
      this.ClearSearch();
      if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns)
      {
        this.FrameObjectInTwoColumnMode(instanceID, frame, ping);
      }
      else
      {
        if (this.m_ViewMode != ProjectBrowser.ViewMode.OneColumn)
          return;
        this.m_AssetTree.Frame(instanceID, frame, ping);
      }
    }

    private void FrameObjectInTwoColumnMode(int instanceID, bool frame, bool ping)
    {
      int num = 0;
      string assetPath = AssetDatabase.GetAssetPath(instanceID);
      if (!string.IsNullOrEmpty(assetPath))
      {
        string containingFolder = ProjectWindowUtil.GetContainingFolder(assetPath);
        if (!string.IsNullOrEmpty(containingFolder))
          num = AssetDatabase.GetMainAssetInstanceID(containingFolder);
        if (num == 0)
          num = ProjectBrowserColumnOneTreeViewDataSource.GetAssetsFolderInstanceID();
      }
      if (num == 0)
        return;
      this.m_FolderTree.Frame(num, frame, ping);
      if (frame)
        this.ShowFolderContents(num, true);
      this.m_ListArea.Frame(instanceID, frame, ping);
    }

    private static int[] GetTreeViewFolderSelection()
    {
      ProjectBrowser interactedProjectBrowser = ProjectBrowser.s_LastInteractedProjectBrowser;
      if ((UnityEngine.Object) interactedProjectBrowser != (UnityEngine.Object) null && interactedProjectBrowser.useTreeViewSelectionInsteadOfMainSelection && interactedProjectBrowser.m_FolderTree != null)
        return ProjectBrowser.s_LastInteractedProjectBrowser.m_FolderTree.GetSelection();
      return new int[0];
    }

    public float listAreaGridSize
    {
      get
      {
        return (float) this.m_ListArea.gridSize;
      }
    }

    private int GetProjectBrowserDebugID()
    {
      for (int index = 0; index < ProjectBrowser.s_ProjectBrowsers.Count; ++index)
      {
        if ((UnityEngine.Object) ProjectBrowser.s_ProjectBrowsers[index] == (UnityEngine.Object) this)
          return index;
      }
      return -1;
    }

    internal static void DeleteSelectedAssets(bool askIfSure)
    {
      int[] viewFolderSelection = ProjectBrowser.GetTreeViewFolderSelection();
      List<int> instanceIDs = viewFolderSelection.Length <= 0 ? new List<int>((IEnumerable<int>) Selection.instanceIDs) : new List<int>((IEnumerable<int>) viewFolderSelection);
      if (instanceIDs.Count == 0)
        return;
      ProjectWindowUtil.DeleteAssets(instanceIDs, askIfSure);
      Selection.instanceIDs = new int[0];
    }

    internal IHierarchyProperty GetHierarchyPropertyUsingFilter(string textFilter)
    {
      return FilteredHierarchyProperty.CreateHierarchyPropertyForFilter(new FilteredHierarchy(HierarchyType.Assets) { searchFilter = SearchFilter.CreateSearchFilterFromString(textFilter) });
    }

    internal void ShowObjectsInList(int[] instanceIDs)
    {
      if (!this.Initialized())
        this.Init();
      if (this.m_ViewMode == ProjectBrowser.ViewMode.TwoColumns)
      {
        this.m_ListArea.ShowObjectsInList(instanceIDs);
        this.m_FolderTree.SetSelection(new int[0], false);
      }
      else
      {
        if (this.m_ViewMode != ProjectBrowser.ViewMode.OneColumn)
          return;
        foreach (int instanceId in Selection.instanceIDs)
          this.m_AssetTree.Frame(instanceId, true, false);
      }
    }

    private static void ShowSelectedObjectsInLastInteractedProjectBrowser()
    {
      if (!((UnityEngine.Object) ProjectBrowser.s_LastInteractedProjectBrowser != (UnityEngine.Object) null))
        return;
      int[] instanceIds = Selection.instanceIDs;
      ProjectBrowser.s_LastInteractedProjectBrowser.ShowObjectsInList(instanceIds);
    }

    protected virtual void ShowButton(Rect r)
    {
      if (ProjectBrowser.s_Styles == null)
        ProjectBrowser.s_Styles = new ProjectBrowser.Styles();
      this.m_IsLocked = GUI.Toggle(r, this.m_IsLocked, GUIContent.none, ProjectBrowser.s_Styles.lockButton);
    }

    internal enum ItemType
    {
      Asset,
      SavedFilter,
    }

    private enum ViewMode
    {
      OneColumn,
      TwoColumns,
    }

    public enum SearchViewState
    {
      NotSearching,
      AllAssets,
      SubFolders,
      AssetStore,
    }

    private class Styles
    {
      public GUIStyle bottomBarBg = (GUIStyle) "ProjectBrowserBottomBarBg";
      public GUIStyle topBarBg = (GUIStyle) "ProjectBrowserTopBarBg";
      public GUIStyle selectedPathLabel = (GUIStyle) "Label";
      public GUIStyle exposablePopup = ProjectBrowser.Styles.GetStyle("ExposablePopupMenu");
      public GUIStyle lockButton = (GUIStyle) "IN LockButton";
      public GUIStyle foldout = (GUIStyle) "AC RightArrow";
      public GUIContent m_FilterByLabel = new GUIContent((Texture) EditorGUIUtility.FindTexture("FilterByLabel"), "Search by Label");
      public GUIContent m_FilterByType = new GUIContent((Texture) EditorGUIUtility.FindTexture("FilterByType"), "Search by Type");
      public GUIContent m_CreateDropdownContent = new GUIContent("Create");
      public GUIContent m_SaveFilterContent = new GUIContent((Texture) EditorGUIUtility.FindTexture("Favorite"), "Save search");
      public GUIContent m_EmptyFolderText = new GUIContent("This folder is empty");
      public GUIContent m_SearchIn = new GUIContent("Search:");

      private static GUIStyle GetStyle(string styleName)
      {
        return (GUIStyle) styleName;
      }
    }

    internal class SavedFiltersContextMenu
    {
      private int m_SavedFilterInstanceID;

      private SavedFiltersContextMenu(int savedFilterInstanceID)
      {
        this.m_SavedFilterInstanceID = savedFilterInstanceID;
      }

      internal static void Show(int savedFilterInstanceID)
      {
        GUIContent content = new GUIContent("Delete");
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(content, false, new GenericMenu.MenuFunction(new ProjectBrowser.SavedFiltersContextMenu(savedFilterInstanceID).Delete));
        genericMenu.ShowAsContext();
      }

      private void Delete()
      {
        ProjectBrowser.DeleteFilter(this.m_SavedFilterInstanceID);
      }
    }

    internal class BreadCrumbListMenu
    {
      private static ProjectBrowser m_Caller;
      private string m_SubFolder;

      private BreadCrumbListMenu(string subFolder)
      {
        this.m_SubFolder = subFolder;
      }

      internal static void Show(string folder, string currentSubFolder, Rect activatorRect, ProjectBrowser caller)
      {
        ProjectBrowser.BreadCrumbListMenu.m_Caller = caller;
        string[] subFolders = AssetDatabase.GetSubFolders(folder);
        GenericMenu genericMenu = new GenericMenu();
        if (subFolders.Length >= 0)
        {
          currentSubFolder = Path.GetFileName(currentSubFolder);
          foreach (string str in subFolders)
          {
            string fileName = Path.GetFileName(str);
            genericMenu.AddItem(new GUIContent(fileName), fileName == currentSubFolder, new GenericMenu.MenuFunction(new ProjectBrowser.BreadCrumbListMenu(str).SelectSubFolder));
            genericMenu.ShowAsContext();
          }
        }
        else
          genericMenu.AddDisabledItem(new GUIContent("No sub folders..."));
        genericMenu.DropDown(activatorRect);
      }

      private void SelectSubFolder()
      {
        int mainAssetInstanceId = AssetDatabase.GetMainAssetInstanceID(this.m_SubFolder);
        if (mainAssetInstanceId == 0)
          return;
        ProjectBrowser.BreadCrumbListMenu.m_Caller.ShowFolderContents(mainAssetInstanceId, false);
      }
    }

    internal class AssetStoreItemContextMenu
    {
      private AssetStoreItemContextMenu()
      {
      }

      internal static void Show()
      {
        GenericMenu genericMenu = new GenericMenu();
        GUIContent content = new GUIContent("Show in Asset Store window");
        AssetStoreAsset firstAsset = AssetStoreAssetSelection.GetFirstAsset();
        if (firstAsset != null && firstAsset.id != 0)
          genericMenu.AddItem(content, false, new GenericMenu.MenuFunction(new ProjectBrowser.AssetStoreItemContextMenu().OpenAssetStoreWindow));
        else
          genericMenu.AddDisabledItem(content);
        genericMenu.ShowAsContext();
      }

      private void OpenAssetStoreWindow()
      {
        AssetStoreAsset firstAsset = AssetStoreAssetSelection.GetFirstAsset();
        if (firstAsset == null)
          return;
        AssetStoreAssetInspector.OpenItemInAssetStore(firstAsset);
      }
    }
  }
}
