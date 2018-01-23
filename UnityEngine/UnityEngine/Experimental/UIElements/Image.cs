// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.Image
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  public class Image : VisualElement
  {
    public Image()
    {
      this.scaleMode = ScaleMode.ScaleAndCrop;
    }

    public Texture image { get; set; }

    public ScaleMode scaleMode { get; set; }

    protected internal override Vector2 DoMeasure(float width, VisualElement.MeasureMode widthMode, float height, VisualElement.MeasureMode heightMode)
    {
      float x = float.NaN;
      float y = float.NaN;
      if ((Object) this.image == (Object) null)
        return new Vector2(x, y);
      float num1 = (float) this.image.width;
      float num2 = (float) this.image.height;
      if (widthMode == VisualElement.MeasureMode.AtMost)
        num1 = Mathf.Min(num1, width);
      if (heightMode == VisualElement.MeasureMode.AtMost)
        num2 = Mathf.Min(num2, height);
      return new Vector2(num1, num2);
    }

    internal override void DoRepaint(IStylePainter painter)
    {
      if ((Object) this.image == (Object) null)
      {
        Debug.LogWarning((object) "null texture passed to GUI.DrawTexture");
      }
      else
      {
        TextureStylePainterParameters painterParams = new TextureStylePainterParameters() { layout = this.contentRect, texture = this.image, color = GUI.color, scaleMode = this.scaleMode };
        painter.DrawTexture(painterParams);
      }
    }
  }
}
