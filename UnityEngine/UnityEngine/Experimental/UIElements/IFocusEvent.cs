// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.IFocusEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  public interface IFocusEvent
  {
    /// <summary>
    ///   <para>Related target. See implementation for specific meaning.</para>
    /// </summary>
    Focusable relatedTarget { get; }

    /// <summary>
    ///   <para>Direction of the focus change.</para>
    /// </summary>
    FocusChangeDirection direction { get; }
  }
}
