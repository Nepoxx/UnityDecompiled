// Decompiled with JetBrains decompiler
// Type: UnityEditor.Utils.Program
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace UnityEditor.Utils
{
  internal class Program : IDisposable
  {
    private ProcessOutputStreamReader _stdout;
    private ProcessOutputStreamReader _stderr;
    private Stream _stdin;
    public Process _process;

    protected Program()
    {
      this._process = new Process();
    }

    public Program(ProcessStartInfo si)
      : this()
    {
      this._process.StartInfo = si;
    }

    public void Start()
    {
      this.Start((EventHandler) null);
    }

    public void Start(EventHandler exitCallback)
    {
      if (exitCallback != null)
      {
        this._process.EnableRaisingEvents = true;
        this._process.Exited += exitCallback;
      }
      this._process.StartInfo.RedirectStandardInput = true;
      this._process.StartInfo.RedirectStandardError = true;
      this._process.StartInfo.RedirectStandardOutput = true;
      this._process.StartInfo.UseShellExecute = false;
      this._process.Start();
      this._stdout = new ProcessOutputStreamReader(this._process, this._process.StandardOutput);
      this._stderr = new ProcessOutputStreamReader(this._process, this._process.StandardError);
      this._stdin = this._process.StandardInput.BaseStream;
    }

    public ProcessStartInfo GetProcessStartInfo()
    {
      return this._process.StartInfo;
    }

    public void LogProcessStartInfo()
    {
      if (this._process != null)
        Program.LogProcessStartInfo(this._process.StartInfo);
      else
        Console.WriteLine("Failed to retrieve process startInfo");
    }

    private static void LogProcessStartInfo(ProcessStartInfo si)
    {
      Console.WriteLine("Filename: " + si.FileName);
      Console.WriteLine("Arguments: " + si.Arguments);
      IEnumerator enumerator = si.EnvironmentVariables.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          DictionaryEntry current = (DictionaryEntry) enumerator.Current;
          if (current.Key.ToString().StartsWith("MONO"))
            Console.WriteLine("{0}: {1}", current.Key, current.Value);
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      int startIndex = si.Arguments.IndexOf("Temp/UnityTempFile");
      Console.WriteLine("index: " + (object) startIndex);
      if (startIndex <= 0)
        return;
      string path = si.Arguments.Substring(startIndex);
      Console.WriteLine("Responsefile: " + path + " Contents: ");
      Console.WriteLine(File.ReadAllText(path));
    }

    public string GetAllOutput()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("stdout:");
      foreach (string str in this.GetStandardOutput())
        stringBuilder.AppendLine(str);
      stringBuilder.AppendLine("stderr:");
      foreach (string str in this.GetErrorOutput())
        stringBuilder.AppendLine(str);
      return stringBuilder.ToString();
    }

    public bool HasExited
    {
      get
      {
        if (this._process == null)
          throw new InvalidOperationException("You cannot call HasExited before calling Start");
        try
        {
          return this._process.HasExited;
        }
        catch (InvalidOperationException ex)
        {
          return true;
        }
      }
    }

    public int ExitCode
    {
      get
      {
        return this._process.ExitCode;
      }
    }

    public int Id
    {
      get
      {
        return this._process.Id;
      }
    }

    public void Dispose()
    {
      this.Kill();
      this._process.Dispose();
    }

    public void Kill()
    {
      if (this.HasExited)
        return;
      this._process.Kill();
      this._process.WaitForExit();
    }

    public Stream GetStandardInput()
    {
      return this._stdin;
    }

    public string[] GetStandardOutput()
    {
      return this._stdout.GetOutput();
    }

    public string GetStandardOutputAsString()
    {
      return Program.GetOutputAsString(this.GetStandardOutput());
    }

    public string[] GetErrorOutput()
    {
      return this._stderr.GetOutput();
    }

    public string GetErrorOutputAsString()
    {
      return Program.GetOutputAsString(this.GetErrorOutput());
    }

    private static string GetOutputAsString(string[] output)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str in output)
        stringBuilder.AppendLine(str);
      return stringBuilder.ToString();
    }

    public void WaitForExit()
    {
      this._process.WaitForExit();
    }

    public bool WaitForExit(int milliseconds)
    {
      return this._process.WaitForExit(milliseconds);
    }
  }
}
