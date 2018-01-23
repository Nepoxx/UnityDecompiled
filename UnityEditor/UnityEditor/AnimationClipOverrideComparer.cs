// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationClipOverrideComparer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class AnimationClipOverrideComparer : IComparer<KeyValuePair<AnimationClip, AnimationClip>>
  {
    public int Compare(KeyValuePair<AnimationClip, AnimationClip> x, KeyValuePair<AnimationClip, AnimationClip> y)
    {
      return string.Compare(x.Key.name, y.Key.name, StringComparison.OrdinalIgnoreCase);
    }
  }
}
