// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Playables.TexturePlayableOutput
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
  public struct TexturePlayableOutput : IPlayableOutput
  {
    private PlayableOutputHandle m_Handle;

    internal TexturePlayableOutput(PlayableOutputHandle handle)
    {
      if (handle.IsValid() && !handle.IsPlayableOutputOfType<TexturePlayableOutput>())
        throw new InvalidCastException("Can't set handle: the playable is not an TexturePlayableOutput.");
      this.m_Handle = handle;
    }

    public static TexturePlayableOutput Create(PlayableGraph graph, string name, RenderTexture target)
    {
      PlayableOutputHandle handle;
      if (!TexturePlayableGraphExtensions.InternalCreateTextureOutput(ref graph, name, out handle))
        return TexturePlayableOutput.Null;
      TexturePlayableOutput texturePlayableOutput = new TexturePlayableOutput(handle);
      texturePlayableOutput.SetTarget(target);
      return texturePlayableOutput;
    }

    public static TexturePlayableOutput Null
    {
      get
      {
        return new TexturePlayableOutput(PlayableOutputHandle.Null);
      }
    }

    public PlayableOutputHandle GetHandle()
    {
      return this.m_Handle;
    }

    public static implicit operator PlayableOutput(TexturePlayableOutput output)
    {
      return new PlayableOutput(output.GetHandle());
    }

    public static explicit operator TexturePlayableOutput(PlayableOutput output)
    {
      return new TexturePlayableOutput(output.GetHandle());
    }

    public RenderTexture GetTarget()
    {
      return TexturePlayableOutput.InternalGetTarget(ref this.m_Handle);
    }

    public void SetTarget(RenderTexture value)
    {
      TexturePlayableOutput.InternalSetTarget(ref this.m_Handle, value);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern RenderTexture InternalGetTarget(ref PlayableOutputHandle output);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void InternalSetTarget(ref PlayableOutputHandle output, RenderTexture target);
  }
}
