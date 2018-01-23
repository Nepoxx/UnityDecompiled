// Decompiled with JetBrains decompiler
// Type: UnityEditor.PaintableSceneViewGrid
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
  internal class PaintableSceneViewGrid : PaintableGrid
  {
    private SceneView activeSceneView = (SceneView) null;

    private Transform gridTransform
    {
      get
      {
        return !((UnityEngine.Object) this.grid != (UnityEngine.Object) null) ? (Transform) null : this.grid.transform;
      }
    }

    private Grid grid
    {
      get
      {
        return !((UnityEngine.Object) this.brushTarget != (UnityEngine.Object) null) ? (!((UnityEngine.Object) Selection.activeGameObject != (UnityEngine.Object) null) ? (Grid) null : Selection.activeGameObject.GetComponentInParent<Grid>()) : this.brushTarget.GetComponentInParent<Grid>();
      }
    }

    private GridBrushBase gridBrush
    {
      get
      {
        return GridPaintingState.gridBrush;
      }
    }

    private GameObject brushTarget
    {
      get
      {
        return GridPaintingState.scenePaintTarget;
      }
    }

    public Tilemap tilemap
    {
      get
      {
        if ((UnityEngine.Object) this.brushTarget != (UnityEngine.Object) null)
          return this.brushTarget.GetComponent<Tilemap>();
        return (Tilemap) null;
      }
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      SceneView.onSceneGUIDelegate += new SceneView.OnSceneFunc(this.OnSceneGUI);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      GridSelection.gridSelectionChanged += new Action(this.OnGridSelectionChanged);
    }

    protected override void OnDisable()
    {
      SceneView.onSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnSceneGUI);
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      GridSelection.gridSelectionChanged -= new Action(this.OnGridSelectionChanged);
      base.OnDisable();
    }

    private void OnGridSelectionChanged()
    {
      SceneView.RepaintAll();
    }

    public void OnSceneGUI(SceneView sceneView)
    {
      this.UpdateMouseGridPosition();
      this.OnGUI();
      if (PaintableGrid.InGridEditMode())
      {
        this.CallOnSceneGUI();
        if ((UnityEngine.Object) this.grid != (UnityEngine.Object) null && ((UnityEngine.Object) GridPaintingState.activeGrid == (UnityEngine.Object) this || GridSelection.active))
          this.CallOnPaintSceneGUI();
        if (Event.current.type == EventType.Repaint)
          EditorGUIUtility.AddCursorRect(new Rect(0.0f, 17f, sceneView.position.width, sceneView.position.height - 17f), MouseCursor.CustomCursor);
      }
      this.HandleMouseEnterLeave(sceneView);
    }

    private void HandleMouseEnterLeave(SceneView sceneView)
    {
      if (!this.inEditMode)
        return;
      if (Event.current.type == EventType.MouseEnterWindow)
        this.OnMouseEnter(sceneView);
      else if (Event.current.type == EventType.MouseLeaveWindow)
        this.OnMouseLeave(sceneView);
      else if (sceneView.docked && (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.LinuxEditor))
      {
        Vector2 screenPoint = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        if (sceneView.position.Contains(screenPoint))
        {
          if ((UnityEngine.Object) GridPaintingState.activeGrid != (UnityEngine.Object) this)
            this.OnMouseEnter(sceneView);
        }
        else if ((UnityEngine.Object) this.activeSceneView == (UnityEngine.Object) sceneView && (UnityEngine.Object) GridPaintingState.activeGrid == (UnityEngine.Object) this)
          this.OnMouseLeave(sceneView);
      }
    }

    private void OnMouseEnter(SceneView sceneView)
    {
      if ((UnityEngine.Object) GridPaintingState.activeBrushEditor != (UnityEngine.Object) null)
        GridPaintingState.activeBrushEditor.OnMouseEnter();
      GridPaintingState.activeGrid = (PaintableGrid) this;
      this.activeSceneView = sceneView;
    }

    private void OnMouseLeave(SceneView sceneView)
    {
      if ((UnityEngine.Object) GridPaintingState.activeBrushEditor != (UnityEngine.Object) null)
        GridPaintingState.activeBrushEditor.OnMouseLeave();
      GridPaintingState.activeGrid = (PaintableGrid) null;
      this.activeSceneView = (SceneView) null;
    }

    private void UndoRedoPerformed()
    {
      this.RefreshAllTiles();
    }

    private void RefreshAllTiles()
    {
      if (!((UnityEngine.Object) this.tilemap != (UnityEngine.Object) null))
        return;
      this.tilemap.RefreshAllTiles();
    }

    protected override void RegisterUndo()
    {
      if (!((UnityEngine.Object) GridPaintingState.activeBrushEditor != (UnityEngine.Object) null))
        return;
      GridPaintingState.activeBrushEditor.RegisterUndo(this.brushTarget, PaintableGrid.EditModeToBrushTool(UnityEditorInternal.EditMode.editMode));
    }

    protected override void Paint(Vector3Int position)
    {
      if (!((UnityEngine.Object) this.grid != (UnityEngine.Object) null))
        return;
      this.gridBrush.Paint((GridLayout) this.grid, this.brushTarget, position);
    }

    protected override void Erase(Vector3Int position)
    {
      if (!((UnityEngine.Object) this.grid != (UnityEngine.Object) null))
        return;
      this.gridBrush.Erase((GridLayout) this.grid, this.brushTarget, position);
    }

    protected override void BoxFill(BoundsInt position)
    {
      if (!((UnityEngine.Object) this.grid != (UnityEngine.Object) null))
        return;
      this.gridBrush.BoxFill((GridLayout) this.grid, this.brushTarget, position);
    }

    protected override void BoxErase(BoundsInt position)
    {
      if (!((UnityEngine.Object) this.grid != (UnityEngine.Object) null))
        return;
      this.gridBrush.BoxErase((GridLayout) this.grid, this.brushTarget, position);
    }

    protected override void FloodFill(Vector3Int position)
    {
      if (!((UnityEngine.Object) this.grid != (UnityEngine.Object) null))
        return;
      this.gridBrush.FloodFill((GridLayout) this.grid, this.brushTarget, position);
    }

    protected override void PickBrush(BoundsInt position, Vector3Int pickStart)
    {
      if (!((UnityEngine.Object) this.grid != (UnityEngine.Object) null))
        return;
      this.gridBrush.Pick((GridLayout) this.grid, this.brushTarget, position, pickStart);
    }

    protected override void Select(BoundsInt position)
    {
      if (!((UnityEngine.Object) this.grid != (UnityEngine.Object) null))
        return;
      GridSelection.Select((UnityEngine.Object) this.brushTarget, position);
      this.gridBrush.Select((GridLayout) this.grid, this.brushTarget, position);
    }

    protected override void Move(BoundsInt from, BoundsInt to)
    {
      if (!((UnityEngine.Object) this.grid != (UnityEngine.Object) null))
        return;
      this.gridBrush.Move((GridLayout) this.grid, this.brushTarget, from, to);
    }

    protected override void MoveStart(BoundsInt position)
    {
      if (!((UnityEngine.Object) this.grid != (UnityEngine.Object) null))
        return;
      this.gridBrush.MoveStart((GridLayout) this.grid, this.brushTarget, position);
    }

    protected override void MoveEnd(BoundsInt position)
    {
      if (!((UnityEngine.Object) this.grid != (UnityEngine.Object) null))
        return;
      this.gridBrush.MoveEnd((GridLayout) this.grid, this.brushTarget, position);
    }

    protected override void ClearGridSelection()
    {
      GridSelection.Clear();
    }

    public override void Repaint()
    {
      SceneView.RepaintAll();
    }

    protected override bool ValidateFloodFillPosition(Vector3Int position)
    {
      return true;
    }

    protected override Vector2Int ScreenToGrid(Vector2 screenPosition)
    {
      if ((UnityEngine.Object) this.tilemap != (UnityEngine.Object) null)
      {
        Transform transform = this.tilemap.transform;
        Plane plane = new Plane(this.tilemap.orientationMatrix.MultiplyVector(transform.forward) * -1f, transform.position);
        Vector3Int grid = this.LocalToGrid((GridLayout) this.tilemap, GridEditorUtility.ScreenToLocal(transform, screenPosition, plane));
        return new Vector2Int(grid.x, grid.y);
      }
      if (!(bool) ((UnityEngine.Object) this.grid))
        return Vector2Int.zero;
      Vector3Int grid1 = this.LocalToGrid((GridLayout) this.grid, GridEditorUtility.ScreenToLocal(this.gridTransform, screenPosition, this.GetGridPlane(this.grid)));
      return new Vector2Int(grid1.x, grid1.y);
    }

    protected override bool PickingIsDefaultTool()
    {
      return false;
    }

    protected override bool CanPickOutsideEditMode()
    {
      return false;
    }

    protected override GridLayout.CellLayout CellLayout()
    {
      return this.grid.cellLayout;
    }

    private Vector3Int LocalToGrid(GridLayout gridLayout, Vector3 local)
    {
      return gridLayout.LocalToCell(local);
    }

    private Plane GetGridPlane(Grid grid)
    {
      switch (grid.cellSwizzle)
      {
        case GridLayout.CellSwizzle.XYZ:
          return new Plane(grid.transform.forward * -1f, grid.transform.position);
        case GridLayout.CellSwizzle.XZY:
          return new Plane(grid.transform.up * -1f, grid.transform.position);
        case GridLayout.CellSwizzle.YXZ:
          return new Plane(grid.transform.forward, grid.transform.position);
        case GridLayout.CellSwizzle.YZX:
          return new Plane(grid.transform.up, grid.transform.position);
        case GridLayout.CellSwizzle.ZXY:
          return new Plane(grid.transform.right, grid.transform.position);
        case GridLayout.CellSwizzle.ZYX:
          return new Plane(grid.transform.right * -1f, grid.transform.position);
        default:
          return new Plane(grid.transform.forward * -1f, grid.transform.position);
      }
    }

    private void CallOnPaintSceneGUI()
    {
      bool flag = GridSelection.active && (UnityEngine.Object) GridSelection.target == (UnityEngine.Object) this.brushTarget;
      if (!flag && (UnityEngine.Object) GridPaintingState.activeGrid != (UnityEngine.Object) this)
        return;
      RectInt rectInt = new RectInt(this.mouseGridPosition, new Vector2Int(1, 1));
      if (this.m_MarqueeStart.HasValue)
        rectInt = GridEditorUtility.GetMarqueeRect(this.mouseGridPosition, this.m_MarqueeStart.Value);
      else if (flag)
        rectInt = new RectInt(GridSelection.position.xMin, GridSelection.position.yMin, GridSelection.position.size.x, GridSelection.position.size.y);
      GridLayout gridLayout = !((UnityEngine.Object) this.tilemap != (UnityEngine.Object) null) ? (GridLayout) this.grid : (GridLayout) this.tilemap;
      if ((UnityEngine.Object) GridPaintingState.activeBrushEditor != (UnityEngine.Object) null)
        GridPaintingState.activeBrushEditor.OnPaintSceneGUI(gridLayout, this.brushTarget, new BoundsInt(new Vector3Int(rectInt.x, rectInt.y, 0), new Vector3Int(rectInt.width, rectInt.height, 1)), PaintableGrid.EditModeToBrushTool(UnityEditorInternal.EditMode.editMode), this.m_MarqueeStart.HasValue || this.executing);
      else
        GridBrushEditorBase.OnPaintSceneGUIInternal(gridLayout, this.brushTarget, new BoundsInt(new Vector3Int(rectInt.x, rectInt.y, 0), new Vector3Int(rectInt.width, rectInt.height, 1)), PaintableGrid.EditModeToBrushTool(UnityEditorInternal.EditMode.editMode), this.m_MarqueeStart.HasValue || this.executing);
    }

    private void CallOnSceneGUI()
    {
      if (!((UnityEngine.Object) GridPaintingState.activeBrushEditor != (UnityEngine.Object) null))
        return;
      MethodInfo method = GridPaintingState.activeBrushEditor.GetType().GetMethod("OnSceneGUI");
      if (method != null)
        method.Invoke((object) GridPaintingState.activeBrushEditor, (object[]) null);
    }
  }
}
