// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;
using UnityEditor.Audio;
using UnityEngine;

namespace UnityEditor
{
  internal class AudioMixerSelection
  {
    private AudioMixerController m_Controller;

    public AudioMixerSelection(AudioMixerController controller)
    {
      this.m_Controller = controller;
      this.ChannelStripSelection = new List<AudioMixerGroupController>();
      this.SyncToUnitySelection();
    }

    public List<AudioMixerGroupController> ChannelStripSelection { get; private set; }

    public void SyncToUnitySelection()
    {
      if (!((Object) this.m_Controller != (Object) null))
        return;
      this.RefreshCachedChannelStripSelection();
    }

    public void SetChannelStrips(List<AudioMixerGroupController> newSelection)
    {
      Selection.objects = (Object[]) newSelection.ToArray();
    }

    public void SetSingleChannelStrip(AudioMixerGroupController group)
    {
      Selection.objects = (Object[]) new AudioMixerGroupController[1]
      {
        group
      };
    }

    public void ToggleChannelStrip(AudioMixerGroupController group)
    {
      List<Object> objectList = new List<Object>((IEnumerable<Object>) Selection.objects);
      if (objectList.Contains((Object) group))
        objectList.Remove((Object) group);
      else
        objectList.Add((Object) group);
      Selection.objects = objectList.ToArray();
    }

    public void ClearChannelStrips()
    {
      Selection.objects = new Object[0];
    }

    public bool HasSingleChannelStripSelection()
    {
      return this.ChannelStripSelection.Count == 1;
    }

    private void RefreshCachedChannelStripSelection()
    {
      Object[] filtered = Selection.GetFiltered(typeof (AudioMixerGroupController), SelectionMode.Deep);
      this.ChannelStripSelection = new List<AudioMixerGroupController>();
      foreach (AudioMixerGroupController mixerGroupController in this.m_Controller.GetAllAudioGroupsSlow())
      {
        if (((IEnumerable<Object>) filtered).Contains<Object>((Object) mixerGroupController))
          this.ChannelStripSelection.Add(mixerGroupController);
      }
    }

    public void Sanitize()
    {
      this.RefreshCachedChannelStripSelection();
    }
  }
}
