// Decompiled with JetBrains decompiler
// Type: UnityEditor.StyleSheets.StyleSheetImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using ExCSS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.StyleSheets;

namespace UnityEditor.StyleSheets
{
  [ScriptedImporter(3, "uss", -20)]
  internal class StyleSheetImporter : ScriptedImporter
  {
    private ExCSS.Parser m_Parser;
    private const string k_ResourcePathFunctionName = "resource";
    private StyleSheetBuilder m_Builder;
    private StyleSheetImportErrors m_Errors;
    private static Dictionary<string, StyleValueKeyword> s_NameCache;

    public StyleSheetImporter()
    {
      this.m_Parser = new ExCSS.Parser();
      this.m_Builder = new StyleSheetBuilder();
      this.m_Errors = new StyleSheetImportErrors();
    }

    public override void OnImportAsset(AssetImportContext ctx)
    {
      StyleSheetImporter styleSheetImporter = new StyleSheetImporter();
      string contents = File.ReadAllText(ctx.assetPath);
      UnityEngine.StyleSheets.StyleSheet instance = ScriptableObject.CreateInstance<UnityEngine.StyleSheets.StyleSheet>();
      instance.hideFlags = HideFlags.NotEditable;
      styleSheetImporter.Import(instance, contents, ctx.assetPath);
      ctx.AddObjectToAsset("stylesheet", (UnityEngine.Object) instance);
      StyleSheetAssetPostprocessor.ClearReferencedAssets();
      StyleContext.ClearStyleCache();
    }

    public void Import(UnityEngine.StyleSheets.StyleSheet asset, string contents, string contextAssetPath)
    {
      ExCSS.StyleSheet styleSheet = this.m_Parser.Parse(contents);
      if (styleSheet.Errors.Count > 0)
      {
        foreach (object error in styleSheet.Errors)
          this.m_Errors.AddSyntaxError(error.ToString());
      }
      else
      {
        try
        {
          this.VisitSheet(styleSheet);
        }
        catch (Exception ex)
        {
          this.m_Errors.AddInternalError(ex.Message);
        }
      }
      if (this.m_Errors.hasErrors)
      {
        foreach (string formatError in this.m_Errors.FormatErrors())
          Debug.LogErrorFormat(formatError);
      }
      else
        this.m_Builder.BuildTo(asset);
    }

    private void VisitSheet(ExCSS.StyleSheet styleSheet)
    {
      foreach (ExCSS.StyleRule styleRule in (IEnumerable<ExCSS.StyleRule>) styleSheet.StyleRules)
      {
        this.m_Builder.BeginRule(styleRule.Line);
        this.VisitBaseSelector(styleRule.Selector);
        foreach (Property declaration in styleRule.Declarations)
        {
          this.m_Builder.BeginProperty(declaration.Name);
          StyleSheetImporter.VisitValue(this.m_Errors, this.m_Builder, declaration.Term);
          this.m_Builder.EndProperty();
        }
        this.m_Builder.EndRule();
      }
    }

    internal static void VisitValue(StyleSheetImportErrors errors, StyleSheetBuilder ssb, Term term)
    {
      PrimitiveTerm primitiveTerm1 = term as PrimitiveTerm;
      HtmlColor htmlColor = term as HtmlColor;
      GenericFunction genericFunction = term as GenericFunction;
      TermList termList = term as TermList;
      if (term == Term.Inherit)
        ssb.AddValue(StyleValueKeyword.Inherit);
      else if (primitiveTerm1 != null)
      {
        string rawStr = term.ToString();
        UnitType primitiveType = primitiveTerm1.PrimitiveType;
        switch (primitiveType)
        {
          case UnitType.String:
            string str = rawStr.Trim('\'', '"');
            ssb.AddValue(str, StyleValueType.String);
            break;
          case UnitType.Ident:
            StyleValueKeyword keyword;
            if (StyleSheetImporter.TryParseKeyword(rawStr, out keyword))
            {
              ssb.AddValue(keyword);
              break;
            }
            ssb.AddValue(rawStr, StyleValueType.Enum);
            break;
          default:
            if (primitiveType == UnitType.Number || primitiveType == UnitType.Pixel)
            {
              float? floatValue = primitiveTerm1.GetFloatValue(UnitType.Pixel);
              ssb.AddValue(floatValue.Value);
              break;
            }
            errors.AddSemanticError(StyleSheetImportErrorCode.UnsupportedUnit, primitiveTerm1.ToString());
            break;
        }
      }
      else if (htmlColor != (HtmlColor) null)
      {
        Color color = new Color((float) htmlColor.R / (float) byte.MaxValue, (float) htmlColor.G / (float) byte.MaxValue, (float) htmlColor.B / (float) byte.MaxValue, (float) htmlColor.A / (float) byte.MaxValue);
        ssb.AddValue(color);
      }
      else if (genericFunction != null)
      {
        PrimitiveTerm primitiveTerm2 = genericFunction.Arguments.FirstOrDefault<Term>() as PrimitiveTerm;
        if (genericFunction.Name == "resource" && primitiveTerm2 != null)
        {
          string str = primitiveTerm2.Value as string;
          ssb.AddValue(str, StyleValueType.ResourcePath);
        }
        else
          errors.AddSemanticError(StyleSheetImportErrorCode.UnsupportedFunction, genericFunction.Name);
      }
      else if (termList != null)
      {
        foreach (Term term1 in termList)
          StyleSheetImporter.VisitValue(errors, ssb, term1);
      }
      else
        errors.AddInternalError(term.GetType().Name);
    }

    private void VisitBaseSelector(BaseSelector selector)
    {
      AggregateSelectorList selectorList = selector as AggregateSelectorList;
      if (selectorList != null)
      {
        this.VisitSelectorList(selectorList);
      }
      else
      {
        ComplexSelector complexSelector = selector as ComplexSelector;
        if (complexSelector != null)
        {
          this.VisitComplexSelector(complexSelector);
        }
        else
        {
          SimpleSelector simpleSelector = selector as SimpleSelector;
          if (simpleSelector == null)
            return;
          this.VisitSimpleSelector(simpleSelector.ToString());
        }
      }
    }

    private void VisitSelectorList(AggregateSelectorList selectorList)
    {
      if (selectorList.Delimiter == ",")
      {
        foreach (BaseSelector selector in (SelectorList) selectorList)
          this.VisitBaseSelector(selector);
      }
      else if (selectorList.Delimiter == string.Empty)
        this.VisitSimpleSelector(selectorList.ToString());
      else
        this.m_Errors.AddSemanticError(StyleSheetImportErrorCode.InvalidSelectorListDelimiter, selectorList.Delimiter);
    }

    private void VisitComplexSelector(ComplexSelector complexSelector)
    {
      int selectorSpecificity = CSSSpec.GetSelectorSpecificity(complexSelector.ToString());
      if (selectorSpecificity == 0)
      {
        this.m_Errors.AddInternalError("Failed to calculate selector specificity " + (object) complexSelector);
      }
      else
      {
        using (this.m_Builder.BeginComplexSelector(selectorSpecificity))
        {
          StyleSelectorRelationship previousRelationsip = StyleSelectorRelationship.None;
          foreach (CombinatorSelector combinatorSelector in complexSelector)
          {
            string simpleSelector = this.ExtractSimpleSelector(combinatorSelector.Selector);
            if (string.IsNullOrEmpty(simpleSelector))
            {
              this.m_Errors.AddInternalError("Expected simple selector inside complex selector " + simpleSelector);
              break;
            }
            StyleSelectorPart[] parts;
            if (!this.CheckSimpleSelector(simpleSelector, out parts))
              break;
            this.m_Builder.AddSimpleSelector(parts, previousRelationsip);
            switch (combinatorSelector.Delimiter)
            {
              case Combinator.Child:
                previousRelationsip = StyleSelectorRelationship.Child;
                break;
              case Combinator.Descendent:
                previousRelationsip = StyleSelectorRelationship.Descendent;
                break;
              default:
                this.m_Errors.AddSemanticError(StyleSheetImportErrorCode.InvalidComplexSelectorDelimiter, complexSelector.ToString());
                return;
            }
          }
        }
      }
    }

    private void VisitSimpleSelector(string selector)
    {
      StyleSelectorPart[] parts;
      if (!this.CheckSimpleSelector(selector, out parts))
        return;
      int selectorSpecificity = CSSSpec.GetSelectorSpecificity(parts);
      if (selectorSpecificity == 0)
      {
        this.m_Errors.AddInternalError("Failed to calculate selector specificity " + selector);
      }
      else
      {
        using (this.m_Builder.BeginComplexSelector(selectorSpecificity))
          this.m_Builder.AddSimpleSelector(parts, StyleSelectorRelationship.None);
      }
    }

    private string ExtractSimpleSelector(BaseSelector selector)
    {
      if (selector is SimpleSelector)
        return selector.ToString();
      AggregateSelectorList aggregateSelectorList = selector as AggregateSelectorList;
      if (aggregateSelectorList != null && aggregateSelectorList.Delimiter == string.Empty)
        return aggregateSelectorList.ToString();
      return string.Empty;
    }

    private static bool TryParseKeyword(string rawStr, out StyleValueKeyword value)
    {
      if (StyleSheetImporter.s_NameCache == null)
      {
        StyleSheetImporter.s_NameCache = new Dictionary<string, StyleValueKeyword>();
        IEnumerator enumerator = Enum.GetValues(typeof (StyleValueKeyword)).GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            StyleValueKeyword current = (StyleValueKeyword) enumerator.Current;
            StyleSheetImporter.s_NameCache[current.ToString().ToLower()] = current;
          }
        }
        finally
        {
          IDisposable disposable;
          if ((disposable = enumerator as IDisposable) != null)
            disposable.Dispose();
        }
      }
      return StyleSheetImporter.s_NameCache.TryGetValue(rawStr.ToLower(), out value);
    }

    private bool CheckSimpleSelector(string selector, out StyleSelectorPart[] parts)
    {
      if (!CSSSpec.ParseSelector(selector, out parts))
      {
        this.m_Errors.AddSemanticError(StyleSheetImportErrorCode.UnsupportedSelectorFormat, selector);
        return false;
      }
      if (((IEnumerable<StyleSelectorPart>) parts).Any<StyleSelectorPart>((Func<StyleSelectorPart, bool>) (p => p.type == StyleSelectorType.Unknown)))
      {
        this.m_Errors.AddSemanticError(StyleSheetImportErrorCode.UnsupportedSelectorFormat, selector);
        return false;
      }
      if (!((IEnumerable<StyleSelectorPart>) parts).Any<StyleSelectorPart>((Func<StyleSelectorPart, bool>) (p => p.type == StyleSelectorType.RecursivePseudoClass)))
        return true;
      this.m_Errors.AddSemanticError(StyleSheetImportErrorCode.RecursiveSelectorDetected, selector);
      return false;
    }
  }
}
