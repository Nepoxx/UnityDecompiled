// Decompiled with JetBrains decompiler
// Type: UnityEngine.RaycastHit2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  public struct RaycastHit2D
  {
    private Vector2 m_Centroid;
    private Vector2 m_Point;
    private Vector2 m_Normal;
    private float m_Distance;
    private float m_Fraction;
    private Collider2D m_Collider;

    public Vector2 centroid
    {
      get
      {
        return this.m_Centroid;
      }
      set
      {
        this.m_Centroid = value;
      }
    }

    public Vector2 point
    {
      get
      {
        return this.m_Point;
      }
      set
      {
        this.m_Point = value;
      }
    }

    public Vector2 normal
    {
      get
      {
        return this.m_Normal;
      }
      set
      {
        this.m_Normal = value;
      }
    }

    public float distance
    {
      get
      {
        return this.m_Distance;
      }
      set
      {
        this.m_Distance = value;
      }
    }

    public float fraction
    {
      get
      {
        return this.m_Fraction;
      }
      set
      {
        this.m_Fraction = value;
      }
    }

    public Collider2D collider
    {
      get
      {
        return this.m_Collider;
      }
    }

    public Rigidbody2D rigidbody
    {
      get
      {
        return !((Object) this.collider != (Object) null) ? (Rigidbody2D) null : this.collider.attachedRigidbody;
      }
    }

    public Transform transform
    {
      get
      {
        Rigidbody2D rigidbody = this.rigidbody;
        if ((Object) rigidbody != (Object) null)
          return rigidbody.transform;
        if ((Object) this.collider != (Object) null)
          return this.collider.transform;
        return (Transform) null;
      }
    }

    public static implicit operator bool(RaycastHit2D hit)
    {
      return (Object) hit.collider != (Object) null;
    }

    public int CompareTo(RaycastHit2D other)
    {
      if ((Object) this.collider == (Object) null)
        return 1;
      if ((Object) other.collider == (Object) null)
        return -1;
      return this.fraction.CompareTo(other.fraction);
    }
  }
}
