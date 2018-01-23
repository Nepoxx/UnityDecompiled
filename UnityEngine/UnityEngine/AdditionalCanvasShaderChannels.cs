// Decompiled with JetBrains decompiler
// Type: UnityEngine.AdditionalCanvasShaderChannels
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Enum mask of possible shader channel properties that can also be included when the Canvas mesh is created.</para>
  /// </summary>
  [Flags]
  public enum AdditionalCanvasShaderChannels
  {
    None = 0,
    TexCoord1 = 1,
    TexCoord2 = 2,
    TexCoord3 = 4,
    Normal = 8,
    Tangent = 16, // 0x00000010
  }
}
