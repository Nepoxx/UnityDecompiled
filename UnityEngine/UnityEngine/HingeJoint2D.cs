// Decompiled with JetBrains decompiler
// Type: UnityEngine.HingeJoint2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public sealed class HingeJoint2D : AnchoredJoint2D
  {
    public extern bool useMotor { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool useLimits { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

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

    public JointAngleLimits2D limits
    {
      get
      {
        JointAngleLimits2D ret;
        this.get_limits_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_limits_Injected(ref value);
      }
    }

    public extern JointLimitState2D limitState { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern float referenceAngle { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern float jointAngle { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern float jointSpeed { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float GetMotorTorque(float timeStep);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_motor_Injected(out JointMotor2D ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_motor_Injected(ref JointMotor2D value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_limits_Injected(out JointAngleLimits2D ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_limits_Injected(ref JointAngleLimits2D value);
  }
}
