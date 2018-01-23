// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>AnimationMode is used by the AnimationWindow to store properties modified
  /// by the AnimationClip playback.</para>
  /// </summary>
  public sealed class AnimationMode
  {
    private static bool s_InAnimationPlaybackMode = false;
    private static bool s_InAnimationRecordMode = false;
    private static PrefColor s_AnimatedPropertyColor = new PrefColor("Animation/Property Animated", 0.82f, 0.97f, 1f, 1f, 0.54f, 0.85f, 1f, 1f);
    private static PrefColor s_RecordedPropertyColor = new PrefColor("Animation/Property Recorded", 1f, 0.6f, 0.6f, 1f, 1f, 0.5f, 0.5f, 1f);
    private static PrefColor s_CandidatePropertyColor = new PrefColor("Animation/Property Candidate", 1f, 0.7f, 0.6f, 1f, 1f, 0.67f, 0.43f, 1f);
    private static AnimationModeDriver s_DummyDriver;

    /// <summary>
    ///   <para>The color used to show that a property is currently being animated.</para>
    /// </summary>
    public static Color animatedPropertyColor
    {
      get
      {
        return (Color) AnimationMode.s_AnimatedPropertyColor;
      }
    }

    /// <summary>
    ///   <para>The color used to show that an animated property automatically records changes in the animation clip.</para>
    /// </summary>
    public static Color recordedPropertyColor
    {
      get
      {
        return (Color) AnimationMode.s_RecordedPropertyColor;
      }
    }

    /// <summary>
    ///   <para>The color used to show that an animated property has been modified.</para>
    /// </summary>
    public static Color candidatePropertyColor
    {
      get
      {
        return (Color) AnimationMode.s_CandidatePropertyColor;
      }
    }

    private static AnimationModeDriver DummyDriver()
    {
      if ((UnityEngine.Object) AnimationMode.s_DummyDriver == (UnityEngine.Object) null)
      {
        AnimationMode.s_DummyDriver = ScriptableObject.CreateInstance<AnimationModeDriver>();
        AnimationMode.s_DummyDriver.name = nameof (DummyDriver);
      }
      return AnimationMode.s_DummyDriver;
    }

    /// <summary>
    ///   <para>Is the specified property currently in animation mode and being animated?</para>
    /// </summary>
    /// <param name="target">The object to determine if it contained the animation.</param>
    /// <param name="propertyPath">The name of the animation to search for.</param>
    /// <returns>
    ///   <para>Whether the property search is found or not.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsPropertyAnimated(UnityEngine.Object target, string propertyPath);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsPropertyCandidate(UnityEngine.Object target, string propertyPath);

    /// <summary>
    ///   <para>Stops Animation mode, reverts all properties that were animated in animation mode.</para>
    /// </summary>
    public static void StopAnimationMode()
    {
      AnimationMode.StopAnimationMode((UnityEngine.Object) AnimationMode.DummyDriver());
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void StopAnimationMode(UnityEngine.Object driver);

    /// <summary>
    ///   <para>Are we currently in AnimationMode?</para>
    /// </summary>
    public static bool InAnimationMode()
    {
      return AnimationMode.Internal_InAnimationModeNoDriver();
    }

    internal static bool InAnimationMode(UnityEngine.Object driver)
    {
      return AnimationMode.Internal_InAnimationMode(driver);
    }

    /// <summary>
    ///   <para>Starts the animation mode.</para>
    /// </summary>
    public static void StartAnimationMode()
    {
      AnimationMode.StartAnimationMode((UnityEngine.Object) AnimationMode.DummyDriver());
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void StartAnimationMode(UnityEngine.Object driver);

    internal static void StopAnimationPlaybackMode()
    {
      AnimationMode.s_InAnimationPlaybackMode = false;
    }

    internal static bool InAnimationPlaybackMode()
    {
      return AnimationMode.s_InAnimationPlaybackMode;
    }

    internal static void StartAnimationPlaybackMode()
    {
      AnimationMode.s_InAnimationPlaybackMode = true;
    }

    internal static void StopAnimationRecording()
    {
      AnimationMode.s_InAnimationRecordMode = false;
    }

    internal static bool InAnimationRecording()
    {
      return AnimationMode.s_InAnimationRecordMode;
    }

    internal static void StartAnimationRecording()
    {
      AnimationMode.s_InAnimationRecordMode = true;
    }

    internal static void StartCandidateRecording(UnityEngine.Object driver)
    {
      if (!AnimationMode.InAnimationMode())
        throw new InvalidOperationException("AnimationMode.StartCandidateRecording may only be called in animation mode.  See AnimationMode.StartAnimationMode.");
      AnimationMode.Internal_StartCandidateRecording(driver);
    }

    internal static void AddCandidate(EditorCurveBinding binding, PropertyModification modification, bool keepPrefabOverride)
    {
      if (!AnimationMode.IsRecordingCandidates())
        throw new InvalidOperationException("AnimationMode.AddCandidate may only be called when recording candidates.  See AnimationMode.StartCandidateRecording.");
      AnimationMode.Internal_AddCandidate(binding, modification, keepPrefabOverride);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void StopCandidateRecording();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsRecordingCandidates();

    /// <summary>
    ///   <para>Initialise the start of the animation clip sampling.</para>
    /// </summary>
    public static void BeginSampling()
    {
      if (!AnimationMode.InAnimationMode())
        throw new InvalidOperationException("AnimationMode.BeginSampling may only be called in animation mode.  See AnimationMode.StartAnimationMode.");
      AnimationMode.Internal_BeginSampling();
    }

    /// <summary>
    ///   <para>Finish the sampling of the animation clip.</para>
    /// </summary>
    public static void EndSampling()
    {
      if (!AnimationMode.InAnimationMode())
        throw new InvalidOperationException("AnimationMode.EndSampling may only be called in animation mode.  See AnimationMode.StartAnimationMode.");
      AnimationMode.Internal_EndSampling();
    }

    /// <summary>
    ///   <para>Samples an AnimationClip on the object and also records any modified
    ///   properties in AnimationMode.</para>
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="clip"></param>
    /// <param name="time"></param>
    public static void SampleAnimationClip(GameObject gameObject, AnimationClip clip, float time)
    {
      if (!AnimationMode.InAnimationMode())
        throw new InvalidOperationException("AnimationMode.SampleAnimationClip may only be called in animation mode.  See AnimationMode.StartAnimationMode.");
      AnimationMode.Internal_SampleAnimationClip(gameObject, clip, time);
    }

    internal static void SampleCandidateClip(GameObject gameObject, AnimationClip clip, float time)
    {
      if (!AnimationMode.IsRecordingCandidates())
        throw new InvalidOperationException("AnimationMode.SampleCandidateClip may only be called when recording candidates.  See AnimationMode.StartAnimationMode.");
      AnimationMode.Internal_SampleCandidateClip(gameObject, clip, time);
    }

    /// <summary>
    ///   <para>Marks a property as currently being animated.</para>
    /// </summary>
    /// <param name="binding">Description of the animation clip curve being modified.</param>
    /// <param name="modification">Object property being modified.</param>
    /// <param name="keepPrefabOverride">Indicates whether to retain modifications when the targeted object is an instance of prefab.</param>
    public static void AddPropertyModification(EditorCurveBinding binding, PropertyModification modification, bool keepPrefabOverride)
    {
      if (!AnimationMode.InAnimationMode())
        throw new InvalidOperationException("AnimationMode.AddPropertyModification may only be called in animation mode.  See AnimationMode.StartAnimationMode.");
      AnimationMode.Internal_AddPropertyModification(binding, modification, keepPrefabOverride);
    }

    internal static void InitializePropertyModificationForGameObject(GameObject gameObject, AnimationClip clip)
    {
      if (!AnimationMode.InAnimationMode())
        throw new InvalidOperationException("AnimationMode.InitializePropertyModificationForGameObject may only be called in animation mode.  See AnimationMode.StartAnimationMode.");
      AnimationMode.Internal_InitializePropertyModificationForGameObject(gameObject, clip);
    }

    internal static void InitializePropertyModificationForObject(UnityEngine.Object target, AnimationClip clip)
    {
      if (!AnimationMode.InAnimationMode())
        throw new InvalidOperationException("AnimationMode.InitializePropertyModificationForObject may only be called in animation mode.  See AnimationMode.StartAnimationMode.");
      AnimationMode.Internal_InitializePropertyModificationForObject(target, clip);
    }

    internal static void RevertPropertyModificationsForGameObject(GameObject gameObject)
    {
      if (!AnimationMode.InAnimationMode())
        throw new InvalidOperationException("AnimationMode.RevertPropertyModificationsForGameObject may only be called in animation mode.  See AnimationMode.StartAnimationMode.");
      AnimationMode.Internal_RevertPropertyModificationsForGameObject(gameObject);
    }

    internal static void RevertPropertyModificationsForObject(UnityEngine.Object target)
    {
      if (!AnimationMode.InAnimationMode())
        throw new InvalidOperationException("AnimationMode.RevertPropertyModificationsForObject may only be called in animation mode.  See AnimationMode.StartAnimationMode.");
      AnimationMode.Internal_RevertPropertyModificationsForObject(target);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_InAnimationMode(UnityEngine.Object driver);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_InAnimationModeNoDriver();

    private static void Internal_AddCandidate(EditorCurveBinding binding, PropertyModification modification, bool keepPrefabOverride)
    {
      AnimationMode.INTERNAL_CALL_Internal_AddCandidate(ref binding, modification, keepPrefabOverride);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_AddCandidate(ref EditorCurveBinding binding, PropertyModification modification, bool keepPrefabOverride);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_StartCandidateRecording(UnityEngine.Object driver);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_BeginSampling();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_EndSampling();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SampleAnimationClip(GameObject gameObject, AnimationClip clip, float time);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SampleCandidateClip(GameObject gameObject, AnimationClip clip, float time);

    private static void Internal_AddPropertyModification(EditorCurveBinding binding, PropertyModification modification, bool keepPrefabOverride)
    {
      AnimationMode.INTERNAL_CALL_Internal_AddPropertyModification(ref binding, modification, keepPrefabOverride);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_AddPropertyModification(ref EditorCurveBinding binding, PropertyModification modification, bool keepPrefabOverride);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_InitializePropertyModificationForGameObject(GameObject gameObject, AnimationClip clip);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_InitializePropertyModificationForObject(UnityEngine.Object target, AnimationClip clip);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_RevertPropertyModificationsForGameObject(GameObject gameObject);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_RevertPropertyModificationsForObject(UnityEngine.Object target);
  }
}
