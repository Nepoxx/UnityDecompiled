// Decompiled with JetBrains decompiler
// Type: UnityEditor.Settings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UnityEditor
{
  internal class Settings
  {
    private static List<IPrefType> m_AddedPrefs = new List<IPrefType>();
    private static SortedList<string, object> m_Prefs = new SortedList<string, object>();

    internal static void Add(IPrefType value)
    {
      Settings.m_AddedPrefs.Add(value);
    }

    internal static T Get<T>(string name, T defaultValue) where T : IPrefType, new()
    {
      Settings.Load();
      if ((object) defaultValue == null)
        throw new ArgumentException("default can not be null", nameof (defaultValue));
      if (Settings.m_Prefs.ContainsKey(name))
        return (T) Settings.m_Prefs[name];
      string sstr = EditorPrefs.GetString(name, "");
      if (sstr == "")
      {
        Settings.Set<T>(name, defaultValue);
        return defaultValue;
      }
      defaultValue.FromUniqueString(sstr);
      Settings.Set<T>(name, defaultValue);
      return defaultValue;
    }

    internal static void Set<T>(string name, T value) where T : IPrefType
    {
      Settings.Load();
      EditorPrefs.SetString(name, value.ToUniqueString());
      Settings.m_Prefs[name] = (object) value;
    }

    [DebuggerHidden]
    internal static IEnumerable<KeyValuePair<string, T>> Prefs<T>() where T : IPrefType
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Settings.\u003CPrefs\u003Ec__Iterator0<T> prefsCIterator0_1 = new Settings.\u003CPrefs\u003Ec__Iterator0<T>();
      // ISSUE: variable of a compiler-generated type
      Settings.\u003CPrefs\u003Ec__Iterator0<T> prefsCIterator0_2 = prefsCIterator0_1;
      // ISSUE: reference to a compiler-generated field
      prefsCIterator0_2.\u0024PC = -2;
      return (IEnumerable<KeyValuePair<string, T>>) prefsCIterator0_2;
    }

    private static void Load()
    {
      if (!Settings.m_AddedPrefs.Any<IPrefType>())
        return;
      List<IPrefType> prefTypeList = new List<IPrefType>((IEnumerable<IPrefType>) Settings.m_AddedPrefs);
      Settings.m_AddedPrefs.Clear();
      foreach (IPrefType prefType in prefTypeList)
        prefType.Load();
    }
  }
}
