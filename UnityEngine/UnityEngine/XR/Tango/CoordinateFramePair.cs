// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.Tango.CoordinateFramePair
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.XR.Tango
{
  [UsedByNativeCode]
  [StructLayout(LayoutKind.Explicit, Size = 8)]
  internal struct CoordinateFramePair
  {
    [FieldOffset(0)]
    public CoordinateFrame baseFrame;
    [FieldOffset(4)]
    public CoordinateFrame targetFrame;
  }
}
