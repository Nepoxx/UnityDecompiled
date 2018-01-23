// Decompiled with JetBrains decompiler
// Type: UnityEngine.Joint
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Joint is the base class for all joints.</para>
  /// </summary>
  [RequireComponent(typeof (Rigidbody))]
  [NativeClass("Unity::Joint")]
  public class Joint : Component
  {
    /// <summary>
    ///   <para>A reference to another rigidbody this joint connects to.</para>
    /// </summary>
    public extern Rigidbody connectedBody { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The Direction of the axis around which the body is constrained.</para>
    /// </summary>
    public Vector3 axis
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_axis(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_axis(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_axis(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_axis(ref Vector3 value);

    /// <summary>
    ///   <para>The Position of the anchor around which the joints motion is constrained.</para>
    /// </summary>
    public Vector3 anchor
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_anchor(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_anchor(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_anchor(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_anchor(ref Vector3 value);

    /// <summary>
    ///   <para>Position of the anchor relative to the connected Rigidbody.</para>
    /// </summary>
    public Vector3 connectedAnchor
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_connectedAnchor(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_connectedAnchor(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_connectedAnchor(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_connectedAnchor(ref Vector3 value);

    /// <summary>
    ///   <para>Should the connectedAnchor be calculated automatically?</para>
    /// </summary>
    public extern bool autoConfigureConnectedAnchor { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The force that needs to be applied for this joint to break.</para>
    /// </summary>
    public extern float breakForce { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The torque that needs to be applied for this joint to break.</para>
    /// </summary>
    public extern float breakTorque { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable collision between bodies connected with the joint.</para>
    /// </summary>
    public extern bool enableCollision { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Toggle preprocessing for this joint.</para>
    /// </summary>
    public extern bool enablePreprocessing { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The force applied by the solver to satisfy all constraints.</para>
    /// </summary>
    public Vector3 currentForce
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_currentForce(out vector3);
        return vector3;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_currentForce(out Vector3 value);

    /// <summary>
    ///   <para>The torque applied by the solver to satisfy all constraints.</para>
    /// </summary>
    public Vector3 currentTorque
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_currentTorque(out vector3);
        return vector3;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_currentTorque(out Vector3 value);

    /// <summary>
    ///   <para>The scale to apply to the inverse mass and inertia tensor of the body prior to solving the constraints.</para>
    /// </summary>
    public extern float massScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The scale to apply to the inverse mass and inertia tensor of the connected body prior to solving the constraints.</para>
    /// </summary>
    public extern float connectedMassScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal Matrix4x4 GetActorLocalPose(int actorIndex)
    {
      Matrix4x4 matrix4x4;
      Joint.INTERNAL_CALL_GetActorLocalPose(this, actorIndex, out matrix4x4);
      return matrix4x4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetActorLocalPose(Joint self, int actorIndex, out Matrix4x4 value);
  }
}
