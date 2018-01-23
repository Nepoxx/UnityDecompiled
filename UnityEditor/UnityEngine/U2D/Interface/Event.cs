// Decompiled with JetBrains decompiler
// Type: UnityEngine.U2D.Interface.Event
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEngine.U2D.Interface
{
  internal class Event : IEvent
  {
    private UnityEngine.Event m_Event;

    public Event()
    {
      this.m_Event = UnityEngine.Event.current;
    }

    public EventType type
    {
      get
      {
        return this.m_Event.type;
      }
    }

    public string commandName
    {
      get
      {
        return this.m_Event.commandName;
      }
    }

    public bool control
    {
      get
      {
        return this.m_Event.control;
      }
    }

    public bool alt
    {
      get
      {
        return this.m_Event.alt;
      }
    }

    public bool shift
    {
      get
      {
        return this.m_Event.shift;
      }
    }

    public KeyCode keyCode
    {
      get
      {
        return this.m_Event.keyCode;
      }
    }

    public Vector2 mousePosition
    {
      get
      {
        return this.m_Event.mousePosition;
      }
    }

    public int button
    {
      get
      {
        return this.m_Event.button;
      }
    }

    public void Use()
    {
      this.m_Event.Use();
    }

    public EventModifiers modifiers
    {
      get
      {
        return this.m_Event.modifiers;
      }
    }

    public EventType GetTypeForControl(int id)
    {
      return this.m_Event.GetTypeForControl(id);
    }
  }
}
