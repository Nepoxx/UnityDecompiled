// Decompiled with JetBrains decompiler
// Type: UnityEngine.RenderTextureCreationFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Flags]
  public enum RenderTextureCreationFlags
  {
    MipMap = 1,
    AutoGenerateMips = 2,
    SRGB = 4,
    EyeTexture = 8,
    EnableRandomWrite = 16, // 0x00000010
    CreatedFromScript = 32, // 0x00000020
    AllowVerticalFlip = 128, // 0x00000080
    NoResolvedColorSurface = 256, // 0x00000100
    DynamicallyScalable = 1024, // 0x00000400
  }
}
