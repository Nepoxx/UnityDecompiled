// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.NetCoreProgram
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Diagnostics;
using System.IO;
using UnityEditor.Scripting.Compilers;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditor.Scripting
{
  internal class NetCoreProgram : Program
  {
    private static bool s_NetCoreAvailableChecked = false;
    private static bool s_NetCoreAvailable = false;

    public NetCoreProgram(string executable, string arguments, Action<ProcessStartInfo> setupStartInfo)
    {
      if (!NetCoreProgram.IsNetCoreAvailable())
        UnityEngine.Debug.LogError((object) "Creating NetCoreProgram, but IsNetCoreAvailable() == false; fix the caller!");
      ProcessStartInfo startInfoForArgs = NetCoreProgram.CreateDotNetCoreStartInfoForArgs(CommandLineFormatter.PrepareFileName(executable) + " " + arguments);
      if (setupStartInfo != null)
        setupStartInfo(startInfoForArgs);
      this._process.StartInfo = startInfoForArgs;
    }

    private static ProcessStartInfo CreateDotNetCoreStartInfoForArgs(string arguments)
    {
      string str1 = Paths.Combine(NetCoreProgram.GetSdkRoot(), "dotnet");
      if (Application.platform == RuntimePlatform.WindowsEditor)
        str1 = CommandLineFormatter.PrepareFileName(str1 + ".exe");
      ProcessStartInfo processStartInfo = new ProcessStartInfo() { Arguments = arguments, CreateNoWindow = true, FileName = str1, WorkingDirectory = Application.dataPath + "/.." };
      if (Application.platform == RuntimePlatform.OSXEditor)
      {
        string str2 = Path.Combine(Path.Combine(Path.Combine(NetCoreProgram.GetNetCoreRoot(), "NativeDeps"), "osx"), "lib");
        if (processStartInfo.EnvironmentVariables.ContainsKey("DYLD_LIBRARY_PATH"))
          processStartInfo.EnvironmentVariables["DYLD_LIBRARY_PATH"] = string.Format("{0}:{1}", (object) str2, (object) processStartInfo.EnvironmentVariables["DYLD_LIBRARY_PATH"]);
        else
          processStartInfo.EnvironmentVariables.Add("DYLD_LIBRARY_PATH", str2);
      }
      return processStartInfo;
    }

    private static string GetSdkRoot()
    {
      return Path.Combine(NetCoreProgram.GetNetCoreRoot(), "Sdk");
    }

    private static string GetNetCoreRoot()
    {
      return Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "NetCore");
    }

    public static bool IsNetCoreAvailable()
    {
      if (!NetCoreProgram.s_NetCoreAvailableChecked)
      {
        NetCoreProgram.s_NetCoreAvailableChecked = true;
        Program program = new Program(NetCoreProgram.CreateDotNetCoreStartInfoForArgs("--version"));
        try
        {
          program.Start();
        }
        catch (Exception ex)
        {
          UnityEngine.Debug.LogWarningFormat("Disabling CoreCLR, got exception trying to run with --version: {0}", (object) ex);
          return false;
        }
        program.WaitForExit(5000);
        if (!program.HasExited)
        {
          program.Kill();
          UnityEngine.Debug.LogWarning((object) "Disabling CoreCLR, timed out trying to run with --version");
          return false;
        }
        if (program.ExitCode != 0)
        {
          UnityEngine.Debug.LogWarningFormat("Disabling CoreCLR, got non-zero exit code: {0}, stderr: '{1}'", new object[2]
          {
            (object) program.ExitCode,
            (object) program.GetErrorOutputAsString()
          });
          return false;
        }
        NetCoreProgram.s_NetCoreAvailable = true;
      }
      return NetCoreProgram.s_NetCoreAvailable;
    }
  }
}
