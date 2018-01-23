// Decompiled with JetBrains decompiler
// Type: UnityEngine.Tilemaps.Tilemap
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Tilemaps
{
  [RequireComponent(typeof (Transform))]
  [NativeType(Header = "Modules/Tilemap/Public/Tilemap.h")]
  public sealed class Tilemap : GridLayout
  {
    public extern Grid layoutGrid { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public Vector3 GetCellCenterLocal(Vector3Int position)
    {
      return this.CellToLocalInterpolated((Vector3) position + this.tileAnchor);
    }

    public Vector3 GetCellCenterWorld(Vector3Int position)
    {
      return this.LocalToWorld(this.CellToLocalInterpolated((Vector3) position + this.tileAnchor));
    }

    public BoundsInt cellBounds
    {
      get
      {
        return new BoundsInt(this.origin, this.size);
      }
    }

    public Bounds localBounds
    {
      get
      {
        Bounds ret;
        this.get_localBounds_Injected(out ret);
        return ret;
      }
    }

    public extern float animationFrameRate { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public Color color
    {
      get
      {
        Color ret;
        this.get_color_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_color_Injected(ref value);
      }
    }

    public Vector3Int origin
    {
      get
      {
        Vector3Int ret;
        this.get_origin_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_origin_Injected(ref value);
      }
    }

    public Vector3Int size
    {
      get
      {
        Vector3Int ret;
        this.get_size_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_size_Injected(ref value);
      }
    }

    public Vector3 tileAnchor
    {
      get
      {
        Vector3 ret;
        this.get_tileAnchor_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_tileAnchor_Injected(ref value);
      }
    }

    public extern Tilemap.Orientation orientation { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public Matrix4x4 orientationMatrix
    {
      get
      {
        Matrix4x4 ret;
        this.get_orientationMatrix_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_orientationMatrix_Injected(ref value);
      }
    }

    internal Object GetTileAsset(Vector3Int position)
    {
      return this.GetTileAsset_Injected(ref position);
    }

    public TileBase GetTile(Vector3Int position)
    {
      return (TileBase) this.GetTileAsset(position);
    }

    public T GetTile<T>(Vector3Int position) where T : TileBase
    {
      return this.GetTileAsset(position) as T;
    }

    internal Object[] GetTileAssetsBlock(Vector3Int position, Vector3Int blockDimensions)
    {
      return this.GetTileAssetsBlock_Injected(ref position, ref blockDimensions);
    }

    public TileBase[] GetTilesBlock(BoundsInt bounds)
    {
      Object[] tileAssetsBlock = this.GetTileAssetsBlock(bounds.min, bounds.size);
      TileBase[] tileBaseArray = new TileBase[tileAssetsBlock.Length];
      for (int index = 0; index < tileAssetsBlock.Length; ++index)
        tileBaseArray[index] = (TileBase) tileAssetsBlock[index];
      return tileBaseArray;
    }

    internal void SetTileAsset(Vector3Int position, Object tile)
    {
      this.SetTileAsset_Injected(ref position, tile);
    }

    public void SetTile(Vector3Int position, TileBase tile)
    {
      this.SetTileAsset(position, (Object) tile);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetTileAssets(Vector3Int[] positionArray, Object[] tileArray);

    public void SetTiles(Vector3Int[] positionArray, TileBase[] tileArray)
    {
      this.SetTileAssets(positionArray, (Object[]) tileArray);
    }

    private void INTERNAL_CALL_SetTileAssetsBlock(Vector3Int position, Vector3Int blockDimensions, Object[] tileArray)
    {
      this.INTERNAL_CALL_SetTileAssetsBlock_Injected(ref position, ref blockDimensions, tileArray);
    }

    public void SetTilesBlock(BoundsInt position, TileBase[] tileArray)
    {
      this.INTERNAL_CALL_SetTileAssetsBlock(position.min, position.size, (Object[]) tileArray);
    }

    public bool HasTile(Vector3Int position)
    {
      return this.GetTileAsset(position) != (Object) null;
    }

    public void RefreshTile(Vector3Int position)
    {
      this.RefreshTile_Injected(ref position);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void RefreshAllTiles();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SwapTileAsset(Object changeTile, Object newTile);

    public void SwapTile(TileBase changeTile, TileBase newTile)
    {
      this.SwapTileAsset((Object) changeTile, (Object) newTile);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool ContainsTileAsset(Object tileAsset);

    public bool ContainsTile(TileBase tileAsset)
    {
      return this.ContainsTileAsset((Object) tileAsset);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int GetUsedTilesCount();

    public int GetUsedTilesNonAlloc(TileBase[] usedTiles)
    {
      return this.Internal_GetUsedTilesNonAlloc((Object[]) usedTiles);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern int Internal_GetUsedTilesNonAlloc(Object[] usedTiles);

    public Sprite GetSprite(Vector3Int position)
    {
      return this.GetSprite_Injected(ref position);
    }

    public Matrix4x4 GetTransformMatrix(Vector3Int position)
    {
      Matrix4x4 ret;
      this.GetTransformMatrix_Injected(ref position, out ret);
      return ret;
    }

    public void SetTransformMatrix(Vector3Int position, Matrix4x4 transform)
    {
      this.SetTransformMatrix_Injected(ref position, ref transform);
    }

    public Color GetColor(Vector3Int position)
    {
      Color ret;
      this.GetColor_Injected(ref position, out ret);
      return ret;
    }

    public void SetColor(Vector3Int position, Color color)
    {
      this.SetColor_Injected(ref position, ref color);
    }

    public TileFlags GetTileFlags(Vector3Int position)
    {
      return this.GetTileFlags_Injected(ref position);
    }

    public void SetTileFlags(Vector3Int position, TileFlags flags)
    {
      this.SetTileFlags_Injected(ref position, flags);
    }

    public void AddTileFlags(Vector3Int position, TileFlags flags)
    {
      this.AddTileFlags_Injected(ref position, flags);
    }

    public void RemoveTileFlags(Vector3Int position, TileFlags flags)
    {
      this.RemoveTileFlags_Injected(ref position, flags);
    }

    public GameObject GetInstantiatedObject(Vector3Int position)
    {
      return this.GetInstantiatedObject_Injected(ref position);
    }

    public void SetColliderType(Vector3Int position, Tile.ColliderType colliderType)
    {
      this.SetColliderType_Injected(ref position, colliderType);
    }

    public Tile.ColliderType GetColliderType(Vector3Int position)
    {
      return this.GetColliderType_Injected(ref position);
    }

    public void FloodFill(Vector3Int position, TileBase tile)
    {
      this.FloodFillTileAsset(position, (Object) tile);
    }

    private void FloodFillTileAsset(Vector3Int position, Object tile)
    {
      this.FloodFillTileAsset_Injected(ref position, tile);
    }

    public void BoxFill(Vector3Int position, TileBase tile, int startX, int startY, int endX, int endY)
    {
      this.BoxFillTileAsset(position, (Object) tile, startX, startY, endX, endY);
    }

    private void BoxFillTileAsset(Vector3Int position, Object tile, int startX, int startY, int endX, int endY)
    {
      this.BoxFillTileAsset_Injected(ref position, tile, startX, startY, endX, endY);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearAllTiles();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ResizeBounds();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void CompressBounds();

    public Vector3Int editorPreviewOrigin
    {
      get
      {
        Vector3Int ret;
        this.get_editorPreviewOrigin_Injected(out ret);
        return ret;
      }
    }

    public Vector3Int editorPreviewSize
    {
      get
      {
        Vector3Int ret;
        this.get_editorPreviewSize_Injected(out ret);
        return ret;
      }
    }

    internal Object GetEditorPreviewTileAsset(Vector3Int position)
    {
      return this.GetEditorPreviewTileAsset_Injected(ref position);
    }

    public TileBase GetEditorPreviewTile(Vector3Int position)
    {
      return this.GetEditorPreviewTileAsset(position) as TileBase;
    }

    public T GetEditorPreviewTile<T>(Vector3Int position) where T : TileBase
    {
      return this.GetEditorPreviewTile(position) as T;
    }

    internal void SetEditorPreviewTileAsset(Vector3Int position, Object tile)
    {
      this.SetEditorPreviewTileAsset_Injected(ref position, tile);
    }

    public void SetEditorPreviewTile(Vector3Int position, TileBase tile)
    {
      this.SetEditorPreviewTileAsset(position, (Object) tile);
    }

    public bool HasEditorPreviewTile(Vector3Int position)
    {
      return this.GetEditorPreviewTileAsset(position) != (Object) null;
    }

    public Sprite GetEditorPreviewSprite(Vector3Int position)
    {
      return this.GetEditorPreviewSprite_Injected(ref position);
    }

    public Matrix4x4 GetEditorPreviewTransformMatrix(Vector3Int position)
    {
      Matrix4x4 ret;
      this.GetEditorPreviewTransformMatrix_Injected(ref position, out ret);
      return ret;
    }

    public void SetEditorPreviewTransformMatrix(Vector3Int position, Matrix4x4 transform)
    {
      this.SetEditorPreviewTransformMatrix_Injected(ref position, ref transform);
    }

    public Color GetEditorPreviewColor(Vector3Int position)
    {
      Color ret;
      this.GetEditorPreviewColor_Injected(ref position, out ret);
      return ret;
    }

    public void SetEditorPreviewColor(Vector3Int position, Color color)
    {
      this.SetEditorPreviewColor_Injected(ref position, ref color);
    }

    public TileFlags GetEditorPreviewTileFlags(Vector3Int position)
    {
      return this.GetEditorPreviewTileFlags_Injected(ref position);
    }

    public void EditorPreviewFloodFill(Vector3Int position, TileBase tile)
    {
      this.EditorPreviewFloodFillTileAsset(position, (Object) tile);
    }

    private void EditorPreviewFloodFillTileAsset(Vector3Int position, Object tile)
    {
      this.EditorPreviewFloodFillTileAsset_Injected(ref position, tile);
    }

    public void EditorPreviewBoxFill(Vector3Int position, Object tile, int startX, int startY, int endX, int endY)
    {
      this.EditorPreviewBoxFillTileAsset(position, tile, startX, startY, endX, endY);
    }

    private void EditorPreviewBoxFillTileAsset(Vector3Int position, Object tile, int startX, int startY, int endX, int endY)
    {
      this.EditorPreviewBoxFillTileAsset_Injected(ref position, tile, startX, startY, endX, endY);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearAllEditorPreviewTiles();

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_localBounds_Injected(out Bounds ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_color_Injected(out Color ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_color_Injected(ref Color value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_origin_Injected(out Vector3Int ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_origin_Injected(ref Vector3Int value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_size_Injected(out Vector3Int ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_size_Injected(ref Vector3Int value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_tileAnchor_Injected(out Vector3 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_tileAnchor_Injected(ref Vector3 value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_orientationMatrix_Injected(out Matrix4x4 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_orientationMatrix_Injected(ref Matrix4x4 value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Object GetTileAsset_Injected(ref Vector3Int position);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Object[] GetTileAssetsBlock_Injected(ref Vector3Int position, ref Vector3Int blockDimensions);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetTileAsset_Injected(ref Vector3Int position, Object tile);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_CALL_SetTileAssetsBlock_Injected(ref Vector3Int position, ref Vector3Int blockDimensions, Object[] tileArray);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void RefreshTile_Injected(ref Vector3Int position);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Sprite GetSprite_Injected(ref Vector3Int position);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetTransformMatrix_Injected(ref Vector3Int position, out Matrix4x4 ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetTransformMatrix_Injected(ref Vector3Int position, ref Matrix4x4 transform);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetColor_Injected(ref Vector3Int position, out Color ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetColor_Injected(ref Vector3Int position, ref Color color);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern TileFlags GetTileFlags_Injected(ref Vector3Int position);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetTileFlags_Injected(ref Vector3Int position, TileFlags flags);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void AddTileFlags_Injected(ref Vector3Int position, TileFlags flags);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void RemoveTileFlags_Injected(ref Vector3Int position, TileFlags flags);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern GameObject GetInstantiatedObject_Injected(ref Vector3Int position);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetColliderType_Injected(ref Vector3Int position, Tile.ColliderType colliderType);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Tile.ColliderType GetColliderType_Injected(ref Vector3Int position);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void FloodFillTileAsset_Injected(ref Vector3Int position, Object tile);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void BoxFillTileAsset_Injected(ref Vector3Int position, Object tile, int startX, int startY, int endX, int endY);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_editorPreviewOrigin_Injected(out Vector3Int ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_editorPreviewSize_Injected(out Vector3Int ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Object GetEditorPreviewTileAsset_Injected(ref Vector3Int position);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetEditorPreviewTileAsset_Injected(ref Vector3Int position, Object tile);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Sprite GetEditorPreviewSprite_Injected(ref Vector3Int position);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetEditorPreviewTransformMatrix_Injected(ref Vector3Int position, out Matrix4x4 ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetEditorPreviewTransformMatrix_Injected(ref Vector3Int position, ref Matrix4x4 transform);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetEditorPreviewColor_Injected(ref Vector3Int position, out Color ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetEditorPreviewColor_Injected(ref Vector3Int position, ref Color color);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern TileFlags GetEditorPreviewTileFlags_Injected(ref Vector3Int position);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void EditorPreviewFloodFillTileAsset_Injected(ref Vector3Int position, Object tile);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void EditorPreviewBoxFillTileAsset_Injected(ref Vector3Int position, Object tile, int startX, int startY, int endX, int endY);

    public enum Orientation
    {
      XY,
      XZ,
      YX,
      YZ,
      ZX,
      ZY,
      Custom,
    }
  }
}
