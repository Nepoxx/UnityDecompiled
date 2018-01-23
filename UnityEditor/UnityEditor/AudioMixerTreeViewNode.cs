// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerTreeViewNode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Audio;
using UnityEditor.IMGUI.Controls;

namespace UnityEditor
{
  internal class AudioMixerTreeViewNode : TreeViewItem
  {
    public AudioMixerTreeViewNode(int instanceID, int depth, TreeViewItem parent, string displayName, AudioMixerGroupController group)
      : base(instanceID, depth, parent, displayName)
    {
      this.group = group;
    }

    public AudioMixerGroupController group { get; set; }
  }
}
