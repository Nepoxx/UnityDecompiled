// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.IEventHandler
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  public interface IEventHandler
  {
    /// <summary>
    ///   <para>Callback executed when the event handler loses the capture.</para>
    /// </summary>
    void OnLostCapture();

    /// <summary>
    ///   <para>Handle an event.</para>
    /// </summary>
    /// <param name="evt">The event to handle.</param>
    void HandleEvent(EventBase evt);

    /// <summary>
    ///   <para>Return true if event handlers for the event propagation capture phase have been attached on this object.</para>
    /// </summary>
    /// <returns>
    ///   <para>True if object has event handlers for the capture phase.</para>
    /// </returns>
    bool HasCaptureHandlers();

    /// <summary>
    ///   <para>Return true if event handlers for the event propagation bubble up phase have been attached on this object.</para>
    /// </summary>
    /// <returns>
    ///   <para>True if object has event handlers for the bubble up phase.</para>
    /// </returns>
    bool HasBubbleHandlers();
  }
}
