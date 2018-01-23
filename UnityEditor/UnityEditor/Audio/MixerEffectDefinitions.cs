// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.MixerEffectDefinitions
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor.Audio
{
  internal sealed class MixerEffectDefinitions
  {
    private static readonly List<MixerEffectDefinition> s_MixerEffectDefinitions = new List<MixerEffectDefinition>();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ClearDefinitionsRuntime();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void AddDefinitionRuntime(string name, MixerParameterDefinition[] parameters);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetAudioEffectNames();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern MixerParameterDefinition[] GetAudioEffectParameterDesc(string effectName);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool EffectCanBeSidechainTarget(AudioMixerEffectController effect);

    public static void Refresh()
    {
      MixerEffectDefinitions.ClearDefinitions();
      MixerEffectDefinitions.RegisterAudioMixerEffect("Attenuation", new MixerParameterDefinition[0]);
      MixerEffectDefinitions.RegisterAudioMixerEffect("Send", new MixerParameterDefinition[0]);
      MixerEffectDefinitions.RegisterAudioMixerEffect("Receive", new MixerParameterDefinition[0]);
      MixerParameterDefinition[] parameterDefinitionArray = new MixerParameterDefinition[7]{ new MixerParameterDefinition() { name = "Threshold", units = "dB", displayScale = 1f, displayExponent = 1f, minRange = -80f, maxRange = 0.0f, defaultValue = -10f, description = "Threshold of side-chain level detector" }, new MixerParameterDefinition() { name = "Ratio", units = "%", displayScale = 100f, displayExponent = 1f, minRange = 0.2f, maxRange = 10f, defaultValue = 2f, description = "Ratio of compression applied when side-chain signal exceeds threshold" }, new MixerParameterDefinition() { name = "Attack Time", units = "ms", displayScale = 1000f, displayExponent = 3f, minRange = 0.0f, maxRange = 10f, defaultValue = 0.1f, description = "Level detector attack time" }, new MixerParameterDefinition() { name = "Release Time", units = "ms", displayScale = 1000f, displayExponent = 3f, minRange = 0.0f, maxRange = 10f, defaultValue = 0.1f, description = "Level detector release time" }, new MixerParameterDefinition() { name = "Make-up Gain", units = "dB", displayScale = 1f, displayExponent = 1f, minRange = -80f, maxRange = 40f, defaultValue = 0.0f, description = "Make-up gain" }, new MixerParameterDefinition() { name = "Knee", units = "dB", displayScale = 1f, displayExponent = 1f, minRange = 0.0f, maxRange = 50f, defaultValue = 10f, description = "Sharpness of compression curve knee" }, new MixerParameterDefinition() { name = "Sidechain Mix", units = "%", displayScale = 100f, displayExponent = 1f, minRange = 0.0f, maxRange = 1f, defaultValue = 1f, description = "Sidechain/source mix. If set to 100% the compressor detects level entirely from sidechain signal." } };
      MixerEffectDefinitions.RegisterAudioMixerEffect("Duck Volume", parameterDefinitionArray);
      MixerEffectDefinitions.AddDefinitionRuntime("Duck Volume", parameterDefinitionArray);
      foreach (string audioEffectName in MixerEffectDefinitions.GetAudioEffectNames())
      {
        MixerParameterDefinition[] effectParameterDesc = MixerEffectDefinitions.GetAudioEffectParameterDesc(audioEffectName);
        MixerEffectDefinitions.RegisterAudioMixerEffect(audioEffectName, effectParameterDesc);
      }
    }

    public static bool EffectExists(string name)
    {
      foreach (MixerEffectDefinition effectDefinition in MixerEffectDefinitions.s_MixerEffectDefinitions)
      {
        if (effectDefinition.name == name)
          return true;
      }
      return false;
    }

    public static string[] GetEffectList()
    {
      string[] strArray = new string[MixerEffectDefinitions.s_MixerEffectDefinitions.Count];
      for (int index = 0; index < MixerEffectDefinitions.s_MixerEffectDefinitions.Count; ++index)
        strArray[index] = MixerEffectDefinitions.s_MixerEffectDefinitions[index].name;
      return strArray;
    }

    public static void ClearDefinitions()
    {
      MixerEffectDefinitions.s_MixerEffectDefinitions.Clear();
      MixerEffectDefinitions.ClearDefinitionsRuntime();
    }

    public static MixerParameterDefinition[] GetEffectParameters(string effect)
    {
      foreach (MixerEffectDefinition effectDefinition in MixerEffectDefinitions.s_MixerEffectDefinitions)
      {
        if (effectDefinition.name == effect)
          return effectDefinition.parameters;
      }
      return new MixerParameterDefinition[0];
    }

    public static bool RegisterAudioMixerEffect(string name, MixerParameterDefinition[] definitions)
    {
      foreach (MixerEffectDefinition effectDefinition in MixerEffectDefinitions.s_MixerEffectDefinitions)
      {
        if (effectDefinition.name == name)
          return false;
      }
      MixerEffectDefinition effectDefinition1 = new MixerEffectDefinition(name, definitions);
      MixerEffectDefinitions.s_MixerEffectDefinitions.Add(effectDefinition1);
      MixerEffectDefinitions.ClearDefinitionsRuntime();
      foreach (MixerEffectDefinition effectDefinition2 in MixerEffectDefinitions.s_MixerEffectDefinitions)
        MixerEffectDefinitions.AddDefinitionRuntime(effectDefinition2.name, effectDefinition2.parameters);
      return true;
    }
  }
}
