// Decompiled with JetBrains decompiler
// Type: UnityEngine.Playables.PlayableOutput
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
  [RequiredByNativeCode]
  public struct PlayableOutput : IPlayableOutput, IEquatable<PlayableOutput>
  {
    private static readonly PlayableOutput m_NullPlayableOutput = new PlayableOutput(PlayableOutputHandle.Null);
    private PlayableOutputHandle m_Handle;

    internal PlayableOutput(PlayableOutputHandle handle)
    {
      this.m_Handle = handle;
    }

    public static PlayableOutput Null
    {
      get
      {
        return PlayableOutput.m_NullPlayableOutput;
      }
    }

    public PlayableOutputHandle GetHandle()
    {
      return this.m_Handle;
    }

    public bool IsPlayableOutputOfType<T>() where T : struct, IPlayableOutput
    {
      return this.GetHandle().IsPlayableOutputOfType<T>();
    }

    public System.Type GetPlayableOutputType()
    {
      return this.GetHandle().GetPlayableOutputType();
    }

    public bool Equals(PlayableOutput other)
    {
      return this.GetHandle() == other.GetHandle();
    }
  }
}
