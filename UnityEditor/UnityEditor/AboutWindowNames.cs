// Decompiled with JetBrains decompiler
// Type: UnityEditor.AboutWindowNames
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnityEditor
{
  internal static class AboutWindowNames
  {
    private static string s_Country = (string) null;
    private static string[] s_CachedNames = new string[0];
    public static List<AboutWindowNames.CreditEntry> s_Credits = new List<AboutWindowNames.CreditEntry>();
    private const int kChunkSize = 100;

    private static string CreditsFilePath
    {
      get
      {
        return Path.Combine(EditorApplication.applicationContentsPath, "Resources/credits.csv");
      }
    }

    public static string RemoveDiacritics(string text)
    {
      string str = text.Normalize(NormalizationForm.FormD);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (char ch in str)
      {
        if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
          stringBuilder.Append(ch);
      }
      return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    public static void ParseCredits()
    {
      using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("credits.csv"))
      {
        using (StreamReader streamReader = new StreamReader(manifestResourceStream))
        {
          string str;
          do
          {
            str = streamReader.ReadLine();
            if (str != null && str.Length > 0)
            {
              string[] strArray = str.Split(',');
              AboutWindowNames.CreditEntry creditEntry = new AboutWindowNames.CreditEntry() { name = strArray[0], normalizedName = AboutWindowNames.RemoveDiacritics(strArray[0]), alumni = strArray[1] == "1", country_code = strArray[2], region = strArray[3], twitter = strArray[4] };
              AboutWindowNames.s_Credits.Add(creditEntry);
            }
          }
          while (str != null);
        }
      }
    }

    public static string[] Names(string country_filter = null, bool chunked = false)
    {
      if (AboutWindowNames.s_Country == country_filter && AboutWindowNames.s_CachedNames.Length > 0)
        return AboutWindowNames.s_CachedNames;
      AboutWindowNames.s_Country = country_filter;
      List<string> source = new List<string>();
      foreach (AboutWindowNames.CreditEntry credit in AboutWindowNames.s_Credits)
      {
        if (string.IsNullOrEmpty(country_filter) || credit.country_code == country_filter)
          source.Add(credit.FormattedName);
      }
      if (!chunked)
      {
        AboutWindowNames.s_CachedNames = source.ToArray();
      }
      else
      {
        string[] strArray = new string[source.Count / 100 + 1];
        for (int index = 0; index * 100 < source.Count; ++index)
          strArray[index] = string.Join(", ", source.Skip<string>(index * 100).Take<string>(100).ToArray<string>());
        AboutWindowNames.s_CachedNames = ((IEnumerable<string>) strArray).ToArray<string>();
      }
      return AboutWindowNames.s_CachedNames;
    }

    public class CreditEntry
    {
      public string name;
      public string normalizedName;
      public string country_code;
      public string office;
      public string region;
      public string twitter;
      public string nationality;
      public string gravatar_hash;
      public bool alumni;

      public string FormattedName
      {
        get
        {
          string str = this.name;
          if (!string.IsNullOrEmpty(this.twitter))
            str = str + " ( @" + this.twitter + " )";
          return str;
        }
      }
    }
  }
}
