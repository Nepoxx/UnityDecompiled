// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Modules;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Game", useTypeNameAsIconName = true)]
  internal class GameView : EditorWindow, IHasCustomMenu, IGameViewSizeMenuUser
  {
    private static List<GameView> s_GameViews = new List<GameView>();
    private static GameView s_LastFocusedGameView = (GameView) null;
    private readonly Vector2 kWarningSize = new Vector2(400f, 140f);
    private readonly Color kClearBlack = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    [SerializeField]
    private int[] m_SelectedSizes = new int[0];
    [SerializeField]
    private float m_defaultScale = -1f;
    [SerializeField]
    private ColorSpace m_CurrentColorSpace = ColorSpace.Uninitialized;
    [SerializeField]
    private bool m_ClearInEditMode = true;
    [SerializeField]
    private bool m_NoCameraWarning = true;
    [SerializeField]
    private bool[] m_LowResolutionForAspectRatios = new bool[0];
    private int m_SizeChangeID = int.MinValue;
    private const int kBorderSize = 5;
    private const int kScaleSliderMinWidth = 30;
    private const int kScaleSliderMaxWidth = 150;
    private const int kScaleSliderSnapThreshold = 4;
    private const int kScaleLabelWidth = 30;
    private const float kMinScale = 1f;
    private const float kMaxScale = 5f;
    private const float kScrollZoomSnapDelay = 0.2f;
    [SerializeField]
    private bool m_MaximizeOnPlay;
    [SerializeField]
    private bool m_Gizmos;
    [SerializeField]
    private bool m_Stats;
    [SerializeField]
    private int m_TargetDisplay;
    [SerializeField]
    private ZoomableArea m_ZoomArea;
    [SerializeField]
    private RenderTexture m_TargetTexture;
    private bool m_TargetClamped;
    [SerializeField]
    private Vector2 m_LastWindowPixelSize;
    private static double s_LastScrollTime;

    public GameView()
    {
      this.autoRepaintOnSceneChange = true;
      this.m_TargetDisplay = 0;
      this.InitializeZoomArea();
    }

    private float minScale
    {
      get
      {
        float a = Mathf.Min(1f, this.ScaleThatFitsTargetInView(this.targetSize, this.viewInWindow.size));
        if (this.m_LowResolutionForAspectRatios[(int) GameView.currentSizeGroupType] && this.currentGameViewSize.sizeType == GameViewSizeType.AspectRatio)
          a = Mathf.Max(a, EditorGUIUtility.pixelsPerPoint);
        return a;
      }
    }

    private float maxScale
    {
      get
      {
        return Mathf.Max(5f * EditorGUIUtility.pixelsPerPoint, this.ScaleThatFitsTargetInView(this.targetSize, this.viewInWindow.size));
      }
    }

    public bool lowResolutionForAspectRatios
    {
      get
      {
        this.EnsureSelectedSizeAreValid();
        return this.m_LowResolutionForAspectRatios[(int) GameView.currentSizeGroupType];
      }
      set
      {
        this.EnsureSelectedSizeAreValid();
        if (value == this.m_LowResolutionForAspectRatios[(int) GameView.currentSizeGroupType])
          return;
        this.m_LowResolutionForAspectRatios[(int) GameView.currentSizeGroupType] = value;
        this.UpdateZoomAreaAndParent();
      }
    }

    public bool forceLowResolutionAspectRatios
    {
      get
      {
        return (double) EditorGUIUtility.pixelsPerPoint == 1.0;
      }
    }

    public bool showLowResolutionToggle
    {
      get
      {
        return EditorApplication.supportsHiDPI;
      }
    }

    public bool maximizeOnPlay
    {
      get
      {
        return this.m_MaximizeOnPlay;
      }
      set
      {
        this.m_MaximizeOnPlay = value;
      }
    }

    private int selectedSizeIndex
    {
      get
      {
        this.EnsureSelectedSizeAreValid();
        return this.m_SelectedSizes[(int) GameView.currentSizeGroupType];
      }
      set
      {
        this.EnsureSelectedSizeAreValid();
        this.m_SelectedSizes[(int) GameView.currentSizeGroupType] = value;
      }
    }

    private static GameViewSizeGroupType currentSizeGroupType
    {
      get
      {
        return ScriptableSingleton<GameViewSizes>.instance.currentGroupType;
      }
    }

    private GameViewSize currentGameViewSize
    {
      get
      {
        return ScriptableSingleton<GameViewSizes>.instance.currentGroup.GetGameViewSize(this.selectedSizeIndex);
      }
    }

    private Rect viewInWindow
    {
      get
      {
        return new Rect(0.0f, 17f, this.position.width, this.position.height - 17f);
      }
    }

    internal Vector2 targetSize
    {
      get
      {
        return GameViewSizes.GetRenderTargetSize(!this.lowResolutionForAspectRatios ? EditorGUIUtility.PointsToPixels(this.viewInWindow) : this.viewInWindow, GameView.currentSizeGroupType, this.selectedSizeIndex, out this.m_TargetClamped);
      }
    }

    private Rect targetInContent
    {
      get
      {
        return EditorGUIUtility.PixelsToPoints(new Rect(-0.5f * this.targetSize, this.targetSize));
      }
    }

    private Rect targetInView
    {
      get
      {
        return new Rect(this.m_ZoomArea.DrawingToViewTransformPoint(this.targetInContent.position), this.m_ZoomArea.DrawingToViewTransformVector(this.targetInContent.size));
      }
    }

    private Rect deviceFlippedTargetInView
    {
      get
      {
        if (!SystemInfo.graphicsUVStartsAtTop)
          return this.targetInView;
        Rect targetInView = this.targetInView;
        targetInView.y += targetInView.height;
        targetInView.height = -targetInView.height;
        return targetInView;
      }
    }

    private Rect viewInParent
    {
      get
      {
        Rect viewInWindow = this.viewInWindow;
        RectOffset borderSize = this.m_Parent.borderSize;
        viewInWindow.x += (float) borderSize.left;
        viewInWindow.y += (float) (borderSize.top + borderSize.bottom);
        return viewInWindow;
      }
    }

    private Rect targetInParent
    {
      get
      {
        return new Rect(this.targetInView.position + this.viewInParent.position, this.targetInView.size);
      }
    }

    private Rect clippedTargetInParent
    {
      get
      {
        return Rect.MinMaxRect(Mathf.Max(this.targetInParent.xMin, this.viewInParent.xMin), Mathf.Max(this.targetInParent.yMin, this.viewInParent.yMin), Mathf.Min(this.targetInParent.xMax, this.viewInParent.xMax), Mathf.Min(this.targetInParent.yMax, this.viewInParent.yMax));
      }
    }

    private Rect warningPosition
    {
      get
      {
        return new Rect((this.viewInWindow.size - this.kWarningSize) * 0.5f, this.kWarningSize);
      }
    }

    private Vector2 gameMouseOffset
    {
      get
      {
        return -this.viewInWindow.position - this.targetInView.position;
      }
    }

    private float gameMouseScale
    {
      get
      {
        return EditorGUIUtility.pixelsPerPoint / this.m_ZoomArea.scale.y;
      }
    }

    private Vector2 WindowToGameMousePosition(Vector2 windowMousePosition)
    {
      return (windowMousePosition + this.gameMouseOffset) * this.gameMouseScale;
    }

    private void InitializeZoomArea()
    {
      this.m_ZoomArea = new ZoomableArea(true, false);
      this.m_ZoomArea.uniformScale = true;
      this.m_ZoomArea.upDirection = ZoomableArea.YDirection.Negative;
    }

    public void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
      this.UpdateZoomAreaAndParent();
      this.dontClearBackground = true;
      GameView.s_GameViews.Add(this);
    }

    public void OnDisable()
    {
      GameView.s_GameViews.Remove(this);
      if (!(bool) ((UnityEngine.Object) this.m_TargetTexture))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_TargetTexture);
    }

    internal static GameView GetMainGameView()
    {
      if ((UnityEngine.Object) GameView.s_LastFocusedGameView == (UnityEngine.Object) null && GameView.s_GameViews != null && GameView.s_GameViews.Count > 0)
        GameView.s_LastFocusedGameView = GameView.s_GameViews[0];
      return GameView.s_LastFocusedGameView;
    }

    public static void RepaintAll()
    {
      if (GameView.s_GameViews == null)
        return;
      foreach (EditorWindow gameView in GameView.s_GameViews)
        gameView.Repaint();
    }

    internal static Vector2 GetSizeOfMainGameView()
    {
      return GameView.GetMainGameViewTargetSize();
    }

    internal static Vector2 GetMainGameViewTargetSize()
    {
      GameView mainGameView = GameView.GetMainGameView();
      if ((UnityEngine.Object) mainGameView != (UnityEngine.Object) null && (bool) ((UnityEngine.Object) mainGameView.m_Parent))
        return mainGameView.targetSize;
      return new Vector2(640f, 480f);
    }

    [RequiredByNativeCode]
    private static void GetMainGameViewTargetSizeNoBox(out Vector2 result)
    {
      result = GameView.GetMainGameViewTargetSize();
    }

    private void UpdateZoomAreaAndParent()
    {
      bool flag = Mathf.Approximately(this.m_ZoomArea.scale.y, this.m_defaultScale);
      this.ConfigureZoomArea();
      this.m_defaultScale = this.DefaultScaleForTargetInView(this.targetSize, this.viewInWindow.size);
      if (flag)
      {
        this.m_ZoomArea.SetTransform(Vector2.zero, Vector2.one * this.m_defaultScale);
        this.EnforceZoomAreaConstraints();
      }
      this.CopyDimensionsToParentView();
      this.m_LastWindowPixelSize = this.position.size * EditorGUIUtility.pixelsPerPoint;
      EditorApplication.SetSceneRepaintDirty();
    }

    private void AllowCursorLockAndHide(bool enable)
    {
      Unsupported.SetAllowCursorLock(enable);
      Unsupported.SetAllowCursorHide(enable);
    }

    private void OnFocus()
    {
      this.AllowCursorLockAndHide(true);
      GameView.s_LastFocusedGameView = this;
      InternalEditorUtility.OnGameViewFocus(true);
    }

    private void OnLostFocus()
    {
      if (!EditorApplicationLayout.IsInitializingPlaymodeLayout())
        this.AllowCursorLockAndHide(false);
      InternalEditorUtility.OnGameViewFocus(false);
    }

    internal void CopyDimensionsToParentView()
    {
      if (!(bool) ((UnityEngine.Object) this.m_Parent))
        return;
      this.SetParentGameViewDimensions(this.targetInParent, this.clippedTargetInParent, this.targetSize);
    }

    private void EnsureSelectedSizeAreValid()
    {
      if (ScriptableSingleton<GameViewSizes>.instance.GetChangeID() == this.m_SizeChangeID)
        return;
      this.m_SizeChangeID = ScriptableSingleton<GameViewSizes>.instance.GetChangeID();
      Array values = Enum.GetValues(typeof (GameViewSizeGroupType));
      if (this.m_SelectedSizes.Length != values.Length)
        Array.Resize<int>(ref this.m_SelectedSizes, values.Length);
      IEnumerator enumerator = values.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          GameViewSizeGroupType current = (GameViewSizeGroupType) enumerator.Current;
          GameViewSizeGroup group = ScriptableSingleton<GameViewSizes>.instance.GetGroup(current);
          int index = (int) current;
          this.m_SelectedSizes[index] = Mathf.Clamp(this.m_SelectedSizes[index], 0, group.GetTotalCount() - 1);
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      int length = this.m_LowResolutionForAspectRatios.Length;
      if (this.m_LowResolutionForAspectRatios.Length != values.Length)
        Array.Resize<bool>(ref this.m_LowResolutionForAspectRatios, values.Length);
      for (int index = length; index < values.Length; ++index)
        this.m_LowResolutionForAspectRatios[index] = GameViewSizes.DefaultLowResolutionSettingForSizeGroupType((GameViewSizeGroupType) values.GetValue(index));
    }

    public bool IsShowingGizmos()
    {
      return this.m_Gizmos;
    }

    private void OnSelectionChange()
    {
      if (!this.m_Gizmos)
        return;
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
      if (RenderDoc.IsInstalled() && !RenderDoc.IsLoaded())
        menu.AddItem(GameView.Styles.loadRenderDocContent, false, new GenericMenu.MenuFunction(this.LoadRenderDoc));
      menu.AddItem(GameView.Styles.noCameraWarningContextMenuContent, this.m_NoCameraWarning, new GenericMenu.MenuFunction(this.ToggleNoCameraWarning));
      menu.AddItem(GameView.Styles.clearEveryFrameContextMenuContent, this.m_ClearInEditMode, new GenericMenu.MenuFunction(this.ToggleClearInEditMode));
    }

    private void ToggleNoCameraWarning()
    {
      this.m_NoCameraWarning = !this.m_NoCameraWarning;
    }

    private void ToggleClearInEditMode()
    {
      this.m_ClearInEditMode = !this.m_ClearInEditMode;
    }

    public void SizeSelectionCallback(int indexClicked, object objectSelected)
    {
      if (indexClicked == this.selectedSizeIndex)
        return;
      this.selectedSizeIndex = indexClicked;
      this.dontClearBackground = true;
      this.UpdateZoomAreaAndParent();
    }

    private void SnapZoomDelayed()
    {
      if (EditorApplication.timeSinceStartup <= GameView.s_LastScrollTime + 0.200000002980232)
        return;
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.SnapZoomDelayed);
      this.SnapZoom(this.m_ZoomArea.scale.y);
      this.Repaint();
    }

    private void SnapZoom(float newZoom)
    {
      float num1 = Mathf.Log10(newZoom);
      float num2 = Mathf.Log10(this.minScale);
      float num3 = Mathf.Log10(this.maxScale);
      float num4 = float.MaxValue;
      if ((double) num1 > (double) num2 && (double) num1 < (double) num3)
      {
        for (int index = 1; (double) index <= (double) this.maxScale; ++index)
        {
          float num5 = (float) (150.0 * (double) Mathf.Abs(num1 - Mathf.Log10((float) index)) / ((double) num3 - (double) num2));
          if ((double) num5 < 4.0 && (double) num5 < (double) num4)
          {
            newZoom = (float) index;
            num4 = num5;
          }
        }
      }
      Rect areaInsideMargins = this.m_ZoomArea.shownAreaInsideMargins;
      this.m_ZoomArea.SetScaleFocused(areaInsideMargins.position + areaInsideMargins.size * 0.5f, Vector2.one * newZoom);
    }

    private void DoZoomSlider()
    {
      GUILayout.Label(GameView.Styles.zoomSliderContent, EditorStyles.miniLabel, new GUILayoutOption[0]);
      EditorGUI.BeginChangeCheck();
      float p = GUILayout.HorizontalSlider(Mathf.Log10(this.m_ZoomArea.scale.y), Mathf.Log10(this.minScale), Mathf.Log10(this.maxScale), GUILayout.MaxWidth(150f), GUILayout.MinWidth(30f));
      if (EditorGUI.EndChangeCheck())
        this.SnapZoom(Mathf.Pow(10f, p));
      GUIContent content = EditorGUIUtility.TempContent(string.Format("{0}x", (object) this.m_ZoomArea.scale.y.ToString("G2")));
      content.tooltip = GameView.Styles.zoomSliderContent.tooltip;
      GUILayout.Label(content, EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        GUILayout.Width(30f)
      });
      content.tooltip = string.Empty;
    }

    private void DoToolbarGUI()
    {
      ScriptableSingleton<GameViewSizes>.instance.RefreshStandaloneAndRemoteDefaultSizes();
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      if (ModuleManager.ShouldShowMultiDisplayOption())
      {
        int num = EditorGUILayout.Popup(this.m_TargetDisplay, DisplayUtility.GetDisplayNames(), EditorStyles.toolbarPopup, new GUILayoutOption[1]{ GUILayout.Width(80f) });
        EditorGUILayout.Space();
        if (num != this.m_TargetDisplay)
        {
          this.m_TargetDisplay = num;
          this.UpdateZoomAreaAndParent();
        }
      }
      EditorGUILayout.GameViewSizePopup(GameView.currentSizeGroupType, this.selectedSizeIndex, (IGameViewSizeMenuUser) this, EditorStyles.toolbarPopup, GUILayout.Width(160f));
      this.DoZoomSlider();
      if (FrameDebuggerUtility.IsLocalEnabled())
      {
        GUILayout.FlexibleSpace();
        Color color = GUI.color;
        GUI.color *= AnimationMode.recordedPropertyColor;
        GUILayout.Label(GameView.Styles.frameDebuggerOnContent, EditorStyles.miniLabel, new GUILayoutOption[0]);
        GUI.color = color;
        if (Event.current.type == EventType.Repaint)
          FrameDebuggerWindow.RepaintAll();
      }
      GUILayout.FlexibleSpace();
      if (RenderDoc.IsLoaded())
      {
        using (new EditorGUI.DisabledScope(!RenderDoc.IsSupported()))
        {
          if (GUILayout.Button(GameView.Styles.renderdocContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
          {
            this.m_Parent.CaptureRenderDoc();
            GUIUtility.ExitGUI();
          }
        }
      }
      this.m_MaximizeOnPlay = GUILayout.Toggle(this.m_MaximizeOnPlay, GameView.Styles.maximizeOnPlayContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      EditorUtility.audioMasterMute = GUILayout.Toggle(EditorUtility.audioMasterMute, GameView.Styles.muteContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      this.m_Stats = GUILayout.Toggle(this.m_Stats, GameView.Styles.statsContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      Rect rect = GUILayoutUtility.GetRect(GameView.Styles.gizmosContent, GameView.Styles.gizmoButtonStyle);
      if (EditorGUI.DropdownButton(new Rect(rect.xMax - (float) GameView.Styles.gizmoButtonStyle.border.right, rect.y, (float) GameView.Styles.gizmoButtonStyle.border.right, rect.height), GUIContent.none, FocusType.Passive, GUIStyle.none) && AnnotationWindow.ShowAtPosition(GUILayoutUtility.topLevel.GetLast(), true))
        GUIUtility.ExitGUI();
      this.m_Gizmos = GUI.Toggle(rect, this.m_Gizmos, GameView.Styles.gizmosContent, GameView.Styles.gizmoButtonStyle);
      GUILayout.EndHorizontal();
    }

    private void ClearTargetTexture()
    {
      if (!this.m_TargetTexture.IsCreated())
        return;
      RenderTexture active = RenderTexture.active;
      RenderTexture.active = this.m_TargetTexture;
      GL.Clear(true, true, this.kClearBlack);
      RenderTexture.active = active;
    }

    private void ConfigureTargetTexture(int width, int height)
    {
      bool flag = false;
      if ((bool) ((UnityEngine.Object) this.m_TargetTexture) && this.m_CurrentColorSpace != QualitySettings.activeColorSpace)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_TargetTexture);
      if (!(bool) ((UnityEngine.Object) this.m_TargetTexture))
      {
        this.m_CurrentColorSpace = QualitySettings.activeColorSpace;
        this.m_TargetTexture = new RenderTexture(0, 0, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        this.m_TargetTexture.name = "GameView RT";
        this.m_TargetTexture.filterMode = UnityEngine.FilterMode.Point;
        this.m_TargetTexture.hideFlags = HideFlags.HideAndDontSave;
        EditorGUIUtility.SetGUITextureBlitColorspaceSettings(EditorGUIUtility.GUITextureBlitColorspaceMaterial);
      }
      if (this.m_TargetTexture.width != width || this.m_TargetTexture.height != height)
      {
        this.m_TargetTexture.Release();
        this.m_TargetTexture.width = width;
        this.m_TargetTexture.height = height;
        this.m_TargetTexture.antiAliasing = 1;
        flag = true;
        if (this.m_TargetClamped)
          Debug.LogWarningFormat("GameView reduced to a reasonable size for this system ({0}x{1})", new object[2]
          {
            (object) width,
            (object) height
          });
      }
      this.m_TargetTexture.Create();
      if (!flag)
        return;
      this.ClearTargetTexture();
    }

    private float ScaleThatFitsTargetInView(Vector2 targetInPixels, Vector2 viewInPoints)
    {
      Vector2 points = EditorGUIUtility.PixelsToPoints(targetInPixels);
      Vector2 vector2 = new Vector2(viewInPoints.x / points.x, viewInPoints.y / points.y);
      return Mathf.Min(vector2.x, vector2.y);
    }

    private float DefaultScaleForTargetInView(Vector2 targetToFit, Vector2 viewSize)
    {
      float f = this.ScaleThatFitsTargetInView(targetToFit, viewSize);
      if ((double) f > 1.0)
        f = Mathf.Min(this.maxScale * EditorGUIUtility.pixelsPerPoint, (float) Mathf.FloorToInt(f));
      return f;
    }

    private void ConfigureZoomArea()
    {
      this.m_ZoomArea.rect = this.viewInWindow;
      this.m_ZoomArea.hBaseRangeMin = this.targetInContent.xMin;
      this.m_ZoomArea.vBaseRangeMin = this.targetInContent.yMin;
      this.m_ZoomArea.hBaseRangeMax = this.targetInContent.xMax;
      this.m_ZoomArea.vBaseRangeMax = this.targetInContent.yMax;
      ZoomableArea zoomArea1 = this.m_ZoomArea;
      float minScale = this.minScale;
      this.m_ZoomArea.vScaleMin = minScale;
      double num1 = (double) minScale;
      zoomArea1.hScaleMin = (float) num1;
      ZoomableArea zoomArea2 = this.m_ZoomArea;
      float maxScale = this.maxScale;
      this.m_ZoomArea.vScaleMax = maxScale;
      double num2 = (double) maxScale;
      zoomArea2.hScaleMax = (float) num2;
    }

    private void EnforceZoomAreaConstraints()
    {
      Rect shownArea = this.m_ZoomArea.shownArea;
      shownArea.x = (double) shownArea.width <= (double) this.targetInContent.width ? Mathf.Clamp(shownArea.x, this.targetInContent.xMin, this.targetInContent.xMax - shownArea.width) : -0.5f * shownArea.width;
      shownArea.y = (double) shownArea.height <= (double) this.targetInContent.height ? Mathf.Clamp(shownArea.y, this.targetInContent.yMin, this.targetInContent.yMax - shownArea.height) : -0.5f * shownArea.height;
      this.m_ZoomArea.shownArea = shownArea;
    }

    private void OnGUI()
    {
      if (this.position.size * EditorGUIUtility.pixelsPerPoint != this.m_LastWindowPixelSize)
        this.UpdateZoomAreaAndParent();
      this.DoToolbarGUI();
      this.CopyDimensionsToParentView();
      EditorGUIUtility.AddCursorRect(this.viewInWindow, MouseCursor.CustomCursor);
      EventType type1 = Event.current.type;
      if (type1 == EventType.MouseDown && this.viewInWindow.Contains(Event.current.mousePosition))
        this.AllowCursorLockAndHide(true);
      else if (type1 == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        Unsupported.SetAllowCursorLock(false);
      bool flag1 = EditorApplication.isPlaying && !EditorApplication.isPaused;
      this.m_ZoomArea.hSlider = !flag1 && (double) this.m_ZoomArea.shownArea.width < (double) this.targetInContent.width;
      this.m_ZoomArea.vSlider = !flag1 && (double) this.m_ZoomArea.shownArea.height < (double) this.targetInContent.height;
      this.m_ZoomArea.enableMouseInput = !flag1;
      this.ConfigureZoomArea();
      if (flag1)
        GUIUtility.keyboardControl = 0;
      Vector2 mousePosition = Event.current.mousePosition;
      Vector2 gameMousePosition = this.WindowToGameMousePosition(mousePosition);
      GUI.color = Color.white;
      EventType type2 = Event.current.type;
      this.m_ZoomArea.BeginViewGUI();
      switch (type1)
      {
        case EventType.Repaint:
          GUI.Box(this.m_ZoomArea.drawRect, GUIContent.none, GameView.Styles.gameViewBackgroundStyle);
          Vector2 screenPointOffset = GUIUtility.s_EditorScreenPointOffset;
          GUIUtility.s_EditorScreenPointOffset = Vector2.zero;
          SavedGUIState savedGuiState = SavedGUIState.Create();
          this.ConfigureTargetTexture((int) this.targetSize.x, (int) this.targetSize.y);
          if (this.m_ClearInEditMode && !EditorApplication.isPlaying)
            this.ClearTargetTexture();
          int targetDisplay = 0;
          if (ModuleManager.ShouldShowMultiDisplayOption())
            targetDisplay = this.m_TargetDisplay;
          if (this.m_TargetTexture.IsCreated())
          {
            EditorGUIUtility.RenderGameViewCamerasInternal(this.m_TargetTexture, targetDisplay, GUIClip.Unclip(this.viewInWindow), gameMousePosition, this.m_Gizmos);
            savedGuiState.ApplyAndForget();
            GUIUtility.s_EditorScreenPointOffset = screenPointOffset;
            GUI.BeginGroup(this.m_ZoomArea.drawRect);
            GL.sRGBWrite = this.m_CurrentColorSpace == ColorSpace.Linear;
            Graphics.DrawTexture(this.deviceFlippedTargetInView, (Texture) this.m_TargetTexture, new Rect(0.0f, 0.0f, 1f, 1f), 0, 0, 0, 0, GUI.color, EditorGUIUtility.GUITextureBlitColorspaceMaterial);
            GL.sRGBWrite = false;
            GUI.EndGroup();
            goto case EventType.Layout;
          }
          else
            goto case EventType.Layout;
        case EventType.Layout:
        case EventType.Used:
          this.m_ZoomArea.EndViewGUI();
          if (type2 == EventType.ScrollWheel && Event.current.type == EventType.Used)
          {
            EditorApplication.update -= new EditorApplication.CallbackFunction(this.SnapZoomDelayed);
            EditorApplication.update += new EditorApplication.CallbackFunction(this.SnapZoomDelayed);
            GameView.s_LastScrollTime = EditorApplication.timeSinceStartup;
          }
          this.EnforceZoomAreaConstraints();
          if ((bool) ((UnityEngine.Object) this.m_TargetTexture))
          {
            if ((double) this.m_ZoomArea.scale.y < 1.0)
              this.m_TargetTexture.filterMode = UnityEngine.FilterMode.Bilinear;
            else
              this.m_TargetTexture.filterMode = UnityEngine.FilterMode.Point;
          }
          if (this.m_NoCameraWarning && !EditorGUIUtility.IsDisplayReferencedByCameras(this.m_TargetDisplay))
          {
            GUI.Label(this.warningPosition, GUIContent.none, EditorStyles.notificationBackground);
            EditorGUI.DoDropShadowLabel(this.warningPosition, EditorGUIUtility.TempContent(string.Format("{0}\nNo cameras rendering", !ModuleManager.ShouldShowMultiDisplayOption() ? (object) string.Empty : (object) DisplayUtility.GetDisplayNames()[this.m_TargetDisplay].text)), EditorStyles.notificationText, 0.3f);
          }
          if (!this.m_Stats)
            break;
          GameViewGUI.GameViewStatsGUI();
          break;
        default:
          if (WindowLayout.s_MaximizeKey.activated && (!EditorApplication.isPlaying || EditorApplication.isPaused))
            break;
          bool flag2 = this.viewInWindow.Contains(Event.current.mousePosition);
          if (Event.current.rawType == EventType.MouseDown && !flag2)
            break;
          int displayIndex = Event.current.displayIndex;
          Event.current.mousePosition = gameMousePosition;
          Event.current.displayIndex = this.m_TargetDisplay;
          EditorGUIUtility.QueueGameViewInputEvent(Event.current);
          bool flag3 = true;
          if (Event.current.rawType == EventType.MouseUp && !flag2)
            flag3 = false;
          if (type1 == EventType.ExecuteCommand || type1 == EventType.ValidateCommand)
            flag3 = false;
          if (flag3)
            Event.current.Use();
          else
            Event.current.mousePosition = mousePosition;
          Event.current.displayIndex = displayIndex;
          goto case EventType.Layout;
      }
    }

    internal static class Styles
    {
      public static GUIContent gizmosContent = EditorGUIUtility.TextContent("Gizmos");
      public static GUIContent zoomSliderContent = EditorGUIUtility.TextContent("Scale|Size of the game view on the screen.");
      public static GUIContent maximizeOnPlayContent = EditorGUIUtility.TextContent("Maximize On Play");
      public static GUIContent muteContent = EditorGUIUtility.TextContent("Mute Audio");
      public static GUIContent statsContent = EditorGUIUtility.TextContent("Stats");
      public static GUIContent frameDebuggerOnContent = EditorGUIUtility.TextContent("Frame Debugger On");
      public static GUIContent loadRenderDocContent = EditorGUIUtility.TextContent("Load RenderDoc");
      public static GUIContent noCameraWarningContextMenuContent = EditorGUIUtility.TextContent("Warn if No Cameras Rendering");
      public static GUIContent clearEveryFrameContextMenuContent = EditorGUIUtility.TextContent("Clear Every Frame in Edit Mode");
      public static GUIContent lowResAspectRatiosContextMenuContent = EditorGUIUtility.TextContent("Low Resolution Aspect Ratios");
      public static GUIStyle gameViewBackgroundStyle = (GUIStyle) "GameViewBackground";
      public static GUIStyle gizmoButtonStyle = (GUIStyle) "GV Gizmo DropDown";
      public static GUIContent renderdocContent = EditorGUIUtility.IconContent("renderdoc", "Capture|Capture the current view and open in RenderDoc.");
    }
  }
}
