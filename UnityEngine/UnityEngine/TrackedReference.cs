// Decompiled with JetBrains decompiler
// Type: UnityEngine.TrackedReference
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public class TrackedReference
  {
    internal IntPtr m_Ptr;

    protected TrackedReference()
    {
    }

    public static bool operator ==(TrackedReference x, TrackedReference y)
    {
      object obj1 = (object) x;
      object obj2 = (object) y;
      if (obj2 == null && obj1 == null)
        return true;
      if (obj2 == null)
        return x.m_Ptr == IntPtr.Zero;
      if (obj1 == null)
        return y.m_Ptr == IntPtr.Zero;
      return x.m_Ptr == y.m_Ptr;
    }

    public static bool operator !=(TrackedReference x, TrackedReference y)
    {
      return !(x == y);
    }

    public override bool Equals(object o)
    {
      return o as TrackedReference == this;
    }

    public override int GetHashCode()
    {
      return (int) this.m_Ptr;
    }

    public static implicit operator bool(TrackedReference exists)
    {
      return exists != (TrackedReference) null;
    }
  }
}
