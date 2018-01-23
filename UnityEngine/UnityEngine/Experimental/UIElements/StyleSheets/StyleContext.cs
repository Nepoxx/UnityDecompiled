// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.StyleSheets.StyleContext
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
  internal class StyleContext
  {
    private static Dictionary<long, VisualElementStylesData> s_StyleCache = new Dictionary<long, VisualElementStylesData>();
    private static StyleContext.StyleContextHierarchyTraversal s_StyleContextHierarchyTraversal = new StyleContext.StyleContextHierarchyTraversal();
    private List<RuleMatcher> m_Matchers;
    private VisualElement m_VisualTree;

    public StyleContext(VisualElement tree)
    {
      this.m_VisualTree = tree;
      this.m_Matchers = new List<RuleMatcher>(0);
    }

    public float currentPixelsPerPoint { get; set; }

    public void DirtyStyleSheets()
    {
      StyleContext.PropagateDirtyStyleSheets(this.m_VisualTree);
    }

    public void ApplyStyles()
    {
      Debug.Assert(this.m_VisualTree.panel != null);
      StyleContext.s_StyleContextHierarchyTraversal.currentPixelsPerPoint = this.currentPixelsPerPoint;
      StyleContext.s_StyleContextHierarchyTraversal.Traverse(this.m_VisualTree, 0, this.m_Matchers);
      this.m_Matchers.Clear();
    }

    private static void PropagateDirtyStyleSheets(VisualElement element)
    {
      if (element == null)
        return;
      if (element.styleSheets != null)
        element.LoadStyleSheetsFromPaths();
      foreach (VisualElement child in element.shadow.Children())
        StyleContext.PropagateDirtyStyleSheets(child);
    }

    public static void ClearStyleCache()
    {
      StyleContext.s_StyleCache.Clear();
    }

    private struct RuleRef
    {
      public StyleComplexSelector selector;
      public StyleSheet sheet;
    }

    private class StyleContextHierarchyTraversal : HierarchyTraversal
    {
      private List<StyleContext.RuleRef> m_MatchedRules = new List<StyleContext.RuleRef>(0);
      private long m_MatchingRulesHash;

      public float currentPixelsPerPoint { get; set; }

      public override bool ShouldSkipElement(VisualElement element)
      {
        return !element.IsDirty(ChangeType.Styles) && !element.IsDirty(ChangeType.StylesPath);
      }

      public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
      {
        StyleRule rule = matcher.complexSelector.rule;
        int specificity = matcher.complexSelector.specificity;
        this.m_MatchingRulesHash = this.m_MatchingRulesHash * 397L ^ (long) rule.GetHashCode();
        this.m_MatchingRulesHash = this.m_MatchingRulesHash * 397L ^ (long) specificity;
        this.m_MatchedRules.Add(new StyleContext.RuleRef()
        {
          selector = matcher.complexSelector,
          sheet = matcher.sheet
        });
        return false;
      }

      public override void OnBeginElementTest(VisualElement element, List<RuleMatcher> ruleMatchers)
      {
        if (element != null && element.styleSheets != null)
        {
          foreach (StyleSheet styleSheet in element.styleSheets)
          {
            StyleComplexSelector[] complexSelectors = styleSheet.complexSelectors;
            int val2 = ruleMatchers.Count + complexSelectors.Length;
            ruleMatchers.Capacity = Math.Max(ruleMatchers.Capacity, val2);
            for (int index = 0; index < complexSelectors.Length; ++index)
            {
              StyleComplexSelector styleComplexSelector = complexSelectors[index];
              ruleMatchers.Add(new RuleMatcher()
              {
                sheet = styleSheet,
                complexSelector = styleComplexSelector,
                simpleSelectorIndex = 0,
                depth = int.MaxValue
              });
            }
          }
        }
        this.m_MatchedRules.Clear();
        this.m_MatchingRulesHash = (long) element.fullTypeName.GetHashCode() * 397L ^ (long) this.currentPixelsPerPoint.GetHashCode();
      }

      public override void ProcessMatchedRules(VisualElement element)
      {
        VisualElementStylesData sharedStyle1;
        if (StyleContext.s_StyleCache.TryGetValue(this.m_MatchingRulesHash, out sharedStyle1))
        {
          element.SetSharedStyles(sharedStyle1);
        }
        else
        {
          VisualElementStylesData sharedStyle2 = new VisualElementStylesData(true);
          int index = 0;
          for (int count = this.m_MatchedRules.Count; index < count; ++index)
          {
            StyleContext.RuleRef matchedRule = this.m_MatchedRules[index];
            StylePropertyID[] propertyIds = StyleSheetCache.GetPropertyIDs(matchedRule.sheet, matchedRule.selector.ruleIndex);
            sharedStyle2.ApplyRule(matchedRule.sheet, matchedRule.selector.specificity, matchedRule.selector.rule, propertyIds);
          }
          StyleContext.s_StyleCache[this.m_MatchingRulesHash] = sharedStyle2;
          element.SetSharedStyles(sharedStyle2);
        }
      }
    }
  }
}
