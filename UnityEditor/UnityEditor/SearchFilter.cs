// Decompiled with JetBrains decompiler
// Type: UnityEditor.SearchFilter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class SearchFilter
  {
    [SerializeField]
    private string m_NameFilter = "";
    [SerializeField]
    private string[] m_ClassNames = new string[0];
    [SerializeField]
    private string[] m_AssetLabels = new string[0];
    [SerializeField]
    private string[] m_AssetBundleNames = new string[0];
    [SerializeField]
    private string[] m_VersionControlStates = new string[0];
    [SerializeField]
    private string[] m_SoftLockControlStates = new string[0];
    [SerializeField]
    private int[] m_ReferencingInstanceIDs = new int[0];
    [SerializeField]
    private bool m_ShowAllHits = false;
    [SerializeField]
    private SearchFilter.SearchArea m_SearchArea = SearchFilter.SearchArea.AllAssets;
    [SerializeField]
    private string[] m_Folders = new string[0];
    [SerializeField]
    private string[] m_ScenePaths;

    public string nameFilter
    {
      get
      {
        return this.m_NameFilter;
      }
      set
      {
        this.m_NameFilter = value;
      }
    }

    public string[] classNames
    {
      get
      {
        return this.m_ClassNames;
      }
      set
      {
        this.m_ClassNames = value;
      }
    }

    public string[] assetLabels
    {
      get
      {
        return this.m_AssetLabels;
      }
      set
      {
        this.m_AssetLabels = value;
      }
    }

    public string[] versionControlStates
    {
      get
      {
        return this.m_VersionControlStates;
      }
      set
      {
        this.m_VersionControlStates = value;
      }
    }

    public string[] softLockControlStates
    {
      get
      {
        return this.m_SoftLockControlStates;
      }
      set
      {
        this.m_SoftLockControlStates = value;
      }
    }

    public string[] assetBundleNames
    {
      get
      {
        return this.m_AssetBundleNames;
      }
      set
      {
        this.m_AssetBundleNames = value;
      }
    }

    public int[] referencingInstanceIDs
    {
      get
      {
        return this.m_ReferencingInstanceIDs;
      }
      set
      {
        this.m_ReferencingInstanceIDs = value;
      }
    }

    public string[] scenePaths
    {
      get
      {
        return this.m_ScenePaths;
      }
      set
      {
        this.m_ScenePaths = value;
      }
    }

    public bool showAllHits
    {
      get
      {
        return this.m_ShowAllHits;
      }
      set
      {
        this.m_ShowAllHits = value;
      }
    }

    public string[] folders
    {
      get
      {
        return this.m_Folders;
      }
      set
      {
        this.m_Folders = value;
      }
    }

    public SearchFilter.SearchArea searchArea
    {
      get
      {
        return this.m_SearchArea;
      }
      set
      {
        this.m_SearchArea = value;
      }
    }

    public void ClearSearch()
    {
      this.m_NameFilter = "";
      this.m_ClassNames = new string[0];
      this.m_AssetLabels = new string[0];
      this.m_AssetBundleNames = new string[0];
      this.m_ReferencingInstanceIDs = new int[0];
      this.m_ScenePaths = new string[0];
      this.m_VersionControlStates = new string[0];
      this.m_SoftLockControlStates = new string[0];
      this.m_ShowAllHits = false;
    }

    private bool IsNullOrEmpty<T>(T[] list)
    {
      return list == null || list.Length == 0;
    }

    public SearchFilter.State GetState()
    {
      bool flag1 = !string.IsNullOrEmpty(this.m_NameFilter) || !this.IsNullOrEmpty<string>(this.m_AssetLabels) || (!this.IsNullOrEmpty<string>(this.m_ClassNames) || !this.IsNullOrEmpty<string>(this.m_AssetBundleNames)) || !this.IsNullOrEmpty<int>(this.m_ReferencingInstanceIDs) || !this.IsNullOrEmpty<string>(this.m_VersionControlStates) || !this.IsNullOrEmpty<string>(this.m_SoftLockControlStates);
      bool flag2 = !this.IsNullOrEmpty<string>(this.m_Folders);
      if (flag1)
      {
        if (this.m_SearchArea == SearchFilter.SearchArea.AssetStore)
          return SearchFilter.State.SearchingInAssetStore;
        return flag2 && this.m_SearchArea == SearchFilter.SearchArea.SelectedFolders ? SearchFilter.State.SearchingInFolders : SearchFilter.State.SearchingInAllAssets;
      }
      return flag2 ? SearchFilter.State.FolderBrowsing : SearchFilter.State.EmptySearchFilter;
    }

    public bool IsSearching()
    {
      SearchFilter.State state = this.GetState();
      int num;
      switch (state)
      {
        case SearchFilter.State.SearchingInAllAssets:
        case SearchFilter.State.SearchingInFolders:
          num = 1;
          break;
        default:
          num = state == SearchFilter.State.SearchingInAssetStore ? 1 : 0;
          break;
      }
      return num != 0;
    }

    public bool SetNewFilter(SearchFilter newFilter)
    {
      bool flag = false;
      if (newFilter.m_NameFilter != this.m_NameFilter)
      {
        this.m_NameFilter = newFilter.m_NameFilter;
        flag = true;
      }
      if (newFilter.m_ClassNames != this.m_ClassNames)
      {
        this.m_ClassNames = newFilter.m_ClassNames;
        flag = true;
      }
      if (newFilter.m_Folders != this.m_Folders)
      {
        this.m_Folders = newFilter.m_Folders;
        flag = true;
      }
      if (newFilter.m_VersionControlStates != this.m_VersionControlStates)
      {
        this.m_VersionControlStates = newFilter.m_VersionControlStates;
        flag = true;
      }
      if (newFilter.m_SoftLockControlStates != this.m_SoftLockControlStates)
      {
        this.m_SoftLockControlStates = newFilter.m_SoftLockControlStates;
        flag = true;
      }
      if (newFilter.m_AssetLabels != this.m_AssetLabels)
      {
        this.m_AssetLabels = newFilter.m_AssetLabels;
        flag = true;
      }
      if (newFilter.m_AssetBundleNames != this.m_AssetBundleNames)
      {
        this.m_AssetBundleNames = newFilter.m_AssetBundleNames;
        flag = true;
      }
      if (newFilter.m_ReferencingInstanceIDs != this.m_ReferencingInstanceIDs)
      {
        this.m_ReferencingInstanceIDs = newFilter.m_ReferencingInstanceIDs;
        flag = true;
      }
      if (newFilter.m_ScenePaths != this.m_ScenePaths)
      {
        this.m_ScenePaths = newFilter.m_ScenePaths;
        flag = true;
      }
      if (newFilter.m_SearchArea != this.m_SearchArea)
      {
        this.m_SearchArea = newFilter.m_SearchArea;
        flag = true;
      }
      this.m_ShowAllHits = newFilter.m_ShowAllHits;
      return flag;
    }

    public override string ToString()
    {
      string str = "SearchFilter: " + string.Format("[Area: {0}, State: {1}]", (object) this.m_SearchArea, (object) this.GetState());
      if (!string.IsNullOrEmpty(this.m_NameFilter))
        str = str + "[Name: " + this.m_NameFilter + "]";
      if (this.m_AssetLabels != null && this.m_AssetLabels.Length > 0)
        str = str + "[Labels: " + this.m_AssetLabels[0] + "]";
      if (this.m_VersionControlStates != null && this.m_VersionControlStates.Length > 0)
        str = str + "[VersionStates: " + this.m_VersionControlStates[0] + "]";
      if (this.m_SoftLockControlStates != null && this.m_SoftLockControlStates.Length > 0)
        str = str + "[SoftLockStates: " + this.m_SoftLockControlStates[0] + "]";
      if (this.m_AssetBundleNames != null && this.m_AssetBundleNames.Length > 0)
        str = str + "[AssetBundleNames: " + this.m_AssetBundleNames[0] + "]";
      if (this.m_ClassNames != null && this.m_ClassNames.Length > 0)
        str = str + "[Types: " + this.m_ClassNames[0] + " (" + (object) this.m_ClassNames.Length + ")]";
      if (this.m_ReferencingInstanceIDs != null && this.m_ReferencingInstanceIDs.Length > 0)
        str = str + "[RefIDs: " + (object) this.m_ReferencingInstanceIDs[0] + "]";
      if (this.m_Folders != null && this.m_Folders.Length > 0)
        str = str + "[Folders: " + this.m_Folders[0] + "]";
      return str + "[ShowAllHits: " + (object) this.showAllHits + "]";
    }

    internal string FilterToSearchFieldString()
    {
      string result = "";
      if (!string.IsNullOrEmpty(this.m_NameFilter))
        result += this.m_NameFilter;
      this.AddToString<string>("t:", this.m_ClassNames, ref result);
      this.AddToString<string>("l:", this.m_AssetLabels, ref result);
      this.AddToString<string>("v:", this.m_VersionControlStates, ref result);
      this.AddToString<string>("s:", this.m_SoftLockControlStates, ref result);
      this.AddToString<string>("b:", this.m_AssetBundleNames, ref result);
      return result;
    }

    private void AddToString<T>(string prefix, T[] list, ref string result)
    {
      if (list == null)
        return;
      if (result == null)
        result = "";
      foreach (T obj in list)
      {
        if (!string.IsNullOrEmpty(result))
          result += " ";
        result = result + prefix + (object) obj;
      }
    }

    internal void SearchFieldStringToFilter(string searchString)
    {
      this.ClearSearch();
      if (string.IsNullOrEmpty(searchString))
        return;
      SearchUtility.ParseSearchString(searchString, this);
    }

    internal static SearchFilter CreateSearchFilterFromString(string searchText)
    {
      SearchFilter filter = new SearchFilter();
      SearchUtility.ParseSearchString(searchText, filter);
      return filter;
    }

    public static string[] Split(string text)
    {
      if (string.IsNullOrEmpty(text))
        return new string[0];
      List<string> stringList = new List<string>();
      IEnumerator enumerator = Regex.Matches(text, "\".+?\"|\\S+").GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Match current = (Match) enumerator.Current;
          stringList.Add(current.Value.Replace("\"", ""));
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      return stringList.ToArray();
    }

    public enum SearchArea
    {
      AllAssets,
      SelectedFolders,
      AssetStore,
    }

    public enum State
    {
      EmptySearchFilter,
      FolderBrowsing,
      SearchingInAllAssets,
      SearchingInFolders,
      SearchingInAssetStore,
    }
  }
}
