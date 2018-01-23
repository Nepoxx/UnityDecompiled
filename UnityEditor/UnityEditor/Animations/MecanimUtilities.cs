// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.MecanimUtilities
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Animations
{
  internal class MecanimUtilities
  {
    public static bool StateMachineRelativePath(AnimatorStateMachine parent, AnimatorStateMachine toFind, ref List<AnimatorStateMachine> hierarchy)
    {
      hierarchy.Add(parent);
      if ((Object) parent == (Object) toFind)
        return true;
      foreach (ChildAnimatorStateMachine childStateMachine in AnimatorStateMachine.StateMachineCache.GetChildStateMachines(parent))
      {
        if (MecanimUtilities.StateMachineRelativePath(childStateMachine.stateMachine, toFind, ref hierarchy))
          return true;
      }
      hierarchy.Remove(parent);
      return false;
    }

    internal static bool AreSameAsset(Object obj1, Object obj2)
    {
      return AssetDatabase.GetAssetPath(obj1) == AssetDatabase.GetAssetPath(obj2);
    }

    internal static void DestroyBlendTreeRecursive(BlendTree blendTree)
    {
      for (int index = 0; index < blendTree.children.Length; ++index)
      {
        BlendTree motion = blendTree.children[index].motion as BlendTree;
        if ((Object) motion != (Object) null && MecanimUtilities.AreSameAsset((Object) blendTree, (Object) motion))
          MecanimUtilities.DestroyBlendTreeRecursive(motion);
      }
      Undo.DestroyObjectImmediate((Object) blendTree);
    }
  }
}
