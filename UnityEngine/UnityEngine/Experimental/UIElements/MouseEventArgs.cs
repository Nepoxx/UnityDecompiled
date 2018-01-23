// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.MouseEventArgs
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  internal struct MouseEventArgs
  {
    private readonly EventModifiers m_Modifiers;

    public MouseEventArgs(Vector2 pos, int clickCount, EventModifiers modifiers)
    {
      this = new MouseEventArgs();
      this.mousePosition = pos;
      this.clickCount = clickCount;
      this.m_Modifiers = modifiers;
    }

    public Vector2 mousePosition { get; private set; }

    public int clickCount { get; private set; }

    public bool shift
    {
      get
      {
        return (this.m_Modifiers & EventModifiers.Shift) != EventModifiers.None;
      }
    }
  }
}
