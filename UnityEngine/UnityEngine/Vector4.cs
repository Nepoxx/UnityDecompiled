// Decompiled with JetBrains decompiler
// Type: UnityEngine.Vector4
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  public struct Vector4
  {
    private static readonly Vector4 zeroVector = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
    private static readonly Vector4 oneVector = new Vector4(1f, 1f, 1f, 1f);
    private static readonly Vector4 positiveInfinityVector = new Vector4(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    private static readonly Vector4 negativeInfinityVector = new Vector4(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
    public const float kEpsilon = 1E-05f;
    public float x;
    public float y;
    public float z;
    public float w;

    public Vector4(float x, float y, float z, float w)
    {
      this.x = x;
      this.y = y;
      this.z = z;
      this.w = w;
    }

    public Vector4(float x, float y, float z)
    {
      this.x = x;
      this.y = y;
      this.z = z;
      this.w = 0.0f;
    }

    public Vector4(float x, float y)
    {
      this.x = x;
      this.y = y;
      this.z = 0.0f;
      this.w = 0.0f;
    }

    public float this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.x;
          case 1:
            return this.y;
          case 2:
            return this.z;
          case 3:
            return this.w;
          default:
            throw new IndexOutOfRangeException("Invalid Vector4 index!");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.x = value;
            break;
          case 1:
            this.y = value;
            break;
          case 2:
            this.z = value;
            break;
          case 3:
            this.w = value;
            break;
          default:
            throw new IndexOutOfRangeException("Invalid Vector4 index!");
        }
      }
    }

    public void Set(float newX, float newY, float newZ, float newW)
    {
      this.x = newX;
      this.y = newY;
      this.z = newZ;
      this.w = newW;
    }

    public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
    {
      t = Mathf.Clamp01(t);
      return new Vector4(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.w + (b.w - a.w) * t);
    }

    public static Vector4 LerpUnclamped(Vector4 a, Vector4 b, float t)
    {
      return new Vector4(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.w + (b.w - a.w) * t);
    }

    public static Vector4 MoveTowards(Vector4 current, Vector4 target, float maxDistanceDelta)
    {
      Vector4 vector4 = target - current;
      float magnitude = vector4.magnitude;
      if ((double) magnitude <= (double) maxDistanceDelta || (double) magnitude == 0.0)
        return target;
      return current + vector4 / magnitude * maxDistanceDelta;
    }

    public static Vector4 Scale(Vector4 a, Vector4 b)
    {
      return new Vector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
    }

    public void Scale(Vector4 scale)
    {
      this.x *= scale.x;
      this.y *= scale.y;
      this.z *= scale.z;
      this.w *= scale.w;
    }

    public override int GetHashCode()
    {
      return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2 ^ this.w.GetHashCode() >> 1;
    }

    public override bool Equals(object other)
    {
      if (!(other is Vector4))
        return false;
      Vector4 vector4 = (Vector4) other;
      return this.x.Equals(vector4.x) && this.y.Equals(vector4.y) && this.z.Equals(vector4.z) && this.w.Equals(vector4.w);
    }

    public static Vector4 Normalize(Vector4 a)
    {
      float num = Vector4.Magnitude(a);
      if ((double) num > 9.99999974737875E-06)
        return a / num;
      return Vector4.zero;
    }

    public void Normalize()
    {
      float num = Vector4.Magnitude(this);
      if ((double) num > 9.99999974737875E-06)
        this = this / num;
      else
        this = Vector4.zero;
    }

    public Vector4 normalized
    {
      get
      {
        return Vector4.Normalize(this);
      }
    }

    public static float Dot(Vector4 a, Vector4 b)
    {
      return (float) ((double) a.x * (double) b.x + (double) a.y * (double) b.y + (double) a.z * (double) b.z + (double) a.w * (double) b.w);
    }

    public static Vector4 Project(Vector4 a, Vector4 b)
    {
      return b * Vector4.Dot(a, b) / Vector4.Dot(b, b);
    }

    public static float Distance(Vector4 a, Vector4 b)
    {
      return Vector4.Magnitude(a - b);
    }

    public static float Magnitude(Vector4 a)
    {
      return Mathf.Sqrt(Vector4.Dot(a, a));
    }

    public float magnitude
    {
      get
      {
        return Mathf.Sqrt(Vector4.Dot(this, this));
      }
    }

    public float sqrMagnitude
    {
      get
      {
        return Vector4.Dot(this, this);
      }
    }

    public static Vector4 Min(Vector4 lhs, Vector4 rhs)
    {
      return new Vector4(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), Mathf.Min(lhs.z, rhs.z), Mathf.Min(lhs.w, rhs.w));
    }

    public static Vector4 Max(Vector4 lhs, Vector4 rhs)
    {
      return new Vector4(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), Mathf.Max(lhs.z, rhs.z), Mathf.Max(lhs.w, rhs.w));
    }

    public static Vector4 zero
    {
      get
      {
        return Vector4.zeroVector;
      }
    }

    public static Vector4 one
    {
      get
      {
        return Vector4.oneVector;
      }
    }

    public static Vector4 positiveInfinity
    {
      get
      {
        return Vector4.positiveInfinityVector;
      }
    }

    public static Vector4 negativeInfinity
    {
      get
      {
        return Vector4.negativeInfinityVector;
      }
    }

    public static Vector4 operator +(Vector4 a, Vector4 b)
    {
      return new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
    }

    public static Vector4 operator -(Vector4 a, Vector4 b)
    {
      return new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
    }

    public static Vector4 operator -(Vector4 a)
    {
      return new Vector4(-a.x, -a.y, -a.z, -a.w);
    }

    public static Vector4 operator *(Vector4 a, float d)
    {
      return new Vector4(a.x * d, a.y * d, a.z * d, a.w * d);
    }

    public static Vector4 operator *(float d, Vector4 a)
    {
      return new Vector4(a.x * d, a.y * d, a.z * d, a.w * d);
    }

    public static Vector4 operator /(Vector4 a, float d)
    {
      return new Vector4(a.x / d, a.y / d, a.z / d, a.w / d);
    }

    public static bool operator ==(Vector4 lhs, Vector4 rhs)
    {
      return (double) Vector4.SqrMagnitude(lhs - rhs) < 9.99999943962493E-11;
    }

    public static bool operator !=(Vector4 lhs, Vector4 rhs)
    {
      return !(lhs == rhs);
    }

    public static implicit operator Vector4(Vector3 v)
    {
      return new Vector4(v.x, v.y, v.z, 0.0f);
    }

    public static implicit operator Vector3(Vector4 v)
    {
      return new Vector3(v.x, v.y, v.z);
    }

    public static implicit operator Vector4(Vector2 v)
    {
      return new Vector4(v.x, v.y, 0.0f, 0.0f);
    }

    public static implicit operator Vector2(Vector4 v)
    {
      return new Vector2(v.x, v.y);
    }

    public override string ToString()
    {
      return UnityString.Format("({0:F1}, {1:F1}, {2:F1}, {3:F1})", (object) this.x, (object) this.y, (object) this.z, (object) this.w);
    }

    public string ToString(string format)
    {
      return UnityString.Format("({0}, {1}, {2}, {3})", (object) this.x.ToString(format), (object) this.y.ToString(format), (object) this.z.ToString(format), (object) this.w.ToString(format));
    }

    public static float SqrMagnitude(Vector4 a)
    {
      return Vector4.Dot(a, a);
    }

    public float SqrMagnitude()
    {
      return Vector4.Dot(this, this);
    }
  }
}
