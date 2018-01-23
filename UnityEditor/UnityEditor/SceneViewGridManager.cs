// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneViewGridManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class SceneViewGridManager : ScriptableSingleton<SceneViewGridManager>
  {
    internal static readonly PrefColor sceneViewGridComponentGizmo = new PrefColor("Scene/Grid Component", 1f, 1f, 1f, 0.1f);
    private static Mesh s_GridProxyMesh;
    private static Material s_GridProxyMaterial;
    private static Color s_LastGridProxyColor;
    [SerializeField]
    private GridLayout m_ActiveGridProxy;
    private bool m_RegisteredEventHandlers;

    private bool active
    {
      get
      {
        return (UnityEngine.Object) this.m_ActiveGridProxy != (UnityEngine.Object) null;
      }
    }

    private GridLayout activeGridProxy
    {
      get
      {
        return this.m_ActiveGridProxy;
      }
    }

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
      ScriptableSingleton<SceneViewGridManager>.instance.RegisterEventHandlers();
    }

    private void OnEnable()
    {
      this.RegisterEventHandlers();
    }

    private void RegisterEventHandlers()
    {
      if (this.m_RegisteredEventHandlers)
        return;
      SceneView.onSceneGUIDelegate += new SceneView.OnSceneFunc(this.OnSceneGuiDelegate);
      Selection.selectionChanged += new Action(this.UpdateCache);
      EditorApplication.hierarchyWindowChanged += new EditorApplication.CallbackFunction(this.UpdateCache);
      UnityEditorInternal.EditMode.editModeStarted += new Action<IToolModeOwner, UnityEditorInternal.EditMode.SceneViewEditMode>(this.OnEditModeStart);
      UnityEditorInternal.EditMode.editModeEnded += new Action<IToolModeOwner>(this.OnEditModeEnd);
      GridPaintingState.brushChanged += new Action<GridBrushBase>(this.OnBrushChanged);
      GridPaintingState.scenePaintTargetChanged += new Action<GameObject>(this.OnScenePaintTargetChanged);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.OnUndoRedoPerformed);
      GridSnapping.snapPosition = new Func<Vector3, Vector3>(this.OnSnapPosition);
      GridSnapping.activeFunc = new Func<bool>(this.GetActive);
      this.m_RegisteredEventHandlers = true;
    }

    private void OnBrushChanged(GridBrushBase brush)
    {
      this.UpdateCache();
    }

    private void OnEditModeEnd(IToolModeOwner owner)
    {
      this.UpdateCache();
    }

    private void OnEditModeStart(IToolModeOwner owner, UnityEditorInternal.EditMode.SceneViewEditMode editMode)
    {
      this.UpdateCache();
    }

    private void OnScenePaintTargetChanged(GameObject scenePaintTarget)
    {
      this.UpdateCache();
    }

    private void OnUndoRedoPerformed()
    {
      SceneViewGridManager.FlushCachedGridProxy();
    }

    private void OnDisable()
    {
      SceneViewGridManager.FlushCachedGridProxy();
      SceneView.onSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnSceneGuiDelegate);
      Selection.selectionChanged -= new Action(this.UpdateCache);
      EditorApplication.hierarchyWindowChanged -= new EditorApplication.CallbackFunction(this.UpdateCache);
      UnityEditorInternal.EditMode.editModeStarted -= new Action<IToolModeOwner, UnityEditorInternal.EditMode.SceneViewEditMode>(this.OnEditModeStart);
      UnityEditorInternal.EditMode.editModeEnded -= new Action<IToolModeOwner>(this.OnEditModeEnd);
      GridPaintingState.brushChanged -= new Action<GridBrushBase>(this.OnBrushChanged);
      GridPaintingState.scenePaintTargetChanged -= new Action<GameObject>(this.OnScenePaintTargetChanged);
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.OnUndoRedoPerformed);
      GridSnapping.snapPosition = (Func<Vector3, Vector3>) null;
      GridSnapping.activeFunc = (Func<bool>) null;
      this.m_RegisteredEventHandlers = false;
    }

    private void UpdateCache()
    {
      GridLayout gridLayout = !PaintableGrid.InGridEditMode() ? (!((UnityEngine.Object) Selection.activeGameObject != (UnityEngine.Object) null) ? (GridLayout) null : Selection.activeGameObject.GetComponentInParent<GridLayout>()) : (!((UnityEngine.Object) GridPaintingState.scenePaintTarget != (UnityEngine.Object) null) ? (GridLayout) null : GridPaintingState.scenePaintTarget.GetComponentInParent<GridLayout>());
      if ((UnityEngine.Object) gridLayout != (UnityEngine.Object) this.m_ActiveGridProxy)
      {
        this.m_ActiveGridProxy = gridLayout;
        SceneViewGridManager.FlushCachedGridProxy();
      }
      this.ShowGlobalGrid(!this.active);
    }

    private void OnSceneGuiDelegate(SceneView sceneView)
    {
      if (!this.active || !AnnotationUtility.showGrid)
        return;
      SceneViewGridManager.DrawGrid(this.activeGridProxy);
    }

    private static void DrawGrid(GridLayout gridLayout)
    {
      if (SceneViewGridManager.sceneViewGridComponentGizmo.Color != SceneViewGridManager.s_LastGridProxyColor)
      {
        SceneViewGridManager.FlushCachedGridProxy();
        SceneViewGridManager.s_LastGridProxyColor = SceneViewGridManager.sceneViewGridComponentGizmo.Color;
      }
      GridEditorUtility.DrawGridGizmo(gridLayout, gridLayout.transform, SceneViewGridManager.s_LastGridProxyColor, ref SceneViewGridManager.s_GridProxyMesh, ref SceneViewGridManager.s_GridProxyMaterial);
    }

    private void ShowGlobalGrid(bool value)
    {
      IEnumerator enumerator = SceneView.sceneViews.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          ((SceneView) enumerator.Current).showGlobalGrid = value;
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    private bool GetActive()
    {
      return this.active;
    }

    private Vector3 OnSnapPosition(Vector3 position)
    {
      Vector3 vector3 = position;
      if (this.active && !EditorGUI.actionKey)
      {
        Vector3 cellInterpolated = this.activeGridProxy.LocalToCellInterpolated(this.activeGridProxy.WorldToLocal(position));
        vector3 = this.activeGridProxy.LocalToWorld(this.activeGridProxy.CellToLocalInterpolated(new Vector3(Mathf.Round(2f * cellInterpolated.x) / 2f, Mathf.Round(2f * cellInterpolated.y) / 2f, Mathf.Round(2f * cellInterpolated.z) / 2f)));
      }
      return vector3;
    }

    internal static void FlushCachedGridProxy()
    {
      if ((UnityEngine.Object) SceneViewGridManager.s_GridProxyMesh == (UnityEngine.Object) null)
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) SceneViewGridManager.s_GridProxyMesh);
      SceneViewGridManager.s_GridProxyMesh = (Mesh) null;
      SceneViewGridManager.s_GridProxyMaterial = (Material) null;
    }
  }
}
