// Decompiled with JetBrains decompiler
// Type: UnityEngine.BoundsInt
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  public struct BoundsInt
  {
    private Vector3Int m_Position;
    private Vector3Int m_Size;

    public BoundsInt(int xMin, int yMin, int zMin, int sizeX, int sizeY, int sizeZ)
    {
      this.m_Position = new Vector3Int(xMin, yMin, zMin);
      this.m_Size = new Vector3Int(sizeX, sizeY, sizeZ);
    }

    public BoundsInt(Vector3Int position, Vector3Int size)
    {
      this.m_Position = position;
      this.m_Size = size;
    }

    public int x
    {
      get
      {
        return this.m_Position.x;
      }
      set
      {
        this.m_Position.x = value;
      }
    }

    public int y
    {
      get
      {
        return this.m_Position.y;
      }
      set
      {
        this.m_Position.y = value;
      }
    }

    public int z
    {
      get
      {
        return this.m_Position.z;
      }
      set
      {
        this.m_Position.z = value;
      }
    }

    public Vector3 center
    {
      get
      {
        return new Vector3((float) this.x + (float) this.m_Size.x / 2f, (float) this.y + (float) this.m_Size.y / 2f, (float) this.z + (float) this.m_Size.z / 2f);
      }
    }

    public Vector3Int min
    {
      get
      {
        return new Vector3Int(this.xMin, this.yMin, this.zMin);
      }
      set
      {
        this.xMin = value.x;
        this.yMin = value.y;
        this.zMin = value.z;
      }
    }

    public Vector3Int max
    {
      get
      {
        return new Vector3Int(this.xMax, this.yMax, this.zMax);
      }
      set
      {
        this.xMax = value.x;
        this.yMax = value.y;
        this.zMax = value.z;
      }
    }

    public int xMin
    {
      get
      {
        return Math.Min(this.m_Position.x, this.m_Position.x + this.m_Size.x);
      }
      set
      {
        int xMax = this.xMax;
        this.m_Position.x = value;
        this.m_Size.x = xMax - this.m_Position.x;
      }
    }

    public int yMin
    {
      get
      {
        return Math.Min(this.m_Position.y, this.m_Position.y + this.m_Size.y);
      }
      set
      {
        int yMax = this.yMax;
        this.m_Position.y = value;
        this.m_Size.y = yMax - this.m_Position.y;
      }
    }

    public int zMin
    {
      get
      {
        return Math.Min(this.m_Position.z, this.m_Position.z + this.m_Size.z);
      }
      set
      {
        int zMax = this.zMax;
        this.m_Position.z = value;
        this.m_Size.z = zMax - this.m_Position.z;
      }
    }

    public int xMax
    {
      get
      {
        return Math.Max(this.m_Position.x, this.m_Position.x + this.m_Size.x);
      }
      set
      {
        this.m_Size.x = value - this.m_Position.x;
      }
    }

    public int yMax
    {
      get
      {
        return Math.Max(this.m_Position.y, this.m_Position.y + this.m_Size.y);
      }
      set
      {
        this.m_Size.y = value - this.m_Position.y;
      }
    }

    public int zMax
    {
      get
      {
        return Math.Max(this.m_Position.z, this.m_Position.z + this.m_Size.z);
      }
      set
      {
        this.m_Size.z = value - this.m_Position.z;
      }
    }

    public Vector3Int position
    {
      get
      {
        return this.m_Position;
      }
      set
      {
        this.m_Position = value;
      }
    }

    public Vector3Int size
    {
      get
      {
        return this.m_Size;
      }
      set
      {
        this.m_Size = value;
      }
    }

    public void SetMinMax(Vector3Int minPosition, Vector3Int maxPosition)
    {
      this.min = minPosition;
      this.max = maxPosition;
    }

    public void ClampToBounds(BoundsInt bounds)
    {
      this.position = new Vector3Int(Math.Max(Math.Min(bounds.xMax, this.position.x), bounds.xMin), Math.Max(Math.Min(bounds.yMax, this.position.y), bounds.yMin), Math.Max(Math.Min(bounds.zMax, this.position.z), bounds.zMin));
      this.size = new Vector3Int(Math.Min(bounds.xMax - this.position.x, this.size.x), Math.Min(bounds.yMax - this.position.y, this.size.y), Math.Min(bounds.zMax - this.position.z, this.size.z));
    }

    public bool Contains(Vector3Int position)
    {
      return position.x >= this.m_Position.x && position.y >= this.m_Position.y && (position.z >= this.m_Position.z && position.x < this.m_Position.x + this.m_Size.x) && position.y < this.m_Position.y + this.m_Size.y && position.z < this.m_Position.z + this.m_Size.z;
    }

    public override string ToString()
    {
      return UnityString.Format("Position: {0}, Size: {1}", (object) this.m_Position, (object) this.m_Size);
    }

    public static bool operator ==(BoundsInt lhs, BoundsInt rhs)
    {
      return lhs.m_Position == rhs.m_Position && lhs.m_Size == rhs.m_Size;
    }

    public static bool operator !=(BoundsInt lhs, BoundsInt rhs)
    {
      return !(lhs == rhs);
    }

    public override bool Equals(object other)
    {
      if (!(other is BoundsInt))
        return false;
      BoundsInt boundsInt = (BoundsInt) other;
      return this.m_Position.Equals((object) boundsInt.m_Position) && this.m_Size.Equals((object) boundsInt.m_Size);
    }

    public override int GetHashCode()
    {
      return this.m_Position.GetHashCode() ^ this.m_Size.GetHashCode() << 2;
    }

    public BoundsInt.PositionEnumerator allPositionsWithin
    {
      get
      {
        return new BoundsInt.PositionEnumerator(this.min, this.max);
      }
    }

    public struct PositionEnumerator : IEnumerator<Vector3Int>, IEnumerator, IDisposable
    {
      private readonly Vector3Int _min;
      private readonly Vector3Int _max;
      private Vector3Int _current;

      public PositionEnumerator(Vector3Int min, Vector3Int max)
      {
        this._min = this._current = min;
        this._max = max;
        this.Reset();
      }

      public BoundsInt.PositionEnumerator GetEnumerator()
      {
        return this;
      }

      public bool MoveNext()
      {
        if (this._current.z >= this._max.z)
          return false;
        ++this._current.x;
        if (this._current.x >= this._max.x)
        {
          this._current.x = this._min.x;
          ++this._current.y;
          if (this._current.y >= this._max.y)
          {
            this._current.y = this._min.y;
            ++this._current.z;
            if (this._current.z >= this._max.z)
              return false;
          }
        }
        return true;
      }

      public void Reset()
      {
        this._current = this._min;
        --this._current.x;
      }

      public Vector3Int Current
      {
        get
        {
          return this._current;
        }
      }

      object IEnumerator.Current
      {
        get
        {
          return (object) this.Current;
        }
      }

      void IDisposable.Dispose()
      {
      }
    }
  }
}
