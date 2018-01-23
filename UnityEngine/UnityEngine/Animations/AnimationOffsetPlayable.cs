// Decompiled with JetBrains decompiler
// Type: UnityEngine.Animations.AnimationOffsetPlayable
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
  internal struct AnimationOffsetPlayable : IPlayable, IEquatable<AnimationOffsetPlayable>
  {
    private static readonly AnimationOffsetPlayable m_NullPlayable = new AnimationOffsetPlayable(PlayableHandle.Null);
    private PlayableHandle m_Handle;

    internal AnimationOffsetPlayable(PlayableHandle handle)
    {
      if (handle.IsValid() && !handle.IsPlayableOfType<AnimationOffsetPlayable>())
        throw new InvalidCastException("Can't set handle: the playable is not an AnimationOffsetPlayable.");
      this.m_Handle = handle;
    }

    public static AnimationOffsetPlayable Null
    {
      get
      {
        return AnimationOffsetPlayable.m_NullPlayable;
      }
    }

    public static AnimationOffsetPlayable Create(PlayableGraph graph, Vector3 position, Quaternion rotation, int inputCount)
    {
      return new AnimationOffsetPlayable(AnimationOffsetPlayable.CreateHandle(graph, position, rotation, inputCount));
    }

    private static PlayableHandle CreateHandle(PlayableGraph graph, Vector3 position, Quaternion rotation, int inputCount)
    {
      PlayableHandle handle = PlayableHandle.Null;
      if (!AnimationOffsetPlayable.CreateHandleInternal(graph, position, rotation, ref handle))
        return PlayableHandle.Null;
      handle.SetInputCount(inputCount);
      return handle;
    }

    public PlayableHandle GetHandle()
    {
      return this.m_Handle;
    }

    public static implicit operator Playable(AnimationOffsetPlayable playable)
    {
      return new Playable(playable.GetHandle());
    }

    public static explicit operator AnimationOffsetPlayable(Playable playable)
    {
      return new AnimationOffsetPlayable(playable.GetHandle());
    }

    public bool Equals(AnimationOffsetPlayable other)
    {
      return this.Equals((object) other.GetHandle());
    }

    public Vector3 GetPosition()
    {
      return AnimationOffsetPlayable.GetPositionInternal(ref this.m_Handle);
    }

    public void SetPosition(Vector3 value)
    {
      AnimationOffsetPlayable.SetPositionInternal(ref this.m_Handle, value);
    }

    public Quaternion GetRotation()
    {
      return AnimationOffsetPlayable.GetRotationInternal(ref this.m_Handle);
    }

    public void SetRotation(Quaternion value)
    {
      AnimationOffsetPlayable.SetRotationInternal(ref this.m_Handle, value);
    }

    private static bool CreateHandleInternal(PlayableGraph graph, Vector3 position, Quaternion rotation, ref PlayableHandle handle)
    {
      return AnimationOffsetPlayable.CreateHandleInternal_Injected(ref graph, ref position, ref rotation, ref handle);
    }

    private static Vector3 GetPositionInternal(ref PlayableHandle handle)
    {
      Vector3 ret;
      AnimationOffsetPlayable.GetPositionInternal_Injected(ref handle, out ret);
      return ret;
    }

    private static void SetPositionInternal(ref PlayableHandle handle, Vector3 value)
    {
      AnimationOffsetPlayable.SetPositionInternal_Injected(ref handle, ref value);
    }

    private static Quaternion GetRotationInternal(ref PlayableHandle handle)
    {
      Quaternion ret;
      AnimationOffsetPlayable.GetRotationInternal_Injected(ref handle, out ret);
      return ret;
    }

    private static void SetRotationInternal(ref PlayableHandle handle, Quaternion value)
    {
      AnimationOffsetPlayable.SetRotationInternal_Injected(ref handle, ref value);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool CreateHandleInternal_Injected(ref PlayableGraph graph, ref Vector3 position, ref Quaternion rotation, ref PlayableHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetPositionInternal_Injected(ref PlayableHandle handle, out Vector3 ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetPositionInternal_Injected(ref PlayableHandle handle, ref Vector3 value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetRotationInternal_Injected(ref PlayableHandle handle, out Quaternion ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetRotationInternal_Injected(ref PlayableHandle handle, ref Quaternion value);
  }
}
