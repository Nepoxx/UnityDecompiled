// Decompiled with JetBrains decompiler
// Type: UnityEngine.TerrainChangedFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Flags]
  public enum TerrainChangedFlags
  {
    Heightmap = 1,
    TreeInstances = 2,
    DelayedHeightmapUpdate = 4,
    FlushEverythingImmediately = 8,
    RemoveDirtyDetailsImmediately = 16, // 0x00000010
    WillBeDestroyed = 256, // 0x00000100
  }
}
