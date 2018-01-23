// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.ModuleManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.DataContract;
using UnityEditor.Build;
using UnityEditor.DeploymentTargets;
using UnityEditor.Hardware;
using UnityEditor.Utils;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Modules
{
  internal static class ModuleManager
  {
    [NonSerialized]
    private static List<IPlatformSupportModule> s_PlatformModules;
    [NonSerialized]
    private static bool s_PlatformModulesInitialized;
    [NonSerialized]
    private static List<IEditorModule> s_EditorModules;
    [NonSerialized]
    private static IPackageManagerModule s_PackageManager;
    [NonSerialized]
    private static IPlatformSupportModule s_ActivePlatformModule;

    internal static bool EnableLogging
    {
      get
      {
        return (bool) (UnityEngine.Debug.GetDiagnosticSwitch("ModuleManagerLogging") ?? (object) false);
      }
    }

    internal static IPackageManagerModule packageManager
    {
      get
      {
        ModuleManager.InitializeModuleManager();
        return ModuleManager.s_PackageManager;
      }
    }

    internal static IEnumerable<IPlatformSupportModule> platformSupportModules
    {
      get
      {
        ModuleManager.InitializeModuleManager();
        if (ModuleManager.s_PlatformModules == null)
          ModuleManager.RegisterPlatformSupportModules();
        return (IEnumerable<IPlatformSupportModule>) ModuleManager.s_PlatformModules;
      }
    }

    internal static IEnumerable<IPlatformSupportModule> platformSupportModulesDontRegister
    {
      get
      {
        if (ModuleManager.s_PlatformModules == null)
          return (IEnumerable<IPlatformSupportModule>) new List<IPlatformSupportModule>();
        return (IEnumerable<IPlatformSupportModule>) ModuleManager.s_PlatformModules;
      }
    }

    private static List<IEditorModule> editorModules
    {
      get
      {
        if (ModuleManager.s_EditorModules == null)
          return new List<IEditorModule>();
        return ModuleManager.s_EditorModules;
      }
    }

    private static void OnActiveBuildTargetChanged(BuildTarget oldTarget, BuildTarget newTarget)
    {
      ModuleManager.ChangeActivePlatformModuleTo(ModuleManager.GetTargetStringFromBuildTarget(newTarget));
    }

    private static void DeactivateActivePlatformModule()
    {
      if (ModuleManager.s_ActivePlatformModule == null)
        return;
      ModuleManager.s_ActivePlatformModule.OnDeactivate();
      ModuleManager.s_ActivePlatformModule = (IPlatformSupportModule) null;
    }

    private static void ChangeActivePlatformModuleTo(string target)
    {
      ModuleManager.DeactivateActivePlatformModule();
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
        {
          ModuleManager.s_ActivePlatformModule = platformSupportModule;
          platformSupportModule.OnActivate();
          break;
        }
      }
    }

    internal static bool IsRegisteredModule(string file)
    {
      return ModuleManager.s_PackageManager != null && ModuleManager.s_PackageManager.GetType().Assembly.Location.NormalizePath() == file.NormalizePath();
    }

    [RequiredByNativeCode]
    internal static bool IsPlatformSupportLoaded(string target)
    {
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return true;
      }
      return false;
    }

    [RequiredByNativeCode]
    internal static void RegisterAdditionalUnityExtensions()
    {
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
        platformSupportModule.RegisterAdditionalUnityExtensions();
    }

    [RequiredByNativeCode]
    internal static void InitializeModuleManager()
    {
      if (ModuleManager.s_PackageManager != null)
        return;
      ModuleManager.RegisterPackageManager();
      if (ModuleManager.s_PackageManager != null)
        ModuleManager.LoadUnityExtensions();
      else
        UnityEngine.Debug.LogError((object) "Failed to load package manager");
    }

    private static string CombinePaths(params string[] paths)
    {
      if (paths == null)
        throw new ArgumentNullException(nameof (paths));
      if (paths.Length == 1)
        return paths[0];
      StringBuilder stringBuilder = new StringBuilder(paths[0]);
      for (int index = 1; index < paths.Length; ++index)
        stringBuilder.AppendFormat("{0}{1}", (object) "/", (object) paths[index]);
      return stringBuilder.ToString();
    }

    private static void LoadUnityExtensions()
    {
      foreach (Unity.DataContract.PackageInfo unityExtension in ModuleManager.s_PackageManager.unityExtensions)
      {
        if (ModuleManager.EnableLogging)
          Console.WriteLine("Setting {0} v{1} for Unity v{2} to {3}", new object[4]
          {
            (object) unityExtension.name,
            (object) unityExtension.version,
            (object) unityExtension.unityVersion,
            (object) unityExtension.basePath
          });
        foreach (KeyValuePair<string, PackageFileData> keyValuePair in unityExtension.files.Where<KeyValuePair<string, PackageFileData>>((Func<KeyValuePair<string, PackageFileData>, bool>) (f => f.Value.type == PackageFileType.Dll)))
        {
          string str = Path.Combine(unityExtension.basePath, keyValuePair.Key).NormalizePath();
          if (!File.Exists(str))
          {
            UnityEngine.Debug.LogWarningFormat("Missing assembly \t{0} for {1}. Extension support may be incomplete.", new object[2]
            {
              (object) keyValuePair.Key,
              (object) unityExtension.name
            });
          }
          else
          {
            bool flag = !string.IsNullOrEmpty(keyValuePair.Value.guid);
            if (ModuleManager.EnableLogging)
              Console.WriteLine("  {0} ({1}) GUID: {2}", (object) keyValuePair.Key, !flag ? (object) "Custom" : (object) "Extension", (object) keyValuePair.Value.guid);
            if (flag)
              InternalEditorUtility.RegisterExtensionDll(str.Replace('\\', '/'), keyValuePair.Value.guid);
            else
              InternalEditorUtility.RegisterPrecompiledAssembly(Path.GetFileName(str), str);
          }
        }
        ModuleManager.s_PackageManager.LoadPackage(unityExtension);
      }
    }

    [RequiredByNativeCode]
    internal static void InitializePlatformSupportModules()
    {
      if (ModuleManager.s_PlatformModulesInitialized)
      {
        Console.WriteLine("Platform modules already initialized, skipping");
      }
      else
      {
        ModuleManager.InitializeModuleManager();
        ModuleManager.RegisterPlatformSupportModules();
        foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
        {
          foreach (string nativeLibrary in platformSupportModule.NativeLibraries)
            EditorUtility.LoadPlatformSupportNativeLibrary(nativeLibrary);
          foreach (string referencesForUserScript in platformSupportModule.AssemblyReferencesForUserScripts)
            InternalEditorUtility.RegisterPrecompiledAssembly(Path.GetFileName(referencesForUserScript), referencesForUserScript);
          EditorUtility.LoadPlatformSupportModuleNativeDllInternal(platformSupportModule.TargetName);
          platformSupportModule.OnLoad();
        }
        ModuleManager.OnActiveBuildTargetChanged(BuildTarget.NoTarget, EditorUserBuildSettings.activeBuildTarget);
        ModuleManager.s_PlatformModulesInitialized = true;
      }
    }

    [RequiredByNativeCode]
    internal static void ShutdownPlatformSupportModules()
    {
      ModuleManager.DeactivateActivePlatformModule();
      if (ModuleManager.s_PlatformModules == null)
        return;
      foreach (IPlatformSupportModule platformModule in ModuleManager.s_PlatformModules)
        platformModule.OnUnload();
    }

    [RequiredByNativeCode(true)]
    internal static void ShutdownModuleManager()
    {
      if (ModuleManager.s_PackageManager != null)
        ModuleManager.s_PackageManager.Shutdown(true);
      ModuleManager.s_PackageManager = (IPackageManagerModule) null;
      ModuleManager.s_PlatformModules = (List<IPlatformSupportModule>) null;
      ModuleManager.s_EditorModules = (List<IEditorModule>) null;
    }

    private static void RegisterPackageManager()
    {
      ModuleManager.s_EditorModules = new List<IEditorModule>();
      try
      {
        Assembly assembly = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => null != a.GetType("Unity.PackageManager.PackageManager")));
        if (assembly != null)
        {
          if (ModuleManager.InitializePackageManager(assembly, (Unity.DataContract.PackageInfo) null))
            return;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error enumerating assemblies looking for package manager. {0}", (object) ex);
      }
      System.Type type = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name == "Unity.Locator")).Select<Assembly, System.Type>((Func<Assembly, System.Type>) (a => a.GetType("Unity.PackageManager.Locator"))).FirstOrDefault<System.Type>();
      try
      {
        string unityPath = FileUtil.CombinePaths(Directory.GetParent(EditorApplication.applicationPath).ToString(), "PlaybackEngines");
        type.InvokeMember("Scan", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, (Binder) null, (object) null, new object[2]
        {
          (object) new string[2]
          {
            FileUtil.NiceWinPath(EditorApplication.applicationContentsPath),
            FileUtil.NiceWinPath(unityPath)
          },
          (object) Application.unityVersion
        });
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error scanning for packages. {0}", (object) ex);
        return;
      }
      Unity.DataContract.PackageInfo package;
      try
      {
        package = type.InvokeMember("GetPackageManager", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, (Binder) null, (object) null, (object[]) new string[1]
        {
          Application.unityVersion
        }) as Unity.DataContract.PackageInfo;
        if (package == (Unity.DataContract.PackageInfo) null)
        {
          Console.WriteLine("No package manager found!");
          return;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error scanning for packages. {0}", (object) ex);
        return;
      }
      try
      {
        ModuleManager.InitializePackageManager(package);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error initializing package manager. {0}", (object) ex);
      }
      if (ModuleManager.s_PackageManager == null)
        return;
      ModuleManager.s_PackageManager.CheckForUpdates();
    }

    private static bool InitializePackageManager(Unity.DataContract.PackageInfo package)
    {
      string str = package.files.Where<KeyValuePair<string, PackageFileData>>((Func<KeyValuePair<string, PackageFileData>, bool>) (x => x.Value.type == PackageFileType.Dll)).Select<KeyValuePair<string, PackageFileData>, string>((Func<KeyValuePair<string, PackageFileData>, string>) (x => x.Key)).FirstOrDefault<string>();
      if (str == null || !File.Exists(Path.Combine(package.basePath, str)))
        return false;
      InternalEditorUtility.SetPlatformPath(package.basePath);
      return ModuleManager.InitializePackageManager(InternalEditorUtility.LoadAssemblyWrapper(Path.GetFileName(str), Path.Combine(package.basePath, str)), package);
    }

    private static bool InitializePackageManager(Assembly assembly, Unity.DataContract.PackageInfo package)
    {
      ModuleManager.s_PackageManager = AssemblyHelper.FindImplementors<IPackageManagerModule>(assembly).FirstOrDefault<IPackageManagerModule>();
      if (ModuleManager.s_PackageManager == null)
        return false;
      string location = assembly.Location;
      if (package != (Unity.DataContract.PackageInfo) null)
        InternalEditorUtility.RegisterPrecompiledAssembly(Path.GetFileName(location), location);
      else
        package = new Unity.DataContract.PackageInfo()
        {
          basePath = Path.GetDirectoryName(location)
        };
      ModuleManager.s_PackageManager.moduleInfo = package;
      ModuleManager.s_PackageManager.editorInstallPath = EditorApplication.applicationContentsPath;
      ModuleManager.s_PackageManager.unityVersion = (string) new PackageVersion(Application.unityVersion);
      ModuleManager.s_PackageManager.Initialize();
      foreach (Unity.DataContract.PackageInfo playbackEngine in ModuleManager.s_PackageManager.playbackEngines)
      {
        BuildTargetGroup buildTargetGroup;
        BuildTarget target;
        if (ModuleManager.TryParseBuildTarget(playbackEngine.name, out buildTargetGroup, out target))
        {
          if (ModuleManager.EnableLogging)
            Console.WriteLine("Setting {4}:{0} v{1} for Unity v{2} to {3}", (object) target, (object) playbackEngine.version, (object) playbackEngine.unityVersion, (object) playbackEngine.basePath, (object) buildTargetGroup);
          foreach (KeyValuePair<string, PackageFileData> keyValuePair in playbackEngine.files.Where<KeyValuePair<string, PackageFileData>>((Func<KeyValuePair<string, PackageFileData>, bool>) (f => f.Value.type == PackageFileType.Dll)))
          {
            if (!File.Exists(Path.Combine(playbackEngine.basePath, keyValuePair.Key).NormalizePath()))
              UnityEngine.Debug.LogWarningFormat("Missing assembly \t{0} for {1}. Player support may be incomplete.", new object[2]
              {
                (object) playbackEngine.basePath,
                (object) playbackEngine.name
              });
            else
              InternalEditorUtility.RegisterPrecompiledAssembly(Path.GetFileName(location), location);
          }
          BuildPipeline.SetPlaybackEngineDirectory(target, BuildOptions.None, playbackEngine.basePath);
          InternalEditorUtility.SetPlatformPath(playbackEngine.basePath);
          ModuleManager.s_PackageManager.LoadPackage(playbackEngine);
        }
      }
      return true;
    }

    private static bool TryParseBuildTarget(string targetString, out BuildTargetGroup buildTargetGroup, out BuildTarget target)
    {
      buildTargetGroup = BuildTargetGroup.Standalone;
      target = BuildTarget.StandaloneWindows;
      try
      {
        if (targetString == BuildTargetGroup.Facebook.ToString())
        {
          buildTargetGroup = BuildTargetGroup.Facebook;
          target = BuildTarget.StandaloneWindows;
        }
        else
        {
          target = (BuildTarget) Enum.Parse(typeof (BuildTarget), targetString);
          buildTargetGroup = BuildPipeline.GetBuildTargetGroup(target);
        }
        return true;
      }
      catch
      {
        UnityEngine.Debug.LogWarning((object) string.Format("Couldn't find build target for {0}", (object) targetString));
      }
      return false;
    }

    private static void RegisterPlatformSupportModules()
    {
      if (ModuleManager.s_PlatformModules != null)
      {
        Console.WriteLine("Modules already registered, not loading");
      }
      else
      {
        Console.WriteLine("Registering platform support modules:");
        Stopwatch stopwatch = Stopwatch.StartNew();
        // ISSUE: reference to a compiler-generated field
        if (ModuleManager.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ModuleManager.\u003C\u003Ef__mg\u0024cache0 = new Func<Assembly, IEnumerable<IPlatformSupportModule>>(ModuleManager.RegisterPlatformSupportModulesFromAssembly);
        }
        // ISSUE: reference to a compiler-generated field
        ModuleManager.s_PlatformModules = ModuleManager.RegisterModulesFromLoadedAssemblies<IPlatformSupportModule>(ModuleManager.\u003C\u003Ef__mg\u0024cache0).ToList<IPlatformSupportModule>();
        stopwatch.Stop();
        Console.WriteLine("Registered platform support modules in: " + (object) stopwatch.Elapsed.TotalSeconds + "s.");
      }
    }

    private static IEnumerable<T> RegisterModulesFromLoadedAssemblies<T>(Func<Assembly, IEnumerable<T>> processAssembly)
    {
      if (processAssembly == null)
        throw new ArgumentNullException(nameof (processAssembly));
      return (IEnumerable<T>) ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Aggregate<Assembly, List<T>>(new List<T>(), (Func<List<T>, Assembly, List<T>>) ((list, assembly) =>
      {
        try
        {
          IEnumerable<T> objs = processAssembly(assembly);
          if (objs != null)
          {
            if (objs.Any<T>())
              list.AddRange(objs);
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("Error while registering modules from " + assembly.FullName + ": " + ex.Message);
        }
        return list;
      }));
    }

    internal static IEnumerable<IPlatformSupportModule> RegisterPlatformSupportModulesFromAssembly(Assembly assembly)
    {
      return AssemblyHelper.FindImplementors<IPlatformSupportModule>(assembly);
    }

    private static IEnumerable<IEditorModule> RegisterEditorModulesFromAssembly(Assembly assembly)
    {
      return AssemblyHelper.FindImplementors<IEditorModule>(assembly);
    }

    internal static List<string> GetJamTargets()
    {
      List<string> stringList = new List<string>();
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
        stringList.Add(platformSupportModule.JamTarget);
      return stringList;
    }

    internal static IPlatformSupportModule FindPlatformSupportModule(string moduleName)
    {
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == moduleName)
          return platformSupportModule;
      }
      return (IPlatformSupportModule) null;
    }

    internal static IDevice GetDevice(string deviceId)
    {
      DevDevice device;
      if (!DevDeviceList.FindDevice(deviceId, out device))
        throw new ApplicationException("Couldn't create device API for device: " + deviceId);
      IPlatformSupportModule platformSupportModule = ModuleManager.FindPlatformSupportModule(device.module);
      if (platformSupportModule != null)
        return platformSupportModule.CreateDevice(deviceId);
      throw new ApplicationException("Couldn't find module for target: " + device.module);
    }

    internal static IUserAssembliesValidator GetUserAssembliesValidator(string target)
    {
      if (target == null)
        return (IUserAssembliesValidator) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreateUserAssembliesValidatorExtension();
      }
      return (IUserAssembliesValidator) null;
    }

    internal static IBuildPostprocessor GetBuildPostProcessor(string target)
    {
      if (target == null)
        return (IBuildPostprocessor) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreateBuildPostprocessor();
      }
      return (IBuildPostprocessor) null;
    }

    internal static IBuildPostprocessor GetBuildPostProcessor(BuildTargetGroup targetGroup, BuildTarget target)
    {
      return ModuleManager.GetBuildPostProcessor(ModuleManager.GetTargetStringFrom(targetGroup, target));
    }

    internal static IDeploymentTargetsExtension GetDeploymentTargetsExtension(string target)
    {
      if (target == null)
        return (IDeploymentTargetsExtension) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreateDeploymentTargetsExtension();
      }
      return (IDeploymentTargetsExtension) null;
    }

    internal static IDeploymentTargetsExtension GetDeploymentTargetsExtension(BuildTargetGroup targetGroup, BuildTarget target)
    {
      return ModuleManager.GetDeploymentTargetsExtension(ModuleManager.GetTargetStringFrom(targetGroup, target));
    }

    internal static IBuildAnalyzer GetBuildAnalyzer(string target)
    {
      if (target == null)
        return (IBuildAnalyzer) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreateBuildAnalyzer();
      }
      return (IBuildAnalyzer) null;
    }

    internal static IBuildAnalyzer GetBuildAnalyzer(BuildTarget target)
    {
      return ModuleManager.GetBuildAnalyzer(ModuleManager.GetTargetStringFromBuildTarget(target));
    }

    internal static ISettingEditorExtension GetEditorSettingsExtension(string target)
    {
      if (string.IsNullOrEmpty(target))
        return (ISettingEditorExtension) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreateSettingsEditorExtension();
      }
      return (ISettingEditorExtension) null;
    }

    internal static ITextureImportSettingsExtension GetTextureImportSettingsExtension(BuildTarget target)
    {
      return ModuleManager.GetTextureImportSettingsExtension(ModuleManager.GetTargetStringFromBuildTarget(target));
    }

    internal static ITextureImportSettingsExtension GetTextureImportSettingsExtension(string targetName)
    {
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == targetName)
          return platformSupportModule.CreateTextureImportSettingsExtension();
      }
      return (ITextureImportSettingsExtension) new DefaultTextureImportSettingsExtension();
    }

    internal static List<IPreferenceWindowExtension> GetPreferenceWindowExtensions()
    {
      List<IPreferenceWindowExtension> preferenceWindowExtensionList = new List<IPreferenceWindowExtension>();
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        IPreferenceWindowExtension preferenceWindowExtension = platformSupportModule.CreatePreferenceWindowExtension();
        if (preferenceWindowExtension != null)
          preferenceWindowExtensionList.Add(preferenceWindowExtension);
      }
      return preferenceWindowExtensionList;
    }

    internal static IBuildWindowExtension GetBuildWindowExtension(string target)
    {
      if (string.IsNullOrEmpty(target))
        return (IBuildWindowExtension) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreateBuildWindowExtension();
      }
      return (IBuildWindowExtension) null;
    }

    internal static ICompilationExtension GetCompilationExtension(string target)
    {
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreateCompilationExtension();
      }
      return (ICompilationExtension) new DefaultCompilationExtension();
    }

    private static IScriptingImplementations GetScriptingImplementations(string target)
    {
      if (string.IsNullOrEmpty(target))
        return (IScriptingImplementations) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreateScriptingImplementations();
      }
      return (IScriptingImplementations) null;
    }

    internal static IScriptingImplementations GetScriptingImplementations(BuildTargetGroup target)
    {
      if (target == BuildTargetGroup.Standalone)
        return (IScriptingImplementations) new DesktopStandalonePostProcessor.ScriptingImplementations();
      return ModuleManager.GetScriptingImplementations(ModuleManager.GetTargetStringFromBuildTargetGroup(target));
    }

    internal static IPluginImporterExtension GetPluginImporterExtension(string target)
    {
      if (target == null)
        return (IPluginImporterExtension) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreatePluginImporterExtension();
      }
      return (IPluginImporterExtension) null;
    }

    internal static IPluginImporterExtension GetPluginImporterExtension(BuildTarget target)
    {
      return ModuleManager.GetPluginImporterExtension(ModuleManager.GetTargetStringFromBuildTarget(target));
    }

    internal static IPluginImporterExtension GetPluginImporterExtension(BuildTargetGroup target)
    {
      return ModuleManager.GetPluginImporterExtension(ModuleManager.GetTargetStringFromBuildTargetGroup(target));
    }

    internal static string GetTargetStringFromBuildTarget(BuildTarget target)
    {
      switch (target)
      {
        case BuildTarget.StandaloneLinux:
        case BuildTarget.StandaloneLinux64:
        case BuildTarget.StandaloneLinuxUniversal:
          return "LinuxStandalone";
        case BuildTarget.StandaloneWindows64:
label_17:
          return "WindowsStandalone";
        case BuildTarget.WebGL:
          return "WebGL";
        case BuildTarget.WSAPlayer:
          return "Metro";
        case BuildTarget.StandaloneOSXIntel64:
label_18:
          return "OSXStandalone";
        case BuildTarget.Tizen:
          return "Tizen";
        case BuildTarget.PSP2:
          return "PSP2";
        case BuildTarget.PS4:
          return "PS4";
        case BuildTarget.PSM:
          return "PSM";
        case BuildTarget.XboxOne:
          return "XboxOne";
        case BuildTarget.N3DS:
          return "N3DS";
        case BuildTarget.WiiU:
          return "WiiU";
        case BuildTarget.tvOS:
          return "tvOS";
        case BuildTarget.Switch:
          return "Switch";
        default:
          switch (target - 2)
          {
            case ~BuildTarget.iPhone:
            case BuildTarget.StandaloneOSX:
              goto label_18;
            case (BuildTarget) 3:
              goto label_17;
            case BuildTarget.WebPlayerStreamed:
              return "iOS";
            default:
              if (target == BuildTarget.Android)
                return "Android";
              return (string) null;
          }
      }
    }

    internal static string GetTargetStringFromBuildTargetGroup(BuildTargetGroup target)
    {
      switch (target)
      {
        case BuildTargetGroup.WebGL:
          return "WebGL";
        case BuildTargetGroup.WSA:
          return "Metro";
        case BuildTargetGroup.Tizen:
          return "Tizen";
        case BuildTargetGroup.PSP2:
          return "PSP2";
        case BuildTargetGroup.PS4:
          return "PS4";
        case BuildTargetGroup.PSM:
          return "PSM";
        case BuildTargetGroup.XboxOne:
          return "XboxOne";
        case BuildTargetGroup.N3DS:
          return "N3DS";
        case BuildTargetGroup.WiiU:
          return "WiiU";
        case BuildTargetGroup.tvOS:
          return "tvOS";
        case BuildTargetGroup.Facebook:
          return "Facebook";
        case BuildTargetGroup.Switch:
          return "Switch";
        default:
          switch (target - 4)
          {
            case BuildTargetGroup.Unknown:
              return "iOS";
            case BuildTargetGroup.Standalone | BuildTargetGroup.WebPlayer:
              return "Android";
            default:
              return (string) null;
          }
      }
    }

    internal static string GetTargetStringFrom(BuildTargetGroup targetGroup, BuildTarget target)
    {
      if (targetGroup == BuildTargetGroup.Unknown)
        throw new ArgumentException("targetGroup must be valid");
      if (targetGroup == BuildTargetGroup.Facebook)
        return "Facebook";
      if (targetGroup == BuildTargetGroup.Standalone)
        return ModuleManager.GetTargetStringFromBuildTarget(target);
      return ModuleManager.GetTargetStringFromBuildTargetGroup(targetGroup);
    }

    internal static bool IsPlatformSupported(BuildTarget target)
    {
      return ModuleManager.GetTargetStringFromBuildTarget(target) != null;
    }

    internal static bool HaveLicenseForBuildTarget(string targetString)
    {
      BuildTargetGroup buildTargetGroup;
      BuildTarget target;
      if (!ModuleManager.TryParseBuildTarget(targetString, out buildTargetGroup, out target))
        return false;
      return BuildPipeline.LicenseCheck(target);
    }

    internal static string GetExtensionVersion(string target)
    {
      if (string.IsNullOrEmpty(target))
        return (string) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.ExtensionVersion;
      }
      return (string) null;
    }

    internal static bool ShouldShowMultiDisplayOption()
    {
      return BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget) == BuildTargetGroup.Standalone || BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget) == BuildTargetGroup.WSA || ModuleManager.GetDisplayNames(EditorUserBuildSettings.activeBuildTarget.ToString()) != null;
    }

    internal static GUIContent[] GetDisplayNames(string target)
    {
      if (string.IsNullOrEmpty(target))
        return (GUIContent[]) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.GetDisplayNames();
      }
      return (GUIContent[]) null;
    }

    private class BuildTargetChangedHandler : IActiveBuildTargetChanged, IOrderedCallback
    {
      public int callbackOrder
      {
        get
        {
          return 0;
        }
      }

      public void OnActiveBuildTargetChanged(BuildTarget oldTarget, BuildTarget newTarget)
      {
        ModuleManager.OnActiveBuildTargetChanged(oldTarget, newTarget);
      }
    }
  }
}
