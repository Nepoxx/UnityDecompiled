// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.GraphicsDeviceType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
  [UsedByNativeCode]
  public enum GraphicsDeviceType
  {
    [Obsolete("OpenGL2 is no longer supported in Unity 5.5+")] OpenGL2 = 0,
    [Obsolete("Direct3D 9 is no longer supported in Unity 2017.2+")] Direct3D9 = 1,
    Direct3D11 = 2,
    [Obsolete("PS3 is no longer supported in Unity 5.5+")] PlayStation3 = 3,
    Null = 4,
    [Obsolete("Xbox360 is no longer supported in Unity 5.5+")] Xbox360 = 6,
    OpenGLES2 = 8,
    OpenGLES3 = 11, // 0x0000000B
    PlayStationVita = 12, // 0x0000000C
    PlayStation4 = 13, // 0x0000000D
    XboxOne = 14, // 0x0000000E
    PlayStationMobile = 15, // 0x0000000F
    Metal = 16, // 0x00000010
    OpenGLCore = 17, // 0x00000011
    Direct3D12 = 18, // 0x00000012
    N3DS = 19, // 0x00000013
    Vulkan = 21, // 0x00000015
    XboxOneD3D12 = 23, // 0x00000017
  }
}
