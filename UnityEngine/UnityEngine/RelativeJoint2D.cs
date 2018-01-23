// Decompiled with JetBrains decompiler
// Type: UnityEngine.RelativeJoint2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public sealed class RelativeJoint2D : Joint2D
  {
    public extern float maxForce { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern float maxTorque { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern float correctionScale { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool autoConfigureOffset { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public Vector2 linearOffset
    {
      get
      {
        Vector2 ret;
        this.get_linearOffset_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_linearOffset_Injected(ref value);
      }
    }

    public extern float angularOffset { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public Vector2 target
    {
      get
      {
        Vector2 ret;
        this.get_target_Injected(out ret);
        return ret;
      }
    }

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_linearOffset_Injected(out Vector2 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_linearOffset_Injected(ref Vector2 value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_target_Injected(out Vector2 ret);
  }
}
