// Decompiled with JetBrains decompiler
// Type: UnityEditor.MonoCrossCompile
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace UnityEditor
{
  internal class MonoCrossCompile
  {
    public static string ArtifactsPath = (string) null;

    public static void CrossCompileAOTDirectory(BuildTarget buildTarget, CrossCompileOptions crossCompileOptions, string sourceAssembliesFolder, string targetCrossCompiledASMFolder, string additionalOptions)
    {
      MonoCrossCompile.CrossCompileAOTDirectory(buildTarget, crossCompileOptions, sourceAssembliesFolder, targetCrossCompiledASMFolder, "", additionalOptions);
    }

    public static void CrossCompileAOTDirectory(BuildTarget buildTarget, CrossCompileOptions crossCompileOptions, string sourceAssembliesFolder, string targetCrossCompiledASMFolder, string pathExtension, string additionalOptions)
    {
      string buildToolsDirectory = BuildPipeline.GetBuildToolsDirectory(buildTarget);
      string crossCompilerAbsolutePath = Application.platform != RuntimePlatform.OSXEditor ? Path.Combine(Path.Combine(buildToolsDirectory, pathExtension), "mono-xcompiler.exe") : Path.Combine(Path.Combine(buildToolsDirectory, pathExtension), "mono-xcompiler");
      sourceAssembliesFolder = Path.Combine(Directory.GetCurrentDirectory(), sourceAssembliesFolder);
      targetCrossCompiledASMFolder = Path.Combine(Directory.GetCurrentDirectory(), targetCrossCompiledASMFolder);
      foreach (string file in Directory.GetFiles(sourceAssembliesFolder))
      {
        if (!(Path.GetExtension(file) != ".dll"))
        {
          string fileName = Path.GetFileName(file);
          string output = Path.Combine(targetCrossCompiledASMFolder, fileName + ".s");
          if (EditorUtility.DisplayCancelableProgressBar("Building Player", "AOT cross compile " + fileName, 0.95f))
            throw new OperationCanceledException();
          MonoCrossCompile.CrossCompileAOT(buildTarget, crossCompilerAbsolutePath, sourceAssembliesFolder, crossCompileOptions, fileName, output, additionalOptions);
        }
      }
    }

    public static bool CrossCompileAOTDirectoryParallel(BuildTarget buildTarget, CrossCompileOptions crossCompileOptions, string sourceAssembliesFolder, string targetCrossCompiledASMFolder, string additionalOptions)
    {
      return MonoCrossCompile.CrossCompileAOTDirectoryParallel(buildTarget, crossCompileOptions, sourceAssembliesFolder, targetCrossCompiledASMFolder, "", additionalOptions);
    }

    public static bool CrossCompileAOTDirectoryParallel(BuildTarget buildTarget, CrossCompileOptions crossCompileOptions, string sourceAssembliesFolder, string targetCrossCompiledASMFolder, string pathExtension, string additionalOptions)
    {
      string buildToolsDirectory = BuildPipeline.GetBuildToolsDirectory(buildTarget);
      return MonoCrossCompile.CrossCompileAOTDirectoryParallel(Application.platform != RuntimePlatform.OSXEditor ? Path.Combine(Path.Combine(buildToolsDirectory, pathExtension), "mono-xcompiler.exe") : Path.Combine(Path.Combine(buildToolsDirectory, pathExtension), "mono-xcompiler"), buildTarget, crossCompileOptions, sourceAssembliesFolder, targetCrossCompiledASMFolder, additionalOptions);
    }

    private static bool WaitForBuildOfFile(List<ManualResetEvent> events, ref long timeout)
    {
      long num1 = DateTime.Now.Ticks / 10000L;
      int index = WaitHandle.WaitAny((WaitHandle[]) events.ToArray(), (int) timeout);
      long num2 = DateTime.Now.Ticks / 10000L;
      if (index == 258)
        return false;
      events.RemoveAt(index);
      timeout -= num2 - num1;
      if (timeout < 0L)
        timeout = 0L;
      return true;
    }

    public static void DisplayAOTProgressBar(int totalFiles, int filesFinished)
    {
      EditorUtility.DisplayProgressBar("Building Player", string.Format("AOT cross compile ({0}/{1})", (object) (filesFinished + 1).ToString(), (object) totalFiles.ToString()), 0.95f);
    }

    public static bool CrossCompileAOTDirectoryParallel(string crossCompilerPath, BuildTarget buildTarget, CrossCompileOptions crossCompileOptions, string sourceAssembliesFolder, string targetCrossCompiledASMFolder, string additionalOptions)
    {
      sourceAssembliesFolder = Path.Combine(Directory.GetCurrentDirectory(), sourceAssembliesFolder);
      targetCrossCompiledASMFolder = Path.Combine(Directory.GetCurrentDirectory(), targetCrossCompiledASMFolder);
      int workerThreads = 1;
      int completionPortThreads = 1;
      ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
      List<MonoCrossCompile.JobCompileAOT> jobCompileAotList = new List<MonoCrossCompile.JobCompileAOT>();
      List<ManualResetEvent> events = new List<ManualResetEvent>();
      bool flag = true;
      List<string> stringList = new List<string>(((IEnumerable<string>) Directory.GetFiles(sourceAssembliesFolder)).Where<string>((Func<string, bool>) (path => Path.GetExtension(path) == ".dll")));
      int count = stringList.Count;
      int filesFinished = 0;
      MonoCrossCompile.DisplayAOTProgressBar(count, filesFinished);
      long timeout = (long) Math.Min(1800000, (count + 3) * 1000 * 30);
      foreach (string path in stringList)
      {
        string fileName = Path.GetFileName(path);
        string output = Path.Combine(targetCrossCompiledASMFolder, fileName + ".s");
        MonoCrossCompile.JobCompileAOT jobCompileAot = new MonoCrossCompile.JobCompileAOT(buildTarget, crossCompilerPath, sourceAssembliesFolder, crossCompileOptions, fileName, output, additionalOptions);
        jobCompileAotList.Add(jobCompileAot);
        events.Add(jobCompileAot.m_doneEvent);
        ThreadPool.QueueUserWorkItem(new WaitCallback(jobCompileAot.ThreadPoolCallback));
        if (events.Count >= Environment.ProcessorCount)
        {
          flag = MonoCrossCompile.WaitForBuildOfFile(events, ref timeout);
          MonoCrossCompile.DisplayAOTProgressBar(count, filesFinished);
          ++filesFinished;
          if (!flag)
            break;
        }
      }
      while (events.Count > 0)
      {
        flag = MonoCrossCompile.WaitForBuildOfFile(events, ref timeout);
        MonoCrossCompile.DisplayAOTProgressBar(count, filesFinished);
        ++filesFinished;
        if (!flag)
          break;
      }
      foreach (MonoCrossCompile.JobCompileAOT jobCompileAot in jobCompileAotList)
      {
        if (jobCompileAot.m_Exception != null)
        {
          UnityEngine.Debug.LogErrorFormat("Cross compilation job {0} failed.\n{1}", new object[2]
          {
            (object) jobCompileAot.m_input,
            (object) jobCompileAot.m_Exception
          });
          flag = false;
        }
      }
      return flag;
    }

    private static bool IsDebugableAssembly(string fname)
    {
      fname = Path.GetFileName(fname);
      return fname.StartsWith("Assembly", StringComparison.OrdinalIgnoreCase);
    }

    private static void CrossCompileAOT(BuildTarget target, string crossCompilerAbsolutePath, string assembliesAbsoluteDirectory, CrossCompileOptions crossCompileOptions, string input, string output, string additionalOptions)
    {
      string str1 = "";
      if (!MonoCrossCompile.IsDebugableAssembly(input))
      {
        crossCompileOptions &= ~CrossCompileOptions.Debugging;
        crossCompileOptions &= ~CrossCompileOptions.LoadSymbols;
      }
      bool flag1 = (crossCompileOptions & CrossCompileOptions.Debugging) != CrossCompileOptions.Dynamic;
      bool flag2 = (crossCompileOptions & CrossCompileOptions.LoadSymbols) != CrossCompileOptions.Dynamic;
      bool flag3 = flag1 || flag2;
      if (flag3)
        str1 += "--debug ";
      if (flag1)
        str1 += "--optimize=-linears ";
      string str2 = str1 + "--aot=full,asmonly,";
      if (flag3)
        str2 += "write-symbols,";
      if ((crossCompileOptions & CrossCompileOptions.Debugging) != CrossCompileOptions.Dynamic)
        str2 += "soft-debug,";
      else if (!flag3)
        str2 += "nodebug,";
      if (target != BuildTarget.iOS)
        str2 += "print-skipped,";
      if (additionalOptions != null & additionalOptions.Trim().Length > 0)
        str2 = str2 + additionalOptions.Trim() + ",";
      string fileName = Path.GetFileName(output);
      string str3 = Path.Combine(assembliesAbsoluteDirectory, fileName);
      if ((crossCompileOptions & CrossCompileOptions.FastICall) != CrossCompileOptions.Dynamic)
        str2 += "ficall,";
      if ((crossCompileOptions & CrossCompileOptions.Static) != CrossCompileOptions.Dynamic)
        str2 += "static,";
      string str4 = str2 + "outfile=\"" + fileName + "\" \"" + input + "\" ";
      Process process = new Process();
      process.StartInfo.FileName = crossCompilerAbsolutePath;
      process.StartInfo.Arguments = str4;
      process.StartInfo.EnvironmentVariables["MONO_PATH"] = assembliesAbsoluteDirectory;
      process.StartInfo.EnvironmentVariables["GAC_PATH"] = assembliesAbsoluteDirectory;
      process.StartInfo.EnvironmentVariables["GC_DONT_GC"] = "yes please";
      if ((crossCompileOptions & CrossCompileOptions.ExplicitNullChecks) != CrossCompileOptions.Dynamic)
        process.StartInfo.EnvironmentVariables["MONO_DEBUG"] = "explicit-null-checks";
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.RedirectStandardOutput = true;
      if (MonoCrossCompile.ArtifactsPath != null)
      {
        if (!Directory.Exists(MonoCrossCompile.ArtifactsPath))
          Directory.CreateDirectory(MonoCrossCompile.ArtifactsPath);
        File.AppendAllText(MonoCrossCompile.ArtifactsPath + "output.txt", process.StartInfo.FileName + "\n");
        File.AppendAllText(MonoCrossCompile.ArtifactsPath + "output.txt", process.StartInfo.Arguments + "\n");
        File.AppendAllText(MonoCrossCompile.ArtifactsPath + "output.txt", assembliesAbsoluteDirectory + "\n");
        File.AppendAllText(MonoCrossCompile.ArtifactsPath + "output.txt", str3 + "\n");
        File.AppendAllText(MonoCrossCompile.ArtifactsPath + "output.txt", input + "\n");
        File.AppendAllText(MonoCrossCompile.ArtifactsPath + "houtput.txt", fileName + "\n\n");
        File.Copy(assembliesAbsoluteDirectory + "\\" + input, MonoCrossCompile.ArtifactsPath + "\\" + input, true);
      }
      process.StartInfo.WorkingDirectory = assembliesAbsoluteDirectory;
      MonoProcessUtility.RunMonoProcess(process, "AOT cross compiler", str3);
      File.Move(str3, output);
      if ((crossCompileOptions & CrossCompileOptions.Static) != CrossCompileOptions.Dynamic)
        return;
      string str5 = Path.Combine(assembliesAbsoluteDirectory, fileName + ".def");
      if (File.Exists(str5))
        File.Move(str5, output + ".def");
    }

    private class JobCompileAOT
    {
      public ManualResetEvent m_doneEvent = new ManualResetEvent(false);
      public Exception m_Exception = (Exception) null;
      private BuildTarget m_target;
      private string m_crossCompilerAbsolutePath;
      private string m_assembliesAbsoluteDirectory;
      private CrossCompileOptions m_crossCompileOptions;
      public string m_input;
      public string m_output;
      public string m_additionalOptions;

      public JobCompileAOT(BuildTarget target, string crossCompilerAbsolutePath, string assembliesAbsoluteDirectory, CrossCompileOptions crossCompileOptions, string input, string output, string additionalOptions)
      {
        this.m_target = target;
        this.m_crossCompilerAbsolutePath = crossCompilerAbsolutePath;
        this.m_assembliesAbsoluteDirectory = assembliesAbsoluteDirectory;
        this.m_crossCompileOptions = crossCompileOptions;
        this.m_input = input;
        this.m_output = output;
        this.m_additionalOptions = additionalOptions;
      }

      public void ThreadPoolCallback(object threadContext)
      {
        try
        {
          MonoCrossCompile.CrossCompileAOT(this.m_target, this.m_crossCompilerAbsolutePath, this.m_assembliesAbsoluteDirectory, this.m_crossCompileOptions, this.m_input, this.m_output, this.m_additionalOptions);
        }
        catch (Exception ex)
        {
          this.m_Exception = ex;
        }
        this.m_doneEvent.Set();
      }
    }
  }
}
