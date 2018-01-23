// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilation.CustomScriptAssembly
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.Scripting.ScriptCompilation
{
  internal class CustomScriptAssembly
  {
    static CustomScriptAssembly()
    {
      CustomScriptAssembly.Platforms = new CustomScriptAssemblyPlatform[20]
      {
        new CustomScriptAssemblyPlatform("Editor", "Editor", BuildTarget.NoTarget),
        new CustomScriptAssemblyPlatform("macOSStandalone", "macOS", BuildTarget.StandaloneOSX),
        new CustomScriptAssemblyPlatform("WindowsStandalone32", "Windows 32-bit", BuildTarget.StandaloneWindows),
        new CustomScriptAssemblyPlatform("WindowsStandalone64", "Windows 64-bit", BuildTarget.StandaloneWindows64),
        new CustomScriptAssemblyPlatform("LinuxStandalone32", "Linux 32-bit", BuildTarget.StandaloneLinux),
        new CustomScriptAssemblyPlatform("LinuxStandalone64", "Linux 64-bit", BuildTarget.StandaloneLinux64),
        new CustomScriptAssemblyPlatform("LinuxStandaloneUniversal", "Linux Universal", BuildTarget.StandaloneLinuxUniversal),
        new CustomScriptAssemblyPlatform("iOS", BuildTarget.iOS),
        new CustomScriptAssemblyPlatform("Android", BuildTarget.Android),
        new CustomScriptAssemblyPlatform("WebGL", BuildTarget.WebGL),
        new CustomScriptAssemblyPlatform("WSA", "Windows Store App", BuildTarget.WSAPlayer),
        new CustomScriptAssemblyPlatform("Tizen", BuildTarget.Tizen),
        new CustomScriptAssemblyPlatform("PSVita", BuildTarget.PSP2),
        new CustomScriptAssemblyPlatform("PS4", BuildTarget.PS4),
        new CustomScriptAssemblyPlatform("PSMobile", BuildTarget.PSM),
        new CustomScriptAssemblyPlatform("XboxOne", BuildTarget.XboxOne),
        new CustomScriptAssemblyPlatform("Nintendo3DS", BuildTarget.N3DS),
        new CustomScriptAssemblyPlatform("WiiU", BuildTarget.WiiU),
        new CustomScriptAssemblyPlatform("tvOS", BuildTarget.tvOS),
        new CustomScriptAssemblyPlatform("Switch", BuildTarget.Switch)
      };
    }

    public string FilePath { get; set; }

    public string PathPrefix { get; set; }

    public string Name { get; set; }

    public string[] References { get; set; }

    public CustomScriptAssemblyPlatform[] IncludePlatforms { get; set; }

    public CustomScriptAssemblyPlatform[] ExcludePlatforms { get; set; }

    public AssemblyFlags AssemblyFlags
    {
      get
      {
        return this.IncludePlatforms != null && this.IncludePlatforms.Length == 1 && this.IncludePlatforms[0].BuildTarget == BuildTarget.NoTarget ? AssemblyFlags.EditorOnly : AssemblyFlags.None;
      }
    }

    public static CustomScriptAssemblyPlatform[] Platforms { get; private set; }

    public bool IsCompatibleWithEditor()
    {
      if (this.ExcludePlatforms != null)
        return ((IEnumerable<CustomScriptAssemblyPlatform>) this.ExcludePlatforms).All<CustomScriptAssemblyPlatform>((Func<CustomScriptAssemblyPlatform, bool>) (p => p.BuildTarget != BuildTarget.NoTarget));
      if (this.IncludePlatforms != null)
        return ((IEnumerable<CustomScriptAssemblyPlatform>) this.IncludePlatforms).Any<CustomScriptAssemblyPlatform>((Func<CustomScriptAssemblyPlatform, bool>) (p => p.BuildTarget == BuildTarget.NoTarget));
      return true;
    }

    public bool IsCompatibleWith(BuildTarget buildTarget, EditorScriptCompilationOptions options)
    {
      if (this.IncludePlatforms == null && this.ExcludePlatforms == null)
        return true;
      bool flag = (options & EditorScriptCompilationOptions.BuildingForEditor) == EditorScriptCompilationOptions.BuildingForEditor;
      if (flag)
        return this.IsCompatibleWithEditor();
      if (flag)
        buildTarget = BuildTarget.NoTarget;
      if (this.ExcludePlatforms != null)
        return ((IEnumerable<CustomScriptAssemblyPlatform>) this.ExcludePlatforms).All<CustomScriptAssemblyPlatform>((Func<CustomScriptAssemblyPlatform, bool>) (p => p.BuildTarget != buildTarget));
      return ((IEnumerable<CustomScriptAssemblyPlatform>) this.IncludePlatforms).Any<CustomScriptAssemblyPlatform>((Func<CustomScriptAssemblyPlatform, bool>) (p => p.BuildTarget == buildTarget));
    }

    public static CustomScriptAssembly Create(string name, string directory)
    {
      CustomScriptAssembly customScriptAssembly = new CustomScriptAssembly();
      string source = AssetPath.ReplaceSeparators(directory);
      if ((int) source.Last<char>() != (int) AssetPath.Separator)
        source += (string) (object) AssetPath.Separator;
      customScriptAssembly.Name = name;
      customScriptAssembly.FilePath = source;
      customScriptAssembly.PathPrefix = source;
      customScriptAssembly.References = new string[0];
      return customScriptAssembly;
    }

    public static CustomScriptAssembly FromCustomScriptAssemblyData(string path, CustomScriptAssemblyData customScriptAssemblyData)
    {
      if (customScriptAssemblyData == null)
        return (CustomScriptAssembly) null;
      string str = path.Substring(0, path.Length - AssetPath.GetFileName(path).Length);
      CustomScriptAssembly customScriptAssembly = new CustomScriptAssembly();
      customScriptAssembly.Name = customScriptAssemblyData.name;
      customScriptAssembly.References = customScriptAssemblyData.references;
      customScriptAssembly.FilePath = path;
      customScriptAssembly.PathPrefix = str;
      if (customScriptAssemblyData.includePlatforms != null && customScriptAssemblyData.includePlatforms.Length > 0)
        customScriptAssembly.IncludePlatforms = ((IEnumerable<string>) customScriptAssemblyData.includePlatforms).Select<string, CustomScriptAssemblyPlatform>((Func<string, CustomScriptAssemblyPlatform>) (name => CustomScriptAssembly.GetPlatformFromName(name))).ToArray<CustomScriptAssemblyPlatform>();
      if (customScriptAssemblyData.excludePlatforms != null && customScriptAssemblyData.excludePlatforms.Length > 0)
        customScriptAssembly.ExcludePlatforms = ((IEnumerable<string>) customScriptAssemblyData.excludePlatforms).Select<string, CustomScriptAssemblyPlatform>((Func<string, CustomScriptAssemblyPlatform>) (name => CustomScriptAssembly.GetPlatformFromName(name))).ToArray<CustomScriptAssemblyPlatform>();
      return customScriptAssembly;
    }

    public static CustomScriptAssemblyPlatform GetPlatformFromName(string name)
    {
      foreach (CustomScriptAssemblyPlatform platform in CustomScriptAssembly.Platforms)
      {
        if (string.Equals(platform.Name, name, StringComparison.OrdinalIgnoreCase))
          return platform;
      }
      string[] array = ((IEnumerable<CustomScriptAssemblyPlatform>) CustomScriptAssembly.Platforms).Select<CustomScriptAssemblyPlatform, string>((Func<CustomScriptAssemblyPlatform, string>) (p => string.Format("\"{0}\"", (object) p.Name))).ToArray<string>();
      Array.Sort<string>(array);
      string str = string.Join(",\n", array);
      throw new ArgumentException(string.Format("Platform name '{0}' not supported.\nSupported platform names:\n{1}\n", (object) name, (object) str));
    }

    public static CustomScriptAssemblyPlatform GetPlatformFromBuildTarget(BuildTarget buildTarget)
    {
      foreach (CustomScriptAssemblyPlatform platform in CustomScriptAssembly.Platforms)
      {
        if (platform.BuildTarget == buildTarget)
          return platform;
      }
      throw new ArgumentException(string.Format("No CustomScriptAssemblyPlatform setup for BuildTarget '{0}'", (object) buildTarget));
    }
  }
}
