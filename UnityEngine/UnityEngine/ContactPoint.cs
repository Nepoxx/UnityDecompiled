// Decompiled with JetBrains decompiler
// Type: UnityEngine.ContactPoint
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Describes a contact point where the collision occurs.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct ContactPoint
  {
    internal Vector3 m_Point;
    internal Vector3 m_Normal;
    internal int m_ThisColliderInstanceID;
    internal int m_OtherColliderInstanceID;
    internal float m_Separation;

    /// <summary>
    ///   <para>The point of contact.</para>
    /// </summary>
    public Vector3 point
    {
      get
      {
        return this.m_Point;
      }
    }

    /// <summary>
    ///   <para>Normal of the contact point.</para>
    /// </summary>
    public Vector3 normal
    {
      get
      {
        return this.m_Normal;
      }
    }

    /// <summary>
    ///   <para>The first collider in contact at the point.</para>
    /// </summary>
    public Collider thisCollider
    {
      get
      {
        return ContactPoint.ColliderFromInstanceId(this.m_ThisColliderInstanceID);
      }
    }

    /// <summary>
    ///   <para>The other collider in contact at the point.</para>
    /// </summary>
    public Collider otherCollider
    {
      get
      {
        return ContactPoint.ColliderFromInstanceId(this.m_OtherColliderInstanceID);
      }
    }

    /// <summary>
    ///   <para>The distance between the colliders at the contact point.</para>
    /// </summary>
    public float separation
    {
      get
      {
        return this.m_Separation;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Collider ColliderFromInstanceId(int instanceID);
  }
}
