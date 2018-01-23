// Decompiled with JetBrains decompiler
// Type: UnityEditor.SearchableEditorWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
  public class SearchableEditorWindow : EditorWindow
  {
    private static List<SearchableEditorWindow> searchableWindows = new List<SearchableEditorWindow>();
    private static int s_SearchableEditorWindowSearchField = "SearchableEditorWindowSearchField".GetHashCode();
    internal HierarchyType m_HierarchyType = HierarchyType.Assets;
    internal string m_SearchFilter = "";
    internal SearchableEditorWindow.SearchMode m_SearchMode = SearchableEditorWindow.SearchMode.All;
    private bool m_FocusSearchField = false;
    private bool m_HasSearchFilterFocus = false;
    private int m_SearchFieldControlId;

    internal static SearchFilter CreateFilter(string searchString, SearchableEditorWindow.SearchMode searchMode)
    {
      SearchFilter filter = new SearchFilter();
      if (string.IsNullOrEmpty(searchString))
        return filter;
      switch (searchMode)
      {
        case SearchableEditorWindow.SearchMode.All:
          if (!SearchUtility.ParseSearchString(searchString, filter))
          {
            filter.nameFilter = searchString;
            filter.classNames = new string[1]
            {
              searchString
            };
            filter.assetLabels = new string[1]
            {
              searchString
            };
            filter.assetBundleNames = new string[1]
            {
              searchString
            };
            filter.showAllHits = true;
            break;
          }
          break;
        case SearchableEditorWindow.SearchMode.Name:
          filter.nameFilter = searchString;
          break;
        case SearchableEditorWindow.SearchMode.Type:
          filter.classNames = new string[1]{ searchString };
          break;
        case SearchableEditorWindow.SearchMode.Label:
          filter.assetLabels = new string[1]{ searchString };
          break;
        case SearchableEditorWindow.SearchMode.AssetBundleName:
          filter.assetBundleNames = new string[1]
          {
            searchString
          };
          break;
      }
      return filter;
    }

    [MenuItem("Assets/Find References In Scene", false, 25)]
    private static void OnSearchForReferences()
    {
      SearchableEditorWindow.SearchForReferencesToInstanceID(Selection.activeInstanceID);
    }

    [MenuItem("Assets/Find References In Scene", true)]
    private static bool OnSearchForReferencesValidate()
    {
      UnityEngine.Object activeObject = Selection.activeObject;
      if (activeObject != (UnityEngine.Object) null && AssetDatabase.Contains(activeObject))
        return !Directory.Exists(AssetDatabase.GetAssetPath(activeObject));
      return false;
    }

    public virtual void OnEnable()
    {
      SearchableEditorWindow.searchableWindows.Add(this);
    }

    public virtual void OnDisable()
    {
      SearchableEditorWindow.searchableWindows.Remove(this);
    }

    internal bool hasSearchFilter
    {
      get
      {
        return this.m_SearchFilter != "";
      }
    }

    internal bool hasSearchFilterFocus
    {
      get
      {
        return this.m_HasSearchFilterFocus;
      }
      set
      {
        this.m_HasSearchFilterFocus = value;
      }
    }

    internal SearchableEditorWindow.SearchMode searchMode
    {
      get
      {
        return this.m_SearchMode;
      }
      set
      {
        this.m_SearchMode = value;
      }
    }

    internal static void SearchForReferencesToInstanceID(int instanceID)
    {
      string str = AssetDatabase.GetAssetPath(instanceID).Substring(7);
      if (str.IndexOf(' ') != -1)
        str = 34.ToString() + str + (object) '"';
      string searchFilter;
      if (AssetDatabase.IsMainAsset(instanceID))
        searchFilter = "ref:" + str;
      else
        searchFilter = "ref:" + (object) instanceID + ":" + str;
      foreach (SearchableEditorWindow searchableWindow in SearchableEditorWindow.searchableWindows)
      {
        if (searchableWindow.m_HierarchyType == HierarchyType.GameObjects)
        {
          searchableWindow.SetSearchFilter(searchFilter, SearchableEditorWindow.SearchMode.All, false);
          searchableWindow.m_HasSearchFilterFocus = true;
          searchableWindow.Repaint();
        }
      }
    }

    internal void FocusSearchField()
    {
      this.m_FocusSearchField = true;
    }

    internal void ClearSearchFilter()
    {
      this.SetSearchFilter("", this.m_SearchMode, true);
      if (EditorGUI.s_RecycledEditor == null)
        return;
      EditorGUI.s_RecycledEditor.EndEditing();
    }

    internal void SelectPreviousSearchResult()
    {
      foreach (SearchableEditorWindow searchableWindow in SearchableEditorWindow.searchableWindows)
      {
        if (searchableWindow is SceneHierarchyWindow)
        {
          ((SceneHierarchyWindow) searchableWindow).SelectPrevious();
          break;
        }
      }
    }

    internal void SelectNextSearchResult()
    {
      foreach (SearchableEditorWindow searchableWindow in SearchableEditorWindow.searchableWindows)
      {
        if (searchableWindow is SceneHierarchyWindow)
        {
          ((SceneHierarchyWindow) searchableWindow).SelectNext();
          break;
        }
      }
    }

    internal virtual void SetSearchFilter(string searchFilter, SearchableEditorWindow.SearchMode mode, bool setAll)
    {
      this.m_SearchMode = mode;
      this.m_SearchFilter = searchFilter;
      if (setAll)
      {
        foreach (SearchableEditorWindow searchableWindow in SearchableEditorWindow.searchableWindows)
        {
          if ((UnityEngine.Object) searchableWindow != (UnityEngine.Object) this && searchableWindow.m_HierarchyType == this.m_HierarchyType && searchableWindow.m_HierarchyType != HierarchyType.Assets)
            searchableWindow.SetSearchFilter(this.m_SearchFilter, this.m_SearchMode, false);
        }
      }
      this.Repaint();
      EditorApplication.Internal_CallSearchHasChanged();
    }

    internal virtual void ClickedSearchField()
    {
    }

    internal void SearchFieldGUI()
    {
      this.SearchFieldGUI(EditorGUILayout.kLabelFloatMaxW * 1.5f);
    }

    internal void SearchFieldGUI(float maxWidth)
    {
      Rect rect = GUILayoutUtility.GetRect(EditorGUILayout.kLabelFloatMaxW * 0.2f, maxWidth, 16f, 16f, EditorStyles.toolbarSearchField);
      if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        this.ClickedSearchField();
      GUI.SetNextControlName("SearchFilter");
      if (this.m_FocusSearchField)
      {
        EditorGUI.FocusTextInControl("SearchFilter");
        if (Event.current.type == EventType.Repaint)
          this.m_FocusSearchField = false;
      }
      int searchMode = (int) this.m_SearchMode;
      if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape && GUI.GetNameOfFocusedControl() == "SearchFilter")
        this.SetSearchFilter("", (SearchableEditorWindow.SearchMode) searchMode, true);
      string[] names = Enum.GetNames(this.m_HierarchyType != HierarchyType.GameObjects ? typeof (SearchableEditorWindow.SearchMode) : typeof (SearchableEditorWindow.SearchModeHierarchyWindow));
      this.m_SearchFieldControlId = GUIUtility.GetControlID(SearchableEditorWindow.s_SearchableEditorWindowSearchField, FocusType.Keyboard, rect);
      EditorGUI.BeginChangeCheck();
      string searchFilter = EditorGUI.ToolbarSearchField(this.m_SearchFieldControlId, rect, names, ref searchMode, this.m_SearchFilter);
      if (EditorGUI.EndChangeCheck())
        this.SetSearchFilter(searchFilter, (SearchableEditorWindow.SearchMode) searchMode, true);
      this.m_HasSearchFilterFocus = GUIUtility.keyboardControl == this.m_SearchFieldControlId;
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Escape || (!(this.m_SearchFilter != "") || GUIUtility.hotControl != 0))
        return;
      this.m_SearchFilter = "";
      this.SetSearchFilter(searchFilter, (SearchableEditorWindow.SearchMode) searchMode, true);
      Event.current.Use();
      this.m_HasSearchFilterFocus = false;
    }

    public enum SearchMode
    {
      All,
      Name,
      Type,
      Label,
      AssetBundleName,
    }

    public enum SearchModeHierarchyWindow
    {
      All,
      Name,
      Type,
    }
  }
}
