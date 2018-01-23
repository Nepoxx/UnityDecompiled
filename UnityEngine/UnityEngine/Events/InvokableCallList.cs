// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.InvokableCallList
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.Events
{
  internal class InvokableCallList
  {
    private readonly List<BaseInvokableCall> m_PersistentCalls = new List<BaseInvokableCall>();
    private readonly List<BaseInvokableCall> m_RuntimeCalls = new List<BaseInvokableCall>();
    private readonly List<BaseInvokableCall> m_ExecutingCalls = new List<BaseInvokableCall>();
    private bool m_NeedsUpdate = true;

    public int Count
    {
      get
      {
        return this.m_PersistentCalls.Count + this.m_RuntimeCalls.Count;
      }
    }

    public void AddPersistentInvokableCall(BaseInvokableCall call)
    {
      this.m_PersistentCalls.Add(call);
      this.m_NeedsUpdate = true;
    }

    public void AddListener(BaseInvokableCall call)
    {
      this.m_RuntimeCalls.Add(call);
      this.m_NeedsUpdate = true;
    }

    public void RemoveListener(object targetObj, MethodInfo method)
    {
      List<BaseInvokableCall> baseInvokableCallList = new List<BaseInvokableCall>();
      for (int index = 0; index < this.m_RuntimeCalls.Count; ++index)
      {
        if (this.m_RuntimeCalls[index].Find(targetObj, method))
          baseInvokableCallList.Add(this.m_RuntimeCalls[index]);
      }
      this.m_RuntimeCalls.RemoveAll(new Predicate<BaseInvokableCall>(baseInvokableCallList.Contains));
      this.m_NeedsUpdate = true;
    }

    public void Clear()
    {
      this.m_RuntimeCalls.Clear();
      this.m_NeedsUpdate = true;
    }

    public void ClearPersistent()
    {
      this.m_PersistentCalls.Clear();
      this.m_NeedsUpdate = true;
    }

    public List<BaseInvokableCall> PrepareInvoke()
    {
      if (this.m_NeedsUpdate)
      {
        this.m_ExecutingCalls.Clear();
        this.m_ExecutingCalls.AddRange((IEnumerable<BaseInvokableCall>) this.m_PersistentCalls);
        this.m_ExecutingCalls.AddRange((IEnumerable<BaseInvokableCall>) this.m_RuntimeCalls);
        this.m_NeedsUpdate = false;
      }
      return this.m_ExecutingCalls;
    }
  }
}
