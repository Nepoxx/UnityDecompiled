// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.IAnimationRecordingState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal interface IAnimationRecordingState
  {
    GameObject activeGameObject { get; }

    GameObject activeRootGameObject { get; }

    AnimationClip activeAnimationClip { get; }

    int currentFrame { get; }

    bool addZeroFrame { get; }

    bool DiscardModification(PropertyModification modification);

    void SaveCurve(AnimationWindowCurve curve);

    void AddPropertyModification(EditorCurveBinding binding, PropertyModification propertyModification, bool keepPrefabOverride);
  }
}
