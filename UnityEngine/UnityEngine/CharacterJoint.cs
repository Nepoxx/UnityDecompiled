// Decompiled with JetBrains decompiler
// Type: UnityEngine.CharacterJoint
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Character Joints are mainly used for Ragdoll effects.</para>
  /// </summary>
  public sealed class CharacterJoint : Joint
  {
    [Obsolete("TargetRotation not in use for Unity 5 and assumed disabled.", true)]
    public Quaternion targetRotation;
    [Obsolete("TargetAngularVelocity not in use for Unity 5 and assumed disabled.", true)]
    public Vector3 targetAngularVelocity;
    [Obsolete("RotationDrive not in use for Unity 5 and assumed disabled.", true)]
    public JointDrive rotationDrive;

    /// <summary>
    ///   <para>The secondary axis around which the joint can rotate.</para>
    /// </summary>
    public Vector3 swingAxis
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_swingAxis(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_swingAxis(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_swingAxis(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_swingAxis(ref Vector3 value);

    /// <summary>
    ///   <para>The configuration of the spring attached to the twist limits of the joint.</para>
    /// </summary>
    public SoftJointLimitSpring twistLimitSpring
    {
      get
      {
        SoftJointLimitSpring jointLimitSpring;
        this.INTERNAL_get_twistLimitSpring(out jointLimitSpring);
        return jointLimitSpring;
      }
      set
      {
        this.INTERNAL_set_twistLimitSpring(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_twistLimitSpring(out SoftJointLimitSpring value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_twistLimitSpring(ref SoftJointLimitSpring value);

    /// <summary>
    ///   <para>The configuration of the spring attached to the swing limits of the joint.</para>
    /// </summary>
    public SoftJointLimitSpring swingLimitSpring
    {
      get
      {
        SoftJointLimitSpring jointLimitSpring;
        this.INTERNAL_get_swingLimitSpring(out jointLimitSpring);
        return jointLimitSpring;
      }
      set
      {
        this.INTERNAL_set_swingLimitSpring(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_swingLimitSpring(out SoftJointLimitSpring value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_swingLimitSpring(ref SoftJointLimitSpring value);

    /// <summary>
    ///   <para>The lower limit around the primary axis of the character joint.</para>
    /// </summary>
    public SoftJointLimit lowTwistLimit
    {
      get
      {
        SoftJointLimit softJointLimit;
        this.INTERNAL_get_lowTwistLimit(out softJointLimit);
        return softJointLimit;
      }
      set
      {
        this.INTERNAL_set_lowTwistLimit(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_lowTwistLimit(out SoftJointLimit value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_lowTwistLimit(ref SoftJointLimit value);

    /// <summary>
    ///   <para>The upper limit around the primary axis of the character joint.</para>
    /// </summary>
    public SoftJointLimit highTwistLimit
    {
      get
      {
        SoftJointLimit softJointLimit;
        this.INTERNAL_get_highTwistLimit(out softJointLimit);
        return softJointLimit;
      }
      set
      {
        this.INTERNAL_set_highTwistLimit(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_highTwistLimit(out SoftJointLimit value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_highTwistLimit(ref SoftJointLimit value);

    /// <summary>
    ///   <para>The angular limit of rotation (in degrees) around the primary axis of the character joint.</para>
    /// </summary>
    public SoftJointLimit swing1Limit
    {
      get
      {
        SoftJointLimit softJointLimit;
        this.INTERNAL_get_swing1Limit(out softJointLimit);
        return softJointLimit;
      }
      set
      {
        this.INTERNAL_set_swing1Limit(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_swing1Limit(out SoftJointLimit value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_swing1Limit(ref SoftJointLimit value);

    /// <summary>
    ///   <para>The angular limit of rotation (in degrees) around the primary axis of the character joint.</para>
    /// </summary>
    public SoftJointLimit swing2Limit
    {
      get
      {
        SoftJointLimit softJointLimit;
        this.INTERNAL_get_swing2Limit(out softJointLimit);
        return softJointLimit;
      }
      set
      {
        this.INTERNAL_set_swing2Limit(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_swing2Limit(out SoftJointLimit value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_swing2Limit(ref SoftJointLimit value);

    /// <summary>
    ///   <para>Brings violated constraints back into alignment even when the solver fails.</para>
    /// </summary>
    public extern bool enableProjection { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set the linear tolerance threshold for projection.</para>
    /// </summary>
    public extern float projectionDistance { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set the angular tolerance threshold (in degrees) for projection.</para>
    /// </summary>
    public extern float projectionAngle { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
