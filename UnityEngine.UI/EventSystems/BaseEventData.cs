// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.BaseEventData
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

namespace UnityEngine.EventSystems
{
  /// <summary>
  ///   <para>A class that contains the base event data that is common to all event types in the new EventSystem.</para>
  /// </summary>
  public class BaseEventData : AbstractEventData
  {
    private readonly EventSystem m_EventSystem;

    /// <summary>
    ///   <para>Construct a BaseEventData tied to the passed EventSystem.</para>
    /// </summary>
    /// <param name="eventSystem"></param>
    public BaseEventData(EventSystem eventSystem)
    {
      this.m_EventSystem = eventSystem;
    }

    /// <summary>
    ///   <para>A reference to the BaseInputModule that sent this event.</para>
    /// </summary>
    public BaseInputModule currentInputModule
    {
      get
      {
        return this.m_EventSystem.currentInputModule;
      }
    }

    /// <summary>
    ///   <para>The object currently considered selected by the EventSystem.</para>
    /// </summary>
    public GameObject selectedObject
    {
      get
      {
        return this.m_EventSystem.currentSelectedGameObject;
      }
      set
      {
        this.m_EventSystem.SetSelectedGameObject(value, this);
      }
    }
  }
}
