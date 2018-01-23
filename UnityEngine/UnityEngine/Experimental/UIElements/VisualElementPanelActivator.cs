// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.VisualElementPanelActivator
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  internal class VisualElementPanelActivator
  {
    private IVisualElementPanelActivatable m_Activatable;

    public VisualElementPanelActivator(IVisualElementPanelActivatable activatable)
    {
      this.m_Activatable = activatable;
    }

    public bool isActive { get; private set; }

    public bool isDetaching { get; private set; }

    public void SetActive(bool action)
    {
      if (this.isActive == action)
        return;
      this.isActive = action;
      if (this.isActive)
      {
        this.m_Activatable.element.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnEnter), Capture.NoCapture);
        this.m_Activatable.element.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnLeave), Capture.NoCapture);
        this.SendActivation();
      }
      else
      {
        this.m_Activatable.element.UnregisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnEnter), Capture.NoCapture);
        this.m_Activatable.element.UnregisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnLeave), Capture.NoCapture);
        this.SendDeactivation();
      }
    }

    public void SendActivation()
    {
      if (!this.m_Activatable.CanBeActivated())
        return;
      this.m_Activatable.OnPanelActivate();
    }

    public void SendDeactivation()
    {
      if (!this.m_Activatable.CanBeActivated())
        return;
      this.m_Activatable.OnPanelDeactivate();
    }

    private void OnEnter(AttachToPanelEvent evt)
    {
      if (!this.isActive)
        return;
      this.SendActivation();
    }

    private void OnLeave(DetachFromPanelEvent evt)
    {
      if (!this.isActive)
        return;
      this.isDetaching = true;
      try
      {
        this.SendDeactivation();
      }
      finally
      {
        this.isDetaching = false;
      }
    }
  }
}
