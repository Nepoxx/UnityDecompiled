// Decompiled with JetBrains decompiler
// Type: UnityEditor.SearchUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class SearchUtility
  {
    private static void RemoveUnwantedWhitespaces(ref string searchString)
    {
      searchString = searchString.Replace(": ", ":");
    }

    internal static bool ParseSearchString(string searchText, SearchFilter filter)
    {
      if (string.IsNullOrEmpty(searchText))
        return false;
      filter.ClearSearch();
      string searchString1 = string.Copy(searchText);
      SearchUtility.RemoveUnwantedWhitespaces(ref searchString1);
      bool flag = false;
      int startIndex = SearchUtility.FindFirstPositionNotOf(searchString1, " \t,*?");
      if (startIndex == -1)
        startIndex = 0;
      int num1;
      for (; startIndex < searchString1.Length; startIndex = num1 + 1)
      {
        num1 = searchString1.IndexOfAny(" \t,*?".ToCharArray(), startIndex);
        int quote1 = searchString1.IndexOf('"', startIndex);
        int num2 = -1;
        if (quote1 != -1)
        {
          num2 = searchString1.IndexOf('"', quote1 + 1);
          num1 = num2 == -1 ? -1 : searchString1.IndexOfAny(" \t,*?".ToCharArray(), num2);
        }
        if (num1 == -1)
          num1 = searchString1.Length;
        if (num1 > startIndex)
        {
          string searchString2 = searchString1.Substring(startIndex, num1 - startIndex);
          if (SearchUtility.CheckForKeyWords(searchString2, filter, quote1, num2))
          {
            flag = true;
          }
          else
          {
            SearchFilter searchFilter = filter;
            searchFilter.nameFilter = searchFilter.nameFilter + (!string.IsNullOrEmpty(filter.nameFilter) ? " " : "") + searchString2;
          }
        }
      }
      return flag;
    }

    internal static bool CheckForKeyWords(string searchString, SearchFilter filter, int quote1, int quote2)
    {
      bool flag = false;
      int num1 = searchString.IndexOf("t:");
      if (num1 == 0)
      {
        string str = searchString.Substring(num1 + 2);
        filter.classNames = new List<string>((IEnumerable<string>) filter.classNames)
        {
          str
        }.ToArray();
        flag = true;
      }
      int num2 = searchString.IndexOf("l:");
      if (num2 == 0)
      {
        string str = searchString.Substring(num2 + 2);
        filter.assetLabels = new List<string>((IEnumerable<string>) filter.assetLabels)
        {
          str
        }.ToArray();
        flag = true;
      }
      int num3 = searchString.IndexOf("v:");
      if (num3 >= 0)
      {
        string str = searchString.Substring(num3 + 2);
        filter.versionControlStates = new List<string>((IEnumerable<string>) filter.versionControlStates)
        {
          str
        }.ToArray();
        flag = true;
      }
      int num4 = searchString.IndexOf("s:");
      if (num4 >= 0)
      {
        string str = searchString.Substring(num4 + 2);
        filter.softLockControlStates = new List<string>((IEnumerable<string>) filter.softLockControlStates)
        {
          str
        }.ToArray();
        flag = true;
      }
      int num5 = searchString.IndexOf("b:");
      if (num5 == 0)
      {
        string str = searchString.Substring(num5 + 2);
        filter.assetBundleNames = new List<string>((IEnumerable<string>) filter.assetBundleNames)
        {
          str
        }.ToArray();
        flag = true;
      }
      int num6 = searchString.IndexOf("ref:");
      if (num6 == 0)
      {
        int num7 = 0;
        int num8 = num6 + 3;
        int num9 = searchString.IndexOf(':', num8 + 1);
        if (num9 >= 0)
        {
          int result;
          if (int.TryParse(searchString.Substring(num8 + 1, num9 - num8 - 1), out result))
            num7 = result;
        }
        else
        {
          string assetPath;
          if (quote1 != -1 && quote2 != -1)
          {
            int startIndex = quote1 + 1;
            int length = quote2 - quote1 - 1;
            if (length < 0 || quote2 == -1)
              length = searchString.Length - startIndex;
            assetPath = "Assets/" + searchString.Substring(startIndex, length);
          }
          else
            assetPath = "Assets/" + searchString.Substring(num8 + 1);
          Object @object = AssetDatabase.LoadMainAssetAtPath(assetPath);
          if (@object != (Object) null)
            num7 = @object.GetInstanceID();
        }
        filter.referencingInstanceIDs = new int[1]{ num7 };
        flag = true;
      }
      return flag;
    }

    private static int FindFirstPositionNotOf(string source, string chars)
    {
      if (source == null)
        return -1;
      if (chars == null)
        return 0;
      if (source.Length == 0)
        return -1;
      if (chars.Length == 0)
        return 0;
      for (int index = 0; index < source.Length; ++index)
      {
        if (chars.IndexOf(source[index]) == -1)
          return index;
      }
      return -1;
    }
  }
}
