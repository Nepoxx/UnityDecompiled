// Decompiled with JetBrains decompiler
// Type: UnityEditor.IntCurveRenderer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class IntCurveRenderer : NormalCurveRenderer
  {
    private const float kSegmentWindowResolution = 1000f;
    private const int kMaximumSampleCount = 1000;
    private const float kStepHelperOffset = 1E-06f;

    public IntCurveRenderer(AnimationCurve curve)
      : base(curve)
    {
    }

    public override float ClampedValue(float value)
    {
      return Mathf.Floor(value + 0.5f);
    }

    public override float EvaluateCurveSlow(float time)
    {
      return this.ClampedValue(this.GetCurve().Evaluate(time));
    }

    protected override int GetSegmentResolution(float minTime, float maxTime, float keyTime, float nextKeyTime)
    {
      float num = maxTime - minTime;
      return Mathf.Clamp(Mathf.RoundToInt((float) (1000.0 * ((double) (nextKeyTime - keyTime) / (double) num))), 1, 1000);
    }

    protected override void AddPoint(ref List<Vector3> points, ref float lastTime, float sampleTime, ref float lastValue, float sampleValue)
    {
      if ((double) lastValue != (double) sampleValue)
        points.Add(new Vector3(lastTime + 1E-06f, sampleValue));
      points.Add(new Vector3(sampleTime, sampleValue));
      lastTime = sampleTime;
      lastValue = sampleValue;
    }
  }
}
