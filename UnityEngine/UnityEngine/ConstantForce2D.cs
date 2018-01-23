// Decompiled with JetBrains decompiler
// Type: UnityEngine.ConstantForce2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  [RequireComponent(typeof (Rigidbody2D))]
  public sealed class ConstantForce2D : PhysicsUpdateBehaviour2D
  {
    public Vector2 force
    {
      get
      {
        Vector2 ret;
        this.get_force_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_force_Injected(ref value);
      }
    }

    public Vector2 relativeForce
    {
      get
      {
        Vector2 ret;
        this.get_relativeForce_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_relativeForce_Injected(ref value);
      }
    }

    public extern float torque { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_force_Injected(out Vector2 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_force_Injected(ref Vector2 value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_relativeForce_Injected(out Vector2 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_relativeForce_Injected(ref Vector2 value);
  }
}
