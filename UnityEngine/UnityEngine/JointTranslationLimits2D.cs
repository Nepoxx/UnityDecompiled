// Decompiled with JetBrains decompiler
// Type: UnityEngine.JointTranslationLimits2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  public struct JointTranslationLimits2D
  {
    private float m_LowerTranslation;
    private float m_UpperTranslation;

    public float min
    {
      get
      {
        return this.m_LowerTranslation;
      }
      set
      {
        this.m_LowerTranslation = value;
      }
    }

    public float max
    {
      get
      {
        return this.m_UpperTranslation;
      }
      set
      {
        this.m_UpperTranslation = value;
      }
    }
  }
}
