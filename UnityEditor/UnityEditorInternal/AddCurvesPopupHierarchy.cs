// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AddCurvesPopupHierarchy
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AddCurvesPopupHierarchy
  {
    private TreeViewController m_TreeView;
    private TreeViewState m_TreeViewState;
    private AddCurvesPopupHierarchyDataSource m_TreeViewDataSource;

    public void OnGUI(Rect position, EditorWindow owner)
    {
      this.InitIfNeeded(owner, position);
      this.m_TreeView.OnEvent();
      this.m_TreeView.OnGUI(position, GUIUtility.GetControlID(FocusType.Keyboard));
    }

    public void InitIfNeeded(EditorWindow owner, Rect rect)
    {
      if (this.m_TreeViewState != null)
        return;
      this.m_TreeViewState = new TreeViewState();
      this.m_TreeView = new TreeViewController(owner, this.m_TreeViewState);
      this.m_TreeView.deselectOnUnhandledMouseDown = true;
      this.m_TreeViewDataSource = new AddCurvesPopupHierarchyDataSource(this.m_TreeView);
      TreeViewGUI treeViewGui = (TreeViewGUI) new AddCurvesPopupHierarchyGUI(this.m_TreeView, owner);
      this.m_TreeView.Init(rect, (ITreeViewDataSource) this.m_TreeViewDataSource, (ITreeViewGUI) treeViewGui, (ITreeViewDragging) null);
      this.m_TreeViewDataSource.UpdateData();
    }

    internal virtual bool IsRenamingNodeAllowed(TreeViewItem node)
    {
      return false;
    }
  }
}
