// Decompiled with JetBrains decompiler
// Type: UnityEngine.HingeJoint
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The HingeJoint groups together 2 rigid bodies, constraining them to move like connected by a hinge.</para>
  /// </summary>
  public sealed class HingeJoint : Joint
  {
    /// <summary>
    ///   <para>The motor will apply a force up to a maximum force to achieve the target velocity in degrees per second.</para>
    /// </summary>
    public JointMotor motor
    {
      get
      {
        JointMotor jointMotor;
        this.INTERNAL_get_motor(out jointMotor);
        return jointMotor;
      }
      set
      {
        this.INTERNAL_set_motor(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_motor(out JointMotor value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_motor(ref JointMotor value);

    /// <summary>
    ///   <para>Limit of angular rotation (in degrees) on the hinge joint.</para>
    /// </summary>
    public JointLimits limits
    {
      get
      {
        JointLimits jointLimits;
        this.INTERNAL_get_limits(out jointLimits);
        return jointLimits;
      }
      set
      {
        this.INTERNAL_set_limits(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_limits(out JointLimits value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_limits(ref JointLimits value);

    /// <summary>
    ///   <para>The spring attempts to reach a target angle by adding spring and damping forces.</para>
    /// </summary>
    public JointSpring spring
    {
      get
      {
        JointSpring jointSpring;
        this.INTERNAL_get_spring(out jointSpring);
        return jointSpring;
      }
      set
      {
        this.INTERNAL_set_spring(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_spring(out JointSpring value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_spring(ref JointSpring value);

    /// <summary>
    ///   <para>Enables the joint's motor. Disabled by default.</para>
    /// </summary>
    public extern bool useMotor { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enables the joint's limits. Disabled by default.</para>
    /// </summary>
    public extern bool useLimits { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enables the joint's spring. Disabled by default.</para>
    /// </summary>
    public extern bool useSpring { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The angular velocity of the joint in degrees per second. (Read Only)</para>
    /// </summary>
    public extern float velocity { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The current angle in degrees of the joint relative to its rest position. (Read Only)</para>
    /// </summary>
    public extern float angle { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
