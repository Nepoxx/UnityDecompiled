// Decompiled with JetBrains decompiler
// Type: UnityEditor.MonoProcessRunner
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace UnityEditor
{
  internal class MonoProcessRunner
  {
    public StringBuilder Output = new StringBuilder("");
    public StringBuilder Error = new StringBuilder("");

    public bool Run(Process process)
    {
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError = true;
      Thread thread1 = new Thread(new ParameterizedThreadStart(this.ReadOutput));
      Thread thread2 = new Thread(new ParameterizedThreadStart(this.ReadErrors));
      process.Start();
      thread1.Start((object) process);
      thread2.Start((object) process);
      bool flag = process.WaitForExit(600000);
      DateTime now = DateTime.Now;
      while ((thread1.IsAlive || thread2.IsAlive) && (DateTime.Now - now).TotalMilliseconds < 5.0)
        Thread.Sleep(0);
      if (thread1.IsAlive)
        thread1.Abort();
      if (thread2.IsAlive)
        thread2.Abort();
      thread1.Join();
      thread2.Join();
      return flag;
    }

    private void ReadOutput(object process)
    {
      Process process1 = process as Process;
      try
      {
        using (StreamReader standardOutput = process1.StandardOutput)
          this.Output.Append(standardOutput.ReadToEnd());
      }
      catch (ThreadAbortException ex)
      {
      }
    }

    private void ReadErrors(object process)
    {
      Process process1 = process as Process;
      try
      {
        using (StreamReader standardError = process1.StandardError)
          this.Error.Append(standardError.ReadToEnd());
      }
      catch (ThreadAbortException ex)
      {
      }
    }
  }
}
