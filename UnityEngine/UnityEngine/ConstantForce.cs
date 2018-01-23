// Decompiled with JetBrains decompiler
// Type: UnityEngine.ConstantForce
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A force applied constantly.</para>
  /// </summary>
  [RequireComponent(typeof (Rigidbody))]
  public sealed class ConstantForce : Behaviour
  {
    /// <summary>
    ///   <para>The force applied to the rigidbody every frame.</para>
    /// </summary>
    public Vector3 force
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_force(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_force(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_force(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_force(ref Vector3 value);

    /// <summary>
    ///   <para>The force - relative to the rigid bodies coordinate system - applied every frame.</para>
    /// </summary>
    public Vector3 relativeForce
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_relativeForce(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_relativeForce(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_relativeForce(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_relativeForce(ref Vector3 value);

    /// <summary>
    ///   <para>The torque applied to the rigidbody every frame.</para>
    /// </summary>
    public Vector3 torque
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_torque(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_torque(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_torque(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_torque(ref Vector3 value);

    /// <summary>
    ///   <para>The torque - relative to the rigid bodies coordinate system - applied every frame.</para>
    /// </summary>
    public Vector3 relativeTorque
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_relativeTorque(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_relativeTorque(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_relativeTorque(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_relativeTorque(ref Vector3 value);
  }
}
