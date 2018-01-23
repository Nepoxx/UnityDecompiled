// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Playables.TextureMixerPlayable
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Playables
{
  [RequiredByNativeCode]
  public struct TextureMixerPlayable : IPlayable, IEquatable<TextureMixerPlayable>
  {
    private PlayableHandle m_Handle;

    internal TextureMixerPlayable(PlayableHandle handle)
    {
      if (handle.IsValid() && !handle.IsPlayableOfType<TextureMixerPlayable>())
        throw new InvalidCastException("Can't set handle: the playable is not an TextureMixerPlayable.");
      this.m_Handle = handle;
    }

    public static TextureMixerPlayable Create(PlayableGraph graph)
    {
      return new TextureMixerPlayable(TextureMixerPlayable.CreateHandle(graph));
    }

    private static PlayableHandle CreateHandle(PlayableGraph graph)
    {
      PlayableHandle handle = PlayableHandle.Null;
      if (!TextureMixerPlayable.CreateTextureMixerPlayableInternal(ref graph, ref handle))
        return PlayableHandle.Null;
      return handle;
    }

    public PlayableHandle GetHandle()
    {
      return this.m_Handle;
    }

    public static implicit operator Playable(TextureMixerPlayable playable)
    {
      return new Playable(playable.GetHandle());
    }

    public static explicit operator TextureMixerPlayable(Playable playable)
    {
      return new TextureMixerPlayable(playable.GetHandle());
    }

    public bool Equals(TextureMixerPlayable other)
    {
      return this.GetHandle() == other.GetHandle();
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool CreateTextureMixerPlayableInternal(ref PlayableGraph graph, ref PlayableHandle handle);
  }
}
