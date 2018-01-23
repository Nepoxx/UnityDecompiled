// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetsTreeViewDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;

namespace UnityEditor
{
  internal class AssetsTreeViewDragging : TreeViewDragging
  {
    public AssetsTreeViewDragging(TreeViewController treeView)
      : base(treeView)
    {
    }

    public override void StartDrag(TreeViewItem draggedItem, List<int> draggedItemIDs)
    {
      DragAndDrop.PrepareStartDrag();
      DragAndDrop.objectReferences = ProjectWindowUtil.GetDragAndDropObjects(draggedItem.id, draggedItemIDs);
      DragAndDrop.paths = ProjectWindowUtil.GetDragAndDropPaths(draggedItem.id, draggedItemIDs);
      if (DragAndDrop.objectReferences.Length > 1)
        DragAndDrop.StartDrag("<Multiple>");
      else
        DragAndDrop.StartDrag(ObjectNames.GetDragAndDropTitle(InternalEditorUtility.GetObjectFromInstanceID(draggedItem.id)));
    }

    public override DragAndDropVisualMode DoDrag(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
    {
      HierarchyProperty property = new HierarchyProperty(HierarchyType.Assets);
      if (parentItem == null || !property.Find(parentItem.id, (int[]) null))
        property = (HierarchyProperty) null;
      return InternalEditorUtility.ProjectWindowDrag(property, perform);
    }
  }
}
