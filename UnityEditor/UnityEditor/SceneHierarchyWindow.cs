// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneHierarchyWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Hierarchy", useTypeNameAsIconName = true)]
  internal class SceneHierarchyWindow : SearchableEditorWindow, IHasCustomMenu
  {
    private static List<SceneHierarchyWindow> s_SceneHierarchyWindow = new List<SceneHierarchyWindow>();
    [SerializeField]
    private List<string> m_ExpandedScenes = new List<string>();
    [SerializeField]
    private int m_CurrenRootInstanceID = 0;
    [SerializeField]
    private string m_CurrentSortingName = "";
    [NonSerialized]
    private int m_LastFramedID = -1;
    private Dictionary<string, HierarchySorting> m_SortingObjects = (Dictionary<string, HierarchySorting>) null;
    private static SceneHierarchyWindow s_LastInteractedHierarchy;
    private static SceneHierarchyWindow.Styles s_Styles;
    private const int kInvalidSceneHandle = 0;
    private TreeViewController m_TreeView;
    [SerializeField]
    private TreeViewState m_TreeViewState;
    private int m_TreeViewKeyboardControlID;
    [SerializeField]
    private bool m_Locked;
    [NonSerialized]
    private bool m_TreeViewReloadNeeded;
    [NonSerialized]
    private bool m_SelectionSyncNeeded;
    [NonSerialized]
    private bool m_FrameOnSelectionSync;
    [NonSerialized]
    private bool m_DidSelectSearchResult;
    private bool m_AllowAlphaNumericalSort;
    [NonSerialized]
    private double m_LastUserInteractionTime;
    private bool m_Debug;

    public static SceneHierarchyWindow lastInteractedHierarchyWindow
    {
      get
      {
        return SceneHierarchyWindow.s_LastInteractedHierarchy;
      }
    }

    public static List<SceneHierarchyWindow> GetAllSceneHierarchyWindows()
    {
      return SceneHierarchyWindow.s_SceneHierarchyWindow;
    }

    internal static bool debug
    {
      get
      {
        return SceneHierarchyWindow.lastInteractedHierarchyWindow.m_Debug;
      }
      set
      {
        SceneHierarchyWindow.lastInteractedHierarchyWindow.m_Debug = value;
      }
    }

    public static bool s_Debug
    {
      get
      {
        return SessionState.GetBool("HierarchyWindowDebug", false);
      }
      set
      {
        SessionState.SetBool("HierarchyWindowDebug", value);
      }
    }

    private bool treeViewReloadNeeded
    {
      get
      {
        return this.m_TreeViewReloadNeeded;
      }
      set
      {
        this.m_TreeViewReloadNeeded = value;
        if (!value)
          return;
        this.Repaint();
        if (SceneHierarchyWindow.s_Debug)
          Debug.Log((object) "Reload treeview on next event");
      }
    }

    private bool selectionSyncNeeded
    {
      get
      {
        return this.m_SelectionSyncNeeded;
      }
      set
      {
        this.m_SelectionSyncNeeded = value;
        if (!value)
          return;
        this.Repaint();
        if (SceneHierarchyWindow.s_Debug)
          Debug.Log((object) "Selection sync and frameing on next event");
      }
    }

    private string currentSortingName
    {
      get
      {
        return this.m_CurrentSortingName;
      }
      set
      {
        this.m_CurrentSortingName = value;
        if (!this.m_SortingObjects.ContainsKey(this.m_CurrentSortingName))
          this.m_CurrentSortingName = this.GetNameForType(typeof (TransformSorting));
        ((GameObjectTreeViewDataSource) this.treeView.data).sortingState = this.m_SortingObjects[this.m_CurrentSortingName];
      }
    }

    private bool hasSortMethods
    {
      get
      {
        return this.m_SortingObjects.Count > 1;
      }
    }

    private Rect treeViewRect
    {
      get
      {
        return new Rect(0.0f, 17f, this.position.width, this.position.height - 17f);
      }
    }

    private TreeViewController treeView
    {
      get
      {
        if (this.m_TreeView == null)
          this.Init();
        return this.m_TreeView;
      }
    }

    private void Init()
    {
      if (this.m_TreeViewState == null)
        this.m_TreeViewState = new TreeViewState();
      this.m_TreeView = new TreeViewController((EditorWindow) this, this.m_TreeViewState);
      this.m_TreeView.itemDoubleClickedCallback += new Action<int>(this.TreeViewItemDoubleClicked);
      this.m_TreeView.selectionChangedCallback += new Action<int[]>(this.TreeViewSelectionChanged);
      this.m_TreeView.onGUIRowCallback += new Action<int, Rect>(this.OnGUIAssetCallback);
      this.m_TreeView.dragEndedCallback += new Action<int[], bool>(this.OnDragEndedCallback);
      this.m_TreeView.contextClickItemCallback += new Action<int>(this.ItemContextClick);
      this.m_TreeView.contextClickOutsideItemsCallback += new Action(this.ContextClickOutsideItems);
      this.m_TreeView.deselectOnUnhandledMouseDown = true;
      GameObjectTreeViewDataSource treeViewDataSource = new GameObjectTreeViewDataSource(this.m_TreeView, this.m_CurrenRootInstanceID, false, false);
      GameObjectsTreeViewDragging treeViewDragging = new GameObjectsTreeViewDragging(this.m_TreeView);
      GameObjectTreeViewGUI objectTreeViewGui = new GameObjectTreeViewGUI(this.m_TreeView, false);
      this.m_TreeView.Init(this.treeViewRect, (ITreeViewDataSource) treeViewDataSource, (ITreeViewGUI) objectTreeViewGui, (ITreeViewDragging) treeViewDragging);
      treeViewDataSource.searchMode = (int) this.searchMode;
      treeViewDataSource.searchString = this.m_SearchFilter;
      this.m_AllowAlphaNumericalSort = EditorPrefs.GetBool("AllowAlphaNumericHierarchy", false) || !InternalEditorUtility.isHumanControllingUs;
      this.SetUpSortMethodLists();
      this.m_TreeView.ReloadData();
    }

    internal void SetupForTesting()
    {
      this.m_AllowAlphaNumericalSort = true;
      this.SetUpSortMethodLists();
    }

    public void SetCurrentRootInstanceID(int instanceID)
    {
      this.m_CurrenRootInstanceID = instanceID;
      this.Init();
      GUIUtility.ExitGUI();
    }

    public string[] GetCurrentVisibleObjects()
    {
      IList<TreeViewItem> rows = this.m_TreeView.data.GetRows();
      string[] strArray = new string[rows.Count];
      for (int index = 0; index < rows.Count; ++index)
        strArray[index] = rows[index].displayName;
      return strArray;
    }

    internal void SelectPrevious()
    {
      this.m_TreeView.OffsetSelection(-1);
    }

    internal void SelectNext()
    {
      this.m_TreeView.OffsetSelection(1);
    }

    private void OnProjectWasLoaded()
    {
      this.m_TreeViewState.expandedIDs.Clear();
      if (SceneManager.sceneCount == 1)
        this.treeView.data.SetExpanded(SceneManager.GetSceneAt(0).handle, true);
      this.SetScenesExpanded(this.m_ExpandedScenes);
    }

    private IEnumerable<string> GetExpandedSceneNames()
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < SceneManager.sceneCount; ++index)
      {
        Scene sceneAt = SceneManager.GetSceneAt(index);
        if (this.treeView.data.IsExpanded(sceneAt.handle))
          stringList.Add(sceneAt.name);
      }
      return (IEnumerable<string>) stringList;
    }

    private void SetScenesExpanded(List<string> sceneNames)
    {
      List<int> intList = new List<int>();
      foreach (string sceneName in sceneNames)
      {
        Scene sceneByName = SceneManager.GetSceneByName(sceneName);
        if (sceneByName.IsValid())
          intList.Add(sceneByName.handle);
      }
      if (intList.Count <= 0)
        return;
      this.treeView.data.SetExpandedIDs(intList.ToArray());
    }

    private void OnSceneCreated(Scene scene, NewSceneSetup setup, NewSceneMode mode)
    {
      this.ExpandTreeViewItem(scene.handle, true);
    }

    private void OnSceneOpened(Scene scene, OpenSceneMode mode)
    {
      this.ExpandTreeViewItem(scene.handle, true);
    }

    private void ExpandTreeViewItem(int id, bool expand)
    {
      TreeViewDataSource data = this.treeView.data as TreeViewDataSource;
      if (data == null)
        return;
      data.SetExpanded(id, expand);
    }

    private void Awake()
    {
      this.m_HierarchyType = HierarchyType.GameObjects;
      if (this.m_TreeViewState == null)
        return;
      this.m_TreeViewState.OnAwake();
    }

    private void OnBecameVisible()
    {
      if (SceneManager.sceneCount <= 0)
        return;
      this.treeViewReloadNeeded = true;
    }

    public override void OnEnable()
    {
      base.OnEnable();
      this.titleContent = this.GetLocalizedTitleContent();
      SceneHierarchyWindow.s_SceneHierarchyWindow.Add(this);
      EditorApplication.projectWindowChanged += new EditorApplication.CallbackFunction(this.ReloadData);
      EditorApplication.editorApplicationQuit += new UnityAction(this.OnQuit);
      EditorApplication.searchChanged += new EditorApplication.CallbackFunction(this.SearchChanged);
      EditorApplication.projectWasLoaded += new UnityAction(this.OnProjectWasLoaded);
      EditorSceneManager.newSceneCreated += new EditorSceneManager.NewSceneCreatedCallback(this.OnSceneCreated);
      EditorSceneManager.sceneOpened += new EditorSceneManager.SceneOpenedCallback(this.OnSceneOpened);
      SceneHierarchyWindow.s_LastInteractedHierarchy = this;
    }

    public override void OnDisable()
    {
      EditorApplication.projectWindowChanged -= new EditorApplication.CallbackFunction(this.ReloadData);
      EditorApplication.editorApplicationQuit -= new UnityAction(this.OnQuit);
      EditorApplication.searchChanged -= new EditorApplication.CallbackFunction(this.SearchChanged);
      EditorApplication.projectWasLoaded -= new UnityAction(this.OnProjectWasLoaded);
      EditorSceneManager.newSceneCreated -= new EditorSceneManager.NewSceneCreatedCallback(this.OnSceneCreated);
      EditorSceneManager.sceneOpened -= new EditorSceneManager.SceneOpenedCallback(this.OnSceneOpened);
      SceneHierarchyWindow.s_SceneHierarchyWindow.Remove(this);
    }

    private void OnQuit()
    {
      this.m_ExpandedScenes = this.GetExpandedSceneNames().ToList<string>();
    }

    public void OnDestroy()
    {
      if (!((UnityEngine.Object) SceneHierarchyWindow.s_LastInteractedHierarchy == (UnityEngine.Object) this))
        return;
      SceneHierarchyWindow.s_LastInteractedHierarchy = (SceneHierarchyWindow) null;
      foreach (SceneHierarchyWindow sceneHierarchyWindow in SceneHierarchyWindow.s_SceneHierarchyWindow)
      {
        if ((UnityEngine.Object) sceneHierarchyWindow != (UnityEngine.Object) this)
          SceneHierarchyWindow.s_LastInteractedHierarchy = sceneHierarchyWindow;
      }
    }

    private void SetAsLastInteractedHierarchy()
    {
      SceneHierarchyWindow.s_LastInteractedHierarchy = this;
    }

    private void SyncIfNeeded()
    {
      if (this.treeViewReloadNeeded)
      {
        this.treeViewReloadNeeded = false;
        this.ReloadData();
      }
      if (!this.selectionSyncNeeded)
        return;
      this.selectionSyncNeeded = false;
      bool flag = EditorApplication.timeSinceStartup - this.m_LastUserInteractionTime < 0.2;
      bool revealSelectionAndFrameLastSelected = !this.m_Locked || this.m_FrameOnSelectionSync || flag;
      bool animatedFraming = flag && revealSelectionAndFrameLastSelected;
      this.m_FrameOnSelectionSync = false;
      this.treeView.SetSelection(Selection.instanceIDs, revealSelectionAndFrameLastSelected, animatedFraming);
    }

    private void DetectUserInteraction()
    {
      Event current = Event.current;
      if (current.type == EventType.Layout || current.type == EventType.Repaint)
        return;
      this.m_LastUserInteractionTime = EditorApplication.timeSinceStartup;
    }

    private void OnGUI()
    {
      if (SceneHierarchyWindow.s_Styles == null)
        SceneHierarchyWindow.s_Styles = new SceneHierarchyWindow.Styles();
      this.DetectUserInteraction();
      this.SyncIfNeeded();
      this.m_TreeViewKeyboardControlID = GUIUtility.GetControlID(FocusType.Keyboard);
      this.OnEvent();
      Rect rect = new Rect(0.0f, 0.0f, this.position.width, this.position.height);
      Event current = Event.current;
      if (current.type == EventType.MouseDown && rect.Contains(current.mousePosition))
      {
        this.treeView.EndPing();
        this.SetAsLastInteractedHierarchy();
      }
      this.DoToolbar();
      this.DoTreeView(this.DoSearchResultPathGUI());
      this.ExecuteCommands();
    }

    private void OnLostFocus()
    {
      this.treeView.EndNameEditing(true);
    }

    public static bool IsSceneHeaderInHierarchyWindow(Scene scene)
    {
      return scene.IsValid();
    }

    private void TreeViewItemDoubleClicked(int instanceID)
    {
      Scene sceneByHandle = EditorSceneManager.GetSceneByHandle(instanceID);
      if (SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(sceneByHandle))
      {
        if (!sceneByHandle.isLoaded)
          return;
        SceneManager.SetActiveScene(sceneByHandle);
      }
      else
        SceneView.FrameLastActiveSceneView();
    }

    public void SetExpandedRecursive(int id, bool expand)
    {
      TreeViewItem treeViewItem = this.treeView.data.FindItem(id);
      if (treeViewItem == null)
      {
        this.ReloadData();
        treeViewItem = this.treeView.data.FindItem(id);
      }
      if (treeViewItem == null)
        return;
      this.treeView.data.SetExpandedWithChildren(treeViewItem, expand);
    }

    private void OnGUIAssetCallback(int instanceID, Rect rect)
    {
      if (EditorApplication.hierarchyWindowItemOnGUI == null)
        return;
      EditorApplication.hierarchyWindowItemOnGUI(instanceID, rect);
    }

    private void OnDragEndedCallback(int[] draggedInstanceIds, bool draggedItemsFromOwnTreeView)
    {
      if (draggedInstanceIds == null || !draggedItemsFromOwnTreeView)
        return;
      this.ReloadData();
      this.treeView.SetSelection(draggedInstanceIds, true);
      this.treeView.NotifyListenersThatSelectionChanged();
      this.Repaint();
      GUIUtility.ExitGUI();
    }

    public void ReloadData()
    {
      if (this.m_TreeView == null)
        this.Init();
      else
        this.m_TreeView.ReloadData();
    }

    public void SearchChanged()
    {
      GameObjectTreeViewDataSource data = (GameObjectTreeViewDataSource) this.treeView.data;
      if ((SearchableEditorWindow.SearchMode) data.searchMode == this.searchMode && data.searchString == this.m_SearchFilter)
        return;
      data.searchMode = (int) this.searchMode;
      data.searchString = this.m_SearchFilter;
      if (this.m_SearchFilter == "")
        this.treeView.Frame(Selection.activeInstanceID, true, false);
      this.ReloadData();
    }

    private void TreeViewSelectionChanged(int[] ids)
    {
      Selection.instanceIDs = ids;
      this.m_DidSelectSearchResult = !string.IsNullOrEmpty(this.m_SearchFilter);
    }

    private bool IsTreeViewSelectionInSyncWithBackend()
    {
      if (this.m_TreeView != null)
        return this.m_TreeView.state.selectedIDs.SequenceEqual<int>((IEnumerable<int>) Selection.instanceIDs);
      return false;
    }

    private void OnSelectionChange()
    {
      if (!this.IsTreeViewSelectionInSyncWithBackend())
        this.selectionSyncNeeded = true;
      else if (SceneHierarchyWindow.s_Debug)
        Debug.Log((object) "OnSelectionChange: Selection is already in sync so no framing will happen");
    }

    private void OnHierarchyChange()
    {
      if (this.m_TreeView != null)
        this.m_TreeView.EndNameEditing(false);
      this.treeViewReloadNeeded = true;
    }

    private float DoSearchResultPathGUI()
    {
      if (!this.hasSearchFilter)
        return 0.0f;
      GUILayout.FlexibleSpace();
      Rect rect = EditorGUILayout.BeginVertical(EditorStyles.inspectorBig, new GUILayoutOption[0]);
      GUILayout.Label("Path:");
      if (this.m_TreeView.HasSelection())
      {
        int instanceID = this.m_TreeView.GetSelection()[0];
        IHierarchyProperty hierarchyProperty = (IHierarchyProperty) new HierarchyProperty(HierarchyType.GameObjects);
        hierarchyProperty.Find(instanceID, (int[]) null);
        if (hierarchyProperty.isValid)
        {
          do
          {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label((Texture) hierarchyProperty.icon);
            GUILayout.Label(hierarchyProperty.name);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
          }
          while (hierarchyProperty.Parent());
        }
      }
      EditorGUILayout.EndVertical();
      GUILayout.Space(0.0f);
      return rect.height;
    }

    private void OnEvent()
    {
      this.treeView.OnEvent();
    }

    private void DoTreeView(float searchPathHeight)
    {
      Rect treeViewRect = this.treeViewRect;
      treeViewRect.height -= searchPathHeight;
      this.treeView.OnGUI(treeViewRect, this.m_TreeViewKeyboardControlID);
    }

    private void DoToolbar()
    {
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      this.CreateGameObjectPopup();
      GUILayout.Space(6f);
      if (SceneHierarchyWindow.s_Debug)
      {
        int firstRowVisible;
        int lastRowVisible;
        this.m_TreeView.gui.GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible);
        GUILayout.Label(string.Format("{0} ({1}, {2})", (object) this.m_TreeView.data.rowCount, (object) firstRowVisible, (object) lastRowVisible), EditorStyles.miniLabel, new GUILayoutOption[0]);
        GUILayout.Space(6f);
      }
      GUILayout.FlexibleSpace();
      Event current = Event.current;
      if (this.hasSearchFilterFocus && current.type == EventType.KeyDown && (current.keyCode == KeyCode.DownArrow || current.keyCode == KeyCode.UpArrow))
      {
        GUIUtility.keyboardControl = this.m_TreeViewKeyboardControlID;
        if (this.treeView.IsLastClickedPartOfRows())
        {
          this.treeView.Frame(this.treeView.state.lastClickedID, true, false);
          this.m_DidSelectSearchResult = !string.IsNullOrEmpty(this.m_SearchFilter);
        }
        else
          this.treeView.OffsetSelection(1);
        current.Use();
      }
      this.SearchFieldGUI();
      GUILayout.Space(6f);
      if (this.hasSortMethods)
      {
        if (Application.isPlaying && ((GameObjectTreeViewDataSource) this.treeView.data).isFetchAIssue)
          GUILayout.Toggle(false, SceneHierarchyWindow.s_Styles.fetchWarning, SceneHierarchyWindow.s_Styles.MiniButton, new GUILayoutOption[0]);
        this.SortMethodsDropDown();
      }
      GUILayout.EndHorizontal();
    }

    internal override void SetSearchFilter(string searchFilter, SearchableEditorWindow.SearchMode searchMode, bool setAll)
    {
      base.SetSearchFilter(searchFilter, searchMode, setAll);
      if (!this.m_DidSelectSearchResult || !string.IsNullOrEmpty(searchFilter))
        return;
      this.m_DidSelectSearchResult = false;
      this.FrameObjectPrivate(Selection.activeInstanceID, true, false, false);
      if (GUIUtility.keyboardControl == 0)
        GUIUtility.keyboardControl = this.m_TreeViewKeyboardControlID;
    }

    private void AddCreateGameObjectItemsToMenu(GenericMenu menu, UnityEngine.Object[] context, bool includeCreateEmptyChild, bool includeGameObjectInPath, int targetSceneHandle)
    {
      foreach (string submenu in Unsupported.GetSubmenus("GameObject"))
      {
        UnityEngine.Object[] temporaryContext = context;
        if (includeCreateEmptyChild || !(submenu.ToLower() == "GameObject/Create Empty Child".ToLower()))
        {
          if (submenu.EndsWith("..."))
            temporaryContext = (UnityEngine.Object[]) null;
          if (submenu.ToLower() == "GameObject/Center On Children".ToLower())
            break;
          string replacementMenuString = submenu;
          if (!includeGameObjectInPath)
            replacementMenuString = submenu.Substring(11);
          MenuUtils.ExtractMenuItemWithPath(submenu, menu, replacementMenuString, temporaryContext, targetSceneHandle, new Action<string, UnityEngine.Object[], int>(this.BeforeCreateGameObjectMenuItemWasExecuted), new Action<string, UnityEngine.Object[], int>(this.AfterCreateGameObjectMenuItemWasExecuted));
        }
      }
    }

    private void BeforeCreateGameObjectMenuItemWasExecuted(string menuPath, UnityEngine.Object[] contextObjects, int userData)
    {
      EditorSceneManager.SetTargetSceneForNewGameObjects(userData);
    }

    private void AfterCreateGameObjectMenuItemWasExecuted(string menuPath, UnityEngine.Object[] contextObjects, int userData)
    {
      EditorSceneManager.SetTargetSceneForNewGameObjects(0);
      if (!this.m_Locked)
        return;
      this.m_FrameOnSelectionSync = true;
    }

    private void CreateGameObjectPopup()
    {
      Rect rect = GUILayoutUtility.GetRect(SceneHierarchyWindow.s_Styles.createContent, EditorStyles.toolbarDropDown, (GUILayoutOption[]) null);
      if (Event.current.type == EventType.Repaint)
        EditorStyles.toolbarDropDown.Draw(rect, SceneHierarchyWindow.s_Styles.createContent, false, false, false, false);
      if (Event.current.type != EventType.MouseDown || !rect.Contains(Event.current.mousePosition))
        return;
      GUIUtility.hotControl = 0;
      GenericMenu menu = new GenericMenu();
      this.AddCreateGameObjectItemsToMenu(menu, (UnityEngine.Object[]) null, true, false, 0);
      menu.DropDown(rect);
      Event.current.Use();
    }

    private void SortMethodsDropDown()
    {
      if (!this.hasSortMethods)
        return;
      GUIContent content = this.m_SortingObjects[this.currentSortingName].content;
      if (content == null)
      {
        content = SceneHierarchyWindow.s_Styles.defaultSortingContent;
        content.tooltip = this.currentSortingName;
      }
      Rect rect = GUILayoutUtility.GetRect(content, EditorStyles.toolbarButton);
      if (EditorGUI.DropdownButton(rect, content, FocusType.Passive, EditorStyles.toolbarButton))
      {
        List<SceneHierarchySortingWindow.InputData> data = new List<SceneHierarchySortingWindow.InputData>();
        foreach (KeyValuePair<string, HierarchySorting> sortingObject in this.m_SortingObjects)
          data.Add(new SceneHierarchySortingWindow.InputData()
          {
            m_TypeName = sortingObject.Key,
            m_Name = ObjectNames.NicifyVariableName(sortingObject.Key),
            m_Selected = sortingObject.Key == this.m_CurrentSortingName
          });
        if (SceneHierarchySortingWindow.ShowAtPosition(new Vector2(rect.x, rect.y + rect.height), data, new SceneHierarchySortingWindow.OnSelectCallback(this.SortFunctionCallback)))
          GUIUtility.ExitGUI();
      }
    }

    private void SetUpSortMethodLists()
    {
      this.m_SortingObjects = new Dictionary<string, HierarchySorting>();
      TransformSorting transformSorting = new TransformSorting();
      this.m_SortingObjects.Add(this.GetNameForType(transformSorting.GetType()), (HierarchySorting) transformSorting);
      if (this.m_AllowAlphaNumericalSort || !InternalEditorUtility.isHumanControllingUs)
      {
        AlphabeticalSorting alphabeticalSorting = new AlphabeticalSorting();
        this.m_SortingObjects.Add(this.GetNameForType(alphabeticalSorting.GetType()), (HierarchySorting) alphabeticalSorting);
      }
      this.currentSortingName = this.m_CurrentSortingName;
    }

    private string GetNameForType(System.Type type)
    {
      return type.Name;
    }

    private void SortFunctionCallback(SceneHierarchySortingWindow.InputData data)
    {
      this.SetSortFunction(data.m_TypeName);
    }

    public void SetSortFunction(System.Type sortType)
    {
      this.SetSortFunction(this.GetNameForType(sortType));
    }

    private void SetSortFunction(string sortTypeName)
    {
      if (!this.m_SortingObjects.ContainsKey(sortTypeName))
      {
        Debug.LogError((object) ("Invalid search type name: " + sortTypeName));
      }
      else
      {
        this.currentSortingName = sortTypeName;
        if (((IEnumerable<int>) this.treeView.GetSelection()).Any<int>())
          this.treeView.Frame(((IEnumerable<int>) this.treeView.GetSelection()).First<int>(), true, false);
        this.treeView.ReloadData();
      }
    }

    public void DirtySortingMethods()
    {
      this.m_AllowAlphaNumericalSort = EditorPrefs.GetBool("AllowAlphaNumericHierarchy", false);
      this.SetUpSortMethodLists();
      this.treeView.SetSelection(this.treeView.GetSelection(), true);
      this.treeView.ReloadData();
    }

    private void ExecuteCommands()
    {
      Event current = Event.current;
      if (current.type != EventType.ExecuteCommand && current.type != EventType.ValidateCommand)
        return;
      bool flag = current.type == EventType.ExecuteCommand;
      if (current.commandName == "Delete" || current.commandName == "SoftDelete")
      {
        if (flag)
          this.DeleteGO();
        current.Use();
        GUIUtility.ExitGUI();
      }
      else if (current.commandName == "Duplicate")
      {
        if (flag)
          this.DuplicateGO();
        current.Use();
        GUIUtility.ExitGUI();
      }
      else if (current.commandName == "Copy")
      {
        if (flag)
          this.CopyGO();
        current.Use();
        GUIUtility.ExitGUI();
      }
      else if (current.commandName == "Paste")
      {
        if (flag)
          this.PasteGO();
        current.Use();
        GUIUtility.ExitGUI();
      }
      else if (current.commandName == "SelectAll")
      {
        if (flag)
          this.SelectAll();
        current.Use();
        GUIUtility.ExitGUI();
      }
      else if (current.commandName == "FrameSelected")
      {
        if (current.type == EventType.ExecuteCommand)
          this.FrameObjectPrivate(Selection.activeInstanceID, true, true, true);
        current.Use();
        GUIUtility.ExitGUI();
      }
      else
      {
        if (!(current.commandName == "Find"))
          return;
        if (current.type == EventType.ExecuteCommand)
          this.FocusSearchField();
        current.Use();
      }
    }

    private void CreateGameObjectContextClick(GenericMenu menu, int contextClickedItemID)
    {
      menu.AddItem(EditorGUIUtility.TextContent("Copy"), false, new GenericMenu.MenuFunction(this.CopyGO));
      menu.AddItem(EditorGUIUtility.TextContent("Paste"), false, new GenericMenu.MenuFunction(this.PasteGO));
      menu.AddSeparator("");
      if (!this.hasSearchFilter && this.m_TreeViewState.selectedIDs.Count == 1)
        menu.AddItem(EditorGUIUtility.TextContent("Rename"), false, new GenericMenu.MenuFunction(this.RenameGO));
      else
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Rename"));
      menu.AddItem(EditorGUIUtility.TextContent("Duplicate"), false, new GenericMenu.MenuFunction(this.DuplicateGO));
      menu.AddItem(EditorGUIUtility.TextContent("Delete"), false, new GenericMenu.MenuFunction(this.DeleteGO));
      menu.AddSeparator("");
      bool flag = false;
      if (this.m_TreeViewState.selectedIDs.Count == 1)
      {
        GameObjectTreeViewItem objectTreeViewItem = this.treeView.FindItem(this.m_TreeViewState.selectedIDs[0]) as GameObjectTreeViewItem;
        if (objectTreeViewItem != null)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          SceneHierarchyWindow.\u003CCreateGameObjectContextClick\u003Ec__AnonStorey0 clickCAnonStorey0 = new SceneHierarchyWindow.\u003CCreateGameObjectContextClick\u003Ec__AnonStorey0();
          // ISSUE: reference to a compiler-generated field
          clickCAnonStorey0.prefab = PrefabUtility.GetPrefabParent(objectTreeViewItem.objectPPTR);
          // ISSUE: reference to a compiler-generated field
          if (clickCAnonStorey0.prefab != (UnityEngine.Object) null)
          {
            // ISSUE: reference to a compiler-generated method
            menu.AddItem(EditorGUIUtility.TextContent("Select Prefab"), false, new GenericMenu.MenuFunction(clickCAnonStorey0.\u003C\u003Em__0));
            flag = true;
          }
        }
      }
      if (!flag)
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Select Prefab"));
      menu.AddSeparator("");
      this.AddCreateGameObjectItemsToMenu(menu, (UnityEngine.Object[]) ((IEnumerable<Transform>) Selection.transforms).Select<Transform, GameObject>((Func<Transform, GameObject>) (t => t.gameObject)).ToArray<GameObject>(), false, false, 0);
      menu.ShowAsContext();
    }

    private void CreateMultiSceneHeaderContextClick(GenericMenu menu, int contextClickedItemID)
    {
      Scene sceneByHandle = EditorSceneManager.GetSceneByHandle(contextClickedItemID);
      if (!SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(sceneByHandle))
      {
        Debug.LogError((object) "Context clicked item is not a scene");
      }
      else
      {
        bool flag1 = SceneManager.sceneCount > 1;
        if (sceneByHandle.isLoaded)
        {
          GUIContent content = EditorGUIUtility.TextContent("Set Active Scene");
          if (flag1 && SceneManager.GetActiveScene() != sceneByHandle)
            menu.AddItem(content, false, new GenericMenu.MenuFunction2(this.SetSceneActive), (object) contextClickedItemID);
          else
            menu.AddDisabledItem(content);
          menu.AddSeparator("");
        }
        if (sceneByHandle.isLoaded)
        {
          if (!EditorApplication.isPlaying)
          {
            menu.AddItem(EditorGUIUtility.TextContent("Save Scene"), false, new GenericMenu.MenuFunction2(this.SaveSelectedScenes), (object) contextClickedItemID);
            menu.AddItem(EditorGUIUtility.TextContent("Save Scene As"), false, new GenericMenu.MenuFunction2(this.SaveSceneAs), (object) contextClickedItemID);
            if (flag1)
              menu.AddItem(EditorGUIUtility.TextContent("Save All"), false, new GenericMenu.MenuFunction2(this.SaveAllScenes), (object) contextClickedItemID);
            else
              menu.AddDisabledItem(EditorGUIUtility.TextContent("Save All"));
          }
          else
          {
            menu.AddDisabledItem(EditorGUIUtility.TextContent("Save Scene"));
            menu.AddDisabledItem(EditorGUIUtility.TextContent("Save Scene As"));
            menu.AddDisabledItem(EditorGUIUtility.TextContent("Save All"));
          }
          menu.AddSeparator("");
        }
        bool flag2 = EditorSceneManager.loadedSceneCount != this.GetNumLoadedScenesInSelection();
        if (sceneByHandle.isLoaded)
        {
          GUIContent content = EditorGUIUtility.TextContent("Unload Scene");
          if (flag2 && !EditorApplication.isPlaying && !string.IsNullOrEmpty(sceneByHandle.path))
            menu.AddItem(content, false, new GenericMenu.MenuFunction2(this.UnloadSelectedScenes), (object) contextClickedItemID);
          else
            menu.AddDisabledItem(content);
        }
        else
        {
          GUIContent content = EditorGUIUtility.TextContent("Load Scene");
          if (!EditorApplication.isPlaying)
            menu.AddItem(content, false, new GenericMenu.MenuFunction2(this.LoadSelectedScenes), (object) contextClickedItemID);
          else
            menu.AddDisabledItem(content);
        }
        GUIContent content1 = EditorGUIUtility.TextContent("Remove Scene");
        bool flag3 = this.GetSelectedScenes().Count == SceneManager.sceneCount;
        if (flag2 && !flag3 && !EditorApplication.isPlaying)
          menu.AddItem(content1, false, new GenericMenu.MenuFunction2(this.RemoveSelectedScenes), (object) contextClickedItemID);
        else
          menu.AddDisabledItem(content1);
        if (sceneByHandle.isLoaded)
        {
          GUIContent content2 = EditorGUIUtility.TextContent("Discard changes");
          if (!EditorApplication.isPlaying && this.GetModifiedScenes(this.GetSelectedScenes()).Length > 0)
            menu.AddItem(content2, false, new GenericMenu.MenuFunction2(this.DiscardChangesInSelectedScenes), (object) contextClickedItemID);
          else
            menu.AddDisabledItem(content2);
        }
        menu.AddSeparator("");
        GUIContent content3 = EditorGUIUtility.TextContent("Select Scene Asset");
        if (!string.IsNullOrEmpty(sceneByHandle.path))
          menu.AddItem(content3, false, new GenericMenu.MenuFunction2(this.SelectSceneAsset), (object) contextClickedItemID);
        else
          menu.AddDisabledItem(content3);
        GUIContent content4 = EditorGUIUtility.TextContent("Add New Scene");
        if (!EditorApplication.isPlaying)
          menu.AddItem(content4, false, new GenericMenu.MenuFunction2(this.AddNewScene), (object) contextClickedItemID);
        else
          menu.AddDisabledItem(content4);
        if (!sceneByHandle.isLoaded)
          return;
        menu.AddSeparator("");
        this.AddCreateGameObjectItemsToMenu(menu, (UnityEngine.Object[]) ((IEnumerable<Transform>) Selection.transforms).Select<Transform, GameObject>((Func<Transform, GameObject>) (t => t.gameObject)).ToArray<GameObject>(), false, true, sceneByHandle.handle);
      }
    }

    private int GetNumLoadedScenesInSelection()
    {
      int num = 0;
      foreach (int selectedScene in this.GetSelectedScenes())
      {
        if (EditorSceneManager.GetSceneByHandle(selectedScene).isLoaded)
          ++num;
      }
      return num;
    }

    private List<int> GetSelectedScenes()
    {
      List<int> intList = new List<int>();
      foreach (int handle in this.m_TreeView.GetSelection())
      {
        if (SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(EditorSceneManager.GetSceneByHandle(handle)))
          intList.Add(handle);
      }
      return intList;
    }

    private void ContextClickOutsideItems()
    {
      Event.current.Use();
      GenericMenu menu = new GenericMenu();
      this.CreateGameObjectContextClick(menu, 0);
      menu.ShowAsContext();
    }

    private void ItemContextClick(int contextClickedItemID)
    {
      Event.current.Use();
      GenericMenu menu = new GenericMenu();
      if (SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(EditorSceneManager.GetSceneByHandle(contextClickedItemID)))
        this.CreateMultiSceneHeaderContextClick(menu, contextClickedItemID);
      else
        this.CreateGameObjectContextClick(menu, contextClickedItemID);
      menu.ShowAsContext();
    }

    private void CopyGO()
    {
      Unsupported.CopyGameObjectsToPasteboard();
    }

    private void PasteGO()
    {
      Unsupported.PasteGameObjectsFromPasteboard();
    }

    private void DuplicateGO()
    {
      Unsupported.DuplicateGameObjectsUsingPasteboard();
    }

    private void RenameGO()
    {
      this.treeView.BeginNameEditing(0.0f);
    }

    private void DeleteGO()
    {
      Unsupported.DeleteGameObjectSelection();
    }

    private void SetSceneActive(object userData)
    {
      SceneManager.SetActiveScene(EditorSceneManager.GetSceneByHandle((int) userData));
    }

    private void LoadSelectedScenes(object userdata)
    {
      foreach (int selectedScene in this.GetSelectedScenes())
      {
        Scene sceneByHandle = EditorSceneManager.GetSceneByHandle(selectedScene);
        if (!sceneByHandle.isLoaded)
          EditorSceneManager.OpenScene(sceneByHandle.path, OpenSceneMode.Additive);
      }
      EditorApplication.RequestRepaintAllViews();
    }

    private void SaveSceneAs(object userdata)
    {
      Scene sceneByHandle = EditorSceneManager.GetSceneByHandle((int) userdata);
      if (!sceneByHandle.isLoaded)
        return;
      EditorSceneManager.SaveSceneAs(sceneByHandle);
    }

    private void SaveAllScenes(object userdata)
    {
      EditorSceneManager.SaveOpenScenes();
    }

    private void SaveSelectedScenes(object userdata)
    {
      foreach (int selectedScene in this.GetSelectedScenes())
      {
        Scene sceneByHandle = EditorSceneManager.GetSceneByHandle(selectedScene);
        if (sceneByHandle.isLoaded)
          EditorSceneManager.SaveScene(sceneByHandle);
      }
    }

    private void UnloadSelectedScenes(object userdata)
    {
      this.CloseSelectedScenes(false);
    }

    private void RemoveSelectedScenes(object userData)
    {
      this.CloseSelectedScenes(true);
    }

    private bool UserAllowedDiscardingChanges(Scene[] modifiedScenes)
    {
      return EditorUtility.DisplayDialog("Discard Changes", string.Format("Are you sure you want to discard the changes in the following scenes:\n\n   {0}\n\nYour changes will be lost.", (object) string.Join("\n   ", ((IEnumerable<Scene>) modifiedScenes).Select<Scene, string>((Func<Scene, string>) (scene => scene.name)).ToArray<string>())), "OK", "Cancel");
    }

    private void DiscardChangesInSelectedScenes(object userData)
    {
      IEnumerable<string> expandedSceneNames = this.GetExpandedSceneNames();
      Scene[] modifiedScenes = this.GetModifiedScenes(this.GetSelectedScenes());
      Scene[] array = ((IEnumerable<Scene>) modifiedScenes).Where<Scene>((Func<Scene, bool>) (scene => !string.IsNullOrEmpty(scene.path))).ToArray<Scene>();
      if (!this.UserAllowedDiscardingChanges(array))
        return;
      if (array.Length != modifiedScenes.Length)
        Debug.LogWarning((object) "Discarding changes in a scene that have not yet been saved is not supported. Save the scene first or create a new scene.");
      foreach (Scene scene in array)
        EditorSceneManager.ReloadScene(scene);
      if (SceneManager.sceneCount == 1)
        this.SetScenesExpanded(expandedSceneNames.ToList<string>());
      EditorApplication.RequestRepaintAllViews();
    }

    private Scene[] GetModifiedScenes(List<int> handles)
    {
      return handles.Select<int, Scene>((Func<int, Scene>) (handle => EditorSceneManager.GetSceneByHandle(handle))).Where<Scene>((Func<Scene, bool>) (scene => scene.isDirty)).ToArray<Scene>();
    }

    private void CloseSelectedScenes(bool removeScenes)
    {
      List<int> selectedScenes = this.GetSelectedScenes();
      if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(this.GetModifiedScenes(selectedScenes)))
        return;
      foreach (int handle in selectedScenes)
        EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByHandle(handle), removeScenes);
      EditorApplication.RequestRepaintAllViews();
    }

    private void AddNewScene(object userData)
    {
      Scene sceneByPath = SceneManager.GetSceneByPath("");
      if (sceneByPath.IsValid() && (!EditorUtility.DisplayDialog(EditorGUIUtility.TextContent("Save Untitled Scene").text, EditorGUIUtility.TextContent("Existing Untitled scene needs to be saved before creating a new scene. Only one untitled scene is supported at a time.").text, "Save", "Cancel") || !EditorSceneManager.SaveScene(sceneByPath)))
        return;
      Scene src = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Additive);
      Scene sceneByHandle = EditorSceneManager.GetSceneByHandle((int) userData);
      if (!sceneByHandle.IsValid())
        return;
      EditorSceneManager.MoveSceneAfter(src, sceneByHandle);
    }

    private void SelectSceneAsset(object userData)
    {
      int instanceIdFromGuid = AssetDatabase.GetInstanceIDFromGUID(AssetDatabase.AssetPathToGUID(EditorSceneManager.GetSceneByHandle((int) userData).path));
      Selection.activeInstanceID = instanceIdFromGuid;
      EditorGUIUtility.PingObject(instanceIdFromGuid);
    }

    private void SelectAll()
    {
      int[] rowIds = this.treeView.GetRowIDs();
      this.treeView.SetSelection(rowIds, false);
      this.TreeViewSelectionChanged(rowIds);
    }

    private static void ToggleDebugMode()
    {
      SceneHierarchyWindow.s_Debug = !SceneHierarchyWindow.s_Debug;
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      if (!Unsupported.IsDeveloperBuild())
        return;
      GenericMenu genericMenu = menu;
      GUIContent content = new GUIContent("DEVELOPER/Toggle DebugMode");
      int num = 0;
      // ISSUE: reference to a compiler-generated field
      if (SceneHierarchyWindow.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SceneHierarchyWindow.\u003C\u003Ef__mg\u0024cache0 = new GenericMenu.MenuFunction(SceneHierarchyWindow.ToggleDebugMode);
      }
      // ISSUE: reference to a compiler-generated field
      GenericMenu.MenuFunction fMgCache0 = SceneHierarchyWindow.\u003C\u003Ef__mg\u0024cache0;
      genericMenu.AddItem(content, num != 0, fMgCache0);
    }

    public void FrameObject(int instanceID, bool ping)
    {
      this.FrameObjectPrivate(instanceID, true, ping, true);
    }

    private void FrameObjectPrivate(int instanceID, bool frame, bool ping, bool animatedFraming)
    {
      if (instanceID == 0)
        return;
      if (this.m_LastFramedID != instanceID)
        this.treeView.EndPing();
      this.SetSearchFilter("", SearchableEditorWindow.SearchMode.All, true);
      this.m_LastFramedID = instanceID;
      this.treeView.Frame(instanceID, frame, ping, animatedFraming);
      this.FrameObjectPrivate(InternalEditorUtility.GetGameObjectInstanceIDFromComponent(instanceID), frame, ping, animatedFraming);
    }

    protected virtual void ShowButton(Rect r)
    {
      if (SceneHierarchyWindow.s_Styles == null)
        SceneHierarchyWindow.s_Styles = new SceneHierarchyWindow.Styles();
      this.m_Locked = GUI.Toggle(r, this.m_Locked, GUIContent.none, SceneHierarchyWindow.s_Styles.lockButton);
    }

    private class Styles
    {
      public GUIContent defaultSortingContent = new GUIContent((Texture) EditorGUIUtility.FindTexture("CustomSorting"));
      public GUIContent createContent = new GUIContent("Create");
      public GUIContent fetchWarning = new GUIContent("", (Texture) EditorGUIUtility.FindTexture("console.warnicon.sml"), "The current sorting method is taking a lot of time. Consider using 'Transform Sort' in playmode for better performance.");
      public GUIStyle lockButton = (GUIStyle) "IN LockButton";
      private const string kCustomSorting = "CustomSorting";
      private const string kWarningSymbol = "console.warnicon.sml";
      private const string kWarningMessage = "The current sorting method is taking a lot of time. Consider using 'Transform Sort' in playmode for better performance.";
      public GUIStyle MiniButton;

      public Styles()
      {
        this.MiniButton = (GUIStyle) "ToolbarButton";
      }
    }
  }
}
