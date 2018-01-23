// Decompiled with JetBrains decompiler
// Type: UnityEngine.TerrainRenderFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Flags]
  public enum TerrainRenderFlags
  {
    [Obsolete("TerrainRenderFlags.heightmap is obsolete, use TerrainRenderFlags.Heightmap instead. (UnityUpgradable) -> Heightmap")] heightmap = 1,
    [Obsolete("TerrainRenderFlags.trees is obsolete, use TerrainRenderFlags.Trees instead. (UnityUpgradable) -> Trees")] trees = 2,
    [Obsolete("TerrainRenderFlags.details is obsolete, use TerrainRenderFlags.Details instead. (UnityUpgradable) -> Details")] details = 4,
    [Obsolete("TerrainRenderFlags.all is obsolete, use TerrainRenderFlags.All instead. (UnityUpgradable) -> All")] all = details | trees | heightmap, // 0x00000007
    Heightmap = heightmap, // 0x00000001
    Trees = trees, // 0x00000002
    Details = details, // 0x00000004
    All = Details | Trees | Heightmap, // 0x00000007
  }
}
