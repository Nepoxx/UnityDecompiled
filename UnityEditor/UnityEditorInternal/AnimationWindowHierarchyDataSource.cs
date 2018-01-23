// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowHierarchyDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AnimationWindowHierarchyDataSource : TreeViewDataSource
  {
    public AnimationWindowHierarchyDataSource(TreeViewController treeView, AnimationWindowState animationWindowState)
      : base(treeView)
    {
      this.state = animationWindowState;
    }

    private AnimationWindowState state { get; set; }

    public bool showAll { get; set; }

    private void SetupRootNodeSettings()
    {
      this.showRootItem = false;
      this.rootIsCollapsable = false;
      this.SetExpanded(this.m_RootItem, true);
    }

    private AnimationWindowHierarchyNode GetEmptyRootNode()
    {
      return new AnimationWindowHierarchyNode(0, -1, (TreeViewItem) null, (System.Type) null, "", "", "root");
    }

    public override void FetchData()
    {
      this.m_RootItem = (TreeViewItem) this.GetEmptyRootNode();
      this.SetupRootNodeSettings();
      this.m_NeedRefreshRows = true;
      if (this.state.selection.disabled)
      {
        this.root.children = (List<TreeViewItem>) null;
      }
      else
      {
        List<AnimationWindowHierarchyNode> windowHierarchyNodeList = new List<AnimationWindowHierarchyNode>();
        if (this.state.allCurves.Count > 0)
        {
          AnimationWindowHierarchyMasterNode hierarchyMasterNode = new AnimationWindowHierarchyMasterNode();
          hierarchyMasterNode.curves = this.state.allCurves.ToArray();
          windowHierarchyNodeList.Add((AnimationWindowHierarchyNode) hierarchyMasterNode);
        }
        windowHierarchyNodeList.AddRange((IEnumerable<AnimationWindowHierarchyNode>) this.CreateTreeFromCurves());
        windowHierarchyNodeList.Add((AnimationWindowHierarchyNode) new AnimationWindowHierarchyAddButtonNode());
        TreeViewUtility.SetChildParentReferences((IList<TreeViewItem>) new List<TreeViewItem>((IEnumerable<TreeViewItem>) windowHierarchyNodeList.ToArray()), this.root);
      }
    }

    public override bool IsRenamingItemAllowed(TreeViewItem item)
    {
      return !(item is AnimationWindowHierarchyAddButtonNode) && !(item is AnimationWindowHierarchyMasterNode) && !(item is AnimationWindowHierarchyClipNode) && (item as AnimationWindowHierarchyNode).path.Length != 0;
    }

    public List<AnimationWindowHierarchyNode> CreateTreeFromCurves()
    {
      List<AnimationWindowHierarchyNode> windowHierarchyNodeList = new List<AnimationWindowHierarchyNode>();
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      foreach (AnimationWindowSelectionItem selectedItem in this.state.selection.ToArray())
      {
        AnimationWindowCurve[] array = selectedItem.curves.ToArray();
        AnimationWindowHierarchyNode parentNode = (AnimationWindowHierarchyNode) this.m_RootItem;
        if (this.state.selection.count > 1)
        {
          AnimationWindowHierarchyNode hierarchy = (AnimationWindowHierarchyNode) this.AddClipNodeToHierarchy(selectedItem, array, parentNode);
          windowHierarchyNodeList.Add(hierarchy);
          parentNode = hierarchy;
        }
        for (int index = 0; index < array.Length; ++index)
        {
          AnimationWindowCurve animationWindowCurve1 = array[index];
          AnimationWindowCurve animationWindowCurve2 = index >= array.Length - 1 ? (AnimationWindowCurve) null : array[index + 1];
          animationWindowCurveList.Add(animationWindowCurve1);
          bool flag1 = animationWindowCurve2 != null && AnimationWindowUtility.GetPropertyGroupName(animationWindowCurve2.propertyName) == AnimationWindowUtility.GetPropertyGroupName(animationWindowCurve1.propertyName);
          bool flag2 = animationWindowCurve2 != null && animationWindowCurve1.path.Equals(animationWindowCurve2.path) && animationWindowCurve1.type == animationWindowCurve2.type;
          if (index == array.Length - 1 || !flag1 || !flag2)
          {
            if (animationWindowCurveList.Count > 1)
              windowHierarchyNodeList.Add((AnimationWindowHierarchyNode) this.AddPropertyGroupToHierarchy(selectedItem, animationWindowCurveList.ToArray(), parentNode));
            else
              windowHierarchyNodeList.Add((AnimationWindowHierarchyNode) this.AddPropertyToHierarchy(selectedItem, animationWindowCurveList[0], parentNode));
            animationWindowCurveList.Clear();
          }
        }
      }
      return windowHierarchyNodeList;
    }

    private AnimationWindowHierarchyClipNode AddClipNodeToHierarchy(AnimationWindowSelectionItem selectedItem, AnimationWindowCurve[] curves, AnimationWindowHierarchyNode parentNode)
    {
      AnimationWindowHierarchyClipNode hierarchyClipNode = new AnimationWindowHierarchyClipNode((TreeViewItem) parentNode, selectedItem.id, selectedItem.animationClip.name);
      hierarchyClipNode.curves = curves;
      return hierarchyClipNode;
    }

    private AnimationWindowHierarchyPropertyGroupNode AddPropertyGroupToHierarchy(AnimationWindowSelectionItem selectedItem, AnimationWindowCurve[] curves, AnimationWindowHierarchyNode parentNode)
    {
      List<AnimationWindowHierarchyNode> windowHierarchyNodeList = new List<AnimationWindowHierarchyNode>();
      AnimationWindowHierarchyPropertyGroupNode propertyGroupNode = new AnimationWindowHierarchyPropertyGroupNode(curves[0].type, selectedItem.id, AnimationWindowUtility.GetPropertyGroupName(curves[0].propertyName), curves[0].path, (TreeViewItem) parentNode);
      propertyGroupNode.icon = this.GetIcon(selectedItem, curves[0].binding);
      propertyGroupNode.indent = curves[0].depth;
      propertyGroupNode.curves = curves;
      foreach (AnimationWindowCurve curve in curves)
      {
        AnimationWindowHierarchyPropertyNode hierarchy = this.AddPropertyToHierarchy(selectedItem, curve, (AnimationWindowHierarchyNode) propertyGroupNode);
        hierarchy.displayName = AnimationWindowUtility.GetPropertyDisplayName(hierarchy.propertyName);
        windowHierarchyNodeList.Add((AnimationWindowHierarchyNode) hierarchy);
      }
      TreeViewUtility.SetChildParentReferences((IList<TreeViewItem>) new List<TreeViewItem>((IEnumerable<TreeViewItem>) windowHierarchyNodeList.ToArray()), (TreeViewItem) propertyGroupNode);
      return propertyGroupNode;
    }

    private AnimationWindowHierarchyPropertyNode AddPropertyToHierarchy(AnimationWindowSelectionItem selectedItem, AnimationWindowCurve curve, AnimationWindowHierarchyNode parentNode)
    {
      AnimationWindowHierarchyPropertyNode hierarchyPropertyNode = new AnimationWindowHierarchyPropertyNode(curve.type, selectedItem.id, curve.propertyName, curve.path, (TreeViewItem) parentNode, curve.binding, curve.isPPtrCurve);
      if ((UnityEngine.Object) parentNode.icon != (UnityEngine.Object) null)
        hierarchyPropertyNode.icon = parentNode.icon;
      else
        hierarchyPropertyNode.icon = this.GetIcon(selectedItem, curve.binding);
      hierarchyPropertyNode.indent = curve.depth;
      hierarchyPropertyNode.curves = new AnimationWindowCurve[1]
      {
        curve
      };
      return hierarchyPropertyNode;
    }

    public Texture2D GetIcon(AnimationWindowSelectionItem selectedItem, EditorCurveBinding curveBinding)
    {
      if ((UnityEngine.Object) selectedItem.rootGameObject != (UnityEngine.Object) null)
      {
        UnityEngine.Object animatedObject = AnimationUtility.GetAnimatedObject(selectedItem.rootGameObject, curveBinding);
        if (animatedObject != (UnityEngine.Object) null)
          return AssetPreview.GetMiniThumbnail(animatedObject);
      }
      return AssetPreview.GetMiniTypeThumbnail(curveBinding.type);
    }

    public void UpdateData()
    {
      this.m_TreeView.ReloadData();
    }
  }
}
