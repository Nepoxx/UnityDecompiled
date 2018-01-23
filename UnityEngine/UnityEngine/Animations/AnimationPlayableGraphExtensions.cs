// Decompiled with JetBrains decompiler
// Type: UnityEngine.Animations.AnimationPlayableGraphExtensions
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Playables;

namespace UnityEngine.Animations
{
  internal static class AnimationPlayableGraphExtensions
  {
    internal static void SyncUpdateAndTimeMode(this PlayableGraph graph, Animator animator)
    {
      AnimationPlayableGraphExtensions.InternalSyncUpdateAndTimeMode(ref graph, animator);
    }

    internal static void DestroyOutput(this PlayableGraph graph, PlayableOutputHandle handle)
    {
      AnimationPlayableGraphExtensions.InternalDestroyOutput(ref graph, ref handle);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool InternalCreateAnimationOutput(ref PlayableGraph graph, string name, out PlayableOutputHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void InternalSyncUpdateAndTimeMode(ref PlayableGraph graph, Animator animator);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void InternalDestroyOutput(ref PlayableGraph graph, ref PlayableOutputHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int InternalAnimationOutputCount(ref PlayableGraph graph);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool InternalGetAnimationOutput(ref PlayableGraph graph, int index, out PlayableOutputHandle handle);
  }
}
