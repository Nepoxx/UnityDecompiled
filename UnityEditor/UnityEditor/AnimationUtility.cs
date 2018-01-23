// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Editor utility functions for modifying animation clips.</para>
  /// </summary>
  public sealed class AnimationUtility
  {
    /// <summary>
    ///   <para>Triggered when an animation curve inside an animation clip has been modified.</para>
    /// </summary>
    public static AnimationUtility.OnCurveWasModified onCurveWasModified;

    /// <summary>
    ///   <para>Returns the array of AnimationClips that are referenced in the Animation component.</para>
    /// </summary>
    /// <param name="component"></param>
    [Obsolete("GetAnimationClips(Animation) is deprecated. Use GetAnimationClips(GameObject) instead.")]
    public static AnimationClip[] GetAnimationClips(Animation component)
    {
      return AnimationUtility.GetAnimationClips(component.gameObject);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AnimationClip[] GetAnimationClips(GameObject gameObject);

    /// <summary>
    ///   <para>Sets the array of AnimationClips to be referenced in the Animation component.</para>
    /// </summary>
    /// <param name="animation"></param>
    /// <param name="clips"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetAnimationClips(Animation animation, AnimationClip[] clips);

    /// <summary>
    ///   <para>Returns all the animatable bindings that a specific game object has.</para>
    /// </summary>
    /// <param name="targetObject"></param>
    /// <param name="root"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern EditorCurveBinding[] GetAnimatableBindings(GameObject targetObject, GameObject root);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern EditorCurveBinding[] GetScriptableObjectAnimatableBindings(ScriptableObject scriptableObject);

    public static bool GetFloatValue(GameObject root, EditorCurveBinding binding, out float data)
    {
      return AnimationUtility.INTERNAL_CALL_GetFloatValue(root, ref binding, out data);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_GetFloatValue(GameObject root, ref EditorCurveBinding binding, out float data);

    public static System.Type GetEditorCurveValueType(GameObject root, EditorCurveBinding binding)
    {
      return AnimationUtility.INTERNAL_CALL_GetEditorCurveValueType(root, ref binding);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern System.Type INTERNAL_CALL_GetEditorCurveValueType(GameObject root, ref EditorCurveBinding binding);

    internal static System.Type GetScriptableObjectEditorCurveValueType(ScriptableObject scriptableObject, EditorCurveBinding binding)
    {
      return AnimationUtility.INTERNAL_CALL_GetScriptableObjectEditorCurveValueType(scriptableObject, ref binding);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern System.Type INTERNAL_CALL_GetScriptableObjectEditorCurveValueType(ScriptableObject scriptableObject, ref EditorCurveBinding binding);

    public static bool GetObjectReferenceValue(GameObject root, EditorCurveBinding binding, out UnityEngine.Object targetObject)
    {
      return AnimationUtility.INTERNAL_CALL_GetObjectReferenceValue(root, ref binding, out targetObject);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_GetObjectReferenceValue(GameObject root, ref EditorCurveBinding binding, out UnityEngine.Object targetObject);

    /// <summary>
    ///   <para>Returns the animated object that the binding is pointing to.</para>
    /// </summary>
    /// <param name="root"></param>
    /// <param name="binding"></param>
    public static UnityEngine.Object GetAnimatedObject(GameObject root, EditorCurveBinding binding)
    {
      return AnimationUtility.INTERNAL_CALL_GetAnimatedObject(root, ref binding);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern UnityEngine.Object INTERNAL_CALL_GetAnimatedObject(GameObject root, ref EditorCurveBinding binding);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern System.Type PropertyModificationToEditorCurveBinding(PropertyModification modification, GameObject gameObject, out EditorCurveBinding binding);

    internal static PropertyModification EditorCurveBindingToPropertyModification(EditorCurveBinding binding, GameObject gameObject)
    {
      return AnimationUtility.INTERNAL_CALL_EditorCurveBindingToPropertyModification(ref binding, gameObject);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern PropertyModification INTERNAL_CALL_EditorCurveBindingToPropertyModification(ref EditorCurveBinding binding, GameObject gameObject);

    /// <summary>
    ///   <para>Returns all the float curve bindings currently stored in the clip.</para>
    /// </summary>
    /// <param name="clip"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern EditorCurveBinding[] GetCurveBindings(AnimationClip clip);

    /// <summary>
    ///   <para>Returns all the object reference curve bindings currently stored in the clip.</para>
    /// </summary>
    /// <param name="clip"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern EditorCurveBinding[] GetObjectReferenceCurveBindings(AnimationClip clip);

    /// <summary>
    ///   <para>Return the object reference curve that the binding is pointing to.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="binding"></param>
    public static ObjectReferenceKeyframe[] GetObjectReferenceCurve(AnimationClip clip, EditorCurveBinding binding)
    {
      return AnimationUtility.INTERNAL_CALL_GetObjectReferenceCurve(clip, ref binding);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern ObjectReferenceKeyframe[] INTERNAL_CALL_GetObjectReferenceCurve(AnimationClip clip, ref EditorCurveBinding binding);

    private static void Internal_SetObjectReferenceCurve(AnimationClip clip, EditorCurveBinding binding, ObjectReferenceKeyframe[] keyframes)
    {
      AnimationUtility.INTERNAL_CALL_Internal_SetObjectReferenceCurve(clip, ref binding, keyframes);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_SetObjectReferenceCurve(AnimationClip clip, ref EditorCurveBinding binding, ObjectReferenceKeyframe[] keyframes);

    /// <summary>
    ///   <para>Return the float curve that the binding is pointing to.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="relativePath"></param>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <param name="binding"></param>
    public static AnimationCurve GetEditorCurve(AnimationClip clip, EditorCurveBinding binding)
    {
      return AnimationUtility.INTERNAL_CALL_GetEditorCurve(clip, ref binding);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AnimationCurve INTERNAL_CALL_GetEditorCurve(AnimationClip clip, ref EditorCurveBinding binding);

    private static void Internal_SetEditorCurve(AnimationClip clip, EditorCurveBinding binding, AnimationCurve curve, bool syncEditorCurve)
    {
      AnimationUtility.INTERNAL_CALL_Internal_SetEditorCurve(clip, ref binding, curve, syncEditorCurve);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_SetEditorCurve(AnimationClip clip, ref EditorCurveBinding binding, AnimationCurve curve, bool syncEditorCurve);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SyncEditorCurves(AnimationClip clip);

    [RequiredByNativeCode]
    private static void Internal_CallAnimationClipAwake(AnimationClip clip)
    {
      if (AnimationUtility.onCurveWasModified == null)
        return;
      AnimationUtility.onCurveWasModified(clip, new EditorCurveBinding(), AnimationUtility.CurveModifiedType.ClipModified);
    }

    /// <summary>
    ///   <para>Adds, modifies or removes an editor float curve in a given clip.</para>
    /// </summary>
    /// <param name="clip">The animation clip to which the curve will be added.</param>
    /// <param name="binding">The bindings which defines the path and the property of the curve.</param>
    /// <param name="curve">The curve to add. Setting this to null will remove the curve.</param>
    public static void SetEditorCurve(AnimationClip clip, EditorCurveBinding binding, AnimationCurve curve)
    {
      AnimationUtility.Internal_SetEditorCurve(clip, binding, curve, true);
      if (AnimationUtility.onCurveWasModified == null)
        return;
      AnimationUtility.onCurveWasModified(clip, binding, curve == null ? AnimationUtility.CurveModifiedType.CurveDeleted : AnimationUtility.CurveModifiedType.CurveModified);
    }

    internal static void SetEditorCurves(AnimationClip clip, EditorCurveBinding[] bindings, AnimationCurve[] curves)
    {
      if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (clip));
      if (curves == null)
        throw new ArgumentNullException(nameof (curves));
      if (bindings == null)
        throw new ArgumentNullException(nameof (bindings));
      if (bindings.Length != curves.Length)
        throw new ArgumentException("bindings and curves array sizes do not match");
      for (int index = 0; index < bindings.Length; ++index)
      {
        AnimationUtility.Internal_SetEditorCurve(clip, bindings[index], curves[index], false);
        if (AnimationUtility.onCurveWasModified != null)
          AnimationUtility.onCurveWasModified(clip, bindings[index], curves[index] == null ? AnimationUtility.CurveModifiedType.CurveDeleted : AnimationUtility.CurveModifiedType.CurveModified);
      }
      AnimationUtility.Internal_SyncEditorCurves(clip);
    }

    /// <summary>
    ///   <para>Adds, modifies or removes an object reference curve in a given clip.</para>
    /// </summary>
    /// <param name="keyframes">Setting this to null will remove the curve.</param>
    /// <param name="clip"></param>
    /// <param name="binding"></param>
    public static void SetObjectReferenceCurve(AnimationClip clip, EditorCurveBinding binding, ObjectReferenceKeyframe[] keyframes)
    {
      AnimationUtility.Internal_SetObjectReferenceCurve(clip, binding, keyframes);
      if (AnimationUtility.onCurveWasModified == null)
        return;
      AnimationUtility.onCurveWasModified(clip, binding, keyframes == null ? AnimationUtility.CurveModifiedType.CurveDeleted : AnimationUtility.CurveModifiedType.CurveModified);
    }

    private static void VerifyCurve(AnimationCurve curve)
    {
      if (curve == null)
        throw new ArgumentNullException(nameof (curve));
    }

    private static void VerifyCurveAndKeyframeIndex(AnimationCurve curve, int index)
    {
      AnimationUtility.VerifyCurve(curve);
      if (index < 0 || index >= curve.length)
        throw new ArgumentOutOfRangeException(nameof (index), string.Format("index {0} must be in the range of 0 to {1}.", (object) index, (object) (curve.length - 1)));
    }

    internal static void UpdateTangentsFromModeSurrounding(AnimationCurve curve, int index)
    {
      AnimationUtility.VerifyCurveAndKeyframeIndex(curve, index);
      AnimationUtility.UpdateTangentsFromModeSurroundingInternal(curve, index);
    }

    internal static void UpdateTangentsFromMode(AnimationCurve curve)
    {
      AnimationUtility.VerifyCurve(curve);
      AnimationUtility.UpdateTangentsFromModeInternal(curve);
    }

    /// <summary>
    ///   <para>Retrieve the left tangent mode of the keyframe at specified index.</para>
    /// </summary>
    /// <param name="curve">Curve to query.</param>
    /// <param name="index">Keyframe index.</param>
    /// <returns>
    ///   <para>Tangent mode at specified index.</para>
    /// </returns>
    public static AnimationUtility.TangentMode GetKeyLeftTangentMode(AnimationCurve curve, int index)
    {
      AnimationUtility.VerifyCurveAndKeyframeIndex(curve, index);
      return AnimationUtility.GetKeyLeftTangentModeInternal(curve, index);
    }

    /// <summary>
    ///   <para>Retrieve the right tangent mode of the keyframe at specified index.</para>
    /// </summary>
    /// <param name="curve">Curve to query.</param>
    /// <param name="index">Keyframe index.</param>
    /// <returns>
    ///   <para>Tangent mode at specified index.</para>
    /// </returns>
    public static AnimationUtility.TangentMode GetKeyRightTangentMode(AnimationCurve curve, int index)
    {
      AnimationUtility.VerifyCurveAndKeyframeIndex(curve, index);
      return AnimationUtility.GetKeyRightTangentModeInternal(curve, index);
    }

    internal static bool GetKeyBroken(Keyframe key)
    {
      return AnimationUtility.INTERNAL_CALL_GetKeyBroken(ref key);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_GetKeyBroken(ref Keyframe key);

    internal static void SetKeyLeftTangentMode(ref Keyframe key, AnimationUtility.TangentMode tangentMode)
    {
      AnimationUtility.INTERNAL_CALL_SetKeyLeftTangentMode(ref key, tangentMode);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetKeyLeftTangentMode(ref Keyframe key, AnimationUtility.TangentMode tangentMode);

    internal static void SetKeyRightTangentMode(ref Keyframe key, AnimationUtility.TangentMode tangentMode)
    {
      AnimationUtility.INTERNAL_CALL_SetKeyRightTangentMode(ref key, tangentMode);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetKeyRightTangentMode(ref Keyframe key, AnimationUtility.TangentMode tangentMode);

    internal static void SetKeyBroken(ref Keyframe key, bool broken)
    {
      AnimationUtility.INTERNAL_CALL_SetKeyBroken(ref key, broken);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetKeyBroken(ref Keyframe key, bool broken);

    /// <summary>
    ///   <para>Change the specified keyframe broken tangent flag.</para>
    /// </summary>
    /// <param name="curve">The curve to modify.</param>
    /// <param name="index">Keyframe index.</param>
    /// <param name="broken">Broken flag.</param>
    public static void SetKeyBroken(AnimationCurve curve, int index, bool broken)
    {
      AnimationUtility.VerifyCurveAndKeyframeIndex(curve, index);
      AnimationUtility.SetKeyBrokenInternal(curve, index, broken);
    }

    public static void SetKeyLeftTangentMode(AnimationCurve curve, int index, AnimationUtility.TangentMode tangentMode)
    {
      AnimationUtility.VerifyCurveAndKeyframeIndex(curve, index);
      AnimationUtility.SetKeyLeftTangentModeInternal(curve, index, tangentMode);
    }

    public static void SetKeyRightTangentMode(AnimationCurve curve, int index, AnimationUtility.TangentMode tangentMode)
    {
      AnimationUtility.VerifyCurveAndKeyframeIndex(curve, index);
      AnimationUtility.SetKeyRightTangentModeInternal(curve, index, tangentMode);
    }

    /// <summary>
    ///   <para>Retrieve the specified keyframe broken tangent flag.</para>
    /// </summary>
    /// <param name="curve">Curve to query.</param>
    /// <param name="index">Keyframe index.</param>
    /// <returns>
    ///   <para>Broken flag at specified index.</para>
    /// </returns>
    public static bool GetKeyBroken(AnimationCurve curve, int index)
    {
      AnimationUtility.VerifyCurveAndKeyframeIndex(curve, index);
      return AnimationUtility.GetKeyBrokenInternal(curve, index);
    }

    internal static AnimationUtility.TangentMode GetKeyLeftTangentMode(Keyframe key)
    {
      return AnimationUtility.INTERNAL_CALL_GetKeyLeftTangentMode(ref key);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AnimationUtility.TangentMode INTERNAL_CALL_GetKeyLeftTangentMode(ref Keyframe key);

    internal static AnimationUtility.TangentMode GetKeyRightTangentMode(Keyframe key)
    {
      return AnimationUtility.INTERNAL_CALL_GetKeyRightTangentMode(ref key);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AnimationUtility.TangentMode INTERNAL_CALL_GetKeyRightTangentMode(ref Keyframe key);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void UpdateTangentsFromModeSurroundingInternal(AnimationCurve curve, int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void UpdateTangentsFromModeInternal(AnimationCurve curve);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AnimationUtility.TangentMode GetKeyLeftTangentModeInternal(AnimationCurve curve, int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AnimationUtility.TangentMode GetKeyRightTangentModeInternal(AnimationCurve curve, int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetKeyBrokenInternal(AnimationCurve curve, int index, bool broken);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetKeyLeftTangentModeInternal(AnimationCurve curve, int index, AnimationUtility.TangentMode tangentMode);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetKeyRightTangentModeInternal(AnimationCurve curve, int index, AnimationUtility.TangentMode tangentMode);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool GetKeyBrokenInternal(AnimationCurve curve, int index);

    /// <summary>
    ///   <para>Retrieves all curves from a specific animation clip.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="includeCurveData"></param>
    [ExcludeFromDocs]
    [Obsolete("GetAllCurves is deprecated. Use GetCurveBindings and GetObjectReferenceCurveBindings instead.")]
    public static AnimationClipCurveData[] GetAllCurves(AnimationClip clip)
    {
      bool includeCurveData = true;
      return AnimationUtility.GetAllCurves(clip, includeCurveData);
    }

    /// <summary>
    ///   <para>Retrieves all curves from a specific animation clip.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="includeCurveData"></param>
    [Obsolete("GetAllCurves is deprecated. Use GetCurveBindings and GetObjectReferenceCurveBindings instead.")]
    public static AnimationClipCurveData[] GetAllCurves(AnimationClip clip, [DefaultValue("true")] bool includeCurveData)
    {
      EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
      AnimationClipCurveData[] animationClipCurveDataArray = new AnimationClipCurveData[curveBindings.Length];
      for (int index = 0; index < animationClipCurveDataArray.Length; ++index)
      {
        animationClipCurveDataArray[index] = new AnimationClipCurveData(curveBindings[index]);
        if (includeCurveData)
          animationClipCurveDataArray[index].curve = AnimationUtility.GetEditorCurve(clip, curveBindings[index]);
      }
      return animationClipCurveDataArray;
    }

    [Obsolete("This overload is deprecated. Use the one with EditorCurveBinding instead.")]
    public static bool GetFloatValue(GameObject root, string relativePath, System.Type type, string propertyName, out float data)
    {
      return AnimationUtility.GetFloatValue(root, EditorCurveBinding.FloatCurve(relativePath, type, propertyName), out data);
    }

    [Obsolete("This overload is deprecated. Use the one with EditorCurveBinding instead.")]
    public static void SetEditorCurve(AnimationClip clip, string relativePath, System.Type type, string propertyName, AnimationCurve curve)
    {
      AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(relativePath, type, propertyName), curve);
    }

    /// <summary>
    ///   <para>Return the float curve that the binding is pointing to.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="relativePath"></param>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <param name="binding"></param>
    [Obsolete("This overload is deprecated. Use the one with EditorCurveBinding instead.")]
    public static AnimationCurve GetEditorCurve(AnimationClip clip, string relativePath, System.Type type, string propertyName)
    {
      return AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(relativePath, type, propertyName));
    }

    /// <summary>
    ///   <para>Retrieves all animation events associated with the animation clip.</para>
    /// </summary>
    /// <param name="clip"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AnimationEvent[] GetAnimationEvents(AnimationClip clip);

    /// <summary>
    ///   <para>Replaces all animation events in the animation clip.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="events"></param>
    public static void SetAnimationEvents(AnimationClip clip, AnimationEvent[] events)
    {
      if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (clip));
      if (events == null)
        throw new ArgumentNullException(nameof (events));
      AnimationUtility.Internal_SetAnimationEvents(clip, events);
      if (AnimationUtility.onCurveWasModified == null)
        return;
      AnimationUtility.onCurveWasModified(clip, new EditorCurveBinding(), AnimationUtility.CurveModifiedType.ClipModified);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetAnimationEvents(AnimationClip clip, AnimationEvent[] events);

    /// <summary>
    ///   <para>Calculates path from root transform to target transform.</para>
    /// </summary>
    /// <param name="targetTransform"></param>
    /// <param name="root"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string CalculateTransformPath(Transform targetTransform, Transform root);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AnimationClipSettings GetAnimationClipSettings(AnimationClip clip);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetAnimationClipSettings(AnimationClip clip, AnimationClipSettings srcClipInfo);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetAnimationClipSettingsNoDirty(AnimationClip clip, AnimationClipSettings srcClipInfo);

    /// <summary>
    ///   <para>Set the additive reference pose from referenceClip at time for animation clip clip.</para>
    /// </summary>
    /// <param name="clip">The animation clip to be used.</param>
    /// <param name="referenceClip">The animation clip containing the reference pose.</param>
    /// <param name="time">Time that defines the reference pose in referenceClip.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetAdditiveReferencePose(AnimationClip clip, AnimationClip referenceClip, float time);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsValidOptimizedPolynomialCurve(AnimationCurve curve);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ConstrainToPolynomialCurve(AnimationCurve curve);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetMaxNumPolynomialSegmentsSupported();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern AnimationUtility.PolynomialValid IsValidPolynomialCurve(AnimationCurve curve);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern AnimationClipStats GetAnimationClipStats(AnimationClip clip);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool GetGenerateMotionCurves(AnimationClip clip);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetGenerateMotionCurves(AnimationClip clip, bool value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasGenericRootTransform(AnimationClip clip);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasMotionFloatCurves(AnimationClip clip);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasMotionCurves(AnimationClip clip);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasRootCurves(AnimationClip clip);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool AmbiguousBinding(string path, int classID, Transform root);

    internal static Vector3 GetClosestEuler(Quaternion q, Vector3 eulerHint, RotationOrder rotationOrder)
    {
      Vector3 vector3;
      AnimationUtility.INTERNAL_CALL_GetClosestEuler(ref q, ref eulerHint, rotationOrder, out vector3);
      return vector3;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetClosestEuler(ref Quaternion q, ref Vector3 eulerHint, RotationOrder rotationOrder, out Vector3 value);

    [Obsolete("Use AnimationMode.InAnimationMode instead")]
    public static bool InAnimationMode()
    {
      return AnimationMode.InAnimationMode();
    }

    [Obsolete("Use AnimationMode.StartAnimationmode instead")]
    public static void StartAnimationMode(UnityEngine.Object[] objects)
    {
      Debug.LogWarning((object) "AnimationUtility.StartAnimationMode is deprecated. Use AnimationMode.StartAnimationMode with the new APIs. The objects passed to this function will no longer be reverted automatically. See AnimationMode.AddPropertyModification");
      AnimationMode.StartAnimationMode();
    }

    [Obsolete("Use AnimationMode.StopAnimationMode instead")]
    public static void StopAnimationMode()
    {
      AnimationMode.StopAnimationMode();
    }

    [Obsolete("SetAnimationType is no longer supported", true)]
    public static void SetAnimationType(AnimationClip clip, ModelImporterAnimationType type)
    {
    }

    /// <summary>
    ///   <para>Describes the type of modification that caused OnCurveWasModified to fire.</para>
    /// </summary>
    public enum CurveModifiedType
    {
      CurveDeleted,
      CurveModified,
      ClipModified,
    }

    /// <summary>
    ///   <para>Triggered when an animation curve inside an animation clip has been modified.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="binding"></param>
    /// <param name="deleted"></param>
    public delegate void OnCurveWasModified(AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType deleted);

    /// <summary>
    ///   <para>Tangent constraints on Keyframe.</para>
    /// </summary>
    public enum TangentMode
    {
      Free,
      Auto,
      Linear,
      Constant,
      ClampedAuto,
    }

    internal enum PolynomialValid
    {
      Valid,
      InvalidPreWrapMode,
      InvalidPostWrapMode,
      TooManySegments,
    }
  }
}
