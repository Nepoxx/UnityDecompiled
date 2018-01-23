// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.StyleSheets.VisualElementStylesData
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
  internal class VisualElementStylesData : ICustomStyle
  {
    public static VisualElementStylesData none = new VisualElementStylesData(true);
    internal readonly bool isShared;
    private Dictionary<string, CustomProperty> m_CustomProperties;
    internal StyleValue<float> width;
    internal StyleValue<float> height;
    internal StyleValue<float> maxWidth;
    internal StyleValue<float> maxHeight;
    internal StyleValue<float> minWidth;
    internal StyleValue<float> minHeight;
    internal StyleValue<float> flex;
    internal StyleValue<int> overflow;
    internal StyleValue<float> positionLeft;
    internal StyleValue<float> positionTop;
    internal StyleValue<float> positionRight;
    internal StyleValue<float> positionBottom;
    internal StyleValue<float> marginLeft;
    internal StyleValue<float> marginTop;
    internal StyleValue<float> marginRight;
    internal StyleValue<float> marginBottom;
    internal StyleValue<float> borderLeft;
    internal StyleValue<float> borderTop;
    internal StyleValue<float> borderRight;
    internal StyleValue<float> borderBottom;
    internal StyleValue<float> paddingLeft;
    internal StyleValue<float> paddingTop;
    internal StyleValue<float> paddingRight;
    internal StyleValue<float> paddingBottom;
    internal StyleValue<int> positionType;
    internal StyleValue<int> alignSelf;
    internal StyleValue<int> textAlignment;
    internal StyleValue<int> fontStyle;
    internal StyleValue<int> textClipping;
    internal StyleValue<Font> font;
    internal StyleValue<int> fontSize;
    internal StyleValue<bool> wordWrap;
    internal StyleValue<Color> textColor;
    internal StyleValue<int> flexDirection;
    internal StyleValue<Color> backgroundColor;
    internal StyleValue<Color> borderColor;
    internal StyleValue<Texture2D> backgroundImage;
    internal StyleValue<int> backgroundSize;
    internal StyleValue<int> alignItems;
    internal StyleValue<int> alignContent;
    internal StyleValue<int> justifyContent;
    internal StyleValue<int> flexWrap;
    internal StyleValue<float> borderLeftWidth;
    internal StyleValue<float> borderTopWidth;
    internal StyleValue<float> borderRightWidth;
    internal StyleValue<float> borderBottomWidth;
    internal StyleValue<float> borderTopLeftRadius;
    internal StyleValue<float> borderTopRightRadius;
    internal StyleValue<float> borderBottomRightRadius;
    internal StyleValue<float> borderBottomLeftRadius;
    internal StyleValue<int> sliceLeft;
    internal StyleValue<int> sliceTop;
    internal StyleValue<int> sliceRight;
    internal StyleValue<int> sliceBottom;
    internal StyleValue<float> opacity;

    public VisualElementStylesData(bool isShared)
    {
      this.isShared = isShared;
    }

    public void Apply(VisualElementStylesData other, StylePropertyApplyMode mode)
    {
      this.m_CustomProperties = other.m_CustomProperties;
      this.width.Apply(other.width, mode);
      this.height.Apply(other.height, mode);
      this.maxWidth.Apply(other.maxWidth, mode);
      this.maxHeight.Apply(other.maxHeight, mode);
      this.minWidth.Apply(other.minWidth, mode);
      this.minHeight.Apply(other.minHeight, mode);
      this.flex.Apply(other.flex, mode);
      this.overflow.Apply(other.overflow, mode);
      this.positionLeft.Apply(other.positionLeft, mode);
      this.positionTop.Apply(other.positionTop, mode);
      this.positionRight.Apply(other.positionRight, mode);
      this.positionBottom.Apply(other.positionBottom, mode);
      this.marginLeft.Apply(other.marginLeft, mode);
      this.marginTop.Apply(other.marginTop, mode);
      this.marginRight.Apply(other.marginRight, mode);
      this.marginBottom.Apply(other.marginBottom, mode);
      this.borderLeft.Apply(other.borderLeft, mode);
      this.borderTop.Apply(other.borderTop, mode);
      this.borderRight.Apply(other.borderRight, mode);
      this.borderBottom.Apply(other.borderBottom, mode);
      this.paddingLeft.Apply(other.paddingLeft, mode);
      this.paddingTop.Apply(other.paddingTop, mode);
      this.paddingRight.Apply(other.paddingRight, mode);
      this.paddingBottom.Apply(other.paddingBottom, mode);
      this.positionType.Apply(other.positionType, mode);
      this.alignSelf.Apply(other.alignSelf, mode);
      this.textAlignment.Apply(other.textAlignment, mode);
      this.fontStyle.Apply(other.fontStyle, mode);
      this.textClipping.Apply(other.textClipping, mode);
      this.fontSize.Apply(other.fontSize, mode);
      this.font.Apply(other.font, mode);
      this.wordWrap.Apply(other.wordWrap, mode);
      this.textColor.Apply(other.textColor, mode);
      this.flexDirection.Apply(other.flexDirection, mode);
      this.backgroundColor.Apply(other.backgroundColor, mode);
      this.borderColor.Apply(other.borderColor, mode);
      this.backgroundImage.Apply(other.backgroundImage, mode);
      this.backgroundSize.Apply(other.backgroundSize, mode);
      this.alignItems.Apply(other.alignItems, mode);
      this.alignContent.Apply(other.alignContent, mode);
      this.justifyContent.Apply(other.justifyContent, mode);
      this.flexWrap.Apply(other.flexWrap, mode);
      this.borderLeftWidth.Apply(other.borderLeftWidth, mode);
      this.borderTopWidth.Apply(other.borderTopWidth, mode);
      this.borderRightWidth.Apply(other.borderRightWidth, mode);
      this.borderBottomWidth.Apply(other.borderBottomWidth, mode);
      this.borderTopLeftRadius.Apply(other.borderTopLeftRadius, mode);
      this.borderTopRightRadius.Apply(other.borderTopRightRadius, mode);
      this.borderBottomRightRadius.Apply(other.borderBottomRightRadius, mode);
      this.borderBottomLeftRadius.Apply(other.borderBottomLeftRadius, mode);
      this.sliceLeft.Apply(other.sliceLeft, mode);
      this.sliceTop.Apply(other.sliceTop, mode);
      this.sliceRight.Apply(other.sliceRight, mode);
      this.sliceBottom.Apply(other.sliceBottom, mode);
      this.opacity.Apply(other.opacity, mode);
    }

    public void WriteToGUIStyle(GUIStyle style)
    {
      style.alignment = (TextAnchor) this.textAlignment.GetSpecifiedValueOrDefault((int) style.alignment);
      style.wordWrap = this.wordWrap.GetSpecifiedValueOrDefault(style.wordWrap);
      style.clipping = (TextClipping) this.textClipping.GetSpecifiedValueOrDefault((int) style.clipping);
      if ((UnityEngine.Object) this.font.value != (UnityEngine.Object) null)
        style.font = this.font.value;
      style.fontSize = this.fontSize.GetSpecifiedValueOrDefault(style.fontSize);
      style.fontStyle = (FontStyle) this.fontStyle.GetSpecifiedValueOrDefault((int) style.fontStyle);
      this.AssignRect(style.margin, ref this.marginLeft, ref this.marginTop, ref this.marginRight, ref this.marginBottom);
      this.AssignRect(style.padding, ref this.paddingLeft, ref this.paddingTop, ref this.paddingRight, ref this.paddingBottom);
      this.AssignRect(style.border, ref this.sliceLeft, ref this.sliceTop, ref this.sliceRight, ref this.sliceBottom);
      this.AssignState(style.normal);
      this.AssignState(style.focused);
      this.AssignState(style.hover);
      this.AssignState(style.active);
      this.AssignState(style.onNormal);
      this.AssignState(style.onFocused);
      this.AssignState(style.onHover);
      this.AssignState(style.onActive);
    }

    private void AssignState(GUIStyleState state)
    {
      state.textColor = this.textColor.GetSpecifiedValueOrDefault(state.textColor);
      if (!((UnityEngine.Object) this.backgroundImage.value != (UnityEngine.Object) null))
        return;
      state.background = this.backgroundImage.value;
      if (state.scaledBackgrounds == null || state.scaledBackgrounds.Length < 1 || (UnityEngine.Object) state.scaledBackgrounds[0] != (UnityEngine.Object) this.backgroundImage.value)
        state.scaledBackgrounds = new Texture2D[1]
        {
          this.backgroundImage.value
        };
    }

    private void AssignRect(RectOffset rect, ref StyleValue<int> left, ref StyleValue<int> top, ref StyleValue<int> right, ref StyleValue<int> bottom)
    {
      rect.left = left.GetSpecifiedValueOrDefault(rect.left);
      rect.top = top.GetSpecifiedValueOrDefault(rect.top);
      rect.right = right.GetSpecifiedValueOrDefault(rect.right);
      rect.bottom = bottom.GetSpecifiedValueOrDefault(rect.bottom);
    }

    private void AssignRect(RectOffset rect, ref StyleValue<float> left, ref StyleValue<float> top, ref StyleValue<float> right, ref StyleValue<float> bottom)
    {
      rect.left = (int) left.GetSpecifiedValueOrDefault((float) rect.left);
      rect.top = (int) top.GetSpecifiedValueOrDefault((float) rect.top);
      rect.right = (int) right.GetSpecifiedValueOrDefault((float) rect.right);
      rect.bottom = (int) bottom.GetSpecifiedValueOrDefault((float) rect.bottom);
    }

    internal void ApplyRule(StyleSheet registry, int specificity, StyleRule rule, StylePropertyID[] propertyIDs)
    {
      for (int index = 0; index < rule.properties.Length; ++index)
      {
        StyleProperty property = rule.properties[index];
        StylePropertyID propertyId = propertyIDs[index];
        StyleValueHandle[] values = property.values;
        switch (propertyId)
        {
          case StylePropertyID.MarginLeft:
            StyleSheet sheet1 = registry;
            StyleValueHandle[] handles1 = values;
            int specificity1 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache10 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache10 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache10 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache10;
            sheet1.Apply<float>(handles1, specificity1, ref this.marginLeft, fMgCache10);
            break;
          case StylePropertyID.MarginTop:
            StyleSheet sheet2 = registry;
            StyleValueHandle[] handles2 = values;
            int specificity2 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache11 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache11 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache11 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache11;
            sheet2.Apply<float>(handles2, specificity2, ref this.marginTop, fMgCache11);
            break;
          case StylePropertyID.MarginRight:
            StyleSheet sheet3 = registry;
            StyleValueHandle[] handles3 = values;
            int specificity3 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache12 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache12 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache12 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache12;
            sheet3.Apply<float>(handles3, specificity3, ref this.marginRight, fMgCache12);
            break;
          case StylePropertyID.MarginBottom:
            StyleSheet sheet4 = registry;
            StyleValueHandle[] handles4 = values;
            int specificity4 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache13 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache13 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache13 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache13;
            sheet4.Apply<float>(handles4, specificity4, ref this.marginBottom, fMgCache13);
            break;
          case StylePropertyID.PaddingLeft:
            StyleSheet sheet5 = registry;
            StyleValueHandle[] handles5 = values;
            int specificity5 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache19 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache19 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache19 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache19;
            sheet5.Apply<float>(handles5, specificity5, ref this.paddingLeft, fMgCache19);
            break;
          case StylePropertyID.PaddingTop:
            StyleSheet sheet6 = registry;
            StyleValueHandle[] handles6 = values;
            int specificity6 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1A == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1A = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache1A = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1A;
            sheet6.Apply<float>(handles6, specificity6, ref this.paddingTop, fMgCache1A);
            break;
          case StylePropertyID.PaddingRight:
            StyleSheet sheet7 = registry;
            StyleValueHandle[] handles7 = values;
            int specificity7 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1B == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1B = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache1B = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1B;
            sheet7.Apply<float>(handles7, specificity7, ref this.paddingRight, fMgCache1B);
            break;
          case StylePropertyID.PaddingBottom:
            StyleSheet sheet8 = registry;
            StyleValueHandle[] handles8 = values;
            int specificity8 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1C == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1C = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache1C = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1C;
            sheet8.Apply<float>(handles8, specificity8, ref this.paddingBottom, fMgCache1C);
            break;
          case StylePropertyID.BorderLeft:
            StyleSheet sheet9 = registry;
            StyleValueHandle[] handles9 = values;
            int specificity9 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache4 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache4 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache4;
            sheet9.Apply<float>(handles9, specificity9, ref this.borderLeft, fMgCache4);
            break;
          case StylePropertyID.BorderTop:
            StyleSheet sheet10 = registry;
            StyleValueHandle[] handles10 = values;
            int specificity10 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache5 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache5 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache5;
            sheet10.Apply<float>(handles10, specificity10, ref this.borderTop, fMgCache5);
            break;
          case StylePropertyID.BorderRight:
            StyleSheet sheet11 = registry;
            StyleValueHandle[] handles11 = values;
            int specificity11 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache6 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache6 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache6 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache6;
            sheet11.Apply<float>(handles11, specificity11, ref this.borderRight, fMgCache6);
            break;
          case StylePropertyID.BorderBottom:
            StyleSheet sheet12 = registry;
            StyleValueHandle[] handles12 = values;
            int specificity12 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache7 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache7 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache7 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache7;
            sheet12.Apply<float>(handles12, specificity12, ref this.borderBottom, fMgCache7);
            break;
          case StylePropertyID.PositionType:
            StyleSheet sheet13 = registry;
            StyleValueHandle[] handles13 = values;
            int specificity13 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1D == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1D = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<PositionType>);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCache1D = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1D;
            sheet13.Apply<int>(handles13, specificity13, ref this.positionType, fMgCache1D);
            break;
          case StylePropertyID.PositionLeft:
            StyleSheet sheet14 = registry;
            StyleValueHandle[] handles14 = values;
            int specificity14 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache20 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache20 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache20 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache20;
            sheet14.Apply<float>(handles14, specificity14, ref this.positionLeft, fMgCache20);
            break;
          case StylePropertyID.PositionTop:
            StyleSheet sheet15 = registry;
            StyleValueHandle[] handles15 = values;
            int specificity15 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1E == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1E = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache1E = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1E;
            sheet15.Apply<float>(handles15, specificity15, ref this.positionTop, fMgCache1E);
            break;
          case StylePropertyID.PositionRight:
            StyleSheet sheet16 = registry;
            StyleValueHandle[] handles16 = values;
            int specificity16 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache21 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache21 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache21 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache21;
            sheet16.Apply<float>(handles16, specificity16, ref this.positionRight, fMgCache21);
            break;
          case StylePropertyID.PositionBottom:
            StyleSheet sheet17 = registry;
            StyleValueHandle[] handles17 = values;
            int specificity17 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1F == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1F = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache1F = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1F;
            sheet17.Apply<float>(handles17, specificity17, ref this.positionBottom, fMgCache1F);
            break;
          case StylePropertyID.Width:
            StyleSheet sheet18 = registry;
            StyleValueHandle[] handles18 = values;
            int specificity18 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache25 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache25 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache25 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache25;
            sheet18.Apply<float>(handles18, specificity18, ref this.width, fMgCache25);
            break;
          case StylePropertyID.Height:
            StyleSheet sheet19 = registry;
            StyleValueHandle[] handles19 = values;
            int specificity19 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheE == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheE = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCacheE = VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheE;
            sheet19.Apply<float>(handles19, specificity19, ref this.height, fMgCacheE);
            break;
          case StylePropertyID.MinWidth:
            StyleSheet sheet20 = registry;
            StyleValueHandle[] handles20 = values;
            int specificity20 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache17 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache17 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache17 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache17;
            sheet20.Apply<float>(handles20, specificity20, ref this.minWidth, fMgCache17);
            break;
          case StylePropertyID.MinHeight:
            StyleSheet sheet21 = registry;
            StyleValueHandle[] handles21 = values;
            int specificity21 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache16 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache16 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache16 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache16;
            sheet21.Apply<float>(handles21, specificity21, ref this.minHeight, fMgCache16);
            break;
          case StylePropertyID.MaxWidth:
            StyleSheet sheet22 = registry;
            StyleValueHandle[] handles22 = values;
            int specificity22 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache15 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache15 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache15 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache15;
            sheet22.Apply<float>(handles22, specificity22, ref this.maxWidth, fMgCache15);
            break;
          case StylePropertyID.MaxHeight:
            StyleSheet sheet23 = registry;
            StyleValueHandle[] handles23 = values;
            int specificity23 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache14 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache14 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache14 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache14;
            sheet23.Apply<float>(handles23, specificity23, ref this.maxHeight, fMgCache14);
            break;
          case StylePropertyID.Flex:
            StyleSheet sheet24 = registry;
            StyleValueHandle[] handles24 = values;
            int specificity24 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache8 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache8 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache8 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache8;
            sheet24.Apply<float>(handles24, specificity24, ref this.flex, fMgCache8);
            break;
          case StylePropertyID.BorderLeftWidth:
            StyleSheet sheet25 = registry;
            StyleValueHandle[] handles25 = values;
            int specificity25 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2A == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2A = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache2A = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2A;
            sheet25.Apply<float>(handles25, specificity25, ref this.borderLeftWidth, fMgCache2A);
            break;
          case StylePropertyID.BorderTopWidth:
            StyleSheet sheet26 = registry;
            StyleValueHandle[] handles26 = values;
            int specificity26 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2B == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2B = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache2B = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2B;
            sheet26.Apply<float>(handles26, specificity26, ref this.borderTopWidth, fMgCache2B);
            break;
          case StylePropertyID.BorderRightWidth:
            StyleSheet sheet27 = registry;
            StyleValueHandle[] handles27 = values;
            int specificity27 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2C == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2C = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache2C = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2C;
            sheet27.Apply<float>(handles27, specificity27, ref this.borderRightWidth, fMgCache2C);
            break;
          case StylePropertyID.BorderBottomWidth:
            StyleSheet sheet28 = registry;
            StyleValueHandle[] handles28 = values;
            int specificity28 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2D == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2D = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache2D = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2D;
            sheet28.Apply<float>(handles28, specificity28, ref this.borderBottomWidth, fMgCache2D);
            break;
          case StylePropertyID.BorderTopLeftRadius:
            StyleSheet sheet29 = registry;
            StyleValueHandle[] handles29 = values;
            int specificity29 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2E == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2E = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache2E = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2E;
            sheet29.Apply<float>(handles29, specificity29, ref this.borderTopLeftRadius, fMgCache2E);
            break;
          case StylePropertyID.BorderTopRightRadius:
            StyleSheet sheet30 = registry;
            StyleValueHandle[] handles30 = values;
            int specificity30 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2F == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2F = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache2F = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2F;
            sheet30.Apply<float>(handles30, specificity30, ref this.borderTopRightRadius, fMgCache2F);
            break;
          case StylePropertyID.BorderBottomRightRadius:
            StyleSheet sheet31 = registry;
            StyleValueHandle[] handles31 = values;
            int specificity31 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache30 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache30 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache30 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache30;
            sheet31.Apply<float>(handles31, specificity31, ref this.borderBottomRightRadius, fMgCache30);
            break;
          case StylePropertyID.BorderBottomLeftRadius:
            StyleSheet sheet32 = registry;
            StyleValueHandle[] handles32 = values;
            int specificity32 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache31 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache31 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache31 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache31;
            sheet32.Apply<float>(handles32, specificity32, ref this.borderBottomLeftRadius, fMgCache31);
            break;
          case StylePropertyID.FlexDirection:
            StyleSheet sheet33 = registry;
            StyleValueHandle[] handles33 = values;
            int specificity33 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheC == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheC = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<FlexDirection>);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCacheC = VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheC;
            sheet33.Apply<int>(handles33, specificity33, ref this.flexDirection, fMgCacheC);
            break;
          case StylePropertyID.FlexWrap:
            StyleSheet sheet34 = registry;
            StyleValueHandle[] handles34 = values;
            int specificity34 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheD == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheD = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Wrap>);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCacheD = VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheD;
            sheet34.Apply<int>(handles34, specificity34, ref this.flexWrap, fMgCacheD);
            break;
          case StylePropertyID.JustifyContent:
            StyleSheet sheet35 = registry;
            StyleValueHandle[] handles35 = values;
            int specificity35 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheF == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheF = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Justify>);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCacheF = VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheF;
            sheet35.Apply<int>(handles35, specificity35, ref this.justifyContent, fMgCacheF);
            break;
          case StylePropertyID.AlignContent:
            StyleSheet sheet36 = registry;
            StyleValueHandle[] handles36 = values;
            int specificity36 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache0 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Align>);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCache0 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache0;
            sheet36.Apply<int>(handles36, specificity36, ref this.alignContent, fMgCache0);
            break;
          case StylePropertyID.AlignSelf:
            StyleSheet sheet37 = registry;
            StyleValueHandle[] handles37 = values;
            int specificity37 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Align>);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCache2 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache2;
            sheet37.Apply<int>(handles37, specificity37, ref this.alignSelf, fMgCache2);
            break;
          case StylePropertyID.AlignItems:
            StyleSheet sheet38 = registry;
            StyleValueHandle[] handles38 = values;
            int specificity38 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Align>);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCache1 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache1;
            sheet38.Apply<int>(handles38, specificity38, ref this.alignItems, fMgCache1);
            break;
          case StylePropertyID.TextAlignment:
            StyleSheet sheet39 = registry;
            StyleValueHandle[] handles39 = values;
            int specificity39 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache22 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache22 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<TextAnchor>);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCache22 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache22;
            sheet39.Apply<int>(handles39, specificity39, ref this.textAlignment, fMgCache22);
            break;
          case StylePropertyID.TextClipping:
            StyleSheet sheet40 = registry;
            StyleValueHandle[] handles40 = values;
            int specificity40 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache23 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache23 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<TextClipping>);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCache23 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache23;
            sheet40.Apply<int>(handles40, specificity40, ref this.textClipping, fMgCache23);
            break;
          case StylePropertyID.Font:
            StyleSheet sheet41 = registry;
            StyleValueHandle[] handles41 = values;
            int specificity41 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache9 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache9 = new HandlesApplicatorFunction<Font>(StyleSheetApplicator.ApplyResource<Font>);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<Font> fMgCache9 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache9;
            sheet41.Apply<Font>(handles41, specificity41, ref this.font, fMgCache9);
            break;
          case StylePropertyID.FontSize:
            StyleSheet sheet42 = registry;
            StyleValueHandle[] handles42 = values;
            int specificity42 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheA == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheA = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCacheA = VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheA;
            sheet42.Apply<int>(handles42, specificity42, ref this.fontSize, fMgCacheA);
            break;
          case StylePropertyID.FontStyle:
            StyleSheet sheet43 = registry;
            StyleValueHandle[] handles43 = values;
            int specificity43 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheB == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheB = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<FontStyle>);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCacheB = VisualElementStylesData.\u003C\u003Ef__mg\u0024cacheB;
            sheet43.Apply<int>(handles43, specificity43, ref this.fontStyle, fMgCacheB);
            break;
          case StylePropertyID.BackgroundSize:
            StyleSheet sheet44 = registry;
            StyleValueHandle[] handles44 = values;
            int specificity44 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache28 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache28 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCache28 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache28;
            sheet44.Apply<int>(handles44, specificity44, ref this.backgroundSize, fMgCache28);
            break;
          case StylePropertyID.WordWrap:
            StyleSheet sheet45 = registry;
            StyleValueHandle[] handles45 = values;
            int specificity45 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache26 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache26 = new HandlesApplicatorFunction<bool>(StyleSheetApplicator.ApplyBool);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<bool> fMgCache26 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache26;
            sheet45.Apply<bool>(handles45, specificity45, ref this.wordWrap, fMgCache26);
            break;
          case StylePropertyID.BackgroundImage:
            StyleSheet sheet46 = registry;
            StyleValueHandle[] handles46 = values;
            int specificity46 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3 = new HandlesApplicatorFunction<Texture2D>(StyleSheetApplicator.ApplyResource<Texture2D>);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<Texture2D> fMgCache3 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3;
            sheet46.Apply<Texture2D>(handles46, specificity46, ref this.backgroundImage, fMgCache3);
            break;
          case StylePropertyID.TextColor:
            StyleSheet sheet47 = registry;
            StyleValueHandle[] handles47 = values;
            int specificity47 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache24 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache24 = new HandlesApplicatorFunction<Color>(StyleSheetApplicator.ApplyColor);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<Color> fMgCache24 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache24;
            sheet47.Apply<Color>(handles47, specificity47, ref this.textColor, fMgCache24);
            break;
          case StylePropertyID.BackgroundColor:
            StyleSheet sheet48 = registry;
            StyleValueHandle[] handles48 = values;
            int specificity48 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache27 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache27 = new HandlesApplicatorFunction<Color>(StyleSheetApplicator.ApplyColor);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<Color> fMgCache27 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache27;
            sheet48.Apply<Color>(handles48, specificity48, ref this.backgroundColor, fMgCache27);
            break;
          case StylePropertyID.BorderColor:
            StyleSheet sheet49 = registry;
            StyleValueHandle[] handles49 = values;
            int specificity49 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache29 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache29 = new HandlesApplicatorFunction<Color>(StyleSheetApplicator.ApplyColor);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<Color> fMgCache29 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache29;
            sheet49.Apply<Color>(handles49, specificity49, ref this.borderColor, fMgCache29);
            break;
          case StylePropertyID.Overflow:
            StyleSheet sheet50 = registry;
            StyleValueHandle[] handles50 = values;
            int specificity50 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache18 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache18 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Overflow>);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCache18 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache18;
            sheet50.Apply<int>(handles50, specificity50, ref this.overflow, fMgCache18);
            break;
          case StylePropertyID.SliceLeft:
            StyleSheet sheet51 = registry;
            StyleValueHandle[] handles51 = values;
            int specificity51 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache32 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache32 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCache32 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache32;
            sheet51.Apply<int>(handles51, specificity51, ref this.sliceLeft, fMgCache32);
            break;
          case StylePropertyID.SliceTop:
            StyleSheet sheet52 = registry;
            StyleValueHandle[] handles52 = values;
            int specificity52 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache33 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache33 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCache33 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache33;
            sheet52.Apply<int>(handles52, specificity52, ref this.sliceTop, fMgCache33);
            break;
          case StylePropertyID.SliceRight:
            StyleSheet sheet53 = registry;
            StyleValueHandle[] handles53 = values;
            int specificity53 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache34 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache34 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCache34 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache34;
            sheet53.Apply<int>(handles53, specificity53, ref this.sliceRight, fMgCache34);
            break;
          case StylePropertyID.SliceBottom:
            StyleSheet sheet54 = registry;
            StyleValueHandle[] handles54 = values;
            int specificity54 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache35 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache35 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<int> fMgCache35 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache35;
            sheet54.Apply<int>(handles54, specificity54, ref this.sliceBottom, fMgCache35);
            break;
          case StylePropertyID.Opacity:
            StyleSheet sheet55 = registry;
            StyleValueHandle[] handles55 = values;
            int specificity55 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache36 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache36 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache36 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache36;
            sheet55.Apply<float>(handles55, specificity55, ref this.opacity, fMgCache36);
            break;
          case StylePropertyID.BorderRadius:
            StyleSheet sheet56 = registry;
            StyleValueHandle[] handles56 = values;
            int specificity56 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache37 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache37 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache37 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache37;
            sheet56.Apply<float>(handles56, specificity56, ref this.borderTopLeftRadius, fMgCache37);
            StyleSheet sheet57 = registry;
            StyleValueHandle[] handles57 = values;
            int specificity57 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache38 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache38 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache38 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache38;
            sheet57.Apply<float>(handles57, specificity57, ref this.borderTopRightRadius, fMgCache38);
            StyleSheet sheet58 = registry;
            StyleValueHandle[] handles58 = values;
            int specificity58 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache39 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache39 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache39 = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache39;
            sheet58.Apply<float>(handles58, specificity58, ref this.borderBottomLeftRadius, fMgCache39);
            StyleSheet sheet59 = registry;
            StyleValueHandle[] handles59 = values;
            int specificity59 = specificity;
            // ISSUE: reference to a compiler-generated field
            if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3A == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3A = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
            }
            // ISSUE: reference to a compiler-generated field
            HandlesApplicatorFunction<float> fMgCache3A = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3A;
            sheet59.Apply<float>(handles59, specificity59, ref this.borderBottomRightRadius, fMgCache3A);
            break;
          case StylePropertyID.Custom:
            if (this.m_CustomProperties == null)
              this.m_CustomProperties = new Dictionary<string, CustomProperty>();
            CustomProperty customProperty = new CustomProperty();
            if (!this.m_CustomProperties.TryGetValue(property.name, out customProperty) || specificity >= customProperty.specificity)
            {
              customProperty.handles = values;
              customProperty.data = registry;
              customProperty.specificity = specificity;
              this.m_CustomProperties[property.name] = customProperty;
              break;
            }
            break;
          default:
            throw new ArgumentException(string.Format("Non exhaustive switch statement (value={0})", (object) propertyId));
        }
      }
    }

    public void ApplyCustomProperty(string propertyName, ref StyleValue<float> target)
    {
      string propertyName1 = propertyName;
      int num = 1;
      // ISSUE: reference to a compiler-generated field
      if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3B == null)
      {
        // ISSUE: reference to a compiler-generated field
        VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3B = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
      }
      // ISSUE: reference to a compiler-generated field
      HandlesApplicatorFunction<float> fMgCache3B = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3B;
      this.ApplyCustomProperty<float>(propertyName1, ref target, (StyleValueType) num, fMgCache3B);
    }

    public void ApplyCustomProperty(string propertyName, ref StyleValue<int> target)
    {
      string propertyName1 = propertyName;
      int num = 1;
      // ISSUE: reference to a compiler-generated field
      if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3C == null)
      {
        // ISSUE: reference to a compiler-generated field
        VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3C = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
      }
      // ISSUE: reference to a compiler-generated field
      HandlesApplicatorFunction<int> fMgCache3C = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3C;
      this.ApplyCustomProperty<int>(propertyName1, ref target, (StyleValueType) num, fMgCache3C);
    }

    public void ApplyCustomProperty(string propertyName, ref StyleValue<bool> target)
    {
      string propertyName1 = propertyName;
      int num = 0;
      // ISSUE: reference to a compiler-generated field
      if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3D == null)
      {
        // ISSUE: reference to a compiler-generated field
        VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3D = new HandlesApplicatorFunction<bool>(StyleSheetApplicator.ApplyBool);
      }
      // ISSUE: reference to a compiler-generated field
      HandlesApplicatorFunction<bool> fMgCache3D = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3D;
      this.ApplyCustomProperty<bool>(propertyName1, ref target, (StyleValueType) num, fMgCache3D);
    }

    public void ApplyCustomProperty(string propertyName, ref StyleValue<Color> target)
    {
      string propertyName1 = propertyName;
      int num = 2;
      // ISSUE: reference to a compiler-generated field
      if (VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3E == null)
      {
        // ISSUE: reference to a compiler-generated field
        VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3E = new HandlesApplicatorFunction<Color>(StyleSheetApplicator.ApplyColor);
      }
      // ISSUE: reference to a compiler-generated field
      HandlesApplicatorFunction<Color> fMgCache3E = VisualElementStylesData.\u003C\u003Ef__mg\u0024cache3E;
      this.ApplyCustomProperty<Color>(propertyName1, ref target, (StyleValueType) num, fMgCache3E);
    }

    public void ApplyCustomProperty<T>(string propertyName, ref StyleValue<T> target) where T : UnityEngine.Object
    {
      this.ApplyCustomProperty<T>(propertyName, ref target, StyleValueType.ResourcePath, new HandlesApplicatorFunction<T>(StyleSheetApplicator.ApplyResource<T>));
    }

    public void ApplyCustomProperty(string propertyName, ref StyleValue<string> target)
    {
      StyleValue<string> other = new StyleValue<string>(string.Empty);
      CustomProperty customProperty;
      if (this.m_CustomProperties != null && this.m_CustomProperties.TryGetValue(propertyName, out customProperty))
      {
        other.value = customProperty.data.ReadAsString(customProperty.handles[0]);
        other.specificity = customProperty.specificity;
      }
      target.Apply(other, StylePropertyApplyMode.CopyIfNotInline);
    }

    internal void ApplyCustomProperty<T>(string propertyName, ref StyleValue<T> target, StyleValueType valueType, HandlesApplicatorFunction<T> applicatorFunc)
    {
      StyleValue<T> property = new StyleValue<T>();
      CustomProperty customProperty;
      if (this.m_CustomProperties != null && this.m_CustomProperties.TryGetValue(propertyName, out customProperty))
      {
        StyleValueHandle handle = customProperty.handles[0];
        if (handle.valueType == valueType)
          customProperty.data.Apply<T>(customProperty.handles, customProperty.specificity, ref property, applicatorFunc);
        else
          Debug.LogWarning((object) string.Format("Trying to read value as {0} while parsed type is {1}", (object) valueType, (object) handle.valueType));
      }
      target.Apply(property, StylePropertyApplyMode.CopyIfNotInline);
    }
  }
}
