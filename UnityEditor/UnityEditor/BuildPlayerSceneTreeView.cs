// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildPlayerSceneTreeView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  internal class BuildPlayerSceneTreeView : TreeView
  {
    public BuildPlayerSceneTreeView(TreeViewState state)
      : base(state)
    {
      this.showBorder = true;
      EditorBuildSettings.sceneListChanged += new Action(this.HandleExternalSceneListChange);
    }

    internal void UnsubscribeListChange()
    {
      EditorBuildSettings.sceneListChanged -= new Action(this.HandleExternalSceneListChange);
    }

    private void HandleExternalSceneListChange()
    {
      this.Reload();
    }

    protected override TreeViewItem BuildRoot()
    {
      TreeViewItem treeViewItem = new TreeViewItem(-1, -1);
      treeViewItem.children = new List<TreeViewItem>();
      foreach (EditorBuildSettingsScene buildSettingsScene in new List<EditorBuildSettingsScene>((IEnumerable<EditorBuildSettingsScene>) EditorBuildSettings.scenes))
      {
        BuildPlayerSceneTreeViewItem sceneTreeViewItem = new BuildPlayerSceneTreeViewItem(buildSettingsScene.guid.GetHashCode(), 0, buildSettingsScene.path, buildSettingsScene.enabled);
        treeViewItem.AddChild((TreeViewItem) sceneTreeViewItem);
      }
      return treeViewItem;
    }

    protected override bool CanBeParent(TreeViewItem item)
    {
      return false;
    }

    protected override void BeforeRowsGUI()
    {
      int num = 0;
      foreach (TreeViewItem child in this.rootItem.children)
      {
        BuildPlayerSceneTreeViewItem sceneTreeViewItem = child as BuildPlayerSceneTreeViewItem;
        if (sceneTreeViewItem != null)
          sceneTreeViewItem.UpdateName();
        if (sceneTreeViewItem.active)
        {
          sceneTreeViewItem.counter = num;
          ++num;
        }
        else
          sceneTreeViewItem.counter = BuildPlayerSceneTreeViewItem.kInvalidCounter;
      }
      base.BeforeRowsGUI();
    }

    protected override void RowGUI(TreeView.RowGUIArgs args)
    {
      BuildPlayerSceneTreeViewItem sceneTreeViewItem = args.item as BuildPlayerSceneTreeViewItem;
      if (sceneTreeViewItem != null)
      {
        bool flag1 = !sceneTreeViewItem.guid.Empty() && File.Exists(sceneTreeViewItem.fullName);
        using (new EditorGUI.DisabledScope(!flag1))
        {
          bool flag2 = sceneTreeViewItem.active;
          if (!flag1)
            flag2 = false;
          bool flag3 = GUI.Toggle(new Rect(args.rowRect.x, args.rowRect.y, 16f, 16f), flag2, "");
          if (flag3 != sceneTreeViewItem.active)
          {
            if (this.GetSelection().Contains(sceneTreeViewItem.id))
            {
              foreach (int id in (IEnumerable<int>) this.GetSelection())
                (this.FindItem(id, this.rootItem) as BuildPlayerSceneTreeViewItem).active = flag3;
            }
            else
              sceneTreeViewItem.active = flag3;
            EditorBuildSettings.scenes = this.GetSceneList();
          }
          base.RowGUI(args);
          if (sceneTreeViewItem.counter != BuildPlayerSceneTreeViewItem.kInvalidCounter)
          {
            TreeView.DefaultGUI.LabelRightAligned(args.rowRect, "" + (object) sceneTreeViewItem.counter, args.selected, args.focused);
          }
          else
          {
            if (!(sceneTreeViewItem.displayName == string.Empty) && flag1)
              return;
            TreeView.DefaultGUI.LabelRightAligned(args.rowRect, "Deleted", args.selected, args.focused);
          }
        }
      }
      else
        base.RowGUI(args);
    }

    protected override DragAndDropVisualMode HandleDragAndDrop(TreeView.DragAndDropArgs args)
    {
      DragAndDropVisualMode andDropVisualMode = DragAndDropVisualMode.None;
      List<int> genericData = DragAndDrop.GetGenericData("BuildPlayerSceneTreeViewItem") as List<int>;
      if (genericData != null && genericData.Count > 0)
      {
        andDropVisualMode = DragAndDropVisualMode.Move;
        if (args.performDrop)
        {
          int dropAtIndex = this.FindDropAtIndex(args);
          List<TreeViewItem> treeViewItemList = new List<TreeViewItem>();
          int num = 0;
          foreach (TreeViewItem child in this.rootItem.children)
          {
            if (num == dropAtIndex)
            {
              foreach (int id in genericData)
                treeViewItemList.Add(this.FindItem(id, this.rootItem));
            }
            ++num;
            if (!genericData.Contains(child.id))
              treeViewItemList.Add(child);
          }
          if (treeViewItemList.Count < this.rootItem.children.Count)
          {
            foreach (int id in genericData)
              treeViewItemList.Add(this.FindItem(id, this.rootItem));
          }
          this.rootItem.children = treeViewItemList;
          EditorBuildSettings.scenes = this.GetSceneList();
          this.ReloadAndSelect((IList<int>) genericData);
          this.Repaint();
        }
      }
      else if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
      {
        andDropVisualMode = DragAndDropVisualMode.Copy;
        if (args.performDrop)
        {
          List<EditorBuildSettingsScene> buildSettingsSceneList1 = new List<EditorBuildSettingsScene>((IEnumerable<EditorBuildSettingsScene>) EditorBuildSettings.scenes);
          List<EditorBuildSettingsScene> buildSettingsSceneList2 = new List<EditorBuildSettingsScene>();
          List<int> intList = new List<int>();
          foreach (string path in DragAndDrop.paths)
          {
            if (AssetDatabase.GetMainAssetTypeAtPath(path) == typeof (SceneAsset))
            {
              GUID guid = new GUID(AssetDatabase.AssetPathToGUID(path));
              intList.Add(guid.GetHashCode());
              bool flag = true;
              foreach (EditorBuildSettingsScene buildSettingsScene in buildSettingsSceneList1)
              {
                if (buildSettingsScene.path == path)
                {
                  flag = false;
                  break;
                }
              }
              if (flag)
                buildSettingsSceneList2.Add(new EditorBuildSettingsScene(path, true));
            }
          }
          int dropAtIndex = this.FindDropAtIndex(args);
          buildSettingsSceneList1.InsertRange(dropAtIndex, (IEnumerable<EditorBuildSettingsScene>) buildSettingsSceneList2);
          EditorBuildSettings.scenes = buildSettingsSceneList1.ToArray();
          this.ReloadAndSelect((IList<int>) intList);
          this.Repaint();
        }
      }
      return andDropVisualMode;
    }

    private void ReloadAndSelect(IList<int> hashCodes)
    {
      this.Reload();
      this.SetSelection(hashCodes, TreeViewSelectionOptions.RevealAndFrame);
      this.SelectionChanged(hashCodes);
    }

    protected override void DoubleClickedItem(int id)
    {
      EditorGUIUtility.PingObject(AssetDatabase.GetInstanceIDFromGUID(AssetDatabase.AssetPathToGUID((this.FindItem(id, this.rootItem) as BuildPlayerSceneTreeViewItem).fullName)));
    }

    protected int FindDropAtIndex(TreeView.DragAndDropArgs args)
    {
      int num = args.insertAtIndex;
      if (num < 0 || num > this.rootItem.children.Count)
        num = this.rootItem.children.Count;
      return num;
    }

    protected override bool CanStartDrag(TreeView.CanStartDragArgs args)
    {
      return true;
    }

    protected override void SetupDragAndDrop(TreeView.SetupDragAndDropArgs args)
    {
      DragAndDrop.PrepareStartDrag();
      DragAndDrop.paths = (string[]) null;
      DragAndDrop.objectReferences = new UnityEngine.Object[0];
      DragAndDrop.SetGenericData("BuildPlayerSceneTreeViewItem", (object) new List<int>((IEnumerable<int>) args.draggedItemIDs));
      DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
      DragAndDrop.StartDrag(nameof (BuildPlayerSceneTreeView));
    }

    protected override void KeyEvent()
    {
      if (Event.current.keyCode != KeyCode.Delete && Event.current.keyCode != KeyCode.Backspace || this.GetSelection().Count <= 0)
        return;
      this.RemoveSelection();
    }

    protected override void ContextClicked()
    {
      if (this.GetSelection().Count <= 0)
        return;
      GenericMenu genericMenu = new GenericMenu();
      genericMenu.AddItem(new GUIContent("Remove Selection"), false, new GenericMenu.MenuFunction(this.RemoveSelection));
      genericMenu.ShowAsContext();
    }

    protected void RemoveSelection()
    {
      foreach (int id in (IEnumerable<int>) this.GetSelection())
        this.rootItem.children.Remove(this.FindItem(id, this.rootItem));
      EditorBuildSettings.scenes = this.GetSceneList();
      this.Reload();
      this.Repaint();
    }

    public EditorBuildSettingsScene[] GetSceneList()
    {
      EditorBuildSettingsScene[] buildSettingsSceneArray = new EditorBuildSettingsScene[this.rootItem.children.Count];
      for (int index = 0; index < this.rootItem.children.Count; ++index)
      {
        BuildPlayerSceneTreeViewItem child = this.rootItem.children[index] as BuildPlayerSceneTreeViewItem;
        buildSettingsSceneArray[index] = new EditorBuildSettingsScene(child.fullName, child.active);
      }
      return buildSettingsSceneArray;
    }
  }
}
