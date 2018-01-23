// Decompiled with JetBrains decompiler
// Type: UnityEditor.SavedBool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  internal class SavedBool
  {
    private bool m_Value;
    private string m_Name;
    private bool m_Loaded;

    public SavedBool(string name, bool value)
    {
      this.m_Name = name;
      this.m_Loaded = false;
      this.m_Value = value;
    }

    private void Load()
    {
      if (this.m_Loaded)
        return;
      this.m_Loaded = true;
      this.m_Value = EditorPrefs.GetBool(this.m_Name, this.m_Value);
    }

    public bool value
    {
      get
      {
        this.Load();
        return this.m_Value;
      }
      set
      {
        this.Load();
        if (this.m_Value == value)
          return;
        this.m_Value = value;
        EditorPrefs.SetBool(this.m_Name, value);
      }
    }

    public static implicit operator bool(SavedBool s)
    {
      return s.value;
    }
  }
}
