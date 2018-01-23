// Decompiled with JetBrains decompiler
// Type: UnityEngine.Collider
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A base class of all colliders.</para>
  /// </summary>
  [RequiredByNativeCode]
  [RequireComponent(typeof (Transform))]
  public class Collider : Component
  {
    /// <summary>
    ///   <para>Enabled Colliders will collide with other Colliders, disabled Colliders won't.</para>
    /// </summary>
    public extern bool enabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The rigidbody the collider is attached to.</para>
    /// </summary>
    public extern Rigidbody attachedRigidbody { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is the collider a trigger?</para>
    /// </summary>
    public extern bool isTrigger { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Contact offset value of this collider.</para>
    /// </summary>
    public extern float contactOffset { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The material used by the collider.</para>
    /// </summary>
    public extern PhysicMaterial material { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The closest point to the bounding box of the attached collider.</para>
    /// </summary>
    /// <param name="position"></param>
    public Vector3 ClosestPointOnBounds(Vector3 position)
    {
      Vector3 vector3;
      Collider.INTERNAL_CALL_ClosestPointOnBounds(this, ref position, out vector3);
      return vector3;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_ClosestPointOnBounds(Collider self, ref Vector3 position, out Vector3 value);

    /// <summary>
    ///   <para>Returns a point on the collider that is closest to a given location.</para>
    /// </summary>
    /// <param name="position">Location you want to find the closest point to.</param>
    /// <returns>
    ///   <para>The point on the collider that is closest to the specified location.</para>
    /// </returns>
    public Vector3 ClosestPoint(Vector3 position)
    {
      Vector3 vector3;
      Collider.INTERNAL_CALL_ClosestPoint(this, ref position, out vector3);
      return vector3;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_ClosestPoint(Collider self, ref Vector3 position, out Vector3 value);

    /// <summary>
    ///   <para>The shared physic material of this collider.</para>
    /// </summary>
    public extern PhysicMaterial sharedMaterial { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The world space bounding volume of the collider.</para>
    /// </summary>
    public Bounds bounds
    {
      get
      {
        Bounds bounds;
        this.INTERNAL_get_bounds(out bounds);
        return bounds;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_bounds(out Bounds value);

    private static bool Internal_Raycast(Collider col, Ray ray, out RaycastHit hitInfo, float maxDistance)
    {
      return Collider.INTERNAL_CALL_Internal_Raycast(col, ref ray, out hitInfo, maxDistance);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_Internal_Raycast(Collider col, ref Ray ray, out RaycastHit hitInfo, float maxDistance);

    public bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
    {
      return Collider.Internal_Raycast(this, ray, out hitInfo, maxDistance);
    }
  }
}
