// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.CollabFilters
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Collaboration
{
  internal class CollabFilters : AbstractFilters
  {
    [SerializeField]
    private bool m_SearchFilterWasSet = false;

    public CollabFilters()
    {
      this.InitializeFilters();
    }

    public override void InitializeFilters()
    {
      this.filters = new List<string[]>()
      {
        new string[2]{ "All Modified", "v:any" },
        new string[2]{ "All Conflicts", "v:conflicted" },
        new string[2]{ "All Excluded", "v:ignored" }
      };
    }

    public void ShowInProjectBrowser(string filterString)
    {
      ProjectBrowser projectBrowser = ProjectBrowser.s_LastInteractedProjectBrowser;
      if ((Object) projectBrowser == (Object) null)
      {
        List<ProjectBrowser> allProjectBrowsers = ProjectBrowser.GetAllProjectBrowsers();
        if (allProjectBrowsers != null && allProjectBrowsers.Count > 0)
          projectBrowser = allProjectBrowsers.First<ProjectBrowser>();
      }
      if (!string.IsNullOrEmpty(filterString))
      {
        if ((Object) projectBrowser == (Object) null)
        {
          projectBrowser = EditorWindow.GetWindow<ProjectBrowser>();
          this.ShowInFavoriteSearchFilters();
          projectBrowser.RepaintImmediately();
        }
        this.m_SearchFilterWasSet = true;
        string str = "v:" + filterString;
        if (projectBrowser.IsTwoColumns())
        {
          foreach (string[] filter in this.filters)
          {
            if (str == filter[1])
            {
              int filterInstanceId = SavedSearchFilters.GetFilterInstanceID(filter[0], str);
              if (filterInstanceId > ProjectWindowUtil.k_FavoritesStartInstanceID)
              {
                projectBrowser.SetFolderSelection(new int[1]
                {
                  filterInstanceId
                }, true);
                break;
              }
            }
          }
        }
        projectBrowser.SetSearch(str);
        projectBrowser.Repaint();
        projectBrowser.Focus();
      }
      else
      {
        if (this.m_SearchFilterWasSet && (Object) projectBrowser != (Object) null)
        {
          if (projectBrowser.IsTwoColumns())
          {
            int mainAssetInstanceId = AssetDatabase.GetMainAssetInstanceID("assets");
            projectBrowser.SetFolderSelection(new int[1]
            {
              mainAssetInstanceId
            }, true);
          }
          projectBrowser.SetSearch("");
          projectBrowser.Repaint();
        }
        this.m_SearchFilterWasSet = false;
      }
    }

    public void OnCollabStateChanged(CollabInfo info)
    {
      if (!info.ready || info.inProgress || info.maintenance)
        return;
      foreach (ProjectBrowser allProjectBrowser in ProjectBrowser.GetAllProjectBrowsers())
        allProjectBrowser.RefreshSearchIfFilterContains("v:");
    }
  }
}
