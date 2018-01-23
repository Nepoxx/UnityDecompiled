// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameObjectTreeViewDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  internal class GameObjectTreeViewDataSource : LazyTreeViewDataSource
  {
    private string m_SearchString = "";
    private int m_SearchMode = 0;
    private double m_LastFetchTime = 0.0;
    private int m_DelayedFetches = 0;
    private List<GameObjectTreeViewItem> m_StickySceneHeaderItems = new List<GameObjectTreeViewItem>();
    public HierarchySorting sortingState = (HierarchySorting) new TransformSorting();
    private const double k_LongFetchTime = 0.05;
    private const double k_FetchDelta = 0.1;
    private const int k_MaxDelayedFetch = 5;
    private const HierarchyType k_HierarchyType = HierarchyType.GameObjects;
    private const int k_DefaultStartCapacity = 1000;
    private int m_RootInstanceID;
    private bool m_NeedsChildParentReferenceSetup;
    private bool m_RowsPartiallyInitialized;
    private int m_RowCount;
    private List<TreeViewItem> m_ListOfRows;

    public GameObjectTreeViewDataSource(TreeViewController treeView, int rootInstanceID, bool showRoot, bool rootItemIsCollapsable)
      : base(treeView)
    {
      this.m_RootInstanceID = rootInstanceID;
      this.showRootItem = showRoot;
      this.rootIsCollapsable = rootItemIsCollapsable;
    }

    public List<GameObjectTreeViewItem> sceneHeaderItems
    {
      get
      {
        return this.m_StickySceneHeaderItems;
      }
    }

    public string searchString
    {
      get
      {
        return this.m_SearchString;
      }
      set
      {
        if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(this.m_SearchString))
          this.ClearSearchFilter();
        this.m_SearchString = value;
      }
    }

    public int searchMode
    {
      get
      {
        return this.m_SearchMode;
      }
      set
      {
        this.m_SearchMode = value;
      }
    }

    public bool isFetchAIssue
    {
      get
      {
        return this.m_DelayedFetches >= 5;
      }
    }

    internal void SetupChildParentReferencesIfNeeded()
    {
      this.EnsureFullyInitialized();
      if (!this.m_NeedsChildParentReferenceSetup)
        return;
      this.m_NeedsChildParentReferenceSetup = false;
      TreeViewUtility.SetChildParentReferences(this.GetRows(), this.m_RootItem);
    }

    public void EnsureFullyInitialized()
    {
      if (!this.m_RowsPartiallyInitialized)
        return;
      this.InitializeFull();
      this.m_RowsPartiallyInitialized = false;
    }

    public override int rowCount
    {
      get
      {
        return this.m_RowCount;
      }
    }

    public override void RevealItem(int itemID)
    {
      if (!this.IsValidHierarchyInstanceID(itemID))
        return;
      base.RevealItem(itemID);
    }

    public override bool IsRevealed(int id)
    {
      return this.GetRow(id) != -1;
    }

    private bool IsValidHierarchyInstanceID(int instanceID)
    {
      return SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(EditorSceneManager.GetSceneByHandle(instanceID)) || InternalEditorUtility.GetTypeWithoutLoadingObject(instanceID) == typeof (GameObject);
    }

    private HierarchyProperty FindHierarchyProperty(int instanceID)
    {
      if (!this.IsValidHierarchyInstanceID(instanceID))
        return (HierarchyProperty) null;
      HierarchyProperty hierarchyProperty = this.CreateHierarchyProperty();
      if (hierarchyProperty.Find(instanceID, this.m_TreeView.state.expandedIDs.ToArray()))
        return hierarchyProperty;
      return (HierarchyProperty) null;
    }

    public override int GetRow(int id)
    {
      if (!string.IsNullOrEmpty(this.m_SearchString))
        return base.GetRow(id);
      HierarchyProperty hierarchyProperty = this.FindHierarchyProperty(id);
      if (hierarchyProperty != null)
        return hierarchyProperty.row;
      return -1;
    }

    public override TreeViewItem GetItem(int row)
    {
      return this.m_Rows[row];
    }

    public override IList<TreeViewItem> GetRows()
    {
      this.InitIfNeeded();
      this.EnsureFullyInitialized();
      return this.m_Rows;
    }

    public override TreeViewItem FindItem(int id)
    {
      this.RevealItem(id);
      this.SetupChildParentReferencesIfNeeded();
      return base.FindItem(id);
    }

    private HierarchyProperty CreateHierarchyProperty()
    {
      HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.GameObjects);
      hierarchyProperty.Reset();
      hierarchyProperty.alphaSorted = this.IsUsingAlphaSort();
      return hierarchyProperty;
    }

    private void CreateRootItem(HierarchyProperty property)
    {
      int depth = 0;
      if (property.isValid)
        this.m_RootItem = (TreeViewItem) new GameObjectTreeViewItem(this.m_RootInstanceID, depth, (TreeViewItem) null, property.name);
      else
        this.m_RootItem = (TreeViewItem) new GameObjectTreeViewItem(this.m_RootInstanceID, depth, (TreeViewItem) null, "RootOfAll");
      if (this.showRootItem)
        return;
      this.SetExpanded(this.m_RootItem, true);
    }

    private void ClearSearchFilter()
    {
      this.CreateHierarchyProperty().SetSearchFilter("", 0);
    }

    public override void FetchData()
    {
      Profiler.BeginSample("SceneHierarchyWindow.FetchData");
      this.m_RowsPartiallyInitialized = false;
      double timeSinceStartup1 = EditorApplication.timeSinceStartup;
      HierarchyProperty hierarchyProperty = this.CreateHierarchyProperty();
      if (this.m_RootInstanceID != 0 && !hierarchyProperty.Find(this.m_RootInstanceID, (int[]) null))
      {
        Debug.LogError((object) ("Root gameobject with id " + (object) this.m_RootInstanceID + " not found!!"));
        this.m_RootInstanceID = 0;
        hierarchyProperty.Reset();
      }
      this.CreateRootItem(hierarchyProperty);
      this.m_NeedRefreshRows = false;
      this.m_NeedsChildParentReferenceSetup = true;
      bool subTreeWanted = this.m_RootInstanceID != 0;
      bool isSearching = !string.IsNullOrEmpty(this.m_SearchString);
      if (isSearching || subTreeWanted)
      {
        if (isSearching)
          hierarchyProperty.SetSearchFilter(this.m_SearchString, this.m_SearchMode);
        this.InitializeProgressivly(hierarchyProperty, subTreeWanted, isSearching);
      }
      else
        this.InitializeMinimal();
      double timeSinceStartup2 = EditorApplication.timeSinceStartup;
      double num = timeSinceStartup2 - timeSinceStartup1;
      if (timeSinceStartup2 - this.m_LastFetchTime > 0.1 && num > 0.05)
        ++this.m_DelayedFetches;
      else
        this.m_DelayedFetches = 0;
      this.m_LastFetchTime = timeSinceStartup1;
      this.m_TreeView.SetSelection(Selection.instanceIDs, false);
      this.CreateSceneHeaderItems();
      if (SceneHierarchyWindow.s_Debug)
        Debug.Log((object) ("Fetch time: " + (object) (num * 1000.0) + " ms, alphaSort = " + (object) this.IsUsingAlphaSort()));
      Profiler.EndSample();
    }

    public override bool CanBeParent(TreeViewItem item)
    {
      this.SetupChildParentReferencesIfNeeded();
      return base.CanBeParent(item);
    }

    private bool IsUsingAlphaSort()
    {
      return this.sortingState.GetType() == typeof (AlphabeticalSorting);
    }

    private static void Resize(List<TreeViewItem> list, int count)
    {
      int count1 = list.Count;
      if (count < count1)
      {
        list.RemoveRange(count, count1 - count);
      }
      else
      {
        if (count <= count1)
          return;
        if (count > list.Capacity)
          list.Capacity = count + 20;
        list.AddRange(Enumerable.Repeat<TreeViewItem>((TreeViewItem) null, count - count1));
      }
    }

    private void ResizeItemList(int count)
    {
      this.AllocateBackingArrayIfNeeded();
      if (this.m_ListOfRows.Count == count)
        return;
      GameObjectTreeViewDataSource.Resize(this.m_ListOfRows, count);
    }

    private void AllocateBackingArrayIfNeeded()
    {
      if (this.m_Rows != null)
        return;
      this.m_ListOfRows = new List<TreeViewItem>(this.m_RowCount <= 1000 ? 1000 : this.m_RowCount);
      this.m_Rows = (IList<TreeViewItem>) this.m_ListOfRows;
    }

    private void InitializeMinimal()
    {
      int[] array = this.m_TreeView.state.expandedIDs.ToArray();
      HierarchyProperty hierarchyProperty = this.CreateHierarchyProperty();
      this.m_RowCount = hierarchyProperty.CountRemaining(array);
      this.ResizeItemList(this.m_RowCount);
      hierarchyProperty.Reset();
      if (SceneHierarchyWindow.debug)
        GameObjectTreeViewDataSource.Log("Init minimal (" + (object) this.m_RowCount + ")");
      int firstRowVisible;
      int lastRowVisible;
      this.m_TreeView.gui.GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible);
      this.InitializeRows(hierarchyProperty, firstRowVisible, lastRowVisible);
      this.m_RowsPartiallyInitialized = true;
    }

    private void InitializeFull()
    {
      if (SceneHierarchyWindow.debug)
        GameObjectTreeViewDataSource.Log("Init full (" + (object) this.m_RowCount + ")");
      HierarchyProperty hierarchyProperty = this.CreateHierarchyProperty();
      this.m_RowCount = hierarchyProperty.CountRemaining(this.m_TreeView.state.expandedIDs.ToArray());
      this.ResizeItemList(this.m_RowCount);
      hierarchyProperty.Reset();
      this.InitializeRows(hierarchyProperty, 0, this.m_RowCount - 1);
    }

    private void InitializeProgressivly(HierarchyProperty property, bool subTreeWanted, bool isSearching)
    {
      this.AllocateBackingArrayIfNeeded();
      int num1 = !subTreeWanted ? 0 : property.depth + 1;
      if (!isSearching)
      {
        int row = 0;
        int[] array = this.expandedIDs.ToArray();
        int num2 = !subTreeWanted ? 0 : property.depth + 1;
        while (property.NextWithDepthCheck(array, num1))
        {
          this.InitTreeViewItem(this.EnsureCreatedItem(row), property, property.hasChildren, property.depth - num2);
          ++row;
        }
        this.m_RowCount = row;
      }
      else
        this.m_RowCount = this.InitializeSearchResults(property, num1);
      this.ResizeItemList(this.m_RowCount);
    }

    private int InitializeSearchResults(HierarchyProperty property, int minAllowedDepth)
    {
      int currentSceneHandle = -1;
      int row = 0;
      List<int> intList = new List<int>();
      while (property.NextWithDepthCheck((int[]) null, minAllowedDepth))
      {
        GameObjectTreeViewItem objectTreeViewItem = this.EnsureCreatedItem(row);
        if (this.AddSceneHeaderToSearchIfNeeded(objectTreeViewItem, property, ref currentSceneHandle))
        {
          ++row;
          intList.Add(row);
          if (!this.IsSceneHeader(property))
            objectTreeViewItem = this.EnsureCreatedItem(row);
          else
            continue;
        }
        this.InitTreeViewItem(objectTreeViewItem, property, false, 0);
        ++row;
      }
      int num = row;
      if (intList.Count > 0)
      {
        int index1 = intList[0];
        for (int index2 = 1; index2 < intList.Count; ++index2)
        {
          int count = intList[index2] - index1 - 1;
          this.m_ListOfRows.Sort(index1, count, (IComparer<TreeViewItem>) new TreeViewItemAlphaNumericSort());
          index1 = intList[index2];
        }
        this.m_ListOfRows.Sort(index1, num - index1, (IComparer<TreeViewItem>) new TreeViewItemAlphaNumericSort());
      }
      return num;
    }

    private bool AddSceneHeaderToSearchIfNeeded(GameObjectTreeViewItem item, HierarchyProperty property, ref int currentSceneHandle)
    {
      Scene scene = property.GetScene();
      if (currentSceneHandle == scene.handle)
        return false;
      currentSceneHandle = scene.handle;
      this.InitTreeViewItem(item, scene.handle, scene, true, 0, (Object) null, false, 0);
      return true;
    }

    private GameObjectTreeViewItem EnsureCreatedItem(int row)
    {
      if (row >= this.m_Rows.Count)
        this.m_Rows.Add((TreeViewItem) null);
      GameObjectTreeViewItem objectTreeViewItem = (GameObjectTreeViewItem) this.m_Rows[row];
      if (objectTreeViewItem == null)
      {
        objectTreeViewItem = new GameObjectTreeViewItem(0, 0, (TreeViewItem) null, (string) null);
        this.m_Rows[row] = (TreeViewItem) objectTreeViewItem;
      }
      return objectTreeViewItem;
    }

    private void InitializeRows(HierarchyProperty property, int firstRow, int lastRow)
    {
      property.Reset();
      int[] array = this.expandedIDs.ToArray();
      if (firstRow > 0)
      {
        int count = firstRow;
        if (!property.Skip(count, array))
          Debug.LogError((object) ("Failed to skip " + (object) count));
      }
      for (int row = firstRow; property.Next(array) && row <= lastRow; ++row)
        this.InitTreeViewItem(this.EnsureCreatedItem(row), property, property.hasChildren, property.depth);
    }

    private void InitTreeViewItem(GameObjectTreeViewItem item, HierarchyProperty property, bool itemHasChildren, int itemDepth)
    {
      this.InitTreeViewItem(item, property.instanceID, property.GetScene(), this.IsSceneHeader(property), property.colorCode, property.pptrValue, itemHasChildren, itemDepth);
    }

    private void InitTreeViewItem(GameObjectTreeViewItem item, int itemID, Scene scene, bool isSceneHeader, int colorCode, Object pptrObject, bool hasChildren, int depth)
    {
      item.children = (List<TreeViewItem>) null;
      item.id = itemID;
      item.depth = depth;
      item.parent = (TreeViewItem) null;
      if (isSceneHeader)
        item.displayName = !string.IsNullOrEmpty(scene.name) ? scene.name : "Untitled";
      else
        item.displayName = (string) null;
      item.colorCode = colorCode;
      item.objectPPTR = pptrObject;
      item.shouldDisplay = true;
      item.isSceneHeader = isSceneHeader;
      item.scene = scene;
      item.icon = !isSceneHeader ? (Texture2D) null : EditorGUIUtility.FindTexture("SceneAsset Icon");
      if (!hasChildren)
        return;
      item.children = LazyTreeViewDataSource.CreateChildListForCollapsedParent();
    }

    private bool IsSceneHeader(HierarchyProperty property)
    {
      return property.pptrValue == (Object) null;
    }

    protected override HashSet<int> GetParentsAbove(int id)
    {
      HashSet<int> intSet = new HashSet<int>();
      if (!this.IsValidHierarchyInstanceID(id))
        return intSet;
      IHierarchyProperty hierarchyProperty = (IHierarchyProperty) new HierarchyProperty(HierarchyType.GameObjects);
      if (hierarchyProperty.Find(id, (int[]) null))
      {
        while (hierarchyProperty.Parent())
          intSet.Add(hierarchyProperty.instanceID);
      }
      return intSet;
    }

    protected override HashSet<int> GetParentsBelow(int id)
    {
      HashSet<int> intSet = new HashSet<int>();
      if (!this.IsValidHierarchyInstanceID(id))
        return intSet;
      IHierarchyProperty hierarchyProperty = (IHierarchyProperty) new HierarchyProperty(HierarchyType.GameObjects);
      if (hierarchyProperty.Find(id, (int[]) null))
      {
        intSet.Add(id);
        int depth = hierarchyProperty.depth;
        while (hierarchyProperty.Next((int[]) null) && hierarchyProperty.depth > depth)
        {
          if (hierarchyProperty.hasChildren)
            intSet.Add(hierarchyProperty.instanceID);
        }
      }
      return intSet;
    }

    private static void Log(string text)
    {
      Debug.Log((object) text);
    }

    private void CreateSceneHeaderItems()
    {
      this.m_StickySceneHeaderItems.Clear();
      int sceneCount = SceneManager.sceneCount;
      for (int index = 0; index < sceneCount; ++index)
      {
        Scene sceneAt = SceneManager.GetSceneAt(index);
        GameObjectTreeViewItem objectTreeViewItem = new GameObjectTreeViewItem(0, 0, (TreeViewItem) null, (string) null);
        this.InitTreeViewItem(objectTreeViewItem, sceneAt.handle, sceneAt, true, 0, (Object) null, false, 0);
        this.m_StickySceneHeaderItems.Add(objectTreeViewItem);
      }
    }
  }
}
