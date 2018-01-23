// Decompiled with JetBrains decompiler
// Type: UnityEditor.KeyIdentifier
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class KeyIdentifier
  {
    public AnimationCurve curve;
    public int curveId;
    public int key;
    public EditorCurveBinding binding;

    public KeyIdentifier(AnimationCurve _curve, int _curveId, int _keyIndex)
    {
      this.curve = _curve;
      this.curveId = _curveId;
      this.key = _keyIndex;
    }

    public KeyIdentifier(AnimationCurve _curve, int _curveId, int _keyIndex, EditorCurveBinding _binding)
    {
      this.curve = _curve;
      this.curveId = _curveId;
      this.key = _keyIndex;
      this.binding = _binding;
    }

    public Keyframe keyframe
    {
      get
      {
        return this.curve[this.key];
      }
    }
  }
}
