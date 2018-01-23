// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridPalette
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>GridPalette stores settings for Palette assets when shown in the Palette window.</para>
  /// </summary>
  public class GridPalette : ScriptableObject
  {
    /// <summary>
    ///   <para>Determines the sizing of cells for a Palette.</para>
    /// </summary>
    [SerializeField]
    public GridPalette.CellSizing cellSizing;

    /// <summary>
    ///   <para>Controls the sizing of cells for a Palette.</para>
    /// </summary>
    public enum CellSizing
    {
      Automatic = 0,
      Manual = 100, // 0x00000064
    }
  }
}
