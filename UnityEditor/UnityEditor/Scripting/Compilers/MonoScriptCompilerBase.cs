// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.MonoScriptCompilerBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor.Utils;

namespace UnityEditor.Scripting.Compilers
{
  internal abstract class MonoScriptCompilerBase : ScriptCompilerBase
  {
    protected MonoScriptCompilerBase(MonoIsland island, bool runUpdater)
      : base(island, runUpdater)
    {
    }

    protected ManagedProgram StartCompiler(BuildTarget target, string compiler, List<string> arguments)
    {
      this.AddCustomResponseFileIfPresent(arguments, Path.GetFileNameWithoutExtension(compiler) + ".rsp");
      string monodistro = this._island._api_compatibility_level != ApiCompatibilityLevel.NET_4_6 ? MonoInstallationFinder.GetMonoInstallation() : MonoInstallationFinder.GetMonoBleedingEdgeInstallation();
      return this.StartCompiler(target, compiler, arguments, true, monodistro);
    }

    protected ManagedProgram StartCompiler(BuildTarget target, string compiler, List<string> arguments, bool setMonoEnvironmentVariables, string monodistro)
    {
      string responseFile = CommandLineFormatter.GenerateResponseFile((IEnumerable<string>) arguments);
      this.RunAPIUpdaterIfRequired(responseFile);
      ManagedProgram managedProgram = new ManagedProgram(monodistro, BuildPipeline.CompatibilityProfileToClassLibFolder(this._island._api_compatibility_level), compiler, " @" + responseFile, setMonoEnvironmentVariables, (Action<ProcessStartInfo>) null);
      managedProgram.Start();
      return managedProgram;
    }
  }
}
