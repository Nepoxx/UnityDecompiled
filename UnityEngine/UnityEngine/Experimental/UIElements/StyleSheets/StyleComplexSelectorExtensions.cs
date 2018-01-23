// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.StyleSheets.StyleComplexSelectorExtensions
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
  internal static class StyleComplexSelectorExtensions
  {
    private static Dictionary<string, StyleComplexSelectorExtensions.PseudoStateData> s_PseudoStates;

    public static void CachePseudoStateMasks(this StyleComplexSelector complexSelector)
    {
      if (complexSelector.selectors[0].pseudoStateMask != -1)
        return;
      if (StyleComplexSelectorExtensions.s_PseudoStates == null)
      {
        StyleComplexSelectorExtensions.s_PseudoStates = new Dictionary<string, StyleComplexSelectorExtensions.PseudoStateData>();
        StyleComplexSelectorExtensions.s_PseudoStates["active"] = new StyleComplexSelectorExtensions.PseudoStateData(PseudoStates.Active, false);
        StyleComplexSelectorExtensions.s_PseudoStates["hover"] = new StyleComplexSelectorExtensions.PseudoStateData(PseudoStates.Hover, false);
        StyleComplexSelectorExtensions.s_PseudoStates["checked"] = new StyleComplexSelectorExtensions.PseudoStateData(PseudoStates.Checked, false);
        StyleComplexSelectorExtensions.s_PseudoStates["selected"] = new StyleComplexSelectorExtensions.PseudoStateData(PseudoStates.Selected, false);
        StyleComplexSelectorExtensions.s_PseudoStates["disabled"] = new StyleComplexSelectorExtensions.PseudoStateData(PseudoStates.Disabled, false);
        StyleComplexSelectorExtensions.s_PseudoStates["focus"] = new StyleComplexSelectorExtensions.PseudoStateData(PseudoStates.Focus, false);
        StyleComplexSelectorExtensions.s_PseudoStates["inactive"] = new StyleComplexSelectorExtensions.PseudoStateData(PseudoStates.Active, true);
        StyleComplexSelectorExtensions.s_PseudoStates["enabled"] = new StyleComplexSelectorExtensions.PseudoStateData(PseudoStates.Disabled, true);
      }
      int index1 = 0;
      for (int length = complexSelector.selectors.Length; index1 < length; ++index1)
      {
        StyleSelector selector = complexSelector.selectors[index1];
        StyleSelectorPart[] parts = selector.parts;
        PseudoStates pseudoStates1 = (PseudoStates) 0;
        PseudoStates pseudoStates2 = (PseudoStates) 0;
        for (int index2 = 0; index2 < selector.parts.Length; ++index2)
        {
          if (selector.parts[index2].type == StyleSelectorType.PseudoClass)
          {
            StyleComplexSelectorExtensions.PseudoStateData pseudoStateData;
            if (StyleComplexSelectorExtensions.s_PseudoStates.TryGetValue(parts[index2].value, out pseudoStateData))
            {
              if (!pseudoStateData.negate)
                pseudoStates1 |= pseudoStateData.state;
              else
                pseudoStates2 |= pseudoStateData.state;
            }
            else
              Debug.LogWarningFormat("Unknown pseudo class \"{0}\"", (object) parts[index2].value);
          }
        }
        selector.pseudoStateMask = (int) pseudoStates1;
        selector.negatedPseudoStateMask = (int) pseudoStates2;
      }
    }

    private struct PseudoStateData
    {
      public readonly PseudoStates state;
      public readonly bool negate;

      public PseudoStateData(PseudoStates state, bool negate)
      {
        this.state = state;
        this.negate = negate;
      }
    }
  }
}
