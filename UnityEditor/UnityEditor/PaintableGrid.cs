// Decompiled with JetBrains decompiler
// Type: UnityEditor.PaintableGrid
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class PaintableGrid : ScriptableObject
  {
    protected Vector2Int? m_PreviousMove = new Vector2Int?();
    protected Vector2Int? m_MarqueeStart = new Vector2Int?();
    private PaintableGrid.MarqueeType m_MarqueeType = PaintableGrid.MarqueeType.None;
    private int m_PermanentControlID;
    internal static PaintableGrid s_LastActivePaintableGrid;
    private Vector2Int m_PreviousMouseGridPosition;
    private Vector2Int m_MouseGridPosition;
    private bool m_MouseGridPositionChanged;
    private bool m_PositionChangeRepaintDone;
    private bool m_IsExecuting;
    private UnityEditorInternal.EditMode.SceneViewEditMode m_ModeBeforePicking;

    public abstract void Repaint();

    protected abstract void RegisterUndo();

    protected abstract void Paint(Vector3Int position);

    protected abstract void Erase(Vector3Int position);

    protected abstract void BoxFill(BoundsInt position);

    protected abstract void BoxErase(BoundsInt position);

    protected abstract void FloodFill(Vector3Int position);

    protected abstract void PickBrush(BoundsInt position, Vector3Int pickStart);

    protected abstract void Select(BoundsInt position);

    protected abstract void Move(BoundsInt from, BoundsInt to);

    protected abstract void MoveStart(BoundsInt position);

    protected abstract void MoveEnd(BoundsInt position);

    protected abstract bool ValidateFloodFillPosition(Vector3Int position);

    protected abstract Vector2Int ScreenToGrid(Vector2 screenPosition);

    protected abstract bool PickingIsDefaultTool();

    protected abstract bool CanPickOutsideEditMode();

    protected abstract GridLayout.CellLayout CellLayout();

    protected abstract void ClearGridSelection();

    protected virtual void OnBrushPickStarted()
    {
    }

    protected virtual void OnBrushPickDragged(BoundsInt position)
    {
    }

    public Vector2Int mouseGridPosition
    {
      get
      {
        return this.m_MouseGridPosition;
      }
    }

    public bool isPicking
    {
      get
      {
        return this.m_MarqueeType == PaintableGrid.MarqueeType.Pick;
      }
    }

    public bool isBoxing
    {
      get
      {
        return this.m_MarqueeType == PaintableGrid.MarqueeType.Box;
      }
    }

    public GridLayout.CellLayout cellLayout
    {
      get
      {
        return this.CellLayout();
      }
    }

    protected bool executing
    {
      get
      {
        return this.m_IsExecuting;
      }
      set
      {
        this.m_IsExecuting = value && this.isHotControl;
      }
    }

    protected bool isHotControl
    {
      get
      {
        return GUIUtility.hotControl == this.m_PermanentControlID;
      }
    }

    protected bool mouseGridPositionChanged
    {
      get
      {
        return this.m_MouseGridPositionChanged;
      }
    }

    protected bool inEditMode
    {
      get
      {
        return PaintableGrid.InGridEditMode();
      }
    }

    protected virtual void OnEnable()
    {
      this.m_PermanentControlID = GUIUtility.GetPermanentControlID();
    }

    protected virtual void OnDisable()
    {
    }

    public virtual void OnGUI()
    {
      if (this.CanPickOutsideEditMode() || this.inEditMode)
        this.HandleBrushPicking();
      if (this.inEditMode)
      {
        this.HandleBrushPaintAndErase();
        this.HandleSelectTool();
        this.HandleMoveTool();
        this.HandleEditModeChange();
        this.HandleFloodFill();
        this.HandleBoxTool();
      }
      else if (this.isHotControl && !this.IsPickingEvent(Event.current))
        GUIUtility.hotControl = 0;
      if (!this.mouseGridPositionChanged || this.m_PositionChangeRepaintDone)
        return;
      this.Repaint();
      this.m_PositionChangeRepaintDone = true;
    }

    protected void UpdateMouseGridPosition()
    {
      if (Event.current.type != EventType.MouseDrag && Event.current.type != EventType.MouseMove && Event.current.type != EventType.DragUpdated)
        return;
      this.m_MouseGridPositionChanged = false;
      Vector2Int grid = this.ScreenToGrid(Event.current.mousePosition);
      if (grid != this.m_MouseGridPosition)
      {
        this.m_PreviousMouseGridPosition = this.m_MouseGridPosition;
        this.m_MouseGridPosition = grid;
        this.m_MouseGridPositionChanged = true;
        this.m_PositionChangeRepaintDone = false;
      }
    }

    private void HandleEditModeChange()
    {
      if (this.isPicking && UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridPicking)
      {
        this.m_MarqueeStart = new Vector2Int?();
        this.m_MarqueeType = PaintableGrid.MarqueeType.None;
        if (this.isHotControl)
        {
          GUI.changed = true;
          GUIUtility.hotControl = 0;
        }
      }
      if (this.isBoxing && UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridBox)
      {
        this.m_MarqueeStart = new Vector2Int?();
        this.m_MarqueeType = PaintableGrid.MarqueeType.None;
        if (this.isHotControl)
        {
          GUI.changed = true;
          GUIUtility.hotControl = 0;
        }
      }
      if (UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect || UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridMove)
        return;
      this.ClearGridSelection();
    }

    private void HandleBrushPicking()
    {
      Event current = Event.current;
      if (current.type == EventType.MouseDown && this.IsPickingEvent(current) && !this.isHotControl)
      {
        this.m_ModeBeforePicking = UnityEditorInternal.EditMode.SceneViewEditMode.GridPainting;
        if (this.inEditMode && UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridPicking)
        {
          this.m_ModeBeforePicking = UnityEditorInternal.EditMode.editMode;
          UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.GridPicking, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
        }
        this.m_MarqueeStart = new Vector2Int?(this.mouseGridPosition);
        this.m_MarqueeType = PaintableGrid.MarqueeType.Pick;
        PaintableGrid.s_LastActivePaintableGrid = this;
        Event.current.Use();
        GUI.changed = true;
        GUIUtility.hotControl = this.m_PermanentControlID;
        this.OnBrushPickStarted();
      }
      if (current.type == EventType.MouseDrag && this.isHotControl && (this.m_MarqueeStart.HasValue && this.m_MarqueeType == PaintableGrid.MarqueeType.Pick) && this.IsPickingEvent(current))
      {
        RectInt marqueeRect = GridEditorUtility.GetMarqueeRect(this.m_MarqueeStart.Value, this.mouseGridPosition);
        this.OnBrushPickDragged(new BoundsInt(new Vector3Int(marqueeRect.xMin, marqueeRect.yMin, 0), new Vector3Int(marqueeRect.size.x, marqueeRect.size.y, 1)));
        Event.current.Use();
        GUI.changed = true;
      }
      if (current.type != EventType.MouseUp || !this.m_MarqueeStart.HasValue || (this.m_MarqueeType != PaintableGrid.MarqueeType.Pick || !this.IsPickingEvent(current)))
        return;
      RectInt marqueeRect1 = GridEditorUtility.GetMarqueeRect(this.m_MarqueeStart.Value, this.mouseGridPosition);
      if (this.isHotControl)
      {
        Vector2Int marqueePivot = this.GetMarqueePivot(this.m_MarqueeStart.Value, this.mouseGridPosition);
        this.PickBrush(new BoundsInt(new Vector3Int(marqueeRect1.xMin, marqueeRect1.yMin, 0), new Vector3Int(marqueeRect1.size.x, marqueeRect1.size.y, 1)), new Vector3Int(marqueePivot.x, marqueePivot.y, 0));
        if (this.inEditMode && UnityEditorInternal.EditMode.editMode != this.m_ModeBeforePicking)
          UnityEditorInternal.EditMode.ChangeEditMode(this.m_ModeBeforePicking, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
        GridPaletteBrushes.ActiveGridBrushAssetChanged();
        PaintableGrid.s_LastActivePaintableGrid = this;
        InspectorWindow.RepaintAllInspectors();
        Event.current.Use();
        GUI.changed = true;
        GUIUtility.hotControl = 0;
      }
      this.m_MarqueeType = PaintableGrid.MarqueeType.None;
      this.m_MarqueeStart = new Vector2Int?();
    }

    private bool IsPickingEvent(Event evt)
    {
      return (evt.control && UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridMove || UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridPicking || UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect && this.PickingIsDefaultTool()) && evt.button == 0 && !evt.alt;
    }

    private void HandleSelectTool()
    {
      Event current = Event.current;
      if (current.type == EventType.MouseDown && current.button == 0 && !current.alt)
      {
        switch (UnityEditorInternal.EditMode.editMode)
        {
          case UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect:
            if (UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridMove && current.control)
              UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
            this.m_PreviousMove = new Vector2Int?();
            this.m_MarqueeStart = new Vector2Int?(this.mouseGridPosition);
            this.m_MarqueeType = PaintableGrid.MarqueeType.Select;
            PaintableGrid.s_LastActivePaintableGrid = this;
            GUIUtility.hotControl = this.m_PermanentControlID;
            Event.current.Use();
            break;
          case UnityEditorInternal.EditMode.SceneViewEditMode.GridMove:
            if (!current.control)
              break;
            goto case UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect;
        }
      }
      if (current.type == EventType.MouseUp && current.button == 0 && (!current.alt && this.m_MarqueeStart.HasValue) && (GUIUtility.hotControl == this.m_PermanentControlID && UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect))
      {
        if (this.m_MarqueeStart.HasValue && this.m_MarqueeType == PaintableGrid.MarqueeType.Select)
        {
          RectInt marqueeRect = GridEditorUtility.GetMarqueeRect(this.m_MarqueeStart.Value, this.mouseGridPosition);
          this.Select(new BoundsInt(new Vector3Int(marqueeRect.xMin, marqueeRect.yMin, 0), new Vector3Int(marqueeRect.size.x, marqueeRect.size.y, 1)));
          this.m_MarqueeStart = new Vector2Int?();
          this.m_MarqueeType = PaintableGrid.MarqueeType.None;
          InspectorWindow.RepaintAllInspectors();
        }
        if (current.control)
          UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.GridMove, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
        GUIUtility.hotControl = 0;
        Event.current.Use();
      }
      if (current.type != EventType.KeyDown || current.keyCode != KeyCode.Escape || (this.m_MarqueeStart.HasValue || this.m_PreviousMove.HasValue))
        return;
      this.ClearGridSelection();
      Event.current.Use();
    }

    private void HandleMoveTool()
    {
      Event current = Event.current;
      if (current.type == EventType.MouseDown && current.button == 0 && UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridMove)
      {
        this.RegisterUndo();
        if (GridSelection.active && GridSelection.position.Contains(new Vector3Int(this.mouseGridPosition.x, this.mouseGridPosition.y, GridSelection.position.zMin)))
        {
          GUIUtility.hotControl = this.m_PermanentControlID;
          this.executing = true;
          this.m_MarqueeStart = new Vector2Int?();
          this.m_MarqueeType = PaintableGrid.MarqueeType.None;
          this.m_PreviousMove = new Vector2Int?(this.mouseGridPosition);
          this.MoveStart(GridSelection.position);
          PaintableGrid.s_LastActivePaintableGrid = this;
        }
        Event.current.Use();
      }
      if (current.type == EventType.MouseDrag && current.button == 0 && (UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridMove && GUIUtility.hotControl == this.m_PermanentControlID) && (this.m_MouseGridPositionChanged && this.m_PreviousMove.HasValue))
      {
        this.executing = true;
        BoundsInt position1 = GridSelection.position;
        BoundsInt from = new BoundsInt(new Vector3Int(position1.xMin, position1.yMin, 0), new Vector3Int(position1.size.x, position1.size.y, 1));
        Vector2Int vector2Int = this.mouseGridPosition - this.m_PreviousMove.Value;
        BoundsInt position2 = GridSelection.position;
        position2.position = new Vector3Int(position2.x + vector2Int.x, position2.y + vector2Int.y, position2.z);
        GridSelection.position = position2;
        this.Move(from, position2);
        this.m_PreviousMove = new Vector2Int?(this.mouseGridPosition);
        Event.current.Use();
      }
      if (current.type != EventType.MouseUp || current.button != 0 || (!this.m_PreviousMove.HasValue || UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridMove) || GUIUtility.hotControl != this.m_PermanentControlID)
        return;
      if (this.m_PreviousMove.HasValue)
      {
        this.m_PreviousMove = new Vector2Int?();
        this.MoveEnd(GridSelection.position);
      }
      this.executing = false;
      GUIUtility.hotControl = 0;
      Event.current.Use();
    }

    private void HandleBrushPaintAndErase()
    {
      Event current = Event.current;
      if (!this.IsPaintingEvent(current) && !this.IsErasingEvent(current))
        return;
      switch (current.type)
      {
        case EventType.MouseDown:
          this.RegisterUndo();
          if (this.IsErasingEvent(current))
          {
            if (UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridEraser)
              UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.GridEraser, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
            this.Erase(new Vector3Int(this.mouseGridPosition.x, this.mouseGridPosition.y, 0));
          }
          else
          {
            if (UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridPainting)
              UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.GridPainting, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
            this.Paint(new Vector3Int(this.mouseGridPosition.x, this.mouseGridPosition.y, 0));
          }
          Event.current.Use();
          GUIUtility.hotControl = this.m_PermanentControlID;
          GUI.changed = true;
          this.executing = true;
          break;
        case EventType.MouseUp:
          this.executing = false;
          if (!this.isHotControl)
            break;
          if (Event.current.shift && UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridPainting)
            UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.GridPainting, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
          Event.current.Use();
          GUI.changed = true;
          GUIUtility.hotControl = 0;
          break;
        case EventType.MouseDrag:
          if (this.isHotControl && this.mouseGridPositionChanged)
          {
            List<Vector2Int> list = GridEditorUtility.GetPointsOnLine(this.m_PreviousMouseGridPosition, this.mouseGridPosition).ToList<Vector2Int>();
            if (list[0] == this.mouseGridPosition)
              list.Reverse();
            for (int index = 1; index < list.Count; ++index)
            {
              if (this.IsErasingEvent(current))
                this.Erase(new Vector3Int(list[index].x, list[index].y, 0));
              else
                this.Paint(new Vector3Int(list[index].x, list[index].y, 0));
            }
            Event.current.Use();
            GUI.changed = true;
          }
          this.executing = true;
          break;
      }
    }

    private bool IsPaintingEvent(Event evt)
    {
      return evt.button == 0 && !evt.control && !evt.alt && UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridPainting;
    }

    private bool IsErasingEvent(Event evt)
    {
      return evt.button == 0 && (!evt.control && !evt.alt && (evt.shift && UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridBox) && (UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridFloodFill && UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect && UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridMove) || UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridEraser);
    }

    private void HandleFloodFill()
    {
      if (UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridFloodFill || !((UnityEngine.Object) GridPaintingState.gridBrush != (UnityEngine.Object) null) || !this.ValidateFloodFillPosition(new Vector3Int(this.mouseGridPosition.x, this.mouseGridPosition.y, 0)))
        return;
      Event current = Event.current;
      if (current.type == EventType.MouseDown && current.button == 0)
      {
        GUIUtility.hotControl = this.m_PermanentControlID;
        GUI.changed = true;
        this.executing = true;
        Event.current.Use();
      }
      if (current.type == EventType.MouseUp && current.button == 0 && this.isHotControl)
      {
        this.executing = false;
        this.RegisterUndo();
        this.FloodFill(new Vector3Int(this.mouseGridPosition.x, this.mouseGridPosition.y, 0));
        GUI.changed = true;
        Event.current.Use();
        GUIUtility.hotControl = 0;
      }
    }

    private void HandleBoxTool()
    {
      Event current = Event.current;
      if (current.type == EventType.MouseDown && current.button == 0 && UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridBox)
      {
        this.m_MarqueeStart = new Vector2Int?(this.mouseGridPosition);
        this.m_MarqueeType = PaintableGrid.MarqueeType.Box;
        Event.current.Use();
        GUI.changed = true;
        this.executing = true;
        GUIUtility.hotControl = this.m_PermanentControlID;
      }
      if (current.type == EventType.MouseDrag && current.button == 0 && (UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridBox && this.isHotControl) && this.m_MarqueeStart.HasValue)
      {
        Event.current.Use();
        this.executing = true;
        GUI.changed = true;
      }
      if (current.type != EventType.MouseUp || current.button != 0 || UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridBox)
        return;
      if (this.isHotControl && this.m_MarqueeStart.HasValue)
      {
        this.RegisterUndo();
        RectInt marqueeRect = GridEditorUtility.GetMarqueeRect(this.m_MarqueeStart.Value, this.mouseGridPosition);
        if (current.shift)
          this.BoxErase(new BoundsInt(marqueeRect.x, marqueeRect.y, 0, marqueeRect.size.x, marqueeRect.size.y, 1));
        else
          this.BoxFill(new BoundsInt(marqueeRect.x, marqueeRect.y, 0, marqueeRect.size.x, marqueeRect.size.y, 1));
        Event.current.Use();
        this.executing = false;
        GUI.changed = true;
        GUIUtility.hotControl = 0;
      }
      this.m_MarqueeStart = new Vector2Int?();
      this.m_MarqueeType = PaintableGrid.MarqueeType.None;
    }

    private Vector2Int GetMarqueePivot(Vector2Int start, Vector2Int end)
    {
      return new Vector2Int(Math.Max(end.x - start.x, 0), Math.Max(end.y - start.y, 0));
    }

    public static bool InGridEditMode()
    {
      return UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridBox || UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridEraser || (UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridFloodFill || UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridPainting) || (UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridPicking || UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect) || UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.GridMove;
    }

    public static GridBrushBase.Tool EditModeToBrushTool(UnityEditorInternal.EditMode.SceneViewEditMode editMode)
    {
      switch (editMode)
      {
        case UnityEditorInternal.EditMode.SceneViewEditMode.GridPainting:
          return GridBrushBase.Tool.Paint;
        case UnityEditorInternal.EditMode.SceneViewEditMode.GridPicking:
          return GridBrushBase.Tool.Pick;
        case UnityEditorInternal.EditMode.SceneViewEditMode.GridEraser:
          return GridBrushBase.Tool.Erase;
        case UnityEditorInternal.EditMode.SceneViewEditMode.GridFloodFill:
          return GridBrushBase.Tool.FloodFill;
        case UnityEditorInternal.EditMode.SceneViewEditMode.GridBox:
          return GridBrushBase.Tool.Box;
        case UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect:
          return GridBrushBase.Tool.Select;
        case UnityEditorInternal.EditMode.SceneViewEditMode.GridMove:
          return GridBrushBase.Tool.Move;
        default:
          return GridBrushBase.Tool.Paint;
      }
    }

    public static UnityEditorInternal.EditMode.SceneViewEditMode BrushToolToEditMode(GridBrushBase.Tool tool)
    {
      switch (tool)
      {
        case GridBrushBase.Tool.Select:
          return UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect;
        case GridBrushBase.Tool.Move:
          return UnityEditorInternal.EditMode.SceneViewEditMode.GridMove;
        case GridBrushBase.Tool.Paint:
          return UnityEditorInternal.EditMode.SceneViewEditMode.GridPainting;
        case GridBrushBase.Tool.Box:
          return UnityEditorInternal.EditMode.SceneViewEditMode.GridBox;
        case GridBrushBase.Tool.Pick:
          return UnityEditorInternal.EditMode.SceneViewEditMode.GridPicking;
        case GridBrushBase.Tool.Erase:
          return UnityEditorInternal.EditMode.SceneViewEditMode.GridEraser;
        case GridBrushBase.Tool.FloodFill:
          return UnityEditorInternal.EditMode.SceneViewEditMode.GridFloodFill;
        default:
          return UnityEditorInternal.EditMode.SceneViewEditMode.GridPainting;
      }
    }

    public enum MarqueeType
    {
      None,
      Pick,
      Box,
      Select,
    }
  }
}
