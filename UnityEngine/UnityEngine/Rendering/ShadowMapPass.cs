// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.ShadowMapPass
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Rendering
{
  [Flags]
  public enum ShadowMapPass
  {
    PointlightPositiveX = 1,
    PointlightNegativeX = 2,
    PointlightPositiveY = 4,
    PointlightNegativeY = 8,
    PointlightPositiveZ = 16, // 0x00000010
    PointlightNegativeZ = 32, // 0x00000020
    DirectionalCascade0 = 64, // 0x00000040
    DirectionalCascade1 = 128, // 0x00000080
    DirectionalCascade2 = 256, // 0x00000100
    DirectionalCascade3 = 512, // 0x00000200
    Spotlight = 1024, // 0x00000400
    Pointlight = PointlightNegativeZ | PointlightPositiveZ | PointlightNegativeY | PointlightPositiveY | PointlightNegativeX | PointlightPositiveX, // 0x0000003F
    Directional = DirectionalCascade3 | DirectionalCascade2 | DirectionalCascade1 | DirectionalCascade0, // 0x000003C0
    All = Directional | Pointlight | Spotlight, // 0x000007FF
  }
}
