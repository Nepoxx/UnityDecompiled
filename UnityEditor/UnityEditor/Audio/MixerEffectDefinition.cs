// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.MixerEffectDefinition
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor.Audio
{
  internal class MixerEffectDefinition
  {
    private readonly string m_EffectName;
    private readonly MixerParameterDefinition[] m_Parameters;

    public MixerEffectDefinition(string name, MixerParameterDefinition[] parameters)
    {
      this.m_EffectName = name;
      this.m_Parameters = new MixerParameterDefinition[parameters.Length];
      Array.Copy((Array) parameters, (Array) this.m_Parameters, parameters.Length);
    }

    public string name
    {
      get
      {
        return this.m_EffectName;
      }
    }

    public MixerParameterDefinition[] parameters
    {
      get
      {
        return this.m_Parameters;
      }
    }
  }
}
