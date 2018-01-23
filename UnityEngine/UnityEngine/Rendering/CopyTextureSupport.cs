// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.CopyTextureSupport
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Rendering
{
  [Flags]
  public enum CopyTextureSupport
  {
    None = 0,
    Basic = 1,
    Copy3D = 2,
    DifferentTypes = 4,
    TextureToRT = 8,
    RTToTexture = 16, // 0x00000010
  }
}
