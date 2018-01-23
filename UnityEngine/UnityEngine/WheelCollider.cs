// Decompiled with JetBrains decompiler
// Type: UnityEngine.WheelCollider
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A special collider for vehicle wheels.</para>
  /// </summary>
  public sealed class WheelCollider : Collider
  {
    /// <summary>
    ///   <para>The center of the wheel, measured in the object's local space.</para>
    /// </summary>
    public Vector3 center
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_center(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_center(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_center(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_center(ref Vector3 value);

    /// <summary>
    ///   <para>The radius of the wheel, measured in local space.</para>
    /// </summary>
    public extern float radius { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Maximum extension distance of wheel suspension, measured in local space.</para>
    /// </summary>
    public extern float suspensionDistance { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The parameters of wheel's suspension. The suspension attempts to reach a target position by applying a linear force and a damping force.</para>
    /// </summary>
    public JointSpring suspensionSpring
    {
      get
      {
        JointSpring jointSpring;
        this.INTERNAL_get_suspensionSpring(out jointSpring);
        return jointSpring;
      }
      set
      {
        this.INTERNAL_set_suspensionSpring(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_suspensionSpring(out JointSpring value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_suspensionSpring(ref JointSpring value);

    /// <summary>
    ///   <para>Application point of the suspension and tire forces measured from the base of the resting wheel.</para>
    /// </summary>
    public extern float forceAppPointDistance { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The mass of the wheel, expressed in kilograms. Must be larger than zero. Typical values would be in range (20,80).</para>
    /// </summary>
    public extern float mass { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The damping rate of the wheel. Must be larger than zero.</para>
    /// </summary>
    public extern float wheelDampingRate { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Properties of tire friction in the direction the wheel is pointing in.</para>
    /// </summary>
    public WheelFrictionCurve forwardFriction
    {
      get
      {
        WheelFrictionCurve wheelFrictionCurve;
        this.INTERNAL_get_forwardFriction(out wheelFrictionCurve);
        return wheelFrictionCurve;
      }
      set
      {
        this.INTERNAL_set_forwardFriction(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_forwardFriction(out WheelFrictionCurve value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_forwardFriction(ref WheelFrictionCurve value);

    /// <summary>
    ///   <para>Properties of tire friction in the sideways direction.</para>
    /// </summary>
    public WheelFrictionCurve sidewaysFriction
    {
      get
      {
        WheelFrictionCurve wheelFrictionCurve;
        this.INTERNAL_get_sidewaysFriction(out wheelFrictionCurve);
        return wheelFrictionCurve;
      }
      set
      {
        this.INTERNAL_set_sidewaysFriction(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_sidewaysFriction(out WheelFrictionCurve value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_sidewaysFriction(ref WheelFrictionCurve value);

    /// <summary>
    ///   <para>Motor torque on the wheel axle expressed in Newton metres. Positive or negative depending on direction.</para>
    /// </summary>
    public extern float motorTorque { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Brake torque expressed in Newton metres.</para>
    /// </summary>
    public extern float brakeTorque { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Steering angle in degrees, always around the local y-axis.</para>
    /// </summary>
    public extern float steerAngle { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Indicates whether the wheel currently collides with something (Read Only).</para>
    /// </summary>
    public extern bool isGrounded { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Configure vehicle sub-stepping parameters.</para>
    /// </summary>
    /// <param name="speedThreshold">The speed threshold of the sub-stepping algorithm.</param>
    /// <param name="stepsBelowThreshold">Amount of simulation sub-steps when vehicle's speed is below speedThreshold.</param>
    /// <param name="stepsAboveThreshold">Amount of simulation sub-steps when vehicle's speed is above speedThreshold.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ConfigureVehicleSubsteps(float speedThreshold, int stepsBelowThreshold, int stepsAboveThreshold);

    /// <summary>
    ///   <para>The mass supported by this WheelCollider.</para>
    /// </summary>
    public extern float sprungMass { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetGroundHit(out WheelHit hit);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void GetWorldPose(out Vector3 pos, out Quaternion quat);

    /// <summary>
    ///   <para>Current wheel axle rotation speed, in rotations per minute (Read Only).</para>
    /// </summary>
    public extern float rpm { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
