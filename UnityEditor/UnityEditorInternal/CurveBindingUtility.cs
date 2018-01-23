// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.CurveBindingUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal static class CurveBindingUtility
  {
    public static object GetCurrentValue(AnimationWindowState state, AnimationWindowCurve curve)
    {
      if (state.previewing && (Object) curve.rootGameObject != (Object) null)
        return AnimationWindowUtility.GetCurrentValue(curve.rootGameObject, curve.binding);
      return curve.Evaluate(state.currentTime - curve.timeOffset);
    }

    public static object GetCurrentValue(GameObject rootGameObject, EditorCurveBinding curveBinding)
    {
      if ((Object) rootGameObject != (Object) null)
        return AnimationWindowUtility.GetCurrentValue(rootGameObject, curveBinding);
      if (curveBinding.isPPtrCurve)
        return (object) null;
      return (object) 0.0f;
    }
  }
}
