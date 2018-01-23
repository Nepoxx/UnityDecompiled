// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectBrowserColumnOneTreeViewDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.IO;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  internal class ProjectBrowserColumnOneTreeViewDragging : AssetsTreeViewDragging
  {
    public ProjectBrowserColumnOneTreeViewDragging(TreeViewController treeView)
      : base(treeView)
    {
    }

    public override void StartDrag(TreeViewItem draggedItem, List<int> draggedItemIDs)
    {
      if (SavedSearchFilters.IsSavedFilter(draggedItem.id) && draggedItem.id == SavedSearchFilters.GetRootInstanceID())
        return;
      ProjectWindowUtil.StartDrag(draggedItem.id, draggedItemIDs);
    }

    public override DragAndDropVisualMode DoDrag(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
    {
      if (targetItem == null)
        return DragAndDropVisualMode.None;
      object genericData = DragAndDrop.GetGenericData(ProjectWindowUtil.k_DraggingFavoriteGenericData);
      if (genericData != null)
      {
        int instanceID = (int) genericData;
        if (!(targetItem is SearchFilterTreeItem) || !(parentItem is SearchFilterTreeItem))
          return DragAndDropVisualMode.None;
        bool flag = SavedSearchFilters.CanMoveSavedFilter(instanceID, parentItem.id, targetItem.id, dropPos == TreeViewDragging.DropPosition.Below);
        if (flag && perform)
        {
          SavedSearchFilters.MoveSavedFilter(instanceID, parentItem.id, targetItem.id, dropPos == TreeViewDragging.DropPosition.Below);
          this.m_TreeView.SetSelection(new int[1]
          {
            instanceID
          }, false);
          this.m_TreeView.NotifyListenersThatSelectionChanged();
        }
        return !flag ? DragAndDropVisualMode.None : DragAndDropVisualMode.Copy;
      }
      if (!(targetItem is SearchFilterTreeItem) || !(parentItem is SearchFilterTreeItem))
        return base.DoDrag(parentItem, targetItem, perform, dropPos);
      if (!(DragAndDrop.GetGenericData(ProjectWindowUtil.k_IsFolderGenericData) as string == "isFolder"))
        return DragAndDropVisualMode.None;
      if (perform)
      {
        Object[] objectReferences = DragAndDrop.objectReferences;
        if (objectReferences.Length > 0)
        {
          string assetPath = AssetDatabase.GetAssetPath(objectReferences[0].GetInstanceID());
          if (!string.IsNullOrEmpty(assetPath))
          {
            string name = new DirectoryInfo(assetPath).Name;
            SearchFilter filter = new SearchFilter();
            filter.folders = new string[1]{ assetPath };
            bool addAsChild = targetItem == parentItem;
            float listAreaGridSize = ProjectBrowserColumnOneTreeViewGUI.GetListAreaGridSize();
            this.m_TreeView.SetSelection(new int[1]
            {
              SavedSearchFilters.AddSavedFilterAfterInstanceID(name, filter, listAreaGridSize, targetItem.id, addAsChild)
            }, false);
            this.m_TreeView.NotifyListenersThatSelectionChanged();
          }
          else
            Debug.Log((object) ("Could not get asset path from id " + (object) objectReferences[0].GetInstanceID()));
        }
      }
      return DragAndDropVisualMode.Copy;
    }
  }
}
