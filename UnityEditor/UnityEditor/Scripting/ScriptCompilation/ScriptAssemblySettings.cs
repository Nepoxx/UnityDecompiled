// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilation.ScriptAssemblySettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Scripting.ScriptCompilation
{
  internal class ScriptAssemblySettings
  {
    public ScriptAssemblySettings()
    {
      this.BuildTarget = BuildTarget.NoTarget;
      this.BuildTargetGroup = BuildTargetGroup.Unknown;
    }

    public BuildTarget BuildTarget { get; set; }

    public BuildTargetGroup BuildTargetGroup { get; set; }

    public string OutputDirectory { get; set; }

    public string[] Defines { get; set; }

    public ApiCompatibilityLevel ApiCompatibilityLevel { get; set; }

    public EditorScriptCompilationOptions CompilationOptions { get; set; }

    public string FilenameSuffix { get; set; }

    public bool BuildingForEditor
    {
      get
      {
        return (this.CompilationOptions & EditorScriptCompilationOptions.BuildingForEditor) == EditorScriptCompilationOptions.BuildingForEditor;
      }
    }

    public bool BuildingDevelopmentBuild
    {
      get
      {
        return (this.CompilationOptions & EditorScriptCompilationOptions.BuildingDevelopmentBuild) == EditorScriptCompilationOptions.BuildingDevelopmentBuild;
      }
    }
  }
}
