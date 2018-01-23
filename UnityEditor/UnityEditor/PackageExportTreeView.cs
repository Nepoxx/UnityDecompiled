// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageExportTreeView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Utils;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class PackageExportTreeView
  {
    private static readonly bool s_UseFoldouts = true;
    private List<PackageExportTreeView.PackageExportTreeViewItem> m_Selection = new List<PackageExportTreeView.PackageExportTreeViewItem>();
    private TreeViewController m_TreeView;
    private PackageExport m_PackageExport;

    public PackageExportTreeView(PackageExport packageExport, TreeViewState treeViewState, Rect startRect)
    {
      this.m_PackageExport = packageExport;
      this.m_TreeView = new TreeViewController((EditorWindow) this.m_PackageExport, treeViewState);
      PackageExportTreeView.PackageExportTreeViewDataSource treeViewDataSource = new PackageExportTreeView.PackageExportTreeViewDataSource(this.m_TreeView, this);
      PackageExportTreeView.PackageExportTreeViewGUI exportTreeViewGui = new PackageExportTreeView.PackageExportTreeViewGUI(this.m_TreeView, this);
      this.m_TreeView.Init(startRect, (ITreeViewDataSource) treeViewDataSource, (ITreeViewGUI) exportTreeViewGui, (ITreeViewDragging) null);
      this.m_TreeView.ReloadData();
      this.m_TreeView.selectionChangedCallback += new Action<int[]>(this.SelectionChanged);
      exportTreeViewGui.itemWasToggled += new Action<PackageExportTreeView.PackageExportTreeViewItem>(this.ItemWasToggled);
      this.ComputeEnabledStateForFolders();
    }

    public ExportPackageItem[] items
    {
      get
      {
        return this.m_PackageExport.items;
      }
    }

    private void ComputeEnabledStateForFolders()
    {
      PackageExportTreeView.PackageExportTreeViewItem root = this.m_TreeView.data.root as PackageExportTreeView.PackageExportTreeViewItem;
      this.RecursiveComputeEnabledStateForFolders(root, new HashSet<PackageExportTreeView.PackageExportTreeViewItem>()
      {
        root
      });
    }

    private void RecursiveComputeEnabledStateForFolders(PackageExportTreeView.PackageExportTreeViewItem pitem, HashSet<PackageExportTreeView.PackageExportTreeViewItem> done)
    {
      if (!pitem.isFolder)
        return;
      if (pitem.hasChildren)
      {
        foreach (TreeViewItem child in pitem.children)
          this.RecursiveComputeEnabledStateForFolders(child as PackageExportTreeView.PackageExportTreeViewItem, done);
      }
      if (done.Contains(pitem))
        return;
      PackageExportTreeView.EnabledState childrenEnabledState = this.GetFolderChildrenEnabledState(pitem);
      pitem.enabledState = childrenEnabledState;
      if (childrenEnabledState == PackageExportTreeView.EnabledState.Mixed)
      {
        done.Add(pitem);
        for (PackageExportTreeView.PackageExportTreeViewItem parent = pitem.parent as PackageExportTreeView.PackageExportTreeViewItem; parent != null; parent = parent.parent as PackageExportTreeView.PackageExportTreeViewItem)
        {
          if (!done.Contains(parent))
          {
            parent.enabledState = PackageExportTreeView.EnabledState.Mixed;
            done.Add(parent);
          }
        }
      }
    }

    private PackageExportTreeView.EnabledState GetFolderChildrenEnabledState(PackageExportTreeView.PackageExportTreeViewItem folder)
    {
      if (!folder.isFolder)
        Debug.LogError((object) "Should be a folder item!");
      if (!folder.hasChildren)
        return PackageExportTreeView.EnabledState.None;
      PackageExportTreeView.EnabledState enabledState1 = PackageExportTreeView.EnabledState.NotSet;
      PackageExportTreeView.EnabledState enabledState2 = (folder.children[0] as PackageExportTreeView.PackageExportTreeViewItem).enabledState;
      for (int index = 1; index < folder.children.Count; ++index)
      {
        PackageExportTreeView.PackageExportTreeViewItem child = folder.children[index] as PackageExportTreeView.PackageExportTreeViewItem;
        if (enabledState2 != child.enabledState)
        {
          enabledState1 = PackageExportTreeView.EnabledState.Mixed;
          break;
        }
      }
      if (enabledState1 == PackageExportTreeView.EnabledState.NotSet)
        enabledState1 = enabledState2 != PackageExportTreeView.EnabledState.All ? PackageExportTreeView.EnabledState.None : PackageExportTreeView.EnabledState.All;
      return enabledState1;
    }

    private void SelectionChanged(int[] selectedIDs)
    {
      this.m_Selection = new List<PackageExportTreeView.PackageExportTreeViewItem>();
      foreach (TreeViewItem row in (IEnumerable<TreeViewItem>) this.m_TreeView.data.GetRows())
      {
        if (((IEnumerable<int>) selectedIDs).Contains<int>(row.id))
        {
          PackageExportTreeView.PackageExportTreeViewItem exportTreeViewItem = row as PackageExportTreeView.PackageExportTreeViewItem;
          if (exportTreeViewItem != null)
            this.m_Selection.Add(exportTreeViewItem);
        }
      }
    }

    public void OnGUI(Rect rect)
    {
      int controlId = GUIUtility.GetControlID(FocusType.Keyboard);
      this.m_TreeView.OnGUI(rect, controlId);
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Space || (this.m_Selection == null || this.m_Selection.Count <= 0) || GUIUtility.keyboardControl != controlId)
        return;
      this.m_Selection[0].enabledState = this.m_Selection[0].enabledState == PackageExportTreeView.EnabledState.All ? PackageExportTreeView.EnabledState.None : PackageExportTreeView.EnabledState.All;
      this.ItemWasToggled(this.m_Selection[0]);
      Event.current.Use();
    }

    public void SetAllEnabled(PackageExportTreeView.EnabledState enabled)
    {
      this.EnableChildrenRecursive(this.m_TreeView.data.root, enabled);
      this.ComputeEnabledStateForFolders();
    }

    private void ItemWasToggled(PackageExportTreeView.PackageExportTreeViewItem pitem)
    {
      if (this.m_Selection.Count <= 1)
      {
        this.EnableChildrenRecursive((TreeViewItem) pitem, pitem.enabledState);
      }
      else
      {
        foreach (PackageExportTreeView.PackageExportTreeViewItem exportTreeViewItem in this.m_Selection)
          exportTreeViewItem.enabledState = pitem.enabledState;
      }
      this.ComputeEnabledStateForFolders();
    }

    private void EnableChildrenRecursive(TreeViewItem parentItem, PackageExportTreeView.EnabledState enabled)
    {
      if (!parentItem.hasChildren)
        return;
      foreach (TreeViewItem child in parentItem.children)
      {
        PackageExportTreeView.PackageExportTreeViewItem exportTreeViewItem = child as PackageExportTreeView.PackageExportTreeViewItem;
        exportTreeViewItem.enabledState = enabled;
        this.EnableChildrenRecursive((TreeViewItem) exportTreeViewItem, enabled);
      }
    }

    public enum EnabledState
    {
      NotSet = -1,
      None = 0,
      All = 1,
      Mixed = 2,
    }

    private class PackageExportTreeViewItem : TreeViewItem
    {
      private PackageExportTreeView.EnabledState m_EnabledState = PackageExportTreeView.EnabledState.NotSet;

      public PackageExportTreeViewItem(ExportPackageItem itemIn, int id, int depth, TreeViewItem parent, string displayName)
        : base(id, depth, parent, displayName)
      {
        this.item = itemIn;
      }

      public ExportPackageItem item { get; set; }

      public PackageExportTreeView.EnabledState enabledState
      {
        get
        {
          return this.item == null ? this.m_EnabledState : (PackageExportTreeView.EnabledState) this.item.enabledStatus;
        }
        set
        {
          if (this.item != null)
            this.item.enabledStatus = (int) value;
          else
            this.m_EnabledState = value;
        }
      }

      public bool isFolder
      {
        get
        {
          return this.item == null || this.item.isFolder;
        }
      }
    }

    private class PackageExportTreeViewGUI : TreeViewGUI
    {
      protected float k_FoldoutWidth = 12f;
      public Action<PackageExportTreeView.PackageExportTreeViewItem> itemWasToggled;
      private PackageExportTreeView m_PackageExportView;

      public PackageExportTreeViewGUI(TreeViewController treeView, PackageExportTreeView view)
        : base(treeView)
      {
        this.m_PackageExportView = view;
        this.k_BaseIndent = 4f;
        if (PackageExportTreeView.s_UseFoldouts)
          return;
        this.k_FoldoutWidth = 0.0f;
      }

      public int showPreviewForID { get; set; }

      public override void OnRowGUI(Rect rowRect, TreeViewItem tvItem, int row, bool selected, bool focused)
      {
        this.k_IndentWidth = 18f;
        this.k_FoldoutWidth = 18f;
        PackageExportTreeView.PackageExportTreeViewItem pitem = tvItem as PackageExportTreeView.PackageExportTreeViewItem;
        bool flag = Event.current.type == EventType.Repaint;
        if (selected && flag)
          TreeViewGUI.Styles.selectionStyle.Draw(rowRect, false, false, true, focused);
        if (this.m_TreeView.data.IsExpandable(tvItem))
          this.DoFoldout(rowRect, tvItem, row);
        Rect toggleRect = new Rect(this.k_BaseIndent + (float) tvItem.depth * this.indentWidth + this.k_FoldoutWidth, rowRect.y, 18f, rowRect.height);
        this.DoToggle(pitem, toggleRect);
        using (new EditorGUI.DisabledScope(pitem.item == null))
        {
          Rect contentRect = new Rect(toggleRect.xMax, rowRect.y, rowRect.width, rowRect.height);
          this.DoIconAndText(pitem, contentRect, selected, focused);
        }
      }

      private static void Toggle(ExportPackageItem[] items, PackageExportTreeView.PackageExportTreeViewItem pitem, Rect toggleRect)
      {
        bool flag1 = pitem.enabledState > PackageExportTreeView.EnabledState.None;
        GUIStyle style = EditorStyles.toggle;
        if (pitem.isFolder && pitem.enabledState == PackageExportTreeView.EnabledState.Mixed)
          style = EditorStyles.toggleMixed;
        bool flag2 = GUI.Toggle(toggleRect, flag1, GUIContent.none, style);
        if (flag2 == flag1)
          return;
        pitem.enabledState = !flag2 ? PackageExportTreeView.EnabledState.None : PackageExportTreeView.EnabledState.All;
      }

      private void DoToggle(PackageExportTreeView.PackageExportTreeViewItem pitem, Rect toggleRect)
      {
        EditorGUI.BeginChangeCheck();
        PackageExportTreeView.PackageExportTreeViewGUI.Toggle(this.m_PackageExportView.items, pitem, toggleRect);
        if (!EditorGUI.EndChangeCheck())
          return;
        if (this.m_TreeView.GetSelection().Length <= 1 || !((IEnumerable<int>) this.m_TreeView.GetSelection()).Contains<int>(pitem.id))
        {
          this.m_TreeView.SetSelection(new int[1]
          {
            pitem.id
          }, false);
          this.m_TreeView.NotifyListenersThatSelectionChanged();
        }
        if (this.itemWasToggled != null)
          this.itemWasToggled(pitem);
        Event.current.Use();
      }

      private void DoIconAndText(PackageExportTreeView.PackageExportTreeViewItem item, Rect contentRect, bool selected, bool focused)
      {
        EditorGUIUtility.SetIconSize(new Vector2(this.k_IconWidth, this.k_IconWidth));
        GUIStyle lineStyle = TreeViewGUI.Styles.lineStyle;
        lineStyle.padding.left = 0;
        contentRect.height += 5f;
        if (Event.current.type == EventType.Repaint)
          lineStyle.Draw(contentRect, GUIContent.Temp(item.displayName, this.GetIconForItem((TreeViewItem) item)), false, false, selected, focused);
        EditorGUIUtility.SetIconSize(Vector2.zero);
      }

      protected override Texture GetIconForItem(TreeViewItem tItem)
      {
        ExportPackageItem exportPackageItem = (tItem as PackageExportTreeView.PackageExportTreeViewItem).item;
        if (exportPackageItem == null || exportPackageItem.isFolder)
          return (Texture) PackageExportTreeView.PackageExportTreeViewGUI.Constants.folderIcon;
        Texture cachedIcon = AssetDatabase.GetCachedIcon(exportPackageItem.assetPath);
        if ((UnityEngine.Object) cachedIcon != (UnityEngine.Object) null)
          return cachedIcon;
        return (Texture) InternalEditorUtility.GetIconForFile(exportPackageItem.assetPath);
      }

      protected override void RenameEnded()
      {
      }

      internal static class Constants
      {
        public static Texture2D folderIcon = EditorGUIUtility.FindTexture(EditorResourcesUtility.folderIconName);
      }
    }

    private class PackageExportTreeViewDataSource : TreeViewDataSource
    {
      private PackageExportTreeView m_PackageExportView;

      public PackageExportTreeViewDataSource(TreeViewController treeView, PackageExportTreeView view)
        : base(treeView)
      {
        this.m_PackageExportView = view;
        this.rootIsCollapsable = false;
        this.showRootItem = false;
      }

      public override bool IsRenamingItemAllowed(TreeViewItem item)
      {
        return false;
      }

      public override bool IsExpandable(TreeViewItem item)
      {
        if (!PackageExportTreeView.s_UseFoldouts)
          return false;
        return base.IsExpandable(item);
      }

      public override void FetchData()
      {
        this.m_RootItem = (TreeViewItem) new PackageExportTreeView.PackageExportTreeViewItem((ExportPackageItem) null, "Assets".GetHashCode(), -1, (TreeViewItem) null, "InvisibleAssetsFolder");
        bool initExpandedState = true;
        if (initExpandedState)
          this.m_TreeView.state.expandedIDs.Add(this.m_RootItem.id);
        ExportPackageItem[] items = this.m_PackageExportView.items;
        Dictionary<string, PackageExportTreeView.PackageExportTreeViewItem> treeViewFolders = new Dictionary<string, PackageExportTreeView.PackageExportTreeViewItem>();
        for (int index = 0; index < items.Length; ++index)
        {
          ExportPackageItem itemIn = items[index];
          if (!PackageImport.HasInvalidCharInFilePath(itemIn.assetPath))
          {
            string unity = Path.GetFileName(itemIn.assetPath).ConvertSeparatorsToUnity();
            TreeViewItem parent = this.EnsureFolderPath(Path.GetDirectoryName(itemIn.assetPath).ConvertSeparatorsToUnity(), treeViewFolders, initExpandedState);
            if (parent != null)
            {
              int hashCode = itemIn.assetPath.GetHashCode();
              PackageExportTreeView.PackageExportTreeViewItem exportTreeViewItem = new PackageExportTreeView.PackageExportTreeViewItem(itemIn, hashCode, parent.depth + 1, parent, unity);
              parent.AddChild((TreeViewItem) exportTreeViewItem);
              if (initExpandedState)
                this.m_TreeView.state.expandedIDs.Add(hashCode);
              if (itemIn.isFolder)
                treeViewFolders[itemIn.assetPath] = exportTreeViewItem;
            }
          }
        }
        if (!initExpandedState)
          return;
        this.m_TreeView.state.expandedIDs.Sort();
      }

      private TreeViewItem EnsureFolderPath(string folderPath, Dictionary<string, PackageExportTreeView.PackageExportTreeViewItem> treeViewFolders, bool initExpandedState)
      {
        if (folderPath == "")
          return this.m_RootItem;
        TreeViewItem treeViewItem = TreeViewUtility.FindItem(folderPath.GetHashCode(), this.m_RootItem);
        if (treeViewItem != null)
          return treeViewItem;
        string[] strArray = folderPath.Split('/');
        string key = "";
        TreeViewItem parent = this.m_RootItem;
        int depth = -1;
        for (int index = 0; index < strArray.Length; ++index)
        {
          string displayName = strArray[index];
          if (key != "")
            key += (string) (object) '/';
          key += displayName;
          if (index != 0 || !(key == "Assets"))
          {
            ++depth;
            int hashCode = key.GetHashCode();
            PackageExportTreeView.PackageExportTreeViewItem exportTreeViewItem1;
            if (treeViewFolders.TryGetValue(key, out exportTreeViewItem1))
            {
              parent = (TreeViewItem) exportTreeViewItem1;
            }
            else
            {
              PackageExportTreeView.PackageExportTreeViewItem exportTreeViewItem2 = new PackageExportTreeView.PackageExportTreeViewItem((ExportPackageItem) null, hashCode, depth, parent, displayName);
              parent.AddChild((TreeViewItem) exportTreeViewItem2);
              parent = (TreeViewItem) exportTreeViewItem2;
              if (initExpandedState)
                this.m_TreeView.state.expandedIDs.Add(hashCode);
              treeViewFolders[key] = exportTreeViewItem2;
            }
          }
        }
        return parent;
      }
    }
  }
}
