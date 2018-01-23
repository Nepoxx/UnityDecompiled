// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShapeEditorSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class ShapeEditorSelection : IEnumerable<int>, IEnumerable
  {
    private HashSet<int> m_SelectedPoints = new HashSet<int>();
    private ShapeEditor m_ShapeEditor;

    public ShapeEditorSelection(ShapeEditor owner)
    {
      this.m_ShapeEditor = owner;
    }

    public bool Contains(int i)
    {
      return this.m_SelectedPoints.Contains(i);
    }

    public int Count
    {
      get
      {
        return this.m_SelectedPoints.Count;
      }
    }

    public void DeleteSelection()
    {
      foreach (int num in (IEnumerable<int>) this.m_SelectedPoints.OrderByDescending<int, int>((Func<int, int>) (x => x)))
        this.m_ShapeEditor.RemovePointAt(num);
      if (this.m_ShapeEditor.activePoint >= this.m_ShapeEditor.GetPointsCount())
        this.m_ShapeEditor.activePoint = this.m_ShapeEditor.GetPointsCount() - 1;
      this.m_SelectedPoints.Clear();
    }

    public void MoveSelection(Vector3 delta)
    {
      if ((double) delta.sqrMagnitude < 1.40129846432482E-45)
        return;
      foreach (int selectedPoint in this.m_SelectedPoints)
        this.m_ShapeEditor.SetPointPosition(selectedPoint, this.m_ShapeEditor.GetPointPosition(selectedPoint) + delta);
    }

    public void Clear()
    {
      this.m_SelectedPoints.Clear();
      if (this.m_ShapeEditor == null)
        return;
      this.m_ShapeEditor.activePoint = -1;
    }

    public void SelectPoint(int i, ShapeEditor.SelectionType type)
    {
      switch (type)
      {
        case ShapeEditor.SelectionType.Normal:
          this.m_SelectedPoints.Clear();
          this.m_ShapeEditor.activePoint = i;
          this.m_SelectedPoints.Add(i);
          break;
        case ShapeEditor.SelectionType.Additive:
          this.m_ShapeEditor.activePoint = i;
          this.m_SelectedPoints.Add(i);
          break;
        case ShapeEditor.SelectionType.Subtractive:
          this.m_ShapeEditor.activePoint = i <= 0 ? 0 : i - 1;
          this.m_SelectedPoints.Remove(i);
          break;
        default:
          this.m_ShapeEditor.activePoint = i;
          break;
      }
      this.m_ShapeEditor.Repaint();
    }

    public void RectSelect(Rect rect, ShapeEditor.SelectionType type)
    {
      if (type == ShapeEditor.SelectionType.Normal)
      {
        this.m_SelectedPoints.Clear();
        this.m_ShapeEditor.activePoint = -1;
        type = ShapeEditor.SelectionType.Additive;
      }
      for (int i = 0; i < this.m_ShapeEditor.GetPointsCount(); ++i)
      {
        Vector3 point = this.m_ShapeEditor.GetPointPosition(i);
        if (rect.Contains(point))
          this.SelectPoint(i, type);
      }
      this.m_ShapeEditor.Repaint();
    }

    public HashSet<int> indices
    {
      get
      {
        return this.m_SelectedPoints;
      }
    }

    public IEnumerator<int> GetEnumerator()
    {
      return (IEnumerator<int>) this.m_SelectedPoints.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
