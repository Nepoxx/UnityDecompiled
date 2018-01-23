// Decompiled with JetBrains decompiler
// Type: UnityEditor.PrefKey
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class PrefKey : IPrefType
  {
    private bool m_Loaded;
    private string m_name;
    private Event m_event;
    private string m_Shortcut;
    private string m_DefaultShortcut;

    public PrefKey()
    {
      this.m_Loaded = true;
    }

    public PrefKey(string name, string shortcut)
    {
      this.m_name = name;
      this.m_Shortcut = shortcut;
      this.m_DefaultShortcut = shortcut;
      Settings.Add((IPrefType) this);
      this.m_Loaded = false;
    }

    public void Load()
    {
      if (this.m_Loaded)
        return;
      this.m_Loaded = true;
      this.m_event = Event.KeyboardEvent(this.m_Shortcut);
      PrefKey prefKey = Settings.Get<PrefKey>(this.m_name, this);
      this.m_name = prefKey.Name;
      this.m_event = prefKey.KeyboardEvent;
    }

    public static implicit operator Event(PrefKey pkey)
    {
      pkey.Load();
      return pkey.m_event;
    }

    public string Name
    {
      get
      {
        this.Load();
        return this.m_name;
      }
    }

    public Event KeyboardEvent
    {
      get
      {
        this.Load();
        return this.m_event;
      }
      set
      {
        this.Load();
        this.m_event = value;
      }
    }

    public string ToUniqueString()
    {
      this.Load();
      return this.m_name + ";" + (!this.m_event.alt ? (object) "" : (object) "&") + (!this.m_event.command ? (object) "" : (object) "%") + (!this.m_event.shift ? (object) "" : (object) "#") + (!this.m_event.control ? (object) "" : (object) "^") + (object) this.m_event.keyCode;
    }

    public bool activated
    {
      get
      {
        this.Load();
        return Event.current.Equals((object) (Event) this) && !GUIUtility.textFieldInput;
      }
    }

    public void FromUniqueString(string s)
    {
      this.Load();
      int length = s.IndexOf(";");
      if (length < 0)
      {
        Debug.LogError((object) "Malformed string in Keyboard preferences");
      }
      else
      {
        this.m_name = s.Substring(0, length);
        this.m_event = Event.KeyboardEvent(s.Substring(length + 1));
      }
    }

    internal void ResetToDefault()
    {
      this.Load();
      this.m_event = Event.KeyboardEvent(this.m_DefaultShortcut);
    }
  }
}
