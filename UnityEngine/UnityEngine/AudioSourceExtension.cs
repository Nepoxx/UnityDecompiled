// Decompiled with JetBrains decompiler
// Type: UnityEngine.AudioSourceExtension
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  internal class AudioSourceExtension : ScriptableObject
  {
    internal int m_ExtensionManagerUpdateIndex = -1;
    [SerializeField]
    private AudioSource m_audioSource;

    public AudioSource audioSource
    {
      get
      {
        return this.m_audioSource;
      }
      set
      {
        this.m_audioSource = value;
      }
    }

    public virtual float ReadExtensionProperty(PropertyName propertyName)
    {
      return 0.0f;
    }

    public virtual void WriteExtensionProperty(PropertyName propertyName, float propertyValue)
    {
    }

    public virtual void Play()
    {
    }

    public virtual void Stop()
    {
    }

    public virtual void ExtensionUpdate()
    {
    }

    public void OnDestroy()
    {
      this.Stop();
      AudioExtensionManager.RemoveExtensionFromManager(this);
      if (!((Object) this.audioSource != (Object) null))
        return;
      if ((Object) this.audioSource.spatializerExtension == (Object) this)
        this.audioSource.spatializerExtension = (AudioSourceExtension) null;
      if ((Object) this.audioSource.ambisonicExtension == (Object) this)
        this.audioSource.ambisonicExtension = (AudioSourceExtension) null;
    }
  }
}
