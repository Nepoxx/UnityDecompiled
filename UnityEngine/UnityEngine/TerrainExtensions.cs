// Decompiled with JetBrains decompiler
// Type: UnityEngine.TerrainExtensions
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public static class TerrainExtensions
  {
    public static void UpdateGIMaterials(this Terrain terrain)
    {
      if ((Object) terrain.terrainData == (Object) null)
        throw new ArgumentException("Invalid terrainData.");
      TerrainExtensions.UpdateGIMaterialsForTerrain(terrain.GetInstanceID(), new Rect(0.0f, 0.0f, 1f, 1f));
    }

    public static void UpdateGIMaterials(this Terrain terrain, int x, int y, int width, int height)
    {
      if ((Object) terrain.terrainData == (Object) null)
        throw new ArgumentException("Invalid terrainData.");
      float alphamapWidth = (float) terrain.terrainData.alphamapWidth;
      float alphamapHeight = (float) terrain.terrainData.alphamapHeight;
      TerrainExtensions.UpdateGIMaterialsForTerrain(terrain.GetInstanceID(), new Rect((float) x / alphamapWidth, (float) y / alphamapHeight, (float) width / alphamapWidth, (float) height / alphamapHeight));
    }

    internal static void UpdateGIMaterialsForTerrain(int terrainInstanceID, Rect uvBounds)
    {
      TerrainExtensions.UpdateGIMaterialsForTerrain_Injected(terrainInstanceID, ref uvBounds);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void UpdateGIMaterialsForTerrain_Injected(int terrainInstanceID, ref Rect uvBounds);
  }
}
