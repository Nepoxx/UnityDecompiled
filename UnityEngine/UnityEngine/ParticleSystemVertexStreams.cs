// Decompiled with JetBrains decompiler
// Type: UnityEngine.ParticleSystemVertexStreams
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>All possible particle system vertex shader inputs.</para>
  /// </summary>
  [Flags]
  [Obsolete("ParticleSystemVertexStreams is deprecated. Please use ParticleSystemVertexStream instead.")]
  public enum ParticleSystemVertexStreams
  {
    Position = 1,
    Normal = 2,
    Tangent = 4,
    Color = 8,
    UV = 16, // 0x00000010
    UV2BlendAndFrame = 32, // 0x00000020
    CenterAndVertexID = 64, // 0x00000040
    Size = 128, // 0x00000080
    Rotation = 256, // 0x00000100
    Velocity = 512, // 0x00000200
    Lifetime = 1024, // 0x00000400
    Custom1 = 2048, // 0x00000800
    Custom2 = 4096, // 0x00001000
    Random = 8192, // 0x00002000
    None = 0,
    All = 2147483647, // 0x7FFFFFFF
  }
}
