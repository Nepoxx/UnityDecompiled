// Decompiled with JetBrains decompiler
// Type: UnityEngine.Playables.PlayableBehaviour
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
  [RequiredByNativeCode]
  [Serializable]
  public abstract class PlayableBehaviour : IPlayableBehaviour, ICloneable
  {
    public virtual void OnGraphStart(Playable playable)
    {
    }

    public virtual void OnGraphStop(Playable playable)
    {
    }

    public virtual void OnPlayableCreate(Playable playable)
    {
    }

    public virtual void OnPlayableDestroy(Playable playable)
    {
    }

    public virtual void OnBehaviourDelay(Playable playable, FrameData info)
    {
    }

    public virtual void OnBehaviourPlay(Playable playable, FrameData info)
    {
    }

    public virtual void OnBehaviourPause(Playable playable, FrameData info)
    {
    }

    public virtual void PrepareData(Playable playable, FrameData info)
    {
    }

    public virtual void PrepareFrame(Playable playable, FrameData info)
    {
    }

    public virtual void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
    }

    public virtual object Clone()
    {
      return this.MemberwiseClone();
    }
  }
}
