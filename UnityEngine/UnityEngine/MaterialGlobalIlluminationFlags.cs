// Decompiled with JetBrains decompiler
// Type: UnityEngine.MaterialGlobalIlluminationFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Flags]
  public enum MaterialGlobalIlluminationFlags
  {
    None = 0,
    RealtimeEmissive = 1,
    BakedEmissive = 2,
    EmissiveIsBlack = 4,
    AnyEmissive = BakedEmissive | RealtimeEmissive, // 0x00000003
  }
}
