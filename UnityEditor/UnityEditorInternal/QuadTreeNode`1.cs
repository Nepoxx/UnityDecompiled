// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.QuadTreeNode`1
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class QuadTreeNode<T> where T : IBounds
  {
    private static Color m_DebugFillColor = new Color(1f, 1f, 1f, 0.01f);
    private static Color m_DebugWireColor = new Color(1f, 0.0f, 0.0f, 0.5f);
    private static Color m_DebugBoxFillColor = new Color(1f, 0.0f, 0.0f, 0.01f);
    private List<T> m_Elements = new List<T>();
    private List<QuadTreeNode<T>> m_ChildrenNodes = new List<QuadTreeNode<T>>(4);
    private Rect m_BoundingRect;
    private const float kSmallestAreaForQuadTreeNode = 10f;

    public QuadTreeNode(Rect r)
    {
      this.m_BoundingRect = r;
    }

    public bool IsEmpty
    {
      get
      {
        return (double) this.m_BoundingRect.width == 0.0 && (double) this.m_BoundingRect.height == 0.0 || this.m_ChildrenNodes.Count == 0;
      }
    }

    public Rect BoundingRect
    {
      get
      {
        return this.m_BoundingRect;
      }
    }

    public int CountItemsIncludingChildren()
    {
      return this.Count(true);
    }

    public int CountLocalItems()
    {
      return this.Count(false);
    }

    private int Count(bool recursive)
    {
      int count = this.m_Elements.Count;
      if (recursive)
      {
        foreach (QuadTreeNode<T> childrenNode in this.m_ChildrenNodes)
          count += childrenNode.Count(recursive);
      }
      return count;
    }

    public List<T> GetElementsIncludingChildren()
    {
      return this.Elements(true);
    }

    public List<T> GetElements()
    {
      return this.Elements(false);
    }

    private List<T> Elements(bool recursive)
    {
      List<T> objList = new List<T>();
      if (recursive)
      {
        foreach (QuadTreeNode<T> childrenNode in this.m_ChildrenNodes)
          objList.AddRange((IEnumerable<T>) childrenNode.Elements(recursive));
      }
      objList.AddRange((IEnumerable<T>) this.m_Elements);
      return objList;
    }

    public List<T> IntersectsWith(Rect queryArea)
    {
      List<T> objList = new List<T>();
      foreach (T element in this.m_Elements)
      {
        if (RectUtils.Intersects(element.boundingRect, queryArea))
          objList.Add(element);
      }
      foreach (QuadTreeNode<T> childrenNode in this.m_ChildrenNodes)
      {
        if (!childrenNode.IsEmpty && RectUtils.Intersects(childrenNode.BoundingRect, queryArea))
        {
          objList.AddRange((IEnumerable<T>) childrenNode.IntersectsWith(queryArea));
          break;
        }
      }
      return objList;
    }

    public List<T> ContainedBy(Rect queryArea)
    {
      List<T> objList = new List<T>();
      foreach (T element in this.m_Elements)
      {
        if (RectUtils.Contains(element.boundingRect, queryArea) || queryArea.Overlaps(element.boundingRect))
          objList.Add(element);
      }
      foreach (QuadTreeNode<T> childrenNode in this.m_ChildrenNodes)
      {
        if (!childrenNode.IsEmpty)
        {
          if (RectUtils.Contains(childrenNode.BoundingRect, queryArea))
          {
            objList.AddRange((IEnumerable<T>) childrenNode.ContainedBy(queryArea));
            break;
          }
          if (RectUtils.Contains(queryArea, childrenNode.BoundingRect))
            objList.AddRange((IEnumerable<T>) childrenNode.Elements(true));
          else if (childrenNode.BoundingRect.Overlaps(queryArea))
            objList.AddRange((IEnumerable<T>) childrenNode.ContainedBy(queryArea));
        }
      }
      return objList;
    }

    public void Remove(T item)
    {
      this.m_Elements.Remove(item);
      foreach (QuadTreeNode<T> childrenNode in this.m_ChildrenNodes)
        childrenNode.Remove(item);
    }

    public void Insert(T item)
    {
      if (!RectUtils.Contains(this.m_BoundingRect, item.boundingRect))
      {
        Rect intersection = new Rect();
        if (!RectUtils.Intersection(item.boundingRect, this.m_BoundingRect, out intersection))
          return;
      }
      if (this.m_ChildrenNodes.Count == 0)
        this.Subdivide();
      foreach (QuadTreeNode<T> childrenNode in this.m_ChildrenNodes)
      {
        if (RectUtils.Contains(childrenNode.BoundingRect, item.boundingRect))
        {
          childrenNode.Insert(item);
          return;
        }
      }
      this.m_Elements.Add(item);
    }

    private void Subdivide()
    {
      if ((double) this.m_BoundingRect.height * (double) this.m_BoundingRect.width <= 10.0)
        return;
      float width = this.m_BoundingRect.width / 2f;
      float height = this.m_BoundingRect.height / 2f;
      this.m_ChildrenNodes.Add(new QuadTreeNode<T>(new Rect(this.m_BoundingRect.position.x, this.m_BoundingRect.position.y, width, height)));
      this.m_ChildrenNodes.Add(new QuadTreeNode<T>(new Rect(this.m_BoundingRect.xMin, this.m_BoundingRect.yMin + height, width, height)));
      this.m_ChildrenNodes.Add(new QuadTreeNode<T>(new Rect(this.m_BoundingRect.xMin + width, this.m_BoundingRect.yMin, width, height)));
      this.m_ChildrenNodes.Add(new QuadTreeNode<T>(new Rect(this.m_BoundingRect.xMin + width, this.m_BoundingRect.yMin + height, width, height)));
    }

    public void DebugDraw(Vector2 offset)
    {
      HandleUtility.ApplyWireMaterial();
      Rect boundingRect1 = this.m_BoundingRect;
      boundingRect1.x += offset.x;
      boundingRect1.y += offset.y;
      Handles.DrawSolidRectangleWithOutline(boundingRect1, QuadTreeNode<T>.m_DebugFillColor, QuadTreeNode<T>.m_DebugWireColor);
      foreach (QuadTreeNode<T> childrenNode in this.m_ChildrenNodes)
        childrenNode.DebugDraw(offset);
      foreach (T element in this.Elements(false))
      {
        Rect boundingRect2 = element.boundingRect;
        boundingRect2.x += offset.x;
        boundingRect2.y += offset.y;
        Handles.DrawSolidRectangleWithOutline(boundingRect2, QuadTreeNode<T>.m_DebugBoxFillColor, Color.yellow);
      }
    }
  }
}
