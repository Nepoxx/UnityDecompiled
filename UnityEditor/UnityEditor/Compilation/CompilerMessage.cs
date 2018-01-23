// Decompiled with JetBrains decompiler
// Type: UnityEditor.Compilation.CompilerMessage
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Compilation
{
  /// <summary>
  ///   <para>Compiler Message.</para>
  /// </summary>
  public struct CompilerMessage
  {
    /// <summary>
    ///   <para>Compiler message.</para>
    /// </summary>
    public string message;
    /// <summary>
    ///   <para>File for the message.</para>
    /// </summary>
    public string file;
    /// <summary>
    ///   <para>File line for the message.</para>
    /// </summary>
    public int line;
    /// <summary>
    ///   <para>Line column for the message.</para>
    /// </summary>
    public int column;
    /// <summary>
    ///   <para>Message type.</para>
    /// </summary>
    public CompilerMessageType type;
  }
}
