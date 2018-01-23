// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.IAnimationContextualResponder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal interface IAnimationContextualResponder
  {
    bool IsAnimatable(PropertyModification[] modifications);

    bool IsEditable(Object targetObject);

    bool KeyExists(PropertyModification[] modifications);

    bool CandidateExists(PropertyModification[] modifications);

    bool CurveExists(PropertyModification[] modifications);

    bool HasAnyCandidates();

    bool HasAnyCurves();

    void AddKey(PropertyModification[] modifications);

    void RemoveKey(PropertyModification[] modifications);

    void RemoveCurve(PropertyModification[] modifications);

    void AddCandidateKeys();

    void AddAnimatedKeys();

    void GoToNextKeyframe(PropertyModification[] modifications);

    void GoToPreviousKeyframe(PropertyModification[] modifications);
  }
}
