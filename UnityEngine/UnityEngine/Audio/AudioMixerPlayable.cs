// Decompiled with JetBrains decompiler
// Type: UnityEngine.Audio.AudioMixerPlayable
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Audio
{
  [RequiredByNativeCode]
  public struct AudioMixerPlayable : IPlayable, IEquatable<AudioMixerPlayable>
  {
    private PlayableHandle m_Handle;

    internal AudioMixerPlayable(PlayableHandle handle)
    {
      if (handle.IsValid() && !handle.IsPlayableOfType<AudioMixerPlayable>())
        throw new InvalidCastException("Can't set handle: the playable is not an AudioMixerPlayable.");
      this.m_Handle = handle;
    }

    public static AudioMixerPlayable Create(PlayableGraph graph, int inputCount = 0, bool normalizeInputVolumes = false)
    {
      return new AudioMixerPlayable(AudioMixerPlayable.CreateHandle(graph, inputCount, normalizeInputVolumes));
    }

    private static PlayableHandle CreateHandle(PlayableGraph graph, int inputCount, bool normalizeInputVolumes)
    {
      PlayableHandle handle = PlayableHandle.Null;
      if (!AudioMixerPlayable.CreateAudioMixerPlayableInternal(ref graph, inputCount, normalizeInputVolumes, ref handle))
        return PlayableHandle.Null;
      return handle;
    }

    public PlayableHandle GetHandle()
    {
      return this.m_Handle;
    }

    public static implicit operator Playable(AudioMixerPlayable playable)
    {
      return new Playable(playable.GetHandle());
    }

    public static explicit operator AudioMixerPlayable(Playable playable)
    {
      return new AudioMixerPlayable(playable.GetHandle());
    }

    public bool Equals(AudioMixerPlayable other)
    {
      return this.GetHandle() == other.GetHandle();
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool CreateAudioMixerPlayableInternal(ref PlayableGraph graph, int inputCount, bool normalizeInputVolumes, ref PlayableHandle handle);
  }
}
