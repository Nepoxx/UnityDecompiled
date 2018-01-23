// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilation.ScriptAssembly
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Scripting.Compilers;

namespace UnityEditor.Scripting.ScriptCompilation
{
  internal class ScriptAssembly
  {
    public AssemblyFlags Flags { get; set; }

    public BuildTarget BuildTarget { get; set; }

    public SupportedLanguage Language { get; set; }

    public ApiCompatibilityLevel ApiCompatibilityLevel { get; set; }

    public string Filename { get; set; }

    public string OutputDirectory { get; set; }

    public ScriptAssembly[] ScriptAssemblyReferences { get; set; }

    public string[] References { get; set; }

    public string[] Defines { get; set; }

    public string[] Files { get; set; }

    public bool RunUpdater { get; set; }

    public string FullPath
    {
      get
      {
        return AssetPath.Combine(this.OutputDirectory, this.Filename);
      }
    }

    public string[] GetAllReferences()
    {
      return ((IEnumerable<string>) this.References).Concat<string>(((IEnumerable<ScriptAssembly>) this.ScriptAssemblyReferences).Select<ScriptAssembly, string>((Func<ScriptAssembly, string>) (a => a.FullPath))).ToArray<string>();
    }

    public MonoIsland ToMonoIsland(EditorScriptCompilationOptions options, string buildOutputDirectory)
    {
      return new MonoIsland(this.BuildTarget, (options & EditorScriptCompilationOptions.BuildingForEditor) == EditorScriptCompilationOptions.BuildingForEditor, (options & EditorScriptCompilationOptions.BuildingDevelopmentBuild) == EditorScriptCompilationOptions.BuildingDevelopmentBuild, this.ApiCompatibilityLevel, this.Files, ((IEnumerable<ScriptAssembly>) this.ScriptAssemblyReferences).Select<ScriptAssembly, string>((Func<ScriptAssembly, string>) (a => AssetPath.Combine(a.OutputDirectory, a.Filename))).Concat<string>((IEnumerable<string>) this.References).ToArray<string>(), this.Defines, AssetPath.Combine(buildOutputDirectory, this.Filename));
    }
  }
}
