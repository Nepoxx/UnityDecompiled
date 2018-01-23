// Decompiled with JetBrains decompiler
// Type: NativeCompiler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor.Utils;

internal abstract class NativeCompiler : INativeCompiler
{
  protected virtual string objectFileExtension
  {
    get
    {
      return "o";
    }
  }

  public abstract void CompileDynamicLibrary(string outFile, IEnumerable<string> sources, IEnumerable<string> includePaths, IEnumerable<string> libraries, IEnumerable<string> libraryPaths);

  protected virtual void SetupProcessStartInfo(ProcessStartInfo startInfo)
  {
  }

  protected void Execute(string arguments, string compilerPath)
  {
    ProcessStartInfo startInfo = new ProcessStartInfo(compilerPath, arguments);
    this.SetupProcessStartInfo(startInfo);
    this.RunProgram(startInfo);
  }

  protected void ExecuteCommand(string command, params string[] arguments)
  {
    ProcessStartInfo startInfo = new ProcessStartInfo(command, ((IEnumerable<string>) arguments).Aggregate<string>((Func<string, string, string>) ((buff, s) => buff + " " + s)));
    this.SetupProcessStartInfo(startInfo);
    this.RunProgram(startInfo);
  }

  private void RunProgram(ProcessStartInfo startInfo)
  {
    using (Program program = new Program(startInfo))
    {
      program.Start();
      do
        ;
      while (!program.WaitForExit(100));
      string str = string.Empty;
      string[] standardOutput = program.GetStandardOutput();
      if (standardOutput.Length > 0)
        str = ((IEnumerable<string>) standardOutput).Aggregate<string>((Func<string, string, string>) ((buf, s) => buf + Environment.NewLine + s));
      string[] errorOutput = program.GetErrorOutput();
      if (errorOutput.Length > 0)
        str += ((IEnumerable<string>) errorOutput).Aggregate<string>((Func<string, string, string>) ((buf, s) => buf + Environment.NewLine + s));
      if (program.ExitCode != 0)
      {
        UnityEngine.Debug.LogError((object) ("Failed running " + startInfo.FileName + " " + startInfo.Arguments + "\n\n" + str));
        throw new Exception("IL2CPP compile failed.");
      }
    }
  }

  protected static string Aggregate(IEnumerable<string> items, string prefix, string suffix)
  {
    return items.Aggregate<string, string>("", (Func<string, string, string>) ((current, additionalFile) => current + prefix + additionalFile + suffix));
  }

  internal static void ParallelFor<T>(T[] sources, Action<T> action)
  {
    Thread[] threadArray = new Thread[Environment.ProcessorCount];
    NativeCompiler.Counter counter1 = new NativeCompiler.Counter();
    for (int index = 0; index < threadArray.Length; ++index)
      threadArray[index] = new Thread((ParameterizedThreadStart) (obj =>
      {
        NativeCompiler.Counter counter2 = (NativeCompiler.Counter) obj;
        int num;
        while ((num = Interlocked.Increment(ref counter2.index)) <= sources.Length)
          action(sources[num - 1]);
      }));
    foreach (Thread thread in threadArray)
      thread.Start((object) counter1);
    foreach (Thread thread in threadArray)
      thread.Join();
  }

  protected internal static IEnumerable<string> AllSourceFilesIn(string directory)
  {
    return ((IEnumerable<string>) Directory.GetFiles(directory, "*.cpp", SearchOption.AllDirectories)).Concat<string>((IEnumerable<string>) Directory.GetFiles(directory, "*.c", SearchOption.AllDirectories));
  }

  protected internal static bool IsSourceFile(string source)
  {
    string extension = Path.GetExtension(source);
    return extension == "cpp" || extension == "c";
  }

  protected string ObjectFileFor(string source)
  {
    return Path.ChangeExtension(source, this.objectFileExtension);
  }

  private class Counter
  {
    public int index;
  }
}
