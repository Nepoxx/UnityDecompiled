// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.IEventDispatcher
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  public interface IEventDispatcher
  {
    /// <summary>
    ///   <para>The element capturing the mouse, if any.</para>
    /// </summary>
    IEventHandler capture { get; }

    /// <summary>
    ///   <para>Release the capture.</para>
    /// </summary>
    /// <param name="handler"></param>
    void ReleaseCapture(IEventHandler handler);

    /// <summary>
    ///   <para>Release capture and notify capturing element.</para>
    /// </summary>
    void RemoveCapture();

    /// <summary>
    ///   <para>Take the capture.</para>
    /// </summary>
    /// <param name="handler">The element that takes the capture.</param>
    void TakeCapture(IEventHandler handler);

    /// <summary>
    ///   <para>Dispatch an event to the panel.</para>
    /// </summary>
    /// <param name="evt">The event to dispatch.</param>
    /// <param name="panel">The panel where the event will be dispatched.</param>
    void DispatchEvent(EventBase evt, IPanel panel);
  }
}
