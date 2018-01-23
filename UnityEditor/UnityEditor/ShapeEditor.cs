// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShapeEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D.Interface;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor
{
  internal class ShapeEditor
  {
    private static readonly Color[] k_OutlineColor = new Color[4]{ Color.gray, Color.white, new Color(0.1333333f, 0.6705883f, 1f), Color.white };
    private static readonly Color[] k_FillColor = new Color[4]{ Color.white, new Color(0.5137255f, 0.8627451f, 1f), new Color(0.1333333f, 0.6705883f, 1f), new Color(0.1333333f, 0.6705883f, 1f) };
    private static readonly Color k_TangentColor = new Color(0.1333333f, 0.6705883f, 1f);
    private static readonly Color k_TangentColorAlternative = new Color(0.5137255f, 0.8627451f, 1f);
    public Func<int, Vector3> GetPointPosition = (Func<int, Vector3>) (i => Vector3.zero);
    public Action<int, Vector3> SetPointPosition = (Action<int, Vector3>) ((i, p) => {});
    public Func<int, Vector3> GetPointLTangent = (Func<int, Vector3>) (i => Vector3.zero);
    public Action<int, Vector3> SetPointLTangent = (Action<int, Vector3>) ((i, p) => {});
    public Func<int, Vector3> GetPointRTangent = (Func<int, Vector3>) (i => Vector3.zero);
    public Action<int, Vector3> SetPointRTangent = (Action<int, Vector3>) ((i, p) => {});
    public Func<int, ShapeEditor.TangentMode> GetTangentMode = (Func<int, ShapeEditor.TangentMode>) (i => ShapeEditor.TangentMode.Linear);
    public Action<int, ShapeEditor.TangentMode> SetTangentMode = (Action<int, ShapeEditor.TangentMode>) ((i, m) => {});
    public Action<int, Vector3> InsertPointAt = (Action<int, Vector3>) ((i, p) => {});
    public Action<int> RemovePointAt = (Action<int>) (i => {});
    public Func<int> GetPointsCount = (Func<int>) (() => 0);
    public Func<Vector2, Vector3> ScreenToLocal = (Func<Vector2, Vector3>) (i => (Vector3) i);
    public Func<Vector3, Vector2> LocalToScreen = (Func<Vector3, Vector2>) (i => (Vector2) i);
    public Func<Matrix4x4> LocalToWorldMatrix = (Func<Matrix4x4>) (() => Matrix4x4.identity);
    public Func<ShapeEditor.DistanceToControl> DistanceToRectangle = (Func<ShapeEditor.DistanceToControl>) (() =>
    {
      if (ShapeEditor.\u003C\u003Ef__mg\u0024cache0 == null)
        ShapeEditor.\u003C\u003Ef__mg\u0024cache0 = new ShapeEditor.DistanceToControl(HandleUtility.DistanceToRectangle);
      return ShapeEditor.\u003C\u003Ef__mg\u0024cache0;
    });
    public Func<ShapeEditor.DistanceToControl> DistanceToDiamond = (Func<ShapeEditor.DistanceToControl>) (() =>
    {
      if (ShapeEditor.\u003C\u003Ef__mg\u0024cache1 == null)
        ShapeEditor.\u003C\u003Ef__mg\u0024cache1 = new ShapeEditor.DistanceToControl(HandleUtility.DistanceToDiamond);
      return ShapeEditor.\u003C\u003Ef__mg\u0024cache1;
    });
    public Func<ShapeEditor.DistanceToControl> DistanceToCircle = (Func<ShapeEditor.DistanceToControl>) (() =>
    {
      if (ShapeEditor.\u003C\u003Ef__mg\u0024cache2 == null)
        ShapeEditor.\u003C\u003Ef__mg\u0024cache2 = new ShapeEditor.DistanceToControl(ShapeEditor.DistanceToCircleInternal);
      return ShapeEditor.\u003C\u003Ef__mg\u0024cache2;
    });
    public Action Repaint = (Action) (() => {});
    public Action RecordUndo = (Action) (() => {});
    public Func<Vector3, Vector3> Snap = (Func<Vector3, Vector3>) (i => i);
    public Action<Bounds> Frame = (Action<Bounds>) (b => {});
    public Action<int> OnPointClick = (Action<int>) (i => {});
    public Func<bool> OpenEnded = (Func<bool>) (() => false);
    public Func<float> GetHandleSize = (Func<float>) (() => 5f);
    private int m_ActivePointOnLastMouseDown = -1;
    private int m_NewPointIndex = -1;
    private int m_ActiveEdge = -1;
    private bool m_DelayedReset = false;
    private HashSet<ShapeEditor> m_ShapeEditorListeners = new HashSet<ShapeEditor>();
    private int m_MouseClosestEdge = -1;
    private float m_MouseClosestEdgeDist = float.MaxValue;
    private int m_ShapeEditorRegisteredTo = 0;
    private int m_ShapeEditorUpdateDone = 0;
    private ShapeEditorSelection m_Selection;
    private Vector2 m_MousePositionLastMouseDown;
    private Vector3 m_EdgeDragStartMousePosition;
    private Vector3 m_EdgeDragStartP0;
    private Vector3 m_EdgeDragStartP1;
    private bool m_NewPointDragFinished;
    private ShapeEditorRectSelectionTool m_RectSelectionTool;
    private Dictionary<ShapeEditor.DrawBatchDataKey, List<Vector3>> m_DrawBatch;
    private Vector3[][] m_EdgePoints;
    private const float k_EdgeHoverDistance = 9f;
    private const float k_EdgeWidth = 2f;
    private const float k_ActiveEdgeWidth = 6f;
    private const float k_MinExistingPointDistanceForInsert = 20f;
    private readonly int k_CreatorID;
    private readonly int k_EdgeID;
    private readonly int k_RightTangentID;
    private readonly int k_LeftTangentID;
    private const int k_BezierPatch = 40;

    public ShapeEditor(IGUIUtility gu, IEventSystem es)
    {
      this.m_Selection = new ShapeEditorSelection(this);
      this.guiUtility = gu;
      this.eventSystem = es;
      this.k_CreatorID = this.guiUtility.GetPermanentControlID();
      this.k_EdgeID = this.guiUtility.GetPermanentControlID();
      this.k_RightTangentID = this.guiUtility.GetPermanentControlID();
      this.k_LeftTangentID = this.guiUtility.GetPermanentControlID();
      this.glSystem = GLSystem.GetSystem();
      this.handles = HandlesSystem.GetSystem();
    }

    public ITexture2D lineTexture { get; set; }

    public int activePoint { get; set; }

    public HashSet<int> selectedPoints
    {
      get
      {
        return this.m_Selection.indices;
      }
    }

    public bool inEditMode { get; set; }

    public int activeEdge
    {
      get
      {
        return this.m_ActiveEdge;
      }
      set
      {
        this.m_ActiveEdge = value;
      }
    }

    public bool delayedReset
    {
      set
      {
        this.m_DelayedReset = value;
      }
    }

    private static Color handleOutlineColor { get; set; }

    private static Color handleFillColor { get; set; }

    private Quaternion handleMatrixrotation
    {
      get
      {
        return Quaternion.LookRotation((Vector3) this.handles.matrix.GetColumn(2), (Vector3) this.handles.matrix.GetColumn(1));
      }
    }

    private IGUIUtility guiUtility { get; set; }

    private IEventSystem eventSystem { get; set; }

    private IEvent currentEvent { get; set; }

    private IGL glSystem { get; set; }

    private IHandles handles { get; set; }

    public void SetRectSelectionTool(ShapeEditorRectSelectionTool sers)
    {
      if (this.m_RectSelectionTool != null)
      {
        this.m_RectSelectionTool.RectSelect -= new Action<Rect, ShapeEditor.SelectionType>(this.SelectPointsInRect);
        this.m_RectSelectionTool.ClearSelection -= new Action(this.ClearSelectedPoints);
      }
      this.m_RectSelectionTool = sers;
      this.m_RectSelectionTool.RectSelect += new Action<Rect, ShapeEditor.SelectionType>(this.SelectPointsInRect);
      this.m_RectSelectionTool.ClearSelection += new Action(this.ClearSelectedPoints);
    }

    public void OnDisable()
    {
      this.m_RectSelectionTool.RectSelect -= new Action<Rect, ShapeEditor.SelectionType>(this.SelectPointsInRect);
      this.m_RectSelectionTool.ClearSelection -= new Action(this.ClearSelectedPoints);
      this.m_RectSelectionTool = (ShapeEditorRectSelectionTool) null;
    }

    private void PrepareDrawBatch()
    {
      if (this.currentEvent.type != EventType.Repaint)
        return;
      this.m_DrawBatch = new Dictionary<ShapeEditor.DrawBatchDataKey, List<Vector3>>();
    }

    private void DrawBatch()
    {
      if (this.currentEvent.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      this.glSystem.PushMatrix();
      this.glSystem.MultMatrix(this.handles.matrix);
      foreach (KeyValuePair<ShapeEditor.DrawBatchDataKey, List<Vector3>> keyValuePair in this.m_DrawBatch)
      {
        this.glSystem.Begin(keyValuePair.Key.glMode);
        this.glSystem.Color(keyValuePair.Key.color);
        foreach (Vector3 v in keyValuePair.Value)
          this.glSystem.Vertex(v);
        this.glSystem.End();
      }
      this.glSystem.PopMatrix();
    }

    private List<Vector3> GetDrawBatchList(ShapeEditor.DrawBatchDataKey key)
    {
      if (!this.m_DrawBatch.ContainsKey(key))
        this.m_DrawBatch.Add(key, new List<Vector3>());
      return this.m_DrawBatch[key];
    }

    public void OnGUI()
    {
      this.DelayedResetIfNecessary();
      this.currentEvent = this.eventSystem.current;
      if (this.currentEvent.type == EventType.MouseDown)
        this.StoreMouseDownState();
      Color color = this.handles.color;
      Matrix4x4 matrix = this.handles.matrix;
      this.handles.matrix = this.LocalToWorldMatrix();
      this.PrepareDrawBatch();
      this.Edges();
      if (this.inEditMode)
      {
        this.Framing();
        this.Tangents();
        this.Points();
      }
      this.DrawBatch();
      this.handles.color = color;
      this.handles.matrix = matrix;
      this.OnShapeEditorUpdateDone();
      foreach (ShapeEditor shapeEditorListener in this.m_ShapeEditorListeners)
        shapeEditorListener.OnShapeEditorUpdateDone();
    }

    private void Framing()
    {
      if (!(this.currentEvent.commandName == "FrameSelected") || this.m_Selection.Count <= 0)
        return;
      switch (this.currentEvent.type)
      {
        case EventType.ValidateCommand:
          this.currentEvent.Use();
          break;
        case EventType.ExecuteCommand:
          Bounds bounds = new Bounds(this.GetPointPosition(this.selectedPoints.First<int>()), Vector3.zero);
          foreach (int selectedPoint in this.selectedPoints)
            bounds.Encapsulate(this.GetPointPosition(selectedPoint));
          this.Frame(bounds);
          goto case EventType.ValidateCommand;
      }
    }

    private void PrepareEdgePointList()
    {
      if (this.m_EdgePoints != null)
        return;
      int m1 = this.GetPointsCount();
      int m2 = !this.OpenEnded() ? m1 : m1 - 1;
      this.m_EdgePoints = new Vector3[m2][];
      int index1 = ShapeEditor.mod(m1 - 1, m2);
      for (int index2 = ShapeEditor.mod(index1 + 1, m1); index2 < m1; ++index2)
      {
        Vector3 startPosition = this.GetPointPosition(index1);
        Vector3 endPosition = this.GetPointPosition(index2);
        if (this.GetTangentMode(index1) == ShapeEditor.TangentMode.Linear && this.GetTangentMode(index2) == ShapeEditor.TangentMode.Linear)
        {
          this.m_EdgePoints[index1] = new Vector3[2]
          {
            startPosition,
            endPosition
          };
        }
        else
        {
          Vector3 startTangent = this.GetPointRTangent(index1) + startPosition;
          Vector3 endTangent = this.GetPointLTangent(index2) + endPosition;
          this.m_EdgePoints[index1] = this.handles.MakeBezierPoints(startPosition, endPosition, startTangent, endTangent, 40);
        }
        index1 = index2;
      }
    }

    private float DistancePointEdge(Vector3 point, Vector3[] edge)
    {
      float num1 = float.MaxValue;
      int index1 = edge.Length - 1;
      for (int index2 = 0; index2 < edge.Length; ++index2)
      {
        float num2 = HandleUtility.DistancePointLine(point, edge[index1], edge[index2]);
        if ((double) num2 < (double) num1)
          num1 = num2;
        index1 = index2;
      }
      return num1;
    }

    private float GetMouseClosestEdgeDistance()
    {
      Vector3 point = this.ScreenToLocal(this.eventSystem.current.mousePosition);
      int num1 = this.GetPointsCount();
      if (this.m_MouseClosestEdge == -1 && num1 > 0)
      {
        this.PrepareEdgePointList();
        this.m_MouseClosestEdgeDist = float.MaxValue;
        int num2 = !this.OpenEnded() ? num1 : num1 - 1;
        for (int index = 0; index < num2; ++index)
        {
          float num3 = this.DistancePointEdge(point, this.m_EdgePoints[index]);
          if ((double) num3 < (double) this.m_MouseClosestEdgeDist)
          {
            this.m_MouseClosestEdge = index;
            this.m_MouseClosestEdgeDist = num3;
          }
        }
      }
      if (this.guiUtility.hotControl == this.k_CreatorID || this.guiUtility.hotControl == this.k_EdgeID)
        return float.MinValue;
      return this.m_MouseClosestEdgeDist;
    }

    public void Edges()
    {
      float num1 = float.MaxValue;
      if (this.m_ShapeEditorListeners.Count > 0)
        num1 = this.m_ShapeEditorListeners.Select<ShapeEditor, float>((Func<ShapeEditor, float>) (se => se.GetMouseClosestEdgeDistance())).Max();
      float closestEdgeDistance = this.GetMouseClosestEdgeDistance();
      bool flag1 = this.EdgeDragModifiersActive() && (double) closestEdgeDistance < 9.0 && (double) closestEdgeDistance < (double) num1;
      if (this.currentEvent.type == EventType.Repaint)
      {
        Color color1 = this.handles.color;
        this.PrepareEdgePointList();
        int num2 = this.GetPointsCount();
        int num3 = !this.OpenEnded() ? num2 : num2 - 1;
        for (int index = 0; index < num3; ++index)
        {
          Color color2 = index != this.m_ActiveEdge ? Color.white : Color.yellow;
          float width = index == this.m_ActiveEdge || this.m_MouseClosestEdge == index && flag1 ? 6f : 2f;
          this.handles.color = color2;
          this.handles.DrawAAPolyLine(this.lineTexture, width, this.m_EdgePoints[index]);
        }
        this.handles.color = color1;
      }
      if (this.inEditMode && (double) num1 > (double) closestEdgeDistance)
      {
        bool flag2 = (double) this.m_MouseClosestEdgeDist < 9.0 && ((double) this.MouseDistanceToPoint(this.FindClosestPointToMouse()) > 20.0 && (double) this.MouseDistanceToClosestTangent() > 20.0) && !this.m_RectSelectionTool.isSelecting;
        if (GUIUtility.hotControl == this.k_EdgeID || this.EdgeDragModifiersActive() && flag2)
          this.HandleEdgeDragging(this.m_MouseClosestEdge);
        else if (GUIUtility.hotControl == this.k_CreatorID || this.currentEvent.modifiers == EventModifiers.None && flag2)
          this.HandlePointInsertToEdge(this.m_MouseClosestEdge, this.m_MouseClosestEdgeDist);
      }
      if (this.guiUtility.hotControl != this.k_CreatorID && this.m_NewPointIndex != -1)
      {
        this.m_NewPointDragFinished = true;
        this.guiUtility.keyboardControl = 0;
        this.m_NewPointIndex = -1;
      }
      if (this.guiUtility.hotControl == this.k_EdgeID || this.m_ActiveEdge == -1)
        return;
      this.m_ActiveEdge = -1;
    }

    public void Tangents()
    {
      if (this.activePoint < 0 || this.m_Selection.Count > 1 || this.GetTangentMode(this.activePoint) == ShapeEditor.TangentMode.Linear)
        return;
      IEvent current = this.eventSystem.current;
      Vector3 p0 = this.GetPointPosition(this.activePoint);
      Vector3 vector3_1 = this.GetPointLTangent(this.activePoint);
      Vector3 vector3_2 = this.GetPointRTangent(this.activePoint);
      bool flag1 = this.guiUtility.hotControl == this.k_RightTangentID || this.guiUtility.hotControl == this.k_LeftTangentID;
      bool flag2 = (double) vector3_1.sqrMagnitude == 0.0 && (double) vector3_2.sqrMagnitude == 0.0;
      if (!flag1 && flag2)
        return;
      ShapeEditor.TangentMode tangentMode = this.GetTangentMode(this.activePoint);
      bool flag3 = current.GetTypeForControl(this.k_RightTangentID) == EventType.MouseDown || current.GetTypeForControl(this.k_LeftTangentID) == EventType.MouseDown;
      bool flag4 = current.GetTypeForControl(this.k_RightTangentID) == EventType.MouseUp || current.GetTypeForControl(this.k_LeftTangentID) == EventType.MouseUp;
      Vector3 vector3_3 = this.DoTangent(p0, p0 + vector3_1, this.k_LeftTangentID, this.activePoint, ShapeEditor.k_TangentColor);
      Vector3 vector3_4 = this.DoTangent(p0, p0 + vector3_2, this.k_RightTangentID, this.activePoint, this.GetTangentMode(this.activePoint) != ShapeEditor.TangentMode.Broken ? ShapeEditor.k_TangentColor : ShapeEditor.k_TangentColorAlternative);
      bool flag5 = vector3_3 != vector3_1 || vector3_4 != vector3_2;
      bool flag6 = (double) vector3_3.sqrMagnitude == 0.0 && (double) vector3_4.sqrMagnitude == 0.0;
      if (flag1 && flag3)
        this.SetTangentMode(this.activePoint, (ShapeEditor.TangentMode) ((int) (tangentMode + 1) % 3));
      if (flag4 && flag6)
      {
        this.SetTangentMode(this.activePoint, ShapeEditor.TangentMode.Linear);
        flag5 = true;
      }
      if (flag5)
      {
        this.RecordUndo();
        this.SetPointLTangent(this.activePoint, vector3_3);
        this.SetPointRTangent(this.activePoint, vector3_4);
        this.RefreshTangents(this.activePoint, this.guiUtility.hotControl == this.k_RightTangentID);
        this.Repaint();
      }
    }

    public void Points()
    {
      bool flag1 = (UnityEngine.Event.current.type == EventType.ExecuteCommand || UnityEngine.Event.current.type == EventType.ValidateCommand) && (UnityEngine.Event.current.commandName == "SoftDelete" || UnityEngine.Event.current.commandName == "Delete");
      for (int index = 0; index < this.GetPointsCount(); ++index)
      {
        if (index != this.m_NewPointIndex)
        {
          Vector3 position = this.GetPointPosition(index);
          int controlId = this.guiUtility.GetControlID(5353, FocusType.Keyboard);
          bool flag2 = this.currentEvent.GetTypeForControl(controlId) == EventType.MouseDown;
          bool flag3 = this.currentEvent.GetTypeForControl(controlId) == EventType.MouseUp;
          EditorGUI.BeginChangeCheck();
          if (this.currentEvent.type == EventType.Repaint)
          {
            ShapeEditor.ColorEnum colorForPoint = this.GetColorForPoint(index, controlId);
            ShapeEditor.handleOutlineColor = ShapeEditor.k_OutlineColor[(int) colorForPoint];
            ShapeEditor.handleFillColor = ShapeEditor.k_FillColor[(int) colorForPoint];
          }
          Vector3 vector3 = position;
          int hotControl1 = this.guiUtility.hotControl;
          if (!this.currentEvent.alt || this.guiUtility.hotControl == controlId)
            vector3 = ShapeEditor.DoSlider(controlId, position, Vector3.up, Vector3.right, this.GetHandleSizeForPoint(index), this.GetCapForPoint(index));
          else if (this.currentEvent.type == EventType.Repaint)
            this.GetCapForPoint(index)(controlId, position, Quaternion.LookRotation(Vector3.forward, Vector3.up), this.GetHandleSizeForPoint(index), this.currentEvent.type);
          int hotControl2 = this.guiUtility.hotControl;
          if (flag3 && hotControl1 == controlId && (hotControl2 == 0 && this.currentEvent.mousePosition == this.m_MousePositionLastMouseDown) && !this.currentEvent.shift)
            this.HandlePointClick(index);
          if (EditorGUI.EndChangeCheck())
          {
            this.RecordUndo();
            this.MoveSelections((Vector2) (this.Snap(vector3) - position));
          }
          if (this.guiUtility.hotControl == controlId && flag2 && !this.m_Selection.Contains(index))
          {
            this.SelectPoint(index, !this.currentEvent.shift ? ShapeEditor.SelectionType.Normal : ShapeEditor.SelectionType.Additive);
            this.Repaint();
          }
          if (this.m_NewPointDragFinished && this.activePoint == index && controlId != -1)
          {
            this.guiUtility.keyboardControl = controlId;
            this.m_NewPointDragFinished = false;
          }
        }
      }
      if (!flag1)
        return;
      if (this.currentEvent.type == EventType.ValidateCommand)
        this.currentEvent.Use();
      else if (this.currentEvent.type == EventType.ExecuteCommand)
      {
        this.RecordUndo();
        this.DeleteSelections();
        this.currentEvent.Use();
      }
    }

    public void HandlePointInsertToEdge(int closestEdge, float closestEdgeDist)
    {
      bool flag = GUIUtility.hotControl == this.k_CreatorID;
      Vector3 position = !flag ? this.FindClosestPointOnEdge(closestEdge, this.ScreenToLocal(this.currentEvent.mousePosition), 100) : this.GetPointPosition(this.m_NewPointIndex);
      EditorGUI.BeginChangeCheck();
      ShapeEditor.handleFillColor = ShapeEditor.k_FillColor[3];
      ShapeEditor.handleOutlineColor = ShapeEditor.k_OutlineColor[3];
      if (!flag)
      {
        ShapeEditor.handleFillColor = ShapeEditor.handleFillColor.AlphaMultiplied(0.5f);
        ShapeEditor.handleOutlineColor = ShapeEditor.handleOutlineColor.AlphaMultiplied(0.5f);
      }
      int hotControl = GUIUtility.hotControl;
      Vector3 vector3 = ShapeEditor.DoSlider(this.k_CreatorID, position, Vector3.up, Vector3.right, this.GetHandleSizeForPoint(closestEdge), new Handles.CapFunction(this.RectCap));
      if (hotControl != this.k_CreatorID && GUIUtility.hotControl == this.k_CreatorID)
      {
        this.RecordUndo();
        this.m_NewPointIndex = ShapeEditor.NextIndex(closestEdge, this.GetPointsCount());
        this.InsertPointAt(this.m_NewPointIndex, vector3);
        this.SelectPoint(this.m_NewPointIndex, ShapeEditor.SelectionType.Normal);
      }
      else
      {
        if (!EditorGUI.EndChangeCheck())
          return;
        this.RecordUndo();
        this.MoveSelections((Vector2) (this.Snap(vector3) - position));
      }
    }

    private void HandleEdgeDragging(int closestEdge)
    {
      switch (this.currentEvent.type)
      {
        case EventType.MouseDown:
          this.m_ActiveEdge = closestEdge;
          this.m_EdgeDragStartP0 = this.GetPointPosition(this.m_ActiveEdge);
          this.m_EdgeDragStartP1 = this.GetPointPosition(ShapeEditor.NextIndex(this.m_ActiveEdge, this.GetPointsCount()));
          if (this.currentEvent.shift)
          {
            this.RecordUndo();
            this.InsertPointAt(this.m_ActiveEdge + 1, this.m_EdgeDragStartP0);
            this.InsertPointAt(this.m_ActiveEdge + 2, this.m_EdgeDragStartP1);
            ++this.m_ActiveEdge;
          }
          this.m_EdgeDragStartMousePosition = this.ScreenToLocal(this.currentEvent.mousePosition);
          GUIUtility.hotControl = this.k_EdgeID;
          this.currentEvent.Use();
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != this.k_EdgeID)
            break;
          this.m_ActiveEdge = -1;
          GUIUtility.hotControl = 0;
          this.currentEvent.Use();
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != this.k_EdgeID)
            break;
          this.RecordUndo();
          Vector3 vector3 = this.Snap(this.m_EdgeDragStartP0 + (this.ScreenToLocal(this.currentEvent.mousePosition) - this.m_EdgeDragStartMousePosition)) - this.GetPointPosition(this.m_ActiveEdge);
          int activeEdge = this.m_ActiveEdge;
          int num = ShapeEditor.NextIndex(this.m_ActiveEdge, this.GetPointsCount());
          this.SetPointPosition(this.m_ActiveEdge, this.GetPointPosition(activeEdge) + vector3);
          this.SetPointPosition(num, this.GetPointPosition(num) + vector3);
          this.currentEvent.Use();
          break;
      }
    }

    private Vector3 DoTangent(Vector3 p0, Vector3 t0, int cid, int pointIndex, Color color)
    {
      float handleSizeForPoint = this.GetHandleSizeForPoint(pointIndex);
      float tangentSizeForPoint = this.GetTangentSizeForPoint(pointIndex);
      this.handles.color = color;
      float circle = HandleUtility.DistanceToCircle(t0, tangentSizeForPoint);
      if (this.lineTexture != (ITexture2D) null)
        this.handles.DrawAAPolyLine(this.lineTexture, new Vector3[2]
        {
          p0,
          t0
        });
      else
        this.handles.DrawLine(p0, t0);
      ShapeEditor.handleOutlineColor = (double) circle <= 0.0 ? ShapeEditor.k_OutlineColor[3] : color;
      ShapeEditor.handleFillColor = color;
      Vector3 vector3 = ShapeEditor.DoSlider(cid, t0, Vector3.up, Vector3.right, tangentSizeForPoint, this.GetCapForTangent(pointIndex)) - p0;
      return (double) vector3.magnitude >= (double) handleSizeForPoint ? vector3 : Vector3.zero;
    }

    public void HandlePointClick(int pointIndex)
    {
      if (this.m_Selection.Count > 1)
      {
        this.m_Selection.SelectPoint(pointIndex, ShapeEditor.SelectionType.Normal);
      }
      else
      {
        if (this.currentEvent.control || this.currentEvent.shift || this.m_ActivePointOnLastMouseDown != this.activePoint)
          return;
        this.OnPointClick(pointIndex);
      }
    }

    public void CycleTangentMode()
    {
      ShapeEditor.TangentMode tangentMode = this.GetTangentMode(this.activePoint);
      ShapeEditor.TangentMode nextTangentMode = ShapeEditor.GetNextTangentMode(tangentMode);
      this.SetTangentMode(this.activePoint, nextTangentMode);
      this.RefreshTangentsAfterModeChange(this.activePoint, tangentMode, nextTangentMode);
    }

    public static ShapeEditor.TangentMode GetNextTangentMode(ShapeEditor.TangentMode current)
    {
      return (ShapeEditor.TangentMode) ((int) (current + 1) % Enum.GetValues(typeof (ShapeEditor.TangentMode)).Length);
    }

    public void RefreshTangentsAfterModeChange(int pointIndex, ShapeEditor.TangentMode oldMode, ShapeEditor.TangentMode newMode)
    {
      if (oldMode != ShapeEditor.TangentMode.Linear && newMode == ShapeEditor.TangentMode.Linear)
      {
        this.SetPointLTangent(pointIndex, Vector3.zero);
        this.SetPointRTangent(pointIndex, Vector3.zero);
      }
      if (newMode != ShapeEditor.TangentMode.Continuous)
        return;
      if (oldMode == ShapeEditor.TangentMode.Broken)
        this.SetPointRTangent(pointIndex, this.GetPointLTangent(pointIndex) * -1f);
      if (oldMode == ShapeEditor.TangentMode.Linear)
        this.FromAllZeroToTangents(pointIndex);
    }

    private ShapeEditor.ColorEnum GetColorForPoint(int pointIndex, int handleID)
    {
      bool flag1 = (double) this.MouseDistanceToPoint(pointIndex) <= 0.0;
      bool flag2 = this.m_Selection.Contains(pointIndex);
      if (flag1 && flag2 || GUIUtility.hotControl == handleID)
        return ShapeEditor.ColorEnum.ESelectedHovered;
      if (flag1)
        return ShapeEditor.ColorEnum.EUnselectedHovered;
      return flag2 ? ShapeEditor.ColorEnum.ESelected : ShapeEditor.ColorEnum.EUnselected;
    }

    private void FromAllZeroToTangents(int pointIndex)
    {
      Vector3 vector3_1 = this.GetPointPosition(pointIndex);
      Vector3 vector3_2 = (this.GetPointPosition(pointIndex <= 0 ? this.GetPointsCount() - 1 : pointIndex - 1) - vector3_1) * 0.33f;
      Vector3 vector3_3 = -vector3_2;
      float magnitude1 = (this.LocalToScreen(vector3_1) - this.LocalToScreen(vector3_1 + vector3_2)).magnitude;
      float magnitude2 = (this.LocalToScreen(vector3_1) - this.LocalToScreen(vector3_1 + vector3_3)).magnitude;
      Vector3 vector3_4 = vector3_2 * Mathf.Min(100f / magnitude1, 1f);
      Vector3 vector3_5 = vector3_3 * Mathf.Min(100f / magnitude2, 1f);
      this.SetPointLTangent(pointIndex, vector3_4);
      this.SetPointRTangent(pointIndex, vector3_5);
    }

    private Handles.CapFunction GetCapForTangent(int index)
    {
      if (this.GetTangentMode(index) == ShapeEditor.TangentMode.Continuous)
        return new Handles.CapFunction(this.CircleCap);
      return new Handles.CapFunction(this.DiamondCap);
    }

    private ShapeEditor.DistanceToControl GetDistanceFuncForTangent(int index)
    {
      if (this.GetTangentMode(index) == ShapeEditor.TangentMode.Continuous)
        return this.DistanceToCircle();
      // ISSUE: reference to a compiler-generated field
      if (ShapeEditor.\u003C\u003Ef__mg\u0024cache3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShapeEditor.\u003C\u003Ef__mg\u0024cache3 = new ShapeEditor.DistanceToControl(HandleUtility.DistanceToDiamond);
      }
      // ISSUE: reference to a compiler-generated field
      return ShapeEditor.\u003C\u003Ef__mg\u0024cache3;
    }

    private Handles.CapFunction GetCapForPoint(int index)
    {
      switch (this.GetTangentMode(index))
      {
        case ShapeEditor.TangentMode.Linear:
          return new Handles.CapFunction(this.RectCap);
        case ShapeEditor.TangentMode.Continuous:
          return new Handles.CapFunction(this.CircleCap);
        case ShapeEditor.TangentMode.Broken:
          return new Handles.CapFunction(this.DiamondCap);
        default:
          return new Handles.CapFunction(this.DiamondCap);
      }
    }

    private static float DistanceToCircleInternal(Vector3 position, Quaternion rotation, float size)
    {
      return HandleUtility.DistanceToCircle(position, size);
    }

    private float GetHandleSizeForPoint(int index)
    {
      return !((UnityEngine.Object) Camera.current != (UnityEngine.Object) null) ? this.GetHandleSize() : HandleUtility.GetHandleSize(this.GetPointPosition(index)) * 0.075f;
    }

    private float GetTangentSizeForPoint(int index)
    {
      return this.GetHandleSizeForPoint(index) * 0.8f;
    }

    private void RefreshTangents(int index, bool rightIsActive)
    {
      ShapeEditor.TangentMode tangentMode = this.GetTangentMode(index);
      Vector3 vector3_1 = this.GetPointLTangent(index);
      Vector3 vector3_2 = this.GetPointRTangent(index);
      if (tangentMode == ShapeEditor.TangentMode.Continuous)
      {
        if (rightIsActive)
        {
          Vector3 vector3_3 = -vector3_2;
          float magnitude = vector3_3.magnitude;
          vector3_1 = vector3_3.normalized * magnitude;
        }
        else
        {
          Vector3 vector3_3 = -vector3_1;
          float magnitude = vector3_3.magnitude;
          vector3_2 = vector3_3.normalized * magnitude;
        }
      }
      this.SetPointLTangent(this.activePoint, vector3_1);
      this.SetPointRTangent(this.activePoint, vector3_2);
    }

    private void StoreMouseDownState()
    {
      this.m_MousePositionLastMouseDown = this.currentEvent.mousePosition;
      this.m_ActivePointOnLastMouseDown = this.activePoint;
    }

    private void DelayedResetIfNecessary()
    {
      if (!this.m_DelayedReset)
        return;
      this.guiUtility.hotControl = 0;
      this.guiUtility.keyboardControl = 0;
      this.m_Selection.Clear();
      this.activePoint = -1;
      this.m_DelayedReset = false;
    }

    public Vector3 FindClosestPointOnEdge(int edgeIndex, Vector3 position, int iterations)
    {
      float num1 = 1f / (float) iterations;
      float num2 = float.MaxValue;
      float index = (float) edgeIndex;
      float num3 = 0.0f;
      while ((double) num3 <= 1.0)
      {
        Vector3 positionByIndex = this.GetPositionByIndex((float) edgeIndex + num3);
        float sqrMagnitude = (position - positionByIndex).sqrMagnitude;
        if ((double) sqrMagnitude < (double) num2)
        {
          num2 = sqrMagnitude;
          index = (float) edgeIndex + num3;
        }
        num3 += num1;
      }
      return this.GetPositionByIndex(index);
    }

    private Vector3 GetPositionByIndex(float index)
    {
      int index1 = Mathf.FloorToInt(index);
      int num = ShapeEditor.NextIndex(index1, this.GetPointsCount());
      float t = index - (float) index1;
      return ShapeEditor.GetPoint(this.GetPointPosition(index1), this.GetPointPosition(num), this.GetPointRTangent(index1), this.GetPointLTangent(num), t);
    }

    private static Vector3 GetPoint(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, float t)
    {
      t = Mathf.Clamp01(t);
      float num = 1f - t;
      return num * num * num * startPosition + 3f * num * num * t * (startPosition + startTangent) + 3f * num * t * t * (endPosition + endTangent) + t * t * t * endPosition;
    }

    private int FindClosestPointToMouse()
    {
      return this.FindClosestPointIndex(this.ScreenToLocal(this.currentEvent.mousePosition));
    }

    private float MouseDistanceToClosestTangent()
    {
      if (this.activePoint < 0)
        return float.MaxValue;
      Vector3 vector3_1 = this.GetPointLTangent(this.activePoint);
      Vector3 vector3_2 = this.GetPointRTangent(this.activePoint);
      if ((double) vector3_1.sqrMagnitude == 0.0 && (double) vector3_2.sqrMagnitude == 0.0)
        return float.MaxValue;
      Vector3 vector3_3 = this.GetPointPosition(this.activePoint);
      float tangentSizeForPoint = this.GetTangentSizeForPoint(this.activePoint);
      return Mathf.Min(HandleUtility.DistanceToRectangle(vector3_3 + vector3_1, Quaternion.identity, tangentSizeForPoint), HandleUtility.DistanceToRectangle(vector3_3 + vector3_2, Quaternion.identity, tangentSizeForPoint));
    }

    private int FindClosestPointIndex(Vector3 position)
    {
      float num1 = float.MaxValue;
      int num2 = -1;
      for (int index = 0; index < this.GetPointsCount(); ++index)
      {
        float sqrMagnitude = (this.GetPointPosition(index) - position).sqrMagnitude;
        if ((double) sqrMagnitude < (double) num1)
        {
          num2 = index;
          num1 = sqrMagnitude;
        }
      }
      return num2;
    }

    private ShapeEditor.DistanceToControl GetDistanceFuncForPoint(int index)
    {
      switch (this.GetTangentMode(index))
      {
        case ShapeEditor.TangentMode.Linear:
          return this.DistanceToRectangle();
        case ShapeEditor.TangentMode.Continuous:
          return this.DistanceToCircle();
        case ShapeEditor.TangentMode.Broken:
          return this.DistanceToDiamond();
        default:
          return this.DistanceToRectangle();
      }
    }

    private float MouseDistanceToPoint(int index)
    {
      switch (this.GetTangentMode(index))
      {
        case ShapeEditor.TangentMode.Linear:
          return HandleUtility.DistanceToRectangle(this.GetPointPosition(index), Quaternion.identity, this.GetHandleSizeForPoint(index));
        case ShapeEditor.TangentMode.Continuous:
          return HandleUtility.DistanceToCircle(this.GetPointPosition(index), this.GetHandleSizeForPoint(index));
        case ShapeEditor.TangentMode.Broken:
          return HandleUtility.DistanceToDiamond(this.GetPointPosition(index), Quaternion.identity, this.GetHandleSizeForPoint(index));
        default:
          return float.MaxValue;
      }
    }

    private bool EdgeDragModifiersActive()
    {
      return this.currentEvent.modifiers == EventModifiers.Control;
    }

    private static Vector3 DoSlider(int id, Vector3 position, Vector3 slide1, Vector3 slide2, float s, Handles.CapFunction cap)
    {
      return Slider2D.Do(id, position, Vector3.zero, Vector3.Cross(slide1, slide2), slide1, slide2, s, cap, Vector2.zero, false);
    }

    public void RectCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      if (eventType == EventType.Layout)
      {
        HandleUtility.AddControl(controlID, HandleUtility.DistanceToCircle(position, size * 0.5f));
      }
      else
      {
        if (eventType != EventType.Repaint)
          return;
        Quaternion quaternion = Quaternion.LookRotation((Vector3) this.handles.matrix.GetColumn(2), (ShapeEditor.ProjectPointOnPlane((Vector3) this.handles.matrix.GetColumn(2), position, position + Vector3.up) - position).normalized);
        Vector3 vector3_1 = quaternion * Vector3.right * size;
        Vector3 vector3_2 = quaternion * Vector3.up * size;
        List<Vector3> drawBatchList1 = this.GetDrawBatchList(new ShapeEditor.DrawBatchDataKey(ShapeEditor.handleFillColor, 4));
        drawBatchList1.Add(position + vector3_1 + vector3_2);
        drawBatchList1.Add(position + vector3_1 - vector3_2);
        drawBatchList1.Add(position - vector3_1 - vector3_2);
        drawBatchList1.Add(position - vector3_1 - vector3_2);
        drawBatchList1.Add(position - vector3_1 + vector3_2);
        drawBatchList1.Add(position + vector3_1 + vector3_2);
        List<Vector3> drawBatchList2 = this.GetDrawBatchList(new ShapeEditor.DrawBatchDataKey(ShapeEditor.handleOutlineColor, 1));
        drawBatchList2.Add(position + vector3_1 + vector3_2);
        drawBatchList2.Add(position + vector3_1 - vector3_2);
        drawBatchList2.Add(position + vector3_1 - vector3_2);
        drawBatchList2.Add(position - vector3_1 - vector3_2);
        drawBatchList2.Add(position - vector3_1 - vector3_2);
        drawBatchList2.Add(position - vector3_1 + vector3_2);
        drawBatchList2.Add(position - vector3_1 + vector3_2);
        drawBatchList2.Add(position + vector3_1 + vector3_2);
      }
    }

    public void CircleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      switch (eventType)
      {
        case EventType.Repaint:
          Vector3 vector3 = this.handleMatrixrotation * rotation * Vector3.forward;
          Vector3 from = Vector3.Cross(vector3, Vector3.up);
          if ((double) from.sqrMagnitude < 1.0 / 1000.0)
            from = Vector3.Cross(vector3, Vector3.right);
          Vector3[] dest = new Vector3[60];
          this.handles.SetDiscSectionPoints(dest, position, vector3, from, 360f, size);
          List<Vector3> drawBatchList1 = this.GetDrawBatchList(new ShapeEditor.DrawBatchDataKey(ShapeEditor.handleFillColor, 4));
          for (int index = 1; index < dest.Length; ++index)
          {
            drawBatchList1.Add(position);
            drawBatchList1.Add(dest[index]);
            drawBatchList1.Add(dest[index - 1]);
          }
          List<Vector3> drawBatchList2 = this.GetDrawBatchList(new ShapeEditor.DrawBatchDataKey(ShapeEditor.handleOutlineColor, 1));
          for (int index = 0; index < dest.Length - 1; ++index)
          {
            drawBatchList2.Add(dest[index]);
            drawBatchList2.Add(dest[index + 1]);
          }
          break;
        case EventType.Layout:
          HandleUtility.AddControl(controlID, HandleUtility.DistanceToCircle(position, size * 0.5f));
          break;
      }
    }

    public void DiamondCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      if (eventType == EventType.Layout)
      {
        HandleUtility.AddControl(controlID, HandleUtility.DistanceToCircle(position, size * 0.5f));
      }
      else
      {
        if (eventType != EventType.Repaint)
          return;
        Quaternion quaternion = Quaternion.LookRotation((Vector3) this.handles.matrix.GetColumn(2), (ShapeEditor.ProjectPointOnPlane((Vector3) this.handles.matrix.GetColumn(2), position, position + Vector3.up) - position).normalized);
        Vector3 vector3_1 = quaternion * Vector3.right * size * 1.25f;
        Vector3 vector3_2 = quaternion * Vector3.up * size * 1.25f;
        List<Vector3> drawBatchList1 = this.GetDrawBatchList(new ShapeEditor.DrawBatchDataKey(ShapeEditor.handleFillColor, 4));
        drawBatchList1.Add(position - vector3_2);
        drawBatchList1.Add(position + vector3_1);
        drawBatchList1.Add(position - vector3_1);
        drawBatchList1.Add(position - vector3_1);
        drawBatchList1.Add(position + vector3_2);
        drawBatchList1.Add(position + vector3_1);
        List<Vector3> drawBatchList2 = this.GetDrawBatchList(new ShapeEditor.DrawBatchDataKey(ShapeEditor.handleOutlineColor, 1));
        drawBatchList2.Add(position + vector3_1);
        drawBatchList2.Add(position - vector3_2);
        drawBatchList2.Add(position - vector3_2);
        drawBatchList2.Add(position - vector3_1);
        drawBatchList2.Add(position - vector3_1);
        drawBatchList2.Add(position + vector3_2);
        drawBatchList2.Add(position + vector3_2);
        drawBatchList2.Add(position + vector3_1);
      }
    }

    private static int NextIndex(int index, int total)
    {
      return ShapeEditor.mod(index + 1, total);
    }

    private static int PreviousIndex(int index, int total)
    {
      return ShapeEditor.mod(index - 1, total);
    }

    private static int mod(int x, int m)
    {
      int num = x % m;
      return num >= 0 ? num : num + m;
    }

    private static Vector3 ProjectPointOnPlane(Vector3 planeNormal, Vector3 planePoint, Vector3 point)
    {
      planeNormal.Normalize();
      float num = -Vector3.Dot(planeNormal.normalized, point - planePoint);
      return point + planeNormal * num;
    }

    public void RegisterToShapeEditor(ShapeEditor se)
    {
      ++this.m_ShapeEditorRegisteredTo;
      se.m_ShapeEditorListeners.Add(this);
    }

    public void UnregisterFromShapeEditor(ShapeEditor se)
    {
      --this.m_ShapeEditorRegisteredTo;
      se.m_ShapeEditorListeners.Remove(this);
    }

    private void OnShapeEditorUpdateDone()
    {
      ++this.m_ShapeEditorUpdateDone;
      if (this.m_ShapeEditorUpdateDone < this.m_ShapeEditorRegisteredTo)
        return;
      this.m_ShapeEditorUpdateDone = 0;
      this.m_MouseClosestEdge = -1;
      this.m_MouseClosestEdgeDist = float.MaxValue;
      this.m_EdgePoints = (Vector3[][]) null;
    }

    private void ClearSelectedPoints()
    {
      this.selectedPoints.Clear();
      this.activePoint = -1;
    }

    private void SelectPointsInRect(Rect r, ShapeEditor.SelectionType st)
    {
      this.m_Selection.RectSelect(EditorGUIExt.FromToRect((Vector2) this.ScreenToLocal(r.min), (Vector2) this.ScreenToLocal(r.max)), st);
    }

    private void DeleteSelections()
    {
      foreach (ShapeEditor shapeEditorListener in this.m_ShapeEditorListeners)
        shapeEditorListener.m_Selection.DeleteSelection();
      this.m_Selection.DeleteSelection();
    }

    private void MoveSelections(Vector2 distance)
    {
      foreach (ShapeEditor shapeEditorListener in this.m_ShapeEditorListeners)
        shapeEditorListener.m_Selection.MoveSelection((Vector3) distance);
      this.m_Selection.MoveSelection((Vector3) distance);
    }

    private void SelectPoint(int index, ShapeEditor.SelectionType st)
    {
      if (st == ShapeEditor.SelectionType.Normal)
      {
        foreach (ShapeEditor shapeEditorListener in this.m_ShapeEditorListeners)
          shapeEditorListener.ClearSelectedPoints();
      }
      this.m_Selection.SelectPoint(index, st);
    }

    public delegate float DistanceToControl(Vector3 pos, Quaternion rotation, float handleSize);

    internal enum SelectionType
    {
      Normal,
      Additive,
      Subtractive,
    }

    internal enum Tool
    {
      Edit,
      Create,
      Break,
    }

    internal enum TangentMode
    {
      Linear,
      Continuous,
      Broken,
    }

    private enum ColorEnum
    {
      EUnselected,
      EUnselectedHovered,
      ESelected,
      ESelectedHovered,
    }

    private class DrawBatchDataKey
    {
      private int m_Hash;

      public DrawBatchDataKey(Color c, int mode)
      {
        this.color = c;
        this.glMode = mode;
        this.m_Hash = this.glMode ^ this.color.GetHashCode() << 2;
      }

      public Color color { get; private set; }

      public int glMode { get; private set; }

      public override int GetHashCode()
      {
        return this.m_Hash;
      }

      public override bool Equals(object obj)
      {
        return this.m_Hash == obj.GetHashCode();
      }
    }
  }
}
