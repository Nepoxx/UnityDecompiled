// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridBrush
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Default built-in brush for painting or erasing tiles and/or gamobjects on a grid.</para>
  /// </summary>
  public class GridBrush : GridBrushBase
  {
    [HideInInspector]
    [SerializeField]
    private GridBrush.BrushCell[] m_Cells;
    [HideInInspector]
    [SerializeField]
    private Vector3Int m_Size;
    [HideInInspector]
    [SerializeField]
    private Vector3Int m_Pivot;

    public GridBrush()
    {
      this.Init(Vector3Int.one, Vector3Int.zero);
      this.SizeUpdated();
    }

    /// <summary>
    ///   <para>Size of the brush in cells.</para>
    /// </summary>
    public Vector3Int size
    {
      get
      {
        return this.m_Size;
      }
      set
      {
        this.m_Size = value;
        this.SizeUpdated();
      }
    }

    /// <summary>
    ///   <para>Pivot of the brush.</para>
    /// </summary>
    public Vector3Int pivot
    {
      get
      {
        return this.m_Pivot;
      }
      set
      {
        this.m_Pivot = value;
      }
    }

    /// <summary>
    ///   <para>All the brush cells the brush holds.</para>
    /// </summary>
    public GridBrush.BrushCell[] cells
    {
      get
      {
        return this.m_Cells;
      }
    }

    /// <summary>
    ///   <para>Number of brush cells in the brush.</para>
    /// </summary>
    public int cellCount
    {
      get
      {
        return this.m_Cells == null ? 0 : this.m_Cells.Length;
      }
    }

    /// <summary>
    ///   <para>Initializes the content of the GridBrush.</para>
    /// </summary>
    /// <param name="size">Size of the GridBrush.</param>
    /// <param name="pivot">Pivot point of the GridBrush.</param>
    public void Init(Vector3Int size)
    {
      this.Init(size, Vector3Int.zero);
      this.SizeUpdated();
    }

    /// <summary>
    ///   <para>Initializes the content of the GridBrush.</para>
    /// </summary>
    /// <param name="size">Size of the GridBrush.</param>
    /// <param name="pivot">Pivot point of the GridBrush.</param>
    public void Init(Vector3Int size, Vector3Int pivot)
    {
      this.m_Size = size;
      this.m_Pivot = pivot;
      this.SizeUpdated();
    }

    /// <summary>
    ///   <para>Paints tiles and GameObjects into a given position within the selected layers.</para>
    /// </summary>
    /// <param name="gridLayout"> used for layout.</param>
    /// <param name="brushTarget">Target of the paint operation. By default the currently selected GameObject.</param>
    /// <param name="position">The coordinates of the cell to paint data to.</param>
    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
      BoundsInt position1 = new BoundsInt(position - this.pivot, this.m_Size);
      this.BoxFill(gridLayout, brushTarget, position1);
    }

    private void PaintCell(Vector3Int position, Tilemap tilemap, GridBrush.BrushCell cell)
    {
      if (!((UnityEngine.Object) cell.tile != (UnityEngine.Object) null))
        return;
      GridBrush.SetTilemapCell(tilemap, position, cell.tile, cell.matrix, cell.color);
    }

    /// <summary>
    ///   <para>Erases tiles and GameObjects in a given position within the selected layers.</para>
    /// </summary>
    /// <param name="gridLayout"> used for layout.</param>
    /// <param name="brushTarget">Target of the erase operation. By default the currently selected GameObject.</param>
    /// <param name="position">The coordinates of the cell to erase data from.</param>
    public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
      BoundsInt position1 = new BoundsInt(position - this.pivot, this.m_Size);
      this.BoxErase(gridLayout, brushTarget, position1);
    }

    private void EraseCell(Vector3Int position, Tilemap tilemap)
    {
      GridBrush.ClearTilemapCell(tilemap, position);
    }

    /// <summary>
    ///   <para>Box fills tiles and GameObjects into given bounds within the selected layers.</para>
    /// </summary>
    /// <param name="gridLayout"> to box fill data to.</param>
    /// <param name="brushTarget">Target of the box fill operation. By default the currently selected GameObject.</param>
    /// <param name="position">The bounds to box fill data into.</param>
    public override void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
      if ((UnityEngine.Object) brushTarget == (UnityEngine.Object) null)
        return;
      Tilemap component = brushTarget.GetComponent<Tilemap>();
      foreach (Vector3Int position1 in position.allPositionsWithin)
      {
        Vector3Int vector3Int = position1 - position.min;
        GridBrush.BrushCell cell = this.m_Cells[this.GetCellIndexWrapAround(vector3Int.x, vector3Int.y, vector3Int.z)];
        this.PaintCell(position1, component, cell);
      }
    }

    /// <summary>
    ///   <para>Erases tiles and GameObjects from given bounds within the selected layers.</para>
    /// </summary>
    /// <param name="gridLayout"> to erase data from.</param>
    /// <param name="brushTarget">Target of the erase operation. By default the currently selected GameObject.</param>
    /// <param name="position">The bounds to erase data from.</param>
    public override void BoxErase(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
      if ((UnityEngine.Object) brushTarget == (UnityEngine.Object) null)
        return;
      Tilemap component = brushTarget.GetComponent<Tilemap>();
      foreach (Vector3Int position1 in position.allPositionsWithin)
        this.EraseCell(position1, component);
    }

    /// <summary>
    ///   <para>Flood fills tiles and GameObjects starting from a given position within the selected layers.</para>
    /// </summary>
    /// <param name="gridLayout">GridLayout used for layout.</param>
    /// <param name="brushTarget">Target of the flood fill operation. By default the currently selected GameObject.</param>
    /// <param name="position">Starting position of the flood fill.</param>
    public override void FloodFill(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
      if (this.cellCount == 0 || (UnityEngine.Object) brushTarget == (UnityEngine.Object) null)
        return;
      Tilemap component = brushTarget.GetComponent<Tilemap>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.FloodFill(position, this.cells[0].tile);
    }

    public override void Rotate(GridBrushBase.RotationDirection direction, GridLayout.CellLayout layout)
    {
      Vector3Int size = this.m_Size;
      GridBrush.BrushCell[] brushCellArray = this.m_Cells.Clone() as GridBrush.BrushCell[];
      this.size = new Vector3Int(size.y, size.x, size.z);
      foreach (Vector3Int vector3Int in new BoundsInt(Vector3Int.zero, size).allPositionsWithin)
      {
        int cellIndex1 = this.GetCellIndex(direction != GridBrushBase.RotationDirection.Clockwise ? vector3Int.y : size.y - vector3Int.y - 1, direction != GridBrushBase.RotationDirection.Clockwise ? size.x - vector3Int.x - 1 : vector3Int.x, vector3Int.z);
        int cellIndex2 = this.GetCellIndex(vector3Int.x, vector3Int.y, vector3Int.z, size.x, size.y, size.z);
        this.m_Cells[cellIndex1] = brushCellArray[cellIndex2];
      }
      this.pivot = new Vector3Int(direction != GridBrushBase.RotationDirection.Clockwise ? this.pivot.y : size.y - this.pivot.y - 1, direction != GridBrushBase.RotationDirection.Clockwise ? size.x - this.pivot.x - 1 : this.pivot.x, this.pivot.z);
      Matrix4x4 matrix4x4 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, direction != GridBrushBase.RotationDirection.Clockwise ? -90f : 90f), Vector3.one);
      foreach (GridBrush.BrushCell cell in this.m_Cells)
      {
        Matrix4x4 matrix = cell.matrix;
        cell.matrix = matrix * matrix4x4;
      }
    }

    public override void Flip(GridBrushBase.FlipAxis flip, GridLayout.CellLayout layout)
    {
      if (flip == GridBrushBase.FlipAxis.X)
        this.FlipX();
      else
        this.FlipY();
    }

    /// <summary>
    ///   <para>Picks tiles from selected and child GameObjects, given the coordinates of the cells.</para>
    /// </summary>
    /// <param name="gridLayout"> to pick data from.</param>
    /// <param name="brushTarget">Target of the picking operation. By default the currently selected GameObject.</param>
    /// <param name="position">The coordinates of the cells to paint data from.</param>
    /// <param name="pickStart">Pivot of the picking brush.</param>
    public override void Pick(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pickStart)
    {
      this.Reset();
      this.UpdateSizeAndPivot(new Vector3Int(position.size.x, position.size.y, 1), new Vector3Int(pickStart.x, pickStart.y, 0));
      Tilemap component = brushTarget.GetComponent<Tilemap>();
      foreach (Vector3Int position1 in position.allPositionsWithin)
      {
        Vector3Int brushPosition = new Vector3Int(position1.x - position.x, position1.y - position.y, 0);
        this.PickCell(position1, brushPosition, component);
      }
    }

    private void PickCell(Vector3Int position, Vector3Int brushPosition, Tilemap tilemap)
    {
      if (!((UnityEngine.Object) tilemap != (UnityEngine.Object) null))
        return;
      this.SetTile(brushPosition, tilemap.GetTile(position));
      this.SetMatrix(brushPosition, tilemap.GetTransformMatrix(position));
      this.SetColor(brushPosition, tilemap.GetColor(position));
    }

    /// <summary>
    ///   <para>MoveEnd is called when user starts moving the area previously selected with the selection marquee.</para>
    /// </summary>
    /// <param name="gridLayout"> used for layout.</param>
    /// <param name="brushTarget">Target of the move operation. By default the currently selected GameObject.</param>
    /// <param name="position">Position where the move operation has started.</param>
    public override void MoveStart(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
      this.Reset();
      this.UpdateSizeAndPivot(new Vector3Int(position.size.x, position.size.y, 1), Vector3Int.zero);
      Tilemap component = brushTarget.GetComponent<Tilemap>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      foreach (Vector3Int position1 in position.allPositionsWithin)
      {
        Vector3Int brushPosition = new Vector3Int(position1.x - position.x, position1.y - position.y, 0);
        this.PickCell(position1, brushPosition, component);
        component.SetTile(position1, (TileBase) null);
      }
    }

    /// <summary>
    ///   <para>MoveEnd is called when user has ended the move of the area previously selected with the selection marquee.</para>
    /// </summary>
    /// <param name="gridLayout"> used for layout.</param>
    /// <param name="brushTarget">Target of the move operation. By default the currently selected GameObject.</param>
    /// <param name="position">Position where the move operation has ended.</param>
    public override void MoveEnd(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
      this.Paint(gridLayout, brushTarget, position.min);
      this.Reset();
    }

    /// <summary>
    ///   <para>Clear all data of the brush.</para>
    /// </summary>
    public void Reset()
    {
      this.UpdateSizeAndPivot(Vector3Int.one, Vector3Int.zero);
    }

    private void FlipX()
    {
      GridBrush.BrushCell[] brushCellArray = this.m_Cells.Clone() as GridBrush.BrushCell[];
      foreach (Vector3Int brushPosition in new BoundsInt(Vector3Int.zero, this.m_Size).allPositionsWithin)
      {
        int cellIndex1 = this.GetCellIndex(this.m_Size.x - brushPosition.x - 1, brushPosition.y, brushPosition.z);
        int cellIndex2 = this.GetCellIndex(brushPosition);
        this.m_Cells[cellIndex1] = brushCellArray[cellIndex2];
      }
      this.pivot = new Vector3Int(this.m_Size.x - this.pivot.x - 1, this.pivot.y, this.pivot.z);
      Matrix4x4 matrix4x4 = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, 1f, 1f));
      foreach (GridBrush.BrushCell cell in this.m_Cells)
      {
        Matrix4x4 matrix = cell.matrix;
        cell.matrix = matrix * matrix4x4;
      }
    }

    private void FlipY()
    {
      GridBrush.BrushCell[] brushCellArray = this.m_Cells.Clone() as GridBrush.BrushCell[];
      foreach (Vector3Int brushPosition in new BoundsInt(Vector3Int.zero, this.m_Size).allPositionsWithin)
      {
        int y = this.m_Size.y - brushPosition.y - 1;
        int cellIndex1 = this.GetCellIndex(brushPosition.x, y, brushPosition.z);
        int cellIndex2 = this.GetCellIndex(brushPosition);
        this.m_Cells[cellIndex1] = brushCellArray[cellIndex2];
      }
      this.pivot = new Vector3Int(this.pivot.x, this.m_Size.y - this.pivot.y - 1, this.pivot.z);
      Matrix4x4 matrix4x4 = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, -1f, 1f));
      foreach (GridBrush.BrushCell cell in this.m_Cells)
      {
        Matrix4x4 matrix = cell.matrix;
        cell.matrix = matrix * matrix4x4;
      }
    }

    /// <summary>
    ///   <para>Updates the size, pivot and the number of layers of the brush.</para>
    /// </summary>
    /// <param name="size">New size of the brush.</param>
    /// <param name="pivot">New pivot of the brush.</param>
    public void UpdateSizeAndPivot(Vector3Int size, Vector3Int pivot)
    {
      this.m_Size = size;
      this.m_Pivot = pivot;
      this.SizeUpdated();
    }

    /// <summary>
    ///   <para>Sets a Tile at the position in the brush.</para>
    /// </summary>
    /// <param name="position">Position to set the tile in the brush.</param>
    /// <param name="tile">Tile to set in the brush.</param>
    public void SetTile(Vector3Int position, TileBase tile)
    {
      if (!this.ValidateCellPosition(position))
        return;
      this.m_Cells[this.GetCellIndex(position)].tile = tile;
    }

    /// <summary>
    ///   <para>Sets a transform matrix at the position in the brush. This matrix is used specifically for tiles on a Tilemap and not GameObjects of the brush cell.</para>
    /// </summary>
    /// <param name="position">Position to set the transform matrix in the brush.</param>
    /// <param name="matrix">Transform matrix to set in the brush.</param>
    public void SetMatrix(Vector3Int position, Matrix4x4 matrix)
    {
      if (!this.ValidateCellPosition(position))
        return;
      this.m_Cells[this.GetCellIndex(position)].matrix = matrix;
    }

    /// <summary>
    ///   <para>Sets a tint color at the position in the brush.</para>
    /// </summary>
    /// <param name="position">Position to set the color in the brush.</param>
    /// <param name="color">Tint color to set in the brush.</param>
    public void SetColor(Vector3Int position, Color color)
    {
      if (!this.ValidateCellPosition(position))
        return;
      this.m_Cells[this.GetCellIndex(position)].color = color;
    }

    /// <summary>
    ///   <para>Gets the index to the GridBrush.BrushCell based on the position of the BrushCell.</para>
    /// </summary>
    /// <param name="brushPosition">Position of the BrushCell.</param>
    /// <param name="x">X Position of the BrushCell.</param>
    /// <param name="y">Y Position of the BrushCell.</param>
    /// <param name="z">Z Position of the BrushCell.</param>
    /// <param name="sizex">X Size of Brush.</param>
    /// <param name="sizey">Y Size of Brush.</param>
    /// <param name="sizez">Z Size of Brush.</param>
    /// <returns>
    ///   <para>Index to the BrushCell.</para>
    /// </returns>
    public int GetCellIndex(Vector3Int brushPosition)
    {
      return this.GetCellIndex(brushPosition.x, brushPosition.y, brushPosition.z);
    }

    /// <summary>
    ///   <para>Gets the index to the GridBrush.BrushCell based on the position of the BrushCell.</para>
    /// </summary>
    /// <param name="brushPosition">Position of the BrushCell.</param>
    /// <param name="x">X Position of the BrushCell.</param>
    /// <param name="y">Y Position of the BrushCell.</param>
    /// <param name="z">Z Position of the BrushCell.</param>
    /// <param name="sizex">X Size of Brush.</param>
    /// <param name="sizey">Y Size of Brush.</param>
    /// <param name="sizez">Z Size of Brush.</param>
    /// <returns>
    ///   <para>Index to the BrushCell.</para>
    /// </returns>
    public int GetCellIndex(int x, int y, int z)
    {
      return x + this.m_Size.x * y + this.m_Size.x * this.m_Size.y * z;
    }

    /// <summary>
    ///   <para>Gets the index to the GridBrush.BrushCell based on the position of the BrushCell.</para>
    /// </summary>
    /// <param name="brushPosition">Position of the BrushCell.</param>
    /// <param name="x">X Position of the BrushCell.</param>
    /// <param name="y">Y Position of the BrushCell.</param>
    /// <param name="z">Z Position of the BrushCell.</param>
    /// <param name="sizex">X Size of Brush.</param>
    /// <param name="sizey">Y Size of Brush.</param>
    /// <param name="sizez">Z Size of Brush.</param>
    /// <returns>
    ///   <para>Index to the BrushCell.</para>
    /// </returns>
    public int GetCellIndex(int x, int y, int z, int sizex, int sizey, int sizez)
    {
      return x + sizex * y + sizex * sizey * z;
    }

    /// <summary>
    ///   <para>Gets the index to the GridBrush.BrushCell based on the position of the BrushCell. Wraps each coordinate if it is larger than the size of the GridBrush.</para>
    /// </summary>
    /// <param name="x">X Position of the BrushCell.</param>
    /// <param name="y">Y Position of the BrushCell.</param>
    /// <param name="z">Z Position of the BrushCell.</param>
    /// <returns>
    ///   <para>Index to the BrushCell.</para>
    /// </returns>
    public int GetCellIndexWrapAround(int x, int y, int z)
    {
      return x % this.m_Size.x + this.m_Size.x * (y % this.m_Size.y) + this.m_Size.x * this.m_Size.y * (z % this.m_Size.z);
    }

    private bool ValidateCellPosition(Vector3Int position)
    {
      bool flag = position.x >= 0 && (position.x < this.size.x && position.y >= 0) && (position.y < this.size.y && position.z >= 0) && position.z < this.size.z;
      if (!flag)
        throw new ArgumentException(string.Format("Position {0} is an invalid cell position. Valid range is between [{1}, {2}).", (object) position, (object) Vector3Int.zero, (object) this.size));
      return flag;
    }

    private void SizeUpdated()
    {
      this.m_Cells = new GridBrush.BrushCell[this.m_Size.x * this.m_Size.y * this.m_Size.z];
      foreach (Vector3Int brushPosition in new BoundsInt(Vector3Int.zero, this.m_Size).allPositionsWithin)
        this.m_Cells[this.GetCellIndex(brushPosition)] = new GridBrush.BrushCell();
    }

    private static void SetTilemapCell(Tilemap map, Vector3Int location, TileBase tile, Matrix4x4 transformMatrix, Color color)
    {
      if ((UnityEngine.Object) map == (UnityEngine.Object) null)
        return;
      map.SetTile(location, tile);
      map.SetTransformMatrix(location, transformMatrix);
      map.SetColor(location, color);
    }

    private static void ClearTilemapCell(Tilemap map, Vector3Int location)
    {
      if ((UnityEngine.Object) map == (UnityEngine.Object) null)
        return;
      map.SetTile(location, (TileBase) null);
      map.SetTransformMatrix(location, Matrix4x4.identity);
      map.SetColor(location, Color.white);
    }

    public override int GetHashCode()
    {
      int num = 0;
      foreach (GridBrush.BrushCell cell in this.cells)
        num = num * 33 + cell.GetHashCode();
      return num;
    }

    /// <summary>
    ///   <para>Brush Cell stores the data to be painted in a grid cell.</para>
    /// </summary>
    [Serializable]
    public class BrushCell
    {
      [SerializeField]
      private Matrix4x4 m_Matrix = Matrix4x4.identity;
      [SerializeField]
      private Color m_Color = Color.white;
      [SerializeField]
      private TileBase m_Tile;

      /// <summary>
      ///   <para>Tile to be placed when painting.</para>
      /// </summary>
      public TileBase tile
      {
        get
        {
          return this.m_Tile;
        }
        set
        {
          this.m_Tile = value;
        }
      }

      /// <summary>
      ///   <para>The transform matrix of the brush cell.</para>
      /// </summary>
      public Matrix4x4 matrix
      {
        get
        {
          return this.m_Matrix;
        }
        set
        {
          this.m_Matrix = value;
        }
      }

      /// <summary>
      ///   <para>Color to tint the tile when painting.</para>
      /// </summary>
      public Color color
      {
        get
        {
          return this.m_Color;
        }
        set
        {
          this.m_Color = value;
        }
      }

      public override int GetHashCode()
      {
        return ((!((UnityEngine.Object) this.tile != (UnityEngine.Object) null) ? 0 : this.tile.GetInstanceID()) * 33 + this.matrix.GetHashCode()) * 33 + this.color.GetHashCode();
      }
    }
  }
}
