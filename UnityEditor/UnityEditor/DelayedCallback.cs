// Decompiled with JetBrains decompiler
// Type: UnityEditor.DelayedCallback
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  internal class DelayedCallback
  {
    private Action m_Callback;
    private double m_CallbackTime;

    public DelayedCallback(Action function, double timeFromNow)
    {
      this.m_Callback = function;
      this.m_CallbackTime = EditorApplication.timeSinceStartup + timeFromNow;
      EditorApplication.update += new EditorApplication.CallbackFunction(this.Update);
    }

    public void Clear()
    {
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
      this.m_CallbackTime = 0.0;
      this.m_Callback = (Action) null;
    }

    private void Update()
    {
      if (EditorApplication.timeSinceStartup <= this.m_CallbackTime)
        return;
      Action callback = this.m_Callback;
      this.Clear();
      callback();
    }
  }
}
