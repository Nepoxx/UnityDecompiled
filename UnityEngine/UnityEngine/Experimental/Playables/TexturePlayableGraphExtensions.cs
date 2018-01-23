// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Playables.TexturePlayableGraphExtensions
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Playables;

namespace UnityEngine.Experimental.Playables
{
  internal static class TexturePlayableGraphExtensions
  {
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool InternalCreateTextureOutput(ref PlayableGraph graph, string name, out PlayableOutputHandle handle);
  }
}
