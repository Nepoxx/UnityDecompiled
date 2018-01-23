// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.StateMachine
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine;

namespace UnityEditorInternal
{
  [Obsolete("StateMachine is obsolete. Use UnityEditor.Animations.AnimatorStateMachine instead (UnityUpgradable) -> UnityEditor.Animations.AnimatorStateMachine", true)]
  public class StateMachine : UnityEngine.Object
  {
    public State defaultState
    {
      get
      {
        return (State) null;
      }
      set
      {
      }
    }

    public Vector3 anyStatePosition
    {
      get
      {
        return new Vector3();
      }
      set
      {
      }
    }

    public Vector3 parentStateMachinePosition
    {
      get
      {
        return new Vector3();
      }
      set
      {
      }
    }

    public State GetState(int index)
    {
      return (State) null;
    }

    public State AddState(string stateName)
    {
      return (State) null;
    }

    public StateMachine GetStateMachine(int index)
    {
      return (StateMachine) null;
    }

    public StateMachine AddStateMachine(string stateMachineName)
    {
      return (StateMachine) null;
    }

    public Transition AddTransition(State src, State dst)
    {
      return (Transition) null;
    }

    public Transition AddAnyStateTransition(State dst)
    {
      return (Transition) null;
    }

    public Vector3 GetStateMachinePosition(int i)
    {
      return new Vector3();
    }

    public Transition[] GetTransitionsFromState(State srcState)
    {
      return (Transition[]) null;
    }
  }
}
