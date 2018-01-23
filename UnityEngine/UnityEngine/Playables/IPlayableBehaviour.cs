// Decompiled with JetBrains decompiler
// Type: UnityEngine.Playables.IPlayableBehaviour
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Playables
{
  public interface IPlayableBehaviour
  {
    void OnGraphStart(Playable playable);

    void OnGraphStop(Playable playable);

    void OnPlayableCreate(Playable playable);

    void OnPlayableDestroy(Playable playable);

    void OnBehaviourPlay(Playable playable, FrameData info);

    void OnBehaviourPause(Playable playable, FrameData info);

    void PrepareFrame(Playable playable, FrameData info);

    void ProcessFrame(Playable playable, FrameData info, object playerData);
  }
}
