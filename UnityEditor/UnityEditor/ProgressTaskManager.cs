// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProgressTaskManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class ProgressTaskManager
  {
    private List<Action> m_Tasks = new List<Action>();
    private ProgressHandler m_Handler;
    private int m_ProgressUpdatesForCurrentTask;
    private int m_StartedTasks;

    public ProgressTaskManager(ProgressHandler handler)
    {
      this.m_Handler = handler;
    }

    public void AddTask(Action task)
    {
      this.m_Tasks.Add(task);
    }

    public void Run()
    {
      foreach (Action task in this.m_Tasks)
      {
        ++this.m_StartedTasks;
        this.m_ProgressUpdatesForCurrentTask = 0;
        task();
      }
    }

    public void UpdateProgress(string message)
    {
      if (this.m_Handler != null)
      {
        float num1 = 1f - Mathf.Pow(0.85f, (float) this.m_ProgressUpdatesForCurrentTask);
        int num2 = this.m_Tasks.Count;
        if (num2 <= this.m_StartedTasks)
          num2 = this.m_StartedTasks;
        float num3 = 1f / (float) num2;
        float progress = (float) ((double) (this.m_StartedTasks - 1) * (double) num3 + (double) num1 * (double) num3);
        this.m_Handler.OnProgress(message, progress);
      }
      ++this.m_ProgressUpdatesForCurrentTask;
    }

    public ProgressHandler SpawnProgressHandlerFromCurrentTask()
    {
      if (this.m_Handler == null)
        return (ProgressHandler) null;
      float num = 1f / (float) this.m_Tasks.Count;
      return this.m_Handler.SpawnFromLocalSubRange((float) (this.m_StartedTasks - 1) * num, (float) this.m_StartedTasks * num);
    }
  }
}
