// Decompiled with JetBrains decompiler
// Type: DesktopStandalonePostProcessor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.IO;
using UnityEditor;
using UnityEditor.Modules;
using UnityEditorInternal;
using UnityEngine;

internal abstract class DesktopStandalonePostProcessor : DefaultBuildPostprocessor
{
  protected BuildPostProcessArgs m_PostProcessArgs;

  protected DesktopStandalonePostProcessor()
  {
  }

  protected DesktopStandalonePostProcessor(BuildPostProcessArgs postProcessArgs)
  {
    this.m_PostProcessArgs = postProcessArgs;
  }

  public void PostProcess()
  {
    this.SetupStagingArea();
    this.CopyStagingAreaIntoDestination();
  }

  public override void UpdateBootConfig(BuildTarget target, BootConfigData config, BuildOptions options)
  {
    base.UpdateBootConfig(target, config, options);
    if (PlayerSettings.forceSingleInstance)
      config.AddKey("single-instance");
    if (EditorApplication.scriptingRuntimeVersion == ScriptingRuntimeVersion.Latest)
      config.Set("scripting-runtime-version", "latest");
    if (!IL2CPPUtils.UseIl2CppCodegenWithMonoBackend(BuildPipeline.GetBuildTargetGroup(target)))
      return;
    config.Set("mono-codegen", "il2cpp");
  }

  private void CopyNativePlugins()
  {
    string buildTargetName = BuildPipeline.GetBuildTargetName(this.m_PostProcessArgs.target);
    IPluginImporterExtension importerExtension = (IPluginImporterExtension) new DesktopPluginImporterExtension();
    string areaPluginsFolder = this.StagingAreaPluginsFolder;
    string path1 = Path.Combine(areaPluginsFolder, "x86");
    string path2 = Path.Combine(areaPluginsFolder, "x86_64");
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    foreach (PluginImporter importer in PluginImporter.GetImporters(this.m_PostProcessArgs.target))
    {
      BuildTarget target = this.m_PostProcessArgs.target;
      if (importer.isNativePlugin)
      {
        if (string.IsNullOrEmpty(importer.assetPath))
        {
          Debug.LogWarning((object) ("Got empty plugin importer path for " + this.m_PostProcessArgs.target.ToString()));
        }
        else
        {
          if (!flag1)
          {
            Directory.CreateDirectory(areaPluginsFolder);
            flag1 = true;
          }
          bool flag4 = Directory.Exists(importer.assetPath);
          switch (importer.GetPlatformData(target, "CPU"))
          {
            case "x86":
              if (target != BuildTarget.StandaloneWindows64 && target != BuildTarget.StandaloneLinux64)
              {
                if (!flag2)
                {
                  Directory.CreateDirectory(path1);
                  flag2 = true;
                  break;
                }
                break;
              }
              continue;
            case "x86_64":
              if (target == BuildTarget.StandaloneOSX || target == BuildTarget.StandaloneWindows64 || (target == BuildTarget.StandaloneLinux64 || target == BuildTarget.StandaloneLinuxUniversal))
              {
                if (!flag3)
                {
                  Directory.CreateDirectory(path2);
                  flag3 = true;
                  break;
                }
                break;
              }
              continue;
            case "None":
              continue;
          }
          string finalPluginPath = importerExtension.CalculateFinalPluginPath(buildTargetName, importer);
          if (!string.IsNullOrEmpty(finalPluginPath))
          {
            string str = Path.Combine(areaPluginsFolder, finalPluginPath);
            if (flag4)
              FileUtil.CopyDirectoryRecursive(importer.assetPath, str);
            else
              FileUtil.UnityFileCopy(importer.assetPath, str);
          }
        }
      }
    }
    foreach (PluginDesc extensionPlugin in PluginImporter.GetExtensionPlugins(this.m_PostProcessArgs.target))
    {
      if (!flag1)
      {
        Directory.CreateDirectory(areaPluginsFolder);
        flag1 = true;
      }
      string str = Path.Combine(areaPluginsFolder, Path.GetFileName(extensionPlugin.pluginPath));
      if (!Directory.Exists(str) && !File.Exists(str))
      {
        if (Directory.Exists(extensionPlugin.pluginPath))
          FileUtil.CopyDirectoryRecursive(extensionPlugin.pluginPath, str);
        else
          FileUtil.CopyFileIfExists(extensionPlugin.pluginPath, str, false);
      }
    }
  }

  protected virtual void SetupStagingArea()
  {
    Directory.CreateDirectory(this.DataFolder);
    this.CopyNativePlugins();
    if (this.m_PostProcessArgs.target == BuildTarget.StandaloneWindows || this.m_PostProcessArgs.target == BuildTarget.StandaloneWindows64)
      this.CreateApplicationData();
    PostprocessBuildPlayer.InstallStreamingAssets(this.DataFolder);
    if (this.UseIl2Cpp)
    {
      this.CopyVariationFolderIntoStagingArea();
      string str1 = this.StagingArea + "/Data";
      string str2 = this.DataFolder + "/Managed";
      string str3 = str2 + "/Resources";
      string str4 = str2 + "/Metadata";
      IL2CPPUtils.RunIl2Cpp(str1, this.GetPlatformProvider(this.m_PostProcessArgs.target), (Action<string>) (s => {}), this.m_PostProcessArgs.usedClassRegistry, false);
      FileUtil.CreateOrCleanDirectory(str3);
      IL2CPPUtils.CopyEmbeddedResourceFiles(str1, str3);
      FileUtil.CreateOrCleanDirectory(str4);
      IL2CPPUtils.CopyMetadataFiles(str1, str4);
      IL2CPPUtils.CopySymmapFile(str1 + "/Native/Data", str2);
      if (IL2CPPUtils.UseIl2CppCodegenWithMonoBackend(BuildPipeline.GetBuildTargetGroup(this.m_PostProcessArgs.target)))
        DesktopStandalonePostProcessor.StripAssembliesToLeaveOnlyMetadata(this.m_PostProcessArgs.target, str2);
    }
    if (this.InstallingIntoBuildsFolder)
    {
      this.CopyDataForBuildsFolder();
    }
    else
    {
      if (!this.UseIl2Cpp)
        this.CopyVariationFolderIntoStagingArea();
      this.RenameFilesInStagingArea();
      this.m_PostProcessArgs.report.AddFilesRecursive(this.StagingArea, "");
      this.m_PostProcessArgs.report.RelocateFiles(this.StagingArea, "");
    }
  }

  private static void StripAssembliesToLeaveOnlyMetadata(BuildTarget target, string stagingAreaDataManaged)
  {
    AssemblyReferenceChecker referenceChecker = new AssemblyReferenceChecker();
    referenceChecker.CollectReferences(stagingAreaDataManaged, true, 0.0f, false);
    EditorUtility.DisplayProgressBar("Removing bytecode from assemblies", "Stripping assemblies so that only metadata remains", 0.95f);
    MonoAssemblyStripping.MonoCilStrip(target, stagingAreaDataManaged, referenceChecker.GetAssemblyFileNames());
  }

  protected void CreateApplicationData()
  {
    File.WriteAllText(Path.Combine(this.DataFolder, "app.info"), string.Join("\n", new string[2]
    {
      this.m_PostProcessArgs.companyName,
      this.m_PostProcessArgs.productName
    }));
  }

  protected virtual bool CopyFilter(string path)
  {
    bool flag = true;
    if (Path.GetExtension(path) == ".mdb" && Path.GetFileName(path).StartsWith("UnityEngine."))
      flag = false;
    return flag & !path.Contains("UnityEngine.xml");
  }

  protected virtual void CopyVariationFolderIntoStagingArea()
  {
    FileUtil.CopyDirectoryFiltered(this.m_PostProcessArgs.playerPackage + "/Variations/" + this.GetVariationName(), this.StagingArea, true, (Func<string, bool>) (f => this.CopyFilter(f)), true);
  }

  protected void CopyStagingAreaIntoDestination()
  {
    if (this.InstallingIntoBuildsFolder)
    {
      string str = Unsupported.GetBaseUnityDeveloperFolder() + "/" + this.DestinationFolderForInstallingIntoBuildsFolder;
      if (!Directory.Exists(Path.GetDirectoryName(str)))
        throw new Exception("Installing in builds folder failed because the player has not been built (You most likely want to enable 'Development build').");
      FileUtil.CopyDirectoryFiltered(this.DataFolder, str, true, (Func<string, bool>) (f => true), true);
    }
    else
    {
      this.DeleteDestination();
      FileUtil.CopyDirectoryFiltered(this.StagingArea, this.DestinationFolder, true, (Func<string, bool>) (f => true), true);
    }
  }

  protected abstract string StagingAreaPluginsFolder { get; }

  protected abstract void DeleteDestination();

  protected void DeleteUnusedMono(string dataFolder)
  {
    bool flag = IL2CPPUtils.UseIl2CppCodegenWithMonoBackend(BuildTargetGroup.Standalone);
    if (flag || EditorApplication.scriptingRuntimeVersion == ScriptingRuntimeVersion.Latest)
      FileUtil.DeleteFileOrDirectory(Path.Combine(dataFolder, "Mono"));
    if (!flag && EditorApplication.scriptingRuntimeVersion != ScriptingRuntimeVersion.Legacy)
      return;
    FileUtil.DeleteFileOrDirectory(Path.Combine(dataFolder, "MonoBleedingEdge"));
  }

  protected abstract string DestinationFolderForInstallingIntoBuildsFolder { get; }

  protected abstract void CopyDataForBuildsFolder();

  protected bool InstallingIntoBuildsFolder
  {
    get
    {
      return (this.m_PostProcessArgs.options & BuildOptions.InstallInBuildFolder) != BuildOptions.None;
    }
  }

  protected bool UseIl2Cpp
  {
    get
    {
      return PlayerSettings.GetScriptingBackend(BuildTargetGroup.Standalone) == ScriptingImplementation.IL2CPP;
    }
  }

  protected string StagingArea
  {
    get
    {
      return this.m_PostProcessArgs.stagingArea;
    }
  }

  protected string InstallPath
  {
    get
    {
      return this.m_PostProcessArgs.installPath;
    }
  }

  protected string DataFolder
  {
    get
    {
      return this.StagingArea + "/Data";
    }
  }

  protected BuildTarget Target
  {
    get
    {
      return this.m_PostProcessArgs.target;
    }
  }

  protected virtual string DestinationFolder
  {
    get
    {
      return FileUtil.UnityGetDirectoryName(this.m_PostProcessArgs.installPath);
    }
  }

  protected bool Development
  {
    get
    {
      return (this.m_PostProcessArgs.options & BuildOptions.Development) != BuildOptions.None;
    }
  }

  protected virtual string GetVariationName()
  {
    return string.Format("{0}_{1}", (object) this.PlatformStringFor(this.m_PostProcessArgs.target), !this.Development ? (object) "nondevelopment" : (object) "development");
  }

  protected abstract string PlatformStringFor(BuildTarget target);

  protected abstract void RenameFilesInStagingArea();

  protected abstract IIl2CppPlatformProvider GetPlatformProvider(BuildTarget target);

  internal class ScriptingImplementations : IScriptingImplementations
  {
    public ScriptingImplementation[] Supported()
    {
      return new ScriptingImplementation[2]{ ScriptingImplementation.Mono2x, ScriptingImplementation.IL2CPP };
    }

    public ScriptingImplementation[] Enabled()
    {
      if (!Unsupported.IsDeveloperBuild())
        return new ScriptingImplementation[1];
      return new ScriptingImplementation[2]{ ScriptingImplementation.Mono2x, ScriptingImplementation.IL2CPP };
    }
  }
}
