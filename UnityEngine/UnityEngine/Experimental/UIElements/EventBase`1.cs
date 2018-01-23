// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.EventBase`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>Generic base class for events, implementing event pooling and automatic registration to the event type system.</para>
  /// </summary>
  public abstract class EventBase<T> : EventBase where T : EventBase<T>, new()
  {
    private static readonly long s_TypeId = EventBase.RegisterEventType();
    private static readonly EventPool<T> s_Pool = new EventPool<T>();

    public static long TypeId()
    {
      return EventBase<T>.s_TypeId;
    }

    public static T GetPooled()
    {
      T obj = EventBase<T>.s_Pool.Get();
      obj.Init();
      return obj;
    }

    public static void ReleasePooled(T evt)
    {
      EventBase<T>.s_Pool.Release(evt);
    }

    public override long GetEventTypeId()
    {
      return EventBase<T>.s_TypeId;
    }
  }
}
