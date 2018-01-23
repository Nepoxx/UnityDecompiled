// Decompiled with JetBrains decompiler
// Type: UnityEngine.AudioSettings
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Controls the global audio settings from script.</para>
  /// </summary>
  public sealed class AudioSettings
  {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("AudioSettings.driverCaps is obsolete. Use driverCapabilities instead (UnityUpgradable) -> driverCapabilities", true)]
    public static AudioSpeakerMode driverCaps
    {
      get
      {
        return AudioSettings.driverCapabilities;
      }
    }

    /// <summary>
    ///   <para>Returns the speaker mode capability of the current audio driver. (Read Only)</para>
    /// </summary>
    public static extern AudioSpeakerMode driverCapabilities { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Gets the current speaker mode. Default is 2 channel stereo.</para>
    /// </summary>
    public static extern AudioSpeakerMode speakerMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern int profilerCaptureFlags { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the current time of the audio system.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public static extern double dspTime { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get the mixer's current output rate.</para>
    /// </summary>
    public static extern int outputSampleRate { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void GetDSPBufferSize(out int bufferLength, out int numBuffers);

    [Obsolete("AudioSettings.SetDSPBufferSize is deprecated and has been replaced by audio project settings and the AudioSettings.GetConfiguration/AudioSettings.Reset API.")]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetDSPBufferSize(int bufferLength, int numBuffers);

    internal static extern bool editingInPlaymode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns an array with the names of all the available spatializer plugins.</para>
    /// </summary>
    /// <returns>
    ///   <para>An array of spatializer names.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetSpatializerPluginNames();

    /// <summary>
    ///   <para>Returns the name of the spatializer selected on the currently-running platform.</para>
    /// </summary>
    /// <returns>
    ///   <para>The spatializer plugin name.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetSpatializerPluginName();

    /// <summary>
    ///   <para>Sets the spatializer plugin for all platform groups. If a null or empty string is passed in, the existing spatializer plugin will be cleared.</para>
    /// </summary>
    /// <param name="pluginName">The spatializer plugin name.</param>
    public static void SetSpatializerPluginName(string pluginName)
    {
      if (!AudioSettings.SetSpatializerPluginName_Internal(pluginName))
        throw new ArgumentException("Invalid spatializer plugin name");
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool SetSpatializerPluginName_Internal(string pluginName);

    /// <summary>
    ///   <para>Returns the current configuration of the audio device and system. The values in the struct may then be modified and reapplied via AudioSettings.Reset.</para>
    /// </summary>
    /// <returns>
    ///   <para>The new configuration to be applied.</para>
    /// </returns>
    public static AudioConfiguration GetConfiguration()
    {
      AudioConfiguration audioConfiguration;
      AudioSettings.INTERNAL_CALL_GetConfiguration(out audioConfiguration);
      return audioConfiguration;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetConfiguration(out AudioConfiguration value);

    /// <summary>
    ///   <para>Performs a change of the device configuration. In response to this the AudioSettings.OnAudioConfigurationChanged delegate is invoked with the argument deviceWasChanged=false. It cannot be guaranteed that the exact settings specified can be used, but the an attempt is made to use the closest match supported by the system.</para>
    /// </summary>
    /// <param name="config">The new configuration to be used.</param>
    /// <returns>
    ///   <para>True if all settings could be successfully applied.</para>
    /// </returns>
    public static bool Reset(AudioConfiguration config)
    {
      return AudioSettings.INTERNAL_CALL_Reset(ref config);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_Reset(ref AudioConfiguration config);

    public static event AudioSettings.AudioConfigurationChangeHandler OnAudioConfigurationChanged;

    [RequiredByNativeCode]
    internal static void InvokeOnAudioConfigurationChanged(bool deviceWasChanged)
    {
      // ISSUE: reference to a compiler-generated field
      if (AudioSettings.OnAudioConfigurationChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      AudioSettings.OnAudioConfigurationChanged(deviceWasChanged);
    }

    [RequiredByNativeCode]
    internal static void InvokeOnAudioManagerUpdate()
    {
      AudioExtensionManager.Update();
    }

    [RequiredByNativeCode]
    internal static void InvokeOnAudioSourcePlay(AudioSource source)
    {
      AudioSourceExtension extension1 = AudioExtensionManager.AddSpatializerExtension(source);
      if ((Object) extension1 != (Object) null)
        AudioExtensionManager.GetReadyToPlay(extension1);
      if (!((Object) source.clip != (Object) null) || !source.clip.ambisonic)
        return;
      AudioSourceExtension extension2 = AudioExtensionManager.AddAmbisonicDecoderExtension(source);
      if ((Object) extension2 != (Object) null)
        AudioExtensionManager.GetReadyToPlay(extension2);
    }

    internal static extern bool unityAudioDisabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetAmbisonicDecoderPluginName();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetAmbisonicDecoderPluginName(string name);

    /// <summary>
    ///   <para>A delegate called whenever the global audio settings are changed, either by AudioSettings.Reset or by an external device change such as the OS control panel changing the sample rate or because the default output device was changed, for example when plugging in an HDMI monitor or a USB headset.</para>
    /// </summary>
    /// <param name="deviceWasChanged">True if the change was caused by an device change.</param>
    public delegate void AudioConfigurationChangeHandler(bool deviceWasChanged);
  }
}
