// Decompiled with JetBrains decompiler
// Type: UnityEditor.DragAndDropVisualMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Visual indication mode for Drag &amp; Drop operation.</para>
  /// </summary>
  public enum DragAndDropVisualMode
  {
    None = 0,
    Copy = 1,
    Link = 2,
    Generic = 4,
    Move = 16, // 0x00000010
    Rejected = 32, // 0x00000020
  }
}
