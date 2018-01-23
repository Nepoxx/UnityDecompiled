// Decompiled with JetBrains decompiler
// Type: UnityEditor.Compilation.AssemblyDefinitionException
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor.Compilation
{
  /// <summary>
  ///   <para>An exception throw for Assembly Definition Files errors.</para>
  /// </summary>
  public class AssemblyDefinitionException : Exception
  {
    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="filePaths">File paths for assembly definition files.</param>
    public AssemblyDefinitionException(string message, params string[] filePaths)
      : base(message)
    {
      this.filePaths = filePaths;
    }

    /// <summary>
    ///   <para>File paths of the assembly definition files that caused the exception.</para>
    /// </summary>
    public string[] filePaths { get; private set; }
  }
}
