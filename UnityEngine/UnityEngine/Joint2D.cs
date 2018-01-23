// Decompiled with JetBrains decompiler
// Type: UnityEngine.Joint2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  [RequireComponent(typeof (Transform), typeof (Rigidbody2D))]
  public class Joint2D : Behaviour
  {
    public extern Rigidbody2D attachedRigidbody { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern Rigidbody2D connectedBody { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool enableCollision { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern float breakForce { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern float breakTorque { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public Vector2 reactionForce
    {
      get
      {
        Vector2 ret;
        this.get_reactionForce_Injected(out ret);
        return ret;
      }
    }

    public extern float reactionTorque { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public Vector2 GetReactionForce(float timeStep)
    {
      Vector2 ret;
      this.GetReactionForce_Injected(timeStep, out ret);
      return ret;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float GetReactionTorque(float timeStep);

    [Obsolete("Joint2D.collideConnected has been deprecated. Use Joint2D.enableCollision instead (UnityUpgradable) -> enableCollision", true)]
    public bool collideConnected
    {
      get
      {
        return this.enableCollision;
      }
      set
      {
        this.enableCollision = value;
      }
    }

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_reactionForce_Injected(out Vector2 ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetReactionForce_Injected(float timeStep, out Vector2 ret);
  }
}
