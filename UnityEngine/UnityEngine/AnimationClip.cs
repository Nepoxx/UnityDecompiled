// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimationClip
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Stores keyframe based animations.</para>
  /// </summary>
  [NativeType("Runtime/Animation/AnimationClip.h")]
  public sealed class AnimationClip : Motion
  {
    /// <summary>
    ///   <para>Creates a new animation clip.</para>
    /// </summary>
    public AnimationClip()
    {
      AnimationClip.Internal_CreateAnimationClip(this);
    }

    /// <summary>
    ///   <para>Samples an animation at a given time for any animated properties.</para>
    /// </summary>
    /// <param name="go">The animated game object.</param>
    /// <param name="time">The time to sample an animation.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SampleAnimation(GameObject go, float time);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateAnimationClip([Writable] AnimationClip self);

    /// <summary>
    ///   <para>Animation length in seconds. (Read Only)</para>
    /// </summary>
    public extern float length { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal extern float startTime { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal extern float stopTime { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Frame rate at which keyframes are sampled. (Read Only)</para>
    /// </summary>
    public extern float frameRate { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Assigns the curve to animate a specific property.</para>
    /// </summary>
    /// <param name="relativePath">Path to the game object this curve applies to. The relativePath
    /// is formatted similar to a pathname, e.g. "rootspineleftArm".  If relativePath
    /// is empty it refers to the game object the animation clip is attached to.</param>
    /// <param name="type">The class type of the component that is animated.</param>
    /// <param name="propertyName">The name or path to the property being animated.</param>
    /// <param name="curve">The animation curve.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetCurve(string relativePath, System.Type type, string propertyName, AnimationCurve curve);

    /// <summary>
    ///   <para>Realigns quaternion keys to ensure shortest interpolation paths.</para>
    /// </summary>
    public void EnsureQuaternionContinuity()
    {
      AnimationClip.INTERNAL_CALL_EnsureQuaternionContinuity(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_EnsureQuaternionContinuity(AnimationClip self);

    /// <summary>
    ///   <para>Clears all curves from the clip.</para>
    /// </summary>
    public void ClearCurves()
    {
      AnimationClip.INTERNAL_CALL_ClearCurves(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_ClearCurves(AnimationClip self);

    /// <summary>
    ///   <para>Sets the default wrap mode used in the animation state.</para>
    /// </summary>
    public extern WrapMode wrapMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>AABB of this Animation Clip in local space of Animation component that it is attached too.</para>
    /// </summary>
    public Bounds localBounds
    {
      get
      {
        Bounds bounds;
        this.INTERNAL_get_localBounds(out bounds);
        return bounds;
      }
      set
      {
        this.INTERNAL_set_localBounds(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_localBounds(out Bounds value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_localBounds(ref Bounds value);

    /// <summary>
    ///   <para>Set to true if the AnimationClip will be used with the Legacy Animation component ( instead of the Animator ).</para>
    /// </summary>
    public new extern bool legacy { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns true if the animation contains curve that drives a humanoid rig.</para>
    /// </summary>
    public extern bool humanMotion { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if the animation clip has no curves and no events.</para>
    /// </summary>
    public extern bool empty { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Adds an animation event to the clip.</para>
    /// </summary>
    /// <param name="evt">AnimationEvent to add.</param>
    public void AddEvent(AnimationEvent evt)
    {
      if (evt == null)
        throw new ArgumentNullException(nameof (evt));
      this.AddEventInternal((object) evt);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void AddEventInternal(object evt);

    /// <summary>
    ///   <para>Animation Events for this animation clip.</para>
    /// </summary>
    public AnimationEvent[] events
    {
      get
      {
        return (AnimationEvent[]) this.GetEventsInternal();
      }
      set
      {
        this.SetEventsInternal((Array) value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetEventsInternal(Array value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern Array GetEventsInternal();

    internal extern bool hasRootMotion { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
