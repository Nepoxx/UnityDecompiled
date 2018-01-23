// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.IL2CPPUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Utils;

namespace UnityEditorInternal
{
  internal class IL2CPPUtils
  {
    public const string BinaryMetadataSuffix = "-metadata.dat";

    internal static IIl2CppPlatformProvider PlatformProviderForNotModularPlatform(BuildTarget target, bool developmentBuild)
    {
      throw new Exception("Platform unsupported, or already modular.");
    }

    internal static IL2CPPBuilder RunIl2Cpp(string tempFolder, string stagingAreaData, IIl2CppPlatformProvider platformProvider, Action<string> modifyOutputBeforeCompile, RuntimeClassRegistry runtimeClassRegistry, bool debugBuild)
    {
      IL2CPPBuilder il2CppBuilder = new IL2CPPBuilder(tempFolder, stagingAreaData, platformProvider, modifyOutputBeforeCompile, runtimeClassRegistry, debugBuild, IL2CPPUtils.UseIl2CppCodegenWithMonoBackend(BuildPipeline.GetBuildTargetGroup(platformProvider.target)));
      il2CppBuilder.Run();
      return il2CppBuilder;
    }

    internal static IL2CPPBuilder RunIl2Cpp(string stagingAreaData, IIl2CppPlatformProvider platformProvider, Action<string> modifyOutputBeforeCompile, RuntimeClassRegistry runtimeClassRegistry, bool debugBuild)
    {
      IL2CPPBuilder il2CppBuilder = new IL2CPPBuilder(stagingAreaData, stagingAreaData, platformProvider, modifyOutputBeforeCompile, runtimeClassRegistry, debugBuild, IL2CPPUtils.UseIl2CppCodegenWithMonoBackend(BuildPipeline.GetBuildTargetGroup(platformProvider.target)));
      il2CppBuilder.Run();
      return il2CppBuilder;
    }

    internal static IL2CPPBuilder RunCompileAndLink(string tempFolder, string stagingAreaData, IIl2CppPlatformProvider platformProvider, Action<string> modifyOutputBeforeCompile, RuntimeClassRegistry runtimeClassRegistry, bool debugBuild)
    {
      IL2CPPBuilder il2CppBuilder = new IL2CPPBuilder(tempFolder, stagingAreaData, platformProvider, modifyOutputBeforeCompile, runtimeClassRegistry, debugBuild, IL2CPPUtils.UseIl2CppCodegenWithMonoBackend(BuildPipeline.GetBuildTargetGroup(platformProvider.target)));
      il2CppBuilder.RunCompileAndLink();
      return il2CppBuilder;
    }

    internal static void CopyEmbeddedResourceFiles(string tempFolder, string destinationFolder)
    {
      foreach (string str in ((IEnumerable<string>) Directory.GetFiles(Paths.Combine(IL2CPPBuilder.GetCppOutputPath(tempFolder), "Data", "Resources"))).Where<string>((Func<string, bool>) (f => f.EndsWith("-resources.dat"))))
        File.Copy(str, Paths.Combine(destinationFolder, Path.GetFileName(str)), true);
    }

    internal static void CopySymmapFile(string tempFolder, string destinationFolder)
    {
      IL2CPPUtils.CopySymmapFile(tempFolder, destinationFolder, string.Empty);
    }

    internal static void CopySymmapFile(string tempFolder, string destinationFolder, string destinationFileNameSuffix)
    {
      string str = Path.Combine(tempFolder, "SymbolMap");
      if (!File.Exists(str))
        return;
      File.Copy(str, Path.Combine(destinationFolder, "SymbolMap" + destinationFileNameSuffix), true);
    }

    internal static void CopyMetadataFiles(string tempFolder, string destinationFolder)
    {
      foreach (string str in ((IEnumerable<string>) Directory.GetFiles(Paths.Combine(IL2CPPBuilder.GetCppOutputPath(tempFolder), "Data", "Metadata"))).Where<string>((Func<string, bool>) (f => f.EndsWith("-metadata.dat"))))
        File.Copy(str, Paths.Combine(destinationFolder, Path.GetFileName(str)), true);
    }

    internal static void CopyConfigFiles(string tempFolder, string destinationFolder)
    {
      FileUtil.CopyDirectoryRecursive(Paths.Combine(IL2CPPBuilder.GetCppOutputPath(tempFolder), "Data", "etc"), destinationFolder);
    }

    internal static string editorIl2cppFolder
    {
      get
      {
        return Path.GetFullPath(Path.Combine(EditorApplication.applicationContentsPath, "il2cpp"));
      }
    }

    internal static string ApiCompatibilityLevelToDotNetProfileArgument(ApiCompatibilityLevel compatibilityLevel)
    {
      switch (compatibilityLevel)
      {
        case ApiCompatibilityLevel.NET_2_0:
          return "net20";
        case ApiCompatibilityLevel.NET_2_0_Subset:
          return "legacyunity";
        case ApiCompatibilityLevel.NET_4_6:
          return "net45";
        default:
          throw new NotSupportedException(string.Format("ApiCompatibilityLevel.{0} is not supported by IL2CPP!", (object) compatibilityLevel));
      }
    }

    internal static bool UseIl2CppCodegenWithMonoBackend(BuildTargetGroup targetGroup)
    {
      return EditorApplication.scriptingRuntimeVersion == ScriptingRuntimeVersion.Latest && EditorApplication.useLibmonoBackendForIl2cpp && PlayerSettings.GetScriptingBackend(targetGroup) == ScriptingImplementation.IL2CPP;
    }
  }
}
