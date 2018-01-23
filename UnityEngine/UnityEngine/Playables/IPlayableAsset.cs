// Decompiled with JetBrains decompiler
// Type: UnityEngine.Playables.IPlayableAsset
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;

namespace UnityEngine.Playables
{
  public interface IPlayableAsset
  {
    Playable CreatePlayable(PlayableGraph graph, GameObject owner);

    double duration { get; }

    IEnumerable<PlayableBinding> outputs { get; }
  }
}
