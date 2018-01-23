// Decompiled with JetBrains decompiler
// Type: UnityEngine.Audio.AudioPlayableOutput
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Audio
{
  /// <summary>
  ///   <para>A IPlayableOutput implementation that will be used to play audio.</para>
  /// </summary>
  [RequiredByNativeCode]
  public struct AudioPlayableOutput : IPlayableOutput
  {
    private PlayableOutputHandle m_Handle;

    internal AudioPlayableOutput(PlayableOutputHandle handle)
    {
      if (handle.IsValid() && !handle.IsPlayableOutputOfType<AudioPlayableOutput>())
        throw new InvalidCastException("Can't set handle: the playable is not an AudioPlayableOutput.");
      this.m_Handle = handle;
    }

    /// <summary>
    ///   <para>Creates an AudioPlayableOutput in the PlayableGraph.</para>
    /// </summary>
    /// <param name="graph">The PlayableGraph that will contain the AnimationPlayableOutput.</param>
    /// <param name="name">The name of the output.</param>
    /// <param name="target">The AudioSource that will play the AudioPlayableOutput source Playable.</param>
    /// <returns>
    ///   <para>A new AudioPlayableOutput attached to the PlayableGraph.</para>
    /// </returns>
    public static AudioPlayableOutput Create(PlayableGraph graph, string name, AudioSource target)
    {
      PlayableOutputHandle handle;
      if (!AudioPlayableGraphExtensions.InternalCreateAudioOutput(ref graph, name, out handle))
        return AudioPlayableOutput.Null;
      AudioPlayableOutput audioPlayableOutput = new AudioPlayableOutput(handle);
      audioPlayableOutput.SetTarget(target);
      return audioPlayableOutput;
    }

    /// <summary>
    ///   <para>Returns an invalid AudioPlayableOutput.</para>
    /// </summary>
    public static AudioPlayableOutput Null
    {
      get
      {
        return new AudioPlayableOutput(PlayableOutputHandle.Null);
      }
    }

    public PlayableOutputHandle GetHandle()
    {
      return this.m_Handle;
    }

    public static implicit operator PlayableOutput(AudioPlayableOutput output)
    {
      return new PlayableOutput(output.GetHandle());
    }

    public static explicit operator AudioPlayableOutput(PlayableOutput output)
    {
      return new AudioPlayableOutput(output.GetHandle());
    }

    public AudioSource GetTarget()
    {
      return AudioPlayableOutput.InternalGetTarget(ref this.m_Handle);
    }

    public void SetTarget(AudioSource value)
    {
      AudioPlayableOutput.InternalSetTarget(ref this.m_Handle, value);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AudioSource InternalGetTarget(ref PlayableOutputHandle output);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void InternalSetTarget(ref PlayableOutputHandle output, AudioSource target);
  }
}
