// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ManagedProgram
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
  internal class ManagedProgram : Program
  {
    public ManagedProgram(string monodistribution, string profile, string executable, string arguments, Action<ProcessStartInfo> setupStartInfo)
      : this(monodistribution, profile, executable, arguments, true, setupStartInfo)
    {
    }

    public ManagedProgram(string monodistribution, string profile, string executable, string arguments, bool setMonoEnvironmentVariables, Action<ProcessStartInfo> setupStartInfo)
    {
      string str1 = ManagedProgram.PathCombine(monodistribution, "bin", "mono");
      if (Application.platform == RuntimePlatform.WindowsEditor)
        str1 = CommandLineFormatter.PrepareFileName(str1 + ".exe");
      ProcessStartInfo processStartInfo = new ProcessStartInfo() { Arguments = CommandLineFormatter.PrepareFileName(executable) + " " + arguments, CreateNoWindow = true, FileName = str1, RedirectStandardError = true, RedirectStandardOutput = true, WorkingDirectory = Application.dataPath + "/..", UseShellExecute = false };
      if (setMonoEnvironmentVariables)
      {
        string str2 = ManagedProgram.PathCombine(monodistribution, "lib", "mono", profile);
        processStartInfo.EnvironmentVariables["MONO_PATH"] = str2;
        processStartInfo.EnvironmentVariables["MONO_CFG_DIR"] = ManagedProgram.PathCombine(monodistribution, "etc");
      }
      if (setupStartInfo != null)
        setupStartInfo(processStartInfo);
      this._process.StartInfo = processStartInfo;
    }

    private static string PathCombine(params string[] parts)
    {
      string path1 = parts[0];
      for (int index = 1; index < parts.Length; ++index)
        path1 = Path.Combine(path1, parts[index]);
      return path1;
    }
  }
}
