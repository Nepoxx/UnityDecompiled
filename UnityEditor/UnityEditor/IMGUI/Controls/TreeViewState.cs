// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.TreeViewState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  /// <summary>
  ///   <para>The TreeViewState contains serializable state information for the TreeView.</para>
  /// </summary>
  [Serializable]
  public class TreeViewState
  {
    [SerializeField]
    private List<int> m_SelectedIDs = new List<int>();
    [SerializeField]
    private List<int> m_ExpandedIDs = new List<int>();
    [SerializeField]
    private RenameOverlay m_RenameOverlay = new RenameOverlay();
    /// <summary>
    ///   <para>The current scroll values of the TreeView's scroll view.</para>
    /// </summary>
    public Vector2 scrollPos;
    [SerializeField]
    private int m_LastClickedID;
    [SerializeField]
    private string m_SearchString;

    /// <summary>
    ///   <para>Selected TreeViewItem IDs. Use of the SetSelection and IsSelected API will access this state.</para>
    /// </summary>
    public List<int> selectedIDs
    {
      get
      {
        return this.m_SelectedIDs;
      }
      set
      {
        this.m_SelectedIDs = value;
      }
    }

    /// <summary>
    ///   <para>The ID for the TreeViewItem that currently is being used for multi selection and key navigation.</para>
    /// </summary>
    public int lastClickedID
    {
      get
      {
        return this.m_LastClickedID;
      }
      set
      {
        this.m_LastClickedID = value;
      }
    }

    /// <summary>
    ///   <para>This is the list of currently expanded TreeViewItem IDs.</para>
    /// </summary>
    public List<int> expandedIDs
    {
      get
      {
        return this.m_ExpandedIDs;
      }
      set
      {
        this.m_ExpandedIDs = value;
      }
    }

    internal RenameOverlay renameOverlay
    {
      get
      {
        return this.m_RenameOverlay;
      }
      set
      {
        this.m_RenameOverlay = value;
      }
    }

    /// <summary>
    ///   <para>Search string state that can be used in the TreeView to filter the tree data when creating the TreeViewItems.</para>
    /// </summary>
    public string searchString
    {
      get
      {
        return this.m_SearchString;
      }
      set
      {
        this.m_SearchString = value;
      }
    }

    internal virtual void OnAwake()
    {
      this.m_RenameOverlay.Clear();
    }
  }
}
