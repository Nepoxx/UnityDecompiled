// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.KeyboardEventBase`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>Base class for keyboard events.</para>
  /// </summary>
  public abstract class KeyboardEventBase<T> : EventBase<T>, IKeyboardEvent where T : KeyboardEventBase<T>, new()
  {
    protected KeyboardEventBase()
    {
      this.Init();
    }

    public EventModifiers modifiers { get; protected set; }

    public char character { get; protected set; }

    public KeyCode keyCode { get; protected set; }

    public bool shiftKey
    {
      get
      {
        return (this.modifiers & EventModifiers.Shift) != EventModifiers.None;
      }
    }

    public bool ctrlKey
    {
      get
      {
        return (this.modifiers & EventModifiers.Control) != EventModifiers.None;
      }
    }

    public bool commandKey
    {
      get
      {
        return (this.modifiers & EventModifiers.Command) != EventModifiers.None;
      }
    }

    public bool altKey
    {
      get
      {
        return (this.modifiers & EventModifiers.Alt) != EventModifiers.None;
      }
    }

    protected override void Init()
    {
      base.Init();
      this.flags = EventBase.EventFlags.Bubbles | EventBase.EventFlags.Capturable | EventBase.EventFlags.Cancellable;
      this.modifiers = EventModifiers.None;
      this.character = char.MinValue;
      this.keyCode = KeyCode.None;
    }

    public static T GetPooled(char c, KeyCode keyCode, EventModifiers modifiers)
    {
      T pooled = EventBase<T>.GetPooled();
      pooled.modifiers = modifiers;
      pooled.character = c;
      pooled.keyCode = keyCode;
      return pooled;
    }

    public static T GetPooled(Event systemEvent)
    {
      T pooled = EventBase<T>.GetPooled();
      pooled.imguiEvent = systemEvent;
      if (systemEvent != null)
      {
        pooled.modifiers = systemEvent.modifiers;
        pooled.character = systemEvent.character;
        pooled.keyCode = systemEvent.keyCode;
      }
      return pooled;
    }
  }
}
