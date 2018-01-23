// Decompiled with JetBrains decompiler
// Type: UnityEngine.RigidbodyConstraints2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Flags]
  public enum RigidbodyConstraints2D
  {
    None = 0,
    FreezePositionX = 1,
    FreezePositionY = 2,
    FreezeRotation = 4,
    FreezePosition = FreezePositionY | FreezePositionX, // 0x00000003
    FreezeAll = FreezePosition | FreezeRotation, // 0x00000007
  }
}
