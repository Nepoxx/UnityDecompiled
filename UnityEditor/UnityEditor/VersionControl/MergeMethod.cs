// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.MergeMethod
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>Which method to use when merging.</para>
  /// </summary>
  [System.Flags]
  public enum MergeMethod
  {
    MergeNone = 0,
    MergeAll = 1,
    [Obsolete("This member is no longer supported (UnityUpgradable) -> MergeNone", true)] MergeNonConflicting = 2,
  }
}
