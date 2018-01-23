// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.StyleSheets.ICustomStyle
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
  public interface ICustomStyle
  {
    void ApplyCustomProperty(string propertyName, ref StyleValue<float> target);

    void ApplyCustomProperty(string propertyName, ref StyleValue<int> target);

    void ApplyCustomProperty(string propertyName, ref StyleValue<bool> target);

    void ApplyCustomProperty(string propertyName, ref StyleValue<Color> target);

    void ApplyCustomProperty<T>(string propertyName, ref StyleValue<T> target) where T : Object;

    void ApplyCustomProperty(string propertyName, ref StyleValue<string> target);
  }
}
