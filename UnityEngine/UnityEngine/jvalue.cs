// Decompiled with JetBrains decompiler
// Type: UnityEngine.jvalue
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
  [StructLayout(LayoutKind.Explicit)]
  public struct jvalue
  {
    [FieldOffset(0)]
    public bool z;
    [FieldOffset(0)]
    public byte b;
    [FieldOffset(0)]
    public char c;
    [FieldOffset(0)]
    public short s;
    [FieldOffset(0)]
    public int i;
    [FieldOffset(0)]
    public long j;
    [FieldOffset(0)]
    public float f;
    [FieldOffset(0)]
    public double d;
    [FieldOffset(0)]
    public IntPtr l;
  }
}
