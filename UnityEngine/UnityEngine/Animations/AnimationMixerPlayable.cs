// Decompiled with JetBrains decompiler
// Type: UnityEngine.Animations.AnimationMixerPlayable
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Animations
{
  /// <summary>
  ///   <para>An implementation of IPlayable that controls an animation mixer.</para>
  /// </summary>
  [RequiredByNativeCode]
  public struct AnimationMixerPlayable : IPlayable, IEquatable<AnimationMixerPlayable>
  {
    private PlayableHandle m_Handle;

    internal AnimationMixerPlayable(PlayableHandle handle)
    {
      if (handle.IsValid() && !handle.IsPlayableOfType<AnimationMixerPlayable>())
        throw new InvalidCastException("Can't set handle: the playable is not an AnimationMixerPlayable.");
      this.m_Handle = handle;
    }

    /// <summary>
    ///   <para>Creates an AnimationMixerPlayable in the PlayableGraph.</para>
    /// </summary>
    /// <param name="graph">The PlayableGraph that will contain the new AnimationMixerPlayable.</param>
    /// <param name="inputCount">The number of inputs that the mixer will update.</param>
    /// <param name="normalizeWeights">True to force a weight normalization of the inputs.</param>
    /// <returns>
    ///   <para>A new AnimationMixerPlayable linked to the PlayableGraph.</para>
    /// </returns>
    public static AnimationMixerPlayable Create(PlayableGraph graph, int inputCount = 0, bool normalizeWeights = false)
    {
      return new AnimationMixerPlayable(AnimationMixerPlayable.CreateHandle(graph, inputCount, normalizeWeights));
    }

    private static PlayableHandle CreateHandle(PlayableGraph graph, int inputCount = 0, bool normalizeWeights = false)
    {
      PlayableHandle handle = PlayableHandle.Null;
      if (!AnimationMixerPlayable.CreateHandleInternal(graph, inputCount, normalizeWeights, ref handle))
        return PlayableHandle.Null;
      handle.SetInputCount(inputCount);
      return handle;
    }

    public PlayableHandle GetHandle()
    {
      return this.m_Handle;
    }

    public static implicit operator Playable(AnimationMixerPlayable playable)
    {
      return new Playable(playable.GetHandle());
    }

    public static explicit operator AnimationMixerPlayable(Playable playable)
    {
      return new AnimationMixerPlayable(playable.GetHandle());
    }

    public bool Equals(AnimationMixerPlayable other)
    {
      return this.GetHandle() == other.GetHandle();
    }

    private static bool CreateHandleInternal(PlayableGraph graph, int inputCount, bool normalizeWeights, ref PlayableHandle handle)
    {
      return AnimationMixerPlayable.CreateHandleInternal_Injected(ref graph, inputCount, normalizeWeights, ref handle);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool CreateHandleInternal_Injected(ref PlayableGraph graph, int inputCount, bool normalizeWeights, ref PlayableHandle handle);
  }
}
