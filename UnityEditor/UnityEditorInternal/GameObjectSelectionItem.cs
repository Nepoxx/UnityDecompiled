// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.GameObjectSelectionItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class GameObjectSelectionItem : AnimationWindowSelectionItem
  {
    public static GameObjectSelectionItem Create(GameObject gameObject)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GameObjectSelectionItem.\u003CCreate\u003Ec__AnonStorey0 createCAnonStorey0 = new GameObjectSelectionItem.\u003CCreate\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      createCAnonStorey0.selectionItem = ScriptableObject.CreateInstance(typeof (GameObjectSelectionItem)) as GameObjectSelectionItem;
      // ISSUE: reference to a compiler-generated field
      createCAnonStorey0.selectionItem.hideFlags = HideFlags.HideAndDontSave;
      // ISSUE: reference to a compiler-generated field
      createCAnonStorey0.selectionItem.gameObject = gameObject;
      // ISSUE: reference to a compiler-generated field
      createCAnonStorey0.selectionItem.animationClip = (AnimationClip) null;
      // ISSUE: reference to a compiler-generated field
      createCAnonStorey0.selectionItem.timeOffset = 0.0f;
      // ISSUE: reference to a compiler-generated field
      createCAnonStorey0.selectionItem.id = 0;
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) createCAnonStorey0.selectionItem.rootGameObject != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        AnimationClip[] animationClips = AnimationUtility.GetAnimationClips(createCAnonStorey0.selectionItem.rootGameObject);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) createCAnonStorey0.selectionItem.animationClip == (UnityEngine.Object) null && (UnityEngine.Object) createCAnonStorey0.selectionItem.gameObject != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          createCAnonStorey0.selectionItem.animationClip = animationClips.Length <= 0 ? (AnimationClip) null : animationClips[0];
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          if (!Array.Exists<AnimationClip>(animationClips, new Predicate<AnimationClip>(createCAnonStorey0.\u003C\u003Em__0)))
          {
            // ISSUE: reference to a compiler-generated field
            createCAnonStorey0.selectionItem.animationClip = animationClips.Length <= 0 ? (AnimationClip) null : animationClips[0];
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      return createCAnonStorey0.selectionItem;
    }

    public override AnimationClip animationClip
    {
      set
      {
        base.animationClip = value;
      }
      get
      {
        if ((UnityEngine.Object) this.animationPlayer == (UnityEngine.Object) null)
          return (AnimationClip) null;
        return base.animationClip;
      }
    }

    public override void Synchronize()
    {
      if (!((UnityEngine.Object) this.rootGameObject != (UnityEngine.Object) null))
        return;
      AnimationClip[] animationClips = AnimationUtility.GetAnimationClips(this.rootGameObject);
      if (animationClips.Length > 0)
      {
        if (!Array.Exists<AnimationClip>(animationClips, (Predicate<AnimationClip>) (x => (UnityEngine.Object) x == (UnityEngine.Object) this.animationClip)))
          this.animationClip = animationClips[0];
      }
      else
        this.animationClip = (AnimationClip) null;
    }
  }
}
