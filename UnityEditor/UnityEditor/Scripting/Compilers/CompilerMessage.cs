// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.CompilerMessage
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Scripting.Compilers
{
  internal struct CompilerMessage
  {
    public string message;
    public string file;
    public int line;
    public int column;
    public CompilerMessageType type;
    public NormalizedCompilerStatus normalizedStatus;
  }
}
