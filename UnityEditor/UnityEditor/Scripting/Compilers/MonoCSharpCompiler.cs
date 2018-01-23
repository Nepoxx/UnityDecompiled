// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.MonoCSharpCompiler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditor.Scripting.Compilers
{
  internal class MonoCSharpCompiler : MonoScriptCompilerBase
  {
    public static readonly string ReponseFilename = "mcs.rsp";

    public MonoCSharpCompiler(MonoIsland island, bool runUpdater)
      : base(island, runUpdater)
    {
    }

    protected override Program StartCompiler()
    {
      List<string> arguments = new List<string>() { "-debug", "-target:library", "-nowarn:0169", "-langversion:" + (EditorApplication.scriptingRuntimeVersion != ScriptingRuntimeVersion.Latest ? "4" : "6"), "-out:" + ScriptCompilerBase.PrepareFileName(this._island._output), "-unsafe" };
      if (!this._island._development_player && !this._island._editor)
        arguments.Add("-optimize");
      foreach (string reference in this._island._references)
        arguments.Add("-r:" + ScriptCompilerBase.PrepareFileName(reference));
      foreach (string str in ((IEnumerable<string>) this._island._defines).Distinct<string>())
        arguments.Add("-define:" + str);
      foreach (string file in this._island._files)
        arguments.Add(ScriptCompilerBase.PrepareFileName(file));
      string profileDirectory = MonoInstallationFinder.GetProfileDirectory(this._island._api_compatibility_level != ApiCompatibilityLevel.NET_2_0 ? this.GetMonoProfileLibDirectory() : "2.0-api", "MonoBleedingEdge");
      foreach (string additionalReference in this.GetAdditionalReferences())
      {
        string str = Path.Combine(profileDirectory, additionalReference);
        if (File.Exists(str))
          arguments.Add("-r:" + ScriptCompilerBase.PrepareFileName(str));
      }
      if (!this.AddCustomResponseFileIfPresent(arguments, MonoCSharpCompiler.ReponseFilename))
      {
        if (this._island._api_compatibility_level == ApiCompatibilityLevel.NET_2_0_Subset && this.AddCustomResponseFileIfPresent(arguments, "smcs.rsp"))
          Debug.LogWarning((object) string.Format("Using obsolete custom response file 'smcs.rsp'. Please use '{0}' instead.", (object) MonoCSharpCompiler.ReponseFilename));
        else if (this._island._api_compatibility_level == ApiCompatibilityLevel.NET_2_0 && this.AddCustomResponseFileIfPresent(arguments, "gmcs.rsp"))
          Debug.LogWarning((object) string.Format("Using obsolete custom response file 'gmcs.rsp'. Please use '{0}' instead.", (object) MonoCSharpCompiler.ReponseFilename));
      }
      return (Program) this.StartCompiler(this._island._target, this.GetCompilerPath(arguments), arguments, false, MonoInstallationFinder.GetMonoInstallation("MonoBleedingEdge"));
    }

    private string[] GetAdditionalReferences()
    {
      return new string[5]{ "System.Runtime.Serialization.dll", "System.Xml.Linq.dll", "UnityScript.dll", "UnityScript.Lang.dll", "Boo.Lang.dll" };
    }

    private string GetCompilerPath(List<string> arguments)
    {
      string profileDirectory = MonoInstallationFinder.GetProfileDirectory("4.5", "MonoBleedingEdge");
      string path = Path.Combine(profileDirectory, "mcs.exe");
      if (!File.Exists(path))
        throw new ApplicationException("Unable to find csharp compiler in " + profileDirectory);
      string str = this._island._api_compatibility_level != ApiCompatibilityLevel.NET_4_6 ? BuildPipeline.CompatibilityProfileToClassLibFolder(this._island._api_compatibility_level) : "4.6";
      arguments.Add("-sdk:" + str);
      return path;
    }

    protected override CompilerOutputParserBase CreateOutputParser()
    {
      return (CompilerOutputParserBase) new MonoCSharpCompilerOutputParser();
    }

    public static string[] Compile(string[] sources, string[] references, string[] defines, string outputFile)
    {
      using (MonoCSharpCompiler monoCsharpCompiler = new MonoCSharpCompiler(new MonoIsland(BuildTarget.StandaloneWindows, ApiCompatibilityLevel.NET_2_0_Subset, sources, references, defines, outputFile), false))
      {
        monoCsharpCompiler.BeginCompiling();
        while (!monoCsharpCompiler.Poll())
          Thread.Sleep(50);
        return ((IEnumerable<CompilerMessage>) monoCsharpCompiler.GetCompilerMessages()).Select<CompilerMessage, string>((Func<CompilerMessage, string>) (cm => cm.message)).ToArray<string>();
      }
    }
  }
}
