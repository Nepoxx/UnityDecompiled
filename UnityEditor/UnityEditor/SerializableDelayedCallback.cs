// Decompiled with JetBrains decompiler
// Type: UnityEditor.SerializableDelayedCallback
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [Serializable]
  internal class SerializableDelayedCallback : ScriptableObject
  {
    [SerializeField]
    private long m_CallbackTicks;
    [SerializeField]
    private UnityEvent m_Callback;

    protected SerializableDelayedCallback()
    {
      this.m_Callback = new UnityEvent();
      EditorApplication.update += new EditorApplication.CallbackFunction(this.Update);
    }

    public static SerializableDelayedCallback SubscribeCallback(UnityAction action, TimeSpan delayUntilCallback)
    {
      SerializableDelayedCallback instance = ScriptableObject.CreateInstance<SerializableDelayedCallback>();
      instance.m_CallbackTicks = DateTime.UtcNow.Add(delayUntilCallback).Ticks;
      instance.m_Callback.AddPersistentListener(action, UnityEventCallState.EditorAndRuntime);
      return instance;
    }

    public void Cancel()
    {
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
    }

    private void Update()
    {
      if (DateTime.UtcNow.Ticks < this.m_CallbackTicks)
        return;
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
      this.m_Callback.Invoke();
    }
  }
}
