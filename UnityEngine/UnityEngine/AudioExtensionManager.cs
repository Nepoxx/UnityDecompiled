// Decompiled with JetBrains decompiler
// Type: UnityEngine.AudioExtensionManager
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  internal sealed class AudioExtensionManager
  {
    private static List<AudioSpatializerExtensionDefinition> m_ListenerSpatializerExtensionDefinitions = new List<AudioSpatializerExtensionDefinition>();
    private static List<AudioSpatializerExtensionDefinition> m_SourceSpatializerExtensionDefinitions = new List<AudioSpatializerExtensionDefinition>();
    private static List<AudioAmbisonicExtensionDefinition> m_SourceAmbisonicDecoderExtensionDefinitions = new List<AudioAmbisonicExtensionDefinition>();
    private static List<AudioSourceExtension> m_SourceExtensionsToUpdate = new List<AudioSourceExtension>();
    private static int m_NextStopIndex = 0;
    private static bool m_BuiltinDefinitionsRegistered = false;
    private static PropertyName m_SpatializerName = (PropertyName) 0;
    private static PropertyName m_SpatializerExtensionName = (PropertyName) 0;
    private static PropertyName m_ListenerSpatializerExtensionName = (PropertyName) 0;

    internal static bool IsListenerSpatializerExtensionRegistered()
    {
      foreach (AudioSpatializerExtensionDefinition extensionDefinition in AudioExtensionManager.m_ListenerSpatializerExtensionDefinitions)
      {
        if ((PropertyName) AudioSettings.GetSpatializerPluginName() == extensionDefinition.spatializerName)
          return true;
      }
      return false;
    }

    internal static bool IsSourceSpatializerExtensionRegistered()
    {
      foreach (AudioSpatializerExtensionDefinition extensionDefinition in AudioExtensionManager.m_SourceSpatializerExtensionDefinitions)
      {
        if ((PropertyName) AudioSettings.GetSpatializerPluginName() == extensionDefinition.spatializerName)
          return true;
      }
      return false;
    }

    internal static bool IsSourceAmbisonicDecoderExtensionRegistered()
    {
      foreach (AudioAmbisonicExtensionDefinition extensionDefinition in AudioExtensionManager.m_SourceAmbisonicDecoderExtensionDefinitions)
      {
        if ((PropertyName) AudioSettings.GetAmbisonicDecoderPluginName() == extensionDefinition.ambisonicPluginName)
          return true;
      }
      return false;
    }

    internal static AudioSourceExtension AddSpatializerExtension(AudioSource source)
    {
      if (!source.spatialize)
        return (AudioSourceExtension) null;
      if ((Object) source.spatializerExtension != (Object) null)
        return source.spatializerExtension;
      AudioExtensionManager.RegisterBuiltinDefinitions();
      foreach (AudioSpatializerExtensionDefinition extensionDefinition in AudioExtensionManager.m_SourceSpatializerExtensionDefinitions)
      {
        if ((PropertyName) AudioSettings.GetSpatializerPluginName() == extensionDefinition.spatializerName)
        {
          AudioSourceExtension extension = source.AddSpatializerExtension(extensionDefinition.definition.GetExtensionType());
          if ((Object) extension != (Object) null)
          {
            extension.audioSource = source;
            source.spatializerExtension = extension;
            AudioExtensionManager.WriteExtensionProperties(extension, extensionDefinition.definition.GetExtensionType().Name);
            return extension;
          }
        }
      }
      return (AudioSourceExtension) null;
    }

    internal static AudioSourceExtension AddAmbisonicDecoderExtension(AudioSource source)
    {
      if ((Object) source.ambisonicExtension != (Object) null)
        return source.ambisonicExtension;
      AudioExtensionManager.RegisterBuiltinDefinitions();
      foreach (AudioAmbisonicExtensionDefinition extensionDefinition in AudioExtensionManager.m_SourceAmbisonicDecoderExtensionDefinitions)
      {
        if ((PropertyName) AudioSettings.GetAmbisonicDecoderPluginName() == extensionDefinition.ambisonicPluginName)
        {
          AudioSourceExtension audioSourceExtension = source.AddAmbisonicExtension(extensionDefinition.definition.GetExtensionType());
          if ((Object) audioSourceExtension != (Object) null)
          {
            audioSourceExtension.audioSource = source;
            source.ambisonicExtension = audioSourceExtension;
            return audioSourceExtension;
          }
        }
      }
      return (AudioSourceExtension) null;
    }

    internal static void WriteExtensionProperties(AudioSourceExtension extension, string extensionName)
    {
      if (AudioExtensionManager.m_SpatializerExtensionName == (PropertyName) 0)
        AudioExtensionManager.m_SpatializerExtensionName = (PropertyName) extensionName;
      for (int sourceIndex = 0; sourceIndex < extension.audioSource.GetNumExtensionProperties(); ++sourceIndex)
      {
        if (extension.audioSource.ReadExtensionName(sourceIndex) == AudioExtensionManager.m_SpatializerExtensionName)
        {
          PropertyName propertyName = extension.audioSource.ReadExtensionPropertyName(sourceIndex);
          float propertyValue = extension.audioSource.ReadExtensionPropertyValue(sourceIndex);
          extension.WriteExtensionProperty(propertyName, propertyValue);
        }
      }
    }

    internal static AudioListenerExtension AddSpatializerExtension(AudioListener listener)
    {
      if ((Object) listener.spatializerExtension != (Object) null)
        return listener.spatializerExtension;
      AudioExtensionManager.RegisterBuiltinDefinitions();
      foreach (AudioSpatializerExtensionDefinition extensionDefinition in AudioExtensionManager.m_ListenerSpatializerExtensionDefinitions)
      {
        if ((PropertyName) AudioSettings.GetSpatializerPluginName() == extensionDefinition.spatializerName || (PropertyName) AudioSettings.GetAmbisonicDecoderPluginName() == extensionDefinition.spatializerName)
        {
          AudioListenerExtension extension = listener.AddExtension(extensionDefinition.definition.GetExtensionType());
          if ((Object) extension != (Object) null)
          {
            extension.audioListener = listener;
            listener.spatializerExtension = extension;
            AudioExtensionManager.WriteExtensionProperties(extension, extensionDefinition.definition.GetExtensionType().Name);
            return extension;
          }
        }
      }
      return (AudioListenerExtension) null;
    }

    internal static void WriteExtensionProperties(AudioListenerExtension extension, string extensionName)
    {
      if (AudioExtensionManager.m_ListenerSpatializerExtensionName == (PropertyName) 0)
        AudioExtensionManager.m_ListenerSpatializerExtensionName = (PropertyName) extensionName;
      for (int listenerIndex = 0; listenerIndex < extension.audioListener.GetNumExtensionProperties(); ++listenerIndex)
      {
        if (extension.audioListener.ReadExtensionName(listenerIndex) == AudioExtensionManager.m_ListenerSpatializerExtensionName)
        {
          PropertyName propertyName = extension.audioListener.ReadExtensionPropertyName(listenerIndex);
          float propertyValue = extension.audioListener.ReadExtensionPropertyValue(listenerIndex);
          extension.WriteExtensionProperty(propertyName, propertyValue);
        }
      }
    }

    internal static AudioListenerExtension GetSpatializerExtension(AudioListener listener)
    {
      if ((Object) listener.spatializerExtension != (Object) null)
        return listener.spatializerExtension;
      return (AudioListenerExtension) null;
    }

    internal static AudioSourceExtension GetSpatializerExtension(AudioSource source)
    {
      return !source.spatialize ? (AudioSourceExtension) null : source.spatializerExtension;
    }

    internal static AudioSourceExtension GetAmbisonicExtension(AudioSource source)
    {
      return source.ambisonicExtension;
    }

    internal static System.Type GetListenerSpatializerExtensionType()
    {
      foreach (AudioSpatializerExtensionDefinition extensionDefinition in AudioExtensionManager.m_ListenerSpatializerExtensionDefinitions)
      {
        if ((PropertyName) AudioSettings.GetSpatializerPluginName() == extensionDefinition.spatializerName)
          return extensionDefinition.definition.GetExtensionType();
      }
      return (System.Type) null;
    }

    internal static System.Type GetListenerSpatializerExtensionEditorType()
    {
      foreach (AudioSpatializerExtensionDefinition extensionDefinition in AudioExtensionManager.m_ListenerSpatializerExtensionDefinitions)
      {
        if ((PropertyName) AudioSettings.GetSpatializerPluginName() == extensionDefinition.spatializerName)
          return extensionDefinition.editorDefinition.GetExtensionType();
      }
      return (System.Type) null;
    }

    internal static System.Type GetSourceSpatializerExtensionType()
    {
      foreach (AudioSpatializerExtensionDefinition extensionDefinition in AudioExtensionManager.m_SourceSpatializerExtensionDefinitions)
      {
        if ((PropertyName) AudioSettings.GetSpatializerPluginName() == extensionDefinition.spatializerName)
          return extensionDefinition.definition.GetExtensionType();
      }
      return (System.Type) null;
    }

    internal static System.Type GetSourceSpatializerExtensionEditorType()
    {
      foreach (AudioSpatializerExtensionDefinition extensionDefinition in AudioExtensionManager.m_SourceSpatializerExtensionDefinitions)
      {
        if ((PropertyName) AudioSettings.GetSpatializerPluginName() == extensionDefinition.spatializerName)
          return extensionDefinition.editorDefinition.GetExtensionType();
      }
      return (System.Type) null;
    }

    internal static System.Type GetSourceAmbisonicExtensionType()
    {
      foreach (AudioAmbisonicExtensionDefinition extensionDefinition in AudioExtensionManager.m_SourceAmbisonicDecoderExtensionDefinitions)
      {
        if ((PropertyName) AudioSettings.GetAmbisonicDecoderPluginName() == extensionDefinition.ambisonicPluginName)
          return extensionDefinition.definition.GetExtensionType();
      }
      return (System.Type) null;
    }

    internal static PropertyName GetSpatializerName()
    {
      return AudioExtensionManager.m_SpatializerName;
    }

    internal static PropertyName GetSourceSpatializerExtensionName()
    {
      return AudioExtensionManager.m_SpatializerExtensionName;
    }

    internal static PropertyName GetListenerSpatializerExtensionName()
    {
      return AudioExtensionManager.m_ListenerSpatializerExtensionName;
    }

    internal static void AddExtensionToManager(AudioSourceExtension extension)
    {
      AudioExtensionManager.RegisterBuiltinDefinitions();
      if (extension.m_ExtensionManagerUpdateIndex != -1)
        return;
      AudioExtensionManager.m_SourceExtensionsToUpdate.Add(extension);
      extension.m_ExtensionManagerUpdateIndex = AudioExtensionManager.m_SourceExtensionsToUpdate.Count - 1;
    }

    internal static void RemoveExtensionFromManager(AudioSourceExtension extension)
    {
      int managerUpdateIndex = extension.m_ExtensionManagerUpdateIndex;
      if (managerUpdateIndex >= 0 && managerUpdateIndex < AudioExtensionManager.m_SourceExtensionsToUpdate.Count)
      {
        int index = AudioExtensionManager.m_SourceExtensionsToUpdate.Count - 1;
        AudioExtensionManager.m_SourceExtensionsToUpdate[managerUpdateIndex] = AudioExtensionManager.m_SourceExtensionsToUpdate[index];
        AudioExtensionManager.m_SourceExtensionsToUpdate[managerUpdateIndex].m_ExtensionManagerUpdateIndex = managerUpdateIndex;
        AudioExtensionManager.m_SourceExtensionsToUpdate.RemoveAt(index);
      }
      extension.m_ExtensionManagerUpdateIndex = -1;
    }

    internal static void Update()
    {
      AudioExtensionManager.RegisterBuiltinDefinitions();
      if (AudioExtensionManager.m_SpatializerName != (PropertyName) AudioSettings.GetSpatializerPluginName())
      {
        AudioExtensionManager.m_SpatializerName = (PropertyName) AudioSettings.GetSpatializerPluginName();
        if (AudioExtensionManager.GetSourceSpatializerExtensionType() != null)
          AudioExtensionManager.m_SpatializerExtensionName = (PropertyName) AudioExtensionManager.GetSourceSpatializerExtensionType().Name;
        if (AudioExtensionManager.GetListenerSpatializerExtensionEditorType() != null)
          AudioExtensionManager.m_ListenerSpatializerExtensionName = (PropertyName) AudioExtensionManager.GetListenerSpatializerExtensionType().Name;
      }
      AudioListener audioListener = AudioExtensionManager.GetAudioListener() as AudioListener;
      if ((Object) audioListener != (Object) null)
      {
        AudioListenerExtension listenerExtension = AudioExtensionManager.AddSpatializerExtension(audioListener);
        if ((Object) listenerExtension != (Object) null)
          listenerExtension.ExtensionUpdate();
      }
      for (int index = 0; index < AudioExtensionManager.m_SourceExtensionsToUpdate.Count; ++index)
        AudioExtensionManager.m_SourceExtensionsToUpdate[index].ExtensionUpdate();
      AudioExtensionManager.m_NextStopIndex = AudioExtensionManager.m_NextStopIndex < AudioExtensionManager.m_SourceExtensionsToUpdate.Count ? AudioExtensionManager.m_NextStopIndex : 0;
      int num = AudioExtensionManager.m_SourceExtensionsToUpdate.Count <= 0 ? 0 : 1 + AudioExtensionManager.m_SourceExtensionsToUpdate.Count / 8;
      for (int index = 0; index < num; ++index)
      {
        AudioSourceExtension extension = AudioExtensionManager.m_SourceExtensionsToUpdate[AudioExtensionManager.m_NextStopIndex];
        if ((Object) extension.audioSource == (Object) null || !extension.audioSource.enabled || !extension.audioSource.isPlaying)
        {
          extension.Stop();
          AudioExtensionManager.RemoveExtensionFromManager(extension);
        }
        else
        {
          ++AudioExtensionManager.m_NextStopIndex;
          AudioExtensionManager.m_NextStopIndex = AudioExtensionManager.m_NextStopIndex < AudioExtensionManager.m_SourceExtensionsToUpdate.Count ? AudioExtensionManager.m_NextStopIndex : 0;
        }
      }
    }

    internal static void GetReadyToPlay(AudioSourceExtension extension)
    {
      if (!((Object) extension != (Object) null))
        return;
      extension.Play();
      AudioExtensionManager.AddExtensionToManager(extension);
    }

    private static void RegisterBuiltinDefinitions()
    {
      bool flag = true;
      if (AudioExtensionManager.m_BuiltinDefinitionsRegistered)
        return;
      if (flag || !(AudioSettings.GetSpatializerPluginName() == "GVR Audio Spatializer"))
        ;
      if (flag || !(AudioSettings.GetAmbisonicDecoderPluginName() == "GVR Audio Spatializer"))
        ;
      AudioExtensionManager.m_BuiltinDefinitionsRegistered = true;
    }

    private static bool RegisterListenerSpatializerDefinition(string spatializerName, AudioExtensionDefinition extensionDefinition, AudioExtensionDefinition editorDefinition)
    {
      foreach (AudioSpatializerExtensionDefinition extensionDefinition1 in AudioExtensionManager.m_ListenerSpatializerExtensionDefinitions)
      {
        if ((PropertyName) spatializerName == extensionDefinition1.spatializerName)
        {
          Debug.Log((object) ("RegisterListenerSpatializerDefinition failed for " + (object) extensionDefinition.GetExtensionType() + ". We only allow one audio listener extension to be registered for each spatializer."));
          return false;
        }
      }
      AudioSpatializerExtensionDefinition extensionDefinition2 = new AudioSpatializerExtensionDefinition(spatializerName, extensionDefinition, editorDefinition);
      AudioExtensionManager.m_ListenerSpatializerExtensionDefinitions.Add(extensionDefinition2);
      return true;
    }

    private static bool RegisterSourceSpatializerDefinition(string spatializerName, AudioExtensionDefinition extensionDefinition, AudioExtensionDefinition editorDefinition)
    {
      foreach (AudioSpatializerExtensionDefinition extensionDefinition1 in AudioExtensionManager.m_SourceSpatializerExtensionDefinitions)
      {
        if ((PropertyName) spatializerName == extensionDefinition1.spatializerName)
        {
          Debug.Log((object) ("RegisterSourceSpatializerDefinition failed for " + (object) extensionDefinition.GetExtensionType() + ". We only allow one audio source extension to be registered for each spatializer."));
          return false;
        }
      }
      AudioSpatializerExtensionDefinition extensionDefinition2 = new AudioSpatializerExtensionDefinition(spatializerName, extensionDefinition, editorDefinition);
      AudioExtensionManager.m_SourceSpatializerExtensionDefinitions.Add(extensionDefinition2);
      return true;
    }

    private static bool RegisterSourceAmbisonicDefinition(string ambisonicDecoderName, AudioExtensionDefinition extensionDefinition)
    {
      foreach (AudioAmbisonicExtensionDefinition extensionDefinition1 in AudioExtensionManager.m_SourceAmbisonicDecoderExtensionDefinitions)
      {
        if ((PropertyName) ambisonicDecoderName == extensionDefinition1.ambisonicPluginName)
        {
          Debug.Log((object) ("RegisterSourceAmbisonicDefinition failed for " + (object) extensionDefinition.GetExtensionType() + ". We only allow one audio source extension to be registered for each ambisonic decoder."));
          return false;
        }
      }
      AudioAmbisonicExtensionDefinition extensionDefinition2 = new AudioAmbisonicExtensionDefinition(ambisonicDecoderName, extensionDefinition);
      AudioExtensionManager.m_SourceAmbisonicDecoderExtensionDefinitions.Add(extensionDefinition2);
      return true;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Object GetAudioListener();
  }
}
