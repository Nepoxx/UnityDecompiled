// Decompiled with JetBrains decompiler
// Type: UnityEditor.LogEntry
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.InteropServices;

namespace UnityEditor
{
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class LogEntry
  {
    public string condition;
    public int errorNum;
    public string file;
    public int line;
    public int mode;
    public int instanceID;
    public int identifier;
    public int isWorldPlaying;
  }
}
