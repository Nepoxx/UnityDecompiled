// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowHierarchyPropertyNode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace UnityEditorInternal
{
  internal class AnimationWindowHierarchyPropertyNode : AnimationWindowHierarchyNode
  {
    public bool isPptrNode;

    public AnimationWindowHierarchyPropertyNode(System.Type animatableObjectType, int setId, string propertyName, string path, TreeViewItem parent, EditorCurveBinding binding, bool isPptrNode)
      : base(AnimationWindowUtility.GetPropertyNodeID(setId, path, animatableObjectType, propertyName), parent == null ? -1 : parent.depth + 1, parent, animatableObjectType, propertyName, path, AnimationWindowUtility.GetNicePropertyDisplayName(animatableObjectType, propertyName))
    {
      this.binding = new EditorCurveBinding?(binding);
      this.isPptrNode = isPptrNode;
    }
  }
}
