// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.IFocusRing
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  public interface IFocusRing
  {
    /// <summary>
    ///   <para>Get the direction of the focus change for the given event. For example, when the Tab key is pressed, focus should be given to the element to the right.</para>
    /// </summary>
    /// <param name="currentFocusable"></param>
    /// <param name="e"></param>
    FocusChangeDirection GetFocusChangeDirection(Focusable currentFocusable, EventBase e);

    /// <summary>
    ///   <para>Get the next element in the given direction.</para>
    /// </summary>
    /// <param name="currentFocusable"></param>
    /// <param name="direction"></param>
    Focusable GetNextFocusable(Focusable currentFocusable, FocusChangeDirection direction);
  }
}
