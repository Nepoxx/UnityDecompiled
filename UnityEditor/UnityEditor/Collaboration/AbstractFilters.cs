// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.AbstractFilters
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Collaboration
{
  internal abstract class AbstractFilters
  {
    [SerializeField]
    private List<string[]> m_Filters;

    public List<string[]> filters
    {
      get
      {
        return this.m_Filters;
      }
      set
      {
        this.m_Filters = value;
      }
    }

    public abstract void InitializeFilters();

    public bool ContainsSearchFilter(string name, string searchString)
    {
      foreach (string[] filter in this.filters)
      {
        if (filter[0] == name && filter[1] == searchString)
          return true;
      }
      return false;
    }

    public void ShowInFavoriteSearchFilters()
    {
      if (SavedSearchFilters.GetRootInstanceID() == 0)
      {
        SavedSearchFilters.AddInitializedListener(new Action(this.ShowInFavoriteSearchFilters));
      }
      else
      {
        SavedSearchFilters.RemoveInitializedListener(new Action(this.ShowInFavoriteSearchFilters));
        int insertAfterID = 0;
        foreach (string[] filter in this.filters)
        {
          if (SavedSearchFilters.GetFilterInstanceID(filter[0], filter[1]) == 0)
          {
            SearchFilter filterFromString = SearchFilter.CreateSearchFilterFromString(filter[1]);
            insertAfterID = insertAfterID != 0 ? SavedSearchFilters.AddSavedFilterAfterInstanceID(filter[0], filterFromString, 64f, insertAfterID, false) : SavedSearchFilters.AddSavedFilter(filter[0], filterFromString, 64f);
          }
        }
        SavedSearchFilters.RefreshSavedFilters();
        foreach (EditorWindow allProjectBrowser in ProjectBrowser.GetAllProjectBrowsers())
          allProjectBrowser.Repaint();
      }
    }

    public void HideFromFavoriteSearchFilters()
    {
      SavedSearchFilters.RefreshSavedFilters();
      foreach (EditorWindow allProjectBrowser in ProjectBrowser.GetAllProjectBrowsers())
        allProjectBrowser.Repaint();
    }
  }
}
