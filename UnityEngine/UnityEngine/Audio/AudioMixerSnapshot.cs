// Decompiled with JetBrains decompiler
// Type: UnityEngine.Audio.AudioMixerSnapshot
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Audio
{
  public class AudioMixerSnapshot : Object
  {
    internal AudioMixerSnapshot()
    {
    }

    public extern AudioMixer audioMixer { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void TransitionTo(float timeToReach);
  }
}
