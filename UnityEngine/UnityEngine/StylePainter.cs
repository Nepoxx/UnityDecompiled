// Decompiled with JetBrains decompiler
// Type: UnityEngine.StylePainter
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class StylePainter : IStylePainter
  {
    private Color m_OpacityColor = Color.white;
    [NonSerialized]
    internal IntPtr m_Ptr;

    public StylePainter()
    {
      this.Init();
    }

    public StylePainter(Vector2 pos)
      : this()
    {
      this.mousePosition = pos;
    }

    public void DrawRect(RectStylePainterParameters painterParams)
    {
      this.DrawRect_Internal(painterParams.layout, painterParams.color * this.m_OpacityColor, new Vector4(painterParams.borderLeftWidth, painterParams.borderTopWidth, painterParams.borderRightWidth, painterParams.borderBottomWidth), new Vector4(painterParams.borderTopLeftRadius, painterParams.borderTopRightRadius, painterParams.borderBottomRightRadius, painterParams.borderBottomLeftRadius));
    }

    public void DrawTexture(TextureStylePainterParameters painterParams)
    {
      Rect layout = painterParams.layout;
      Texture texture = painterParams.texture;
      Color color = painterParams.color;
      ScaleMode scaleMode = painterParams.scaleMode;
      int sliceLeft = painterParams.sliceLeft;
      int sliceTop = painterParams.sliceTop;
      int sliceRight = painterParams.sliceRight;
      int sliceBottom = painterParams.sliceBottom;
      Rect screenRect = layout;
      Rect sourceRect = new Rect(0.0f, 0.0f, 1f, 1f);
      float num1 = (float) texture.width / (float) texture.height;
      float num2 = layout.width / layout.height;
      switch (scaleMode)
      {
        case ScaleMode.ScaleAndCrop:
          if ((double) num2 > (double) num1)
          {
            float height = num1 / num2;
            sourceRect = new Rect(0.0f, (float) ((1.0 - (double) height) * 0.5), 1f, height);
            break;
          }
          float width = num2 / num1;
          sourceRect = new Rect((float) (0.5 - (double) width * 0.5), 0.0f, width, 1f);
          break;
        case ScaleMode.ScaleToFit:
          if ((double) num2 > (double) num1)
          {
            float num3 = num1 / num2;
            screenRect = new Rect(layout.xMin + (float) ((double) layout.width * (1.0 - (double) num3) * 0.5), layout.yMin, num3 * layout.width, layout.height);
            break;
          }
          float num4 = num2 / num1;
          screenRect = new Rect(layout.xMin, layout.yMin + (float) ((double) layout.height * (1.0 - (double) num4) * 0.5), layout.width, num4 * layout.height);
          break;
      }
      Vector4 borderWidths = new Vector4(painterParams.borderLeftWidth, painterParams.borderTopWidth, painterParams.borderRightWidth, painterParams.borderBottomWidth);
      Vector4 borderRadiuses = new Vector4(painterParams.borderTopLeftRadius, painterParams.borderTopRightRadius, painterParams.borderBottomRightRadius, painterParams.borderBottomLeftRadius);
      this.DrawTexture_Internal(screenRect, texture, sourceRect, color * this.m_OpacityColor, borderWidths, borderRadiuses, sliceLeft, sliceTop, sliceRight, sliceBottom);
    }

    public void DrawText(TextStylePainterParameters painterParams)
    {
      this.DrawText_Internal(painterParams.layout, painterParams.text, painterParams.font, painterParams.fontSize, painterParams.fontStyle, painterParams.fontColor * this.m_OpacityColor, painterParams.anchor, painterParams.wordWrap, painterParams.wordWrapWidth, painterParams.richText, painterParams.clipping);
    }

    public Vector2 GetCursorPosition(CursorPositionStylePainterParameters painterParams)
    {
      Font font = painterParams.font;
      if ((Object) font == (Object) null)
      {
        Debug.LogError((object) "StylePainter: Can't process a null font.");
        return Vector2.zero;
      }
      string text = painterParams.text;
      int fontSize = painterParams.fontSize;
      FontStyle fontStyle = painterParams.fontStyle;
      TextAnchor anchor = painterParams.anchor;
      float wordWrapWidth = painterParams.wordWrapWidth;
      bool richText = painterParams.richText;
      Rect layout = painterParams.layout;
      int cursorIndex = painterParams.cursorIndex;
      return this.GetCursorPosition_Internal(text, font, fontSize, fontStyle, anchor, wordWrapWidth, richText, layout, cursorIndex);
    }

    public float ComputeTextWidth(TextStylePainterParameters painterParams)
    {
      return this.ComputeTextWidth_Internal(painterParams.text, painterParams.wordWrapWidth, painterParams.wordWrap, painterParams.font, painterParams.fontSize, painterParams.fontStyle, painterParams.anchor, painterParams.richText);
    }

    public float ComputeTextHeight(TextStylePainterParameters painterParams)
    {
      return this.ComputeTextHeight_Internal(painterParams.text, painterParams.wordWrapWidth, painterParams.wordWrap, painterParams.font, painterParams.fontSize, painterParams.fontStyle, painterParams.anchor, painterParams.richText);
    }

    public Matrix4x4 currentTransform { get; set; }

    public Vector2 mousePosition { get; set; }

    public Rect currentWorldClip { get; set; }

    public Event repaintEvent { get; set; }

    public float opacity
    {
      get
      {
        return this.m_OpacityColor.a;
      }
      set
      {
        this.m_OpacityColor.a = value;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Init();

    internal void DrawRect_Internal(Rect screenRect, Color color, Vector4 borderWidths, Vector4 borderRadiuses)
    {
      StylePainter.INTERNAL_CALL_DrawRect_Internal(this, ref screenRect, ref color, ref borderWidths, ref borderRadiuses);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_DrawRect_Internal(StylePainter self, ref Rect screenRect, ref Color color, ref Vector4 borderWidths, ref Vector4 borderRadiuses);

    internal void DrawTexture_Internal(Rect screenRect, Texture texture, Rect sourceRect, Color color, Vector4 borderWidths, Vector4 borderRadiuses, int leftBorder, int topBorder, int rightBorder, int bottomBorder)
    {
      StylePainter.INTERNAL_CALL_DrawTexture_Internal(this, ref screenRect, texture, ref sourceRect, ref color, ref borderWidths, ref borderRadiuses, leftBorder, topBorder, rightBorder, bottomBorder);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_DrawTexture_Internal(StylePainter self, ref Rect screenRect, Texture texture, ref Rect sourceRect, ref Color color, ref Vector4 borderWidths, ref Vector4 borderRadiuses, int leftBorder, int topBorder, int rightBorder, int bottomBorder);

    internal void DrawText_Internal(Rect screenRect, string text, Font font, int fontSize, FontStyle fontStyle, Color fontColor, TextAnchor anchor, bool wordWrap, float wordWrapWidth, bool richText, TextClipping textClipping)
    {
      StylePainter.INTERNAL_CALL_DrawText_Internal(this, ref screenRect, text, font, fontSize, fontStyle, ref fontColor, anchor, wordWrap, wordWrapWidth, richText, textClipping);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_DrawText_Internal(StylePainter self, ref Rect screenRect, string text, Font font, int fontSize, FontStyle fontStyle, ref Color fontColor, TextAnchor anchor, bool wordWrap, float wordWrapWidth, bool richText, TextClipping textClipping);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float ComputeTextWidth_Internal(string text, float width, bool wordWrap, Font font, int fontSize, FontStyle fontStyle, TextAnchor anchor, bool richText);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float ComputeTextHeight_Internal(string text, float width, bool wordWrap, Font font, int fontSize, FontStyle fontStyle, TextAnchor anchor, bool richText);

    public Vector2 GetCursorPosition_Internal(string text, Font font, int fontSize, FontStyle fontStyle, TextAnchor anchor, float wordWrapWidth, bool richText, Rect screenRect, int cursorPosition)
    {
      Vector2 vector2;
      StylePainter.INTERNAL_CALL_GetCursorPosition_Internal(this, text, font, fontSize, fontStyle, anchor, wordWrapWidth, richText, ref screenRect, cursorPosition, out vector2);
      return vector2;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetCursorPosition_Internal(StylePainter self, string text, Font font, int fontSize, FontStyle fontStyle, TextAnchor anchor, float wordWrapWidth, bool richText, ref Rect screenRect, int cursorPosition, out Vector2 value);
  }
}
