// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal static class CurveUtility
  {
    private static Texture2D iconKey;
    private static Texture2D iconCurve;

    public static int GetPathAndTypeID(string path, System.Type type)
    {
      return path.GetHashCode() * 27 ^ type.GetHashCode();
    }

    public static Texture2D GetIconCurve()
    {
      if ((UnityEngine.Object) CurveUtility.iconCurve == (UnityEngine.Object) null)
        CurveUtility.iconCurve = EditorGUIUtility.LoadIcon("animationanimated");
      return CurveUtility.iconCurve;
    }

    public static Texture2D GetIconKey()
    {
      if ((UnityEngine.Object) CurveUtility.iconKey == (UnityEngine.Object) null)
        CurveUtility.iconKey = EditorGUIUtility.LoadIcon("animationkeyframe");
      return CurveUtility.iconKey;
    }

    public static bool HaveKeysInRange(AnimationCurve curve, float beginTime, float endTime)
    {
      for (int index = curve.length - 1; index >= 0; --index)
      {
        if ((double) curve[index].time >= (double) beginTime && (double) curve[index].time < (double) endTime)
          return true;
      }
      return false;
    }

    public static void RemoveKeysInRange(AnimationCurve curve, float beginTime, float endTime)
    {
      for (int index = curve.length - 1; index >= 0; --index)
      {
        if ((double) curve[index].time >= (double) beginTime && (double) curve[index].time < (double) endTime)
          curve.RemoveKey(index);
      }
    }

    public static float CalculateSmoothTangent(Keyframe key)
    {
      if ((double) key.inTangent == double.PositiveInfinity)
        key.inTangent = 0.0f;
      if ((double) key.outTangent == double.PositiveInfinity)
        key.outTangent = 0.0f;
      return (float) (((double) key.outTangent + (double) key.inTangent) * 0.5);
    }

    public static void SetKeyModeFromContext(AnimationCurve curve, int keyIndex)
    {
      Keyframe key = curve[keyIndex];
      bool broken = false;
      bool flag = false;
      if (keyIndex > 0)
      {
        if (AnimationUtility.GetKeyBroken(curve[keyIndex - 1]))
          broken = true;
        switch (AnimationUtility.GetKeyRightTangentMode(curve[keyIndex - 1]))
        {
          case AnimationUtility.TangentMode.Auto:
          case AnimationUtility.TangentMode.ClampedAuto:
            flag = true;
            break;
        }
      }
      if (keyIndex < curve.length - 1)
      {
        if (AnimationUtility.GetKeyBroken(curve[keyIndex + 1]))
          broken = true;
        switch (AnimationUtility.GetKeyLeftTangentMode(curve[keyIndex + 1]))
        {
          case AnimationUtility.TangentMode.Auto:
          case AnimationUtility.TangentMode.ClampedAuto:
            flag = true;
            break;
        }
      }
      AnimationUtility.SetKeyBroken(ref key, broken);
      if (broken && !flag)
      {
        if (keyIndex > 0)
          AnimationUtility.SetKeyLeftTangentMode(ref key, AnimationUtility.GetKeyRightTangentMode(curve[keyIndex - 1]));
        if (keyIndex < curve.length - 1)
          AnimationUtility.SetKeyRightTangentMode(ref key, AnimationUtility.GetKeyLeftTangentMode(curve[keyIndex + 1]));
        if (keyIndex == 0)
          AnimationUtility.SetKeyLeftTangentMode(ref key, AnimationUtility.GetKeyRightTangentMode(key));
        if (keyIndex == curve.length - 1)
          AnimationUtility.SetKeyRightTangentMode(ref key, AnimationUtility.GetKeyLeftTangentMode(key));
      }
      else
      {
        AnimationUtility.TangentMode tangentMode = AnimationUtility.TangentMode.Free;
        if ((keyIndex == 0 || AnimationUtility.GetKeyRightTangentMode(curve[keyIndex - 1]) == AnimationUtility.TangentMode.ClampedAuto) && (keyIndex == curve.length - 1 || AnimationUtility.GetKeyLeftTangentMode(curve[keyIndex + 1]) == AnimationUtility.TangentMode.ClampedAuto))
          tangentMode = AnimationUtility.TangentMode.ClampedAuto;
        else if ((keyIndex == 0 || AnimationUtility.GetKeyRightTangentMode(curve[keyIndex - 1]) == AnimationUtility.TangentMode.Auto) && (keyIndex == curve.length - 1 || AnimationUtility.GetKeyLeftTangentMode(curve[keyIndex + 1]) == AnimationUtility.TangentMode.Auto))
          tangentMode = AnimationUtility.TangentMode.Auto;
        AnimationUtility.SetKeyLeftTangentMode(ref key, tangentMode);
        AnimationUtility.SetKeyRightTangentMode(ref key, tangentMode);
      }
      curve.MoveKey(keyIndex, key);
    }

    public static string GetClipName(AnimationClip clip)
    {
      if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
        return "[No Clip]";
      string name = clip.name;
      if ((clip.hideFlags & HideFlags.NotEditable) != HideFlags.None)
        name += " (Read-Only)";
      return name;
    }

    public static Color GetBalancedColor(Color c)
    {
      return new Color((float) (0.150000005960464 + 0.75 * (double) c.r), (float) (0.200000002980232 + 0.600000023841858 * (double) c.g), (float) (0.100000001490116 + 0.899999976158142 * (double) c.b));
    }

    public static Color GetPropertyColor(string name)
    {
      Color color = Color.white;
      int num = 0;
      if (name.StartsWith("m_LocalPosition"))
        num = 1;
      if (name.StartsWith("localEulerAngles"))
        num = 2;
      if (name.StartsWith("m_LocalScale"))
        num = 3;
      switch (num)
      {
        case 1:
          if (name.EndsWith(".x"))
          {
            color = Handles.xAxisColor;
            break;
          }
          if (name.EndsWith(".y"))
          {
            color = Handles.yAxisColor;
            break;
          }
          if (name.EndsWith(".z"))
          {
            color = Handles.zAxisColor;
            break;
          }
          break;
        case 2:
          if (name.EndsWith(".x"))
          {
            color = (Color) AnimEditor.kEulerXColor;
            break;
          }
          if (name.EndsWith(".y"))
          {
            color = (Color) AnimEditor.kEulerYColor;
            break;
          }
          if (name.EndsWith(".z"))
          {
            color = (Color) AnimEditor.kEulerZColor;
            break;
          }
          break;
        case 3:
          if (name.EndsWith(".x"))
          {
            color = CurveUtility.GetBalancedColor(new Color(0.7f, 0.4f, 0.4f));
            break;
          }
          if (name.EndsWith(".y"))
          {
            color = CurveUtility.GetBalancedColor(new Color(0.4f, 0.7f, 0.4f));
            break;
          }
          if (name.EndsWith(".z"))
          {
            color = CurveUtility.GetBalancedColor(new Color(0.4f, 0.4f, 0.7f));
            break;
          }
          break;
        default:
          if (name.EndsWith(".x"))
          {
            color = Handles.xAxisColor;
            break;
          }
          if (name.EndsWith(".y"))
          {
            color = Handles.yAxisColor;
            break;
          }
          if (name.EndsWith(".z"))
          {
            color = Handles.zAxisColor;
            break;
          }
          if (name.EndsWith(".w"))
          {
            color = new Color(1f, 0.5f, 0.0f);
            break;
          }
          if (name.EndsWith(".r"))
          {
            color = CurveUtility.GetBalancedColor(Color.red);
            break;
          }
          if (name.EndsWith(".g"))
          {
            color = CurveUtility.GetBalancedColor(Color.green);
            break;
          }
          if (name.EndsWith(".b"))
          {
            color = CurveUtility.GetBalancedColor(Color.blue);
            break;
          }
          if (name.EndsWith(".a"))
          {
            color = CurveUtility.GetBalancedColor(Color.yellow);
            break;
          }
          if (name.EndsWith(".width"))
          {
            color = CurveUtility.GetBalancedColor(Color.blue);
            break;
          }
          if (name.EndsWith(".height"))
          {
            color = CurveUtility.GetBalancedColor(Color.yellow);
            break;
          }
          float f = 6.283185f * (float) (name.GetHashCode() % 1000);
          color = CurveUtility.GetBalancedColor(Color.HSVToRGB(f - Mathf.Floor(f), 1f, 1f));
          break;
      }
      color.a = 1f;
      return color;
    }
  }
}
