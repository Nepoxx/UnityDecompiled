// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.AudioMixerEffectPlugin
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Audio
{
  public class AudioMixerEffectPlugin : IAudioEffectPlugin
  {
    internal AudioMixerController m_Controller;
    internal AudioMixerEffectController m_Effect;
    internal MixerParameterDefinition[] m_ParamDefs;

    public override bool SetFloatParameter(string name, float value)
    {
      this.m_Effect.SetValueForParameter(this.m_Controller, this.m_Controller.TargetSnapshot, name, value);
      return true;
    }

    public override bool GetFloatParameter(string name, out float value)
    {
      value = this.m_Effect.GetValueForParameter(this.m_Controller, this.m_Controller.TargetSnapshot, name);
      return true;
    }

    public override bool GetFloatParameterInfo(string name, out float minRange, out float maxRange, out float defaultValue)
    {
      foreach (MixerParameterDefinition paramDef in this.m_ParamDefs)
      {
        if (paramDef.name == name)
        {
          minRange = paramDef.minRange;
          maxRange = paramDef.maxRange;
          defaultValue = paramDef.defaultValue;
          return true;
        }
      }
      minRange = 0.0f;
      maxRange = 1f;
      defaultValue = 0.5f;
      return false;
    }

    public override bool GetFloatBuffer(string name, out float[] data, int numsamples)
    {
      this.m_Effect.GetFloatBuffer(this.m_Controller, name, out data, numsamples);
      return true;
    }

    public override int GetSampleRate()
    {
      return AudioSettings.outputSampleRate;
    }

    public override bool IsPluginEditableAndEnabled()
    {
      return AudioMixerController.EditingTargetSnapshot() && !this.m_Effect.bypass;
    }
  }
}
