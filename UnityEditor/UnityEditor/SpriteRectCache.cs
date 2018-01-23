// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteRectCache
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.U2D.Interface;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class SpriteRectCache : ScriptableObject, ISpriteRectCache, IUndoableObject
  {
    [SerializeField]
    public List<SpriteRect> m_Rects;

    public int Count
    {
      get
      {
        return this.m_Rects == null ? 0 : this.m_Rects.Count;
      }
    }

    public SpriteRect RectAt(int i)
    {
      return i >= this.Count || i < 0 ? (SpriteRect) null : this.m_Rects[i];
    }

    public void AddRect(SpriteRect r)
    {
      if (this.m_Rects == null)
        return;
      this.m_Rects.Add(r);
    }

    public void RemoveRect(SpriteRect r)
    {
      if (this.m_Rects == null)
        return;
      this.m_Rects.Remove(r);
    }

    public void ClearAll()
    {
      if (this.m_Rects == null)
        return;
      this.m_Rects.Clear();
    }

    public int GetIndex(SpriteRect spriteRect)
    {
      if (this.m_Rects != null)
        return this.m_Rects.FindIndex((Predicate<SpriteRect>) (p => p.Equals((object) spriteRect)));
      return 0;
    }

    public bool Contains(SpriteRect spriteRect)
    {
      if (this.m_Rects != null)
        return this.m_Rects.Contains(spriteRect);
      return false;
    }

    private void OnEnable()
    {
      if (this.m_Rects != null)
        return;
      this.m_Rects = new List<SpriteRect>();
    }
  }
}
