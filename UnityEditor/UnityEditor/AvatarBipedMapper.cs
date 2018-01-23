// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarBipedMapper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class AvatarBipedMapper
  {
    private static AvatarBipedMapper.BipedBone[] s_BipedBones = new AvatarBipedMapper.BipedBone[52]{ new AvatarBipedMapper.BipedBone("Pelvis", 0), new AvatarBipedMapper.BipedBone("L Thigh", 1), new AvatarBipedMapper.BipedBone("R Thigh", 2), new AvatarBipedMapper.BipedBone("L Calf", 3), new AvatarBipedMapper.BipedBone("R Calf", 4), new AvatarBipedMapper.BipedBone("L Foot", 5), new AvatarBipedMapper.BipedBone("R Foot", 6), new AvatarBipedMapper.BipedBone("Spine", 7), new AvatarBipedMapper.BipedBone("Spine1", 8), new AvatarBipedMapper.BipedBone("Spine2", 54), new AvatarBipedMapper.BipedBone("Neck", 9), new AvatarBipedMapper.BipedBone("Head", 10), new AvatarBipedMapper.BipedBone("L Clavicle", 11), new AvatarBipedMapper.BipedBone("R Clavicle", 12), new AvatarBipedMapper.BipedBone("L UpperArm", 13), new AvatarBipedMapper.BipedBone("R UpperArm", 14), new AvatarBipedMapper.BipedBone("L Forearm", 15), new AvatarBipedMapper.BipedBone("R Forearm", 16), new AvatarBipedMapper.BipedBone("L Hand", 17), new AvatarBipedMapper.BipedBone("R Hand", 18), new AvatarBipedMapper.BipedBone("L Toe0", 19), new AvatarBipedMapper.BipedBone("R Toe0", 20), new AvatarBipedMapper.BipedBone("L Finger0", 24), new AvatarBipedMapper.BipedBone("L Finger01", 25), new AvatarBipedMapper.BipedBone("L Finger02", 26), new AvatarBipedMapper.BipedBone("L Finger1", 27), new AvatarBipedMapper.BipedBone("L Finger11", 28), new AvatarBipedMapper.BipedBone("L Finger12", 29), new AvatarBipedMapper.BipedBone("L Finger2", 30), new AvatarBipedMapper.BipedBone("L Finger21", 31), new AvatarBipedMapper.BipedBone("L Finger22", 32), new AvatarBipedMapper.BipedBone("L Finger3", 33), new AvatarBipedMapper.BipedBone("L Finger31", 34), new AvatarBipedMapper.BipedBone("L Finger32", 35), new AvatarBipedMapper.BipedBone("L Finger4", 36), new AvatarBipedMapper.BipedBone("L Finger41", 37), new AvatarBipedMapper.BipedBone("L Finger42", 38), new AvatarBipedMapper.BipedBone("R Finger0", 39), new AvatarBipedMapper.BipedBone("R Finger01", 40), new AvatarBipedMapper.BipedBone("R Finger02", 41), new AvatarBipedMapper.BipedBone("R Finger1", 42), new AvatarBipedMapper.BipedBone("R Finger11", 43), new AvatarBipedMapper.BipedBone("R Finger12", 44), new AvatarBipedMapper.BipedBone("R Finger2", 45), new AvatarBipedMapper.BipedBone("R Finger21", 46), new AvatarBipedMapper.BipedBone("R Finger22", 47), new AvatarBipedMapper.BipedBone("R Finger3", 48), new AvatarBipedMapper.BipedBone("R Finger31", 49), new AvatarBipedMapper.BipedBone("R Finger32", 50), new AvatarBipedMapper.BipedBone("R Finger4", 51), new AvatarBipedMapper.BipedBone("R Finger41", 52), new AvatarBipedMapper.BipedBone("R Finger42", 53) };

    public static bool IsBiped(Transform root, List<string> report)
    {
      if (report != null)
        report.Clear();
      Transform[] humanToTransform = new Transform[HumanTrait.BoneCount];
      return AvatarBipedMapper.MapBipedBones(root, ref humanToTransform, report);
    }

    public static Dictionary<int, Transform> MapBones(Transform root)
    {
      Dictionary<int, Transform> dictionary = new Dictionary<int, Transform>();
      Transform[] humanToTransform = new Transform[HumanTrait.BoneCount];
      if (AvatarBipedMapper.MapBipedBones(root, ref humanToTransform, (List<string>) null))
      {
        for (int key = 0; key < HumanTrait.BoneCount; ++key)
        {
          if ((UnityEngine.Object) humanToTransform[key] != (UnityEngine.Object) null)
            dictionary.Add(key, humanToTransform[key]);
        }
      }
      if (!dictionary.ContainsKey(8) && dictionary.ContainsKey(54))
      {
        dictionary.Add(8, dictionary[54]);
        dictionary.Remove(54);
      }
      return dictionary;
    }

    private static bool MapBipedBones(Transform root, ref Transform[] humanToTransform, List<string> report)
    {
      for (int bipedBoneIndex = 0; bipedBoneIndex < AvatarBipedMapper.s_BipedBones.Length; ++bipedBoneIndex)
      {
        int index = AvatarBipedMapper.s_BipedBones[bipedBoneIndex].index;
        int parentBone1 = HumanTrait.GetParentBone(index);
        bool flag1 = HumanTrait.RequiredBone(index);
        bool flag2 = parentBone1 == -1 || HumanTrait.RequiredBone(parentBone1);
        Transform transform = parentBone1 == -1 ? root : humanToTransform[parentBone1];
        if ((UnityEngine.Object) transform == (UnityEngine.Object) null && !flag2)
        {
          int parentBone2 = HumanTrait.GetParentBone(parentBone1);
          bool flag3 = parentBone2 == -1 || HumanTrait.RequiredBone(parentBone2);
          transform = parentBone2 == -1 ? (Transform) null : humanToTransform[parentBone2];
          if ((UnityEngine.Object) transform == (UnityEngine.Object) null && !flag3)
          {
            int parentBone3 = HumanTrait.GetParentBone(parentBone2);
            transform = parentBone3 == -1 ? (Transform) null : humanToTransform[parentBone3];
          }
        }
        humanToTransform[index] = AvatarBipedMapper.MapBipedBone(bipedBoneIndex, transform, transform, report);
        if ((UnityEngine.Object) humanToTransform[index] == (UnityEngine.Object) null && flag1)
          return false;
      }
      return true;
    }

    private static Transform MapBipedBone(int bipedBoneIndex, Transform transform, Transform parentTransform, List<string> report)
    {
      Transform transform1 = (Transform) null;
      if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
      {
        int childCount = transform.childCount;
        for (int index1 = 0; (UnityEngine.Object) transform1 == (UnityEngine.Object) null && index1 < childCount; ++index1)
        {
          string name = AvatarBipedMapper.s_BipedBones[bipedBoneIndex].name;
          int index2 = AvatarBipedMapper.s_BipedBones[bipedBoneIndex].index;
          if (transform.GetChild(index1).name.EndsWith(name))
          {
            transform1 = transform.GetChild(index1);
            if ((UnityEngine.Object) transform1 != (UnityEngine.Object) null && report != null && (index2 != 0 && (UnityEngine.Object) transform != (UnityEngine.Object) parentTransform))
            {
              string str1 = "- Invalid parent for " + transform1.name + ". Expected " + parentTransform.name + ", but found " + transform.name + ".";
              switch (index2)
              {
                case 1:
                case 2:
                  str1 += " Disable Triangle Pelvis";
                  break;
                case 9:
                  str1 += " Preferred is three Spine Links";
                  break;
                case 10:
                  str1 += " Preferred is one Neck Links";
                  break;
                case 11:
                case 12:
                  str1 += " Enable Triangle Neck";
                  break;
              }
              string str2 = str1 + "\n";
              report.Add(str2);
            }
          }
        }
        for (int index = 0; (UnityEngine.Object) transform1 == (UnityEngine.Object) null && index < childCount; ++index)
          transform1 = AvatarBipedMapper.MapBipedBone(bipedBoneIndex, transform.GetChild(index), parentTransform, report);
      }
      return transform1;
    }

    internal static void BipedPose(GameObject go, AvatarSetupTool.BoneWrapper[] bones)
    {
      AvatarBipedMapper.BipedPose(go.transform, true);
      Quaternion orientation = AvatarSetupTool.AvatarComputeOrientation(bones);
      go.transform.rotation = Quaternion.Inverse(orientation) * go.transform.rotation;
      AvatarSetupTool.MakeCharacterPositionValid(bones);
    }

    private static void BipedPose(Transform t, bool ignore)
    {
      if (t.name.EndsWith("Pelvis"))
      {
        t.localRotation = Quaternion.Euler(270f, 90f, 0.0f);
        ignore = false;
      }
      else if (t.name.EndsWith("Thigh"))
        t.localRotation = Quaternion.Euler(0.0f, 180f, 0.0f);
      else if (t.name.EndsWith("Toe0"))
        t.localRotation = Quaternion.Euler(0.0f, 0.0f, 270f);
      else if (t.name.EndsWith("L Clavicle"))
        t.localRotation = Quaternion.Euler(0.0f, 270f, 180f);
      else if (t.name.EndsWith("R Clavicle"))
        t.localRotation = Quaternion.Euler(0.0f, 90f, 180f);
      else if (t.name.EndsWith("L Hand"))
        t.localRotation = Quaternion.Euler(270f, 0.0f, 0.0f);
      else if (t.name.EndsWith("R Hand"))
        t.localRotation = Quaternion.Euler(90f, 0.0f, 0.0f);
      else if (t.name.EndsWith("L Finger0"))
        t.localRotation = Quaternion.Euler(0.0f, 315f, 0.0f);
      else if (t.name.EndsWith("R Finger0"))
        t.localRotation = Quaternion.Euler(0.0f, 45f, 0.0f);
      else if (!ignore)
        t.localRotation = Quaternion.identity;
      IEnumerator enumerator = t.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AvatarBipedMapper.BipedPose((Transform) enumerator.Current, ignore);
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    private struct BipedBone
    {
      public string name;
      public int index;

      public BipedBone(string name, int index)
      {
        this.name = name;
        this.index = index;
      }
    }
  }
}
