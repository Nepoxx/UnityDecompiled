// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.DataWatchContainer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal abstract class DataWatchContainer : VisualElement
  {
    private IUIElementDataWatchRequest[] handles;

    public bool forceNotififcationOnAdd { get; set; }

    private void OnDataChanged(UnityEngine.Object obj)
    {
      this.OnDataChanged();
    }

    public virtual void OnDataChanged()
    {
    }

    protected abstract UnityEngine.Object[] toWatch { get; }

    protected void AddWatch()
    {
      UnityEngine.Object[] toWatch = this.toWatch;
      this.handles = new IUIElementDataWatchRequest[toWatch.Length];
      for (int index = 0; index < toWatch.Length; ++index)
      {
        if (this.panel != null && toWatch[index] != (UnityEngine.Object) null)
        {
          this.handles[index] = this.dataWatch.RegisterWatch(toWatch[index], new System.Action<UnityEngine.Object>(this.OnDataChanged));
          if (this.forceNotififcationOnAdd)
            this.OnDataChanged();
        }
      }
    }

    protected void RemoveWatch()
    {
      if (this.handles == null)
        return;
      foreach (IUIElementDataWatchRequest handle in this.handles)
      {
        if (handle != null)
          handle.Dispose();
      }
      this.handles = (IUIElementDataWatchRequest[]) null;
    }

    protected internal override void ExecuteDefaultAction(EventBase evt)
    {
      base.ExecuteDefaultAction(evt);
      if (evt.GetEventTypeId() == EventBase<AttachToPanelEvent>.TypeId())
      {
        this.AddWatch();
      }
      else
      {
        if (evt.GetEventTypeId() != EventBase<DetachFromPanelEvent>.TypeId())
          return;
        this.RemoveWatch();
      }
    }
  }
}
