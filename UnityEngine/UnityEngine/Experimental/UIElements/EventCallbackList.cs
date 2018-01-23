// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.EventCallbackList
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
  internal class EventCallbackList
  {
    private List<EventCallbackFunctorBase> m_List;

    public EventCallbackList()
    {
      this.m_List = new List<EventCallbackFunctorBase>();
      this.capturingCallbackCount = 0;
      this.bubblingCallbackCount = 0;
    }

    public EventCallbackList(EventCallbackList source)
    {
      this.m_List = new List<EventCallbackFunctorBase>((IEnumerable<EventCallbackFunctorBase>) source.m_List);
      this.capturingCallbackCount = 0;
      this.bubblingCallbackCount = 0;
    }

    public int capturingCallbackCount { get; private set; }

    public int bubblingCallbackCount { get; private set; }

    public bool Contains(long eventTypeId, Delegate callback, CallbackPhase phase)
    {
      for (int index = 0; index < this.m_List.Count; ++index)
      {
        if (this.m_List[index].IsEquivalentTo(eventTypeId, callback, phase))
          return true;
      }
      return false;
    }

    public bool Remove(long eventTypeId, Delegate callback, CallbackPhase phase)
    {
      for (int index = 0; index < this.m_List.Count; ++index)
      {
        if (this.m_List[index].IsEquivalentTo(eventTypeId, callback, phase))
        {
          this.m_List.RemoveAt(index);
          switch (phase)
          {
            case CallbackPhase.TargetAndBubbleUp:
              --this.bubblingCallbackCount;
              break;
            case CallbackPhase.CaptureAndTarget:
              --this.capturingCallbackCount;
              break;
          }
          return true;
        }
      }
      return false;
    }

    public void Add(EventCallbackFunctorBase item)
    {
      this.m_List.Add(item);
      if (item.phase == CallbackPhase.CaptureAndTarget)
      {
        ++this.capturingCallbackCount;
      }
      else
      {
        if (item.phase != CallbackPhase.TargetAndBubbleUp)
          return;
        ++this.bubblingCallbackCount;
      }
    }

    public void AddRange(EventCallbackList list)
    {
      this.m_List.AddRange((IEnumerable<EventCallbackFunctorBase>) list.m_List);
      foreach (EventCallbackFunctorBase callbackFunctorBase in list.m_List)
      {
        if (callbackFunctorBase.phase == CallbackPhase.CaptureAndTarget)
          ++this.capturingCallbackCount;
        else if (callbackFunctorBase.phase == CallbackPhase.TargetAndBubbleUp)
          ++this.bubblingCallbackCount;
      }
    }

    public int Count
    {
      get
      {
        return this.m_List.Count;
      }
    }

    public EventCallbackFunctorBase this[int i]
    {
      get
      {
        return this.m_List[i];
      }
      set
      {
        this.m_List[i] = value;
      }
    }

    public void Clear()
    {
      this.m_List.Clear();
      this.capturingCallbackCount = 0;
      this.bubblingCallbackCount = 0;
    }
  }
}
