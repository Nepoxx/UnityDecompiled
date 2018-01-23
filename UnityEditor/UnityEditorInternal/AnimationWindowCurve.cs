// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowCurve
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AnimationWindowCurve : IComparable<AnimationWindowCurve>
  {
    public const float timeEpsilon = 1E-05f;
    public List<AnimationWindowKeyframe> m_Keyframes;
    private EditorCurveBinding m_Binding;
    private int m_BindingHashCode;
    private AnimationClip m_Clip;
    private AnimationWindowSelectionItem m_SelectionBinding;
    private System.Type m_ValueType;

    public AnimationWindowCurve(AnimationClip clip, EditorCurveBinding binding, System.Type valueType)
    {
      binding = RotationCurveInterpolation.RemapAnimationBindingForRotationCurves(binding, clip);
      this.m_Binding = binding;
      this.m_BindingHashCode = binding.GetHashCode();
      this.m_ValueType = valueType;
      this.m_Clip = clip;
      this.LoadKeyframes(clip);
    }

    public EditorCurveBinding binding
    {
      get
      {
        return this.m_Binding;
      }
    }

    public bool isPPtrCurve
    {
      get
      {
        return this.m_Binding.isPPtrCurve;
      }
    }

    public bool isDiscreteCurve
    {
      get
      {
        return this.m_Binding.isDiscreteCurve;
      }
    }

    public bool isPhantom
    {
      get
      {
        return this.m_Binding.isPhantom;
      }
    }

    public string propertyName
    {
      get
      {
        return this.m_Binding.propertyName;
      }
    }

    public string path
    {
      get
      {
        return this.m_Binding.path;
      }
    }

    public System.Type type
    {
      get
      {
        return this.m_Binding.type;
      }
    }

    public System.Type valueType
    {
      get
      {
        return this.m_ValueType;
      }
    }

    public int length
    {
      get
      {
        return this.m_Keyframes.Count;
      }
    }

    public int depth
    {
      get
      {
        int num;
        if (this.path.Length > 0)
          num = this.path.Split('/').Length;
        else
          num = 0;
        return num;
      }
    }

    public AnimationClip clip
    {
      get
      {
        return this.m_Clip;
      }
    }

    public GameObject rootGameObject
    {
      get
      {
        return !((UnityEngine.Object) this.m_SelectionBinding != (UnityEngine.Object) null) ? (GameObject) null : this.m_SelectionBinding.rootGameObject;
      }
    }

    public ScriptableObject scriptableObject
    {
      get
      {
        return !((UnityEngine.Object) this.m_SelectionBinding != (UnityEngine.Object) null) ? (ScriptableObject) null : this.m_SelectionBinding.scriptableObject;
      }
    }

    public float timeOffset
    {
      get
      {
        return !((UnityEngine.Object) this.m_SelectionBinding != (UnityEngine.Object) null) ? 0.0f : this.m_SelectionBinding.timeOffset;
      }
    }

    public bool clipIsEditable
    {
      get
      {
        return !((UnityEngine.Object) this.m_SelectionBinding != (UnityEngine.Object) null) || this.m_SelectionBinding.clipIsEditable;
      }
    }

    public bool animationIsEditable
    {
      get
      {
        return !((UnityEngine.Object) this.m_SelectionBinding != (UnityEngine.Object) null) || this.m_SelectionBinding.animationIsEditable;
      }
    }

    public int selectionID
    {
      get
      {
        return !((UnityEngine.Object) this.m_SelectionBinding != (UnityEngine.Object) null) ? 0 : this.m_SelectionBinding.id;
      }
    }

    public AnimationWindowSelectionItem selectionBinding
    {
      get
      {
        return this.m_SelectionBinding;
      }
      set
      {
        this.m_SelectionBinding = value;
      }
    }

    public void LoadKeyframes(AnimationClip clip)
    {
      this.m_Keyframes = new List<AnimationWindowKeyframe>();
      if (!this.m_Binding.isPPtrCurve)
      {
        AnimationCurve editorCurve = AnimationUtility.GetEditorCurve(clip, this.binding);
        for (int index = 0; editorCurve != null && index < editorCurve.length; ++index)
          this.m_Keyframes.Add(new AnimationWindowKeyframe(this, editorCurve[index]));
      }
      else
      {
        ObjectReferenceKeyframe[] objectReferenceCurve = AnimationUtility.GetObjectReferenceCurve(clip, this.binding);
        for (int index = 0; objectReferenceCurve != null && index < objectReferenceCurve.Length; ++index)
          this.m_Keyframes.Add(new AnimationWindowKeyframe(this, objectReferenceCurve[index]));
      }
    }

    public override int GetHashCode()
    {
      return this.selectionID * 92821 ^ (!((UnityEngine.Object) this.clip == (UnityEngine.Object) null) ? this.clip.GetInstanceID() : 0) * 19603 ^ this.GetBindingHashCode();
    }

    public int GetBindingHashCode()
    {
      return this.m_BindingHashCode;
    }

    public int CompareTo(AnimationWindowCurve obj)
    {
      bool flag1 = this.path.Equals(obj.path);
      bool flag2 = obj.type == this.type;
      if (!flag1 && this.depth != obj.depth)
      {
        int num = Math.Min(this.path.Length, obj.path.Length);
        int startIndex = 0;
        int index;
        for (index = 0; index < num && (int) this.path[index] == (int) obj.path[index]; ++index)
        {
          if ((int) this.path[index] == 47)
            startIndex = index + 1;
        }
        if (index == num)
          startIndex = num;
        string input1 = this.path.Substring(startIndex);
        string input2 = obj.path.Substring(startIndex);
        if (string.IsNullOrEmpty(input1))
          return -1;
        if (string.IsNullOrEmpty(input2))
          return 1;
        Regex regex = new Regex("^[^\\/]*\\/");
        Match match1 = regex.Match(input1);
        string str = !match1.Success ? input1 : match1.Value.Substring(0, match1.Value.Length - 1);
        Match match2 = regex.Match(input2);
        string strB = !match2.Success ? input2 : match2.Value.Substring(0, match2.Value.Length - 1);
        return str.CompareTo(strB);
      }
      bool flag3 = this.type == typeof (Transform) && obj.type == typeof (Transform) && flag1;
      bool flag4 = (this.type == typeof (Transform) || obj.type == typeof (Transform)) && flag1;
      if (flag3)
      {
        string groupDisplayName1 = AnimationWindowUtility.GetNicePropertyGroupDisplayName(typeof (Transform), AnimationWindowUtility.GetPropertyGroupName(this.propertyName));
        string groupDisplayName2 = AnimationWindowUtility.GetNicePropertyGroupDisplayName(typeof (Transform), AnimationWindowUtility.GetPropertyGroupName(obj.propertyName));
        if (groupDisplayName1.Contains("Position") && groupDisplayName2.Contains("Rotation"))
          return -1;
        if (groupDisplayName1.Contains("Rotation") && groupDisplayName2.Contains("Position"))
          return 1;
      }
      else if (flag4)
        return this.type == typeof (Transform) ? -1 : 1;
      if (flag1 && flag2)
      {
        int componentIndex1 = AnimationWindowUtility.GetComponentIndex(obj.propertyName);
        int componentIndex2 = AnimationWindowUtility.GetComponentIndex(this.propertyName);
        if (componentIndex1 != -1 && componentIndex2 != -1 && this.propertyName.Substring(0, this.propertyName.Length - 2) == obj.propertyName.Substring(0, obj.propertyName.Length - 2))
          return componentIndex2 - componentIndex1;
      }
      return (this.path + (object) this.type + this.propertyName).CompareTo(obj.path + (object) obj.type + obj.propertyName);
    }

    public AnimationCurve ToAnimationCurve()
    {
      int count = this.m_Keyframes.Count;
      AnimationCurve animationCurve = new AnimationCurve();
      List<Keyframe> keyframeList = new List<Keyframe>();
      float num = float.MinValue;
      for (int index = 0; index < count; ++index)
      {
        if ((double) Mathf.Abs(this.m_Keyframes[index].time - num) > 9.99999974737875E-06)
        {
          keyframeList.Add(new Keyframe(this.m_Keyframes[index].time, (float) this.m_Keyframes[index].value, this.m_Keyframes[index].m_InTangent, this.m_Keyframes[index].m_OutTangent)
          {
            tangentMode = this.m_Keyframes[index].m_TangentMode
          });
          num = this.m_Keyframes[index].time;
        }
      }
      animationCurve.keys = keyframeList.ToArray();
      return animationCurve;
    }

    public ObjectReferenceKeyframe[] ToObjectCurve()
    {
      int count = this.m_Keyframes.Count;
      List<ObjectReferenceKeyframe> referenceKeyframeList = new List<ObjectReferenceKeyframe>();
      float num = float.MinValue;
      for (int index = 0; index < count; ++index)
      {
        if ((double) Mathf.Abs(this.m_Keyframes[index].time - num) > 9.99999974737875E-06)
        {
          ObjectReferenceKeyframe referenceKeyframe = new ObjectReferenceKeyframe();
          referenceKeyframe.time = this.m_Keyframes[index].time;
          referenceKeyframe.value = (UnityEngine.Object) this.m_Keyframes[index].value;
          num = referenceKeyframe.time;
          referenceKeyframeList.Add(referenceKeyframe);
        }
      }
      return referenceKeyframeList.ToArray();
    }

    public AnimationWindowKeyframe FindKeyAtTime(AnimationKeyTime keyTime)
    {
      int keyframeIndex = this.GetKeyframeIndex(keyTime);
      if (keyframeIndex == -1)
        return (AnimationWindowKeyframe) null;
      return this.m_Keyframes[keyframeIndex];
    }

    public object Evaluate(float time)
    {
      if (this.m_Keyframes.Count == 0)
        return !this.isPPtrCurve ? (object) 0.0f : (object) null;
      AnimationWindowKeyframe keyframe1 = this.m_Keyframes[0];
      if ((double) time <= (double) keyframe1.time)
        return keyframe1.value;
      AnimationWindowKeyframe keyframe2 = this.m_Keyframes[this.m_Keyframes.Count - 1];
      if ((double) time >= (double) keyframe2.time)
        return keyframe2.value;
      AnimationWindowKeyframe animationWindowKeyframe = keyframe1;
      for (int index = 1; index < this.m_Keyframes.Count; ++index)
      {
        AnimationWindowKeyframe keyframe3 = this.m_Keyframes[index];
        if ((double) animationWindowKeyframe.time < (double) time && (double) keyframe3.time >= (double) time)
        {
          if (this.isPPtrCurve)
            return animationWindowKeyframe.value;
          Keyframe keyframe4 = new Keyframe(animationWindowKeyframe.time, (float) animationWindowKeyframe.value, animationWindowKeyframe.m_InTangent, animationWindowKeyframe.m_OutTangent);
          Keyframe keyframe5 = new Keyframe(keyframe3.time, (float) keyframe3.value, keyframe3.m_InTangent, keyframe3.m_OutTangent);
          return (object) new AnimationCurve() { keys = new Keyframe[2]{ keyframe4, keyframe5 } }.Evaluate(time);
        }
        animationWindowKeyframe = keyframe3;
      }
      return !this.isPPtrCurve ? (object) 0.0f : (object) null;
    }

    public void AddKeyframe(AnimationWindowKeyframe key, AnimationKeyTime keyTime)
    {
      this.RemoveKeyframe(keyTime);
      this.m_Keyframes.Add(key);
      this.m_Keyframes.Sort((Comparison<AnimationWindowKeyframe>) ((a, b) => a.time.CompareTo(b.time)));
    }

    public void RemoveKeyframe(AnimationKeyTime time)
    {
      for (int index = this.m_Keyframes.Count - 1; index >= 0; --index)
      {
        if (time.ContainsTime(this.m_Keyframes[index].time))
          this.m_Keyframes.RemoveAt(index);
      }
    }

    public bool HasKeyframe(AnimationKeyTime time)
    {
      return this.GetKeyframeIndex(time) != -1;
    }

    public int GetKeyframeIndex(AnimationKeyTime time)
    {
      for (int index = 0; index < this.m_Keyframes.Count; ++index)
      {
        if (time.ContainsTime(this.m_Keyframes[index].time))
          return index;
      }
      return -1;
    }

    public void RemoveKeysAtRange(float startTime, float endTime)
    {
      for (int index = this.m_Keyframes.Count - 1; index >= 0; --index)
      {
        if (Mathf.Approximately(endTime, this.m_Keyframes[index].time) || (double) this.m_Keyframes[index].time > (double) startTime && (double) this.m_Keyframes[index].time < (double) endTime)
          this.m_Keyframes.RemoveAt(index);
      }
    }
  }
}
