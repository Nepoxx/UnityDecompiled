// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditorInternal
{
  [Serializable]
  internal class AnimationWindowSelection
  {
    [SerializeField]
    private List<AnimationWindowSelectionItem> m_Selection = new List<AnimationWindowSelectionItem>();
    private bool m_BatchOperations = false;
    private bool m_SelectionChanged = false;
    private List<AnimationWindowCurve> m_CurvesCache = (List<AnimationWindowCurve>) null;
    [NonSerialized]
    public Action onSelectionChanged;

    public AnimationWindowSelection()
    {
      this.onSelectionChanged += (Action) (() => {});
    }

    public int count
    {
      get
      {
        return this.m_Selection.Count;
      }
    }

    public List<AnimationWindowCurve> curves
    {
      get
      {
        if (this.m_CurvesCache == null)
        {
          this.m_CurvesCache = new List<AnimationWindowCurve>();
          foreach (AnimationWindowSelectionItem windowSelectionItem in this.m_Selection)
            this.m_CurvesCache.AddRange((IEnumerable<AnimationWindowCurve>) windowSelectionItem.curves);
        }
        return this.m_CurvesCache;
      }
    }

    public bool disabled
    {
      get
      {
        if (this.m_Selection.Count > 0)
        {
          foreach (AnimationWindowSelectionItem windowSelectionItem in this.m_Selection)
          {
            if ((UnityEngine.Object) windowSelectionItem.animationClip != (UnityEngine.Object) null)
              return false;
          }
        }
        return true;
      }
    }

    public bool canPreview
    {
      get
      {
        if (this.m_Selection.Count > 0)
          return !this.m_Selection.Any<AnimationWindowSelectionItem>((Func<AnimationWindowSelectionItem, bool>) (item => !item.canPreview));
        return false;
      }
    }

    public bool canRecord
    {
      get
      {
        if (this.m_Selection.Count > 0)
          return !this.m_Selection.Any<AnimationWindowSelectionItem>((Func<AnimationWindowSelectionItem, bool>) (item => !item.canRecord));
        return false;
      }
    }

    public bool canAddCurves
    {
      get
      {
        if (this.m_Selection.Count > 0)
          return !this.m_Selection.Any<AnimationWindowSelectionItem>((Func<AnimationWindowSelectionItem, bool>) (item => !item.canAddCurves));
        return false;
      }
    }

    public void BeginOperations()
    {
      if (this.m_BatchOperations)
      {
        Debug.LogWarning((object) "AnimationWindowSelection: Already inside a BeginOperations/EndOperations block");
      }
      else
      {
        this.m_BatchOperations = true;
        this.m_SelectionChanged = false;
      }
    }

    public void EndOperations()
    {
      if (!this.m_BatchOperations)
        return;
      if (this.m_SelectionChanged)
        this.onSelectionChanged();
      this.m_SelectionChanged = false;
      this.m_BatchOperations = false;
    }

    public void Notify()
    {
      if (this.m_BatchOperations)
        this.m_SelectionChanged = true;
      else
        this.onSelectionChanged();
    }

    public void Set(AnimationWindowSelectionItem newItem)
    {
      this.BeginOperations();
      this.Clear();
      this.Add(newItem);
      this.EndOperations();
    }

    public void Add(AnimationWindowSelectionItem newItem)
    {
      if (this.m_Selection.Contains(newItem))
        return;
      this.m_Selection.Add(newItem);
      this.Notify();
    }

    public void RangeAdd(AnimationWindowSelectionItem[] newItemArray)
    {
      bool flag = false;
      foreach (AnimationWindowSelectionItem newItem in newItemArray)
      {
        if (!this.m_Selection.Contains(newItem))
        {
          this.m_Selection.Add(newItem);
          flag = true;
        }
      }
      if (!flag)
        return;
      this.Notify();
    }

    public void UpdateClip(AnimationWindowSelectionItem itemToUpdate, AnimationClip newClip)
    {
      if (!this.m_Selection.Contains(itemToUpdate))
        return;
      itemToUpdate.animationClip = newClip;
      this.Notify();
    }

    public void UpdateTimeOffset(AnimationWindowSelectionItem itemToUpdate, float timeOffset)
    {
      if (!this.m_Selection.Contains(itemToUpdate))
        return;
      itemToUpdate.timeOffset = timeOffset;
    }

    public bool Exists(AnimationWindowSelectionItem itemToFind)
    {
      return this.m_Selection.Contains(itemToFind);
    }

    public bool Exists(Predicate<AnimationWindowSelectionItem> predicate)
    {
      return this.m_Selection.Exists(predicate);
    }

    public AnimationWindowSelectionItem Find(Predicate<AnimationWindowSelectionItem> predicate)
    {
      return this.m_Selection.Find(predicate);
    }

    public AnimationWindowSelectionItem First()
    {
      return this.m_Selection.First<AnimationWindowSelectionItem>();
    }

    public int GetRefreshHash()
    {
      int num = 0;
      foreach (AnimationWindowSelectionItem windowSelectionItem in this.m_Selection)
        num ^= windowSelectionItem.GetRefreshHash();
      return num;
    }

    public void Refresh()
    {
      this.ClearCache();
      foreach (AnimationWindowSelectionItem windowSelectionItem in this.m_Selection)
        windowSelectionItem.ClearCache();
    }

    public AnimationWindowSelectionItem[] ToArray()
    {
      return this.m_Selection.ToArray();
    }

    public void Clear()
    {
      if (this.m_Selection.Count <= 0)
        return;
      foreach (UnityEngine.Object @object in this.m_Selection)
        UnityEngine.Object.DestroyImmediate(@object);
      this.m_Selection.Clear();
      this.Notify();
    }

    public void ClearCache()
    {
      this.m_CurvesCache = (List<AnimationWindowCurve>) null;
    }

    public void Synchronize()
    {
      if (this.m_Selection.Count <= 0)
        return;
      foreach (AnimationWindowSelectionItem windowSelectionItem in this.m_Selection)
        windowSelectionItem.Synchronize();
    }
  }
}
