// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.Il2CppNativeCodeBuilderUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.IO;

namespace UnityEditorInternal
{
  public static class Il2CppNativeCodeBuilderUtils
  {
    public static IEnumerable<string> AddBuilderArguments(Il2CppNativeCodeBuilder builder, string outputRelativePath, IEnumerable<string> includeRelativePaths, bool debugBuild)
    {
      List<string> stringList = new List<string>();
      stringList.Add("--compile-cpp");
      if (builder.LinkLibIl2CppStatically)
        stringList.Add("--libil2cpp-static");
      stringList.Add(Il2CppNativeCodeBuilderUtils.FormatArgument("platform", builder.CompilerPlatform));
      stringList.Add(Il2CppNativeCodeBuilderUtils.FormatArgument("architecture", builder.CompilerArchitecture));
      if (debugBuild)
        stringList.Add(Il2CppNativeCodeBuilderUtils.FormatArgument("configuration", "Debug"));
      else
        stringList.Add(Il2CppNativeCodeBuilderUtils.FormatArgument("configuration", "Release"));
      stringList.Add(Il2CppNativeCodeBuilderUtils.FormatArgument("outputpath", builder.ConvertOutputFileToFullPath(outputRelativePath)));
      if (!string.IsNullOrEmpty(builder.CacheDirectory))
        stringList.Add(Il2CppNativeCodeBuilderUtils.FormatArgument("cachedirectory", Il2CppNativeCodeBuilderUtils.CacheDirectoryPathFor(builder.CacheDirectory)));
      if (!string.IsNullOrEmpty(builder.CompilerFlags))
        stringList.Add(Il2CppNativeCodeBuilderUtils.FormatArgument("compiler-flags", builder.CompilerFlags));
      if (!string.IsNullOrEmpty(builder.LinkerFlags))
        stringList.Add(Il2CppNativeCodeBuilderUtils.FormatArgument("linker-flags", builder.LinkerFlags));
      if (!string.IsNullOrEmpty(builder.PluginPath))
        stringList.Add(Il2CppNativeCodeBuilderUtils.FormatArgument("plugin", builder.PluginPath));
      foreach (string fullPath in builder.ConvertIncludesToFullPaths(includeRelativePaths))
        stringList.Add(Il2CppNativeCodeBuilderUtils.FormatArgument("additional-include-directories", fullPath));
      stringList.AddRange(builder.AdditionalIl2CPPArguments);
      return (IEnumerable<string>) stringList;
    }

    public static void ClearAndPrepareCacheDirectory(Il2CppNativeCodeBuilder builder)
    {
      string fullUnityVersion = InternalEditorUtility.GetFullUnityVersion();
      Il2CppNativeCodeBuilderUtils.ClearCacheIfEditorVersionDiffers(builder, fullUnityVersion);
      Il2CppNativeCodeBuilderUtils.PrepareCacheDirectory(builder, fullUnityVersion);
    }

    public static void ClearCacheIfEditorVersionDiffers(Il2CppNativeCodeBuilder builder, string currentEditorVersion)
    {
      string path = Il2CppNativeCodeBuilderUtils.CacheDirectoryPathFor(builder.CacheDirectory);
      if (!Directory.Exists(path) || File.Exists(Path.Combine(builder.CacheDirectory, Il2CppNativeCodeBuilderUtils.EditorVersionFilenameFor(currentEditorVersion))))
        return;
      Directory.Delete(path, true);
    }

    public static void PrepareCacheDirectory(Il2CppNativeCodeBuilder builder, string currentEditorVersion)
    {
      Directory.CreateDirectory(Il2CppNativeCodeBuilderUtils.CacheDirectoryPathFor(builder.CacheDirectory));
      string path = Path.Combine(builder.CacheDirectory, Il2CppNativeCodeBuilderUtils.EditorVersionFilenameFor(currentEditorVersion));
      if (File.Exists(path))
        return;
      File.Create(path).Dispose();
    }

    public static string ObjectFilePathInCacheDirectoryFor(string builderCacheDirectory)
    {
      return Il2CppNativeCodeBuilderUtils.CacheDirectoryPathFor(builderCacheDirectory);
    }

    private static string CacheDirectoryPathFor(string builderCacheDirectory)
    {
      return builderCacheDirectory + "/il2cpp_cache";
    }

    private static string FormatArgument(string name, string value)
    {
      return string.Format("--{0}=\"{1}\"", (object) name, (object) Il2CppNativeCodeBuilderUtils.EscapeEmbeddedQuotes(value));
    }

    private static string EditorVersionFilenameFor(string editorVersion)
    {
      return string.Format("il2cpp_cache {0}", (object) editorVersion);
    }

    private static string EscapeEmbeddedQuotes(string value)
    {
      return value.Replace("\"", "\\\"");
    }
  }
}
