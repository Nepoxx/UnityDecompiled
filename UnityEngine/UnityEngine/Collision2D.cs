// Decompiled with JetBrains decompiler
// Type: UnityEngine.Collision2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public class Collision2D
  {
    internal int m_Collider;
    internal int m_OtherCollider;
    internal int m_Rigidbody;
    internal int m_OtherRigidbody;
    internal ContactPoint2D[] m_Contacts;
    internal Vector2 m_RelativeVelocity;
    internal int m_Enabled;

    public Collider2D collider
    {
      get
      {
        return Physics2D.GetColliderFromInstanceID(this.m_Collider);
      }
    }

    public Collider2D otherCollider
    {
      get
      {
        return Physics2D.GetColliderFromInstanceID(this.m_OtherCollider);
      }
    }

    public Rigidbody2D rigidbody
    {
      get
      {
        return Physics2D.GetRigidbodyFromInstanceID(this.m_Rigidbody);
      }
    }

    public Rigidbody2D otherRigidbody
    {
      get
      {
        return Physics2D.GetRigidbodyFromInstanceID(this.m_OtherRigidbody);
      }
    }

    public Transform transform
    {
      get
      {
        return !((Object) this.rigidbody != (Object) null) ? this.collider.transform : this.rigidbody.transform;
      }
    }

    public GameObject gameObject
    {
      get
      {
        return !((Object) this.rigidbody != (Object) null) ? this.collider.gameObject : this.rigidbody.gameObject;
      }
    }

    public ContactPoint2D[] contacts
    {
      get
      {
        if (this.m_Contacts == null)
          this.m_Contacts = Collision2D.CreateCollisionContacts(this.collider, this.otherCollider, this.rigidbody, this.otherRigidbody, this.enabled);
        return this.m_Contacts;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern ContactPoint2D[] CreateCollisionContacts(Collider2D collider, Collider2D otherCollider, Rigidbody2D rigidbody, Rigidbody2D otherRigidbody, bool enabled);

    public int GetContacts(ContactPoint2D[] contacts)
    {
      return Physics2D.GetContacts(this.collider, this.otherCollider, new ContactFilter2D().NoFilter(), contacts);
    }

    public Vector2 relativeVelocity
    {
      get
      {
        return this.m_RelativeVelocity;
      }
    }

    public bool enabled
    {
      get
      {
        return this.m_Enabled == 1;
      }
    }
  }
}
