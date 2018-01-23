// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.FocusOutEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>Event sent immediately before an element loses focus. Capturable, bubbles, non-cancellable.</para>
  /// </summary>
  public class FocusOutEvent : FocusEventBase<FocusOutEvent>
  {
    /// <summary>
    ///   <para>Constructor. Avoid newing events. Instead, use GetPooled() to get an event from a pool of reusable events.</para>
    /// </summary>
    public FocusOutEvent()
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
