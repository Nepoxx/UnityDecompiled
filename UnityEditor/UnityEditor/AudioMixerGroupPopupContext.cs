// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerGroupPopupContext
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Audio;

namespace UnityEditor
{
  internal class AudioMixerGroupPopupContext
  {
    public AudioMixerController controller;
    public AudioMixerGroupController[] groups;

    public AudioMixerGroupPopupContext(AudioMixerController controller, AudioMixerGroupController group)
    {
      this.controller = controller;
      this.groups = new AudioMixerGroupController[1]
      {
        group
      };
    }

    public AudioMixerGroupPopupContext(AudioMixerController controller, AudioMixerGroupController[] groups)
    {
      this.controller = controller;
      this.groups = groups;
    }
  }
}
