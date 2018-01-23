// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.Runner
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Scripting;
using UnityEditor.Scripting.Compilers;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class Runner
  {
    internal static void RunManagedProgram(string exe, string args)
    {
      Runner.RunManagedProgram(exe, args, Application.dataPath + "/..", (CompilerOutputParserBase) null, (Action<ProcessStartInfo>) null);
    }

    internal static void RunManagedProgram(string exe, string args, string workingDirectory, CompilerOutputParserBase parser, Action<ProcessStartInfo> setupStartInfo)
    {
      Program p;
      if (Application.platform == RuntimePlatform.WindowsEditor)
      {
        ProcessStartInfo si = new ProcessStartInfo() { Arguments = args, CreateNoWindow = true, FileName = exe };
        if (setupStartInfo != null)
          setupStartInfo(si);
        p = new Program(si);
      }
      else
        p = (Program) new ManagedProgram(MonoInstallationFinder.GetMonoInstallation("MonoBleedingEdge"), (string) null, exe, args, false, setupStartInfo);
      Runner.RunProgram(p, exe, args, workingDirectory, parser);
    }

    internal static void RunNetCoreProgram(string exe, string args, string workingDirectory, CompilerOutputParserBase parser, Action<ProcessStartInfo> setupStartInfo)
    {
      Runner.RunProgram((Program) new NetCoreProgram(exe, args, setupStartInfo), exe, args, workingDirectory, parser);
    }

    public static void RunNativeProgram(string exe, string args)
    {
      using (NativeProgram nativeProgram = new NativeProgram(exe, args))
      {
        nativeProgram.Start();
        nativeProgram.WaitForExit();
        if (nativeProgram.ExitCode != 0)
        {
          UnityEngine.Debug.LogError((object) ("Failed running " + exe + " " + args + "\n\n" + nativeProgram.GetAllOutput()));
          throw new Exception(string.Format("{0} did not run properly!", (object) exe));
        }
      }
    }

    private static void RunProgram(Program p, string exe, string args, string workingDirectory, CompilerOutputParserBase parser)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      using (p)
      {
        p.GetProcessStartInfo().WorkingDirectory = workingDirectory;
        p.Start();
        p.WaitForExit();
        stopwatch.Stop();
        Console.WriteLine("{0} exited after {1} ms.", (object) exe, (object) stopwatch.ElapsedMilliseconds);
        IEnumerable<CompilerMessage> compilerMessages = (IEnumerable<CompilerMessage>) null;
        if (parser != null)
        {
          string[] errorOutput = p.GetErrorOutput();
          string[] standardOutput = p.GetStandardOutput();
          compilerMessages = parser.Parse(errorOutput, standardOutput, true);
        }
        if (p.ExitCode != 0)
        {
          if (compilerMessages != null)
          {
            foreach (CompilerMessage compilerMessage in compilerMessages)
              UnityEngine.Debug.LogPlayerBuildError(compilerMessage.message, compilerMessage.file, compilerMessage.line, compilerMessage.column);
          }
          UnityEngine.Debug.LogError((object) ("Failed running " + exe + " " + args + "\n\n" + p.GetAllOutput()));
          throw new Exception(string.Format("{0} did not run properly!", (object) exe));
        }
        if (compilerMessages == null)
          return;
        foreach (CompilerMessage compilerMessage in compilerMessages)
          Console.WriteLine(compilerMessage.message + " - " + compilerMessage.file + " - " + (object) compilerMessage.line + " - " + (object) compilerMessage.column);
      }
    }
  }
}
