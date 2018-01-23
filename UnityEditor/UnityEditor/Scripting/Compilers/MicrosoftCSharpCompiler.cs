// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.MicrosoftCSharpCompiler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor.Modules;
using UnityEditor.Utils;

namespace UnityEditor.Scripting.Compilers
{
  internal class MicrosoftCSharpCompiler : ScriptCompilerBase
  {
    public MicrosoftCSharpCompiler(MonoIsland island, bool runUpdater)
      : base(island, runUpdater)
    {
    }

    private BuildTarget BuildTarget
    {
      get
      {
        return this._island._target;
      }
    }

    private static string[] GetReferencesFromMonoDistribution()
    {
      return new string[9]{ "mscorlib.dll", "System.dll", "System.Core.dll", "System.Runtime.Serialization.dll", "System.Xml.dll", "System.Xml.Linq.dll", "UnityScript.dll", "UnityScript.Lang.dll", "Boo.Lang.dll" };
    }

    private string[] GetClassLibraries()
    {
      BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(this.BuildTarget);
      if (PlayerSettings.GetScriptingBackend(buildTargetGroup) != ScriptingImplementation.WinRTDotNET)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MicrosoftCSharpCompiler.\u003CGetClassLibraries\u003Ec__AnonStorey0 librariesCAnonStorey0 = new MicrosoftCSharpCompiler.\u003CGetClassLibraries\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        librariesCAnonStorey0.monoAssemblyDirectory = this.GetMonoProfileLibDirectory();
        List<string> stringList = new List<string>();
        // ISSUE: reference to a compiler-generated method
        stringList.AddRange(((IEnumerable<string>) MicrosoftCSharpCompiler.GetReferencesFromMonoDistribution()).Select<string, string>(new Func<string, string>(librariesCAnonStorey0.\u003C\u003Em__0)));
        if (PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup) == ApiCompatibilityLevel.NET_4_6)
        {
          // ISSUE: reference to a compiler-generated field
          string path = Path.Combine(librariesCAnonStorey0.monoAssemblyDirectory, "Facades");
          stringList.AddRange((IEnumerable<string>) Directory.GetFiles(path, "*.dll"));
        }
        return stringList.ToArray();
      }
      if (this.BuildTarget != BuildTarget.WSAPlayer)
        throw new InvalidOperationException(string.Format("MicrosoftCSharpCompiler cannot build for .NET Scripting backend for BuildTarget.{0}.", (object) this.BuildTarget));
      return new NuGetPackageResolver() { ProjectLockFile = "UWP\\project.lock.json" }.Resolve();
    }

    private void FillCompilerOptions(List<string> arguments, out string argsPrefix)
    {
      argsPrefix = "/noconfig ";
      arguments.Add("/nostdlib+");
      arguments.Add("/preferreduilang:en-US");
      ICompilationExtension compilationExtension = ModuleManager.FindPlatformSupportModule(ModuleManager.GetTargetStringFromBuildTarget(this.BuildTarget)).CreateCompilationExtension();
      arguments.AddRange(((IEnumerable<string>) this.GetClassLibraries()).Select<string, string>((Func<string, string>) (r => "/reference:\"" + r + "\"")));
      arguments.AddRange(compilationExtension.GetAdditionalAssemblyReferences().Select<string, string>((Func<string, string>) (r => "/reference:\"" + r + "\"")));
      arguments.AddRange(compilationExtension.GetWindowsMetadataReferences().Select<string, string>((Func<string, string>) (r => "/reference:\"" + r + "\"")));
      arguments.AddRange(compilationExtension.GetAdditionalDefines().Select<string, string>((Func<string, string>) (d => "/define:" + d)));
      arguments.AddRange(compilationExtension.GetAdditionalSourceFiles());
    }

    private static void ThrowCompilerNotFoundException(string path)
    {
      throw new Exception(string.Format("'{0}' not found. Is your Unity installation corrupted?", (object) path));
    }

    private Program StartCompilerImpl(List<string> arguments, string argsPrefix)
    {
      foreach (string reference in this._island._references)
        arguments.Add("/reference:" + ScriptCompilerBase.PrepareFileName(reference));
      foreach (string str in ((IEnumerable<string>) this._island._defines).Distinct<string>())
        arguments.Add("/define:" + str);
      foreach (string file in this._island._files)
        arguments.Add(ScriptCompilerBase.PrepareFileName(file).Replace('/', '\\'));
      string path1 = Paths.Combine(EditorApplication.applicationContentsPath, "Tools", "Roslyn", "CoreRun.exe").Replace('/', '\\');
      string path2 = Paths.Combine(EditorApplication.applicationContentsPath, "Tools", "Roslyn", "csc.exe").Replace('/', '\\');
      if (!File.Exists(path1))
        MicrosoftCSharpCompiler.ThrowCompilerNotFoundException(path1);
      if (!File.Exists(path2))
        MicrosoftCSharpCompiler.ThrowCompilerNotFoundException(path2);
      this.AddCustomResponseFileIfPresent(arguments, "csc.rsp");
      string responseFile = CommandLineFormatter.GenerateResponseFile((IEnumerable<string>) arguments);
      this.RunAPIUpdaterIfRequired(responseFile);
      Program program = new Program(new ProcessStartInfo() { Arguments = "\"" + path2 + "\" " + argsPrefix + "@" + responseFile, FileName = path1, CreateNoWindow = true });
      program.Start();
      return program;
    }

    protected override Program StartCompiler()
    {
      List<string> arguments = new List<string>() { "/target:library", "/nowarn:0169", "/unsafe", "/out:" + ScriptCompilerBase.PrepareFileName(this._island._output) };
      if (!this._island._development_player)
      {
        arguments.Add("/debug:pdbonly");
        arguments.Add("/optimize+");
      }
      else
      {
        arguments.Add("/debug:full");
        arguments.Add("/optimize-");
      }
      string argsPrefix;
      this.FillCompilerOptions(arguments, out argsPrefix);
      return this.StartCompilerImpl(arguments, argsPrefix);
    }

    protected override string[] GetStreamContainingCompilerMessages()
    {
      return this.GetStandardOutput();
    }

    protected override CompilerOutputParserBase CreateOutputParser()
    {
      return (CompilerOutputParserBase) new MicrosoftCSharpCompilerOutputParser();
    }
  }
}
