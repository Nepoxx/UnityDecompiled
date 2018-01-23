// Decompiled with JetBrains decompiler
// Type: UnityEditor.GUID
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [RequiredByNativeCode]
  [Serializable]
  public struct GUID : IComparable, IComparable<GUID>
  {
    private uint m_Value0;
    private uint m_Value1;
    private uint m_Value2;
    private uint m_Value3;

    public GUID(string hexRepresentation)
    {
      this.m_Value0 = 0U;
      this.m_Value1 = 0U;
      this.m_Value2 = 0U;
      this.m_Value3 = 0U;
      GUID.TryParse(hexRepresentation, out this);
    }

    public static bool operator ==(GUID x, GUID y)
    {
      return (int) x.m_Value0 == (int) y.m_Value0 && (int) x.m_Value1 == (int) y.m_Value1 && (int) x.m_Value2 == (int) y.m_Value2 && (int) x.m_Value3 == (int) y.m_Value3;
    }

    public static bool operator !=(GUID x, GUID y)
    {
      return !(x == y);
    }

    public static bool operator <(GUID x, GUID y)
    {
      if ((int) x.m_Value0 != (int) y.m_Value0)
        return x.m_Value0 < y.m_Value0;
      if ((int) x.m_Value1 != (int) y.m_Value1)
        return x.m_Value1 < y.m_Value1;
      if ((int) x.m_Value2 != (int) y.m_Value2)
        return x.m_Value2 < y.m_Value2;
      return x.m_Value3 < y.m_Value3;
    }

    public static bool operator >(GUID x, GUID y)
    {
      return !(x < y) && !(x == y);
    }

    public override bool Equals(object obj)
    {
      if (obj == null || !(obj is GUID))
        return false;
      return (GUID) obj == this;
    }

    public override int GetHashCode()
    {
      return (((int) this.m_Value0 * 397 ^ (int) this.m_Value1) * 397 ^ (int) this.m_Value2) * 397 ^ (int) this.m_Value3;
    }

    public int CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      return this.CompareTo((GUID) obj);
    }

    public int CompareTo(GUID rhs)
    {
      if (this < rhs)
        return -1;
      return this > rhs ? 1 : 0;
    }

    public bool Empty()
    {
      return (int) this.m_Value0 == 0 && (int) this.m_Value1 == 0 && (int) this.m_Value2 == 0 && (int) this.m_Value3 == 0;
    }

    [Obsolete("Use TryParse instead")]
    public bool ParseExact(string hex)
    {
      return GUID.TryParse(hex, out this);
    }

    public static bool TryParse(string hex, out GUID result)
    {
      result = GUID.HexToGUIDInternal(hex);
      return !result.Empty();
    }

    public static GUID Generate()
    {
      return GUID.GenerateGUIDInternal();
    }

    public override string ToString()
    {
      return GUID.GUIDToHexInternal(ref this);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GUIDToHexInternal(ref GUID value);

    private static GUID HexToGUIDInternal(string hex)
    {
      GUID ret;
      GUID.HexToGUIDInternal_Injected(hex, out ret);
      return ret;
    }

    private static GUID GenerateGUIDInternal()
    {
      GUID ret;
      GUID.GenerateGUIDInternal_Injected(out ret);
      return ret;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void HexToGUIDInternal_Injected(string hex, out GUID ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GenerateGUIDInternal_Injected(out GUID ret);
  }
}
