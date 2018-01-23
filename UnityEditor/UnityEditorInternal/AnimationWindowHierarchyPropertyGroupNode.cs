// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowHierarchyPropertyGroupNode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;

namespace UnityEditorInternal
{
  internal class AnimationWindowHierarchyPropertyGroupNode : AnimationWindowHierarchyNode
  {
    public AnimationWindowHierarchyPropertyGroupNode(System.Type animatableObjectType, int setId, string propertyName, string path, TreeViewItem parent)
      : base(AnimationWindowUtility.GetPropertyNodeID(setId, path, animatableObjectType, propertyName), parent == null ? -1 : parent.depth + 1, parent, animatableObjectType, AnimationWindowUtility.GetPropertyGroupName(propertyName), path, AnimationWindowUtility.GetNicePropertyGroupDisplayName(animatableObjectType, propertyName))
    {
    }
  }
}
