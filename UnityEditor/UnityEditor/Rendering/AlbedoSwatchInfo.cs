// Decompiled with JetBrains decompiler
// Type: UnityEditor.Rendering.AlbedoSwatchInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Rendering
{
  /// <summary>
  ///   <para>Contains the custom albedo swatch data.</para>
  /// </summary>
  public struct AlbedoSwatchInfo
  {
    /// <summary>
    ///   <para>Name of the albedo swatch to show in the physically based renderer validator user interface.</para>
    /// </summary>
    public string name;
    /// <summary>
    ///   <para>Color of the albedo swatch that is shown in the physically based rendering validator user interface.</para>
    /// </summary>
    public Color color;
    /// <summary>
    ///   <para>The minimum luminance value used to validate the albedo for the physically based rendering albedo validator.</para>
    /// </summary>
    public float minLuminance;
    /// <summary>
    ///   <para>The maximum luminance value used to validate the albedo for the physically based rendering albedo validator.</para>
    /// </summary>
    public float maxLuminance;
  }
}
