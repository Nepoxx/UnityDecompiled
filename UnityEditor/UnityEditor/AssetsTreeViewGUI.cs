// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetsTreeViewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEditor.ProjectWindowCallback;
using UnityEditor.VersionControl;
using UnityEditorInternal.VersionControl;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetsTreeViewGUI : TreeViewGUI
  {
    private static IDictionary<int, string> s_GUIDCache = (IDictionary<int, string>) null;
    private static bool s_VCEnabled;
    private const float k_IconOverlayPadding = 7f;

    public AssetsTreeViewGUI(TreeViewController treeView)
      : base(treeView)
    {
      AssetsTreeViewGUI assetsTreeViewGui1 = this;
      assetsTreeViewGui1.iconOverlayGUI = assetsTreeViewGui1.iconOverlayGUI + new Action<TreeViewItem, Rect>(this.OnIconOverlayGUI);
      AssetsTreeViewGUI assetsTreeViewGui2 = this;
      assetsTreeViewGui2.labelOverlayGUI = assetsTreeViewGui2.labelOverlayGUI + new Action<TreeViewItem, Rect>(this.OnLabelOverlayGUI);
      this.k_TopRowMargin = 4f;
    }

    internal static event AssetsTreeViewGUI.OnAssetIconDrawDelegate postAssetIconDrawCallback = null;

    internal static event AssetsTreeViewGUI.OnAssetLabelDrawDelegate postAssetLabelDrawCallback = null;

    public override void BeginRowGUI()
    {
      AssetsTreeViewGUI.s_VCEnabled = Provider.isActive;
      float num = !AssetsTreeViewGUI.s_VCEnabled ? 0.0f : 7f;
      this.iconRightPadding = num;
      this.iconLeftPadding = num;
      base.BeginRowGUI();
    }

    protected CreateAssetUtility GetCreateAssetUtility()
    {
      return ((TreeViewStateWithAssetUtility) this.m_TreeView.state).createAssetUtility;
    }

    protected virtual bool IsCreatingNewAsset(int instanceID)
    {
      return this.GetCreateAssetUtility().IsCreatingNewAsset() && this.IsRenaming(instanceID);
    }

    protected override void ClearRenameAndNewItemState()
    {
      this.GetCreateAssetUtility().Clear();
      base.ClearRenameAndNewItemState();
    }

    protected override void RenameEnded()
    {
      string name = !string.IsNullOrEmpty(this.GetRenameOverlay().name) ? this.GetRenameOverlay().name : this.GetRenameOverlay().originalName;
      int userData = this.GetRenameOverlay().userData;
      bool flag = this.GetCreateAssetUtility().IsCreatingNewAsset();
      if (!this.GetRenameOverlay().userAcceptedRename)
        return;
      if (flag)
        this.GetCreateAssetUtility().EndNewAssetCreation(name);
      else
        ObjectNames.SetNameSmartWithInstanceID(userData, name);
    }

    protected override void SyncFakeItem()
    {
      if (!this.m_TreeView.data.HasFakeItem() && this.GetCreateAssetUtility().IsCreatingNewAsset())
        this.m_TreeView.data.InsertFakeItem(this.GetCreateAssetUtility().instanceID, AssetDatabase.GetMainAssetInstanceID(this.GetCreateAssetUtility().folder), this.GetCreateAssetUtility().originalName, this.GetCreateAssetUtility().icon);
      if (!this.m_TreeView.data.HasFakeItem() || this.GetCreateAssetUtility().IsCreatingNewAsset())
        return;
      this.m_TreeView.data.RemoveFakeItem();
    }

    public virtual void BeginCreateNewAsset(int instanceID, EndNameEditAction endAction, string pathName, Texture2D icon, string resourceFile)
    {
      this.ClearRenameAndNewItemState();
      if (!this.GetCreateAssetUtility().BeginNewAssetCreation(instanceID, endAction, pathName, icon, resourceFile))
        return;
      this.SyncFakeItem();
      if (!this.GetRenameOverlay().BeginRename(this.GetCreateAssetUtility().originalName, instanceID, 0.0f))
        Debug.LogError((object) "Rename not started (when creating new asset)");
    }

    protected override Texture GetIconForItem(TreeViewItem item)
    {
      if (item == null)
        return (Texture) null;
      Texture texture = (Texture) null;
      if (this.IsCreatingNewAsset(item.id))
        texture = (Texture) this.GetCreateAssetUtility().icon;
      if ((UnityEngine.Object) texture == (UnityEngine.Object) null)
        texture = (Texture) item.icon;
      if ((UnityEngine.Object) texture == (UnityEngine.Object) null && item.id != 0)
        texture = AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(item.id));
      return texture;
    }

    private void OnIconOverlayGUI(TreeViewItem item, Rect overlayRect)
    {
      // ISSUE: reference to a compiler-generated field
      if (AssetsTreeViewGUI.postAssetIconDrawCallback != null && AssetDatabase.IsMainAsset(item.id))
      {
        string guidForInstanceId = AssetsTreeViewGUI.GetGUIDForInstanceID(item.id);
        // ISSUE: reference to a compiler-generated field
        AssetsTreeViewGUI.postAssetIconDrawCallback(overlayRect, guidForInstanceId);
      }
      if (!AssetsTreeViewGUI.s_VCEnabled || !AssetDatabase.IsMainAsset(item.id))
        return;
      ProjectHooks.OnProjectWindowItem(AssetsTreeViewGUI.GetGUIDForInstanceID(item.id), overlayRect);
    }

    private void OnLabelOverlayGUI(TreeViewItem item, Rect labelRect)
    {
      // ISSUE: reference to a compiler-generated field
      if (AssetsTreeViewGUI.postAssetLabelDrawCallback == null || !AssetDatabase.IsMainAsset(item.id))
        return;
      string guidForInstanceId = AssetsTreeViewGUI.GetGUIDForInstanceID(item.id);
      // ISSUE: reference to a compiler-generated field
      int num = AssetsTreeViewGUI.postAssetLabelDrawCallback(labelRect, guidForInstanceId) ? 1 : 0;
    }

    private static string GetGUIDForInstanceID(int instanceID)
    {
      if (AssetsTreeViewGUI.s_GUIDCache == null)
        AssetsTreeViewGUI.s_GUIDCache = (IDictionary<int, string>) new Dictionary<int, string>();
      string str = (string) null;
      if (!AssetsTreeViewGUI.s_GUIDCache.TryGetValue(instanceID, out str))
      {
        str = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(instanceID));
        AssetsTreeViewGUI.s_GUIDCache.Add(instanceID, str);
      }
      return str;
    }

    internal delegate void OnAssetIconDrawDelegate(Rect iconRect, string guid);

    internal delegate bool OnAssetLabelDrawDelegate(Rect drawRect, string guid);
  }
}
