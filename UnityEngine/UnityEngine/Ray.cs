// Decompiled with JetBrains decompiler
// Type: UnityEngine.Ray
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  public struct Ray
  {
    private Vector3 m_Origin;
    private Vector3 m_Direction;

    public Ray(Vector3 origin, Vector3 direction)
    {
      this.m_Origin = origin;
      this.m_Direction = direction.normalized;
    }

    public Vector3 origin
    {
      get
      {
        return this.m_Origin;
      }
      set
      {
        this.m_Origin = value;
      }
    }

    public Vector3 direction
    {
      get
      {
        return this.m_Direction;
      }
      set
      {
        this.m_Direction = value.normalized;
      }
    }

    public Vector3 GetPoint(float distance)
    {
      return this.m_Origin + this.m_Direction * distance;
    }

    public override string ToString()
    {
      return UnityString.Format("Origin: {0}, Dir: {1}", (object) this.m_Origin, (object) this.m_Direction);
    }

    public string ToString(string format)
    {
      return UnityString.Format("Origin: {0}, Dir: {1}", (object) this.m_Origin.ToString(format), (object) this.m_Direction.ToString(format));
    }
  }
}
