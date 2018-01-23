// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewExamples.TreeViewTestWithCustomHeight
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor.TreeViewExamples
{
  internal class TreeViewTestWithCustomHeight
  {
    private BackendData m_BackendData;
    private TreeViewController m_TreeView;

    public TreeViewTestWithCustomHeight(EditorWindow editorWindow, BackendData backendData, Rect rect)
    {
      this.m_BackendData = backendData;
      TreeViewState treeViewState = new TreeViewState();
      this.m_TreeView = new TreeViewController(editorWindow, treeViewState);
      TestGUICustomItemHeights customItemHeights = new TestGUICustomItemHeights(this.m_TreeView);
      TestDragging testDragging = new TestDragging(this.m_TreeView, this.m_BackendData);
      TestDataSource testDataSource1 = new TestDataSource(this.m_TreeView, this.m_BackendData);
      TestDataSource testDataSource2 = testDataSource1;
      testDataSource2.onVisibleRowsChanged = testDataSource2.onVisibleRowsChanged + new Action(((TreeViewGUIWithCustomItemsHeights) customItemHeights).CalculateRowRects);
      this.m_TreeView.Init(rect, (ITreeViewDataSource) testDataSource1, (ITreeViewGUI) customItemHeights, (ITreeViewDragging) testDragging);
      testDataSource1.SetExpanded(testDataSource1.root, true);
    }

    public void OnGUI(Rect rect)
    {
      int controlId = GUIUtility.GetControlID(FocusType.Keyboard, rect);
      this.m_TreeView.OnGUI(rect, controlId);
    }
  }
}
