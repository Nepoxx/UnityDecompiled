// Decompiled with JetBrains decompiler
// Type: UnityEngine.Playables.AnimationPlayableUtilities
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Animations;

namespace UnityEngine.Playables
{
  /// <summary>
  ///   <para>Implements high-level utility methods to simplify use of the Playable API with Animations.</para>
  /// </summary>
  public static class AnimationPlayableUtilities
  {
    /// <summary>
    ///   <para>Plays the Playable on  the given Animator.</para>
    /// </summary>
    /// <param name="animator">Target Animator.</param>
    /// <param name="playable">The Playable that will be played.</param>
    /// <param name="graph">The Graph that owns the Playable.</param>
    public static void Play(Animator animator, Playable playable, PlayableGraph graph)
    {
      AnimationPlayableOutput output = AnimationPlayableOutput.Create(graph, "AnimationClip", animator);
      output.SetSourcePlayable<AnimationPlayableOutput, Playable>(playable);
      output.SetSourceInputPort<AnimationPlayableOutput>(0);
      graph.SyncUpdateAndTimeMode(animator);
      graph.Play();
    }

    public static AnimationClipPlayable PlayClip(Animator animator, AnimationClip clip, out PlayableGraph graph)
    {
      graph = PlayableGraph.Create();
      AnimationPlayableOutput output = AnimationPlayableOutput.Create(graph, "AnimationClip", animator);
      AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(graph, clip);
      output.SetSourcePlayable<AnimationPlayableOutput, AnimationClipPlayable>(animationClipPlayable);
      graph.SyncUpdateAndTimeMode(animator);
      graph.Play();
      return animationClipPlayable;
    }

    public static AnimationMixerPlayable PlayMixer(Animator animator, int inputCount, out PlayableGraph graph)
    {
      graph = PlayableGraph.Create();
      AnimationPlayableOutput output = AnimationPlayableOutput.Create(graph, "Mixer", animator);
      AnimationMixerPlayable animationMixerPlayable = AnimationMixerPlayable.Create(graph, inputCount, false);
      output.SetSourcePlayable<AnimationPlayableOutput, AnimationMixerPlayable>(animationMixerPlayable);
      graph.SyncUpdateAndTimeMode(animator);
      graph.Play();
      return animationMixerPlayable;
    }

    public static AnimationLayerMixerPlayable PlayLayerMixer(Animator animator, int inputCount, out PlayableGraph graph)
    {
      graph = PlayableGraph.Create();
      AnimationPlayableOutput output = AnimationPlayableOutput.Create(graph, "Mixer", animator);
      AnimationLayerMixerPlayable layerMixerPlayable = AnimationLayerMixerPlayable.Create(graph, inputCount);
      output.SetSourcePlayable<AnimationPlayableOutput, AnimationLayerMixerPlayable>(layerMixerPlayable);
      graph.SyncUpdateAndTimeMode(animator);
      graph.Play();
      return layerMixerPlayable;
    }

    public static AnimatorControllerPlayable PlayAnimatorController(Animator animator, RuntimeAnimatorController controller, out PlayableGraph graph)
    {
      graph = PlayableGraph.Create();
      AnimationPlayableOutput output = AnimationPlayableOutput.Create(graph, "AnimatorControllerPlayable", animator);
      AnimatorControllerPlayable controllerPlayable = AnimatorControllerPlayable.Create(graph, controller);
      output.SetSourcePlayable<AnimationPlayableOutput, AnimatorControllerPlayable>(controllerPlayable);
      graph.SyncUpdateAndTimeMode(animator);
      graph.Play();
      return controllerPlayable;
    }
  }
}
