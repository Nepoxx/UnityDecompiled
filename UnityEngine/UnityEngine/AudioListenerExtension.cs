// Decompiled with JetBrains decompiler
// Type: UnityEngine.AudioListenerExtension
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  internal class AudioListenerExtension : ScriptableObject
  {
    [SerializeField]
    private AudioListener m_audioListener;

    public AudioListener audioListener
    {
      get
      {
        return this.m_audioListener;
      }
      set
      {
        this.m_audioListener = value;
      }
    }

    public virtual float ReadExtensionProperty(PropertyName propertyName)
    {
      return 0.0f;
    }

    public virtual void WriteExtensionProperty(PropertyName propertyName, float propertyValue)
    {
    }

    public virtual void ExtensionUpdate()
    {
    }
  }
}
