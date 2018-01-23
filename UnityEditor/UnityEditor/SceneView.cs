// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.AnimatedValues;
using UnityEditor.Rendering;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Scene", useTypeNameAsIconName = true)]
  public class SceneView : SearchableEditorWindow, IHasCustomMenu
  {
    private static readonly PrefColor kSceneViewBackground = new PrefColor("Scene/Background", 0.278431f, 0.278431f, 0.278431f, 0.0f);
    private static readonly PrefColor kSceneViewWire = new PrefColor("Scene/Wireframe", 0.0f, 0.0f, 0.0f, 0.5f);
    private static readonly PrefColor kSceneViewWireOverlay = new PrefColor("Scene/Wireframe Overlay", 0.0f, 0.0f, 0.0f, 0.25f);
    private static readonly PrefColor kSceneViewSelectedOutline = new PrefColor("Scene/Selected Outline", 1f, 0.4f, 0.0f, 0.0f);
    private static readonly PrefColor kSceneViewSelectedWire = new PrefColor("Scene/Wireframe Selected", 0.3686275f, 0.4666667f, 0.6078432f, 0.2509804f);
    private static readonly PrefColor kSceneViewMaterialValidateLow = new PrefColor("Scene/Material Validator Value Too Low", 1f, 0.0f, 0.0f, 1f);
    private static readonly PrefColor kSceneViewMaterialValidateHigh = new PrefColor("Scene/Material Validator Value Too High", 0.0f, 0.0f, 1f, 1f);
    private static readonly PrefColor kSceneViewMaterialValidatePureMetal = new PrefColor("Scene/Material Validator Pure Metal", 1f, 1f, 0.0f, 1f);
    internal static Color kSceneViewFrontLight = new Color(0.769f, 0.769f, 0.769f, 1f);
    internal static Color kSceneViewUpLight = new Color(0.212f, 0.227f, 0.259f, 1f);
    internal static Color kSceneViewMidLight = new Color(57f / 500f, 0.125f, 0.133f, 1f);
    internal static Color kSceneViewDownLight = new Color(0.047f, 0.043f, 0.035f, 1f);
    [NonSerialized]
    private static readonly Quaternion kDefaultRotation = Quaternion.LookRotation(new Vector3(-1f, -0.7f, -1f));
    [NonSerialized]
    private static readonly Vector3 kDefaultPivot = Vector3.zero;
    private static readonly PrefKey k2DMode = new PrefKey("Tools/2D Mode", "2");
    private static MouseCursor s_LastCursor = MouseCursor.Arrow;
    private static readonly List<SceneView.CursorRect> s_MouseRects = new List<SceneView.CursorRect>();
    private static ArrayList s_SceneViews = new ArrayList();
    [SerializeField]
    public bool m_SceneLighting = true;
    public double lastFramingTime = 0.0;
    [SerializeField]
    private bool m_isRotationLocked = false;
    public bool m_AudioPlay = false;
    [SerializeField]
    private AnimVector3 m_Position = new AnimVector3(SceneView.kDefaultPivot);
    public DrawCameraMode m_RenderMode = DrawCameraMode.Textured;
    private DrawCameraMode lastRenderMode = DrawCameraMode.Textured;
    public bool m_ValidateTrueMetals = false;
    [SerializeField]
    internal AnimQuaternion m_Rotation = new AnimQuaternion(SceneView.kDefaultRotation);
    [SerializeField]
    private AnimFloat m_Size = new AnimFloat(10f);
    [SerializeField]
    internal AnimBool m_Ortho = new AnimBool();
    [SerializeField]
    private bool m_ShowGlobalGrid = true;
    [NonSerialized]
    private Light[] m_Light = new Light[3];
    private double m_StartSearchFilterTime = -1.0;
    internal bool m_ShowSceneViewWindows = false;
    private GUIStyle m_TooLowColorStyle = (GUIStyle) null;
    private GUIStyle m_TooHighColorStyle = (GUIStyle) null;
    private GUIStyle m_PureMetalColorStyle = (GUIStyle) null;
    private int m_SelectedAlbedoSwatchIndex = 0;
    private float m_AlbedoSwatchHueTolerance = 0.1f;
    private float m_AlbedoSwatchSaturationTolerance = 0.2f;
    private ColorSpace m_LastKnownColorSpace = ColorSpace.Uninitialized;
    private static SceneView s_LastActiveSceneView;
    private static SceneView s_CurrentDrawingSceneView;
    private const float kDefaultViewSize = 10f;
    private const CameraEvent kCommandBufferCameraEvent = CameraEvent.AfterImageEffectsOpaque;
    private const float kOrthoThresholdAngle = 3f;
    private const float kOneOverSqrt2 = 0.7071068f;
    [NonSerialized]
    private ActiveEditorTracker m_Tracker;
    private const double k_MaxDoubleKeypressTime = 0.5;
    private static bool waitingFor2DModeKeyUp;
    [SerializeField]
    private bool m_2DMode;
    internal UnityEngine.Object m_OneClickDragObject;
    private static SceneView s_AudioSceneView;
    public static SceneView.OnSceneFunc onSceneGUIDelegate;
    internal static SceneView.OnSceneFunc onPreSceneGUIDelegate;
    [SerializeField]
    private SceneView.SceneViewState m_SceneViewState;
    [SerializeField]
    private SceneViewGrid grid;
    [SerializeField]
    internal SceneViewRotation svRot;
    [NonSerialized]
    private Camera m_Camera;
    [SerializeField]
    private Quaternion m_LastSceneViewRotation;
    [SerializeField]
    private bool m_LastSceneViewOrtho;
    private bool s_DraggingCursorIsCached;
    private RectSelection m_RectSelection;
    private const float kPerspectiveFov = 90f;
    private static Material s_AlphaOverlayMaterial;
    private static Material s_DeferredOverlayMaterial;
    private static Shader s_ShowOverdrawShader;
    private static Shader s_ShowMipsShader;
    private static Shader s_AuraShader;
    private static Texture2D s_MipColorsTexture;
    private GUIContent m_Lighting;
    private GUIContent m_Fx;
    private GUIContent m_AudioPlayContent;
    private GUIContent m_GizmosContent;
    private GUIContent m_2DModeContent;
    private GUIContent m_RenderDocContent;
    private static Tool s_CurrentTool;
    private RenderTexture m_SceneTargetTexture;
    private int m_MainViewControlID;
    [SerializeField]
    private Shader m_ReplacementShader;
    [SerializeField]
    private string m_ReplacementString;
    private SceneViewOverlay m_SceneViewOverlay;
    private EditorCache m_DragEditorCache;
    private SceneView.DraggingLockedState m_DraggingLockedState;
    [SerializeField]
    private UnityEngine.Object m_LastLockedObject;
    [SerializeField]
    private bool m_ViewIsLockedToObject;
    private static GUIStyle s_DropDownStyle;
    private bool m_RequestedSceneViewFiltering;
    private double m_lastRenderedTime;
    private AlbedoSwatchInfo[] m_AlbedoSwatchInfos;
    private GUIStyle[] m_AlbedoSwatchColorStyles;
    private string[] m_AlbedoSwatchDescriptions;
    private GUIContent[] m_AlbedoSwatchGUIContent;
    private string[] m_AlbedoSwatchLuminanceStrings;

    public SceneView()
    {
      this.m_HierarchyType = HierarchyType.GameObjects;
      this.depthBufferBits = 32;
    }

    public static SceneView lastActiveSceneView
    {
      get
      {
        return SceneView.s_LastActiveSceneView;
      }
    }

    public static SceneView currentDrawingSceneView
    {
      get
      {
        return SceneView.s_CurrentDrawingSceneView;
      }
    }

    public bool in2DMode
    {
      get
      {
        return this.m_2DMode;
      }
      set
      {
        if (this.m_2DMode == value || Tools.viewTool == ViewTool.FPS || Tools.viewTool == ViewTool.Orbit)
          return;
        this.m_2DMode = value;
        this.On2DModeChange();
      }
    }

    public bool isRotationLocked
    {
      get
      {
        return this.m_isRotationLocked;
      }
      set
      {
        if (this.m_isRotationLocked == value)
          return;
        this.m_isRotationLocked = value;
      }
    }

    public DrawCameraMode renderMode
    {
      get
      {
        return this.m_RenderMode;
      }
      set
      {
        this.m_RenderMode = value;
        this.SetupPBRValidation();
      }
    }

    public SceneView.SceneViewState sceneViewState
    {
      get
      {
        return this.m_SceneViewState;
      }
    }

    internal bool showGlobalGrid
    {
      get
      {
        return this.m_ShowGlobalGrid;
      }
      set
      {
        this.m_ShowGlobalGrid = value;
      }
    }

    internal bool drawGlobalGrid
    {
      get
      {
        return AnnotationUtility.showGrid && this.showGlobalGrid;
      }
    }

    public Quaternion lastSceneViewRotation
    {
      get
      {
        if (this.m_LastSceneViewRotation == new Quaternion(0.0f, 0.0f, 0.0f, 0.0f))
          this.m_LastSceneViewRotation = Quaternion.identity;
        return this.m_LastSceneViewRotation;
      }
      set
      {
        this.m_LastSceneViewRotation = value;
      }
    }

    internal static void AddCursorRect(Rect rect, MouseCursor cursor)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      SceneView.s_MouseRects.Add(new SceneView.CursorRect(rect, cursor));
    }

    public float cameraDistance
    {
      get
      {
        float num = this.m_Ortho.Fade(90f, 0.0f);
        if (!this.camera.orthographic)
          return this.size / Mathf.Tan((float) ((double) num * 0.5 * (Math.PI / 180.0)));
        return this.size * 2f;
      }
    }

    public static ArrayList sceneViews
    {
      get
      {
        return SceneView.s_SceneViews;
      }
    }

    public Camera camera
    {
      get
      {
        return this.m_Camera;
      }
    }

    public void SetSceneViewShaderReplace(Shader shader, string replaceString)
    {
      this.m_ReplacementShader = shader;
      this.m_ReplacementString = replaceString;
    }

    internal SceneView.DraggingLockedState draggingLocked
    {
      set
      {
        this.m_DraggingLockedState = value;
      }
      get
      {
        return this.m_DraggingLockedState;
      }
    }

    internal bool viewIsLockedToObject
    {
      get
      {
        return this.m_ViewIsLockedToObject;
      }
      set
      {
        this.m_LastLockedObject = !value ? (UnityEngine.Object) null : Selection.activeObject;
        this.m_ViewIsLockedToObject = value;
        this.draggingLocked = SceneView.DraggingLockedState.LookAt;
      }
    }

    [RequiredByNativeCode]
    public static bool FrameLastActiveSceneView()
    {
      if ((UnityEngine.Object) SceneView.lastActiveSceneView == (UnityEngine.Object) null)
        return false;
      return SceneView.lastActiveSceneView.SendEvent(EditorGUIUtility.CommandEvent("FrameSelected"));
    }

    [RequiredByNativeCode]
    public static bool FrameLastActiveSceneViewWithLock()
    {
      if ((UnityEngine.Object) SceneView.lastActiveSceneView == (UnityEngine.Object) null)
        return false;
      return SceneView.lastActiveSceneView.SendEvent(EditorGUIUtility.CommandEvent("FrameSelectedWithLock"));
    }

    private Editor[] GetActiveEditors()
    {
      if (this.m_Tracker == null)
        this.m_Tracker = ActiveEditorTracker.sharedTracker;
      return this.m_Tracker.activeEditors;
    }

    public static Camera[] GetAllSceneCameras()
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < SceneView.s_SceneViews.Count; ++index)
      {
        Camera camera = ((SceneView) SceneView.s_SceneViews[index]).m_Camera;
        if ((UnityEngine.Object) camera != (UnityEngine.Object) null)
          arrayList.Add((object) camera);
      }
      return (Camera[]) arrayList.ToArray(typeof (Camera));
    }

    public static void RepaintAll()
    {
      IEnumerator enumerator = SceneView.s_SceneViews.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          ((EditorWindow) enumerator.Current).Repaint();
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    internal override void SetSearchFilter(string searchFilter, SearchableEditorWindow.SearchMode mode, bool setAll)
    {
      if (this.m_SearchFilter == "" || searchFilter == "")
        this.m_StartSearchFilterTime = EditorApplication.timeSinceStartup;
      base.SetSearchFilter(searchFilter, mode, setAll);
    }

    internal void OnLostFocus()
    {
      GameView editorWindowOfType = (GameView) WindowLayout.FindEditorWindowOfType(typeof (GameView));
      if ((bool) ((UnityEngine.Object) editorWindowOfType) && (UnityEngine.Object) editorWindowOfType.m_Parent != (UnityEngine.Object) null && ((UnityEngine.Object) this.m_Parent != (UnityEngine.Object) null && (UnityEngine.Object) editorWindowOfType.m_Parent == (UnityEngine.Object) this.m_Parent))
        editorWindowOfType.m_Parent.backgroundValid = false;
      if (!((UnityEngine.Object) SceneView.s_LastActiveSceneView == (UnityEngine.Object) this))
        return;
      SceneViewMotion.ResetMotion();
    }

    public override void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
      this.m_RectSelection = new RectSelection((EditorWindow) this);
      if (this.grid == null)
        this.grid = new SceneViewGrid();
      this.grid.Register(this);
      if (this.svRot == null)
        this.svRot = new SceneViewRotation();
      this.svRot.Register(this);
      this.autoRepaintOnSceneChange = true;
      this.m_Rotation.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_Position.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_Size.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_Ortho.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.wantsMouseMove = true;
      this.wantsMouseEnterLeaveWindow = true;
      this.dontClearBackground = true;
      SceneView.s_SceneViews.Add((object) this);
      this.m_Lighting = EditorGUIUtility.IconContent("SceneviewLighting", "Lighting|When toggled on, the Scene lighting is used. When toggled off, a light attached to the Scene view camera is used.");
      this.m_Fx = EditorGUIUtility.IconContent("SceneviewFx", "Effects|Toggle skybox, fog, and various other effects.");
      this.m_AudioPlayContent = EditorGUIUtility.IconContent("SceneviewAudio", "AudioPlay|Toggle audio on or off.");
      this.m_GizmosContent = EditorGUIUtility.TextContent("Gizmos|Toggle the visibility of different Gizmos in the Scene view.");
      this.m_2DModeContent = new GUIContent("2D", "When togggled on, the Scene is in 2D view. When toggled off, the Scene is in 3D view.");
      this.m_RenderDocContent = EditorGUIUtility.IconContent("renderdoc", "Capture|Capture the current view and open in RenderDoc.");
      this.m_SceneViewOverlay = new SceneViewOverlay(this);
      EditorApplication.playModeStateChanged += new Action<PlayModeStateChange>(this.OnPlayModeStateChanged);
      EditorApplication.CallbackFunction modifierKeysChanged = EditorApplication.modifierKeysChanged;
      // ISSUE: reference to a compiler-generated field
      if (SceneView.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SceneView.\u003C\u003Ef__mg\u0024cache0 = new EditorApplication.CallbackFunction(SceneView.RepaintAll);
      }
      // ISSUE: reference to a compiler-generated field
      EditorApplication.CallbackFunction fMgCache0 = SceneView.\u003C\u003Ef__mg\u0024cache0;
      EditorApplication.modifierKeysChanged = modifierKeysChanged + fMgCache0;
      this.m_DraggingLockedState = SceneView.DraggingLockedState.NotDragging;
      this.CreateSceneCameraAndLights();
      if (this.m_2DMode)
        this.LookAt(this.pivot, Quaternion.identity, this.size, true, true);
      base.OnEnable();
    }

    internal void Awake()
    {
      if (this.sceneViewState == null)
        this.m_SceneViewState = new SceneView.SceneViewState();
      if (!this.m_2DMode && EditorSettings.defaultBehaviorMode != EditorBehaviorMode.Mode2D)
        return;
      this.m_LastSceneViewRotation = Quaternion.LookRotation(new Vector3(-1f, -0.7f, -1f));
      this.m_LastSceneViewOrtho = false;
      this.m_Rotation.value = Quaternion.identity;
      this.m_Ortho.value = true;
      this.m_2DMode = true;
      if (Tools.current == Tool.Move)
        Tools.current = Tool.Rect;
    }

    internal static void PlaceGameObjectInFrontOfSceneView(GameObject go)
    {
      if (SceneView.s_SceneViews.Count < 1)
        return;
      SceneView sceneView = SceneView.s_LastActiveSceneView;
      if (!(bool) ((UnityEngine.Object) sceneView))
        sceneView = SceneView.s_SceneViews[0] as SceneView;
      if ((bool) ((UnityEngine.Object) sceneView))
        sceneView.MoveToView(go.transform);
    }

    internal static Camera GetLastActiveSceneViewCamera()
    {
      SceneView lastActiveSceneView = SceneView.s_LastActiveSceneView;
      return !(bool) ((UnityEngine.Object) lastActiveSceneView) ? (Camera) null : lastActiveSceneView.camera;
    }

    public override void OnDisable()
    {
      EditorApplication.CallbackFunction modifierKeysChanged = EditorApplication.modifierKeysChanged;
      // ISSUE: reference to a compiler-generated field
      if (SceneView.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SceneView.\u003C\u003Ef__mg\u0024cache1 = new EditorApplication.CallbackFunction(SceneView.RepaintAll);
      }
      // ISSUE: reference to a compiler-generated field
      EditorApplication.CallbackFunction fMgCache1 = SceneView.\u003C\u003Ef__mg\u0024cache1;
      EditorApplication.modifierKeysChanged = modifierKeysChanged - fMgCache1;
      EditorApplication.playModeStateChanged -= new Action<PlayModeStateChange>(this.OnPlayModeStateChanged);
      if ((bool) ((UnityEngine.Object) this.m_Camera))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Camera.gameObject, true);
      if ((bool) ((UnityEngine.Object) this.m_Light[0]))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Light[0].gameObject, true);
      if ((bool) ((UnityEngine.Object) this.m_Light[1]))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Light[1].gameObject, true);
      if ((bool) ((UnityEngine.Object) this.m_Light[2]))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Light[2].gameObject, true);
      if ((bool) ((UnityEngine.Object) SceneView.s_MipColorsTexture))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) SceneView.s_MipColorsTexture, true);
      SceneView.s_SceneViews.Remove((object) this);
      if ((UnityEngine.Object) SceneView.s_LastActiveSceneView == (UnityEngine.Object) this)
        SceneView.s_LastActiveSceneView = SceneView.s_SceneViews.Count <= 0 ? (SceneView) null : SceneView.s_SceneViews[0] as SceneView;
      this.CleanupEditorDragFunctions();
      base.OnDisable();
    }

    public void OnDestroy()
    {
      if (!this.m_AudioPlay)
        return;
      this.m_AudioPlay = false;
      this.RefreshAudioPlay();
    }

    internal void OnPlayModeStateChanged(PlayModeStateChange state)
    {
      if (!this.m_AudioPlay)
        return;
      this.m_AudioPlay = false;
      this.RefreshAudioPlay();
    }

    private GUIStyle effectsDropDownStyle
    {
      get
      {
        if (SceneView.s_DropDownStyle == null)
          SceneView.s_DropDownStyle = (GUIStyle) "GV Gizmo DropDown";
        return SceneView.s_DropDownStyle;
      }
    }

    private void DoToolbarGUI()
    {
      GUILayout.BeginHorizontal((GUIStyle) "toolbar", new GUILayoutOption[0]);
      GUIContent guiContent = SceneRenderModeWindow.GetGUIContent(this.m_RenderMode);
      guiContent.tooltip = LocalizationDatabase.GetLocalizedString("The Draw Mode used to display the Scene.");
      if (EditorGUI.DropdownButton(GUILayoutUtility.GetRect(guiContent, EditorStyles.toolbarDropDown, new GUILayoutOption[1]{ GUILayout.Width(120f) }), guiContent, FocusType.Passive, EditorStyles.toolbarDropDown))
      {
        PopupWindow.Show(GUILayoutUtility.topLevel.GetLast(), (PopupWindowContent) new SceneRenderModeWindow(this));
        GUIUtility.ExitGUI();
      }
      EditorGUILayout.Space();
      this.in2DMode = GUILayout.Toggle(this.in2DMode, this.m_2DModeContent, (GUIStyle) "toolbarbutton", new GUILayoutOption[0]);
      EditorGUILayout.Space();
      this.m_SceneLighting = GUILayout.Toggle(this.m_SceneLighting, this.m_Lighting, (GUIStyle) "toolbarbutton", new GUILayoutOption[0]);
      if (this.renderMode == DrawCameraMode.ShadowCascades)
        this.m_SceneLighting = true;
      GUI.enabled = !Application.isPlaying;
      GUI.changed = false;
      this.m_AudioPlay = GUILayout.Toggle(this.m_AudioPlay, this.m_AudioPlayContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (GUI.changed)
        this.RefreshAudioPlay();
      GUI.enabled = true;
      Rect rect = GUILayoutUtility.GetRect(this.m_Fx, this.effectsDropDownStyle);
      if (EditorGUI.DropdownButton(new Rect(rect.xMax - (float) this.effectsDropDownStyle.border.right, rect.y, (float) this.effectsDropDownStyle.border.right, rect.height), GUIContent.none, FocusType.Passive, GUIStyle.none))
      {
        PopupWindow.Show(GUILayoutUtility.topLevel.GetLast(), (PopupWindowContent) new SceneFXWindow(this));
        GUIUtility.ExitGUI();
      }
      bool flag = GUI.Toggle(rect, this.sceneViewState.IsAllOn(), this.m_Fx, this.effectsDropDownStyle);
      if (flag != this.sceneViewState.IsAllOn())
        this.sceneViewState.Toggle(flag);
      EditorGUILayout.Space();
      GUILayout.FlexibleSpace();
      if (this.m_MainViewControlID != GUIUtility.keyboardControl && Event.current.type == EventType.KeyDown && !string.IsNullOrEmpty(this.m_SearchFilter))
      {
        switch (Event.current.keyCode)
        {
          case KeyCode.UpArrow:
          case KeyCode.DownArrow:
            if (Event.current.keyCode == KeyCode.UpArrow)
              this.SelectPreviousSearchResult();
            else
              this.SelectNextSearchResult();
            this.FrameSelected(false);
            Event.current.Use();
            GUIUtility.ExitGUI();
            return;
        }
      }
      if (RenderDoc.IsLoaded())
      {
        using (new EditorGUI.DisabledScope(!RenderDoc.IsSupported()))
        {
          if (GUILayout.Button(this.m_RenderDocContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
          {
            this.m_Parent.CaptureRenderDoc();
            GUIUtility.ExitGUI();
          }
        }
      }
      if (EditorGUI.DropdownButton(GUILayoutUtility.GetRect(this.m_GizmosContent, EditorStyles.toolbarDropDown), this.m_GizmosContent, FocusType.Passive, EditorStyles.toolbarDropDown) && AnnotationWindow.ShowAtPosition(GUILayoutUtility.topLevel.GetLast(), false))
        GUIUtility.ExitGUI();
      GUILayout.Space(6f);
      this.SearchFieldGUI(EditorGUILayout.kLabelFloatMaxW);
      GUILayout.EndHorizontal();
    }

    private void RefreshAudioPlay()
    {
      if ((UnityEngine.Object) SceneView.s_AudioSceneView != (UnityEngine.Object) null && (UnityEngine.Object) SceneView.s_AudioSceneView != (UnityEngine.Object) this && SceneView.s_AudioSceneView.m_AudioPlay)
      {
        SceneView.s_AudioSceneView.m_AudioPlay = false;
        SceneView.s_AudioSceneView.Repaint();
      }
      foreach (AudioSource audioSource in (AudioSource[]) UnityEngine.Object.FindObjectsOfType(typeof (AudioSource)))
      {
        if (audioSource.playOnAwake)
        {
          if (!this.m_AudioPlay)
            audioSource.Stop();
          else if (!audioSource.isPlaying)
            audioSource.Play();
        }
      }
      AudioUtil.SetListenerTransform(!this.m_AudioPlay ? (Transform) null : this.m_Camera.transform);
      SceneView.s_AudioSceneView = this;
    }

    public void OnSelectionChange()
    {
      if (Selection.activeObject != (UnityEngine.Object) null && this.m_LastLockedObject != Selection.activeObject)
        this.viewIsLockedToObject = false;
      this.Repaint();
    }

    private void LoadRenderDoc()
    {
      if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        return;
      RenderDoc.Load();
      ShaderUtil.RecreateGfxDevice();
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      if (!RenderDoc.IsInstalled() || RenderDoc.IsLoaded())
        return;
      menu.AddItem(new GUIContent("Load RenderDoc"), false, new GenericMenu.MenuFunction(this.LoadRenderDoc));
    }

    [MenuItem("GameObject/Set as first sibling %=")]
    internal static void MenuMoveToFront()
    {
      foreach (Transform transform in Selection.transforms)
      {
        Undo.SetTransformParent(transform, transform.parent, "Set as first sibling");
        transform.SetAsFirstSibling();
      }
    }

    [MenuItem("GameObject/Set as first sibling %=", true)]
    internal static bool ValidateMenuMoveToFront()
    {
      if (!((UnityEngine.Object) Selection.activeTransform != (UnityEngine.Object) null))
        return false;
      Transform parent = Selection.activeTransform.parent;
      return (UnityEngine.Object) parent != (UnityEngine.Object) null && (UnityEngine.Object) parent.GetChild(0) != (UnityEngine.Object) Selection.activeTransform;
    }

    [MenuItem("GameObject/Set as last sibling %-")]
    internal static void MenuMoveToBack()
    {
      foreach (Transform transform in Selection.transforms)
      {
        Undo.SetTransformParent(transform, transform.parent, "Set as last sibling");
        transform.SetAsLastSibling();
      }
    }

    [MenuItem("GameObject/Set as last sibling %-", true)]
    internal static bool ValidateMenuMoveToBack()
    {
      if (!((UnityEngine.Object) Selection.activeTransform != (UnityEngine.Object) null))
        return false;
      Transform parent = Selection.activeTransform.parent;
      return (UnityEngine.Object) parent != (UnityEngine.Object) null && (UnityEngine.Object) parent.GetChild(parent.childCount - 1) != (UnityEngine.Object) Selection.activeTransform;
    }

    [MenuItem("GameObject/Move To View %&f")]
    internal static void MenuMoveToView()
    {
      if (!SceneView.ValidateMoveToView())
        return;
      SceneView.s_LastActiveSceneView.MoveToView();
    }

    [MenuItem("GameObject/Move To View %&f", true)]
    private static bool ValidateMoveToView()
    {
      return (UnityEngine.Object) SceneView.s_LastActiveSceneView != (UnityEngine.Object) null && Selection.transforms.Length != 0;
    }

    [MenuItem("GameObject/Align With View %#f")]
    internal static void MenuAlignWithView()
    {
      if (!SceneView.ValidateAlignWithView())
        return;
      SceneView.s_LastActiveSceneView.AlignWithView();
    }

    [MenuItem("GameObject/Align With View %#f", true)]
    internal static bool ValidateAlignWithView()
    {
      return (UnityEngine.Object) SceneView.s_LastActiveSceneView != (UnityEngine.Object) null && (UnityEngine.Object) Selection.activeTransform != (UnityEngine.Object) null;
    }

    [MenuItem("GameObject/Align View to Selected")]
    internal static void MenuAlignViewToSelected()
    {
      if (!SceneView.ValidateAlignViewToSelected())
        return;
      SceneView.s_LastActiveSceneView.AlignViewToObject(Selection.activeTransform);
    }

    [MenuItem("GameObject/Align View to Selected", true)]
    internal static bool ValidateAlignViewToSelected()
    {
      return (UnityEngine.Object) SceneView.s_LastActiveSceneView != (UnityEngine.Object) null && (UnityEngine.Object) Selection.activeTransform != (UnityEngine.Object) null;
    }

    [MenuItem("GameObject/Toggle Active State &#a")]
    internal static void ActivateSelection()
    {
      if (!((UnityEngine.Object) Selection.activeTransform != (UnityEngine.Object) null))
        return;
      GameObject[] gameObjects = Selection.gameObjects;
      Undo.RecordObjects((UnityEngine.Object[]) gameObjects, "Toggle Active State");
      bool flag = !Selection.activeGameObject.activeSelf;
      foreach (GameObject gameObject in gameObjects)
        gameObject.SetActive(flag);
    }

    [MenuItem("GameObject/Toggle Active State &#a", true)]
    internal static bool ValidateActivateSelection()
    {
      return (UnityEngine.Object) Selection.activeTransform != (UnityEngine.Object) null;
    }

    private static void CreateMipColorsTexture()
    {
      if ((bool) ((UnityEngine.Object) SceneView.s_MipColorsTexture))
        return;
      SceneView.s_MipColorsTexture = new Texture2D(32, 32, TextureFormat.RGBA32, true);
      SceneView.s_MipColorsTexture.hideFlags = HideFlags.HideAndDontSave;
      Color[] colorArray = new Color[6]{ new Color(0.0f, 0.0f, 1f, 0.8f), new Color(0.0f, 0.5f, 1f, 0.4f), new Color(1f, 1f, 1f, 0.0f), new Color(1f, 0.7f, 0.0f, 0.2f), new Color(1f, 0.3f, 0.0f, 0.6f), new Color(1f, 0.0f, 0.0f, 0.8f) };
      int num = Mathf.Min(6, SceneView.s_MipColorsTexture.mipmapCount);
      for (int miplevel = 0; miplevel < num; ++miplevel)
      {
        Color[] colors = new Color[Mathf.Max(SceneView.s_MipColorsTexture.width >> miplevel, 1) * Mathf.Max(SceneView.s_MipColorsTexture.height >> miplevel, 1)];
        for (int index = 0; index < colors.Length; ++index)
          colors[index] = colorArray[miplevel];
        SceneView.s_MipColorsTexture.SetPixels(colors, miplevel);
      }
      SceneView.s_MipColorsTexture.filterMode = UnityEngine.FilterMode.Trilinear;
      SceneView.s_MipColorsTexture.Apply(false);
      Shader.SetGlobalTexture("_SceneViewMipcolorsTexture", (Texture) SceneView.s_MipColorsTexture);
    }

    public void SetSceneViewFiltering(bool enable)
    {
      this.m_RequestedSceneViewFiltering = enable;
    }

    private bool UseSceneFiltering()
    {
      return !string.IsNullOrEmpty(this.m_SearchFilter) || this.m_RequestedSceneViewFiltering;
    }

    internal bool SceneViewIsRenderingHDR()
    {
      return (UnityEngine.Object) this.m_Camera != (UnityEngine.Object) null && this.m_Camera.allowHDR;
    }

    private void HandleClickAndDragToFocus()
    {
      Event current = Event.current;
      if (current.type == EventType.MouseDown || current.type == EventType.MouseDrag)
        SceneView.s_LastActiveSceneView = this;
      else if ((UnityEngine.Object) SceneView.s_LastActiveSceneView == (UnityEngine.Object) null)
        SceneView.s_LastActiveSceneView = this;
      if (current.type == EventType.MouseDrag)
        this.draggingLocked = SceneView.DraggingLockedState.Dragging;
      else if (GUIUtility.hotControl == 0 && this.draggingLocked == SceneView.DraggingLockedState.Dragging)
        this.draggingLocked = SceneView.DraggingLockedState.LookAt;
      if (current.type == EventType.MouseDown)
      {
        Tools.s_ButtonDown = current.button;
        if (current.button != 1 || Application.platform != RuntimePlatform.OSXEditor)
          return;
        this.Focus();
      }
      else
      {
        if (current.type != EventType.MouseUp || Tools.s_ButtonDown != current.button)
          return;
        Tools.s_ButtonDown = -1;
      }
    }

    private void SetupFogAndShadowDistance(out bool oldFog, out float oldShadowDistance)
    {
      oldFog = RenderSettings.fog;
      oldShadowDistance = QualitySettings.shadowDistance;
      if (Event.current.type != EventType.Repaint)
        return;
      if (!this.sceneViewState.showFog)
        Unsupported.SetRenderSettingsUseFogNoDirty(false);
      if (this.m_Camera.orthographic)
        Unsupported.SetQualitySettingsShadowDistanceTemporarily(QualitySettings.shadowDistance + 0.5f * this.cameraDistance);
    }

    private void RestoreFogAndShadowDistance(bool oldFog, float oldShadowDistance)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Unsupported.SetRenderSettingsUseFogNoDirty(oldFog);
      Unsupported.SetQualitySettingsShadowDistanceTemporarily(oldShadowDistance);
    }

    private void CreateCameraTargetTexture(Rect cameraRect, bool hdr)
    {
      bool flag1 = QualitySettings.activeColorSpace == ColorSpace.Linear;
      int num = Mathf.Max(1, QualitySettings.antiAliasing);
      if (this.IsSceneCameraDeferred())
        num = 1;
      if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal)
        num = 1;
      RenderTextureFormat format = !hdr || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) ? RenderTextureFormat.ARGB32 : RenderTextureFormat.ARGBHalf;
      if ((UnityEngine.Object) this.m_SceneTargetTexture != (UnityEngine.Object) null)
      {
        bool flag2 = (UnityEngine.Object) this.m_SceneTargetTexture != (UnityEngine.Object) null && flag1 == this.m_SceneTargetTexture.sRGB;
        if (RenderTextureEditor.IsHDRFormat(format))
          flag2 = true;
        if (this.m_SceneTargetTexture.format != format || this.m_SceneTargetTexture.antiAliasing != num || !flag2)
        {
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_SceneTargetTexture);
          this.m_SceneTargetTexture = (RenderTexture) null;
        }
      }
      Rect cameraRect1 = Handles.GetCameraRect(cameraRect);
      int width = (int) cameraRect1.width;
      int height = (int) cameraRect1.height;
      if ((UnityEngine.Object) this.m_SceneTargetTexture == (UnityEngine.Object) null)
      {
        this.m_SceneTargetTexture = new RenderTexture(0, 0, 24, format);
        this.m_SceneTargetTexture.name = "SceneView RT";
        this.m_SceneTargetTexture.antiAliasing = num;
        this.m_SceneTargetTexture.hideFlags = HideFlags.HideAndDontSave;
      }
      if (this.m_SceneTargetTexture.width != width || this.m_SceneTargetTexture.height != height)
      {
        this.m_SceneTargetTexture.Release();
        this.m_SceneTargetTexture.width = width;
        this.m_SceneTargetTexture.height = height;
      }
      this.m_SceneTargetTexture.Create();
      EditorGUIUtility.SetGUITextureBlitColorspaceSettings(EditorGUIUtility.GUITextureBlitColorspaceMaterial);
    }

    internal bool IsCameraDrawModeEnabled(DrawCameraMode mode)
    {
      return Handles.IsCameraDrawModeEnabled(this.m_Camera, mode);
    }

    internal bool IsSceneCameraDeferred()
    {
      return !((UnityEngine.Object) this.m_Camera == (UnityEngine.Object) null) && (this.m_Camera.actualRenderingPath == RenderingPath.DeferredLighting || this.m_Camera.actualRenderingPath == RenderingPath.DeferredShading);
    }

    internal static bool DoesCameraDrawModeSupportDeferred(DrawCameraMode mode)
    {
      return mode == DrawCameraMode.Normal || mode == DrawCameraMode.Textured || (mode == DrawCameraMode.TexturedWire || mode == DrawCameraMode.ShadowCascades) || (mode == DrawCameraMode.RenderPaths || mode == DrawCameraMode.AlphaChannel || (mode == DrawCameraMode.DeferredDiffuse || mode == DrawCameraMode.DeferredSpecular)) || (mode == DrawCameraMode.DeferredSmoothness || mode == DrawCameraMode.DeferredNormal || (mode == DrawCameraMode.RealtimeCharting || mode == DrawCameraMode.Systems) || (mode == DrawCameraMode.Clustering || mode == DrawCameraMode.LitClustering || (mode == DrawCameraMode.RealtimeAlbedo || mode == DrawCameraMode.RealtimeEmissive))) || (mode == DrawCameraMode.RealtimeIndirect || mode == DrawCameraMode.RealtimeDirectionality || (mode == DrawCameraMode.BakedLightmap || mode == DrawCameraMode.ValidateAlbedo)) || mode == DrawCameraMode.ValidateMetalSpecular;
    }

    internal static bool DoesCameraDrawModeSupportHDR(DrawCameraMode mode)
    {
      return mode == DrawCameraMode.Textured || mode == DrawCameraMode.TexturedWire;
    }

    private void PrepareCameraTargetTexture(Rect cameraRect)
    {
      bool hdr = this.SceneViewIsRenderingHDR();
      this.CreateCameraTargetTexture(cameraRect, hdr);
      this.m_Camera.targetTexture = this.m_SceneTargetTexture;
      if (!this.UseSceneFiltering() && SceneView.DoesCameraDrawModeSupportDeferred(this.m_RenderMode) || !this.IsSceneCameraDeferred())
        return;
      this.m_Camera.renderingPath = RenderingPath.Forward;
    }

    private void PrepareCameraReplacementShader()
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Handles.SetSceneViewColors((Color) SceneView.kSceneViewWire, (Color) SceneView.kSceneViewWireOverlay, (Color) SceneView.kSceneViewSelectedOutline, (Color) SceneView.kSceneViewSelectedWire);
      if (this.m_RenderMode == DrawCameraMode.Overdraw)
      {
        if (!(bool) ((UnityEngine.Object) SceneView.s_ShowOverdrawShader))
          SceneView.s_ShowOverdrawShader = EditorGUIUtility.LoadRequired("SceneView/SceneViewShowOverdraw.shader") as Shader;
        this.m_Camera.SetReplacementShader(SceneView.s_ShowOverdrawShader, "RenderType");
      }
      else if (this.m_RenderMode == DrawCameraMode.Mipmaps)
      {
        if (!(bool) ((UnityEngine.Object) SceneView.s_ShowMipsShader))
          SceneView.s_ShowMipsShader = EditorGUIUtility.LoadRequired("SceneView/SceneViewShowMips.shader") as Shader;
        if ((UnityEngine.Object) SceneView.s_ShowMipsShader != (UnityEngine.Object) null && SceneView.s_ShowMipsShader.isSupported)
        {
          SceneView.CreateMipColorsTexture();
          this.m_Camera.SetReplacementShader(SceneView.s_ShowMipsShader, "RenderType");
        }
        else
          this.m_Camera.SetReplacementShader(this.m_ReplacementShader, this.m_ReplacementString);
      }
      else
        this.m_Camera.SetReplacementShader(this.m_ReplacementShader, this.m_ReplacementString);
    }

    private bool SceneCameraRendersIntoRT()
    {
      return (UnityEngine.Object) this.m_Camera.targetTexture != (UnityEngine.Object) null;
    }

    private void DoDrawCamera(Rect cameraRect, out bool pushedGUIClip)
    {
      pushedGUIClip = false;
      if (!this.m_Camera.gameObject.activeInHierarchy)
        return;
      DrawGridParameters gridParam = this.grid.PrepareGridRender(this.camera, this.pivot, this.m_Rotation.target, this.m_Size.value, this.m_Ortho.target, this.drawGlobalGrid);
      Event current = Event.current;
      if (this.UseSceneFiltering())
      {
        if (current.type == EventType.Repaint)
        {
          Handles.EnableCameraFx(this.m_Camera, true);
          Handles.SetCameraFilterMode(this.m_Camera, Handles.FilterMode.ShowRest);
          float fade = Mathf.Clamp01((float) (EditorApplication.timeSinceStartup - this.m_StartSearchFilterTime));
          Handles.DrawCamera(cameraRect, this.m_Camera, this.m_RenderMode);
          Handles.DrawCameraFade(this.m_Camera, fade);
          Handles.EnableCameraFx(this.m_Camera, false);
          Handles.SetCameraFilterMode(this.m_Camera, Handles.FilterMode.ShowFiltered);
          if (!(bool) ((UnityEngine.Object) SceneView.s_AuraShader))
            SceneView.s_AuraShader = EditorGUIUtility.LoadRequired("SceneView/SceneViewAura.shader") as Shader;
          this.m_Camera.SetReplacementShader(SceneView.s_AuraShader, "");
          Handles.DrawCamera(cameraRect, this.m_Camera, this.m_RenderMode);
          this.m_Camera.SetReplacementShader(this.m_ReplacementShader, this.m_ReplacementString);
          Handles.DrawCamera(cameraRect, this.m_Camera, this.m_RenderMode, gridParam);
          if ((double) fade < 1.0)
            this.Repaint();
        }
        Rect screenRect = cameraRect;
        if (current.type == EventType.Repaint)
          RenderTexture.active = (RenderTexture) null;
        GUI.EndGroup();
        GUI.BeginGroup(new Rect(0.0f, 17f, this.position.width, this.position.height - 17f));
        if (current.type == EventType.Repaint)
        {
          GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
          Graphics.DrawTexture(screenRect, (Texture) this.m_SceneTargetTexture, new Rect(0.0f, 0.0f, 1f, 1f), 0, 0, 0, 0, GUI.color, EditorGUIUtility.GUITextureBlitColorspaceMaterial);
          GL.sRGBWrite = false;
        }
        Handles.SetCamera(cameraRect, this.m_Camera);
        this.HandleSelectionAndOnSceneGUI();
      }
      else
      {
        if (this.SceneCameraRendersIntoRT())
        {
          GUIClip.Push(new Rect(0.0f, 0.0f, this.position.width, this.position.height), Vector2.zero, Vector2.zero, true);
          pushedGUIClip = true;
        }
        Handles.DrawCameraStep1(cameraRect, this.m_Camera, this.m_RenderMode, gridParam);
        this.DrawRenderModeOverlay(cameraRect);
      }
    }

    private void SetupPBRValidation()
    {
      if (this.m_RenderMode == DrawCameraMode.ValidateAlbedo)
      {
        this.CreateAlbedoSwatchData();
        this.UpdateAlbedoSwatch();
      }
      if ((this.m_RenderMode == DrawCameraMode.ValidateAlbedo || this.m_RenderMode == DrawCameraMode.ValidateMetalSpecular) && (this.lastRenderMode != DrawCameraMode.ValidateAlbedo && this.lastRenderMode != DrawCameraMode.ValidateMetalSpecular))
        SceneView.onSceneGUIDelegate += new SceneView.OnSceneFunc(this.DrawValidateAlbedoSwatches);
      else if (this.m_RenderMode != DrawCameraMode.ValidateAlbedo && this.m_RenderMode != DrawCameraMode.ValidateMetalSpecular && (this.lastRenderMode == DrawCameraMode.ValidateAlbedo || this.lastRenderMode == DrawCameraMode.ValidateMetalSpecular))
        SceneView.onSceneGUIDelegate -= new SceneView.OnSceneFunc(this.DrawValidateAlbedoSwatches);
      this.lastRenderMode = this.m_RenderMode;
    }

    private void DoClearCamera(Rect cameraRect)
    {
      float verticalFov = this.GetVerticalFOV(90f);
      float fieldOfView = this.m_Camera.fieldOfView;
      this.m_Camera.fieldOfView = verticalFov;
      Handles.ClearCamera(cameraRect, this.m_Camera);
      this.m_Camera.fieldOfView = fieldOfView;
    }

    private void SetupCustomSceneLighting()
    {
      if (this.m_SceneLighting)
        return;
      this.m_Light[0].transform.rotation = this.m_Camera.transform.rotation;
      if (Event.current.type != EventType.Repaint)
        return;
      InternalEditorUtility.SetCustomLighting(this.m_Light, SceneView.kSceneViewMidLight);
    }

    private void CleanupCustomSceneLighting()
    {
      if (this.m_SceneLighting || Event.current.type != EventType.Repaint)
        return;
      InternalEditorUtility.RemoveCustomLighting();
    }

    private void HandleViewToolCursor()
    {
      if (!Tools.viewToolActive || Event.current.type != EventType.Repaint)
        return;
      MouseCursor cursor = MouseCursor.Arrow;
      switch (Tools.viewTool)
      {
        case ViewTool.Orbit:
          cursor = MouseCursor.Orbit;
          break;
        case ViewTool.Pan:
          cursor = MouseCursor.Pan;
          break;
        case ViewTool.Zoom:
          cursor = MouseCursor.Zoom;
          break;
        case ViewTool.FPS:
          cursor = MouseCursor.FPS;
          break;
      }
      if (cursor == MouseCursor.Arrow)
        return;
      SceneView.AddCursorRect(new Rect(0.0f, 17f, this.position.width, this.position.height - 17f), cursor);
    }

    private static bool ComponentHasImageEffectAttribute(Component c)
    {
      if ((UnityEngine.Object) c == (UnityEngine.Object) null)
        return false;
      return Attribute.IsDefined((MemberInfo) c.GetType(), typeof (ImageEffectAllowedInSceneView));
    }

    private void UpdateImageEffects(bool enable)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Camera mainCamera = SceneView.GetMainCamera();
      if (!enable || (UnityEngine.Object) mainCamera == (UnityEngine.Object) null)
      {
        GameObject gameObject = this.m_Camera.gameObject;
        // ISSUE: reference to a compiler-generated field
        if (SceneView.\u003C\u003Ef__mg\u0024cache2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SceneView.\u003C\u003Ef__mg\u0024cache2 = new ComponentUtility.IsDesiredComponent(SceneView.ComponentHasImageEffectAttribute);
        }
        // ISSUE: reference to a compiler-generated field
        ComponentUtility.IsDesiredComponent fMgCache2 = SceneView.\u003C\u003Ef__mg\u0024cache2;
        ComponentUtility.DestroyComponentsMatching(gameObject, fMgCache2);
      }
      else
      {
        GameObject gameObject1 = mainCamera.gameObject;
        GameObject gameObject2 = this.m_Camera.gameObject;
        // ISSUE: reference to a compiler-generated field
        if (SceneView.\u003C\u003Ef__mg\u0024cache3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SceneView.\u003C\u003Ef__mg\u0024cache3 = new ComponentUtility.IsDesiredComponent(SceneView.ComponentHasImageEffectAttribute);
        }
        // ISSUE: reference to a compiler-generated field
        ComponentUtility.IsDesiredComponent fMgCache3 = SceneView.\u003C\u003Ef__mg\u0024cache3;
        ComponentUtility.ReplaceComponentsIfDifferent(gameObject1, gameObject2, fMgCache3);
      }
    }

    private void DoOnPreSceneGUICallbacks(Rect cameraRect)
    {
      if (this.UseSceneFiltering())
        return;
      Handles.SetCamera(cameraRect, this.m_Camera);
      this.CallOnPreSceneGUI();
    }

    private GUIStyle CreateSwatchStyleForColor(Color c)
    {
      Texture2D texture2D = new Texture2D(1, 1);
      if (PlayerSettings.colorSpace == ColorSpace.Linear)
        c = c.gamma;
      texture2D.SetPixel(0, 0, c);
      texture2D.Apply();
      return new GUIStyle() { normal = { background = texture2D } };
    }

    private string CreateSwatchDescriptionForName(float minLum, float maxLum)
    {
      return "Luminance (" + minLum.ToString("F2") + " - " + maxLum.ToString("F2") + ")";
    }

    private void CreateAlbedoSwatchData()
    {
      AlbedoSwatchInfo[] albedoSwatches = EditorGraphicsSettings.albedoSwatches;
      if (albedoSwatches.Length != 0)
        this.m_AlbedoSwatchInfos = albedoSwatches;
      else
        this.m_AlbedoSwatchInfos = new AlbedoSwatchInfo[13]
        {
          new AlbedoSwatchInfo()
          {
            name = "Black Acrylic Paint",
            color = new Color(0.2196078f, 0.2196078f, 0.2196078f),
            minLuminance = 0.03f,
            maxLuminance = 0.07f
          },
          new AlbedoSwatchInfo()
          {
            name = "Dark Soil",
            color = new Color(0.3333333f, 0.2392157f, 0.1921569f),
            minLuminance = 0.05f,
            maxLuminance = 0.14f
          },
          new AlbedoSwatchInfo()
          {
            name = "Worn Asphalt",
            color = new Color(0.3568628f, 0.3568628f, 0.3568628f),
            minLuminance = 0.1f,
            maxLuminance = 0.15f
          },
          new AlbedoSwatchInfo()
          {
            name = "Dry Clay Soil",
            color = new Color(0.5372549f, 0.4705882f, 0.4f),
            minLuminance = 0.15f,
            maxLuminance = 0.35f
          },
          new AlbedoSwatchInfo()
          {
            name = "Green Grass",
            color = new Color(0.4823529f, 0.5137255f, 0.2901961f),
            minLuminance = 0.16f,
            maxLuminance = 0.26f
          },
          new AlbedoSwatchInfo()
          {
            name = "Old Concrete",
            color = new Color(0.5294118f, 0.5333334f, 0.5137255f),
            minLuminance = 0.17f,
            maxLuminance = 0.3f
          },
          new AlbedoSwatchInfo()
          {
            name = "Red Clay Tile",
            color = new Color(0.772549f, 0.4901961f, 0.3921569f),
            minLuminance = 0.23f,
            maxLuminance = 0.33f
          },
          new AlbedoSwatchInfo()
          {
            name = "Dry Sand",
            color = new Color(0.6941177f, 0.654902f, 0.5176471f),
            minLuminance = 0.2f,
            maxLuminance = 0.45f
          },
          new AlbedoSwatchInfo()
          {
            name = "New Concrete",
            color = new Color(0.7254902f, 0.7137255f, 0.6862745f),
            minLuminance = 0.32f,
            maxLuminance = 0.55f
          },
          new AlbedoSwatchInfo()
          {
            name = "White Acrylic Paint",
            color = new Color(0.8901961f, 0.8901961f, 0.8901961f),
            minLuminance = 0.75f,
            maxLuminance = 0.85f
          },
          new AlbedoSwatchInfo()
          {
            name = "Fresh Snow",
            color = new Color(0.9529412f, 0.9529412f, 0.9529412f),
            minLuminance = 0.85f,
            maxLuminance = 0.95f
          },
          new AlbedoSwatchInfo()
          {
            name = "Blue Sky",
            color = new Color(0.3647059f, 0.4823529f, 0.6156863f),
            minLuminance = new Color(0.3647059f, 0.4823529f, 0.6156863f).linear.maxColorComponent - 0.05f,
            maxLuminance = new Color(0.3647059f, 0.4823529f, 0.6156863f).linear.maxColorComponent + 0.05f
          },
          new AlbedoSwatchInfo()
          {
            name = "Foliage",
            color = new Color(0.3568628f, 0.4235294f, 0.254902f),
            minLuminance = new Color(0.3568628f, 0.4235294f, 0.254902f).linear.maxColorComponent - 0.05f,
            maxLuminance = new Color(0.3568628f, 0.4235294f, 0.254902f).linear.maxColorComponent + 0.05f
          }
        };
      this.UpdateAlbedoSwatchGUI();
    }

    private void UpdateAlbedoSwatchGUI()
    {
      this.m_LastKnownColorSpace = PlayerSettings.colorSpace;
      this.m_AlbedoSwatchColorStyles = new GUIStyle[this.m_AlbedoSwatchInfos.Length + 1];
      this.m_AlbedoSwatchGUIContent = new GUIContent[this.m_AlbedoSwatchInfos.Length + 1];
      this.m_AlbedoSwatchDescriptions = new string[this.m_AlbedoSwatchInfos.Length + 1];
      this.m_AlbedoSwatchLuminanceStrings = new string[this.m_AlbedoSwatchInfos.Length + 1];
      this.m_AlbedoSwatchColorStyles[0] = this.CreateSwatchStyleForColor(Color.gray);
      this.m_AlbedoSwatchDescriptions[0] = "Default Luminance";
      this.m_AlbedoSwatchGUIContent[0] = new GUIContent(this.m_AlbedoSwatchDescriptions[0]);
      this.m_AlbedoSwatchLuminanceStrings[0] = this.CreateSwatchDescriptionForName(0.012f, 0.9f);
      for (int index = 1; index < this.m_AlbedoSwatchInfos.Length + 1; ++index)
      {
        this.m_AlbedoSwatchColorStyles[index] = this.CreateSwatchStyleForColor(this.m_AlbedoSwatchInfos[index - 1].color);
        this.m_AlbedoSwatchDescriptions[index] = this.m_AlbedoSwatchInfos[index - 1].name;
        this.m_AlbedoSwatchGUIContent[index] = new GUIContent(this.m_AlbedoSwatchDescriptions[index]);
        this.m_AlbedoSwatchLuminanceStrings[index] = this.CreateSwatchDescriptionForName(this.m_AlbedoSwatchInfos[index - 1].minLuminance, this.m_AlbedoSwatchInfos[index - 1].maxLuminance);
      }
    }

    private void UpdatePBRColorLegend()
    {
      this.m_TooLowColorStyle = this.CreateSwatchStyleForColor(SceneView.kSceneViewMaterialValidateLow.Color);
      this.m_TooHighColorStyle = this.CreateSwatchStyleForColor(SceneView.kSceneViewMaterialValidateHigh.Color);
      this.m_PureMetalColorStyle = this.CreateSwatchStyleForColor(SceneView.kSceneViewMaterialValidatePureMetal.Color);
      Shader.SetGlobalColor("unity_MaterialValidateLowColor", SceneView.kSceneViewMaterialValidateLow.Color.linear);
      Shader.SetGlobalColor("unity_MaterialValidateHighColor", SceneView.kSceneViewMaterialValidateHigh.Color.linear);
      Shader.SetGlobalColor("unity_MaterialValidatePureMetalColor", SceneView.kSceneViewMaterialValidatePureMetal.Color.linear);
    }

    private void UpdateAlbedoSwatch()
    {
      Color color = Color.gray;
      if (this.m_SelectedAlbedoSwatchIndex != 0)
      {
        color = this.m_AlbedoSwatchInfos[this.m_SelectedAlbedoSwatchIndex - 1].color;
        Shader.SetGlobalFloat("_AlbedoMinLuminance", this.m_AlbedoSwatchInfos[this.m_SelectedAlbedoSwatchIndex - 1].minLuminance);
        Shader.SetGlobalFloat("_AlbedoMaxLuminance", this.m_AlbedoSwatchInfos[this.m_SelectedAlbedoSwatchIndex - 1].maxLuminance);
        Shader.SetGlobalFloat("_AlbedoHueTolerance", this.m_AlbedoSwatchHueTolerance);
        Shader.SetGlobalFloat("_AlbedoSaturationTolerance", this.m_AlbedoSwatchSaturationTolerance);
      }
      Shader.SetGlobalColor("_AlbedoCompareColor", color.linear);
      Shader.SetGlobalInt("_CheckAlbedo", this.m_SelectedAlbedoSwatchIndex == 0 ? 0 : 1);
      Shader.SetGlobalInt("_CheckPureMetal", !this.m_ValidateTrueMetals ? 0 : 1);
    }

    internal void DrawTrueMetalCheckbox()
    {
      EditorGUI.BeginChangeCheck();
      this.m_ValidateTrueMetals = EditorGUILayout.ToggleLeft(new GUIContent("Check Pure Metals", "Check if albedo is black for materials with an average specular color above 0.45"), this.m_ValidateTrueMetals);
      if (!EditorGUI.EndChangeCheck())
        return;
      Shader.SetGlobalInt("_CheckPureMetal", !this.m_ValidateTrueMetals ? 0 : 1);
    }

    internal void DrawPBRSettingsForScene()
    {
      if (this.m_RenderMode == DrawCameraMode.ValidateAlbedo)
      {
        if (PlayerSettings.colorSpace == ColorSpace.Gamma)
          EditorGUILayout.HelpBox("Albedo Validation doesn't work when Color Space is set to gamma space", MessageType.Warning);
        EditorGUIUtility.labelWidth = 140f;
        this.m_SelectedAlbedoSwatchIndex = EditorGUILayout.Popup(new GUIContent("Luminance Validation:", "Select default luminance validation or validate against a configured albedo swatch"), this.m_SelectedAlbedoSwatchIndex, this.m_AlbedoSwatchGUIContent, new GUILayoutOption[0]);
        ++EditorGUI.indentLevel;
        using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
        {
          EditorGUIUtility.labelWidth = 5f;
          EditorGUILayout.LabelField(" ", this.m_AlbedoSwatchColorStyles[this.m_SelectedAlbedoSwatchIndex], new GUILayoutOption[0]);
          EditorGUIUtility.labelWidth = 140f;
          EditorGUILayout.LabelField(this.m_AlbedoSwatchLuminanceStrings[this.m_SelectedAlbedoSwatchIndex]);
        }
        this.UpdateAlbedoSwatch();
        --EditorGUI.indentLevel;
        using (new EditorGUI.DisabledScope(this.m_SelectedAlbedoSwatchIndex == 0))
        {
          EditorGUI.BeginChangeCheck();
          using (new EditorGUI.DisabledScope(this.m_SelectedAlbedoSwatchIndex == 0))
          {
            this.m_AlbedoSwatchHueTolerance = EditorGUILayout.Slider(new GUIContent("Hue Tolerance:", "Check that the hue of the albedo value of a material is within the tolerance of the hue of the albedo swatch being validated against"), this.m_AlbedoSwatchHueTolerance, 0.0f, 0.5f, new GUILayoutOption[0]);
            this.m_AlbedoSwatchSaturationTolerance = EditorGUILayout.Slider(new GUIContent("Saturation Tolerance:", "Check that the saturation of the albedo value of a material is within the tolerance of the saturation of the albedo swatch being validated against"), this.m_AlbedoSwatchSaturationTolerance, 0.0f, 0.5f, new GUILayoutOption[0]);
          }
          if (EditorGUI.EndChangeCheck())
            this.UpdateAlbedoSwatch();
        }
      }
      this.UpdatePBRColorLegend();
      EditorGUILayout.LabelField("Color Legend:");
      ++EditorGUI.indentLevel;
      string str = this.m_RenderMode != DrawCameraMode.ValidateAlbedo ? "Specular" : "Luminance";
      using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
      {
        EditorGUIUtility.labelWidth = 2f;
        EditorGUILayout.LabelField("", this.m_TooLowColorStyle, new GUILayoutOption[0]);
        EditorGUIUtility.labelWidth = 200f;
        EditorGUILayout.LabelField("Below Minimum " + str + " Value");
      }
      using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
      {
        EditorGUIUtility.labelWidth = 2f;
        EditorGUILayout.LabelField("", this.m_TooHighColorStyle, new GUILayoutOption[0]);
        EditorGUIUtility.labelWidth = 200f;
        EditorGUILayout.LabelField("Above Maximum " + str + " Value");
      }
      using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
      {
        EditorGUIUtility.labelWidth = 2f;
        EditorGUILayout.LabelField("", this.m_PureMetalColorStyle, new GUILayoutOption[0]);
        EditorGUIUtility.labelWidth = 200f;
        EditorGUILayout.LabelField("Not A Pure Metal");
      }
    }

    internal void PrepareValidationUI()
    {
      if (this.m_AlbedoSwatchInfos == null)
        this.CreateAlbedoSwatchData();
      if (PlayerSettings.colorSpace == this.m_LastKnownColorSpace)
        return;
      this.UpdateAlbedoSwatchGUI();
      this.UpdateAlbedoSwatch();
    }

    private static void DrawPBRSettings(UnityEngine.Object target, SceneView sceneView)
    {
      sceneView.DrawTrueMetalCheckbox();
      sceneView.DrawPBRSettingsForScene();
    }

    private void DrawValidateAlbedoSwatches(SceneView sceneView)
    {
      if (sceneView.m_RenderMode != DrawCameraMode.ValidateAlbedo && sceneView.m_RenderMode != DrawCameraMode.ValidateMetalSpecular)
        return;
      sceneView.PrepareValidationUI();
      GUIContent title = new GUIContent("PBR Validation Settings");
      // ISSUE: reference to a compiler-generated field
      if (SceneView.\u003C\u003Ef__mg\u0024cache4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SceneView.\u003C\u003Ef__mg\u0024cache4 = new SceneViewOverlay.WindowFunction(SceneView.DrawPBRSettings);
      }
      // ISSUE: reference to a compiler-generated field
      SceneViewOverlay.WindowFunction fMgCache4 = SceneView.\u003C\u003Ef__mg\u0024cache4;
      int order = 450;
      SceneView sceneView1 = sceneView;
      int num = 1;
      SceneViewOverlay.Window(title, fMgCache4, order, (UnityEngine.Object) sceneView1, (SceneViewOverlay.WindowDisplayOption) num);
    }

    private void RepaintGizmosThatAreRenderedOnTopOfSceneView()
    {
      this.svRot.OnGUI(this);
    }

    private void InputForGizmosThatAreRenderedOnTopOfSceneView()
    {
      if (Event.current.type == EventType.Repaint)
        return;
      this.svRot.OnGUI(this);
    }

    internal void OnGUI()
    {
      SceneView.s_CurrentDrawingSceneView = this;
      Event current = Event.current;
      if (current.type == EventType.Repaint)
      {
        SceneView.s_MouseRects.Clear();
        Profiler.BeginSample("SceneView.Repaint");
      }
      Color color = GUI.color;
      Rect rect1 = this.m_Camera.rect;
      this.HandleClickAndDragToFocus();
      if (current.type == EventType.Layout)
        this.m_ShowSceneViewWindows = (UnityEngine.Object) SceneView.lastActiveSceneView == (UnityEngine.Object) this;
      this.m_SceneViewOverlay.Begin();
      bool oldFog;
      float oldShadowDistance;
      this.SetupFogAndShadowDistance(out oldFog, out oldShadowDistance);
      this.DoToolbarGUI();
      GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
      GUI.color = Color.white;
      EditorGUIUtility.labelWidth = 100f;
      this.SetupCamera();
      RenderingPath renderingPath = this.m_Camera.renderingPath;
      this.SetupCustomSceneLighting();
      GUI.BeginGroup(new Rect(0.0f, 17f, this.position.width, this.position.height - 17f));
      Rect rect2 = new Rect(0.0f, 0.0f, this.position.width, this.position.height - 17f);
      Rect pixels = EditorGUIUtility.PointsToPixels(rect2);
      this.HandleViewToolCursor();
      this.PrepareCameraTargetTexture(pixels);
      this.DoClearCamera(pixels);
      this.m_Camera.cullingMask = Tools.visibleLayers;
      this.InputForGizmosThatAreRenderedOnTopOfSceneView();
      this.DoOnPreSceneGUICallbacks(pixels);
      this.PrepareCameraReplacementShader();
      this.m_MainViewControlID = GUIUtility.GetControlID(FocusType.Keyboard);
      if (current.GetTypeForControl(this.m_MainViewControlID) == EventType.MouseDown)
        GUIUtility.keyboardControl = this.m_MainViewControlID;
      bool pushedGUIClip;
      this.DoDrawCamera(rect2, out pushedGUIClip);
      this.CleanupCustomSceneLighting();
      RenderTexture renderTexture = this.m_SceneTargetTexture;
      if (!this.UseSceneFiltering() && current.type == EventType.Repaint && RenderTextureEditor.IsHDRFormat(this.m_SceneTargetTexture.format))
      {
        RenderTextureDescriptor descriptor = this.m_SceneTargetTexture.descriptor;
        descriptor.colorFormat = RenderTextureFormat.ARGB32;
        descriptor.depthBufferBits = 0;
        renderTexture = RenderTexture.GetTemporary(descriptor);
        Graphics.Blit((Texture) this.m_SceneTargetTexture, renderTexture);
        Graphics.SetRenderTarget(renderTexture.colorBuffer, this.m_SceneTargetTexture.depthBuffer);
      }
      if (!this.UseSceneFiltering())
      {
        Handles.DrawCameraStep2(this.m_Camera, this.m_RenderMode);
        bool sRgbWrite = GL.sRGBWrite;
        GL.sRGBWrite = false;
        this.HandleSelectionAndOnSceneGUI();
        GL.sRGBWrite = sRgbWrite;
      }
      if (current.type == EventType.ExecuteCommand || current.type == EventType.ValidateCommand)
        this.CommandsGUI();
      this.RestoreFogAndShadowDistance(oldFog, oldShadowDistance);
      this.m_Camera.renderingPath = renderingPath;
      if (this.UseSceneFiltering())
        Handles.SetCameraFilterMode(Camera.current, Handles.FilterMode.ShowFiltered);
      else
        Handles.SetCameraFilterMode(Camera.current, Handles.FilterMode.Off);
      bool sRgbWrite1 = GL.sRGBWrite;
      GL.sRGBWrite = false;
      this.DefaultHandles();
      GL.sRGBWrite = sRgbWrite1;
      if (!this.UseSceneFiltering())
      {
        if (current.type == EventType.Repaint)
        {
          Profiler.BeginSample("SceneView.BlitRT");
          Graphics.SetRenderTarget((RenderTexture) null);
        }
        if (pushedGUIClip)
          GUIClip.Pop();
        if (current.type == EventType.Repaint)
        {
          GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
          Graphics.DrawTexture(rect2, (Texture) renderTexture, new Rect(0.0f, 0.0f, 1f, 1f), 0, 0, 0, 0, GUI.color, EditorGUIUtility.GUITextureBlitColorspaceMaterial);
          if (RenderTextureEditor.IsHDRFormat(this.m_SceneTargetTexture.format))
            RenderTexture.ReleaseTemporary(renderTexture);
          GL.sRGBWrite = false;
          Profiler.EndSample();
        }
      }
      Handles.SetCameraFilterMode(Camera.current, Handles.FilterMode.Off);
      Handles.SetCameraFilterMode(this.m_Camera, Handles.FilterMode.Off);
      this.HandleDragging();
      this.RepaintGizmosThatAreRenderedOnTopOfSceneView();
      if ((UnityEngine.Object) SceneView.s_LastActiveSceneView == (UnityEngine.Object) this)
      {
        SceneViewMotion.ArrowKeys(this);
        SceneViewMotion.DoViewTool(this);
      }
      this.Handle2DModeSwitch();
      GUI.EndGroup();
      GUI.color = color;
      this.m_SceneViewOverlay.End();
      this.HandleMouseCursor();
      if (current.type == EventType.Repaint)
        Profiler.EndSample();
      SceneView.s_CurrentDrawingSceneView = (SceneView) null;
      this.m_Camera.rect = rect1;
    }

    private void Handle2DModeSwitch()
    {
      Event current = Event.current;
      if (SceneView.k2DMode.activated && !SceneView.waitingFor2DModeKeyUp)
      {
        SceneView.waitingFor2DModeKeyUp = true;
        this.in2DMode = !this.in2DMode;
        current.Use();
      }
      else if (current.type == EventType.KeyUp && current.keyCode == SceneView.k2DMode.KeyboardEvent.keyCode)
        SceneView.waitingFor2DModeKeyUp = false;
    }

    private void HandleMouseCursor()
    {
      Event current = Event.current;
      if (GUIUtility.hotControl == 0)
        this.s_DraggingCursorIsCached = false;
      Rect position = new Rect(0.0f, 0.0f, this.position.width, this.position.height);
      if (!this.s_DraggingCursorIsCached)
      {
        MouseCursor mouseCursor = MouseCursor.Arrow;
        if (current.type == EventType.MouseMove || current.type == EventType.Repaint)
        {
          foreach (SceneView.CursorRect mouseRect in SceneView.s_MouseRects)
          {
            if (mouseRect.rect.Contains(current.mousePosition))
            {
              mouseCursor = mouseRect.cursor;
              position = mouseRect.rect;
            }
          }
          if (GUIUtility.hotControl != 0)
            this.s_DraggingCursorIsCached = true;
          if (mouseCursor != SceneView.s_LastCursor)
          {
            SceneView.s_LastCursor = mouseCursor;
            InternalEditorUtility.ResetCursor();
            this.Repaint();
          }
        }
      }
      if (current.type != EventType.Repaint || SceneView.s_LastCursor == MouseCursor.Arrow)
        return;
      EditorGUIUtility.AddCursorRect(position, SceneView.s_LastCursor);
    }

    private void DrawRenderModeOverlay(Rect cameraRect)
    {
      if (this.m_RenderMode == DrawCameraMode.AlphaChannel)
      {
        if (!(bool) ((UnityEngine.Object) SceneView.s_AlphaOverlayMaterial))
          SceneView.s_AlphaOverlayMaterial = EditorGUIUtility.LoadRequired("SceneView/SceneViewAlphaMaterial.mat") as Material;
        Handles.BeginGUI();
        if (Event.current.type == EventType.Repaint)
          Graphics.DrawTexture(cameraRect, (Texture) EditorGUIUtility.whiteTexture, SceneView.s_AlphaOverlayMaterial);
        Handles.EndGUI();
      }
      if (this.m_RenderMode != DrawCameraMode.DeferredDiffuse && this.m_RenderMode != DrawCameraMode.DeferredSpecular && (this.m_RenderMode != DrawCameraMode.DeferredSmoothness && this.m_RenderMode != DrawCameraMode.DeferredNormal))
        return;
      if (!(bool) ((UnityEngine.Object) SceneView.s_DeferredOverlayMaterial))
        SceneView.s_DeferredOverlayMaterial = EditorGUIUtility.LoadRequired("SceneView/SceneViewDeferredMaterial.mat") as Material;
      Handles.BeginGUI();
      if (Event.current.type == EventType.Repaint)
      {
        SceneView.s_DeferredOverlayMaterial.SetInt("_DisplayMode", (int) (this.m_RenderMode - 8));
        Graphics.DrawTexture(cameraRect, (Texture) EditorGUIUtility.whiteTexture, SceneView.s_DeferredOverlayMaterial);
      }
      Handles.EndGUI();
    }

    private void HandleSelectionAndOnSceneGUI()
    {
      this.m_RectSelection.OnGUI();
      this.CallOnSceneGUI();
    }

    public Vector3 pivot
    {
      get
      {
        return this.m_Position.value;
      }
      set
      {
        this.m_Position.value = value;
      }
    }

    public Quaternion rotation
    {
      get
      {
        return this.m_Rotation.value;
      }
      set
      {
        this.m_Rotation.value = value;
      }
    }

    public float size
    {
      get
      {
        return this.m_Size.value;
      }
      set
      {
        if ((double) value > 40000.0)
          value = 40000f;
        this.m_Size.value = value;
      }
    }

    public bool orthographic
    {
      get
      {
        return this.m_Ortho.value;
      }
      set
      {
        this.m_Ortho.value = value;
      }
    }

    public void FixNegativeSize()
    {
      float num = 90f;
      if ((double) this.size >= 0.0)
        return;
      Vector3 vector3 = this.m_Position.value + this.rotation * new Vector3(0.0f, 0.0f, -(this.size / Mathf.Tan((float) ((double) num * 0.5 * (Math.PI / 180.0)))));
      this.size = -this.size;
      float z = this.size / Mathf.Tan((float) ((double) num * 0.5 * (Math.PI / 180.0)));
      this.m_Position.value = vector3 + this.rotation * new Vector3(0.0f, 0.0f, z);
    }

    private float CalcCameraDist()
    {
      float num = this.m_Ortho.Fade(90f, 0.0f);
      if ((double) num <= 3.0)
        return 0.0f;
      this.m_Camera.orthographic = false;
      return this.size / Mathf.Tan((float) ((double) num * 0.5 * (Math.PI / 180.0)));
    }

    private void ResetIfNaN()
    {
      if (float.IsInfinity(this.m_Position.value.x) || float.IsNaN(this.m_Position.value.x))
        this.m_Position.value = Vector3.zero;
      if (!float.IsInfinity(this.m_Rotation.value.x) && !float.IsNaN(this.m_Rotation.value.x))
        return;
      this.m_Rotation.value = Quaternion.identity;
    }

    internal static Camera GetMainCamera()
    {
      Camera main = Camera.main;
      if ((UnityEngine.Object) main != (UnityEngine.Object) null)
        return main;
      Camera[] allCameras = Camera.allCameras;
      if (allCameras != null && allCameras.Length == 1)
        return allCameras[0];
      return (Camera) null;
    }

    internal static RenderingPath GetSceneViewRenderingPath()
    {
      Camera mainCamera = SceneView.GetMainCamera();
      if ((UnityEngine.Object) mainCamera != (UnityEngine.Object) null)
        return mainCamera.renderingPath;
      return RenderingPath.UsePlayerSettings;
    }

    internal static bool IsUsingDeferredRenderingPath()
    {
      int num;
      switch (SceneView.GetSceneViewRenderingPath())
      {
        case RenderingPath.UsePlayerSettings:
          num = EditorGraphicsSettings.GetCurrentTierSettings().renderingPath == RenderingPath.DeferredShading ? 1 : 0;
          break;
        case RenderingPath.DeferredShading:
          num = 1;
          break;
        default:
          num = 0;
          break;
      }
      return num != 0;
    }

    internal bool CheckDrawModeForRenderingPath(DrawCameraMode mode)
    {
      RenderingPath actualRenderingPath = this.m_Camera.actualRenderingPath;
      if (mode == DrawCameraMode.DeferredDiffuse || mode == DrawCameraMode.DeferredSpecular || (mode == DrawCameraMode.DeferredSmoothness || mode == DrawCameraMode.DeferredNormal))
        return actualRenderingPath == RenderingPath.DeferredShading;
      return true;
    }

    private void SetSceneCameraHDRAndDepthModes()
    {
      if (!this.m_SceneLighting || !SceneView.DoesCameraDrawModeSupportHDR(this.m_RenderMode))
      {
        this.m_Camera.allowHDR = false;
        this.m_Camera.depthTextureMode = DepthTextureMode.None;
        this.m_Camera.clearStencilAfterLightingPass = false;
      }
      else
      {
        Camera mainCamera = SceneView.GetMainCamera();
        if ((UnityEngine.Object) mainCamera == (UnityEngine.Object) null)
        {
          this.m_Camera.allowHDR = false;
          this.m_Camera.depthTextureMode = DepthTextureMode.None;
          this.m_Camera.clearStencilAfterLightingPass = false;
        }
        else
        {
          this.m_Camera.allowHDR = mainCamera.allowHDR;
          this.m_Camera.depthTextureMode = mainCamera.depthTextureMode;
          this.m_Camera.clearStencilAfterLightingPass = mainCamera.clearStencilAfterLightingPass;
        }
      }
    }

    private void SetupCamera()
    {
      this.m_Camera.backgroundColor = this.m_RenderMode != DrawCameraMode.Overdraw ? (Color) SceneView.kSceneViewBackground : Color.black;
      if (Event.current.type == EventType.Repaint)
        this.UpdateImageEffects(!this.UseSceneFiltering() && (this.m_RenderMode == DrawCameraMode.Textured && this.sceneViewState.showImageEffects));
      EditorUtility.SetCameraAnimateMaterials(this.m_Camera, this.sceneViewState.showMaterialUpdate);
      ParticleSystemEditorUtils.editorRenderInSceneView = this.m_SceneViewState.showParticleSystems;
      this.ResetIfNaN();
      this.m_Camera.transform.rotation = this.m_Rotation.value;
      float aspectNeutralFOV = this.m_Ortho.Fade(90f, 0.0f);
      if ((double) aspectNeutralFOV > 3.0)
      {
        this.m_Camera.orthographic = false;
        this.m_Camera.fieldOfView = this.GetVerticalFOV(aspectNeutralFOV);
      }
      else
      {
        this.m_Camera.orthographic = true;
        this.m_Camera.orthographicSize = this.GetVerticalOrthoSize();
      }
      this.m_Camera.transform.position = this.m_Position.value + this.m_Camera.transform.rotation * new Vector3(0.0f, 0.0f, -this.cameraDistance);
      float num = Mathf.Max(1000f, 2000f * this.size);
      this.m_Camera.nearClipPlane = num * 5E-06f;
      this.m_Camera.farClipPlane = num;
      this.m_Camera.renderingPath = SceneView.GetSceneViewRenderingPath();
      if (!this.CheckDrawModeForRenderingPath(this.m_RenderMode))
        this.m_RenderMode = DrawCameraMode.Textured;
      this.SetSceneCameraHDRAndDepthModes();
      if (this.m_RenderMode == DrawCameraMode.Textured || this.m_RenderMode == DrawCameraMode.TexturedWire)
      {
        Handles.EnableCameraFlares(this.m_Camera, this.sceneViewState.showFlares);
        Handles.EnableCameraSkybox(this.m_Camera, this.sceneViewState.showSkybox);
      }
      else
      {
        Handles.EnableCameraFlares(this.m_Camera, false);
        Handles.EnableCameraSkybox(this.m_Camera, false);
      }
      this.m_Light[0].transform.position = this.m_Camera.transform.position;
      this.m_Light[0].transform.rotation = this.m_Camera.transform.rotation;
      if (this.m_AudioPlay)
      {
        AudioUtil.SetListenerTransform(this.m_Camera.transform);
        AudioUtil.UpdateAudio();
      }
      if (!this.m_ViewIsLockedToObject || Selection.gameObjects.Length <= 0)
        return;
      Bounds selectionBounds = InternalEditorUtility.CalculateSelectionBounds(false, Tools.pivotMode == PivotMode.Pivot);
      switch (this.draggingLocked)
      {
        case SceneView.DraggingLockedState.NotDragging:
          this.m_Position.value = selectionBounds.center;
          break;
        case SceneView.DraggingLockedState.LookAt:
          if (!this.m_Position.value.Equals((object) this.m_Position.target))
          {
            this.Frame(selectionBounds, EditorApplication.isPlaying);
            break;
          }
          this.draggingLocked = SceneView.DraggingLockedState.NotDragging;
          break;
      }
    }

    private void OnBecameVisible()
    {
      EditorApplication.update += new EditorApplication.CallbackFunction(this.UpdateAnimatedMaterials);
    }

    private void OnBecameInvisible()
    {
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.UpdateAnimatedMaterials);
    }

    private void UpdateAnimatedMaterials()
    {
      if (!this.sceneViewState.showMaterialUpdate || this.m_lastRenderedTime + 0.0329999998211861 >= EditorApplication.timeSinceStartup)
        return;
      this.m_lastRenderedTime = EditorApplication.timeSinceStartup;
      this.Repaint();
    }

    internal Quaternion cameraTargetRotation
    {
      get
      {
        return this.m_Rotation.target;
      }
    }

    internal Vector3 cameraTargetPosition
    {
      get
      {
        return this.m_Position.target + this.m_Rotation.target * new Vector3(0.0f, 0.0f, this.cameraDistance);
      }
    }

    internal float GetVerticalFOV(float aspectNeutralFOV)
    {
      return (float) ((double) Mathf.Atan(Mathf.Tan((float) ((double) aspectNeutralFOV * 0.5 * (Math.PI / 180.0))) * 0.7071068f / Mathf.Sqrt(this.m_Camera.aspect)) * 2.0 * 57.2957801818848);
    }

    internal float GetVerticalOrthoSize()
    {
      return this.size * 0.7071068f / Mathf.Sqrt(this.m_Camera.aspect);
    }

    public void LookAt(Vector3 pos)
    {
      this.FixNegativeSize();
      this.m_Position.target = pos;
    }

    public void LookAt(Vector3 pos, Quaternion rot)
    {
      this.FixNegativeSize();
      this.m_Position.target = pos;
      this.m_Rotation.target = rot;
      this.svRot.UpdateGizmoLabel(this, rot * Vector3.forward, this.m_Ortho.target);
    }

    public void LookAtDirect(Vector3 pos, Quaternion rot)
    {
      this.FixNegativeSize();
      this.m_Position.value = pos;
      this.m_Rotation.value = rot;
      this.svRot.UpdateGizmoLabel(this, rot * Vector3.forward, this.m_Ortho.target);
    }

    public void LookAt(Vector3 pos, Quaternion rot, float newSize)
    {
      this.FixNegativeSize();
      this.m_Position.target = pos;
      this.m_Rotation.target = rot;
      this.m_Size.target = Mathf.Abs(newSize);
      this.svRot.UpdateGizmoLabel(this, rot * Vector3.forward, this.m_Ortho.target);
    }

    public void LookAtDirect(Vector3 pos, Quaternion rot, float newSize)
    {
      this.FixNegativeSize();
      this.m_Position.value = pos;
      this.m_Rotation.value = rot;
      this.m_Size.value = Mathf.Abs(newSize);
      this.svRot.UpdateGizmoLabel(this, rot * Vector3.forward, this.m_Ortho.target);
    }

    public void LookAt(Vector3 pos, Quaternion rot, float newSize, bool ortho)
    {
      this.LookAt(pos, rot, newSize, ortho, false);
    }

    public void LookAt(Vector3 pos, Quaternion rot, float newSize, bool ortho, bool instant)
    {
      this.FixNegativeSize();
      if (instant)
      {
        this.m_Position.value = pos;
        this.m_Rotation.value = rot;
        this.m_Size.value = Mathf.Abs(newSize);
        this.m_Ortho.value = ortho;
        this.draggingLocked = SceneView.DraggingLockedState.NotDragging;
      }
      else
      {
        this.m_Position.target = pos;
        this.m_Rotation.target = rot;
        this.m_Size.target = Mathf.Abs(newSize);
        this.m_Ortho.target = ortho;
      }
      this.svRot.UpdateGizmoLabel(this, rot * Vector3.forward, this.m_Ortho.target);
    }

    private void DefaultHandles()
    {
      EditorGUI.BeginChangeCheck();
      bool flag1 = Event.current.GetTypeForControl(GUIUtility.hotControl) == EventType.MouseDrag;
      bool flag2 = Event.current.GetTypeForControl(GUIUtility.hotControl) == EventType.MouseUp;
      if (GUIUtility.hotControl == 0)
        SceneView.s_CurrentTool = !Tools.viewToolActive ? Tools.current : Tool.View;
      switch ((Event.current.type != EventType.Repaint ? (int) SceneView.s_CurrentTool : (int) Tools.current) + 1)
      {
        case 2:
          MoveTool.OnGUI(this);
          break;
        case 3:
          RotateTool.OnGUI(this);
          break;
        case 4:
          ScaleTool.OnGUI(this);
          break;
        case 5:
          RectTool.OnGUI(this);
          break;
        case 6:
          TransformTool.OnGUI(this);
          break;
      }
      if (EditorGUI.EndChangeCheck() && EditorApplication.isPlaying && flag1)
        Physics2D.SetEditorDragMovement(true, Selection.gameObjects);
      if (!EditorApplication.isPlaying || !flag2)
        return;
      Physics2D.SetEditorDragMovement(false, Selection.gameObjects);
    }

    private void CleanupEditorDragFunctions()
    {
      if (this.m_DragEditorCache != null)
        this.m_DragEditorCache.Dispose();
      this.m_DragEditorCache = (EditorCache) null;
    }

    private void CallEditorDragFunctions()
    {
      Event current = Event.current;
      SpriteUtility.OnSceneDrag(this);
      if (current.type == EventType.Used || DragAndDrop.objectReferences.Length == 0)
        return;
      if (this.m_DragEditorCache == null)
        this.m_DragEditorCache = new EditorCache(EditorFeatures.OnSceneDrag);
      foreach (UnityEngine.Object objectReference in DragAndDrop.objectReferences)
      {
        if (!(objectReference == (UnityEngine.Object) null))
        {
          EditorWrapper editorWrapper = this.m_DragEditorCache[objectReference];
          if (editorWrapper != null)
            editorWrapper.OnSceneDrag(this);
          if (current.type == EventType.Used)
            break;
        }
      }
    }

    private void HandleDragging()
    {
      Event current = Event.current;
      switch (current.type)
      {
        case EventType.DragUpdated:
        case EventType.DragPerform:
          this.CallEditorDragFunctions();
          if (current.type == EventType.Used)
            break;
          bool perform = current.type == EventType.DragPerform;
          if (DragAndDrop.visualMode != DragAndDropVisualMode.Copy)
            DragAndDrop.visualMode = InternalEditorUtility.SceneViewDrag((UnityEngine.Object) HandleUtility.PickGameObject(Event.current.mousePosition, true), this.pivot, Event.current.mousePosition, perform);
          if (perform && DragAndDrop.visualMode != DragAndDropVisualMode.None)
          {
            DragAndDrop.AcceptDrag();
            current.Use();
            GUIUtility.ExitGUI();
          }
          current.Use();
          break;
        case EventType.DragExited:
          this.CallEditorDragFunctions();
          this.CleanupEditorDragFunctions();
          break;
      }
    }

    private void CommandsGUI()
    {
      bool flag = Event.current.type == EventType.ExecuteCommand;
      string commandName = Event.current.commandName;
      if (commandName == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (SceneView.\u003C\u003Ef__switch\u0024map2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SceneView.\u003C\u003Ef__switch\u0024map2 = new Dictionary<string, int>(9)
        {
          {
            "Find",
            0
          },
          {
            "FrameSelected",
            1
          },
          {
            "FrameSelectedWithLock",
            2
          },
          {
            "SoftDelete",
            3
          },
          {
            "Delete",
            3
          },
          {
            "Duplicate",
            4
          },
          {
            "Copy",
            5
          },
          {
            "Paste",
            6
          },
          {
            "SelectAll",
            7
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!SceneView.\u003C\u003Ef__switch\u0024map2.TryGetValue(commandName, out num))
        return;
      switch (num)
      {
        case 0:
          if (flag)
            this.FocusSearchField();
          Event.current.Use();
          break;
        case 1:
          if (flag)
          {
            this.FrameSelected(EditorApplication.timeSinceStartup - this.lastFramingTime < 0.5);
            this.lastFramingTime = EditorApplication.timeSinceStartup;
          }
          Event.current.Use();
          break;
        case 2:
          if (flag)
            this.FrameSelected(true);
          Event.current.Use();
          break;
        case 3:
          if (flag)
            Unsupported.DeleteGameObjectSelection();
          Event.current.Use();
          break;
        case 4:
          if (flag)
            Unsupported.DuplicateGameObjectsUsingPasteboard();
          Event.current.Use();
          break;
        case 5:
          if (flag)
            Unsupported.CopyGameObjectsToPasteboard();
          Event.current.Use();
          break;
        case 6:
          if (flag)
            Unsupported.PasteGameObjectsFromPasteboard();
          Event.current.Use();
          break;
        case 7:
          if (flag)
            Selection.objects = UnityEngine.Object.FindObjectsOfType(typeof (GameObject));
          Event.current.Use();
          break;
      }
    }

    public void AlignViewToObject(Transform t)
    {
      this.FixNegativeSize();
      this.size = 10f;
      this.LookAt(t.position + t.forward * this.CalcCameraDist(), t.rotation);
    }

    public void AlignWithView()
    {
      this.FixNegativeSize();
      Vector3 position = this.camera.transform.position;
      Vector3 vector3 = position - Tools.handlePosition;
      float angle;
      Vector3 axis1;
      (Quaternion.Inverse(Selection.activeTransform.rotation) * this.camera.transform.rotation).ToAngleAxis(out angle, out axis1);
      Vector3 axis2 = Selection.activeTransform.TransformDirection(axis1);
      Undo.RecordObjects((UnityEngine.Object[]) Selection.transforms, "Align with view");
      foreach (Transform transform in Selection.transforms)
      {
        transform.position += vector3;
        transform.RotateAround(position, axis2, angle);
      }
    }

    public void MoveToView()
    {
      this.FixNegativeSize();
      Vector3 vector3 = this.pivot - Tools.handlePosition;
      Undo.RecordObjects((UnityEngine.Object[]) Selection.transforms, "Move to view");
      foreach (Transform transform in Selection.transforms)
        transform.position += vector3;
    }

    public void MoveToView(Transform target)
    {
      target.position = this.pivot;
    }

    public bool FrameSelected()
    {
      return this.FrameSelected(false);
    }

    public bool FrameSelected(bool lockView)
    {
      this.viewIsLockedToObject = lockView;
      this.FixNegativeSize();
      Bounds bounds = InternalEditorUtility.CalculateSelectionBounds(false, Tools.pivotMode == PivotMode.Pivot);
      foreach (Editor activeEditor in this.GetActiveEditors())
      {
        MethodInfo method1 = activeEditor.GetType().GetMethod("HasFrameBounds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
        if (method1 != null)
        {
          object obj1 = method1.Invoke((object) activeEditor, (object[]) null);
          if (obj1 is bool && (bool) obj1)
          {
            MethodInfo method2 = activeEditor.GetType().GetMethod("OnGetFrameBounds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            if (method2 != null)
            {
              object obj2 = method2.Invoke((object) activeEditor, (object[]) null);
              if (obj2 is Bounds)
                bounds = (Bounds) obj2;
            }
          }
        }
      }
      return this.Frame(bounds, EditorApplication.isPlaying);
    }

    public bool Frame(Bounds bounds, bool instant = true)
    {
      float num = bounds.extents.magnitude * 1.5f;
      if ((double) num == double.PositiveInfinity)
        return false;
      if ((double) num == 0.0)
        num = 10f;
      this.LookAt(bounds.center, this.m_Rotation.target, num * 2.2f, this.m_Ortho.value, instant);
      return true;
    }

    private void CreateSceneCameraAndLights()
    {
      GameObject objectWithHideFlags1 = EditorUtility.CreateGameObjectWithHideFlags("SceneCamera", HideFlags.HideAndDontSave, typeof (Camera));
      objectWithHideFlags1.AddComponentInternal("FlareLayer");
      objectWithHideFlags1.AddComponentInternal("HaloLayer");
      this.m_Camera = objectWithHideFlags1.GetComponent<Camera>();
      this.m_Camera.enabled = false;
      this.m_Camera.cameraType = CameraType.SceneView;
      for (int index = 0; index < 3; ++index)
      {
        GameObject objectWithHideFlags2 = EditorUtility.CreateGameObjectWithHideFlags("SceneLight", HideFlags.HideAndDontSave, typeof (Light));
        this.m_Light[index] = objectWithHideFlags2.GetComponent<Light>();
        this.m_Light[index].type = LightType.Directional;
        this.m_Light[index].intensity = 1f;
        this.m_Light[index].enabled = false;
      }
      this.m_Light[0].color = SceneView.kSceneViewFrontLight;
      this.m_Light[1].color = SceneView.kSceneViewUpLight - SceneView.kSceneViewMidLight;
      this.m_Light[1].transform.LookAt(Vector3.down);
      this.m_Light[1].renderMode = LightRenderMode.ForceVertex;
      this.m_Light[2].color = SceneView.kSceneViewDownLight - SceneView.kSceneViewMidLight;
      this.m_Light[2].transform.LookAt(Vector3.up);
      this.m_Light[2].renderMode = LightRenderMode.ForceVertex;
      HandleUtility.handleMaterial.SetColor("_SkyColor", SceneView.kSceneViewUpLight * 1.5f);
      HandleUtility.handleMaterial.SetColor("_GroundColor", SceneView.kSceneViewDownLight * 1.5f);
      HandleUtility.handleMaterial.SetColor("_Color", SceneView.kSceneViewFrontLight * 1.5f);
      this.SetupPBRValidation();
    }

    private void CallOnSceneGUI()
    {
      foreach (Editor activeEditor in this.GetActiveEditors())
      {
        if (EditorGUIUtility.IsGizmosAllowedForObject(activeEditor.target))
        {
          MethodInfo method = activeEditor.GetType().GetMethod("OnSceneGUI", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
          if (method != null)
          {
            Editor.m_AllowMultiObjectAccess = true;
            for (int index = 0; index < activeEditor.targets.Length; ++index)
            {
              this.ResetOnSceneGUIState();
              activeEditor.referenceTargetIndex = index;
              EditorGUI.BeginChangeCheck();
              Editor.m_AllowMultiObjectAccess = !activeEditor.canEditMultipleObjects;
              method.Invoke((object) activeEditor, (object[]) null);
              Editor.m_AllowMultiObjectAccess = true;
              if (EditorGUI.EndChangeCheck())
                activeEditor.serializedObject.SetIsDifferentCacheDirty();
            }
            this.ResetOnSceneGUIState();
          }
        }
      }
      if (SceneView.onSceneGUIDelegate == null)
        return;
      SceneView.onSceneGUIDelegate(this);
      this.ResetOnSceneGUIState();
    }

    private void ResetOnSceneGUIState()
    {
      Handles.ClearHandles();
      HandleUtility.s_CustomPickDistance = 5f;
      EditorGUIUtility.ResetGUIState();
      GUI.color = Color.white;
    }

    private void CallOnPreSceneGUI()
    {
      foreach (Editor activeEditor in this.GetActiveEditors())
      {
        Handles.ClearHandles();
        Component target = activeEditor.target as Component;
        if (!(bool) ((UnityEngine.Object) target) || target.gameObject.activeInHierarchy)
        {
          MethodInfo method = activeEditor.GetType().GetMethod("OnPreSceneGUI", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
          if (method != null)
          {
            Editor.m_AllowMultiObjectAccess = true;
            for (int index = 0; index < activeEditor.targets.Length; ++index)
            {
              activeEditor.referenceTargetIndex = index;
              Editor.m_AllowMultiObjectAccess = !activeEditor.canEditMultipleObjects;
              method.Invoke((object) activeEditor, (object[]) null);
              Editor.m_AllowMultiObjectAccess = true;
            }
          }
        }
      }
      if (SceneView.onPreSceneGUIDelegate != null)
        SceneView.onPreSceneGUIDelegate(this);
      Handles.ClearHandles();
    }

    internal static void ShowNotification(string notificationText)
    {
      UnityEngine.Object[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (SceneView));
      List<EditorWindow> editorWindowList = new List<EditorWindow>();
      foreach (SceneView sceneView in objectsOfTypeAll)
      {
        if (sceneView.m_Parent is DockArea)
        {
          DockArea parent = (DockArea) sceneView.m_Parent;
          if ((bool) ((UnityEngine.Object) parent) && (UnityEngine.Object) parent.actualView == (UnityEngine.Object) sceneView)
            editorWindowList.Add((EditorWindow) sceneView);
        }
      }
      if (editorWindowList.Count > 0)
      {
        foreach (EditorWindow editorWindow in editorWindowList)
          editorWindow.ShowNotification(GUIContent.Temp(notificationText));
      }
      else
        Debug.LogError((object) notificationText);
    }

    public static void ShowCompileErrorNotification()
    {
      SceneView.ShowNotification("All compiler errors have to be fixed before you can enter playmode!");
    }

    internal static void ShowSceneViewPlayModeSaveWarning()
    {
      GameView editorWindowOfType = (GameView) WindowLayout.FindEditorWindowOfType(typeof (GameView));
      if ((UnityEngine.Object) editorWindowOfType != (UnityEngine.Object) null && editorWindowOfType.hasFocus)
        editorWindowOfType.ShowNotification(new GUIContent("You must exit play mode to save the scene!"));
      else
        SceneView.ShowNotification("You must exit play mode to save the scene!");
    }

    private void ResetToDefaults(EditorBehaviorMode behaviorMode)
    {
      if (behaviorMode != EditorBehaviorMode.Mode2D)
      {
        if (behaviorMode == EditorBehaviorMode.Mode3D)
          ;
        this.m_2DMode = false;
        this.m_Rotation.value = SceneView.kDefaultRotation;
        this.m_Position.value = SceneView.kDefaultPivot;
        this.m_Size.value = 10f;
        this.m_Ortho.value = false;
      }
      else
      {
        this.m_2DMode = true;
        this.m_Rotation.value = Quaternion.identity;
        this.m_Position.value = SceneView.kDefaultPivot;
        this.m_Size.value = 10f;
        this.m_Ortho.value = true;
        this.m_LastSceneViewRotation = SceneView.kDefaultRotation;
        this.m_LastSceneViewOrtho = false;
      }
    }

    internal void OnNewProjectLayoutWasCreated()
    {
      this.ResetToDefaults(EditorSettings.defaultBehaviorMode);
    }

    private void On2DModeChange()
    {
      if (this.m_2DMode)
      {
        this.lastSceneViewRotation = this.rotation;
        this.m_LastSceneViewOrtho = this.orthographic;
        this.LookAt(this.pivot, Quaternion.identity, this.size, true);
        if (Tools.current == Tool.Move)
          Tools.current = Tool.Rect;
      }
      else
      {
        this.LookAt(this.pivot, this.lastSceneViewRotation, this.size, this.m_LastSceneViewOrtho);
        if (Tools.current == Tool.Rect)
          Tools.current = Tool.Move;
      }
      HandleUtility.ignoreRaySnapObjects = (Transform[]) null;
      Tools.vertexDragging = false;
      Tools.handleOffset = Vector3.zero;
    }

    [Serializable]
    public class SceneViewState
    {
      public bool showFog = true;
      public bool showMaterialUpdate = false;
      public bool showSkybox = true;
      public bool showFlares = true;
      public bool showImageEffects = true;
      public bool showParticleSystems = true;

      public SceneViewState()
      {
      }

      public SceneViewState(SceneView.SceneViewState other)
      {
        this.showFog = other.showFog;
        this.showMaterialUpdate = other.showMaterialUpdate;
        this.showSkybox = other.showSkybox;
        this.showFlares = other.showFlares;
        this.showImageEffects = other.showImageEffects;
        this.showParticleSystems = other.showParticleSystems;
      }

      public bool IsAllOn()
      {
        return this.showFog && this.showMaterialUpdate && (this.showSkybox && this.showFlares) && this.showImageEffects && this.showParticleSystems;
      }

      public void Toggle(bool value)
      {
        this.showFog = value;
        this.showMaterialUpdate = value;
        this.showSkybox = value;
        this.showFlares = value;
        this.showImageEffects = value;
        this.showParticleSystems = value;
      }
    }

    public delegate void OnSceneFunc(SceneView sceneView);

    private struct CursorRect
    {
      public Rect rect;
      public MouseCursor cursor;

      public CursorRect(Rect rect, MouseCursor cursor)
      {
        this.rect = rect;
        this.cursor = cursor;
      }
    }

    internal enum DraggingLockedState
    {
      NotDragging,
      Dragging,
      LookAt,
    }
  }
}
