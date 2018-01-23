// Decompiled with JetBrains decompiler
// Type: UnityEditor.Media.VideoTrackAttributes
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Media
{
  /// <summary>
  ///   <para>Descriptor for audio track format.</para>
  /// </summary>
  public struct VideoTrackAttributes
  {
    /// <summary>
    ///   <para>Frames per second.</para>
    /// </summary>
    public MediaRational frameRate;
    /// <summary>
    ///   <para>Image width in pixels.</para>
    /// </summary>
    public uint width;
    /// <summary>
    ///   <para>Image height in pixels.</para>
    /// </summary>
    public uint height;
    /// <summary>
    ///   <para>True if the track is to include the alpha channel found in the texture passed to AddFrame. False otherwise.</para>
    /// </summary>
    public bool includeAlpha;
  }
}
