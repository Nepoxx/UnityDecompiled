// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioGroupDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.Audio;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  internal class AudioGroupDataSource : TreeViewDataSource
  {
    public AudioMixerController m_Controller;

    public AudioGroupDataSource(TreeViewController treeView, AudioMixerController controller)
      : base(treeView)
    {
      this.m_Controller = controller;
    }

    private void AddNodesRecursively(AudioMixerGroupController group, TreeViewItem parent, int depth)
    {
      List<TreeViewItem> treeViewItemList = new List<TreeViewItem>();
      for (int index = 0; index < group.children.Length; ++index)
      {
        AudioMixerTreeViewNode mixerTreeViewNode = new AudioMixerTreeViewNode(AudioGroupDataSource.GetUniqueNodeID(group.children[index]), depth, parent, group.children[index].name, group.children[index]);
        mixerTreeViewNode.parent = parent;
        treeViewItemList.Add((TreeViewItem) mixerTreeViewNode);
        this.AddNodesRecursively(group.children[index], (TreeViewItem) mixerTreeViewNode, depth + 1);
      }
      parent.children = treeViewItemList;
    }

    public static int GetUniqueNodeID(AudioMixerGroupController group)
    {
      return group.GetInstanceID();
    }

    public override void FetchData()
    {
      if ((Object) this.m_Controller == (Object) null)
        this.m_RootItem = (TreeViewItem) null;
      else if ((Object) this.m_Controller.masterGroup == (Object) null)
      {
        Debug.LogError((object) "The Master group is missing !!!");
        this.m_RootItem = (TreeViewItem) null;
      }
      else
      {
        this.m_RootItem = (TreeViewItem) new AudioMixerTreeViewNode(AudioGroupDataSource.GetUniqueNodeID(this.m_Controller.masterGroup), 0, (TreeViewItem) null, this.m_Controller.masterGroup.name, this.m_Controller.masterGroup);
        this.AddNodesRecursively(this.m_Controller.masterGroup, this.m_RootItem, 1);
        this.m_NeedRefreshRows = true;
      }
    }

    public override bool IsRenamingItemAllowed(TreeViewItem node)
    {
      return !((Object) (node as AudioMixerTreeViewNode).group == (Object) this.m_Controller.masterGroup);
    }
  }
}
