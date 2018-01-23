// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioGroupTreeViewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.Audio;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  internal class AudioGroupTreeViewGUI : TreeViewGUI
  {
    private readonly float column1Width = 20f;
    private readonly Texture2D k_VisibleON = EditorGUIUtility.FindTexture("VisibilityOn");
    public AudioMixerController m_Controller = (AudioMixerController) null;
    public Action<AudioMixerTreeViewNode, bool> NodeWasToggled;

    public AudioGroupTreeViewGUI(TreeViewController treeView)
      : base(treeView)
    {
      this.k_BaseIndent = this.column1Width;
      this.k_IconWidth = 0.0f;
      this.k_TopRowMargin = this.k_BottomRowMargin = 2f;
    }

    private void OpenGroupContextMenu(AudioMixerTreeViewNode audioNode, bool visible)
    {
      GenericMenu menu = new GenericMenu();
      if (this.NodeWasToggled != null)
        menu.AddItem(new GUIContent(!visible ? "Show Group" : "Hide group"), false, (GenericMenu.MenuFunction) (() => this.NodeWasToggled(audioNode, !visible)));
      menu.AddSeparator(string.Empty);
      AudioMixerGroupController[] groups;
      if (this.m_Controller.CachedSelection.Contains(audioNode.group))
        groups = this.m_Controller.CachedSelection.ToArray();
      else
        groups = new AudioMixerGroupController[1]
        {
          audioNode.group
        };
      AudioMixerColorCodes.AddColorItemsToGenericMenu(menu, groups);
      menu.ShowAsContext();
    }

    public override void OnRowGUI(Rect rowRect, TreeViewItem node, int row, bool selected, bool focused)
    {
      Event current = Event.current;
      this.DoItemGUI(rowRect, row, node, selected, focused, false);
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      AudioMixerTreeViewNode audioNode = node as AudioMixerTreeViewNode;
      if (audioNode == null)
        return;
      bool visible = this.m_Controller.CurrentViewContainsGroup(audioNode.group.groupID);
      float num = 3f;
      Rect position = new Rect(rowRect.x + num, rowRect.y, 16f, 16f);
      Rect rect1 = new Rect(position.x + 1f, position.y + 1f, position.width - 2f, position.height - 2f);
      int userColorIndex = audioNode.group.userColorIndex;
      if (userColorIndex > 0)
        EditorGUI.DrawRect(new Rect(rowRect.x, rect1.y, 2f, rect1.height), AudioMixerColorCodes.GetColor(userColorIndex));
      EditorGUI.DrawRect(rect1, new Color(0.5f, 0.5f, 0.5f, 0.2f));
      if (visible)
        GUI.DrawTexture(position, (Texture) this.k_VisibleON);
      Rect rect2 = new Rect(2f, rowRect.y, rowRect.height, rowRect.height);
      if (current.type == EventType.MouseUp && current.button == 0 && (rect2.Contains(current.mousePosition) && this.NodeWasToggled != null))
        this.NodeWasToggled(audioNode, !visible);
      if (current.type == EventType.ContextClick && position.Contains(current.mousePosition))
      {
        this.OpenGroupContextMenu(audioNode, visible);
        current.Use();
      }
    }

    protected override Texture GetIconForItem(TreeViewItem node)
    {
      if (node != null && (UnityEngine.Object) node.icon != (UnityEngine.Object) null)
        return (Texture) node.icon;
      return (Texture) null;
    }

    protected override void SyncFakeItem()
    {
    }

    protected override void RenameEnded()
    {
      if (!this.GetRenameOverlay().userAcceptedRename)
        return;
      string name = !string.IsNullOrEmpty(this.GetRenameOverlay().name) ? this.GetRenameOverlay().name : this.GetRenameOverlay().originalName;
      int userData = this.GetRenameOverlay().userData;
      AudioMixerTreeViewNode mixerTreeViewNode = this.m_TreeView.FindItem(userData) as AudioMixerTreeViewNode;
      if (mixerTreeViewNode != null)
      {
        ObjectNames.SetNameSmartWithInstanceID(userData, name);
        foreach (AudioMixerEffectController effect in mixerTreeViewNode.group.effects)
          effect.ClearCachedDisplayName();
        this.m_TreeView.ReloadData();
        if ((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null)
          this.m_Controller.OnSubAssetChanged();
      }
    }
  }
}
