// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.AudioMixerEffectController
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Audio
{
  internal sealed class AudioMixerEffectController : Object
  {
    private int m_LastCachedGroupDisplayNameID;
    private string m_DisplayName;

    public AudioMixerEffectController(string name)
    {
      AudioMixerEffectController.Internal_CreateAudioMixerEffectController(this, name);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateAudioMixerEffectController(AudioMixerEffectController mono, string name);

    public extern GUID effectID { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern string effectName { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool IsSend()
    {
      return this.effectName == "Send";
    }

    public bool IsReceive()
    {
      return this.effectName == "Receive";
    }

    public bool IsDuckVolume()
    {
      return this.effectName == "Duck Volume";
    }

    public bool IsAttenuation()
    {
      return this.effectName == "Attenuation";
    }

    public bool DisallowsBypass()
    {
      return this.IsSend() || this.IsReceive() || this.IsDuckVolume() || this.IsAttenuation();
    }

    public void ClearCachedDisplayName()
    {
      this.m_DisplayName = (string) null;
    }

    public string GetDisplayString(Dictionary<AudioMixerEffectController, AudioMixerGroupController> effectMap)
    {
      AudioMixerGroupController effect = effectMap[this];
      if (effect.GetInstanceID() != this.m_LastCachedGroupDisplayNameID || this.m_DisplayName == null)
      {
        this.m_DisplayName = effect.GetDisplayString() + AudioMixerController.s_GroupEffectDisplaySeperator + AudioMixerController.FixNameForPopupMenu(this.effectName);
        this.m_LastCachedGroupDisplayNameID = effect.GetInstanceID();
      }
      return this.m_DisplayName;
    }

    public string GetSendTargetDisplayString(Dictionary<AudioMixerEffectController, AudioMixerGroupController> effectMap)
    {
      return !((Object) this.sendTarget != (Object) null) ? string.Empty : this.sendTarget.GetDisplayString(effectMap);
    }

    public extern AudioMixerEffectController sendTarget { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool enableWetMix { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool bypass { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void PreallocateGUIDs();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern GUID GetGUIDForMixLevel();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float GetValueForMixLevel(AudioMixerController controller, AudioMixerSnapshotController snapshot);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetValueForMixLevel(AudioMixerController controller, AudioMixerSnapshotController snapshot, float value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern GUID GetGUIDForParameter(string parameterName);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float GetValueForParameter(AudioMixerController controller, AudioMixerSnapshotController snapshot, string parameterName);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetValueForParameter(AudioMixerController controller, AudioMixerSnapshotController snapshot, string parameterName, float value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetFloatBuffer(AudioMixerController controller, string name, out float[] data, int numsamples);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float GetCPUUsage(AudioMixerController controller);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool ContainsParameterGUID(GUID guid);
  }
}
