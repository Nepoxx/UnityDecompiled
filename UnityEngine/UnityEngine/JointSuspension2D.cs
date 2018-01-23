// Decompiled with JetBrains decompiler
// Type: UnityEngine.JointSuspension2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  public struct JointSuspension2D
  {
    private float m_DampingRatio;
    private float m_Frequency;
    private float m_Angle;

    public float dampingRatio
    {
      get
      {
        return this.m_DampingRatio;
      }
      set
      {
        this.m_DampingRatio = value;
      }
    }

    public float frequency
    {
      get
      {
        return this.m_Frequency;
      }
      set
      {
        this.m_Frequency = value;
      }
    }

    public float angle
    {
      get
      {
        return this.m_Angle;
      }
      set
      {
        this.m_Angle = value;
      }
    }
  }
}
