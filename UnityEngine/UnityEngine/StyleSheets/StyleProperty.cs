// Decompiled with JetBrains decompiler
// Type: UnityEngine.StyleSheets.StyleProperty
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.StyleSheets
{
  [Serializable]
  internal class StyleProperty
  {
    [SerializeField]
    private string m_Name;
    [SerializeField]
    private StyleValueHandle[] m_Values;

    public string name
    {
      get
      {
        return this.m_Name;
      }
      internal set
      {
        this.m_Name = value;
      }
    }

    public StyleValueHandle[] values
    {
      get
      {
        return this.m_Values;
      }
      internal set
      {
        this.m_Values = value;
      }
    }
  }
}
