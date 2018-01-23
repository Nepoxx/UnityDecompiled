// Decompiled with JetBrains decompiler
// Type: UnityEngine.Collider2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [RequireComponent(typeof (Transform))]
  [RequireComponent(typeof (Transform))]
  public class Collider2D : Behaviour
  {
    public extern float density { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool isTrigger { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool usedByEffector { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool usedByComposite { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern CompositeCollider2D composite { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public Vector2 offset
    {
      get
      {
        Vector2 ret;
        this.get_offset_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_offset_Injected(ref value);
      }
    }

    public extern Rigidbody2D attachedRigidbody { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern int shapeCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public Bounds bounds
    {
      get
      {
        Bounds ret;
        this.get_bounds_Injected(out ret);
        return ret;
      }
    }

    internal extern ColliderErrorState2D errorState { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal extern bool compositeCapable { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern PhysicsMaterial2D sharedMaterial { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern float friction { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern float bounciness { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool IsTouching([NotNull, Writable] Collider2D collider);

    public bool IsTouching([Writable] Collider2D collider, ContactFilter2D contactFilter)
    {
      return this.IsTouching_OtherColliderWithFilter(collider, contactFilter);
    }

    private bool IsTouching_OtherColliderWithFilter([NotNull, Writable] Collider2D collider, ContactFilter2D contactFilter)
    {
      return this.IsTouching_OtherColliderWithFilter_Injected(collider, ref contactFilter);
    }

    public bool IsTouching(ContactFilter2D contactFilter)
    {
      return this.IsTouching_AnyColliderWithFilter(contactFilter);
    }

    private bool IsTouching_AnyColliderWithFilter(ContactFilter2D contactFilter)
    {
      return this.IsTouching_AnyColliderWithFilter_Injected(ref contactFilter);
    }

    public bool IsTouchingLayers()
    {
      return this.IsTouchingLayers(-1);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool IsTouchingLayers([DefaultValue("Physics2D.AllLayers")] int layerMask);

    public bool OverlapPoint(Vector2 point)
    {
      return this.OverlapPoint_Injected(ref point);
    }

    public ColliderDistance2D Distance([Writable] Collider2D collider)
    {
      return Physics2D.Distance(this, collider);
    }

    public int OverlapCollider(ContactFilter2D contactFilter, Collider2D[] results)
    {
      return Physics2D.OverlapCollider(this, contactFilter, results);
    }

    [ExcludeFromDocs]
    public int Raycast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
    {
      float distance = float.PositiveInfinity;
      return this.Raycast(direction, contactFilter, results, distance);
    }

    public int Raycast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
    {
      return this.Internal_Raycast(direction, distance, contactFilter, results);
    }

    [ExcludeFromDocs]
    public int Raycast(Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
    {
      float maxDepth = float.PositiveInfinity;
      return this.Raycast(direction, results, distance, layerMask, minDepth, maxDepth);
    }

    [ExcludeFromDocs]
    public int Raycast(Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
    {
      float maxDepth = float.PositiveInfinity;
      float minDepth = float.NegativeInfinity;
      return this.Raycast(direction, results, distance, layerMask, minDepth, maxDepth);
    }

    [ExcludeFromDocs]
    public int Raycast(Vector2 direction, RaycastHit2D[] results, float distance)
    {
      float maxDepth = float.PositiveInfinity;
      float minDepth = float.NegativeInfinity;
      int layerMask = -1;
      return this.Raycast(direction, results, distance, layerMask, minDepth, maxDepth);
    }

    [ExcludeFromDocs]
    public int Raycast(Vector2 direction, RaycastHit2D[] results)
    {
      float maxDepth = float.PositiveInfinity;
      float minDepth = float.NegativeInfinity;
      int layerMask = -1;
      float distance = float.PositiveInfinity;
      return this.Raycast(direction, results, distance, layerMask, minDepth, maxDepth);
    }

    public int Raycast(Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("Physics2D.AllLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
    {
      ContactFilter2D legacyFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
      return this.Internal_Raycast(direction, distance, legacyFilter, results);
    }

    private int Internal_Raycast(Vector2 direction, float distance, ContactFilter2D contactFilter, RaycastHit2D[] results)
    {
      return Collider2D.INTERNAL_CALL_Internal_Raycast(this, ref direction, distance, ref contactFilter, results);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int INTERNAL_CALL_Internal_Raycast(Collider2D self, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

    [ExcludeFromDocs]
    public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, float distance)
    {
      bool ignoreSiblingColliders = true;
      return this.Cast(direction, contactFilter, results, distance, ignoreSiblingColliders);
    }

    [ExcludeFromDocs]
    public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
    {
      bool ignoreSiblingColliders = true;
      float distance = float.PositiveInfinity;
      return this.Cast(direction, contactFilter, results, distance, ignoreSiblingColliders);
    }

    public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("true")] bool ignoreSiblingColliders)
    {
      return this.Internal_Cast(direction, contactFilter, distance, ignoreSiblingColliders, results);
    }

    [ExcludeFromDocs]
    public int Cast(Vector2 direction, RaycastHit2D[] results, float distance)
    {
      bool ignoreSiblingColliders = true;
      return this.Cast(direction, results, distance, ignoreSiblingColliders);
    }

    [ExcludeFromDocs]
    public int Cast(Vector2 direction, RaycastHit2D[] results)
    {
      bool ignoreSiblingColliders = true;
      float distance = float.PositiveInfinity;
      return this.Cast(direction, results, distance, ignoreSiblingColliders);
    }

    public int Cast(Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("true")] bool ignoreSiblingColliders)
    {
      ContactFilter2D contactFilter = new ContactFilter2D();
      contactFilter.useTriggers = Physics2D.queriesHitTriggers;
      contactFilter.SetLayerMask((LayerMask) Physics2D.GetLayerCollisionMask(this.gameObject.layer));
      return this.Internal_Cast(direction, contactFilter, distance, ignoreSiblingColliders, results);
    }

    private int Internal_Cast(Vector2 direction, ContactFilter2D contactFilter, float distance, bool ignoreSiblingColliders, RaycastHit2D[] results)
    {
      return Collider2D.INTERNAL_CALL_Internal_Cast(this, ref direction, ref contactFilter, distance, ignoreSiblingColliders, results);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int INTERNAL_CALL_Internal_Cast(Collider2D self, ref Vector2 direction, ref ContactFilter2D contactFilter, float distance, bool ignoreSiblingColliders, RaycastHit2D[] results);

    public int GetContacts(ContactPoint2D[] contacts)
    {
      return Physics2D.GetContacts(this, new ContactFilter2D().NoFilter(), contacts);
    }

    public int GetContacts(ContactFilter2D contactFilter, ContactPoint2D[] contacts)
    {
      return Physics2D.GetContacts(this, contactFilter, contacts);
    }

    public int GetContacts(Collider2D[] colliders)
    {
      return Physics2D.GetContacts(this, new ContactFilter2D().NoFilter(), colliders);
    }

    public int GetContacts(ContactFilter2D contactFilter, Collider2D[] colliders)
    {
      return Physics2D.GetContacts(this, contactFilter, colliders);
    }

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_offset_Injected(out Vector2 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_offset_Injected(ref Vector2 value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_bounds_Injected(out Bounds ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool IsTouching_OtherColliderWithFilter_Injected([Writable] Collider2D collider, ref ContactFilter2D contactFilter);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool IsTouching_AnyColliderWithFilter_Injected(ref ContactFilter2D contactFilter);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool OverlapPoint_Injected(ref Vector2 point);
  }
}
