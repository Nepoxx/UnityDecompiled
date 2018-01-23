// Decompiled with JetBrains decompiler
// Type: UnityEditor.CallbackController
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class CallbackController
  {
    private readonly Action m_Callback;
    private readonly float m_CallbacksPerSecond;
    private double m_NextCallback;

    public CallbackController(Action callback, float callbacksPerSecond)
    {
      this.m_Callback = callback;
      this.m_CallbacksPerSecond = Mathf.Max(callbacksPerSecond, 1f);
    }

    public bool active { get; private set; }

    public void Start()
    {
      this.m_NextCallback = 0.0;
      EditorApplication.update += new EditorApplication.CallbackFunction(this.Update);
      this.active = true;
    }

    public void Stop()
    {
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
      this.active = false;
    }

    private void Update()
    {
      double timeSinceStartup = EditorApplication.timeSinceStartup;
      if (timeSinceStartup <= this.m_NextCallback)
        return;
      this.m_NextCallback = timeSinceStartup + 1.0 / (double) this.m_CallbacksPerSecond;
      if (this.m_Callback != null)
        this.m_Callback();
    }
  }
}
