// Decompiled with JetBrains decompiler
// Type: UnityEngine.Tilemaps.TileBase
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps
{
  [RequiredByNativeCode]
  public abstract class TileBase : ScriptableObject
  {
    public virtual void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
      tilemap.RefreshTile(position);
    }

    public virtual void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
    }

    private TileData GetTileDataNoRef(Vector3Int position, ITilemap tilemap)
    {
      TileData tileData = new TileData();
      this.GetTileData(position, tilemap, ref tileData);
      return tileData;
    }

    public virtual bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
      return false;
    }

    private TileAnimationData GetTileAnimationDataNoRef(Vector3Int position, ITilemap tilemap)
    {
      TileAnimationData tileAnimationData = new TileAnimationData();
      this.GetTileAnimationData(position, tilemap, ref tileAnimationData);
      return tileAnimationData;
    }

    public virtual bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
      return false;
    }
  }
}
