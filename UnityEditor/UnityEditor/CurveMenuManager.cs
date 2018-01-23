// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveMenuManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class CurveMenuManager
  {
    private CurveUpdater updater;

    public CurveMenuManager(CurveUpdater updater)
    {
      this.updater = updater;
    }

    public void AddTangentMenuItems(GenericMenu menu, List<KeyIdentifier> keyList)
    {
      bool flag = keyList.Count > 0;
      bool on1 = flag;
      bool on2 = flag;
      bool on3 = flag;
      bool on4 = flag;
      bool on5 = flag;
      bool on6 = flag;
      bool on7 = flag;
      bool on8 = flag;
      bool on9 = flag;
      bool on10 = flag;
      bool on11 = flag;
      foreach (KeyIdentifier key in keyList)
      {
        Keyframe keyframe = key.keyframe;
        AnimationUtility.TangentMode keyLeftTangentMode = AnimationUtility.GetKeyLeftTangentMode(keyframe);
        AnimationUtility.TangentMode rightTangentMode = AnimationUtility.GetKeyRightTangentMode(keyframe);
        bool keyBroken = AnimationUtility.GetKeyBroken(keyframe);
        if (keyLeftTangentMode != AnimationUtility.TangentMode.ClampedAuto || rightTangentMode != AnimationUtility.TangentMode.ClampedAuto)
          on1 = false;
        if (keyLeftTangentMode != AnimationUtility.TangentMode.Auto || rightTangentMode != AnimationUtility.TangentMode.Auto)
          on2 = false;
        if (keyBroken || keyLeftTangentMode != AnimationUtility.TangentMode.Free || rightTangentMode != AnimationUtility.TangentMode.Free)
          on3 = false;
        if (keyBroken || keyLeftTangentMode != AnimationUtility.TangentMode.Free || ((double) keyframe.inTangent != 0.0 || rightTangentMode != AnimationUtility.TangentMode.Free) || (double) keyframe.outTangent != 0.0)
          on4 = false;
        if (!keyBroken)
          on5 = false;
        if (!keyBroken || keyLeftTangentMode != AnimationUtility.TangentMode.Free)
          on6 = false;
        if (!keyBroken || keyLeftTangentMode != AnimationUtility.TangentMode.Linear)
          on7 = false;
        if (!keyBroken || keyLeftTangentMode != AnimationUtility.TangentMode.Constant)
          on8 = false;
        if (!keyBroken || rightTangentMode != AnimationUtility.TangentMode.Free)
          on9 = false;
        if (!keyBroken || rightTangentMode != AnimationUtility.TangentMode.Linear)
          on10 = false;
        if (!keyBroken || rightTangentMode != AnimationUtility.TangentMode.Constant)
          on11 = false;
      }
      if (flag)
      {
        menu.AddItem(EditorGUIUtility.TextContent("Clamped Auto"), on1, new GenericMenu.MenuFunction2(this.SetClampedAuto), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Auto"), on2, new GenericMenu.MenuFunction2(this.SetAuto), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Free Smooth"), on3, new GenericMenu.MenuFunction2(this.SetEditable), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Flat"), on4, new GenericMenu.MenuFunction2(this.SetFlat), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Broken"), on5, new GenericMenu.MenuFunction2(this.SetBroken), (object) keyList);
        menu.AddSeparator("");
        menu.AddItem(EditorGUIUtility.TextContent("Left Tangent/Free"), on6, new GenericMenu.MenuFunction2(this.SetLeftEditable), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Left Tangent/Linear"), on7, new GenericMenu.MenuFunction2(this.SetLeftLinear), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Left Tangent/Constant"), on8, new GenericMenu.MenuFunction2(this.SetLeftConstant), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Right Tangent/Free"), on9, new GenericMenu.MenuFunction2(this.SetRightEditable), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Right Tangent/Linear"), on10, new GenericMenu.MenuFunction2(this.SetRightLinear), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Right Tangent/Constant"), on11, new GenericMenu.MenuFunction2(this.SetRightConstant), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Both Tangents/Free"), on9 && on6, new GenericMenu.MenuFunction2(this.SetBothEditable), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Both Tangents/Linear"), on10 && on7, new GenericMenu.MenuFunction2(this.SetBothLinear), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Both Tangents/Constant"), on11 && on8, new GenericMenu.MenuFunction2(this.SetBothConstant), (object) keyList);
      }
      else
      {
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Clamped Auto"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Auto"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Free Smooth"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Flat"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Broken"));
        menu.AddSeparator("");
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Left Tangent/Free"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Left Tangent/Linear"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Left Tangent/Constant"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Right Tangent/Free"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Right Tangent/Linear"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Right Tangent/Constant"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Both Tangents/Free"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Both Tangents/Linear"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Both Tangents/Constant"));
      }
    }

    public void SetClampedAuto(object keysToSet)
    {
      this.SetBoth(AnimationUtility.TangentMode.ClampedAuto, (List<KeyIdentifier>) keysToSet);
    }

    public void SetAuto(object keysToSet)
    {
      this.SetBoth(AnimationUtility.TangentMode.Auto, (List<KeyIdentifier>) keysToSet);
    }

    public void SetEditable(object keysToSet)
    {
      this.SetBoth(AnimationUtility.TangentMode.Free, (List<KeyIdentifier>) keysToSet);
    }

    public void SetFlat(object keysToSet)
    {
      this.SetBoth(AnimationUtility.TangentMode.Free, (List<KeyIdentifier>) keysToSet);
      this.Flatten((List<KeyIdentifier>) keysToSet);
    }

    public void SetBoth(AnimationUtility.TangentMode mode, List<KeyIdentifier> keysToSet)
    {
      List<ChangedCurve> curve1 = new List<ChangedCurve>();
      foreach (KeyIdentifier keysTo in keysToSet)
      {
        AnimationCurve curve2 = keysTo.curve;
        Keyframe keyframe = keysTo.keyframe;
        AnimationUtility.SetKeyBroken(ref keyframe, false);
        AnimationUtility.SetKeyRightTangentMode(ref keyframe, mode);
        AnimationUtility.SetKeyLeftTangentMode(ref keyframe, mode);
        if (mode == AnimationUtility.TangentMode.Free)
        {
          float smoothTangent = CurveUtility.CalculateSmoothTangent(keyframe);
          keyframe.inTangent = smoothTangent;
          keyframe.outTangent = smoothTangent;
        }
        curve2.MoveKey(keysTo.key, keyframe);
        AnimationUtility.UpdateTangentsFromModeSurrounding(curve2, keysTo.key);
        ChangedCurve changedCurve = new ChangedCurve(curve2, keysTo.curveId, keysTo.binding);
        if (!curve1.Contains(changedCurve))
          curve1.Add(changedCurve);
      }
      this.updater.UpdateCurves(curve1, "Set Tangents");
    }

    public void Flatten(List<KeyIdentifier> keysToSet)
    {
      List<ChangedCurve> curve1 = new List<ChangedCurve>();
      foreach (KeyIdentifier keysTo in keysToSet)
      {
        AnimationCurve curve2 = keysTo.curve;
        Keyframe keyframe = keysTo.keyframe;
        keyframe.inTangent = 0.0f;
        keyframe.outTangent = 0.0f;
        curve2.MoveKey(keysTo.key, keyframe);
        AnimationUtility.UpdateTangentsFromModeSurrounding(curve2, keysTo.key);
        ChangedCurve changedCurve = new ChangedCurve(curve2, keysTo.curveId, keysTo.binding);
        if (!curve1.Contains(changedCurve))
          curve1.Add(changedCurve);
      }
      this.updater.UpdateCurves(curve1, "Set Tangents");
    }

    public void SetBroken(object _keysToSet)
    {
      List<ChangedCurve> curve1 = new List<ChangedCurve>();
      foreach (KeyIdentifier keysTo in (List<KeyIdentifier>) _keysToSet)
      {
        AnimationCurve curve2 = keysTo.curve;
        Keyframe keyframe = keysTo.keyframe;
        AnimationUtility.SetKeyBroken(ref keyframe, true);
        if (AnimationUtility.GetKeyRightTangentMode(keyframe) == AnimationUtility.TangentMode.ClampedAuto || AnimationUtility.GetKeyRightTangentMode(keyframe) == AnimationUtility.TangentMode.Auto)
          AnimationUtility.SetKeyRightTangentMode(ref keyframe, AnimationUtility.TangentMode.Free);
        if (AnimationUtility.GetKeyLeftTangentMode(keyframe) == AnimationUtility.TangentMode.ClampedAuto || AnimationUtility.GetKeyLeftTangentMode(keyframe) == AnimationUtility.TangentMode.Auto)
          AnimationUtility.SetKeyLeftTangentMode(ref keyframe, AnimationUtility.TangentMode.Free);
        curve2.MoveKey(keysTo.key, keyframe);
        AnimationUtility.UpdateTangentsFromModeSurrounding(curve2, keysTo.key);
        ChangedCurve changedCurve = new ChangedCurve(curve2, keysTo.curveId, keysTo.binding);
        if (!curve1.Contains(changedCurve))
          curve1.Add(changedCurve);
      }
      this.updater.UpdateCurves(curve1, "Set Tangents");
    }

    public void SetLeftEditable(object keysToSet)
    {
      this.SetTangent(0, AnimationUtility.TangentMode.Free, (List<KeyIdentifier>) keysToSet);
    }

    public void SetLeftLinear(object keysToSet)
    {
      this.SetTangent(0, AnimationUtility.TangentMode.Linear, (List<KeyIdentifier>) keysToSet);
    }

    public void SetLeftConstant(object keysToSet)
    {
      this.SetTangent(0, AnimationUtility.TangentMode.Constant, (List<KeyIdentifier>) keysToSet);
    }

    public void SetRightEditable(object keysToSet)
    {
      this.SetTangent(1, AnimationUtility.TangentMode.Free, (List<KeyIdentifier>) keysToSet);
    }

    public void SetRightLinear(object keysToSet)
    {
      this.SetTangent(1, AnimationUtility.TangentMode.Linear, (List<KeyIdentifier>) keysToSet);
    }

    public void SetRightConstant(object keysToSet)
    {
      this.SetTangent(1, AnimationUtility.TangentMode.Constant, (List<KeyIdentifier>) keysToSet);
    }

    public void SetBothEditable(object keysToSet)
    {
      this.SetTangent(2, AnimationUtility.TangentMode.Free, (List<KeyIdentifier>) keysToSet);
    }

    public void SetBothLinear(object keysToSet)
    {
      this.SetTangent(2, AnimationUtility.TangentMode.Linear, (List<KeyIdentifier>) keysToSet);
    }

    public void SetBothConstant(object keysToSet)
    {
      this.SetTangent(2, AnimationUtility.TangentMode.Constant, (List<KeyIdentifier>) keysToSet);
    }

    public void SetTangent(int leftRight, AnimationUtility.TangentMode mode, List<KeyIdentifier> keysToSet)
    {
      List<ChangedCurve> curve1 = new List<ChangedCurve>();
      foreach (KeyIdentifier keysTo in keysToSet)
      {
        AnimationCurve curve2 = keysTo.curve;
        Keyframe keyframe = keysTo.keyframe;
        AnimationUtility.SetKeyBroken(ref keyframe, true);
        switch (leftRight)
        {
          case 0:
            AnimationUtility.SetKeyLeftTangentMode(ref keyframe, mode);
            if (AnimationUtility.GetKeyRightTangentMode(keyframe) == AnimationUtility.TangentMode.ClampedAuto || AnimationUtility.GetKeyRightTangentMode(keyframe) == AnimationUtility.TangentMode.Auto)
            {
              AnimationUtility.SetKeyRightTangentMode(ref keyframe, AnimationUtility.TangentMode.Free);
              break;
            }
            break;
          case 2:
            AnimationUtility.SetKeyLeftTangentMode(ref keyframe, mode);
            AnimationUtility.SetKeyRightTangentMode(ref keyframe, mode);
            goto label_10;
          default:
            AnimationUtility.SetKeyRightTangentMode(ref keyframe, mode);
            if (AnimationUtility.GetKeyLeftTangentMode(keyframe) == AnimationUtility.TangentMode.ClampedAuto || AnimationUtility.GetKeyLeftTangentMode(keyframe) == AnimationUtility.TangentMode.Auto)
              AnimationUtility.SetKeyLeftTangentMode(ref keyframe, AnimationUtility.TangentMode.Free);
            break;
        }
label_10:
        if (mode == AnimationUtility.TangentMode.Constant && (leftRight == 0 || leftRight == 2))
          keyframe.inTangent = float.PositiveInfinity;
        if (mode == AnimationUtility.TangentMode.Constant && (leftRight == 1 || leftRight == 2))
          keyframe.outTangent = float.PositiveInfinity;
        curve2.MoveKey(keysTo.key, keyframe);
        AnimationUtility.UpdateTangentsFromModeSurrounding(curve2, keysTo.key);
        ChangedCurve changedCurve = new ChangedCurve(curve2, keysTo.curveId, keysTo.binding);
        if (!curve1.Contains(changedCurve))
          curve1.Add(changedCurve);
      }
      this.updater.UpdateCurves(curve1, "Set Tangents");
    }
  }
}
