// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class CurveEditor : TimeArea, CurveUpdater
  {
    private static int s_SelectKeyHash = "SelectKeys".GetHashCode();
    private static int s_TangentControlIDHash = nameof (s_TangentControlIDHash).GetHashCode();
    private List<int> m_DrawOrder = new List<int>();
    internal Bounds m_DefaultBounds = new Bounds(new Vector3(0.5f, 0.5f, 0.0f), new Vector3(1f, 1f, 0.0f));
    private CurveEditorSettings m_Settings = new CurveEditorSettings();
    private Color m_TangentColor = new Color(1f, 1f, 1f, 0.5f);
    public float invSnap = 0.0f;
    private List<CurveSelection> preCurveDragSelection = (List<CurveSelection>) null;
    private bool m_InRangeSelection = false;
    private bool s_TimeRangeSelectionActive = false;
    private bool m_BoundsAreDirty = true;
    private bool m_SelectionBoundsAreDirty = true;
    private bool m_EnableCurveGroups = false;
    private Bounds m_SelectionBounds = new Bounds(Vector3.zero, Vector3.zero);
    private Bounds m_CurveBounds = new Bounds(Vector3.zero, Vector3.zero);
    private Bounds m_DrawingBounds = new Bounds(Vector3.zero, Vector3.zero);
    private CurveWrapper m_DraggingKey = (CurveWrapper) null;
    private string m_AxisLabelFormat = "n1";
    private string m_FocusedPointField = (string) null;
    private CurveWrapper[] m_DraggingCurveOrRegion = (CurveWrapper[]) null;
    private CurveWrapper[] m_AnimationCurves;
    public CurveEditor.CallbackFunction curvesUpdated;
    public ICurveEditorState state;
    private CurveMenuManager m_MenuManager;
    [SerializeField]
    private CurveEditorSelection m_Selection;
    private CurveSelection m_SelectedTangentPoint;
    private List<CurveSelection> s_SelectionBackup;
    private float s_TimeRangeSelectionStart;
    private float s_TimeRangeSelectionEnd;
    private List<CurveEditor.SavedCurve> m_CurveBackups;
    private Vector2 m_DraggedCoord;
    private Vector2 m_MoveCoord;
    private Vector2 m_PreviousDrawPointCenter;
    private CurveEditor.AxisLock m_AxisLock;
    private CurveControlPointRenderer m_PointRenderer;
    private CurveEditorRectangleTool m_RectangleTool;
    private const float kMaxPickDistSqr = 100f;
    private const float kExactPickDistSqr = 16f;
    private const float kCurveTimeEpsilon = 1E-05f;
    private Dictionary<int, int> m_CurveIDToIndexMap;
    private Vector2 s_StartMouseDragPosition;
    private Vector2 s_EndMouseDragPosition;
    private Vector2 s_StartKeyDragPosition;
    private CurveEditor.PickMode s_PickMode;
    private bool m_EditingPoints;
    private bool m_TimeWasEdited;
    private bool m_ValueWasEdited;
    private float m_NewTime;
    private float m_NewValue;
    private const string kPointValueFieldName = "pointValueField";
    private const string kPointTimeFieldName = "pointTimeField";
    private Vector2 m_PointEditingFieldPosition;

    public CurveEditor(Rect rect, CurveWrapper[] curves, bool minimalGUI)
      : base(minimalGUI)
    {
      this.rect = rect;
      this.animationCurves = curves;
      float[] tickModulos = new float[29]{ 1E-07f, 5E-07f, 1E-06f, 5E-06f, 1E-05f, 5E-05f, 0.0001f, 0.0005f, 1f / 1000f, 0.005f, 0.01f, 0.05f, 0.1f, 0.5f, 1f, 5f, 10f, 50f, 100f, 500f, 1000f, 5000f, 10000f, 50000f, 100000f, 500000f, 1000000f, 5000000f, 1E+07f };
      this.hTicks = new TickHandler();
      this.hTicks.SetTickModulos(tickModulos);
      this.vTicks = new TickHandler();
      this.vTicks.SetTickModulos(tickModulos);
      this.margin = 40f;
      this.OnEnable();
    }

    public CurveWrapper[] animationCurves
    {
      get
      {
        if (this.m_AnimationCurves == null)
          this.m_AnimationCurves = new CurveWrapper[0];
        return this.m_AnimationCurves;
      }
      set
      {
        this.m_AnimationCurves = value;
        this.curveIDToIndexMap.Clear();
        this.m_EnableCurveGroups = false;
        for (int index = 0; index < this.m_AnimationCurves.Length; ++index)
        {
          this.m_AnimationCurves[index].listIndex = index;
          this.curveIDToIndexMap.Add(this.m_AnimationCurves[index].id, index);
          this.m_EnableCurveGroups = this.m_EnableCurveGroups || this.m_AnimationCurves[index].groupId != -1;
        }
        this.SyncDrawOrder();
        this.SyncSelection();
        this.ValidateCurveList();
      }
    }

    public bool GetTopMostCurveID(out int curveID)
    {
      if (this.m_DrawOrder.Count > 0)
      {
        curveID = this.m_DrawOrder[this.m_DrawOrder.Count - 1];
        return true;
      }
      curveID = -1;
      return false;
    }

    private void SyncDrawOrder()
    {
      if (this.m_DrawOrder.Count == 0)
      {
        this.m_DrawOrder = ((IEnumerable<CurveWrapper>) this.m_AnimationCurves).Select<CurveWrapper, int>((Func<CurveWrapper, int>) (cw => cw.id)).ToList<int>();
      }
      else
      {
        List<int> intList = new List<int>(this.m_AnimationCurves.Length);
        for (int index1 = 0; index1 < this.m_DrawOrder.Count; ++index1)
        {
          int num = this.m_DrawOrder[index1];
          for (int index2 = 0; index2 < this.m_AnimationCurves.Length; ++index2)
          {
            if (this.m_AnimationCurves[index2].id == num)
            {
              intList.Add(num);
              break;
            }
          }
        }
        this.m_DrawOrder = intList;
        if (this.m_DrawOrder.Count == this.m_AnimationCurves.Length)
          return;
        for (int index1 = 0; index1 < this.m_AnimationCurves.Length; ++index1)
        {
          int id = this.m_AnimationCurves[index1].id;
          bool flag = false;
          for (int index2 = 0; index2 < this.m_DrawOrder.Count; ++index2)
          {
            if (this.m_DrawOrder[index2] == id)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            this.m_DrawOrder.Add(id);
        }
        if (this.m_DrawOrder.Count == this.m_AnimationCurves.Length)
          return;
        this.m_DrawOrder = ((IEnumerable<CurveWrapper>) this.m_AnimationCurves).Select<CurveWrapper, int>((Func<CurveWrapper, int>) (cw => cw.id)).ToList<int>();
      }
    }

    public TimeArea.TimeFormat timeFormat
    {
      get
      {
        if (this.state != null)
          return this.state.timeFormat;
        return TimeArea.TimeFormat.None;
      }
    }

    private Matrix4x4 TimeOffsetMatrix(CurveWrapper cw)
    {
      return Matrix4x4.TRS(new Vector3(cw.timeOffset * this.m_Scale.x, 0.0f, 0.0f), Quaternion.identity, Vector3.one);
    }

    private Matrix4x4 DrawingToOffsetViewMatrix(CurveWrapper cw)
    {
      return this.TimeOffsetMatrix(cw) * this.drawingToViewMatrix;
    }

    private Vector2 DrawingToOffsetViewTransformPoint(CurveWrapper cw, Vector2 lhs)
    {
      return new Vector2((float) ((double) lhs.x * (double) this.m_Scale.x + (double) this.m_Translation.x + (double) cw.timeOffset * (double) this.m_Scale.x), lhs.y * this.m_Scale.y + this.m_Translation.y);
    }

    private Vector3 DrawingToOffsetViewTransformPoint(CurveWrapper cw, Vector3 lhs)
    {
      return new Vector3((float) ((double) lhs.x * (double) this.m_Scale.x + (double) this.m_Translation.x + (double) cw.timeOffset * (double) this.m_Scale.x), lhs.y * this.m_Scale.y + this.m_Translation.y, 0.0f);
    }

    private Vector2 OffsetViewToDrawingTransformPoint(CurveWrapper cw, Vector2 lhs)
    {
      return new Vector2((float) ((double) lhs.x - (double) this.m_Translation.x - (double) cw.timeOffset * (double) this.m_Scale.x) / this.m_Scale.x, (lhs.y - this.m_Translation.y) / this.m_Scale.y);
    }

    private Vector3 OffsetViewToDrawingTransformPoint(CurveWrapper cw, Vector3 lhs)
    {
      return new Vector3((float) ((double) lhs.x - (double) this.m_Translation.x - (double) cw.timeOffset * (double) this.m_Scale.x) / this.m_Scale.x, (lhs.y - this.m_Translation.y) / this.m_Scale.y, 0.0f);
    }

    private Vector2 OffsetMousePositionInDrawing(CurveWrapper cw)
    {
      return this.OffsetViewToDrawingTransformPoint(cw, Event.current.mousePosition);
    }

    public CurveEditorSettings settings
    {
      get
      {
        return this.m_Settings;
      }
      set
      {
        if (value == null)
          return;
        this.m_Settings = value;
        this.ApplySettings();
      }
    }

    protected void ApplySettings()
    {
      this.hRangeLocked = this.settings.hRangeLocked;
      this.vRangeLocked = this.settings.vRangeLocked;
      this.hRangeMin = this.settings.hRangeMin;
      this.hRangeMax = this.settings.hRangeMax;
      this.vRangeMin = this.settings.vRangeMin;
      this.vRangeMax = this.settings.vRangeMax;
      this.scaleWithWindow = this.settings.scaleWithWindow;
      this.hSlider = this.settings.hSlider;
      this.vSlider = this.settings.vSlider;
      this.RecalculateBounds();
    }

    public Color tangentColor
    {
      get
      {
        return this.m_TangentColor;
      }
      set
      {
        this.m_TangentColor = value;
      }
    }

    internal CurveEditorSelection selection
    {
      get
      {
        if ((UnityEngine.Object) this.m_Selection == (UnityEngine.Object) null)
        {
          this.m_Selection = ScriptableObject.CreateInstance<CurveEditorSelection>();
          this.m_Selection.hideFlags = HideFlags.HideAndDontSave;
        }
        return this.m_Selection;
      }
    }

    internal List<CurveSelection> selectedCurves
    {
      get
      {
        return this.selection.selectedCurves;
      }
      set
      {
        this.selection.selectedCurves = value;
        this.InvalidateSelectionBounds();
      }
    }

    public bool hasSelection
    {
      get
      {
        return this.selectedCurves.Count != 0;
      }
    }

    internal void BeginRangeSelection()
    {
      this.m_InRangeSelection = true;
    }

    internal void EndRangeSelection()
    {
      this.m_InRangeSelection = false;
      this.selectedCurves.Sort();
    }

    internal void AddSelection(CurveSelection curveSelection)
    {
      this.selectedCurves.Add(curveSelection);
      if (!this.m_InRangeSelection)
        this.selectedCurves.Sort();
      this.InvalidateSelectionBounds();
    }

    internal void RemoveSelection(CurveSelection curveSelection)
    {
      this.selectedCurves.Remove(curveSelection);
      this.InvalidateSelectionBounds();
    }

    internal void ClearSelection()
    {
      this.selectedCurves.Clear();
      this.InvalidateSelectionBounds();
    }

    internal CurveWrapper GetCurveWrapperFromID(int curveID)
    {
      int index;
      if (this.m_AnimationCurves == null || !this.curveIDToIndexMap.TryGetValue(curveID, out index))
        return (CurveWrapper) null;
      return this.m_AnimationCurves[index];
    }

    internal CurveWrapper GetCurveWrapperFromSelection(CurveSelection curveSelection)
    {
      return this.GetCurveWrapperFromID(curveSelection.curveID);
    }

    internal AnimationCurve GetCurveFromSelection(CurveSelection curveSelection)
    {
      CurveWrapper wrapperFromSelection = this.GetCurveWrapperFromSelection(curveSelection);
      return wrapperFromSelection == null ? (AnimationCurve) null : wrapperFromSelection.curve;
    }

    internal Keyframe GetKeyframeFromSelection(CurveSelection curveSelection)
    {
      AnimationCurve curveFromSelection = this.GetCurveFromSelection(curveSelection);
      if (curveFromSelection != null && curveSelection.key >= 0 && curveSelection.key < curveFromSelection.length)
        return curveFromSelection[curveSelection.key];
      return new Keyframe();
    }

    public Bounds selectionBounds
    {
      get
      {
        this.RecalculateSelectionBounds();
        return this.m_SelectionBounds;
      }
    }

    public Bounds curveBounds
    {
      get
      {
        this.RecalculateBounds();
        return this.m_CurveBounds;
      }
    }

    public override Bounds drawingBounds
    {
      get
      {
        this.RecalculateBounds();
        return this.m_DrawingBounds;
      }
    }

    public void OnEnable()
    {
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    public void OnDisable()
    {
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      if (this.m_PointRenderer == null)
        return;
      this.m_PointRenderer.FlushCache();
    }

    public void OnDestroy()
    {
      if (!((UnityEngine.Object) this.m_Selection != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Selection);
    }

    private void UndoRedoPerformed()
    {
      if (this.settings.undoRedoSelection)
        this.InvalidateSelectionBounds();
      else
        this.SelectNone();
    }

    private void ValidateCurveList()
    {
      for (int index = 0; index < this.m_AnimationCurves.Length; ++index)
      {
        int regionId1 = this.m_AnimationCurves[index].regionId;
        if (regionId1 >= 0)
        {
          if (index == this.m_AnimationCurves.Length - 1)
          {
            Debug.LogError((object) "Region has only one curve last! Regions should be added as two curves after each other with same regionId");
            return;
          }
          int regionId2 = this.m_AnimationCurves[++index].regionId;
          if (regionId1 != regionId2)
          {
            Debug.LogError((object) ("Regions should be added as two curves after each other with same regionId: " + (object) regionId1 + " != " + (object) regionId2));
            return;
          }
        }
      }
      if (this.m_DrawOrder.Count != this.m_AnimationCurves.Length)
      {
        Debug.LogError((object) ("DrawOrder and AnimationCurves mismatch: DrawOrder " + (object) this.m_DrawOrder.Count + ", AnimationCurves: " + (object) this.m_AnimationCurves.Length));
      }
      else
      {
        int count = this.m_DrawOrder.Count;
        for (int index = 0; index < count; ++index)
        {
          int regionId1 = this.GetCurveWrapperFromID(this.m_DrawOrder[index]).regionId;
          if (regionId1 >= 0)
          {
            if (index == count - 1)
            {
              Debug.LogError((object) "Region has only one curve last! Regions should be added as two curves after each other with same regionId");
              break;
            }
            int regionId2 = this.GetCurveWrapperFromID(this.m_DrawOrder[++index]).regionId;
            if (regionId1 != regionId2)
            {
              Debug.LogError((object) ("DrawOrder: Regions not added correctly after each other. RegionIds: " + (object) regionId1 + " , " + (object) regionId2));
              break;
            }
          }
        }
      }
    }

    private Dictionary<int, int> curveIDToIndexMap
    {
      get
      {
        if (this.m_CurveIDToIndexMap == null)
          this.m_CurveIDToIndexMap = new Dictionary<int, int>();
        return this.m_CurveIDToIndexMap;
      }
    }

    private void SyncSelection()
    {
      this.Init();
      List<CurveSelection> curveSelectionList = new List<CurveSelection>(this.selectedCurves.Count);
      foreach (CurveSelection selectedCurve in this.selectedCurves)
      {
        CurveWrapper wrapperFromSelection = this.GetCurveWrapperFromSelection(selectedCurve);
        if (wrapperFromSelection != null && (!wrapperFromSelection.hidden || wrapperFromSelection.groupId != -1))
        {
          wrapperFromSelection.selected = CurveWrapper.SelectionMode.Selected;
          curveSelectionList.Add(selectedCurve);
        }
      }
      if (curveSelectionList.Count != this.selectedCurves.Count)
        this.selectedCurves = curveSelectionList;
      this.InvalidateBounds();
    }

    public void InvalidateBounds()
    {
      this.m_BoundsAreDirty = true;
    }

    private void RecalculateBounds()
    {
      if (this.InLiveEdit() || !this.m_BoundsAreDirty)
        return;
      this.m_DrawingBounds = this.m_DefaultBounds;
      this.m_CurveBounds = this.m_DefaultBounds;
      if (this.animationCurves != null)
      {
        bool flag = false;
        for (int index = 0; index < this.animationCurves.Length; ++index)
        {
          CurveWrapper animationCurve = this.animationCurves[index];
          if (!animationCurve.hidden && animationCurve.curve.length != 0)
          {
            if (!flag)
            {
              this.m_CurveBounds = animationCurve.bounds;
              flag = true;
            }
            else
              this.m_CurveBounds.Encapsulate(animationCurve.bounds);
          }
        }
      }
      this.m_DrawingBounds.SetMinMax(new Vector3((double) this.hRangeMin == double.NegativeInfinity ? this.m_CurveBounds.min.x : this.hRangeMin, (double) this.vRangeMin == double.NegativeInfinity ? this.m_CurveBounds.min.y : this.vRangeMin, this.m_CurveBounds.min.z), new Vector3((double) this.hRangeMax == double.PositiveInfinity ? this.m_CurveBounds.max.x : this.hRangeMax, (double) this.vRangeMax == double.PositiveInfinity ? this.m_CurveBounds.max.y : this.vRangeMax, this.m_CurveBounds.max.z));
      this.m_DrawingBounds.size = new Vector3(Mathf.Max(this.m_DrawingBounds.size.x, 0.1f), Mathf.Max(this.m_DrawingBounds.size.y, 0.1f), 0.0f);
      this.m_CurveBounds.size = new Vector3(Mathf.Max(this.m_CurveBounds.size.x, 0.1f), Mathf.Max(this.m_CurveBounds.size.y, 0.1f), 0.0f);
      this.m_BoundsAreDirty = false;
    }

    public void InvalidateSelectionBounds()
    {
      this.m_SelectionBoundsAreDirty = true;
    }

    private void RecalculateSelectionBounds()
    {
      if (!this.m_SelectionBoundsAreDirty)
        return;
      if (this.hasSelection)
      {
        List<CurveSelection> selectedCurves = this.selectedCurves;
        CurveWrapper wrapperFromSelection = this.GetCurveWrapperFromSelection(selectedCurves[0]);
        float num = wrapperFromSelection == null ? 0.0f : wrapperFromSelection.timeOffset;
        Keyframe keyframeFromSelection = this.GetKeyframeFromSelection(selectedCurves[0]);
        this.m_SelectionBounds = new Bounds((Vector3) new Vector2(keyframeFromSelection.time + num, keyframeFromSelection.value), (Vector3) Vector2.zero);
        for (int index = 1; index < selectedCurves.Count; ++index)
        {
          keyframeFromSelection = this.GetKeyframeFromSelection(selectedCurves[index]);
          this.m_SelectionBounds.Encapsulate((Vector3) new Vector2(keyframeFromSelection.time + num, keyframeFromSelection.value));
        }
      }
      else
        this.m_SelectionBounds = new Bounds(Vector3.zero, Vector3.zero);
      this.m_SelectionBoundsAreDirty = false;
    }

    public void FrameClip(bool horizontally, bool vertically)
    {
      Bounds curveBounds = this.curveBounds;
      if (curveBounds.size == Vector3.zero)
        return;
      if (horizontally)
        this.SetShownHRangeInsideMargins(curveBounds.min.x, curveBounds.max.x);
      if (!vertically)
        return;
      this.SetShownVRangeInsideMargins(curveBounds.min.y, curveBounds.max.y);
    }

    public void FrameSelected(bool horizontally, bool vertically)
    {
      if (!this.hasSelection)
      {
        this.FrameClip(horizontally, vertically);
      }
      else
      {
        Bounds bounds = new Bounds();
        if (this.selectedCurves.Count == 1)
        {
          CurveSelection selectedCurve = this.selectedCurves[0];
          CurveWrapper wrapperFromSelection = this.GetCurveWrapperFromSelection(selectedCurve);
          bounds = new Bounds((Vector3) new Vector2(wrapperFromSelection.curve[selectedCurve.key].time, wrapperFromSelection.curve[selectedCurve.key].value), (Vector3) Vector2.zero);
          if (selectedCurve.key - 1 >= 0)
            bounds.Encapsulate((Vector3) new Vector2(wrapperFromSelection.curve[selectedCurve.key - 1].time, wrapperFromSelection.curve[selectedCurve.key - 1].value));
          if (selectedCurve.key + 1 < wrapperFromSelection.curve.length)
            bounds.Encapsulate((Vector3) new Vector2(wrapperFromSelection.curve[selectedCurve.key + 1].time, wrapperFromSelection.curve[selectedCurve.key + 1].value));
        }
        else
          bounds = this.selectionBounds;
        bounds.size = new Vector3(Mathf.Max(bounds.size.x, 0.1f), Mathf.Max(bounds.size.y, 0.1f), 0.0f);
        if (horizontally)
          this.SetShownHRangeInsideMargins(bounds.min.x, bounds.max.x);
        if (!vertically)
          return;
        this.SetShownVRangeInsideMargins(bounds.min.y, bounds.max.y);
      }
    }

    public void UpdateCurves(List<int> curveIds, string undoText)
    {
      foreach (int curveId in curveIds)
        this.GetCurveWrapperFromID(curveId).changed = true;
      if (this.curvesUpdated == null)
        return;
      this.curvesUpdated();
    }

    public void UpdateCurves(List<ChangedCurve> changedCurves, string undoText)
    {
      this.UpdateCurves(new List<int>(changedCurves.Select<ChangedCurve, int>((Func<ChangedCurve, int>) (curve => curve.curveId))), undoText);
    }

    public void StartLiveEdit()
    {
      this.MakeCurveBackups();
    }

    public void EndLiveEdit()
    {
      this.m_CurveBackups = (List<CurveEditor.SavedCurve>) null;
    }

    public bool InLiveEdit()
    {
      return this.m_CurveBackups != null;
    }

    private void Init()
    {
    }

    public void OnGUI()
    {
      this.BeginViewGUI();
      this.GridGUI();
      this.DrawWrapperPopups();
      this.CurveGUI();
      this.EndViewGUI();
    }

    public void CurveGUI()
    {
      if (this.m_PointRenderer == null)
        this.m_PointRenderer = new CurveControlPointRenderer();
      if (this.m_RectangleTool == null)
      {
        this.m_RectangleTool = new CurveEditorRectangleTool();
        this.m_RectangleTool.Initialize((TimeArea) this);
      }
      GUI.BeginGroup(this.drawRect);
      this.Init();
      GUIUtility.GetControlID(CurveEditor.s_SelectKeyHash, FocusType.Passive);
      Color white = Color.white;
      GUI.backgroundColor = white;
      GUI.contentColor = white;
      Color color = GUI.color;
      Event current = Event.current;
      if (current.type != EventType.Repaint)
        this.EditSelectedPoints();
      EventType type = current.type;
      switch (type)
      {
        case EventType.ValidateCommand:
        case EventType.ExecuteCommand:
          bool flag1 = current.type == EventType.ExecuteCommand;
          switch (current.commandName)
          {
            case "Delete":
              if (this.hasSelection)
              {
                if (flag1)
                  this.DeleteSelectedKeys();
                current.Use();
                break;
              }
              break;
            case "FrameSelected":
              if (flag1)
                this.FrameSelected(true, true);
              current.Use();
              break;
            case "SelectAll":
              if (flag1)
                this.SelectAll();
              current.Use();
              break;
          }
        case EventType.ContextClick:
          CurveSelection nearest = this.FindNearest();
          if (nearest != null)
          {
            List<KeyIdentifier> keyList = new List<KeyIdentifier>();
            bool flag2 = false;
            foreach (CurveSelection selectedCurve in this.selectedCurves)
            {
              keyList.Add(new KeyIdentifier(this.GetCurveFromSelection(selectedCurve), selectedCurve.curveID, selectedCurve.key));
              if (selectedCurve.curveID == nearest.curveID && selectedCurve.key == nearest.key)
                flag2 = true;
            }
            if (!flag2)
            {
              keyList.Clear();
              keyList.Add(new KeyIdentifier(this.GetCurveFromSelection(nearest), nearest.curveID, nearest.key));
              this.ClearSelection();
              this.AddSelection(nearest);
            }
            bool flag3 = !this.selectedCurves.Exists((Predicate<CurveSelection>) (sel => !this.GetCurveWrapperFromSelection(sel).animationIsEditable));
            this.m_MenuManager = new CurveMenuManager((CurveUpdater) this);
            GenericMenu menu = new GenericMenu();
            string text1 = keyList.Count <= 1 ? "Delete Key" : "Delete Keys";
            string text2 = keyList.Count <= 1 ? "Edit Key..." : "Edit Keys...";
            if (flag3)
            {
              menu.AddItem(new GUIContent(text1), false, new GenericMenu.MenuFunction2(this.DeleteKeys), (object) keyList);
              menu.AddItem(new GUIContent(text2), false, new GenericMenu.MenuFunction2(this.StartEditingSelectedPointsContext), (object) this.OffsetMousePositionInDrawing(this.GetCurveWrapperFromSelection(nearest)));
            }
            else
            {
              menu.AddDisabledItem(new GUIContent(text1));
              menu.AddDisabledItem(new GUIContent(text2));
            }
            if (flag3)
            {
              menu.AddSeparator("");
              this.m_MenuManager.AddTangentMenuItems(menu, keyList);
            }
            menu.ShowAsContext();
            Event.current.Use();
            break;
          }
          break;
        default:
          if (type == EventType.KeyDown)
          {
            if ((current.keyCode == KeyCode.Backspace || current.keyCode == KeyCode.Delete) && this.hasSelection)
            {
              this.DeleteSelectedKeys();
              current.Use();
            }
            if (current.keyCode == KeyCode.A)
            {
              this.FrameClip(true, true);
              current.Use();
              break;
            }
            break;
          }
          break;
      }
      GUI.color = color;
      this.m_RectangleTool.HandleOverlayEvents();
      this.DragTangents();
      this.m_RectangleTool.HandleEvents();
      this.EditAxisLabels();
      this.SelectPoints();
      EditorGUI.BeginChangeCheck();
      Vector2 vector2 = this.MovePoints();
      if (EditorGUI.EndChangeCheck() && this.m_DraggingKey != null)
        this.m_MoveCoord = vector2;
      if (current.type == EventType.Repaint)
      {
        this.DrawCurves();
        this.m_RectangleTool.OnGUI();
        this.DrawCurvesTangents();
        this.DrawCurvesOverlay();
        this.m_RectangleTool.OverlayOnGUI();
        this.EditSelectedPoints();
      }
      GUI.color = color;
      GUI.EndGroup();
    }

    private void RecalcCurveSelection()
    {
      foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
        animationCurve.selected = CurveWrapper.SelectionMode.None;
      foreach (CurveSelection selectedCurve in this.selectedCurves)
      {
        CurveWrapper wrapperFromSelection = this.GetCurveWrapperFromSelection(selectedCurve);
        if (wrapperFromSelection != null)
          wrapperFromSelection.selected = !selectedCurve.semiSelected ? CurveWrapper.SelectionMode.Selected : CurveWrapper.SelectionMode.SemiSelected;
      }
    }

    private void RecalcSecondarySelection()
    {
      if (!this.m_EnableCurveGroups)
        return;
      List<CurveSelection> curveSelectionList = new List<CurveSelection>(this.selectedCurves.Count);
      foreach (CurveSelection selectedCurve in this.selectedCurves)
      {
        CurveWrapper wrapperFromSelection = this.GetCurveWrapperFromSelection(selectedCurve);
        if (wrapperFromSelection != null)
        {
          int groupId = wrapperFromSelection.groupId;
          if (groupId != -1 && !selectedCurve.semiSelected)
          {
            curveSelectionList.Add(selectedCurve);
            foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
            {
              if (animationCurve.groupId == groupId && animationCurve != wrapperFromSelection)
                curveSelectionList.Add(new CurveSelection(animationCurve.id, selectedCurve.key)
                {
                  semiSelected = true
                });
            }
          }
          else
            curveSelectionList.Add(selectedCurve);
        }
      }
      curveSelectionList.Sort();
      int index = 0;
      while (index < curveSelectionList.Count - 1)
      {
        CurveSelection curveSelection1 = curveSelectionList[index];
        CurveSelection curveSelection2 = curveSelectionList[index + 1];
        if (curveSelection1.curveID == curveSelection2.curveID && curveSelection1.key == curveSelection2.key)
        {
          if (!curveSelection1.semiSelected || !curveSelection2.semiSelected)
            curveSelection1.semiSelected = false;
          curveSelectionList.RemoveAt(index + 1);
        }
        else
          ++index;
      }
      this.selectedCurves = curveSelectionList;
    }

    private void DragTangents()
    {
      Event current = Event.current;
      int controlId = GUIUtility.GetControlID(CurveEditor.s_TangentControlIDHash, FocusType.Passive);
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (current.button != 0 || current.alt)
            break;
          this.m_SelectedTangentPoint = (CurveSelection) null;
          float num = 100f;
          Vector2 mousePosition = Event.current.mousePosition;
          foreach (CurveSelection selectedCurve in this.selectedCurves)
          {
            CurveWrapper wrapperFromSelection = this.GetCurveWrapperFromSelection(selectedCurve);
            if (wrapperFromSelection != null)
            {
              if (this.IsLeftTangentEditable(selectedCurve))
              {
                CurveSelection selection = new CurveSelection(selectedCurve.curveID, selectedCurve.key, CurveSelection.SelectionType.InTangent);
                float sqrMagnitude = (this.DrawingToOffsetViewTransformPoint(wrapperFromSelection, this.GetPosition(selection)) - mousePosition).sqrMagnitude;
                if ((double) sqrMagnitude <= (double) num)
                {
                  this.m_SelectedTangentPoint = selection;
                  num = sqrMagnitude;
                }
              }
              if (this.IsRightTangentEditable(selectedCurve))
              {
                CurveSelection selection = new CurveSelection(selectedCurve.curveID, selectedCurve.key, CurveSelection.SelectionType.OutTangent);
                float sqrMagnitude = (this.DrawingToOffsetViewTransformPoint(wrapperFromSelection, this.GetPosition(selection)) - mousePosition).sqrMagnitude;
                if ((double) sqrMagnitude <= (double) num)
                {
                  this.m_SelectedTangentPoint = selection;
                  num = sqrMagnitude;
                }
              }
            }
          }
          if (this.m_SelectedTangentPoint != null)
          {
            this.SaveKeySelection("Edit Curve");
            GUIUtility.hotControl = controlId;
            current.Use();
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != controlId)
            break;
          GUIUtility.hotControl = 0;
          current.Use();
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != controlId)
            break;
          CurveSelection selectedTangentPoint = this.m_SelectedTangentPoint;
          CurveWrapper wrapperFromSelection1 = this.GetCurveWrapperFromSelection(selectedTangentPoint);
          if (wrapperFromSelection1 != null && wrapperFromSelection1.animationIsEditable)
          {
            Vector2 vector2_1 = this.OffsetMousePositionInDrawing(wrapperFromSelection1);
            Keyframe keyframeFromSelection = this.GetKeyframeFromSelection(selectedTangentPoint);
            if (selectedTangentPoint.type == CurveSelection.SelectionType.InTangent)
            {
              Vector2 vector2_2 = vector2_1 - new Vector2(keyframeFromSelection.time, keyframeFromSelection.value);
              keyframeFromSelection.inTangent = (double) vector2_2.x >= -9.99999974737875E-05 ? float.PositiveInfinity : vector2_2.y / vector2_2.x;
              AnimationUtility.SetKeyLeftTangentMode(ref keyframeFromSelection, AnimationUtility.TangentMode.Free);
              if (!AnimationUtility.GetKeyBroken(keyframeFromSelection))
              {
                keyframeFromSelection.outTangent = keyframeFromSelection.inTangent;
                AnimationUtility.SetKeyRightTangentMode(ref keyframeFromSelection, AnimationUtility.TangentMode.Free);
              }
            }
            else if (selectedTangentPoint.type == CurveSelection.SelectionType.OutTangent)
            {
              Vector2 vector2_2 = vector2_1 - new Vector2(keyframeFromSelection.time, keyframeFromSelection.value);
              keyframeFromSelection.outTangent = (double) vector2_2.x <= 9.99999974737875E-05 ? float.PositiveInfinity : vector2_2.y / vector2_2.x;
              AnimationUtility.SetKeyRightTangentMode(ref keyframeFromSelection, AnimationUtility.TangentMode.Free);
              if (!AnimationUtility.GetKeyBroken(keyframeFromSelection))
              {
                keyframeFromSelection.inTangent = keyframeFromSelection.outTangent;
                AnimationUtility.SetKeyLeftTangentMode(ref keyframeFromSelection, AnimationUtility.TangentMode.Free);
              }
            }
            selectedTangentPoint.key = wrapperFromSelection1.MoveKey(selectedTangentPoint.key, ref keyframeFromSelection);
            AnimationUtility.UpdateTangentsFromModeSurrounding(wrapperFromSelection1.curve, selectedTangentPoint.key);
            wrapperFromSelection1.changed = true;
            GUI.changed = true;
          }
          Event.current.Use();
          break;
        case EventType.Repaint:
          if (GUIUtility.hotControl != controlId)
            break;
          EditorGUIUtility.AddCursorRect(new Rect(current.mousePosition.x - 10f, current.mousePosition.y - 10f, 20f, 20f), MouseCursor.MoveArrow);
          break;
      }
    }

    internal void DeleteSelectedKeys()
    {
      this.SaveKeySelection(this.selectedCurves.Count <= 1 ? "Delete Key" : "Delete Keys");
      for (int index = this.selectedCurves.Count - 1; index >= 0; --index)
      {
        CurveSelection selectedCurve = this.selectedCurves[index];
        CurveWrapper wrapperFromSelection = this.GetCurveWrapperFromSelection(selectedCurve);
        if (wrapperFromSelection != null && wrapperFromSelection.animationIsEditable && (this.settings.allowDeleteLastKeyInCurve || wrapperFromSelection.curve.keys.Length != 1))
        {
          wrapperFromSelection.curve.RemoveKey(selectedCurve.key);
          AnimationUtility.UpdateTangentsFromMode(wrapperFromSelection.curve);
          wrapperFromSelection.changed = true;
          GUI.changed = true;
        }
      }
      this.SelectNone();
    }

    private void DeleteKeys(object obj)
    {
      List<KeyIdentifier> keyIdentifierList = (List<KeyIdentifier>) obj;
      string str = keyIdentifierList.Count <= 1 ? "Delete Key" : "Delete Keys";
      this.SaveKeySelection(str);
      List<int> curveIds = new List<int>();
      for (int index = keyIdentifierList.Count - 1; index >= 0; --index)
      {
        if ((this.settings.allowDeleteLastKeyInCurve || keyIdentifierList[index].curve.keys.Length != 1) && this.GetCurveWrapperFromID(keyIdentifierList[index].curveId).animationIsEditable)
        {
          keyIdentifierList[index].curve.RemoveKey(keyIdentifierList[index].key);
          AnimationUtility.UpdateTangentsFromMode(keyIdentifierList[index].curve);
          curveIds.Add(keyIdentifierList[index].curveId);
        }
      }
      this.UpdateCurves(curveIds, str);
      this.SelectNone();
    }

    private float ClampVerticalValue(float value, int curveID)
    {
      value = Mathf.Clamp(value, this.vRangeMin, this.vRangeMax);
      CurveWrapper curveWrapperFromId = this.GetCurveWrapperFromID(curveID);
      if (curveWrapperFromId != null)
        value = Mathf.Clamp(value, curveWrapperFromId.vRangeMin, curveWrapperFromId.vRangeMax);
      return value;
    }

    internal void TranslateSelectedKeys(Vector2 movement)
    {
      bool flag = this.InLiveEdit();
      if (!flag)
        this.StartLiveEdit();
      this.UpdateCurvesFromPoints((CurveEditor.SavedCurve.KeyFrameOperation) ((keyframe, curve) =>
      {
        if (keyframe.selected == CurveWrapper.SelectionMode.None)
          return keyframe;
        CurveEditor.SavedCurve.SavedKeyFrame savedKeyFrame = keyframe.Clone();
        savedKeyFrame.key.time = Mathf.Clamp(savedKeyFrame.key.time + movement.x, this.hRangeMin, this.hRangeMax);
        if (savedKeyFrame.selected == CurveWrapper.SelectionMode.Selected)
          savedKeyFrame.key.value = this.ClampVerticalValue(savedKeyFrame.key.value + movement.y, curve.curveId);
        return savedKeyFrame;
      }));
      if (flag)
        return;
      this.EndLiveEdit();
    }

    internal void SetSelectedKeyPositions(float newTime, float newValue, bool updateTime, bool updateValue)
    {
      if (!updateTime && !updateValue)
        return;
      bool flag = this.InLiveEdit();
      if (!flag)
        this.StartLiveEdit();
      this.UpdateCurvesFromPoints((CurveEditor.SavedCurve.KeyFrameOperation) ((keyframe, curve) =>
      {
        if (keyframe.selected == CurveWrapper.SelectionMode.None)
          return keyframe;
        CurveEditor.SavedCurve.SavedKeyFrame savedKeyFrame = keyframe.Clone();
        if (updateTime)
          savedKeyFrame.key.time = Mathf.Clamp(newTime, this.hRangeMin, this.hRangeMax);
        if (updateValue)
          savedKeyFrame.key.value = this.ClampVerticalValue(newValue, curve.curveId);
        return savedKeyFrame;
      }));
      if (flag)
        return;
      this.EndLiveEdit();
    }

    internal void TransformSelectedKeys(Matrix4x4 matrix, bool flipX, bool flipY)
    {
      bool flag = this.InLiveEdit();
      if (!flag)
        this.StartLiveEdit();
      this.UpdateCurvesFromPoints((CurveEditor.SavedCurve.KeyFrameOperation) ((keyframe, curve) =>
      {
        if (keyframe.selected == CurveWrapper.SelectionMode.None)
          return keyframe;
        CurveEditor.SavedCurve.SavedKeyFrame savedKeyFrame = keyframe.Clone();
        Vector3 point = new Vector3(savedKeyFrame.key.time, savedKeyFrame.key.value, 0.0f);
        point = matrix.MultiplyPoint3x4(point);
        point.x = this.SnapTime(point.x);
        savedKeyFrame.key.time = Mathf.Clamp(point.x, this.hRangeMin, this.hRangeMax);
        if (flipX)
        {
          savedKeyFrame.key.inTangent = (double) keyframe.key.outTangent == double.PositiveInfinity ? float.PositiveInfinity : -keyframe.key.outTangent;
          savedKeyFrame.key.outTangent = (double) keyframe.key.inTangent == double.PositiveInfinity ? float.PositiveInfinity : -keyframe.key.inTangent;
        }
        if (savedKeyFrame.selected == CurveWrapper.SelectionMode.Selected)
        {
          savedKeyFrame.key.value = this.ClampVerticalValue(point.y, curve.curveId);
          if (flipY)
          {
            savedKeyFrame.key.inTangent = (double) savedKeyFrame.key.inTangent == double.PositiveInfinity ? float.PositiveInfinity : -savedKeyFrame.key.inTangent;
            savedKeyFrame.key.outTangent = (double) savedKeyFrame.key.outTangent == double.PositiveInfinity ? float.PositiveInfinity : -savedKeyFrame.key.outTangent;
          }
        }
        return savedKeyFrame;
      }));
      if (flag)
        return;
      this.EndLiveEdit();
    }

    internal void TransformRippleKeys(Matrix4x4 matrix, float t1, float t2, bool flipX)
    {
      bool flag = this.InLiveEdit();
      if (!flag)
        this.StartLiveEdit();
      this.UpdateCurvesFromPoints((CurveEditor.SavedCurve.KeyFrameOperation) ((keyframe, curve) =>
      {
        float a = keyframe.key.time;
        if (keyframe.selected != CurveWrapper.SelectionMode.None)
        {
          float x = matrix.MultiplyPoint3x4(new Vector3(keyframe.key.time, 0.0f, 0.0f)).x;
          CurveEditor.SavedCurve.SavedKeyFrame savedKeyFrame = keyframe.Clone();
          savedKeyFrame.key.time = this.SnapTime(Mathf.Clamp(x, this.hRangeMin, this.hRangeMax));
          if (flipX)
          {
            savedKeyFrame.key.inTangent = (double) keyframe.key.outTangent == double.PositiveInfinity ? float.PositiveInfinity : -keyframe.key.outTangent;
            savedKeyFrame.key.outTangent = (double) keyframe.key.inTangent == double.PositiveInfinity ? float.PositiveInfinity : -keyframe.key.inTangent;
          }
          return savedKeyFrame;
        }
        if ((double) keyframe.key.time > (double) t2)
        {
          float num = matrix.MultiplyPoint3x4(new Vector3(!flipX ? t2 : t1, 0.0f, 0.0f)).x - t2;
          if ((double) num > 0.0)
            a = keyframe.key.time + num;
        }
        else if ((double) keyframe.key.time < (double) t1)
        {
          float num = matrix.MultiplyPoint3x4(new Vector3(!flipX ? t1 : t2, 0.0f, 0.0f)).x - t1;
          if ((double) num < 0.0)
            a = keyframe.key.time + num;
        }
        if (Mathf.Approximately(a, keyframe.key.time))
          return keyframe;
        CurveEditor.SavedCurve.SavedKeyFrame savedKeyFrame1 = keyframe.Clone();
        savedKeyFrame1.key.time = this.SnapTime(Mathf.Clamp(a, this.hRangeMin, this.hRangeMax));
        return savedKeyFrame1;
      }));
      if (flag)
        return;
      this.EndLiveEdit();
    }

    private void UpdateCurvesFromPoints(CurveEditor.SavedCurve.KeyFrameOperation action)
    {
      if (this.m_CurveBackups == null)
        return;
      List<CurveSelection> curveSelectionList = new List<CurveSelection>();
      foreach (CurveEditor.SavedCurve curveBackup in this.m_CurveBackups)
      {
        CurveWrapper curveWrapperFromId = this.GetCurveWrapperFromID(curveBackup.curveId);
        if (curveWrapperFromId.animationIsEditable)
        {
          SortedList<float, CurveEditor.SavedCurve.SavedKeyFrame> sortedList = new SortedList<float, CurveEditor.SavedCurve.SavedKeyFrame>((IComparer<float>) CurveEditor.SavedCurve.SavedKeyFrameComparer.Instance);
          foreach (CurveEditor.SavedCurve.SavedKeyFrame key in curveBackup.keys)
          {
            if (key.selected == CurveWrapper.SelectionMode.None)
            {
              CurveEditor.SavedCurve.SavedKeyFrame savedKeyFrame = action(key, curveBackup);
              curveWrapperFromId.PreProcessKey(ref savedKeyFrame.key);
              sortedList[savedKeyFrame.key.time] = savedKeyFrame;
            }
          }
          foreach (CurveEditor.SavedCurve.SavedKeyFrame key in curveBackup.keys)
          {
            if (key.selected != CurveWrapper.SelectionMode.None)
            {
              CurveEditor.SavedCurve.SavedKeyFrame savedKeyFrame = action(key, curveBackup);
              curveWrapperFromId.PreProcessKey(ref savedKeyFrame.key);
              sortedList[savedKeyFrame.key.time] = savedKeyFrame;
            }
          }
          int key1 = 0;
          Keyframe[] keyframeArray = new Keyframe[sortedList.Count];
          foreach (KeyValuePair<float, CurveEditor.SavedCurve.SavedKeyFrame> keyValuePair in sortedList)
          {
            CurveEditor.SavedCurve.SavedKeyFrame savedKeyFrame = keyValuePair.Value;
            keyframeArray[key1] = savedKeyFrame.key;
            if (savedKeyFrame.selected != CurveWrapper.SelectionMode.None)
            {
              CurveSelection curveSelection = new CurveSelection(curveBackup.curveId, key1);
              if (savedKeyFrame.selected == CurveWrapper.SelectionMode.SemiSelected)
                curveSelection.semiSelected = true;
              curveSelectionList.Add(curveSelection);
            }
            ++key1;
          }
          curveWrapperFromId.curve.keys = keyframeArray;
          AnimationUtility.UpdateTangentsFromMode(curveWrapperFromId.curve);
          curveWrapperFromId.changed = true;
        }
      }
      this.selectedCurves = curveSelectionList;
    }

    private float SnapTime(float t)
    {
      if (EditorGUI.actionKey)
      {
        float periodOfLevel = this.hTicks.GetPeriodOfLevel(this.hTicks.GetLevelWithMinSeparation(5f));
        t = Mathf.Round(t / periodOfLevel) * periodOfLevel;
      }
      else if ((double) this.invSnap != 0.0)
        t = Mathf.Round(t * this.invSnap) / this.invSnap;
      return t;
    }

    private float SnapValue(float v)
    {
      if (EditorGUI.actionKey)
      {
        float periodOfLevel = this.vTicks.GetPeriodOfLevel(this.vTicks.GetLevelWithMinSeparation(5f));
        v = Mathf.Round(v / periodOfLevel) * periodOfLevel;
      }
      return v;
    }

    private Vector2 GetGUIPoint(CurveWrapper cw, Vector3 point)
    {
      return HandleUtility.WorldToGUIPoint(this.DrawingToOffsetViewTransformPoint(cw, point));
    }

    private Rect GetWorldRect(CurveWrapper cw, Rect rect)
    {
      Vector2 worldPoint1 = this.GetWorldPoint(cw, rect.min);
      Vector2 worldPoint2 = this.GetWorldPoint(cw, rect.max);
      return Rect.MinMaxRect(worldPoint1.x, worldPoint2.y, worldPoint2.x, worldPoint1.y);
    }

    private Vector2 GetWorldPoint(CurveWrapper cw, Vector2 point)
    {
      return this.OffsetViewToDrawingTransformPoint(cw, point);
    }

    private Rect GetCurveRect(CurveWrapper cw)
    {
      Bounds bounds = cw.bounds;
      return Rect.MinMaxRect(bounds.min.x, bounds.min.y, bounds.max.x, bounds.max.y);
    }

    private int OnlyOneEditableCurve()
    {
      int num1 = -1;
      int num2 = 0;
      for (int index = 0; index < this.m_AnimationCurves.Length; ++index)
      {
        CurveWrapper animationCurve = this.m_AnimationCurves[index];
        if (!animationCurve.hidden && !animationCurve.readOnly)
        {
          ++num2;
          num1 = index;
        }
      }
      if (num2 == 1)
        return num1;
      return -1;
    }

    private int GetCurveAtPosition(Vector2 viewPos, out Vector2 closestPointOnCurve)
    {
      int num1 = (int) Mathf.Sqrt(100f);
      float num2 = 100f;
      int index1 = -1;
      closestPointOnCurve = (Vector2) Vector3.zero;
      for (int index2 = this.m_DrawOrder.Count - 1; index2 >= 0; --index2)
      {
        CurveWrapper curveWrapperFromId = this.GetCurveWrapperFromID(this.m_DrawOrder[index2]);
        if (!curveWrapperFromId.hidden && !curveWrapperFromId.readOnly)
        {
          Vector2 drawingTransformPoint = this.OffsetViewToDrawingTransformPoint(curveWrapperFromId, viewPos);
          Vector2 lhs;
          lhs.x = drawingTransformPoint.x - (float) num1 / this.scale.x;
          lhs.y = curveWrapperFromId.renderer.EvaluateCurveSlow(lhs.x);
          lhs = this.DrawingToOffsetViewTransformPoint(curveWrapperFromId, lhs);
          for (int index3 = -num1; index3 < num1; ++index3)
          {
            Vector2 viewTransformPoint;
            viewTransformPoint.x = drawingTransformPoint.x + (float) (index3 + 1) / this.scale.x;
            viewTransformPoint.y = curveWrapperFromId.renderer.EvaluateCurveSlow(viewTransformPoint.x);
            viewTransformPoint = this.DrawingToOffsetViewTransformPoint(curveWrapperFromId, viewTransformPoint);
            float num3 = HandleUtility.DistancePointLine((Vector3) viewPos, (Vector3) lhs, (Vector3) viewTransformPoint);
            float num4 = num3 * num3;
            if ((double) num4 < (double) num2)
            {
              num2 = num4;
              index1 = curveWrapperFromId.listIndex;
              closestPointOnCurve = (Vector2) HandleUtility.ProjectPointLine((Vector3) viewPos, (Vector3) lhs, (Vector3) viewTransformPoint);
            }
            lhs = viewTransformPoint;
          }
        }
      }
      if (index1 >= 0)
        closestPointOnCurve = this.OffsetViewToDrawingTransformPoint(this.m_AnimationCurves[index1], closestPointOnCurve);
      return index1;
    }

    private void CreateKeyFromClick(object obj)
    {
      string str = "Add Key";
      this.SaveKeySelection(str);
      List<int> keyFromClick = this.CreateKeyFromClick((Vector2) obj);
      if (keyFromClick.Count <= 0)
        return;
      this.UpdateCurves(keyFromClick, str);
    }

    private List<int> CreateKeyFromClick(Vector2 viewPos)
    {
      List<int> intList = new List<int>();
      int curveIndex = this.OnlyOneEditableCurve();
      if (curveIndex >= 0)
      {
        CurveWrapper animationCurve = this.m_AnimationCurves[curveIndex];
        Vector2 drawingTransformPoint = this.OffsetViewToDrawingTransformPoint(animationCurve, viewPos);
        float num = drawingTransformPoint.x - animationCurve.timeOffset;
        if (animationCurve.curve.keys.Length == 0 || (double) num < (double) animationCurve.curve.keys[0].time || (double) num > (double) animationCurve.curve.keys[animationCurve.curve.keys.Length - 1].time)
        {
          if (this.CreateKeyFromClick(curveIndex, drawingTransformPoint))
            intList.Add(animationCurve.id);
          return intList;
        }
      }
      Vector2 closestPointOnCurve;
      int curveAtPosition = this.GetCurveAtPosition(viewPos, out closestPointOnCurve);
      if (this.CreateKeyFromClick(curveAtPosition, closestPointOnCurve.x) && curveAtPosition >= 0)
        intList.Add(this.m_AnimationCurves[curveAtPosition].id);
      return intList;
    }

    private bool CreateKeyFromClick(int curveIndex, float time)
    {
      time = Mathf.Clamp(time, this.settings.hRangeMin, this.settings.hRangeMax);
      if (curveIndex >= 0)
      {
        CurveSelection curveSelection1 = (CurveSelection) null;
        CurveWrapper animationCurve1 = this.m_AnimationCurves[curveIndex];
        if (animationCurve1.animationIsEditable)
        {
          if (animationCurve1.groupId == -1)
          {
            curveSelection1 = this.AddKeyAtTime(animationCurve1, time);
          }
          else
          {
            foreach (CurveWrapper animationCurve2 in this.m_AnimationCurves)
            {
              if (animationCurve2.groupId == animationCurve1.groupId)
              {
                CurveSelection curveSelection2 = this.AddKeyAtTime(animationCurve2, time);
                if (animationCurve2.id == animationCurve1.id)
                  curveSelection1 = curveSelection2;
              }
            }
          }
          if (curveSelection1 != null)
          {
            this.ClearSelection();
            this.AddSelection(curveSelection1);
            this.RecalcSecondarySelection();
          }
          else
            this.SelectNone();
          return true;
        }
      }
      return false;
    }

    private bool CreateKeyFromClick(int curveIndex, Vector2 localPos)
    {
      localPos.x = Mathf.Clamp(localPos.x, this.settings.hRangeMin, this.settings.hRangeMax);
      if (curveIndex >= 0)
      {
        CurveSelection curveSelection = (CurveSelection) null;
        CurveWrapper animationCurve1 = this.m_AnimationCurves[curveIndex];
        if (animationCurve1.animationIsEditable)
        {
          if (animationCurve1.groupId == -1)
          {
            curveSelection = this.AddKeyAtPosition(animationCurve1, localPos);
          }
          else
          {
            foreach (CurveWrapper animationCurve2 in this.m_AnimationCurves)
            {
              if (animationCurve2.groupId == animationCurve1.groupId)
              {
                if (animationCurve2.id == animationCurve1.id)
                  curveSelection = this.AddKeyAtPosition(animationCurve2, localPos);
                else
                  this.AddKeyAtTime(animationCurve2, localPos.x);
              }
            }
          }
          if (curveSelection != null)
          {
            this.ClearSelection();
            this.AddSelection(curveSelection);
            this.RecalcSecondarySelection();
          }
          else
            this.SelectNone();
          return true;
        }
      }
      return false;
    }

    public void AddKey(CurveWrapper cw, Keyframe key)
    {
      CurveSelection curveSelection = this.AddKeyframeAndSelect(key, cw);
      if (curveSelection != null)
      {
        this.ClearSelection();
        this.AddSelection(curveSelection);
        this.RecalcSecondarySelection();
      }
      else
        this.SelectNone();
    }

    private CurveSelection AddKeyAtTime(CurveWrapper cw, float time)
    {
      time = this.SnapTime(time);
      float num1 = (double) this.invSnap == 0.0 ? 0.0001f : 0.5f / this.invSnap;
      if (CurveUtility.HaveKeysInRange(cw.curve, time - num1, time + num1))
        return (CurveSelection) null;
      float curveDeltaSlow = cw.renderer.EvaluateCurveDeltaSlow(time);
      float num2 = this.ClampVerticalValue(this.SnapValue(cw.renderer.EvaluateCurveSlow(time)), cw.id);
      return this.AddKeyframeAndSelect(new Keyframe(time, num2, curveDeltaSlow, curveDeltaSlow), cw);
    }

    private CurveSelection AddKeyAtPosition(CurveWrapper cw, Vector2 position)
    {
      position.x = this.SnapTime(position.x);
      float num1 = (double) this.invSnap == 0.0 ? 0.0001f : 0.5f / this.invSnap;
      if (CurveUtility.HaveKeysInRange(cw.curve, position.x - num1, position.x + num1))
        return (CurveSelection) null;
      float num2 = 0.0f;
      return this.AddKeyframeAndSelect(new Keyframe(position.x, this.SnapValue(position.y), num2, num2), cw);
    }

    private CurveSelection AddKeyframeAndSelect(Keyframe key, CurveWrapper cw)
    {
      if (!cw.animationIsEditable)
        return (CurveSelection) null;
      int num = cw.AddKey(key);
      CurveUtility.SetKeyModeFromContext(cw.curve, num);
      AnimationUtility.UpdateTangentsFromModeSurrounding(cw.curve, num);
      CurveSelection curveSelection = new CurveSelection(cw.id, num);
      cw.selected = CurveWrapper.SelectionMode.Selected;
      cw.changed = true;
      return curveSelection;
    }

    private CurveSelection FindNearest()
    {
      Vector2 mousePosition = Event.current.mousePosition;
      bool flag = false;
      int curveID = -1;
      int key1 = -1;
      float num = 100f;
      for (int index = this.m_DrawOrder.Count - 1; index >= 0; --index)
      {
        CurveWrapper curveWrapperFromId = this.GetCurveWrapperFromID(this.m_DrawOrder[index]);
        if (!curveWrapperFromId.readOnly && !curveWrapperFromId.hidden)
        {
          for (int key2 = 0; key2 < curveWrapperFromId.curve.keys.Length; ++key2)
          {
            Keyframe key3 = curveWrapperFromId.curve.keys[key2];
            float sqrMagnitude = (this.GetGUIPoint(curveWrapperFromId, (Vector3) new Vector2(key3.time, key3.value)) - mousePosition).sqrMagnitude;
            if ((double) sqrMagnitude <= 16.0)
              return new CurveSelection(curveWrapperFromId.id, key2);
            if ((double) sqrMagnitude < (double) num)
            {
              flag = true;
              curveID = curveWrapperFromId.id;
              key1 = key2;
              num = sqrMagnitude;
            }
          }
          if (index == this.m_DrawOrder.Count - 1 && curveID >= 0)
            num = 16f;
        }
      }
      if (flag)
        return new CurveSelection(curveID, key1);
      return (CurveSelection) null;
    }

    public void SelectNone()
    {
      this.ClearSelection();
      foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
        animationCurve.selected = CurveWrapper.SelectionMode.None;
    }

    public void SelectAll()
    {
      int capacity = 0;
      foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
      {
        if (!animationCurve.hidden)
          capacity += animationCurve.curve.length;
      }
      List<CurveSelection> curveSelectionList = new List<CurveSelection>(capacity);
      foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
      {
        animationCurve.selected = CurveWrapper.SelectionMode.Selected;
        for (int key = 0; key < animationCurve.curve.length; ++key)
          curveSelectionList.Add(new CurveSelection(animationCurve.id, key));
      }
      this.selectedCurves = curveSelectionList;
    }

    public bool IsDraggingKey()
    {
      return this.m_DraggingKey != null;
    }

    public bool IsDraggingCurveOrRegion()
    {
      return this.m_DraggingCurveOrRegion != null;
    }

    public bool IsDraggingCurve(CurveWrapper cw)
    {
      return this.m_DraggingCurveOrRegion != null && this.m_DraggingCurveOrRegion.Length == 1 && this.m_DraggingCurveOrRegion[0] == cw;
    }

    public bool IsDraggingRegion(CurveWrapper cw1, CurveWrapper cw2)
    {
      return this.m_DraggingCurveOrRegion != null && this.m_DraggingCurveOrRegion.Length == 2 && (this.m_DraggingCurveOrRegion[0] == cw1 || this.m_DraggingCurveOrRegion[0] == cw2);
    }

    private bool HandleCurveAndRegionMoveToFrontOnMouseDown(ref Vector2 timeValue, ref CurveWrapper[] curves)
    {
      Vector2 closestPointOnCurve;
      int curveAtPosition = this.GetCurveAtPosition(Event.current.mousePosition, out closestPointOnCurve);
      if (curveAtPosition >= 0)
      {
        this.MoveCurveToFront(this.m_AnimationCurves[curveAtPosition].id);
        timeValue = this.OffsetMousePositionInDrawing(this.m_AnimationCurves[curveAtPosition]);
        curves = new CurveWrapper[1]
        {
          this.m_AnimationCurves[curveAtPosition]
        };
        return true;
      }
      for (int index = this.m_DrawOrder.Count - 1; index >= 0; --index)
      {
        CurveWrapper curveWrapperFromId = this.GetCurveWrapperFromID(this.m_DrawOrder[index]);
        if (curveWrapperFromId != null && !curveWrapperFromId.hidden && curveWrapperFromId.curve.length != 0)
        {
          CurveWrapper curveWrapper = (CurveWrapper) null;
          if (index > 0)
            curveWrapper = this.GetCurveWrapperFromID(this.m_DrawOrder[index - 1]);
          if (this.IsRegion(curveWrapperFromId, curveWrapper))
          {
            Vector2 vector2_1 = this.OffsetMousePositionInDrawing(curveWrapperFromId);
            Vector2 vector2_2 = this.OffsetMousePositionInDrawing(curveWrapper);
            float num1 = curveWrapperFromId.renderer.EvaluateCurveSlow(vector2_1.x);
            float num2 = curveWrapper.renderer.EvaluateCurveSlow(vector2_2.x);
            if ((double) num1 > (double) num2)
            {
              float num3 = num1;
              num1 = num2;
              num2 = num3;
            }
            if ((double) vector2_1.y >= (double) num1 && (double) vector2_1.y <= (double) num2)
            {
              timeValue = vector2_1;
              curves = new CurveWrapper[2]
              {
                curveWrapperFromId,
                curveWrapper
              };
              this.MoveCurveToFront(curveWrapperFromId.id);
              return true;
            }
            --index;
          }
        }
      }
      return false;
    }

    private void SelectPoints()
    {
      int controlId = GUIUtility.GetControlID(897560, FocusType.Passive);
      Event current = Event.current;
      bool shift = current.shift;
      bool actionKey = EditorGUI.actionKey;
      EventType typeForControl = current.GetTypeForControl(controlId);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          if (current.clickCount == 2 && current.button == 0)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey4 pointsCAnonStorey4 = new CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey4();
            // ISSUE: reference to a compiler-generated field
            pointsCAnonStorey4.selectedPoint = this.FindNearest();
            // ISSUE: reference to a compiler-generated field
            if (pointsCAnonStorey4.selectedPoint != null)
            {
              if (!shift)
                this.ClearSelection();
              // ISSUE: reference to a compiler-generated field
              AnimationCurve curveFromSelection = this.GetCurveFromSelection(pointsCAnonStorey4.selectedPoint);
              if (curveFromSelection != null)
              {
                this.BeginRangeSelection();
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey5 pointsCAnonStorey5 = new CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey5();
                // ISSUE: reference to a compiler-generated field
                pointsCAnonStorey5.\u003C\u003Ef__ref\u00244 = pointsCAnonStorey4;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                for (pointsCAnonStorey5.keyIndex = 0; pointsCAnonStorey5.keyIndex < curveFromSelection.keys.Length; ++pointsCAnonStorey5.keyIndex)
                {
                  // ISSUE: reference to a compiler-generated method
                  if (!this.selectedCurves.Any<CurveSelection>(new Func<CurveSelection, bool>(pointsCAnonStorey5.\u003C\u003Em__0)))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.AddSelection(new CurveSelection(pointsCAnonStorey4.selectedPoint.curveID, pointsCAnonStorey5.keyIndex));
                  }
                }
                this.EndRangeSelection();
              }
            }
            else
            {
              this.SaveKeySelection("Add Key");
              List<int> keyFromClick = this.CreateKeyFromClick(Event.current.mousePosition);
              if (keyFromClick.Count > 0)
              {
                foreach (int curveID in keyFromClick)
                  this.GetCurveWrapperFromID(curveID).changed = true;
                GUI.changed = true;
              }
            }
            current.Use();
            break;
          }
          if (current.button == 0)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey6 pointsCAnonStorey6 = new CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey6();
            // ISSUE: reference to a compiler-generated field
            pointsCAnonStorey6.selectedPoint = this.FindNearest();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (pointsCAnonStorey6.selectedPoint == null || pointsCAnonStorey6.selectedPoint.semiSelected)
            {
              Vector2 zero = Vector2.zero;
              CurveWrapper[] curves = (CurveWrapper[]) null;
              bool frontOnMouseDown = this.HandleCurveAndRegionMoveToFrontOnMouseDown(ref zero, ref curves);
              if (!shift && !actionKey && !frontOnMouseDown)
                this.SelectNone();
              GUIUtility.hotControl = controlId;
              this.s_EndMouseDragPosition = this.s_StartMouseDragPosition = current.mousePosition;
              this.s_PickMode = CurveEditor.PickMode.Click;
              if (!frontOnMouseDown)
                current.Use();
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.MoveCurveToFront(pointsCAnonStorey6.selectedPoint.curveID);
              // ISSUE: reference to a compiler-generated field
              Keyframe keyframeFromSelection = this.GetKeyframeFromSelection(pointsCAnonStorey6.selectedPoint);
              this.s_StartKeyDragPosition = new Vector2(keyframeFromSelection.time, keyframeFromSelection.value);
              if (shift)
              {
                bool flag = false;
                // ISSUE: reference to a compiler-generated field
                int a1 = pointsCAnonStorey6.selectedPoint.key;
                // ISSUE: reference to a compiler-generated field
                int a2 = pointsCAnonStorey6.selectedPoint.key;
                for (int index = 0; index < this.selectedCurves.Count; ++index)
                {
                  CurveSelection selectedCurve = this.selectedCurves[index];
                  // ISSUE: reference to a compiler-generated field
                  if (selectedCurve.curveID == pointsCAnonStorey6.selectedPoint.curveID)
                  {
                    flag = true;
                    a1 = Mathf.Min(a1, selectedCurve.key);
                    a2 = Mathf.Max(a2, selectedCurve.key);
                  }
                }
                if (!flag)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (!this.selectedCurves.Contains(pointsCAnonStorey6.selectedPoint))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.AddSelection(pointsCAnonStorey6.selectedPoint);
                  }
                }
                else
                {
                  this.BeginRangeSelection();
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey7 pointsCAnonStorey7 = new CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey7();
                  // ISSUE: reference to a compiler-generated field
                  pointsCAnonStorey7.\u003C\u003Ef__ref\u00246 = pointsCAnonStorey6;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  for (pointsCAnonStorey7.keyIndex = a1; pointsCAnonStorey7.keyIndex <= a2; ++pointsCAnonStorey7.keyIndex)
                  {
                    // ISSUE: reference to a compiler-generated method
                    if (!this.selectedCurves.Any<CurveSelection>(new Func<CurveSelection, bool>(pointsCAnonStorey7.\u003C\u003Em__0)))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.AddSelection(new CurveSelection(pointsCAnonStorey6.selectedPoint.curveID, pointsCAnonStorey7.keyIndex));
                    }
                  }
                  this.EndRangeSelection();
                }
                Event.current.Use();
              }
              else if (actionKey)
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.selectedCurves.Contains(pointsCAnonStorey6.selectedPoint))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.AddSelection(pointsCAnonStorey6.selectedPoint);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.RemoveSelection(pointsCAnonStorey6.selectedPoint);
                }
                Event.current.Use();
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.selectedCurves.Contains(pointsCAnonStorey6.selectedPoint))
                {
                  this.ClearSelection();
                  // ISSUE: reference to a compiler-generated field
                  this.AddSelection(pointsCAnonStorey6.selectedPoint);
                }
              }
              this.RecalcSecondarySelection();
              this.RecalcCurveSelection();
            }
            HandleUtility.Repaint();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            if (this.s_PickMode != CurveEditor.PickMode.Click)
            {
              HashSet<int> intSet = new HashSet<int>();
              for (int index = 0; index < this.selectedCurves.Count; ++index)
              {
                CurveWrapper wrapperFromSelection = this.GetCurveWrapperFromSelection(this.selectedCurves[index]);
                if (!intSet.Contains(wrapperFromSelection.id))
                {
                  this.MoveCurveToFront(wrapperFromSelection.id);
                  intSet.Add(wrapperFromSelection.id);
                }
              }
            }
            GUIUtility.hotControl = 0;
            this.s_PickMode = CurveEditor.PickMode.None;
            Event.current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            this.s_EndMouseDragPosition = current.mousePosition;
            if (this.s_PickMode == CurveEditor.PickMode.Click)
            {
              this.s_PickMode = CurveEditor.PickMode.Marquee;
              this.s_SelectionBackup = shift || actionKey ? new List<CurveSelection>((IEnumerable<CurveSelection>) this.selectedCurves) : new List<CurveSelection>();
            }
            else
            {
              Rect rect = EditorGUIExt.FromToRect(this.s_StartMouseDragPosition, current.mousePosition);
              List<CurveSelection> curveSelectionList = new List<CurveSelection>((IEnumerable<CurveSelection>) this.s_SelectionBackup);
              for (int index = 0; index < this.m_AnimationCurves.Length; ++index)
              {
                CurveWrapper animationCurve = this.m_AnimationCurves[index];
                if (!animationCurve.readOnly && !animationCurve.hidden)
                {
                  Rect worldRect = this.GetWorldRect(animationCurve, rect);
                  if (this.GetCurveRect(animationCurve).Overlaps(worldRect))
                  {
                    int key1 = 0;
                    foreach (Keyframe key2 in animationCurve.curve.keys)
                    {
                      if (worldRect.Contains(new Vector2(key2.time, key2.value)))
                        curveSelectionList.Add(new CurveSelection(animationCurve.id, key1));
                      ++key1;
                    }
                  }
                }
              }
              this.selectedCurves = curveSelectionList;
              if (this.s_SelectionBackup.Count > 0)
                this.selectedCurves.Sort();
              this.RecalcSecondarySelection();
              this.RecalcCurveSelection();
            }
            current.Use();
            break;
          }
          break;
        default:
          if (typeForControl != EventType.Layout)
          {
            if (typeForControl == EventType.ContextClick)
            {
              Rect drawRect = this.drawRect;
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              Rect& local = @drawRect;
              float num1 = 0.0f;
              drawRect.y = num1;
              double num2 = (double) num1;
              // ISSUE: explicit reference operation
              (^local).x = (float) num2;
              if (drawRect.Contains(Event.current.mousePosition))
              {
                Vector2 closestPointOnCurve;
                int curveAtPosition = this.GetCurveAtPosition(Event.current.mousePosition, out closestPointOnCurve);
                if (curveAtPosition >= 0)
                {
                  GenericMenu genericMenu = new GenericMenu();
                  if (this.m_AnimationCurves[curveAtPosition].animationIsEditable)
                    genericMenu.AddItem(new GUIContent("Add Key"), false, new GenericMenu.MenuFunction2(this.CreateKeyFromClick), (object) Event.current.mousePosition);
                  else
                    genericMenu.AddDisabledItem(new GUIContent("Add Key"));
                  genericMenu.ShowAsContext();
                  Event.current.Use();
                }
                break;
              }
              break;
            }
            break;
          }
          HandleUtility.AddDefaultControl(controlId);
          break;
      }
      if (this.s_PickMode != CurveEditor.PickMode.Marquee)
        return;
      GUI.Label(EditorGUIExt.FromToRect(this.s_StartMouseDragPosition, this.s_EndMouseDragPosition), GUIContent.none, CurveEditor.Styles.selectionRect);
    }

    private void EditAxisLabels()
    {
      int controlId = GUIUtility.GetControlID(18975602, FocusType.Keyboard);
      List<CurveWrapper> curveWrapperList = new List<CurveWrapper>();
      Vector2 axisUiScalars = this.GetAxisUiScalars(curveWrapperList);
      if ((double) axisUiScalars.y < 0.0 || curveWrapperList.Count <= 0 || curveWrapperList[0].setAxisUiScalarsCallback == null)
        return;
      Rect position1 = new Rect(0.0f, this.topmargin - 8f, this.leftmargin - 4f, 16f);
      Rect position2 = position1;
      position2.y -= position1.height;
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (current.button == 0)
          {
            if (position2.Contains(Event.current.mousePosition) && GUIUtility.hotControl == 0)
            {
              GUIUtility.keyboardControl = 0;
              GUIUtility.hotControl = controlId;
              GUI.changed = true;
              current.Use();
            }
            if (!position1.Contains(Event.current.mousePosition))
              GUIUtility.keyboardControl = 0;
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            GUIUtility.hotControl = 0;
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            float num = Mathf.Clamp01(Mathf.Max(axisUiScalars.y, Mathf.Pow(Mathf.Abs(axisUiScalars.y), 0.5f)) * 0.01f);
            axisUiScalars.y += HandleUtility.niceMouseDelta * num;
            if ((double) axisUiScalars.y < 1.0 / 1000.0)
              axisUiScalars.y = 1f / 1000f;
            this.SetAxisUiScalars(axisUiScalars, curveWrapperList);
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          if (GUIUtility.hotControl == 0)
          {
            EditorGUIUtility.AddCursorRect(position2, MouseCursor.SlideArrow);
            break;
          }
          break;
      }
      string fieldFormatString = EditorGUI.kFloatFieldFormatString;
      EditorGUI.kFloatFieldFormatString = this.m_AxisLabelFormat;
      float y = EditorGUI.FloatField(position1, axisUiScalars.y, CurveEditor.Styles.axisLabelNumberField);
      if ((double) axisUiScalars.y != (double) y)
        this.SetAxisUiScalars(new Vector2(axisUiScalars.x, y), curveWrapperList);
      EditorGUI.kFloatFieldFormatString = fieldFormatString;
    }

    public void BeginTimeRangeSelection(float time, bool addToSelection)
    {
      if (this.s_TimeRangeSelectionActive)
      {
        Debug.LogError((object) "BeginTimeRangeSelection can only be called once");
      }
      else
      {
        this.s_TimeRangeSelectionActive = true;
        this.s_TimeRangeSelectionStart = this.s_TimeRangeSelectionEnd = time;
        if (addToSelection)
          this.s_SelectionBackup = new List<CurveSelection>((IEnumerable<CurveSelection>) this.selectedCurves);
        else
          this.s_SelectionBackup = new List<CurveSelection>();
      }
    }

    public void TimeRangeSelectTo(float time)
    {
      if (!this.s_TimeRangeSelectionActive)
      {
        Debug.LogError((object) "TimeRangeSelectTo can only be called after BeginTimeRangeSelection");
      }
      else
      {
        this.s_TimeRangeSelectionEnd = time;
        List<CurveSelection> curveSelectionList = new List<CurveSelection>((IEnumerable<CurveSelection>) this.s_SelectionBackup);
        float num1 = Mathf.Min(this.s_TimeRangeSelectionStart, this.s_TimeRangeSelectionEnd);
        float num2 = Mathf.Max(this.s_TimeRangeSelectionStart, this.s_TimeRangeSelectionEnd);
        foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
        {
          if (!animationCurve.readOnly && !animationCurve.hidden)
          {
            int key1 = 0;
            foreach (Keyframe key2 in animationCurve.curve.keys)
            {
              if ((double) key2.time >= (double) num1 && (double) key2.time < (double) num2)
                curveSelectionList.Add(new CurveSelection(animationCurve.id, key1));
              ++key1;
            }
          }
        }
        this.selectedCurves = curveSelectionList;
        this.RecalcSecondarySelection();
        this.RecalcCurveSelection();
      }
    }

    public void EndTimeRangeSelection()
    {
      if (!this.s_TimeRangeSelectionActive)
      {
        Debug.LogError((object) "EndTimeRangeSelection can only be called after BeginTimeRangeSelection");
      }
      else
      {
        this.s_TimeRangeSelectionStart = this.s_TimeRangeSelectionEnd = 0.0f;
        this.s_TimeRangeSelectionActive = false;
      }
    }

    public void CancelTimeRangeSelection()
    {
      if (!this.s_TimeRangeSelectionActive)
      {
        Debug.LogError((object) "CancelTimeRangeSelection can only be called after BeginTimeRangeSelection");
      }
      else
      {
        this.selectedCurves = this.s_SelectionBackup;
        this.s_TimeRangeSelectionActive = false;
      }
    }

    private Vector2 GetPointEditionFieldPosition()
    {
      return new Vector2(this.selectedCurves.Min<CurveSelection>((Func<CurveSelection, float>) (x => this.GetKeyframeFromSelection(x).time)) + this.selectedCurves.Max<CurveSelection>((Func<CurveSelection, float>) (x => this.GetKeyframeFromSelection(x).time)), this.selectedCurves.Min<CurveSelection>((Func<CurveSelection, float>) (x => this.GetKeyframeFromSelection(x).value)) + this.selectedCurves.Max<CurveSelection>((Func<CurveSelection, float>) (x => this.GetKeyframeFromSelection(x).value))) * 0.5f;
    }

    private void StartEditingSelectedPointsContext(object fieldPosition)
    {
      this.StartEditingSelectedPoints((Vector2) fieldPosition);
    }

    private void StartEditingSelectedPoints()
    {
      this.StartEditingSelectedPoints(this.GetPointEditionFieldPosition());
    }

    private void StartEditingSelectedPoints(Vector2 fieldPosition)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CurveEditor.\u003CStartEditingSelectedPoints\u003Ec__AnonStorey8 pointsCAnonStorey8 = new CurveEditor.\u003CStartEditingSelectedPoints\u003Ec__AnonStorey8();
      // ISSUE: reference to a compiler-generated field
      pointsCAnonStorey8.\u0024this = this;
      this.m_PointEditingFieldPosition = fieldPosition;
      this.m_FocusedPointField = "pointValueField";
      this.m_TimeWasEdited = false;
      this.m_ValueWasEdited = false;
      this.m_NewTime = 0.0f;
      this.m_NewValue = 0.0f;
      // ISSUE: reference to a compiler-generated field
      pointsCAnonStorey8.keyframe = this.GetKeyframeFromSelection(this.selectedCurves[0]);
      // ISSUE: reference to a compiler-generated method
      if (this.selectedCurves.All<CurveSelection>(new Func<CurveSelection, bool>(pointsCAnonStorey8.\u003C\u003Em__0)))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_NewTime = pointsCAnonStorey8.keyframe.time;
      }
      // ISSUE: reference to a compiler-generated method
      if (this.selectedCurves.All<CurveSelection>(new Func<CurveSelection, bool>(pointsCAnonStorey8.\u003C\u003Em__1)))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_NewValue = pointsCAnonStorey8.keyframe.value;
      }
      this.m_EditingPoints = true;
    }

    private void FinishEditingPoints()
    {
      this.m_EditingPoints = false;
    }

    private void EditSelectedPoints()
    {
      Event current = Event.current;
      if (this.m_EditingPoints && !this.hasSelection)
        this.m_EditingPoints = false;
      bool flag = false;
      if (current.type == EventType.KeyDown)
      {
        if (current.keyCode == KeyCode.KeypadEnter || current.keyCode == KeyCode.Return)
        {
          if (this.hasSelection && !this.m_EditingPoints)
          {
            this.StartEditingSelectedPoints();
            current.Use();
          }
          else if (this.m_EditingPoints)
          {
            this.SetSelectedKeyPositions(this.m_NewTime, this.m_NewValue, this.m_TimeWasEdited, this.m_ValueWasEdited);
            this.FinishEditingPoints();
            GUI.changed = true;
            current.Use();
          }
        }
        else if (current.keyCode == KeyCode.Escape)
          flag = true;
      }
      if (!this.m_EditingPoints)
        return;
      Vector2 viewTransformPoint = this.DrawingToViewTransformPoint(this.m_PointEditingFieldPosition);
      Rect rect = Rect.MinMaxRect(this.leftmargin, this.topmargin, this.rect.width - this.rightmargin, this.rect.height - this.bottommargin);
      viewTransformPoint.x = Mathf.Clamp(viewTransformPoint.x, rect.xMin, rect.xMax - 80f);
      viewTransformPoint.y = Mathf.Clamp(viewTransformPoint.y, rect.yMin, rect.yMax - 36f);
      EditorGUI.BeginChangeCheck();
      GUI.SetNextControlName("pointTimeField");
      this.m_NewTime = this.PointFieldForSelection(new Rect(viewTransformPoint.x, viewTransformPoint.y, 80f, 18f), 1, this.m_NewTime, (Func<CurveSelection, float>) (x => this.GetKeyframeFromSelection(x).time), (Func<Rect, int, float, float>) ((r, id, time) => this.TimeField(r, id, time, this.invSnap, this.timeFormat)), "time");
      if (EditorGUI.EndChangeCheck())
        this.m_TimeWasEdited = true;
      EditorGUI.BeginChangeCheck();
      GUI.SetNextControlName("pointValueField");
      this.m_NewValue = this.PointFieldForSelection(new Rect(viewTransformPoint.x, viewTransformPoint.y + 18f, 80f, 18f), 2, this.m_NewValue, (Func<CurveSelection, float>) (x => this.GetKeyframeFromSelection(x).value), (Func<Rect, int, float, float>) ((r, id, value) => this.ValueField(r, id, value)), "value");
      if (EditorGUI.EndChangeCheck())
        this.m_ValueWasEdited = true;
      if (flag)
        this.FinishEditingPoints();
      if (this.m_FocusedPointField != null)
      {
        EditorGUI.FocusTextInControl(this.m_FocusedPointField);
        if (current.type == EventType.Repaint)
          this.m_FocusedPointField = (string) null;
      }
      if (current.type == EventType.KeyDown && ((int) current.character == 9 || (int) current.character == 25))
      {
        if (this.m_TimeWasEdited || this.m_ValueWasEdited)
        {
          this.SetSelectedKeyPositions(this.m_NewTime, this.m_NewValue, this.m_TimeWasEdited, this.m_ValueWasEdited);
          this.m_PointEditingFieldPosition = this.GetPointEditionFieldPosition();
        }
        this.m_FocusedPointField = !(GUI.GetNameOfFocusedControl() == "pointValueField") ? "pointValueField" : "pointTimeField";
        current.Use();
      }
      if (current.type != EventType.MouseDown)
        return;
      this.FinishEditingPoints();
    }

    private float PointFieldForSelection(Rect rect, int customID, float value, Func<CurveSelection, float> memberGetter, Func<Rect, int, float, float> fieldCreator, string label)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CurveEditor.\u003CPointFieldForSelection\u003Ec__AnonStorey9 selectionCAnonStorey9 = new CurveEditor.\u003CPointFieldForSelection\u003Ec__AnonStorey9();
      // ISSUE: reference to a compiler-generated field
      selectionCAnonStorey9.memberGetter = memberGetter;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      selectionCAnonStorey9.firstSelectedValue = selectionCAnonStorey9.memberGetter(this.selectedCurves[0]);
      // ISSUE: reference to a compiler-generated method
      if (!this.selectedCurves.All<CurveSelection>(new Func<CurveSelection, bool>(selectionCAnonStorey9.\u003C\u003Em__0)))
        EditorGUI.showMixedValue = true;
      Rect position = rect;
      position.x -= position.width;
      int controlId = GUIUtility.GetControlID(customID, FocusType.Keyboard, rect);
      Color color = GUI.color;
      GUI.color = Color.white;
      GUI.Label(position, label, CurveEditor.Styles.rightAlignedLabel);
      value = fieldCreator(rect, controlId, value);
      GUI.color = color;
      EditorGUI.showMixedValue = false;
      return value;
    }

    private void SetupKeyOrCurveDragging(Vector2 timeValue, CurveWrapper cw, int id, Vector2 mousePos)
    {
      this.m_DraggedCoord = timeValue;
      this.m_DraggingKey = cw;
      GUIUtility.hotControl = id;
      this.s_StartMouseDragPosition = mousePos;
    }

    public Vector2 MovePoints()
    {
      int controlId = GUIUtility.GetControlID(FocusType.Passive);
      if (!this.hasSelection && !this.settings.allowDraggingCurvesAndRegions)
        return Vector2.zero;
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (current.button == 0)
          {
            foreach (CurveSelection selectedCurve in this.selectedCurves)
            {
              CurveWrapper wrapperFromSelection = this.GetCurveWrapperFromSelection(selectedCurve);
              if (wrapperFromSelection != null && !wrapperFromSelection.hidden && (double) (this.DrawingToOffsetViewTransformPoint(wrapperFromSelection, this.GetPosition(selectedCurve)) - current.mousePosition).sqrMagnitude <= 100.0)
              {
                Keyframe keyframeFromSelection = this.GetKeyframeFromSelection(selectedCurve);
                this.SetupKeyOrCurveDragging(new Vector2(keyframeFromSelection.time, keyframeFromSelection.value), wrapperFromSelection, controlId, current.mousePosition);
                this.m_RectangleTool.OnStartMove(this.s_StartMouseDragPosition, this.m_RectangleTool.rippleTimeClutch);
                current.Use();
                break;
              }
            }
            if (this.settings.allowDraggingCurvesAndRegions && this.m_DraggingKey == null)
            {
              Vector2 zero = Vector2.zero;
              CurveWrapper[] curves = (CurveWrapper[]) null;
              if (this.HandleCurveAndRegionMoveToFrontOnMouseDown(ref zero, ref curves))
              {
                List<CurveSelection> curveSelectionList = new List<CurveSelection>();
                foreach (CurveWrapper curveWrapper in curves)
                {
                  for (int key = 0; key < curveWrapper.curve.keys.Length; ++key)
                    curveSelectionList.Add(new CurveSelection(curveWrapper.id, key));
                  this.MoveCurveToFront(curveWrapper.id);
                }
                this.preCurveDragSelection = this.selectedCurves;
                this.selectedCurves = curveSelectionList;
                this.SetupKeyOrCurveDragging(zero, curves[0], controlId, current.mousePosition);
                this.m_DraggingCurveOrRegion = curves;
                this.m_RectangleTool.OnStartMove(this.s_StartMouseDragPosition, false);
                current.Use();
              }
            }
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            this.m_RectangleTool.OnEndMove();
            this.ResetDragging();
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            Vector2 lhs = current.mousePosition - this.s_StartMouseDragPosition;
            Vector2 vector2 = Vector2.zero;
            if (current.shift && this.m_AxisLock == CurveEditor.AxisLock.None)
              this.m_AxisLock = (double) Mathf.Abs(lhs.x) <= (double) Mathf.Abs(lhs.y) ? CurveEditor.AxisLock.Y : CurveEditor.AxisLock.X;
            if (this.m_DraggingCurveOrRegion != null)
            {
              lhs.x = 0.0f;
              vector2 = this.ViewToDrawingTransformVector(lhs);
              vector2.y = this.SnapValue(vector2.y + this.s_StartKeyDragPosition.y) - this.s_StartKeyDragPosition.y;
            }
            else
            {
              switch (this.m_AxisLock)
              {
                case CurveEditor.AxisLock.None:
                  vector2 = this.ViewToDrawingTransformVector(lhs);
                  vector2.x = this.SnapTime(vector2.x + this.s_StartKeyDragPosition.x) - this.s_StartKeyDragPosition.x;
                  vector2.y = this.SnapValue(vector2.y + this.s_StartKeyDragPosition.y) - this.s_StartKeyDragPosition.y;
                  break;
                case CurveEditor.AxisLock.X:
                  lhs.y = 0.0f;
                  vector2 = this.ViewToDrawingTransformVector(lhs);
                  vector2.x = this.SnapTime(vector2.x + this.s_StartKeyDragPosition.x) - this.s_StartKeyDragPosition.x;
                  break;
                case CurveEditor.AxisLock.Y:
                  lhs.x = 0.0f;
                  vector2 = this.ViewToDrawingTransformVector(lhs);
                  vector2.y = this.SnapValue(vector2.y + this.s_StartKeyDragPosition.y) - this.s_StartKeyDragPosition.y;
                  break;
              }
            }
            this.m_RectangleTool.OnMove(this.s_StartMouseDragPosition + vector2);
            GUI.changed = true;
            current.Use();
            return vector2;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.hotControl == controlId && current.keyCode == KeyCode.Escape)
          {
            this.TranslateSelectedKeys(Vector2.zero);
            this.ResetDragging();
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          Rect position = new Rect(current.mousePosition.x - 10f, current.mousePosition.y - 10f, 20f, 20f);
          if (this.m_DraggingCurveOrRegion != null)
          {
            EditorGUIUtility.AddCursorRect(position, MouseCursor.ResizeVertical);
            break;
          }
          if (this.m_DraggingKey != null)
          {
            EditorGUIUtility.AddCursorRect(position, MouseCursor.MoveArrow);
            break;
          }
          break;
      }
      return Vector2.zero;
    }

    private void ResetDragging()
    {
      if (this.m_DraggingCurveOrRegion != null)
      {
        this.selectedCurves = this.preCurveDragSelection;
        this.preCurveDragSelection = (List<CurveSelection>) null;
      }
      GUIUtility.hotControl = 0;
      this.m_DraggingKey = (CurveWrapper) null;
      this.m_DraggingCurveOrRegion = (CurveWrapper[]) null;
      this.m_MoveCoord = Vector2.zero;
      this.m_AxisLock = CurveEditor.AxisLock.None;
    }

    private void MakeCurveBackups()
    {
      this.SaveKeySelection("Edit Curve");
      this.m_CurveBackups = new List<CurveEditor.SavedCurve>();
      int num = -1;
      CurveEditor.SavedCurve savedCurve = (CurveEditor.SavedCurve) null;
      for (int index = 0; index < this.selectedCurves.Count; ++index)
      {
        CurveSelection selectedCurve = this.selectedCurves[index];
        if (selectedCurve.curveID != num)
        {
          AnimationCurve curveFromSelection = this.GetCurveFromSelection(selectedCurve);
          if (curveFromSelection != null)
          {
            savedCurve = new CurveEditor.SavedCurve();
            num = savedCurve.curveId = selectedCurve.curveID;
            Keyframe[] keys = curveFromSelection.keys;
            savedCurve.keys = new List<CurveEditor.SavedCurve.SavedKeyFrame>(keys.Length);
            foreach (Keyframe key in keys)
              savedCurve.keys.Add(new CurveEditor.SavedCurve.SavedKeyFrame(key, CurveWrapper.SelectionMode.None));
            this.m_CurveBackups.Add(savedCurve);
          }
        }
        savedCurve.keys[selectedCurve.key].selected = !selectedCurve.semiSelected ? CurveWrapper.SelectionMode.Selected : CurveWrapper.SelectionMode.SemiSelected;
      }
    }

    public void SaveKeySelection(string undoLabel)
    {
      if (!this.settings.undoRedoSelection)
        return;
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.selection, undoLabel);
    }

    private Vector2 GetPosition(CurveSelection selection)
    {
      Keyframe keyframeFromSelection = this.GetKeyframeFromSelection(selection);
      Vector2 vector2 = new Vector2(keyframeFromSelection.time, keyframeFromSelection.value);
      float num = 50f;
      if (selection.type == CurveSelection.SelectionType.InTangent)
      {
        Vector2 vec = new Vector2(1f, keyframeFromSelection.inTangent);
        if ((double) keyframeFromSelection.inTangent == double.PositiveInfinity)
          vec = new Vector2(0.0f, -1f);
        vec = this.NormalizeInViewSpace(vec);
        return vector2 - vec * num;
      }
      if (selection.type != CurveSelection.SelectionType.OutTangent)
        return vector2;
      Vector2 vec1 = new Vector2(1f, keyframeFromSelection.outTangent);
      if ((double) keyframeFromSelection.outTangent == double.PositiveInfinity)
        vec1 = new Vector2(0.0f, -1f);
      vec1 = this.NormalizeInViewSpace(vec1);
      return vector2 + vec1 * num;
    }

    private void MoveCurveToFront(int curveID)
    {
      int count = this.m_DrawOrder.Count;
      for (int index = 0; index < count; ++index)
      {
        if (this.m_DrawOrder[index] == curveID)
        {
          int regionId = this.GetCurveWrapperFromID(curveID).regionId;
          if (regionId >= 0)
          {
            int num1 = 0;
            int num2 = -1;
            if (index - 1 >= 0)
            {
              int curveID1 = this.m_DrawOrder[index - 1];
              if (regionId == this.GetCurveWrapperFromID(curveID1).regionId)
              {
                num2 = curveID1;
                num1 = -1;
              }
            }
            if (index + 1 < count && num2 < 0)
            {
              int curveID1 = this.m_DrawOrder[index + 1];
              if (regionId == this.GetCurveWrapperFromID(curveID1).regionId)
              {
                num2 = curveID1;
                num1 = 0;
              }
            }
            if (num2 >= 0)
            {
              this.m_DrawOrder.RemoveRange(index + num1, 2);
              this.m_DrawOrder.Add(num2);
              this.m_DrawOrder.Add(curveID);
              this.ValidateCurveList();
              break;
            }
            Debug.LogError((object) "Unhandled region");
          }
          else
          {
            if (index == count - 1)
              break;
            this.m_DrawOrder.RemoveAt(index);
            this.m_DrawOrder.Add(curveID);
            this.ValidateCurveList();
            break;
          }
        }
      }
    }

    private bool IsCurveSelected(CurveWrapper cw)
    {
      if (cw != null)
        return cw.selected != CurveWrapper.SelectionMode.None;
      return false;
    }

    private bool IsRegionCurveSelected(CurveWrapper cw1, CurveWrapper cw2)
    {
      return this.IsCurveSelected(cw1) || this.IsCurveSelected(cw2);
    }

    private bool IsRegion(CurveWrapper cw1, CurveWrapper cw2)
    {
      if (cw1 != null && cw2 != null && cw1.regionId >= 0)
        return cw1.regionId == cw2.regionId;
      return false;
    }

    private bool IsLeftTangentEditable(CurveSelection selection)
    {
      switch (AnimationUtility.GetKeyLeftTangentMode(this.GetKeyframeFromSelection(selection)))
      {
        case AnimationUtility.TangentMode.Free:
          return true;
        case AnimationUtility.TangentMode.Auto:
        case AnimationUtility.TangentMode.ClampedAuto:
          return true;
        default:
          return false;
      }
    }

    private bool IsRightTangentEditable(CurveSelection selection)
    {
      switch (AnimationUtility.GetKeyRightTangentMode(this.GetKeyframeFromSelection(selection)))
      {
        case AnimationUtility.TangentMode.Free:
          return true;
        case AnimationUtility.TangentMode.Auto:
        case AnimationUtility.TangentMode.ClampedAuto:
          return true;
        default:
          return false;
      }
    }

    private void DrawCurvesAndRegion(CurveWrapper cw1, CurveWrapper cw2, List<CurveSelection> selection, bool hasFocus)
    {
      this.DrawRegion(cw1, cw2, hasFocus);
      this.DrawCurveAndPoints(cw1, !this.IsCurveSelected(cw1) ? (List<CurveSelection>) null : selection, hasFocus);
      this.DrawCurveAndPoints(cw2, !this.IsCurveSelected(cw2) ? (List<CurveSelection>) null : selection, hasFocus);
    }

    private void DrawCurveAndPoints(CurveWrapper cw, List<CurveSelection> selection, bool hasFocus)
    {
      this.DrawCurve(cw, hasFocus);
      this.DrawPointsOnCurve(cw, selection, hasFocus);
    }

    private bool ShouldCurveHaveFocus(int indexIntoDrawOrder, CurveWrapper cw1, CurveWrapper cw2)
    {
      bool flag = false;
      if (indexIntoDrawOrder == this.m_DrawOrder.Count - 1)
        flag = true;
      else if (this.hasSelection)
        flag = this.IsCurveSelected(cw1) || this.IsCurveSelected(cw2);
      return flag;
    }

    private void DrawCurves()
    {
      if (Event.current.type != EventType.Repaint)
        return;
      this.m_PointRenderer.Clear();
      for (int indexIntoDrawOrder = 0; indexIntoDrawOrder < this.m_DrawOrder.Count; ++indexIntoDrawOrder)
      {
        CurveWrapper curveWrapperFromId = this.GetCurveWrapperFromID(this.m_DrawOrder[indexIntoDrawOrder]);
        if (curveWrapperFromId != null && !curveWrapperFromId.hidden && curveWrapperFromId.curve.length != 0)
        {
          CurveWrapper cw2 = (CurveWrapper) null;
          if (indexIntoDrawOrder < this.m_DrawOrder.Count - 1)
            cw2 = this.GetCurveWrapperFromID(this.m_DrawOrder[indexIntoDrawOrder + 1]);
          if (this.IsRegion(curveWrapperFromId, cw2))
          {
            ++indexIntoDrawOrder;
            bool hasFocus = this.ShouldCurveHaveFocus(indexIntoDrawOrder, curveWrapperFromId, cw2);
            this.DrawCurvesAndRegion(curveWrapperFromId, cw2, !this.IsRegionCurveSelected(curveWrapperFromId, cw2) ? (List<CurveSelection>) null : this.selectedCurves, hasFocus);
          }
          else
          {
            bool hasFocus = this.ShouldCurveHaveFocus(indexIntoDrawOrder, curveWrapperFromId, (CurveWrapper) null);
            this.DrawCurveAndPoints(curveWrapperFromId, !this.IsCurveSelected(curveWrapperFromId) ? (List<CurveSelection>) null : this.selectedCurves, hasFocus);
          }
        }
      }
      this.m_PointRenderer.Render();
    }

    private void DrawCurvesTangents()
    {
      if (this.m_DraggingCurveOrRegion != null)
        return;
      HandleUtility.ApplyWireMaterial();
      GL.Begin(1);
      GL.Color(this.m_TangentColor * new Color(1f, 1f, 1f, 0.75f));
      for (int index = 0; index < this.selectedCurves.Count; ++index)
      {
        CurveSelection selectedCurve = this.selectedCurves[index];
        if (!selectedCurve.semiSelected)
        {
          Vector2 position1 = this.GetPosition(selectedCurve);
          CurveWrapper wrapperFromSelection = this.GetCurveWrapperFromSelection(selectedCurve);
          if (wrapperFromSelection != null)
          {
            AnimationCurve curve = wrapperFromSelection.curve;
            if (curve != null && curve.length != 0)
            {
              if (this.IsLeftTangentEditable(selectedCurve) && (double) this.GetKeyframeFromSelection(selectedCurve).time != (double) curve.keys[0].time)
              {
                Vector2 position2 = this.GetPosition(new CurveSelection(selectedCurve.curveID, selectedCurve.key, CurveSelection.SelectionType.InTangent));
                this.DrawCurveLine(wrapperFromSelection, position2, position1);
              }
              if (this.IsRightTangentEditable(selectedCurve) && (double) this.GetKeyframeFromSelection(selectedCurve).time != (double) curve.keys[curve.keys.Length - 1].time)
              {
                Vector2 position2 = this.GetPosition(new CurveSelection(selectedCurve.curveID, selectedCurve.key, CurveSelection.SelectionType.OutTangent));
                this.DrawCurveLine(wrapperFromSelection, position1, position2);
              }
            }
          }
        }
      }
      GL.End();
      this.m_PointRenderer.Clear();
      GUI.color = this.m_TangentColor;
      for (int index = 0; index < this.selectedCurves.Count; ++index)
      {
        CurveSelection selectedCurve = this.selectedCurves[index];
        if (!selectedCurve.semiSelected)
        {
          CurveWrapper wrapperFromSelection = this.GetCurveWrapperFromSelection(selectedCurve);
          if (wrapperFromSelection != null)
          {
            AnimationCurve curve = wrapperFromSelection.curve;
            if (curve != null && curve.length != 0)
            {
              if (this.IsLeftTangentEditable(selectedCurve) && (double) this.GetKeyframeFromSelection(selectedCurve).time != (double) curve.keys[0].time)
                this.DrawPoint(this.DrawingToOffsetViewTransformPoint(wrapperFromSelection, this.GetPosition(new CurveSelection(selectedCurve.curveID, selectedCurve.key, CurveSelection.SelectionType.InTangent))), CurveWrapper.SelectionMode.None);
              if (this.IsRightTangentEditable(selectedCurve) && (double) this.GetKeyframeFromSelection(selectedCurve).time != (double) curve.keys[curve.keys.Length - 1].time)
                this.DrawPoint(this.DrawingToOffsetViewTransformPoint(wrapperFromSelection, this.GetPosition(new CurveSelection(selectedCurve.curveID, selectedCurve.key, CurveSelection.SelectionType.OutTangent))), CurveWrapper.SelectionMode.None);
            }
          }
        }
      }
      this.m_PointRenderer.Render();
    }

    private void DrawCurvesOverlay()
    {
      if (this.m_DraggingCurveOrRegion != null || this.m_DraggingKey == null || this.settings.rectangleToolFlags != CurveEditorSettings.RectangleToolFlags.NoRectangleTool)
        return;
      GUI.color = Color.white;
      float vRangeMin = this.vRangeMin;
      float vRangeMax = this.vRangeMax;
      float min = Mathf.Max(vRangeMin, this.m_DraggingKey.vRangeMin);
      float max = Mathf.Min(vRangeMax, this.m_DraggingKey.vRangeMax);
      Vector2 lhs = this.m_DraggedCoord + this.m_MoveCoord;
      lhs.x = Mathf.Clamp(lhs.x, this.hRangeMin, this.hRangeMax);
      lhs.y = Mathf.Clamp(lhs.y, min, max);
      Vector2 viewTransformPoint = this.DrawingToOffsetViewTransformPoint(this.m_DraggingKey, lhs);
      Vector2 vector2_1 = this.m_DraggingKey.getAxisUiScalarsCallback == null ? Vector2.one : this.m_DraggingKey.getAxisUiScalarsCallback();
      if ((double) vector2_1.x >= 0.0)
        lhs.x *= vector2_1.x;
      if ((double) vector2_1.y >= 0.0)
        lhs.y *= vector2_1.y;
      GUIContent content = new GUIContent(string.Format("{0}, {1}", (object) this.FormatTime(lhs.x, this.invSnap, this.timeFormat), (object) this.FormatValue(lhs.y)));
      Vector2 vector2_2 = CurveEditor.Styles.dragLabel.CalcSize(content);
      EditorGUI.DoDropShadowLabel(new Rect(viewTransformPoint.x, viewTransformPoint.y - vector2_2.y, vector2_2.x, vector2_2.y), content, CurveEditor.Styles.dragLabel, 0.3f);
    }

    private List<Vector3> CreateRegion(CurveWrapper minCurve, CurveWrapper maxCurve, float deltaTime)
    {
      List<Vector3> vector3List = new List<Vector3>();
      List<float> floatList = new List<float>();
      float num1 = deltaTime;
      while ((double) num1 <= 1.0)
      {
        floatList.Add(num1);
        num1 += deltaTime;
      }
      if ((double) num1 != 1.0)
        floatList.Add(1f);
      foreach (Keyframe key in maxCurve.curve.keys)
      {
        if ((double) key.time > 0.0 && (double) key.time < 1.0)
        {
          floatList.Add(key.time - 0.0001f);
          floatList.Add(key.time);
          floatList.Add(key.time + 0.0001f);
        }
      }
      foreach (Keyframe key in minCurve.curve.keys)
      {
        if ((double) key.time > 0.0 && (double) key.time < 1.0)
        {
          floatList.Add(key.time - 0.0001f);
          floatList.Add(key.time);
          floatList.Add(key.time + 0.0001f);
        }
      }
      floatList.Sort();
      Vector3 point1 = new Vector3(0.0f, maxCurve.renderer.EvaluateCurveSlow(0.0f), 0.0f);
      Vector3 point2 = new Vector3(0.0f, minCurve.renderer.EvaluateCurveSlow(0.0f), 0.0f);
      Vector3 vector3_1 = this.DrawingToOffsetViewMatrix(maxCurve).MultiplyPoint(point1);
      Vector3 vector3_2 = this.DrawingToOffsetViewMatrix(minCurve).MultiplyPoint(point2);
      for (int index = 0; index < floatList.Count; ++index)
      {
        float num2 = floatList[index];
        Vector3 point3 = new Vector3(num2, maxCurve.renderer.EvaluateCurveSlow(num2), 0.0f);
        Vector3 point4 = new Vector3(num2, minCurve.renderer.EvaluateCurveSlow(num2), 0.0f);
        Vector3 vector3_3 = this.DrawingToOffsetViewMatrix(maxCurve).MultiplyPoint(point3);
        Vector3 vector3_4 = this.DrawingToOffsetViewMatrix(minCurve).MultiplyPoint(point4);
        if ((double) point1.y >= (double) point2.y && (double) point3.y >= (double) point4.y)
        {
          vector3List.Add(vector3_1);
          vector3List.Add(vector3_4);
          vector3List.Add(vector3_2);
          vector3List.Add(vector3_1);
          vector3List.Add(vector3_3);
          vector3List.Add(vector3_4);
        }
        else if ((double) point1.y <= (double) point2.y && (double) point3.y <= (double) point4.y)
        {
          vector3List.Add(vector3_2);
          vector3List.Add(vector3_3);
          vector3List.Add(vector3_1);
          vector3List.Add(vector3_2);
          vector3List.Add(vector3_4);
          vector3List.Add(vector3_3);
        }
        else
        {
          Vector2 zero = Vector2.zero;
          if (Mathf.LineIntersection((Vector2) vector3_1, (Vector2) vector3_3, (Vector2) vector3_2, (Vector2) vector3_4, ref zero))
          {
            vector3List.Add(vector3_1);
            vector3List.Add((Vector3) zero);
            vector3List.Add(vector3_2);
            vector3List.Add(vector3_3);
            vector3List.Add((Vector3) zero);
            vector3List.Add(vector3_4);
          }
          else
            Debug.Log((object) "Error: No intersection found! There should be one...");
        }
        point1 = point3;
        point2 = point4;
        vector3_1 = vector3_3;
        vector3_2 = vector3_4;
      }
      return vector3List;
    }

    public void DrawRegion(CurveWrapper curve1, CurveWrapper curve2, bool hasFocus)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      float deltaTime = (float) (1.0 / ((double) this.rect.width / 10.0));
      List<Vector3> region = this.CreateRegion(curve1, curve2, deltaTime);
      Color color = curve1.color;
      Color c;
      if (this.IsDraggingRegion(curve1, curve2))
      {
        c = Color.Lerp(color, Color.black, 0.1f);
        c.a = 0.4f;
      }
      else if (this.settings.useFocusColors && !hasFocus)
      {
        c = color * 0.4f;
        c.a = 0.1f;
      }
      else
      {
        c = color * 1f;
        c.a = 0.4f;
      }
      Shader.SetGlobalColor("_HandleColor", c);
      HandleUtility.ApplyWireMaterial();
      GL.Begin(4);
      int num = region.Count / 3;
      for (int index = 0; index < num; ++index)
      {
        GL.Color(c);
        GL.Vertex(region[index * 3]);
        GL.Vertex(region[index * 3 + 1]);
        GL.Vertex(region[index * 3 + 2]);
      }
      GL.End();
    }

    private void DrawCurve(CurveWrapper cw, bool hasFocus)
    {
      CurveRenderer renderer = cw.renderer;
      Color color = cw.color;
      if (this.IsDraggingCurve(cw) || cw.selected == CurveWrapper.SelectionMode.Selected)
        color = Color.Lerp(color, Color.white, 0.3f);
      else if (this.settings.useFocusColors && !hasFocus)
      {
        color *= 0.5f;
        color.a = 0.8f;
      }
      Rect shownArea = this.shownArea;
      renderer.DrawCurve(shownArea.xMin - cw.timeOffset, shownArea.xMax, color, this.DrawingToOffsetViewMatrix(cw), (Color) this.settings.wrapColor * cw.wrapColorMultiplier);
    }

    private void DrawPointsOnCurve(CurveWrapper cw, List<CurveSelection> selected, bool hasFocus)
    {
      this.m_PreviousDrawPointCenter = new Vector2(float.MinValue, float.MinValue);
      if (selected == null)
      {
        Color color = cw.color;
        if (this.settings.useFocusColors && !hasFocus)
          color *= 0.5f;
        GUI.color = color;
        foreach (Keyframe key in cw.curve.keys)
          this.DrawPoint(this.DrawingToOffsetViewTransformPoint(cw, new Vector2(key.time, key.value)), CurveWrapper.SelectionMode.None);
      }
      else
      {
        GUI.color = Color.Lerp(cw.color, Color.white, 0.2f);
        int index = 0;
        while (index < selected.Count && selected[index].curveID != cw.id)
          ++index;
        int num = 0;
        foreach (Keyframe key in cw.curve.keys)
        {
          if (index < selected.Count && selected[index].key == num && selected[index].curveID == cw.id)
          {
            if (selected[index].semiSelected)
              this.DrawPoint(this.DrawingToOffsetViewTransformPoint(cw, new Vector2(key.time, key.value)), CurveWrapper.SelectionMode.SemiSelected);
            else
              this.DrawPoint(this.DrawingToOffsetViewTransformPoint(cw, new Vector2(key.time, key.value)), CurveWrapper.SelectionMode.Selected, this.settings.rectangleToolFlags != CurveEditorSettings.RectangleToolFlags.NoRectangleTool ? MouseCursor.Arrow : MouseCursor.MoveArrow);
            ++index;
          }
          else
            this.DrawPoint(this.DrawingToOffsetViewTransformPoint(cw, new Vector2(key.time, key.value)), CurveWrapper.SelectionMode.None);
          ++num;
        }
        GUI.color = Color.white;
      }
    }

    private void DrawPoint(Vector2 viewPos, CurveWrapper.SelectionMode selected)
    {
      this.DrawPoint(viewPos, selected, MouseCursor.Arrow);
    }

    private void DrawPoint(Vector2 viewPos, CurveWrapper.SelectionMode selected, MouseCursor mouseCursor)
    {
      Rect rect = new Rect(Mathf.Floor(viewPos.x) - 4f, Mathf.Floor(viewPos.y) - 4f, (float) CurveEditor.Styles.pointIcon.width, (float) CurveEditor.Styles.pointIcon.height);
      Vector2 center = rect.center;
      if ((double) (center - this.m_PreviousDrawPointCenter).magnitude <= 8.0)
        return;
      switch (selected)
      {
        case CurveWrapper.SelectionMode.None:
          this.m_PointRenderer.AddPoint(rect, GUI.color);
          goto label_6;
        case CurveWrapper.SelectionMode.Selected:
          this.m_PointRenderer.AddSelectedPoint(rect, GUI.color);
          break;
        default:
          this.m_PointRenderer.AddSemiSelectedPoint(rect, GUI.color);
          break;
      }
label_6:
      if (mouseCursor != MouseCursor.Arrow && GUIUtility.hotControl == 0)
        EditorGUIUtility.AddCursorRect(rect, mouseCursor);
      this.m_PreviousDrawPointCenter = center;
    }

    private void DrawLine(Vector2 lhs, Vector2 rhs)
    {
      GL.Vertex(this.DrawingToViewTransformPoint(new Vector3(lhs.x, lhs.y, 0.0f)));
      GL.Vertex(this.DrawingToViewTransformPoint(new Vector3(rhs.x, rhs.y, 0.0f)));
    }

    private void DrawCurveLine(CurveWrapper cw, Vector2 lhs, Vector2 rhs)
    {
      GL.Vertex(this.DrawingToOffsetViewTransformPoint(cw, new Vector3(lhs.x, lhs.y, 0.0f)));
      GL.Vertex(this.DrawingToOffsetViewTransformPoint(cw, new Vector3(rhs.x, rhs.y, 0.0f)));
    }

    private void DrawWrapperPopups()
    {
      if (!this.settings.showWrapperPopups)
        return;
      int curveID;
      this.GetTopMostCurveID(out curveID);
      if (curveID == -1)
        return;
      CurveWrapper curveWrapperFromId = this.GetCurveWrapperFromID(curveID);
      AnimationCurve curve = curveWrapperFromId.curve;
      if (curve == null || curve.length < 2 || curve.preWrapMode == WrapMode.Default)
        return;
      GUI.BeginGroup(this.drawRect);
      Color contentColor = GUI.contentColor;
      WrapMode wrapMode1 = this.WrapModeIconPopup(curve.keys[0], curve.preWrapMode, -1.5f);
      if (wrapMode1 != curve.preWrapMode)
      {
        curve.preWrapMode = wrapMode1;
        curveWrapperFromId.changed = true;
      }
      WrapMode wrapMode2 = this.WrapModeIconPopup(curve.keys[curve.length - 1], curve.postWrapMode, 0.5f);
      if (wrapMode2 != curve.postWrapMode)
      {
        curve.postWrapMode = wrapMode2;
        curveWrapperFromId.changed = true;
      }
      if (curveWrapperFromId.changed)
      {
        curveWrapperFromId.renderer.SetWrap(curve.preWrapMode, curve.postWrapMode);
        if (this.curvesUpdated != null)
          this.curvesUpdated();
      }
      GUI.contentColor = contentColor;
      GUI.EndGroup();
    }

    private WrapMode WrapModeIconPopup(Keyframe key, WrapMode oldWrap, float hOffset)
    {
      float width = (float) CurveEditor.Styles.wrapModeMenuIcon.image.width;
      Vector3 viewTransformPoint = this.DrawingToViewTransformPoint(new Vector3(key.time, key.value));
      Rect position = new Rect(viewTransformPoint.x + width * hOffset, viewTransformPoint.y, width, width);
      WrapModeFixedCurve wrapModeFixedCurve = (WrapModeFixedCurve) oldWrap;
      Enum[] array1 = Enum.GetValues(typeof (WrapModeFixedCurve)).Cast<Enum>().ToArray<Enum>();
      string[] array2 = ((IEnumerable<string>) Enum.GetNames(typeof (WrapModeFixedCurve))).Select<string, string>((Func<string, string>) (x => ObjectNames.NicifyVariableName(x))).ToArray<string>();
      int selected = Array.IndexOf<Enum>(array1, (Enum) wrapModeFixedCurve);
      int controlId = GUIUtility.GetControlID(nameof (WrapModeIconPopup).GetHashCode(), FocusType.Keyboard, position);
      int selectedValueForControl = EditorGUI.PopupCallbackInfo.GetSelectedValueForControl(controlId, selected);
      GUIContent[] options = EditorGUIUtility.TempContent(array2);
      Event current = Event.current;
      switch (current.type)
      {
        case EventType.MouseDown:
          if (current.button == 0 && position.Contains(current.mousePosition))
          {
            if (Application.platform == RuntimePlatform.OSXEditor)
              position.y = (float) ((double) position.y - (double) (selectedValueForControl * 16) - 19.0);
            EditorGUI.PopupCallbackInfo.instance = new EditorGUI.PopupCallbackInfo(controlId);
            EditorUtility.DisplayCustomMenu(position, options, selectedValueForControl, new EditorUtility.SelectMenuItemFunction(EditorGUI.PopupCallbackInfo.instance.SetEnumValueDelegate), (object) null);
            GUIUtility.keyboardControl = controlId;
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (current.MainActionKeyForControl(controlId))
          {
            if (Application.platform == RuntimePlatform.OSXEditor)
              position.y = (float) ((double) position.y - (double) (selectedValueForControl * 16) - 19.0);
            EditorGUI.PopupCallbackInfo.instance = new EditorGUI.PopupCallbackInfo(controlId);
            EditorUtility.DisplayCustomMenu(position, options, selectedValueForControl, new EditorUtility.SelectMenuItemFunction(EditorGUI.PopupCallbackInfo.instance.SetEnumValueDelegate), (object) null);
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          GUIStyle.none.Draw(position, CurveEditor.Styles.wrapModeMenuIcon, controlId, false);
          break;
      }
      return (WrapMode) array1[selectedValueForControl];
    }

    private Vector2 GetAxisUiScalars(List<CurveWrapper> curvesWithSameParameterSpace)
    {
      if (this.selectedCurves.Count <= 1)
      {
        if (this.m_DrawOrder.Count > 0)
        {
          CurveWrapper curveWrapperFromId = this.GetCurveWrapperFromID(this.m_DrawOrder[this.m_DrawOrder.Count - 1]);
          if (curveWrapperFromId != null && curveWrapperFromId.getAxisUiScalarsCallback != null)
          {
            if (curvesWithSameParameterSpace != null)
              curvesWithSameParameterSpace.Add(curveWrapperFromId);
            return curveWrapperFromId.getAxisUiScalarsCallback();
          }
        }
        return Vector2.one;
      }
      Vector2 vector2_1 = new Vector2(-1f, -1f);
      if (this.selectedCurves.Count > 1)
      {
        bool flag1 = true;
        bool flag2 = true;
        Vector2 vector2_2 = Vector2.one;
        for (int index = 0; index < this.selectedCurves.Count; ++index)
        {
          CurveWrapper wrapperFromSelection = this.GetCurveWrapperFromSelection(this.selectedCurves[index]);
          if (wrapperFromSelection != null && wrapperFromSelection.getAxisUiScalarsCallback != null)
          {
            Vector2 vector2_3 = wrapperFromSelection.getAxisUiScalarsCallback();
            if (index == 0)
            {
              vector2_2 = vector2_3;
            }
            else
            {
              if ((double) Mathf.Abs(vector2_3.x - vector2_2.x) > 9.99999974737875E-06)
                flag1 = false;
              if ((double) Mathf.Abs(vector2_3.y - vector2_2.y) > 9.99999974737875E-06)
                flag2 = false;
              vector2_2 = vector2_3;
            }
            if (curvesWithSameParameterSpace != null)
              curvesWithSameParameterSpace.Add(wrapperFromSelection);
          }
        }
        if (flag1)
          vector2_1.x = vector2_2.x;
        if (flag2)
          vector2_1.y = vector2_2.y;
      }
      return vector2_1;
    }

    private void SetAxisUiScalars(Vector2 newScalars, List<CurveWrapper> curvesInSameSpace)
    {
      foreach (CurveWrapper curveWrapper in curvesInSameSpace)
      {
        Vector2 newAxisScalars = curveWrapper.getAxisUiScalarsCallback();
        if ((double) newScalars.x >= 0.0)
          newAxisScalars.x = newScalars.x;
        if ((double) newScalars.y >= 0.0)
          newAxisScalars.y = newScalars.y;
        if (curveWrapper.setAxisUiScalarsCallback != null)
          curveWrapper.setAxisUiScalarsCallback(newAxisScalars);
      }
    }

    public void GridGUI()
    {
      if (Event.current.type != EventType.Repaint)
        return;
      GUI.BeginClip(this.drawRect);
      Color color = GUI.color;
      Vector2 axisUiScalars = this.GetAxisUiScalars((List<CurveWrapper>) null);
      Rect shownArea = this.shownArea;
      this.hTicks.SetRanges(shownArea.xMin * axisUiScalars.x, shownArea.xMax * axisUiScalars.x, this.drawRect.xMin, this.drawRect.xMax);
      this.vTicks.SetRanges(shownArea.yMin * axisUiScalars.y, shownArea.yMax * axisUiScalars.y, this.drawRect.yMin, this.drawRect.yMax);
      HandleUtility.ApplyWireMaterial();
      GL.Begin(1);
      this.hTicks.SetTickStrengths((float) this.settings.hTickStyle.distMin, (float) this.settings.hTickStyle.distFull, false);
      float y1;
      float y2;
      if (this.settings.hTickStyle.stubs)
      {
        y1 = shownArea.yMin;
        y2 = shownArea.yMin - 40f / this.scale.y;
      }
      else
      {
        y1 = Mathf.Max(shownArea.yMin, this.vRangeMin);
        y2 = Mathf.Min(shownArea.yMax, this.vRangeMax);
      }
      for (int level = 0; level < this.hTicks.tickLevels; ++level)
      {
        float strengthOfLevel = this.hTicks.GetStrengthOfLevel(level);
        if ((double) strengthOfLevel > 0.0)
        {
          GL.Color((Color) this.settings.hTickStyle.tickColor * new Color(1f, 1f, 1f, strengthOfLevel) * new Color(1f, 1f, 1f, 0.75f));
          float[] ticksAtLevel = this.hTicks.GetTicksAtLevel(level, true);
          for (int index = 0; index < ticksAtLevel.Length; ++index)
          {
            ticksAtLevel[index] /= axisUiScalars.x;
            if ((double) ticksAtLevel[index] > (double) this.hRangeMin && (double) ticksAtLevel[index] < (double) this.hRangeMax)
              this.DrawLine(new Vector2(ticksAtLevel[index], y1), new Vector2(ticksAtLevel[index], y2));
          }
        }
      }
      GL.Color((Color) this.settings.hTickStyle.tickColor * new Color(1f, 1f, 1f, 1f) * new Color(1f, 1f, 1f, 0.75f));
      if ((double) this.hRangeMin != double.NegativeInfinity)
        this.DrawLine(new Vector2(this.hRangeMin, y1), new Vector2(this.hRangeMin, y2));
      if ((double) this.hRangeMax != double.PositiveInfinity)
        this.DrawLine(new Vector2(this.hRangeMax, y1), new Vector2(this.hRangeMax, y2));
      this.vTicks.SetTickStrengths((float) this.settings.vTickStyle.distMin, (float) this.settings.vTickStyle.distFull, false);
      float x1;
      float x2;
      if (this.settings.vTickStyle.stubs)
      {
        x1 = shownArea.xMin;
        x2 = shownArea.xMin + 40f / this.scale.x;
      }
      else
      {
        x1 = Mathf.Max(shownArea.xMin, this.hRangeMin);
        x2 = Mathf.Min(shownArea.xMax, this.hRangeMax);
      }
      for (int level = 0; level < this.vTicks.tickLevels; ++level)
      {
        float strengthOfLevel = this.vTicks.GetStrengthOfLevel(level);
        if ((double) strengthOfLevel > 0.0)
        {
          GL.Color((Color) this.settings.vTickStyle.tickColor * new Color(1f, 1f, 1f, strengthOfLevel) * new Color(1f, 1f, 1f, 0.75f));
          float[] ticksAtLevel = this.vTicks.GetTicksAtLevel(level, true);
          for (int index = 0; index < ticksAtLevel.Length; ++index)
          {
            ticksAtLevel[index] /= axisUiScalars.y;
            if ((double) ticksAtLevel[index] > (double) this.vRangeMin && (double) ticksAtLevel[index] < (double) this.vRangeMax)
              this.DrawLine(new Vector2(x1, ticksAtLevel[index]), new Vector2(x2, ticksAtLevel[index]));
          }
        }
      }
      GL.Color((Color) this.settings.vTickStyle.tickColor * new Color(1f, 1f, 1f, 1f) * new Color(1f, 1f, 1f, 0.75f));
      if ((double) this.vRangeMin != double.NegativeInfinity)
        this.DrawLine(new Vector2(x1, this.vRangeMin), new Vector2(x2, this.vRangeMin));
      if ((double) this.vRangeMax != double.PositiveInfinity)
        this.DrawLine(new Vector2(x1, this.vRangeMax), new Vector2(x2, this.vRangeMax));
      GL.End();
      if (this.settings.showAxisLabels)
      {
        if (this.settings.hTickStyle.distLabel > 0 && (double) axisUiScalars.x > 0.0)
        {
          GUI.color = (Color) this.settings.hTickStyle.labelColor;
          int withMinSeparation = this.hTicks.GetLevelWithMinSeparation((float) this.settings.hTickStyle.distLabel);
          int minimumDifference = MathUtils.GetNumberOfDecimalsForMinimumDifference(this.hTicks.GetPeriodOfLevel(withMinSeparation));
          float[] ticksAtLevel = this.hTicks.GetTicksAtLevel(withMinSeparation, false);
          float[] numArray = (float[]) ticksAtLevel.Clone();
          float y3 = Mathf.Floor(this.drawRect.height);
          for (int index = 0; index < ticksAtLevel.Length; ++index)
          {
            numArray[index] /= axisUiScalars.x;
            if ((double) numArray[index] >= (double) this.hRangeMin && (double) numArray[index] <= (double) this.hRangeMax)
            {
              Vector2 vector2 = this.DrawingToViewTransformPoint(new Vector2(numArray[index], 0.0f));
              vector2 = new Vector2(Mathf.Floor(vector2.x), y3);
              float num = ticksAtLevel[index];
              TextAnchor textAnchor;
              Rect position;
              if (this.settings.hTickStyle.centerLabel)
              {
                textAnchor = TextAnchor.UpperCenter;
                position = new Rect(vector2.x, vector2.y - 16f - this.settings.hTickLabelOffset, 1f, 16f);
              }
              else
              {
                textAnchor = TextAnchor.UpperLeft;
                position = new Rect(vector2.x, vector2.y - 16f - this.settings.hTickLabelOffset, 50f, 16f);
              }
              if (CurveEditor.Styles.labelTickMarksX.alignment != textAnchor)
                CurveEditor.Styles.labelTickMarksX.alignment = textAnchor;
              GUI.Label(position, num.ToString("n" + (object) minimumDifference) + this.settings.hTickStyle.unit, CurveEditor.Styles.labelTickMarksX);
            }
          }
        }
        if (this.settings.vTickStyle.distLabel > 0 && (double) axisUiScalars.y > 0.0)
        {
          GUI.color = (Color) this.settings.vTickStyle.labelColor;
          int withMinSeparation = this.vTicks.GetLevelWithMinSeparation((float) this.settings.vTickStyle.distLabel);
          float[] ticksAtLevel = this.vTicks.GetTicksAtLevel(withMinSeparation, false);
          float[] numArray = (float[]) ticksAtLevel.Clone();
          string format = "n" + (object) MathUtils.GetNumberOfDecimalsForMinimumDifference(this.vTicks.GetPeriodOfLevel(withMinSeparation));
          this.m_AxisLabelFormat = format;
          float width = 35f;
          if (!this.settings.vTickStyle.stubs && ticksAtLevel.Length > 1)
          {
            float num1 = ticksAtLevel[1];
            float num2 = ticksAtLevel[ticksAtLevel.Length - 1];
            string text1 = num1.ToString(format) + this.settings.vTickStyle.unit;
            string text2 = num2.ToString(format) + this.settings.vTickStyle.unit;
            width = Mathf.Max(CurveEditor.Styles.labelTickMarksY.CalcSize(new GUIContent(text1)).x, CurveEditor.Styles.labelTickMarksY.CalcSize(new GUIContent(text2)).x) + 6f;
          }
          for (int index = 0; index < ticksAtLevel.Length; ++index)
          {
            numArray[index] /= axisUiScalars.y;
            if ((double) numArray[index] >= (double) this.vRangeMin && (double) numArray[index] <= (double) this.vRangeMax)
            {
              Vector2 vector2 = this.DrawingToViewTransformPoint(new Vector2(0.0f, numArray[index]));
              vector2 = new Vector2(vector2.x, Mathf.Floor(vector2.y));
              float num = ticksAtLevel[index];
              GUI.Label(!this.settings.vTickStyle.centerLabel ? new Rect(0.0f, vector2.y - 13f, width, 16f) : new Rect(0.0f, vector2.y - 8f, this.leftmargin - 4f, 16f), num.ToString(format) + this.settings.vTickStyle.unit, CurveEditor.Styles.labelTickMarksY);
            }
          }
        }
      }
      GUI.color = color;
      GUI.EndClip();
    }

    public delegate void CallbackFunction();

    private class SavedCurve
    {
      public int curveId;
      public List<CurveEditor.SavedCurve.SavedKeyFrame> keys;

      public class SavedKeyFrame
      {
        public Keyframe key;
        public CurveWrapper.SelectionMode selected;

        public SavedKeyFrame(Keyframe key, CurveWrapper.SelectionMode selected)
        {
          this.key = key;
          this.selected = selected;
        }

        public CurveEditor.SavedCurve.SavedKeyFrame Clone()
        {
          return new CurveEditor.SavedCurve.SavedKeyFrame(this.key, this.selected);
        }
      }

      public class SavedKeyFrameComparer : IComparer<float>
      {
        public static CurveEditor.SavedCurve.SavedKeyFrameComparer Instance = new CurveEditor.SavedCurve.SavedKeyFrameComparer();

        public int Compare(float time1, float time2)
        {
          float num = time1 - time2;
          return (double) num >= -9.99999974737875E-06 ? ((double) num < 9.99999974737875E-06 ? 0 : 1) : -1;
        }
      }

      public delegate CurveEditor.SavedCurve.SavedKeyFrame KeyFrameOperation(CurveEditor.SavedCurve.SavedKeyFrame keyframe, CurveEditor.SavedCurve curve);
    }

    private enum AxisLock
    {
      None,
      X,
      Y,
    }

    private struct KeyFrameCopy
    {
      public float time;
      public float value;
      public float inTangent;
      public float outTangent;
      public int idx;
      public int selectionIdx;

      public KeyFrameCopy(int idx, int selectionIdx, Keyframe source)
      {
        this.idx = idx;
        this.selectionIdx = selectionIdx;
        this.time = source.time;
        this.value = source.value;
        this.inTangent = source.inTangent;
        this.outTangent = source.outTangent;
      }
    }

    internal static class Styles
    {
      public static Texture2D pointIcon = EditorGUIUtility.LoadIcon("curvekeyframe");
      public static Texture2D pointIconSelected = EditorGUIUtility.LoadIcon("curvekeyframeselected");
      public static Texture2D pointIconSelectedOverlay = EditorGUIUtility.LoadIcon("curvekeyframeselectedoverlay");
      public static Texture2D pointIconSemiSelectedOverlay = EditorGUIUtility.LoadIcon("curvekeyframesemiselectedoverlay");
      public static GUIContent wrapModeMenuIcon = EditorGUIUtility.IconContent("AnimationWrapModeMenu");
      public static GUIStyle none = new GUIStyle();
      public static GUIStyle labelTickMarksY = (GUIStyle) "CurveEditorLabelTickMarks";
      public static GUIStyle selectionRect = (GUIStyle) "SelectionRect";
      public static GUIStyle dragLabel = (GUIStyle) "ProfilerBadge";
      public static GUIStyle axisLabelNumberField = new GUIStyle(EditorStyles.miniTextField);
      public static GUIStyle rightAlignedLabel = new GUIStyle(EditorStyles.label);
      public static GUIStyle labelTickMarksX;

      static Styles()
      {
        CurveEditor.Styles.axisLabelNumberField.alignment = TextAnchor.UpperRight;
        CurveEditor.Styles.labelTickMarksY.contentOffset = Vector2.zero;
        CurveEditor.Styles.labelTickMarksX = new GUIStyle(CurveEditor.Styles.labelTickMarksY);
        CurveEditor.Styles.labelTickMarksX.clipping = TextClipping.Overflow;
        CurveEditor.Styles.rightAlignedLabel.alignment = TextAnchor.UpperRight;
      }
    }

    internal enum PickMode
    {
      None,
      Click,
      Marquee,
    }
  }
}
