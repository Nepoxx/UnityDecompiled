// Decompiled with JetBrains decompiler
// Type: UnityEditor.ListViewState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class ListViewState
  {
    public bool drawDropHere = false;
    public Rect dropHereRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    public string[] fileNames = (string[]) null;
    public int customDraggedFromID = 0;
    internal ListViewShared.InternalLayoutedListViewState ilvState = new ListViewShared.InternalLayoutedListViewState();
    private const int c_rowHeight = 16;
    public int row;
    public int column;
    public Vector2 scrollPos;
    public int totalRows;
    public int rowHeight;
    public int ID;
    public bool selectionChanged;
    public int draggedFrom;
    public int draggedTo;

    public ListViewState()
    {
      this.Init(0, 16);
    }

    public ListViewState(int totalRows)
    {
      this.Init(totalRows, 16);
    }

    public ListViewState(int totalRows, int rowHeight)
    {
      this.Init(totalRows, rowHeight);
    }

    private void Init(int totalRows, int rowHeight)
    {
      this.row = -1;
      this.column = 0;
      this.scrollPos = Vector2.zero;
      this.totalRows = totalRows;
      this.rowHeight = rowHeight;
      this.selectionChanged = false;
    }
  }
}
