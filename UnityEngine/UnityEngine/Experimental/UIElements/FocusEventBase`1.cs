// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.FocusEventBase`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>Base class for focus related events.</para>
  /// </summary>
  public abstract class FocusEventBase<T> : EventBase<T>, IFocusEvent, IPropagatableEvent where T : FocusEventBase<T>, new()
  {
    protected FocusEventBase()
    {
      this.Init();
    }

    public Focusable relatedTarget { get; protected set; }

    public FocusChangeDirection direction { get; protected set; }

    protected override void Init()
    {
      base.Init();
      this.flags = EventBase.EventFlags.Capturable;
      this.relatedTarget = (Focusable) null;
      this.direction = FocusChangeDirection.unspecified;
    }

    public static T GetPooled(IEventHandler target, Focusable relatedTarget, FocusChangeDirection direction)
    {
      T pooled = EventBase<T>.GetPooled();
      pooled.target = target;
      pooled.relatedTarget = relatedTarget;
      pooled.direction = direction;
      return pooled;
    }
  }
}
