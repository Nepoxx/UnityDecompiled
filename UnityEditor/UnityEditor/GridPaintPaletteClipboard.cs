// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridPaintPaletteClipboard
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
  internal class GridPaintPaletteClipboard : PaintableGrid
  {
    private static readonly Color k_GridColor = Color.white.AlphaMultiplied(0.1f);
    private float k_KeyboardPanningSpeed = 3f;
    private Rect m_GUIRect = new Rect(0.0f, 0.0f, 200f, 200f);
    private bool m_PingTileAsset = false;
    private bool m_PaletteNeedsSave;
    private const float k_ZoomSpeed = 7f;
    private const float k_MinZoom = 10f;
    private const float k_MaxZoom = 100f;
    private const float k_Padding = 0.75f;
    private int m_KeyboardPanningID;
    private int m_MousePanningID;
    private Vector3 m_KeyboardPanning;
    private bool m_OldFog;
    [SerializeField]
    private GridPaintPaletteWindow m_Owner;
    [SerializeField]
    private bool m_CameraInitializedToBounds;
    [SerializeField]
    public bool m_CameraPositionSaved;
    [SerializeField]
    public Vector3 m_CameraPosition;
    [SerializeField]
    public float m_CameraOrthographicSize;
    private RectInt? m_ActivePick;
    private Dictionary<Vector2Int, UnityEngine.Object> m_HoverData;
    private bool m_Unlocked;
    private Mesh m_GridMesh;
    private int m_LastGridHash;
    private Material m_GridMaterial;
    private bool m_PaletteUsed;
    private Vector2? m_PreviousMousePosition;

    public Rect guiRect
    {
      get
      {
        return this.m_GUIRect;
      }
      set
      {
        if (!(this.m_GUIRect != value))
          return;
        Rect guiRect = this.m_GUIRect;
        this.m_GUIRect = value;
        this.OnViewSizeChanged(guiRect, this.m_GUIRect);
      }
    }

    public bool activeDragAndDrop
    {
      get
      {
        return DragAndDrop.objectReferences.Length > 0 && this.guiRect.Contains(Event.current.mousePosition);
      }
    }

    public GameObject palette
    {
      get
      {
        return this.m_Owner.palette;
      }
    }

    public GameObject paletteInstance
    {
      get
      {
        return this.m_Owner.paletteInstance;
      }
    }

    public Tilemap tilemap
    {
      get
      {
        return !((UnityEngine.Object) this.paletteInstance != (UnityEngine.Object) null) ? (Tilemap) null : this.paletteInstance.GetComponentInChildren<Tilemap>();
      }
    }

    private Grid grid
    {
      get
      {
        return !((UnityEngine.Object) this.paletteInstance != (UnityEngine.Object) null) ? (Grid) null : this.paletteInstance.GetComponent<Grid>();
      }
    }

    private Grid prefabGrid
    {
      get
      {
        return !((UnityEngine.Object) this.palette != (UnityEngine.Object) null) ? (Grid) null : this.palette.GetComponent<Grid>();
      }
    }

    public PreviewRenderUtility previewUtility
    {
      get
      {
        return this.m_Owner.previewUtility;
      }
    }

    private GridBrushBase gridBrush
    {
      get
      {
        return GridPaintingState.gridBrush;
      }
    }

    public TileBase activeTile
    {
      get
      {
        if (this.m_ActivePick.HasValue && (this.m_ActivePick.Value.size == Vector2Int.one && (UnityEngine.Object) GridPaintingState.defaultBrush != (UnityEngine.Object) null && GridPaintingState.defaultBrush.cellCount > 0))
          return GridPaintingState.defaultBrush.cells[0].tile;
        return (TileBase) null;
      }
    }

    private RectInt bounds
    {
      get
      {
        if ((UnityEngine.Object) this.tilemap == (UnityEngine.Object) null)
          return new RectInt();
        RectInt rectInt = new RectInt(this.tilemap.origin.x, this.tilemap.origin.y, this.tilemap.size.x, this.tilemap.size.y);
        if (GridPaintPaletteClipboard.TilemapIsEmpty(this.tilemap))
          return rectInt;
        int num1 = this.tilemap.origin.x + this.tilemap.size.x;
        int num2 = this.tilemap.origin.y + this.tilemap.size.y;
        int val1_1 = this.tilemap.origin.x;
        int val1_2 = this.tilemap.origin.y;
        foreach (Vector2Int vector2Int in rectInt.allPositionsWithin)
        {
          if ((UnityEngine.Object) this.tilemap.GetTile(new Vector3Int(vector2Int.x, vector2Int.y, 0)) != (UnityEngine.Object) null)
          {
            num1 = Math.Min(num1, vector2Int.x);
            num2 = Math.Min(num2, vector2Int.y);
            val1_1 = Math.Max(val1_1, vector2Int.x);
            val1_2 = Math.Max(val1_2, vector2Int.y);
          }
        }
        return new RectInt(num1, num2, val1_1 - num1 + 1, val1_2 - num2 + 1);
      }
    }

    private Rect paddedBounds
    {
      get
      {
        float x = (float) ((double) this.previewUtility.camera.orthographicSize * (double) (this.m_GUIRect.width / this.m_GUIRect.height) * 0.75 * 2.0);
        float y = (float) ((double) this.previewUtility.camera.orthographicSize * 0.75 * 2.0);
        RectInt bounds = this.bounds;
        Vector2 local1 = (Vector2) this.grid.CellToLocal(new Vector3Int(bounds.xMin, bounds.yMin, 0));
        Vector2 local2 = (Vector2) this.grid.CellToLocal(new Vector3Int(bounds.xMax, bounds.yMax, 0));
        return new Rect(local1 - new Vector2(x, y), local2 - local1 + new Vector2(x, y) * 2f);
      }
    }

    private RectInt paddedBoundsInt
    {
      get
      {
        Vector3Int cell = this.grid.LocalToCell((Vector3) this.paddedBounds.min);
        Vector3Int vector3Int = this.grid.LocalToCell((Vector3) this.paddedBounds.max) + Vector3Int.one;
        return new RectInt(cell.x, cell.y, vector3Int.x - cell.x, vector3Int.y - cell.y);
      }
    }

    private GameObject brushTarget
    {
      get
      {
        return !((UnityEngine.Object) this.tilemap != (UnityEngine.Object) null) ? (GameObject) null : this.tilemap.gameObject;
      }
    }

    public bool unlocked
    {
      get
      {
        return this.m_Unlocked;
      }
      set
      {
        if (!value && this.m_Unlocked && (UnityEngine.Object) this.tilemap != (UnityEngine.Object) null)
        {
          this.tilemap.ClearAllEditorPreviewTiles();
          this.SavePaletteIfNecessary();
        }
        this.m_Unlocked = value;
      }
    }

    public bool pingTileAsset
    {
      get
      {
        return this.m_PingTileAsset;
      }
      set
      {
        if (value && !this.m_PingTileAsset && this.m_ActivePick.HasValue)
          this.PingTileAsset(this.m_ActivePick.Value);
        this.m_PingTileAsset = value;
      }
    }

    public bool invalidClipboard
    {
      get
      {
        return (UnityEngine.Object) this.m_Owner.palette == (UnityEngine.Object) null;
      }
    }

    public bool isReceivingDragAndDrop
    {
      get
      {
        return this.m_HoverData != null && this.m_HoverData.Count > 0;
      }
    }

    public bool showNewEmptyClipboardInfo
    {
      get
      {
        return !((UnityEngine.Object) this.paletteInstance == (UnityEngine.Object) null) && !((UnityEngine.Object) this.tilemap == (UnityEngine.Object) null) && (GridPaintPaletteClipboard.TilemapIsEmpty(this.tilemap) && !this.isReceivingDragAndDrop) && !this.m_PaletteUsed;
      }
    }

    public bool isModified
    {
      get
      {
        return this.m_PaletteNeedsSave;
      }
    }

    public GridPaintPaletteWindow owner
    {
      set
      {
        this.m_Owner = value;
      }
    }

    public void OnBeforePaletteSelectionChanged()
    {
      this.SavePaletteIfNecessary();
      this.DestroyPreviewInstance();
      this.FlushHoverData();
    }

    private void FlushHoverData()
    {
      if (this.m_HoverData == null)
        return;
      this.m_HoverData.Clear();
      this.m_HoverData = (Dictionary<Vector2Int, UnityEngine.Object>) null;
    }

    public void OnAfterPaletteSelectionChanged()
    {
      this.m_PaletteUsed = false;
      this.ResetPreviewInstance();
      if (!((UnityEngine.Object) this.palette != (UnityEngine.Object) null))
        return;
      this.ResetPreviewCamera();
    }

    public void SetupPreviewCameraOnInit()
    {
      if (this.m_CameraPositionSaved)
        this.LoadSavedCameraPosition();
      else
        this.ResetPreviewCamera();
    }

    private void LoadSavedCameraPosition()
    {
      this.previewUtility.camera.transform.position = this.m_CameraPosition;
      this.previewUtility.camera.orthographicSize = this.m_CameraOrthographicSize;
      this.previewUtility.camera.nearClipPlane = 0.01f;
      this.previewUtility.camera.farClipPlane = 100f;
    }

    private void ResetPreviewCamera()
    {
      this.previewUtility.camera.transform.position = new Vector3(0.0f, 0.0f, -10f);
      this.previewUtility.camera.transform.rotation = Quaternion.identity;
      this.previewUtility.camera.nearClipPlane = 0.01f;
      this.previewUtility.camera.farClipPlane = 100f;
      this.FrameEntirePalette();
    }

    private void DestroyPreviewInstance()
    {
      this.m_Owner.DestroyPreviewInstance();
    }

    private void ResetPreviewInstance()
    {
      this.m_Owner.ResetPreviewInstance();
    }

    public void ResetPreviewMesh()
    {
      if ((UnityEngine.Object) this.m_GridMesh != (UnityEngine.Object) null)
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_GridMesh);
        this.m_GridMesh = (Mesh) null;
      }
      this.m_GridMaterial = (Material) null;
    }

    public void FrameEntirePalette()
    {
      this.Frame(this.bounds);
    }

    private void Frame(RectInt rect)
    {
      if ((UnityEngine.Object) this.grid == (UnityEngine.Object) null)
        return;
      this.previewUtility.camera.transform.position = this.grid.CellToLocalInterpolated(new Vector3(rect.center.x, rect.center.y, 0.0f));
      this.previewUtility.camera.transform.position.Set(this.previewUtility.camera.transform.position.x, this.previewUtility.camera.transform.position.y, -10f);
      float magnitude1 = (this.grid.CellToLocal(new Vector3Int(0, rect.yMax, 0)) - this.grid.CellToLocal(new Vector3Int(0, rect.yMin, 0))).magnitude;
      float magnitude2 = (this.grid.CellToLocal(new Vector3Int(rect.xMax, 0, 0)) - this.grid.CellToLocal(new Vector3Int(rect.xMin, 0, 0))).magnitude;
      float num1 = magnitude1 + this.grid.cellSize.y;
      float num2 = magnitude2 + this.grid.cellSize.x;
      float num3 = this.m_GUIRect.width / this.m_GUIRect.height;
      float num4 = num2 / num1;
      this.previewUtility.camera.orthographicSize = (double) num3 <= (double) num4 ? (float) ((double) num2 / (double) num3 / 2.0) : num1 / 2f;
      this.ClampZoomAndPan();
    }

    private void RefreshAllTiles()
    {
      if (!((UnityEngine.Object) this.tilemap != (UnityEngine.Object) null))
        return;
      this.tilemap.RefreshAllTiles();
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      EditorApplication.editorApplicationQuit += new UnityAction(this.EditorApplicationQuit);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      this.m_KeyboardPanningID = GUIUtility.GetPermanentControlID();
      this.m_MousePanningID = GUIUtility.GetPermanentControlID();
    }

    protected override void OnDisable()
    {
      this.m_CameraPosition = this.previewUtility.camera.transform.position;
      this.m_CameraOrthographicSize = this.previewUtility.camera.orthographicSize;
      this.m_CameraPositionSaved = true;
      this.SavePaletteIfNecessary();
      this.DestroyPreviewInstance();
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      EditorApplication.editorApplicationQuit -= new UnityAction(this.EditorApplicationQuit);
      base.OnDisable();
    }

    private void OnDestroy()
    {
      this.previewUtility.Cleanup();
    }

    public override void OnGUI()
    {
      if ((double) this.guiRect.width == 0.0 || (double) this.guiRect.height == 0.0)
        return;
      this.UpdateMouseGridPosition();
      this.HandleDragAndDrop();
      if ((UnityEngine.Object) this.palette == (UnityEngine.Object) null)
        return;
      this.HandlePanAndZoom();
      if (this.showNewEmptyClipboardInfo)
        return;
      if (Event.current.type == EventType.Repaint && !this.m_CameraInitializedToBounds)
      {
        this.Frame(this.bounds);
        this.m_CameraInitializedToBounds = true;
      }
      this.HandleMouseEnterLeave();
      if (this.guiRect.Contains(Event.current.mousePosition))
      {
        if ((this.m_PreviousMousePosition.HasValue && !this.guiRect.Contains(this.m_PreviousMousePosition.Value) || !this.m_PreviousMousePosition.HasValue) && (UnityEngine.Object) GridPaintingState.activeBrushEditor != (UnityEngine.Object) null)
          GridPaintingState.activeBrushEditor.OnMouseEnter();
        base.OnGUI();
      }
      else if (this.m_PreviousMousePosition.HasValue && this.guiRect.Contains(this.m_PreviousMousePosition.Value) && (!this.guiRect.Contains(Event.current.mousePosition) && (UnityEngine.Object) GridPaintingState.activeBrushEditor != (UnityEngine.Object) null))
      {
        GridPaintingState.activeBrushEditor.OnMouseLeave();
        this.Repaint();
      }
      if (Event.current.type == EventType.Repaint)
        this.Render();
      else
        this.DoBrush();
      this.m_PreviousMousePosition = new Vector2?(Event.current.mousePosition);
    }

    public void OnViewSizeChanged(Rect oldSize, Rect newSize)
    {
      if ((double) oldSize.height * (double) oldSize.width * (double) newSize.height * (double) newSize.width == 0.0)
        return;
      this.previewUtility.camera.transform.Translate((Vector3) (new Vector2((float) ((double) newSize.width / (double) this.LocalToScreenRatio(newSize.height) - (double) oldSize.width / (double) this.LocalToScreenRatio(oldSize.height)), (float) ((double) newSize.height / (double) this.LocalToScreenRatio(newSize.height) - (double) oldSize.height / (double) this.LocalToScreenRatio(oldSize.height))) / 2f));
      this.ClampZoomAndPan();
    }

    private void EditorApplicationQuit()
    {
      this.SavePaletteIfNecessary();
    }

    private void UndoRedoPerformed()
    {
      if (!this.unlocked)
        return;
      this.m_PaletteNeedsSave = true;
      this.RefreshAllTiles();
      this.Repaint();
    }

    private void HandlePanAndZoom()
    {
      switch (Event.current.type)
      {
        case EventType.MouseDown:
          if (!GridPaintPaletteClipboard.MousePanningEvent() || (!this.guiRect.Contains(Event.current.mousePosition) || GUIUtility.hotControl != 0))
            break;
          GUIUtility.hotControl = this.m_MousePanningID;
          Event.current.Use();
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != this.m_MousePanningID)
            break;
          this.ClampZoomAndPan();
          GUIUtility.hotControl = 0;
          Event.current.Use();
          break;
        case EventType.MouseMove:
          if (GUIUtility.hotControl != this.m_MousePanningID || GridPaintPaletteClipboard.MousePanningEvent())
            break;
          GUIUtility.hotControl = 0;
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != this.m_MousePanningID)
            break;
          this.previewUtility.camera.transform.Translate(new Vector3(-Event.current.delta.x, Event.current.delta.y, 0.0f) / this.LocalToScreenRatio());
          this.ClampZoomAndPan();
          Event.current.Use();
          break;
        case EventType.KeyDown:
          if (GUIUtility.hotControl != 0)
            break;
          switch (Event.current.keyCode)
          {
            case KeyCode.UpArrow:
              this.m_KeyboardPanning = new Vector3(0.0f, this.k_KeyboardPanningSpeed) / this.LocalToScreenRatio();
              GUIUtility.hotControl = this.m_KeyboardPanningID;
              Event.current.Use();
              break;
            case KeyCode.DownArrow:
              this.m_KeyboardPanning = new Vector3(0.0f, -this.k_KeyboardPanningSpeed) / this.LocalToScreenRatio();
              GUIUtility.hotControl = this.m_KeyboardPanningID;
              Event.current.Use();
              break;
            case KeyCode.RightArrow:
              this.m_KeyboardPanning = new Vector3(this.k_KeyboardPanningSpeed, 0.0f) / this.LocalToScreenRatio();
              GUIUtility.hotControl = this.m_KeyboardPanningID;
              Event.current.Use();
              break;
            case KeyCode.LeftArrow:
              this.m_KeyboardPanning = new Vector3(-this.k_KeyboardPanningSpeed, 0.0f) / this.LocalToScreenRatio();
              GUIUtility.hotControl = this.m_KeyboardPanningID;
              Event.current.Use();
              break;
          }
          break;
        case EventType.KeyUp:
          if (GUIUtility.hotControl != this.m_KeyboardPanningID)
            break;
          this.m_KeyboardPanning = Vector3.zero;
          GUIUtility.hotControl = 0;
          Event.current.Use();
          break;
        case EventType.ScrollWheel:
          if (!this.guiRect.Contains(Event.current.mousePosition))
            break;
          float num = (float) ((double) HandleUtility.niceMouseDeltaZoom * (!Event.current.shift ? -3.0 : -9.0) * 7.0);
          Camera camera = this.previewUtility.camera;
          Vector3 local = (Vector3) this.ScreenToLocal(Event.current.mousePosition);
          camera.orthographicSize = Mathf.Max(0.0001f, camera.orthographicSize * (float) (1.0 + (double) num * (1.0 / 1000.0)));
          this.ClampZoomAndPan();
          Vector3 vector3 = (Vector3) this.ScreenToLocal(Event.current.mousePosition) - local;
          camera.transform.position = camera.transform.position - vector3;
          this.ClampZoomAndPan();
          Event.current.Use();
          break;
        case EventType.Repaint:
          if (GUIUtility.hotControl == this.m_KeyboardPanningID)
          {
            this.previewUtility.camera.transform.Translate(this.m_KeyboardPanning);
            this.ClampZoomAndPan();
            this.Repaint();
          }
          if (GUIUtility.hotControl != this.m_MousePanningID)
            break;
          EditorGUIUtility.AddCursorRect(this.guiRect, MouseCursor.Pan);
          break;
        case EventType.ValidateCommand:
          if (!(Event.current.commandName == "FrameSelected"))
            break;
          Event.current.Use();
          break;
        case EventType.ExecuteCommand:
          if (!(Event.current.commandName == "FrameSelected"))
            break;
          if (this.m_ActivePick.HasValue)
            this.Frame(this.m_ActivePick.Value);
          else
            this.FrameEntirePalette();
          Event.current.Use();
          break;
      }
    }

    private static bool MousePanningEvent()
    {
      return Event.current.button == 0 && Event.current.alt || Event.current.button > 0;
    }

    public void ClampZoomAndPan()
    {
      float num = this.grid.cellSize.y * this.LocalToScreenRatio();
      if ((double) num < 10.0)
        this.previewUtility.camera.orthographicSize = (float) ((double) this.grid.cellSize.y * (double) this.guiRect.height / 20.0);
      else if ((double) num > 100.0)
        this.previewUtility.camera.orthographicSize = (float) ((double) this.grid.cellSize.y * (double) this.guiRect.height / 200.0);
      Camera camera = this.previewUtility.camera;
      Rect paddedBounds = this.paddedBounds;
      Vector3 position = camera.transform.position;
      Vector2 vector2_1 = (Vector2) (position - new Vector3(camera.orthographicSize * (this.guiRect.width / this.guiRect.height), camera.orthographicSize));
      Vector2 vector2_2 = (Vector2) (position + new Vector3(camera.orthographicSize * (this.guiRect.width / this.guiRect.height), camera.orthographicSize));
      if ((double) vector2_1.x < (double) paddedBounds.min.x)
        position += new Vector3(paddedBounds.min.x - vector2_1.x, 0.0f, 0.0f);
      if ((double) vector2_1.y < (double) paddedBounds.min.y)
        position += new Vector3(0.0f, paddedBounds.min.y - vector2_1.y, 0.0f);
      if ((double) vector2_2.x > (double) paddedBounds.max.x)
        position += new Vector3(paddedBounds.max.x - vector2_2.x, 0.0f, 0.0f);
      if ((double) vector2_2.y > (double) paddedBounds.max.y)
        position += new Vector3(0.0f, paddedBounds.max.y - vector2_2.y, 0.0f);
      position.Set(position.x, position.y, -10f);
      camera.transform.position = position;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_GridMesh);
      this.m_GridMesh = (Mesh) null;
    }

    private void Render()
    {
      if ((UnityEngine.Object) this.m_GridMesh != (UnityEngine.Object) null && this.GetGridHash() != this.m_LastGridHash)
      {
        this.ResetPreviewInstance();
        this.ResetPreviewMesh();
      }
      this.previewUtility.BeginPreview(this.guiRect, GridPaintPaletteClipboard.Styles.background);
      this.BeginPreviewInstance();
      this.RenderGrid();
      this.EndPreviewInstance();
      this.RenderDragAndDropPreview();
      this.RenderSelectedBrushMarquee();
      this.DoBrush();
      this.previewUtility.EndAndDrawPreview(this.guiRect);
      this.m_LastGridHash = this.GetGridHash();
    }

    private int GetGridHash()
    {
      if ((UnityEngine.Object) this.prefabGrid == (UnityEngine.Object) null)
        return 0;
      return ((((this.prefabGrid.GetHashCode() * 33 + this.prefabGrid.cellGap.GetHashCode()) * 33 + this.prefabGrid.cellLayout.GetHashCode()) * 33 + this.prefabGrid.cellSize.GetHashCode()) * 33 + this.prefabGrid.cellSwizzle.GetHashCode()) * 33 + SceneViewGridManager.sceneViewGridComponentGizmo.Color.GetHashCode();
    }

    private void RenderDragAndDropPreview()
    {
      if (!this.activeDragAndDrop || this.m_HoverData == null || this.m_HoverData.Count == 0)
        return;
      RectInt minMaxRect = TileDragAndDrop.GetMinMaxRect(this.m_HoverData.Keys.ToList<Vector2Int>());
      minMaxRect.position += this.mouseGridPosition;
      DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
      GridEditorUtility.DrawGridMarquee((GridLayout) this.grid, new BoundsInt(new Vector3Int(minMaxRect.xMin, minMaxRect.yMin, 0), new Vector3Int(minMaxRect.width, minMaxRect.height, 1)), Color.white);
    }

    private void RenderGrid()
    {
      if ((UnityEngine.Object) this.m_GridMesh == (UnityEngine.Object) null && this.grid.cellLayout == GridLayout.CellLayout.Rectangle)
        this.m_GridMesh = GridEditorUtility.GenerateCachedGridMesh((GridLayout) this.grid, GridPaintPaletteClipboard.k_GridColor, 1f / this.LocalToScreenRatio(), this.paddedBoundsInt, MeshTopology.Quads);
      GridEditorUtility.DrawGridGizmo((GridLayout) this.grid, this.grid.transform, GridPaintPaletteClipboard.k_GridColor, ref this.m_GridMesh, ref this.m_GridMaterial);
    }

    private void DoBrush()
    {
      if (this.activeDragAndDrop)
        return;
      this.RenderSelectedBrushMarquee();
      this.CallOnPaintSceneGUI(this.mouseGridPosition);
    }

    private void BeginPreviewInstance()
    {
      this.m_OldFog = RenderSettings.fog;
      Unsupported.SetRenderSettingsUseFogNoDirty(false);
      Handles.DrawCameraImpl(this.m_GUIRect, this.previewUtility.camera, DrawCameraMode.Textured, false, new DrawGridParameters(), true, false);
      PreviewRenderUtility.SetEnabledRecursive(this.paletteInstance, true);
    }

    private void EndPreviewInstance()
    {
      this.previewUtility.Render(false, true);
      PreviewRenderUtility.SetEnabledRecursive(this.paletteInstance, false);
      Unsupported.SetRenderSettingsUseFogNoDirty(this.m_OldFog);
    }

    public void HandleDragAndDrop()
    {
      if (DragAndDrop.objectReferences.Length == 0 || !this.guiRect.Contains(Event.current.mousePosition))
        return;
      switch (Event.current.type)
      {
        case EventType.DragUpdated:
          this.m_HoverData = TileDragAndDrop.CreateHoverData(TileDragAndDrop.GetValidSpritesheets(DragAndDrop.objectReferences), TileDragAndDrop.GetValidSingleSprites(DragAndDrop.objectReferences), TileDragAndDrop.GetValidTiles(DragAndDrop.objectReferences));
          if (this.m_HoverData != null && this.m_HoverData.Count > 0)
          {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            Event.current.Use();
            GUI.changed = true;
            break;
          }
          break;
        case EventType.DragPerform:
          if (this.m_HoverData == null || this.m_HoverData.Count == 0)
            return;
          this.RegisterUndo();
          bool flag = GridPaintPaletteClipboard.TilemapIsEmpty(this.tilemap);
          Vector2Int mouseGridPosition = this.mouseGridPosition;
          DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
          foreach (KeyValuePair<Vector2Int, TileBase> keyValuePair in TileDragAndDrop.ConvertToTileSheet(this.m_HoverData))
            this.SetTile(this.tilemap, mouseGridPosition + keyValuePair.Key, keyValuePair.Value, Color.white, Matrix4x4.identity);
          this.OnPaletteChanged();
          this.m_PaletteNeedsSave = true;
          this.FlushHoverData();
          GUI.changed = true;
          this.SavePaletteIfNecessary();
          if (flag)
          {
            this.ResetPreviewInstance();
            this.FrameEntirePalette();
          }
          Event.current.Use();
          GUIUtility.ExitGUI();
          break;
      }
      if (this.m_HoverData == null || Event.current.type != EventType.DragExited && (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Escape))
        return;
      DragAndDrop.visualMode = DragAndDropVisualMode.None;
      this.FlushHoverData();
      Event.current.Use();
    }

    private static bool GridSizeUninitialized(Grid grid)
    {
      return Mathf.Approximately(grid.cellSize.x, 1E-05f) && Mathf.Approximately(grid.cellSize.y, 1E-05f) && Mathf.Approximately(grid.cellSize.z, 1E-05f);
    }

    public void SetEditorPreviewTile(Tilemap tilemap, Vector2Int position, TileBase tile, Color color, Matrix4x4 matrix)
    {
      Vector3Int position1 = new Vector3Int(position.x, position.y, 0);
      tilemap.SetEditorPreviewTile(position1, tile);
      tilemap.SetEditorPreviewColor(position1, color);
      tilemap.SetEditorPreviewTransformMatrix(position1, matrix);
    }

    public void SetTile(Tilemap tilemap, Vector2Int position, TileBase tile, Color color, Matrix4x4 matrix)
    {
      Vector3Int position1 = new Vector3Int(position.x, position.y, 0);
      tilemap.SetTile(position1, tile);
      tilemap.SetColor(position1, color);
      tilemap.SetTransformMatrix(position1, matrix);
    }

    protected override void Paint(Vector3Int position)
    {
      if ((UnityEngine.Object) this.gridBrush == (UnityEngine.Object) null)
        return;
      this.gridBrush.Paint((GridLayout) this.grid, this.brushTarget, position);
      this.OnPaletteChanged();
    }

    protected override void Erase(Vector3Int position)
    {
      if ((UnityEngine.Object) this.gridBrush == (UnityEngine.Object) null)
        return;
      this.gridBrush.Erase((GridLayout) this.grid, this.brushTarget, position);
      this.OnPaletteChanged();
    }

    protected override void BoxFill(BoundsInt position)
    {
      if ((UnityEngine.Object) this.gridBrush == (UnityEngine.Object) null)
        return;
      this.gridBrush.BoxFill((GridLayout) this.grid, this.brushTarget, position);
      this.OnPaletteChanged();
    }

    protected override void BoxErase(BoundsInt position)
    {
      if ((UnityEngine.Object) this.gridBrush == (UnityEngine.Object) null)
        return;
      this.gridBrush.BoxErase((GridLayout) this.grid, this.brushTarget, position);
      this.OnPaletteChanged();
    }

    protected override void FloodFill(Vector3Int position)
    {
      if ((UnityEngine.Object) this.gridBrush == (UnityEngine.Object) null)
        return;
      this.gridBrush.FloodFill((GridLayout) this.grid, this.brushTarget, position);
      this.OnPaletteChanged();
    }

    protected override void PickBrush(BoundsInt position, Vector3Int pickingStart)
    {
      if ((UnityEngine.Object) this.grid == (UnityEngine.Object) null || (UnityEngine.Object) this.gridBrush == (UnityEngine.Object) null)
        return;
      this.gridBrush.Pick((GridLayout) this.grid, this.brushTarget, position, pickingStart);
      if (!PaintableGrid.InGridEditMode())
        UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.GridPainting, new Bounds(), (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
      this.m_ActivePick = new RectInt?(new RectInt(position.min.x, position.min.y, position.size.x, position.size.y));
    }

    protected override void Select(BoundsInt position)
    {
      if (!(bool) ((UnityEngine.Object) this.grid))
        return;
      GridSelection.Select((UnityEngine.Object) this.brushTarget, position);
      this.gridBrush.Select((GridLayout) this.grid, this.brushTarget, position);
    }

    protected override void Move(BoundsInt from, BoundsInt to)
    {
      if (!(bool) ((UnityEngine.Object) this.grid))
        return;
      this.gridBrush.Move((GridLayout) this.grid, this.brushTarget, from, to);
    }

    protected override void MoveStart(BoundsInt position)
    {
      if (!(bool) ((UnityEngine.Object) this.grid))
        return;
      this.gridBrush.MoveStart((GridLayout) this.grid, this.brushTarget, position);
    }

    protected override void MoveEnd(BoundsInt position)
    {
      if (!(bool) ((UnityEngine.Object) this.grid))
        return;
      this.gridBrush.MoveEnd((GridLayout) this.grid, this.brushTarget, position);
      this.OnPaletteChanged();
    }

    public override void Repaint()
    {
      this.m_Owner.Repaint();
    }

    protected override void ClearGridSelection()
    {
      GridSelection.Clear();
    }

    protected override void OnBrushPickStarted()
    {
    }

    protected override void OnBrushPickDragged(BoundsInt position)
    {
      this.m_ActivePick = new RectInt?(new RectInt(position.min.x, position.min.y, position.size.x, position.size.y));
    }

    private void PingTileAsset(RectInt rect)
    {
      if (!(rect.size == Vector2Int.zero) || !((UnityEngine.Object) this.tilemap != (UnityEngine.Object) null))
        return;
      TileBase tile = this.tilemap.GetTile(new Vector3Int(rect.xMin, rect.yMin, 0));
      EditorGUIUtility.PingObject((UnityEngine.Object) tile);
      Selection.activeObject = (UnityEngine.Object) tile;
    }

    protected override bool ValidateFloodFillPosition(Vector3Int position)
    {
      return true;
    }

    protected override bool PickingIsDefaultTool()
    {
      return !this.m_Unlocked;
    }

    protected override bool CanPickOutsideEditMode()
    {
      return true;
    }

    protected override GridLayout.CellLayout CellLayout()
    {
      return this.grid.cellLayout;
    }

    protected override Vector2Int ScreenToGrid(Vector2 screenPosition)
    {
      Vector3Int cell = this.grid.LocalToCell((Vector3) this.ScreenToLocal(screenPosition));
      return new Vector2Int(cell.x, cell.y);
    }

    private void RenderSelectedBrushMarquee()
    {
      if (this.unlocked || !this.m_ActivePick.HasValue)
        return;
      this.DrawSelectionGizmo(this.m_ActivePick.Value);
    }

    protected void DrawSelectionGizmo(RectInt rect)
    {
      if (Event.current.type != EventType.Repaint || !GUI.enabled)
        return;
      Color color = Color.white;
      if (this.isPicking)
        color = Color.cyan;
      GridEditorUtility.DrawGridMarquee((GridLayout) this.grid, new BoundsInt(new Vector3Int(rect.xMin, rect.yMin, 0), new Vector3Int(rect.width, rect.height, 1)), color);
    }

    private void HandleMouseEnterLeave()
    {
      if (Event.current.type == EventType.MouseEnterWindow)
      {
        if (!PaintableGrid.InGridEditMode())
          return;
        GridPaintingState.activeGrid = (PaintableGrid) this;
        Event.current.Use();
      }
      else
      {
        if (Event.current.type != EventType.MouseLeaveWindow)
          return;
        if (this.m_PreviousMousePosition.HasValue && (this.guiRect.Contains(this.m_PreviousMousePosition.Value) && (UnityEngine.Object) GridPaintingState.activeBrushEditor != (UnityEngine.Object) null))
          GridPaintingState.activeBrushEditor.OnMouseLeave();
        this.m_PreviousMousePosition = new Vector2?();
        if (PaintableGrid.InGridEditMode())
        {
          GridPaintingState.activeGrid = (PaintableGrid) null;
          Event.current.Use();
          this.Repaint();
        }
      }
    }

    private void CallOnPaintSceneGUI(Vector2Int position)
    {
      if (!this.unlocked && UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect && UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridPicking)
        return;
      bool flag = GridSelection.active && (UnityEngine.Object) GridSelection.target == (UnityEngine.Object) this.brushTarget;
      if (!flag && (UnityEngine.Object) GridPaintingState.activeGrid != (UnityEngine.Object) this || (UnityEngine.Object) GridPaintingState.gridBrush == (UnityEngine.Object) null)
        return;
      RectInt rectInt = new RectInt(position, new Vector2Int(1, 1));
      if (this.m_MarqueeStart.HasValue)
        rectInt = GridEditorUtility.GetMarqueeRect(position, this.m_MarqueeStart.Value);
      else if (flag)
        rectInt = new RectInt(GridSelection.position.xMin, GridSelection.position.yMin, GridSelection.position.size.x, GridSelection.position.size.y);
      GridLayout gridLayout = !((UnityEngine.Object) this.tilemap != (UnityEngine.Object) null) ? (GridLayout) this.grid : (GridLayout) this.tilemap;
      BoundsInt position1 = new BoundsInt(new Vector3Int(rectInt.x, rectInt.y, 0), new Vector3Int(rectInt.width, rectInt.height, 1));
      if ((UnityEngine.Object) GridPaintingState.activeBrushEditor != (UnityEngine.Object) null)
        GridPaintingState.activeBrushEditor.OnPaintSceneGUI(gridLayout, this.brushTarget, position1, PaintableGrid.EditModeToBrushTool(UnityEditorInternal.EditMode.editMode), this.m_MarqueeStart.HasValue || this.executing);
      else
        GridBrushEditorBase.OnPaintSceneGUIInternal(gridLayout, Selection.activeGameObject, position1, PaintableGrid.EditModeToBrushTool(UnityEditorInternal.EditMode.editMode), this.m_MarqueeStart.HasValue || this.executing);
    }

    protected override void RegisterUndo()
    {
      if (this.invalidClipboard)
        return;
      Undo.RegisterFullObjectHierarchyUndo((UnityEngine.Object) this.paletteInstance, "Edit Palette");
    }

    private void OnPaletteChanged()
    {
      this.m_PaletteUsed = true;
      this.m_PaletteNeedsSave = true;
      Undo.FlushUndoRecordObjects();
    }

    public void SavePaletteIfNecessary()
    {
      if (!this.m_PaletteNeedsSave)
        return;
      this.m_Owner.SavePalette();
      this.m_PaletteNeedsSave = false;
    }

    private static RectInt SnapInsideBounds(RectInt rect, RectInt bounds)
    {
      if (rect.xMin < bounds.xMin)
        rect.position += new Vector2Int(bounds.xMin - rect.xMin, 0);
      if (rect.yMin < bounds.yMin)
        rect.position += new Vector2Int(0, bounds.yMin - rect.yMin);
      if (rect.xMax > bounds.xMax)
        rect.position -= new Vector2Int(rect.xMax - bounds.xMax, 0);
      if (rect.yMax > bounds.yMax)
        rect.position -= new Vector2Int(0, rect.yMax - bounds.yMax);
      return rect;
    }

    public Vector2 GridToScreen(Vector2 gridPosition)
    {
      return this.LocalToScreen((Vector2) this.grid.CellToLocalInterpolated(new Vector3(gridPosition.x, gridPosition.y, 0.0f)));
    }

    protected Vector2 GridToScreen(Vector2Int gridPosition)
    {
      return this.LocalToScreen((Vector2) this.grid.CellToLocal(new Vector3Int(gridPosition.x, gridPosition.y, 0)));
    }

    public Vector2 ScreenToLocal(Vector2 screenPosition)
    {
      Vector2 position = (Vector2) this.previewUtility.camera.transform.position;
      screenPosition -= new Vector2(this.guiRect.xMin, this.guiRect.yMin);
      Vector2 vector2 = new Vector2(screenPosition.x - this.guiRect.width * 0.5f, this.guiRect.height * 0.5f - screenPosition.y);
      return position + vector2 / this.LocalToScreenRatio();
    }

    protected Vector2 LocalToScreen(Vector2 localPosition)
    {
      Vector2 position = (Vector2) this.previewUtility.camera.transform.position;
      return new Vector2(localPosition.x - position.x, position.y - localPosition.y) * this.LocalToScreenRatio() + new Vector2(this.guiRect.width * 0.5f + this.guiRect.xMin, this.guiRect.height * 0.5f + this.guiRect.yMin);
    }

    private float LocalToScreenRatio()
    {
      return this.guiRect.height / (this.previewUtility.camera.orthographicSize * 2f);
    }

    private float LocalToScreenRatio(float viewHeight)
    {
      return viewHeight / (this.previewUtility.camera.orthographicSize * 2f);
    }

    protected Vector2Int GetPivot(Vector2Int corner, Vector2Int position)
    {
      return position - corner;
    }

    private static bool TilemapIsEmpty(Tilemap tilemap)
    {
      return tilemap.GetUsedTilesCount() == 0;
    }

    private static class Styles
    {
      public static readonly GUIStyle background = new GUIStyle((GUIStyle) "CurveEditorBackground");
    }
  }
}
