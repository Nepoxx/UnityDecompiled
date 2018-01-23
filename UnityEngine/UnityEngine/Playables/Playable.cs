// Decompiled with JetBrains decompiler
// Type: UnityEngine.Playables.Playable
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
  [RequiredByNativeCode]
  public struct Playable : IPlayable, IEquatable<Playable>
  {
    private static readonly Playable m_NullPlayable = new Playable(PlayableHandle.Null);
    private PlayableHandle m_Handle;

    internal Playable(PlayableHandle handle)
    {
      this.m_Handle = handle;
    }

    public static Playable Null
    {
      get
      {
        return Playable.m_NullPlayable;
      }
    }

    public static Playable Create(PlayableGraph graph, int inputCount = 0)
    {
      Playable playable = new Playable(graph.CreatePlayableHandle());
      playable.SetInputCount<Playable>(inputCount);
      return playable;
    }

    public PlayableHandle GetHandle()
    {
      return this.m_Handle;
    }

    public bool IsPlayableOfType<T>() where T : struct, IPlayable
    {
      return this.GetHandle().IsPlayableOfType<T>();
    }

    public System.Type GetPlayableType()
    {
      return this.GetHandle().GetPlayableType();
    }

    public bool Equals(Playable other)
    {
      return this.GetHandle() == other.GetHandle();
    }
  }
}
