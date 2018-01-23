// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryElementSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  [Serializable]
  internal class MemoryElementSelection
  {
    private MemoryElement m_Selected = (MemoryElement) null;

    public void SetSelection(MemoryElement node)
    {
      this.m_Selected = node;
      for (MemoryElement parent = node.parent; parent != null; parent = parent.parent)
        parent.expanded = true;
    }

    public void ClearSelection()
    {
      this.m_Selected = (MemoryElement) null;
    }

    public bool isSelected(MemoryElement node)
    {
      return this.m_Selected == node;
    }

    public MemoryElement Selected
    {
      get
      {
        return this.m_Selected;
      }
    }

    public void MoveUp()
    {
      if (this.m_Selected == null || this.m_Selected.parent == null)
        return;
      MemoryElement prevNode = this.m_Selected.GetPrevNode();
      if (prevNode.parent != null)
        this.SetSelection(prevNode);
      else
        this.SetSelection(prevNode.FirstChild());
    }

    public void MoveDown()
    {
      if (this.m_Selected == null || this.m_Selected.parent == null)
        return;
      MemoryElement nextNode = this.m_Selected.GetNextNode();
      if (nextNode == null)
        return;
      this.SetSelection(nextNode);
    }

    public void MoveFirst()
    {
      if (this.m_Selected == null || this.m_Selected.parent == null)
        return;
      this.SetSelection(this.m_Selected.GetRoot().FirstChild());
    }

    public void MoveLast()
    {
      if (this.m_Selected == null || this.m_Selected.parent == null)
        return;
      this.SetSelection(this.m_Selected.GetRoot().LastChild());
    }

    public void MoveParent()
    {
      if (this.m_Selected == null || this.m_Selected.parent == null || this.m_Selected.parent.parent == null)
        return;
      this.SetSelection(this.m_Selected.parent);
    }
  }
}
