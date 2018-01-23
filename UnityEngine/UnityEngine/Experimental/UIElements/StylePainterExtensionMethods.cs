// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.StylePainterExtensionMethods
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  internal static class StylePainterExtensionMethods
  {
    internal static TextureStylePainterParameters GetDefaultTextureParameters(this IStylePainter painter, VisualElement ve)
    {
      IStyle style = ve.style;
      return new TextureStylePainterParameters() { layout = ve.layout, color = Color.white, texture = (Texture) (Texture2D) style.backgroundImage, scaleMode = (ScaleMode) style.backgroundSize, borderLeftWidth = (float) style.borderLeftWidth, borderTopWidth = (float) style.borderTopWidth, borderRightWidth = (float) style.borderRightWidth, borderBottomWidth = (float) style.borderBottomWidth, borderTopLeftRadius = (float) style.borderTopLeftRadius, borderTopRightRadius = (float) style.borderTopRightRadius, borderBottomRightRadius = (float) style.borderBottomRightRadius, borderBottomLeftRadius = (float) style.borderBottomLeftRadius, sliceLeft = (int) style.sliceLeft, sliceTop = (int) style.sliceTop, sliceRight = (int) style.sliceRight, sliceBottom = (int) style.sliceBottom };
    }

    internal static RectStylePainterParameters GetDefaultRectParameters(this IStylePainter painter, VisualElement ve)
    {
      IStyle style = ve.style;
      return new RectStylePainterParameters() { layout = ve.layout, color = (Color) style.backgroundColor, borderLeftWidth = (float) style.borderLeftWidth, borderTopWidth = (float) style.borderTopWidth, borderRightWidth = (float) style.borderRightWidth, borderBottomWidth = (float) style.borderBottomWidth, borderTopLeftRadius = (float) style.borderTopLeftRadius, borderTopRightRadius = (float) style.borderTopRightRadius, borderBottomRightRadius = (float) style.borderBottomRightRadius, borderBottomLeftRadius = (float) style.borderBottomLeftRadius };
    }

    internal static TextStylePainterParameters GetDefaultTextParameters(this IStylePainter painter, VisualElement ve)
    {
      IStyle style = ve.style;
      return new TextStylePainterParameters() { layout = ve.contentRect, text = ve.text, font = (Font) style.font, fontSize = (int) style.fontSize, fontStyle = (FontStyle) style.fontStyle, fontColor = style.textColor.GetSpecifiedValueOrDefault(Color.black), anchor = (TextAnchor) style.textAlignment, wordWrap = (bool) style.wordWrap, wordWrapWidth = !(bool) style.wordWrap ? 0.0f : ve.contentRect.width, richText = false, clipping = (TextClipping) style.textClipping };
    }

    internal static CursorPositionStylePainterParameters GetDefaultCursorPositionParameters(this IStylePainter painter, VisualElement ve)
    {
      IStyle style = ve.style;
      return new CursorPositionStylePainterParameters() { layout = ve.contentRect, text = ve.text, font = (Font) style.font, fontSize = (int) style.fontSize, fontStyle = (FontStyle) style.fontStyle, anchor = (TextAnchor) style.textAlignment, wordWrapWidth = !(bool) style.wordWrap ? 0.0f : ve.contentRect.width, richText = false, cursorIndex = 0 };
    }

    internal static void DrawBackground(this IStylePainter painter, VisualElement ve)
    {
      IStyle style = ve.style;
      if ((Color) style.backgroundColor != Color.clear)
      {
        RectStylePainterParameters defaultRectParameters = painter.GetDefaultRectParameters(ve);
        defaultRectParameters.borderLeftWidth = 0.0f;
        defaultRectParameters.borderTopWidth = 0.0f;
        defaultRectParameters.borderRightWidth = 0.0f;
        defaultRectParameters.borderBottomWidth = 0.0f;
        painter.DrawRect(defaultRectParameters);
      }
      if (!((Object) style.backgroundImage.value != (Object) null))
        return;
      TextureStylePainterParameters textureParameters = painter.GetDefaultTextureParameters(ve);
      textureParameters.borderLeftWidth = 0.0f;
      textureParameters.borderTopWidth = 0.0f;
      textureParameters.borderRightWidth = 0.0f;
      textureParameters.borderBottomWidth = 0.0f;
      painter.DrawTexture(textureParameters);
    }

    internal static void DrawBorder(this IStylePainter painter, VisualElement ve)
    {
      IStyle style = ve.style;
      if (!((Color) style.borderColor != Color.clear) || (double) (float) style.borderLeftWidth <= 0.0 && (double) (float) style.borderTopWidth <= 0.0 && ((double) (float) style.borderRightWidth <= 0.0 && (double) (float) style.borderBottomWidth <= 0.0))
        return;
      RectStylePainterParameters defaultRectParameters = painter.GetDefaultRectParameters(ve);
      defaultRectParameters.color = (Color) style.borderColor;
      painter.DrawRect(defaultRectParameters);
    }

    internal static void DrawText(this IStylePainter painter, VisualElement ve)
    {
      if (string.IsNullOrEmpty(ve.text) || (double) ve.contentRect.width <= 0.0 || (double) ve.contentRect.height <= 0.0)
        return;
      painter.DrawText(painter.GetDefaultTextParameters(ve));
    }
  }
}
