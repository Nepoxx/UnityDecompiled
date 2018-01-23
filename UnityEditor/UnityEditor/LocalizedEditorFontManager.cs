// Decompiled with JetBrains decompiler
// Type: UnityEditor.LocalizedEditorFontManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityEditor
{
  internal class LocalizedEditorFontManager
  {
    private static Dictionary<SystemLanguage, LocalizedEditorFontManager.FontDictionary> m_fontDictionaries;

    private static LocalizedEditorFontManager.FontDictionary GetFontDictionary(SystemLanguage language)
    {
      if (!LocalizedEditorFontManager.m_fontDictionaries.ContainsKey(language))
        return (LocalizedEditorFontManager.FontDictionary) null;
      return LocalizedEditorFontManager.m_fontDictionaries[language];
    }

    private static void ReadFontSettings()
    {
      if (LocalizedEditorFontManager.m_fontDictionaries == null)
        LocalizedEditorFontManager.m_fontDictionaries = new Dictionary<SystemLanguage, LocalizedEditorFontManager.FontDictionary>();
      LocalizedEditorFontManager.m_fontDictionaries.Clear();
      string path = (string) null;
      if (path == null || !File.Exists(path))
        path = LocalizationDatabase.GetLocalizationResourceFolder() + "/fontsettings.txt";
      if (!File.Exists(path))
        return;
      string str1 = Encoding.UTF8.GetString(File.ReadAllBytes(path));
      char[] chArray1 = new char[1]{ '\n' };
      foreach (string str2 in str1.Split(chArray1))
      {
        char[] chArray2 = new char[1]{ '#' };
        string str3 = str2.Split(chArray2)[0].Trim();
        if (str3.Length > 0)
        {
          string[] strArray1 = str3.Split('|');
          if (strArray1.Length != 2)
          {
            Debug.LogError((object) "wrong format for the fontsettings.txt.");
          }
          else
          {
            SystemLanguage key1 = (SystemLanguage) Enum.Parse(typeof (SystemLanguage), strArray1[0].Trim());
            string[] strArray2 = strArray1[1].Split('=');
            if (strArray2.Length != 2)
            {
              Debug.LogError((object) "wrong format for the fontsettings.txt.");
            }
            else
            {
              string key2 = strArray2[0].Trim();
              string[] fontNames = strArray2[1].Split(',');
              for (int index = 0; index < fontNames.Length; ++index)
                fontNames[index] = fontNames[index].Trim();
              if (!LocalizedEditorFontManager.m_fontDictionaries.ContainsKey(key1))
                LocalizedEditorFontManager.m_fontDictionaries.Add(key1, new LocalizedEditorFontManager.FontDictionary());
              LocalizedEditorFontManager.m_fontDictionaries[key1].Add(key2, new LocalizedEditorFontManager.FontSetting(fontNames));
            }
          }
        }
      }
    }

    private static void ModifyFont(Font font, LocalizedEditorFontManager.FontDictionary dict)
    {
      string key = font.ToString();
      if (dict.ContainsKey(key))
        font.fontNames = dict[key].fontNames;
      else
        Debug.LogError((object) ("no matching for:" + key));
    }

    private static void UpdateSkinFontInternal(GUISkin skin, LocalizedEditorFontManager.FontDictionary dict)
    {
      if ((UnityEngine.Object) skin == (UnityEngine.Object) null)
        return;
      foreach (object obj in skin)
      {
        GUIStyle guiStyle = obj as GUIStyle;
        if (guiStyle != null && (UnityEngine.Object) guiStyle.font != (UnityEngine.Object) null)
          LocalizedEditorFontManager.ModifyFont(guiStyle.font, dict);
      }
    }

    public static void UpdateSkinFont(SystemLanguage language)
    {
      LocalizedEditorFontManager.ReadFontSettings();
      LocalizedEditorFontManager.FontDictionary fontDictionary = LocalizedEditorFontManager.GetFontDictionary(language);
      if (fontDictionary == null)
        return;
      LocalizedEditorFontManager.UpdateSkinFontInternal(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector), fontDictionary);
      LocalizedEditorFontManager.UpdateSkinFontInternal(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene), fontDictionary);
    }

    public static void UpdateSkinFont()
    {
      LocalizedEditorFontManager.UpdateSkinFont(LocalizationDatabase.GetCurrentEditorLanguage());
    }

    private static void OnBoot()
    {
      LocalizedEditorFontManager.UpdateSkinFont();
    }

    private class FontSetting
    {
      private string[] m_fontNames;

      public FontSetting(string[] fontNames)
      {
        this.m_fontNames = fontNames;
      }

      public string[] fontNames
      {
        get
        {
          return this.m_fontNames;
        }
      }
    }

    private class FontDictionary
    {
      private Dictionary<string, LocalizedEditorFontManager.FontSetting> m_dictionary;

      public FontDictionary()
      {
        this.m_dictionary = new Dictionary<string, LocalizedEditorFontManager.FontSetting>();
      }

      public void Add(string key, LocalizedEditorFontManager.FontSetting value)
      {
        this.m_dictionary.Add(key, value);
      }

      private string trim_key(string key)
      {
        int length = key.IndexOf(" (UnityEngine.Font)");
        return key.Substring(0, length);
      }

      public bool ContainsKey(string key)
      {
        return this.m_dictionary.ContainsKey(this.trim_key(key));
      }

      public LocalizedEditorFontManager.FontSetting this[string key]
      {
        get
        {
          return this.m_dictionary[this.trim_key(key)];
        }
      }
    }
  }
}
