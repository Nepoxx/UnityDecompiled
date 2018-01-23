// Decompiled with JetBrains decompiler
// Type: UnityEngine.ParticleSystemSubEmitterProperties
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The properties of sub-emitter particles.</para>
  /// </summary>
  [Flags]
  public enum ParticleSystemSubEmitterProperties
  {
    InheritNothing = 0,
    InheritEverything = 15, // 0x0000000F
    InheritColor = 1,
    InheritSize = 2,
    InheritRotation = 4,
    InheritLifetime = 8,
  }
}
