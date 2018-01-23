// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioGroupTreeViewDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Audio;
using UnityEditor.IMGUI.Controls;

namespace UnityEditor
{
  internal class AudioGroupTreeViewDragging : AssetsTreeViewDragging
  {
    private AudioMixerGroupTreeView m_owner;

    public AudioGroupTreeViewDragging(TreeViewController treeView, AudioMixerGroupTreeView owner)
      : base(treeView)
    {
      this.m_owner = owner;
    }

    public override void StartDrag(TreeViewItem draggedItem, List<int> draggedItemIDs)
    {
      if (EditorApplication.isPlaying)
        return;
      base.StartDrag(draggedItem, draggedItemIDs);
    }

    public override DragAndDropVisualMode DoDrag(TreeViewItem parentNode, TreeViewItem targetNode, bool perform, TreeViewDragging.DropPosition dragPos)
    {
      AudioMixerTreeViewNode mixerTreeViewNode = parentNode as AudioMixerTreeViewNode;
      List<AudioMixerGroupController> list1 = new List<UnityEngine.Object>((IEnumerable<UnityEngine.Object>) DragAndDrop.objectReferences).OfType<AudioMixerGroupController>().ToList<AudioMixerGroupController>();
      if (mixerTreeViewNode == null || list1.Count <= 0)
        return DragAndDropVisualMode.None;
      List<int> list2 = list1.Select<AudioMixerGroupController, int>((Func<AudioMixerGroupController, int>) (i => i.GetInstanceID())).ToList<int>();
      bool flag = this.ValidDrag(parentNode, list2) && !AudioMixerController.WillModificationOfTopologyCauseFeedback(this.m_owner.Controller.GetAllAudioGroupsSlow(), list1, mixerTreeViewNode.group, (List<AudioMixerController.ConnectionNode>) null);
      if (perform && flag)
      {
        this.m_owner.Controller.ReparentSelection(mixerTreeViewNode.group, TreeViewDragging.GetInsertionIndex(parentNode, targetNode, dragPos), list1);
        this.m_owner.ReloadTree();
        this.m_TreeView.SetSelection(list2.ToArray(), true);
      }
      return !flag ? DragAndDropVisualMode.Rejected : DragAndDropVisualMode.Move;
    }

    private bool ValidDrag(TreeViewItem parent, List<int> draggedInstanceIDs)
    {
      for (TreeViewItem treeViewItem = parent; treeViewItem != null; treeViewItem = treeViewItem.parent)
      {
        if (draggedInstanceIDs.Contains(treeViewItem.id))
          return false;
      }
      return true;
    }
  }
}
