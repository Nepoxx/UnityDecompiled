// Decompiled with JetBrains decompiler
// Type: UnityEngine.StyleSheets.StyleComplexSelector
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.StyleSheets
{
  [Serializable]
  internal class StyleComplexSelector
  {
    [SerializeField]
    private int m_Specificity;
    [SerializeField]
    private StyleSelector[] m_Selectors;
    [SerializeField]
    internal int ruleIndex;

    public int specificity
    {
      get
      {
        return this.m_Specificity;
      }
      internal set
      {
        this.m_Specificity = value;
      }
    }

    public StyleRule rule { get; internal set; }

    public bool isSimple
    {
      get
      {
        return this.selectors.Length == 1;
      }
    }

    public StyleSelector[] selectors
    {
      get
      {
        return this.m_Selectors;
      }
      internal set
      {
        this.m_Selectors = value;
      }
    }
  }
}
