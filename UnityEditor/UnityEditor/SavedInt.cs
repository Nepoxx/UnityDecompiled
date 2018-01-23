// Decompiled with JetBrains decompiler
// Type: UnityEditor.SavedInt
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  internal class SavedInt
  {
    private int m_Value;
    private string m_Name;
    private bool m_Loaded;

    public SavedInt(string name, int value)
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
      this.m_Value = EditorPrefs.GetInt(this.m_Name, this.m_Value);
    }

    public int value
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
        EditorPrefs.SetInt(this.m_Name, value);
      }
    }

    public static implicit operator int(SavedInt s)
    {
      return s.value;
    }
  }
}
