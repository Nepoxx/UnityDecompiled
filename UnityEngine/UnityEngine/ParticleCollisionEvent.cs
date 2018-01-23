// Decompiled with JetBrains decompiler
// Type: UnityEngine.ParticleCollisionEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Information about a particle collision.</para>
  /// </summary>
  [RequiredByNativeCode(Optional = true)]
  public struct ParticleCollisionEvent
  {
    private Vector3 m_Intersection;
    private Vector3 m_Normal;
    private Vector3 m_Velocity;
    private int m_ColliderInstanceID;

    /// <summary>
    ///   <para>The Collider for the GameObject struck by the particles.</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("collider property is deprecated. Use colliderComponent instead, which supports Collider and Collider2D components (UnityUpgradable) -> colliderComponent", true)]
    public Component collider
    {
      get
      {
        throw new InvalidOperationException("collider property is deprecated. Use colliderComponent instead, which supports Collider and Collider2D components");
      }
    }

    /// <summary>
    ///   <para>Intersection point of the collision in world coordinates.</para>
    /// </summary>
    public Vector3 intersection
    {
      get
      {
        return this.m_Intersection;
      }
    }

    /// <summary>
    ///   <para>Geometry normal at the intersection point of the collision.</para>
    /// </summary>
    public Vector3 normal
    {
      get
      {
        return this.m_Normal;
      }
    }

    /// <summary>
    ///   <para>Incident velocity at the intersection point of the collision.</para>
    /// </summary>
    public Vector3 velocity
    {
      get
      {
        return this.m_Velocity;
      }
    }

    /// <summary>
    ///   <para>The Collider or Collider2D for the GameObject struck by the particles.</para>
    /// </summary>
    public Component colliderComponent
    {
      get
      {
        return ParticleCollisionEvent.InstanceIDToColliderComponent(this.m_ColliderInstanceID);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Component InstanceIDToColliderComponent(int instanceID);
  }
}
