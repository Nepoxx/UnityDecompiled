// Decompiled with JetBrains decompiler
// Type: UnityEngine.Playables.AudioPlayableGraphExtensions
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine.Playables
{
  internal static class AudioPlayableGraphExtensions
  {
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool InternalCreateAudioOutput(ref PlayableGraph graph, string name, out PlayableOutputHandle handle);
  }
}
