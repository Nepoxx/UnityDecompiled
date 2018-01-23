// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.UQuery
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>UQuery is a set of extension methods allowing you to select individual or collection of visualElements inside a complex hierarchy.</para>
  /// </summary>
  public static class UQuery
  {
    private static UQuery.FirstQueryMatcher s_First = new UQuery.FirstQueryMatcher();
    private static UQuery.LastQueryMatcher s_Last = new UQuery.LastQueryMatcher();
    private static UQuery.IndexQueryMatcher s_Index = new UQuery.IndexQueryMatcher();

    internal interface IVisualPredicateWrapper
    {
      bool Predicate(object e);
    }

    internal class IsOfType<T> : UQuery.IVisualPredicateWrapper where T : VisualElement
    {
      public static UQuery.IsOfType<T> s_Instance = new UQuery.IsOfType<T>();

      public bool Predicate(object e)
      {
        return e is T;
      }
    }

    internal class PredicateWrapper<T> : UQuery.IVisualPredicateWrapper where T : VisualElement
    {
      private Func<T, bool> predicate;

      public PredicateWrapper(Func<T, bool> p)
      {
        this.predicate = p;
      }

      public bool Predicate(object e)
      {
        T obj = e as T;
        if ((object) obj != null)
          return this.predicate(obj);
        return false;
      }
    }

    private abstract class UQueryMatcher : HierarchyTraversal
    {
      public override bool ShouldSkipElement(VisualElement element)
      {
        return false;
      }

      protected override bool MatchSelectorPart(VisualElement element, StyleSelector selector, StyleSelectorPart part)
      {
        if (part.type != StyleSelectorType.Predicate)
          return base.MatchSelectorPart(element, selector, part);
        UQuery.IVisualPredicateWrapper tempData = part.tempData as UQuery.IVisualPredicateWrapper;
        return tempData != null && tempData.Predicate((object) element);
      }

      public virtual void Run(VisualElement root, List<RuleMatcher> ruleMatchers)
      {
        this.Traverse(root, 0, ruleMatchers);
      }
    }

    private abstract class SingleQueryMatcher : UQuery.UQueryMatcher
    {
      public VisualElement match { get; protected set; }

      public override void Run(VisualElement root, List<RuleMatcher> ruleMatchers)
      {
        this.match = (VisualElement) null;
        base.Run(root, ruleMatchers);
      }
    }

    private class FirstQueryMatcher : UQuery.SingleQueryMatcher
    {
      public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
      {
        if (this.match == null)
          this.match = element;
        return true;
      }
    }

    private class LastQueryMatcher : UQuery.SingleQueryMatcher
    {
      public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
      {
        this.match = element;
        return false;
      }
    }

    private class IndexQueryMatcher : UQuery.SingleQueryMatcher
    {
      private int matchCount = -1;
      private int _matchIndex;

      public int matchIndex
      {
        get
        {
          return this._matchIndex;
        }
        set
        {
          this.matchCount = -1;
          this._matchIndex = value;
        }
      }

      public override void Run(VisualElement root, List<RuleMatcher> ruleMatchers)
      {
        this.matchCount = -1;
        base.Run(root, ruleMatchers);
      }

      public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
      {
        ++this.matchCount;
        if (this.matchCount == this._matchIndex)
          this.match = element;
        return this.matchCount >= this._matchIndex;
      }
    }

    /// <summary>
    ///   <para>Utility Object that contructs a set of selection rules to be ran on a root visual element.</para>
    /// </summary>
    public struct QueryBuilder<T> where T : VisualElement
    {
      private List<StyleSelector> m_StyleSelectors;
      private List<StyleSelectorPart> m_Parts;
      private VisualElement m_Element;
      private List<RuleMatcher> m_Matchers;
      private StyleSelectorRelationship m_Relationship;
      private int pseudoStatesMask;
      private int negatedPseudoStatesMask;

      public QueryBuilder(VisualElement visualElement)
      {
        this = new UQuery.QueryBuilder<T>();
        this.m_Element = visualElement;
        this.m_Parts = (List<StyleSelectorPart>) null;
        this.m_StyleSelectors = (List<StyleSelector>) null;
        this.m_Relationship = StyleSelectorRelationship.None;
        this.m_Matchers = new List<RuleMatcher>();
        this.pseudoStatesMask = this.negatedPseudoStatesMask = 0;
      }

      private List<StyleSelector> styleSelectors
      {
        get
        {
          return this.m_StyleSelectors ?? (this.m_StyleSelectors = new List<StyleSelector>());
        }
      }

      private List<StyleSelectorPart> parts
      {
        get
        {
          return this.m_Parts ?? (this.m_Parts = new List<StyleSelectorPart>());
        }
      }

      public UQuery.QueryBuilder<T> Class(string classname)
      {
        this.AddClass(classname);
        return this;
      }

      public UQuery.QueryBuilder<T> Name(string id)
      {
        this.AddName(id);
        return this;
      }

      public UQuery.QueryBuilder<T2> Descendents<T2>(string name = null, params string[] classNames) where T2 : VisualElement
      {
        this.FinishCurrentSelector();
        this.AddType<T2>();
        this.AddName(name);
        this.AddClasses(classNames);
        return this.AddRelationship<T2>(StyleSelectorRelationship.Descendent);
      }

      public UQuery.QueryBuilder<T2> Descendents<T2>(string name = null, string classname = null) where T2 : VisualElement
      {
        this.FinishCurrentSelector();
        this.AddType<T2>();
        this.AddName(name);
        this.AddClass(classname);
        return this.AddRelationship<T2>(StyleSelectorRelationship.Descendent);
      }

      public UQuery.QueryBuilder<T2> Children<T2>(string name = null, params string[] classes) where T2 : VisualElement
      {
        this.FinishCurrentSelector();
        this.AddType<T2>();
        this.AddName(name);
        this.AddClasses(classes);
        return this.AddRelationship<T2>(StyleSelectorRelationship.Child);
      }

      public UQuery.QueryBuilder<T2> Children<T2>(string name = null, string className = null) where T2 : VisualElement
      {
        this.FinishCurrentSelector();
        this.AddType<T2>();
        this.AddName(name);
        this.AddClass(className);
        return this.AddRelationship<T2>(StyleSelectorRelationship.Child);
      }

      public UQuery.QueryBuilder<T2> OfType<T2>(string name = null, params string[] classes) where T2 : VisualElement
      {
        this.AddType<T2>();
        this.AddName(name);
        this.AddClasses(classes);
        return this.AddRelationship<T2>(StyleSelectorRelationship.None);
      }

      public UQuery.QueryBuilder<T2> OfType<T2>(string name = null, string className = null) where T2 : VisualElement
      {
        this.AddType<T2>();
        this.AddName(name);
        this.AddClass(className);
        return this.AddRelationship<T2>(StyleSelectorRelationship.None);
      }

      public UQuery.QueryBuilder<T> Where(Func<T, bool> selectorPredicate)
      {
        this.parts.Add(StyleSelectorPart.CreatePredicate((object) new UQuery.PredicateWrapper<T>(selectorPredicate)));
        return this;
      }

      private void AddClass(string c)
      {
        if (c == null)
          return;
        this.parts.Add(StyleSelectorPart.CreateClass(c));
      }

      private void AddClasses(params string[] classes)
      {
        if (classes == null)
          return;
        for (int index = 0; index < classes.Length; ++index)
          this.AddClass(classes[index]);
      }

      private void AddName(string id)
      {
        if (id == null)
          return;
        this.parts.Add(StyleSelectorPart.CreateId(id));
      }

      private void AddType<T2>() where T2 : VisualElement
      {
        if (typeof (T2) == typeof (VisualElement))
          return;
        this.parts.Add(StyleSelectorPart.CreatePredicate((object) UQuery.IsOfType<T2>.s_Instance));
      }

      private UQuery.QueryBuilder<T> AddPseudoState(PseudoStates s)
      {
        this.pseudoStatesMask = (int) ((PseudoStates) this.pseudoStatesMask | s);
        return this;
      }

      private UQuery.QueryBuilder<T> AddNegativePseudoState(PseudoStates s)
      {
        this.negatedPseudoStatesMask = (int) ((PseudoStates) this.negatedPseudoStatesMask | s);
        return this;
      }

      public UQuery.QueryBuilder<T> Active()
      {
        return this.AddPseudoState(PseudoStates.Active);
      }

      public UQuery.QueryBuilder<T> NotActive()
      {
        return this.AddNegativePseudoState(PseudoStates.Active);
      }

      public UQuery.QueryBuilder<T> Visible()
      {
        return this.AddNegativePseudoState(PseudoStates.Invisible);
      }

      public UQuery.QueryBuilder<T> NotVisible()
      {
        return this.AddPseudoState(PseudoStates.Invisible);
      }

      public UQuery.QueryBuilder<T> Hovered()
      {
        return this.AddPseudoState(PseudoStates.Hover);
      }

      public UQuery.QueryBuilder<T> NotHovered()
      {
        return this.AddNegativePseudoState(PseudoStates.Hover);
      }

      public UQuery.QueryBuilder<T> Checked()
      {
        return this.AddPseudoState(PseudoStates.Checked);
      }

      public UQuery.QueryBuilder<T> NotChecked()
      {
        return this.AddNegativePseudoState(PseudoStates.Checked);
      }

      public UQuery.QueryBuilder<T> Selected()
      {
        return this.AddPseudoState(PseudoStates.Selected);
      }

      public UQuery.QueryBuilder<T> NotSelected()
      {
        return this.AddNegativePseudoState(PseudoStates.Selected);
      }

      public UQuery.QueryBuilder<T> Enabled()
      {
        return this.AddNegativePseudoState(PseudoStates.Disabled);
      }

      public UQuery.QueryBuilder<T> NotEnabled()
      {
        return this.AddPseudoState(PseudoStates.Disabled);
      }

      public UQuery.QueryBuilder<T> Focused()
      {
        return this.AddPseudoState(PseudoStates.Focus);
      }

      public UQuery.QueryBuilder<T> NotFocused()
      {
        return this.AddNegativePseudoState(PseudoStates.Focus);
      }

      private UQuery.QueryBuilder<T2> AddRelationship<T2>(StyleSelectorRelationship relationship) where T2 : VisualElement
      {
        return new UQuery.QueryBuilder<T2>(this.m_Element) { m_Matchers = this.m_Matchers, m_Parts = this.m_Parts, m_StyleSelectors = this.m_StyleSelectors, m_Relationship = relationship, pseudoStatesMask = this.pseudoStatesMask, negatedPseudoStatesMask = this.negatedPseudoStatesMask };
      }

      private void AddPseudoStatesRuleIfNecessasy()
      {
        if (this.pseudoStatesMask == 0 && this.negatedPseudoStatesMask == 0)
          return;
        this.parts.Add(new StyleSelectorPart()
        {
          type = StyleSelectorType.PseudoClass
        });
      }

      private void FinishSelector()
      {
        this.FinishCurrentSelector();
        if (this.styleSelectors.Count <= 0)
          return;
        StyleComplexSelector styleComplexSelector = new StyleComplexSelector();
        styleComplexSelector.selectors = this.styleSelectors.ToArray();
        this.styleSelectors.Clear();
        this.m_Matchers.Add(new RuleMatcher()
        {
          complexSelector = styleComplexSelector,
          simpleSelectorIndex = 0,
          depth = int.MaxValue
        });
      }

      private bool CurrentSelectorEmpty()
      {
        return this.parts.Count == 0 && this.m_Relationship == StyleSelectorRelationship.None && this.pseudoStatesMask == 0 && this.negatedPseudoStatesMask == 0;
      }

      private void FinishCurrentSelector()
      {
        if (this.CurrentSelectorEmpty())
          return;
        StyleSelector styleSelector = new StyleSelector();
        styleSelector.previousRelationship = this.m_Relationship;
        this.AddPseudoStatesRuleIfNecessasy();
        styleSelector.parts = this.m_Parts.ToArray();
        styleSelector.pseudoStateMask = this.pseudoStatesMask;
        styleSelector.negatedPseudoStateMask = this.negatedPseudoStatesMask;
        this.styleSelectors.Add(styleSelector);
        this.m_Parts.Clear();
        this.pseudoStatesMask = this.negatedPseudoStatesMask = 0;
      }

      public UQuery.QueryState<T> Build()
      {
        this.FinishSelector();
        return new UQuery.QueryState<T>(this.m_Element, this.m_Matchers);
      }

      public static implicit operator T(UQuery.QueryBuilder<T> s)
      {
        return s.First();
      }

      public T First()
      {
        return this.Build().First();
      }

      public T Last()
      {
        return this.Build().Last();
      }

      public List<T> ToList()
      {
        return this.Build().ToList();
      }

      public void ToList(List<T> results)
      {
        this.Build().ToList(results);
      }

      public T AtIndex(int index)
      {
        return this.Build().AtIndex(index);
      }

      public void ForEach<T2>(List<T2> result, Func<T, T2> funcCall)
      {
        this.Build().ForEach<T2>(result, funcCall);
      }

      public List<T2> ForEach<T2>(Func<T, T2> funcCall)
      {
        return this.Build().ForEach<T2>(funcCall);
      }

      public void ForEach(Action<T> funcCall)
      {
        this.Build().ForEach(funcCall);
      }
    }

    /// <summary>
    ///   <para>Query object containing all the selection rules. Can be saved and rerun later without re-allocating memory.</para>
    /// </summary>
    public struct QueryState<T> where T : VisualElement
    {
      private static readonly UQuery.QueryState<T>.ListQueryMatcher s_List = new UQuery.QueryState<T>.ListQueryMatcher();
      private static UQuery.QueryState<T>.ActionQueryMatcher s_Action = new UQuery.QueryState<T>.ActionQueryMatcher();
      private readonly VisualElement m_Element;
      private readonly List<RuleMatcher> m_Matchers;

      internal QueryState(VisualElement element, List<RuleMatcher> matchers)
      {
        this.m_Element = element;
        this.m_Matchers = matchers;
      }

      public UQuery.QueryState<T> RebuildOn(VisualElement element)
      {
        return new UQuery.QueryState<T>(element, this.m_Matchers);
      }

      public T First()
      {
        UQuery.s_First.Run(this.m_Element, this.m_Matchers);
        return UQuery.s_First.match as T;
      }

      public T Last()
      {
        UQuery.s_Last.Run(this.m_Element, this.m_Matchers);
        return UQuery.s_Last.match as T;
      }

      public void ToList(List<T> results)
      {
        UQuery.QueryState<T>.s_List.matches = results;
        UQuery.QueryState<T>.s_List.Run(this.m_Element, this.m_Matchers);
        UQuery.QueryState<T>.s_List.Reset();
      }

      public List<T> ToList()
      {
        List<T> results = new List<T>();
        this.ToList(results);
        return results;
      }

      public T AtIndex(int index)
      {
        UQuery.s_Index.matchIndex = index;
        UQuery.s_Index.Run(this.m_Element, this.m_Matchers);
        return UQuery.s_Index.match as T;
      }

      public void ForEach(Action<T> funcCall)
      {
        UQuery.QueryState<T>.s_Action.callBack = funcCall;
        UQuery.QueryState<T>.s_Action.Run(this.m_Element, this.m_Matchers);
        UQuery.QueryState<T>.s_Action.callBack = (Action<T>) null;
      }

      public void ForEach<T2>(List<T2> result, Func<T, T2> funcCall)
      {
        UQuery.QueryState<T>.DelegateQueryMatcher<T2> instance = UQuery.QueryState<T>.DelegateQueryMatcher<T2>.s_Instance;
        instance.callBack = funcCall;
        instance.result = result;
        instance.Run(this.m_Element, this.m_Matchers);
        instance.callBack = (Func<T, T2>) null;
        instance.result = (List<T2>) null;
      }

      public List<T2> ForEach<T2>(Func<T, T2> funcCall)
      {
        List<T2> result = new List<T2>();
        this.ForEach<T2>(result, funcCall);
        return result;
      }

      private class ListQueryMatcher : UQuery.UQueryMatcher
      {
        public List<T> matches { get; set; }

        public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
        {
          this.matches.Add(element as T);
          return false;
        }

        public void Reset()
        {
          this.matches = (List<T>) null;
        }
      }

      private class ActionQueryMatcher : UQuery.UQueryMatcher
      {
        internal Action<T> callBack { get; set; }

        public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
        {
          T obj = element as T;
          if ((object) obj != null)
            this.callBack(obj);
          return false;
        }
      }

      private class DelegateQueryMatcher<TReturnType> : UQuery.UQueryMatcher
      {
        public static UQuery.QueryState<T>.DelegateQueryMatcher<TReturnType> s_Instance = new UQuery.QueryState<T>.DelegateQueryMatcher<TReturnType>();

        public Func<T, TReturnType> callBack { get; set; }

        public List<TReturnType> result { get; set; }

        public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
        {
          T obj = element as T;
          if ((object) obj != null)
            this.result.Add(this.callBack(obj));
          return false;
        }
      }
    }
  }
}
