// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParticleSystemClipboard
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ParticleSystemClipboard
  {
    private static AnimationCurve m_AnimationCurve1;
    private static AnimationCurve m_AnimationCurve2;
    private static float m_AnimationCurveScalar;
    private static Gradient m_Gradient1;
    private static Gradient m_Gradient2;

    public static bool HasSingleGradient()
    {
      return ParticleSystemClipboard.m_Gradient1 != null && ParticleSystemClipboard.m_Gradient2 == null;
    }

    public static bool HasDoubleGradient()
    {
      return ParticleSystemClipboard.m_Gradient1 != null && ParticleSystemClipboard.m_Gradient2 != null;
    }

    public static void CopyGradient(Gradient gradient1, Gradient gradient2)
    {
      ParticleSystemClipboard.m_Gradient1 = gradient1;
      ParticleSystemClipboard.m_Gradient2 = gradient2;
    }

    public static void PasteGradient(SerializedProperty gradientProperty, SerializedProperty gradientProperty2)
    {
      if (gradientProperty != null && ParticleSystemClipboard.m_Gradient1 != null)
        gradientProperty.gradientValue = ParticleSystemClipboard.m_Gradient1;
      if (gradientProperty2 == null || ParticleSystemClipboard.m_Gradient2 == null)
        return;
      gradientProperty2.gradientValue = ParticleSystemClipboard.m_Gradient2;
    }

    public static bool HasSingleAnimationCurve()
    {
      return ParticleSystemClipboard.m_AnimationCurve1 != null && ParticleSystemClipboard.m_AnimationCurve2 == null;
    }

    public static bool HasDoubleAnimationCurve()
    {
      return ParticleSystemClipboard.m_AnimationCurve1 != null && ParticleSystemClipboard.m_AnimationCurve2 != null;
    }

    public static void CopyAnimationCurves(AnimationCurve animCurve, AnimationCurve animCurve2, float scalar)
    {
      ParticleSystemClipboard.m_AnimationCurve1 = animCurve;
      ParticleSystemClipboard.m_AnimationCurve2 = animCurve2;
      ParticleSystemClipboard.m_AnimationCurveScalar = scalar;
    }

    private static void ClampCurve(SerializedProperty animCurveProperty, Rect curveRanges)
    {
      AnimationCurve animationCurveValue = animCurveProperty.animationCurveValue;
      Keyframe[] keys = animationCurveValue.keys;
      for (int index = 0; index < keys.Length; ++index)
      {
        keys[index].time = Mathf.Clamp(keys[index].time, curveRanges.xMin, curveRanges.xMax);
        keys[index].value = Mathf.Clamp(keys[index].value, curveRanges.yMin, curveRanges.yMax);
      }
      animationCurveValue.keys = keys;
      animCurveProperty.animationCurveValue = animationCurveValue;
    }

    public static void PasteAnimationCurves(SerializedProperty animCurveProperty, SerializedProperty animCurveProperty2, SerializedProperty scalarProperty, Rect curveRanges, ParticleSystemCurveEditor particleSystemCurveEditor)
    {
      if (animCurveProperty != null && ParticleSystemClipboard.m_AnimationCurve1 != null)
      {
        animCurveProperty.animationCurveValue = ParticleSystemClipboard.m_AnimationCurve1;
        ParticleSystemClipboard.ClampCurve(animCurveProperty, curveRanges);
      }
      if (animCurveProperty2 != null && ParticleSystemClipboard.m_AnimationCurve2 != null)
      {
        animCurveProperty2.animationCurveValue = ParticleSystemClipboard.m_AnimationCurve2;
        ParticleSystemClipboard.ClampCurve(animCurveProperty2, curveRanges);
      }
      if (scalarProperty != null)
        scalarProperty.floatValue = ParticleSystemClipboard.m_AnimationCurveScalar;
      if (particleSystemCurveEditor == null)
        return;
      particleSystemCurveEditor.Refresh();
    }
  }
}
