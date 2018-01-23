// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationRecording
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AnimationRecording
  {
    private const string kLocalRotation = "m_LocalRotation";
    private const string kLocalEulerAnglesHint = "m_LocalEulerAnglesHint";

    private static bool HasAnyRecordableModifications(GameObject root, UndoPropertyModification[] modifications)
    {
      for (int index = 0; index < modifications.Length; ++index)
      {
        EditorCurveBinding binding;
        if (AnimationUtility.PropertyModificationToEditorCurveBinding(modifications[index].previousValue, root, out binding) != null)
          return true;
      }
      return false;
    }

    private static PropertyModification FindPropertyModification(GameObject root, UndoPropertyModification[] modifications, EditorCurveBinding binding)
    {
      for (int index = 0; index < modifications.Length; ++index)
      {
        EditorCurveBinding binding1;
        AnimationUtility.PropertyModificationToEditorCurveBinding(modifications[index].previousValue, root, out binding1);
        if (binding1 == binding)
          return modifications[index].previousValue;
      }
      return (PropertyModification) null;
    }

    private static PropertyModification CreateDummyPropertyModification(GameObject root, PropertyModification baseProperty, EditorCurveBinding binding)
    {
      PropertyModification propertyModification = new PropertyModification();
      propertyModification.target = baseProperty.target;
      propertyModification.propertyPath = binding.propertyName;
      object currentValue = CurveBindingUtility.GetCurrentValue(root, binding);
      if (binding.isPPtrCurve)
        propertyModification.objectReference = (UnityEngine.Object) currentValue;
      else
        propertyModification.value = ((float) currentValue).ToString();
      return propertyModification;
    }

    private static UndoPropertyModification[] FilterModifications(IAnimationRecordingState state, ref UndoPropertyModification[] modifications)
    {
      AnimationClip activeAnimationClip = state.activeAnimationClip;
      GameObject activeRootGameObject = state.activeRootGameObject;
      List<UndoPropertyModification> propertyModificationList1 = new List<UndoPropertyModification>();
      List<UndoPropertyModification> propertyModificationList2 = new List<UndoPropertyModification>();
      for (int index = 0; index < modifications.Length; ++index)
      {
        UndoPropertyModification propertyModification = modifications[index];
        PropertyModification previousValue = propertyModification.previousValue;
        if (state.DiscardModification(previousValue))
        {
          propertyModificationList1.Add(propertyModification);
        }
        else
        {
          EditorCurveBinding binding = new EditorCurveBinding();
          if (AnimationUtility.PropertyModificationToEditorCurveBinding(previousValue, activeRootGameObject, out binding) != null)
            propertyModificationList2.Add(propertyModification);
          else
            propertyModificationList1.Add(propertyModification);
        }
      }
      if (propertyModificationList1.Count > 0)
        modifications = propertyModificationList2.ToArray();
      return propertyModificationList1.ToArray();
    }

    private static void CollectRotationModifications(IAnimationRecordingState state, ref UndoPropertyModification[] modifications, ref Dictionary<object, AnimationRecording.RotationModification> rotationModifications)
    {
      List<UndoPropertyModification> propertyModificationList = new List<UndoPropertyModification>();
      foreach (UndoPropertyModification propertyModification in modifications)
      {
        PropertyModification previousValue = propertyModification.previousValue;
        if (!(previousValue.target is Transform))
        {
          propertyModificationList.Add(propertyModification);
        }
        else
        {
          EditorCurveBinding binding = new EditorCurveBinding();
          AnimationUtility.PropertyModificationToEditorCurveBinding(previousValue, state.activeRootGameObject, out binding);
          if (binding.propertyName.StartsWith("m_LocalRotation"))
          {
            AnimationRecording.RotationModification rotationModification;
            if (!rotationModifications.TryGetValue((object) previousValue.target, out rotationModification))
            {
              rotationModification = new AnimationRecording.RotationModification();
              rotationModifications[(object) previousValue.target] = rotationModification;
            }
            if (binding.propertyName.EndsWith("x"))
              rotationModification.x = propertyModification;
            else if (binding.propertyName.EndsWith("y"))
              rotationModification.y = propertyModification;
            else if (binding.propertyName.EndsWith("z"))
              rotationModification.z = propertyModification;
            else if (binding.propertyName.EndsWith("w"))
              rotationModification.w = propertyModification;
            rotationModification.lastQuatModification = propertyModification;
          }
          else if (previousValue.propertyPath.StartsWith("m_LocalEulerAnglesHint"))
          {
            AnimationRecording.RotationModification rotationModification;
            if (!rotationModifications.TryGetValue((object) previousValue.target, out rotationModification))
            {
              rotationModification = new AnimationRecording.RotationModification();
              rotationModifications[(object) previousValue.target] = rotationModification;
            }
            rotationModification.useEuler = true;
            if (previousValue.propertyPath.EndsWith("x"))
              rotationModification.eulerX = propertyModification;
            else if (previousValue.propertyPath.EndsWith("y"))
              rotationModification.eulerY = propertyModification;
            else if (previousValue.propertyPath.EndsWith("z"))
              rotationModification.eulerZ = propertyModification;
          }
          else
            propertyModificationList.Add(propertyModification);
        }
      }
      if (rotationModifications.Count <= 0)
        return;
      modifications = propertyModificationList.ToArray();
    }

    private static void DiscardRotationModification(AnimationRecording.RotationModification rotationModification, ref List<UndoPropertyModification> discardedModifications)
    {
      if (rotationModification.x.currentValue != null)
        discardedModifications.Add(rotationModification.x);
      if (rotationModification.y.currentValue != null)
        discardedModifications.Add(rotationModification.y);
      if (rotationModification.z.currentValue != null)
        discardedModifications.Add(rotationModification.z);
      if (rotationModification.w.currentValue != null)
        discardedModifications.Add(rotationModification.w);
      if (rotationModification.eulerX.currentValue != null)
        discardedModifications.Add(rotationModification.eulerX);
      if (rotationModification.eulerY.currentValue != null)
        discardedModifications.Add(rotationModification.eulerY);
      if (rotationModification.eulerZ.currentValue == null)
        return;
      discardedModifications.Add(rotationModification.eulerZ);
    }

    private static UndoPropertyModification[] FilterRotationModifications(IAnimationRecordingState state, ref Dictionary<object, AnimationRecording.RotationModification> rotationModifications)
    {
      AnimationClip activeAnimationClip = state.activeAnimationClip;
      GameObject activeRootGameObject = state.activeRootGameObject;
      List<object> objectList = new List<object>();
      List<UndoPropertyModification> discardedModifications = new List<UndoPropertyModification>();
      foreach (KeyValuePair<object, AnimationRecording.RotationModification> keyValuePair in rotationModifications)
      {
        AnimationRecording.RotationModification rotationModification = keyValuePair.Value;
        if (state.DiscardModification(rotationModification.lastQuatModification.currentValue))
        {
          AnimationRecording.DiscardRotationModification(rotationModification, ref discardedModifications);
          objectList.Add(keyValuePair.Key);
        }
        else
        {
          EditorCurveBinding binding = new EditorCurveBinding();
          if (AnimationUtility.PropertyModificationToEditorCurveBinding(rotationModification.lastQuatModification.currentValue, activeRootGameObject, out binding) == null)
          {
            AnimationRecording.DiscardRotationModification(rotationModification, ref discardedModifications);
            objectList.Add(keyValuePair.Key);
          }
        }
      }
      foreach (object key in objectList)
        rotationModifications.Remove(key);
      return discardedModifications.ToArray();
    }

    private static void AddRotationPropertyModification(IAnimationRecordingState state, EditorCurveBinding baseBinding, UndoPropertyModification modification)
    {
      if (modification.previousValue == null)
        return;
      EditorCurveBinding binding = baseBinding;
      binding.propertyName = modification.previousValue.propertyPath;
      state.AddPropertyModification(binding, modification.previousValue, modification.keepPrefabOverride);
    }

    private static UndoPropertyModification[] ProcessRotationModifications(IAnimationRecordingState state, ref UndoPropertyModification[] modifications)
    {
      Dictionary<object, AnimationRecording.RotationModification> rotationModifications = new Dictionary<object, AnimationRecording.RotationModification>();
      AnimationRecording.CollectRotationModifications(state, ref modifications, ref rotationModifications);
      UndoPropertyModification[] propertyModificationArray = AnimationRecording.FilterRotationModifications(state, ref rotationModifications);
      foreach (KeyValuePair<object, AnimationRecording.RotationModification> keyValuePair in rotationModifications)
      {
        AnimationRecording.RotationModification rotationModification = keyValuePair.Value;
        Transform key = keyValuePair.Key as Transform;
        if (!((UnityEngine.Object) key == (UnityEngine.Object) null))
        {
          EditorCurveBinding binding = new EditorCurveBinding();
          System.Type editorCurveBinding = AnimationUtility.PropertyModificationToEditorCurveBinding(rotationModification.lastQuatModification.currentValue, state.activeRootGameObject, out binding);
          if (editorCurveBinding != null)
          {
            AnimationRecording.AddRotationPropertyModification(state, binding, rotationModification.x);
            AnimationRecording.AddRotationPropertyModification(state, binding, rotationModification.y);
            AnimationRecording.AddRotationPropertyModification(state, binding, rotationModification.z);
            AnimationRecording.AddRotationPropertyModification(state, binding, rotationModification.w);
            Quaternion localRotation1 = key.localRotation;
            Quaternion localRotation2 = key.localRotation;
            object outObject1;
            if (AnimationRecording.ValueFromPropertyModification(rotationModification.x.previousValue, binding, out outObject1))
              localRotation1.x = (float) outObject1;
            object outObject2;
            if (AnimationRecording.ValueFromPropertyModification(rotationModification.y.previousValue, binding, out outObject2))
              localRotation1.y = (float) outObject2;
            object outObject3;
            if (AnimationRecording.ValueFromPropertyModification(rotationModification.z.previousValue, binding, out outObject3))
              localRotation1.z = (float) outObject3;
            object outObject4;
            if (AnimationRecording.ValueFromPropertyModification(rotationModification.w.previousValue, binding, out outObject4))
              localRotation1.w = (float) outObject4;
            if (AnimationRecording.ValueFromPropertyModification(rotationModification.x.currentValue, binding, out outObject1))
              localRotation2.x = (float) outObject1;
            if (AnimationRecording.ValueFromPropertyModification(rotationModification.y.currentValue, binding, out outObject2))
              localRotation2.y = (float) outObject2;
            if (AnimationRecording.ValueFromPropertyModification(rotationModification.z.currentValue, binding, out outObject3))
              localRotation2.z = (float) outObject3;
            if (AnimationRecording.ValueFromPropertyModification(rotationModification.w.currentValue, binding, out outObject4))
              localRotation2.w = (float) outObject4;
            if (rotationModification.useEuler)
            {
              AnimationRecording.AddRotationPropertyModification(state, binding, rotationModification.eulerX);
              AnimationRecording.AddRotationPropertyModification(state, binding, rotationModification.eulerY);
              AnimationRecording.AddRotationPropertyModification(state, binding, rotationModification.eulerZ);
              Vector3 vector3_1 = key.GetLocalEulerAngles(RotationOrder.OrderZXY);
              Vector3 vector3_2 = vector3_1;
              object outObject5;
              if (AnimationRecording.ValueFromPropertyModification(rotationModification.eulerX.previousValue, binding, out outObject5))
                vector3_1.x = (float) outObject5;
              object outObject6;
              if (AnimationRecording.ValueFromPropertyModification(rotationModification.eulerY.previousValue, binding, out outObject6))
                vector3_1.y = (float) outObject6;
              object outObject7;
              if (AnimationRecording.ValueFromPropertyModification(rotationModification.eulerZ.previousValue, binding, out outObject7))
                vector3_1.z = (float) outObject7;
              if (AnimationRecording.ValueFromPropertyModification(rotationModification.eulerX.currentValue, binding, out outObject5))
                vector3_2.x = (float) outObject5;
              if (AnimationRecording.ValueFromPropertyModification(rotationModification.eulerY.currentValue, binding, out outObject6))
                vector3_2.y = (float) outObject6;
              if (AnimationRecording.ValueFromPropertyModification(rotationModification.eulerZ.currentValue, binding, out outObject7))
                vector3_2.z = (float) outObject7;
              vector3_1 = AnimationUtility.GetClosestEuler(localRotation1, vector3_1, RotationOrder.OrderZXY);
              vector3_2 = AnimationUtility.GetClosestEuler(localRotation2, vector3_2, RotationOrder.OrderZXY);
              AnimationRecording.AddRotationKey(state, binding, editorCurveBinding, vector3_1, vector3_2);
            }
            else
            {
              Vector3 localEulerAngles = key.GetLocalEulerAngles(RotationOrder.OrderZXY);
              Vector3 closestEuler1 = AnimationUtility.GetClosestEuler(localRotation1, localEulerAngles, RotationOrder.OrderZXY);
              Vector3 closestEuler2 = AnimationUtility.GetClosestEuler(localRotation2, localEulerAngles, RotationOrder.OrderZXY);
              AnimationRecording.AddRotationKey(state, binding, editorCurveBinding, closestEuler1, closestEuler2);
            }
          }
        }
      }
      return propertyModificationArray;
    }

    public static UndoPropertyModification[] ProcessModifications(IAnimationRecordingState state, UndoPropertyModification[] modifications)
    {
      AnimationClip activeAnimationClip = state.activeAnimationClip;
      GameObject activeRootGameObject = state.activeRootGameObject;
      Animator component = activeRootGameObject.GetComponent<Animator>();
      UndoPropertyModification[] propertyModificationArray = AnimationRecording.FilterModifications(state, ref modifications);
      for (int index1 = 0; index1 < modifications.Length; ++index1)
      {
        EditorCurveBinding binding = new EditorCurveBinding();
        PropertyModification previousValue = modifications[index1].previousValue;
        System.Type editorCurveBinding = AnimationUtility.PropertyModificationToEditorCurveBinding(previousValue, activeRootGameObject, out binding);
        if (editorCurveBinding != null)
        {
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.isHuman && (binding.type == typeof (Transform) && (UnityEngine.Object) component.gameObject.transform != previousValue.target) && component.IsBoneTransform(previousValue.target as Transform))
          {
            Debug.LogWarning((object) "Keyframing for humanoid rig is not supported!", (UnityEngine.Object) (previousValue.target as Transform));
          }
          else
          {
            EditorCurveBinding[] editorCurveBindingArray = RotationCurveInterpolation.RemapAnimationBindingForAddKey(binding, activeAnimationClip);
            if (editorCurveBindingArray != null)
            {
              for (int index2 = 0; index2 < editorCurveBindingArray.Length; ++index2)
              {
                PropertyModification propertyModification = AnimationRecording.FindPropertyModification(activeRootGameObject, modifications, editorCurveBindingArray[index2]) ?? AnimationRecording.CreateDummyPropertyModification(activeRootGameObject, previousValue, editorCurveBindingArray[index2]);
                state.AddPropertyModification(editorCurveBindingArray[index2], propertyModification, modifications[index1].keepPrefabOverride);
                AnimationRecording.AddKey(state, editorCurveBindingArray[index2], editorCurveBinding, propertyModification);
              }
            }
            else
            {
              state.AddPropertyModification(binding, previousValue, modifications[index1].keepPrefabOverride);
              AnimationRecording.AddKey(state, binding, editorCurveBinding, previousValue);
            }
          }
        }
      }
      return propertyModificationArray;
    }

    public static UndoPropertyModification[] Process(IAnimationRecordingState state, UndoPropertyModification[] modifications)
    {
      GameObject activeRootGameObject = state.activeRootGameObject;
      if ((UnityEngine.Object) activeRootGameObject == (UnityEngine.Object) null || !AnimationRecording.HasAnyRecordableModifications(activeRootGameObject, modifications))
        return modifications;
      return ((IEnumerable<UndoPropertyModification>) AnimationRecording.ProcessRotationModifications(state, ref modifications)).Concat<UndoPropertyModification>((IEnumerable<UndoPropertyModification>) AnimationRecording.ProcessModifications(state, modifications)).ToArray<UndoPropertyModification>();
    }

    private static bool ValueFromPropertyModification(PropertyModification modification, EditorCurveBinding binding, out object outObject)
    {
      if (modification == null)
      {
        outObject = (object) null;
        return false;
      }
      if (binding.isPPtrCurve)
      {
        outObject = (object) modification.objectReference;
        return true;
      }
      float result;
      if (float.TryParse(modification.value, out result))
      {
        outObject = (object) result;
        return true;
      }
      outObject = (object) null;
      return false;
    }

    private static void AddKey(IAnimationRecordingState state, EditorCurveBinding binding, System.Type type, PropertyModification modification)
    {
      GameObject activeRootGameObject = state.activeRootGameObject;
      AnimationClip activeAnimationClip = state.activeAnimationClip;
      if ((activeAnimationClip.hideFlags & HideFlags.NotEditable) != HideFlags.None)
        return;
      AnimationWindowCurve curve = new AnimationWindowCurve(activeAnimationClip, binding, type);
      object currentValue = CurveBindingUtility.GetCurrentValue(activeRootGameObject, binding);
      if (state.addZeroFrame && curve.length == 0)
      {
        object outObject = (object) null;
        if (!AnimationRecording.ValueFromPropertyModification(modification, binding, out outObject))
          outObject = currentValue;
        if (state.currentFrame != 0)
          AnimationWindowUtility.AddKeyframeToCurve(curve, outObject, type, AnimationKeyTime.Frame(0, activeAnimationClip.frameRate));
      }
      AnimationWindowUtility.AddKeyframeToCurve(curve, currentValue, type, AnimationKeyTime.Frame(state.currentFrame, activeAnimationClip.frameRate));
      state.SaveCurve(curve);
    }

    public static void SaveModifiedCurve(AnimationWindowCurve curve, AnimationClip clip)
    {
      curve.m_Keyframes.Sort((Comparison<AnimationWindowKeyframe>) ((a, b) => a.time.CompareTo(b.time)));
      if (curve.isPPtrCurve)
      {
        ObjectReferenceKeyframe[] keyframes = curve.ToObjectCurve();
        if (keyframes.Length == 0)
          keyframes = (ObjectReferenceKeyframe[]) null;
        AnimationUtility.SetObjectReferenceCurve(clip, curve.binding, keyframes);
      }
      else
      {
        AnimationCurve curve1 = curve.ToAnimationCurve();
        if (curve1.keys.Length == 0)
          curve1 = (AnimationCurve) null;
        else
          QuaternionCurveTangentCalculation.UpdateTangentsFromMode(curve1, clip, curve.binding);
        AnimationUtility.SetEditorCurve(clip, curve.binding, curve1);
      }
    }

    private static void AddRotationKey(IAnimationRecordingState state, EditorCurveBinding binding, System.Type type, Vector3 previousEulerAngles, Vector3 currentEulerAngles)
    {
      AnimationClip activeAnimationClip = state.activeAnimationClip;
      if ((activeAnimationClip.hideFlags & HideFlags.NotEditable) != HideFlags.None)
        return;
      EditorCurveBinding[] editorCurveBindingArray = RotationCurveInterpolation.RemapAnimationBindingForRotationAddKey(binding, activeAnimationClip);
      for (int index = 0; index < 3; ++index)
      {
        AnimationWindowCurve curve = new AnimationWindowCurve(activeAnimationClip, editorCurveBindingArray[index], type);
        if (state.addZeroFrame && curve.length == 0 && state.currentFrame != 0)
          AnimationWindowUtility.AddKeyframeToCurve(curve, (object) previousEulerAngles[index], type, AnimationKeyTime.Frame(0, activeAnimationClip.frameRate));
        AnimationWindowUtility.AddKeyframeToCurve(curve, (object) currentEulerAngles[index], type, AnimationKeyTime.Frame(state.currentFrame, activeAnimationClip.frameRate));
        state.SaveCurve(curve);
      }
    }

    internal class RotationModification
    {
      public bool useEuler = false;
      public UndoPropertyModification x;
      public UndoPropertyModification y;
      public UndoPropertyModification z;
      public UndoPropertyModification w;
      public UndoPropertyModification lastQuatModification;
      public UndoPropertyModification eulerX;
      public UndoPropertyModification eulerY;
      public UndoPropertyModification eulerZ;
    }
  }
}
