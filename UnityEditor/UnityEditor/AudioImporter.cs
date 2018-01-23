// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioImporter
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
  ///   <para>Audio importer lets you modify AudioClip import settings from editor scripts.</para>
  /// </summary>
  public sealed class AudioImporter : AssetImporter
  {
    /// <summary>
    ///   <para>The default sample settings for the AudioClip importer.</para>
    /// </summary>
    public extern AudioImporterSampleSettings defaultSampleSettings { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns whether a given build target has its sample settings currently overridden.</para>
    /// </summary>
    /// <param name="platform">The platform to query if this AudioImporter has an override for.</param>
    /// <returns>
    ///   <para>Returns true if the platform is currently overriden in this AudioImporter.</para>
    /// </returns>
    public bool ContainsSampleSettingsOverride(string platform)
    {
      BuildTargetGroup targetGroupByName = BuildPipeline.GetBuildTargetGroupByName(platform);
      if (targetGroupByName != BuildTargetGroup.Unknown)
        return this.Internal_ContainsSampleSettingsOverride(targetGroupByName);
      Debug.LogError((object) ("Unknown platform passed to AudioImporter.ContainsSampleSettingsOverride (" + platform + "), please use one of 'Web', 'Standalone', 'iOS', 'Android', 'WebGL', 'PS4', 'PSP2', 'PSM', 'XboxOne' or 'WSA'"));
      return false;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool Internal_ContainsSampleSettingsOverride(BuildTargetGroup platformGroup);

    /// <summary>
    ///   <para>Return the current override settings for the given platform.</para>
    /// </summary>
    /// <param name="platform">The platform to get the override settings for.</param>
    /// <returns>
    ///   <para>The override sample settings for the given platform.</para>
    /// </returns>
    public AudioImporterSampleSettings GetOverrideSampleSettings(string platform)
    {
      BuildTargetGroup targetGroupByName = BuildPipeline.GetBuildTargetGroupByName(platform);
      if (targetGroupByName != BuildTargetGroup.Unknown)
        return this.Internal_GetOverrideSampleSettings(targetGroupByName);
      Debug.LogError((object) ("Unknown platform passed to AudioImporter.GetOverrideSampleSettings (" + platform + "), please use one of 'Web', 'Standalone', 'iOS', 'Android', 'WebGL', 'PS4', 'PSP2', 'PSM', 'XboxOne' or 'WSA'"));
      return this.defaultSampleSettings;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern AudioImporterSampleSettings Internal_GetOverrideSampleSettings(BuildTargetGroup platformGroup);

    /// <summary>
    ///   <para>Sets the override sample settings for the given platform.</para>
    /// </summary>
    /// <param name="platform">The platform which will have the sample settings overridden.</param>
    /// <param name="settings">The override settings for the given platform.</param>
    /// <returns>
    ///   <para>Returns true if the settings were successfully overriden. Some setting overrides are not possible for the given platform, in which case false is returned and the settings are not registered.</para>
    /// </returns>
    public bool SetOverrideSampleSettings(string platform, AudioImporterSampleSettings settings)
    {
      BuildTargetGroup targetGroupByName = BuildPipeline.GetBuildTargetGroupByName(platform);
      if (targetGroupByName != BuildTargetGroup.Unknown)
        return this.Internal_SetOverrideSampleSettings(targetGroupByName, settings);
      Debug.LogError((object) ("Unknown platform passed to AudioImporter.SetOverrideSampleSettings (" + platform + "), please use one of 'Web', 'Standalone', 'iOS', 'Android', 'WebGL', 'PS4', 'PSP2', 'PSM', 'XboxOne' or 'WSA'"));
      return false;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool Internal_SetOverrideSampleSettings(BuildTargetGroup platformGroup, AudioImporterSampleSettings settings);

    /// <summary>
    ///   <para>Clears the sample settings override for the given platform.</para>
    /// </summary>
    /// <param name="platform">The platform to clear the overrides for.</param>
    /// <returns>
    ///   <para>Returns true if any overrides were actually cleared.</para>
    /// </returns>
    public bool ClearSampleSettingOverride(string platform)
    {
      BuildTargetGroup targetGroupByName = BuildPipeline.GetBuildTargetGroupByName(platform);
      if (targetGroupByName != BuildTargetGroup.Unknown)
        return this.Internal_ClearSampleSettingOverride(targetGroupByName);
      Debug.LogError((object) ("Unknown platform passed to AudioImporter.ClearSampleSettingOverride (" + platform + "), please use one of 'Web', 'Standalone', 'iOS', 'Android', 'WebGL', 'PS4', 'PSP2', 'PSM', 'XboxOne' or 'WSA'"));
      return false;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool Internal_ClearSampleSettingOverride(BuildTargetGroup platform);

    /// <summary>
    ///   <para>Force this clip to mono?</para>
    /// </summary>
    public extern bool forceToMono { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>When this flag is set, the audio clip will be treated as being ambisonic.</para>
    /// </summary>
    public bool ambisonic
    {
      get
      {
        return this.Internal_GetAmbisonic();
      }
      set
      {
        this.Internal_SetAmbisonic(value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetAmbisonic(bool flag);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool Internal_GetAmbisonic();

    /// <summary>
    ///   <para>Corresponding to the "Load In Background" flag in the AudioClip inspector, when this flag is set, the loading of the clip will happen delayed without blocking the main thread.</para>
    /// </summary>
    public bool loadInBackground
    {
      get
      {
        return this.Internal_GetLoadInBackground();
      }
      set
      {
        this.Internal_SetLoadInBackground(value);
      }
    }

    /// <summary>
    ///   <para>Preloads audio data of the clip when the clip asset is loaded. When this flag is off, scripts have to call AudioClip.LoadAudioData() to load the data before the clip can be played. Properties like length, channels and format are available before the audio data has been loaded.</para>
    /// </summary>
    public bool preloadAudioData
    {
      get
      {
        return this.Internal_GetPreloadAudioData();
      }
      set
      {
        this.Internal_SetPreloadAudioData(value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetLoadInBackground(bool flag);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool Internal_GetLoadInBackground();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetPreloadAudioData(bool flag);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool Internal_GetPreloadAudioData();

    [Obsolete("Setting and getting the compression format is not used anymore (use compressionFormat in defaultSampleSettings instead). Source audio file is assumed to be PCM Wav.")]
    private AudioImporterFormat format
    {
      get
      {
        return this.defaultSampleSettings.compressionFormat != AudioCompressionFormat.PCM ? AudioImporterFormat.Compressed : AudioImporterFormat.Native;
      }
      set
      {
        AudioImporterSampleSettings defaultSampleSettings = this.defaultSampleSettings;
        defaultSampleSettings.compressionFormat = value != AudioImporterFormat.Native ? AudioCompressionFormat.Vorbis : AudioCompressionFormat.PCM;
        this.defaultSampleSettings = defaultSampleSettings;
      }
    }

    [Obsolete("Setting and getting import channels is not used anymore (use forceToMono instead)", true)]
    public AudioImporterChannels channels
    {
      get
      {
        return AudioImporterChannels.Automatic;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Compression bitrate.</para>
    /// </summary>
    [Obsolete("AudioImporter.compressionBitrate is no longer supported", true)]
    public extern int compressionBitrate { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("AudioImporter.loopable is no longer supported. All audio assets encoded by Unity are by default loopable.")]
    public extern bool loopable { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("AudioImporter.hardware is no longer supported. All mixing of audio is done by software and only some platforms use hardware acceleration to perform decoding.")]
    public extern bool hardware { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("Setting/Getting decompressOnLoad is deprecated. Use AudioImporterSampleSettings.loadType instead.")]
    private bool decompressOnLoad
    {
      get
      {
        return this.defaultSampleSettings.loadType == AudioClipLoadType.DecompressOnLoad;
      }
      set
      {
        AudioImporterSampleSettings defaultSampleSettings = this.defaultSampleSettings;
        defaultSampleSettings.loadType = !value ? AudioClipLoadType.CompressedInMemory : AudioClipLoadType.DecompressOnLoad;
        this.defaultSampleSettings = defaultSampleSettings;
      }
    }

    [Obsolete("AudioImporter.quality is no longer supported. Use AudioImporterSampleSettings.")]
    private float quality
    {
      get
      {
        return this.defaultSampleSettings.quality;
      }
      set
      {
        AudioImporterSampleSettings defaultSampleSettings = this.defaultSampleSettings;
        defaultSampleSettings.quality = value;
        this.defaultSampleSettings = defaultSampleSettings;
      }
    }

    [Obsolete("AudioImporter.threeD is no longer supported")]
    public extern bool threeD { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("AudioImporter.updateOrigData is deprecated.", true)]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void updateOrigData();

    [Obsolete("AudioImporter.durationMS is deprecated.", true)]
    internal extern int durationMS { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("AudioImporter.frequency is deprecated.", true)]
    internal extern int frequency { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("AudioImporter.origChannelCount is deprecated.", true)]
    internal extern int origChannelCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("AudioImporter.origIsCompressible is deprecated.", true)]
    internal extern bool origIsCompressible { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("AudioImporter.origIsMonoForcable is deprecated.", true)]
    internal extern bool origIsMonoForcable { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("AudioImporter.minBitrate is deprecated.", true)]
    internal int minBitrate(AudioType type)
    {
      return 0;
    }

    [Obsolete("AudioImporter.maxBitrate is deprecated.", true)]
    internal int maxBitrate(AudioType type)
    {
      return 0;
    }

    [Obsolete("AudioImporter.defaultBitrate is deprecated.", true)]
    internal extern int defaultBitrate { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("AudioImporter.origType is deprecated.", true)]
    internal AudioType origType
    {
      get
      {
        return AudioType.UNKNOWN;
      }
    }

    [Obsolete("AudioImporter.origFileSize is deprecated.", true)]
    internal extern int origFileSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal extern int origSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal extern int compSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
