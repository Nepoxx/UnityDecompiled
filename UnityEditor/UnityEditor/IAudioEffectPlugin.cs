// Decompiled with JetBrains decompiler
// Type: UnityEditor.IAudioEffectPlugin
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  public abstract class IAudioEffectPlugin
  {
    public abstract bool SetFloatParameter(string name, float value);

    public abstract bool GetFloatParameter(string name, out float value);

    public abstract bool GetFloatParameterInfo(string name, out float minRange, out float maxRange, out float defaultValue);

    public abstract bool GetFloatBuffer(string name, out float[] data, int numsamples);

    public abstract int GetSampleRate();

    public abstract bool IsPluginEditableAndEnabled();
  }
}
