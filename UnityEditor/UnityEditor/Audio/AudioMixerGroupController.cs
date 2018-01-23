// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.AudioMixerGroupController
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Audio;
using UnityEngine.Scripting;

namespace UnityEditor.Audio
{
  internal sealed class AudioMixerGroupController : AudioMixerGroup
  {
    public AudioMixerGroupController(AudioMixer owner)
    {
      AudioMixerGroupController.Internal_CreateAudioMixerGroupController(this, owner);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateAudioMixerGroupController(AudioMixerGroupController mono, AudioMixer owner);

    public extern GUID groupID { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern int userColorIndex { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern AudioMixerController controller { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void PreallocateGUIDs();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern GUID GetGUIDForVolume();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float GetValueForVolume(AudioMixerController controller, AudioMixerSnapshotController snapshot);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetValueForVolume(AudioMixerController controller, AudioMixerSnapshotController snapshot, float value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern GUID GetGUIDForPitch();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float GetValueForPitch(AudioMixerController controller, AudioMixerSnapshotController snapshot);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetValueForPitch(AudioMixerController controller, AudioMixerSnapshotController snapshot, float value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool HasDependentMixers();

    public extern AudioMixerGroupController[] children { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern AudioMixerEffectController[] effects { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool mute { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool solo { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool bypassEffects { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public void InsertEffect(AudioMixerEffectController effect, int index)
    {
      List<AudioMixerEffectController> effectControllerList = new List<AudioMixerEffectController>((IEnumerable<AudioMixerEffectController>) this.effects);
      effectControllerList.Add((AudioMixerEffectController) null);
      for (int index1 = effectControllerList.Count - 1; index1 > index; --index1)
        effectControllerList[index1] = effectControllerList[index1 - 1];
      effectControllerList[index] = effect;
      this.effects = effectControllerList.ToArray();
    }

    public bool HasAttenuation()
    {
      foreach (AudioMixerEffectController effect in this.effects)
      {
        if (effect.IsAttenuation())
          return true;
      }
      return false;
    }

    public void DumpHierarchy(string title, int level)
    {
      if (title != "")
        Console.WriteLine(title);
      string str1 = "";
      int num = level;
      while (num-- > 0)
        str1 += "  ";
      Console.WriteLine(str1 + "name=" + this.name);
      string str2 = str1 + "  ";
      foreach (AudioMixerEffectController effect in this.effects)
        Console.WriteLine(str2 + "effect=" + effect.ToString());
      foreach (AudioMixerGroupController child in this.children)
        child.DumpHierarchy("", level + 1);
    }

    public string GetDisplayString()
    {
      return this.name;
    }

    public override string ToString()
    {
      return this.name;
    }
  }
}
