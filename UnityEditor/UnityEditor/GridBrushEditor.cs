// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridBrushEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Editor for GridBrush.</para>
  /// </summary>
  [CustomEditor(typeof (GridBrush))]
  public class GridBrushEditor : GridBrushEditorBase
  {
    private GridLayout m_LastGrid = (GridLayout) null;
    private GameObject m_LastBrushTarget = (GameObject) null;
    private BoundsInt? m_LastBounds = new BoundsInt?();
    private GridBrushBase.Tool? m_LastTool = new GridBrushBase.Tool?();
    private int m_LastPreviewRefreshHash;
    private TileBase[] m_SelectionTiles;
    private Color[] m_SelectionColors;
    private Matrix4x4[] m_SelectionMatrices;
    private TileFlags[] m_SelectionFlagsArray;
    private Sprite[] m_SelectionSprites;
    private Tile.ColliderType[] m_SelectionColliderTypes;

    /// <summary>
    ///   <para>The GridBrush that is the target for this editor.</para>
    /// </summary>
    public GridBrush brush
    {
      get
      {
        return this.target as GridBrush;
      }
    }

    protected virtual void OnEnable()
    {
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    protected virtual void OnDisable()
    {
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    private void UndoRedoPerformed()
    {
      this.ClearPreview();
      this.m_LastPreviewRefreshHash = 0;
    }

    public override void OnPaintSceneGUI(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing)
    {
      BoundsInt position1 = position;
      bool flag = false;
      if (Event.current.type == EventType.Layout)
      {
        int hash = GridBrushEditor.GetHash(gridLayout, brushTarget, position, tool, this.brush);
        flag = hash != this.m_LastPreviewRefreshHash;
        if (flag)
          this.m_LastPreviewRefreshHash = hash;
      }
      switch (tool)
      {
        case GridBrushBase.Tool.Move:
          if (flag && executing)
          {
            this.ClearPreview();
            this.PaintPreview(gridLayout, brushTarget, position.min);
            break;
          }
          break;
        case GridBrushBase.Tool.Paint:
        case GridBrushBase.Tool.Erase:
          if (flag)
          {
            this.ClearPreview();
            if (tool != GridBrushBase.Tool.Erase)
              this.PaintPreview(gridLayout, brushTarget, position.min);
          }
          position1 = new BoundsInt(position.min - this.brush.pivot, this.brush.size);
          break;
        case GridBrushBase.Tool.Box:
          if (flag)
          {
            this.ClearPreview();
            this.BoxFillPreview(gridLayout, brushTarget, position);
            break;
          }
          break;
        case GridBrushBase.Tool.FloodFill:
          if (flag)
          {
            this.ClearPreview();
            this.FloodFillPreview(gridLayout, brushTarget, position.min);
          }
          break;
      }
      base.OnPaintSceneGUI(gridLayout, brushTarget, position1, tool, executing);
    }

    /// <summary>
    ///   <para>Callback for drawing the Inspector GUI when there is an active GridSelection made in a Tilemap.</para>
    /// </summary>
    public override void OnSelectionInspectorGUI()
    {
      BoundsInt position1 = GridSelection.position;
      Tilemap component = GridSelection.target.GetComponent<Tilemap>();
      int length = position1.size.x * position1.size.y * position1.size.z;
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || length <= 0)
        return;
      base.OnSelectionInspectorGUI();
      GUILayout.Space(10f);
      if (this.m_SelectionTiles == null || this.m_SelectionTiles.Length != length)
      {
        this.m_SelectionTiles = new TileBase[length];
        this.m_SelectionColors = new Color[length];
        this.m_SelectionMatrices = new Matrix4x4[length];
        this.m_SelectionFlagsArray = new TileFlags[length];
        this.m_SelectionSprites = new Sprite[length];
        this.m_SelectionColliderTypes = new Tile.ColliderType[length];
      }
      int index = 0;
      foreach (Vector3Int position2 in position1.allPositionsWithin)
      {
        this.m_SelectionTiles[index] = component.GetTile(position2);
        this.m_SelectionColors[index] = component.GetColor(position2);
        this.m_SelectionMatrices[index] = component.GetTransformMatrix(position2);
        this.m_SelectionFlagsArray[index] = component.GetTileFlags(position2);
        this.m_SelectionSprites[index] = component.GetSprite(position2);
        this.m_SelectionColliderTypes[index] = component.GetColliderType(position2);
        ++index;
      }
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = ((IEnumerable<TileBase>) this.m_SelectionTiles).Any<TileBase>((Func<TileBase, bool>) (tile => (UnityEngine.Object) tile != (UnityEngine.Object) ((IEnumerable<TileBase>) this.m_SelectionTiles).First<TileBase>()));
      Vector3Int position3 = new Vector3Int(position1.xMin, position1.yMin, position1.zMin);
      TileBase tile1 = EditorGUILayout.ObjectField(GridBrushEditor.Styles.tileLabel, (UnityEngine.Object) component.GetTile(position3), typeof (TileBase), false, new GUILayoutOption[0]) as TileBase;
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject((UnityEngine.Object) component, "Edit Tilemap");
        foreach (Vector3Int position2 in position1.allPositionsWithin)
          component.SetTile(position2, tile1);
      }
      bool flag1 = ((IEnumerable<TileFlags>) this.m_SelectionFlagsArray).All<TileFlags>((Func<TileFlags, bool>) (flags => (flags & TileFlags.LockColor) == (((IEnumerable<TileFlags>) this.m_SelectionFlagsArray).First<TileFlags>() & TileFlags.LockColor)));
      using (new EditorGUI.DisabledScope(!flag1 || (this.m_SelectionFlagsArray[0] & TileFlags.LockColor) != TileFlags.None))
      {
        EditorGUI.showMixedValue = ((IEnumerable<Color>) this.m_SelectionColors).Any<Color>((Func<Color, bool>) (color => color != ((IEnumerable<Color>) this.m_SelectionColors).First<Color>()));
        EditorGUI.BeginChangeCheck();
        Color color1 = EditorGUILayout.ColorField(GridBrushEditor.Styles.colorLabel, this.m_SelectionColors[0], new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RecordObject((UnityEngine.Object) component, "Edit Tilemap");
          foreach (Vector3Int position2 in position1.allPositionsWithin)
            component.SetColor(position2, color1);
        }
      }
      bool flag2 = ((IEnumerable<TileFlags>) this.m_SelectionFlagsArray).All<TileFlags>((Func<TileFlags, bool>) (flags => (flags & TileFlags.LockTransform) == (((IEnumerable<TileFlags>) this.m_SelectionFlagsArray).First<TileFlags>() & TileFlags.LockTransform)));
      using (new EditorGUI.DisabledScope(!flag2 || (this.m_SelectionFlagsArray[0] & TileFlags.LockTransform) != TileFlags.None))
      {
        EditorGUI.showMixedValue = ((IEnumerable<Matrix4x4>) this.m_SelectionMatrices).Any<Matrix4x4>((Func<Matrix4x4, bool>) (matrix => matrix != ((IEnumerable<Matrix4x4>) this.m_SelectionMatrices).First<Matrix4x4>()));
        EditorGUI.BeginChangeCheck();
        Matrix4x4 transform = TileEditor.TransformMatrixOnGUI(this.m_SelectionMatrices[0]);
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RecordObject((UnityEngine.Object) component, "Edit Tilemap");
          foreach (Vector3Int position2 in position1.allPositionsWithin)
            component.SetTransformMatrix(position2, transform);
        }
      }
      using (new EditorGUI.DisabledScope(true))
      {
        EditorGUI.showMixedValue = ((IEnumerable<Sprite>) this.m_SelectionSprites).Any<Sprite>((Func<Sprite, bool>) (sprite => (UnityEngine.Object) sprite != (UnityEngine.Object) ((IEnumerable<Sprite>) this.m_SelectionSprites).First<Sprite>()));
        EditorGUILayout.ObjectField(GridBrushEditor.Styles.spriteLabel, (UnityEngine.Object) this.m_SelectionSprites[0], typeof (Sprite), false, new GUILayoutOption[1]
        {
          GUILayout.Height(16f)
        });
        EditorGUI.showMixedValue = ((IEnumerable<Tile.ColliderType>) this.m_SelectionColliderTypes).Any<Tile.ColliderType>((Func<Tile.ColliderType, bool>) (colliderType => colliderType != ((IEnumerable<Tile.ColliderType>) this.m_SelectionColliderTypes).First<Tile.ColliderType>()));
        EditorGUILayout.EnumPopup(GridBrushEditor.Styles.colliderTypeLabel, (Enum) this.m_SelectionColliderTypes[0], new GUILayoutOption[0]);
        EditorGUI.showMixedValue = !flag1;
        EditorGUILayout.Toggle(GridBrushEditor.Styles.lockColorLabel, (this.m_SelectionFlagsArray[0] & TileFlags.LockColor) != TileFlags.None, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = !flag2;
        EditorGUILayout.Toggle(GridBrushEditor.Styles.lockTransformLabel, (this.m_SelectionFlagsArray[0] & TileFlags.LockTransform) != TileFlags.None, new GUILayoutOption[0]);
      }
      EditorGUI.showMixedValue = false;
    }

    public override void OnMouseLeave()
    {
      this.ClearPreview();
    }

    public override void OnToolDeactivated(GridBrushBase.Tool tool)
    {
      this.ClearPreview();
    }

    public override void RegisterUndo(GameObject brushTarget, GridBrushBase.Tool tool)
    {
      if (!((UnityEngine.Object) brushTarget != (UnityEngine.Object) null))
        return;
      Undo.RegisterFullObjectHierarchyUndo((UnityEngine.Object) brushTarget, tool.ToString());
    }

    /// <summary>
    ///   <para>Returns all valid targets that the brush can edit.</para>
    /// </summary>
    public override GameObject[] validTargets
    {
      get
      {
        return ((IEnumerable<Tilemap>) UnityEngine.Object.FindObjectsOfType<Tilemap>()).Select<Tilemap, GameObject>((Func<Tilemap, GameObject>) (x => x.gameObject)).ToArray<GameObject>();
      }
    }

    /// <summary>
    ///   <para>Paints preview data into a cell of a grid given the coordinates of the cell.</para>
    /// </summary>
    /// <param name="gridLayout"> to paint data to.</param>
    /// <param name="brushTarget">Target of the paint operation. By default the currently selected GameObject.</param>
    /// <param name="position">The coordinates of the cell to paint data to.</param>
    public virtual void PaintPreview(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
      Vector3Int position1 = position - this.brush.pivot;
      Vector3Int vector3Int = position1 + this.brush.size;
      BoundsInt boundsInt = new BoundsInt(position1, vector3Int - position1);
      if ((UnityEngine.Object) brushTarget != (UnityEngine.Object) null)
      {
        Tilemap component = brushTarget.GetComponent<Tilemap>();
        foreach (Vector3Int location in boundsInt.allPositionsWithin)
        {
          GridBrush.BrushCell cell = this.brush.cells[this.brush.GetCellIndex(location - position1)];
          if ((UnityEngine.Object) cell.tile != (UnityEngine.Object) null && (UnityEngine.Object) component != (UnityEngine.Object) null)
            GridBrushEditor.SetTilemapPreviewCell(component, location, cell.tile, cell.matrix, cell.color);
        }
      }
      this.m_LastGrid = gridLayout;
      this.m_LastBounds = new BoundsInt?(boundsInt);
      this.m_LastBrushTarget = brushTarget;
      this.m_LastTool = new GridBrushBase.Tool?(GridBrushBase.Tool.Paint);
    }

    /// <summary>
    ///   <para>Does a preview of what happens when a GridBrush.BoxFill is done with the same parameters.</para>
    /// </summary>
    /// <param name="gridLayout"> to box fill data to.</param>
    /// <param name="brushTarget">Target of box fill operation. By default the currently selected GameObject.</param>
    /// <param name="position">The bounds to box fill data to.</param>
    public virtual void BoxFillPreview(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
      if ((UnityEngine.Object) brushTarget != (UnityEngine.Object) null)
      {
        Tilemap component = brushTarget.GetComponent<Tilemap>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          foreach (Vector3Int location in position.allPositionsWithin)
          {
            Vector3Int vector3Int = location - position.min;
            GridBrush.BrushCell cell = this.brush.cells[this.brush.GetCellIndexWrapAround(vector3Int.x, vector3Int.y, vector3Int.z)];
            if ((UnityEngine.Object) cell.tile != (UnityEngine.Object) null)
              GridBrushEditor.SetTilemapPreviewCell(component, location, cell.tile, cell.matrix, cell.color);
          }
        }
      }
      this.m_LastGrid = gridLayout;
      this.m_LastBounds = new BoundsInt?(position);
      this.m_LastBrushTarget = brushTarget;
      this.m_LastTool = new GridBrushBase.Tool?(GridBrushBase.Tool.Box);
    }

    /// <summary>
    ///   <para>Does a preview of what happens when a GridBrush.FloodFill is done with the same parameters.</para>
    /// </summary>
    /// <param name="gridLayout"> to paint data to.</param>
    /// <param name="brushTarget">Target of the flood fill operation. By default the currently selected GameObject.</param>
    /// <param name="position">The coordinates of the cell to flood fill data to.</param>
    public virtual void FloodFillPreview(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
      if (!EditorPrefs.GetBool(GridBrushEditor.Styles.floodFillPreviewEditorPref, true))
        return;
      BoundsInt boundsInt = new BoundsInt(position, Vector3Int.one);
      if ((UnityEngine.Object) brushTarget != (UnityEngine.Object) null && this.brush.cellCount > 0)
      {
        Tilemap component = brushTarget.GetComponent<Tilemap>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          GridBrush.BrushCell cell = this.brush.cells[0];
          component.EditorPreviewFloodFill(position, cell.tile);
          boundsInt.min = component.origin;
          boundsInt.max = component.origin + component.size;
        }
      }
      this.m_LastGrid = gridLayout;
      this.m_LastBounds = new BoundsInt?(boundsInt);
      this.m_LastBrushTarget = brushTarget;
      this.m_LastTool = new GridBrushBase.Tool?(GridBrushBase.Tool.FloodFill);
    }

    [PreferenceItem("2D")]
    private static void PreferencesGUI()
    {
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUILayout.Toggle(GridBrushEditor.Styles.floodFillPreviewLabel, EditorPrefs.GetBool(GridBrushEditor.Styles.floodFillPreviewEditorPref, true), new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      EditorPrefs.SetBool(GridBrushEditor.Styles.floodFillPreviewEditorPref, flag);
    }

    /// <summary>
    ///   <para>Clears any preview drawn previously by the GridBrushEditor.</para>
    /// </summary>
    public virtual void ClearPreview()
    {
      if ((UnityEngine.Object) this.m_LastGrid == (UnityEngine.Object) null || (!this.m_LastBounds.HasValue || (UnityEngine.Object) this.m_LastBrushTarget == (UnityEngine.Object) null) || !this.m_LastTool.HasValue)
        return;
      Tilemap component = this.m_LastBrushTarget.GetComponent<Tilemap>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        GridBrushBase.Tool? lastTool = this.m_LastTool;
        if (lastTool.HasValue)
        {
          switch (lastTool.Value)
          {
            case GridBrushBase.Tool.Paint:
              using (BoundsInt.PositionEnumerator enumerator = this.m_LastBounds.Value.allPositionsWithin.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  Vector3Int current = enumerator.Current;
                  GridBrushEditor.ClearTilemapPreview(component, current);
                }
                break;
              }
            case GridBrushBase.Tool.Box:
              Vector3Int position = this.m_LastBounds.Value.position;
              Vector3Int vector3Int = position + this.m_LastBounds.Value.size;
              using (BoundsInt.PositionEnumerator enumerator = new BoundsInt(position, vector3Int - position).allPositionsWithin.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  Vector3Int current = enumerator.Current;
                  GridBrushEditor.ClearTilemapPreview(component, current);
                }
                break;
              }
            case GridBrushBase.Tool.FloodFill:
              component.ClearAllEditorPreviewTiles();
              break;
          }
        }
      }
      this.m_LastBrushTarget = (GameObject) null;
      this.m_LastGrid = (GridLayout) null;
      this.m_LastBounds = new BoundsInt?();
      this.m_LastTool = new GridBrushBase.Tool?();
    }

    private static void SetTilemapPreviewCell(Tilemap map, Vector3Int location, TileBase tile, Matrix4x4 transformMatrix, Color color)
    {
      if ((UnityEngine.Object) map == (UnityEngine.Object) null)
        return;
      map.SetEditorPreviewTile(location, tile);
      map.SetEditorPreviewTransformMatrix(location, transformMatrix);
      map.SetEditorPreviewColor(location, color);
    }

    private static void ClearTilemapPreview(Tilemap map, Vector3Int location)
    {
      if ((UnityEngine.Object) map == (UnityEngine.Object) null)
        return;
      map.SetEditorPreviewTile(location, (TileBase) null);
      map.SetEditorPreviewTransformMatrix(location, Matrix4x4.identity);
      map.SetEditorPreviewColor(location, Color.white);
    }

    private static int GetHash(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, GridBrush brush)
    {
      return ((((0 * 33 + (!((UnityEngine.Object) gridLayout != (UnityEngine.Object) null) ? 0 : gridLayout.GetHashCode())) * 33 + (!((UnityEngine.Object) brushTarget != (UnityEngine.Object) null) ? 0 : brushTarget.GetHashCode())) * 33 + position.GetHashCode()) * 33 + tool.GetHashCode()) * 33 + (!((UnityEngine.Object) brush != (UnityEngine.Object) null) ? 0 : brush.GetHashCode());
    }

    private static class Styles
    {
      public static readonly GUIContent multieditingNotSupported = EditorGUIUtility.TextContent("Multi-editing cells not supported");
      public static readonly GUIContent tileLabel = EditorGUIUtility.TextContent("Tile|Tile set in tilemap");
      public static readonly GUIContent spriteLabel = EditorGUIUtility.TextContent("Sprite|Sprite set when tile is set in tilemap");
      public static readonly GUIContent colorLabel = EditorGUIUtility.TextContent("Color|Color set when tile is set in tilemap");
      public static readonly GUIContent gameObjectLabel = EditorGUIUtility.TextContent("GameObject|Game Object instantiated when tile is set in tilemap");
      public static readonly GUIContent colliderTypeLabel = EditorGUIUtility.TextContent("Collider Type|Collider shape used for tile");
      public static readonly GUIContent gridPositionLabel = EditorGUIUtility.TextContent("Grid Position|Position of the tile in the tilemap");
      public static readonly GUIContent lockColorLabel = EditorGUIUtility.TextContent("Lock Color|Prevents tilemap from changing color of tile");
      public static readonly GUIContent lockTransformLabel = EditorGUIUtility.TextContent("Lock Transform|Prevents tilemap from changing transform of tile");
      public static readonly GUIContent instantiateGameObjectRuntimeOnlyLabel = EditorGUIUtility.TextContent("Instantiate GameObject Runtime Only|Instantiates GameObject in runtime play mode only");
      public static readonly GUIContent floodFillPreviewLabel = EditorGUIUtility.TextContent("Show Flood Fill Preview|Whether a preview is shown while painting a Tilemap when Flood Fill mode is enabled");
      public static readonly string floodFillPreviewEditorPref = "GridBrush.EnableFloodFillPreview";
    }
  }
}
