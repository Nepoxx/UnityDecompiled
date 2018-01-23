// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.IL2CPPBuilder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Scripting;
using UnityEditor.Scripting.Compilers;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class IL2CPPBuilder
  {
    private readonly LinkXmlReader m_linkXmlReader = new LinkXmlReader();
    private readonly string m_TempFolder;
    private readonly string m_StagingAreaData;
    private readonly IIl2CppPlatformProvider m_PlatformProvider;
    private readonly Action<string> m_ModifyOutputBeforeCompile;
    private readonly RuntimeClassRegistry m_RuntimeClassRegistry;
    private readonly bool m_DebugBuild;
    private readonly bool m_BuildForMonoRuntime;

    public IL2CPPBuilder(string tempFolder, string stagingAreaData, IIl2CppPlatformProvider platformProvider, Action<string> modifyOutputBeforeCompile, RuntimeClassRegistry runtimeClassRegistry, bool debugBuild, bool buildForMonoRuntime)
    {
      this.m_TempFolder = tempFolder;
      this.m_StagingAreaData = stagingAreaData;
      this.m_PlatformProvider = platformProvider;
      this.m_ModifyOutputBeforeCompile = modifyOutputBeforeCompile;
      this.m_RuntimeClassRegistry = runtimeClassRegistry;
      this.m_DebugBuild = debugBuild;
      this.m_BuildForMonoRuntime = buildForMonoRuntime;
    }

    public void Run()
    {
      string directoryInStagingArea = this.GetCppOutputDirectoryInStagingArea();
      string fullPath = Path.GetFullPath(Path.Combine(this.m_StagingAreaData, "Managed"));
      foreach (string file in Directory.GetFiles(fullPath))
        new FileInfo(file).IsReadOnly = false;
      AssemblyStripper.StripAssemblies(this.m_StagingAreaData, this.m_PlatformProvider, this.m_RuntimeClassRegistry);
      FileUtil.CreateOrCleanDirectory(directoryInStagingArea);
      if (this.m_ModifyOutputBeforeCompile != null)
        this.m_ModifyOutputBeforeCompile(directoryInStagingArea);
      this.ConvertPlayerDlltoCpp((ICollection<string>) this.GetUserAssembliesToConvert(fullPath), directoryInStagingArea, fullPath);
      if (this.m_PlatformProvider.CreateNativeCompiler() == null || this.m_PlatformProvider.CreateIl2CppNativeCodeBuilder() != null)
        return;
      this.m_PlatformProvider.CreateNativeCompiler().CompileDynamicLibrary(this.OutputFileRelativePath(), NativeCompiler.AllSourceFilesIn(directoryInStagingArea), (IEnumerable<string>) new List<string>((IEnumerable<string>) this.m_PlatformProvider.includePaths)
      {
        directoryInStagingArea
      }, (IEnumerable<string>) this.m_PlatformProvider.libraryPaths, (IEnumerable<string>) new string[0]);
    }

    public void RunCompileAndLink()
    {
      Il2CppNativeCodeBuilder nativeCodeBuilder = this.m_PlatformProvider.CreateIl2CppNativeCodeBuilder();
      if (nativeCodeBuilder == null)
        return;
      Il2CppNativeCodeBuilderUtils.ClearAndPrepareCacheDirectory(nativeCodeBuilder);
      List<string> list = Il2CppNativeCodeBuilderUtils.AddBuilderArguments(nativeCodeBuilder, this.OutputFileRelativePath(), (IEnumerable<string>) this.m_PlatformProvider.includePaths, this.m_DebugBuild).ToList<string>();
      list.Add(string.Format("--map-file-parser=\"{0}\"", (object) IL2CPPBuilder.GetMapFileParserPath()));
      list.Add(string.Format("--generatedcppdir=\"{0}\"", (object) Path.GetFullPath(this.GetCppOutputDirectoryInStagingArea())));
      if (PlayerSettings.GetApiCompatibilityLevel(BuildPipeline.GetBuildTargetGroup(this.m_PlatformProvider.target)) == ApiCompatibilityLevel.NET_4_6)
        list.Add("--dotnetprofile=\"net45\"");
      Action<ProcessStartInfo> setupStartInfo = new Action<ProcessStartInfo>(nativeCodeBuilder.SetupStartInfo);
      string fullPath = Path.GetFullPath(Path.Combine(this.m_StagingAreaData, "Managed"));
      this.RunIl2CppWithArguments(list, setupStartInfo, fullPath);
    }

    private string OutputFileRelativePath()
    {
      string str = Path.Combine(this.m_StagingAreaData, "Native");
      Directory.CreateDirectory(str);
      return Path.Combine(str, this.m_PlatformProvider.nativeLibraryFileName);
    }

    internal List<string> GetUserAssembliesToConvert(string managedDir)
    {
      HashSet<string> userAssemblies = this.GetUserAssemblies(managedDir);
      userAssemblies.Add(((IEnumerable<string>) Directory.GetFiles(managedDir, "UnityEngine.dll", SearchOption.TopDirectoryOnly)).Single<string>());
      userAssemblies.UnionWith(this.FilterUserAssemblies((IEnumerable<string>) Directory.GetFiles(managedDir, "*.dll", SearchOption.TopDirectoryOnly), new Predicate<string>(this.m_linkXmlReader.IsDLLUsed), managedDir));
      return userAssemblies.ToList<string>();
    }

    private HashSet<string> GetUserAssemblies(string managedDir)
    {
      HashSet<string> stringSet = new HashSet<string>();
      stringSet.UnionWith(this.FilterUserAssemblies((IEnumerable<string>) this.m_RuntimeClassRegistry.GetUserAssemblies(), new Predicate<string>(this.m_RuntimeClassRegistry.IsDLLUsed), managedDir));
      stringSet.UnionWith(this.FilterUserAssemblies((IEnumerable<string>) Directory.GetFiles(managedDir, "I18N*.dll", SearchOption.TopDirectoryOnly), (Predicate<string>) (assembly => true), managedDir));
      return stringSet;
    }

    private IEnumerable<string> FilterUserAssemblies(IEnumerable<string> assemblies, Predicate<string> isUsed, string managedDir)
    {
      return assemblies.Where<string>((Func<string, bool>) (assembly => isUsed(assembly))).Select<string, string>((Func<string, string>) (usedAssembly => Path.Combine(managedDir, usedAssembly)));
    }

    public string GetCppOutputDirectoryInStagingArea()
    {
      return IL2CPPBuilder.GetCppOutputPath(this.m_TempFolder);
    }

    public static string GetCppOutputPath(string tempFolder)
    {
      return Path.Combine(tempFolder, "il2cppOutput");
    }

    public static string GetMapFileParserPath()
    {
      return Path.GetFullPath(Path.Combine(EditorApplication.applicationContentsPath, Application.platform != RuntimePlatform.WindowsEditor ? "Tools/MapFileParser/MapFileParser" : "Tools\\MapFileParser\\MapFileParser.exe"));
    }

    private void ConvertPlayerDlltoCpp(ICollection<string> userAssemblies, string outputDirectory, string workingDirectory)
    {
      if (userAssemblies.Count == 0)
        return;
      List<string> arguments = new List<string>();
      arguments.Add("--convert-to-cpp");
      if (this.m_PlatformProvider.emitNullChecks)
        arguments.Add("--emit-null-checks");
      if (this.m_PlatformProvider.enableStackTraces)
        arguments.Add("--enable-stacktrace");
      if (this.m_PlatformProvider.enableArrayBoundsCheck)
        arguments.Add("--enable-array-bounds-check");
      if (this.m_PlatformProvider.enableDivideByZeroCheck)
        arguments.Add("--enable-divide-by-zero-check");
      if (this.m_PlatformProvider.developmentMode)
        arguments.Add("--development-mode");
      if (this.m_BuildForMonoRuntime)
        arguments.Add("--mono-runtime");
      if (PlayerSettings.GetApiCompatibilityLevel(BuildPipeline.GetBuildTargetGroup(this.m_PlatformProvider.target)) == ApiCompatibilityLevel.NET_4_6)
        arguments.Add("--dotnetprofile=\"net45\"");
      Il2CppNativeCodeBuilder nativeCodeBuilder = this.m_PlatformProvider.CreateIl2CppNativeCodeBuilder();
      if (nativeCodeBuilder != null)
      {
        Il2CppNativeCodeBuilderUtils.ClearAndPrepareCacheDirectory(nativeCodeBuilder);
        arguments.AddRange(Il2CppNativeCodeBuilderUtils.AddBuilderArguments(nativeCodeBuilder, this.OutputFileRelativePath(), (IEnumerable<string>) this.m_PlatformProvider.includePaths, this.m_DebugBuild));
      }
      arguments.Add(string.Format("--map-file-parser=\"{0}\"", (object) IL2CPPBuilder.GetMapFileParserPath()));
      string additionalIl2CppArgs = PlayerSettings.GetAdditionalIl2CppArgs();
      if (!string.IsNullOrEmpty(additionalIl2CppArgs))
        arguments.Add(additionalIl2CppArgs);
      string environmentVariable = Environment.GetEnvironmentVariable("IL2CPP_ADDITIONAL_ARGS");
      if (!string.IsNullOrEmpty(environmentVariable))
        arguments.Add(environmentVariable);
      List<string> source = new List<string>((IEnumerable<string>) userAssemblies);
      arguments.AddRange(source.Select<string, string>((Func<string, string>) (arg => "--assembly=\"" + Path.GetFullPath(arg) + "\"")));
      arguments.Add(string.Format("--generatedcppdir=\"{0}\"", (object) Path.GetFullPath(outputDirectory)));
      string info = "Converting managed assemblies to C++";
      if (nativeCodeBuilder != null)
        info = "Building native binary with IL2CPP...";
      if (EditorUtility.DisplayCancelableProgressBar("Building Player", info, 0.3f))
        throw new OperationCanceledException();
      Action<ProcessStartInfo> setupStartInfo = (Action<ProcessStartInfo>) null;
      if (nativeCodeBuilder != null)
        setupStartInfo = new Action<ProcessStartInfo>(nativeCodeBuilder.SetupStartInfo);
      this.RunIl2CppWithArguments(arguments, setupStartInfo, workingDirectory);
    }

    private void RunIl2CppWithArguments(List<string> arguments, Action<ProcessStartInfo> setupStartInfo, string workingDirectory)
    {
      string args = arguments.Aggregate<string, string>(string.Empty, (Func<string, string, string>) ((current, arg) => current + arg + " "));
      bool flag = this.ShouldUseIl2CppCore();
      string exe = !flag ? this.GetIl2CppExe() : this.GetIl2CppCoreExe();
      Console.WriteLine("Invoking il2cpp with arguments: " + args);
      CompilerOutputParserBase parser = this.m_PlatformProvider.CreateIl2CppOutputParser() ?? (CompilerOutputParserBase) new Il2CppOutputParser();
      if (flag)
        Runner.RunNetCoreProgram(exe, args, workingDirectory, parser, setupStartInfo);
      else
        Runner.RunManagedProgram(exe, args, workingDirectory, parser, setupStartInfo);
    }

    private string GetIl2CppExe()
    {
      return this.m_PlatformProvider.il2CppFolder + "/build/il2cpp.exe";
    }

    private string GetIl2CppCoreExe()
    {
      return this.m_PlatformProvider.il2CppFolder + "/build/il2cppcore/il2cppcore.dll";
    }

    private bool ShouldUseIl2CppCore()
    {
      bool flag = false;
      if (Application.platform == RuntimePlatform.OSXEditor)
      {
        if (SystemInfo.operatingSystem.StartsWith("Mac OS X 10."))
        {
          Version version = new Version(SystemInfo.operatingSystem.Substring(9));
          if (version >= new Version(10, 9) && version < new Version(10, 13))
            flag = true;
        }
        else
          flag = true;
      }
      return flag && NetCoreProgram.IsNetCoreAvailable();
    }
  }
}
