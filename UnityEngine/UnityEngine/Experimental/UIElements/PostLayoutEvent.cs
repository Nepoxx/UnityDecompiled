// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.PostLayoutEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>Event sent after the layout is done in a tree. Non-capturable, does not bubble, non-cancellable.</para>
  /// </summary>
  public class PostLayoutEvent : EventBase<PostLayoutEvent>, IPropagatableEvent
  {
    /// <summary>
    ///   <para>Constructor. Avoid newing events. Instead, use GetPooled() to get an event from a pool of reusable events.</para>
    /// </summary>
    public PostLayoutEvent()
    {
      this.Init();
    }

    /// <summary>
    ///   <para>Get an event from the event pool and initialize it with the given values. Use this function instead of newing events. Events obtained from this function should be released back to the pool using ReleaseEvent().</para>
    /// </summary>
    /// <param name="hasNewLayout">Whether the target layout changed.</param>
    /// <returns>
    ///   <para>An event.</para>
    /// </returns>
    public static PostLayoutEvent GetPooled(bool hasNewLayout)
    {
      PostLayoutEvent pooled = EventBase<PostLayoutEvent>.GetPooled();
      pooled.hasNewLayout = hasNewLayout;
      return pooled;
    }

    /// <summary>
    ///   <para>Reset the event members to their initial value.</para>
    /// </summary>
    protected override void Init()
    {
      base.Init();
      this.hasNewLayout = false;
    }

    /// <summary>
    ///   <para>True if the layout of the element has changed.</para>
    /// </summary>
    public bool hasNewLayout { get; private set; }
  }
}
