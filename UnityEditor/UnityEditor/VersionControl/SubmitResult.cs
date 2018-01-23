// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.SubmitResult
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>The status of an operation returned by the VCS.</para>
  /// </summary>
  public enum SubmitResult
  {
    OK = 1,
    Error = 2,
    ConflictingFiles = 4,
    UnaddedFiles = 8,
  }
}
