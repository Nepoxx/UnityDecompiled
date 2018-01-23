// Decompiled with JetBrains decompiler
// Type: UnityEditor.StyleSheets.CSSSpec
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Text.RegularExpressions;
using UnityEngine.StyleSheets;

namespace UnityEditor.StyleSheets
{
  internal static class CSSSpec
  {
    private static readonly Regex rgx = new Regex("(?<id>#[-]?\\w[\\w-]*)|(?<class>\\.\\w+)|(?<pseudoclass>:[\\w-]+(\\((?<param>.+)\\))?)|(?<type>[^\\-]\\w+)|(?<wildcard>\\*)|\\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private const int typeSelectorWeight = 1;
    private const int classSelectorWeight = 10;
    private const int idSelectorWeight = 100;

    public static int GetSelectorSpecificity(string selector)
    {
      int num = 0;
      StyleSelectorPart[] parts;
      if (CSSSpec.ParseSelector(selector, out parts))
        num = CSSSpec.GetSelectorSpecificity(parts);
      return num;
    }

    public static int GetSelectorSpecificity(StyleSelectorPart[] parts)
    {
      int num = 1;
      for (int index = 0; index < parts.Length; ++index)
      {
        switch (parts[index].type)
        {
          case StyleSelectorType.Type:
            ++num;
            break;
          case StyleSelectorType.Class:
          case StyleSelectorType.PseudoClass:
            num += 10;
            break;
          case StyleSelectorType.RecursivePseudoClass:
            throw new ArgumentException("Recursive pseudo classes are not supported");
          case StyleSelectorType.ID:
            num += 100;
            break;
        }
      }
      return num;
    }

    public static bool ParseSelector(string selector, out StyleSelectorPart[] parts)
    {
      MatchCollection matchCollection = CSSSpec.rgx.Matches(selector);
      int count = matchCollection.Count;
      if (count < 1)
      {
        parts = (StyleSelectorPart[]) null;
        return false;
      }
      parts = new StyleSelectorPart[count];
      for (int index = 0; index < count; ++index)
      {
        Match match = matchCollection[index];
        StyleSelectorType styleSelectorType = StyleSelectorType.Unknown;
        string str1 = string.Empty;
        if (!string.IsNullOrEmpty(match.Groups["wildcard"].Value))
        {
          str1 = "*";
          styleSelectorType = StyleSelectorType.Wildcard;
        }
        else if (!string.IsNullOrEmpty(match.Groups["id"].Value))
        {
          str1 = match.Groups["id"].Value.Substring(1);
          styleSelectorType = StyleSelectorType.ID;
        }
        else if (!string.IsNullOrEmpty(match.Groups["class"].Value))
        {
          str1 = match.Groups["class"].Value.Substring(1);
          styleSelectorType = StyleSelectorType.Class;
        }
        else if (!string.IsNullOrEmpty(match.Groups["pseudoclass"].Value))
        {
          string str2 = match.Groups["param"].Value;
          if (!string.IsNullOrEmpty(str2))
          {
            str1 = str2;
            styleSelectorType = StyleSelectorType.RecursivePseudoClass;
          }
          else
          {
            str1 = match.Groups["pseudoclass"].Value.Substring(1);
            styleSelectorType = StyleSelectorType.PseudoClass;
          }
        }
        else if (!string.IsNullOrEmpty(match.Groups["type"].Value))
        {
          str1 = match.Groups["type"].Value;
          styleSelectorType = StyleSelectorType.Type;
        }
        parts[index] = new StyleSelectorPart()
        {
          type = styleSelectorType,
          value = str1
        };
      }
      return true;
    }
  }
}
