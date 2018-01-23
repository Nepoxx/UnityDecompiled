// Decompiled with JetBrains decompiler
// Type: UnityEngine.Plane
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  public struct Plane
  {
    private Vector3 m_Normal;
    private float m_Distance;

    public Plane(Vector3 inNormal, Vector3 inPoint)
    {
      this.m_Normal = Vector3.Normalize(inNormal);
      this.m_Distance = -Vector3.Dot(this.m_Normal, inPoint);
    }

    public Plane(Vector3 inNormal, float d)
    {
      this.m_Normal = Vector3.Normalize(inNormal);
      this.m_Distance = d;
    }

    public Plane(Vector3 a, Vector3 b, Vector3 c)
    {
      this.m_Normal = Vector3.Normalize(Vector3.Cross(b - a, c - a));
      this.m_Distance = -Vector3.Dot(this.m_Normal, a);
    }

    public Vector3 normal
    {
      get
      {
        return this.m_Normal;
      }
      set
      {
        this.m_Normal = value;
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

    public void SetNormalAndPosition(Vector3 inNormal, Vector3 inPoint)
    {
      this.m_Normal = Vector3.Normalize(inNormal);
      this.m_Distance = -Vector3.Dot(inNormal, inPoint);
    }

    public void Set3Points(Vector3 a, Vector3 b, Vector3 c)
    {
      this.m_Normal = Vector3.Normalize(Vector3.Cross(b - a, c - a));
      this.m_Distance = -Vector3.Dot(this.m_Normal, a);
    }

    public void Flip()
    {
      this.m_Normal = -this.m_Normal;
      this.m_Distance = -this.m_Distance;
    }

    public Plane flipped
    {
      get
      {
        return new Plane(-this.m_Normal, -this.m_Distance);
      }
    }

    public void Translate(Vector3 translation)
    {
      this.m_Distance += Vector3.Dot(this.m_Normal, translation);
    }

    public static Plane Translate(Plane plane, Vector3 translation)
    {
      return new Plane(plane.m_Normal, plane.m_Distance += Vector3.Dot(plane.m_Normal, translation));
    }

    public Vector3 ClosestPointOnPlane(Vector3 point)
    {
      float num = Vector3.Dot(this.m_Normal, point) + this.m_Distance;
      return point - this.m_Normal * num;
    }

    public float GetDistanceToPoint(Vector3 point)
    {
      return Vector3.Dot(this.m_Normal, point) + this.m_Distance;
    }

    public bool GetSide(Vector3 point)
    {
      return (double) Vector3.Dot(this.m_Normal, point) + (double) this.m_Distance > 0.0;
    }

    public bool SameSide(Vector3 inPt0, Vector3 inPt1)
    {
      float distanceToPoint1 = this.GetDistanceToPoint(inPt0);
      float distanceToPoint2 = this.GetDistanceToPoint(inPt1);
      return (double) distanceToPoint1 > 0.0 && (double) distanceToPoint2 > 0.0 || (double) distanceToPoint1 <= 0.0 && (double) distanceToPoint2 <= 0.0;
    }

    public bool Raycast(Ray ray, out float enter)
    {
      float a = Vector3.Dot(ray.direction, this.m_Normal);
      float num = -Vector3.Dot(ray.origin, this.m_Normal) - this.m_Distance;
      if (Mathf.Approximately(a, 0.0f))
      {
        enter = 0.0f;
        return false;
      }
      enter = num / a;
      return (double) enter > 0.0;
    }

    public override string ToString()
    {
      return UnityString.Format("(normal:({0:F1}, {1:F1}, {2:F1}), distance:{3:F1})", (object) this.m_Normal.x, (object) this.m_Normal.y, (object) this.m_Normal.z, (object) this.m_Distance);
    }

    public string ToString(string format)
    {
      return UnityString.Format("(normal:({0}, {1}, {2}), distance:{3})", (object) this.m_Normal.x.ToString(format), (object) this.m_Normal.y.ToString(format), (object) this.m_Normal.z.ToString(format), (object) this.m_Distance.ToString(format));
    }
  }
}
