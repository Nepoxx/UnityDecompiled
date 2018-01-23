// Decompiled with JetBrains decompiler
// Type: UnityEngine.StyleSheets.StyleRule
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.StyleSheets
{
  [Serializable]
  internal class StyleRule
  {
    [SerializeField]
    private StyleProperty[] m_Properties;
    [SerializeField]
    internal int line;

    public StyleProperty[] properties
    {
      get
      {
        return this.m_Properties;
      }
      internal set
      {
        this.m_Properties = value;
      }
    }
  }
}
