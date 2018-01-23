// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.ColorWriteMask
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Rendering
{
  [Flags]
  public enum ColorWriteMask
  {
    Alpha = 1,
    Blue = 2,
    Green = 4,
    Red = 8,
    All = Red | Green | Blue | Alpha, // 0x0000000F
  }
}
