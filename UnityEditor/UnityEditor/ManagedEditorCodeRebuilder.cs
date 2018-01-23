// Decompiled with JetBrains decompiler
// Type: UnityEditor.ManagedEditorCodeRebuilder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor.Modules;
using UnityEditor.Scripting.Compilers;

namespace UnityEditor
{
  internal class ManagedEditorCodeRebuilder
  {
    private static readonly char[] kNewlineChars = new char[2]{ '\r', '\n' };

    private static bool Run(out CompilerMessage[] messages, bool includeModules)
    {
      int exitCode;
      messages = ManagedEditorCodeRebuilder.ParseResults(ManagedEditorCodeRebuilder.GetOutputStream(ManagedEditorCodeRebuilder.GetJamStartInfo(includeModules), out exitCode));
      return exitCode == 0;
    }

    private static ProcessStartInfo GetJamStartInfo(bool includeModules)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("jam.pl LiveReloadableEditorAssemblies");
      if (includeModules)
      {
        foreach (string jamTarget in ModuleManager.GetJamTargets())
          stringBuilder.Append(" ").Append(jamTarget);
      }
      return new ProcessStartInfo() { WorkingDirectory = Unsupported.GetBaseUnityDeveloperFolder(), RedirectStandardOutput = true, RedirectStandardError = false, Arguments = stringBuilder.ToString(), FileName = "perl" };
    }

    private static CompilerMessage[] ParseResults(string text)
    {
      Console.Write(text);
      string[] errorOutput = text.Split(ManagedEditorCodeRebuilder.kNewlineChars, StringSplitOptions.RemoveEmptyEntries);
      string unityDeveloperFolder = Unsupported.GetBaseUnityDeveloperFolder();
      List<CompilerMessage> list = new MonoCSharpCompilerOutputParser().Parse(errorOutput, false).ToList<CompilerMessage>();
      for (int index = 0; index < list.Count; ++index)
      {
        CompilerMessage compilerMessage = list[index];
        compilerMessage.file = Path.Combine(unityDeveloperFolder, compilerMessage.file);
        list[index] = compilerMessage;
      }
      return list.ToArray();
    }

    private static string GetOutputStream(ProcessStartInfo startInfo, out int exitCode)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ManagedEditorCodeRebuilder.\u003CGetOutputStream\u003Ec__AnonStorey0 streamCAnonStorey0 = new ManagedEditorCodeRebuilder.\u003CGetOutputStream\u003Ec__AnonStorey0();
      startInfo.UseShellExecute = false;
      startInfo.CreateNoWindow = true;
      Process process = new Process() { StartInfo = startInfo };
      // ISSUE: reference to a compiler-generated field
      streamCAnonStorey0.sbStandardOut = new StringBuilder();
      // ISSUE: reference to a compiler-generated field
      streamCAnonStorey0.sbStandardError = new StringBuilder();
      // ISSUE: reference to a compiler-generated method
      process.OutputDataReceived += new DataReceivedEventHandler(streamCAnonStorey0.\u003C\u003Em__0);
      // ISSUE: reference to a compiler-generated method
      process.ErrorDataReceived += new DataReceivedEventHandler(streamCAnonStorey0.\u003C\u003Em__1);
      process.Start();
      if (startInfo.RedirectStandardError)
        process.BeginErrorReadLine();
      else
        process.BeginOutputReadLine();
      process.WaitForExit();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      string str = !startInfo.RedirectStandardError ? streamCAnonStorey0.sbStandardOut.ToString() : streamCAnonStorey0.sbStandardError.ToString();
      exitCode = process.ExitCode;
      return str;
    }
  }
}
