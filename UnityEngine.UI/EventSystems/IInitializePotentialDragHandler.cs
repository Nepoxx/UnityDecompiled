// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.IInitializePotentialDragHandler
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

namespace UnityEngine.EventSystems
{
  public interface IInitializePotentialDragHandler : IEventSystemHandler
  {
    /// <summary>
    ///   <para>Called by a BaseInputModule when a drag has been found but before it is valid to begin the drag.</para>
    /// </summary>
    /// <param name="eventData"></param>
    void OnInitializePotentialDrag(PointerEventData eventData);
  }
}
