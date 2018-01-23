// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.MaterialAnimationUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal static class MaterialAnimationUtility
  {
    private const string kMaterialPrefix = "material.";

    private static PropertyModification[] CreatePropertyModifications(int count, UnityEngine.Object target)
    {
      PropertyModification[] propertyModificationArray = new PropertyModification[count];
      for (int index = 0; index < propertyModificationArray.Length; ++index)
      {
        propertyModificationArray[index] = new PropertyModification();
        propertyModificationArray[index].target = target;
      }
      return propertyModificationArray;
    }

    private static void SetupPropertyModification(string name, float value, PropertyModification prop)
    {
      prop.propertyPath = "material." + name;
      prop.value = value.ToString();
    }

    private static PropertyModification[] MaterialPropertyToPropertyModifications(MaterialProperty materialProp, UnityEngine.Object target, float value)
    {
      PropertyModification[] propertyModifications = MaterialAnimationUtility.CreatePropertyModifications(1, target);
      MaterialAnimationUtility.SetupPropertyModification(materialProp.name, value, propertyModifications[0]);
      return propertyModifications;
    }

    private static PropertyModification[] MaterialPropertyToPropertyModifications(MaterialProperty materialProp, UnityEngine.Object target, Color color)
    {
      PropertyModification[] propertyModifications = MaterialAnimationUtility.CreatePropertyModifications(4, target);
      MaterialAnimationUtility.SetupPropertyModification(materialProp.name + ".r", color.r, propertyModifications[0]);
      MaterialAnimationUtility.SetupPropertyModification(materialProp.name + ".g", color.g, propertyModifications[1]);
      MaterialAnimationUtility.SetupPropertyModification(materialProp.name + ".b", color.b, propertyModifications[2]);
      MaterialAnimationUtility.SetupPropertyModification(materialProp.name + ".a", color.a, propertyModifications[3]);
      return propertyModifications;
    }

    private static PropertyModification[] MaterialPropertyToPropertyModifications(string name, UnityEngine.Object target, Vector4 vec)
    {
      PropertyModification[] propertyModifications = MaterialAnimationUtility.CreatePropertyModifications(4, target);
      MaterialAnimationUtility.SetupPropertyModification(name + ".x", vec.x, propertyModifications[0]);
      MaterialAnimationUtility.SetupPropertyModification(name + ".y", vec.y, propertyModifications[1]);
      MaterialAnimationUtility.SetupPropertyModification(name + ".z", vec.z, propertyModifications[2]);
      MaterialAnimationUtility.SetupPropertyModification(name + ".w", vec.w, propertyModifications[3]);
      return propertyModifications;
    }

    private static bool ApplyMaterialModificationToAnimationRecording(PropertyModification[] modifications)
    {
      UndoPropertyModification[] modifications1 = new UndoPropertyModification[modifications.Length];
      for (int index = 0; index < modifications1.Length; ++index)
        modifications1[index].previousValue = modifications[index];
      return Undo.postprocessModifications(modifications1).Length != modifications.Length;
    }

    public static bool OverridePropertyColor(MaterialProperty materialProp, Renderer target, out Color color)
    {
      List<string> stringList = new List<string>();
      string str = "material." + materialProp.name;
      if (materialProp.type == MaterialProperty.PropType.Texture)
      {
        stringList.Add(str + "_ST.x");
        stringList.Add(str + "_ST.y");
        stringList.Add(str + "_ST.z");
        stringList.Add(str + "_ST.w");
      }
      else if (materialProp.type == MaterialProperty.PropType.Color)
      {
        stringList.Add(str + ".r");
        stringList.Add(str + ".g");
        stringList.Add(str + ".b");
        stringList.Add(str + ".a");
      }
      else
        stringList.Add(str);
      if (stringList.Exists((Predicate<string>) (path => AnimationMode.IsPropertyAnimated((UnityEngine.Object) target, path))))
      {
        color = AnimationMode.animatedPropertyColor;
        if (AnimationMode.InAnimationRecording())
          color = AnimationMode.recordedPropertyColor;
        else if (stringList.Exists((Predicate<string>) (path => AnimationMode.IsPropertyCandidate((UnityEngine.Object) target, path))))
          color = AnimationMode.candidatePropertyColor;
        return true;
      }
      color = Color.white;
      return false;
    }

    public static void SetupMaterialPropertyBlock(MaterialProperty materialProp, int changedMask, Renderer target)
    {
      MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
      target.GetPropertyBlock(materialPropertyBlock);
      materialProp.WriteToMaterialPropertyBlock(materialPropertyBlock, changedMask);
      target.SetPropertyBlock(materialPropertyBlock);
    }

    public static void TearDownMaterialPropertyBlock(Renderer target)
    {
      target.SetPropertyBlock((MaterialPropertyBlock) null);
    }

    public static bool ApplyMaterialModificationToAnimationRecording(MaterialProperty materialProp, int changedMask, Renderer target, object oldValue)
    {
      switch (materialProp.type)
      {
        case MaterialProperty.PropType.Color:
          MaterialAnimationUtility.SetupMaterialPropertyBlock(materialProp, changedMask, target);
          bool animationRecording1 = MaterialAnimationUtility.ApplyMaterialModificationToAnimationRecording(MaterialAnimationUtility.MaterialPropertyToPropertyModifications(materialProp, (UnityEngine.Object) target, (Color) oldValue));
          if (!animationRecording1)
            MaterialAnimationUtility.TearDownMaterialPropertyBlock(target);
          return animationRecording1;
        case MaterialProperty.PropType.Vector:
          MaterialAnimationUtility.SetupMaterialPropertyBlock(materialProp, changedMask, target);
          bool animationRecording2 = MaterialAnimationUtility.ApplyMaterialModificationToAnimationRecording(MaterialAnimationUtility.MaterialPropertyToPropertyModifications(materialProp, (UnityEngine.Object) target, (Color) ((Vector4) oldValue)));
          if (!animationRecording2)
            MaterialAnimationUtility.TearDownMaterialPropertyBlock(target);
          return animationRecording2;
        case MaterialProperty.PropType.Float:
        case MaterialProperty.PropType.Range:
          MaterialAnimationUtility.SetupMaterialPropertyBlock(materialProp, changedMask, target);
          bool animationRecording3 = MaterialAnimationUtility.ApplyMaterialModificationToAnimationRecording(MaterialAnimationUtility.MaterialPropertyToPropertyModifications(materialProp, (UnityEngine.Object) target, (float) oldValue));
          if (!animationRecording3)
            MaterialAnimationUtility.TearDownMaterialPropertyBlock(target);
          return animationRecording3;
        case MaterialProperty.PropType.Texture:
          if (!MaterialProperty.IsTextureOffsetAndScaleChangedMask(changedMask))
            return false;
          string name = materialProp.name + "_ST";
          MaterialAnimationUtility.SetupMaterialPropertyBlock(materialProp, changedMask, target);
          bool animationRecording4 = MaterialAnimationUtility.ApplyMaterialModificationToAnimationRecording(MaterialAnimationUtility.MaterialPropertyToPropertyModifications(name, (UnityEngine.Object) target, (Vector4) oldValue));
          if (!animationRecording4)
            MaterialAnimationUtility.TearDownMaterialPropertyBlock(target);
          return animationRecording4;
        default:
          return false;
      }
    }

    public static PropertyModification[] MaterialPropertyToPropertyModifications(MaterialProperty materialProp, Renderer target)
    {
      switch (materialProp.type)
      {
        case MaterialProperty.PropType.Color:
          return MaterialAnimationUtility.MaterialPropertyToPropertyModifications(materialProp, (UnityEngine.Object) target, materialProp.colorValue);
        case MaterialProperty.PropType.Vector:
          return MaterialAnimationUtility.MaterialPropertyToPropertyModifications(materialProp, (UnityEngine.Object) target, (Color) materialProp.vectorValue);
        case MaterialProperty.PropType.Float:
        case MaterialProperty.PropType.Range:
          return MaterialAnimationUtility.MaterialPropertyToPropertyModifications(materialProp, (UnityEngine.Object) target, materialProp.floatValue);
        case MaterialProperty.PropType.Texture:
          return MaterialAnimationUtility.MaterialPropertyToPropertyModifications(materialProp.name + "_ST", (UnityEngine.Object) target, materialProp.vectorValue);
        default:
          return (PropertyModification[]) null;
      }
    }
  }
}
