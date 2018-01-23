// Decompiled with JetBrains decompiler
// Type: UnityEngine.JointDrive
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>How the joint's movement will behave along its local X axis.</para>
  /// </summary>
  public struct JointDrive
  {
    private float m_PositionSpring;
    private float m_PositionDamper;
    private float m_MaximumForce;

    /// <summary>
    ///   <para>Whether the drive should attempt to reach position, velocity, both or nothing.</para>
    /// </summary>
    [Obsolete("JointDriveMode is obsolete")]
    public JointDriveMode mode
    {
      get
      {
        return JointDriveMode.None;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Strength of a rubber-band pull toward the defined direction. Only used if mode includes Position.</para>
    /// </summary>
    public float positionSpring
    {
      get
      {
        return this.m_PositionSpring;
      }
      set
      {
        this.m_PositionSpring = value;
      }
    }

    /// <summary>
    ///   <para>Resistance strength against the Position Spring. Only used if mode includes Position.</para>
    /// </summary>
    public float positionDamper
    {
      get
      {
        return this.m_PositionDamper;
      }
      set
      {
        this.m_PositionDamper = value;
      }
    }

    /// <summary>
    ///   <para>Amount of force applied to push the object toward the defined direction.</para>
    /// </summary>
    public float maximumForce
    {
      get
      {
        return this.m_MaximumForce;
      }
      set
      {
        this.m_MaximumForce = value;
      }
    }
  }
}
