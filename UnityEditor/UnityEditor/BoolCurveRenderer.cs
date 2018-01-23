// Decompiled with JetBrains decompiler
// Type: UnityEditor.BoolCurveRenderer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class BoolCurveRenderer : NormalCurveRenderer
  {
    public BoolCurveRenderer(AnimationCurve curve)
      : base(curve)
    {
    }

    public override float ClampedValue(float value)
    {
      return (double) value == 0.0 ? 0.0f : 1f;
    }

    public override float EvaluateCurveSlow(float time)
    {
      return this.ClampedValue(this.GetCurve().Evaluate(time));
    }
  }
}
