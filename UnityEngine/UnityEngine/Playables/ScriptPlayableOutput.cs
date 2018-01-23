// Decompiled with JetBrains decompiler
// Type: UnityEngine.Playables.ScriptPlayableOutput
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
  [RequiredByNativeCode]
  public struct ScriptPlayableOutput : IPlayableOutput
  {
    private PlayableOutputHandle m_Handle;

    internal ScriptPlayableOutput(PlayableOutputHandle handle)
    {
      if (handle.IsValid() && !handle.IsPlayableOutputOfType<ScriptPlayableOutput>())
        throw new InvalidCastException("Can't set handle: the playable is not a ScriptPlayableOutput.");
      this.m_Handle = handle;
    }

    public static ScriptPlayableOutput Create(PlayableGraph graph, string name)
    {
      PlayableOutputHandle handle;
      if (!graph.CreateScriptOutputInternal(name, out handle))
        return ScriptPlayableOutput.Null;
      return new ScriptPlayableOutput(handle);
    }

    public static ScriptPlayableOutput Null
    {
      get
      {
        return new ScriptPlayableOutput(PlayableOutputHandle.Null);
      }
    }

    public PlayableOutputHandle GetHandle()
    {
      return this.m_Handle;
    }

    public static implicit operator PlayableOutput(ScriptPlayableOutput output)
    {
      return new PlayableOutput(output.GetHandle());
    }

    public static explicit operator ScriptPlayableOutput(PlayableOutput output)
    {
      return new ScriptPlayableOutput(output.GetHandle());
    }
  }
}
