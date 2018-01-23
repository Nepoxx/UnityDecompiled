// Decompiled with JetBrains decompiler
// Type: UnityEngine.U2D.Interface.IEvent
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEngine.U2D.Interface
{
  internal interface IEvent
  {
    EventType type { get; }

    string commandName { get; }

    bool control { get; }

    bool alt { get; }

    bool shift { get; }

    KeyCode keyCode { get; }

    Vector2 mousePosition { get; }

    int button { get; }

    EventModifiers modifiers { get; }

    EventType GetTypeForControl(int id);

    void Use();
  }
}
