// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixersTreeViewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace UnityEditor
{
  internal class AudioMixersTreeViewGUI : TreeViewGUI
  {
    public AudioMixersTreeViewGUI(TreeViewController treeView)
      : base(treeView)
    {
      this.k_IconWidth = 0.0f;
      this.k_TopRowMargin = this.k_BottomRowMargin = 2f;
    }

    protected override void OnContentGUI(Rect rect, int row, TreeViewItem item, string label, bool selected, bool focused, bool useBoldFont, bool isPinging)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      if (!isPinging)
      {
        float contentIndent = this.GetContentIndent(item);
        rect.x += contentIndent;
        rect.width -= contentIndent;
      }
      AudioMixerItem audioMixerItem = item as AudioMixerItem;
      if (audioMixerItem == null)
        return;
      GUIStyle guiStyle = !useBoldFont ? TreeViewGUI.Styles.lineStyle : TreeViewGUI.Styles.lineBoldStyle;
      guiStyle.padding.left = (int) ((double) this.k_IconWidth + (double) this.iconTotalPadding + (double) this.k_SpaceBetweenIconAndText);
      guiStyle.Draw(rect, label, false, false, selected, focused);
      audioMixerItem.UpdateSuspendedString(false);
      if ((double) audioMixerItem.labelWidth <= 0.0)
        audioMixerItem.labelWidth = guiStyle.CalcSize(GUIContent.Temp(label)).x;
      Rect position = rect;
      position.x += audioMixerItem.labelWidth + 8f;
      using (new EditorGUI.DisabledScope(true))
        guiStyle.Draw(position, audioMixerItem.infoText, false, false, false, false);
      if (this.iconOverlayGUI == null)
        return;
      Rect rect1 = rect;
      rect1.width = this.k_IconWidth + this.iconTotalPadding;
      this.iconOverlayGUI(item, rect1);
    }

    protected override Texture GetIconForItem(TreeViewItem node)
    {
      return (Texture) null;
    }

    protected CreateAssetUtility GetCreateAssetUtility()
    {
      return ((TreeViewStateWithAssetUtility) this.m_TreeView.state).createAssetUtility;
    }

    protected override void RenameEnded()
    {
      string name = !string.IsNullOrEmpty(this.GetRenameOverlay().name) ? this.GetRenameOverlay().name : this.GetRenameOverlay().originalName;
      int userData = this.GetRenameOverlay().userData;
      bool flag = this.GetCreateAssetUtility().IsCreatingNewAsset();
      if (!this.GetRenameOverlay().userAcceptedRename)
        return;
      if (flag)
      {
        this.GetCreateAssetUtility().EndNewAssetCreation(name);
        this.m_TreeView.ReloadData();
      }
      else
        ObjectNames.SetNameSmartWithInstanceID(userData, name);
    }

    protected override void ClearRenameAndNewItemState()
    {
      this.GetCreateAssetUtility().Clear();
      base.ClearRenameAndNewItemState();
    }

    private AudioMixerItem GetSelectedItem()
    {
      return this.m_TreeView.FindItem(((IEnumerable<int>) this.m_TreeView.GetSelection()).FirstOrDefault<int>()) as AudioMixerItem;
    }

    protected override void SyncFakeItem()
    {
      if (!this.m_TreeView.data.HasFakeItem() && this.GetCreateAssetUtility().IsCreatingNewAsset())
      {
        int id = this.m_TreeView.data.root.id;
        AudioMixerItem selectedItem = this.GetSelectedItem();
        if (selectedItem != null)
          id = selectedItem.parent.id;
        this.m_TreeView.data.InsertFakeItem(this.GetCreateAssetUtility().instanceID, id, this.GetCreateAssetUtility().originalName, this.GetCreateAssetUtility().icon);
      }
      if (!this.m_TreeView.data.HasFakeItem() || this.GetCreateAssetUtility().IsCreatingNewAsset())
        return;
      this.m_TreeView.data.RemoveFakeItem();
    }

    public void BeginCreateNewMixer()
    {
      this.ClearRenameAndNewItemState();
      string empty = string.Empty;
      AudioMixerItem selectedItem = this.GetSelectedItem();
      if (selectedItem != null && (Object) selectedItem.mixer.outputAudioMixerGroup != (Object) null)
        empty = selectedItem.mixer.outputAudioMixerGroup.GetInstanceID().ToString();
      int num = 0;
      if (!this.GetCreateAssetUtility().BeginNewAssetCreation(num, (EndNameEditAction) ScriptableObject.CreateInstance<DoCreateAudioMixer>(), "NewAudioMixer.mixer", (Texture2D) null, empty))
        return;
      this.SyncFakeItem();
      if (!this.GetRenameOverlay().BeginRename(this.GetCreateAssetUtility().originalName, num, 0.0f))
        Debug.LogError((object) "Rename not started (when creating new asset)");
    }
  }
}
