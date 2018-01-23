// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AddCurvesPopupHierarchyDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AddCurvesPopupHierarchyDataSource : TreeViewDataSource
  {
    public AddCurvesPopupHierarchyDataSource(TreeViewController treeView)
      : base(treeView)
    {
      this.showRootItem = false;
      this.rootIsCollapsable = false;
    }

    public static bool showEntireHierarchy { get; set; }

    private void SetupRootNodeSettings()
    {
      this.showRootItem = false;
      this.SetExpanded(this.root, true);
    }

    public override void FetchData()
    {
      if (AddCurvesPopup.selection == null)
        return;
      AnimationWindowSelectionItem[] array = AddCurvesPopup.selection.ToArray();
      if (array.Length > 1)
        this.m_RootItem = (TreeViewItem) new AddCurvesPopupObjectNode((TreeViewItem) null, "", "");
      foreach (AnimationWindowSelectionItem selectionItem in array)
      {
        if (selectionItem.canAddCurves)
        {
          if ((UnityEngine.Object) selectionItem.rootGameObject != (UnityEngine.Object) null)
            this.AddGameObjectToHierarchy(selectionItem.rootGameObject, selectionItem, this.m_RootItem);
          else if ((UnityEngine.Object) selectionItem.scriptableObject != (UnityEngine.Object) null)
            this.AddScriptableObjectToHierarchy(selectionItem.scriptableObject, selectionItem, this.m_RootItem);
        }
      }
      this.SetupRootNodeSettings();
      this.m_NeedRefreshRows = true;
    }

    private TreeViewItem AddGameObjectToHierarchy(GameObject gameObject, AnimationWindowSelectionItem selectionItem, TreeViewItem parent)
    {
      string transformPath = AnimationUtility.CalculateTransformPath(gameObject.transform, selectionItem.rootGameObject.transform);
      TreeViewItem treeViewItem = (TreeViewItem) new AddCurvesPopupGameObjectNode(gameObject, parent, gameObject.name);
      List<TreeViewItem> treeViewItemList = new List<TreeViewItem>();
      if (this.m_RootItem == null)
        this.m_RootItem = treeViewItem;
      EditorCurveBinding[] animatableBindings = AnimationUtility.GetAnimatableBindings(gameObject, selectionItem.rootGameObject);
      List<EditorCurveBinding> editorCurveBindingList = new List<EditorCurveBinding>();
      for (int index = 0; index < animatableBindings.Length; ++index)
      {
        EditorCurveBinding binding = animatableBindings[index];
        editorCurveBindingList.Add(binding);
        if (binding.propertyName == "m_IsActive")
        {
          if (binding.path != "")
          {
            TreeViewItem node = this.CreateNode(selectionItem, editorCurveBindingList.ToArray(), treeViewItem);
            if (node != null)
              treeViewItemList.Add(node);
            editorCurveBindingList.Clear();
          }
          else
            editorCurveBindingList.Clear();
        }
        else
        {
          bool flag1 = index == animatableBindings.Length - 1;
          bool flag2 = false;
          if (!flag1)
            flag2 = animatableBindings[index + 1].type != binding.type;
          if (AnimationWindowUtility.IsCurveCreated(selectionItem.animationClip, binding))
            editorCurveBindingList.Remove(binding);
          if (binding.type == typeof (Animator) && binding.propertyName == "m_Enabled")
            editorCurveBindingList.Remove(binding);
          if ((flag1 || flag2) && editorCurveBindingList.Count > 0)
          {
            treeViewItemList.Add(this.AddAnimatableObjectToHierarchy(selectionItem, editorCurveBindingList.ToArray(), treeViewItem, transformPath));
            editorCurveBindingList.Clear();
          }
        }
      }
      if (AddCurvesPopupHierarchyDataSource.showEntireHierarchy)
      {
        for (int index = 0; index < gameObject.transform.childCount; ++index)
        {
          TreeViewItem hierarchy = this.AddGameObjectToHierarchy(gameObject.transform.GetChild(index).gameObject, selectionItem, treeViewItem);
          if (hierarchy != null)
            treeViewItemList.Add(hierarchy);
        }
      }
      TreeViewUtility.SetChildParentReferences((IList<TreeViewItem>) treeViewItemList, treeViewItem);
      return treeViewItem;
    }

    private TreeViewItem AddScriptableObjectToHierarchy(ScriptableObject scriptableObject, AnimationWindowSelectionItem selectionItem, TreeViewItem parent)
    {
      EditorCurveBinding[] array = ((IEnumerable<EditorCurveBinding>) AnimationUtility.GetScriptableObjectAnimatableBindings(scriptableObject)).Where<EditorCurveBinding>((Func<EditorCurveBinding, bool>) (c => !AnimationWindowUtility.IsCurveCreated(selectionItem.animationClip, c))).ToArray<EditorCurveBinding>();
      TreeViewItem treeViewItem = array.Length <= 0 ? (TreeViewItem) new AddCurvesPopupObjectNode(parent, "", scriptableObject.name) : this.AddAnimatableObjectToHierarchy(selectionItem, array, parent, "");
      if (this.m_RootItem == null)
        this.m_RootItem = treeViewItem;
      return treeViewItem;
    }

    private static string GetClassName(AnimationWindowSelectionItem selectionItem, EditorCurveBinding binding)
    {
      if ((UnityEngine.Object) selectionItem.rootGameObject != (UnityEngine.Object) null)
      {
        UnityEngine.Object animatedObject = AnimationUtility.GetAnimatedObject(selectionItem.rootGameObject, binding);
        if ((bool) animatedObject)
          return ObjectNames.GetInspectorTitle(animatedObject);
      }
      return binding.type.Name;
    }

    private static Texture2D GetIcon(AnimationWindowSelectionItem selectionItem, EditorCurveBinding binding)
    {
      if ((UnityEngine.Object) selectionItem.rootGameObject != (UnityEngine.Object) null)
        return AssetPreview.GetMiniThumbnail(AnimationUtility.GetAnimatedObject(selectionItem.rootGameObject, binding));
      if ((UnityEngine.Object) selectionItem.scriptableObject != (UnityEngine.Object) null)
        return AssetPreview.GetMiniThumbnail((UnityEngine.Object) selectionItem.scriptableObject);
      return (Texture2D) null;
    }

    private TreeViewItem AddAnimatableObjectToHierarchy(AnimationWindowSelectionItem selectionItem, EditorCurveBinding[] curveBindings, TreeViewItem parentNode, string path)
    {
      TreeViewItem treeViewItem = (TreeViewItem) new AddCurvesPopupObjectNode(parentNode, path, AddCurvesPopupHierarchyDataSource.GetClassName(selectionItem, curveBindings[0]));
      treeViewItem.icon = AddCurvesPopupHierarchyDataSource.GetIcon(selectionItem, curveBindings[0]);
      List<TreeViewItem> treeViewItemList = new List<TreeViewItem>();
      List<EditorCurveBinding> editorCurveBindingList = new List<EditorCurveBinding>();
      for (int index = 0; index < curveBindings.Length; ++index)
      {
        EditorCurveBinding curveBinding = curveBindings[index];
        editorCurveBindingList.Add(curveBinding);
        if (index == curveBindings.Length - 1 || AnimationWindowUtility.GetPropertyGroupName(curveBindings[index + 1].propertyName) != AnimationWindowUtility.GetPropertyGroupName(curveBinding.propertyName))
        {
          TreeViewItem node = this.CreateNode(selectionItem, editorCurveBindingList.ToArray(), treeViewItem);
          if (node != null)
            treeViewItemList.Add(node);
          editorCurveBindingList.Clear();
        }
      }
      treeViewItemList.Sort();
      TreeViewUtility.SetChildParentReferences((IList<TreeViewItem>) treeViewItemList, treeViewItem);
      return treeViewItem;
    }

    private TreeViewItem CreateNode(AnimationWindowSelectionItem selectionItem, EditorCurveBinding[] curveBindings, TreeViewItem parentNode)
    {
      AddCurvesPopupPropertyNode popupPropertyNode = new AddCurvesPopupPropertyNode(parentNode, selectionItem, curveBindings);
      if (AnimationWindowUtility.IsRectTransformPosition(popupPropertyNode.curveBindings[0]))
        popupPropertyNode.curveBindings = new EditorCurveBinding[1]
        {
          popupPropertyNode.curveBindings[2]
        };
      popupPropertyNode.icon = parentNode.icon;
      return (TreeViewItem) popupPropertyNode;
    }

    public void UpdateData()
    {
      this.m_TreeView.ReloadData();
    }
  }
}
