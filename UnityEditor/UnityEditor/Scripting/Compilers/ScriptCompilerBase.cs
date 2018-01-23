// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.ScriptCompilerBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditor.Scripting.Compilers
{
  internal abstract class ScriptCompilerBase : IDisposable
  {
    private string _responseFile = (string) null;
    private Program process;
    private bool _runAPIUpdater;
    protected MonoIsland _island;

    protected ScriptCompilerBase(MonoIsland island, bool runAPIUpdater)
    {
      this._island = island;
      this._runAPIUpdater = runAPIUpdater;
    }

    protected abstract Program StartCompiler();

    protected abstract CompilerOutputParserBase CreateOutputParser();

    protected string[] GetErrorOutput()
    {
      return this.process.GetErrorOutput();
    }

    protected string[] GetStandardOutput()
    {
      return this.process.GetStandardOutput();
    }

    public void BeginCompiling()
    {
      if (this.process != null)
        throw new InvalidOperationException("Compilation has already begun!");
      this.process = this.StartCompiler();
    }

    public virtual void Dispose()
    {
      if (this.process != null)
      {
        this.process.Dispose();
        this.process = (Program) null;
      }
      if (this._responseFile == null)
        return;
      File.Delete(this._responseFile);
      this._responseFile = (string) null;
    }

    public virtual bool Poll()
    {
      if (this.process == null)
        return true;
      return this.process.HasExited;
    }

    public void WaitForCompilationToFinish()
    {
      this.process.WaitForExit();
    }

    protected string GetMonoProfileLibDirectory()
    {
      return MonoInstallationFinder.GetProfileDirectory(BuildPipeline.CompatibilityProfileToClassLibFolder(this._island._api_compatibility_level), this._island._api_compatibility_level != ApiCompatibilityLevel.NET_4_6 ? "Mono" : "MonoBleedingEdge");
    }

    protected bool AddCustomResponseFileIfPresent(List<string> arguments, string responseFileName)
    {
      string path = Path.Combine("Assets", responseFileName);
      if (!File.Exists(path))
        return false;
      arguments.Add("@" + path);
      return true;
    }

    protected string GenerateResponseFile(List<string> arguments)
    {
      this._responseFile = CommandLineFormatter.GenerateResponseFile((IEnumerable<string>) arguments);
      return this._responseFile;
    }

    public static string[] GetResponseFileDefinesFromFile(string responseFileName)
    {
      string path = Path.Combine("Assets", responseFileName);
      if (!File.Exists(path))
        return new string[0];
      return ScriptCompilerBase.GetResponseFileDefinesFromText(File.ReadAllText(path));
    }

    public static string[] GetResponseFileDefinesFromText(string responseFileText)
    {
      int length = "-define:".Length;
      if (!responseFileText.Contains("-define:"))
        return new string[0];
      List<string> stringList = new List<string>();
      string str1 = responseFileText;
      char[] chArray = new char[2]{ ' ', '\n' };
      foreach (string str2 in str1.Split(chArray))
      {
        string str3 = str2.Trim();
        if (str3.StartsWith("-define:"))
        {
          string[] strArray = str3.Substring(length).Split(new char[2]{ ',', ';' });
          stringList.AddRange((IEnumerable<string>) strArray);
        }
      }
      return stringList.ToArray();
    }

    protected static string PrepareFileName(string fileName)
    {
      return CommandLineFormatter.PrepareFileName(fileName);
    }

    public virtual CompilerMessage[] GetCompilerMessages()
    {
      if (!this.Poll())
        Debug.LogWarning((object) "Compile process is not finished yet. This should not happen.");
      this.DumpStreamOutputToLog();
      return this.CreateOutputParser().Parse(this.GetStreamContainingCompilerMessages(), this.CompilationHadFailure()).ToArray<CompilerMessage>();
    }

    protected bool CompilationHadFailure()
    {
      return this.process.ExitCode != 0;
    }

    protected virtual string[] GetStreamContainingCompilerMessages()
    {
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) this.GetErrorOutput());
      stringList.Add(string.Empty);
      stringList.AddRange((IEnumerable<string>) this.GetStandardOutput());
      return stringList.ToArray();
    }

    private void DumpStreamOutputToLog()
    {
      bool flag = this.CompilationHadFailure();
      string[] errorOutput = this.GetErrorOutput();
      if (!flag && errorOutput.Length == 0)
        return;
      Console.WriteLine("");
      Console.WriteLine("-----Compiler Commandline Arguments:");
      this.process.LogProcessStartInfo();
      string[] standardOutput = this.GetStandardOutput();
      Console.WriteLine("-----CompilerOutput:-stdout--exitcode: " + (object) this.process.ExitCode + "--compilationhadfailure: " + (object) flag + "--outfile: " + this._island._output);
      foreach (string str in standardOutput)
        Console.WriteLine(str);
      Console.WriteLine("-----CompilerOutput:-stderr----------");
      foreach (string str in errorOutput)
        Console.WriteLine(str);
      Console.WriteLine("-----EndCompilerOutput---------------");
    }

    protected void RunAPIUpdaterIfRequired(string responseFile)
    {
      if (!this._runAPIUpdater)
        return;
      APIUpdaterHelper.UpdateScripts(responseFile, this._island.GetExtensionOfSourceFiles());
    }
  }
}
