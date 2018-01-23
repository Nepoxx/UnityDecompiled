// Decompiled with JetBrains decompiler
// Type: UnityEngine.Assertions.Comparers.FloatComparer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Assertions.Comparers
{
  public class FloatComparer : IEqualityComparer<float>
  {
    public static readonly FloatComparer s_ComparerWithDefaultTolerance = new FloatComparer(1E-05f);
    private readonly float m_Error;
    private readonly bool m_Relative;
    public const float kEpsilon = 1E-05f;

    public FloatComparer()
      : this(1E-05f, false)
    {
    }

    public FloatComparer(bool relative)
      : this(1E-05f, relative)
    {
    }

    public FloatComparer(float error)
      : this(error, false)
    {
    }

    public FloatComparer(float error, bool relative)
    {
      this.m_Error = error;
      this.m_Relative = relative;
    }

    public bool Equals(float a, float b)
    {
      return !this.m_Relative ? FloatComparer.AreEqual(a, b, this.m_Error) : FloatComparer.AreEqualRelative(a, b, this.m_Error);
    }

    public int GetHashCode(float obj)
    {
      return this.GetHashCode();
    }

    public static bool AreEqual(float expected, float actual, float error)
    {
      return (double) Math.Abs(actual - expected) <= (double) error;
    }

    public static bool AreEqualRelative(float expected, float actual, float error)
    {
      if ((double) expected == (double) actual)
        return true;
      float num1 = Math.Abs(expected);
      float num2 = Math.Abs(actual);
      return (double) Math.Abs((float) (((double) actual - (double) expected) / ((double) num1 <= (double) num2 ? (double) num2 : (double) num1))) <= (double) error;
    }
  }
}
