// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.ObjectPool`1
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI
{
  internal class ObjectPool<T> where T : new()
  {
    private readonly Stack<T> m_Stack = new Stack<T>();
    private readonly UnityAction<T> m_ActionOnGet;
    private readonly UnityAction<T> m_ActionOnRelease;

    public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
    {
      this.m_ActionOnGet = actionOnGet;
      this.m_ActionOnRelease = actionOnRelease;
    }

    public int countAll { get; private set; }

    public int countActive
    {
      get
      {
        return this.countAll - this.countInactive;
      }
    }

    public int countInactive
    {
      get
      {
        return this.m_Stack.Count;
      }
    }

    public T Get()
    {
      T obj;
      if (this.m_Stack.Count == 0)
      {
        obj = Activator.CreateInstance<T>();
        ++this.countAll;
      }
      else
        obj = this.m_Stack.Pop();
      if (this.m_ActionOnGet != null)
        this.m_ActionOnGet(obj);
      return obj;
    }

    public void Release(T element)
    {
      if (this.m_Stack.Count > 0 && object.ReferenceEquals((object) this.m_Stack.Peek(), (object) element))
        Debug.LogError((object) "Internal error. Trying to destroy object that is already released to pool.");
      if (this.m_ActionOnRelease != null)
        this.m_ActionOnRelease(element);
      this.m_Stack.Push(element);
    }
  }
}
