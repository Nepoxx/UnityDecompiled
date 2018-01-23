// Decompiled with JetBrains decompiler
// Type: UnityEditor.LookDevView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Look Dev", useTypeNameAsIconName = true)]
  internal class LookDevView : EditorWindow, IHasCustomMenu
  {
    private static readonly Vector2 s_MinWindowSize = new Vector2(300f, 60f);
    private static string m_configAssetPath = "Library/LookDevConfig.asset";
    private static LookDevView.Styles s_Styles = (LookDevView.Styles) null;
    private bool m_IsSaveRegistered = false;
    private LookDevView.PreviewContext[] m_PreviewUtilityContexts = new LookDevView.PreviewContext[2];
    private Rect[] m_PreviewRects = new Rect[3];
    private LookDevEditionContext m_CurrentDragContext = LookDevEditionContext.None;
    private LookDevOperationType m_LookDevOperationType = LookDevOperationType.None;
    private RenderTexture m_FinalCompositionTexture = (RenderTexture) null;
    private LookDevEnvironmentWindow m_LookDevEnvWindow = (LookDevEnvironmentWindow) null;
    private bool m_ShowLookDevEnvWindow = false;
    private bool m_CaptureRD = false;
    private bool[] m_LookDevModeToggles = new bool[5];
    private float m_GizmoThickness = 0.0028f;
    private float m_GizmoThicknessSelected = 0.015f;
    private float m_GizmoCircleRadius = 0.014f;
    private float m_GizmoCircleRadiusSelected = 0.03f;
    private bool m_ForceGizmoRenderSelector = false;
    private LookDevOperationType m_GizmoRenderMode = LookDevOperationType.None;
    private float m_BlendFactorCircleSelectionRadius = 0.03f;
    private float m_BlendFactorCircleRadius = 0.01f;
    private float kLineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    private LookDevConfig m_LookDevConfig = (LookDevConfig) null;
    private LookDevEnvironmentLibrary m_LookDevEnvLibrary = (LookDevEnvironmentLibrary) null;
    [SerializeField]
    private LookDevEnvironmentLibrary m_LookDevUserEnvLibrary = (LookDevEnvironmentLibrary) null;
    private bool m_DisplayDebugGizmo = false;
    private float kReferenceScale = 1080f;
    private int m_hotControlID = 0;
    private float m_DirBias = 0.01f;
    private float m_DirNormalBias = 0.4f;
    private float m_CurrentObjRotationOffset = 0.0f;
    private float m_ObjRotationAcc = 0.0f;
    private float m_EnvRotationAcc = 0.0f;
    private float kDefaultSceneHeight = -500f;
    private CameraControllerStandard m_CameraController = new CameraControllerStandard();
    public static Color32 m_FirstViewGizmoColor;
    public static Color32 m_SecondViewGizmoColor;
    private GUIContent m_RenderdocContent;
    private GUIContent m_SyncLightVertical;
    private GUIContent m_ResetEnvironment;
    private Rect m_DisplayRect;
    private Vector4 m_ScreenRatio;
    private Vector2 m_OnMouseDownOffsetToGizmo;
    private Rect m_ControlWindowRect;

    public LookDevView()
    {
      for (int index = 0; index < 5; ++index)
        this.m_LookDevModeToggles[index] = false;
      this.wantsMouseMove = true;
      this.minSize = LookDevView.s_MinWindowSize;
    }

    public static LookDevView.Styles styles
    {
      get
      {
        if (LookDevView.s_Styles == null)
          LookDevView.s_Styles = new LookDevView.Styles();
        return LookDevView.s_Styles;
      }
    }

    public static void DrawFullScreenQuad(Rect previewRect)
    {
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      GL.PushMatrix();
      GL.LoadOrtho();
      GL.Viewport(previewRect);
      GL.Begin(7);
      GL.TexCoord2(0.0f, 0.0f);
      GL.Vertex3(0.0f, 0.0f, 0.0f);
      GL.TexCoord2(0.0f, 1f);
      GL.Vertex3(0.0f, 1f, 0.0f);
      GL.TexCoord2(1f, 1f);
      GL.Vertex3(1f, 1f, 0.0f);
      GL.TexCoord2(1f, 0.0f);
      GL.Vertex3(1f, 0.0f, 0.0f);
      GL.End();
      GL.PopMatrix();
      GL.sRGBWrite = false;
    }

    internal LookDevView.PreviewContext[] previewUtilityContexts
    {
      get
      {
        return this.m_PreviewUtilityContexts;
      }
    }

    public int hotControl
    {
      get
      {
        return this.m_hotControlID;
      }
    }

    public LookDevConfig config
    {
      get
      {
        return this.m_LookDevConfig;
      }
    }

    public LookDevEnvironmentLibrary envLibrary
    {
      get
      {
        return this.m_LookDevEnvLibrary;
      }
      set
      {
        if ((UnityEngine.Object) value == (UnityEngine.Object) null)
        {
          this.m_LookDevEnvLibrary = ScriptableObject.CreateInstance<LookDevEnvironmentLibrary>();
          this.m_LookDevUserEnvLibrary = (LookDevEnvironmentLibrary) null;
        }
        else if ((UnityEngine.Object) value != (UnityEngine.Object) this.m_LookDevUserEnvLibrary)
        {
          this.m_LookDevUserEnvLibrary = value;
          this.m_LookDevEnvLibrary = UnityEngine.Object.Instantiate<LookDevEnvironmentLibrary>(value);
          this.m_LookDevEnvLibrary.SetLookDevView(this);
        }
        int hdriCount = this.m_LookDevEnvLibrary.hdriCount;
        if (this.m_LookDevConfig.GetIntProperty(LookDevProperty.HDRI, LookDevEditionContext.Left) < hdriCount && this.m_LookDevConfig.GetIntProperty(LookDevProperty.HDRI, LookDevEditionContext.Right) < hdriCount)
          return;
        this.m_LookDevConfig.UpdatePropertyLink(LookDevProperty.HDRI, true);
        this.m_LookDevConfig.UpdateIntProperty(LookDevProperty.HDRI, 0);
      }
    }

    public LookDevEnvironmentLibrary userEnvLibrary
    {
      get
      {
        return this.m_LookDevUserEnvLibrary;
      }
    }

    public void CreateNewLibrary(string assetPath)
    {
      AssetDatabase.CreateAsset((UnityEngine.Object) UnityEngine.Object.Instantiate<LookDevEnvironmentLibrary>(this.envLibrary), assetPath);
      this.envLibrary = AssetDatabase.LoadAssetAtPath(assetPath, typeof (LookDevEnvironmentLibrary)) as LookDevEnvironmentLibrary;
    }

    public static void OpenInLookDevTool(UnityEngine.Object go)
    {
      LookDevView window = EditorWindow.GetWindow<LookDevView>();
      window.m_LookDevConfig.SetCurrentPreviewObject(go as GameObject, LookDevEditionContext.Left);
      window.m_LookDevConfig.SetCurrentPreviewObject(go as GameObject, LookDevEditionContext.Right);
      window.Frame(LookDevEditionContext.Left, false);
      window.Repaint();
    }

    private void Initialize()
    {
      LookDevResources.Initialize();
      this.InitializePreviewUtilities();
      this.LoadLookDevConfig();
      if (this.m_LookDevEnvLibrary.hdriList.Count == 0)
        this.UpdateContextWithCurrentHDRI(LookDevResources.m_DefaultHDRI);
      if (this.m_LookDevEnvWindow != null)
        return;
      this.m_LookDevEnvWindow = new LookDevEnvironmentWindow(this);
    }

    private void InitializePreviewUtilities()
    {
      if (this.m_PreviewUtilityContexts[0] != null)
        return;
      if (QualitySettings.activeColorSpace == ColorSpace.Gamma)
        Debug.LogWarning((object) "Look Dev is designed for linear color space. Currently project is set to gamma color space. This can be changed in player settings.");
      if (EditorGraphicsSettings.GetCurrentTierSettings().renderingPath != RenderingPath.DeferredShading)
        Debug.LogWarning((object) "Look Dev switched rendering mode to deferred shading for display.");
      if (!Camera.main.allowHDR)
        Debug.LogWarning((object) "Look Dev switched HDR mode on for display.");
      for (int index = 0; index < 2; ++index)
        this.m_PreviewUtilityContexts[index] = new LookDevView.PreviewContext();
    }

    private void Cleanup()
    {
      LookDevResources.Cleanup();
      this.m_LookDevConfig.Cleanup();
      for (int index = 0; index < 2; ++index)
      {
        if (this.m_PreviewUtilityContexts[index] != null)
          this.m_PreviewUtilityContexts[index].Cleanup();
        this.m_PreviewUtilityContexts[index] = (LookDevView.PreviewContext) null;
      }
      if (!(bool) ((UnityEngine.Object) this.m_FinalCompositionTexture))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_FinalCompositionTexture);
      this.m_FinalCompositionTexture = (RenderTexture) null;
    }

    public void OnDestroy()
    {
      this.SaveLookDevConfig();
      this.Cleanup();
    }

    private void UpdateRenderTexture(Rect rect)
    {
      int width = (int) rect.width;
      int height = (int) rect.height;
      if ((bool) ((UnityEngine.Object) this.m_FinalCompositionTexture) && this.m_FinalCompositionTexture.width == width && this.m_FinalCompositionTexture.height == height)
        return;
      if ((bool) ((UnityEngine.Object) this.m_FinalCompositionTexture))
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_FinalCompositionTexture);
        this.m_FinalCompositionTexture = (RenderTexture) null;
      }
      this.m_FinalCompositionTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
      this.m_FinalCompositionTexture.hideFlags = HideFlags.HideAndDontSave;
    }

    private void GetRenderableBoundsRecurse(ref Bounds bounds, GameObject go)
    {
      MeshRenderer component1 = go.GetComponent(typeof (MeshRenderer)) as MeshRenderer;
      MeshFilter component2 = go.GetComponent(typeof (MeshFilter)) as MeshFilter;
      if ((bool) ((UnityEngine.Object) component1) && (bool) ((UnityEngine.Object) component2) && (bool) ((UnityEngine.Object) component2.sharedMesh))
      {
        if (bounds.extents == Vector3.zero)
          bounds = component1.bounds;
        else
          bounds.Encapsulate(component1.bounds);
      }
      SkinnedMeshRenderer component3 = go.GetComponent(typeof (SkinnedMeshRenderer)) as SkinnedMeshRenderer;
      if ((bool) ((UnityEngine.Object) component3) && (bool) ((UnityEngine.Object) component3.sharedMesh))
      {
        if (bounds.extents == Vector3.zero)
          bounds = component3.bounds;
        else
          bounds.Encapsulate(component3.bounds);
      }
      SpriteRenderer component4 = go.GetComponent(typeof (SpriteRenderer)) as SpriteRenderer;
      if ((bool) ((UnityEngine.Object) component4) && (bool) ((UnityEngine.Object) component4.sprite))
      {
        if (bounds.extents == Vector3.zero)
          bounds = component4.bounds;
        else
          bounds.Encapsulate(component4.bounds);
      }
      IEnumerator enumerator = go.transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Transform current = (Transform) enumerator.Current;
          this.GetRenderableBoundsRecurse(ref bounds, current.gameObject);
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    private void RenderScene(Rect previewRect, LookDevContext lookDevContext, LookDevView.PreviewContext previewUtilityContext, GameObject[] currentObjectByPasses, CameraState originalCameraState, bool secondView)
    {
      bool flag = !this.m_LookDevConfig.enableShadowCubemap || this.m_LookDevConfig.enableShadowCubemap && lookDevContext.shadingMode != -1 && lookDevContext.shadingMode != 2;
      previewUtilityContext.m_PreviewResult[2] = !flag ? this.RenderScene(previewRect, lookDevContext, previewUtilityContext, currentObjectByPasses[2], originalCameraState, (CubemapInfo) null, LookDevView.PreviewContext.PreviewContextPass.kShadow, secondView) : (Texture) Texture2D.whiteTexture;
      CubemapInfo hdri = this.m_LookDevEnvLibrary.hdriList[lookDevContext.currentHDRIIndex];
      previewUtilityContext.m_PreviewResult[0] = this.RenderScene(previewRect, lookDevContext, previewUtilityContext, currentObjectByPasses[0], originalCameraState, hdri, LookDevView.PreviewContext.PreviewContextPass.kView, secondView);
      previewUtilityContext.m_PreviewResult[1] = this.RenderScene(previewRect, lookDevContext, previewUtilityContext, currentObjectByPasses[1], originalCameraState, hdri.cubemapShadowInfo, LookDevView.PreviewContext.PreviewContextPass.kViewWithShadow, secondView);
    }

    private Texture RenderScene(Rect previewRect, LookDevContext lookDevContext, LookDevView.PreviewContext previewUtilityContext, GameObject currentObject, CameraState originalCameraState, CubemapInfo cubemapInfo, LookDevView.PreviewContext.PreviewContextPass contextPass, bool secondView)
    {
      PreviewRenderUtility previewRenderUtility = previewUtilityContext.m_PreviewUtility[(int) contextPass];
      LookDevView.PreviewContextCB previewContextCb = previewUtilityContext.m_PreviewCB[(int) contextPass];
      UnityEngine.Rendering.DefaultReflectionMode defaultReflectionMode = RenderSettings.defaultReflectionMode;
      AmbientMode ambientMode = RenderSettings.ambientMode;
      Cubemap customReflection = RenderSettings.customReflection;
      Material skybox = RenderSettings.skybox;
      float ambientIntensity = RenderSettings.ambientIntensity;
      SphericalHarmonicsL2 ambientProbe = RenderSettings.ambientProbe;
      float reflectionIntensity = RenderSettings.reflectionIntensity;
      previewRenderUtility.BeginPreview(previewRect, LookDevView.styles.sBigTitleInnerStyle);
      bool flag1 = contextPass == LookDevView.PreviewContext.PreviewContextPass.kShadow;
      DrawCameraMode shadingMode = (DrawCameraMode) lookDevContext.shadingMode;
      bool flag2 = shadingMode != DrawCameraMode.Normal && shadingMode != DrawCameraMode.TexturedWire;
      float shadowDistance = QualitySettings.shadowDistance;
      Vector3 shadowCascade4Split = QualitySettings.shadowCascade4Split;
      float angleOffset = this.m_LookDevEnvLibrary.hdriList[lookDevContext.currentHDRIIndex].angleOffset;
      float num1 = (float) -((double) lookDevContext.envRotation + (double) angleOffset);
      CameraState cameraState = originalCameraState.Clone();
      Vector3 eulerAngles = cameraState.rotation.value.eulerAngles;
      cameraState.rotation.value = Quaternion.Euler(eulerAngles + new Vector3(0.0f, num1, 0.0f));
      cameraState.pivot.value = new Vector3(0.0f, this.kDefaultSceneHeight, 0.0f);
      cameraState.UpdateCamera(previewRenderUtility.camera);
      previewRenderUtility.camera.renderingPath = RenderingPath.DeferredShading;
      previewRenderUtility.camera.clearFlags = !flag1 ? CameraClearFlags.Skybox : CameraClearFlags.Color;
      previewRenderUtility.camera.backgroundColor = Color.white;
      previewRenderUtility.camera.allowHDR = true;
      for (int index = 0; index < 2; ++index)
      {
        previewRenderUtility.lights[index].enabled = false;
        previewRenderUtility.lights[index].intensity = 0.0f;
        previewRenderUtility.lights[index].shadows = LightShadows.None;
      }
      if ((UnityEngine.Object) currentObject != (UnityEngine.Object) null && flag1 && (this.m_LookDevConfig.enableShadowCubemap && !flag2))
      {
        Bounds bounds = new Bounds(currentObject.transform.position, Vector3.zero);
        this.GetRenderableBoundsRecurse(ref bounds, currentObject);
        float num2 = Mathf.Max(bounds.max.x, Mathf.Max(bounds.max.y, bounds.max.z));
        float num3 = (double) this.m_LookDevConfig.shadowDistance <= 0.0 ? 25f * num2 : this.m_LookDevConfig.shadowDistance;
        float num4 = Mathf.Min(num2 * 2f, 20f) / num3;
        QualitySettings.shadowDistance = num3;
        QualitySettings.shadowCascade4Split = new Vector3(Mathf.Clamp(num4, 0.0f, 1f), Mathf.Clamp(num4 * 2f, 0.0f, 1f), Mathf.Clamp(num4 * 6f, 0.0f, 1f));
        ShadowInfo shadowInfo = this.m_LookDevEnvLibrary.hdriList[lookDevContext.currentHDRIIndex].shadowInfo;
        previewRenderUtility.lights[0].intensity = 1f;
        previewRenderUtility.lights[0].color = Color.white;
        previewRenderUtility.lights[0].shadows = LightShadows.Soft;
        previewRenderUtility.lights[0].shadowBias = this.m_DirBias;
        previewRenderUtility.lights[0].shadowNormalBias = this.m_DirNormalBias;
        previewRenderUtility.lights[0].transform.rotation = Quaternion.Euler(shadowInfo.latitude, shadowInfo.longitude, 0.0f);
        previewContextCb.m_patchGBufferCB.Clear();
        RenderTargetIdentifier[] colors1 = new RenderTargetIdentifier[2]{ (RenderTargetIdentifier) BuiltinRenderTextureType.GBuffer0, (RenderTargetIdentifier) BuiltinRenderTextureType.GBuffer1 };
        previewContextCb.m_patchGBufferCB.SetRenderTarget(colors1, (RenderTargetIdentifier) BuiltinRenderTextureType.CameraTarget);
        previewContextCb.m_patchGBufferCB.DrawMesh(LookDevResources.m_ScreenQuadMesh, Matrix4x4.identity, LookDevResources.m_GBufferPatchMaterial);
        previewRenderUtility.camera.AddCommandBuffer(CameraEvent.AfterGBuffer, previewContextCb.m_patchGBufferCB);
        if (this.m_LookDevConfig.showBalls)
        {
          previewContextCb.m_drawBallCB.Clear();
          RenderTargetIdentifier[] colors2 = new RenderTargetIdentifier[1]{ (RenderTargetIdentifier) BuiltinRenderTextureType.CameraTarget };
          previewContextCb.m_drawBallCB.SetRenderTarget(colors2, (RenderTargetIdentifier) BuiltinRenderTextureType.CameraTarget);
          previewContextCb.m_drawBallPB.SetVector("_WindowsSize", new Vector4((float) previewRenderUtility.camera.pixelWidth, (float) previewRenderUtility.camera.pixelHeight, !secondView ? 0.0f : 1f, 0.0f));
          previewContextCb.m_drawBallCB.DrawMesh(LookDevResources.m_ScreenQuadMesh, Matrix4x4.identity, LookDevResources.m_DrawBallsMaterial, 0, 1, previewContextCb.m_drawBallPB);
          previewRenderUtility.camera.AddCommandBuffer(CameraEvent.AfterLighting, previewContextCb.m_drawBallCB);
        }
      }
      previewRenderUtility.ambientColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
      RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Custom;
      Cubemap cubemap = cubemapInfo == null ? (Cubemap) null : cubemapInfo.cubemap;
      LookDevResources.m_SkyboxMaterial.SetTexture("_Tex", (Texture) cubemap);
      LookDevResources.m_SkyboxMaterial.SetFloat("_Exposure", 1f);
      RenderSettings.customReflection = cubemap;
      if (cubemapInfo != null && !cubemapInfo.alreadyComputed && !flag1)
      {
        RenderSettings.ambientMode = AmbientMode.Skybox;
        RenderSettings.skybox = LookDevResources.m_SkyboxMaterial;
        DynamicGI.UpdateEnvironment();
        cubemapInfo.ambientProbe = RenderSettings.ambientProbe;
        RenderSettings.skybox = skybox;
        cubemapInfo.alreadyComputed = true;
      }
      RenderSettings.ambientProbe = cubemapInfo == null ? LookDevResources.m_ZeroAmbientProbe : cubemapInfo.ambientProbe;
      RenderSettings.skybox = LookDevResources.m_SkyboxMaterial;
      RenderSettings.ambientIntensity = 1f;
      RenderSettings.ambientMode = AmbientMode.Skybox;
      RenderSettings.reflectionIntensity = 1f;
      if (contextPass == LookDevView.PreviewContext.PreviewContextPass.kView && this.m_LookDevConfig.showBalls)
      {
        Vector4[] outCoefficients = new Vector4[7];
        this.GetShaderConstantsFromNormalizedSH(RenderSettings.ambientProbe, outCoefficients);
        previewContextCb.m_drawBallCB.Clear();
        RenderTargetIdentifier[] colors = new RenderTargetIdentifier[4]{ (RenderTargetIdentifier) BuiltinRenderTextureType.GBuffer0, (RenderTargetIdentifier) BuiltinRenderTextureType.GBuffer1, (RenderTargetIdentifier) BuiltinRenderTextureType.GBuffer2, (RenderTargetIdentifier) BuiltinRenderTextureType.CameraTarget };
        previewContextCb.m_drawBallCB.SetRenderTarget(colors, (RenderTargetIdentifier) BuiltinRenderTextureType.CameraTarget);
        previewContextCb.m_drawBallPB.SetVector("_SHAr", outCoefficients[0]);
        previewContextCb.m_drawBallPB.SetVector("_SHAg", outCoefficients[1]);
        previewContextCb.m_drawBallPB.SetVector("_SHAb", outCoefficients[2]);
        previewContextCb.m_drawBallPB.SetVector("_SHBr", outCoefficients[3]);
        previewContextCb.m_drawBallPB.SetVector("_SHBg", outCoefficients[4]);
        previewContextCb.m_drawBallPB.SetVector("_SHBb", outCoefficients[5]);
        previewContextCb.m_drawBallPB.SetVector("_SHC", outCoefficients[6]);
        previewContextCb.m_drawBallPB.SetVector("_WindowsSize", new Vector4((float) previewRenderUtility.camera.pixelWidth, (float) previewRenderUtility.camera.pixelHeight, !secondView ? 0.0f : 1f, 0.0f));
        previewContextCb.m_drawBallCB.DrawMesh(LookDevResources.m_ScreenQuadMesh, Matrix4x4.identity, LookDevResources.m_DrawBallsMaterial, 0, 0, previewContextCb.m_drawBallPB);
        previewRenderUtility.camera.AddCommandBuffer(CameraEvent.AfterGBuffer, previewContextCb.m_drawBallCB);
      }
      Vector3 vector3_1 = Vector3.zero;
      Vector3 vector3_2 = Vector3.zero;
      if ((UnityEngine.Object) currentObject != (UnityEngine.Object) null)
      {
        LODGroup component = currentObject.GetComponent(typeof (LODGroup)) as LODGroup;
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.ForceLOD(lookDevContext.lodIndex);
        PreviewRenderUtility.SetEnabledRecursive(currentObject, true);
        vector3_1 = currentObject.transform.eulerAngles;
        vector3_2 = currentObject.transform.localPosition;
        currentObject.transform.position = new Vector3(0.0f, this.kDefaultSceneHeight, 0.0f);
        currentObject.transform.rotation = Quaternion.identity;
        currentObject.transform.Rotate(0.0f, num1, 0.0f);
        currentObject.transform.Translate(-originalCameraState.pivot.value);
        currentObject.transform.Rotate(0.0f, this.m_CurrentObjRotationOffset, 0.0f);
      }
      if (shadingMode == DrawCameraMode.TexturedWire && !flag1)
      {
        Handles.ClearCamera(previewRect, previewRenderUtility.camera);
        Handles.DrawCamera(previewRect, previewRenderUtility.camera, shadingMode);
      }
      else
        previewRenderUtility.Render(true, false);
      if ((UnityEngine.Object) currentObject != (UnityEngine.Object) null)
      {
        currentObject.transform.eulerAngles = vector3_1;
        currentObject.transform.position = vector3_2;
        PreviewRenderUtility.SetEnabledRecursive(currentObject, false);
      }
      if (flag2 && !flag1 && Event.current.type == EventType.Repaint)
      {
        float scaleFactor = previewRenderUtility.GetScaleFactor(previewRect.width, previewRect.height);
        LookDevResources.m_DeferredOverlayMaterial.SetInt("_DisplayMode", (int) (shadingMode - 8));
        Graphics.DrawTexture(new Rect(0.0f, 0.0f, previewRect.width * scaleFactor, previewRect.height * scaleFactor), (Texture) EditorGUIUtility.whiteTexture, LookDevResources.m_DeferredOverlayMaterial);
      }
      if (flag1)
      {
        previewRenderUtility.camera.RemoveCommandBuffer(CameraEvent.AfterGBuffer, previewContextCb.m_patchGBufferCB);
        if (this.m_LookDevConfig.showBalls)
          previewRenderUtility.camera.RemoveCommandBuffer(CameraEvent.AfterLighting, previewContextCb.m_drawBallCB);
      }
      else if (contextPass == LookDevView.PreviewContext.PreviewContextPass.kView && this.m_LookDevConfig.showBalls)
        previewRenderUtility.camera.RemoveCommandBuffer(CameraEvent.AfterGBuffer, previewContextCb.m_drawBallCB);
      QualitySettings.shadowCascade4Split = shadowCascade4Split;
      QualitySettings.shadowDistance = shadowDistance;
      RenderSettings.defaultReflectionMode = defaultReflectionMode;
      RenderSettings.ambientMode = ambientMode;
      RenderSettings.customReflection = customReflection;
      RenderSettings.skybox = skybox;
      RenderSettings.ambientIntensity = ambientIntensity;
      RenderSettings.reflectionIntensity = reflectionIntensity;
      RenderSettings.ambientProbe = ambientProbe;
      return previewRenderUtility.EndPreview();
    }

    public void UpdateLookDevModeToggle(LookDevMode lookDevMode, bool value)
    {
      LookDevMode lookDevMode1 = lookDevMode;
      LookDevMode lookDevMode2;
      if (value)
      {
        this.m_LookDevModeToggles[(int) lookDevMode] = value;
        for (int index = 0; index < 5; ++index)
        {
          if ((LookDevMode) index != lookDevMode)
            this.m_LookDevModeToggles[index] = false;
        }
        lookDevMode2 = lookDevMode;
      }
      else
      {
        for (int index = 0; index < 5; ++index)
        {
          if (this.m_LookDevModeToggles[index])
            lookDevMode1 = (LookDevMode) index;
        }
        this.m_LookDevModeToggles[(int) lookDevMode] = true;
        lookDevMode2 = lookDevMode;
      }
      this.m_LookDevConfig.lookDevMode = lookDevMode2;
      this.Repaint();
    }

    private void OnUndoRedo()
    {
      this.Repaint();
    }

    private void DoAdditionalGUI()
    {
      if (this.m_LookDevConfig.lookDevMode != LookDevMode.SideBySide)
        return;
      int num = 32;
      GUILayout.BeginArea(new Rect((float) (((double) this.m_PreviewRects[2].width - (double) num) / 2.0), (float) (((double) this.m_PreviewRects[2].height - (double) num) / 2.0), (float) num, (float) num));
      EditorGUI.BeginChangeCheck();
      bool sideCameraLinked = this.m_LookDevConfig.sideBySideCameraLinked;
      bool flag = GUILayout.Toggle(sideCameraLinked, this.GetGUIContentLink(sideCameraLinked), LookDevView.styles.sToolBarButton, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_LookDevConfig.sideBySideCameraLinked = flag;
        if (flag)
          (this.m_LookDevConfig.currentEditionContext != LookDevEditionContext.Left ? this.m_LookDevConfig.cameraStateLeft : this.m_LookDevConfig.cameraStateRight).Copy(this.m_LookDevConfig.currentEditionContext != LookDevEditionContext.Left ? this.m_LookDevConfig.cameraStateRight : this.m_LookDevConfig.cameraStateLeft);
      }
      GUILayout.EndArea();
    }

    private GUIStyle GetPropertyLabelStyle(LookDevProperty property)
    {
      if (this.m_LookDevConfig.IsPropertyLinked(property) || this.m_LookDevConfig.lookDevMode == LookDevMode.Single1 || this.m_LookDevConfig.lookDevMode == LookDevMode.Single2)
        return LookDevView.styles.sPropertyLabelStyle[2];
      return LookDevView.styles.sPropertyLabelStyle[this.m_LookDevConfig.currentEditionContextIndex];
    }

    private GUIContent GetGUIContentLink(bool active)
    {
      return !active ? LookDevView.styles.sLinkInactive : LookDevView.styles.sLinkActive;
    }

    private void DoControlWindow()
    {
      if (!this.m_LookDevConfig.showControlWindows)
        return;
      float width1 = 68f;
      float width2 = 150f;
      float width3 = 30f;
      GUILayout.BeginArea(this.m_ControlWindowRect, LookDevView.styles.sBigTitleInnerStyle);
      GUILayout.BeginVertical();
      GUILayout.BeginHorizontal(GUILayout.Height(this.kLineHeight));
      GUILayout.FlexibleSpace();
      EditorGUI.BeginChangeCheck();
      bool flag1 = GUILayout.Toggle(this.m_LookDevModeToggles[0], LookDevView.styles.sSingleMode1, LookDevView.styles.sToolBarButton, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        this.UpdateLookDevModeToggle(LookDevMode.Single1, flag1);
        this.m_LookDevConfig.UpdateFocus(LookDevEditionContext.Left);
        this.Repaint();
      }
      EditorGUI.BeginChangeCheck();
      bool flag2 = GUILayout.Toggle(this.m_LookDevModeToggles[1], LookDevView.styles.sSingleMode2, LookDevView.styles.sToolBarButton, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        this.UpdateLookDevModeToggle(LookDevMode.Single2, flag2);
        this.m_LookDevConfig.UpdateFocus(LookDevEditionContext.Right);
        this.Repaint();
      }
      EditorGUI.BeginChangeCheck();
      bool flag3 = GUILayout.Toggle(this.m_LookDevModeToggles[2], LookDevView.styles.sSideBySideMode, LookDevView.styles.sToolBarButton, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.UpdateLookDevModeToggle(LookDevMode.SideBySide, flag3);
      EditorGUI.BeginChangeCheck();
      bool flag4 = GUILayout.Toggle(this.m_LookDevModeToggles[3], LookDevView.styles.sSplitMode, LookDevView.styles.sToolBarButton, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.UpdateLookDevModeToggle(LookDevMode.Split, flag4);
      EditorGUI.BeginChangeCheck();
      bool flag5 = GUILayout.Toggle(this.m_LookDevModeToggles[4], LookDevView.styles.sZoneMode, LookDevView.styles.sToolBarButton, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.UpdateLookDevModeToggle(LookDevMode.Zone, flag5);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal(GUILayout.Height(this.kLineHeight));
      GUILayout.Label(LookDevViewsWindow.styles.sExposure, this.GetPropertyLabelStyle(LookDevProperty.ExposureValue), new GUILayoutOption[1]
      {
        GUILayout.Width(width1)
      });
      float exposureValue = this.m_LookDevConfig.currentLookDevContext.exposureValue;
      EditorGUI.BeginChangeCheck();
      float num1 = Mathf.Round(this.m_LookDevConfig.exposureRange);
      float num2 = Mathf.Clamp(EditorGUILayout.FloatField(Mathf.Clamp(GUILayout.HorizontalSlider((float) Math.Round((double) exposureValue, (double) exposureValue >= 0.0 ? 2 : 1), (float) -(double) num1, num1, GUILayout.Width(width2)), -num1, num1), GUILayout.Width(width3)), -num1, num1);
      if (EditorGUI.EndChangeCheck())
        this.m_LookDevConfig.UpdateFloatProperty(LookDevProperty.ExposureValue, num2);
      EditorGUI.BeginChangeCheck();
      bool active1 = this.m_LookDevConfig.IsPropertyLinked(LookDevProperty.ExposureValue);
      bool flag6 = GUILayout.Toggle(active1, this.GetGUIContentLink(active1), LookDevView.styles.sToolBarButton, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.m_LookDevConfig.UpdatePropertyLink(LookDevProperty.ExposureValue, flag6);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal(GUILayout.Height(this.kLineHeight));
      using (new EditorGUI.DisabledScope(this.m_LookDevEnvLibrary.hdriList.Count <= 1))
      {
        GUILayout.Label(LookDevViewsWindow.styles.sEnvironment, this.GetPropertyLabelStyle(LookDevProperty.HDRI), new GUILayoutOption[1]
        {
          GUILayout.Width(width1)
        });
        if (this.m_LookDevEnvLibrary.hdriList.Count > 1)
        {
          int max = this.m_LookDevEnvLibrary.hdriList.Count - 1;
          int currentHdriIndex = this.m_LookDevConfig.currentLookDevContext.currentHDRIIndex;
          EditorGUI.BeginChangeCheck();
          int num3 = Mathf.Clamp(EditorGUILayout.IntField((int) GUILayout.HorizontalSlider((float) currentHdriIndex, 0.0f, (float) max, GUILayout.Width(width2)), GUILayout.Width(width3)), 0, max);
          if (EditorGUI.EndChangeCheck())
            this.m_LookDevConfig.UpdateIntProperty(LookDevProperty.HDRI, num3);
        }
        else
        {
          double num3 = (double) GUILayout.HorizontalSlider(0.0f, 0.0f, 0.0f, GUILayout.Width(width2));
          GUILayout.Label(LookDevViewsWindow.styles.sZero, EditorStyles.miniLabel, new GUILayoutOption[1]
          {
            GUILayout.Width(width3)
          });
        }
      }
      EditorGUI.BeginChangeCheck();
      bool active2 = this.m_LookDevConfig.IsPropertyLinked(LookDevProperty.HDRI);
      bool flag7 = GUILayout.Toggle(active2, this.GetGUIContentLink(active2), LookDevView.styles.sToolBarButton, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.m_LookDevConfig.UpdatePropertyLink(LookDevProperty.HDRI, flag7);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal(GUILayout.Height(this.kLineHeight));
      GUILayout.Label(LookDevViewsWindow.styles.sShadingMode, this.GetPropertyLabelStyle(LookDevProperty.ShadingMode), new GUILayoutOption[1]
      {
        GUILayout.Width(width1)
      });
      int shadingMode = this.m_LookDevConfig.currentLookDevContext.shadingMode;
      EditorGUI.BeginChangeCheck();
      int num4 = EditorGUILayout.IntPopup("", shadingMode, LookDevViewsWindow.styles.sShadingModeStrings, LookDevViewsWindow.styles.sShadingModeValues, new GUILayoutOption[1]{ GUILayout.Width((float) ((double) width3 + (double) width2 + 4.0)) });
      if (EditorGUI.EndChangeCheck())
        this.m_LookDevConfig.UpdateIntProperty(LookDevProperty.ShadingMode, num4);
      EditorGUI.BeginChangeCheck();
      bool active3 = this.m_LookDevConfig.IsPropertyLinked(LookDevProperty.ShadingMode);
      bool flag8 = GUILayout.Toggle(active3, this.GetGUIContentLink(active3), LookDevView.styles.sToolBarButton, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.m_LookDevConfig.UpdatePropertyLink(LookDevProperty.ShadingMode, flag8);
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      GUILayout.EndArea();
    }

    public void Update()
    {
      if ((double) this.m_ObjRotationAcc <= 0.0 && (double) this.m_EnvRotationAcc <= 0.0)
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

    private float ComputeLookDevEnvWindowWidth()
    {
      return (float) (250.0 + ((double) this.m_DisplayRect.height - 5.0 >= 146.0 * (double) this.m_LookDevEnvLibrary.hdriCount ? 5.0 : 19.0));
    }

    private float ComputeLookDevEnvWindowHeight()
    {
      return this.m_DisplayRect.height;
    }

    private void DoToolbarGUI()
    {
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      if (EditorGUI.DropdownButton(GUILayoutUtility.GetRect(LookDevSettingsWindow.styles.sTitle, EditorStyles.toolbarDropDown, new GUILayoutOption[1]{ GUILayout.Width(120f) }), LookDevSettingsWindow.styles.sTitle, FocusType.Passive, EditorStyles.toolbarDropDown))
      {
        PopupWindow.Show(GUILayoutUtility.topLevel.GetLast(), (PopupWindowContent) new LookDevSettingsWindow(this));
        GUIUtility.ExitGUI();
      }
      if (EditorGUI.DropdownButton(GUILayoutUtility.GetRect(LookDevViewsWindow.styles.sTitle, EditorStyles.toolbarDropDown, new GUILayoutOption[1]{ GUILayout.Width(120f) }), LookDevViewsWindow.styles.sTitle, FocusType.Passive, EditorStyles.toolbarDropDown))
      {
        PopupWindow.Show(GUILayoutUtility.topLevel.GetLast(), (PopupWindowContent) new LookDevViewsWindow(this));
        GUIUtility.ExitGUI();
      }
      this.m_LookDevConfig.enableShadowCubemap = GUILayout.Toggle(this.m_LookDevConfig.enableShadowCubemap, LookDevSettingsWindow.styles.sEnableShadowIcon, LookDevView.styles.sToolBarButton, new GUILayoutOption[0]);
      this.m_LookDevConfig.rotateObjectMode = GUILayout.Toggle(this.m_LookDevConfig.rotateObjectMode, LookDevSettingsWindow.styles.sEnableObjRotationIcon, LookDevView.styles.sToolBarButton, new GUILayoutOption[0]);
      this.m_LookDevConfig.rotateEnvMode = GUILayout.Toggle(this.m_LookDevConfig.rotateEnvMode, LookDevSettingsWindow.styles.sEnableEnvRotationIcon, LookDevView.styles.sToolBarButton, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      if (this.m_ShowLookDevEnvWindow)
      {
        if (GUILayout.Button(this.m_SyncLightVertical, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        {
          Undo.RecordObject((UnityEngine.Object) this.m_LookDevEnvLibrary, "Synchronize lights");
          int currentHdriIndex = this.m_LookDevConfig.currentLookDevContext.currentHDRIIndex;
          for (int index = 0; index < this.m_LookDevEnvLibrary.hdriList.Count; ++index)
          {
            this.m_LookDevEnvLibrary.hdriList[index].angleOffset += (float) ((double) this.m_LookDevEnvLibrary.hdriList[currentHdriIndex].shadowInfo.longitude + (double) this.m_LookDevEnvLibrary.hdriList[currentHdriIndex].angleOffset - ((double) this.m_LookDevEnvLibrary.hdriList[index].shadowInfo.longitude + (double) this.m_LookDevEnvLibrary.hdriList[index].angleOffset));
            this.m_LookDevEnvLibrary.hdriList[index].angleOffset = (float) (((double) this.m_LookDevEnvLibrary.hdriList[index].angleOffset + 360.0) % 360.0);
          }
          this.m_LookDevEnvLibrary.dirty = true;
          GUIUtility.ExitGUI();
        }
        if (GUILayout.Button(this.m_ResetEnvironment, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        {
          Undo.RecordObject((UnityEngine.Object) this.m_LookDevEnvLibrary, "Reset environment");
          for (int index = 0; index < this.m_LookDevEnvLibrary.hdriList.Count; ++index)
            this.m_LookDevEnvLibrary.hdriList[index].angleOffset = 0.0f;
          this.m_LookDevEnvLibrary.dirty = true;
          GUIUtility.ExitGUI();
        }
      }
      if (RenderDoc.IsLoaded())
      {
        using (new EditorGUI.DisabledScope(!RenderDoc.IsSupported()))
        {
          if (GUILayout.Button(this.m_RenderdocContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
          {
            this.m_CaptureRD = true;
            GUIUtility.ExitGUI();
          }
        }
      }
      if (EditorGUI.DropdownButton(GUILayoutUtility.GetRect(LookDevEnvironmentWindow.styles.sTitle, EditorStyles.iconButton), LookDevEnvironmentWindow.styles.sTitle, FocusType.Passive, EditorStyles.iconButton))
        this.m_ShowLookDevEnvWindow = !this.m_ShowLookDevEnvWindow;
      if (this.m_ShowLookDevEnvWindow)
      {
        Rect GUIRect = new Rect();
        GUIRect.x = 0.0f;
        GUIRect.y = 0.0f;
        GUIRect.width = this.ComputeLookDevEnvWindowWidth();
        GUIRect.height = this.ComputeLookDevEnvWindowHeight();
        Rect rect = new Rect();
        rect.x = this.m_DisplayRect.width - this.ComputeLookDevEnvWindowWidth();
        rect.y = this.m_DisplayRect.y;
        rect.width = this.ComputeLookDevEnvWindowWidth();
        rect.height = this.ComputeLookDevEnvWindowHeight();
        this.m_LookDevEnvWindow.SetRects(rect, GUIRect, this.m_DisplayRect);
        GUILayout.Window(0, rect, new GUI.WindowFunction(this.m_LookDevEnvWindow.OnGUI), "", LookDevView.styles.sBigTitleInnerStyle, new GUILayoutOption[0]);
      }
      GUILayout.EndHorizontal();
    }

    private void UpdateContextWithCurrentHDRI(Cubemap cubemap)
    {
      bool recordUndo = (UnityEngine.Object) cubemap != (UnityEngine.Object) LookDevResources.m_DefaultHDRI;
      int num = this.m_LookDevEnvLibrary.hdriList.FindIndex((Predicate<CubemapInfo>) (x => (UnityEngine.Object) x.cubemap == (UnityEngine.Object) cubemap));
      if (num == -1)
      {
        this.m_LookDevEnvLibrary.InsertHDRI(cubemap);
        num = this.m_LookDevEnvLibrary.hdriList.Count - 1;
      }
      this.m_LookDevConfig.UpdateIntProperty(LookDevProperty.HDRI, num, recordUndo);
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      if (!RenderDoc.IsInstalled() || RenderDoc.IsLoaded())
        return;
      menu.AddItem(new GUIContent("Load RenderDoc"), false, new GenericMenu.MenuFunction(this.LoadRenderDoc));
    }

    public void ResetView()
    {
      Undo.RecordObject((UnityEngine.Object) this.m_LookDevConfig, "Reset View");
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_LookDevConfig);
      this.m_LookDevConfig = ScriptableObject.CreateInstance<LookDevConfig>();
      this.m_LookDevConfig.SetLookDevView(this);
      this.UpdateLookDevModeToggle(this.m_LookDevConfig.lookDevMode, true);
    }

    private void LoadLookDevConfig()
    {
      if ((UnityEngine.Object) this.m_LookDevConfig == (UnityEngine.Object) null)
      {
        LookDevConfig lookDevConfig = new ScriptableObjectSaveLoadHelper<LookDevConfig>("asset", SaveType.Text).Load(LookDevView.m_configAssetPath);
        this.m_LookDevConfig = !((UnityEngine.Object) lookDevConfig == (UnityEngine.Object) null) ? lookDevConfig : ScriptableObject.CreateInstance<LookDevConfig>();
        this.m_IsSaveRegistered = false;
      }
      this.m_LookDevConfig.SetLookDevView(this);
      this.m_LookDevConfig.UpdateCurrentObjectArray();
      if ((UnityEngine.Object) this.m_LookDevEnvLibrary == (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.m_LookDevUserEnvLibrary != (UnityEngine.Object) null)
          this.m_LookDevEnvLibrary = UnityEngine.Object.Instantiate<LookDevEnvironmentLibrary>(this.m_LookDevUserEnvLibrary);
        else
          this.envLibrary = (LookDevEnvironmentLibrary) null;
      }
      this.m_LookDevEnvLibrary.SetLookDevView(this);
    }

    public void SaveLookDevConfig()
    {
      ScriptableObjectSaveLoadHelper<LookDevConfig> objectSaveLoadHelper = new ScriptableObjectSaveLoadHelper<LookDevConfig>("asset", SaveType.Text);
      if (!((UnityEngine.Object) this.m_LookDevConfig != (UnityEngine.Object) null))
        return;
      objectSaveLoadHelper.Save(this.m_LookDevConfig, LookDevView.m_configAssetPath);
    }

    public bool SaveLookDevLibrary()
    {
      if ((UnityEngine.Object) this.m_LookDevUserEnvLibrary != (UnityEngine.Object) null)
      {
        EditorUtility.CopySerialized((UnityEngine.Object) this.m_LookDevEnvLibrary, (UnityEngine.Object) this.m_LookDevUserEnvLibrary);
        EditorUtility.SetDirty((UnityEngine.Object) this.m_LookDevEnvLibrary);
        return true;
      }
      string assetPath = EditorUtility.SaveFilePanelInProject("Save New Environment Library", "New Env Library", "asset", "");
      if (string.IsNullOrEmpty(assetPath))
        return false;
      this.CreateNewLibrary(assetPath);
      return true;
    }

    public void OnEnable()
    {
      this.InitializePreviewUtilities();
      LookDevView.m_FirstViewGizmoColor = !EditorGUIUtility.isProSkin ? new Color32((byte) 0, (byte) 127, byte.MaxValue, byte.MaxValue) : new Color32((byte) 0, (byte) 204, (byte) 204, byte.MaxValue);
      LookDevView.m_SecondViewGizmoColor = !EditorGUIUtility.isProSkin ? new Color32(byte.MaxValue, (byte) 127, (byte) 0, byte.MaxValue) : new Color32(byte.MaxValue, (byte) 107, (byte) 33, byte.MaxValue);
      this.LoadLookDevConfig();
      this.autoRepaintOnSceneChange = true;
      this.titleContent = this.GetLocalizedTitleContent();
      this.m_RenderdocContent = EditorGUIUtility.IconContent("renderdoc", "Capture|Capture the current view and open in RenderDoc");
      this.m_SyncLightVertical = EditorGUIUtility.IconContent("LookDevCenterLight", "Sync|Sync all light vertically with current light position in current selected HDRI");
      this.m_ResetEnvironment = EditorGUIUtility.IconContent("LookDevResetEnv", "Reset|Reset all environment");
      this.UpdateLookDevModeToggle(this.m_LookDevConfig.lookDevMode, true);
      this.m_LookDevConfig.cameraStateCommon.rotation.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_LookDevConfig.cameraStateCommon.pivot.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_LookDevConfig.cameraStateCommon.viewSize.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_LookDevConfig.cameraStateLeft.rotation.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_LookDevConfig.cameraStateLeft.pivot.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_LookDevConfig.cameraStateLeft.viewSize.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_LookDevConfig.cameraStateRight.rotation.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_LookDevConfig.cameraStateRight.pivot.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_LookDevConfig.cameraStateRight.viewSize.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.OnUndoRedo);
      EditorApplication.editorApplicationQuit += new UnityAction(this.OnQuit);
      EditorApplication.update += new EditorApplication.CallbackFunction(this.EditorUpdate);
    }

    public void OnDisable()
    {
      this.SaveLookDevConfig();
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.OnUndoRedo);
      EditorApplication.editorApplicationQuit -= new UnityAction(this.OnQuit);
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.EditorUpdate);
    }

    private void OnQuit()
    {
      this.SaveLookDevConfig();
    }

    private void DelayedSaveLookDevConfig()
    {
      if (this.m_IsSaveRegistered)
        return;
      this.m_IsSaveRegistered = true;
      EditorApplication.delayCall += new EditorApplication.CallbackFunction(this.DoDelayedSaveLookDevConfig);
    }

    private void DoDelayedSaveLookDevConfig()
    {
      this.m_IsSaveRegistered = false;
      this.SaveLookDevConfig();
    }

    private void RenderPreviewSingle()
    {
      int index = this.m_LookDevConfig.lookDevMode != LookDevMode.Single1 ? 1 : 0;
      this.UpdateRenderTexture(this.m_PreviewRects[2]);
      this.RenderScene(this.m_PreviewRects[2], this.m_LookDevConfig.lookDevContexts[index], this.m_PreviewUtilityContexts[index], this.m_LookDevConfig.currentObjectInstances[index], this.m_LookDevConfig.cameraState[index], false);
      this.RenderCompositing(this.m_PreviewRects[2], this.m_PreviewUtilityContexts[index], this.m_PreviewUtilityContexts[index], false);
    }

    private void RenderPreviewSideBySide()
    {
      this.UpdateRenderTexture(this.m_PreviewRects[2]);
      this.RenderScene(this.m_PreviewRects[0], this.m_LookDevConfig.lookDevContexts[0], this.m_PreviewUtilityContexts[0], this.m_LookDevConfig.currentObjectInstances[0], this.m_LookDevConfig.cameraState[0], false);
      this.RenderScene(this.m_PreviewRects[1], this.m_LookDevConfig.lookDevContexts[1], this.m_PreviewUtilityContexts[1], this.m_LookDevConfig.currentObjectInstances[1], this.m_LookDevConfig.cameraState[1], true);
      this.RenderCompositing(this.m_PreviewRects[2], this.m_PreviewUtilityContexts[0], this.m_PreviewUtilityContexts[1], true);
    }

    private void RenderPreviewDualView()
    {
      this.UpdateRenderTexture(this.m_PreviewRects[2]);
      this.RenderScene(this.m_PreviewRects[2], this.m_LookDevConfig.lookDevContexts[0], this.m_PreviewUtilityContexts[0], this.m_LookDevConfig.currentObjectInstances[0], this.m_LookDevConfig.cameraState[0], false);
      this.RenderScene(this.m_PreviewRects[2], this.m_LookDevConfig.lookDevContexts[1], this.m_PreviewUtilityContexts[1], this.m_LookDevConfig.currentObjectInstances[1], this.m_LookDevConfig.cameraState[1], false);
      this.RenderCompositing(this.m_PreviewRects[2], this.m_PreviewUtilityContexts[0], this.m_PreviewUtilityContexts[1], true);
    }

    private void RenderCompositing(Rect previewRect, LookDevView.PreviewContext previewContext0, LookDevView.PreviewContext previewContext1, bool dualView)
    {
      if (this.m_FinalCompositionTexture.width < 1 || this.m_FinalCompositionTexture.height < 1)
        return;
      Vector4 vector4_1 = new Vector4(this.m_LookDevConfig.gizmo.center.x, this.m_LookDevConfig.gizmo.center.y, 0.0f, 0.0f);
      Vector4 vector4_2 = new Vector4(this.m_LookDevConfig.gizmo.point2.x, this.m_LookDevConfig.gizmo.point2.y, 0.0f, 0.0f);
      Vector4 vector4_3 = new Vector4(this.m_GizmoThickness, this.m_GizmoThicknessSelected, 0.0f, 0.0f);
      Vector4 vector4_4 = new Vector4(this.m_GizmoCircleRadius, this.m_GizmoCircleRadiusSelected, 0.0f, 0.0f);
      int index1 = this.m_LookDevConfig.lookDevMode != LookDevMode.Single2 ? 0 : 1;
      int index2 = this.m_LookDevConfig.lookDevMode != LookDevMode.Single1 ? 1 : 0;
      float y = this.m_LookDevConfig.lookDevContexts[index1].shadingMode == -1 || this.m_LookDevConfig.lookDevContexts[index1].shadingMode == 2 ? this.m_LookDevConfig.lookDevContexts[index1].exposureValue : 0.0f;
      float z = this.m_LookDevConfig.lookDevContexts[index2].shadingMode == -1 || this.m_LookDevConfig.lookDevContexts[index2].shadingMode == 2 ? this.m_LookDevConfig.lookDevContexts[index2].exposureValue : 0.0f;
      float x = this.m_CurrentDragContext != LookDevEditionContext.Left ? (this.m_CurrentDragContext != LookDevEditionContext.Right ? 0.0f : -1f) : 1f;
      CubemapInfo hdri1 = this.m_LookDevEnvLibrary.hdriList[this.m_LookDevConfig.lookDevContexts[index1].currentHDRIIndex];
      CubemapInfo hdri2 = this.m_LookDevEnvLibrary.hdriList[this.m_LookDevConfig.lookDevContexts[index2].currentHDRIIndex];
      float shadowIntensity1 = hdri1.shadowInfo.shadowIntensity;
      float shadowIntensity2 = hdri2.shadowInfo.shadowIntensity;
      Color shadowColor1 = hdri1.shadowInfo.shadowColor;
      Color shadowColor2 = hdri2.shadowInfo.shadowColor;
      Texture texture1 = previewContext0.m_PreviewResult[0];
      Texture texture2 = previewContext0.m_PreviewResult[1];
      Texture texture3 = previewContext0.m_PreviewResult[2];
      Texture texture4 = previewContext1.m_PreviewResult[0];
      Texture texture5 = previewContext1.m_PreviewResult[1];
      Texture texture6 = previewContext1.m_PreviewResult[2];
      Vector4 vector4_5 = new Vector4(this.m_LookDevConfig.dualViewBlendFactor, y, z, this.m_LookDevConfig.currentEditionContext != LookDevEditionContext.Left ? -1f : 1f);
      Vector4 vector4_6 = new Vector4(x, !this.m_LookDevConfig.enableToneMap ? -1f : 1f, shadowIntensity1, shadowIntensity2);
      Vector4 vector4_7 = new Vector4(1.4f, 1f, 0.5f, 0.5f);
      Vector4 vector4_8 = new Vector4(0.0f, 0.0f, 5.3f, 1f);
      RenderTexture active = RenderTexture.active;
      RenderTexture.active = this.m_FinalCompositionTexture;
      LookDevResources.m_LookDevCompositing.SetTexture("_Tex0Normal", texture1);
      LookDevResources.m_LookDevCompositing.SetTexture("_Tex0WithoutSun", texture2);
      LookDevResources.m_LookDevCompositing.SetTexture("_Tex0Shadows", texture3);
      LookDevResources.m_LookDevCompositing.SetColor("_ShadowColor0", shadowColor1);
      LookDevResources.m_LookDevCompositing.SetTexture("_Tex1Normal", texture4);
      LookDevResources.m_LookDevCompositing.SetTexture("_Tex1WithoutSun", texture5);
      LookDevResources.m_LookDevCompositing.SetTexture("_Tex1Shadows", texture6);
      LookDevResources.m_LookDevCompositing.SetColor("_ShadowColor1", shadowColor2);
      LookDevResources.m_LookDevCompositing.SetVector("_CompositingParams", vector4_5);
      LookDevResources.m_LookDevCompositing.SetVector("_CompositingParams2", vector4_6);
      LookDevResources.m_LookDevCompositing.SetColor("_FirstViewColor", (Color) LookDevView.m_FirstViewGizmoColor);
      LookDevResources.m_LookDevCompositing.SetColor("_SecondViewColor", (Color) LookDevView.m_SecondViewGizmoColor);
      LookDevResources.m_LookDevCompositing.SetVector("_GizmoPosition", vector4_1);
      LookDevResources.m_LookDevCompositing.SetVector("_GizmoZoneCenter", vector4_2);
      LookDevResources.m_LookDevCompositing.SetVector("_GizmoSplitPlane", this.m_LookDevConfig.gizmo.plane);
      LookDevResources.m_LookDevCompositing.SetVector("_GizmoSplitPlaneOrtho", this.m_LookDevConfig.gizmo.planeOrtho);
      LookDevResources.m_LookDevCompositing.SetFloat("_GizmoLength", this.m_LookDevConfig.gizmo.length);
      LookDevResources.m_LookDevCompositing.SetVector("_GizmoThickness", vector4_3);
      LookDevResources.m_LookDevCompositing.SetVector("_GizmoCircleRadius", vector4_4);
      LookDevResources.m_LookDevCompositing.SetFloat("_BlendFactorCircleRadius", this.m_BlendFactorCircleRadius);
      LookDevResources.m_LookDevCompositing.SetFloat("_GetBlendFactorMaxGizmoDistance", this.GetBlendFactorMaxGizmoDistance());
      LookDevResources.m_LookDevCompositing.SetFloat("_GizmoRenderMode", !this.m_ForceGizmoRenderSelector ? (float) this.m_GizmoRenderMode : 4f);
      LookDevResources.m_LookDevCompositing.SetVector("_ScreenRatio", this.m_ScreenRatio);
      LookDevResources.m_LookDevCompositing.SetVector("_ToneMapCoeffs1", vector4_7);
      LookDevResources.m_LookDevCompositing.SetVector("_ToneMapCoeffs2", vector4_8);
      LookDevResources.m_LookDevCompositing.SetPass((int) this.m_LookDevConfig.lookDevMode);
      LookDevView.DrawFullScreenQuad(new Rect(0.0f, 0.0f, previewRect.width, previewRect.height));
      RenderTexture.active = active;
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      GUI.DrawTexture(previewRect, (Texture) this.m_FinalCompositionTexture, ScaleMode.StretchToFill, false);
      GL.sRGBWrite = false;
    }

    private void EditorUpdate()
    {
      for (int index1 = 0; index1 < 2; ++index1)
      {
        for (int index2 = 0; index2 < this.m_LookDevConfig.previewObjects[index1].Length; ++index2)
        {
          GameObject go = this.m_LookDevConfig.previewObjects[index1][index2];
          if (!((UnityEngine.Object) go == (UnityEngine.Object) null))
          {
            EditorUtility.InitInstantiatedPreviewRecursive(go);
            LookDevConfig.DisableRendererProperties(go);
            PreviewRenderUtility.SetEnabledRecursive(go, false);
          }
        }
      }
    }

    private void RenderPreview()
    {
      if (Event.current.type != EventType.Repaint)
        return;
      this.m_ObjRotationAcc = !this.m_LookDevConfig.rotateObjectMode ? 0.0f : Math.Min(this.m_ObjRotationAcc + Time.deltaTime * 0.5f, 1f);
      this.m_EnvRotationAcc = !this.m_LookDevConfig.rotateEnvMode ? 0.0f : Math.Min(this.m_EnvRotationAcc + Time.deltaTime * 0.5f, 1f);
      this.m_CurrentObjRotationOffset = (float) (((double) this.m_CurrentObjRotationOffset + (double) Time.deltaTime * 360.0 * 0.300000011920929 * (double) this.m_LookDevConfig.objRotationSpeed * (double) this.m_ObjRotationAcc) % 360.0);
      this.m_LookDevConfig.lookDevContexts[0].envRotation = (float) (((double) this.m_LookDevConfig.lookDevContexts[0].envRotation + (double) Time.deltaTime * 360.0 * 0.0299999993294477 * (double) this.m_LookDevConfig.envRotationSpeed * (double) this.m_EnvRotationAcc) % 720.0);
      this.m_LookDevConfig.lookDevContexts[1].envRotation = (float) (((double) this.m_LookDevConfig.lookDevContexts[1].envRotation + (double) Time.deltaTime * 360.0 * 0.0299999993294477 * (double) this.m_LookDevConfig.envRotationSpeed * (double) this.m_EnvRotationAcc) % 720.0);
      switch (this.m_LookDevConfig.lookDevMode)
      {
        case LookDevMode.Single1:
        case LookDevMode.Single2:
          this.RenderPreviewSingle();
          break;
        case LookDevMode.SideBySide:
          this.RenderPreviewSideBySide();
          break;
        case LookDevMode.Split:
        case LookDevMode.Zone:
          this.RenderPreviewDualView();
          break;
      }
    }

    private void DoGizmoDebug()
    {
      if (!this.m_DisplayDebugGizmo)
        return;
      int num = 7;
      float width1 = 150f;
      float height = this.kLineHeight * (float) num;
      float width2 = 60f;
      float width3 = 90f;
      GUILayout.BeginArea(new Rect((float) ((double) this.position.width - (double) width1 - 10.0), (float) ((double) this.position.height - (double) height - 10.0), width1, height), LookDevView.styles.sBigTitleInnerStyle);
      GUILayout.BeginVertical();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Thickness", EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        GUILayout.Width(width3)
      });
      this.m_GizmoThickness = Mathf.Clamp(EditorGUILayout.FloatField(this.m_GizmoThickness, GUILayout.Width(width2)), 0.0f, 1f);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("ThicknessSelected", EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        GUILayout.Width(width3)
      });
      this.m_GizmoThicknessSelected = Mathf.Clamp(EditorGUILayout.FloatField(this.m_GizmoThicknessSelected, GUILayout.Width(width2)), 0.0f, 1f);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Radius", EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        GUILayout.Width(width3)
      });
      this.m_GizmoCircleRadius = Mathf.Clamp(EditorGUILayout.FloatField(this.m_GizmoCircleRadius, GUILayout.Width(width2)), 0.0f, 1f);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("RadiusSelected", EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        GUILayout.Width(width3)
      });
      this.m_GizmoCircleRadiusSelected = Mathf.Clamp(EditorGUILayout.FloatField(this.m_GizmoCircleRadiusSelected, GUILayout.Width(width2)), 0.0f, 1f);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("BlendRadius", EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        GUILayout.Width(width3)
      });
      this.m_BlendFactorCircleRadius = Mathf.Clamp(EditorGUILayout.FloatField(this.m_BlendFactorCircleRadius, GUILayout.Width(width2)), 0.0f, 1f);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Selected", EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        GUILayout.Width(width3)
      });
      this.m_ForceGizmoRenderSelector = GUILayout.Toggle(this.m_ForceGizmoRenderSelector, "");
      GUILayout.EndHorizontal();
      if (GUILayout.Button("Reset Gizmo"))
        this.m_LookDevConfig.gizmo.Update(new Vector2(0.0f, 0.0f), 0.2f, 0.0f);
      GUILayout.EndVertical();
      GUILayout.EndArea();
    }

    private void UpdateViewSpecific()
    {
      this.UpdatePreviewRects(this.m_DisplayRect);
      this.m_ScreenRatio.Set(this.m_PreviewRects[2].width / this.kReferenceScale, this.m_PreviewRects[2].height / this.kReferenceScale, this.m_PreviewRects[2].width, this.m_PreviewRects[2].height);
      int num = 4;
      float width = 292f;
      float height = this.kLineHeight * (float) num + EditorGUIUtility.standardVerticalSpacing;
      this.m_ControlWindowRect = new Rect((float) ((double) this.m_PreviewRects[2].width / 2.0 - (double) width / 2.0), (float) ((double) this.m_PreviewRects[2].height - (double) height - 10.0), width, height);
    }

    private void UpdatePreviewRects(Rect previewRect)
    {
      this.m_PreviewRects[2] = new Rect(previewRect);
      if (this.m_ShowLookDevEnvWindow)
        this.m_PreviewRects[2].width -= this.ComputeLookDevEnvWindowWidth();
      this.m_PreviewRects[0] = new Rect(this.m_PreviewRects[2].x, this.m_PreviewRects[2].y, this.m_PreviewRects[2].width / 2f, this.m_PreviewRects[2].height);
      this.m_PreviewRects[1] = new Rect(this.m_PreviewRects[2].width / 2f, this.m_PreviewRects[2].y, this.m_PreviewRects[2].width / 2f, this.m_PreviewRects[2].height);
    }

    private void HandleCamera()
    {
      if (this.m_LookDevOperationType != LookDevOperationType.None || this.m_ControlWindowRect.Contains(Event.current.mousePosition))
        return;
      int editionContextIndex = this.m_LookDevConfig.currentEditionContextIndex;
      int index1 = (editionContextIndex + 1) % 2;
      this.m_CameraController.Update(this.m_LookDevConfig.cameraState[editionContextIndex], this.m_PreviewUtilityContexts[this.m_LookDevConfig.currentEditionContextIndex].m_PreviewUtility[0].camera);
      if ((this.m_LookDevConfig.lookDevMode == LookDevMode.Single1 || this.m_LookDevConfig.lookDevMode == LookDevMode.Single2 || this.m_LookDevConfig.lookDevMode == LookDevMode.SideBySide) && this.m_LookDevConfig.sideBySideCameraLinked)
        this.m_LookDevConfig.cameraState[index1].Copy(this.m_LookDevConfig.cameraState[editionContextIndex]);
      if (this.m_CameraController.currentViewTool == ViewTool.None && Event.current.type == EventType.KeyUp && (Event.current.keyCode == KeyCode.F && !EditorGUIUtility.editingTextField))
      {
        this.Frame(this.m_LookDevConfig.currentEditionContext, true);
        Event.current.Use();
      }
      for (int index2 = 0; index2 < 3; ++index2)
      {
        this.m_LookDevConfig.cameraState[0].UpdateCamera(this.m_PreviewUtilityContexts[0].m_PreviewUtility[index2].camera);
        this.m_LookDevConfig.cameraState[1].UpdateCamera(this.m_PreviewUtilityContexts[1].m_PreviewUtility[index2].camera);
      }
      this.m_LookDevConfig.cameraStateLeft.Copy(this.m_LookDevConfig.cameraState[0]);
      this.m_LookDevConfig.cameraStateRight.Copy(this.m_LookDevConfig.cameraState[1]);
      this.DelayedSaveLookDevConfig();
    }

    public void HandleKeyboardShortcut()
    {
      if (Event.current.type == EventType.Layout || EditorGUIUtility.editingTextField)
        return;
      if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.RightArrow)
      {
        this.m_LookDevConfig.UpdateIntProperty(LookDevProperty.HDRI, Math.Min(this.m_LookDevConfig.currentLookDevContext.currentHDRIIndex + 1, this.m_LookDevEnvLibrary.hdriList.Count - 1));
        Event.current.Use();
      }
      else if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.LeftArrow)
      {
        this.m_LookDevConfig.UpdateIntProperty(LookDevProperty.HDRI, Math.Max(this.m_LookDevConfig.currentLookDevContext.currentHDRIIndex - 1, 0));
        Event.current.Use();
      }
      if (Event.current.type != EventType.KeyUp || Event.current.keyCode != KeyCode.R)
        return;
      this.m_LookDevConfig.ResynchronizeObjects();
      Event.current.Use();
    }

    public void Frame()
    {
      this.Frame(true);
    }

    public void Frame(bool animate)
    {
      this.Frame(LookDevEditionContext.Left, animate);
      this.Frame(LookDevEditionContext.Right, animate);
    }

    private void Frame(LookDevEditionContext context, bool animate)
    {
      GameObject go = this.m_LookDevConfig.currentObjectInstances[(int) context][0];
      if ((UnityEngine.Object) go != (UnityEngine.Object) null)
      {
        Bounds bounds = new Bounds(go.transform.position, Vector3.zero);
        this.GetRenderableBoundsRecurse(ref bounds, go);
        float num = bounds.extents.magnitude * 1.5f;
        if ((double) num == 0.0)
          num = 10f;
        CameraState cameraState = this.m_LookDevConfig.cameraState[(int) context];
        if (animate)
        {
          cameraState.pivot.target = bounds.center;
          cameraState.viewSize.target = Mathf.Abs(num * 2.2f);
        }
        else
        {
          cameraState.pivot.value = bounds.center;
          cameraState.viewSize.value = Mathf.Abs(num * 2.2f);
        }
      }
      this.m_CurrentObjRotationOffset = 0.0f;
    }

    private void HandleDragging()
    {
      Event current = Event.current;
      EventType type = current.type;
      switch (type)
      {
        case EventType.Repaint:
          break;
        case EventType.DragUpdated:
          bool flag1 = false;
          foreach (UnityEngine.Object objectReference in DragAndDrop.objectReferences)
          {
            if ((bool) ((UnityEngine.Object) (objectReference as Cubemap)))
              flag1 = true;
            Material material = objectReference as Material;
            if ((bool) ((UnityEngine.Object) material) && material.shader.name.Contains("Skybox/Cubemap"))
              flag1 = true;
            GameObject go = objectReference as GameObject;
            if ((bool) ((UnityEngine.Object) go) && EditorUtility.IsPersistent((UnityEngine.Object) go) && (PrefabUtility.GetPrefabObject((UnityEngine.Object) go) != (UnityEngine.Object) null && GameObjectInspector.HasRenderableParts(go)))
              flag1 = true;
            if ((bool) ((UnityEngine.Object) (objectReference as LookDevEnvironmentLibrary)))
              flag1 = true;
          }
          DragAndDrop.visualMode = !flag1 ? DragAndDropVisualMode.Rejected : DragAndDropVisualMode.Link;
          this.m_CurrentDragContext = this.GetEditionContext(Event.current.mousePosition);
          current.Use();
          break;
        case EventType.DragPerform:
          bool flag2 = false;
          if (this.m_PreviewRects[2].Contains(current.mousePosition))
          {
            foreach (UnityEngine.Object objectReference in DragAndDrop.objectReferences)
            {
              Cubemap cubemap = objectReference as Cubemap;
              if ((bool) ((UnityEngine.Object) cubemap))
              {
                this.UpdateFocus(Event.current.mousePosition);
                this.UpdateContextWithCurrentHDRI(cubemap);
              }
              Material material = objectReference as Material;
              if ((bool) ((UnityEngine.Object) material) && material.shader.name.Contains("Skybox/Cubemap"))
              {
                Cubemap texture = material.GetTexture("_Tex") as Cubemap;
                if ((bool) ((UnityEngine.Object) texture))
                {
                  this.UpdateFocus(Event.current.mousePosition);
                  this.UpdateContextWithCurrentHDRI(texture);
                }
              }
              GameObject go = objectReference as GameObject;
              if ((bool) ((UnityEngine.Object) go) && !flag2 && GameObjectInspector.HasRenderableParts(go))
              {
                this.UpdateFocus(Event.current.mousePosition);
                Undo.RecordObject((UnityEngine.Object) this.m_LookDevConfig, "Set current preview object");
                bool flag3 = this.m_LookDevConfig.SetCurrentPreviewObject(go);
                this.DelayedSaveLookDevConfig();
                this.Frame(this.m_LookDevConfig.currentEditionContext, false);
                if (flag3)
                  this.Frame(this.m_LookDevConfig.currentEditionContext != LookDevEditionContext.Left ? LookDevEditionContext.Left : LookDevEditionContext.Right, false);
                flag2 = true;
              }
              LookDevEnvironmentLibrary environmentLibrary = objectReference as LookDevEnvironmentLibrary;
              if ((bool) ((UnityEngine.Object) environmentLibrary))
                this.envLibrary = environmentLibrary;
            }
          }
          DragAndDrop.AcceptDrag();
          this.m_CurrentDragContext = LookDevEditionContext.None;
          this.m_LookDevEnvWindow.CancelSelection();
          current.Use();
          break;
        default:
          if (type != EventType.DragExited)
            break;
          this.m_CurrentDragContext = LookDevEditionContext.None;
          break;
      }
    }

    private Vector2 GetNormalizedCoordinates(Vector2 mousePosition, Rect previewRect)
    {
      Vector2 vector2 = (Vector2) new Vector3((mousePosition.x - previewRect.x) / previewRect.width, (mousePosition.y - previewRect.y) / previewRect.height);
      vector2.x = (float) ((double) vector2.x * 2.0 - 1.0) * this.m_ScreenRatio.x;
      vector2.y = (float) -((double) vector2.y * 2.0 - 1.0) * this.m_ScreenRatio.y;
      return vector2;
    }

    private LookDevEditionContext GetEditionContext(Vector2 position)
    {
      if (!this.m_PreviewRects[2].Contains(position))
        return LookDevEditionContext.None;
      LookDevEditionContext devEditionContext;
      switch (this.m_LookDevConfig.lookDevMode)
      {
        case LookDevMode.Single1:
          devEditionContext = LookDevEditionContext.Left;
          break;
        case LookDevMode.Single2:
          devEditionContext = LookDevEditionContext.Right;
          break;
        case LookDevMode.SideBySide:
          devEditionContext = !this.m_PreviewRects[0].Contains(position) ? LookDevEditionContext.Right : LookDevEditionContext.Left;
          break;
        case LookDevMode.Split:
          Vector2 normalizedCoordinates = this.GetNormalizedCoordinates(position, this.m_PreviewRects[2]);
          devEditionContext = (double) Vector3.Dot(new Vector3(normalizedCoordinates.x, normalizedCoordinates.y, 1f), (Vector3) this.m_LookDevConfig.gizmo.plane) <= 0.0 ? LookDevEditionContext.Right : LookDevEditionContext.Left;
          break;
        case LookDevMode.Zone:
          devEditionContext = (double) Vector2.Distance(this.GetNormalizedCoordinates(position, this.m_PreviewRects[2]), this.m_LookDevConfig.gizmo.point2) - (double) this.m_LookDevConfig.gizmo.length * 2.0 <= 0.0 ? LookDevEditionContext.Right : LookDevEditionContext.Left;
          break;
        default:
          devEditionContext = LookDevEditionContext.Left;
          break;
      }
      return devEditionContext;
    }

    public void UpdateFocus(Vector2 position)
    {
      this.m_LookDevConfig.UpdateFocus(this.GetEditionContext(position));
    }

    private LookDevOperationType GetGizmoZoneOperation(Vector2 mousePosition, Rect previewRect)
    {
      Vector2 normalizedCoordinates = this.GetNormalizedCoordinates(mousePosition, previewRect);
      Vector3 lhs = new Vector3(normalizedCoordinates.x, normalizedCoordinates.y, 1f);
      float num1 = Mathf.Abs(Vector3.Dot(lhs, (Vector3) this.m_LookDevConfig.gizmo.plane));
      float num2 = Vector2.Distance(normalizedCoordinates, this.m_LookDevConfig.gizmo.center);
      float num3 = (double) Vector3.Dot(lhs, (Vector3) this.m_LookDevConfig.gizmo.planeOrtho) <= 0.0 ? -1f : 1f;
      Vector2 vector2_1 = new Vector2(this.m_LookDevConfig.gizmo.planeOrtho.x, this.m_LookDevConfig.gizmo.planeOrtho.y);
      LookDevOperationType devOperationType = LookDevOperationType.None;
      if ((double) num1 < (double) this.m_GizmoCircleRadiusSelected && (double) num2 < (double) this.m_LookDevConfig.gizmo.length + (double) this.m_GizmoCircleRadiusSelected)
      {
        if ((double) num1 < (double) this.m_GizmoThicknessSelected)
          devOperationType = LookDevOperationType.GizmoTranslation;
        Vector2 b = this.m_LookDevConfig.gizmo.center + num3 * vector2_1 * this.m_LookDevConfig.gizmo.length;
        if ((double) Vector2.Distance(normalizedCoordinates, b) <= (double) this.m_GizmoCircleRadiusSelected)
          devOperationType = (double) num3 <= 0.0 ? LookDevOperationType.GizmoRotationZone2 : LookDevOperationType.GizmoRotationZone1;
        float maxGizmoDistance = this.GetBlendFactorMaxGizmoDistance();
        float num4 = this.GetBlendFactorMaxGizmoDistance() + this.m_BlendFactorCircleRadius - this.m_BlendFactorCircleSelectionRadius;
        float f = this.m_LookDevConfig.dualViewBlendFactor * this.GetBlendFactorMaxGizmoDistance();
        Vector2 vector2_2 = this.m_LookDevConfig.gizmo.center - vector2_1 * f;
        float num5 = Mathf.Lerp(this.m_BlendFactorCircleRadius, this.m_BlendFactorCircleSelectionRadius, Mathf.Clamp((float) (((double) maxGizmoDistance - (double) Mathf.Abs(f)) / ((double) maxGizmoDistance - (double) num4)), 0.0f, 1f));
        if ((double) (normalizedCoordinates - vector2_2).magnitude < (double) num5)
          devOperationType = LookDevOperationType.BlendFactor;
      }
      return devOperationType;
    }

    private bool IsOperatingGizmo()
    {
      return this.m_LookDevOperationType == LookDevOperationType.BlendFactor || this.m_LookDevOperationType == LookDevOperationType.GizmoRotationZone1 || this.m_LookDevOperationType == LookDevOperationType.GizmoRotationZone2 || this.m_LookDevOperationType == LookDevOperationType.GizmoTranslation;
    }

    private void HandleMouseInput()
    {
      Event current = Event.current;
      this.m_hotControlID = GUIUtility.GetControlID(FocusType.Passive);
      switch (current.GetTypeForControl(this.m_hotControlID))
      {
        case EventType.MouseDown:
          if ((this.m_LookDevConfig.lookDevMode == LookDevMode.Split || this.m_LookDevConfig.lookDevMode == LookDevMode.Zone) && current.button == 0)
          {
            this.m_LookDevOperationType = this.GetGizmoZoneOperation(Event.current.mousePosition, this.m_PreviewRects[2]);
            this.m_OnMouseDownOffsetToGizmo = this.GetNormalizedCoordinates(Event.current.mousePosition, this.m_PreviewRects[2]) - this.m_LookDevConfig.gizmo.center;
          }
          if (this.m_LookDevOperationType == LookDevOperationType.None)
          {
            if (current.shift && current.button == 0)
              this.m_LookDevOperationType = LookDevOperationType.RotateLight;
            else if (current.control && current.button == 0)
              this.m_LookDevOperationType = LookDevOperationType.RotateEnvironment;
          }
          if (!this.IsOperatingGizmo() && !this.m_ControlWindowRect.Contains(Event.current.mousePosition))
            this.UpdateFocus(Event.current.mousePosition);
          GUIUtility.hotControl = this.m_hotControlID;
          break;
        case EventType.MouseUp:
          if (this.m_LookDevOperationType == LookDevOperationType.BlendFactor && (double) Mathf.Abs(this.m_LookDevConfig.dualViewBlendFactor) < (double) this.m_GizmoCircleRadiusSelected / ((double) this.m_LookDevConfig.gizmo.length - (double) this.m_GizmoCircleRadius))
            this.m_LookDevConfig.dualViewBlendFactor = 0.0f;
          this.m_LookDevOperationType = LookDevOperationType.None;
          if (this.m_LookDevEnvWindow != null)
          {
            Cubemap currentSelection = this.m_LookDevEnvWindow.GetCurrentSelection();
            if ((UnityEngine.Object) currentSelection != (UnityEngine.Object) null)
            {
              this.UpdateFocus(Event.current.mousePosition);
              this.UpdateContextWithCurrentHDRI(currentSelection);
              this.m_LookDevEnvWindow.CancelSelection();
              this.m_CurrentDragContext = LookDevEditionContext.None;
              this.Repaint();
            }
          }
          GUIUtility.hotControl = 0;
          break;
        case EventType.MouseMove:
          this.m_GizmoRenderMode = this.GetGizmoZoneOperation(Event.current.mousePosition, this.m_PreviewRects[2]);
          this.Repaint();
          break;
        case EventType.MouseDrag:
          if (this.m_LookDevOperationType == LookDevOperationType.RotateEnvironment)
          {
            this.m_LookDevConfig.UpdateFloatProperty(LookDevProperty.EnvRotation, (float) (((double) this.m_LookDevConfig.currentLookDevContext.envRotation + (double) current.delta.x / (double) Mathf.Min(this.position.width, this.position.height) * 140.0 + 720.0) % 720.0));
            Event.current.Use();
            break;
          }
          if (this.m_LookDevOperationType == LookDevOperationType.RotateLight && this.m_LookDevConfig.enableShadowCubemap)
          {
            ShadowInfo shadowInfo = this.m_LookDevEnvLibrary.hdriList[this.m_LookDevConfig.currentLookDevContext.currentHDRIIndex].shadowInfo;
            shadowInfo.latitude -= current.delta.y * 0.6f;
            shadowInfo.longitude -= current.delta.x * 0.6f;
            this.Repaint();
            break;
          }
          break;
      }
      if (Event.current.rawType == EventType.MouseUp && (UnityEngine.Object) this.m_LookDevEnvWindow.GetCurrentSelection() != (UnityEngine.Object) null)
        this.m_LookDevEnvWindow.CancelSelection();
      if (this.m_LookDevOperationType == LookDevOperationType.GizmoTranslation)
      {
        Vector2 center = this.GetNormalizedCoordinates(Event.current.mousePosition, this.m_PreviewRects[2]) - this.m_OnMouseDownOffsetToGizmo;
        Vector2 normalizedCoordinates1 = this.GetNormalizedCoordinates(new Vector2(this.m_DisplayRect.x, this.m_PreviewRects[2].y + this.m_DisplayRect.height), this.m_PreviewRects[2]);
        Vector2 normalizedCoordinates2 = this.GetNormalizedCoordinates(new Vector2(this.m_DisplayRect.x + this.m_DisplayRect.width, this.m_PreviewRects[2].y), this.m_PreviewRects[2]);
        float num = 0.05f;
        center.x = Mathf.Clamp(center.x, normalizedCoordinates1.x + num, normalizedCoordinates2.x - num);
        center.y = Mathf.Clamp(center.y, normalizedCoordinates1.y + num, normalizedCoordinates2.y - num);
        this.m_LookDevConfig.gizmo.Update(center, this.m_LookDevConfig.gizmo.length, this.m_LookDevConfig.gizmo.angle);
        this.Repaint();
      }
      if (this.m_LookDevOperationType == LookDevOperationType.GizmoRotationZone1 || this.m_LookDevOperationType == LookDevOperationType.GizmoRotationZone2)
      {
        Vector2 normalizedCoordinates = this.GetNormalizedCoordinates(Event.current.mousePosition, this.m_PreviewRects[2]);
        float num1 = 0.3926991f;
        Vector2 vector2_1;
        Vector2 vector2_2;
        if (this.m_LookDevOperationType == LookDevOperationType.GizmoRotationZone1)
        {
          vector2_1 = normalizedCoordinates;
          vector2_2 = this.m_LookDevConfig.gizmo.point2;
        }
        else
        {
          vector2_1 = normalizedCoordinates;
          vector2_2 = this.m_LookDevConfig.gizmo.point1;
        }
        float magnitude1 = (vector2_2 - vector2_1).magnitude;
        float num2 = (float) ((double) Mathf.Min(this.position.width, this.position.height) / (double) this.kReferenceScale * 2.0 * 0.899999976158142);
        if ((double) magnitude1 > (double) num2)
        {
          Vector2 vector2_3 = vector2_1 - vector2_2;
          vector2_3.Normalize();
          vector2_1 = vector2_2 + vector2_3 * num2;
        }
        if (Event.current.shift)
        {
          Vector3 rhs = new Vector3(-1f, 0.0f, vector2_2.x);
          float num3 = Vector3.Dot(new Vector3(normalizedCoordinates.x, normalizedCoordinates.y, 1f), rhs);
          float num4 = (float) Math.PI / 180f * Vector2.Angle(new Vector2(0.0f, 1f), normalizedCoordinates - vector2_2);
          if ((double) num3 > 0.0)
            num4 = 6.283185f - num4;
          float f = (float) (int) ((double) num4 / (double) num1) * num1;
          float magnitude2 = (normalizedCoordinates - vector2_2).magnitude;
          vector2_1 = vector2_2 + new Vector2(Mathf.Sin(f), Mathf.Cos(f)) * magnitude2;
        }
        if (this.m_LookDevOperationType == LookDevOperationType.GizmoRotationZone1)
          this.m_LookDevConfig.gizmo.Update(vector2_1, vector2_2);
        else
          this.m_LookDevConfig.gizmo.Update(vector2_2, vector2_1);
        this.Repaint();
      }
      if (this.m_LookDevOperationType != LookDevOperationType.BlendFactor)
        return;
      Vector2 normalizedCoordinates3 = this.GetNormalizedCoordinates(Event.current.mousePosition, this.m_PreviewRects[2]);
      this.m_LookDevConfig.dualViewBlendFactor = Mathf.Clamp(-Vector3.Dot(new Vector3(normalizedCoordinates3.x, normalizedCoordinates3.y, 1f), (Vector3) this.m_LookDevConfig.gizmo.planeOrtho) / this.GetBlendFactorMaxGizmoDistance(), -1f, 1f);
      this.Repaint();
    }

    private float GetBlendFactorMaxGizmoDistance()
    {
      return this.m_LookDevConfig.gizmo.length - this.m_GizmoCircleRadius - this.m_BlendFactorCircleRadius;
    }

    private void CleanupDeletedHDRI()
    {
      this.m_LookDevEnvLibrary.CleanupDeletedHDRI();
    }

    private void OnGUI()
    {
      if (Event.current.type != EventType.Repaint || !this.m_CaptureRD)
        ;
      this.Initialize();
      this.CleanupDeletedHDRI();
      this.BeginWindows();
      this.m_DisplayRect = new Rect(0.0f, this.kLineHeight, this.position.width, this.position.height - this.kLineHeight);
      this.UpdateViewSpecific();
      this.DoToolbarGUI();
      this.HandleDragging();
      this.RenderPreview();
      this.DoControlWindow();
      this.DoAdditionalGUI();
      this.DoGizmoDebug();
      this.HandleMouseInput();
      this.HandleCamera();
      this.HandleKeyboardShortcut();
      if ((UnityEngine.Object) this.m_LookDevConfig.currentObjectInstances[0][0] == (UnityEngine.Object) null && (UnityEngine.Object) this.m_LookDevConfig.currentObjectInstances[1][0] == (UnityEngine.Object) null)
      {
        Color color = GUI.color;
        GUI.color = Color.gray;
        Vector2 vector2 = GUI.skin.label.CalcSize(LookDevView.styles.sDragAndDropObjsText);
        GUI.Label(new Rect((float) ((double) this.m_DisplayRect.width * 0.5 - (double) vector2.x * 0.5), (float) ((double) this.m_DisplayRect.height * 0.200000002980232 - (double) vector2.y * 0.5), vector2.x, vector2.y), LookDevView.styles.sDragAndDropObjsText);
        GUI.color = color;
      }
      this.EndWindows();
      if (Event.current.type == EventType.Repaint)
      {
        if (this.m_LookDevEnvWindow != null && (UnityEngine.Object) this.m_LookDevEnvWindow.GetCurrentSelection() != (UnityEngine.Object) null)
        {
          this.m_CurrentDragContext = this.GetEditionContext(Event.current.mousePosition);
          GUI.DrawTexture(new Rect(Event.current.mousePosition.x - this.m_LookDevEnvWindow.GetSelectedPositionOffset().x, Event.current.mousePosition.y - this.m_LookDevEnvWindow.GetSelectedPositionOffset().y, 250f, 125f), (Texture) LookDevResources.m_SelectionTexture, ScaleMode.ScaleToFit, true);
        }
        else
          this.m_CurrentDragContext = LookDevEditionContext.None;
      }
      if (Event.current.type != EventType.Repaint || !this.m_CaptureRD)
        return;
      this.m_CaptureRD = false;
    }

    private void GetShaderConstantsFromNormalizedSH(SphericalHarmonicsL2 ambientProbe, Vector4[] outCoefficients)
    {
      for (int index = 0; index < 3; ++index)
      {
        outCoefficients[index].x = ambientProbe[index, 3];
        outCoefficients[index].y = ambientProbe[index, 1];
        outCoefficients[index].z = ambientProbe[index, 2];
        outCoefficients[index].w = ambientProbe[index, 0] - ambientProbe[index, 6];
        outCoefficients[index + 3].x = ambientProbe[index, 4];
        outCoefficients[index + 3].y = ambientProbe[index, 5];
        outCoefficients[index + 3].z = ambientProbe[index, 6] * 3f;
        outCoefficients[index + 3].w = ambientProbe[index, 7];
      }
      outCoefficients[6].x = ambientProbe[0, 8];
      outCoefficients[6].y = ambientProbe[1, 8];
      outCoefficients[6].z = ambientProbe[2, 8];
      outCoefficients[6].w = 1f;
    }

    public class Styles
    {
      public readonly GUIStyle sBigTitleInnerStyle = (GUIStyle) "IN BigTitle inner";
      public readonly GUIStyle sToolBarButton = (GUIStyle) "toolbarbutton";
      public readonly GUIContent sSingleMode1 = EditorGUIUtility.IconContent("LookDevSingle1", "Single1|Single1 object view");
      public readonly GUIContent sSingleMode2 = EditorGUIUtility.IconContent("LookDevSingle2", "Single2|Single2 object view");
      public readonly GUIContent sSideBySideMode = EditorGUIUtility.IconContent("LookDevSideBySide", "Side|Side by side comparison view");
      public readonly GUIContent sSplitMode = EditorGUIUtility.IconContent("LookDevSplit", "Split|Single object split comparison view");
      public readonly GUIContent sZoneMode = EditorGUIUtility.IconContent("LookDevZone", "Zone|Single object zone comparison view");
      public readonly GUIContent sLinkActive = EditorGUIUtility.IconContent("LookDevMirrorViewsActive", "Link|Links the property between the different views");
      public readonly GUIContent sLinkInactive = EditorGUIUtility.IconContent("LookDevMirrorViewsInactive", "Link|Links the property between the different views");
      public readonly GUIContent sDragAndDropObjsText = EditorGUIUtility.TextContent("Drag and drop Prefabs here.");
      public readonly GUIStyle[] sPropertyLabelStyle = new GUIStyle[3]{ new GUIStyle(EditorStyles.miniLabel), new GUIStyle(EditorStyles.miniLabel), new GUIStyle(EditorStyles.miniLabel) };

      public Styles()
      {
        this.sPropertyLabelStyle[0].normal.textColor = (Color) LookDevView.m_FirstViewGizmoColor;
        this.sPropertyLabelStyle[1].normal.textColor = (Color) LookDevView.m_SecondViewGizmoColor;
      }
    }

    internal class PreviewContextCB
    {
      public CommandBuffer m_drawBallCB;
      public CommandBuffer m_patchGBufferCB;
      public MaterialPropertyBlock m_drawBallPB;

      public PreviewContextCB()
      {
        this.m_drawBallCB = new CommandBuffer();
        this.m_drawBallCB.name = "draw ball";
        this.m_patchGBufferCB = new CommandBuffer();
        this.m_patchGBufferCB.name = "patch gbuffer";
        this.m_drawBallPB = new MaterialPropertyBlock();
      }
    }

    internal class PreviewContext
    {
      public PreviewRenderUtility[] m_PreviewUtility = new PreviewRenderUtility[3];
      public Texture[] m_PreviewResult = new Texture[3];
      public LookDevView.PreviewContextCB[] m_PreviewCB = new LookDevView.PreviewContextCB[3];

      public PreviewContext()
      {
        for (int index = 0; index < 3; ++index)
        {
          this.m_PreviewUtility[index] = new PreviewRenderUtility();
          this.m_PreviewUtility[index].camera.fieldOfView = 30f;
          this.m_PreviewUtility[index].camera.cullingMask = 1 << Camera.PreviewCullingLayer;
          this.m_PreviewCB[index] = new LookDevView.PreviewContextCB();
        }
      }

      public void Cleanup()
      {
        for (int index = 0; index < 3; ++index)
        {
          if (this.m_PreviewUtility[index] != null)
          {
            this.m_PreviewUtility[index].Cleanup();
            this.m_PreviewUtility[index] = (PreviewRenderUtility) null;
          }
        }
      }

      public enum PreviewContextPass
      {
        kView,
        kViewWithShadow,
        kShadow,
        kCount,
      }
    }
  }
}
