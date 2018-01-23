// Decompiled with JetBrains decompiler
// Type: UnityEditor.Media.AudioTrackAttributes
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Media
{
  /// <summary>
  ///   <para>Descriptor for audio track format.</para>
  /// </summary>
  public struct AudioTrackAttributes
  {
    /// <summary>
    ///   <para>Audio sampling rate.</para>
    /// </summary>
    public MediaRational sampleRate;
    /// <summary>
    ///   <para>Number of channels.</para>
    /// </summary>
    public ushort channelCount;
    /// <summary>
    ///   <para>Dialogue language, if applicable.  Can be empty.</para>
    /// </summary>
    public string language;
  }
}
