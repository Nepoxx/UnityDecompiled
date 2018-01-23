// Decompiled with JetBrains decompiler
// Type: UnityEngine.Animations.AnimationMotionXToDeltaPlayable
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Animations
{
  [RequiredByNativeCode]
  internal struct AnimationMotionXToDeltaPlayable : IPlayable, IEquatable<AnimationMotionXToDeltaPlayable>
  {
    private PlayableHandle m_Handle;

    private AnimationMotionXToDeltaPlayable(PlayableHandle handle)
    {
      if (handle.IsValid() && !handle.IsPlayableOfType<AnimationMotionXToDeltaPlayable>())
        throw new InvalidCastException("Can't set handle: the playable is not an AnimationMotionXToDeltaPlayable.");
      this.m_Handle = handle;
    }

    public static AnimationMotionXToDeltaPlayable Create(PlayableGraph graph)
    {
      return new AnimationMotionXToDeltaPlayable(AnimationMotionXToDeltaPlayable.CreateHandle(graph));
    }

    private static PlayableHandle CreateHandle(PlayableGraph graph)
    {
      PlayableHandle handle = PlayableHandle.Null;
      if (!AnimationMotionXToDeltaPlayable.CreateHandleInternal(graph, ref handle))
        return PlayableHandle.Null;
      handle.SetInputCount(1);
      return handle;
    }

    public PlayableHandle GetHandle()
    {
      return this.m_Handle;
    }

    public static implicit operator Playable(AnimationMotionXToDeltaPlayable playable)
    {
      return new Playable(playable.GetHandle());
    }

    public static explicit operator AnimationMotionXToDeltaPlayable(Playable playable)
    {
      return new AnimationMotionXToDeltaPlayable(playable.GetHandle());
    }

    public bool Equals(AnimationMotionXToDeltaPlayable other)
    {
      return this.GetHandle() == other.GetHandle();
    }

    private static bool CreateHandleInternal(PlayableGraph graph, ref PlayableHandle handle)
    {
      return AnimationMotionXToDeltaPlayable.CreateHandleInternal_Injected(ref graph, ref handle);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool CreateHandleInternal_Injected(ref PlayableGraph graph, ref PlayableHandle handle);
  }
}
