// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.FocusInEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>Event sent immediately before an element gains focus. Capturable, bubbles, non-cancellable.</para>
  /// </summary>
  public class FocusInEvent : FocusEventBase<FocusInEvent>
  {
    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    public FocusInEvent()
    {
      this.Init();
    }

    /// <summary>
    ///   <para>Reset the event members to their initial value.</para>
    /// </summary>
    protected override void Init()
    {
      base.Init();
      this.flags = EventBase.EventFlags.Bubbles | EventBase.EventFlags.Capturable;
    }
  }
}
