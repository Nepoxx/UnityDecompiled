// Decompiled with JetBrains decompiler
// Type: UnityEditor.Build.BuildFailedException
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine.Scripting;

namespace UnityEditor.Build
{
  /// <summary>
  ///   <para>An exception class that represents a failed build.</para>
  /// </summary>
  [RequiredByNativeCode]
  public class BuildFailedException : Exception
  {
    /// <summary>
    ///   <para>Constructs a BuildFailedException object.</para>
    /// </summary>
    /// <param name="message">A string of text describing the error that caused the build to fail.</param>
    /// <param name="innerException">The exception that caused the build to fail.</param>
    public BuildFailedException(string message)
      : base(message)
    {
    }

    /// <summary>
    ///   <para>Constructs a BuildFailedException object.</para>
    /// </summary>
    /// <param name="message">A string of text describing the error that caused the build to fail.</param>
    /// <param name="innerException">The exception that caused the build to fail.</param>
    public BuildFailedException(Exception innerException)
      : base((string) null, innerException)
    {
    }

    [RequiredByNativeCode]
    private Exception BuildFailedException_GetInnerException()
    {
      return this.InnerException;
    }
  }
}
