// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.AudioGroupParameterPath
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Audio
{
  internal class AudioGroupParameterPath : AudioParameterPath
  {
    public AudioMixerGroupController group;

    public AudioGroupParameterPath(AudioMixerGroupController group, GUID parameter)
    {
      this.group = group;
      this.parameter = parameter;
    }

    public override string ResolveStringPath(bool getOnlyBasePath)
    {
      if (getOnlyBasePath)
        return this.GetBasePath(this.group.GetDisplayString(), (string) null);
      if (this.group.GetGUIDForVolume() == this.parameter)
        return "Volume" + this.GetBasePath(this.group.GetDisplayString(), (string) null);
      if (this.group.GetGUIDForPitch() == this.parameter)
        return "Pitch" + this.GetBasePath(this.group.GetDisplayString(), (string) null);
      return "Error finding Parameter path.";
    }

    protected string GetBasePath(string group, string effect)
    {
      string str = " (of " + group;
      if (!string.IsNullOrEmpty(effect))
        str = str + "➔" + effect;
      return str + ")";
    }
  }
}
