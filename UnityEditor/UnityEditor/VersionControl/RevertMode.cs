// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.RevertMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>Defines the behaviour of the version control revert methods.</para>
  /// </summary>
  [System.Flags]
  public enum RevertMode
  {
    Normal = 0,
    Unchanged = 1,
    KeepModifications = 2,
  }
}
