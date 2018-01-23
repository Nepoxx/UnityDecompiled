// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameObjectsTreeViewDragging
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
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  internal class GameObjectsTreeViewDragging : TreeViewDragging
  {
    private const string kSceneHeaderDragString = "SceneHeaderList";

    public GameObjectsTreeViewDragging(TreeViewController treeView)
      : base(treeView)
    {
    }

    public override void StartDrag(TreeViewItem draggedItem, List<int> draggedItemIDs)
    {
      DragAndDrop.PrepareStartDrag();
      draggedItemIDs = this.m_TreeView.SortIDsInVisiblityOrder((IList<int>) draggedItemIDs);
      if (!draggedItemIDs.Contains(draggedItem.id))
        draggedItemIDs = new List<int>() { draggedItem.id };
      UnityEngine.Object[] dragAndDropObjects = ProjectWindowUtil.GetDragAndDropObjects(draggedItem.id, draggedItemIDs);
      DragAndDrop.objectReferences = dragAndDropObjects;
      List<Scene> draggedScenes = this.GetDraggedScenes(draggedItemIDs);
      if (draggedScenes != null)
      {
        DragAndDrop.SetGenericData("SceneHeaderList", (object) draggedScenes);
        List<string> stringList = new List<string>();
        foreach (Scene scene in draggedScenes)
        {
          if (scene.path.Length > 0)
            stringList.Add(scene.path);
        }
        DragAndDrop.paths = stringList.ToArray();
      }
      else
        DragAndDrop.paths = new string[0];
      string title;
      if (draggedItemIDs.Count > 1)
        title = "<Multiple>";
      else if (dragAndDropObjects.Length == 1)
        title = ObjectNames.GetDragAndDropTitle(dragAndDropObjects[0]);
      else if (draggedScenes != null && draggedScenes.Count == 1)
      {
        title = draggedScenes[0].path;
      }
      else
      {
        title = "Unhandled dragged item";
        Debug.LogError((object) "Unhandled dragged item");
      }
      DragAndDrop.StartDrag(title);
      if (!(this.m_TreeView.data is GameObjectTreeViewDataSource))
        return;
      ((GameObjectTreeViewDataSource) this.m_TreeView.data).SetupChildParentReferencesIfNeeded();
    }

    public override DragAndDropVisualMode DoDrag(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
    {
      DragAndDropVisualMode andDropVisualMode = this.DoDragScenes(parentItem as GameObjectTreeViewItem, targetItem as GameObjectTreeViewItem, perform, dropPos);
      if (andDropVisualMode != DragAndDropVisualMode.None)
        return andDropVisualMode;
      InternalEditorUtility.HierarchyDropMode hierarchyDropMode = InternalEditorUtility.HierarchyDropMode.kHierarchyDragNormal;
      bool flag1 = !string.IsNullOrEmpty(((GameObjectTreeViewDataSource) this.m_TreeView.data).searchString);
      if (flag1)
        hierarchyDropMode |= InternalEditorUtility.HierarchyDropMode.kHierarchySearchActive;
      if (parentItem == null || targetItem == null)
      {
        InternalEditorUtility.HierarchyDropMode dropMode = hierarchyDropMode | InternalEditorUtility.HierarchyDropMode.kHierarchyDropUpon;
        return InternalEditorUtility.HierarchyWindowDrag((HierarchyProperty) null, perform, dropMode);
      }
      HierarchyProperty property = new HierarchyProperty(HierarchyType.GameObjects);
      if (!property.Find(targetItem.id, (int[]) null))
        property = (HierarchyProperty) null;
      bool flag2 = dropPos == TreeViewDragging.DropPosition.Upon;
      if (flag1 && !flag2)
        return DragAndDropVisualMode.None;
      InternalEditorUtility.HierarchyDropMode dropMode1 = hierarchyDropMode | (!flag2 ? InternalEditorUtility.HierarchyDropMode.kHierarchyDropBetween : InternalEditorUtility.HierarchyDropMode.kHierarchyDropUpon);
      if (parentItem != null && targetItem != parentItem && dropPos == TreeViewDragging.DropPosition.Above && parentItem.children[0] == targetItem)
        dropMode1 |= InternalEditorUtility.HierarchyDropMode.kHierarchyDropAfterParent;
      return InternalEditorUtility.HierarchyWindowDrag(property, perform, dropMode1);
    }

    public override void DragCleanup(bool revertExpanded)
    {
      DragAndDrop.SetGenericData("SceneHeaderList", (object) null);
      base.DragCleanup(revertExpanded);
    }

    private List<Scene> GetDraggedScenes(List<int> draggedItemIDs)
    {
      List<Scene> sceneList = new List<Scene>();
      foreach (int draggedItemId in draggedItemIDs)
      {
        Scene sceneByHandle = EditorSceneManager.GetSceneByHandle(draggedItemId);
        if (!SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(sceneByHandle))
          return (List<Scene>) null;
        sceneList.Add(sceneByHandle);
      }
      return sceneList;
    }

    private DragAndDropVisualMode DoDragScenes(GameObjectTreeViewItem parentItem, GameObjectTreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
    {
      List<Scene> genericData = DragAndDrop.GetGenericData("SceneHeaderList") as List<Scene>;
      bool flag1 = genericData != null;
      bool flag2 = false;
      if (!flag1 && DragAndDrop.objectReferences.Length > 0)
      {
        int num = 0;
        foreach (UnityEngine.Object objectReference in DragAndDrop.objectReferences)
        {
          if (objectReference is SceneAsset)
            ++num;
        }
        flag2 = num == DragAndDrop.objectReferences.Length;
      }
      if (!flag1 && !flag2)
        return DragAndDropVisualMode.None;
      if (perform)
      {
        List<Scene> sceneList = (List<Scene>) null;
        if (flag2)
        {
          List<Scene> source = new List<Scene>();
          foreach (UnityEngine.Object objectReference in DragAndDrop.objectReferences)
          {
            string assetPath = AssetDatabase.GetAssetPath(objectReference);
            Scene sceneByPath = SceneManager.GetSceneByPath(assetPath);
            if (SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(sceneByPath))
            {
              this.m_TreeView.Frame(sceneByPath.handle, true, true);
            }
            else
            {
              Scene scene = !Event.current.alt ? EditorSceneManager.OpenScene(assetPath, OpenSceneMode.Additive) : EditorSceneManager.OpenScene(assetPath, OpenSceneMode.AdditiveWithoutLoading);
              if (SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(scene))
                source.Add(scene);
            }
          }
          if (targetItem != null)
            sceneList = source;
          if (source.Count > 0)
          {
            Selection.instanceIDs = source.Select<Scene, int>((Func<Scene, int>) (x => x.handle)).ToArray<int>();
            this.m_TreeView.Frame(source.Last<Scene>().handle, true, false);
          }
        }
        else
          sceneList = genericData;
        if (sceneList != null)
        {
          if (targetItem != null)
          {
            Scene scene = targetItem.scene;
            if (SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(scene))
            {
              if (!targetItem.isSceneHeader || dropPos == TreeViewDragging.DropPosition.Upon)
                dropPos = TreeViewDragging.DropPosition.Below;
              if (dropPos == TreeViewDragging.DropPosition.Above)
              {
                for (int index = 0; index < sceneList.Count; ++index)
                  EditorSceneManager.MoveSceneBefore(sceneList[index], scene);
              }
              else if (dropPos == TreeViewDragging.DropPosition.Below)
              {
                for (int index = sceneList.Count - 1; index >= 0; --index)
                  EditorSceneManager.MoveSceneAfter(sceneList[index], scene);
              }
            }
          }
          else
          {
            Scene sceneAt = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            for (int index = sceneList.Count - 1; index >= 0; --index)
              EditorSceneManager.MoveSceneAfter(sceneList[index], sceneAt);
          }
        }
      }
      return DragAndDropVisualMode.Move;
    }
  }
}
