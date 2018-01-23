// Decompiled with JetBrains decompiler
// Type: UnityEngine.Animations.AnimationClipPlayable
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
  ///   <para>A Playable that controls an AnimationClip.</para>
  /// </summary>
  [RequiredByNativeCode]
  public struct AnimationClipPlayable : IPlayable, IEquatable<AnimationClipPlayable>
  {
    private PlayableHandle m_Handle;

    internal AnimationClipPlayable(PlayableHandle handle)
    {
      if (handle.IsValid() && !handle.IsPlayableOfType<AnimationClipPlayable>())
        throw new InvalidCastException("Can't set handle: the playable is not an AnimationClipPlayable.");
      this.m_Handle = handle;
    }

    /// <summary>
    ///   <para>Creates an AnimationClipPlayable in the PlayableGraph.</para>
    /// </summary>
    /// <param name="graph">The PlayableGraph object that will own the AnimationClipPlayable.</param>
    /// <param name="clip">The AnimationClip that will be added in the PlayableGraph.</param>
    /// <returns>
    ///   <para>A AnimationClipPlayable linked to the PlayableGraph.</para>
    /// </returns>
    public static AnimationClipPlayable Create(PlayableGraph graph, AnimationClip clip)
    {
      return new AnimationClipPlayable(AnimationClipPlayable.CreateHandle(graph, clip));
    }

    private static PlayableHandle CreateHandle(PlayableGraph graph, AnimationClip clip)
    {
      PlayableHandle handle = PlayableHandle.Null;
      if (!AnimationClipPlayable.CreateHandleInternal(graph, clip, ref handle))
        return PlayableHandle.Null;
      return handle;
    }

    public PlayableHandle GetHandle()
    {
      return this.m_Handle;
    }

    public static implicit operator Playable(AnimationClipPlayable playable)
    {
      return new Playable(playable.GetHandle());
    }

    public static explicit operator AnimationClipPlayable(Playable playable)
    {
      return new AnimationClipPlayable(playable.GetHandle());
    }

    public bool Equals(AnimationClipPlayable other)
    {
      return this.GetHandle() == other.GetHandle();
    }

    /// <summary>
    ///   <para>Returns the AnimationClip stored in the AnimationClipPlayable.</para>
    /// </summary>
    public AnimationClip GetAnimationClip()
    {
      return AnimationClipPlayable.GetAnimationClipInternal(ref this.m_Handle);
    }

    /// <summary>
    ///   <para>Returns the state of the ApplyFootIK flag.</para>
    /// </summary>
    public bool GetApplyFootIK()
    {
      return AnimationClipPlayable.GetApplyFootIKInternal(ref this.m_Handle);
    }

    /// <summary>
    ///   <para>Sets the value of the ApplyFootIK flag.</para>
    /// </summary>
    /// <param name="value">The new value of the ApplyFootIK flag.</param>
    public void SetApplyFootIK(bool value)
    {
      AnimationClipPlayable.SetApplyFootIKInternal(ref this.m_Handle, value);
    }

    internal bool GetRemoveStartOffset()
    {
      return AnimationClipPlayable.GetRemoveStartOffsetInternal(ref this.m_Handle);
    }

    internal void SetRemoveStartOffset(bool value)
    {
      AnimationClipPlayable.SetRemoveStartOffsetInternal(ref this.m_Handle, value);
    }

    private static bool CreateHandleInternal(PlayableGraph graph, AnimationClip clip, ref PlayableHandle handle)
    {
      return AnimationClipPlayable.CreateHandleInternal_Injected(ref graph, clip, ref handle);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AnimationClip GetAnimationClipInternal(ref PlayableHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool GetApplyFootIKInternal(ref PlayableHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetApplyFootIKInternal(ref PlayableHandle handle, bool value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool GetRemoveStartOffsetInternal(ref PlayableHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetRemoveStartOffsetInternal(ref PlayableHandle handle, bool value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool CreateHandleInternal_Injected(ref PlayableGraph graph, AnimationClip clip, ref PlayableHandle handle);
  }
}
