// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.ResolveMethod
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>How assets should be resolved.</para>
  /// </summary>
  [System.Flags]
  public enum ResolveMethod
  {
    UseMine = 1,
    UseTheirs = 2,
    UseMerged = UseTheirs | UseMine, // 0x00000003
  }
}
