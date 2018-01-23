// Decompiled with JetBrains decompiler
// Type: UnityEngine.ColliderDistance2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  public struct ColliderDistance2D
  {
    private Vector2 m_PointA;
    private Vector2 m_PointB;
    private Vector2 m_Normal;
    private float m_Distance;
    private int m_IsValid;

    public Vector2 pointA
    {
      get
      {
        return this.m_PointA;
      }
      set
      {
        this.m_PointA = value;
      }
    }

    public Vector2 pointB
    {
      get
      {
        return this.m_PointB;
      }
      set
      {
        this.m_PointB = value;
      }
    }

    public Vector2 normal
    {
      get
      {
        return this.m_Normal;
      }
    }

    public float distance
    {
      get
      {
        return this.m_Distance;
      }
      set
      {
        this.m_Distance = value;
      }
    }

    public bool isOverlapped
    {
      get
      {
        return (double) this.m_Distance < 0.0;
      }
    }

    public bool isValid
    {
      get
      {
        return this.m_IsValid != 0;
      }
      set
      {
        this.m_IsValid = !value ? 0 : 1;
      }
    }
  }
}
