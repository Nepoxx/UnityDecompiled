// Decompiled with JetBrains decompiler
// Type: UnityEngine.SoftJointLimitSpring
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>The configuration of the spring attached to the joint's limits: linear and angular. Used by CharacterJoint and ConfigurableJoint.</para>
  /// </summary>
  public struct SoftJointLimitSpring
  {
    private float m_Spring;
    private float m_Damper;

    /// <summary>
    ///   <para>The stiffness of the spring limit. When stiffness is zero the limit is hard, otherwise soft.</para>
    /// </summary>
    public float spring
    {
      get
      {
        return this.m_Spring;
      }
      set
      {
        this.m_Spring = value;
      }
    }

    /// <summary>
    ///   <para>The damping of the spring limit. In effect when the stiffness of the sprint limit is not zero.</para>
    /// </summary>
    public float damper
    {
      get
      {
        return this.m_Damper;
      }
      set
      {
        this.m_Damper = value;
      }
    }
  }
}
