// Decompiled with JetBrains decompiler
// Type: UnityEngine.Playables.PlayableAsset
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
  [RequiredByNativeCode]
  [Serializable]
  public abstract class PlayableAsset : ScriptableObject, IPlayableAsset
  {
    public abstract Playable CreatePlayable(PlayableGraph graph, GameObject owner);

    public virtual double duration
    {
      get
      {
        return PlayableBinding.DefaultDuration;
      }
    }

    public virtual IEnumerable<PlayableBinding> outputs
    {
      get
      {
        return (IEnumerable<PlayableBinding>) PlayableBinding.None;
      }
    }

    [RequiredByNativeCode]
    internal static unsafe void Internal_CreatePlayable(PlayableAsset asset, PlayableGraph graph, GameObject go, IntPtr ptr)
    {
      Playable playable = !((UnityEngine.Object) asset == (UnityEngine.Object) null) ? asset.CreatePlayable(graph, go) : Playable.Null;
      *(Playable*) ptr.ToPointer() = playable;
    }

    [RequiredByNativeCode]
    internal static unsafe void Internal_GetPlayableAssetDuration(PlayableAsset asset, IntPtr ptrToDouble)
    {
      double duration = asset.duration;
      *(double*) ptrToDouble.ToPointer() = duration;
    }
  }
}
