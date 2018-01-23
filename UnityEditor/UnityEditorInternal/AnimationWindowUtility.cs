// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
  internal static class AnimationWindowUtility
  {
    internal static string s_LastPathUsedForNewClip;

    public static void CreateDefaultCurves(AnimationWindowState state, AnimationWindowSelectionItem selectionItem, EditorCurveBinding[] properties)
    {
      properties = RotationCurveInterpolation.ConvertRotationPropertiesToDefaultInterpolation(selectionItem.animationClip, properties);
      foreach (EditorCurveBinding property in properties)
        state.SaveCurve(AnimationWindowUtility.CreateDefaultCurve(selectionItem, property));
    }

    public static AnimationWindowCurve CreateDefaultCurve(AnimationWindowSelectionItem selectionItem, EditorCurveBinding binding)
    {
      AnimationClip animationClip = selectionItem.animationClip;
      System.Type editorCurveValueType = selectionItem.GetEditorCurveValueType(binding);
      AnimationWindowCurve curve = new AnimationWindowCurve(animationClip, binding, editorCurveValueType);
      object currentValue = CurveBindingUtility.GetCurrentValue(selectionItem.rootGameObject, binding);
      if ((double) animationClip.length == 0.0)
      {
        AnimationWindowUtility.AddKeyframeToCurve(curve, currentValue, editorCurveValueType, AnimationKeyTime.Time(0.0f, animationClip.frameRate));
      }
      else
      {
        AnimationWindowUtility.AddKeyframeToCurve(curve, currentValue, editorCurveValueType, AnimationKeyTime.Time(0.0f, animationClip.frameRate));
        AnimationWindowUtility.AddKeyframeToCurve(curve, currentValue, editorCurveValueType, AnimationKeyTime.Time(animationClip.length, animationClip.frameRate));
      }
      return curve;
    }

    public static bool ShouldShowAnimationWindowCurve(EditorCurveBinding curveBinding)
    {
      if (AnimationWindowUtility.IsTransformType(curveBinding.type))
        return !curveBinding.propertyName.EndsWith(".w");
      return true;
    }

    public static bool IsNodeLeftOverCurve(AnimationWindowHierarchyNode node)
    {
      if (node.binding.HasValue && node.curves.Length > 0)
      {
        AnimationWindowSelectionItem selectionBinding = node.curves[0].selectionBinding;
        if ((UnityEngine.Object) selectionBinding != (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) selectionBinding.rootGameObject == (UnityEngine.Object) null && (UnityEngine.Object) selectionBinding.scriptableObject == (UnityEngine.Object) null)
            return false;
          return selectionBinding.GetEditorCurveValueType(node.binding.Value) == null;
        }
      }
      if (node.hasChildren)
      {
        using (List<TreeViewItem>.Enumerator enumerator = node.children.GetEnumerator())
        {
          if (enumerator.MoveNext())
            return AnimationWindowUtility.IsNodeLeftOverCurve(enumerator.Current as AnimationWindowHierarchyNode);
        }
      }
      return false;
    }

    public static bool IsNodeAmbiguous(AnimationWindowHierarchyNode node)
    {
      if (node.binding.HasValue && node.curves.Length > 0)
      {
        AnimationWindowSelectionItem selectionBinding = node.curves[0].selectionBinding;
        if ((UnityEngine.Object) selectionBinding != (UnityEngine.Object) null && (UnityEngine.Object) selectionBinding.rootGameObject != (UnityEngine.Object) null)
          return AnimationUtility.AmbiguousBinding(node.binding.Value.path, node.binding.Value.m_ClassID, selectionBinding.rootGameObject.transform);
      }
      if (node.hasChildren)
      {
        using (List<TreeViewItem>.Enumerator enumerator = node.children.GetEnumerator())
        {
          if (enumerator.MoveNext())
            return AnimationWindowUtility.IsNodeAmbiguous(enumerator.Current as AnimationWindowHierarchyNode);
        }
      }
      return false;
    }

    public static bool IsNodePhantom(AnimationWindowHierarchyNode node)
    {
      if (node.binding.HasValue)
        return node.binding.Value.isPhantom;
      return false;
    }

    public static void AddSelectedKeyframes(AnimationWindowState state, AnimationKeyTime time)
    {
      List<AnimationWindowCurve> animationWindowCurveList = state.activeCurves.Count <= 0 ? state.allCurves : state.activeCurves;
      AnimationWindowUtility.AddKeyframes(state, animationWindowCurveList.ToArray(), time);
    }

    public static void AddKeyframes(AnimationWindowState state, AnimationWindowCurve[] curves, AnimationKeyTime time)
    {
      string undoLabel = "Add Key";
      state.SaveKeySelection(undoLabel);
      state.ClearKeySelections();
      foreach (AnimationWindowCurve curve1 in curves)
      {
        if (curve1.animationIsEditable)
        {
          AnimationKeyTime time1 = AnimationKeyTime.Time(time.time - curve1.timeOffset, time.frameRate);
          object currentValue = CurveBindingUtility.GetCurrentValue(state, curve1);
          AnimationWindowKeyframe curve2 = AnimationWindowUtility.AddKeyframeToCurve(curve1, currentValue, curve1.valueType, time1);
          if (curve2 != null)
          {
            state.SaveCurve(curve1, undoLabel);
            state.SelectKey(curve2);
          }
        }
      }
    }

    public static void RemoveKeyframes(AnimationWindowState state, AnimationWindowCurve[] curves, AnimationKeyTime time)
    {
      string undoLabel = "Remove Key";
      state.SaveKeySelection(undoLabel);
      foreach (AnimationWindowCurve curve in curves)
      {
        if (curve.animationIsEditable)
        {
          AnimationKeyTime time1 = AnimationKeyTime.Time(time.time - curve.timeOffset, time.frameRate);
          curve.RemoveKeyframe(time1);
          state.SaveCurve(curve, undoLabel);
        }
      }
    }

    public static AnimationWindowKeyframe AddKeyframeToCurve(AnimationWindowCurve curve, object value, System.Type type, AnimationKeyTime time)
    {
      AnimationWindowKeyframe keyAtTime = curve.FindKeyAtTime(time);
      if (keyAtTime != null)
      {
        keyAtTime.value = value;
        return keyAtTime;
      }
      AnimationWindowKeyframe key1 = (AnimationWindowKeyframe) null;
      if (curve.isPPtrCurve)
      {
        key1 = new AnimationWindowKeyframe();
        key1.time = time.time;
        key1.value = value;
        key1.curve = curve;
        curve.AddKeyframe(key1, time);
      }
      else if (type == typeof (bool) || type == typeof (float) || type == typeof (int))
      {
        key1 = new AnimationWindowKeyframe();
        AnimationCurve animationCurve = curve.ToAnimationCurve();
        Keyframe key2 = new Keyframe(time.time, (float) value);
        if (type == typeof (bool) || type == typeof (int))
        {
          if (type == typeof (int) && !curve.isDiscreteCurve)
          {
            AnimationUtility.SetKeyLeftTangentMode(ref key2, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(ref key2, AnimationUtility.TangentMode.Linear);
          }
          else
          {
            AnimationUtility.SetKeyLeftTangentMode(ref key2, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(ref key2, AnimationUtility.TangentMode.Constant);
          }
          AnimationUtility.SetKeyBroken(ref key2, true);
          key1.m_TangentMode = key2.tangentMode;
        }
        else
        {
          int index = animationCurve.AddKey(key2);
          if (index != -1)
          {
            AnimationUtility.SetKeyLeftTangentMode(animationCurve, index, AnimationUtility.TangentMode.ClampedAuto);
            AnimationUtility.SetKeyRightTangentMode(animationCurve, index, AnimationUtility.TangentMode.ClampedAuto);
            AnimationUtility.UpdateTangentsFromModeSurrounding(animationCurve, index);
            CurveUtility.SetKeyModeFromContext(animationCurve, index);
            key1.m_TangentMode = animationCurve[index].tangentMode;
            key1.m_InTangent = animationCurve[index].inTangent;
            key1.m_OutTangent = animationCurve[index].outTangent;
          }
        }
        key1.time = time.time;
        key1.value = value;
        key1.curve = curve;
        curve.AddKeyframe(key1, time);
      }
      return key1;
    }

    public static List<AnimationWindowCurve> FilterCurves(AnimationWindowCurve[] curves, string path, bool entireHierarchy)
    {
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      if (curves != null)
      {
        foreach (AnimationWindowCurve curve in curves)
        {
          if (curve.path.Equals(path) || entireHierarchy && curve.path.Contains(path))
            animationWindowCurveList.Add(curve);
        }
      }
      return animationWindowCurveList;
    }

    public static List<AnimationWindowCurve> FilterCurves(AnimationWindowCurve[] curves, string path, System.Type animatableObjectType)
    {
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      if (curves != null)
      {
        foreach (AnimationWindowCurve curve in curves)
        {
          if (curve.path.Equals(path) && curve.type == animatableObjectType)
            animationWindowCurveList.Add(curve);
        }
      }
      return animationWindowCurveList;
    }

    public static bool IsCurveCreated(AnimationClip clip, EditorCurveBinding binding)
    {
      if (binding.isPPtrCurve)
        return AnimationUtility.GetObjectReferenceCurve(clip, binding) != null;
      if (AnimationWindowUtility.IsRectTransformPosition(binding))
        binding.propertyName = binding.propertyName.Replace(".x", ".z").Replace(".y", ".z");
      if (AnimationWindowUtility.IsRotationCurve(binding))
        return AnimationUtility.GetEditorCurve(clip, binding) != null || AnimationWindowUtility.HasOtherRotationCurve(clip, binding);
      return AnimationUtility.GetEditorCurve(clip, binding) != null;
    }

    internal static bool HasOtherRotationCurve(AnimationClip clip, EditorCurveBinding rotationBinding)
    {
      if (rotationBinding.propertyName.StartsWith("m_LocalRotation"))
      {
        EditorCurveBinding binding1 = rotationBinding;
        EditorCurveBinding binding2 = rotationBinding;
        EditorCurveBinding binding3 = rotationBinding;
        binding1.propertyName = "localEulerAnglesRaw.x";
        binding2.propertyName = "localEulerAnglesRaw.y";
        binding3.propertyName = "localEulerAnglesRaw.z";
        return AnimationUtility.GetEditorCurve(clip, binding1) != null || AnimationUtility.GetEditorCurve(clip, binding2) != null || AnimationUtility.GetEditorCurve(clip, binding3) != null;
      }
      EditorCurveBinding binding4 = rotationBinding;
      EditorCurveBinding binding5 = rotationBinding;
      EditorCurveBinding binding6 = rotationBinding;
      EditorCurveBinding binding7 = rotationBinding;
      binding4.propertyName = "m_LocalRotation.x";
      binding5.propertyName = "m_LocalRotation.y";
      binding6.propertyName = "m_LocalRotation.z";
      binding7.propertyName = "m_LocalRotation.w";
      return AnimationUtility.GetEditorCurve(clip, binding4) != null || AnimationUtility.GetEditorCurve(clip, binding5) != null || AnimationUtility.GetEditorCurve(clip, binding6) != null || AnimationUtility.GetEditorCurve(clip, binding7) != null;
    }

    internal static bool IsRotationCurve(EditorCurveBinding curveBinding)
    {
      string propertyGroupName = AnimationWindowUtility.GetPropertyGroupName(curveBinding.propertyName);
      return propertyGroupName == "m_LocalRotation" || propertyGroupName == "localEulerAnglesRaw";
    }

    public static bool IsRectTransformPosition(EditorCurveBinding curveBinding)
    {
      return curveBinding.type == typeof (RectTransform) && AnimationWindowUtility.GetPropertyGroupName(curveBinding.propertyName) == "m_LocalPosition";
    }

    public static bool ContainsFloatKeyframes(List<AnimationWindowKeyframe> keyframes)
    {
      if (keyframes == null || keyframes.Count == 0)
        return false;
      foreach (AnimationWindowKeyframe keyframe in keyframes)
      {
        if (!keyframe.isPPtrCurve)
          return true;
      }
      return false;
    }

    public static List<AnimationWindowCurve> FilterCurves(AnimationWindowCurve[] curves, string path, System.Type animatableObjectType, string propertyName)
    {
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      if (curves != null)
      {
        string propertyGroupName = AnimationWindowUtility.GetPropertyGroupName(propertyName);
        bool flag1 = propertyGroupName == propertyName;
        foreach (AnimationWindowCurve curve in curves)
        {
          bool flag2 = !flag1 ? curve.propertyName.Equals(propertyName) : AnimationWindowUtility.GetPropertyGroupName(curve.propertyName).Equals(propertyGroupName);
          if (curve.path.Equals(path) && curve.type == animatableObjectType && flag2)
            animationWindowCurveList.Add(curve);
        }
      }
      return animationWindowCurveList;
    }

    public static object GetCurrentValue(GameObject rootGameObject, EditorCurveBinding curveBinding)
    {
      if (curveBinding.isPPtrCurve)
      {
        UnityEngine.Object targetObject;
        AnimationUtility.GetObjectReferenceValue(rootGameObject, curveBinding, out targetObject);
        return (object) targetObject;
      }
      float data;
      AnimationUtility.GetFloatValue(rootGameObject, curveBinding, out data);
      return (object) data;
    }

    public static List<EditorCurveBinding> GetAnimatableProperties(GameObject gameObject, GameObject root, System.Type valueType)
    {
      EditorCurveBinding[] animatableBindings = AnimationUtility.GetAnimatableBindings(gameObject, root);
      List<EditorCurveBinding> editorCurveBindingList = new List<EditorCurveBinding>();
      foreach (EditorCurveBinding binding in animatableBindings)
      {
        if (AnimationUtility.GetEditorCurveValueType(root, binding) == valueType)
          editorCurveBindingList.Add(binding);
      }
      return editorCurveBindingList;
    }

    public static List<EditorCurveBinding> GetAnimatableProperties(GameObject gameObject, GameObject root, System.Type objectType, System.Type valueType)
    {
      EditorCurveBinding[] animatableBindings = AnimationUtility.GetAnimatableBindings(gameObject, root);
      List<EditorCurveBinding> editorCurveBindingList = new List<EditorCurveBinding>();
      foreach (EditorCurveBinding binding in animatableBindings)
      {
        if (binding.type == objectType && AnimationUtility.GetEditorCurveValueType(root, binding) == valueType)
          editorCurveBindingList.Add(binding);
      }
      return editorCurveBindingList;
    }

    public static List<EditorCurveBinding> GetAnimatableProperties(ScriptableObject scriptableObject, System.Type valueType)
    {
      EditorCurveBinding[] animatableBindings = AnimationUtility.GetScriptableObjectAnimatableBindings(scriptableObject);
      List<EditorCurveBinding> editorCurveBindingList = new List<EditorCurveBinding>();
      foreach (EditorCurveBinding binding in animatableBindings)
      {
        if (AnimationUtility.GetScriptableObjectEditorCurveValueType(scriptableObject, binding) == valueType)
          editorCurveBindingList.Add(binding);
      }
      return editorCurveBindingList;
    }

    public static bool PropertyIsAnimatable(UnityEngine.Object targetObject, string propertyPath, UnityEngine.Object rootObject)
    {
      if (targetObject is ScriptableObject)
        return Array.Exists<EditorCurveBinding>(AnimationUtility.GetScriptableObjectAnimatableBindings((ScriptableObject) targetObject), (Predicate<EditorCurveBinding>) (binding => binding.propertyName == propertyPath));
      GameObject gameObject = targetObject as GameObject;
      if (targetObject is Component)
        gameObject = ((Component) targetObject).gameObject;
      if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
        return false;
      PropertyModification modification = new PropertyModification();
      modification.propertyPath = propertyPath;
      modification.target = targetObject;
      EditorCurveBinding binding1 = new EditorCurveBinding();
      return AnimationUtility.PropertyModificationToEditorCurveBinding(modification, !(rootObject == (UnityEngine.Object) null) ? (GameObject) rootObject : gameObject, out binding1) != null;
    }

    public static PropertyModification[] SerializedPropertyToPropertyModifications(SerializedProperty property)
    {
      List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>();
      serializedPropertyList.Add(property);
      if (property.hasChildren)
      {
        SerializedProperty x = property.Copy();
        SerializedProperty endProperty = property.GetEndProperty(false);
        while (x.Next(true) && !SerializedProperty.EqualContents(x, endProperty) && x.propertyPath.StartsWith(property.propertyPath))
          serializedPropertyList.Add(x.Copy());
      }
      if (property.propertyPath.StartsWith("m_LocalRotation"))
      {
        SerializedObject serializedObject = property.serializedObject;
        if (serializedObject.targetObject is Transform)
        {
          SerializedProperty property1 = serializedObject.FindProperty("m_LocalEulerAnglesHint");
          if (property1 != null && property1.hasChildren)
          {
            SerializedProperty x = property1.Copy();
            SerializedProperty endProperty = property1.GetEndProperty(false);
            while (x.Next(true) && !SerializedProperty.EqualContents(x, endProperty) && x.propertyPath.StartsWith(property1.propertyPath))
              serializedPropertyList.Add(x.Copy());
          }
        }
      }
      List<PropertyModification> propertyModificationList = new List<PropertyModification>();
      for (int index1 = 0; index1 < serializedPropertyList.Count; ++index1)
      {
        SerializedProperty serializedProperty = serializedPropertyList[index1];
        bool flag1 = serializedProperty.propertyType == SerializedPropertyType.ObjectReference;
        bool flag2 = serializedProperty.propertyType == SerializedPropertyType.Float;
        bool flag3 = serializedProperty.propertyType == SerializedPropertyType.Boolean;
        bool flag4 = serializedProperty.propertyType == SerializedPropertyType.Integer;
        bool flag5 = serializedProperty.propertyType == SerializedPropertyType.Enum;
        if (flag1 || flag2 || (flag3 || flag4) || flag5)
        {
          UnityEngine.Object[] targetObjects = serializedProperty.serializedObject.targetObjects;
          if (serializedProperty.hasMultipleDifferentValues)
          {
            for (int index2 = 0; index2 < targetObjects.Length; ++index2)
            {
              SerializedProperty property1 = new SerializedObject(targetObjects[index2]).FindProperty(serializedProperty.propertyPath);
              string str = string.Empty;
              UnityEngine.Object @object = (UnityEngine.Object) null;
              if (flag1)
                @object = property1.objectReferenceValue;
              else
                str = !flag2 ? (!flag4 ? (!flag5 ? (!property1.boolValue ? "0" : "1") : property1.enumValueIndex.ToString()) : property1.intValue.ToString()) : property1.floatValue.ToString();
              propertyModificationList.Add(new PropertyModification()
              {
                target = targetObjects[index2],
                propertyPath = property1.propertyPath,
                value = str,
                objectReference = @object
              });
            }
          }
          else
          {
            string str = string.Empty;
            UnityEngine.Object @object = (UnityEngine.Object) null;
            if (flag1)
              @object = serializedProperty.objectReferenceValue;
            else
              str = !flag2 ? (!flag4 ? (!serializedProperty.boolValue ? "0" : "1") : serializedProperty.intValue.ToString()) : serializedProperty.floatValue.ToString();
            for (int index2 = 0; index2 < targetObjects.Length; ++index2)
              propertyModificationList.Add(new PropertyModification()
              {
                target = targetObjects[index2],
                propertyPath = serializedProperty.propertyPath,
                value = str,
                objectReference = @object
              });
          }
        }
      }
      return propertyModificationList.ToArray();
    }

    public static EditorCurveBinding[] PropertyModificationsToEditorCurveBindings(PropertyModification[] modifications, GameObject rootGameObject, AnimationClip animationClip)
    {
      if (modifications == null)
        return new EditorCurveBinding[0];
      HashSet<EditorCurveBinding> source = new HashSet<EditorCurveBinding>();
      for (int index1 = 0; index1 < modifications.Length; ++index1)
      {
        EditorCurveBinding binding = new EditorCurveBinding();
        if (AnimationUtility.PropertyModificationToEditorCurveBinding(modifications[index1], rootGameObject, out binding) != null)
        {
          EditorCurveBinding[] editorCurveBindingArray = RotationCurveInterpolation.RemapAnimationBindingForAddKey(binding, animationClip);
          if (editorCurveBindingArray != null)
          {
            for (int index2 = 0; index2 < editorCurveBindingArray.Length; ++index2)
              source.Add(editorCurveBindingArray[index2]);
          }
          else
            source.Add(binding);
        }
      }
      return source.ToArray<EditorCurveBinding>();
    }

    public static EditorCurveBinding[] SerializedPropertyToEditorCurveBindings(SerializedProperty property, GameObject rootGameObject, AnimationClip animationClip)
    {
      return AnimationWindowUtility.PropertyModificationsToEditorCurveBindings(AnimationWindowUtility.SerializedPropertyToPropertyModifications(property), rootGameObject, animationClip);
    }

    public static bool CurveExists(EditorCurveBinding binding, AnimationWindowCurve[] curves)
    {
      foreach (AnimationWindowCurve curve in curves)
      {
        if (binding.propertyName == curve.binding.propertyName && binding.type == curve.binding.type && binding.path == curve.binding.path)
          return true;
      }
      return false;
    }

    public static EditorCurveBinding GetRenamedBinding(EditorCurveBinding binding, string newPath)
    {
      return new EditorCurveBinding() { path = newPath, propertyName = binding.propertyName, type = binding.type };
    }

    public static void RenameCurvePath(AnimationWindowCurve curve, EditorCurveBinding newBinding, AnimationClip clip)
    {
      if (curve.isPPtrCurve)
      {
        AnimationUtility.SetObjectReferenceCurve(clip, curve.binding, (ObjectReferenceKeyframe[]) null);
        AnimationUtility.SetObjectReferenceCurve(clip, newBinding, curve.ToObjectCurve());
      }
      else
      {
        AnimationUtility.SetEditorCurve(clip, curve.binding, (AnimationCurve) null);
        AnimationUtility.SetEditorCurve(clip, newBinding, curve.ToAnimationCurve());
      }
    }

    public static string GetPropertyDisplayName(string propertyName)
    {
      propertyName = propertyName.Replace("m_LocalPosition", "Position");
      propertyName = propertyName.Replace("m_LocalScale", "Scale");
      propertyName = propertyName.Replace("m_LocalRotation", "Rotation");
      propertyName = propertyName.Replace("localEulerAnglesBaked", "Rotation");
      propertyName = propertyName.Replace("localEulerAnglesRaw", "Rotation");
      propertyName = propertyName.Replace("localEulerAngles", "Rotation");
      propertyName = propertyName.Replace("m_Materials.Array.data", "Material Reference");
      propertyName = ObjectNames.NicifyVariableName(propertyName);
      propertyName = propertyName.Replace("m_", "");
      return propertyName;
    }

    public static bool ShouldPrefixWithTypeName(System.Type animatableObjectType, string propertyName)
    {
      return animatableObjectType != typeof (Transform) && animatableObjectType != typeof (RectTransform) && (animatableObjectType != typeof (SpriteRenderer) || !(propertyName == "m_Sprite"));
    }

    public static string GetNicePropertyDisplayName(System.Type animatableObjectType, string propertyName)
    {
      if (AnimationWindowUtility.ShouldPrefixWithTypeName(animatableObjectType, propertyName))
        return ObjectNames.NicifyVariableName(animatableObjectType.Name) + "." + AnimationWindowUtility.GetPropertyDisplayName(propertyName);
      return AnimationWindowUtility.GetPropertyDisplayName(propertyName);
    }

    public static string GetNicePropertyGroupDisplayName(System.Type animatableObjectType, string propertyGroupName)
    {
      if (AnimationWindowUtility.ShouldPrefixWithTypeName(animatableObjectType, propertyGroupName))
        return ObjectNames.NicifyVariableName(animatableObjectType.Name) + "." + AnimationWindowUtility.NicifyPropertyGroupName(animatableObjectType, propertyGroupName);
      return AnimationWindowUtility.NicifyPropertyGroupName(animatableObjectType, propertyGroupName);
    }

    public static string NicifyPropertyGroupName(System.Type animatableObjectType, string propertyGroupName)
    {
      string str = AnimationWindowUtility.GetPropertyGroupName(AnimationWindowUtility.GetPropertyDisplayName(propertyGroupName));
      if (animatableObjectType == typeof (RectTransform) & str.Equals("Position"))
        str = "Position (Z)";
      return str;
    }

    public static int GetComponentIndex(string name)
    {
      if (name == null || name.Length < 3 || (int) name[name.Length - 2] != 46)
        return -1;
      switch (name[name.Length - 1])
      {
        case 'a':
          return 3;
        case 'b':
          return 2;
        case 'g':
          return 1;
        case 'r':
          return 0;
        case 'w':
          return 3;
        case 'x':
          return 0;
        case 'y':
          return 1;
        case 'z':
          return 2;
        default:
          return -1;
      }
    }

    public static string GetPropertyGroupName(string propertyName)
    {
      if (AnimationWindowUtility.GetComponentIndex(propertyName) != -1)
        return propertyName.Substring(0, propertyName.Length - 2);
      return propertyName;
    }

    public static float GetNextKeyframeTime(AnimationWindowCurve[] curves, float currentTime, float frameRate)
    {
      float num1 = float.MaxValue;
      AnimationKeyTime animationKeyTime1 = AnimationKeyTime.Time(currentTime, frameRate);
      AnimationKeyTime animationKeyTime2 = AnimationKeyTime.Frame(animationKeyTime1.frame + 1, frameRate);
      bool flag = false;
      foreach (AnimationWindowCurve curve in curves)
      {
        foreach (AnimationWindowKeyframe keyframe in curve.m_Keyframes)
        {
          float num2 = keyframe.time + curve.timeOffset;
          if ((double) num2 < (double) num1 && (double) num2 >= (double) animationKeyTime2.time)
          {
            num1 = num2;
            flag = true;
          }
        }
      }
      return !flag ? animationKeyTime1.time : num1;
    }

    public static float GetPreviousKeyframeTime(AnimationWindowCurve[] curves, float currentTime, float frameRate)
    {
      float num1 = float.MinValue;
      AnimationKeyTime animationKeyTime1 = AnimationKeyTime.Time(currentTime, frameRate);
      AnimationKeyTime animationKeyTime2 = AnimationKeyTime.Frame(animationKeyTime1.frame - 1, frameRate);
      bool flag = false;
      foreach (AnimationWindowCurve curve in curves)
      {
        foreach (AnimationWindowKeyframe keyframe in curve.m_Keyframes)
        {
          float num2 = keyframe.time + curve.timeOffset;
          if ((double) num2 > (double) num1 && (double) num2 <= (double) animationKeyTime2.time)
          {
            num1 = num2;
            flag = true;
          }
        }
      }
      return !flag ? animationKeyTime1.time : num1;
    }

    public static bool InitializeGameobjectForAnimation(GameObject animatedObject)
    {
      Component componentInParents = AnimationWindowUtility.GetClosestAnimationPlayerComponentInParents(animatedObject.transform);
      if (!((UnityEngine.Object) componentInParents == (UnityEngine.Object) null))
        return AnimationWindowUtility.EnsureAnimationPlayerHasClip(componentInParents);
      AnimationClip newClip = AnimationWindowUtility.CreateNewClip(animatedObject.name);
      if ((UnityEngine.Object) newClip == (UnityEngine.Object) null)
        return false;
      Component animationPlayer = AnimationWindowUtility.EnsureActiveAnimationPlayer(animatedObject);
      bool animationPlayerComponent = AnimationWindowUtility.AddClipToAnimationPlayerComponent(animationPlayer, newClip);
      if (!animationPlayerComponent)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) animationPlayer);
      return animationPlayerComponent;
    }

    public static Component EnsureActiveAnimationPlayer(GameObject animatedObject)
    {
      Component componentInParents = AnimationWindowUtility.GetClosestAnimationPlayerComponentInParents(animatedObject.transform);
      if ((UnityEngine.Object) componentInParents == (UnityEngine.Object) null)
        return (Component) Undo.AddComponent<Animator>(animatedObject);
      return componentInParents;
    }

    private static bool EnsureAnimationPlayerHasClip(Component animationPlayer)
    {
      if ((UnityEngine.Object) animationPlayer == (UnityEngine.Object) null)
        return false;
      if (AnimationUtility.GetAnimationClips(animationPlayer.gameObject).Length > 0)
        return true;
      AnimationClip newClip = AnimationWindowUtility.CreateNewClip(animationPlayer.gameObject.name);
      if ((UnityEngine.Object) newClip == (UnityEngine.Object) null)
        return false;
      AnimationMode.StopAnimationMode();
      return AnimationWindowUtility.AddClipToAnimationPlayerComponent(animationPlayer, newClip);
    }

    public static bool AddClipToAnimationPlayerComponent(Component animationPlayer, AnimationClip newClip)
    {
      if (animationPlayer is Animator)
        return AnimationWindowUtility.AddClipToAnimatorComponent(animationPlayer as Animator, newClip);
      if (animationPlayer is Animation)
        return AnimationWindowUtility.AddClipToAnimationComponent(animationPlayer as Animation, newClip);
      return false;
    }

    public static bool AddClipToAnimatorComponent(Animator animator, AnimationClip newClip)
    {
      UnityEditor.Animations.AnimatorController animatorController = UnityEditor.Animations.AnimatorController.GetEffectiveAnimatorController(animator);
      if ((UnityEngine.Object) animatorController == (UnityEngine.Object) null)
      {
        UnityEditor.Animations.AnimatorController controllerForClip = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerForClip(newClip, animator.gameObject);
        UnityEditor.Animations.AnimatorController.SetAnimatorController(animator, controllerForClip);
        return (UnityEngine.Object) controllerForClip != (UnityEngine.Object) null;
      }
      ChildAnimatorState state = animatorController.layers[0].stateMachine.FindState(newClip.name);
      if (state.Equals((object) new ChildAnimatorState()))
        animatorController.AddMotion((Motion) newClip);
      else if ((bool) ((UnityEngine.Object) state.state) && (UnityEngine.Object) state.state.motion == (UnityEngine.Object) null)
        state.state.motion = (Motion) newClip;
      else if ((bool) ((UnityEngine.Object) state.state) && (UnityEngine.Object) state.state.motion != (UnityEngine.Object) newClip)
        animatorController.AddMotion((Motion) newClip);
      return true;
    }

    public static bool AddClipToAnimationComponent(Animation animation, AnimationClip newClip)
    {
      AnimationWindowUtility.SetClipAsLegacy(newClip);
      animation.AddClip(newClip, newClip.name);
      return true;
    }

    internal static AnimationClip CreateNewClip(string gameObjectName)
    {
      string message = string.Format("Create a new animation for the game object '{0}':", (object) gameObjectName);
      string path = ProjectWindowUtil.GetActiveFolderPath();
      if (AnimationWindowUtility.s_LastPathUsedForNewClip != null)
      {
        string directoryName = Path.GetDirectoryName(AnimationWindowUtility.s_LastPathUsedForNewClip);
        if (directoryName != null && Directory.Exists(directoryName))
          path = directoryName;
      }
      string clipPath = EditorUtility.SaveFilePanelInProject("Create New Animation", "New Animation", "anim", message, path);
      if (clipPath == "")
        return (AnimationClip) null;
      return AnimationWindowUtility.CreateNewClipAtPath(clipPath);
    }

    internal static AnimationClip CreateNewClipAtPath(string clipPath)
    {
      AnimationWindowUtility.s_LastPathUsedForNewClip = clipPath;
      AnimationClip clip = new AnimationClip();
      AnimationClipSettings animationClipSettings = AnimationUtility.GetAnimationClipSettings(clip);
      animationClipSettings.loopTime = true;
      AnimationUtility.SetAnimationClipSettingsNoDirty(clip, animationClipSettings);
      AnimationClip animationClip = AssetDatabase.LoadMainAssetAtPath(clipPath) as AnimationClip;
      if ((bool) ((UnityEngine.Object) animationClip))
      {
        EditorUtility.CopySerialized((UnityEngine.Object) clip, (UnityEngine.Object) animationClip);
        AssetDatabase.SaveAssets();
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) clip);
        return animationClip;
      }
      AssetDatabase.CreateAsset((UnityEngine.Object) clip, clipPath);
      return clip;
    }

    private static void SetClipAsLegacy(AnimationClip clip)
    {
      SerializedObject serializedObject = new SerializedObject((UnityEngine.Object) clip);
      serializedObject.FindProperty("m_Legacy").boolValue = true;
      serializedObject.ApplyModifiedProperties();
    }

    internal static AnimationClip AllocateAndSetupClip(bool useAnimator)
    {
      AnimationClip clip = new AnimationClip();
      if (useAnimator)
      {
        AnimationClipSettings animationClipSettings = AnimationUtility.GetAnimationClipSettings(clip);
        animationClipSettings.loopTime = true;
        AnimationUtility.SetAnimationClipSettingsNoDirty(clip, animationClipSettings);
      }
      return clip;
    }

    public static int GetPropertyNodeID(int setId, string path, System.Type type, string propertyName)
    {
      return (setId.ToString() + path + type.Name + propertyName).GetHashCode();
    }

    public static Component GetClosestAnimationPlayerComponentInParents(Transform tr)
    {
      Animator animatorInParents = AnimationWindowUtility.GetClosestAnimatorInParents(tr);
      if ((UnityEngine.Object) animatorInParents != (UnityEngine.Object) null)
        return (Component) animatorInParents;
      Animation animationInParents = AnimationWindowUtility.GetClosestAnimationInParents(tr);
      if ((UnityEngine.Object) animationInParents != (UnityEngine.Object) null)
        return (Component) animationInParents;
      return (Component) null;
    }

    public static Animator GetClosestAnimatorInParents(Transform tr)
    {
      for (; !((UnityEngine.Object) tr.GetComponent<Animator>() != (UnityEngine.Object) null); tr = tr.parent)
      {
        if ((UnityEngine.Object) tr == (UnityEngine.Object) tr.root)
          return (Animator) null;
      }
      return tr.GetComponent<Animator>();
    }

    public static Animation GetClosestAnimationInParents(Transform tr)
    {
      for (; !((UnityEngine.Object) tr.GetComponent<Animation>() != (UnityEngine.Object) null); tr = tr.parent)
      {
        if ((UnityEngine.Object) tr == (UnityEngine.Object) tr.root)
          return (Animation) null;
      }
      return tr.GetComponent<Animation>();
    }

    public static void SyncTimeArea(TimeArea from, TimeArea to)
    {
      to.SetDrawRectHack(from.drawRect);
      to.m_Scale = new Vector2(from.m_Scale.x, to.m_Scale.y);
      to.m_Translation = new Vector2(from.m_Translation.x, to.m_Translation.y);
      to.EnforceScaleAndRange();
    }

    public static void DrawInRangeOverlay(Rect rect, Color color, float startOfClipPixel, float endOfClipPixel)
    {
      if ((double) endOfClipPixel < (double) rect.xMin || (double) color.a <= 0.0)
        return;
      AnimationWindowUtility.DrawRect(Rect.MinMaxRect(Mathf.Max(startOfClipPixel, rect.xMin), rect.yMin, Mathf.Min(endOfClipPixel, rect.xMax), rect.yMax), color);
    }

    public static void DrawOutOfRangeOverlay(Rect rect, Color color, float startOfClipPixel, float endOfClipPixel)
    {
      Color color1 = Color.white.RGBMultiplied(0.4f);
      if ((double) startOfClipPixel > (double) rect.xMin)
      {
        Rect rect1 = Rect.MinMaxRect(rect.xMin, rect.yMin, Mathf.Min(startOfClipPixel, rect.xMax), rect.yMax);
        AnimationWindowUtility.DrawRect(rect1, color);
        TimeArea.DrawVerticalLine(rect1.xMax, rect1.yMin, rect1.yMax, color1);
      }
      Rect rect2 = Rect.MinMaxRect(Mathf.Max(endOfClipPixel, rect.xMin), rect.yMin, rect.xMax, rect.yMax);
      AnimationWindowUtility.DrawRect(rect2, color);
      TimeArea.DrawVerticalLine(rect2.xMin, rect2.yMin, rect2.yMax, color1);
    }

    public static void DrawSelectionOverlay(Rect rect, Color color, float startPixel, float endPixel)
    {
      startPixel = Mathf.Max(startPixel, rect.xMin);
      endPixel = Mathf.Max(endPixel, rect.xMin);
      AnimationWindowUtility.DrawRect(Rect.MinMaxRect(startPixel, rect.yMin, endPixel, rect.yMax), color);
    }

    public static void DrawRect(Rect rect, Color color)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      GL.PushMatrix();
      GL.MultMatrix(Handles.matrix);
      GL.Begin(7);
      GL.Color(color);
      GL.Vertex((Vector3) rect.min);
      GL.Vertex((Vector3) new Vector2(rect.xMax, rect.yMin));
      GL.Vertex((Vector3) rect.max);
      GL.Vertex((Vector3) new Vector2(rect.xMin, rect.yMax));
      GL.End();
      GL.PopMatrix();
    }

    private static CurveRenderer CreateRendererForCurve(AnimationWindowCurve curve)
    {
      CurveRenderer curveRenderer;
      switch (System.Type.GetTypeCode(curve.valueType))
      {
        case TypeCode.Boolean:
          curveRenderer = (CurveRenderer) new BoolCurveRenderer(curve.ToAnimationCurve());
          break;
        case TypeCode.Int32:
          curveRenderer = (CurveRenderer) new IntCurveRenderer(curve.ToAnimationCurve());
          break;
        default:
          curveRenderer = (CurveRenderer) new NormalCurveRenderer(curve.ToAnimationCurve());
          break;
      }
      return curveRenderer;
    }

    private static CurveWrapper.PreProcessKeyMovement CreateKeyPreprocessorForCurve(AnimationWindowCurve curve)
    {
      CurveWrapper.PreProcessKeyMovement processKeyMovement;
      switch (System.Type.GetTypeCode(curve.valueType))
      {
        case TypeCode.Boolean:
          processKeyMovement = (CurveWrapper.PreProcessKeyMovement) ((ref Keyframe key) => key.value = (double) key.value <= 0.5 ? 0.0f : 1f);
          break;
        case TypeCode.Int32:
          processKeyMovement = (CurveWrapper.PreProcessKeyMovement) ((ref Keyframe key) => key.value = Mathf.Floor(key.value + 0.5f));
          break;
        default:
          processKeyMovement = (CurveWrapper.PreProcessKeyMovement) null;
          break;
      }
      return processKeyMovement;
    }

    public static CurveWrapper GetCurveWrapper(AnimationWindowCurve curve, AnimationClip clip)
    {
      CurveWrapper curveWrapper = new CurveWrapper();
      curveWrapper.renderer = AnimationWindowUtility.CreateRendererForCurve(curve);
      curveWrapper.preProcessKeyMovementDelegate = AnimationWindowUtility.CreateKeyPreprocessorForCurve(curve);
      curveWrapper.renderer.SetWrap(WrapMode.Once, !clip.isLooping ? WrapMode.Once : WrapMode.Loop);
      curveWrapper.renderer.SetCustomRange(clip.startTime, clip.stopTime);
      curveWrapper.binding = curve.binding;
      curveWrapper.id = curve.GetHashCode();
      curveWrapper.color = CurveUtility.GetPropertyColor(curve.propertyName);
      curveWrapper.hidden = false;
      curveWrapper.selectionBindingInterface = (ISelectionBinding) curve.selectionBinding;
      return curveWrapper;
    }

    public static AnimationWindowKeyframe CurveSelectionToAnimationWindowKeyframe(CurveSelection curveSelection, List<AnimationWindowCurve> allCurves)
    {
      foreach (AnimationWindowCurve allCurve in allCurves)
      {
        if (allCurve.GetHashCode() == curveSelection.curveID && allCurve.m_Keyframes.Count > curveSelection.key)
          return allCurve.m_Keyframes[curveSelection.key];
      }
      return (AnimationWindowKeyframe) null;
    }

    public static CurveSelection AnimationWindowKeyframeToCurveSelection(AnimationWindowKeyframe keyframe, CurveEditor curveEditor)
    {
      int hashCode = keyframe.curve.GetHashCode();
      foreach (CurveWrapper animationCurve in curveEditor.animationCurves)
      {
        if (animationCurve.id == hashCode && keyframe.GetIndex() >= 0)
          return new CurveSelection(animationCurve.id, keyframe.GetIndex());
      }
      return (CurveSelection) null;
    }

    public static AnimationWindowCurve BestMatchForPaste(EditorCurveBinding binding, List<AnimationWindowCurve> clipboardCurves, List<AnimationWindowCurve> targetCurves)
    {
      foreach (AnimationWindowCurve targetCurve in targetCurves)
      {
        if (targetCurve.binding == binding)
          return targetCurve;
      }
      foreach (AnimationWindowCurve targetCurve in targetCurves)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AnimationWindowUtility.\u003CBestMatchForPaste\u003Ec__AnonStorey1 pasteCAnonStorey1 = new AnimationWindowUtility.\u003CBestMatchForPaste\u003Ec__AnonStorey1();
        // ISSUE: reference to a compiler-generated field
        pasteCAnonStorey1.targetCurve = targetCurve;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (pasteCAnonStorey1.targetCurve.binding.propertyName == binding.propertyName && !clipboardCurves.Exists(new Predicate<AnimationWindowCurve>(pasteCAnonStorey1.\u003C\u003Em__0)))
        {
          // ISSUE: reference to a compiler-generated field
          return pasteCAnonStorey1.targetCurve;
        }
      }
      return (AnimationWindowCurve) null;
    }

    internal static Rect FromToRect(Vector2 start, Vector2 end)
    {
      Rect rect = new Rect(start.x, start.y, end.x - start.x, end.y - start.y);
      if ((double) rect.width < 0.0)
      {
        rect.x += rect.width;
        rect.width = -rect.width;
      }
      if ((double) rect.height < 0.0)
      {
        rect.y += rect.height;
        rect.height = -rect.height;
      }
      return rect;
    }

    public static bool IsTransformType(System.Type type)
    {
      return type == typeof (Transform) || type == typeof (RectTransform);
    }

    public static bool ForceGrouping(EditorCurveBinding binding)
    {
      if (binding.type == typeof (Transform))
        return true;
      if (binding.type == typeof (RectTransform))
      {
        string propertyGroupName = AnimationWindowUtility.GetPropertyGroupName(binding.propertyName);
        return propertyGroupName == "m_LocalPosition" || propertyGroupName == "m_LocalScale" || (propertyGroupName == "m_LocalRotation" || propertyGroupName == "localEulerAnglesBaked") || propertyGroupName == "localEulerAngles" || propertyGroupName == "localEulerAnglesRaw";
      }
      if (typeof (Renderer).IsAssignableFrom(binding.type))
        return AnimationWindowUtility.GetPropertyGroupName(binding.propertyName) == "material._Color";
      return false;
    }

    public static void ControllerChanged()
    {
      foreach (AnimationWindow allAnimationWindow in AnimationWindow.GetAllAnimationWindows())
        allAnimationWindow.OnControllerChange();
    }
  }
}
