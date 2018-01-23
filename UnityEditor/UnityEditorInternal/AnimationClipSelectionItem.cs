// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationClipSelectionItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditorInternal
{
  internal class AnimationClipSelectionItem : AnimationWindowSelectionItem
  {
    public static AnimationClipSelectionItem Create(AnimationClip animationClip, Object sourceObject)
    {
      AnimationClipSelectionItem instance = ScriptableObject.CreateInstance(typeof (AnimationClipSelectionItem)) as AnimationClipSelectionItem;
      instance.hideFlags = HideFlags.HideAndDontSave;
      instance.gameObject = sourceObject as GameObject;
      instance.scriptableObject = sourceObject as ScriptableObject;
      instance.animationClip = animationClip;
      instance.timeOffset = 0.0f;
      instance.id = 0;
      return instance;
    }

    public override bool canPreview
    {
      get
      {
        return false;
      }
    }

    public override bool canRecord
    {
      get
      {
        return false;
      }
    }

    public override bool canChangeAnimationClip
    {
      get
      {
        return false;
      }
    }

    public override bool canSyncSceneSelection
    {
      get
      {
        return false;
      }
    }
  }
}
