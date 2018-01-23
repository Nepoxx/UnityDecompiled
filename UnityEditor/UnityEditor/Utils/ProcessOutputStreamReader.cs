// Decompiled with JetBrains decompiler
// Type: UnityEditor.Utils.ProcessOutputStreamReader
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace UnityEditor.Utils
{
  internal class ProcessOutputStreamReader
  {
    private readonly Func<bool> hostProcessExited;
    private readonly StreamReader stream;
    internal List<string> lines;
    private Thread thread;

    internal ProcessOutputStreamReader(Process p, StreamReader stream)
      : this((Func<bool>) (() => p.HasExited), stream)
    {
    }

    internal ProcessOutputStreamReader(Func<bool> hostProcessExited, StreamReader stream)
    {
      this.hostProcessExited = hostProcessExited;
      this.stream = stream;
      this.lines = new List<string>();
      this.thread = new Thread(new ThreadStart(this.ThreadFunc));
      this.thread.Start();
    }

    private void ThreadFunc()
    {
      if (this.hostProcessExited())
        return;
      try
      {
        while (this.stream.BaseStream != null)
        {
          string str = this.stream.ReadLine();
          if (str == null)
            break;
          lock ((object) this.lines)
            this.lines.Add(str);
        }
      }
      catch (ObjectDisposedException ex)
      {
        lock ((object) this.lines)
          this.lines.Add("Could not read output because an ObjectDisposedException was thrown.");
      }
    }

    internal string[] GetOutput()
    {
      if (this.hostProcessExited())
        this.thread.Join();
      lock ((object) this.lines)
        return this.lines.ToArray();
    }
  }
}
