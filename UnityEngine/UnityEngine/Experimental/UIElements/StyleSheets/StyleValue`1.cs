// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.StyleSheets.StyleValue`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
  /// <summary>
  ///   <para>This generic structure encodes a value type that can come from USS or be specified programmatically.</para>
  /// </summary>
  public struct StyleValue<T>
  {
    private static readonly StyleValue<T> defaultStyle = new StyleValue<T>();
    internal int specificity;
    public T value;

    public StyleValue(T value)
    {
      this.value = value;
      this.specificity = 0;
    }

    internal StyleValue(T value, int specifity)
    {
      this.value = value;
      this.specificity = specifity;
    }

    public static StyleValue<T> nil
    {
      get
      {
        return StyleValue<T>.defaultStyle;
      }
    }

    public T GetSpecifiedValueOrDefault(T defaultValue)
    {
      if (this.specificity > 0)
        defaultValue = this.value;
      return defaultValue;
    }

    public static implicit operator T(StyleValue<T> sp)
    {
      return sp.value;
    }

    internal bool Apply(StyleValue<T> other, StylePropertyApplyMode mode)
    {
      return this.Apply(other.value, other.specificity, mode);
    }

    internal bool Apply(T otherValue, int otherSpecificity, StylePropertyApplyMode mode)
    {
      switch (mode)
      {
        case StylePropertyApplyMode.Copy:
          this.value = otherValue;
          this.specificity = otherSpecificity;
          return true;
        case StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity:
          if (otherSpecificity < this.specificity)
            return false;
          this.value = otherValue;
          this.specificity = otherSpecificity;
          return true;
        case StylePropertyApplyMode.CopyIfNotInline:
          if (this.specificity >= int.MaxValue)
            return false;
          this.value = otherValue;
          this.specificity = otherSpecificity;
          return true;
        default:
          Debug.Assert(false, "Invalid mode " + (object) mode);
          return false;
      }
    }

    public static implicit operator StyleValue<T>(T value)
    {
      return StyleValue<T>.Create(value);
    }

    public static StyleValue<T> Create(T value)
    {
      return new StyleValue<T>(value, int.MaxValue);
    }

    public override string ToString()
    {
      return string.Format("[StyleProperty<{2}>: specifity={0}, value={1}]", (object) this.specificity, (object) this.value, (object) typeof (T).Name);
    }
  }
}
