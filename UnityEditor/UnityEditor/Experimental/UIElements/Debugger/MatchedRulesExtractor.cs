// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.Debugger.MatchedRulesExtractor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.StyleSheets;

namespace UnityEditor.Experimental.UIElements.Debugger
{
  internal class MatchedRulesExtractor : HierarchyTraversal
  {
    internal List<RuleMatcher> ruleMatchers = new List<RuleMatcher>();
    internal HashSet<MatchedRulesExtractor.MatchedRule> selectedElementRules = new HashSet<MatchedRulesExtractor.MatchedRule>(MatchedRulesExtractor.MatchedRule.lineNumberFullPathComparer);
    internal HashSet<string> selectedElementStylesheets = new HashSet<string>();
    private List<VisualElement> m_Hierarchy = new List<VisualElement>();
    private VisualElement m_Target;
    private int m_Index;

    public VisualElement target
    {
      get
      {
        return this.m_Target;
      }
      set
      {
        this.m_Target = value;
        this.m_Hierarchy.Clear();
        this.m_Hierarchy.Add(value);
        for (VisualElement visualElement = value; visualElement != null; visualElement = visualElement.shadow.parent)
        {
          if (visualElement.styleSheets != null)
          {
            foreach (StyleSheet styleSheet in visualElement.styleSheets)
            {
              this.selectedElementStylesheets.Add(AssetDatabase.GetAssetPath((UnityEngine.Object) styleSheet) ?? "<unknown>");
              this.PushStyleSheet(styleSheet);
            }
          }
          this.m_Hierarchy.Add(visualElement);
        }
        this.m_Index = this.m_Hierarchy.Count - 1;
      }
    }

    private void PushStyleSheet(StyleSheet styleSheetData)
    {
      StyleComplexSelector[] complexSelectors = styleSheetData.complexSelectors;
      this.ruleMatchers.Capacity = Math.Max(this.ruleMatchers.Capacity, this.ruleMatchers.Count + complexSelectors.Length);
      for (int index = 0; index < complexSelectors.Length; ++index)
      {
        StyleComplexSelector styleComplexSelector = complexSelectors[index];
        this.ruleMatchers.Add(new RuleMatcher()
        {
          sheet = styleSheetData,
          complexSelector = styleComplexSelector,
          simpleSelectorIndex = 0,
          depth = int.MaxValue
        });
      }
    }

    public override bool ShouldSkipElement(VisualElement element)
    {
      return false;
    }

    public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
    {
      if (element == this.target)
        this.selectedElementRules.Add(new MatchedRulesExtractor.MatchedRule(matcher));
      return false;
    }

    protected override void Recurse(VisualElement element, int depth, List<RuleMatcher> allRuleMatchers)
    {
      --this.m_Index;
      if (this.m_Index < 0)
        return;
      this.Traverse(this.m_Hierarchy[this.m_Index], depth + 1, allRuleMatchers);
    }

    internal struct MatchedRule
    {
      public static IEqualityComparer<MatchedRulesExtractor.MatchedRule> lineNumberFullPathComparer = (IEqualityComparer<MatchedRulesExtractor.MatchedRule>) new MatchedRulesExtractor.MatchedRule.LineNumberFullPathEqualityComparer();
      public readonly RuleMatcher ruleMatcher;
      public readonly string displayPath;
      public readonly int lineNumber;
      public readonly string fullPath;

      public MatchedRule(RuleMatcher ruleMatcher)
      {
        this = new MatchedRulesExtractor.MatchedRule();
        this.ruleMatcher = ruleMatcher;
        this.fullPath = AssetDatabase.GetAssetPath((UnityEngine.Object) ruleMatcher.sheet);
        this.lineNumber = ruleMatcher.complexSelector.rule.line;
        if (this.fullPath == null)
          return;
        this.displayPath = !(this.fullPath == "Library/unity editor resources") ? Path.GetFileNameWithoutExtension(this.fullPath) + ":" + (object) this.lineNumber : ruleMatcher.sheet.name + ":" + (object) this.lineNumber;
      }

      private sealed class LineNumberFullPathEqualityComparer : IEqualityComparer<MatchedRulesExtractor.MatchedRule>
      {
        public bool Equals(MatchedRulesExtractor.MatchedRule x, MatchedRulesExtractor.MatchedRule y)
        {
          return x.lineNumber == y.lineNumber && string.Equals(x.fullPath, y.fullPath);
        }

        public int GetHashCode(MatchedRulesExtractor.MatchedRule obj)
        {
          return obj.lineNumber * 397 ^ (obj.fullPath == null ? 0 : obj.fullPath.GetHashCode());
        }
      }
    }
  }
}
