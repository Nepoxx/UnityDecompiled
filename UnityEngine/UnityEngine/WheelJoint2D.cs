// Decompiled with JetBrains decompiler
// Type: UnityEngine.WheelJoint2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public sealed class WheelJoint2D : AnchoredJoint2D
  {
    public JointSuspension2D suspension
    {
      get
      {
        JointSuspension2D ret;
        this.get_suspension_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_suspension_Injected(ref value);
      }
    }

    public extern bool useMotor { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public JointMotor2D motor
    {
      get
      {
        JointMotor2D ret;
        this.get_motor_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_motor_Injected(ref value);
      }
    }

    public extern float jointTranslation { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern float jointLinearSpeed { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern float jointSpeed { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern float jointAngle { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float GetMotorTorque(float timeStep);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_suspension_Injected(out JointSuspension2D ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_suspension_Injected(ref JointSuspension2D value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_motor_Injected(out JointMotor2D ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_motor_Injected(ref JointMotor2D value);
  }
}
