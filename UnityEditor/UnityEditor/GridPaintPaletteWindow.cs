// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridPaintPaletteWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
  internal class GridPaintPaletteWindow : EditorWindow
  {
    public static readonly GUIContent tilePalette = EditorGUIUtility.TextContent("Tile Palette");
    private static PrefKey kGridSelectKey = new PrefKey("Grid Painting/Select", "s");
    private static PrefKey kGridMoveKey = new PrefKey("Grid Painting/Move", "m");
    private static PrefKey kGridBrushKey = new PrefKey("Grid Painting/Brush", "b");
    private static PrefKey kGridRectangleKey = new PrefKey("Grid Painting/Rectangle", "u");
    private static PrefKey kGridPickerKey = new PrefKey("Grid Painting/Picker", "i");
    private static PrefKey kGridEraseKey = new PrefKey("Grid Painting/Erase", "d");
    private static PrefKey kGridFillKey = new PrefKey("Grid Painting/Fill", "g");
    private static PrefKey kRotateClockwise = new PrefKey("Grid Painting/Rotate Clockwise", "[");
    private static PrefKey kRotateAntiClockwise = new PrefKey("Grid Painting/Rotate Anti-Clockwise", "]");
    private static PrefKey kFlipX = new PrefKey("Grid Painting/Flip X", "#[");
    private static PrefKey kFlipY = new PrefKey("Grid Painting/Flip Y", "#]");
    private const float k_DropdownWidth = 200f;
    private const float k_ActiveTargetLabelWidth = 90f;
    private const float k_ActiveTargetDropdownWidth = 130f;
    private const float k_TopAreaHeight = 95f;
    private const float k_MinBrushInspectorHeight = 50f;
    private const float k_MinClipboardHeight = 200f;
    private const float k_ToolbarHeight = 17f;
    private const float k_ResizerDragRectPadding = 10f;
    private PaintableSceneViewGrid m_PaintableSceneViewGrid;
    private static List<GridPaintPaletteWindow> s_Instances;
    [SerializeField]
    private PreviewResizer m_PreviewResizer;
    [SerializeField]
    private GameObject m_Palette;
    private GameObject m_PaletteInstance;
    private GridPalettesDropdown m_PaletteDropdown;
    [SerializeField]
    public GameObject m_Target;
    private Vector2 m_BrushScroll;
    private GridBrushEditorBase m_PreviousToolActivatedEditor;
    private GridBrushBase.Tool m_PreviousToolActivated;
    private PreviewRenderUtility m_PreviewUtility;

    public PaintableGrid paintableSceneViewGrid
    {
      get
      {
        return (PaintableGrid) this.m_PaintableSceneViewGrid;
      }
    }

    public static List<GridPaintPaletteWindow> instances
    {
      get
      {
        if (GridPaintPaletteWindow.s_Instances == null)
          GridPaintPaletteWindow.s_Instances = new List<GridPaintPaletteWindow>();
        return GridPaintPaletteWindow.s_Instances;
      }
    }

    public GameObject palette
    {
      get
      {
        return this.m_Palette;
      }
      set
      {
        if (!((UnityEngine.Object) this.m_Palette != (UnityEngine.Object) value))
          return;
        this.clipboardView.OnBeforePaletteSelectionChanged();
        this.m_Palette = value;
        this.clipboardView.OnAfterPaletteSelectionChanged();
        TilemapEditorUserSettings.lastUsedPalette = this.m_Palette;
      }
    }

    public GameObject paletteInstance
    {
      get
      {
        return this.m_PaletteInstance;
      }
    }

    public GridPaintPaletteClipboard clipboardView { get; private set; }

    public PreviewRenderUtility previewUtility
    {
      get
      {
        if (this.m_PreviewUtility == null)
          this.InitPreviewUtility();
        return this.m_PreviewUtility;
      }
    }

    private void OnSelectionChange()
    {
      GameObject activeGameObject = Selection.activeGameObject;
      if (!((UnityEngine.Object) activeGameObject != (UnityEngine.Object) null) || !EditorUtility.IsPersistent((UnityEngine.Object) activeGameObject) && (activeGameObject.hideFlags & HideFlags.NotEditable) == HideFlags.None)
        return;
      string assetPath = AssetDatabase.GetAssetPath((UnityEngine.Object) activeGameObject);
      foreach (object obj in AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath))
      {
        if (obj.GetType() == typeof (GridPalette))
        {
          GameObject gameObject = (GameObject) AssetDatabase.LoadMainAssetAtPath(assetPath);
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) this.palette)
          {
            this.palette = gameObject;
            this.Repaint();
            break;
          }
          break;
        }
      }
    }

    private void OnGUI()
    {
      this.HandleContextMenu();
      EditorGUILayout.BeginVertical();
      GUILayout.Space(10f);
      EditorGUILayout.BeginHorizontal();
      float pixels = (float) (((double) Screen.width / (double) EditorGUIUtility.pixelsPerPoint - (double) GridPaintPaletteWindow.Styles.toolbarWidth) * 0.5);
      GUILayout.Space(pixels);
      UnityEditorInternal.EditMode.DoInspectorToolbar(GridPaintPaletteWindow.Styles.sceneViewEditModes, GridPaintPaletteWindow.Styles.toolContents, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(pixels);
      this.DoActiveTargetsGUI();
      EditorGUILayout.EndHorizontal();
      GUILayout.Space(6f);
      EditorGUILayout.EndVertical();
      EditorGUILayout.BeginVertical();
      Rect rect = EditorGUILayout.BeginHorizontal(GUIContent.none, (GUIStyle) "Toolbar", new GUILayoutOption[0]);
      this.DoClipboardHeader();
      EditorGUILayout.EndHorizontal();
      float height = (float) ((double) this.position.height - (double) this.m_PreviewResizer.ResizeHandle(this.position, 50f, 200f, 17f, new Rect(210f, 0.0f, (float) ((double) this.position.width - 200.0 - 10.0), 17f)) - 95.0);
      Rect position = new Rect(0.0f, rect.yMax, this.position.width, height);
      this.OnClipboardGUI(position);
      EditorGUILayout.EndVertical();
      GUILayout.Space(position.height);
      EditorGUILayout.BeginVertical();
      EditorGUILayout.BeginHorizontal(GUIContent.none, (GUIStyle) "Toolbar", new GUILayoutOption[0]);
      this.DoBrushesDropdown();
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
      this.m_BrushScroll = GUILayout.BeginScrollView(this.m_BrushScroll, false, false, new GUILayoutOption[0]);
      GUILayout.Space(5f);
      this.OnBrushInspectorGUI();
      GUILayout.EndScrollView();
      EditorGUILayout.EndVertical();
      Color color = Handles.color;
      Handles.color = Color.black;
      Handles.DrawLine(new Vector3(0.0f, position.yMax + 0.5f, 0.0f), new Vector3((float) Screen.width, position.yMax + 0.5f, 0.0f));
      Handles.color = Color.black.AlphaMultiplied(0.33f);
      Handles.DrawLine(new Vector3(0.0f, GUILayoutUtility.GetLastRect().yMax + 0.5f, 0.0f), new Vector3((float) Screen.width, GUILayoutUtility.GetLastRect().yMax + 0.5f, 0.0f));
      Handles.color = color;
      EditorGUILayout.BeginVertical();
      GUILayout.Space(2f);
      EditorGUILayout.EndVertical();
      if (AssetPreview.IsLoadingAssetPreviews(this.GetInstanceID()))
        this.Repaint();
      if (Event.current.type != EventType.MouseDown)
        return;
      GUIUtility.keyboardControl = 0;
    }

    public void ResetPreviewInstance()
    {
      if (this.m_PreviewUtility == null)
        this.InitPreviewUtility();
      this.DestroyPreviewInstance();
      if (!((UnityEngine.Object) this.palette != (UnityEngine.Object) null))
        return;
      this.m_PaletteInstance = this.previewUtility.InstantiatePrefabInScene(this.palette);
      PrefabUtility.DisconnectPrefabInstance((UnityEngine.Object) this.m_PaletteInstance);
      EditorUtility.InitInstantiatedPreviewRecursive(this.m_PaletteInstance);
      this.m_PaletteInstance.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
      this.m_PaletteInstance.transform.rotation = Quaternion.identity;
      this.m_PaletteInstance.transform.localScale = Vector3.one;
      string assetPath = AssetDatabase.GetAssetPath((UnityEngine.Object) this.palette);
      GridPalette gridPalette = AssetDatabase.LoadAssetAtPath<GridPalette>(assetPath);
      if ((UnityEngine.Object) gridPalette != (UnityEngine.Object) null)
      {
        if (gridPalette.cellSizing == GridPalette.CellSizing.Automatic)
        {
          Grid component = this.m_PaletteInstance.GetComponent<Grid>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            component.cellSize = GridPaletteUtility.CalculateAutoCellSize(component, component.cellSize);
          else
            Debug.LogWarning((object) ("Grid component not found from: " + assetPath));
        }
      }
      else
        Debug.LogWarning((object) ("GridPalette subasset not found from: " + assetPath));
      foreach (Renderer componentsInChild in this.m_PaletteInstance.GetComponentsInChildren<Renderer>())
      {
        componentsInChild.gameObject.layer = Camera.PreviewCullingLayer;
        componentsInChild.allowOcclusionWhenDynamic = false;
      }
      foreach (Component componentsInChild in this.m_PaletteInstance.GetComponentsInChildren<Transform>())
        componentsInChild.gameObject.hideFlags = HideFlags.HideAndDontSave;
      PreviewRenderUtility.SetEnabledRecursive(this.m_PaletteInstance, false);
      this.clipboardView.ResetPreviewMesh();
    }

    public void DestroyPreviewInstance()
    {
      if (!((UnityEngine.Object) this.m_PaletteInstance != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_PaletteInstance);
    }

    public void InitPreviewUtility()
    {
      this.m_PreviewUtility = new PreviewRenderUtility(true, true);
      this.m_PreviewUtility.camera.cullingMask = 1 << Camera.PreviewCullingLayer;
      this.m_PreviewUtility.camera.gameObject.layer = Camera.PreviewCullingLayer;
      this.m_PreviewUtility.lights[0].gameObject.layer = Camera.PreviewCullingLayer;
      this.m_PreviewUtility.camera.orthographic = true;
      this.m_PreviewUtility.camera.orthographicSize = 5f;
      this.m_PreviewUtility.camera.transform.position = new Vector3(0.0f, 0.0f, -10f);
      this.m_PreviewUtility.ambientColor = new Color(1f, 1f, 1f, 0.0f);
      this.ResetPreviewInstance();
      this.clipboardView.SetupPreviewCameraOnInit();
    }

    private void HandleContextMenu()
    {
      if (Event.current.type != EventType.ContextClick)
        return;
      this.DoContextMenu();
      Event.current.Use();
    }

    public void SavePalette()
    {
      if (!((UnityEngine.Object) this.paletteInstance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.palette != (UnityEngine.Object) null))
        return;
      GridPaintingState.savingPalette = true;
      this.SetHideFlagsRecursivelyIgnoringTilemapChildren(this.paletteInstance, HideFlags.HideInHierarchy);
      PrefabUtility.ReplacePrefab(this.paletteInstance, (UnityEngine.Object) this.palette, ReplacePrefabOptions.ReplaceNameBased);
      this.SetHideFlagsRecursivelyIgnoringTilemapChildren(this.paletteInstance, HideFlags.HideAndDontSave);
      GridPaintingState.savingPalette = false;
    }

    private void SetHideFlagsRecursivelyIgnoringTilemapChildren(GameObject root, HideFlags flags)
    {
      root.hideFlags = flags;
      if (!((UnityEngine.Object) root.GetComponent<Tilemap>() == (UnityEngine.Object) null))
        return;
      for (int index = 0; index < root.transform.childCount; ++index)
        this.SetHideFlagsRecursivelyIgnoringTilemapChildren(root.transform.GetChild(index).gameObject, flags);
    }

    private void DoContextMenu()
    {
      GenericMenu genericMenu = new GenericMenu();
      if ((UnityEngine.Object) GridPaintingState.scenePaintTarget != (UnityEngine.Object) null)
        genericMenu.AddItem(GridPaintPaletteWindow.Styles.selectPaintTarget, false, new GenericMenu.MenuFunction(this.SelectPaintTarget));
      else
        genericMenu.AddDisabledItem(GridPaintPaletteWindow.Styles.selectPaintTarget);
      if ((UnityEngine.Object) this.palette != (UnityEngine.Object) null)
        genericMenu.AddItem(GridPaintPaletteWindow.Styles.selectPalettePrefab, false, new GenericMenu.MenuFunction(this.SelectPaletteAsset));
      else
        genericMenu.AddDisabledItem(GridPaintPaletteWindow.Styles.selectPalettePrefab);
      if ((UnityEngine.Object) this.clipboardView.activeTile != (UnityEngine.Object) null)
        genericMenu.AddItem(GridPaintPaletteWindow.Styles.selectTileAsset, false, new GenericMenu.MenuFunction(this.SelectTileAsset));
      else
        genericMenu.AddDisabledItem(GridPaintPaletteWindow.Styles.selectTileAsset);
      genericMenu.AddSeparator("");
      if (this.clipboardView.unlocked)
        genericMenu.AddItem(GridPaintPaletteWindow.Styles.lockPaletteEditing, false, new GenericMenu.MenuFunction(this.FlipLocked));
      else
        genericMenu.AddItem(GridPaintPaletteWindow.Styles.unlockPaletteEditing, false, new GenericMenu.MenuFunction(this.FlipLocked));
      genericMenu.ShowAsContext();
    }

    private void FlipLocked()
    {
      this.clipboardView.unlocked = !this.clipboardView.unlocked;
    }

    private void SelectPaintTarget()
    {
      Selection.activeObject = (UnityEngine.Object) GridPaintingState.scenePaintTarget;
    }

    private void SelectPaletteAsset()
    {
      Selection.activeObject = (UnityEngine.Object) this.palette;
    }

    private void SelectTileAsset()
    {
      Selection.activeObject = (UnityEngine.Object) this.clipboardView.activeTile;
    }

    private bool NotOverridingColor(GridBrush defaultGridBrush)
    {
      foreach (GridBrush.BrushCell cell in defaultGridBrush.cells)
      {
        TileBase tile = cell.tile;
        if (tile is Tile && ((tile as Tile).flags & TileFlags.LockColor) == TileFlags.None)
          return true;
      }
      return false;
    }

    private void DoBrushesDropdown()
    {
      if (!EditorGUILayout.DropdownButton(GUIContent.Temp(GridPaintingState.gridBrush.name), FocusType.Passive, EditorStyles.toolbarPopup, GUILayout.Width(200f)))
        return;
      PopupWindow.Show(GUILayoutUtility.topLevel.GetLast(), (PopupWindowContent) new GridBrushesDropdown((IFlexibleMenuItemProvider) new GridBrushesDropdown.MenuItemProvider(), GridPaletteBrushes.brushes.IndexOf(GridPaintingState.gridBrush), (FlexibleMenuModifyItemUI) null, new Action<int, object>(this.SelectBrush), 200f));
    }

    private void SelectBrush(int i, object o)
    {
      GridPaintingState.gridBrush = GridPaletteBrushes.brushes[i];
    }

    public void OnEnable()
    {
      GridPaintPaletteWindow.instances.Add(this);
      if ((UnityEngine.Object) this.clipboardView == (UnityEngine.Object) null)
      {
        this.clipboardView = ScriptableObject.CreateInstance<GridPaintPaletteClipboard>();
        this.clipboardView.owner = this;
        this.clipboardView.hideFlags = HideFlags.HideAndDontSave;
        this.clipboardView.unlocked = false;
      }
      if ((UnityEngine.Object) this.m_PaintableSceneViewGrid == (UnityEngine.Object) null)
      {
        this.m_PaintableSceneViewGrid = ScriptableObject.CreateInstance<PaintableSceneViewGrid>();
        this.m_PaintableSceneViewGrid.hideFlags = HideFlags.HideAndDontSave;
      }
      GridPaletteBrushes.FlushCache();
      EditorApplication.globalEventHandler += new EditorApplication.CallbackFunction(this.HotkeyHandler);
      UnityEditorInternal.EditMode.editModeStarted += new Action<IToolModeOwner, UnityEditorInternal.EditMode.SceneViewEditMode>(this.OnEditModeStart);
      UnityEditorInternal.EditMode.editModeEnded += new Action<IToolModeOwner>(this.OnEditModeEnd);
      GridSelection.gridSelectionChanged += new Action(this.OnGridSelectionChanged);
      GridPaintingState.RegisterPainterInterest((UnityEngine.Object) this);
      GridPaintingState.scenePaintTargetChanged += new Action<GameObject>(this.OnScenePaintTargetChanged);
      GridPaintingState.brushChanged += new Action<GridBrushBase>(this.OnBrushChanged);
      SceneView.onSceneGUIDelegate += new SceneView.OnSceneFunc(this.OnSceneViewGUI);
      PrefabUtility.prefabInstanceUpdated += new PrefabUtility.PrefabInstanceUpdated(this.PrefabInstanceUpdated);
      AssetPreview.SetPreviewTextureCacheSize(256, this.GetInstanceID());
      this.wantsMouseMove = true;
      this.wantsMouseEnterLeaveWindow = true;
      if (this.m_PreviewResizer == null)
      {
        this.m_PreviewResizer = new PreviewResizer();
        this.m_PreviewResizer.Init("TilemapBrushInspector");
      }
      this.minSize = new Vector2(240f, 200f);
      if ((UnityEngine.Object) this.palette == (UnityEngine.Object) null && (UnityEngine.Object) TilemapEditorUserSettings.lastUsedPalette != (UnityEngine.Object) null && GridPalettes.palettes.Contains(TilemapEditorUserSettings.lastUsedPalette))
        this.palette = TilemapEditorUserSettings.lastUsedPalette;
      Tools.onToolChanged += new Tools.OnToolChangedFunc(this.ToolChanged);
    }

    private void PrefabInstanceUpdated(GameObject updatedPrefab)
    {
      if (!((UnityEngine.Object) this.m_PaletteInstance != (UnityEngine.Object) null) || !(PrefabUtility.GetPrefabParent((UnityEngine.Object) updatedPrefab) == (UnityEngine.Object) this.m_Palette) || GridPaintingState.savingPalette)
        return;
      this.ResetPreviewInstance();
      this.Repaint();
    }

    private void OnBrushChanged(GridBrushBase brush)
    {
      this.DisableFocus();
      if (brush is GridBrush)
        this.EnableFocus();
      SceneView.RepaintAll();
    }

    private void OnGridSelectionChanged()
    {
      this.Repaint();
    }

    private void ToolChanged(Tool from, Tool to)
    {
      if (to != Tool.None && PaintableGrid.InGridEditMode())
        UnityEditorInternal.EditMode.QuitEditMode();
      this.Repaint();
    }

    public void OnDisable()
    {
      this.CallOnToolDeactivated();
      GridPaintPaletteWindow.instances.Remove(this);
      this.DestroyPreviewInstance();
      EditorApplication.globalEventHandler -= new EditorApplication.CallbackFunction(this.HotkeyHandler);
      UnityEditorInternal.EditMode.editModeStarted -= new Action<IToolModeOwner, UnityEditorInternal.EditMode.SceneViewEditMode>(this.OnEditModeStart);
      UnityEditorInternal.EditMode.editModeEnded -= new Action<IToolModeOwner>(this.OnEditModeEnd);
      Tools.onToolChanged -= new Tools.OnToolChangedFunc(this.ToolChanged);
      GridSelection.gridSelectionChanged -= new Action(this.OnGridSelectionChanged);
      SceneView.onSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnSceneViewGUI);
      GridPaintingState.scenePaintTargetChanged -= new Action<GameObject>(this.OnScenePaintTargetChanged);
      GridPaintingState.brushChanged -= new Action<GridBrushBase>(this.OnBrushChanged);
      GridPaintingState.UnregisterPainterInterest((UnityEngine.Object) this);
      PrefabUtility.prefabInstanceUpdated -= new PrefabUtility.PrefabInstanceUpdated(this.PrefabInstanceUpdated);
    }

    private void OnScenePaintTargetChanged(GameObject scenePaintTarget)
    {
      this.DisableFocus();
      this.EnableFocus();
      this.Repaint();
    }

    public void OnDestroy()
    {
      this.DestroyPreviewInstance();
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.clipboardView);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_PaintableSceneViewGrid);
      if (this.m_PreviewUtility != null)
        this.m_PreviewUtility.Cleanup();
      this.m_PreviewUtility = (PreviewRenderUtility) null;
      if (PaintableGrid.InGridEditMode())
        UnityEditorInternal.EditMode.QuitEditMode();
      if (GridPaintPaletteWindow.instances.Count > 1)
        return;
      GridPaintingState.gridBrush = (GridBrushBase) null;
    }

    public void ChangeToTool(GridBrushBase.Tool tool)
    {
      UnityEditorInternal.EditMode.ChangeEditMode(PaintableGrid.BrushToolToEditMode(tool), new Bounds(Vector3.zero, Vector3.positiveInfinity), (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
      this.Repaint();
    }

    private void HotkeyHandler()
    {
      if (GridPaintPaletteWindow.kGridSelectKey.activated)
      {
        if (UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect)
          UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
        else
          UnityEditorInternal.EditMode.QuitEditMode();
        Event.current.Use();
      }
      if (GridPaintPaletteWindow.kGridMoveKey.activated)
      {
        if (UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridMove)
          UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.GridMove, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
        else
          UnityEditorInternal.EditMode.QuitEditMode();
        Event.current.Use();
      }
      if (GridPaintPaletteWindow.kGridBrushKey.activated)
      {
        if (UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridPainting)
          UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.GridPainting, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
        else
          UnityEditorInternal.EditMode.QuitEditMode();
        Event.current.Use();
      }
      if (GridPaintPaletteWindow.kGridEraseKey.activated)
      {
        if (UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridEraser)
          UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.GridEraser, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
        else
          UnityEditorInternal.EditMode.QuitEditMode();
        Event.current.Use();
      }
      if (GridPaintPaletteWindow.kGridFillKey.activated)
      {
        if (UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridFloodFill)
          UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.GridFloodFill, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
        else
          UnityEditorInternal.EditMode.QuitEditMode();
        Event.current.Use();
      }
      if (GridPaintPaletteWindow.kGridPickerKey.activated)
      {
        if (UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridPicking)
          UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.GridPicking, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
        else
          UnityEditorInternal.EditMode.QuitEditMode();
        Event.current.Use();
      }
      if (GridPaintPaletteWindow.kGridRectangleKey.activated)
      {
        if (UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridBox)
          UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.GridBox, (IToolModeOwner) ScriptableSingleton<GridPaintingState>.instance);
        else
          UnityEditorInternal.EditMode.QuitEditMode();
        Event.current.Use();
      }
      if (!((UnityEngine.Object) GridPaintingState.gridBrush != (UnityEngine.Object) null) || !((UnityEngine.Object) GridPaintingState.activeGrid != (UnityEngine.Object) null))
        return;
      if (GridPaintPaletteWindow.kRotateClockwise.activated)
      {
        GridPaintingState.gridBrush.Rotate(GridBrushBase.RotationDirection.Clockwise, GridPaintingState.activeGrid.cellLayout);
        GridPaintingState.activeGrid.Repaint();
        Event.current.Use();
      }
      if (GridPaintPaletteWindow.kRotateAntiClockwise.activated)
      {
        GridPaintingState.gridBrush.Rotate(GridBrushBase.RotationDirection.CounterClockwise, GridPaintingState.activeGrid.cellLayout);
        GridPaintingState.activeGrid.Repaint();
        Event.current.Use();
      }
      if (GridPaintPaletteWindow.kFlipX.activated)
      {
        GridPaintingState.gridBrush.Flip(GridBrushBase.FlipAxis.X, GridPaintingState.activeGrid.cellLayout);
        GridPaintingState.activeGrid.Repaint();
        Event.current.Use();
      }
      if (GridPaintPaletteWindow.kFlipY.activated)
      {
        GridPaintingState.gridBrush.Flip(GridBrushBase.FlipAxis.Y, GridPaintingState.activeGrid.cellLayout);
        GridPaintingState.activeGrid.Repaint();
        Event.current.Use();
      }
    }

    public void OnEditModeStart(IToolModeOwner owner, UnityEditorInternal.EditMode.SceneViewEditMode editMode)
    {
      if ((UnityEngine.Object) GridPaintingState.gridBrush != (UnityEngine.Object) null && PaintableGrid.InGridEditMode() && (UnityEngine.Object) GridPaintingState.activeBrushEditor != (UnityEngine.Object) null)
      {
        GridBrushBase.Tool brushTool = PaintableGrid.EditModeToBrushTool(editMode);
        GridPaintingState.activeBrushEditor.OnToolActivated(brushTool);
        this.m_PreviousToolActivatedEditor = GridPaintingState.activeBrushEditor;
        this.m_PreviousToolActivated = brushTool;
        for (int index = 0; index < GridPaintPaletteWindow.Styles.sceneViewEditModes.Length; ++index)
        {
          if (GridPaintPaletteWindow.Styles.sceneViewEditModes[index] == editMode)
          {
            Cursor.SetCursor(GridPaintPaletteWindow.Styles.mouseCursorTextures[index], !((UnityEngine.Object) GridPaintPaletteWindow.Styles.mouseCursorTextures[index] != (UnityEngine.Object) null) ? Vector2.zero : GridPaintPaletteWindow.Styles.mouseCursorOSHotspot[(int) SystemInfo.operatingSystemFamily], CursorMode.Auto);
            break;
          }
        }
      }
      this.Repaint();
    }

    public void OnEditModeEnd(IToolModeOwner owner)
    {
      if (UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridMove && UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect)
        GridSelection.Clear();
      this.CallOnToolDeactivated();
      this.Repaint();
    }

    private void CallOnToolDeactivated()
    {
      if (!((UnityEngine.Object) GridPaintingState.gridBrush != (UnityEngine.Object) null) || !((UnityEngine.Object) this.m_PreviousToolActivatedEditor != (UnityEngine.Object) null))
        return;
      this.m_PreviousToolActivatedEditor.OnToolDeactivated(this.m_PreviousToolActivated);
      this.m_PreviousToolActivatedEditor = (GridBrushEditorBase) null;
      if (!PaintableGrid.InGridEditMode())
        Cursor.SetCursor((Texture2D) null, Vector2.zero, CursorMode.Auto);
    }

    private void OnBrushInspectorGUI()
    {
      if ((UnityEngine.Object) GridPaintingState.gridBrush == (UnityEngine.Object) null)
        return;
      EditorGUI.BeginChangeCheck();
      if ((UnityEngine.Object) GridPaintingState.activeBrushEditor != (UnityEngine.Object) null)
        GridPaintingState.activeBrushEditor.OnPaintInspectorGUI();
      else if ((UnityEngine.Object) GridPaintingState.fallbackEditor != (UnityEngine.Object) null)
        GridPaintingState.fallbackEditor.OnInspectorGUI();
      if (!EditorGUI.EndChangeCheck())
        return;
      GridPaletteBrushes.ActiveGridBrushAssetChanged();
    }

    private void DoActiveTargetsGUI()
    {
      bool flag = (UnityEngine.Object) GridPaintingState.scenePaintTarget != (UnityEngine.Object) null;
      using (new EditorGUI.DisabledScope(!flag || GridPaintingState.validTargets == null))
      {
        GUILayout.Label(GridPaintPaletteWindow.Styles.activeTargetLabel, new GUILayoutOption[1]
        {
          GUILayout.Width(90f)
        });
        if (!EditorGUILayout.DropdownButton(GUIContent.Temp(!flag ? "Nothing" : GridPaintingState.scenePaintTarget.name), FocusType.Passive, EditorStyles.popup, GUILayout.Width(130f)))
          return;
        PopupWindow.Show(GUILayoutUtility.topLevel.GetLast(), (PopupWindowContent) new GridPaintTargetsDropdown((IFlexibleMenuItemProvider) new GridPaintTargetsDropdown.MenuItemProvider(), !flag ? 0 : Array.IndexOf<GameObject>(GridPaintingState.validTargets, GridPaintingState.scenePaintTarget), (FlexibleMenuModifyItemUI) null, new Action<int, object>(this.SelectTarget), 130f));
      }
    }

    private void SelectTarget(int i, object o)
    {
      GridPaintingState.scenePaintTarget = o as GameObject;
      if (!((UnityEngine.Object) GridPaintingState.scenePaintTarget != (UnityEngine.Object) null))
        return;
      EditorGUIUtility.PingObject((UnityEngine.Object) GridPaintingState.scenePaintTarget);
    }

    private void DoClipboardHeader()
    {
      if (!GridPalettes.palettes.Contains(this.palette) || (UnityEngine.Object) this.palette == (UnityEngine.Object) null)
      {
        GridPalettes.CleanCache();
        if (GridPalettes.palettes.Count > 0)
          this.palette = GridPalettes.palettes.LastOrDefault<GameObject>();
      }
      EditorGUILayout.BeginHorizontal();
      this.DoPalettesDropdown();
      using (new EditorGUI.DisabledScope((UnityEngine.Object) this.palette == (UnityEngine.Object) null))
        this.clipboardView.unlocked = GUILayout.Toggle(this.clipboardView.unlocked, !this.clipboardView.isModified ? GridPaintPaletteWindow.Styles.edit : GridPaintPaletteWindow.Styles.editModified, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
    }

    private void DoPalettesDropdown()
    {
      string t = !((UnityEngine.Object) this.palette != (UnityEngine.Object) null) ? GridPaintPaletteWindow.Styles.createNewPalette.text : this.palette.name;
      Rect rect = GUILayoutUtility.GetRect(GUIContent.Temp(t), EditorStyles.toolbarDropDown, new GUILayoutOption[1]{ GUILayout.Width(200f) });
      if (GridPalettes.palettes.Count == 0)
      {
        if (!EditorGUI.DropdownButton(rect, GUIContent.Temp(t), FocusType.Passive, EditorStyles.toolbarDropDown))
          return;
        this.OpenAddPalettePopup(rect);
      }
      else
      {
        GUIContent content = GUIContent.Temp(GridPalettes.palettes.Count <= 0 || !((UnityEngine.Object) this.palette != (UnityEngine.Object) null) ? GridPaintPaletteWindow.Styles.createNewPalette.text : this.palette.name);
        if (EditorGUI.DropdownButton(rect, content, FocusType.Passive, EditorStyles.toolbarPopup))
        {
          this.m_PaletteDropdown = new GridPalettesDropdown((IFlexibleMenuItemProvider) new GridPalettesDropdown.MenuItemProvider(), GridPalettes.palettes.IndexOf(this.palette), (FlexibleMenuModifyItemUI) null, new Action<int, object>(this.SelectPalette), 200f);
          PopupWindow.Show(GUILayoutUtility.topLevel.GetLast(), (PopupWindowContent) this.m_PaletteDropdown);
        }
      }
    }

    private void SelectPalette(int i, object o)
    {
      if (i < GridPalettes.palettes.Count)
      {
        this.palette = GridPalettes.palettes[i];
      }
      else
      {
        this.m_PaletteDropdown.editorWindow.Close();
        this.OpenAddPalettePopup(new Rect(0.0f, 0.0f, 0.0f, 0.0f));
      }
    }

    private void OpenAddPalettePopup(Rect rect)
    {
      if (!GridPaletteAddPopup.ShowAtPosition(rect, this))
        return;
      GUIUtility.ExitGUI();
    }

    private void OnClipboardGUI(Rect position)
    {
      if (Event.current.type != EventType.Layout && position.Contains(Event.current.mousePosition) && (UnityEngine.Object) GridPaintingState.activeGrid != (UnityEngine.Object) this.clipboardView)
      {
        GridPaintingState.activeGrid = (PaintableGrid) this.clipboardView;
        SceneView.RepaintAll();
      }
      if ((UnityEngine.Object) this.palette == (UnityEngine.Object) null)
      {
        Color color = GUI.color;
        GUI.color = Color.gray;
        if (GridPalettes.palettes.Count == 0)
          GUI.Label(new Rect(position.center.x - GUI.skin.label.CalcSize(GridPaintPaletteWindow.Styles.emptyProjectInfo).x * 0.5f, position.center.y, 500f, 100f), GridPaintPaletteWindow.Styles.emptyProjectInfo);
        else
          GUI.Label(new Rect(position.center.x - GUI.skin.label.CalcSize(GridPaintPaletteWindow.Styles.invalidClipboardInfo).x * 0.5f, position.center.y, 500f, 100f), GridPaintPaletteWindow.Styles.invalidClipboardInfo);
        GUI.color = color;
      }
      else
      {
        bool enabled = GUI.enabled;
        GUI.enabled = !this.clipboardView.showNewEmptyClipboardInfo || DragAndDrop.objectReferences.Length > 0;
        if (Event.current.type == EventType.Repaint)
          this.clipboardView.guiRect = position;
        EditorGUI.BeginChangeCheck();
        this.clipboardView.OnGUI();
        if (EditorGUI.EndChangeCheck())
          this.Repaint();
        GUI.enabled = enabled;
        if (!this.clipboardView.showNewEmptyClipboardInfo)
          return;
        Color color = GUI.color;
        GUI.color = Color.gray;
        GUI.Label(new Rect(position.center.x - GUI.skin.label.CalcSize(GridPaintPaletteWindow.Styles.emptyClipboardInfo).x * 0.5f, position.center.y, 500f, 100f), GridPaintPaletteWindow.Styles.emptyClipboardInfo);
        GUI.color = color;
      }
    }

    private void OnSceneViewGUI(SceneView sceneView)
    {
      if ((UnityEngine.Object) GridPaintingState.defaultBrush != (UnityEngine.Object) null && (UnityEngine.Object) GridPaintingState.scenePaintTarget != (UnityEngine.Object) null)
      {
        SceneViewOverlay.Window(GridPaintPaletteWindow.Styles.rendererOverlayTitleLabel, new SceneViewOverlay.WindowFunction(this.DisplayFocusMode), 500, SceneViewOverlay.WindowDisplayOption.OneWindowPerTitle);
      }
      else
      {
        if (TilemapEditorUserSettings.focusMode == TilemapEditorUserSettings.FocusMode.None)
          return;
        this.DisableFocus();
        TilemapEditorUserSettings.focusMode = TilemapEditorUserSettings.FocusMode.None;
      }
    }

    private void DisplayFocusMode(UnityEngine.Object displayTarget, SceneView sceneView)
    {
      TilemapEditorUserSettings.FocusMode focusMode1 = TilemapEditorUserSettings.focusMode;
      TilemapEditorUserSettings.FocusMode focusMode2 = (TilemapEditorUserSettings.FocusMode) EditorGUILayout.EnumPopup(GridPaintPaletteWindow.Styles.focusLabel, (Enum) focusMode1, new GUILayoutOption[0]);
      if (focusMode2 == focusMode1)
        return;
      this.DisableFocus();
      TilemapEditorUserSettings.focusMode = focusMode2;
      this.EnableFocus();
    }

    private void EnableFocus()
    {
      switch (TilemapEditorUserSettings.focusMode)
      {
        case TilemapEditorUserSettings.FocusMode.Tilemap:
          if ((UnityEngine.Object) SceneView.lastActiveSceneView != (UnityEngine.Object) null)
            SceneView.lastActiveSceneView.SetSceneViewFiltering(true);
          HierarchyProperty.FilterSingleSceneObject(GridPaintingState.scenePaintTarget.GetInstanceID(), false);
          break;
        case TilemapEditorUserSettings.FocusMode.Grid:
          Tilemap component = GridPaintingState.scenePaintTarget.GetComponent<Tilemap>();
          if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.layoutGrid != (UnityEngine.Object) null))
            break;
          if ((UnityEngine.Object) SceneView.lastActiveSceneView != (UnityEngine.Object) null)
            SceneView.lastActiveSceneView.SetSceneViewFiltering(true);
          HierarchyProperty.FilterSingleSceneObject(component.layoutGrid.gameObject.GetInstanceID(), false);
          break;
      }
    }

    private void DisableFocus()
    {
      if (TilemapEditorUserSettings.focusMode == TilemapEditorUserSettings.FocusMode.None)
        return;
      HierarchyProperty.ClearSceneObjectsFilter();
      if (!((UnityEngine.Object) SceneView.lastActiveSceneView != (UnityEngine.Object) null))
        return;
      SceneView.lastActiveSceneView.SetSceneViewFiltering(false);
    }

    [MenuItem("Window/Tile Palette", false, 2015)]
    public static void OpenTilemapPalette()
    {
      EditorWindow.GetWindow<GridPaintPaletteWindow>().titleContent = GridPaintPaletteWindow.tilePalette;
    }

    private static class Styles
    {
      public static readonly GUIContent[] toolContents;
      public static readonly UnityEditorInternal.EditMode.SceneViewEditMode[] sceneViewEditModes;
      public static readonly string[] mouseCursorOSPath;
      public static readonly Vector2[] mouseCursorOSHotspot;
      public static readonly string[] mouseCursorTexturePaths;
      public static readonly Texture2D[] mouseCursorTextures;
      public static readonly GUIContent emptyProjectInfo;
      public static readonly GUIContent emptyClipboardInfo;
      public static readonly GUIContent invalidClipboardInfo;
      public static readonly GUIContent selectPaintTarget;
      public static readonly GUIContent selectPalettePrefab;
      public static readonly GUIContent selectTileAsset;
      public static readonly GUIContent unlockPaletteEditing;
      public static readonly GUIContent lockPaletteEditing;
      public static readonly GUIContent createNewPalette;
      public static readonly GUIContent focusLabel;
      public static readonly GUIContent rendererOverlayTitleLabel;
      public static readonly GUIContent activeTargetLabel;
      public static readonly GUIContent edit;
      public static readonly GUIContent editModified;
      public static readonly GUIStyle ToolbarStyle;
      public static readonly GUIStyle ToolbarTitleStyle;
      public static float toolbarWidth;

      static Styles()
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GridPaintPaletteWindow.Styles.\u003CStyles\u003Ec__AnonStorey0 stylesCAnonStorey0 = new GridPaintPaletteWindow.Styles.\u003CStyles\u003Ec__AnonStorey0();
        GridPaintPaletteWindow.Styles.toolContents = new GUIContent[7]
        {
          EditorGUIUtility.IconContent("Grid.Default", "|Select an area of the grid (S)"),
          EditorGUIUtility.IconContent("Grid.MoveTool", "|Move selection with active brush (M)"),
          EditorGUIUtility.IconContent("Grid.PaintTool", "|Paint with active brush (B)"),
          EditorGUIUtility.IconContent("Grid.BoxTool", "|Paint a filled box with active brush (U)"),
          EditorGUIUtility.IconContent("Grid.PickingTool", "|Pick or marquee select new brush (Ctrl/CMD)."),
          EditorGUIUtility.IconContent("Grid.EraserTool", "|Erase with active brush (Shift)"),
          EditorGUIUtility.IconContent("Grid.FillTool", "|Flood fill with active brush (G)")
        };
        GridPaintPaletteWindow.Styles.sceneViewEditModes = new UnityEditorInternal.EditMode.SceneViewEditMode[7]
        {
          UnityEditorInternal.EditMode.SceneViewEditMode.GridSelect,
          UnityEditorInternal.EditMode.SceneViewEditMode.GridMove,
          UnityEditorInternal.EditMode.SceneViewEditMode.GridPainting,
          UnityEditorInternal.EditMode.SceneViewEditMode.GridBox,
          UnityEditorInternal.EditMode.SceneViewEditMode.GridPicking,
          UnityEditorInternal.EditMode.SceneViewEditMode.GridEraser,
          UnityEditorInternal.EditMode.SceneViewEditMode.GridFloodFill
        };
        GridPaintPaletteWindow.Styles.mouseCursorOSPath = new string[4]
        {
          "",
          "Cursors/macOS",
          "Cursors/Windows",
          "Cursors/Linux"
        };
        GridPaintPaletteWindow.Styles.mouseCursorOSHotspot = new Vector2[4]
        {
          Vector2.zero,
          new Vector2(6f, 4f),
          new Vector2(6f, 4f),
          new Vector2(6f, 4f)
        };
        GridPaintPaletteWindow.Styles.mouseCursorTexturePaths = new string[7]
        {
          "",
          "Grid.MoveTool.png",
          "Grid.PaintTool.png",
          "Grid.BoxTool.png",
          "Grid.PickingTool.png",
          "Grid.EraserTool.png",
          "Grid.FillTool.png"
        };
        GridPaintPaletteWindow.Styles.emptyProjectInfo = EditorGUIUtility.TextContent("Create a new palette in the dropdown above.");
        GridPaintPaletteWindow.Styles.emptyClipboardInfo = EditorGUIUtility.TextContent("Drag Tile, Sprite or Sprite Texture assets here.");
        GridPaintPaletteWindow.Styles.invalidClipboardInfo = EditorGUIUtility.TextContent("This is an invalid clipboard. Did you delete the clipboard asset?");
        GridPaintPaletteWindow.Styles.selectPaintTarget = EditorGUIUtility.TextContent("Select Paint Target");
        GridPaintPaletteWindow.Styles.selectPalettePrefab = EditorGUIUtility.TextContent("Select Palette Prefab");
        GridPaintPaletteWindow.Styles.selectTileAsset = EditorGUIUtility.TextContent("Select Tile Asset");
        GridPaintPaletteWindow.Styles.unlockPaletteEditing = EditorGUIUtility.TextContent("Unlock Palette Editing");
        GridPaintPaletteWindow.Styles.lockPaletteEditing = EditorGUIUtility.TextContent("Lock Palette Editing");
        GridPaintPaletteWindow.Styles.createNewPalette = EditorGUIUtility.TextContent("Create New Palette");
        GridPaintPaletteWindow.Styles.focusLabel = EditorGUIUtility.TextContent("Focus On");
        GridPaintPaletteWindow.Styles.rendererOverlayTitleLabel = EditorGUIUtility.TextContent("Tilemap");
        GridPaintPaletteWindow.Styles.activeTargetLabel = EditorGUIUtility.TextContent("Active Tilemap|Specifies the currently active Tilemap used for painting in the Scene View.");
        GridPaintPaletteWindow.Styles.edit = EditorGUIUtility.TextContent("Edit");
        GridPaintPaletteWindow.Styles.editModified = EditorGUIUtility.TextContent("Edit*");
        GridPaintPaletteWindow.Styles.ToolbarStyle = (GUIStyle) "preToolbar";
        GridPaintPaletteWindow.Styles.ToolbarTitleStyle = (GUIStyle) "preToolbar";
        GridPaintPaletteWindow.Styles.mouseCursorTextures = new Texture2D[GridPaintPaletteWindow.Styles.mouseCursorTexturePaths.Length];
        int operatingSystemFamily = (int) SystemInfo.operatingSystemFamily;
        for (int index = 0; index < GridPaintPaletteWindow.Styles.mouseCursorTexturePaths.Length; ++index)
        {
          if (GridPaintPaletteWindow.Styles.mouseCursorOSPath[operatingSystemFamily] != null && GridPaintPaletteWindow.Styles.mouseCursorOSPath[operatingSystemFamily].Length > 0 && (GridPaintPaletteWindow.Styles.mouseCursorTexturePaths[index] != null && GridPaintPaletteWindow.Styles.mouseCursorTexturePaths[index].Length > 0))
          {
            string path = Paths.Combine(GridPaintPaletteWindow.Styles.mouseCursorOSPath[operatingSystemFamily], GridPaintPaletteWindow.Styles.mouseCursorTexturePaths[index]);
            GridPaintPaletteWindow.Styles.mouseCursorTextures[index] = EditorGUIUtility.LoadRequired(path) as Texture2D;
          }
          else
            GridPaintPaletteWindow.Styles.mouseCursorTextures[index] = (Texture2D) null;
        }
        // ISSUE: reference to a compiler-generated field
        stylesCAnonStorey0.toolbarStyle = (GUIStyle) "Command";
        // ISSUE: reference to a compiler-generated method
        GridPaintPaletteWindow.Styles.toolbarWidth = ((IEnumerable<GUIContent>) GridPaintPaletteWindow.Styles.toolContents).Sum<GUIContent>(new Func<GUIContent, float>(stylesCAnonStorey0.\u003C\u003Em__0));
      }
    }

    public class AssetProcessor : AssetPostprocessor
    {
      public override int GetPostprocessOrder()
      {
        return int.MaxValue;
      }

      private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
      {
        if (GridPaintingState.savingPalette)
          return;
        foreach (GridPaintPaletteWindow instance in GridPaintPaletteWindow.instances)
          instance.ResetPreviewInstance();
      }
    }

    public class PaletteAssetModificationProcessor : AssetModificationProcessor
    {
      private static string[] OnWillSaveAssets(string[] paths)
      {
        if (!GridPaintingState.savingPalette)
        {
          foreach (GridPaintPaletteWindow instance in GridPaintPaletteWindow.instances)
          {
            if (instance.clipboardView.isModified)
            {
              instance.clipboardView.SavePaletteIfNecessary();
              instance.Repaint();
            }
          }
        }
        return paths;
      }
    }
  }
}
