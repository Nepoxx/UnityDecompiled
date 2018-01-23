// Decompiled with JetBrains decompiler
// Type: UnityEngine.Vector2Int
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  public struct Vector2Int
  {
    private static readonly Vector2Int s_Zero = new Vector2Int(0, 0);
    private static readonly Vector2Int s_One = new Vector2Int(1, 1);
    private static readonly Vector2Int s_Up = new Vector2Int(0, 1);
    private static readonly Vector2Int s_Down = new Vector2Int(0, -1);
    private static readonly Vector2Int s_Left = new Vector2Int(-1, 0);
    private static readonly Vector2Int s_Right = new Vector2Int(1, 0);
    private int m_X;
    private int m_Y;

    public Vector2Int(int x, int y)
    {
      this.m_X = x;
      this.m_Y = y;
    }

    public int x
    {
      get
      {
        return this.m_X;
      }
      set
      {
        this.m_X = value;
      }
    }

    public int y
    {
      get
      {
        return this.m_Y;
      }
      set
      {
        this.m_Y = value;
      }
    }

    public void Set(int x, int y)
    {
      this.m_X = x;
      this.m_Y = y;
    }

    public int this[int index]
    {
      get
      {
        if (index == 0)
          return this.x;
        if (index == 1)
          return this.y;
        throw new IndexOutOfRangeException(string.Format("Invalid Vector2Int index addressed: {0}!", (object) index));
      }
      set
      {
        if (index != 0)
        {
          if (index != 1)
            throw new IndexOutOfRangeException(string.Format("Invalid Vector2Int index addressed: {0}!", (object) index));
          this.y = value;
        }
        else
          this.x = value;
      }
    }

    public float magnitude
    {
      get
      {
        return Mathf.Sqrt((float) (this.x * this.x + this.y * this.y));
      }
    }

    public int sqrMagnitude
    {
      get
      {
        return this.x * this.x + this.y * this.y;
      }
    }

    public static float Distance(Vector2Int a, Vector2Int b)
    {
      return (a - b).magnitude;
    }

    public static Vector2Int Min(Vector2Int lhs, Vector2Int rhs)
    {
      return new Vector2Int(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y));
    }

    public static Vector2Int Max(Vector2Int lhs, Vector2Int rhs)
    {
      return new Vector2Int(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y));
    }

    public static Vector2Int Scale(Vector2Int a, Vector2Int b)
    {
      return new Vector2Int(a.x * b.x, a.y * b.y);
    }

    public void Scale(Vector2Int scale)
    {
      this.x *= scale.x;
      this.y *= scale.y;
    }

    public void Clamp(Vector2Int min, Vector2Int max)
    {
      this.x = Math.Max(min.x, this.x);
      this.x = Math.Min(max.x, this.x);
      this.y = Math.Max(min.y, this.y);
      this.y = Math.Min(max.y, this.y);
    }

    public static implicit operator Vector2(Vector2Int v)
    {
      return new Vector2((float) v.x, (float) v.y);
    }

    public static Vector2Int FloorToInt(Vector2 v)
    {
      return new Vector2Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y));
    }

    public static Vector2Int CeilToInt(Vector2 v)
    {
      return new Vector2Int(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y));
    }

    public static Vector2Int RoundToInt(Vector2 v)
    {
      return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
      return new Vector2Int(a.x + b.x, a.y + b.y);
    }

    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
      return new Vector2Int(a.x - b.x, a.y - b.y);
    }

    public static Vector2Int operator *(Vector2Int a, Vector2Int b)
    {
      return new Vector2Int(a.x * b.x, a.y * b.y);
    }

    public static Vector2Int operator *(Vector2Int a, int b)
    {
      return new Vector2Int(a.x * b, a.y * b);
    }

    public static bool operator ==(Vector2Int lhs, Vector2Int rhs)
    {
      return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    public static bool operator !=(Vector2Int lhs, Vector2Int rhs)
    {
      return !(lhs == rhs);
    }

    public override bool Equals(object other)
    {
      if (!(other is Vector2Int))
        return false;
      Vector2Int vector2Int = (Vector2Int) other;
      return this.x.Equals(vector2Int.x) && this.y.Equals(vector2Int.y);
    }

    public override int GetHashCode()
    {
      return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
    }

    public override string ToString()
    {
      return UnityString.Format("({0}, {1})", (object) this.x, (object) this.y);
    }

    public static Vector2Int zero
    {
      get
      {
        return Vector2Int.s_Zero;
      }
    }

    public static Vector2Int one
    {
      get
      {
        return Vector2Int.s_One;
      }
    }

    public static Vector2Int up
    {
      get
      {
        return Vector2Int.s_Up;
      }
    }

    public static Vector2Int down
    {
      get
      {
        return Vector2Int.s_Down;
      }
    }

    public static Vector2Int left
    {
      get
      {
        return Vector2Int.s_Left;
      }
    }

    public static Vector2Int right
    {
      get
      {
        return Vector2Int.s_Right;
      }
    }
  }
}
