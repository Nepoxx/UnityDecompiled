// Decompiled with JetBrains decompiler
// Type: UnityEditor.CameraEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor.AnimatedValues;
using UnityEditor.Modules;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Camera))]
  internal class CameraEditor : Editor
  {
    private static readonly GUIContent[] kCameraRenderPaths = new GUIContent[5]{ new GUIContent("Use Graphics Settings"), new GUIContent("Forward"), new GUIContent("Deferred"), new GUIContent("Legacy Vertex Lit"), new GUIContent("Legacy Deferred (light prepass)") };
    private static readonly int[] kCameraRenderPathValues = new int[5]{ -1, 1, 3, 0, 2 };
    private static readonly GUIContent[] kTargetEyes = new GUIContent[4]{ new GUIContent("Both"), new GUIContent("Left"), new GUIContent("Right"), new GUIContent("None (Main Display)") };
    private static readonly int[] kTargetEyeValues = new int[4]{ 3, 1, 2, 0 };
    private static readonly Color kGizmoCamera = new Color(0.9137255f, 0.9137255f, 0.9137255f, 0.5019608f);
    private readonly AnimBool m_ShowBGColorOptions = new AnimBool();
    private readonly AnimBool m_ShowOrthoOptions = new AnimBool();
    private readonly AnimBool m_ShowTargetEyeOption = new AnimBool();
    private readonly GUIContent m_ViewportLabel = EditorGUIUtility.TextContent("Viewport Rect|Four values that indicate where on the screen this camera view will be drawn. Measured in Viewport Coordinates (values 0–1).");
    private bool m_CommandBuffersShown = true;
    private SerializedProperty m_ClearFlags;
    private SerializedProperty m_BackgroundColor;
    private SerializedProperty m_NormalizedViewPortRect;
    private SerializedProperty m_FieldOfView;
    private SerializedProperty m_Orthographic;
    private SerializedProperty m_OrthographicSize;
    private SerializedProperty m_Depth;
    private SerializedProperty m_CullingMask;
    private SerializedProperty m_RenderingPath;
    private SerializedProperty m_OcclusionCulling;
    private SerializedProperty m_TargetTexture;
    private SerializedProperty m_HDR;
    private SerializedProperty m_AllowMSAA;
    private SerializedProperty m_AllowDynamicResolution;
    private SerializedProperty[] m_NearAndFarClippingPlanes;
    private SerializedProperty m_StereoConvergence;
    private SerializedProperty m_StereoSeparation;
    private SerializedProperty m_TargetDisplay;
    private SerializedProperty m_TargetEye;
    private Camera m_PreviewCamera;
    private const float kPreviewNormalizedSize = 0.2f;

    private Camera camera
    {
      get
      {
        return this.target as Camera;
      }
    }

    private static bool IsDeferredRenderingPath(RenderingPath rp)
    {
      return rp == RenderingPath.DeferredLighting || rp == RenderingPath.DeferredShading;
    }

    private bool wantDeferredRendering
    {
      get
      {
        return CameraEditor.IsDeferredRenderingPath(this.camera.renderingPath) || this.camera.renderingPath == RenderingPath.UsePlayerSettings && CameraEditor.IsDeferredRenderingPath(EditorGraphicsSettings.GetCurrentTierSettings().renderingPath);
      }
    }

    private Camera previewCamera
    {
      get
      {
        if ((UnityEngine.Object) this.m_PreviewCamera == (UnityEngine.Object) null)
          this.m_PreviewCamera = EditorUtility.CreateGameObjectWithHideFlags("Preview Camera", HideFlags.HideAndDontSave, typeof (Camera), typeof (Skybox)).GetComponent<Camera>();
        this.m_PreviewCamera.enabled = false;
        return this.m_PreviewCamera;
      }
    }

    public void OnEnable()
    {
      this.m_ClearFlags = this.serializedObject.FindProperty("m_ClearFlags");
      this.m_BackgroundColor = this.serializedObject.FindProperty("m_BackGroundColor");
      this.m_NormalizedViewPortRect = this.serializedObject.FindProperty("m_NormalizedViewPortRect");
      this.m_NearAndFarClippingPlanes = new SerializedProperty[2]
      {
        this.serializedObject.FindProperty("near clip plane"),
        this.serializedObject.FindProperty("far clip plane")
      };
      this.m_FieldOfView = this.serializedObject.FindProperty("field of view");
      this.m_Orthographic = this.serializedObject.FindProperty("orthographic");
      this.m_OrthographicSize = this.serializedObject.FindProperty("orthographic size");
      this.m_Depth = this.serializedObject.FindProperty("m_Depth");
      this.m_CullingMask = this.serializedObject.FindProperty("m_CullingMask");
      this.m_RenderingPath = this.serializedObject.FindProperty("m_RenderingPath");
      this.m_OcclusionCulling = this.serializedObject.FindProperty("m_OcclusionCulling");
      this.m_TargetTexture = this.serializedObject.FindProperty("m_TargetTexture");
      this.m_HDR = this.serializedObject.FindProperty("m_HDR");
      this.m_AllowMSAA = this.serializedObject.FindProperty("m_AllowMSAA");
      this.m_AllowDynamicResolution = this.serializedObject.FindProperty("m_AllowDynamicResolution");
      this.m_StereoConvergence = this.serializedObject.FindProperty("m_StereoConvergence");
      this.m_StereoSeparation = this.serializedObject.FindProperty("m_StereoSeparation");
      this.m_TargetDisplay = this.serializedObject.FindProperty("m_TargetDisplay");
      this.m_TargetEye = this.serializedObject.FindProperty("m_TargetEye");
      Camera target = (Camera) this.target;
      this.m_ShowBGColorOptions.value = !this.m_ClearFlags.hasMultipleDifferentValues && (target.clearFlags == CameraClearFlags.Color || target.clearFlags == CameraClearFlags.Skybox);
      this.m_ShowOrthoOptions.value = target.orthographic;
      this.m_ShowTargetEyeOption.value = this.m_TargetEye.intValue != 3 || PlayerSettings.virtualRealitySupported;
      this.m_ShowBGColorOptions.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowOrthoOptions.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowTargetEyeOption.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    internal void OnDisable()
    {
      this.m_ShowBGColorOptions.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowOrthoOptions.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowTargetEyeOption.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public void OnDestroy()
    {
      if (!((UnityEngine.Object) this.m_PreviewCamera != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_PreviewCamera.gameObject, true);
    }

    private void DepthTextureModeGUI()
    {
      if (this.targets.Length != 1)
        return;
      Camera target = this.target as Camera;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null || target.depthTextureMode == DepthTextureMode.None)
        return;
      List<string> stringList = new List<string>();
      if ((target.depthTextureMode & DepthTextureMode.Depth) != DepthTextureMode.None)
        stringList.Add("Depth");
      if ((target.depthTextureMode & DepthTextureMode.DepthNormals) != DepthTextureMode.None)
        stringList.Add("DepthNormals");
      if ((target.depthTextureMode & DepthTextureMode.MotionVectors) != DepthTextureMode.None)
        stringList.Add("MotionVectors");
      if (stringList.Count == 0)
        return;
      StringBuilder stringBuilder = new StringBuilder("Info: renders ");
      for (int index = 0; index < stringList.Count; ++index)
      {
        if (index != 0)
          stringBuilder.Append(" & ");
        stringBuilder.Append(stringList[index]);
      }
      stringBuilder.Append(stringList.Count <= 1 ? " texture" : " textures");
      EditorGUILayout.HelpBox(stringBuilder.ToString(), MessageType.None, true);
    }

    private static Rect GetRemoveButtonRect(Rect r)
    {
      Vector2 vector2 = CameraEditor.Styles.invisibleButton.CalcSize(CameraEditor.Styles.iconRemove);
      return new Rect(r.xMax - vector2.x, r.y + (float) (int) ((double) r.height / 2.0 - (double) vector2.y / 2.0), vector2.x, vector2.y);
    }

    [DrawGizmo(GizmoType.NonSelected)]
    private static void DrawCameraBound(Camera camera, GizmoType gizmoType)
    {
      SceneView drawingSceneView = SceneView.currentDrawingSceneView;
      if (!((UnityEngine.Object) drawingSceneView != (UnityEngine.Object) null) || !drawingSceneView.in2DMode || (!((UnityEngine.Object) camera == (UnityEngine.Object) Camera.main) || !camera.orthographic))
        return;
      CameraEditor.RenderGizmo(camera);
    }

    private void CommandBufferGUI()
    {
      if (this.targets.Length != 1)
        return;
      Camera target = this.target as Camera;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return;
      int commandBufferCount = target.commandBufferCount;
      if (commandBufferCount == 0)
        return;
      this.m_CommandBuffersShown = GUILayout.Toggle(this.m_CommandBuffersShown, GUIContent.Temp(commandBufferCount.ToString() + " command buffers"), EditorStyles.foldout, new GUILayoutOption[0]);
      if (!this.m_CommandBuffersShown)
        return;
      ++EditorGUI.indentLevel;
      foreach (CameraEvent evt in (CameraEvent[]) Enum.GetValues(typeof (CameraEvent)))
      {
        foreach (CommandBuffer commandBuffer in target.GetCommandBuffers(evt))
        {
          using (new GUILayout.HorizontalScope(new GUILayoutOption[0]))
          {
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.miniLabel);
            rect.xMin += EditorGUI.indent;
            Rect removeButtonRect = CameraEditor.GetRemoveButtonRect(rect);
            rect.xMax = removeButtonRect.x;
            GUI.Label(rect, string.Format("{0}: {1} ({2})", (object) evt, (object) commandBuffer.name, (object) EditorUtility.FormatBytes(commandBuffer.sizeInBytes)), EditorStyles.miniLabel);
            if (GUI.Button(removeButtonRect, CameraEditor.Styles.iconRemove, CameraEditor.Styles.invisibleButton))
            {
              target.RemoveCommandBuffer(evt, commandBuffer);
              SceneView.RepaintAll();
              GameView.RepaintAll();
              GUIUtility.ExitGUI();
            }
          }
        }
      }
      using (new GUILayout.HorizontalScope(new GUILayoutOption[0]))
      {
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Remove all", EditorStyles.miniButton, new GUILayoutOption[0]))
        {
          target.RemoveAllCommandBuffers();
          SceneView.RepaintAll();
          GameView.RepaintAll();
        }
      }
      --EditorGUI.indentLevel;
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      Camera target = (Camera) this.target;
      this.m_ShowBGColorOptions.target = !this.m_ClearFlags.hasMultipleDifferentValues && (target.clearFlags == CameraClearFlags.Color || target.clearFlags == CameraClearFlags.Skybox);
      this.m_ShowOrthoOptions.target = !this.m_Orthographic.hasMultipleDifferentValues && target.orthographic;
      this.m_ShowTargetEyeOption.target = this.m_TargetEye.intValue != 3 || PlayerSettings.virtualRealitySupported;
      EditorGUILayout.PropertyField(this.m_ClearFlags, EditorGUIUtility.TextContent("Clear Flags|What to display in empty areas of this Camera's view.\n\nChoose Skybox to display a skybox in empty areas, defaulting to a background color if no skybox is found.\n\nChoose Solid Color to display a background color in empty areas.\n\nChoose Depth Only to display nothing in empty areas.\n\nChoose Don't Clear to display whatever was displayed in the previous frame in empty areas."), new GUILayoutOption[0]);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowBGColorOptions.faded))
        EditorGUILayout.PropertyField(this.m_BackgroundColor, EditorGUIUtility.TextContent("Background|The Camera clears the screen to this color before rendering."), new GUILayoutOption[0]);
      EditorGUILayout.EndFadeGroup();
      EditorGUILayout.PropertyField(this.m_CullingMask);
      EditorGUILayout.Space();
      CameraEditor.ProjectionType projectionType1 = !this.m_Orthographic.boolValue ? CameraEditor.ProjectionType.Perspective : CameraEditor.ProjectionType.Orthographic;
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = this.m_Orthographic.hasMultipleDifferentValues;
      CameraEditor.ProjectionType projectionType2 = (CameraEditor.ProjectionType) EditorGUILayout.EnumPopup(EditorGUIUtility.TextContent("Projection|How the Camera renders perspective.\n\nChoose Perspective to render objects with perspective.\n\nChoose Orthographic to render objects uniformly, with no sense of perspective."), (Enum) projectionType1, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        this.m_Orthographic.boolValue = projectionType2 == CameraEditor.ProjectionType.Orthographic;
      if (!this.m_Orthographic.hasMultipleDifferentValues)
      {
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowOrthoOptions.faded))
          EditorGUILayout.PropertyField(this.m_OrthographicSize, new GUIContent("Size"), new GUILayoutOption[0]);
        EditorGUILayout.EndFadeGroup();
        if (EditorGUILayout.BeginFadeGroup(1f - this.m_ShowOrthoOptions.faded))
          EditorGUILayout.Slider(this.m_FieldOfView, 1f, 179f, EditorGUIUtility.TextContent("Field of View|The width of the Camera’s view angle, measured in degrees along the local Y axis."), new GUILayoutOption[0]);
        EditorGUILayout.EndFadeGroup();
      }
      EditorGUILayout.PropertiesField(EditorGUI.s_ClipingPlanesLabel, this.m_NearAndFarClippingPlanes, EditorGUI.s_NearAndFarLabels, 35f);
      EditorGUILayout.PropertyField(this.m_NormalizedViewPortRect, this.m_ViewportLabel, new GUILayoutOption[0]);
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_Depth);
      EditorGUILayout.IntPopup(this.m_RenderingPath, CameraEditor.kCameraRenderPaths, CameraEditor.kCameraRenderPathValues, EditorGUIUtility.TempContent("Rendering Path"), new GUILayoutOption[0]);
      if (this.m_ShowOrthoOptions.target && this.wantDeferredRendering)
        EditorGUILayout.HelpBox("Deferred rendering does not work with Orthographic camera, will use Forward.", MessageType.Warning, true);
      EditorGUILayout.PropertyField(this.m_TargetTexture);
      if (!this.m_TargetTexture.hasMultipleDifferentValues)
      {
        RenderTexture objectReferenceValue = this.m_TargetTexture.objectReferenceValue as RenderTexture;
        if ((bool) ((UnityEngine.Object) objectReferenceValue) && objectReferenceValue.antiAliasing > 1 && this.wantDeferredRendering)
          EditorGUILayout.HelpBox("Manual MSAA target set with deferred rendering. This will lead to undefined behavior.", MessageType.Warning, true);
      }
      EditorGUILayout.PropertyField(this.m_OcclusionCulling);
      EditorGUILayout.PropertyField(this.m_HDR, EditorGUIUtility.TempContent("Allow HDR"), new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_AllowMSAA);
      EditorGUILayout.PropertyField(this.m_AllowDynamicResolution);
      this.DisplayCameraWarnings();
      if (PlayerSettings.virtualRealitySupported)
      {
        EditorGUILayout.PropertyField(this.m_StereoSeparation);
        EditorGUILayout.PropertyField(this.m_StereoConvergence);
      }
      if (ModuleManager.ShouldShowMultiDisplayOption())
      {
        int intValue = this.m_TargetDisplay.intValue;
        EditorGUILayout.Space();
        EditorGUILayout.IntPopup(this.m_TargetDisplay, DisplayUtility.GetDisplayNames(), DisplayUtility.GetDisplayIndices(), EditorGUIUtility.TempContent("Target Display"), new GUILayoutOption[0]);
        if (intValue != this.m_TargetDisplay.intValue)
          GameView.RepaintAll();
      }
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowTargetEyeOption.faded))
        EditorGUILayout.IntPopup(this.m_TargetEye, CameraEditor.kTargetEyes, CameraEditor.kTargetEyeValues, EditorGUIUtility.TempContent("Target Eye"), new GUILayoutOption[0]);
      EditorGUILayout.EndFadeGroup();
      this.DepthTextureModeGUI();
      this.CommandBufferGUI();
      this.serializedObject.ApplyModifiedProperties();
    }

    private void DisplayCameraWarnings()
    {
      Camera target = this.target as Camera;
      if (!((UnityEngine.Object) target != (UnityEngine.Object) null))
        return;
      string[] cameraBufferWarnings = target.GetCameraBufferWarnings();
      if (cameraBufferWarnings.Length > 0)
        EditorGUILayout.HelpBox(string.Join("\n\n", cameraBufferWarnings), MessageType.Warning, true);
    }

    public void OnOverlayGUI(UnityEngine.Object target, SceneView sceneView)
    {
      if (target == (UnityEngine.Object) null)
        return;
      Camera camera = (Camera) target;
      Vector2 gameViewTargetSize = GameView.GetMainGameViewTargetSize();
      if ((double) gameViewTargetSize.x < 0.0)
      {
        gameViewTargetSize.x = sceneView.position.width;
        gameViewTargetSize.y = sceneView.position.height;
      }
      Rect rect1 = camera.rect;
      gameViewTargetSize.x *= Mathf.Max(rect1.width, 0.0f);
      gameViewTargetSize.y *= Mathf.Max(rect1.height, 0.0f);
      if ((double) gameViewTargetSize.x <= 0.0 || (double) gameViewTargetSize.y <= 0.0)
        return;
      float num = gameViewTargetSize.x / gameViewTargetSize.y;
      gameViewTargetSize.y = 0.2f * sceneView.position.height;
      gameViewTargetSize.x = gameViewTargetSize.y * num;
      if ((double) gameViewTargetSize.y > (double) sceneView.position.height * 0.5)
      {
        gameViewTargetSize.y = sceneView.position.height * 0.5f;
        gameViewTargetSize.x = gameViewTargetSize.y * num;
      }
      if ((double) gameViewTargetSize.x > (double) sceneView.position.width * 0.5)
      {
        gameViewTargetSize.x = sceneView.position.width * 0.5f;
        gameViewTargetSize.y = gameViewTargetSize.x / num;
      }
      Rect rect2 = GUILayoutUtility.GetRect(gameViewTargetSize.x, gameViewTargetSize.y);
      if (Event.current.type != EventType.Repaint)
        return;
      this.previewCamera.CopyFrom(camera);
      Skybox component1 = this.previewCamera.GetComponent<Skybox>();
      if ((bool) ((UnityEngine.Object) component1))
      {
        Skybox component2 = camera.GetComponent<Skybox>();
        if ((bool) ((UnityEngine.Object) component2) && component2.enabled)
        {
          component1.enabled = true;
          component1.material = component2.material;
        }
        else
          component1.enabled = false;
      }
      this.previewCamera.targetTexture = RenderTexture.GetTemporary((int) gameViewTargetSize.x, (int) gameViewTargetSize.y, 24, !this.previewCamera.allowHDR ? RenderTextureFormat.ARGB32 : RenderTextureFormat.ARGBHalf);
      Handles.EmitGUIGeometryForCamera(camera, this.previewCamera);
      this.previewCamera.Render();
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      GUI.DrawTexture(rect2, (Texture) this.previewCamera.targetTexture, ScaleMode.StretchToFill, false);
      GL.sRGBWrite = false;
      RenderTexture.ReleaseTemporary(this.previewCamera.targetTexture);
    }

    [RequiredByNativeCode]
    private static float GetGameViewAspectRatio()
    {
      Vector2 gameViewTargetSize = GameView.GetMainGameViewTargetSize();
      if ((double) gameViewTargetSize.x < 0.0)
      {
        gameViewTargetSize.x = (float) Screen.width;
        gameViewTargetSize.y = (float) Screen.height;
      }
      return gameViewTargetSize.x / gameViewTargetSize.y;
    }

    private static float GetFrustumAspectRatio(Camera camera)
    {
      Rect rect = camera.rect;
      if ((double) rect.width <= 0.0 || (double) rect.height <= 0.0)
        return -1f;
      return CameraEditor.GetGameViewAspectRatio() * (rect.width / rect.height);
    }

    private static bool GetFrustum(Camera camera, Vector3[] near, Vector3[] far, out float frustumAspect)
    {
      frustumAspect = CameraEditor.GetFrustumAspectRatio(camera);
      if ((double) frustumAspect < 0.0)
        return false;
      if (far != null)
      {
        far[0] = new Vector3(0.0f, 0.0f, camera.farClipPlane);
        far[1] = new Vector3(0.0f, 1f, camera.farClipPlane);
        far[2] = new Vector3(1f, 1f, camera.farClipPlane);
        far[3] = new Vector3(1f, 0.0f, camera.farClipPlane);
        for (int index = 0; index < 4; ++index)
          far[index] = camera.ViewportToWorldPoint(far[index]);
      }
      if (near != null)
      {
        near[0] = new Vector3(0.0f, 0.0f, camera.nearClipPlane);
        near[1] = new Vector3(0.0f, 1f, camera.nearClipPlane);
        near[2] = new Vector3(1f, 1f, camera.nearClipPlane);
        near[3] = new Vector3(1f, 0.0f, camera.nearClipPlane);
        for (int index = 0; index < 4; ++index)
          near[index] = camera.ViewportToWorldPoint(near[index]);
      }
      return true;
    }

    internal static void RenderGizmo(Camera camera)
    {
      Vector3[] near = new Vector3[4];
      Vector3[] far = new Vector3[4];
      float frustumAspect;
      if (!CameraEditor.GetFrustum(camera, near, far, out frustumAspect))
        return;
      Color color = Handles.color;
      Handles.color = CameraEditor.kGizmoCamera;
      for (int index = 0; index < 4; ++index)
      {
        Handles.DrawLine(near[index], near[(index + 1) % 4]);
        Handles.DrawLine(far[index], far[(index + 1) % 4]);
        Handles.DrawLine(near[index], far[index]);
      }
      Handles.color = color;
    }

    private static bool IsViewPortRectValidToRender(Rect normalizedViewPortRect)
    {
      return (double) normalizedViewPortRect.width > 0.0 && (double) normalizedViewPortRect.height > 0.0 && ((double) normalizedViewPortRect.x < 1.0 && (double) normalizedViewPortRect.xMax > 0.0) && ((double) normalizedViewPortRect.y < 1.0 && (double) normalizedViewPortRect.yMax > 0.0);
    }

    public void OnSceneGUI()
    {
      Camera target = (Camera) this.target;
      if (!CameraEditor.IsViewPortRectValidToRender(target.rect))
        return;
      SceneViewOverlay.Window(new GUIContent("Camera Preview"), new SceneViewOverlay.WindowFunction(this.OnOverlayGUI), -100, this.target, SceneViewOverlay.WindowDisplayOption.OneWindowPerTarget);
      Color color = Handles.color;
      Color kGizmoCamera = CameraEditor.kGizmoCamera;
      kGizmoCamera.a *= 2f;
      Handles.color = kGizmoCamera;
      Vector3[] far = new Vector3[4];
      float frustumAspect;
      if (!CameraEditor.GetFrustum(target, (Vector3[]) null, far, out frustumAspect))
        return;
      Vector3 vector3_1 = far[0];
      Vector3 vector3_2 = far[1];
      Vector3 vector3_3 = far[2];
      Vector3 vector3_4 = far[3];
      bool flag = GUI.changed;
      Vector3 vector3_5 = Vector3.Lerp(vector3_1, vector3_3, 0.5f);
      float num = -1f;
      Vector3 vector3_6 = CameraEditor.MidPointPositionSlider(vector3_2, vector3_3, target.transform.up);
      if (!GUI.changed)
        vector3_6 = CameraEditor.MidPointPositionSlider(vector3_1, vector3_4, -target.transform.up);
      if (GUI.changed)
        num = (vector3_6 - vector3_5).magnitude;
      GUI.changed = false;
      Vector3 vector3_7 = CameraEditor.MidPointPositionSlider(vector3_4, vector3_3, target.transform.right);
      if (!GUI.changed)
        vector3_7 = CameraEditor.MidPointPositionSlider(vector3_1, vector3_2, -target.transform.right);
      if (GUI.changed)
        num = (vector3_7 - vector3_5).magnitude / frustumAspect;
      if ((double) num >= 0.0)
      {
        Undo.RecordObject((UnityEngine.Object) target, "Adjust Camera");
        if (target.orthographic)
        {
          target.orthographicSize = num;
        }
        else
        {
          Vector3 vector3_8 = vector3_5 + target.transform.up * num;
          target.fieldOfView = Vector3.Angle(target.transform.forward, vector3_8 - target.transform.position) * 2f;
        }
        flag = true;
      }
      GUI.changed = flag;
      Handles.color = color;
    }

    private static Vector3 MidPointPositionSlider(Vector3 position1, Vector3 position2, Vector3 direction)
    {
      Vector3 position3 = Vector3.Lerp(position1, position2, 0.5f);
      Vector3 position4 = position3;
      Vector3 direction1 = direction;
      double num1 = (double) HandleUtility.GetHandleSize(position3) * 0.0299999993294477;
      // ISSUE: reference to a compiler-generated field
      if (CameraEditor.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CameraEditor.\u003C\u003Ef__mg\u0024cache0 = new Handles.CapFunction(Handles.DotHandleCap);
      }
      // ISSUE: reference to a compiler-generated field
      Handles.CapFunction fMgCache0 = CameraEditor.\u003C\u003Ef__mg\u0024cache0;
      double num2 = 0.0;
      return Handles.Slider(position4, direction1, (float) num1, fMgCache0, (float) num2);
    }

    private class Styles
    {
      public static GUIContent iconRemove = EditorGUIUtility.IconContent("Toolbar Minus", "|Remove command buffer");
      public static GUIStyle invisibleButton = (GUIStyle) "InvisibleButton";
    }

    private enum ProjectionType
    {
      Perspective,
      Orthographic,
    }
  }
}
