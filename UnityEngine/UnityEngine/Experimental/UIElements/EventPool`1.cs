// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.EventPool`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
  internal class EventPool<T> where T : EventBase<T>, new()
  {
    private readonly Stack<T> m_Stack = new Stack<T>();

    public T Get()
    {
      return this.m_Stack.Count != 0 ? this.m_Stack.Pop() : Activator.CreateInstance<T>();
    }

    public void Release(T element)
    {
      if (this.m_Stack.Contains(element))
        Debug.LogError((object) "Internal error. Trying to destroy object that is already released to pool.");
      this.m_Stack.Push(element);
    }
  }
}
