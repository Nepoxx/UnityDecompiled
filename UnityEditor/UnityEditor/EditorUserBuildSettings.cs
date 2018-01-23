// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorUserBuildSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>User build settings for the Editor</para>
  /// </summary>
  public sealed class EditorUserBuildSettings
  {
    /// <summary>
    ///   <para>Triggered in response to SwitchActiveBuildTarget.</para>
    /// </summary>
    [Obsolete("UnityEditor.activeBuildTargetChanged has been deprecated.Use UnityEditor.Build.IActiveBuildTargetChanged instead.")]
    public static Action activeBuildTargetChanged;

    internal static extern AppleBuildAndRunType appleBuildAndRunType { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The currently selected build target group.</para>
    /// </summary>
    public static extern BuildTargetGroup selectedBuildTargetGroup { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The currently selected target for a standalone build.</para>
    /// </summary>
    public static extern BuildTarget selectedStandaloneTarget { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern BuildTarget selectedFacebookTarget { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern string facebookAccessToken { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>PSM Build Subtarget.</para>
    /// </summary>
    public static extern PSMBuildSubtarget psmBuildSubtarget { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>PS Vita Build subtarget.</para>
    /// </summary>
    public static extern PSP2BuildSubtarget psp2BuildSubtarget { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>PS4 Build Subtarget.</para>
    /// </summary>
    public static extern PS4BuildSubtarget ps4BuildSubtarget { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Specifies which version of PS4 hardware to target.</para>
    /// </summary>
    public static extern PS4HardwareTarget ps4HardwareTarget { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Are null references actively checked?</para>
    /// </summary>
    public static extern bool explicitNullChecks { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Are divide by zero's actively checked?</para>
    /// </summary>
    public static extern bool explicitDivideByZeroChecks { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Build submission materials.</para>
    /// </summary>
    public static extern bool needSubmissionMaterials { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Build data compressed with PSArc.</para>
    /// </summary>
    public static extern bool compressWithPsArc { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Force installation of package, even if error.</para>
    /// </summary>
    public static extern bool forceInstallation { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Compress files in package.</para>
    /// </summary>
    public static extern bool compressFilesInPackage { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enables a Linux headless build.</para>
    /// </summary>
    public static extern bool enableHeadlessMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is build script only enabled.</para>
    /// </summary>
    public static extern bool buildScriptsOnly { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Xbox Build subtarget.</para>
    /// </summary>
    public static extern XboxBuildSubtarget xboxBuildSubtarget { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Wii U player debug level.</para>
    /// </summary>
    public static extern WiiUBuildDebugLevel wiiUBuildDebugLevel { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Built player postprocessing options.</para>
    /// </summary>
    public static extern WiiUBuildOutput wiiuBuildOutput { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable network API.</para>
    /// </summary>
    public static extern bool wiiUEnableNetAPI { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Boot mode of a devkit.</para>
    /// </summary>
    public static extern int wiiUBootMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>When building an Xbox One Streaming Install package (makepkg.exe) The layout generation code in Unity will assign each scene and associated assets to individual chunks. Unity will mark scene 0 as being part of the launch range, IE the set of chunks required to launch the game, you may include additional scenes in this launch range if you desire, this specifies a range of scenes (starting at 0) to be included in the launch set. </para>
    /// </summary>
    public static extern int streamingInstallLaunchRange { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The currently selected Xbox One Deploy Method.</para>
    /// </summary>
    public static extern XboxOneDeployMethod xboxOneDeployMethod { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Windows account username associated with PC share folder.</para>
    /// </summary>
    public static extern string xboxOneUsername { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///         <para>Network shared folder path e.g.
    /// MYCOMPUTER\SHAREDFOLDER\.</para>
    ///       </summary>
    public static extern string xboxOneNetworkSharePath { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static string xboxOneAdditionalDebugPorts { get; set; }

    /// <summary>
    ///   <para>Sets the XBox to reboot and redeploy when the deployment fails.</para>
    /// </summary>
    public static bool xboxOneRebootIfDeployFailsAndRetry { get; set; }

    /// <summary>
    ///   <para>Android platform options.</para>
    /// </summary>
    public static extern MobileTextureSubtarget androidBuildSubtarget { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Compression GetCompressionType(BuildTargetGroup targetGroup);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetCompressionType(BuildTargetGroup targetGroup, Compression type);

    /// <summary>
    ///   <para>ETC2 texture decompression fallback on Android devices that don't support ETC2.</para>
    /// </summary>
    public static extern AndroidETC2Fallback androidETC2Fallback { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set which build system to use for building the Android package.</para>
    /// </summary>
    public static extern AndroidBuildSystem androidBuildSystem { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern AndroidBuildType androidBuildType { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern AndroidMinification androidDebugMinification { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern AndroidMinification androidReleaseMinification { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern string androidDeviceSocketAddress { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Sets and gets target device type for the application to run on when building to Windows Store platform.</para>
    /// </summary>
    public static extern WSASubtarget wsaSubtarget { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("EditorUserBuildSettings.wsaSDK is obsolete and has no effect. It will be removed in a subsequent Unity release.")]
    public static extern WSASDK wsaSDK { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern WSAUWPBuildType wsaUWPBuildType { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Sets and gets target UWP SDK to build Windows Store application against.</para>
    /// </summary>
    public static extern string wsaUWPSDK { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Sets and gets Visual Studio version to build Windows Store application with.</para>
    /// </summary>
    public static extern string wsaUWPVisualStudioVersion { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern WSABuildAndRunDeployTarget wsaBuildAndRunDeployTarget { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Generate and reference C# projects from your main solution.</para>
    /// </summary>
    public static extern bool wsaGenerateReferenceProjects { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///         <para>Enables or Disables .NET Native for specific build configuration.
    /// More information - https:msdn.microsoft.comen-uslibrary/dn584397(v=vs.110).aspx.</para>
    ///       </summary>
    /// <param name="config">Build configuration.</param>
    /// <param name="enabled">Is enabled?</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetWSADotNetNative(WSABuildType config, bool enabled);

    /// <summary>
    ///         <para>Is .NET Native enabled for specific build configuration.
    /// More information - https:msdn.microsoft.comen-uslibrary/dn584397(v=vs.110).aspx.</para>
    ///       </summary>
    /// <param name="config">Build configuration.</param>
    /// <returns>
    ///   <para>True if .NET Native is enabled for the specific build configuration.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetWSADotNetNative(WSABuildType config);

    /// <summary>
    ///   <para>The texture compression type to be used when building.</para>
    /// </summary>
    public static extern MobileTextureSubtarget tizenBuildSubtarget { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Select the streaming option for a webplayer build.</para>
    /// </summary>
    [Obsolete("EditorUserBuildSettings.webPlayerStreamed is obsolete and has no effect. It will be removed in a subsequent Unity release.", true)]
    public static extern bool webPlayerStreamed { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Build the webplayer along with the UnityObject.js file (so it doesn't need to be downloaded).</para>
    /// </summary>
    [Obsolete("EditorUserBuildSettings.webPlayerOfflineDeployment is obsolete and has no effect. It will be removed in a subsequent Unity release.", true)]
    public static extern bool webPlayerOfflineDeployment { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The currently active build target.</para>
    /// </summary>
    public static extern BuildTarget activeBuildTarget { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern BuildTargetGroup activeBuildTargetGroup { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Select a new build target to be active.</para>
    /// </summary>
    /// <param name="target">Target build platform.</param>
    /// <param name="targetGroup">Build target group.</param>
    /// <returns>
    ///   <para>True if the build target was successfully switched, false otherwise (for example, if license checks fail, files are missing, or if the user has cancelled the operation via the UI).</para>
    /// </returns>
    [Obsolete("Please use SwitchActiveBuildTarget (BuildTargetGroup targetGroup, BuildTarget target)")]
    public static bool SwitchActiveBuildTarget(BuildTarget target)
    {
      return EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(target), target);
    }

    /// <summary>
    ///   <para>Select a new build target to be active.</para>
    /// </summary>
    /// <param name="target">Target build platform.</param>
    /// <param name="targetGroup">Build target group.</param>
    /// <returns>
    ///   <para>True if the build target was successfully switched, false otherwise (for example, if license checks fail, files are missing, or if the user has cancelled the operation via the UI).</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SwitchActiveBuildTarget(BuildTargetGroup targetGroup, BuildTarget target);

    /// <summary>
    ///   <para>Select a new build target to be active during the next Editor update.</para>
    /// </summary>
    /// <param name="targetGroup">Target build platform.</param>
    /// <param name="target">Build target group.</param>
    /// <returns>
    ///   <para>True if the build target was successfully switched, false otherwise (for example, if license checks fail, files are missing, or if the user has cancelled the operation via the UI).</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SwitchActiveBuildTargetAsync(BuildTargetGroup targetGroup, BuildTarget target);

    internal static void Internal_ActiveBuildTargetChanged()
    {
      if (EditorUserBuildSettings.activeBuildTargetChanged == null)
        return;
      EditorUserBuildSettings.activeBuildTargetChanged();
    }

    /// <summary>
    ///   <para>DEFINE directives for the compiler.</para>
    /// </summary>
    public static extern string[] activeScriptCompilationDefines { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get the current location for the build.</para>
    /// </summary>
    /// <param name="target"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetBuildLocation(BuildTarget target);

    /// <summary>
    ///   <para>Set a new location for the build.</para>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="location"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetBuildLocation(BuildTarget target, string location);

    /// <summary>
    ///   <para>Set platform specifc Editor setting.</para>
    /// </summary>
    /// <param name="platformName">The name of the platform.</param>
    /// <param name="name">The name of the setting.</param>
    /// <param name="value">Setting value.</param>
    public static void SetPlatformSettings(string platformName, string name, string value)
    {
      EditorUserBuildSettings.SetPlatformSettings(BuildPipeline.GetBuildTargetGroupName(BuildPipeline.GetBuildTargetByName(platformName)), platformName, name, value);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetPlatformSettings(string buildTargetGroup, string buildTarget, string name, string value);

    /// <summary>
    ///   <para>Returns value for platform specifc Editor setting.</para>
    /// </summary>
    /// <param name="platformName">The name of the platform.</param>
    /// <param name="name">The name of the setting.</param>
    public static string GetPlatformSettings(string platformName, string name)
    {
      return EditorUserBuildSettings.GetPlatformSettings(BuildPipeline.GetBuildTargetGroupName(BuildPipeline.GetBuildTargetByName(platformName)), platformName, name);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetPlatformSettings(string buildTargetGroup, string platformName, string name);

    /// <summary>
    ///   <para>Enables a development build.</para>
    /// </summary>
    public static extern bool development { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Use prebuilt JavaScript version of Unity engine.</para>
    /// </summary>
    public static extern bool webGLUsePreBuiltUnityEngine { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Start the player with a connection to the profiler.</para>
    /// </summary>
    public static extern bool connectProfiler { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable source-level debuggers to connect.</para>
    /// </summary>
    public static extern bool allowDebugging { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Export Android Project for use with Android Studio/Gradle.</para>
    /// </summary>
    public static extern bool exportAsGoogleAndroidProject { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Symlink runtime libraries with an iOS Xcode project.</para>
    /// </summary>
    public static extern bool symlinkLibraries { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool symlinkTrampoline { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Scheme with which the project will be run in Xcode.</para>
    /// </summary>
    public static extern iOSBuildType iOSBuildConfigType { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Create a .cia "download image" for deploying to test kits (3DS).</para>
    /// </summary>
    public static extern bool n3dsCreateCIAFile { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool switchCreateSolutionFile { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool switchCreateRomFile { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool switchNVNGraphicsDebugger { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool switchEnableDebugPad { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool switchRedirectWritesToHostMount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Place the built player in the build folder.</para>
    /// </summary>
    public static extern bool installInBuildFolder { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Force full optimizations for script complilation in Development builds.</para>
    /// </summary>
    [Obsolete("forceOptimizeScriptCompilation is obsolete - will always return false. Control script optimization using the 'IL2CPP optimization level' configuration in Player Settings / Other.")]
    public static extern bool forceOptimizeScriptCompilation { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
