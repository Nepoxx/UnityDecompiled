// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildPlayerWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.Build;
using UnityEditor.BuildReporting;
using UnityEditor.Connect;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Modules;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  /// <summary>
  ///   <para>The default build settings window.</para>
  /// </summary>
  public class BuildPlayerWindow : EditorWindow
  {
    private static BuildPlayerWindow.Styles styles = (BuildPlayerWindow.Styles) null;
    private static Regex s_VersionPattern = new Regex("(?<shortVersion>\\d+\\.\\d+\\.\\d+(?<suffix>((?<alphabeta>[abx])|[fp])[^\\s]*))( \\((?<revision>[a-fA-F\\d]+)\\))?", RegexOptions.Compiled);
    private static Dictionary<string, string> s_ModuleNames = new Dictionary<string, string>() { { "tvOS", "AppleTV" }, { "OSXStandalone", "Mac" }, { "WindowsStandalone", "Windows" }, { "LinuxStandalone", "Linux" }, { "Facebook", "Facebook-Games" } };
    private static bool m_Building = false;
    private Vector2 scrollPosition = new Vector2(0.0f, 0.0f);
    private BuildPlayerSceneTreeView m_TreeView = (BuildPlayerSceneTreeView) null;
    private long getCurrentVersionOperationId = -1;
    private long getLatestVersionOperationId = -1;
    private bool isVersionInitialized = false;
    private long packmanOperationId = -1;
    private BuildPlayerWindow.PackmanOperationType packmanOperationType = BuildPlayerWindow.PackmanOperationType.None;
    private bool packmanOperationRunning = false;
    private string xiaomiPackageName = "com.unity.xiaomi";
    private string currentXiaomiPackageVersion = "";
    private string latestXiaomiPackageVersion = "";
    private bool xiaomiPackageInstalled = false;
    private BuildPlayerWindow.PublishStyles publishStyles = (BuildPlayerWindow.PublishStyles) null;
    private const string kEditorBuildSettingsPath = "ProjectSettings/EditorBuildSettings.asset";
    internal const string kSettingDebuggingWaitForManagedDebugger = "WaitForManagedDebugger";
    [SerializeField]
    private TreeViewState m_TreeViewState;
    private static Func<BuildPlayerOptions, BuildPlayerOptions> getBuildPlayerOptionsHandler;
    private static Action<BuildPlayerOptions> buildPlayerHandler;

    public BuildPlayerWindow()
    {
      this.position = new Rect(50f, 50f, 540f, 530f);
      this.minSize = new Vector2(630f, 580f);
      this.titleContent = new GUIContent("Build Settings");
    }

    /// <summary>
    ///   <para>Open the build settings window.</para>
    /// </summary>
    public static void ShowBuildPlayerWindow()
    {
      EditorUserBuildSettings.selectedBuildTargetGroup = EditorUserBuildSettings.activeBuildTargetGroup;
      EditorWindow.GetWindow<BuildPlayerWindow>(true, "Build Settings");
    }

    private static bool BuildLocationIsValid(string path)
    {
      return path.Length > 0 && Directory.Exists(FileUtil.DeleteLastPathNameComponent(path));
    }

    private static void BuildPlayerAndRun()
    {
      BuildPlayerWindow.BuildPlayerAndRun(!BuildPlayerWindow.BuildLocationIsValid(EditorUserBuildSettings.GetBuildLocation(BuildPlayerWindow.CalculateSelectedBuildTarget())));
    }

    private static void BuildPlayerAndRun(bool askForBuildLocation)
    {
      BuildPlayerWindow.CallBuildMethods(askForBuildLocation, BuildOptions.AutoRunPlayer | BuildOptions.StrictMode);
    }

    private void ActiveScenesGUI()
    {
      if (this.m_TreeView == null)
      {
        if (this.m_TreeViewState == null)
          this.m_TreeViewState = new TreeViewState();
        this.m_TreeView = new BuildPlayerSceneTreeView(this.m_TreeViewState);
        this.m_TreeView.Reload();
      }
      GUI.Label(GUILayoutUtility.GetRect(BuildPlayerWindow.styles.scenesInBuild, BuildPlayerWindow.styles.title), BuildPlayerWindow.styles.scenesInBuild, BuildPlayerWindow.styles.title);
      this.m_TreeView.OnGUI(GUILayoutUtility.GetRect(0.0f, this.position.width, 0.0f, this.position.height));
    }

    private void OnDisable()
    {
      if (this.m_TreeView == null)
        return;
      this.m_TreeView.UnsubscribeListChange();
    }

    private void AddOpenScenes()
    {
      List<EditorBuildSettingsScene> source = new List<EditorBuildSettingsScene>((IEnumerable<EditorBuildSettingsScene>) EditorBuildSettings.scenes);
      bool flag = false;
      for (int index = 0; index < SceneManager.sceneCount; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        BuildPlayerWindow.\u003CAddOpenScenes\u003Ec__AnonStorey0 scenesCAnonStorey0 = new BuildPlayerWindow.\u003CAddOpenScenes\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        scenesCAnonStorey0.scene = SceneManager.GetSceneAt(index);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if ((scenesCAnonStorey0.scene.path.Length != 0 || EditorSceneManager.SaveScene(scenesCAnonStorey0.scene, "", false)) && !source.Any<EditorBuildSettingsScene>(new Func<EditorBuildSettingsScene, bool>(scenesCAnonStorey0.\u003C\u003Em__0)))
        {
          GUID result;
          // ISSUE: reference to a compiler-generated field
          GUID.TryParse(scenesCAnonStorey0.scene.guid, out result);
          // ISSUE: reference to a compiler-generated field
          EditorBuildSettingsScene buildSettingsScene = !(result == new GUID()) ? new EditorBuildSettingsScene(result, true) : new EditorBuildSettingsScene(scenesCAnonStorey0.scene.path, true);
          source.Add(buildSettingsScene);
          flag = true;
        }
      }
      if (!flag)
        return;
      EditorBuildSettings.scenes = source.ToArray();
      this.m_TreeView.Reload();
      this.Repaint();
      GUIUtility.ExitGUI();
    }

    internal static BuildTarget CalculateSelectedBuildTarget()
    {
      BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
      switch (buildTargetGroup)
      {
        case BuildTargetGroup.Standalone:
          return DesktopStandaloneBuildWindowExtension.GetBestStandaloneTarget(EditorUserBuildSettings.selectedStandaloneTarget);
        case BuildTargetGroup.Facebook:
          return EditorUserBuildSettings.selectedFacebookTarget;
        default:
          if (BuildPlatforms.instance == null)
            throw new Exception("Build platforms are not initialized.");
          BuildPlatform buildPlatform = BuildPlatforms.instance.BuildPlatformFromTargetGroup(buildTargetGroup);
          if (buildPlatform == null)
            throw new Exception("Could not find build platform for target group " + (object) buildTargetGroup);
          return buildPlatform.defaultTarget;
      }
    }

    private void ActiveBuildTargetsGUI()
    {
      GUILayout.BeginVertical();
      GUILayout.BeginVertical(GUILayout.Width((float) byte.MaxValue));
      GUILayout.Label(BuildPlayerWindow.styles.platformTitle, BuildPlayerWindow.styles.title, new GUILayoutOption[0]);
      this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, (GUIStyle) "OL Box");
      for (int index = 0; index < 2; ++index)
      {
        bool flag1 = index == 0;
        bool flag2 = false;
        foreach (BuildPlatform buildPlatform in BuildPlatforms.instance.buildPlatforms)
        {
          if (BuildPlayerWindow.IsBuildTargetGroupSupported(buildPlatform.targetGroup, buildPlatform.defaultTarget) == flag1 && (BuildPlayerWindow.IsBuildTargetGroupSupported(buildPlatform.targetGroup, buildPlatform.defaultTarget) || buildPlatform.forceShowTarget) && BuildPipeline.IsBuildTargetCompatibleWithOS(buildPlatform.defaultTarget))
          {
            this.ShowOption(buildPlatform, buildPlatform.title, !flag2 ? BuildPlayerWindow.styles.oddRow : BuildPlayerWindow.styles.evenRow);
            flag2 = !flag2;
          }
        }
        GUI.contentColor = Color.white;
      }
      GUILayout.EndScrollView();
      GUILayout.EndVertical();
      GUILayout.Space(10f);
      BuildTarget selectedBuildTarget = BuildPlayerWindow.CalculateSelectedBuildTarget();
      BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
      GUILayout.BeginHorizontal();
      GUI.enabled = BuildPipeline.IsBuildTargetSupported(buildTargetGroup, selectedBuildTarget) && EditorUserBuildSettings.activeBuildTargetGroup != buildTargetGroup;
      if (GUILayout.Button(BuildPlayerWindow.styles.switchPlatform, new GUILayoutOption[1]{ GUILayout.Width(110f) }))
      {
        EditorUserBuildSettings.SwitchActiveBuildTargetAsync(buildTargetGroup, selectedBuildTarget);
        GUIUtility.ExitGUI();
      }
      GUI.enabled = BuildPipeline.IsBuildTargetSupported(buildTargetGroup, selectedBuildTarget);
      if (GUILayout.Button(new GUIContent("Player Settings..."), new GUILayoutOption[1]{ GUILayout.Width(110f) }))
      {
        Selection.activeObject = Unsupported.GetSerializedAssetInterfaceSingleton("PlayerSettings");
        EditorWindow.GetWindow<InspectorWindow>();
      }
      GUILayout.EndHorizontal();
      GUI.enabled = true;
      GUILayout.EndVertical();
    }

    private void ShowAlert()
    {
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      GUILayout.BeginVertical();
      EditorGUILayout.HelpBox(EditorGUIUtility.TextContent("Unable to access Unity services. Please log in, or request membership to this project to use these services.").text, MessageType.Warning);
      GUILayout.EndVertical();
      GUILayout.Space(5f);
      GUILayout.EndHorizontal();
    }

    private void ShowOption(BuildPlatform bp, GUIContent title, GUIStyle background)
    {
      Rect rect = GUILayoutUtility.GetRect(50f, 36f);
      ++rect.x;
      ++rect.y;
      GUI.contentColor = new Color(1f, 1f, 1f, !BuildPipeline.LicenseCheck(bp.defaultTarget) ? 0.7f : 1f);
      bool on = EditorUserBuildSettings.selectedBuildTargetGroup == bp.targetGroup;
      if (Event.current.type == EventType.Repaint)
      {
        background.Draw(rect, GUIContent.none, false, false, on, false);
        GUI.Label(new Rect(rect.x + 3f, rect.y + 3f, 32f, 32f), title.image, GUIStyle.none);
        if (EditorUserBuildSettings.activeBuildTargetGroup == bp.targetGroup)
          GUI.Label(new Rect((float) ((double) rect.xMax - (double) BuildPlayerWindow.styles.activePlatformIcon.width - 8.0), rect.y + 3f + (float) ((32 - BuildPlayerWindow.styles.activePlatformIcon.height) / 2), (float) BuildPlayerWindow.styles.activePlatformIcon.width, (float) BuildPlayerWindow.styles.activePlatformIcon.height), (Texture) BuildPlayerWindow.styles.activePlatformIcon, GUIStyle.none);
      }
      if (!GUI.Toggle(rect, on, title.text, BuildPlayerWindow.styles.platformSelector) || EditorUserBuildSettings.selectedBuildTargetGroup == bp.targetGroup)
        return;
      EditorUserBuildSettings.selectedBuildTargetGroup = bp.targetGroup;
      foreach (UnityEngine.Object @object in UnityEngine.Resources.FindObjectsOfTypeAll(typeof (InspectorWindow)))
      {
        InspectorWindow inspectorWindow = @object as InspectorWindow;
        if ((UnityEngine.Object) inspectorWindow != (UnityEngine.Object) null)
          inspectorWindow.Repaint();
      }
    }

    private void OnGUI()
    {
      if (BuildPlayerWindow.styles == null)
      {
        BuildPlayerWindow.styles = new BuildPlayerWindow.Styles();
        BuildPlayerWindow.styles.toggleSize = BuildPlayerWindow.styles.toggle.CalcSize(new GUIContent("X"));
      }
      if (!UnityConnect.instance.canBuildWithUPID)
        this.ShowAlert();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      GUILayout.BeginVertical();
      string message = "";
      bool disabled = !AssetDatabase.IsOpenForEdit("ProjectSettings/EditorBuildSettings.asset", out message, StatusQueryOptions.UseCachedIfPossible);
      using (new EditorGUI.DisabledScope(disabled))
      {
        this.ActiveScenesGUI();
        GUILayout.BeginHorizontal();
        if (disabled)
        {
          GUI.enabled = true;
          if (Provider.enabled && GUILayout.Button("Check out"))
          {
            Asset assetByPath = Provider.GetAssetByPath("ProjectSettings/EditorBuildSettings.asset");
            AssetList assets = new AssetList();
            assets.Add(assetByPath);
            Provider.Checkout(assets, CheckoutMode.Asset);
          }
          GUILayout.Label(message);
          GUI.enabled = false;
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Open Scenes"))
          this.AddOpenScenes();
        GUILayout.EndHorizontal();
      }
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal(GUILayout.Height(351f));
      this.ActiveBuildTargetsGUI();
      GUILayout.Space(10f);
      GUILayout.BeginVertical();
      this.ShowBuildTargetSettings();
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      GUILayout.Space(10f);
      GUILayout.EndVertical();
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
    }

    internal static bool IsBuildTargetGroupSupported(BuildTargetGroup targetGroup, BuildTarget target)
    {
      if (targetGroup == BuildTargetGroup.Standalone)
        return true;
      return BuildPipeline.IsBuildTargetSupported(targetGroup, target);
    }

    private static void RepairSelectedBuildTargetGroup()
    {
      BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
      if (buildTargetGroup != BuildTargetGroup.Unknown && BuildPlatforms.instance.BuildPlatformIndexFromTargetGroup(buildTargetGroup) >= 0)
        return;
      EditorUserBuildSettings.selectedBuildTargetGroup = BuildTargetGroup.Standalone;
    }

    private static bool IsAnyStandaloneModuleLoaded()
    {
      return ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneLinux)) || ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneOSX)) || ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneWindows));
    }

    private static bool IsColorSpaceValid(BuildPlatform platform)
    {
      if (PlayerSettings.colorSpace != ColorSpace.Linear)
        return true;
      bool flag1 = true;
      bool flag2 = true;
      if (platform.targetGroup == BuildTargetGroup.iPhone)
      {
        GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.iOS);
        flag1 = !((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES3) && !((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES2);
        flag2 = (!string.IsNullOrEmpty(PlayerSettings.iOS.targetOSVersionString) ? new Version(PlayerSettings.iOS.targetOSVersionString) : new Version(6, 0)) >= new Version(8, 0);
      }
      else if (platform.targetGroup == BuildTargetGroup.tvOS)
      {
        GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.tvOS);
        flag1 = !((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES3) && !((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES2);
      }
      else if (platform.targetGroup == BuildTargetGroup.Android)
      {
        GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
        flag1 = (((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.Vulkan) || ((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES3)) && !((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES2);
        flag2 = PlayerSettings.Android.minSdkVersion >= AndroidSdkVersions.AndroidApiLevel18;
      }
      else if (platform.targetGroup == BuildTargetGroup.WebGL)
      {
        GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.WebGL);
        flag1 = ((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES3) && !((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES2);
      }
      return flag1 && flag2;
    }

    public static string GetPlaybackEngineDownloadURL(string moduleName)
    {
      if (moduleName == "PS4" || moduleName == "PSP2" || moduleName == "XboxOne")
        return "https://unity3d.com/platform-installation";
      string fullUnityVersion = InternalEditorUtility.GetFullUnityVersion();
      string str1 = "";
      string str2 = "";
      Match match = BuildPlayerWindow.s_VersionPattern.Match(fullUnityVersion);
      if (!match.Success || !match.Groups["shortVersion"].Success || !match.Groups["suffix"].Success)
        Debug.LogWarningFormat("Error parsing version '{0}'", (object) fullUnityVersion);
      if (match.Groups["shortVersion"].Success)
        str2 = match.Groups["shortVersion"].Value;
      if (match.Groups["revision"].Success)
        str1 = match.Groups["revision"].Value;
      if (BuildPlayerWindow.s_ModuleNames.ContainsKey(moduleName))
        moduleName = BuildPlayerWindow.s_ModuleNames[moduleName];
      string str3 = "download";
      string str4 = "download_unity";
      string str5 = "Unknown";
      string str6 = string.Empty;
      if (match.Groups["alphabeta"].Success)
      {
        str3 = "beta";
        str4 = "download";
      }
      switch (Application.platform)
      {
        case RuntimePlatform.OSXEditor:
          str5 = "MacEditorTargetInstaller";
          str6 = ".pkg";
          break;
        case RuntimePlatform.WindowsEditor:
          str5 = "TargetSupportInstaller";
          str6 = ".exe";
          break;
      }
      return string.Format("http://{0}.unity3d.com/{1}/{2}/{3}/UnitySetup-{4}-Support-for-Editor-{5}{6}", (object) str3, (object) str4, (object) str1, (object) str5, (object) moduleName, (object) str2, (object) str6);
    }

    private bool IsModuleInstalled(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget)
    {
      bool flag = BuildPipeline.LicenseCheck(buildTarget);
      string targetStringFrom = ModuleManager.GetTargetStringFrom(buildTargetGroup, buildTarget);
      return flag && !string.IsNullOrEmpty(targetStringFrom) && ModuleManager.GetBuildPostProcessor(targetStringFrom) == null && (EditorUserBuildSettings.selectedBuildTargetGroup != BuildTargetGroup.Standalone || !BuildPlayerWindow.IsAnyStandaloneModuleLoaded());
    }

    private void ShowBuildTargetSettings()
    {
      EditorGUIUtility.labelWidth = Mathf.Min(180f, (float) (((double) this.position.width - 265.0) * 0.469999998807907));
      BuildTarget selectedBuildTarget = BuildPlayerWindow.CalculateSelectedBuildTarget();
      BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
      BuildPlatform platform = BuildPlatforms.instance.BuildPlatformFromTargetGroup(buildTargetGroup);
      bool flag1 = BuildPipeline.LicenseCheck(selectedBuildTarget);
      GUILayout.Space(18f);
      Rect rect = GUILayoutUtility.GetRect(50f, 36f);
      ++rect.x;
      GUI.Label(new Rect(rect.x + 3f, rect.y + 3f, 32f, 32f), platform.title.image, GUIStyle.none);
      GUI.Toggle(rect, false, platform.title.text, BuildPlayerWindow.styles.platformSelector);
      GUILayout.Space(10f);
      if (platform.targetGroup == BuildTargetGroup.WebGL && !BuildPipeline.IsBuildTargetSupported(platform.targetGroup, selectedBuildTarget) && IntPtr.Size == 4)
      {
        GUILayout.Label("Building for WebGL requires a 64-bit Unity editor.");
        BuildPlayerWindow.GUIBuildButtons(false, false, false, platform);
      }
      else
      {
        string targetStringFrom = ModuleManager.GetTargetStringFrom(buildTargetGroup, selectedBuildTarget);
        if (this.IsModuleInstalled(buildTargetGroup, selectedBuildTarget))
        {
          GUILayout.Label("No " + BuildPlatforms.instance.GetModuleDisplayName(buildTargetGroup, selectedBuildTarget) + " module loaded.");
          if (GUILayout.Button("Open Download Page", EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
            Help.BrowseURL(BuildPlayerWindow.GetPlaybackEngineDownloadURL(targetStringFrom));
          BuildPlayerWindow.GUIBuildButtons(false, false, false, platform);
        }
        else
        {
          if (Application.HasProLicense() && !InternalEditorUtility.HasAdvancedLicenseOnBuildTarget(selectedBuildTarget))
          {
            string text = string.Format("{0} is not included in your Unity Pro license. Your {0} build will include a Unity Personal Edition splash screen.\n\nYou must be eligible to use Unity Personal Edition to use this build option. Please refer to our EULA for further information.", (object) BuildPlatforms.instance.GetBuildTargetDisplayName(buildTargetGroup, selectedBuildTarget));
            GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
            GUILayout.Label(text, EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("EULA", EditorStyles.miniButton, new GUILayoutOption[0]))
              Application.OpenURL("http://unity3d.com/legal/eula");
            if (GUILayout.Button(string.Format("Add {0} to your Unity Pro license", (object) BuildPlatforms.instance.GetBuildTargetDisplayName(buildTargetGroup, selectedBuildTarget)), EditorStyles.miniButton, new GUILayoutOption[0]))
              Application.OpenURL("http://unity3d.com/get-unity");
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
          }
          GUIContent downloadErrorForTarget = BuildPlayerWindow.styles.GetDownloadErrorForTarget(selectedBuildTarget);
          if (downloadErrorForTarget != null)
          {
            GUILayout.Label(downloadErrorForTarget, EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
            BuildPlayerWindow.GUIBuildButtons(false, false, false, platform);
          }
          else if (!flag1)
          {
            int index = BuildPlatforms.instance.BuildPlatformIndexFromTargetGroup(platform.targetGroup);
            GUILayout.Label(BuildPlayerWindow.styles.notLicensedMessages[index, 0], EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
            GUILayout.Space(5f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (BuildPlayerWindow.styles.notLicensedMessages[index, 1].text.Length != 0 && GUILayout.Button(BuildPlayerWindow.styles.notLicensedMessages[index, 1]))
              Application.OpenURL(BuildPlayerWindow.styles.notLicensedMessages[index, 2].text);
            GUILayout.EndHorizontal();
            BuildPlayerWindow.GUIBuildButtons(false, false, false, platform);
          }
          else
          {
            IBuildWindowExtension buildWindowExtension = ModuleManager.GetBuildWindowExtension(ModuleManager.GetTargetStringFrom(platform.targetGroup, selectedBuildTarget));
            if (buildWindowExtension != null)
              buildWindowExtension.ShowPlatformBuildOptions();
            GUI.changed = false;
            switch (platform.targetGroup)
            {
              case BuildTargetGroup.iPhone:
              case BuildTargetGroup.tvOS:
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                  EditorUserBuildSettings.symlinkLibraries = EditorGUILayout.Toggle(BuildPlayerWindow.styles.symlinkiOSLibraries, EditorUserBuildSettings.symlinkLibraries, new GUILayoutOption[0]);
                  break;
                }
                break;
            }
            GUI.enabled = true;
            bool enableBuildButton = buildWindowExtension == null || buildWindowExtension.EnabledBuildButton();
            bool enableBuildAndRunButton = false;
            bool flag2 = buildWindowExtension == null || buildWindowExtension.ShouldDrawScriptDebuggingCheckbox();
            bool flag3 = buildWindowExtension != null && buildWindowExtension.ShouldDrawExplicitNullCheckbox();
            bool flag4 = buildWindowExtension != null && buildWindowExtension.ShouldDrawExplicitDivideByZeroCheckbox();
            bool flag5 = buildWindowExtension == null || buildWindowExtension.ShouldDrawDevelopmentPlayerCheckbox();
            bool flag6 = selectedBuildTarget == BuildTarget.StandaloneLinux || selectedBuildTarget == BuildTarget.StandaloneLinux64 || selectedBuildTarget == BuildTarget.StandaloneLinuxUniversal;
            IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(buildTargetGroup, selectedBuildTarget);
            bool flag7 = buildPostProcessor != null && buildPostProcessor.SupportsScriptsOnlyBuild();
            bool canInstallInBuildFolder = false;
            if (BuildPipeline.IsBuildTargetSupported(buildTargetGroup, selectedBuildTarget))
            {
              bool flag8 = buildWindowExtension == null || buildWindowExtension.ShouldDrawProfilerCheckbox();
              GUI.enabled = flag5;
              if (flag5)
                EditorUserBuildSettings.development = EditorGUILayout.Toggle(BuildPlayerWindow.styles.debugBuild, EditorUserBuildSettings.development, new GUILayoutOption[0]);
              bool development = EditorUserBuildSettings.development;
              GUI.enabled = development;
              if (flag8)
              {
                if (!GUI.enabled)
                {
                  if (!development)
                    BuildPlayerWindow.styles.profileBuild.tooltip = "Profiling only enabled in Development Player";
                }
                else
                  BuildPlayerWindow.styles.profileBuild.tooltip = "";
                EditorUserBuildSettings.connectProfiler = EditorGUILayout.Toggle(BuildPlayerWindow.styles.profileBuild, EditorUserBuildSettings.connectProfiler, new GUILayoutOption[0]);
              }
              GUI.enabled = development;
              if (flag2)
              {
                EditorUserBuildSettings.allowDebugging = EditorGUILayout.Toggle(BuildPlayerWindow.styles.allowDebugging, EditorUserBuildSettings.allowDebugging, new GUILayoutOption[0]);
                if (EditorUserBuildSettings.allowDebugging && Unsupported.IsDeveloperBuild())
                {
                  string buildTargetName = BuildPipeline.GetBuildTargetName(selectedBuildTarget);
                  bool flag9 = EditorGUILayout.Toggle(BuildPlayerWindow.styles.waitForManagedDebugger, EditorUserBuildSettings.GetPlatformSettings(buildTargetName, "WaitForManagedDebugger") == "true", new GUILayoutOption[0]);
                  EditorUserBuildSettings.SetPlatformSettings(buildTargetName, "WaitForManagedDebugger", flag9.ToString().ToLower());
                }
              }
              if (flag3)
              {
                GUI.enabled = !development;
                if (!GUI.enabled)
                  EditorUserBuildSettings.explicitNullChecks = true;
                EditorUserBuildSettings.explicitNullChecks = EditorGUILayout.Toggle(BuildPlayerWindow.styles.explicitNullChecks, EditorUserBuildSettings.explicitNullChecks, new GUILayoutOption[0]);
                GUI.enabled = development;
              }
              if (flag4)
              {
                GUI.enabled = !development;
                if (!GUI.enabled)
                  EditorUserBuildSettings.explicitDivideByZeroChecks = true;
                EditorUserBuildSettings.explicitDivideByZeroChecks = EditorGUILayout.Toggle(BuildPlayerWindow.styles.explicitDivideByZeroChecks, EditorUserBuildSettings.explicitDivideByZeroChecks, new GUILayoutOption[0]);
                GUI.enabled = development;
              }
              if (flag7)
                EditorUserBuildSettings.buildScriptsOnly = EditorGUILayout.Toggle(BuildPlayerWindow.styles.buildScriptsOnly, EditorUserBuildSettings.buildScriptsOnly, new GUILayoutOption[0]);
              GUI.enabled = !development;
              if (flag6)
                EditorUserBuildSettings.enableHeadlessMode = EditorGUILayout.Toggle(BuildPlayerWindow.styles.enableHeadlessMode, EditorUserBuildSettings.enableHeadlessMode && !development, new GUILayoutOption[0]);
              GUI.enabled = true;
              GUILayout.FlexibleSpace();
              if (buildPostProcessor != null && buildPostProcessor.SupportsLz4Compression())
              {
                int selectedIndex = Array.IndexOf<Compression>(BuildPlayerWindow.styles.compressionTypes, EditorUserBuildSettings.GetCompressionType(buildTargetGroup));
                if (selectedIndex == -1)
                  selectedIndex = 1;
                int index = EditorGUILayout.Popup(BuildPlayerWindow.styles.compressionMethod, selectedIndex, BuildPlayerWindow.styles.compressionStrings, new GUILayoutOption[0]);
                EditorUserBuildSettings.SetCompressionType(buildTargetGroup, BuildPlayerWindow.styles.compressionTypes[index]);
              }
              canInstallInBuildFolder = Unsupported.IsDeveloperBuild() && PostprocessBuildPlayer.SupportsInstallInBuildFolder(buildTargetGroup, selectedBuildTarget);
              if (enableBuildButton)
                enableBuildAndRunButton = buildWindowExtension == null ? !EditorUserBuildSettings.installInBuildFolder : buildWindowExtension.EnabledBuildAndRunButton() && !EditorUserBuildSettings.installInBuildFolder;
            }
            else
            {
              GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
              GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
              int index = BuildPlatforms.instance.BuildPlatformIndexFromTargetGroup(platform.targetGroup);
              GUILayout.Label(BuildPlayerWindow.styles.GetTargetNotInstalled(index, 0));
              if (BuildPlayerWindow.styles.GetTargetNotInstalled(index, 1) != null && GUILayout.Button(BuildPlayerWindow.styles.GetTargetNotInstalled(index, 1)))
                Application.OpenURL(BuildPlayerWindow.styles.GetTargetNotInstalled(index, 2).text);
              GUILayout.EndVertical();
              GUILayout.FlexibleSpace();
              GUILayout.EndHorizontal();
            }
            if (selectedBuildTarget == BuildTarget.Android)
              this.AndroidPublishGUI();
            BuildPlayerWindow.GUIBuildButtons(buildWindowExtension, enableBuildButton, enableBuildAndRunButton, canInstallInBuildFolder, platform);
          }
        }
      }
    }

    private static void GUIBuildButtons(bool enableBuildButton, bool enableBuildAndRunButton, bool canInstallInBuildFolder, BuildPlatform platform)
    {
      BuildPlayerWindow.GUIBuildButtons((IBuildWindowExtension) null, enableBuildButton, enableBuildAndRunButton, canInstallInBuildFolder, platform);
    }

    private static void GUIBuildButtons(IBuildWindowExtension buildWindowExtension, bool enableBuildButton, bool enableBuildAndRunButton, bool canInstallInBuildFolder, BuildPlatform platform)
    {
      GUILayout.FlexibleSpace();
      if (canInstallInBuildFolder)
        EditorUserBuildSettings.installInBuildFolder = GUILayout.Toggle((EditorUserBuildSettings.installInBuildFolder ? 1 : 0) != 0, "Install in Builds folder\n(for debugging with source code)", new GUILayoutOption[1]
        {
          GUILayout.ExpandWidth(false)
        });
      else
        EditorUserBuildSettings.installInBuildFolder = false;
      if (buildWindowExtension != null && Unsupported.IsDeveloperBuild())
        buildWindowExtension.ShowInternalPlatformBuildOptions();
      if (!BuildPlayerWindow.IsColorSpaceValid(platform) && enableBuildButton && enableBuildAndRunButton)
      {
        enableBuildAndRunButton = false;
        enableBuildButton = false;
        EditorGUILayout.HelpBox(BuildPlayerWindow.Styles.invalidColorSpaceMessage.text, MessageType.Warning);
      }
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (EditorGUILayout.LinkLabel(BuildPlayerWindow.styles.learnAboutUnityCloudBuild))
        Application.OpenURL(string.Format("{0}/from/editor/buildsettings?upid={1}&pid={2}&currentplatform={3}&selectedplatform={4}&unityversion={5}", (object) WebURLs.cloudBuildPage, (object) PlayerSettings.cloudProjectId, (object) PlayerSettings.productGUID, (object) EditorUserBuildSettings.activeBuildTarget, (object) BuildPlayerWindow.CalculateSelectedBuildTarget(), (object) Application.unityVersion));
      GUILayout.EndHorizontal();
      GUILayout.Space(6f);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUIContent content = BuildPlayerWindow.styles.build;
      if (platform.targetGroup == BuildTargetGroup.Android && EditorUserBuildSettings.exportAsGoogleAndroidProject)
        content = BuildPlayerWindow.styles.export;
      GUI.enabled = enableBuildButton;
      if (GUILayout.Button(content, new GUILayoutOption[1]{ GUILayout.Width(110f) }))
      {
        BuildPlayerWindow.CallBuildMethods(true, BuildOptions.ShowBuiltPlayer | BuildOptions.StrictMode);
        GUIUtility.ExitGUI();
      }
      GUI.enabled = enableBuildAndRunButton;
      if (GUILayout.Button(BuildPlayerWindow.styles.buildAndRun, new GUILayoutOption[1]{ GUILayout.Width(110f) }))
      {
        BuildPlayerWindow.BuildPlayerAndRun(true);
        GUIUtility.ExitGUI();
      }
      GUILayout.EndHorizontal();
    }

    public static void RegisterGetBuildPlayerOptionsHandler(Func<BuildPlayerOptions, BuildPlayerOptions> func)
    {
      if (func != null && BuildPlayerWindow.getBuildPlayerOptionsHandler != null)
        Debug.LogWarning((object) "The get build player options handler in BuildPlayerWindow is being reassigned!");
      BuildPlayerWindow.getBuildPlayerOptionsHandler = func;
    }

    public static void RegisterBuildPlayerHandler(Action<BuildPlayerOptions> func)
    {
      if (func != null && BuildPlayerWindow.buildPlayerHandler != null)
        Debug.LogWarning((object) "The build player handler in BuildPlayerWindow is being reassigned!");
      BuildPlayerWindow.buildPlayerHandler = func;
    }

    private static void CallBuildMethods(bool askForBuildLocation, BuildOptions defaultBuildOptions)
    {
      if (BuildPlayerWindow.m_Building)
        return;
      try
      {
        BuildPlayerWindow.m_Building = true;
        BuildPlayerOptions defaultBuildPlayerOptions = new BuildPlayerOptions();
        defaultBuildPlayerOptions.options = defaultBuildOptions;
        BuildPlayerOptions options = BuildPlayerWindow.getBuildPlayerOptionsHandler == null ? BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptionsInternal(askForBuildLocation, defaultBuildPlayerOptions) : BuildPlayerWindow.getBuildPlayerOptionsHandler(defaultBuildPlayerOptions);
        if (BuildPlayerWindow.buildPlayerHandler != null)
          BuildPlayerWindow.buildPlayerHandler(options);
        else
          BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(options);
      }
      catch (BuildPlayerWindow.BuildMethodException ex)
      {
        if (string.IsNullOrEmpty(ex.Message))
          return;
        Debug.LogError((object) ex);
      }
      finally
      {
        BuildPlayerWindow.m_Building = false;
      }
    }

    private string CurrentXiaomiPackageId
    {
      get
      {
        return this.xiaomiPackageName + "@" + this.currentXiaomiPackageVersion;
      }
    }

    private string LatestXiaomiPackageId
    {
      get
      {
        return this.xiaomiPackageName + "@" + this.latestXiaomiPackageVersion;
      }
    }

    private void AndroidPublishGUI()
    {
      if (this.publishStyles == null)
        this.publishStyles = new BuildPlayerWindow.PublishStyles();
      GUILayout.BeginVertical();
      GUILayout.Label(this.publishStyles.publishTitle, BuildPlayerWindow.styles.title, new GUILayoutOption[0]);
      using (new EditorGUILayout.HorizontalScope(BuildPlayerWindow.styles.box, new GUILayoutOption[1]{ GUILayout.Height(36f) }))
      {
        GUILayout.BeginVertical();
        GUILayout.Space(3f);
        GUILayout.BeginHorizontal();
        GUILayout.Space(4f);
        GUILayout.BeginVertical();
        GUILayout.Space(2f);
        GUILayout.Label(this.publishStyles.xiaomiIcon, new GUILayoutOption[2]
        {
          GUILayout.Width(32f),
          GUILayout.Height(32f)
        });
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Xiaomi Mi Game Center");
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        this.XiaomiPackageControlGUI();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.Space(4f);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
      }
      GUILayout.EndVertical();
    }

    private void XiaomiPackageControlGUI()
    {
      EditorGUI.BeginDisabledGroup(!this.isVersionInitialized || this.packmanOperationRunning);
      if (!this.xiaomiPackageInstalled)
      {
        if (GUILayout.Button("Add", new GUILayoutOption[1]{ GUILayout.Width(60f) }))
        {
          if (this.packmanOperationRunning)
            return;
          if (NativeClient.Add(out this.packmanOperationId, this.LatestXiaomiPackageId) == NativeClient.StatusCode.Error)
          {
            Debug.LogError((object) ("Add " + this.LatestXiaomiPackageId + " error, please add it again."));
            return;
          }
          this.packmanOperationType = BuildPlayerWindow.PackmanOperationType.Add;
          Console.WriteLine("Add: OperationID " + (object) this.packmanOperationId + " for " + this.LatestXiaomiPackageId);
          this.packmanOperationRunning = true;
        }
      }
      else
      {
        GUILayout.BeginHorizontal();
        if (!string.IsNullOrEmpty(this.latestXiaomiPackageVersion) && this.currentXiaomiPackageVersion != this.latestXiaomiPackageVersion)
        {
          if (GUILayout.Button("Update", new GUILayoutOption[1]{ GUILayout.Width(60f) }))
          {
            if (this.packmanOperationRunning)
              return;
            if (EditorUtility.DisplayDialog("Update Xiaomi SDK", "Are you sure you want to update to " + this.latestXiaomiPackageVersion + " ?", "Yes", "No"))
            {
              if (NativeClient.Add(out this.packmanOperationId, this.LatestXiaomiPackageId) == NativeClient.StatusCode.Error)
              {
                Debug.LogError((object) ("Update " + this.LatestXiaomiPackageId + " error, please update it again."));
                return;
              }
              this.packmanOperationType = BuildPlayerWindow.PackmanOperationType.Add;
              Console.WriteLine("Update: OperationID " + (object) this.packmanOperationId + " for " + this.LatestXiaomiPackageId);
              this.packmanOperationRunning = true;
            }
          }
        }
        if (GUILayout.Button("Remove", new GUILayoutOption[1]{ GUILayout.Width(60f) }))
        {
          if (this.packmanOperationRunning)
            return;
          if (NativeClient.Remove(out this.packmanOperationId, this.CurrentXiaomiPackageId) == NativeClient.StatusCode.Error)
          {
            Debug.LogError((object) ("Remove " + this.CurrentXiaomiPackageId + " error, please remove it again."));
            return;
          }
          this.packmanOperationType = BuildPlayerWindow.PackmanOperationType.Remove;
          Console.WriteLine("Remove: OperationID " + (object) this.packmanOperationId + " for " + this.CurrentXiaomiPackageId);
          this.packmanOperationRunning = true;
        }
        GUILayout.EndHorizontal();
      }
      EditorGUI.EndDisabledGroup();
    }

    private bool CheckXiaomiPackageVersions()
    {
      if (this.isVersionInitialized)
        return true;
      NativeClient.StatusCode statusCode1 = this.getCurrentVersionOperationId >= 0L ? NativeClient.GetOperationStatus(this.getCurrentVersionOperationId) : NativeClient.List(out this.getCurrentVersionOperationId);
      if (statusCode1 > NativeClient.StatusCode.Done)
      {
        this.getCurrentVersionOperationId = -1L;
        return false;
      }
      NativeClient.StatusCode statusCode2 = this.getLatestVersionOperationId >= 0L ? NativeClient.GetOperationStatus(this.getLatestVersionOperationId) : NativeClient.Search(out this.getLatestVersionOperationId, this.xiaomiPackageName);
      if (statusCode2 > NativeClient.StatusCode.Done)
      {
        this.getLatestVersionOperationId = -1L;
        return false;
      }
      if (statusCode1 != NativeClient.StatusCode.Done || statusCode2 != NativeClient.StatusCode.Done)
        return false;
      this.CheckPackmanOperation(this.getCurrentVersionOperationId, BuildPlayerWindow.PackmanOperationType.List);
      this.CheckPackmanOperation(this.getLatestVersionOperationId, BuildPlayerWindow.PackmanOperationType.Search);
      Console.WriteLine("Current xiaomi package version is " + (!string.IsNullOrEmpty(this.currentXiaomiPackageVersion) ? this.currentXiaomiPackageVersion : "empty"));
      Console.WriteLine("Latest xiaomi package version is " + (!string.IsNullOrEmpty(this.latestXiaomiPackageVersion) ? this.latestXiaomiPackageVersion : "empty"));
      this.isVersionInitialized = true;
      return true;
    }

    private void Update()
    {
      if (!this.CheckXiaomiPackageVersions() || !this.packmanOperationRunning)
        return;
      this.packmanOperationRunning = !this.CheckPackmanOperation(this.packmanOperationId, this.packmanOperationType);
    }

    private bool CheckPackmanOperation(long operationId, BuildPlayerWindow.PackmanOperationType operationType)
    {
      switch (NativeClient.GetOperationStatus(operationId))
      {
        case NativeClient.StatusCode.InQueue:
        case NativeClient.StatusCode.InProgress:
          return false;
        case NativeClient.StatusCode.Done:
          Console.WriteLine("OperationID " + (object) operationId + " Done");
          switch (operationType)
          {
            case BuildPlayerWindow.PackmanOperationType.List:
              this.ExtractCurrentXiaomiPackageInfo(operationId);
              break;
            case BuildPlayerWindow.PackmanOperationType.Add:
              this.currentXiaomiPackageVersion = this.latestXiaomiPackageVersion;
              this.xiaomiPackageInstalled = true;
              break;
            case BuildPlayerWindow.PackmanOperationType.Remove:
              this.currentXiaomiPackageVersion = "";
              this.xiaomiPackageInstalled = false;
              break;
            case BuildPlayerWindow.PackmanOperationType.Search:
              this.ExtractLatestXiaomiPackageInfo(operationId);
              break;
            default:
              Console.WriteLine("Type " + (object) operationType + " Not Supported");
              break;
          }
          return true;
        case NativeClient.StatusCode.Error:
          UnityEditor.PackageManager.Error operationError = NativeClient.GetOperationError(operationId);
          Debug.LogError((object) ("OperationID " + (object) operationId + " failed with Error: " + (object) operationError));
          return true;
        case NativeClient.StatusCode.NotFound:
          Debug.LogError((object) ("OperationID " + (object) operationId + " Not Found"));
          return true;
        default:
          return true;
      }
    }

    private void ExtractCurrentXiaomiPackageInfo(long operationId)
    {
      foreach (UpmPackageInfo package in NativeClient.GetListOperationData(operationId).packageList)
      {
        if (package.packageId.StartsWith(this.xiaomiPackageName))
        {
          this.xiaomiPackageInstalled = true;
          this.currentXiaomiPackageVersion = package.version;
        }
      }
    }

    private void ExtractLatestXiaomiPackageInfo(long operationId)
    {
      foreach (UpmPackageInfo upmPackageInfo in NativeClient.GetSearchOperationData(operationId))
        this.latestXiaomiPackageVersion = upmPackageInfo.version;
    }

    private class Styles
    {
      public static readonly GUIContent invalidColorSpaceMessage = EditorGUIUtility.TextContent("In order to build a player go to 'Player Settings...' to resolve the incompatibility between the Color Space and the current settings.");
      public GUIStyle selected = (GUIStyle) "OL SelectedRow";
      public GUIStyle box = (GUIStyle) "OL Box";
      public GUIStyle title = EditorStyles.boldLabel;
      public GUIStyle evenRow = (GUIStyle) "CN EntryBackEven";
      public GUIStyle oddRow = (GUIStyle) "CN EntryBackOdd";
      public GUIStyle platformSelector = (GUIStyle) "PlayerSettingsPlatform";
      public GUIStyle toggle = (GUIStyle) "Toggle";
      public GUIStyle levelString = (GUIStyle) "PlayerSettingsLevel";
      public GUIStyle levelStringCounter = new GUIStyle((GUIStyle) "Label");
      public GUIContent noSessionDialogText = EditorGUIUtility.TextContent("In order to publish your build to UDN, you need to sign in via the AssetStore and tick the 'Stay signed in' checkbox.");
      public GUIContent platformTitle = EditorGUIUtility.TextContent("Platform|Which platform to build for");
      public GUIContent switchPlatform = EditorGUIUtility.TextContent("Switch Platform");
      public GUIContent build = EditorGUIUtility.TextContent("Build");
      public GUIContent export = EditorGUIUtility.TextContent("Export");
      public GUIContent buildAndRun = EditorGUIUtility.TextContent("Build And Run");
      public GUIContent scenesInBuild = EditorGUIUtility.TextContent("Scenes In Build|Which scenes to include in the build");
      public Texture2D activePlatformIcon = EditorGUIUtility.IconContent("BuildSettings.SelectedIcon").image as Texture2D;
      public GUIContent[,] notLicensedMessages = new GUIContent[14, 3]{ { EditorGUIUtility.TextContent("Your license does not cover Standalone Publishing."), new GUIContent(""), new GUIContent("https://store.unity3d.com/shop/") }, { EditorGUIUtility.TextContent("Your license does not cover iOS Publishing."), EditorGUIUtility.TextContent("Go to Our Online Store"), new GUIContent("https://store.unity3d.com/shop/") }, { EditorGUIUtility.TextContent("Your license does not cover Apple TV Publishing."), EditorGUIUtility.TextContent("Go to Our Online Store"), new GUIContent("https://store.unity3d.com/shop/") }, { EditorGUIUtility.TextContent("Your license does not cover Android Publishing."), EditorGUIUtility.TextContent("Go to Our Online Store"), new GUIContent("https://store.unity3d.com/shop/") }, { EditorGUIUtility.TextContent("Your license does not cover Tizen Publishing."), EditorGUIUtility.TextContent("Go to Our Online Store"), new GUIContent("https://store.unity3d.com/shop/") }, { EditorGUIUtility.TextContent("Your license does not cover Xbox One Publishing."), EditorGUIUtility.TextContent("Contact sales"), new GUIContent("http://unity3d.com/company/sales?type=sales") }, { EditorGUIUtility.TextContent("Your license does not cover PS Vita Publishing."), EditorGUIUtility.TextContent("Contact sales"), new GUIContent("http://unity3d.com/company/sales?type=sales") }, { EditorGUIUtility.TextContent("Your license does not cover PS4 Publishing."), EditorGUIUtility.TextContent("Contact sales"), new GUIContent("http://unity3d.com/company/sales?type=sales") }, { EditorGUIUtility.TextContent("Your license does not cover Wii U Publishing."), EditorGUIUtility.TextContent("Contact sales"), new GUIContent("http://unity3d.com/company/sales?type=sales") }, { EditorGUIUtility.TextContent("Your license does not cover Universal Windows Platform Publishing."), EditorGUIUtility.TextContent("Go to Our Online Store"), new GUIContent("https://store.unity3d.com/shop/") }, { EditorGUIUtility.TextContent("Your license does not cover Windows Phone 8 Publishing."), EditorGUIUtility.TextContent("Go to Our Online Store"), new GUIContent("https://store.unity3d.com/shop/") }, { EditorGUIUtility.TextContent("Your license does not cover Nintendo 3DS Publishing"), EditorGUIUtility.TextContent("Contact sales"), new GUIContent("http://unity3d.com/company/sales?type=sales") }, { EditorGUIUtility.TextContent("Your license does not cover Facebook Publishing"), EditorGUIUtility.TextContent("Go to Our Online Store"), new GUIContent("https://store.unity3d.com/shop/") }, { EditorGUIUtility.TextContent("Your license does not cover Nintendo Switch Publishing"), EditorGUIUtility.TextContent("Contact sales"), new GUIContent("http://unity3d.com/company/sales?type=sales") } };
      private GUIContent[,] buildTargetNotInstalled = new GUIContent[14, 3]{ { EditorGUIUtility.TextContent("Standalone Player is not supported in this build.\nDownload a build that supports it."), null, new GUIContent("http://unity3d.com/unity/download/") }, { EditorGUIUtility.TextContent("iOS Player is not supported in this build.\nDownload a build that supports it."), null, new GUIContent("http://unity3d.com/unity/download/") }, { EditorGUIUtility.TextContent("Apple TV Player is not supported in this build.\nDownload a build that supports it."), null, new GUIContent("http://unity3d.com/unity/download/") }, { EditorGUIUtility.TextContent("Android Player is not supported in this build.\nDownload a build that supports it."), null, new GUIContent("http://unity3d.com/unity/download/") }, { EditorGUIUtility.TextContent("Tizen is not supported in this build.\nDownload a build that supports it."), null, new GUIContent("http://unity3d.com/unity/download/") }, { EditorGUIUtility.TextContent("Xbox One Player is not supported in this build.\nDownload a build that supports it."), null, new GUIContent("http://unity3d.com/unity/download/") }, { EditorGUIUtility.TextContent("PS Vita Player is not supported in this build.\nDownload a build that supports it."), null, new GUIContent("http://unity3d.com/unity/download/") }, { EditorGUIUtility.TextContent("PS4 Player is not supported in this build.\nDownload a build that supports it."), null, new GUIContent("http://unity3d.com/unity/download/") }, { EditorGUIUtility.TextContent("Wii U Player is not supported in this build.\nDownload a build that supports it."), null, new GUIContent("http://unity3d.com/unity/download/") }, { EditorGUIUtility.TextContent("Universal Windows Platform Player is not supported in\nthis build.\n\nDownload a build that supports it."), null, new GUIContent("http://unity3d.com/unity/download/") }, { EditorGUIUtility.TextContent("Windows Phone 8 Player is not supported\nin this build.\n\nDownload a build that supports it."), null, new GUIContent("http://unity3d.com/unity/download/") }, { EditorGUIUtility.TextContent("Nintendo 3DS is not supported in this build.\nDownload a build that supports it."), null, new GUIContent("http://unity3d.com/unity/download/") }, { EditorGUIUtility.TextContent("Facebook is not supported in this build.\nDownload a build that supports it."), null, new GUIContent("http://unity3d.com/unity/download/") }, { EditorGUIUtility.TextContent("Nintendo Switch is not supported in this build.\nDownload a build that supports it."), null, new GUIContent("http://unity3d.com/unity/download/") } };
      public GUIContent debugBuild = EditorGUIUtility.TextContent("Development Build");
      public GUIContent profileBuild = EditorGUIUtility.TextContent("Autoconnect Profiler");
      public GUIContent allowDebugging = EditorGUIUtility.TextContent("Script Debugging");
      public GUIContent waitForManagedDebugger = EditorGUIUtility.TextContent("Wait For Managed Debugger|Show a dialog where you can attach a managed debugger before any script execution.");
      public GUIContent symlinkiOSLibraries = EditorGUIUtility.TextContent("Symlink Unity libraries");
      public GUIContent explicitNullChecks = EditorGUIUtility.TextContent("Explicit Null Checks");
      public GUIContent explicitDivideByZeroChecks = EditorGUIUtility.TextContent("Divide By Zero Checks");
      public GUIContent enableHeadlessMode = EditorGUIUtility.TextContent("Headless Mode");
      public GUIContent buildScriptsOnly = EditorGUIUtility.TextContent("Scripts Only Build");
      public GUIContent learnAboutUnityCloudBuild = EditorGUIUtility.TextContent("Learn about Unity Cloud Build");
      public GUIContent compressionMethod = EditorGUIUtility.TextContent("Compression Method|Compression applied to Player data (scenes and resources).\nDefault - none or default platform compression.\nLZ4 - fast compression suitable for Development Builds.\nLZ4HC - higher compression rate variance of LZ4, causes longer build times. Works best for Release Builds.");
      public Compression[] compressionTypes = new Compression[3]{ Compression.None, Compression.Lz4, Compression.Lz4HC };
      public GUIContent[] compressionStrings = new GUIContent[3]{ EditorGUIUtility.TextContent("Default"), EditorGUIUtility.TextContent("LZ4"), EditorGUIUtility.TextContent("LZ4HC") };
      public Vector2 toggleSize;
      public const float kButtonWidth = 110f;
      private const string kShopURL = "https://store.unity3d.com/shop/";
      private const string kDownloadURL = "http://unity3d.com/unity/download/";
      private const string kMailURL = "http://unity3d.com/company/sales?type=sales";

      public Styles()
      {
        this.levelStringCounter.alignment = TextAnchor.MiddleRight;
        if (!Unsupported.IsDeveloperBuild() || this.buildTargetNotInstalled.GetLength(0) == this.notLicensedMessages.GetLength(0) && this.buildTargetNotInstalled.GetLength(0) == BuildPlatforms.instance.buildPlatforms.Length)
          return;
        Debug.LogErrorFormat("Build platforms and messages are desynced in BuildPlayerWindow! ({0} vs. {1} vs. {2}) DON'T SHIP THIS!", (object) this.buildTargetNotInstalled.GetLength(0), (object) this.notLicensedMessages.GetLength(0), (object) BuildPlatforms.instance.buildPlatforms.Length);
      }

      public GUIContent GetTargetNotInstalled(int index, int item)
      {
        if (index >= this.buildTargetNotInstalled.GetLength(0))
          index = 0;
        return this.buildTargetNotInstalled[index, item];
      }

      public GUIContent GetDownloadErrorForTarget(BuildTarget target)
      {
        return (GUIContent) null;
      }
    }

    /// <summary>
    ///   <para>Exceptions used to indicate abort or failure in the callbacks registered via BuildPlayerWindow.RegisterBuildPlayerHandler and BuildPlayerWindow.RegisterGetBuildPlayerOptionsHandler.</para>
    /// </summary>
    public class BuildMethodException : Exception
    {
      /// <summary>
      ///   <para>Create a new BuildMethodException.</para>
      /// </summary>
      /// <param name="message">Display this message as an error in the editor log.</param>
      public BuildMethodException()
        : base("")
      {
      }

      /// <summary>
      ///   <para>Create a new BuildMethodException.</para>
      /// </summary>
      /// <param name="message">Display this message as an error in the editor log.</param>
      public BuildMethodException(string message)
        : base(message)
      {
      }
    }

    /// <summary>
    ///   <para>Default build methods for the BuildPlayerWindow.</para>
    /// </summary>
    public static class DefaultBuildMethods
    {
      /// <summary>
      ///   <para>The built-in, default handler for executing a player build. Can be used to provide default functionality in a custom build player window.</para>
      /// </summary>
      /// <param name="options">The options to build with.</param>
      public static void BuildPlayer(BuildPlayerOptions options)
      {
        if (!UnityConnect.instance.canBuildWithUPID && !EditorUtility.DisplayDialog("Missing Project ID", "Because you are not a member of this project this build will not access Unity services.\nDo you want to continue?", "Yes", "No"))
          throw new BuildPlayerWindow.BuildMethodException();
        if (!BuildPipeline.IsBuildTargetSupported(options.targetGroup, options.target))
          throw new BuildPlayerWindow.BuildMethodException("Build target is not supported.");
        IBuildWindowExtension buildWindowExtension = ModuleManager.GetBuildWindowExtension(ModuleManager.GetTargetStringFrom(EditorUserBuildSettings.selectedBuildTargetGroup, options.target));
        if (buildWindowExtension != null && (options.options & BuildOptions.AutoRunPlayer) != BuildOptions.None && !buildWindowExtension.EnabledBuildAndRunButton())
          throw new BuildPlayerWindow.BuildMethodException();
        if (Unsupported.IsBleedingEdgeBuild())
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.AppendLine("This version of Unity is a BleedingEdge build that has not seen any manual testing.");
          stringBuilder.AppendLine("You should consider this build unstable.");
          stringBuilder.AppendLine("We strongly recommend that you use a normal version of Unity instead.");
          if (EditorUtility.DisplayDialog("BleedingEdge Build", stringBuilder.ToString(), "Cancel", "OK"))
            throw new BuildPlayerWindow.BuildMethodException();
        }
        bool delayToAfterScriptReload = false;
        if (EditorUserBuildSettings.activeBuildTarget != options.target || EditorUserBuildSettings.activeBuildTargetGroup != options.targetGroup)
        {
          if (!EditorUserBuildSettings.SwitchActiveBuildTargetAsync(options.targetGroup, options.target))
            throw new BuildPlayerWindow.BuildMethodException(string.Format("Could not switch to build target '{0}', '{1}'.", (object) BuildPipeline.GetBuildTargetGroupDisplayName(options.targetGroup), (object) BuildPlatforms.instance.GetBuildTargetDisplayName(options.targetGroup, options.target)));
          if (EditorApplication.isCompiling)
            delayToAfterScriptReload = true;
        }
        BuildReport buildReport = BuildPipeline.BuildPlayerInternalNoCheck(options.scenes, options.locationPathName, (string) null, options.targetGroup, options.target, options.options, delayToAfterScriptReload);
        if (!((UnityEngine.Object) buildReport != (UnityEngine.Object) null))
          return;
        string str = string.Format("Build completed with a result of '{0}'", (object) buildReport.buildResult.ToString("g"));
        switch (buildReport.buildResult)
        {
          case BuildResult.Unknown:
            Debug.LogWarning((object) str);
            break;
          case BuildResult.Failed:
            Debug.LogError((object) str);
            throw new BuildPlayerWindow.BuildMethodException(buildReport.SummarizeErrors());
          default:
            Debug.Log((object) str);
            break;
        }
      }

      /// <summary>
      ///   <para>The built-in, default handler for calculating build player options. Can be used to provide default functionality in a custom build player window.</para>
      /// </summary>
      /// <param name="defaultBuildPlayerOptions">Default options.</param>
      /// <returns>
      ///   <para>The calculated BuildPlayerOptions.</para>
      /// </returns>
      public static BuildPlayerOptions GetBuildPlayerOptions(BuildPlayerOptions defaultBuildPlayerOptions)
      {
        return BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptionsInternal(true, defaultBuildPlayerOptions);
      }

      internal static BuildPlayerOptions GetBuildPlayerOptionsInternal(bool askForBuildLocation, BuildPlayerOptions defaultBuildPlayerOptions)
      {
        BuildPlayerOptions buildPlayerOptions = defaultBuildPlayerOptions;
        bool updateExistingBuild = false;
        BuildTarget selectedBuildTarget = BuildPlayerWindow.CalculateSelectedBuildTarget();
        BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        bool flag = EditorUserBuildSettings.installInBuildFolder && PostprocessBuildPlayer.SupportsInstallInBuildFolder(buildTargetGroup, selectedBuildTarget) && (Unsupported.IsDeveloperBuild() || BuildPlayerWindow.DefaultBuildMethods.IsMetroPlayer(selectedBuildTarget));
        if (PostprocessBuildPlayer.SupportsLz4Compression(buildTargetGroup, selectedBuildTarget))
        {
          if (EditorUserBuildSettings.GetCompressionType(buildTargetGroup) == Compression.Lz4)
            buildPlayerOptions.options |= BuildOptions.CompressWithLz4;
          else if (EditorUserBuildSettings.GetCompressionType(buildTargetGroup) == Compression.Lz4HC)
            buildPlayerOptions.options |= BuildOptions.CompressWithLz4HC;
        }
        bool development = EditorUserBuildSettings.development;
        if (development)
          buildPlayerOptions.options |= BuildOptions.Development;
        if (EditorUserBuildSettings.allowDebugging && development)
          buildPlayerOptions.options |= BuildOptions.AllowDebugging;
        if (EditorUserBuildSettings.symlinkLibraries)
          buildPlayerOptions.options |= BuildOptions.SymlinkLibraries;
        if (selectedBuildTarget == BuildTarget.Android && EditorUserBuildSettings.exportAsGoogleAndroidProject)
          buildPlayerOptions.options |= BuildOptions.AcceptExternalModificationsToPlayer;
        if (EditorUserBuildSettings.enableHeadlessMode)
          buildPlayerOptions.options |= BuildOptions.EnableHeadlessMode;
        if (EditorUserBuildSettings.connectProfiler && (development || selectedBuildTarget == BuildTarget.WSAPlayer))
          buildPlayerOptions.options |= BuildOptions.ConnectWithProfiler;
        if (EditorUserBuildSettings.buildScriptsOnly)
          buildPlayerOptions.options |= BuildOptions.BuildScriptsOnly;
        if (flag)
          buildPlayerOptions.options |= BuildOptions.InstallInBuildFolder;
        if (!flag)
        {
          if (askForBuildLocation && !BuildPlayerWindow.DefaultBuildMethods.PickBuildLocation(buildTargetGroup, selectedBuildTarget, buildPlayerOptions.options, out updateExistingBuild))
            throw new BuildPlayerWindow.BuildMethodException();
          string buildLocation = EditorUserBuildSettings.GetBuildLocation(selectedBuildTarget);
          if (buildLocation.Length == 0)
            throw new BuildPlayerWindow.BuildMethodException("Build location for buildTarget " + selectedBuildTarget.ToString() + "is not valid.");
          if (!askForBuildLocation)
          {
            switch (InternalEditorUtility.BuildCanBeAppended(selectedBuildTarget, buildLocation))
            {
              case CanAppendBuild.Yes:
                updateExistingBuild = true;
                break;
              case CanAppendBuild.No:
                if (!BuildPlayerWindow.DefaultBuildMethods.PickBuildLocation(buildTargetGroup, selectedBuildTarget, buildPlayerOptions.options, out updateExistingBuild))
                  throw new BuildPlayerWindow.BuildMethodException();
                if (!BuildPlayerWindow.BuildLocationIsValid(EditorUserBuildSettings.GetBuildLocation(selectedBuildTarget)))
                  throw new BuildPlayerWindow.BuildMethodException("Build location for buildTarget " + selectedBuildTarget.ToString() + "is not valid.");
                break;
            }
          }
        }
        if (updateExistingBuild)
          buildPlayerOptions.options |= BuildOptions.AcceptExternalModificationsToPlayer;
        buildPlayerOptions.target = selectedBuildTarget;
        buildPlayerOptions.targetGroup = buildTargetGroup;
        buildPlayerOptions.locationPathName = EditorUserBuildSettings.GetBuildLocation(selectedBuildTarget);
        buildPlayerOptions.assetBundleManifestPath = (string) null;
        ArrayList arrayList = new ArrayList();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
          if (scene.enabled)
            arrayList.Add((object) scene.path);
        }
        buildPlayerOptions.scenes = arrayList.ToArray(typeof (string)) as string[];
        return buildPlayerOptions;
      }

      private static bool PickBuildLocation(BuildTargetGroup targetGroup, BuildTarget target, BuildOptions options, out bool updateExistingBuild)
      {
        updateExistingBuild = false;
        string buildLocation = EditorUserBuildSettings.GetBuildLocation(target);
        string directory;
        string defaultName;
        if (buildLocation == string.Empty)
        {
          directory = FileUtil.DeleteLastPathNameComponent(Application.dataPath);
          defaultName = "";
        }
        else
        {
          directory = FileUtil.DeleteLastPathNameComponent(buildLocation);
          defaultName = FileUtil.GetLastPathNameComponent(buildLocation);
        }
        if (target == BuildTarget.Android && EditorUserBuildSettings.exportAsGoogleAndroidProject)
        {
          string location = EditorUtility.SaveFolderPanel("Export Google Android Project", buildLocation, "");
          if (location == string.Empty)
            return false;
          EditorUserBuildSettings.SetBuildLocation(target, location);
          return true;
        }
        string extensionForBuildTarget = PostprocessBuildPlayer.GetExtensionForBuildTarget(targetGroup, target, options);
        string title = "Build " + BuildPlatforms.instance.GetBuildTargetDisplayName(targetGroup, target);
        string str = EditorUtility.SaveBuildPanel(target, title, directory, defaultName, extensionForBuildTarget, out updateExistingBuild);
        if (str == string.Empty)
          return false;
        if (extensionForBuildTarget != string.Empty && FileUtil.GetPathExtension(str).ToLower() != extensionForBuildTarget)
          str = str + (object) '.' + extensionForBuildTarget;
        if (FileUtil.GetLastPathNameComponent(str) == string.Empty)
          return false;
        string path = !(extensionForBuildTarget != string.Empty) ? str : FileUtil.DeleteLastPathNameComponent(str);
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        if (target == BuildTarget.iOS && Application.platform != RuntimePlatform.OSXEditor && (!BuildPlayerWindow.DefaultBuildMethods.FolderIsEmpty(str) && !BuildPlayerWindow.DefaultBuildMethods.UserWantsToDeleteFiles(str)))
          return false;
        EditorUserBuildSettings.SetBuildLocation(target, str);
        return true;
      }

      private static bool FolderIsEmpty(string path)
      {
        if (!Directory.Exists(path))
          return true;
        return Directory.GetDirectories(path).Length == 0 && Directory.GetFiles(path).Length == 0;
      }

      private static bool UserWantsToDeleteFiles(string path)
      {
        return EditorUtility.DisplayDialog("Deleting existing files", "WARNING: all files and folders located in target folder: '" + path + "' will be deleted by build process.", "OK", "Cancel");
      }

      private static bool IsMetroPlayer(BuildTarget target)
      {
        return target == BuildTarget.WSAPlayer;
      }
    }

    private enum PackmanOperationType : uint
    {
      None,
      List,
      Add,
      Remove,
      Search,
      Outdated,
    }

    private class PublishStyles
    {
      public GUIContent xiaomiIcon = EditorGUIUtility.IconContent("BuildSettings.Xiaomi");
      public GUIContent learnAboutXiaomiInstallation = EditorGUIUtility.TextContent("Installation and Setup");
      public GUIContent publishTitle = EditorGUIUtility.TextContent("SDKs for App Stores|Integrations with 3rd party app stores");
      public const int kIconSize = 32;
      public const int kRowHeight = 36;
    }
  }
}
