// Decompiled with JetBrains decompiler
// Type: UnityEditor.PlayerSettingsEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEditor.Build;
using UnityEditor.CrashReporting;
using UnityEditor.Modules;
using UnityEditor.SceneManagement;
using UnityEditor.Scripting.ScriptCompilation;
using UnityEditorInternal;
using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [CustomEditor(typeof (PlayerSettings))]
  internal class PlayerSettingsEditor : Editor
  {
    private static GraphicsJobMode[] m_GfxJobModeValues = new GraphicsJobMode[2]{ GraphicsJobMode.Native, GraphicsJobMode.Legacy };
    private static GUIContent[] m_GfxJobModeNames = new GUIContent[2]{ new GUIContent("Native"), new GUIContent("Legacy") };
    private static GUIContent[] m_XBoxOneGfxJobModeNames = new GUIContent[2]{ new GUIContent("Native - DX12"), new GUIContent("Legacy - DX11") };
    private static Dictionary<BuildTarget, ReorderableList> s_GraphicsDeviceLists = new Dictionary<BuildTarget, ReorderableList>();
    private static Dictionary<BuildTargetGroup, List<ColorGamut>> s_SupportedColorGamuts = new Dictionary<BuildTargetGroup, List<ColorGamut>>() { { BuildTargetGroup.Standalone, new List<ColorGamut>() { ColorGamut.sRGB, ColorGamut.DisplayP3 } }, { BuildTargetGroup.iPhone, new List<ColorGamut>() { ColorGamut.sRGB, ColorGamut.DisplayP3 } } };
    private static ApiCompatibilityLevel[] only_4_x_profiles = new ApiCompatibilityLevel[1]{ ApiCompatibilityLevel.NET_4_6 };
    private static ApiCompatibilityLevel[] only_2_0_profiles = new ApiCompatibilityLevel[2]{ ApiCompatibilityLevel.NET_2_0, ApiCompatibilityLevel.NET_2_0_Subset };
    private static ApiCompatibilityLevel[] allProfiles = new ApiCompatibilityLevel[3]{ ApiCompatibilityLevel.NET_2_0, ApiCompatibilityLevel.NET_2_0_Subset, ApiCompatibilityLevel.NET_4_6 };
    private SavedInt m_SelectedSection = new SavedInt("PlayerSettings.ShownSection", -1);
    private int selectedPlatform = 0;
    private int scriptingDefinesControlID = 0;
    private AnimBool[] m_SectionAnimators = new AnimBool[7];
    private readonly AnimBool m_ShowDefaultIsNativeResolution = new AnimBool();
    private readonly AnimBool m_ShowResolution = new AnimBool();
    private const int kSlotSize = 64;
    private const int kMaxPreviewSize = 96;
    private const int kIconSpacing = 6;
    private PlayerSettingsSplashScreenEditor m_SplashScreenEditor;
    private BuildPlatform[] validPlatforms;
    private SerializedProperty m_StripEngineCode;
    private SerializedProperty m_ApplicationBundleVersion;
    private SerializedProperty m_UseMacAppStoreValidation;
    private SerializedProperty m_MacAppStoreCategory;
    private SerializedProperty m_IPhoneApplicationDisplayName;
    private SerializedProperty m_CameraUsageDescription;
    private SerializedProperty m_LocationUsageDescription;
    private SerializedProperty m_MicrophoneUsageDescription;
    private SerializedProperty m_IPhoneStrippingLevel;
    private SerializedProperty m_IPhoneScriptCallOptimization;
    private SerializedProperty m_AotOptions;
    private SerializedProperty m_DefaultScreenOrientation;
    private SerializedProperty m_AllowedAutoRotateToPortrait;
    private SerializedProperty m_AllowedAutoRotateToPortraitUpsideDown;
    private SerializedProperty m_AllowedAutoRotateToLandscapeRight;
    private SerializedProperty m_AllowedAutoRotateToLandscapeLeft;
    private SerializedProperty m_UseOSAutoRotation;
    private SerializedProperty m_Use32BitDisplayBuffer;
    private SerializedProperty m_DisableDepthAndStencilBuffers;
    private SerializedProperty m_iosShowActivityIndicatorOnLoading;
    private SerializedProperty m_androidShowActivityIndicatorOnLoading;
    private SerializedProperty m_tizenShowActivityIndicatorOnLoading;
    private SerializedProperty m_AndroidProfiler;
    private SerializedProperty m_UIPrerenderedIcon;
    private SerializedProperty m_UIRequiresPersistentWiFi;
    private SerializedProperty m_UIStatusBarHidden;
    private SerializedProperty m_UIRequiresFullScreen;
    private SerializedProperty m_UIStatusBarStyle;
    private SerializedProperty m_IOSAllowHTTPDownload;
    private SerializedProperty m_SubmitAnalytics;
    private SerializedProperty m_IOSURLSchemes;
    private SerializedProperty m_AccelerometerFrequency;
    private SerializedProperty m_useOnDemandResources;
    private SerializedProperty m_MuteOtherAudioSources;
    private SerializedProperty m_PrepareIOSForRecording;
    private SerializedProperty m_ForceIOSSpeakersWhenRecording;
    private SerializedProperty m_EnableInternalProfiler;
    private SerializedProperty m_ActionOnDotNetUnhandledException;
    private SerializedProperty m_LogObjCUncaughtExceptions;
    private SerializedProperty m_EnableCrashReportAPI;
    private SerializedProperty m_EnableInputSystem;
    private SerializedProperty m_DisableInputManager;
    private SerializedProperty m_VideoMemoryForVertexBuffers;
    private SerializedProperty m_CompanyName;
    private SerializedProperty m_ProductName;
    private SerializedProperty m_DefaultCursor;
    private SerializedProperty m_CursorHotspot;
    private SerializedProperty m_DefaultScreenWidth;
    private SerializedProperty m_DefaultScreenHeight;
    private SerializedProperty m_ActiveColorSpace;
    private SerializedProperty m_StripUnusedMeshComponents;
    private SerializedProperty m_VertexChannelCompressionMask;
    private SerializedProperty m_MetalForceHardShadows;
    private SerializedProperty m_MetalEditorSupport;
    private SerializedProperty m_MetalAPIValidation;
    private SerializedProperty m_MetalFramebufferOnly;
    private SerializedProperty m_DisplayResolutionDialog;
    private SerializedProperty m_DefaultIsFullScreen;
    private SerializedProperty m_DefaultIsNativeResolution;
    private SerializedProperty m_MacRetinaSupport;
    private SerializedProperty m_UsePlayerLog;
    private SerializedProperty m_KeepLoadedShadersAlive;
    private SerializedProperty m_PreloadedAssets;
    private SerializedProperty m_BakeCollisionMeshes;
    private SerializedProperty m_ResizableWindow;
    private SerializedProperty m_MacFullscreenMode;
    private SerializedProperty m_D3D11FullscreenMode;
    private SerializedProperty m_VisibleInBackground;
    private SerializedProperty m_AllowFullscreenSwitch;
    private SerializedProperty m_ForceSingleInstance;
    private SerializedProperty m_RunInBackground;
    private SerializedProperty m_CaptureSingleScreen;
    private SerializedProperty m_SupportedAspectRatios;
    private SerializedProperty m_SkinOnGPU;
    private SerializedProperty m_GraphicsJobs;
    private SerializedProperty m_RequireES31;
    private SerializedProperty m_RequireES31AEP;
    private SerializedProperty m_LightmapEncodingQuality;
    private static ReorderableList s_ColorGamutList;
    public PlayerSettingsEditorVR m_VRSettings;
    private ISettingEditorExtension[] m_SettingsExtensions;
    private const int kNumberGUISections = 7;
    private static Texture2D s_WarningIcon;
    private static Dictionary<ScriptingImplementation, GUIContent> m_NiceScriptingBackendNames;
    private static Dictionary<ApiCompatibilityLevel, GUIContent> m_NiceApiCompatibilityLevelNames;

    private PlayerSettingsSplashScreenEditor splashScreenEditor
    {
      get
      {
        if (this.m_SplashScreenEditor == null)
          this.m_SplashScreenEditor = new PlayerSettingsSplashScreenEditor(this);
        return this.m_SplashScreenEditor;
      }
    }

    public static void SyncPlatformAPIsList(BuildTarget target)
    {
      if (!PlayerSettingsEditor.s_GraphicsDeviceLists.ContainsKey(target))
        return;
      PlayerSettingsEditor.s_GraphicsDeviceLists[target].list = (IList) ((IEnumerable<GraphicsDeviceType>) PlayerSettings.GetGraphicsAPIs(target)).ToList<GraphicsDeviceType>();
    }

    public static void SyncColorGamuts()
    {
      PlayerSettingsEditor.s_ColorGamutList.list = (IList) ((IEnumerable<ColorGamut>) PlayerSettings.GetColorGamuts()).ToList<ColorGamut>();
    }

    public bool IsMobileTarget(BuildTargetGroup targetGroup)
    {
      return targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.Tizen;
    }

    public SerializedProperty FindPropertyAssert(string name)
    {
      SerializedProperty property = this.serializedObject.FindProperty(name);
      if (property == null)
        Debug.LogError((object) ("Failed to find:" + name));
      return property;
    }

    private void OnEnable()
    {
      this.validPlatforms = BuildPlatforms.instance.GetValidPlatforms(true).ToArray();
      this.m_IPhoneStrippingLevel = this.FindPropertyAssert("iPhoneStrippingLevel");
      this.m_StripEngineCode = this.FindPropertyAssert("stripEngineCode");
      this.m_IPhoneScriptCallOptimization = this.FindPropertyAssert("iPhoneScriptCallOptimization");
      this.m_AndroidProfiler = this.FindPropertyAssert("AndroidProfiler");
      this.m_CompanyName = this.FindPropertyAssert("companyName");
      this.m_ProductName = this.FindPropertyAssert("productName");
      this.m_DefaultCursor = this.FindPropertyAssert("defaultCursor");
      this.m_CursorHotspot = this.FindPropertyAssert("cursorHotspot");
      this.m_UIPrerenderedIcon = this.FindPropertyAssert("uIPrerenderedIcon");
      this.m_UIRequiresFullScreen = this.FindPropertyAssert("uIRequiresFullScreen");
      this.m_UIStatusBarHidden = this.FindPropertyAssert("uIStatusBarHidden");
      this.m_UIStatusBarStyle = this.FindPropertyAssert("uIStatusBarStyle");
      this.m_ActiveColorSpace = this.FindPropertyAssert("m_ActiveColorSpace");
      this.m_StripUnusedMeshComponents = this.FindPropertyAssert("StripUnusedMeshComponents");
      this.m_VertexChannelCompressionMask = this.FindPropertyAssert("VertexChannelCompressionMask");
      this.m_MetalForceHardShadows = this.FindPropertyAssert("iOSMetalForceHardShadows");
      this.m_MetalEditorSupport = this.FindPropertyAssert("metalEditorSupport");
      this.m_MetalAPIValidation = this.FindPropertyAssert("metalAPIValidation");
      this.m_MetalFramebufferOnly = this.FindPropertyAssert("metalFramebufferOnly");
      this.m_ApplicationBundleVersion = this.serializedObject.FindProperty("bundleVersion");
      if (this.m_ApplicationBundleVersion == null)
        this.m_ApplicationBundleVersion = this.FindPropertyAssert("iPhoneBundleVersion");
      this.m_useOnDemandResources = this.FindPropertyAssert("useOnDemandResources");
      this.m_AccelerometerFrequency = this.FindPropertyAssert("accelerometerFrequency");
      this.m_MuteOtherAudioSources = this.FindPropertyAssert("muteOtherAudioSources");
      this.m_PrepareIOSForRecording = this.FindPropertyAssert("Prepare IOS For Recording");
      this.m_ForceIOSSpeakersWhenRecording = this.FindPropertyAssert("Force IOS Speakers When Recording");
      this.m_UIRequiresPersistentWiFi = this.FindPropertyAssert("uIRequiresPersistentWiFi");
      this.m_IOSAllowHTTPDownload = this.FindPropertyAssert("iosAllowHTTPDownload");
      this.m_SubmitAnalytics = this.FindPropertyAssert("submitAnalytics");
      this.m_IOSURLSchemes = this.FindPropertyAssert("iOSURLSchemes");
      this.m_AotOptions = this.FindPropertyAssert("aotOptions");
      this.m_CameraUsageDescription = this.FindPropertyAssert("cameraUsageDescription");
      this.m_LocationUsageDescription = this.FindPropertyAssert("locationUsageDescription");
      this.m_MicrophoneUsageDescription = this.FindPropertyAssert("microphoneUsageDescription");
      this.m_EnableInternalProfiler = this.FindPropertyAssert("enableInternalProfiler");
      this.m_ActionOnDotNetUnhandledException = this.FindPropertyAssert("actionOnDotNetUnhandledException");
      this.m_LogObjCUncaughtExceptions = this.FindPropertyAssert("logObjCUncaughtExceptions");
      this.m_EnableCrashReportAPI = this.FindPropertyAssert("enableCrashReportAPI");
      this.m_EnableInputSystem = this.FindPropertyAssert("enableNativePlatformBackendsForNewInputSystem");
      this.m_DisableInputManager = this.FindPropertyAssert("disableOldInputManagerSupport");
      this.m_DefaultScreenWidth = this.FindPropertyAssert("defaultScreenWidth");
      this.m_DefaultScreenHeight = this.FindPropertyAssert("defaultScreenHeight");
      this.m_RunInBackground = this.FindPropertyAssert("runInBackground");
      this.m_DefaultScreenOrientation = this.FindPropertyAssert("defaultScreenOrientation");
      this.m_AllowedAutoRotateToPortrait = this.FindPropertyAssert("allowedAutorotateToPortrait");
      this.m_AllowedAutoRotateToPortraitUpsideDown = this.FindPropertyAssert("allowedAutorotateToPortraitUpsideDown");
      this.m_AllowedAutoRotateToLandscapeRight = this.FindPropertyAssert("allowedAutorotateToLandscapeRight");
      this.m_AllowedAutoRotateToLandscapeLeft = this.FindPropertyAssert("allowedAutorotateToLandscapeLeft");
      this.m_UseOSAutoRotation = this.FindPropertyAssert("useOSAutorotation");
      this.m_Use32BitDisplayBuffer = this.FindPropertyAssert("use32BitDisplayBuffer");
      this.m_DisableDepthAndStencilBuffers = this.FindPropertyAssert("disableDepthAndStencilBuffers");
      this.m_iosShowActivityIndicatorOnLoading = this.FindPropertyAssert("iosShowActivityIndicatorOnLoading");
      this.m_androidShowActivityIndicatorOnLoading = this.FindPropertyAssert("androidShowActivityIndicatorOnLoading");
      this.m_tizenShowActivityIndicatorOnLoading = this.FindPropertyAssert("tizenShowActivityIndicatorOnLoading");
      this.m_DefaultIsFullScreen = this.FindPropertyAssert("defaultIsFullScreen");
      this.m_DefaultIsNativeResolution = this.FindPropertyAssert("defaultIsNativeResolution");
      this.m_MacRetinaSupport = this.FindPropertyAssert("macRetinaSupport");
      this.m_CaptureSingleScreen = this.FindPropertyAssert("captureSingleScreen");
      this.m_DisplayResolutionDialog = this.FindPropertyAssert("displayResolutionDialog");
      this.m_SupportedAspectRatios = this.FindPropertyAssert("m_SupportedAspectRatios");
      this.m_UsePlayerLog = this.FindPropertyAssert("usePlayerLog");
      this.m_KeepLoadedShadersAlive = this.FindPropertyAssert("keepLoadedShadersAlive");
      this.m_PreloadedAssets = this.FindPropertyAssert("preloadedAssets");
      this.m_BakeCollisionMeshes = this.FindPropertyAssert("bakeCollisionMeshes");
      this.m_ResizableWindow = this.FindPropertyAssert("resizableWindow");
      this.m_UseMacAppStoreValidation = this.FindPropertyAssert("useMacAppStoreValidation");
      this.m_MacAppStoreCategory = this.FindPropertyAssert("macAppStoreCategory");
      this.m_D3D11FullscreenMode = this.FindPropertyAssert("d3d11FullscreenMode");
      this.m_VisibleInBackground = this.FindPropertyAssert("visibleInBackground");
      this.m_AllowFullscreenSwitch = this.FindPropertyAssert("allowFullscreenSwitch");
      this.m_MacFullscreenMode = this.FindPropertyAssert("macFullscreenMode");
      this.m_SkinOnGPU = this.FindPropertyAssert("gpuSkinning");
      this.m_GraphicsJobs = this.FindPropertyAssert("graphicsJobs");
      this.m_ForceSingleInstance = this.FindPropertyAssert("forceSingleInstance");
      this.m_RequireES31 = this.FindPropertyAssert("openGLRequireES31");
      this.m_RequireES31AEP = this.FindPropertyAssert("openGLRequireES31AEP");
      this.m_VideoMemoryForVertexBuffers = this.FindPropertyAssert("videoMemoryForVertexBuffers");
      this.m_SettingsExtensions = new ISettingEditorExtension[this.validPlatforms.Length];
      for (int index = 0; index < this.validPlatforms.Length; ++index)
      {
        string buildTargetGroup = ModuleManager.GetTargetStringFromBuildTargetGroup(this.validPlatforms[index].targetGroup);
        this.m_SettingsExtensions[index] = ModuleManager.GetEditorSettingsExtension(buildTargetGroup);
        if (this.m_SettingsExtensions[index] != null)
          this.m_SettingsExtensions[index].OnEnable(this);
      }
      for (int index = 0; index < this.m_SectionAnimators.Length; ++index)
        this.m_SectionAnimators[index] = new AnimBool(this.m_SelectedSection.value == index, new UnityAction(((Editor) this).Repaint));
      this.m_ShowDefaultIsNativeResolution.value = this.m_DefaultIsFullScreen.boolValue;
      this.m_ShowResolution.value = (!this.m_DefaultIsFullScreen.boolValue ? 0 : (this.m_DefaultIsNativeResolution.boolValue ? 1 : 0)) == 0;
      this.m_ShowDefaultIsNativeResolution.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowResolution.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_VRSettings = new PlayerSettingsEditorVR(this);
      this.splashScreenEditor.OnEnable();
      PlayerSettingsEditor.s_GraphicsDeviceLists.Clear();
    }

    public override bool UseDefaultMargins()
    {
      return false;
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins, new GUILayoutOption[0]);
      this.CommonSettings();
      EditorGUILayout.EndVertical();
      EditorGUILayout.Space();
      EditorGUI.BeginChangeCheck();
      int selectedPlatform = this.selectedPlatform;
      this.selectedPlatform = EditorGUILayout.BeginPlatformGrouping(this.validPlatforms, (GUIContent) null);
      if (EditorGUI.EndChangeCheck())
      {
        if (EditorGUI.s_DelayedTextEditor.IsEditingControl(this.scriptingDefinesControlID))
        {
          EditorGUI.EndEditingActiveTextField();
          GUIUtility.keyboardControl = 0;
          PlayerSettings.SetScriptingDefineSymbolsForGroup(this.validPlatforms[selectedPlatform].targetGroup, EditorGUI.s_DelayedTextEditor.text);
        }
        GUI.FocusControl("");
      }
      GUILayout.Label("Settings for " + this.validPlatforms[this.selectedPlatform].title.text);
      EditorGUIUtility.labelWidth = Mathf.Max(150f, EditorGUIUtility.labelWidth - 8f);
      BuildPlatform validPlatform = this.validPlatforms[this.selectedPlatform];
      BuildTargetGroup targetGroup = validPlatform.targetGroup;
      int num1 = 0;
      int num2 = (int) targetGroup;
      ISettingEditorExtension settingsExtension1 = this.m_SettingsExtensions[this.selectedPlatform];
      int sectionIndex1 = num1;
      int num3 = sectionIndex1 + 1;
      this.ResolutionSectionGUI((BuildTargetGroup) num2, settingsExtension1, sectionIndex1);
      int num4 = (int) targetGroup;
      ISettingEditorExtension settingsExtension2 = this.m_SettingsExtensions[this.selectedPlatform];
      int sectionIndex2 = num3;
      int num5 = sectionIndex2 + 1;
      this.IconSectionGUI((BuildTargetGroup) num4, settingsExtension2, sectionIndex2);
      PlayerSettingsSplashScreenEditor splashScreenEditor = this.m_SplashScreenEditor;
      BuildPlatform platform1 = validPlatform;
      int num6 = (int) targetGroup;
      ISettingEditorExtension settingsExtension3 = this.m_SettingsExtensions[this.selectedPlatform];
      int sectionIndex3 = num5;
      int num7 = sectionIndex3 + 1;
      splashScreenEditor.SplashSectionGUI(platform1, (BuildTargetGroup) num6, settingsExtension3, sectionIndex3);
      BuildPlatform platform2 = validPlatform;
      int num8 = (int) targetGroup;
      ISettingEditorExtension settingsExtension4 = this.m_SettingsExtensions[this.selectedPlatform];
      int sectionIndex4 = num7;
      int num9 = sectionIndex4 + 1;
      this.DebugAndCrashReportingGUI(platform2, (BuildTargetGroup) num8, settingsExtension4, sectionIndex4);
      BuildPlatform platform3 = validPlatform;
      int num10 = (int) targetGroup;
      ISettingEditorExtension settingsExtension5 = this.m_SettingsExtensions[this.selectedPlatform];
      int sectionIndex5 = num9;
      int num11 = sectionIndex5 + 1;
      this.OtherSectionGUI(platform3, (BuildTargetGroup) num10, settingsExtension5, sectionIndex5);
      int num12 = (int) targetGroup;
      ISettingEditorExtension settingsExtension6 = this.m_SettingsExtensions[this.selectedPlatform];
      int sectionIndex6 = num11;
      int num13 = sectionIndex6 + 1;
      this.PublishSectionGUI((BuildTargetGroup) num12, settingsExtension6, sectionIndex6);
      PlayerSettingsEditorVR vrSettings = this.m_VRSettings;
      int num14 = (int) targetGroup;
      int sectionIndex7 = num13;
      int num15 = sectionIndex7 + 1;
      vrSettings.XRSectionGUI((BuildTargetGroup) num14, sectionIndex7);
      if (num15 != 7)
        Debug.LogError((object) "Mismatched number of GUI sections.");
      EditorGUILayout.EndPlatformGrouping();
      this.serializedObject.ApplyModifiedProperties();
    }

    private void CommonSettings()
    {
      EditorGUILayout.PropertyField(this.m_CompanyName);
      EditorGUILayout.PropertyField(this.m_ProductName);
      EditorGUILayout.Space();
      GUI.changed = false;
      string platform = "";
      Texture2D[] icons = PlayerSettings.GetAllIconsForPlatform(platform);
      int[] kindsForPlatform = PlayerSettings.GetIconWidthsOfAllKindsForPlatform(platform);
      if (icons.Length != kindsForPlatform.Length)
        icons = new Texture2D[kindsForPlatform.Length];
      icons[0] = (Texture2D) EditorGUILayout.ObjectField(PlayerSettingsEditor.Styles.defaultIcon, (UnityEngine.Object) icons[0], typeof (Texture2D), false, new GUILayoutOption[0]);
      if (GUI.changed)
      {
        Undo.RecordObject(this.target, PlayerSettingsEditor.Styles.undoChangedIconString);
        PlayerSettings.SetIconsForPlatform(platform, icons);
      }
      GUILayout.Space(3f);
      Rect controlRect = EditorGUILayout.GetControlRect(true, 64f, new GUILayoutOption[0]);
      EditorGUI.BeginProperty(controlRect, PlayerSettingsEditor.Styles.defaultCursor, this.m_DefaultCursor);
      this.m_DefaultCursor.objectReferenceValue = EditorGUI.ObjectField(controlRect, PlayerSettingsEditor.Styles.defaultCursor, this.m_DefaultCursor.objectReferenceValue, typeof (Texture2D), false);
      EditorGUI.EndProperty();
      EditorGUI.PropertyField(EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), 0, PlayerSettingsEditor.Styles.cursorHotspot), this.m_CursorHotspot, GUIContent.none);
    }

    public bool BeginSettingsBox(int nr, GUIContent header)
    {
      bool enabled = GUI.enabled;
      GUI.enabled = true;
      EditorGUILayout.BeginVertical(PlayerSettingsEditor.Styles.categoryBox, new GUILayoutOption[0]);
      Rect rect = GUILayoutUtility.GetRect(20f, 18f);
      rect.x += 3f;
      rect.width += 6f;
      EditorGUI.BeginChangeCheck();
      bool flag = GUI.Toggle(rect, this.m_SelectedSection.value == nr, header, EditorStyles.inspectorTitlebarText);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_SelectedSection.value = !flag ? -1 : nr;
        GUIUtility.keyboardControl = 0;
      }
      this.m_SectionAnimators[nr].target = flag;
      GUI.enabled = enabled;
      return EditorGUILayout.BeginFadeGroup(this.m_SectionAnimators[nr].faded);
    }

    public void EndSettingsBox()
    {
      EditorGUILayout.EndFadeGroup();
      EditorGUILayout.EndVertical();
    }

    private void ShowNoSettings()
    {
      GUILayout.Label(PlayerSettingsEditor.Styles.notApplicableInfo, EditorStyles.miniLabel, new GUILayoutOption[0]);
    }

    public void ShowSharedNote()
    {
      GUILayout.Label(PlayerSettingsEditor.Styles.sharedBetweenPlatformsInfo, EditorStyles.miniLabel, new GUILayoutOption[0]);
    }

    private void IconSectionGUI(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension, int sectionIndex)
    {
      if (this.BeginSettingsBox(sectionIndex, PlayerSettingsEditor.Styles.iconTitle))
      {
        bool flag1 = true;
        if (settingsExtension != null)
          flag1 = settingsExtension.UsesStandardIcons();
        if (flag1)
        {
          bool flag2 = this.selectedPlatform < 0;
          BuildPlatform buildPlatform = (BuildPlatform) null;
          targetGroup = BuildTargetGroup.Standalone;
          string platform = "";
          if (!flag2)
          {
            buildPlatform = this.validPlatforms[this.selectedPlatform];
            targetGroup = buildPlatform.targetGroup;
            platform = buildPlatform.name;
          }
          bool enabled = GUI.enabled;
          switch (targetGroup)
          {
            case BuildTargetGroup.WebGL:
              this.ShowNoSettings();
              EditorGUILayout.Space();
              goto case BuildTargetGroup.WSA;
            case BuildTargetGroup.WSA:
              break;
            default:
              Texture2D[] icons = PlayerSettings.GetAllIconsForPlatform(platform);
              int[] kindsForPlatform1 = PlayerSettings.GetIconWidthsOfAllKindsForPlatform(platform);
              int[] kindsForPlatform2 = PlayerSettings.GetIconHeightsOfAllKindsForPlatform(platform);
              IconKind[] kindsForPlatform3 = PlayerSettings.GetIconKindsForPlatform(platform);
              bool flag3 = true;
              if (!flag2)
              {
                GUI.changed = false;
                flag3 = GUILayout.Toggle(icons.Length == kindsForPlatform1.Length, "Override for " + buildPlatform.title.text);
                GUI.enabled = enabled && flag3;
                if (GUI.changed || !flag3 && icons.Length > 0)
                {
                  icons = !flag3 ? new Texture2D[0] : new Texture2D[kindsForPlatform1.Length];
                  if (GUI.changed)
                    PlayerSettings.SetIconsForPlatform(platform, icons);
                }
              }
              GUI.changed = false;
              for (int index = 0; index < kindsForPlatform1.Length; ++index)
              {
                int num1 = Mathf.Min(96, kindsForPlatform1[index]);
                int b = (int) ((double) kindsForPlatform2[index] * (double) num1 / (double) kindsForPlatform1[index]);
                if (targetGroup == BuildTargetGroup.iPhone)
                {
                  if (kindsForPlatform3[index] == IconKind.Spotlight && kindsForPlatform3[index - 1] != IconKind.Spotlight)
                  {
                    Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, 20f);
                    GUI.Label(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, 20f), "Spotlight icons", EditorStyles.boldLabel);
                  }
                  if (kindsForPlatform3[index] == IconKind.Settings && kindsForPlatform3[index - 1] != IconKind.Settings)
                  {
                    Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, 20f);
                    GUI.Label(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, 20f), "Settings icons", EditorStyles.boldLabel);
                  }
                  if (kindsForPlatform3[index] == IconKind.Notification && kindsForPlatform3[index - 1] != IconKind.Notification)
                  {
                    Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, 20f);
                    GUI.Label(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, 20f), "Notification icons", EditorStyles.boldLabel);
                  }
                  if (kindsForPlatform3[index] == IconKind.Store && kindsForPlatform3[index - 1] != IconKind.Store)
                  {
                    Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, 20f);
                    GUI.Label(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, 20f), "App Store icons", EditorStyles.boldLabel);
                  }
                }
                Rect rect1 = GUILayoutUtility.GetRect(64f, (float) (Mathf.Max(64, b) + 6));
                float num2 = Mathf.Min(rect1.width, (float) ((double) EditorGUIUtility.labelWidth + 4.0 + 64.0 + 6.0 + 96.0));
                string text = kindsForPlatform1[index].ToString() + "x" + (object) kindsForPlatform2[index];
                GUI.Label(new Rect(rect1.x, rect1.y, (float) ((double) num2 - 96.0 - 64.0 - 12.0), 20f), text);
                if (flag3)
                {
                  int num3 = 64;
                  int num4 = (int) ((double) kindsForPlatform2[index] / (double) kindsForPlatform1[index] * 64.0);
                  icons[index] = (Texture2D) EditorGUI.ObjectField(new Rect((float) ((double) rect1.x + (double) num2 - 96.0 - 64.0 - 6.0), rect1.y, (float) num3, (float) num4), (UnityEngine.Object) icons[index], typeof (Texture2D), false);
                }
                Rect position = new Rect((float) ((double) rect1.x + (double) num2 - 96.0), rect1.y, (float) num1, (float) b);
                Texture2D forPlatformAtSize = PlayerSettings.GetIconForPlatformAtSize(platform, kindsForPlatform1[index], kindsForPlatform2[index], kindsForPlatform3[index]);
                if ((UnityEngine.Object) forPlatformAtSize != (UnityEngine.Object) null)
                  GUI.DrawTexture(position, (Texture) forPlatformAtSize);
                else
                  GUI.Box(position, "");
              }
              if (GUI.changed)
              {
                Undo.RecordObject(this.target, PlayerSettingsEditor.Styles.undoChangedIconString);
                PlayerSettings.SetIconsForPlatform(platform, icons);
              }
              GUI.enabled = enabled;
              if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
              {
                EditorGUILayout.PropertyField(this.m_UIPrerenderedIcon, PlayerSettingsEditor.Styles.UIPrerenderedIcon, new GUILayoutOption[0]);
                EditorGUILayout.Space();
              }
              goto case BuildTargetGroup.WSA;
          }
        }
        if (settingsExtension != null)
          settingsExtension.IconSectionGUI();
      }
      this.EndSettingsBox();
    }

    private static bool TargetSupportsOptionalBuiltinSplashScreen(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
    {
      if (settingsExtension != null)
        return settingsExtension.CanShowUnitySplashScreen();
      return targetGroup == BuildTargetGroup.Standalone;
    }

    private static bool TargetSupportsProtectedGraphicsMem(BuildTargetGroup targetGroup)
    {
      return targetGroup == BuildTargetGroup.Android;
    }

    public void ResolutionSectionGUI(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension, int sectionIndex = 0)
    {
      if (this.BeginSettingsBox(sectionIndex, PlayerSettingsEditor.Styles.resolutionPresentationTitle))
      {
        if (settingsExtension != null)
        {
          float h = 16f;
          float midWidth = (float) (80.0 + (double) EditorGUIUtility.fieldWidth + 5.0);
          float maxWidth = (float) (80.0 + (double) EditorGUIUtility.fieldWidth + 5.0);
          settingsExtension.ResolutionSectionGUI(h, midWidth, maxWidth);
        }
        if (targetGroup == BuildTargetGroup.Standalone)
        {
          GUILayout.Label(PlayerSettingsEditor.Styles.resolutionTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_DefaultIsFullScreen, PlayerSettingsEditor.Styles.defaultIsFullScreen, new GUILayoutOption[0]);
          this.m_ShowDefaultIsNativeResolution.target = this.m_DefaultIsFullScreen.boolValue;
          if (EditorGUILayout.BeginFadeGroup(this.m_ShowDefaultIsNativeResolution.faded))
            EditorGUILayout.PropertyField(this.m_DefaultIsNativeResolution);
          if ((double) this.m_ShowDefaultIsNativeResolution.faded != 0.0 && (double) this.m_ShowDefaultIsNativeResolution.faded != 1.0)
            EditorGUILayout.EndFadeGroup();
          this.m_ShowResolution.target = (!this.m_DefaultIsFullScreen.boolValue ? 0 : (this.m_DefaultIsNativeResolution.boolValue ? 1 : 0)) == 0;
          if (EditorGUILayout.BeginFadeGroup(this.m_ShowResolution.faded))
          {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(this.m_DefaultScreenWidth, PlayerSettingsEditor.Styles.defaultScreenWidth, new GUILayoutOption[0]);
            if (EditorGUI.EndChangeCheck() && this.m_DefaultScreenWidth.intValue < 1)
              this.m_DefaultScreenWidth.intValue = 1;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(this.m_DefaultScreenHeight, PlayerSettingsEditor.Styles.defaultScreenHeight, new GUILayoutOption[0]);
            if (EditorGUI.EndChangeCheck() && this.m_DefaultScreenHeight.intValue < 1)
              this.m_DefaultScreenHeight.intValue = 1;
          }
          if ((double) this.m_ShowResolution.faded != 0.0 && (double) this.m_ShowResolution.faded != 1.0)
            EditorGUILayout.EndFadeGroup();
        }
        if (targetGroup == BuildTargetGroup.Standalone)
        {
          EditorGUILayout.PropertyField(this.m_MacRetinaSupport, PlayerSettingsEditor.Styles.macRetinaSupport, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_RunInBackground, PlayerSettingsEditor.Styles.runInBackground, new GUILayoutOption[0]);
        }
        if (settingsExtension != null && settingsExtension.SupportsOrientation())
        {
          GUILayout.Label(PlayerSettingsEditor.Styles.orientationTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
          using (new EditorGUI.DisabledScope(PlayerSettings.virtualRealitySupported))
          {
            EditorGUILayout.PropertyField(this.m_DefaultScreenOrientation, PlayerSettingsEditor.Styles.defaultScreenOrientation, new GUILayoutOption[0]);
            if (PlayerSettings.virtualRealitySupported)
              EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.VRSupportOverridenInfo.text, MessageType.Info);
            if (this.m_DefaultScreenOrientation.enumValueIndex == 4)
            {
              if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.Tizen)
                EditorGUILayout.PropertyField(this.m_UseOSAutoRotation, PlayerSettingsEditor.Styles.useOSAutoRotation, new GUILayoutOption[0]);
              ++EditorGUI.indentLevel;
              GUILayout.Label(PlayerSettingsEditor.Styles.allowedOrientationTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
              if (!this.m_AllowedAutoRotateToPortrait.boolValue && !this.m_AllowedAutoRotateToPortraitUpsideDown.boolValue && !this.m_AllowedAutoRotateToLandscapeRight.boolValue && !this.m_AllowedAutoRotateToLandscapeLeft.boolValue)
              {
                this.m_AllowedAutoRotateToPortrait.boolValue = true;
                Debug.LogError((object) "All orientations are disabled. Allowing portrait");
              }
              EditorGUILayout.PropertyField(this.m_AllowedAutoRotateToPortrait, PlayerSettingsEditor.Styles.allowedAutoRotateToPortrait, new GUILayoutOption[0]);
              EditorGUILayout.PropertyField(this.m_AllowedAutoRotateToPortraitUpsideDown, PlayerSettingsEditor.Styles.allowedAutoRotateToPortraitUpsideDown, new GUILayoutOption[0]);
              EditorGUILayout.PropertyField(this.m_AllowedAutoRotateToLandscapeRight, PlayerSettingsEditor.Styles.allowedAutoRotateToLandscapeRight, new GUILayoutOption[0]);
              EditorGUILayout.PropertyField(this.m_AllowedAutoRotateToLandscapeLeft, PlayerSettingsEditor.Styles.allowedAutoRotateToLandscapeLeft, new GUILayoutOption[0]);
              --EditorGUI.indentLevel;
            }
          }
        }
        if (targetGroup == BuildTargetGroup.iPhone)
        {
          GUILayout.Label(PlayerSettingsEditor.Styles.multitaskingSupportTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_UIRequiresFullScreen, PlayerSettingsEditor.Styles.UIRequiresFullScreen, new GUILayoutOption[0]);
          EditorGUILayout.Space();
          GUILayout.Label(PlayerSettingsEditor.Styles.statusBarTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_UIStatusBarHidden, PlayerSettingsEditor.Styles.UIStatusBarHidden, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_UIStatusBarStyle, PlayerSettingsEditor.Styles.UIStatusBarStyle, new GUILayoutOption[0]);
          EditorGUILayout.Space();
        }
        EditorGUILayout.Space();
        if (targetGroup == BuildTargetGroup.Standalone)
        {
          GUILayout.Label(PlayerSettingsEditor.Styles.standalonePlayerOptionsTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_CaptureSingleScreen);
          EditorGUILayout.PropertyField(this.m_DisplayResolutionDialog);
          EditorGUILayout.PropertyField(this.m_UsePlayerLog);
          EditorGUILayout.PropertyField(this.m_ResizableWindow);
          EditorGUILayout.PropertyField(this.m_MacFullscreenMode);
          EditorGUILayout.PropertyField(this.m_D3D11FullscreenMode, PlayerSettingsEditor.Styles.D3D11FullscreenMode, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_VisibleInBackground, PlayerSettingsEditor.Styles.visibleInBackground, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_AllowFullscreenSwitch, PlayerSettingsEditor.Styles.allowFullscreenSwitch, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_ForceSingleInstance);
          EditorGUILayout.PropertyField(this.m_SupportedAspectRatios, true, new GUILayoutOption[0]);
          EditorGUILayout.Space();
        }
        if (this.IsMobileTarget(targetGroup))
        {
          if (targetGroup != BuildTargetGroup.Tizen && targetGroup != BuildTargetGroup.iPhone && targetGroup != BuildTargetGroup.tvOS)
            EditorGUILayout.PropertyField(this.m_Use32BitDisplayBuffer, PlayerSettingsEditor.Styles.use32BitDisplayBuffer, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_DisableDepthAndStencilBuffers, PlayerSettingsEditor.Styles.disableDepthAndStencilBuffers, new GUILayoutOption[0]);
        }
        if (targetGroup == BuildTargetGroup.iPhone)
          EditorGUILayout.PropertyField(this.m_iosShowActivityIndicatorOnLoading, PlayerSettingsEditor.Styles.iosShowActivityIndicatorOnLoading, new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.Android)
          EditorGUILayout.PropertyField(this.m_androidShowActivityIndicatorOnLoading, PlayerSettingsEditor.Styles.androidShowActivityIndicatorOnLoading, new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.Tizen)
          EditorGUILayout.PropertyField(this.m_tizenShowActivityIndicatorOnLoading, EditorGUIUtility.TextContent("Show Loading Indicator"), new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.Tizen)
          EditorGUILayout.Space();
        this.ShowSharedNote();
      }
      this.EndSettingsBox();
    }

    private void AddGraphicsDeviceMenuSelected(object userData, string[] options, int selected)
    {
      BuildTarget platform = (BuildTarget) userData;
      GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(platform);
      if (graphicsApIs == null)
        return;
      GraphicsDeviceType graphicsDeviceType = (GraphicsDeviceType) Enum.Parse(typeof (GraphicsDeviceType), options[selected], true);
      List<GraphicsDeviceType> list = ((IEnumerable<GraphicsDeviceType>) graphicsApIs).ToList<GraphicsDeviceType>();
      list.Add(graphicsDeviceType);
      GraphicsDeviceType[] array = list.ToArray();
      PlayerSettings.SetGraphicsAPIs(platform, array);
    }

    private void AddGraphicsDeviceElement(BuildTarget target, Rect rect, ReorderableList list)
    {
      GraphicsDeviceType[] supportedGraphicsApIs = PlayerSettings.GetSupportedGraphicsAPIs(target);
      if (supportedGraphicsApIs == null || supportedGraphicsApIs.Length == 0)
        return;
      string[] options = new string[supportedGraphicsApIs.Length];
      bool[] enabled = new bool[supportedGraphicsApIs.Length];
      for (int index = 0; index < supportedGraphicsApIs.Length; ++index)
      {
        options[index] = supportedGraphicsApIs[index].ToString();
        enabled[index] = !list.list.Contains((object) supportedGraphicsApIs[index]);
      }
      EditorUtility.DisplayCustomMenu(rect, options, enabled, (int[]) null, new EditorUtility.SelectMenuItemFunction(this.AddGraphicsDeviceMenuSelected), (object) target);
    }

    private bool CanRemoveGraphicsDeviceElement(ReorderableList list)
    {
      return list.list.Count >= 2;
    }

    private void RemoveGraphicsDeviceElement(BuildTarget target, ReorderableList list)
    {
      GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(target);
      if (graphicsApIs == null)
        return;
      if (graphicsApIs.Length < 2)
      {
        EditorApplication.Beep();
      }
      else
      {
        List<GraphicsDeviceType> list1 = ((IEnumerable<GraphicsDeviceType>) graphicsApIs).ToList<GraphicsDeviceType>();
        list1.RemoveAt(list.index);
        GraphicsDeviceType[] array = list1.ToArray();
        this.ApplyChangedGraphicsAPIList(target, array, list.index == 0);
      }
    }

    private void ReorderGraphicsDeviceElement(BuildTarget target, ReorderableList list)
    {
      GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(target);
      GraphicsDeviceType[] array = ((List<GraphicsDeviceType>) list.list).ToArray();
      bool firstEntryChanged = graphicsApIs[0] != array[0];
      this.ApplyChangedGraphicsAPIList(target, array, firstEntryChanged);
    }

    private PlayerSettingsEditor.ChangeGraphicsApiAction CheckApplyGraphicsAPIList(BuildTarget target, bool firstEntryChanged)
    {
      bool doChange = true;
      bool doReload = false;
      if (firstEntryChanged && PlayerSettingsEditor.WillEditorUseFirstGraphicsAPI(target))
      {
        doChange = false;
        if (EditorUtility.DisplayDialog("Changing editor graphics device", "Changing active graphics API requires reloading all graphics objects, it might take a while", "Apply", "Cancel") && EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
          doChange = doReload = true;
      }
      return new PlayerSettingsEditor.ChangeGraphicsApiAction(doChange, doReload);
    }

    private void ApplyChangeGraphicsApiAction(BuildTarget target, GraphicsDeviceType[] apis, PlayerSettingsEditor.ChangeGraphicsApiAction action)
    {
      if (action.changeList)
        PlayerSettings.SetGraphicsAPIs(target, apis);
      else
        PlayerSettingsEditor.s_GraphicsDeviceLists.Remove(target);
      if (!action.reloadGfx)
        return;
      ShaderUtil.RecreateGfxDevice();
      GUIUtility.ExitGUI();
    }

    private void ApplyChangedGraphicsAPIList(BuildTarget target, GraphicsDeviceType[] apis, bool firstEntryChanged)
    {
      PlayerSettingsEditor.ChangeGraphicsApiAction action = this.CheckApplyGraphicsAPIList(target, firstEntryChanged);
      this.ApplyChangeGraphicsApiAction(target, apis, action);
    }

    private void DrawGraphicsDeviceElement(BuildTarget target, Rect rect, int index, bool selected, bool focused)
    {
      string text = PlayerSettingsEditor.s_GraphicsDeviceLists[target].list[index].ToString();
      if (text == "Direct3D12")
        text = "Direct3D12 (Experimental)";
      if (text == "Vulkan" && target != BuildTarget.Android)
        text = "Vulkan (Experimental)";
      if (target == BuildTarget.WebGL)
      {
        if (text == "OpenGLES3")
          text = "WebGL 2.0";
        else if (text == "OpenGLES2")
          text = "WebGL 1.0";
      }
      GUI.Label(rect, text, EditorStyles.label);
    }

    private static bool WillEditorUseFirstGraphicsAPI(BuildTarget targetPlatform)
    {
      return Application.platform == RuntimePlatform.WindowsEditor && targetPlatform == BuildTarget.StandaloneWindows || Application.platform == RuntimePlatform.OSXEditor && targetPlatform == BuildTarget.StandaloneOSX;
    }

    private void OpenGLES31OptionsGUI(BuildTargetGroup targetGroup, BuildTarget targetPlatform)
    {
      if (targetGroup != BuildTargetGroup.Android)
        return;
      GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(targetPlatform);
      if (!((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES3) || ((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES2))
        return;
      EditorGUILayout.PropertyField(this.m_RequireES31, PlayerSettingsEditor.Styles.require31, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_RequireES31AEP, PlayerSettingsEditor.Styles.requireAEP, new GUILayoutOption[0]);
    }

    private void GraphicsAPIsGUIOnePlatform(BuildTargetGroup targetGroup, BuildTarget targetPlatform, string platformTitle)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PlayerSettingsEditor.\u003CGraphicsAPIsGUIOnePlatform\u003Ec__AnonStorey0 platformCAnonStorey0 = new PlayerSettingsEditor.\u003CGraphicsAPIsGUIOnePlatform\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      platformCAnonStorey0.targetPlatform = targetPlatform;
      // ISSUE: reference to a compiler-generated field
      platformCAnonStorey0.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      GraphicsDeviceType[] supportedGraphicsApIs = PlayerSettings.GetSupportedGraphicsAPIs(platformCAnonStorey0.targetPlatform);
      if (supportedGraphicsApIs == null || supportedGraphicsApIs.Length < 2 || targetGroup == BuildTargetGroup.XboxOne)
        return;
      EditorGUI.BeginChangeCheck();
      // ISSUE: reference to a compiler-generated field
      bool defaultGraphicsApIs = PlayerSettings.GetUseDefaultGraphicsAPIs(platformCAnonStorey0.targetPlatform);
      bool automatic = EditorGUILayout.Toggle("Auto Graphics API" + (platformTitle ?? string.Empty), defaultGraphicsApIs, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(this.target, PlayerSettingsEditor.Styles.undoChangedGraphicsAPIString);
        // ISSUE: reference to a compiler-generated field
        PlayerSettings.SetUseDefaultGraphicsAPIs(platformCAnonStorey0.targetPlatform, automatic);
      }
      if (automatic)
        return;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PlayerSettingsEditor.\u003CGraphicsAPIsGUIOnePlatform\u003Ec__AnonStorey1 platformCAnonStorey1 = new PlayerSettingsEditor.\u003CGraphicsAPIsGUIOnePlatform\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      platformCAnonStorey1.\u003C\u003Ef__ref\u00240 = platformCAnonStorey0;
      // ISSUE: reference to a compiler-generated field
      if (PlayerSettingsEditor.WillEditorUseFirstGraphicsAPI(platformCAnonStorey0.targetPlatform))
        EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.recordingInfo.text, MessageType.Info, true);
      // ISSUE: reference to a compiler-generated field
      platformCAnonStorey1.displayTitle = "Graphics APIs";
      if (platformTitle != null)
      {
        // ISSUE: reference to a compiler-generated field
        platformCAnonStorey1.displayTitle += platformTitle;
      }
      // ISSUE: reference to a compiler-generated field
      if (!PlayerSettingsEditor.s_GraphicsDeviceLists.ContainsKey(platformCAnonStorey0.targetPlatform))
      {
        // ISSUE: reference to a compiler-generated field
        GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(platformCAnonStorey0.targetPlatform);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        PlayerSettingsEditor.s_GraphicsDeviceLists.Add(platformCAnonStorey0.targetPlatform, new ReorderableList(graphicsApIs == null ? (IList) new List<GraphicsDeviceType>() : (IList) ((IEnumerable<GraphicsDeviceType>) graphicsApIs).ToList<GraphicsDeviceType>(), typeof (GraphicsDeviceType), true, true, true, true)
        {
          onAddDropdownCallback = new ReorderableList.AddDropdownCallbackDelegate(platformCAnonStorey1.\u003C\u003Em__0),
          onCanRemoveCallback = new ReorderableList.CanRemoveCallbackDelegate(this.CanRemoveGraphicsDeviceElement),
          onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(platformCAnonStorey1.\u003C\u003Em__1),
          onReorderCallback = new ReorderableList.ReorderCallbackDelegate(platformCAnonStorey1.\u003C\u003Em__2),
          drawElementCallback = new ReorderableList.ElementCallbackDelegate(platformCAnonStorey1.\u003C\u003Em__3),
          drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(platformCAnonStorey1.\u003C\u003Em__4),
          elementHeight = 16f
        });
      }
      // ISSUE: reference to a compiler-generated field
      PlayerSettingsEditor.s_GraphicsDeviceLists[platformCAnonStorey0.targetPlatform].DoLayoutList();
      // ISSUE: reference to a compiler-generated field
      this.OpenGLES31OptionsGUI(targetGroup, platformCAnonStorey0.targetPlatform);
    }

    private void GraphicsAPIsGUI(BuildTargetGroup targetGroup, BuildTarget target)
    {
      if (targetGroup == BuildTargetGroup.Standalone)
      {
        this.GraphicsAPIsGUIOnePlatform(targetGroup, BuildTarget.StandaloneWindows, " for Windows");
        this.GraphicsAPIsGUIOnePlatform(targetGroup, BuildTarget.StandaloneOSX, " for Mac");
        this.GraphicsAPIsGUIOnePlatform(targetGroup, BuildTarget.StandaloneLinuxUniversal, " for Linux");
      }
      else
        this.GraphicsAPIsGUIOnePlatform(targetGroup, target, (string) null);
    }

    private static bool IsColorGamutSupportedOnTargetGroup(BuildTargetGroup targetGroup, ColorGamut gamut)
    {
      return gamut == ColorGamut.sRGB || PlayerSettingsEditor.s_SupportedColorGamuts.ContainsKey(targetGroup) && PlayerSettingsEditor.s_SupportedColorGamuts[targetGroup].Contains(gamut);
    }

    private static string GetColorGamutDisplayString(BuildTargetGroup targetGroup, ColorGamut gamut)
    {
      string str = gamut.ToString();
      if (!PlayerSettingsEditor.IsColorGamutSupportedOnTargetGroup(targetGroup, gamut))
        str += " (not supported on this platform)";
      return str;
    }

    private void AddColorGamutElement(BuildTargetGroup targetGroup, Rect rect, ReorderableList list)
    {
      ColorGamut[] colorGamutArray = new ColorGamut[2]{ ColorGamut.sRGB, ColorGamut.DisplayP3 };
      string[] options = new string[colorGamutArray.Length];
      bool[] enabled = new bool[colorGamutArray.Length];
      for (int index = 0; index < colorGamutArray.Length; ++index)
      {
        options[index] = PlayerSettingsEditor.GetColorGamutDisplayString(targetGroup, colorGamutArray[index]);
        enabled[index] = !list.list.Contains((object) colorGamutArray[index]);
      }
      EditorUtility.DisplayCustomMenu(rect, options, enabled, (int[]) null, new EditorUtility.SelectMenuItemFunction(this.AddColorGamutMenuSelected), (object) colorGamutArray);
    }

    private void AddColorGamutMenuSelected(object userData, string[] options, int selected)
    {
      ColorGamut[] colorGamutArray = (ColorGamut[]) userData;
      List<ColorGamut> list = ((IEnumerable<ColorGamut>) PlayerSettings.GetColorGamuts()).ToList<ColorGamut>();
      list.Add(colorGamutArray[selected]);
      PlayerSettings.SetColorGamuts(list.ToArray());
    }

    private bool CanRemoveColorGamutElement(ReorderableList list)
    {
      return ((List<ColorGamut>) list.list)[list.index] != ColorGamut.sRGB;
    }

    private void RemoveColorGamutElement(ReorderableList list)
    {
      List<ColorGamut> list1 = ((IEnumerable<ColorGamut>) PlayerSettings.GetColorGamuts()).ToList<ColorGamut>();
      if (list1.Count < 2)
      {
        EditorApplication.Beep();
      }
      else
      {
        list1.RemoveAt(list.index);
        PlayerSettings.SetColorGamuts(list1.ToArray());
      }
    }

    private void ReorderColorGamutElement(ReorderableList list)
    {
      PlayerSettings.SetColorGamuts(((List<ColorGamut>) list.list).ToArray());
    }

    private void DrawColorGamutElement(BuildTargetGroup targetGroup, Rect rect, int index, bool selected, bool focused)
    {
      object obj = PlayerSettingsEditor.s_ColorGamutList.list[index];
      GUI.Label(rect, PlayerSettingsEditor.GetColorGamutDisplayString(targetGroup, (ColorGamut) obj), EditorStyles.label);
    }

    private void ColorGamutGUI(BuildTargetGroup targetGroup)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PlayerSettingsEditor.\u003CColorGamutGUI\u003Ec__AnonStorey2 gamutGuiCAnonStorey2 = new PlayerSettingsEditor.\u003CColorGamutGUI\u003Ec__AnonStorey2();
      // ISSUE: reference to a compiler-generated field
      gamutGuiCAnonStorey2.targetGroup = targetGroup;
      // ISSUE: reference to a compiler-generated field
      gamutGuiCAnonStorey2.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      if (!PlayerSettingsEditor.s_SupportedColorGamuts.ContainsKey(gamutGuiCAnonStorey2.targetGroup))
        return;
      if (PlayerSettingsEditor.s_ColorGamutList == null)
      {
        ColorGamut[] colorGamuts = PlayerSettings.GetColorGamuts();
        PlayerSettingsEditor.s_ColorGamutList = new ReorderableList(colorGamuts == null ? (IList) new List<ColorGamut>() : (IList) ((IEnumerable<ColorGamut>) colorGamuts).ToList<ColorGamut>(), typeof (ColorGamut), true, true, true, true)
        {
          onCanRemoveCallback = new ReorderableList.CanRemoveCallbackDelegate(this.CanRemoveColorGamutElement),
          onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemoveColorGamutElement),
          onReorderCallback = new ReorderableList.ReorderCallbackDelegate(this.ReorderColorGamutElement),
          elementHeight = 16f
        };
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      gamutGuiCAnonStorey2.header = gamutGuiCAnonStorey2.targetGroup != BuildTargetGroup.Standalone ? PlayerSettingsEditor.Styles.colorGamut : PlayerSettingsEditor.Styles.colorGamutForMac;
      // ISSUE: reference to a compiler-generated method
      PlayerSettingsEditor.s_ColorGamutList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(gamutGuiCAnonStorey2.\u003C\u003Em__0);
      // ISSUE: reference to a compiler-generated method
      PlayerSettingsEditor.s_ColorGamutList.onAddDropdownCallback = new ReorderableList.AddDropdownCallbackDelegate(gamutGuiCAnonStorey2.\u003C\u003Em__1);
      // ISSUE: reference to a compiler-generated method
      PlayerSettingsEditor.s_ColorGamutList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(gamutGuiCAnonStorey2.\u003C\u003Em__2);
      PlayerSettingsEditor.s_ColorGamutList.DoLayoutList();
    }

    public void DebugAndCrashReportingGUI(BuildPlatform platform, BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension, int sectionIndex = 3)
    {
      if (targetGroup != BuildTargetGroup.iPhone && targetGroup != BuildTargetGroup.tvOS)
        return;
      if (this.BeginSettingsBox(sectionIndex, PlayerSettingsEditor.Styles.debuggingCrashReportingTitle))
      {
        GUILayout.Label(PlayerSettingsEditor.Styles.debuggingTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_EnableInternalProfiler, PlayerSettingsEditor.Styles.enableInternalProfiler, new GUILayoutOption[0]);
        EditorGUILayout.Space();
        GUILayout.Label(PlayerSettingsEditor.Styles.crashReportingTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_ActionOnDotNetUnhandledException, PlayerSettingsEditor.Styles.actionOnDotNetUnhandledException, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_LogObjCUncaughtExceptions, PlayerSettingsEditor.Styles.logObjCUncaughtExceptions, new GUILayoutOption[0]);
        GUIContent guiContent = PlayerSettingsEditor.Styles.enableCrashReportAPI;
        bool disabled = false;
        if (CrashReportingSettings.enabled)
        {
          guiContent = new GUIContent(guiContent);
          disabled = true;
          guiContent.tooltip = "CrashReport API must be enabled for Performance Reporting service.";
          this.m_EnableCrashReportAPI.boolValue = true;
        }
        EditorGUI.BeginDisabledGroup(disabled);
        EditorGUILayout.PropertyField(this.m_EnableCrashReportAPI, guiContent, new GUILayoutOption[0]);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space();
      }
      this.EndSettingsBox();
    }

    public static void BuildDisabledEnumPopup(GUIContent selected, GUIContent uiString)
    {
      using (new EditorGUI.DisabledScope(true))
        EditorGUI.Popup(EditorGUILayout.GetControlRect(true, new GUILayoutOption[0]), uiString, 0, new GUIContent[1]
        {
          selected
        });
    }

    public static void BuildEnumPopup<T>(SerializedProperty prop, GUIContent uiString, T[] options, GUIContent[] optionNames)
    {
      T intValue = (T) (ValueType) prop.intValue;
      T obj = PlayerSettingsEditor.BuildEnumPopup<T>(uiString, intValue, options, optionNames);
      if (obj.Equals((object) intValue))
        return;
      prop.intValue = (int) (object) obj;
      prop.serializedObject.ApplyModifiedProperties();
    }

    public static T BuildEnumPopup<T>(GUIContent uiString, T selected, T[] options, GUIContent[] optionNames)
    {
      int selectedIndex = 0;
      for (int index = 1; index < options.Length; ++index)
      {
        if (selected.Equals((object) options[index]))
        {
          selectedIndex = index;
          break;
        }
      }
      int index1 = EditorGUILayout.Popup(uiString, selectedIndex, optionNames, new GUILayoutOption[0]);
      return options[index1];
    }

    public void OtherSectionGUI(BuildPlatform platform, BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension, int sectionIndex = 4)
    {
      if (this.BeginSettingsBox(sectionIndex, PlayerSettingsEditor.Styles.otherSettingsTitle))
      {
        this.OtherSectionRenderingGUI(platform, targetGroup, settingsExtension);
        this.OtherSectionIdentificationGUI(targetGroup, settingsExtension);
        this.OtherSectionConfigurationGUI(targetGroup, settingsExtension);
        this.OtherSectionOptimizationGUI(targetGroup);
        this.OtherSectionLoggingGUI();
        this.ShowSharedNote();
      }
      this.EndSettingsBox();
    }

    private void OtherSectionRenderingGUI(BuildPlatform platform, BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
    {
      GUILayout.Label(PlayerSettingsEditor.Styles.renderingTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
      if (targetGroup == BuildTargetGroup.Standalone || targetGroup == BuildTargetGroup.iPhone || (targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.Android) || (targetGroup == BuildTargetGroup.PS4 || targetGroup == BuildTargetGroup.XboxOne || (targetGroup == BuildTargetGroup.WSA || targetGroup == BuildTargetGroup.WiiU)) || (targetGroup == BuildTargetGroup.WebGL || targetGroup == BuildTargetGroup.Switch))
      {
        using (new EditorGUI.DisabledScope(EditorApplication.isPlaying))
        {
          EditorGUI.BeginChangeCheck();
          EditorGUILayout.PropertyField(this.m_ActiveColorSpace, PlayerSettingsEditor.Styles.activeColorSpace, new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck())
          {
            this.serializedObject.ApplyModifiedProperties();
            GUIUtility.ExitGUI();
          }
        }
        if (PlayerSettings.colorSpace == ColorSpace.Linear)
        {
          if (targetGroup == BuildTargetGroup.iPhone)
          {
            GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.iOS);
            bool flag = !((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES3) && !((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES2);
            Version version1 = new Version(8, 0);
            Version version2 = !string.IsNullOrEmpty(PlayerSettings.iOS.targetOSVersionString) ? new Version(PlayerSettings.iOS.targetOSVersionString) : new Version(6, 0);
            if (!flag || version2 < version1)
              EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.colorSpaceIOSWarning.text, MessageType.Warning);
          }
          if (targetGroup == BuildTargetGroup.tvOS)
          {
            GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.tvOS);
            if (((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES3) || ((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES2))
              EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.colorSpaceTVOSWarning.text, MessageType.Warning);
          }
          if (targetGroup == BuildTargetGroup.Android)
          {
            GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
            if (PlayerSettings.Android.blitType == AndroidBlitType.Never || (!((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.Vulkan) && !((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES3) || ((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES2)) || PlayerSettings.Android.minSdkVersion < AndroidSdkVersions.AndroidApiLevel18)
              EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.colorSpaceAndroidWarning.text, MessageType.Warning);
          }
          if (targetGroup == BuildTargetGroup.WebGL)
          {
            GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.WebGL);
            if (!((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES3) || ((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES2))
              EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.colorSpaceWebGLWarning.text, MessageType.Error);
          }
        }
      }
      this.GraphicsAPIsGUI(targetGroup, platform.defaultTarget);
      this.ColorGamutGUI(targetGroup);
      if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
        this.m_MetalForceHardShadows.boolValue = EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.metalForceHardShadows, this.m_MetalForceHardShadows.boolValue, new GUILayoutOption[0]);
      if (Application.platform == RuntimePlatform.OSXEditor && (targetGroup == BuildTargetGroup.Standalone || targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS))
      {
        bool flag1 = this.m_MetalEditorSupport.boolValue || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal;
        bool flag2 = EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.metalEditorSupport, flag1, new GUILayoutOption[0]);
        if (flag2 != flag1)
        {
          if (Application.platform == RuntimePlatform.OSXEditor)
          {
            GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneOSX);
            bool firstEntryChanged = graphicsApIs[0] != SystemInfo.graphicsDeviceType;
            if (!flag2 && SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal)
              firstEntryChanged = true;
            if (flag2 && graphicsApIs[0] == GraphicsDeviceType.Metal)
              firstEntryChanged = true;
            PlayerSettingsEditor.ChangeGraphicsApiAction action = this.CheckApplyGraphicsAPIList(BuildTarget.StandaloneOSX, firstEntryChanged);
            if (action.changeList)
            {
              this.m_MetalEditorSupport.boolValue = flag2;
              this.serializedObject.ApplyModifiedProperties();
            }
            this.ApplyChangeGraphicsApiAction(BuildTarget.StandaloneOSX, graphicsApIs, action);
          }
          else
          {
            this.m_MetalEditorSupport.boolValue = flag2;
            this.serializedObject.ApplyModifiedProperties();
          }
        }
        if (this.m_MetalEditorSupport.boolValue)
          this.m_MetalAPIValidation.boolValue = EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.metalAPIValidation, this.m_MetalAPIValidation.boolValue, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_MetalFramebufferOnly, PlayerSettingsEditor.Styles.metalFramebufferOnly, new GUILayoutOption[0]);
      }
      if (settingsExtension != null && settingsExtension.SupportsMultithreadedRendering())
        settingsExtension.MultithreadedRenderingGUI(targetGroup);
      bool flag3 = true;
      bool flag4 = true;
      if (settingsExtension != null)
      {
        flag3 = settingsExtension.SupportsStaticBatching();
        flag4 = settingsExtension.SupportsDynamicBatching();
      }
      int staticBatching;
      int dynamicBatching;
      PlayerSettings.GetBatchingForPlatform(platform.defaultTarget, out staticBatching, out dynamicBatching);
      bool flag5 = false;
      if (!flag3 && staticBatching == 1)
      {
        staticBatching = 0;
        flag5 = true;
      }
      if (!flag4 && dynamicBatching == 1)
      {
        dynamicBatching = 0;
        flag5 = true;
      }
      if (flag5)
        PlayerSettings.SetBatchingForPlatform(platform.defaultTarget, staticBatching, dynamicBatching);
      EditorGUI.BeginChangeCheck();
      using (new EditorGUI.DisabledScope(!flag3))
      {
        if (GUI.enabled)
          staticBatching = !EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.staticBatching, staticBatching != 0, new GUILayoutOption[0]) ? 0 : 1;
        else
          EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.staticBatching, false, new GUILayoutOption[0]);
      }
      using (new EditorGUI.DisabledScope(!flag4))
        dynamicBatching = !EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.dynamicBatching, dynamicBatching != 0, new GUILayoutOption[0]) ? 0 : 1;
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(this.target, PlayerSettingsEditor.Styles.undoChangedBatchingString);
        PlayerSettings.SetBatchingForPlatform(platform.defaultTarget, staticBatching, dynamicBatching);
      }
      bool flag6 = false;
      bool flag7 = false;
      bool flag8 = targetGroup == BuildTargetGroup.Standalone;
      if (settingsExtension != null)
      {
        flag6 = settingsExtension.SupportsHighDynamicRangeDisplays();
        flag7 = settingsExtension.SupportsGfxJobModes();
        flag8 = flag8 || settingsExtension.SupportsCustomLightmapEncoding();
      }
      if (targetGroup == BuildTargetGroup.WiiU || targetGroup == BuildTargetGroup.Standalone || (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS) || (targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.PSP2 || (targetGroup == BuildTargetGroup.PS4 || targetGroup == BuildTargetGroup.PSM)) || (targetGroup == BuildTargetGroup.XboxOne || targetGroup == BuildTargetGroup.WSA || targetGroup == BuildTargetGroup.Switch))
      {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(this.m_SkinOnGPU, targetGroup == BuildTargetGroup.PS4 || targetGroup == BuildTargetGroup.Switch ? PlayerSettingsEditor.Styles.skinOnGPUPS4 : PlayerSettingsEditor.Styles.skinOnGPU, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
          ShaderUtil.RecreateSkinnedMeshResources();
      }
      if (targetGroup == BuildTargetGroup.Android && PlayerSettings.gpuSkinning)
        EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.skinOnGPUAndroidWarning.text, MessageType.Warning);
      EditorGUILayout.PropertyField(this.m_GraphicsJobs, PlayerSettingsEditor.Styles.graphicsJobs, new GUILayoutOption[0]);
      if (flag7)
      {
        using (new EditorGUI.DisabledScope(!this.m_GraphicsJobs.boolValue))
        {
          GraphicsJobMode graphicsJobMode1 = PlayerSettings.graphicsJobMode;
          GraphicsJobMode graphicsJobMode2 = PlayerSettingsEditor.BuildEnumPopup<GraphicsJobMode>(PlayerSettingsEditor.Styles.graphicsJobsMode, graphicsJobMode1, PlayerSettingsEditor.m_GfxJobModeValues, targetGroup != BuildTargetGroup.XboxOne ? PlayerSettingsEditor.m_GfxJobModeNames : PlayerSettingsEditor.m_XBoxOneGfxJobModeNames);
          if (graphicsJobMode2 != graphicsJobMode1)
            PlayerSettings.graphicsJobMode = graphicsJobMode2;
        }
        if (targetGroup == BuildTargetGroup.XboxOne)
        {
          bool flag1 = this.m_GraphicsJobs.boolValue && PlayerSettings.graphicsJobMode == GraphicsJobMode.Native;
          GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.XboxOne);
          GraphicsDeviceType graphicsDeviceType = !flag1 ? GraphicsDeviceType.XboxOne : GraphicsDeviceType.XboxOneD3D12;
          if (graphicsDeviceType != graphicsApIs[0])
          {
            PlayerSettingsEditor.ChangeGraphicsApiAction action = this.CheckApplyGraphicsAPIList(BuildTarget.XboxOne, true);
            if (action.changeList)
              this.ApplyChangeGraphicsApiAction(BuildTarget.XboxOne, new GraphicsDeviceType[1]
              {
                graphicsDeviceType
              }, action);
          }
        }
      }
      if (flag8)
      {
        using (new EditorGUI.DisabledScope(EditorApplication.isPlaying || Lightmapping.isRunning))
        {
          EditorGUI.BeginChangeCheck();
          LightmapEncodingQuality forPlatformGroup = PlayerSettings.GetLightmapEncodingQualityForPlatformGroup(targetGroup);
          LightmapEncodingQuality[] options = new LightmapEncodingQuality[2]{ LightmapEncodingQuality.Normal, LightmapEncodingQuality.High };
          LightmapEncodingQuality encodingQuality = PlayerSettingsEditor.BuildEnumPopup<LightmapEncodingQuality>(PlayerSettingsEditor.Styles.lightmapEncodingLabel, forPlatformGroup, options, PlayerSettingsEditor.Styles.lightmapEncodingNames);
          if (EditorGUI.EndChangeCheck())
          {
            PlayerSettings.SetLightmapEncodingQualityForPlatformGroup(targetGroup, encodingQuality);
            Lightmapping.OnUpdateLightmapEncoding(targetGroup);
            this.serializedObject.ApplyModifiedProperties();
            GUIUtility.ExitGUI();
          }
        }
      }
      if (this.m_VRSettings.TargetGroupSupportsVirtualReality(targetGroup) && EditorGUILayout.LinkLabel(PlayerSettingsEditor.Styles.vrSettingsMoved))
        this.m_SelectedSection.value = this.m_VRSettings.GUISectionIndex;
      if (PlayerSettingsEditor.TargetSupportsProtectedGraphicsMem(targetGroup))
        PlayerSettings.protectGraphicsMemory = EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.protectGraphicsMemory, PlayerSettings.protectGraphicsMemory, new GUILayoutOption[0]);
      if (flag6)
        PlayerSettings.useHDRDisplay = EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Use display in HDR mode|Automatically switch the display to HDR output (on supported displays) at start of application."), PlayerSettings.useHDRDisplay, new GUILayoutOption[0]);
      EditorGUILayout.Space();
    }

    private void OtherSectionIdentificationGUI(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
    {
      if (settingsExtension != null && settingsExtension.HasIdentificationGUI())
      {
        GUILayout.Label(PlayerSettingsEditor.Styles.identificationTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
        settingsExtension.IdentificationSectionGUI();
        EditorGUILayout.Space();
      }
      else
      {
        if (targetGroup != BuildTargetGroup.Standalone)
          return;
        GUILayout.Label(PlayerSettingsEditor.Styles.macAppStoreTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
        PlayerSettingsEditor.ShowApplicationIdentifierUI(this.serializedObject, BuildTargetGroup.Standalone, "Bundle Identifier|'CFBundleIdentifier'", PlayerSettingsEditor.Styles.undoChangedBundleIdentifierString);
        EditorGUILayout.PropertyField(this.m_ApplicationBundleVersion, EditorGUIUtility.TextContent("Version*|'CFBundleShortVersionString'"), new GUILayoutOption[0]);
        PlayerSettingsEditor.ShowBuildNumberUI(this.serializedObject, BuildTargetGroup.Standalone, "Build|'CFBundleVersion'", PlayerSettingsEditor.Styles.undoChangedBuildNumberString);
        EditorGUILayout.PropertyField(this.m_MacAppStoreCategory, PlayerSettingsEditor.Styles.macAppStoreCategory, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_UseMacAppStoreValidation, PlayerSettingsEditor.Styles.useMacAppStoreValidation, new GUILayoutOption[0]);
        EditorGUILayout.Space();
      }
    }

    internal static void ShowApplicationIdentifierUI(SerializedObject serializedObject, BuildTargetGroup targetGroup, string label, string undoText)
    {
      EditorGUI.BeginChangeCheck();
      string identifier = EditorGUILayout.TextField(EditorGUIUtility.TextContent(label), PlayerSettings.GetApplicationIdentifier(targetGroup), new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      Undo.RecordObject(serializedObject.targetObject, undoText);
      PlayerSettings.SetApplicationIdentifier(targetGroup, identifier);
    }

    internal static void ShowBuildNumberUI(SerializedObject serializedObject, BuildTargetGroup targetGroup, string label, string undoText)
    {
      EditorGUI.BeginChangeCheck();
      string buildNumber = EditorGUILayout.TextField(EditorGUIUtility.TextContent(label), PlayerSettings.GetBuildNumber(targetGroup), new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      Undo.RecordObject(serializedObject.targetObject, undoText);
      PlayerSettings.SetBuildNumber(targetGroup, buildNumber);
    }

    private void OtherSectionConfigurationGUI(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
    {
      GUILayout.Label(PlayerSettingsEditor.Styles.configurationTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
      ScriptingRuntimeVersion[] options = new ScriptingRuntimeVersion[2]{ ScriptingRuntimeVersion.Legacy, ScriptingRuntimeVersion.Latest };
      GUIContent[] optionNames = new GUIContent[2]{ PlayerSettingsEditor.Styles.scriptingRuntimeVersionLegacy, PlayerSettingsEditor.Styles.scriptingRuntimeVersionLatest };
      ScriptingRuntimeVersion scriptingRuntimeVersion = PlayerSettings.scriptingRuntimeVersion;
      if (EditorApplication.isPlaying)
        PlayerSettingsEditor.BuildDisabledEnumPopup(PlayerSettings.scriptingRuntimeVersion != ScriptingRuntimeVersion.Legacy ? PlayerSettingsEditor.Styles.scriptingRuntimeVersionLatest : PlayerSettingsEditor.Styles.scriptingRuntimeVersionLegacy, PlayerSettingsEditor.Styles.scriptingRuntimeVersion);
      else
        scriptingRuntimeVersion = PlayerSettingsEditor.BuildEnumPopup<ScriptingRuntimeVersion>(PlayerSettingsEditor.Styles.scriptingRuntimeVersion, PlayerSettings.scriptingRuntimeVersion, options, optionNames);
      if (scriptingRuntimeVersion != PlayerSettings.scriptingRuntimeVersion && EditorUtility.DisplayDialog(LocalizationDatabase.GetLocalizedString("Restart required"), LocalizationDatabase.GetLocalizedString("Changing scripting runtime version requires a restart of the Editor to take effect. Do you wish to proceed?"), LocalizationDatabase.GetLocalizedString("Restart"), LocalizationDatabase.GetLocalizedString("Cancel")))
      {
        PlayerSettings.scriptingRuntimeVersion = scriptingRuntimeVersion;
        EditorCompilationInterface.Instance.CleanScriptAssemblies();
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
          EditorApplication.OpenProject(Environment.CurrentDirectory);
      }
      IScriptingImplementations scriptingImplementations = ModuleManager.GetScriptingImplementations(targetGroup);
      if (scriptingImplementations == null)
      {
        PlayerSettingsEditor.BuildDisabledEnumPopup(PlayerSettingsEditor.Styles.scriptingDefault, PlayerSettingsEditor.Styles.scriptingBackend);
      }
      else
      {
        ScriptingImplementation[] scriptingImplementationArray = scriptingImplementations.Enabled();
        ScriptingImplementation scriptingBackend = PlayerSettings.GetScriptingBackend(targetGroup);
        ScriptingImplementation backend;
        if (targetGroup == BuildTargetGroup.tvOS)
        {
          backend = ScriptingImplementation.IL2CPP;
          PlayerSettingsEditor.BuildDisabledEnumPopup(PlayerSettingsEditor.Styles.scriptingIL2CPP, PlayerSettingsEditor.Styles.scriptingBackend);
        }
        else
          backend = PlayerSettingsEditor.BuildEnumPopup<ScriptingImplementation>(PlayerSettingsEditor.Styles.scriptingBackend, scriptingBackend, scriptingImplementationArray, PlayerSettingsEditor.GetNiceScriptingBackendNames(scriptingImplementationArray));
        if (backend != scriptingBackend)
          PlayerSettings.SetScriptingBackend(targetGroup, backend);
      }
      if (targetGroup == BuildTargetGroup.WiiU)
      {
        PlayerSettingsEditor.BuildDisabledEnumPopup(PlayerSettingsEditor.Styles.apiCompatibilityLevel_WiiUSubset, PlayerSettingsEditor.Styles.apiCompatibilityLevel);
      }
      else
      {
        ApiCompatibilityLevel compatibilityLevel1 = PlayerSettings.GetApiCompatibilityLevel(targetGroup);
        ApiCompatibilityLevel[] compatibilityLevels = this.GetAvailableApiCompatibilityLevels(targetGroup);
        ApiCompatibilityLevel compatibilityLevel2 = PlayerSettingsEditor.BuildEnumPopup<ApiCompatibilityLevel>(PlayerSettingsEditor.Styles.apiCompatibilityLevel, compatibilityLevel1, compatibilityLevels, PlayerSettingsEditor.GetNiceApiCompatibilityLevelNames(compatibilityLevels));
        if (compatibilityLevel1 != compatibilityLevel2)
          PlayerSettings.SetApiCompatibilityLevel(targetGroup, compatibilityLevel2);
      }
      if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.WSA)
      {
        if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
          EditorGUILayout.PropertyField(this.m_useOnDemandResources, PlayerSettingsEditor.Styles.useOnDemandResources, new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.WSA)
          EditorGUILayout.PropertyField(this.m_AccelerometerFrequency, PlayerSettingsEditor.Styles.accelerometerFrequency, new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
        {
          EditorGUILayout.PropertyField(this.m_CameraUsageDescription, PlayerSettingsEditor.Styles.cameraUsageDescription, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_LocationUsageDescription, PlayerSettingsEditor.Styles.locationUsageDescription, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_MicrophoneUsageDescription, PlayerSettingsEditor.Styles.microphoneUsageDescription, new GUILayoutOption[0]);
        }
        if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.Android)
          EditorGUILayout.PropertyField(this.m_MuteOtherAudioSources, PlayerSettingsEditor.Styles.muteOtherAudioSources, new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
        {
          if (targetGroup == BuildTargetGroup.iPhone)
          {
            EditorGUILayout.PropertyField(this.m_PrepareIOSForRecording, PlayerSettingsEditor.Styles.prepareIOSForRecording, new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(this.m_ForceIOSSpeakersWhenRecording, PlayerSettingsEditor.Styles.forceIOSSpeakersWhenRecording, new GUILayoutOption[0]);
          }
          EditorGUILayout.PropertyField(this.m_UIRequiresPersistentWiFi, PlayerSettingsEditor.Styles.UIRequiresPersistentWiFi, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_IOSAllowHTTPDownload, PlayerSettingsEditor.Styles.iOSAllowHTTPDownload, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_IOSURLSchemes, PlayerSettingsEditor.Styles.iOSURLSchemes, true, new GUILayoutOption[0]);
        }
      }
      using (new EditorGUI.DisabledScope(!Application.HasProLicense()))
      {
        bool flag1 = !this.m_SubmitAnalytics.boolValue;
        bool flag2 = EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.disableStatistics, flag1, new GUILayoutOption[0]);
        if (flag1 != flag2)
        {
          this.m_SubmitAnalytics.boolValue = !flag2;
          EditorAnalytics.SendEventServiceInfo((object) new PlayerSettingsEditor.HwStatsServiceState()
          {
            hwstats = !flag2
          });
        }
        if (!Application.HasProLicense())
          this.m_SubmitAnalytics.boolValue = true;
      }
      if (settingsExtension != null)
        settingsExtension.ConfigurationSectionGUI();
      EditorGUILayout.LabelField(PlayerSettingsEditor.Styles.scriptingDefineSymbols);
      EditorGUI.BeginChangeCheck();
      string defines = EditorGUILayout.DelayedTextField(PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup), EditorStyles.textField, new GUILayoutOption[0]);
      this.scriptingDefinesControlID = EditorGUIUtility.s_LastControlID;
      if (EditorGUI.EndChangeCheck())
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
      int selectedIndex = this.m_EnableInputSystem.boolValue ? (!this.m_DisableInputManager.boolValue ? 2 : 1) : 0;
      int num1 = selectedIndex;
      EditorGUI.BeginChangeCheck();
      int num2 = EditorGUILayout.Popup(PlayerSettingsEditor.Styles.activeInputHandling, selectedIndex, PlayerSettingsEditor.Styles.activeInputHandlingOptions, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        if (num2 != num1)
        {
          EditorUtility.DisplayDialog("Unity editor restart required", "The Unity editor must be restarted for this change to take effect.", "OK");
          this.m_EnableInputSystem.boolValue = num2 == 1 || num2 == 2;
          this.m_DisableInputManager.boolValue = (num2 == 0 ? 1 : (num2 == 2 ? 1 : 0)) == 0;
          this.m_EnableInputSystem.serializedObject.ApplyModifiedProperties();
        }
        GUIUtility.ExitGUI();
      }
      EditorGUILayout.Space();
    }

    private void OtherSectionOptimizationGUI(BuildTargetGroup targetGroup)
    {
      GUILayout.Label(PlayerSettingsEditor.Styles.optimizationTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_BakeCollisionMeshes, PlayerSettingsEditor.Styles.bakeCollisionMeshes, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_KeepLoadedShadersAlive, PlayerSettingsEditor.Styles.keepLoadedShadersAlive, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_PreloadedAssets, PlayerSettingsEditor.Styles.preloadedAssets, true, new GUILayoutOption[0]);
      if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || (targetGroup == BuildTargetGroup.XboxOne || targetGroup == BuildTargetGroup.WiiU) || targetGroup == BuildTargetGroup.PS4 || targetGroup == BuildTargetGroup.PSP2)
        EditorGUILayout.PropertyField(this.m_AotOptions, PlayerSettingsEditor.Styles.aotOptions, new GUILayoutOption[0]);
      if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || (targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.Tizen) || (targetGroup == BuildTargetGroup.WebGL || targetGroup == BuildTargetGroup.WiiU || (targetGroup == BuildTargetGroup.PSP2 || targetGroup == BuildTargetGroup.PS4)) || targetGroup == BuildTargetGroup.XboxOne || targetGroup == BuildTargetGroup.WSA)
      {
        ScriptingImplementation scriptingBackend = PlayerSettings.GetScriptingBackend(targetGroup);
        if (targetGroup != BuildTargetGroup.WebGL)
        {
          switch (scriptingBackend)
          {
            case ScriptingImplementation.IL2CPP:
              break;
            case ScriptingImplementation.WinRTDotNET:
              goto label_7;
            default:
              EditorGUILayout.PropertyField(this.m_IPhoneStrippingLevel, PlayerSettingsEditor.Styles.iPhoneStrippingLevel, new GUILayoutOption[0]);
              goto label_7;
          }
        }
        EditorGUILayout.PropertyField(this.m_StripEngineCode, PlayerSettingsEditor.Styles.stripEngineCode, new GUILayoutOption[0]);
label_7:;
      }
      if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
        EditorGUILayout.PropertyField(this.m_IPhoneScriptCallOptimization, PlayerSettingsEditor.Styles.iPhoneScriptCallOptimization, new GUILayoutOption[0]);
      if (targetGroup == BuildTargetGroup.Android)
        EditorGUILayout.PropertyField(this.m_AndroidProfiler, PlayerSettingsEditor.Styles.enableInternalProfiler, new GUILayoutOption[0]);
      EditorGUILayout.Space();
      VertexChannelCompressionFlags intValue = (VertexChannelCompressionFlags) this.m_VertexChannelCompressionMask.intValue;
      this.m_VertexChannelCompressionMask.intValue = (int) EditorGUILayout.EnumFlagsField(PlayerSettingsEditor.Styles.vertexChannelCompressionMask, (Enum) intValue, new GUILayoutOption[0]);
      if (targetGroup != BuildTargetGroup.PSM)
        EditorGUILayout.PropertyField(this.m_StripUnusedMeshComponents, PlayerSettingsEditor.Styles.stripUnusedMeshComponents, new GUILayoutOption[0]);
      if (targetGroup == BuildTargetGroup.PSP2 || targetGroup == BuildTargetGroup.PSM)
      {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(this.m_VideoMemoryForVertexBuffers, PlayerSettingsEditor.Styles.videoMemoryForVertexBuffers, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          if (this.m_VideoMemoryForVertexBuffers.intValue < 0)
            this.m_VideoMemoryForVertexBuffers.intValue = 0;
          else if (this.m_VideoMemoryForVertexBuffers.intValue > 192)
            this.m_VideoMemoryForVertexBuffers.intValue = 192;
        }
      }
      EditorGUILayout.Space();
    }

    private ApiCompatibilityLevel[] GetAvailableApiCompatibilityLevels(BuildTargetGroup activeBuildTargetGroup)
    {
      if (EditorApplication.scriptingRuntimeVersion == ScriptingRuntimeVersion.Latest)
        return PlayerSettingsEditor.only_4_x_profiles;
      if (activeBuildTargetGroup == BuildTargetGroup.WSA || activeBuildTargetGroup == BuildTargetGroup.XboxOne)
        return PlayerSettingsEditor.allProfiles;
      return PlayerSettingsEditor.only_2_0_profiles;
    }

    private void OtherSectionLoggingGUI()
    {
      GUILayout.Label(PlayerSettingsEditor.Styles.loggingTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.BeginVertical();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Log Type");
      IEnumerator enumerator1 = Enum.GetValues(typeof (StackTraceLogType)).GetEnumerator();
      try
      {
        while (enumerator1.MoveNext())
          GUILayout.Label(((StackTraceLogType) enumerator1.Current).ToString(), new GUILayoutOption[1]
          {
            GUILayout.Width(70f)
          });
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator1 as IDisposable) != null)
          disposable.Dispose();
      }
      GUILayout.EndHorizontal();
      IEnumerator enumerator2 = Enum.GetValues(typeof (LogType)).GetEnumerator();
      try
      {
        while (enumerator2.MoveNext())
        {
          LogType current1 = (LogType) enumerator2.Current;
          GUILayout.BeginHorizontal();
          GUILayout.Label(current1.ToString());
          IEnumerator enumerator3 = Enum.GetValues(typeof (StackTraceLogType)).GetEnumerator();
          try
          {
            while (enumerator3.MoveNext())
            {
              StackTraceLogType current2 = (StackTraceLogType) enumerator3.Current;
              StackTraceLogType stackTraceLogType = PlayerSettings.GetStackTraceLogType(current1);
              EditorGUI.BeginChangeCheck();
              if (EditorGUI.EndChangeCheck() && EditorGUILayout.ToggleLeft(" ", (stackTraceLogType == current2 ? 1 : 0) != 0, new GUILayoutOption[1]{ GUILayout.Width(70f) }))
                PlayerSettings.SetStackTraceLogType(current1, current2);
            }
          }
          finally
          {
            IDisposable disposable;
            if ((disposable = enumerator3 as IDisposable) != null)
              disposable.Dispose();
          }
          GUILayout.EndHorizontal();
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator2 as IDisposable) != null)
          disposable.Dispose();
      }
      GUILayout.EndVertical();
    }

    private static GUIContent[] GetGUIContentsForValues<T>(Dictionary<T, GUIContent> contents, T[] values)
    {
      GUIContent[] guiContentArray = new GUIContent[values.Length];
      for (int index = 0; index < values.Length; ++index)
      {
        if (!contents.ContainsKey(values[index]))
          throw new NotImplementedException(string.Format("Missing name for {0}", (object) values[index]));
        guiContentArray[index] = contents[values[index]];
      }
      return guiContentArray;
    }

    private static GUIContent[] GetNiceScriptingBackendNames(ScriptingImplementation[] scriptingBackends)
    {
      if (PlayerSettingsEditor.m_NiceScriptingBackendNames == null)
        PlayerSettingsEditor.m_NiceScriptingBackendNames = new Dictionary<ScriptingImplementation, GUIContent>()
        {
          {
            ScriptingImplementation.Mono2x,
            PlayerSettingsEditor.Styles.scriptingMono2x
          },
          {
            ScriptingImplementation.WinRTDotNET,
            PlayerSettingsEditor.Styles.scriptingWinRTDotNET
          },
          {
            ScriptingImplementation.IL2CPP,
            PlayerSettingsEditor.Styles.scriptingIL2CPP
          }
        };
      return PlayerSettingsEditor.GetGUIContentsForValues<ScriptingImplementation>(PlayerSettingsEditor.m_NiceScriptingBackendNames, scriptingBackends);
    }

    private static GUIContent[] GetNiceApiCompatibilityLevelNames(ApiCompatibilityLevel[] apiCompatibilityLevels)
    {
      if (PlayerSettingsEditor.m_NiceApiCompatibilityLevelNames == null)
        PlayerSettingsEditor.m_NiceApiCompatibilityLevelNames = new Dictionary<ApiCompatibilityLevel, GUIContent>()
        {
          {
            ApiCompatibilityLevel.NET_2_0,
            PlayerSettingsEditor.Styles.apiCompatibilityLevel_NET_2_0
          },
          {
            ApiCompatibilityLevel.NET_2_0_Subset,
            PlayerSettingsEditor.Styles.apiCompatibilityLevel_NET_2_0_Subset
          },
          {
            ApiCompatibilityLevel.NET_4_6,
            PlayerSettingsEditor.Styles.apiCompatibilityLevel_NET_4_6
          }
        };
      return PlayerSettingsEditor.GetGUIContentsForValues<ApiCompatibilityLevel>(PlayerSettingsEditor.m_NiceApiCompatibilityLevelNames, apiCompatibilityLevels);
    }

    private void AutoAssignProperty(SerializedProperty property, string packageDir, string fileName)
    {
      if (property.stringValue.Length != 0 && File.Exists(Path.Combine(packageDir, property.stringValue)) || !File.Exists(Path.Combine(packageDir, fileName)))
        return;
      property.stringValue = fileName;
    }

    public void BrowseablePathProperty(string propertyLabel, SerializedProperty property, string browsePanelTitle, string extension, string dir)
    {
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.PrefixLabel(EditorGUIUtility.TextContent(propertyLabel));
      GUIContent content = new GUIContent("...");
      Vector2 vector2 = GUI.skin.GetStyle("Button").CalcSize(content);
      if (GUILayout.Button(content, EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.MaxWidth(vector2.x) }))
      {
        GUI.FocusControl("");
        string text = EditorGUIUtility.TempContent(browsePanelTitle).text;
        string str1 = !string.IsNullOrEmpty(dir) ? dir.Replace('\\', '/') + "/" : Directory.GetCurrentDirectory().Replace('\\', '/') + "/";
        string str2 = !string.IsNullOrEmpty(extension) ? EditorUtility.OpenFilePanel(text, str1, extension) : EditorUtility.OpenFolderPanel(text, str1, "");
        if (str2.StartsWith(str1))
          str2 = str2.Substring(str1.Length);
        if (!string.IsNullOrEmpty(str2))
        {
          property.stringValue = str2;
          this.serializedObject.ApplyModifiedProperties();
          GUIUtility.ExitGUI();
        }
      }
      bool disabled = string.IsNullOrEmpty(property.stringValue);
      using (new EditorGUI.DisabledScope(disabled))
      {
        GUIContent guiContent = !disabled ? EditorGUIUtility.TempContent(property.stringValue) : EditorGUIUtility.TextContent("Not selected.");
        EditorGUI.BeginChangeCheck();
        GUILayoutOption[] guiLayoutOptionArray = new GUILayoutOption[2]{ GUILayout.Width(32f), GUILayout.ExpandWidth(true) };
        string str = EditorGUILayout.TextArea(guiContent.text, guiLayoutOptionArray);
        if (EditorGUI.EndChangeCheck())
        {
          if (string.IsNullOrEmpty(str))
          {
            property.stringValue = "";
            this.serializedObject.ApplyModifiedProperties();
            GUI.FocusControl("");
            GUIUtility.ExitGUI();
          }
        }
      }
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();
    }

    internal static bool BuildPathBoxButton(SerializedProperty prop, string uiString, string directory)
    {
      return PlayerSettingsEditor.BuildPathBoxButton(prop, uiString, directory, (Action) null);
    }

    internal static bool BuildPathBoxButton(SerializedProperty prop, string uiString, string directory, Action onSelect)
    {
      float num = 16f;
      Rect rect = GUILayoutUtility.GetRect((float) (80.0 + (double) EditorGUIUtility.fieldWidth + 5.0), (float) (80.0 + (double) EditorGUIUtility.fieldWidth + 5.0), num, num, EditorStyles.layerMaskField, (GUILayoutOption[]) null);
      float labelWidth = EditorGUIUtility.labelWidth;
      Rect position = new Rect(rect.x + EditorGUI.indent, rect.y, labelWidth - EditorGUI.indent, rect.height);
      EditorGUI.TextArea(new Rect(rect.x + labelWidth, rect.y, rect.width - labelWidth, rect.height), prop.stringValue.Length != 0 ? prop.stringValue : "Not selected.", EditorStyles.label);
      bool flag = false;
      if (GUI.Button(position, EditorGUIUtility.TextContent(uiString)))
      {
        string stringValue = prop.stringValue;
        string path = EditorUtility.OpenFolderPanel(EditorGUIUtility.TextContent(uiString).text, directory, "");
        string projectRelativePath = FileUtil.GetProjectRelativePath(path);
        prop.stringValue = !(projectRelativePath != string.Empty) ? path : projectRelativePath;
        flag = prop.stringValue != stringValue;
        if (onSelect != null)
          onSelect();
        prop.serializedObject.ApplyModifiedProperties();
      }
      return flag;
    }

    internal static bool BuildFileBoxButton(SerializedProperty prop, string uiString, string directory, string ext)
    {
      return PlayerSettingsEditor.BuildFileBoxButton(prop, uiString, directory, ext, (Action) null);
    }

    internal static bool BuildFileBoxButton(SerializedProperty prop, string uiString, string directory, string ext, Action onSelect)
    {
      float num = 16f;
      Rect rect = GUILayoutUtility.GetRect((float) (80.0 + (double) EditorGUIUtility.fieldWidth + 5.0), (float) (80.0 + (double) EditorGUIUtility.fieldWidth + 5.0), num, num, EditorStyles.layerMaskField, (GUILayoutOption[]) null);
      float labelWidth = EditorGUIUtility.labelWidth;
      Rect position = new Rect(rect.x + EditorGUI.indent, rect.y, labelWidth - EditorGUI.indent, rect.height);
      EditorGUI.TextArea(new Rect(rect.x + labelWidth, rect.y, rect.width - labelWidth, rect.height), prop.stringValue.Length != 0 ? prop.stringValue : "Not selected.", EditorStyles.label);
      bool flag = false;
      if (GUI.Button(position, EditorGUIUtility.TextContent(uiString)))
      {
        string stringValue = prop.stringValue;
        string path = EditorUtility.OpenFilePanel(EditorGUIUtility.TextContent(uiString).text, directory, ext);
        string projectRelativePath = FileUtil.GetProjectRelativePath(path);
        prop.stringValue = !(projectRelativePath != string.Empty) ? path : projectRelativePath;
        flag = prop.stringValue != stringValue;
        if (onSelect != null)
          onSelect();
        prop.serializedObject.ApplyModifiedProperties();
      }
      return flag;
    }

    public void PublishSectionGUI(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension, int sectionIndex = 5)
    {
      if (targetGroup != BuildTargetGroup.WSA && targetGroup != BuildTargetGroup.PSP2 && (settingsExtension == null || !settingsExtension.HasPublishSection()))
        return;
      if (this.BeginSettingsBox(sectionIndex, PlayerSettingsEditor.Styles.publishingSettingsTitle))
      {
        float h = 16f;
        float midWidth = (float) (80.0 + (double) EditorGUIUtility.fieldWidth + 5.0);
        float maxWidth = (float) (80.0 + (double) EditorGUIUtility.fieldWidth + 5.0);
        if (settingsExtension != null)
          settingsExtension.PublishSectionGUI(h, midWidth, maxWidth);
        if (targetGroup != BuildTargetGroup.PSM)
          ;
      }
      this.EndSettingsBox();
    }

    private static void ShowWarning(GUIContent warningMessage)
    {
      if ((UnityEngine.Object) PlayerSettingsEditor.s_WarningIcon == (UnityEngine.Object) null)
        PlayerSettingsEditor.s_WarningIcon = EditorGUIUtility.LoadIcon("console.warnicon");
      warningMessage.image = (Texture) PlayerSettingsEditor.s_WarningIcon;
      GUILayout.Space(5f);
      GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
      GUILayout.Label(warningMessage, EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
      GUILayout.EndVertical();
    }

    protected override bool ShouldHideOpenButton()
    {
      return true;
    }

    private static class Styles
    {
      public static readonly GUIStyle categoryBox = new GUIStyle(EditorStyles.helpBox);
      public static readonly GUIContent colorSpaceAndroidWarning = EditorGUIUtility.TextContent("Linear colorspace requires OpenGL ES 3.0 or Vulkan, uncheck 'Automatic Graphics API' to remove OpenGL ES 2 API, Blit Type must be Always Blit or Auto and 'Minimum API Level' must be at least Android 4.3");
      public static readonly GUIContent colorSpaceWebGLWarning = EditorGUIUtility.TextContent("Linear colorspace requires WebGL 2.0, uncheck 'Automatic Graphics API' to remove WebGL 1.0 API. WARNING: If DXT sRGB is not supported by the browser, texture will be decompressed");
      public static readonly GUIContent colorSpaceIOSWarning = EditorGUIUtility.TextContent("Linear colorspace requires Metal API only. Uncheck 'Automatic Graphics API' and remove OpenGL ES 2 API. Additionally, 'minimum iOS version' set to 8.0 at least");
      public static readonly GUIContent colorSpaceTVOSWarning = EditorGUIUtility.TextContent("Linear colorspace requires Metal API only. Uncheck 'Automatic Graphics API' and remove OpenGL ES 2 API.");
      public static readonly GUIContent recordingInfo = EditorGUIUtility.TextContent("Reordering the list will switch editor to the first available platform");
      public static readonly GUIContent notApplicableInfo = EditorGUIUtility.TextContent("Not applicable for this platform.");
      public static readonly GUIContent sharedBetweenPlatformsInfo = EditorGUIUtility.TextContent("* Shared setting between multiple platforms.");
      public static readonly GUIContent VRSupportOverridenInfo = EditorGUIUtility.TextContent("This setting is overridden by Virtual Reality Support.");
      public static readonly GUIContent IL2CPPAndroidExperimentalInfo = EditorGUIUtility.TextContent("IL2CPP on Android is experimental and unsupported");
      public static readonly GUIContent cursorHotspot = EditorGUIUtility.TextContent("Cursor Hotspot");
      public static readonly GUIContent defaultCursor = EditorGUIUtility.TextContent("Default Cursor");
      public static readonly GUIContent defaultIcon = EditorGUIUtility.TextContent("Default Icon");
      public static readonly GUIContent vertexChannelCompressionMask = EditorGUIUtility.TextContent("Vertex Compression*|Select which vertex channels should be compressed. Compression can save memory and bandwidth but precision will be lower.");
      public static readonly GUIContent iconTitle = EditorGUIUtility.TextContent("Icon");
      public static readonly GUIContent resolutionPresentationTitle = EditorGUIUtility.TextContent("Resolution and Presentation");
      public static readonly GUIContent resolutionTitle = EditorGUIUtility.TextContent("Resolution");
      public static readonly GUIContent orientationTitle = EditorGUIUtility.TextContent("Orientation");
      public static readonly GUIContent allowedOrientationTitle = EditorGUIUtility.TextContent("Allowed Orientations for Auto Rotation");
      public static readonly GUIContent multitaskingSupportTitle = EditorGUIUtility.TextContent("Multitasking Support");
      public static readonly GUIContent statusBarTitle = EditorGUIUtility.TextContent("Status Bar");
      public static readonly GUIContent standalonePlayerOptionsTitle = EditorGUIUtility.TextContent("Standalone Player Options");
      public static readonly GUIContent debuggingCrashReportingTitle = EditorGUIUtility.TextContent("Debugging and crash reporting");
      public static readonly GUIContent debuggingTitle = EditorGUIUtility.TextContent("Debugging");
      public static readonly GUIContent crashReportingTitle = EditorGUIUtility.TextContent("Crash Reporting");
      public static readonly GUIContent otherSettingsTitle = EditorGUIUtility.TextContent("Other Settings");
      public static readonly GUIContent renderingTitle = EditorGUIUtility.TextContent("Rendering");
      public static readonly GUIContent identificationTitle = EditorGUIUtility.TextContent("Identification");
      public static readonly GUIContent macAppStoreTitle = EditorGUIUtility.TextContent("Mac App Store Options");
      public static readonly GUIContent configurationTitle = EditorGUIUtility.TextContent("Configuration");
      public static readonly GUIContent optimizationTitle = EditorGUIUtility.TextContent("Optimization");
      public static readonly GUIContent loggingTitle = EditorGUIUtility.TextContent("Logging*");
      public static readonly GUIContent publishingSettingsTitle = EditorGUIUtility.TextContent("Publishing Settings");
      public static readonly GUIContent bakeCollisionMeshes = EditorGUIUtility.TextContent("Prebake Collision Meshes*|Bake collision data into the meshes on build time");
      public static readonly GUIContent keepLoadedShadersAlive = EditorGUIUtility.TextContent("Keep Loaded Shaders Alive*|Prevents shaders from being unloaded");
      public static readonly GUIContent preloadedAssets = EditorGUIUtility.TextContent("Preloaded Assets*|Assets to load at start up in the player and kept alive until the player terminates");
      public static readonly GUIContent stripEngineCode = EditorGUIUtility.TextContent("Strip Engine Code*|Strip Unused Engine Code - Note that byte code stripping of managed assemblies is always enabled for the IL2CPP scripting backend.");
      public static readonly GUIContent iPhoneStrippingLevel = EditorGUIUtility.TextContent("Stripping Level*");
      public static readonly GUIContent iPhoneScriptCallOptimization = EditorGUIUtility.TextContent("Script Call Optimization*");
      public static readonly GUIContent enableInternalProfiler = EditorGUIUtility.TextContent("Enable Internal Profiler* (Deprecated)|Internal profiler counters should be accessed by scripts using UnityEngine.Profiling::Profiler API.");
      public static readonly GUIContent stripUnusedMeshComponents = EditorGUIUtility.TextContent("Optimize Mesh Data*|Remove unused mesh components");
      public static readonly GUIContent videoMemoryForVertexBuffers = EditorGUIUtility.TextContent("Mesh Video Mem*|How many megabytes of video memory to use for mesh data before we use main memory");
      public static readonly GUIContent protectGraphicsMemory = EditorGUIUtility.TextContent("Protect Graphics Memory|Protect GPU memory from being read (on supported devices). Will prevent user from taking screenshots");
      public static readonly GUIContent defaultIsFullScreen = EditorGUIUtility.TextContent("Default Is Full Screen*");
      public static readonly GUIContent useOSAutoRotation = EditorGUIUtility.TextContent("Use Animated Autorotation|If set OS native animated autorotation method will be used. Otherwise orientation will be changed immediately.");
      public static readonly GUIContent UIPrerenderedIcon = EditorGUIUtility.TextContent("Prerendered Icon");
      public static readonly GUIContent defaultScreenWidth = EditorGUIUtility.TextContent("Default Screen Width");
      public static readonly GUIContent defaultScreenHeight = EditorGUIUtility.TextContent("Default Screen Height");
      public static readonly GUIContent macRetinaSupport = EditorGUIUtility.TextContent("Mac Retina Support");
      public static readonly GUIContent runInBackground = EditorGUIUtility.TextContent("Run In Background*");
      public static readonly GUIContent defaultScreenOrientation = EditorGUIUtility.TextContent("Default Orientation*");
      public static readonly GUIContent allowedAutoRotateToPortrait = EditorGUIUtility.TextContent("Portrait");
      public static readonly GUIContent allowedAutoRotateToPortraitUpsideDown = EditorGUIUtility.TextContent("Portrait Upside Down");
      public static readonly GUIContent allowedAutoRotateToLandscapeRight = EditorGUIUtility.TextContent("Landscape Right");
      public static readonly GUIContent allowedAutoRotateToLandscapeLeft = EditorGUIUtility.TextContent("Landscape Left");
      public static readonly GUIContent UIRequiresFullScreen = EditorGUIUtility.TextContent("Requires Fullscreen");
      public static readonly GUIContent UIStatusBarHidden = EditorGUIUtility.TextContent("Status Bar Hidden");
      public static readonly GUIContent UIStatusBarStyle = EditorGUIUtility.TextContent("Status Bar Style");
      public static readonly GUIContent useMacAppStoreValidation = EditorGUIUtility.TextContent("Mac App Store Validation");
      public static readonly GUIContent macAppStoreCategory = EditorGUIUtility.TextContent("Category|'LSApplicationCategoryType'");
      public static readonly GUIContent D3D11FullscreenMode = EditorGUIUtility.TextContent("D3D11 Fullscreen Mode");
      public static readonly GUIContent visibleInBackground = EditorGUIUtility.TextContent("Visible In Background");
      public static readonly GUIContent allowFullscreenSwitch = EditorGUIUtility.TextContent("Allow Fullscreen Switch");
      public static readonly GUIContent use32BitDisplayBuffer = EditorGUIUtility.TextContent("Use 32-bit Display Buffer*|If set Display Buffer will be created to hold 32-bit color values. Use it only if you see banding, as it has performance implications.");
      public static readonly GUIContent disableDepthAndStencilBuffers = EditorGUIUtility.TextContent("Disable Depth and Stencil*");
      public static readonly GUIContent iosShowActivityIndicatorOnLoading = EditorGUIUtility.TextContent("Show Loading Indicator");
      public static readonly GUIContent androidShowActivityIndicatorOnLoading = EditorGUIUtility.TextContent("Show Loading Indicator");
      public static readonly GUIContent actionOnDotNetUnhandledException = EditorGUIUtility.TextContent("On .Net UnhandledException*");
      public static readonly GUIContent logObjCUncaughtExceptions = EditorGUIUtility.TextContent("Log Obj-C Uncaught Exceptions*");
      public static readonly GUIContent enableCrashReportAPI = EditorGUIUtility.TextContent("Enable CrashReport API*");
      public static readonly GUIContent activeColorSpace = EditorGUIUtility.TextContent("Color Space*");
      public static readonly GUIContent colorGamut = EditorGUIUtility.TextContent("Color Gamut*");
      public static readonly GUIContent colorGamutForMac = EditorGUIUtility.TextContent("Color Gamut For Mac*");
      public static readonly GUIContent metalForceHardShadows = EditorGUIUtility.TextContent("Force hard shadows on Metal*");
      public static readonly GUIContent metalEditorSupport = EditorGUIUtility.TextContent("Metal Editor Support* (Experimental)");
      public static readonly GUIContent metalAPIValidation = EditorGUIUtility.TextContent("Metal API Validation*");
      public static readonly GUIContent metalFramebufferOnly = EditorGUIUtility.TextContent("Metal Restricted Backbuffer Use|Set framebufferOnly flag on backbuffer. This prevents readback from backbuffer but enables some driver optimizations.");
      public static readonly GUIContent mTRendering = EditorGUIUtility.TextContent("Multithreaded Rendering*");
      public static readonly GUIContent staticBatching = EditorGUIUtility.TextContent("Static Batching");
      public static readonly GUIContent dynamicBatching = EditorGUIUtility.TextContent("Dynamic Batching");
      public static readonly GUIContent graphicsJobs = EditorGUIUtility.TextContent("Graphics Jobs (Experimental)*");
      public static readonly GUIContent graphicsJobsMode = EditorGUIUtility.TextContent("Graphics Jobs Mode*");
      public static readonly GUIContent applicationBuildNumber = EditorGUIUtility.TextContent("Build");
      public static readonly GUIContent appleDeveloperTeamID = EditorGUIUtility.TextContent("iOS Developer Team ID|Developers can retrieve their Team ID by visiting the Apple Developer site under Account > Membership.");
      public static readonly GUIContent useOnDemandResources = EditorGUIUtility.TextContent("Use on demand resources*");
      public static readonly GUIContent accelerometerFrequency = EditorGUIUtility.TextContent("Accelerometer Frequency*");
      public static readonly GUIContent cameraUsageDescription = EditorGUIUtility.TextContent("Camera Usage Description*");
      public static readonly GUIContent locationUsageDescription = EditorGUIUtility.TextContent("Location Usage Description*");
      public static readonly GUIContent microphoneUsageDescription = EditorGUIUtility.TextContent("Microphone Usage Description*");
      public static readonly GUIContent muteOtherAudioSources = EditorGUIUtility.TextContent("Mute Other Audio Sources*");
      public static readonly GUIContent prepareIOSForRecording = EditorGUIUtility.TextContent("Prepare iOS for Recording");
      public static readonly GUIContent forceIOSSpeakersWhenRecording = EditorGUIUtility.TextContent("Force iOS Speakers when Recording");
      public static readonly GUIContent UIRequiresPersistentWiFi = EditorGUIUtility.TextContent("Requires Persistent WiFi*");
      public static readonly GUIContent iOSAllowHTTPDownload = EditorGUIUtility.TextContent("Allow downloads over HTTP (nonsecure)*");
      public static readonly GUIContent iOSURLSchemes = EditorGUIUtility.TextContent("Supported URL schemes*");
      public static readonly GUIContent aotOptions = EditorGUIUtility.TextContent("AOT Compilation Options*");
      public static readonly GUIContent require31 = EditorGUIUtility.TextContent("Require ES3.1");
      public static readonly GUIContent requireAEP = EditorGUIUtility.TextContent("Require ES3.1+AEP");
      public static readonly GUIContent skinOnGPU = EditorGUIUtility.TextContent("GPU Skinning*|Use DX11/ES3 GPU Skinning");
      public static readonly GUIContent skinOnGPUPS4 = EditorGUIUtility.TextContent("Compute Skinning*|Use Compute pipeline for Skinning");
      public static readonly GUIContent skinOnGPUAndroidWarning = EditorGUIUtility.TextContent("GPU skinning on Android devices is only enabled in VR builds, and is experimental. Be sure to validate behavior and performance on your target devices.");
      public static readonly GUIContent disableStatistics = EditorGUIUtility.TextContent("Disable HW Statistics*|Disables HW Statistics (Pro Only)");
      public static readonly GUIContent scriptingDefineSymbols = EditorGUIUtility.TextContent("Scripting Define Symbols*");
      public static readonly GUIContent scriptingRuntimeVersion = EditorGUIUtility.TextContent("Scripting Runtime Version*|The scripting runtime version to be used. Unity uses different scripting backends based on platform, so these options are listed as equivalent expected behavior.");
      public static readonly GUIContent scriptingRuntimeVersionLegacy = EditorGUIUtility.TextContent("Stable (.NET 3.5 Equivalent)");
      public static readonly GUIContent scriptingRuntimeVersionLatest = EditorGUIUtility.TextContent("Experimental (.NET 4.6 Equivalent)");
      public static readonly GUIContent scriptingBackend = EditorGUIUtility.TextContent("Scripting Backend");
      public static readonly GUIContent scriptingMono2x = EditorGUIUtility.TextContent("Mono");
      public static readonly GUIContent scriptingWinRTDotNET = EditorGUIUtility.TextContent(".NET");
      public static readonly GUIContent scriptingIL2CPP = EditorGUIUtility.TextContent("IL2CPP");
      public static readonly GUIContent scriptingDefault = EditorGUIUtility.TextContent("Default");
      public static readonly GUIContent apiCompatibilityLevel = EditorGUIUtility.TextContent("Api Compatibility Level*");
      public static readonly GUIContent apiCompatibilityLevel_WiiUSubset = EditorGUIUtility.TextContent("WiiU Subset");
      public static readonly GUIContent apiCompatibilityLevel_NET_2_0 = EditorGUIUtility.TextContent(".NET 2.0");
      public static readonly GUIContent apiCompatibilityLevel_NET_2_0_Subset = EditorGUIUtility.TextContent(".NET 2.0 Subset");
      public static readonly GUIContent apiCompatibilityLevel_NET_4_6 = EditorGUIUtility.TextContent(".NET 4.6");
      public static readonly GUIContent activeInputHandling = EditorGUIUtility.TextContent("Active Input Handling*");
      public static readonly GUIContent[] activeInputHandlingOptions = new GUIContent[3]{ new GUIContent("Input Manager"), new GUIContent("Input System (Preview)"), new GUIContent("Both") };
      public static readonly GUIContent vrSettingsMoved = EditorGUIUtility.TextContent("Virtual Reality moved to XR Settings");
      public static readonly GUIContent lightmapEncodingLabel = EditorGUIUtility.TextContent("Lightmap Encoding|Affects the encoding scheme and compression format of the lightmaps.");
      public static readonly GUIContent[] lightmapEncodingNames = new GUIContent[2]{ new GUIContent("Normal Quality"), new GUIContent("High Quality") };

      static Styles()
      {
        PlayerSettingsEditor.Styles.categoryBox.padding.left = 14;
      }

      public static string undoChangedBundleIdentifierString
      {
        get
        {
          return LocalizationDatabase.GetLocalizedString("Changed macOS bundleIdentifier");
        }
      }

      public static string undoChangedBuildNumberString
      {
        get
        {
          return LocalizationDatabase.GetLocalizedString("Changed macOS build number");
        }
      }

      public static string undoChangedBatchingString
      {
        get
        {
          return LocalizationDatabase.GetLocalizedString("Changed Batching Settings");
        }
      }

      public static string undoChangedIconString
      {
        get
        {
          return LocalizationDatabase.GetLocalizedString("Changed Icon");
        }
      }

      public static string undoChangedGraphicsAPIString
      {
        get
        {
          return LocalizationDatabase.GetLocalizedString("Changed Graphics API Settings");
        }
      }
    }

    private struct ChangeGraphicsApiAction
    {
      public readonly bool changeList;
      public readonly bool reloadGfx;

      public ChangeGraphicsApiAction(bool doChange, bool doReload)
      {
        this.changeList = doChange;
        this.reloadGfx = doReload;
      }
    }

    [Serializable]
    private struct HwStatsServiceState
    {
      public bool hwstats;
    }
  }
}
