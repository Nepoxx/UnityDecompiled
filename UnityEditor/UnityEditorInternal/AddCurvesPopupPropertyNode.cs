// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AddCurvesPopupPropertyNode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace UnityEditorInternal
{
  internal class AddCurvesPopupPropertyNode : TreeViewItem
  {
    public AnimationWindowSelectionItem selectionItem;
    public EditorCurveBinding[] curveBindings;

    public AddCurvesPopupPropertyNode(TreeViewItem parent, AnimationWindowSelectionItem selectionItem, EditorCurveBinding[] curveBindings)
      : base(curveBindings[0].GetHashCode(), parent.depth + 1, parent, AnimationWindowUtility.NicifyPropertyGroupName(curveBindings[0].type, AnimationWindowUtility.GetPropertyGroupName(curveBindings[0].propertyName)))
    {
      this.selectionItem = selectionItem;
      this.curveBindings = curveBindings;
    }

    public override int CompareTo(TreeViewItem other)
    {
      AddCurvesPopupPropertyNode popupPropertyNode = other as AddCurvesPopupPropertyNode;
      if (popupPropertyNode != null)
      {
        if (this.displayName.Contains("Rotation") && popupPropertyNode.displayName.Contains("Position"))
          return 1;
        if (this.displayName.Contains("Position") && popupPropertyNode.displayName.Contains("Rotation"))
          return -1;
      }
      return base.CompareTo(other);
    }
  }
}
