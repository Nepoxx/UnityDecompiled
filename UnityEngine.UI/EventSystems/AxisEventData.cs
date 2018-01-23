// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.AxisEventData
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

namespace UnityEngine.EventSystems
{
  /// <summary>
  ///   <para>Event Data associated with Axis Events (Controller / Keyboard).</para>
  /// </summary>
  public class AxisEventData : BaseEventData
  {
    public AxisEventData(EventSystem eventSystem)
      : base(eventSystem)
    {
      this.moveVector = Vector2.zero;
      this.moveDir = MoveDirection.None;
    }

    /// <summary>
    ///   <para>Raw input vector associated with this event.</para>
    /// </summary>
    public Vector2 moveVector { get; set; }

    /// <summary>
    ///   <para>MoveDirection for this event.</para>
    /// </summary>
    public MoveDirection moveDir { get; set; }
  }
}
