// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.QuadTree`1
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class QuadTree<T> where T : IBounds
  {
    private QuadTreeNode<T> m_Root = (QuadTreeNode<T>) null;
    private Vector2 m_ScreenSpaceOffset = Vector2.zero;
    private Rect m_Rectangle;

    public QuadTree()
    {
      this.Clear();
    }

    public Vector2 screenSpaceOffset
    {
      get
      {
        return this.m_ScreenSpaceOffset;
      }
      set
      {
        this.m_ScreenSpaceOffset = value;
      }
    }

    public Rect rectangle
    {
      get
      {
        return this.m_Rectangle;
      }
    }

    public void Clear()
    {
      this.SetSize(new Rect(0.0f, 0.0f, 1f, 1f));
    }

    public void SetSize(Rect rectangle)
    {
      this.m_Root = (QuadTreeNode<T>) null;
      this.m_Rectangle = rectangle;
      this.m_Root = new QuadTreeNode<T>(this.m_Rectangle);
    }

    public int Count
    {
      get
      {
        return this.m_Root.CountItemsIncludingChildren();
      }
    }

    public void Insert(List<T> items)
    {
      foreach (T obj in items)
        this.Insert(obj);
    }

    public void Insert(T item)
    {
      this.m_Root.Insert(item);
    }

    public void Remove(T item)
    {
      this.m_Root.Remove(item);
    }

    public List<T> GetItemsAtPosition(Vector2 pos)
    {
      return this.IntersectsWith(new Rect(pos, Vector2.one));
    }

    public List<T> IntersectsWith(Rect area)
    {
      return this.m_Root.IntersectsWith(area);
    }

    public List<T> ContainedBy(Rect area)
    {
      area.x -= this.m_ScreenSpaceOffset.x;
      area.y -= this.m_ScreenSpaceOffset.y;
      return this.m_Root.ContainedBy(area);
    }

    public List<T> Elements()
    {
      return this.m_Root.GetElementsIncludingChildren();
    }

    public void DebugDraw()
    {
      this.m_Root.DebugDraw(this.m_ScreenSpaceOffset);
    }
  }
}
