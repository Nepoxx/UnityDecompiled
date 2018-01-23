// Decompiled with JetBrains decompiler
// Type: UnityEngine.Tilemaps.TileFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Tilemaps
{
  [Flags]
  public enum TileFlags
  {
    None = 0,
    LockColor = 1,
    LockTransform = 2,
    InstantiateGameObjectRuntimeOnly = 4,
    LockAll = LockTransform | LockColor, // 0x00000003
  }
}
