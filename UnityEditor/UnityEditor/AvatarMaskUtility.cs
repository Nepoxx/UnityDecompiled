// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarMaskUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class AvatarMaskUtility
  {
    private static string sHuman = "m_HumanDescription.m_Human";
    private static string sBoneName = "m_BoneName";

    public static string[] GetAvatarHumanTransform(SerializedObject so, string[] refTransformsPath)
    {
      SerializedProperty property = so.FindProperty(AvatarMaskUtility.sHuman);
      if (property == null || !property.isArray)
        return (string[]) null;
      List<string> stringList = new List<string>();
      for (int index = 0; index < property.arraySize; ++index)
      {
        SerializedProperty propertyRelative = property.GetArrayElementAtIndex(index).FindPropertyRelative(AvatarMaskUtility.sBoneName);
        stringList.Add(propertyRelative.stringValue);
      }
      return AvatarMaskUtility.TokeniseHumanTransformsPath(refTransformsPath, stringList.ToArray());
    }

    public static string[] GetAvatarHumanAndActiveExtraTransforms(SerializedObject so, SerializedProperty transformMaskProperty, string[] refTransformsPath)
    {
      SerializedProperty property = so.FindProperty(AvatarMaskUtility.sHuman);
      if (property == null || !property.isArray)
        return (string[]) null;
      List<string> stringList1 = new List<string>();
      for (int index = 0; index < property.arraySize; ++index)
      {
        SerializedProperty propertyRelative = property.GetArrayElementAtIndex(index).FindPropertyRelative(AvatarMaskUtility.sBoneName);
        stringList1.Add(propertyRelative.stringValue);
      }
      List<string> stringList2 = new List<string>((IEnumerable<string>) AvatarMaskUtility.TokeniseHumanTransformsPath(refTransformsPath, stringList1.ToArray()));
      for (int index = 0; index < transformMaskProperty.arraySize; ++index)
      {
        float floatValue = transformMaskProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_Weight").floatValue;
        string stringValue = transformMaskProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_Path").stringValue;
        if ((double) floatValue > 0.0 && !stringList2.Contains(stringValue))
          stringList2.Add(stringValue);
      }
      return stringList2.ToArray();
    }

    public static string[] GetAvatarInactiveTransformMaskPaths(SerializedProperty transformMaskProperty)
    {
      if (transformMaskProperty == null || !transformMaskProperty.isArray)
        return (string[]) null;
      List<string> stringList = new List<string>();
      for (int index = 0; index < transformMaskProperty.arraySize; ++index)
      {
        if ((double) transformMaskProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_Weight").floatValue < 0.5)
        {
          SerializedProperty propertyRelative = transformMaskProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_Path");
          stringList.Add(propertyRelative.stringValue);
        }
      }
      return stringList.ToArray();
    }

    public static void UpdateTransformMask(AvatarMask mask, string[] refTransformsPath, string[] humanTransforms)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey0 maskCAnonStorey0 = new AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      maskCAnonStorey0.refTransformsPath = refTransformsPath;
      // ISSUE: reference to a compiler-generated field
      mask.transformCount = maskCAnonStorey0.refTransformsPath.Length;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey1 maskCAnonStorey1 = new AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      maskCAnonStorey1.\u003C\u003Ef__ref\u00240 = maskCAnonStorey0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (maskCAnonStorey1.i = 0; maskCAnonStorey1.i < maskCAnonStorey0.refTransformsPath.Length; ++maskCAnonStorey1.i)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        mask.SetTransformPath(maskCAnonStorey1.i, maskCAnonStorey0.refTransformsPath[maskCAnonStorey1.i]);
        // ISSUE: reference to a compiler-generated method
        bool flag = humanTransforms == null || ArrayUtility.FindIndex<string>(humanTransforms, new Predicate<string>(maskCAnonStorey1.\u003C\u003Em__0)) != -1;
        // ISSUE: reference to a compiler-generated field
        mask.SetTransformActive(maskCAnonStorey1.i, flag);
      }
    }

    public static void UpdateTransformMask(SerializedProperty transformMask, string[] refTransformsPath, string[] currentPaths, bool areActivePaths = true)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey2 maskCAnonStorey2 = new AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey2();
      // ISSUE: reference to a compiler-generated field
      maskCAnonStorey2.refTransformsPath = refTransformsPath;
      AvatarMask mask = new AvatarMask();
      // ISSUE: reference to a compiler-generated field
      mask.transformCount = maskCAnonStorey2.refTransformsPath.Length;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey3 maskCAnonStorey3 = new AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey3();
      // ISSUE: reference to a compiler-generated field
      maskCAnonStorey3.\u003C\u003Ef__ref\u00242 = maskCAnonStorey2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (maskCAnonStorey3.i = 0; maskCAnonStorey3.i < maskCAnonStorey2.refTransformsPath.Length; ++maskCAnonStorey3.i)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        bool flag = currentPaths == null || (!areActivePaths ? ArrayUtility.FindIndex<string>(currentPaths, new Predicate<string>(maskCAnonStorey3.\u003C\u003Em__1)) == -1 : ArrayUtility.FindIndex<string>(currentPaths, new Predicate<string>(maskCAnonStorey3.\u003C\u003Em__0)) != -1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        mask.SetTransformPath(maskCAnonStorey3.i, maskCAnonStorey2.refTransformsPath[maskCAnonStorey3.i]);
        // ISSUE: reference to a compiler-generated field
        mask.SetTransformActive(maskCAnonStorey3.i, flag);
      }
      ModelImporter.UpdateTransformMask(mask, transformMask);
    }

    public static void SetActiveHumanTransforms(AvatarMask mask, string[] humanTransforms)
    {
      for (int index = 0; index < mask.transformCount; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated method
        if (ArrayUtility.FindIndex<string>(humanTransforms, new Predicate<string>(new AvatarMaskUtility.\u003CSetActiveHumanTransforms\u003Ec__AnonStorey4() { path = mask.GetTransformPath(index) }.\u003C\u003Em__0)) != -1)
          mask.SetTransformActive(index, true);
      }
    }

    private static string[] TokeniseHumanTransformsPath(string[] refTransformsPath, string[] humanTransforms)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvatarMaskUtility.\u003CTokeniseHumanTransformsPath\u003Ec__AnonStorey5 pathCAnonStorey5 = new AvatarMaskUtility.\u003CTokeniseHumanTransformsPath\u003Ec__AnonStorey5();
      // ISSUE: reference to a compiler-generated field
      pathCAnonStorey5.humanTransforms = humanTransforms;
      // ISSUE: reference to a compiler-generated field
      if (pathCAnonStorey5.humanTransforms == null)
        return (string[]) null;
      string[] array = new string[1]{ "" };
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvatarMaskUtility.\u003CTokeniseHumanTransformsPath\u003Ec__AnonStorey6 pathCAnonStorey6 = new AvatarMaskUtility.\u003CTokeniseHumanTransformsPath\u003Ec__AnonStorey6();
      // ISSUE: reference to a compiler-generated field
      pathCAnonStorey6.\u003C\u003Ef__ref\u00245 = pathCAnonStorey5;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (pathCAnonStorey6.i = 0; pathCAnonStorey6.i < pathCAnonStorey5.humanTransforms.Length; ++pathCAnonStorey6.i)
      {
        // ISSUE: reference to a compiler-generated method
        int index = ArrayUtility.FindIndex<string>(refTransformsPath, new Predicate<string>(pathCAnonStorey6.\u003C\u003Em__0));
        if (index != -1)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          AvatarMaskUtility.\u003CTokeniseHumanTransformsPath\u003Ec__AnonStorey7 pathCAnonStorey7 = new AvatarMaskUtility.\u003CTokeniseHumanTransformsPath\u003Ec__AnonStorey7();
          int length = array.Length;
          int num;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          for (pathCAnonStorey7.path = refTransformsPath[index]; pathCAnonStorey7.path.Length > 0; pathCAnonStorey7.path = pathCAnonStorey7.path.Substring(0, num == -1 ? 0 : num))
          {
            // ISSUE: reference to a compiler-generated method
            if (ArrayUtility.FindIndex<string>(array, new Predicate<string>(pathCAnonStorey7.\u003C\u003Em__0)) == -1)
            {
              // ISSUE: reference to a compiler-generated field
              ArrayUtility.Insert<string>(ref array, length, pathCAnonStorey7.path);
            }
            // ISSUE: reference to a compiler-generated field
            num = pathCAnonStorey7.path.LastIndexOf('/');
          }
        }
      }
      return array;
    }
  }
}
