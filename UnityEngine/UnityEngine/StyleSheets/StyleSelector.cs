// Decompiled with JetBrains decompiler
// Type: UnityEngine.StyleSheets.StyleSelector
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.StyleSheets
{
  [Serializable]
  internal class StyleSelector
  {
    internal int pseudoStateMask = -1;
    internal int negatedPseudoStateMask = -1;
    [SerializeField]
    private StyleSelectorPart[] m_Parts;
    [SerializeField]
    private StyleSelectorRelationship m_PreviousRelationship;

    public StyleSelectorPart[] parts
    {
      get
      {
        return this.m_Parts;
      }
      internal set
      {
        this.m_Parts = value;
      }
    }

    public StyleSelectorRelationship previousRelationship
    {
      get
      {
        return this.m_PreviousRelationship;
      }
      internal set
      {
        this.m_PreviousRelationship = value;
      }
    }

    public override string ToString()
    {
      return string.Join(", ", ((IEnumerable<StyleSelectorPart>) this.parts).Select<StyleSelectorPart, string>((Func<StyleSelectorPart, string>) (p => p.ToString())).ToArray<string>());
    }
  }
}
