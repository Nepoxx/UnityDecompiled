// Decompiled with JetBrains decompiler
// Type: UnityEngine.JointAngleLimits2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  public struct JointAngleLimits2D
  {
    private float m_LowerAngle;
    private float m_UpperAngle;

    public float min
    {
      get
      {
        return this.m_LowerAngle;
      }
      set
      {
        this.m_LowerAngle = value;
      }
    }

    public float max
    {
      get
      {
        return this.m_UpperAngle;
      }
      set
      {
        this.m_UpperAngle = value;
      }
    }
  }
}
