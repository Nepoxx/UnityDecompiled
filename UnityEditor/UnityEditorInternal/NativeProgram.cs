// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.NativeProgram
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Diagnostics;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class NativeProgram : Program
  {
    public NativeProgram(string executable, string arguments)
    {
      this._process.StartInfo = new ProcessStartInfo()
      {
        Arguments = arguments,
        CreateNoWindow = true,
        FileName = executable,
        RedirectStandardError = true,
        RedirectStandardOutput = true,
        WorkingDirectory = Application.dataPath + "/..",
        UseShellExecute = false
      };
    }
  }
}
