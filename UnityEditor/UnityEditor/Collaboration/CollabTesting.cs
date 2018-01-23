// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.CollabTesting
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Collaboration
{
  internal class CollabTesting
  {
    private static IEnumerator<CollabTesting.AsyncState> _enumerator = (IEnumerator<CollabTesting.AsyncState>) null;
    private static Action _runAfter = (Action) null;
    private static CollabTesting.AsyncState _nextState = CollabTesting.AsyncState.NotWaiting;

    public static Func<IEnumerable<CollabTesting.AsyncState>> Tick
    {
      set
      {
        CollabTesting._enumerator = value().GetEnumerator();
      }
    }

    public static Action AfterRun
    {
      set
      {
        CollabTesting._runAfter = value;
      }
    }

    public static bool IsRunning
    {
      get
      {
        return CollabTesting._enumerator != null;
      }
    }

    public static void OnCompleteJob()
    {
      CollabTesting.OnAsyncSignalReceived(CollabTesting.AsyncState.WaitForJobComplete);
    }

    public static void OnChannelMessageHandled()
    {
      CollabTesting.OnAsyncSignalReceived(CollabTesting.AsyncState.WaitForChannelMessageHandled);
    }

    private static void OnAsyncSignalReceived(CollabTesting.AsyncState stateToRemove)
    {
      if ((CollabTesting._nextState & stateToRemove) == CollabTesting.AsyncState.NotWaiting)
        return;
      CollabTesting._nextState &= ~stateToRemove;
      if (CollabTesting._nextState != CollabTesting.AsyncState.NotWaiting)
        return;
      CollabTesting.Execute();
    }

    public static void Execute()
    {
      if (CollabTesting._enumerator == null)
        return;
      if (Collab.instance.AnyJobRunning())
        return;
      try
      {
        if (!CollabTesting._enumerator.MoveNext())
          CollabTesting.End();
        else
          CollabTesting._nextState = CollabTesting._enumerator.Current;
      }
      catch (Exception ex)
      {
        Debug.LogError((object) "Something Went wrong with the test framework itself");
        throw;
      }
    }

    public static void End()
    {
      if (CollabTesting._enumerator == null)
        return;
      CollabTesting._runAfter();
      CollabTesting._enumerator = (IEnumerator<CollabTesting.AsyncState>) null;
    }

    [System.Flags]
    public enum AsyncState
    {
      NotWaiting = 0,
      WaitForJobComplete = 1,
      WaitForChannelMessageHandled = 2,
    }
  }
}
