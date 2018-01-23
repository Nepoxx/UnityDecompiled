// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.Audio;

namespace UnityEditor
{
  internal class AudioMixerUtility
  {
    public static void RepaintAudioMixerAndInspectors()
    {
      InspectorWindow.RepaintAllInspectors();
      AudioMixerWindow.RepaintAudioMixerWindow();
    }

    public static void VisitGroupsRecursivly(AudioMixerGroupController group, Action<AudioMixerGroupController> visitorCallback)
    {
      foreach (AudioMixerGroupController child in group.children)
        AudioMixerUtility.VisitGroupsRecursivly(child, visitorCallback);
      if (visitorCallback == null)
        return;
      visitorCallback(group);
    }

    public class VisitorFetchInstanceIDs
    {
      public List<int> instanceIDs = new List<int>();

      public void Visitor(AudioMixerGroupController group)
      {
        this.instanceIDs.Add(group.GetInstanceID());
      }
    }
  }
}
