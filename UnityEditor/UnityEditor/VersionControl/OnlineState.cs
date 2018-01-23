// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.OnlineState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>Represent the connection state of the version control provider.</para>
  /// </summary>
  [System.Flags]
  public enum OnlineState
  {
    Updating = 0,
    Online = 1,
    Offline = 2,
  }
}
