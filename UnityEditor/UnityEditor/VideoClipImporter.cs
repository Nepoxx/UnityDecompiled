// Decompiled with JetBrains decompiler
// Type: UnityEditor.VideoClipImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>VideoClipImporter lets you modify Video.VideoClip import settings from Editor scripts.</para>
  /// </summary>
  public sealed class VideoClipImporter : AssetImporter
  {
    /// <summary>
    ///   <para>Used in legacy import mode.  Same as MovieImport.quality.</para>
    /// </summary>
    public extern float quality { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Used in legacy import mode.  Same as MovieImport.linearTexture.</para>
    /// </summary>
    public extern bool linearColor { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>True to import a MovieTexture (deprecated), false for a VideoClip (preferred).</para>
    /// </summary>
    public extern bool useLegacyImporter { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Size in bytes of the file before importing.</para>
    /// </summary>
    public extern ulong sourceFileSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Size in bytes of the file once imported.</para>
    /// </summary>
    public extern ulong outputFileSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Number of frames in the clip.</para>
    /// </summary>
    public extern int frameCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Frame rate of the clip.</para>
    /// </summary>
    public extern double frameRate { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Whether to keep the alpha from the source into the transcoded clip.</para>
    /// </summary>
    public extern bool keepAlpha { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>True if the source file has a channel for per-pixel transparency.</para>
    /// </summary>
    public extern bool sourceHasAlpha { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Images are deinterlaced during transcode.  This tells the importer how to interpret fields in the source, if any.</para>
    /// </summary>
    public extern VideoDeinterlaceMode deinterlaceMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Apply a vertical flip during import.</para>
    /// </summary>
    public extern bool flipVertical { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Apply a horizontal flip during import.</para>
    /// </summary>
    public extern bool flipHorizontal { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Import audio tracks from source file.</para>
    /// </summary>
    public extern bool importAudio { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Default values for the platform-specific import settings.</para>
    /// </summary>
    public VideoImporterTargetSettings defaultTargetSettings
    {
      get
      {
        return this.GetTargetSettings(VideoClipImporter.defaultTargetName);
      }
      set
      {
        this.SetTargetSettings(VideoClipImporter.defaultTargetName, value);
      }
    }

    /// <summary>
    ///   <para>Returns the platform-specific import settings for the specified platform.</para>
    /// </summary>
    /// <param name="platform">Platform name.</param>
    /// <returns>
    ///   <para>The platform-specific import settings.  Throws an exception if the platform is unknown.</para>
    /// </returns>
    public VideoImporterTargetSettings GetTargetSettings(string platform)
    {
      BuildTargetGroup targetGroupByName = BuildPipeline.GetBuildTargetGroupByName(platform);
      if (!platform.Equals(VideoClipImporter.defaultTargetName, StringComparison.OrdinalIgnoreCase) && targetGroupByName == BuildTargetGroup.Unknown)
        throw new ArgumentException("Unknown platform passed to AudioImporter.GetOverrideSampleSettings (" + platform + "), please use one of 'Default', 'Web', 'Standalone', 'iOS', 'Android', 'WebGL', 'PS4', 'PSP2', 'PSM', 'XBox360', 'XboxOne', 'WP8', or 'WSA'");
      return this.Internal_GetTargetSettings(targetGroupByName);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern VideoImporterTargetSettings Internal_GetTargetSettings(BuildTargetGroup group);

    /// <summary>
    ///   <para>Sets the platform-specific import settings for the specified platform.</para>
    /// </summary>
    /// <param name="platform">Platform name.</param>
    /// <param name="settings">The new platform-specific import settings.</param>
    public void SetTargetSettings(string platform, VideoImporterTargetSettings settings)
    {
      BuildTargetGroup targetGroupByName = BuildPipeline.GetBuildTargetGroupByName(platform);
      if (!platform.Equals(VideoClipImporter.defaultTargetName, StringComparison.OrdinalIgnoreCase) && targetGroupByName == BuildTargetGroup.Unknown)
        throw new ArgumentException("Unknown platform passed to AudioImporter.GetOverrideSampleSettings (" + platform + "), please use one of 'Default', 'Web', 'Standalone', 'iOS', 'Android', 'WebGL', 'PS4', 'PSP2', 'PSM', 'XBox360', 'XboxOne', 'WP8', or 'WSA'");
      this.Internal_SetTargetSettings(targetGroupByName, settings);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void Internal_SetTargetSettings(BuildTargetGroup group, VideoImporterTargetSettings settings);

    /// <summary>
    ///   <para>Clear the platform-specific import settings for the specified platform, causing them to go back to the default settings.</para>
    /// </summary>
    /// <param name="platform">Platform name.</param>
    public void ClearTargetSettings(string platform)
    {
      if (platform.Equals(VideoClipImporter.defaultTargetName, StringComparison.OrdinalIgnoreCase))
        throw new ArgumentException("Cannot clear the Default VideoClipTargetSettings.");
      BuildTargetGroup targetGroupByName = BuildPipeline.GetBuildTargetGroupByName(platform);
      if (targetGroupByName == BuildTargetGroup.Unknown)
        throw new ArgumentException("Unknown platform passed to AudioImporter.GetOverrideSampleSettings (" + platform + "), please use one of 'Web', 'Standalone', 'iOS', 'Android', 'WebGL', 'PS4', 'PSP2', 'PSM', 'XBox360', 'XboxOne', 'WP8', or 'WSA'");
      this.Internal_ClearTargetSettings(targetGroupByName);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void Internal_ClearTargetSettings(BuildTargetGroup group);

    /// <summary>
    ///   <para>Starts preview playback.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void PlayPreview();

    /// <summary>
    ///   <para>Stops preview playback.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void StopPreview();

    /// <summary>
    ///   <para>Whether the preview is currently playing.</para>
    /// </summary>
    public extern bool isPlayingPreview { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///         <para>Returns a texture with the transcoded clip's current frame.
    /// Returns frame 0 when not playing, and frame at current time when playing.</para>
    ///       </summary>
    /// <returns>
    ///   <para>Texture containing the current frame.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Texture GetPreviewTexture();

    internal static extern string defaultTargetName { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool EqualsDefaultTargetSettings(VideoImporterTargetSettings settings);

    /// <summary>
    ///   <para>Get the full name of the resize operation for the specified resize mode.</para>
    /// </summary>
    /// <param name="mode">Mode for which the width is queried.</param>
    /// <returns>
    ///   <para>Name for the specified resize mode.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetResizeModeName(VideoResizeMode mode);

    /// <summary>
    ///   <para>Get the resulting width of the resize operation for the specified resize mode.</para>
    /// </summary>
    /// <param name="mode">Mode for which the width is queried.</param>
    /// <returns>
    ///   <para>Width for the specified resize mode.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int GetResizeWidth(VideoResizeMode mode);

    /// <summary>
    ///   <para>Get the resulting height of the resize operation for the specified resize mode.</para>
    /// </summary>
    /// <param name="mode">Mode for which the height is queried.</param>
    /// <returns>
    ///   <para>Height for the specified resize mode.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int GetResizeHeight(VideoResizeMode mode);

    /// <summary>
    ///   <para>Number of audio tracks in the source file.</para>
    /// </summary>
    public extern ushort sourceAudioTrackCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Number of audio channels in the specified source track.</para>
    /// </summary>
    /// <param name="audioTrackIdx">Index of the audio track to query.</param>
    /// <returns>
    ///   <para>Number of channels.</para>
    /// </returns>
    public ushort GetSourceAudioChannelCount(ushort audioTrackIdx)
    {
      return VideoClipImporter.INTERNAL_CALL_GetSourceAudioChannelCount(this, audioTrackIdx);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern ushort INTERNAL_CALL_GetSourceAudioChannelCount(VideoClipImporter self, ushort audioTrackIdx);

    /// <summary>
    ///   <para>Sample rate of the specified audio track.</para>
    /// </summary>
    /// <param name="audioTrackIdx">Index of the audio track to query.</param>
    /// <returns>
    ///   <para>Sample rate in Hertz.</para>
    /// </returns>
    public uint GetSourceAudioSampleRate(ushort audioTrackIdx)
    {
      return VideoClipImporter.INTERNAL_CALL_GetSourceAudioSampleRate(this, audioTrackIdx);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern uint INTERNAL_CALL_GetSourceAudioSampleRate(VideoClipImporter self, ushort audioTrackIdx);

    /// <summary>
    ///   <para>Numerator of the pixel aspect ratio (num:den).</para>
    /// </summary>
    public extern int pixelAspectRatioNumerator { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Denominator of the pixel aspect ratio (num:den).</para>
    /// </summary>
    public extern int pixelAspectRatioDenominator { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///         <para>Returns true if transcoding was skipped during import, false otherwise. (Read Only)
    /// 
    /// When VideoImporterTargetSettings.enableTranscoding is set to true, the resulting transcoding operation done at import time may be quite long, up to many hours depending on source resolution and content duration. An option to skip this process is offered in the asset import progress bar. When skipped, the transcoding instead provides a non-transcoded verision of the asset. However, the importer settings stay intact so this property can be inspected to detect the incoherence with the generated artifact.
    /// 
    /// Re-importing without stopping the transcode process, or with transcode turned off, causes this property to become false.</para>
    ///       </summary>
    public extern bool transcodeSkipped { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
