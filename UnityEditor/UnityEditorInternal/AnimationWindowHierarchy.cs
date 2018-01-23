// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowHierarchy
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AnimationWindowHierarchy
  {
    private AnimationWindowState m_State;
    private TreeViewController m_TreeView;

    public AnimationWindowHierarchy(AnimationWindowState state, EditorWindow owner, Rect position)
    {
      this.m_State = state;
      this.Init(owner, position);
    }

    public Vector2 GetContentSize()
    {
      return this.m_TreeView.GetContentSize();
    }

    public Rect GetTotalRect()
    {
      return this.m_TreeView.GetTotalRect();
    }

    public void OnGUI(Rect position)
    {
      this.m_TreeView.OnEvent();
      this.m_TreeView.OnGUI(position, GUIUtility.GetControlID(FocusType.Keyboard));
    }

    public void Init(EditorWindow owner, Rect rect)
    {
      if (this.m_State.hierarchyState == null)
        this.m_State.hierarchyState = new AnimationWindowHierarchyState();
      this.m_TreeView = new TreeViewController(owner, (TreeViewState) this.m_State.hierarchyState);
      this.m_State.hierarchyData = new AnimationWindowHierarchyDataSource(this.m_TreeView, this.m_State);
      this.m_TreeView.Init(rect, (ITreeViewDataSource) this.m_State.hierarchyData, (ITreeViewGUI) new AnimationWindowHierarchyGUI(this.m_TreeView, this.m_State), (ITreeViewDragging) null);
      this.m_TreeView.deselectOnUnhandledMouseDown = true;
      this.m_TreeView.selectionChangedCallback += new Action<int[]>(this.m_State.OnHierarchySelectionChanged);
      this.m_TreeView.ReloadData();
    }

    internal virtual bool IsRenamingNodeAllowed(TreeViewItem node)
    {
      return true;
    }

    public bool IsIDVisible(int id)
    {
      if (this.m_TreeView == null)
        return false;
      return TreeViewController.GetIndexOfID(this.m_TreeView.data.GetRows(), id) >= 0;
    }

    public void EndNameEditing(bool acceptChanges)
    {
      this.m_TreeView.EndNameEditing(acceptChanges);
    }
  }
}
