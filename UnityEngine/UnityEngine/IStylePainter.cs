// Decompiled with JetBrains decompiler
// Type: UnityEngine.IStylePainter
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  internal interface IStylePainter
  {
    void DrawRect(RectStylePainterParameters painterParams);

    void DrawTexture(TextureStylePainterParameters painterParams);

    void DrawText(TextStylePainterParameters painterParams);

    Vector2 GetCursorPosition(CursorPositionStylePainterParameters painterParams);

    Rect currentWorldClip { get; set; }

    Vector2 mousePosition { get; set; }

    Matrix4x4 currentTransform { get; set; }

    Event repaintEvent { get; set; }

    float opacity { get; set; }

    float ComputeTextWidth(TextStylePainterParameters painterParams);

    float ComputeTextHeight(TextStylePainterParameters painterParams);
  }
}
