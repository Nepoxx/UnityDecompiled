// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Button
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>A standard button that can be clicked in order to trigger an event.</para>
  /// </summary>
  [AddComponentMenu("UI/Button", 30)]
  public class Button : Selectable, IPointerClickHandler, ISubmitHandler, IEventSystemHandler
  {
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();

    protected Button()
    {
    }

    /// <summary>
    ///   <para>UnityEvent that is triggered when the button is pressed.</para>
    /// </summary>
    public Button.ButtonClickedEvent onClick
    {
      get
      {
        return this.m_OnClick;
      }
      set
      {
        this.m_OnClick = value;
      }
    }

    private void Press()
    {
      if (!this.IsActive() || !this.IsInteractable())
        return;
      UISystemProfilerApi.AddMarker("Button.onClick", (UnityEngine.Object) this);
      this.m_OnClick.Invoke();
    }

    /// <summary>
    ///   <para>Registered IPointerClickHandler callback.</para>
    /// </summary>
    /// <param name="eventData">Data passed in (Typically by the event system).</param>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      this.Press();
    }

    /// <summary>
    ///   <para>Registered ISubmitHandler callback.</para>
    /// </summary>
    /// <param name="eventData">Data passed in (Typically by the event system).</param>
    public virtual void OnSubmit(BaseEventData eventData)
    {
      this.Press();
      if (!this.IsActive() || !this.IsInteractable())
        return;
      this.DoStateTransition(Selectable.SelectionState.Pressed, false);
      this.StartCoroutine(this.OnFinishSubmit());
    }

    [DebuggerHidden]
    private IEnumerator OnFinishSubmit()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Button.\u003COnFinishSubmit\u003Ec__Iterator0() { \u0024this = this };
    }

    /// <summary>
    ///   <para>Function definition for a button click event.</para>
    /// </summary>
    [Serializable]
    public class ButtonClickedEvent : UnityEvent
    {
    }
  }
}
