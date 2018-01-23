// Decompiled with JetBrains decompiler
// Type: UnityEngine.SliderJoint2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public sealed class SliderJoint2D : AnchoredJoint2D
  {
    public extern bool autoConfigureAngle { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern float angle { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

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

    public JointTranslationLimits2D limits
    {
      get
      {
        JointTranslationLimits2D ret;
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

    public extern float jointTranslation { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern float jointSpeed { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float GetMotorForce(float timeStep);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_motor_Injected(out JointMotor2D ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_motor_Injected(ref JointMotor2D value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_limits_Injected(out JointTranslationLimits2D ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_limits_Injected(ref JointTranslationLimits2D value);
  }
}
