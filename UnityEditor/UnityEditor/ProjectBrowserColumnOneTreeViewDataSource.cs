// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectBrowserColumnOneTreeViewDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class ProjectBrowserColumnOneTreeViewDataSource : TreeViewDataSource
  {
    private static string kProjectBrowserString = "ProjectBrowser";

    public ProjectBrowserColumnOneTreeViewDataSource(TreeViewController treeView)
      : base(treeView)
    {
      this.showRootItem = false;
      this.rootIsCollapsable = false;
      SavedSearchFilters.AddChangeListener(new Action(((TreeViewDataSource) this).ReloadData));
    }

    public override bool SetExpanded(int id, bool expand)
    {
      if (!base.SetExpanded(id, expand))
        return false;
      InternalEditorUtility.expandedProjectWindowItems = this.expandedIDs.ToArray();
      if (this.m_RootItem.hasChildren)
      {
        foreach (TreeViewItem child in this.m_RootItem.children)
        {
          if (child.id == id)
            EditorPrefs.SetBool(ProjectBrowserColumnOneTreeViewDataSource.kProjectBrowserString + child.displayName, expand);
        }
      }
      return true;
    }

    public override bool IsExpandable(TreeViewItem item)
    {
      return item.hasChildren && (item != this.m_RootItem || this.rootIsCollapsable);
    }

    public override bool CanBeMultiSelected(TreeViewItem item)
    {
      return ProjectBrowser.GetItemType(item.id) != ProjectBrowser.ItemType.SavedFilter;
    }

    public override bool CanBeParent(TreeViewItem item)
    {
      return !(item is SearchFilterTreeItem) || SavedSearchFilters.AllowsHierarchy();
    }

    public bool IsVisibleRootNode(TreeViewItem item)
    {
      return item.parent != null && item.parent.parent == null;
    }

    public override bool IsRenamingItemAllowed(TreeViewItem item)
    {
      if (this.IsVisibleRootNode(item))
        return false;
      return base.IsRenamingItemAllowed(item);
    }

    public static int GetAssetsFolderInstanceID()
    {
      return AssetDatabase.GetInstanceIDFromGUID(AssetDatabase.AssetPathToGUID("Assets"));
    }

    public override void FetchData()
    {
      this.m_RootItem = new TreeViewItem(int.MaxValue, 0, (TreeViewItem) null, "Invisible Root Item");
      this.SetExpanded(this.m_RootItem, true);
      List<TreeViewItem> treeViewItemList = new List<TreeViewItem>();
      int folderInstanceId = ProjectBrowserColumnOneTreeViewDataSource.GetAssetsFolderInstanceID();
      int depth = 0;
      string displayName = "Assets";
      TreeViewItem parent1 = new TreeViewItem(folderInstanceId, depth, this.m_RootItem, displayName);
      this.ReadAssetDatabase(HierarchyType.Assets, parent1, depth + 1);
      TreeViewItem parent2 = (TreeViewItem) null;
      if (Unsupported.IsDeveloperBuild() && EditorPrefs.GetBool("ShowPackagesFolder"))
      {
        int instanceIdFromGuid = AssetDatabase.GetInstanceIDFromGUID(AssetDatabase.AssetPathToGUID(AssetDatabase.GetPackagesMountPoint()));
        string packagesMountPoint = AssetDatabase.GetPackagesMountPoint();
        parent2 = new TreeViewItem(instanceIdFromGuid, depth, this.m_RootItem, packagesMountPoint);
        this.ReadAssetDatabase(HierarchyType.Packages, parent2, depth + 1);
      }
      TreeViewItem treeView = SavedSearchFilters.ConvertToTreeView();
      treeView.parent = this.m_RootItem;
      treeViewItemList.Add(treeView);
      treeViewItemList.Add(parent1);
      if (parent2 != null)
        treeViewItemList.Add(parent2);
      this.m_RootItem.children = treeViewItemList;
      foreach (TreeViewItem child in this.m_RootItem.children)
      {
        bool expand = EditorPrefs.GetBool(ProjectBrowserColumnOneTreeViewDataSource.kProjectBrowserString + child.displayName, true);
        this.SetExpanded(child, expand);
      }
      this.m_NeedRefreshRows = true;
    }

    private void ReadAssetDatabase(HierarchyType htype, TreeViewItem parent, int baseDepth)
    {
      IHierarchyProperty hierarchyProperty = (IHierarchyProperty) new HierarchyProperty(htype);
      hierarchyProperty.Reset();
      Texture2D texture1 = EditorGUIUtility.FindTexture(EditorResourcesUtility.folderIconName);
      Texture2D texture2 = EditorGUIUtility.FindTexture(EditorResourcesUtility.emptyFolderIconName);
      List<TreeViewItem> treeViewItemList = new List<TreeViewItem>();
      while (hierarchyProperty.Next((int[]) null))
      {
        if (hierarchyProperty.isFolder)
          treeViewItemList.Add(new TreeViewItem(hierarchyProperty.instanceID, baseDepth + hierarchyProperty.depth, (TreeViewItem) null, hierarchyProperty.name)
          {
            icon = !hierarchyProperty.hasChildren ? texture2 : texture1
          });
      }
      TreeViewUtility.SetChildParentReferences((IList<TreeViewItem>) treeViewItemList, parent);
    }
  }
}
