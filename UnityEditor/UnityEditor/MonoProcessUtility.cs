// Decompiled with JetBrains decompiler
// Type: UnityEditor.MonoProcessUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Diagnostics;
using System.IO;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditor
{
  internal class MonoProcessUtility
  {
    public static string ProcessToString(Process process)
    {
      return process.StartInfo.FileName + " " + process.StartInfo.Arguments + " current dir : " + process.StartInfo.WorkingDirectory + "\n";
    }

    public static void RunMonoProcess(Process process, string name, string resultingFile)
    {
      MonoProcessRunner monoProcessRunner = new MonoProcessRunner();
      bool flag = monoProcessRunner.Run(process);
      if (process.ExitCode != 0 || !File.Exists(resultingFile))
      {
        string message = "Failed " + name + ": " + MonoProcessUtility.ProcessToString(process) + " result file exists: " + (object) File.Exists(resultingFile) + ". Timed out: " + (object) !flag + "\n\n" + "stdout:\n" + (object) monoProcessRunner.Output + "\n" + "stderr:\n" + (object) monoProcessRunner.Error + "\n";
        Console.WriteLine(message);
        throw new UnityException(message);
      }
    }

    public static Process PrepareMonoProcess(string workDir)
    {
      Process process = new Process();
      string str = Application.platform != RuntimePlatform.WindowsEditor ? "mono" : "mono.exe";
      process.StartInfo.FileName = Paths.Combine(MonoInstallationFinder.GetMonoInstallation(), "bin", str);
      process.StartInfo.EnvironmentVariables["_WAPI_PROCESS_HANDLE_OFFSET"] = "5";
      string classLibFolder = BuildPipeline.CompatibilityProfileToClassLibFolder(ApiCompatibilityLevel.NET_2_0);
      process.StartInfo.EnvironmentVariables["MONO_PATH"] = MonoInstallationFinder.GetProfileDirectory(classLibFolder);
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.WorkingDirectory = workDir;
      return process;
    }

    public static Process PrepareMonoProcessBleedingEdge(string workDir)
    {
      Process process = new Process();
      string str = Application.platform != RuntimePlatform.WindowsEditor ? "mono" : "mono.exe";
      process.StartInfo.FileName = Paths.Combine(MonoInstallationFinder.GetMonoBleedingEdgeInstallation(), "bin", str);
      process.StartInfo.EnvironmentVariables["_WAPI_PROCESS_HANDLE_OFFSET"] = "5";
      string classLibFolder = BuildPipeline.CompatibilityProfileToClassLibFolder(ApiCompatibilityLevel.NET_4_6);
      process.StartInfo.EnvironmentVariables["MONO_PATH"] = MonoInstallationFinder.GetProfileDirectory(classLibFolder);
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.WorkingDirectory = workDir;
      return process;
    }
  }
}
