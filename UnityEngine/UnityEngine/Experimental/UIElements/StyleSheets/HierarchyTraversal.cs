// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.StyleSheets.HierarchyTraversal
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
  internal abstract class HierarchyTraversal
  {
    public abstract bool ShouldSkipElement(VisualElement element);

    public abstract bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element);

    public virtual void OnBeginElementTest(VisualElement element, List<RuleMatcher> ruleMatchers)
    {
    }

    public void BeginElementTest(VisualElement element, List<RuleMatcher> ruleMatchers)
    {
      this.OnBeginElementTest(element, ruleMatchers);
    }

    public virtual void ProcessMatchedRules(VisualElement element)
    {
    }

    internal void Traverse(VisualElement element, int depth, List<RuleMatcher> ruleMatchers)
    {
      if (this.ShouldSkipElement(element))
        return;
      int count1 = ruleMatchers.Count;
      this.BeginElementTest(element, ruleMatchers);
      int count2 = ruleMatchers.Count;
      for (int index1 = 0; index1 < count2; ++index1)
      {
        RuleMatcher ruleMatcher1 = ruleMatchers[index1];
        if (ruleMatcher1.depth >= depth && this.Match(element, ref ruleMatcher1))
        {
          StyleSelector[] selectors = ruleMatcher1.complexSelector.selectors;
          int index2 = ruleMatcher1.simpleSelectorIndex + 1;
          int length = selectors.Length;
          if (index2 < length)
          {
            RuleMatcher ruleMatcher2 = new RuleMatcher() { complexSelector = ruleMatcher1.complexSelector, depth = selectors[index2].previousRelationship != StyleSelectorRelationship.Child ? int.MaxValue : depth + 1, simpleSelectorIndex = index2, sheet = ruleMatcher1.sheet };
            ruleMatchers.Add(ruleMatcher2);
          }
          else if (this.OnRuleMatchedElement(ruleMatcher1, element))
            return;
        }
      }
      this.ProcessMatchedRules(element);
      this.Recurse(element, depth, ruleMatchers);
      if (ruleMatchers.Count <= count1)
        return;
      ruleMatchers.RemoveRange(count1, ruleMatchers.Count - count1);
    }

    protected virtual void Recurse(VisualElement element, int depth, List<RuleMatcher> ruleMatchers)
    {
      for (int index = 0; index < element.shadow.childCount; ++index)
        this.Traverse(element.shadow[index], depth + 1, ruleMatchers);
    }

    protected virtual bool MatchSelectorPart(VisualElement element, StyleSelector selector, StyleSelectorPart part)
    {
      bool flag = true;
      switch (part.type)
      {
        case StyleSelectorType.Wildcard:
          return flag;
        case StyleSelectorType.Type:
          flag = element.typeName == part.value;
          goto case StyleSelectorType.Wildcard;
        case StyleSelectorType.Class:
          flag = element.ClassListContains(part.value);
          goto case StyleSelectorType.Wildcard;
        case StyleSelectorType.PseudoClass:
          int pseudoStates = (int) element.pseudoStates;
          flag = (selector.pseudoStateMask & pseudoStates) == selector.pseudoStateMask & (selector.negatedPseudoStateMask & ~pseudoStates) == selector.negatedPseudoStateMask;
          goto case StyleSelectorType.Wildcard;
        case StyleSelectorType.ID:
          flag = element.name == part.value;
          goto case StyleSelectorType.Wildcard;
        default:
          flag = false;
          goto case StyleSelectorType.Wildcard;
      }
    }

    public virtual bool Match(VisualElement element, ref RuleMatcher matcher)
    {
      bool flag = true;
      StyleSelector selector = matcher.complexSelector.selectors[matcher.simpleSelectorIndex];
      int length = selector.parts.Length;
      for (int index = 0; index < length && flag; ++index)
        flag = this.MatchSelectorPart(element, selector, selector.parts[index]);
      return flag;
    }
  }
}
