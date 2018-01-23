// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridPaintingState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class GridPaintingState : ScriptableSingleton<GridPaintingState>, IToolModeOwner
  {
    [SerializeField]
    private HashSet<UnityEngine.Object> m_InterestedPainters = new HashSet<UnityEngine.Object>();
    private GameObject[] m_CachedPaintTargets = (GameObject[]) null;
    [SerializeField]
    private GameObject m_ScenePaintTarget;
    [SerializeField]
    private GridBrushBase m_Brush;
    [SerializeField]
    private PaintableGrid m_ActiveGrid;
    private bool m_FlushPaintTargetCache;
    private Editor m_CachedEditor;
    private bool m_SavingPalette;

    public static event Action<GameObject> scenePaintTargetChanged;

    public static event Action<GridBrushBase> brushChanged;

    private void OnEnable()
    {
      EditorApplication.hierarchyWindowChanged += new EditorApplication.CallbackFunction(this.HierarchyChanged);
      Selection.selectionChanged += new Action(this.OnSelectionChange);
      this.m_FlushPaintTargetCache = true;
    }

    private void OnDisable()
    {
      this.m_InterestedPainters.Clear();
      this.m_InterestedPainters = (HashSet<UnityEngine.Object>) null;
      EditorApplication.hierarchyWindowChanged -= new EditorApplication.CallbackFunction(this.HierarchyChanged);
      Selection.selectionChanged -= new Action(this.OnSelectionChange);
      GridPaintingState.FlushCache();
    }

    private void OnSelectionChange()
    {
      if (!this.hasInterestedPainters || GridPaintingState.validTargets != null || !GridPaintingState.ValidatePaintTarget(Selection.activeGameObject))
        return;
      GridPaintingState.scenePaintTarget = Selection.activeGameObject;
    }

    private void HierarchyChanged()
    {
      if (!this.hasInterestedPainters)
        return;
      this.m_FlushPaintTargetCache = true;
      if (GridPaintingState.validTargets == null || !((IEnumerable<GameObject>) GridPaintingState.validTargets).Contains<GameObject>(GridPaintingState.scenePaintTarget))
        GridPaintingState.AutoSelectPaintTarget();
    }

    public static void AutoSelectPaintTarget()
    {
      if (!((UnityEngine.Object) GridPaintingState.activeBrushEditor != (UnityEngine.Object) null) || GridPaintingState.validTargets == null || GridPaintingState.validTargets.Length <= 0)
        return;
      GridPaintingState.scenePaintTarget = GridPaintingState.validTargets[0];
    }

    public static GameObject scenePaintTarget
    {
      get
      {
        return ScriptableSingleton<GridPaintingState>.instance.m_ScenePaintTarget;
      }
      set
      {
        if (!((UnityEngine.Object) value != (UnityEngine.Object) ScriptableSingleton<GridPaintingState>.instance.m_ScenePaintTarget))
          return;
        ScriptableSingleton<GridPaintingState>.instance.m_ScenePaintTarget = value;
        if (GridPaintingState.scenePaintTargetChanged != null)
          GridPaintingState.scenePaintTargetChanged(ScriptableSingleton<GridPaintingState>.instance.m_ScenePaintTarget);
      }
    }

    public static GridBrushBase gridBrush
    {
      get
      {
        if ((UnityEngine.Object) ScriptableSingleton<GridPaintingState>.instance.m_Brush == (UnityEngine.Object) null)
          ScriptableSingleton<GridPaintingState>.instance.m_Brush = GridPaletteBrushes.brushes[0];
        return ScriptableSingleton<GridPaintingState>.instance.m_Brush;
      }
      set
      {
        if (!((UnityEngine.Object) ScriptableSingleton<GridPaintingState>.instance.m_Brush != (UnityEngine.Object) value))
          return;
        ScriptableSingleton<GridPaintingState>.instance.m_Brush = value;
        ScriptableSingleton<GridPaintingState>.instance.m_FlushPaintTargetCache = true;
        if ((UnityEngine.Object) GridPaintingState.scenePaintTarget != (UnityEngine.Object) null && !GridPaintingState.ValidatePaintTarget(GridPaintingState.scenePaintTarget))
          GridPaintingState.scenePaintTarget = (GameObject) null;
        if ((UnityEngine.Object) GridPaintingState.scenePaintTarget == (UnityEngine.Object) null)
          GridPaintingState.scenePaintTarget = !GridPaintingState.ValidatePaintTarget(Selection.activeGameObject) ? (GameObject) null : Selection.activeGameObject;
        if ((UnityEngine.Object) GridPaintingState.scenePaintTarget == (UnityEngine.Object) null)
          GridPaintingState.AutoSelectPaintTarget();
        if (GridPaintingState.brushChanged != null)
          GridPaintingState.brushChanged(ScriptableSingleton<GridPaintingState>.instance.m_Brush);
      }
    }

    public static GridBrush defaultBrush
    {
      get
      {
        return GridPaintingState.gridBrush as GridBrush;
      }
      set
      {
        GridPaintingState.gridBrush = (GridBrushBase) value;
      }
    }

    public static GridBrushEditorBase activeBrushEditor
    {
      get
      {
        Editor.CreateCachedEditor((UnityEngine.Object) GridPaintingState.gridBrush, (System.Type) null, ref ScriptableSingleton<GridPaintingState>.instance.m_CachedEditor);
        return ScriptableSingleton<GridPaintingState>.instance.m_CachedEditor as GridBrushEditorBase;
      }
    }

    public static Editor fallbackEditor
    {
      get
      {
        Editor.CreateCachedEditor((UnityEngine.Object) GridPaintingState.gridBrush, (System.Type) null, ref ScriptableSingleton<GridPaintingState>.instance.m_CachedEditor);
        return ScriptableSingleton<GridPaintingState>.instance.m_CachedEditor;
      }
    }

    public static PaintableGrid activeGrid
    {
      get
      {
        return ScriptableSingleton<GridPaintingState>.instance.m_ActiveGrid;
      }
      set
      {
        ScriptableSingleton<GridPaintingState>.instance.m_ActiveGrid = value;
      }
    }

    public static bool ValidatePaintTarget(GameObject candidate)
    {
      return !((UnityEngine.Object) candidate == (UnityEngine.Object) null) && (!((UnityEngine.Object) candidate.GetComponentInParent<Grid>() == (UnityEngine.Object) null) || !((UnityEngine.Object) candidate.GetComponent<Grid>() == (UnityEngine.Object) null)) && (GridPaintingState.validTargets == null || ((IEnumerable<GameObject>) GridPaintingState.validTargets).Contains<GameObject>(candidate));
    }

    public static void FlushCache()
    {
      if ((UnityEngine.Object) ScriptableSingleton<GridPaintingState>.instance.m_CachedEditor != (UnityEngine.Object) null)
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) ScriptableSingleton<GridPaintingState>.instance.m_CachedEditor);
        ScriptableSingleton<GridPaintingState>.instance.m_CachedEditor = (Editor) null;
      }
      ScriptableSingleton<GridPaintingState>.instance.m_FlushPaintTargetCache = true;
    }

    public static bool savingPalette
    {
      get
      {
        return ScriptableSingleton<GridPaintingState>.instance.m_SavingPalette;
      }
      set
      {
        ScriptableSingleton<GridPaintingState>.instance.m_SavingPalette = value;
      }
    }

    public static GameObject[] validTargets
    {
      get
      {
        if (ScriptableSingleton<GridPaintingState>.instance.m_FlushPaintTargetCache)
        {
          ScriptableSingleton<GridPaintingState>.instance.m_CachedPaintTargets = (GameObject[]) null;
          if ((UnityEngine.Object) GridPaintingState.activeBrushEditor != (UnityEngine.Object) null)
            ScriptableSingleton<GridPaintingState>.instance.m_CachedPaintTargets = GridPaintingState.activeBrushEditor.validTargets;
          ScriptableSingleton<GridPaintingState>.instance.m_FlushPaintTargetCache = false;
        }
        return ScriptableSingleton<GridPaintingState>.instance.m_CachedPaintTargets;
      }
    }

    public static void RegisterPainterInterest(UnityEngine.Object painter)
    {
      ScriptableSingleton<GridPaintingState>.instance.m_InterestedPainters.Add(painter);
    }

    public static void UnregisterPainterInterest(UnityEngine.Object painter)
    {
      ScriptableSingleton<GridPaintingState>.instance.m_InterestedPainters.Remove(painter);
    }

    public bool hasInterestedPainters
    {
      get
      {
        return this.m_InterestedPainters.Count > 0;
      }
    }

    public bool areToolModesAvailable
    {
      get
      {
        return true;
      }
    }

    public Bounds GetWorldBoundsOfTargets()
    {
      return new Bounds(Vector3.zero, Vector3.positiveInfinity);
    }

    public bool ModeSurvivesSelectionChange(int toolMode)
    {
      return true;
    }

    int IToolModeOwner.GetInstanceID()
    {
      return this.GetInstanceID();
    }
  }
}
