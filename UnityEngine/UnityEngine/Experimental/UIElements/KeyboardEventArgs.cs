// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.KeyboardEventArgs
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  internal struct KeyboardEventArgs
  {
    private readonly EventModifiers m_Modifiers;

    public KeyboardEventArgs(char character, KeyCode keyCode, EventModifiers modifiers)
    {
      this = new KeyboardEventArgs();
      this.character = character;
      this.keyCode = keyCode;
      this.m_Modifiers = modifiers;
    }

    public char character { get; private set; }

    public KeyCode keyCode { get; private set; }

    public bool shift
    {
      get
      {
        return (this.m_Modifiers & EventModifiers.Shift) != EventModifiers.None;
      }
    }

    public bool alt
    {
      get
      {
        return (this.m_Modifiers & EventModifiers.Alt) != EventModifiers.None;
      }
    }

    public Event ToEvent()
    {
      return new Event() { character = this.character, keyCode = this.keyCode, modifiers = this.m_Modifiers, type = EventType.KeyDown };
    }
  }
}
