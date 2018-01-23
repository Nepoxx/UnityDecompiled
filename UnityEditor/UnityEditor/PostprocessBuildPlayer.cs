// Decompiled with JetBrains decompiler
// Type: UnityEditor.PostprocessBuildPlayer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor.BuildReporting;
using UnityEditor.DeploymentTargets;
using UnityEditor.Modules;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditor
{
  internal class PostprocessBuildPlayer
  {
    internal const string StreamingAssets = "Assets/StreamingAssets";

    internal static string GenerateBundleIdentifier(string companyName, string productName)
    {
      return "unity." + companyName + "." + productName;
    }

    internal static bool InstallPluginsByExtension(string pluginSourceFolder, string extension, string debugExtension, string destPluginFolder, bool copyDirectories)
    {
      bool flag = false;
      if (!Directory.Exists(pluginSourceFolder))
        return flag;
      foreach (string fileSystemEntry in Directory.GetFileSystemEntries(pluginSourceFolder))
      {
        string fileName = Path.GetFileName(fileSystemEntry);
        string extension1 = Path.GetExtension(fileSystemEntry);
        if (extension1.Equals(extension, StringComparison.OrdinalIgnoreCase) || fileName.Equals(extension, StringComparison.OrdinalIgnoreCase) || debugExtension != null && debugExtension.Length != 0 && (extension1.Equals(debugExtension, StringComparison.OrdinalIgnoreCase) || fileName.Equals(debugExtension, StringComparison.OrdinalIgnoreCase)))
        {
          if (!Directory.Exists(destPluginFolder))
            Directory.CreateDirectory(destPluginFolder);
          string str = Path.Combine(destPluginFolder, fileName);
          if (copyDirectories)
            FileUtil.CopyDirectoryRecursive(fileSystemEntry, str);
          else if (!Directory.Exists(fileSystemEntry))
            FileUtil.UnityFileCopy(fileSystemEntry, str);
          flag = true;
        }
      }
      return flag;
    }

    internal static void InstallStreamingAssets(string stagingAreaDataPath)
    {
      PostprocessBuildPlayer.InstallStreamingAssets(stagingAreaDataPath, (BuildReport) null);
    }

    internal static void InstallStreamingAssets(string stagingAreaDataPath, BuildReport report)
    {
      if (!Directory.Exists("Assets/StreamingAssets"))
        return;
      string str = Path.Combine(stagingAreaDataPath, "StreamingAssets");
      FileUtil.CopyDirectoryRecursiveForPostprocess("Assets/StreamingAssets", str, true);
      if ((UnityEngine.Object) report != (UnityEngine.Object) null)
        report.AddFilesRecursive(str, "Streaming Assets");
    }

    public static string PrepareForBuild(BuildOptions options, BuildTargetGroup targetGroup, BuildTarget target)
    {
      IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, target);
      if (buildPostProcessor == null)
        return (string) null;
      return buildPostProcessor.PrepareForBuild(options, target);
    }

    public static bool SupportsScriptsOnlyBuild(BuildTargetGroup targetGroup, BuildTarget target)
    {
      IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, target);
      if (buildPostProcessor != null)
        return buildPostProcessor.SupportsScriptsOnlyBuild();
      return false;
    }

    public static string GetExtensionForBuildTarget(BuildTargetGroup targetGroup, BuildTarget target, BuildOptions options)
    {
      IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, target);
      if (buildPostProcessor == null)
        return string.Empty;
      return buildPostProcessor.GetExtension(target, options);
    }

    public static bool SupportsInstallInBuildFolder(BuildTargetGroup targetGroup, BuildTarget target)
    {
      IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, target);
      if (buildPostProcessor != null)
        return buildPostProcessor.SupportsInstallInBuildFolder();
      switch (target)
      {
        case BuildTarget.PSP2:
        case BuildTarget.PSM:
          return true;
        default:
          if (target != BuildTarget.Android && target != BuildTarget.WSAPlayer)
            return false;
          goto case BuildTarget.PSP2;
      }
    }

    public static bool SupportsLz4Compression(BuildTargetGroup targetGroup, BuildTarget target)
    {
      IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, target);
      if (buildPostProcessor != null)
        return buildPostProcessor.SupportsLz4Compression();
      return false;
    }

    public static void Launch(BuildTargetGroup targetGroup, BuildTarget buildTarget, string path, string productName, BuildOptions options, BuildReport buildReport)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PostprocessBuildPlayer.\u003CLaunch\u003Ec__AnonStorey1 launchCAnonStorey1 = new PostprocessBuildPlayer.\u003CLaunch\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      launchCAnonStorey1.targetGroup = targetGroup;
      // ISSUE: reference to a compiler-generated field
      launchCAnonStorey1.buildReport = buildReport;
      try
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PostprocessBuildPlayer.\u003CLaunch\u003Ec__AnonStorey0 launchCAnonStorey0 = new PostprocessBuildPlayer.\u003CLaunch\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        launchCAnonStorey0.\u003C\u003Ef__ref\u00241 = launchCAnonStorey1;
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) launchCAnonStorey1.buildReport == (UnityEngine.Object) null)
          throw new NotSupportedException();
        ProgressHandler handler = new ProgressHandler("Deploying Player", (ProgressHandler.ProgressCallback) ((title, message, globalProgress) =>
        {
          if (EditorUtility.DisplayCancelableProgressBar(title, message, globalProgress))
            throw new OperationAbortedException();
        }), 0.1f, 1f);
        // ISSUE: reference to a compiler-generated field
        launchCAnonStorey0.taskManager = new ProgressTaskManager(handler);
        // ISSUE: reference to a compiler-generated field
        launchCAnonStorey0.validTargetIds = (List<DeploymentTargetId>) null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        launchCAnonStorey0.taskManager.AddTask(new Action(launchCAnonStorey0.\u003C\u003Em__0));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        launchCAnonStorey0.taskManager.AddTask(new Action(launchCAnonStorey0.\u003C\u003Em__1));
        // ISSUE: reference to a compiler-generated field
        launchCAnonStorey0.taskManager.Run();
      }
      catch (OperationFailedException ex)
      {
        UnityEngine.Debug.LogException((Exception) ex);
        EditorUtility.DisplayDialog(ex.title, ex.Message, "Ok");
      }
      catch (OperationAbortedException ex)
      {
        Console.WriteLine("Deployment aborted");
      }
      catch (PostprocessBuildPlayer.NoTargetsFoundException ex)
      {
        throw new UnityException(string.Format("Could not find any valid targets to launch on for {0}", (object) buildTarget));
      }
      catch (NotSupportedException ex)
      {
        // ISSUE: reference to a compiler-generated field
        IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(launchCAnonStorey1.targetGroup, buildTarget);
        if (buildPostProcessor == null)
          throw new UnityException(string.Format("Launching {0} build target via mono is not supported", (object) buildTarget));
        BuildLaunchPlayerArgs args;
        args.target = buildTarget;
        args.playerPackage = BuildPipeline.GetPlaybackEngineDirectory(buildTarget, options);
        args.installPath = path;
        args.productName = productName;
        args.options = options;
        // ISSUE: reference to a compiler-generated field
        args.report = launchCAnonStorey1.buildReport;
        buildPostProcessor.LaunchPlayer(args);
      }
    }

    public static void UpdateBootConfig(BuildTargetGroup targetGroup, BuildTarget target, BootConfigData config, BuildOptions options)
    {
      IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, target);
      if (buildPostProcessor == null)
        return;
      buildPostProcessor.UpdateBootConfig(target, config, options);
    }

    public static void Postprocess(BuildTargetGroup targetGroup, BuildTarget target, string installPath, string companyName, string productName, int width, int height, BuildOptions options, RuntimeClassRegistry usedClassRegistry, BuildReport report)
    {
      string str1 = "Temp/StagingArea";
      string str2 = "Temp/StagingArea/Data";
      string str3 = "Temp/StagingArea/Data/Managed";
      string playbackEngineDirectory = BuildPipeline.GetPlaybackEngineDirectory(target, options);
      bool flag = (options & BuildOptions.InstallInBuildFolder) != BuildOptions.None && PostprocessBuildPlayer.SupportsInstallInBuildFolder(targetGroup, target);
      if (installPath == string.Empty && !flag)
        throw new Exception(installPath + " must not be an empty string");
      IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, target);
      if (buildPostProcessor == null)
        throw new UnityException(string.Format("Build target '{0}' not supported", (object) target));
      BuildPostProcessArgs args;
      args.target = target;
      args.stagingAreaData = str2;
      args.stagingArea = str1;
      args.stagingAreaDataManaged = str3;
      args.playerPackage = playbackEngineDirectory;
      args.installPath = installPath;
      args.companyName = companyName;
      args.productName = productName;
      args.productGUID = PlayerSettings.productGUID;
      args.options = options;
      args.usedClassRegistry = usedClassRegistry;
      args.report = report;
      buildPostProcessor.PostProcess(args);
    }

    internal static string ExecuteSystemProcess(string command, string args, string workingdir)
    {
      Program program = new Program(new ProcessStartInfo() { FileName = command, Arguments = args, WorkingDirectory = workingdir, CreateNoWindow = true });
      program.Start();
      do
        ;
      while (!program.WaitForExit(100));
      string standardOutputAsString = program.GetStandardOutputAsString();
      program.Dispose();
      return standardOutputAsString;
    }

    public static string subDir32Bit
    {
      get
      {
        return "x86";
      }
    }

    public static string subDir64Bit
    {
      get
      {
        return "x86_64";
      }
    }

    private class NoTargetsFoundException : Exception
    {
      public NoTargetsFoundException()
      {
      }

      public NoTargetsFoundException(string message)
        : base(message)
      {
      }
    }
  }
}
