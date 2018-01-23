// Decompiled with JetBrains decompiler
// Type: UnityEditor.ChangedCurve
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ChangedCurve
  {
    public AnimationCurve curve;
    public int curveId;
    public EditorCurveBinding binding;

    public ChangedCurve(AnimationCurve curve, int curveId, EditorCurveBinding binding)
    {
      this.curve = curve;
      this.curveId = curveId;
      this.binding = binding;
    }

    public override int GetHashCode()
    {
      return 33 * this.curve.GetHashCode() + this.binding.GetHashCode();
    }
  }
}
