// Decompiled with JetBrains decompiler
// Type: UnityEditor.ISelectionBinding
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal interface ISelectionBinding
  {
    GameObject rootGameObject { get; }

    AnimationClip animationClip { get; }

    bool clipIsEditable { get; }

    bool animationIsEditable { get; }

    float timeOffset { get; }

    int id { get; }
  }
}
