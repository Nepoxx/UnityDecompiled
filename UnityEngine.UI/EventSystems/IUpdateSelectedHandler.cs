// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.IUpdateSelectedHandler
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

namespace UnityEngine.EventSystems
{
  public interface IUpdateSelectedHandler : IEventSystemHandler
  {
    /// <summary>
    ///   <para>Called by the EventSystem when the object associated with this EventTrigger is updated.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    void OnUpdateSelected(BaseEventData eventData);
  }
}
