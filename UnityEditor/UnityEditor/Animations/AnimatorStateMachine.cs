// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.AnimatorStateMachine
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>A graph controlling the interaction of states. Each state references a motion.</para>
  /// </summary>
  public sealed class AnimatorStateMachine : UnityEngine.Object
  {
    private PushUndoIfNeeded undoHandler = new PushUndoIfNeeded(true);

    public AnimatorStateMachine()
    {
      AnimatorStateMachine.Internal_Create(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create(AnimatorStateMachine mono);

    /// <summary>
    ///   <para>The list of states.</para>
    /// </summary>
    public extern ChildAnimatorState[] states { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The list of sub state machines.</para>
    /// </summary>
    public extern ChildAnimatorStateMachine[] stateMachines { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The state that the state machine will be in when it starts.</para>
    /// </summary>
    public extern AnimatorState defaultState { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The position of the AnyState node.</para>
    /// </summary>
    public Vector3 anyStatePosition
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_anyStatePosition(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_anyStatePosition(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_anyStatePosition(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_anyStatePosition(ref Vector3 value);

    /// <summary>
    ///   <para>The position of the entry node.</para>
    /// </summary>
    public Vector3 entryPosition
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_entryPosition(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_entryPosition(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_entryPosition(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_entryPosition(ref Vector3 value);

    /// <summary>
    ///   <para>The position of the exit node.</para>
    /// </summary>
    public Vector3 exitPosition
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_exitPosition(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_exitPosition(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_exitPosition(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_exitPosition(ref Vector3 value);

    /// <summary>
    ///   <para>The position of the parent state machine node. Only valid when in a hierachic state machine.</para>
    /// </summary>
    public Vector3 parentStateMachinePosition
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_parentStateMachinePosition(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_parentStateMachinePosition(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_parentStateMachinePosition(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_parentStateMachinePosition(ref Vector3 value);

    /// <summary>
    ///   <para>The list of AnyState transitions.</para>
    /// </summary>
    public extern AnimatorStateTransition[] anyStateTransitions { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The list of entry transitions in the state machine.</para>
    /// </summary>
    public extern AnimatorTransition[] entryTransitions { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets the list of all outgoing state machine transitions from given state machine.</para>
    /// </summary>
    /// <param name="sourceStateMachine">The source state machine.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern AnimatorTransition[] GetStateMachineTransitions(AnimatorStateMachine sourceStateMachine);

    /// <summary>
    ///   <para>Sets the list of all outgoing state machine transitions from given state machine.</para>
    /// </summary>
    /// <param name="stateMachine">The source state machine.</param>
    /// <param name="transitions">The outgoing transitions.</param>
    /// <param name="sourceStateMachine"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetStateMachineTransitions(AnimatorStateMachine sourceStateMachine, AnimatorTransition[] transitions);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void AddBehaviour(int instanceID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void RemoveBehaviour(int index);

    /// <summary>
    ///   <para>The Behaviour list assigned to this state machine.</para>
    /// </summary>
    public extern StateMachineBehaviour[] behaviours { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern MonoScript GetBehaviourMonoScript(int index);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern ScriptableObject Internal_AddStateMachineBehaviourWithType(System.Type stateMachineBehaviourType);

    /// <summary>
    ///   <para>Adds a state machine behaviour class of type stateMachineBehaviourType to the AnimatorStateMachine. C# Users can use a generic version.</para>
    /// </summary>
    /// <param name="stateMachineBehaviourType"></param>
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    public StateMachineBehaviour AddStateMachineBehaviour(System.Type stateMachineBehaviourType)
    {
      return (StateMachineBehaviour) this.Internal_AddStateMachineBehaviourWithType(stateMachineBehaviourType);
    }

    public T AddStateMachineBehaviour<T>() where T : StateMachineBehaviour
    {
      return this.AddStateMachineBehaviour(typeof (T)) as T;
    }

    /// <summary>
    ///   <para>Makes a unique state name in the context of the parent state machine.</para>
    /// </summary>
    /// <param name="name">Desired name for the state.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string MakeUniqueStateName(string name);

    /// <summary>
    ///   <para>Makes a unique state machine name in the context of the parent state machine.</para>
    /// </summary>
    /// <param name="name">Desired name for the state machine.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string MakeUniqueStateMachineName(string name);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void Clear();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void RemoveStateInternal(AnimatorState state);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void RemoveStateMachineInternal(AnimatorStateMachine stateMachine);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void MoveState(AnimatorState state, AnimatorStateMachine target);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void MoveStateMachine(AnimatorStateMachine stateMachine, AnimatorStateMachine target);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool HasState(AnimatorState state, bool recursive);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool HasStateMachine(AnimatorStateMachine state, bool recursive);

    internal extern int transitionCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal bool pushUndo
    {
      set
      {
        this.undoHandler.pushUndo = value;
      }
    }

    internal List<ChildAnimatorState> statesRecursive
    {
      get
      {
        List<ChildAnimatorState> childAnimatorStateList = new List<ChildAnimatorState>();
        childAnimatorStateList.AddRange((IEnumerable<ChildAnimatorState>) this.states);
        for (int index = 0; index < this.stateMachines.Length; ++index)
          childAnimatorStateList.AddRange((IEnumerable<ChildAnimatorState>) this.stateMachines[index].stateMachine.statesRecursive);
        return childAnimatorStateList;
      }
    }

    internal List<ChildAnimatorStateMachine> stateMachinesRecursive
    {
      get
      {
        List<ChildAnimatorStateMachine> animatorStateMachineList = new List<ChildAnimatorStateMachine>();
        ChildAnimatorStateMachine[] childStateMachines = AnimatorStateMachine.StateMachineCache.GetChildStateMachines(this);
        animatorStateMachineList.AddRange((IEnumerable<ChildAnimatorStateMachine>) childStateMachines);
        for (int index = 0; index < childStateMachines.Length; ++index)
          animatorStateMachineList.AddRange((IEnumerable<ChildAnimatorStateMachine>) childStateMachines[index].stateMachine.stateMachinesRecursive);
        return animatorStateMachineList;
      }
    }

    internal List<AnimatorStateTransition> anyStateTransitionsRecursive
    {
      get
      {
        List<AnimatorStateTransition> animatorStateTransitionList = new List<AnimatorStateTransition>();
        animatorStateTransitionList.AddRange((IEnumerable<AnimatorStateTransition>) this.anyStateTransitions);
        foreach (ChildAnimatorStateMachine stateMachine in this.stateMachines)
          animatorStateTransitionList.AddRange((IEnumerable<AnimatorStateTransition>) stateMachine.stateMachine.anyStateTransitionsRecursive);
        return animatorStateTransitionList;
      }
    }

    internal Vector3 GetStatePosition(AnimatorState state)
    {
      ChildAnimatorState[] states = this.states;
      for (int index = 0; index < states.Length; ++index)
      {
        if ((UnityEngine.Object) state == (UnityEngine.Object) states[index].state)
          return states[index].position;
      }
      return Vector3.zero;
    }

    internal void SetStatePosition(AnimatorState state, Vector3 position)
    {
      ChildAnimatorState[] states = this.states;
      for (int index = 0; index < states.Length; ++index)
      {
        if ((UnityEngine.Object) state == (UnityEngine.Object) states[index].state)
        {
          states[index].position = position;
          this.states = states;
          break;
        }
      }
    }

    internal Vector3 GetStateMachinePosition(AnimatorStateMachine stateMachine)
    {
      ChildAnimatorStateMachine[] stateMachines = this.stateMachines;
      for (int index = 0; index < stateMachines.Length; ++index)
      {
        if ((UnityEngine.Object) stateMachine == (UnityEngine.Object) stateMachines[index].stateMachine)
          return stateMachines[index].position;
      }
      return Vector3.zero;
    }

    internal void SetStateMachinePosition(AnimatorStateMachine stateMachine, Vector3 position)
    {
      ChildAnimatorStateMachine[] stateMachines = this.stateMachines;
      for (int index = 0; index < stateMachines.Length; ++index)
      {
        if ((UnityEngine.Object) stateMachine == (UnityEngine.Object) stateMachines[index].stateMachine)
        {
          stateMachines[index].position = position;
          this.stateMachines = stateMachines;
          break;
        }
      }
    }

    /// <summary>
    ///   <para>Utility function to add a state to the state machine.</para>
    /// </summary>
    /// <param name="name">The name of the new state.</param>
    /// <param name="position">The position of the state node.</param>
    /// <returns>
    ///   <para>The AnimatorState that was created for this state.</para>
    /// </returns>
    public AnimatorState AddState(string name)
    {
      return this.AddState(name, this.states.Length <= 0 ? new Vector3(200f, 0.0f, 0.0f) : this.states[this.states.Length - 1].position + new Vector3(35f, 65f));
    }

    /// <summary>
    ///   <para>Utility function to add a state to the state machine.</para>
    /// </summary>
    /// <param name="name">The name of the new state.</param>
    /// <param name="position">The position of the state node.</param>
    /// <returns>
    ///   <para>The AnimatorState that was created for this state.</para>
    /// </returns>
    public AnimatorState AddState(string name, Vector3 position)
    {
      AnimatorState state = new AnimatorState();
      state.hideFlags = HideFlags.HideInHierarchy;
      state.name = this.MakeUniqueStateName(name);
      if (AssetDatabase.GetAssetPath((UnityEngine.Object) this) != "")
        AssetDatabase.AddObjectToAsset((UnityEngine.Object) state, AssetDatabase.GetAssetPath((UnityEngine.Object) this));
      this.AddState(state, position);
      return state;
    }

    /// <summary>
    ///   <para>Utility function to add a state to the state machine.</para>
    /// </summary>
    /// <param name="state">The state to add.</param>
    /// <param name="position">The position of the state node.</param>
    public void AddState(AnimatorState state, Vector3 position)
    {
      ChildAnimatorState[] states = this.states;
      if (Array.Exists<ChildAnimatorState>(states, (Predicate<ChildAnimatorState>) (childState => (UnityEngine.Object) childState.state == (UnityEngine.Object) state)))
      {
        Debug.LogWarning((object) string.Format("State '{0}' already exists in state machine '{1}', discarding new state.", (object) state.name, (object) this.name));
      }
      else
      {
        this.undoHandler.DoUndo((UnityEngine.Object) this, "State added");
        ArrayUtility.Add<ChildAnimatorState>(ref states, new ChildAnimatorState()
        {
          state = state,
          position = position
        });
        this.states = states;
      }
    }

    /// <summary>
    ///   <para>Utility function to remove a state from the state machine.</para>
    /// </summary>
    /// <param name="state">The state to remove.</param>
    public void RemoveState(AnimatorState state)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "State removed");
      this.undoHandler.DoUndo((UnityEngine.Object) state, "State removed");
      this.RemoveStateInternal(state);
    }

    /// <summary>
    ///   <para>Utility function to add a state machine to the state machine.</para>
    /// </summary>
    /// <param name="name">The name of the new state machine.</param>
    /// <param name="position">The position of the state machine node.</param>
    /// <returns>
    ///   <para>The newly created Animations.AnimatorStateMachine state machine.</para>
    /// </returns>
    public AnimatorStateMachine AddStateMachine(string name)
    {
      return this.AddStateMachine(name, Vector3.zero);
    }

    /// <summary>
    ///   <para>Utility function to add a state machine to the state machine.</para>
    /// </summary>
    /// <param name="name">The name of the new state machine.</param>
    /// <param name="position">The position of the state machine node.</param>
    /// <returns>
    ///   <para>The newly created Animations.AnimatorStateMachine state machine.</para>
    /// </returns>
    public AnimatorStateMachine AddStateMachine(string name, Vector3 position)
    {
      AnimatorStateMachine stateMachine = new AnimatorStateMachine();
      stateMachine.hideFlags = HideFlags.HideInHierarchy;
      stateMachine.name = this.MakeUniqueStateMachineName(name);
      this.AddStateMachine(stateMachine, position);
      if (AssetDatabase.GetAssetPath((UnityEngine.Object) this) != "")
        AssetDatabase.AddObjectToAsset((UnityEngine.Object) stateMachine, AssetDatabase.GetAssetPath((UnityEngine.Object) this));
      return stateMachine;
    }

    /// <summary>
    ///   <para>Utility function to add a state machine to the state machine.</para>
    /// </summary>
    /// <param name="stateMachine">The state machine to add.</param>
    /// <param name="position">The position of the state machine node.</param>
    public void AddStateMachine(AnimatorStateMachine stateMachine, Vector3 position)
    {
      ChildAnimatorStateMachine[] stateMachines = this.stateMachines;
      if (Array.Exists<ChildAnimatorStateMachine>(stateMachines, (Predicate<ChildAnimatorStateMachine>) (childStateMachine => (UnityEngine.Object) childStateMachine.stateMachine == (UnityEngine.Object) stateMachine)))
      {
        Debug.LogWarning((object) string.Format("Sub state machine '{0}' already exists in state machine '{1}', discarding new state machine.", (object) stateMachine.name, (object) this.name));
      }
      else
      {
        this.undoHandler.DoUndo((UnityEngine.Object) this, "StateMachine " + stateMachine.name + " added");
        ArrayUtility.Add<ChildAnimatorStateMachine>(ref stateMachines, new ChildAnimatorStateMachine()
        {
          stateMachine = stateMachine,
          position = position
        });
        this.stateMachines = stateMachines;
      }
    }

    /// <summary>
    ///   <para>Utility function to remove a state machine from its parent state machine.</para>
    /// </summary>
    /// <param name="stateMachine">The state machine to remove.</param>
    public void RemoveStateMachine(AnimatorStateMachine stateMachine)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "StateMachine removed");
      this.undoHandler.DoUndo((UnityEngine.Object) stateMachine, "StateMachine removed");
      this.RemoveStateMachineInternal(stateMachine);
    }

    private AnimatorStateTransition AddAnyStateTransition()
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "AnyState Transition Added");
      AnimatorStateTransition[] stateTransitions = this.anyStateTransitions;
      AnimatorStateTransition animatorStateTransition = new AnimatorStateTransition();
      animatorStateTransition.hasExitTime = false;
      animatorStateTransition.hasFixedDuration = true;
      animatorStateTransition.duration = 0.25f;
      animatorStateTransition.exitTime = 0.75f;
      if (AssetDatabase.GetAssetPath((UnityEngine.Object) this) != "")
        AssetDatabase.AddObjectToAsset((UnityEngine.Object) animatorStateTransition, AssetDatabase.GetAssetPath((UnityEngine.Object) this));
      animatorStateTransition.hideFlags = HideFlags.HideInHierarchy;
      ArrayUtility.Add<AnimatorStateTransition>(ref stateTransitions, animatorStateTransition);
      this.anyStateTransitions = stateTransitions;
      return animatorStateTransition;
    }

    /// <summary>
    ///   <para>Utility function to add an AnyState transition to the specified state or statemachine.</para>
    /// </summary>
    /// <param name="destinationState">The destination state.</param>
    /// <param name="destinationStateMachine">The destination statemachine.</param>
    public AnimatorStateTransition AddAnyStateTransition(AnimatorState destinationState)
    {
      AnimatorStateTransition animatorStateTransition = this.AddAnyStateTransition();
      animatorStateTransition.destinationState = destinationState;
      return animatorStateTransition;
    }

    /// <summary>
    ///   <para>Utility function to add an AnyState transition to the specified state or statemachine.</para>
    /// </summary>
    /// <param name="destinationState">The destination state.</param>
    /// <param name="destinationStateMachine">The destination statemachine.</param>
    public AnimatorStateTransition AddAnyStateTransition(AnimatorStateMachine destinationStateMachine)
    {
      AnimatorStateTransition animatorStateTransition = this.AddAnyStateTransition();
      animatorStateTransition.destinationStateMachine = destinationStateMachine;
      return animatorStateTransition;
    }

    /// <summary>
    ///   <para>Utility function to remove an AnyState transition from the state machine.</para>
    /// </summary>
    /// <param name="transition">The AnyStat transition to remove.</param>
    public bool RemoveAnyStateTransition(AnimatorStateTransition transition)
    {
      if (!new List<AnimatorStateTransition>((IEnumerable<AnimatorStateTransition>) this.anyStateTransitions).Any<AnimatorStateTransition>((Func<AnimatorStateTransition, bool>) (t => (UnityEngine.Object) t == (UnityEngine.Object) transition)))
        return false;
      this.undoHandler.DoUndo((UnityEngine.Object) this, "AnyState Transition Removed");
      AnimatorStateTransition[] stateTransitions = this.anyStateTransitions;
      ArrayUtility.Remove<AnimatorStateTransition>(ref stateTransitions, transition);
      this.anyStateTransitions = stateTransitions;
      if (MecanimUtilities.AreSameAsset((UnityEngine.Object) this, (UnityEngine.Object) transition))
        Undo.DestroyObjectImmediate((UnityEngine.Object) transition);
      return true;
    }

    internal void RemoveAnyStateTransitionRecursive(AnimatorStateTransition transition)
    {
      if (this.RemoveAnyStateTransition(transition))
        return;
      using (List<ChildAnimatorStateMachine>.Enumerator enumerator = this.stateMachinesRecursive.GetEnumerator())
      {
        do
          ;
        while (enumerator.MoveNext() && !enumerator.Current.stateMachine.RemoveAnyStateTransition(transition));
      }
    }

    /// <summary>
    ///   <para>Utility function to add an outgoing transition from the source state machine to the destination.</para>
    /// </summary>
    /// <param name="sourceStateMachine">The source state machine.</param>
    /// <param name="destinationStateMachine">The destination state machine.</param>
    /// <param name="destinationState">The destination state.</param>
    /// <returns>
    ///   <para>The Animations.AnimatorTransition transition that was created.</para>
    /// </returns>
    public AnimatorTransition AddStateMachineTransition(AnimatorStateMachine sourceStateMachine)
    {
      AnimatorStateMachine destinationStateMachine = (AnimatorStateMachine) null;
      return this.AddStateMachineTransition(sourceStateMachine, destinationStateMachine);
    }

    /// <summary>
    ///   <para>Utility function to add an outgoing transition from the source state machine to the destination.</para>
    /// </summary>
    /// <param name="sourceStateMachine">The source state machine.</param>
    /// <param name="destinationStateMachine">The destination state machine.</param>
    /// <param name="destinationState">The destination state.</param>
    /// <returns>
    ///   <para>The Animations.AnimatorTransition transition that was created.</para>
    /// </returns>
    public AnimatorTransition AddStateMachineTransition(AnimatorStateMachine sourceStateMachine, AnimatorStateMachine destinationStateMachine)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "StateMachine Transition Added");
      AnimatorTransition[] machineTransitions = this.GetStateMachineTransitions(sourceStateMachine);
      AnimatorTransition animatorTransition = new AnimatorTransition();
      if ((bool) ((UnityEngine.Object) destinationStateMachine))
        animatorTransition.destinationStateMachine = destinationStateMachine;
      if (AssetDatabase.GetAssetPath((UnityEngine.Object) this) != "")
        AssetDatabase.AddObjectToAsset((UnityEngine.Object) animatorTransition, AssetDatabase.GetAssetPath((UnityEngine.Object) this));
      animatorTransition.hideFlags = HideFlags.HideInHierarchy;
      ArrayUtility.Add<AnimatorTransition>(ref machineTransitions, animatorTransition);
      this.SetStateMachineTransitions(sourceStateMachine, machineTransitions);
      return animatorTransition;
    }

    /// <summary>
    ///   <para>Utility function to add an outgoing transition from the source state machine to the destination.</para>
    /// </summary>
    /// <param name="sourceStateMachine">The source state machine.</param>
    /// <param name="destinationStateMachine">The destination state machine.</param>
    /// <param name="destinationState">The destination state.</param>
    /// <returns>
    ///   <para>The Animations.AnimatorTransition transition that was created.</para>
    /// </returns>
    public AnimatorTransition AddStateMachineTransition(AnimatorStateMachine sourceStateMachine, AnimatorState destinationState)
    {
      AnimatorTransition animatorTransition = this.AddStateMachineTransition(sourceStateMachine);
      animatorTransition.destinationState = destinationState;
      return animatorTransition;
    }

    /// <summary>
    ///   <para>Utility function to add an outgoing transition from the source state machine to the exit of it's parent state machine.</para>
    /// </summary>
    /// <param name="sourceStateMachine">The source state machine.</param>
    public AnimatorTransition AddStateMachineExitTransition(AnimatorStateMachine sourceStateMachine)
    {
      AnimatorTransition animatorTransition = this.AddStateMachineTransition(sourceStateMachine);
      animatorTransition.isExit = true;
      return animatorTransition;
    }

    /// <summary>
    ///   <para>Utility function to remove an outgoing transition from source state machine.</para>
    /// </summary>
    /// <param name="transition">The transition to remove.</param>
    /// <param name="sourceStateMachine">The source state machine.</param>
    public bool RemoveStateMachineTransition(AnimatorStateMachine sourceStateMachine, AnimatorTransition transition)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "StateMachine Transition Removed");
      AnimatorTransition[] machineTransitions = this.GetStateMachineTransitions(sourceStateMachine);
      int length = machineTransitions.Length;
      ArrayUtility.Remove<AnimatorTransition>(ref machineTransitions, transition);
      this.SetStateMachineTransitions(sourceStateMachine, machineTransitions);
      if (MecanimUtilities.AreSameAsset((UnityEngine.Object) this, (UnityEngine.Object) transition))
        Undo.DestroyObjectImmediate((UnityEngine.Object) transition);
      return length != machineTransitions.Length;
    }

    private AnimatorTransition AddEntryTransition()
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "Entry Transition Added");
      AnimatorTransition[] entryTransitions = this.entryTransitions;
      AnimatorTransition animatorTransition = new AnimatorTransition();
      if (AssetDatabase.GetAssetPath((UnityEngine.Object) this) != "")
        AssetDatabase.AddObjectToAsset((UnityEngine.Object) animatorTransition, AssetDatabase.GetAssetPath((UnityEngine.Object) this));
      animatorTransition.hideFlags = HideFlags.HideInHierarchy;
      ArrayUtility.Add<AnimatorTransition>(ref entryTransitions, animatorTransition);
      this.entryTransitions = entryTransitions;
      return animatorTransition;
    }

    /// <summary>
    ///   <para>Utility function to add an incoming transition to the exit of it's parent state machine.</para>
    /// </summary>
    /// <param name="destinationState">The destination Animations.AnimatorState state.</param>
    /// <param name="destinationStateMachine">The destination Animations.AnimatorStateMachine state machine.</param>
    public AnimatorTransition AddEntryTransition(AnimatorState destinationState)
    {
      AnimatorTransition animatorTransition = this.AddEntryTransition();
      animatorTransition.destinationState = destinationState;
      return animatorTransition;
    }

    /// <summary>
    ///   <para>Utility function to add an incoming transition to the exit of it's parent state machine.</para>
    /// </summary>
    /// <param name="destinationState">The destination Animations.AnimatorState state.</param>
    /// <param name="destinationStateMachine">The destination Animations.AnimatorStateMachine state machine.</param>
    public AnimatorTransition AddEntryTransition(AnimatorStateMachine destinationStateMachine)
    {
      AnimatorTransition animatorTransition = this.AddEntryTransition();
      animatorTransition.destinationStateMachine = destinationStateMachine;
      return animatorTransition;
    }

    /// <summary>
    ///   <para>Utility function to remove an entry transition from the state machine.</para>
    /// </summary>
    /// <param name="transition">The transition to remove.</param>
    public bool RemoveEntryTransition(AnimatorTransition transition)
    {
      if (!new List<AnimatorTransition>((IEnumerable<AnimatorTransition>) this.entryTransitions).Any<AnimatorTransition>((Func<AnimatorTransition, bool>) (t => (UnityEngine.Object) t == (UnityEngine.Object) transition)))
        return false;
      this.undoHandler.DoUndo((UnityEngine.Object) this, "Entry Transition Removed");
      AnimatorTransition[] entryTransitions = this.entryTransitions;
      ArrayUtility.Remove<AnimatorTransition>(ref entryTransitions, transition);
      this.entryTransitions = entryTransitions;
      if (MecanimUtilities.AreSameAsset((UnityEngine.Object) this, (UnityEngine.Object) transition))
        Undo.DestroyObjectImmediate((UnityEngine.Object) transition);
      return true;
    }

    internal ChildAnimatorState FindState(int nameHash)
    {
      return new List<ChildAnimatorState>((IEnumerable<ChildAnimatorState>) this.states).Find((Predicate<ChildAnimatorState>) (s => s.state.nameHash == nameHash));
    }

    internal ChildAnimatorState FindState(string name)
    {
      return new List<ChildAnimatorState>((IEnumerable<ChildAnimatorState>) this.states).Find((Predicate<ChildAnimatorState>) (s => s.state.name == name));
    }

    internal bool HasState(AnimatorState state)
    {
      return this.statesRecursive.Any<ChildAnimatorState>((Func<ChildAnimatorState, bool>) (s => (UnityEngine.Object) s.state == (UnityEngine.Object) state));
    }

    internal bool IsDirectParent(AnimatorStateMachine stateMachine)
    {
      return ((IEnumerable<ChildAnimatorStateMachine>) this.stateMachines).Any<ChildAnimatorStateMachine>((Func<ChildAnimatorStateMachine, bool>) (sm => (UnityEngine.Object) sm.stateMachine == (UnityEngine.Object) stateMachine));
    }

    internal bool HasStateMachine(AnimatorStateMachine child)
    {
      return this.stateMachinesRecursive.Any<ChildAnimatorStateMachine>((Func<ChildAnimatorStateMachine, bool>) (sm => (UnityEngine.Object) sm.stateMachine == (UnityEngine.Object) child));
    }

    internal bool HasTransition(AnimatorState stateA, AnimatorState stateB)
    {
      return ((IEnumerable<AnimatorStateTransition>) stateA.transitions).Any<AnimatorStateTransition>((Func<AnimatorStateTransition, bool>) (t => (UnityEngine.Object) t.destinationState == (UnityEngine.Object) stateB)) || ((IEnumerable<AnimatorStateTransition>) stateB.transitions).Any<AnimatorStateTransition>((Func<AnimatorStateTransition, bool>) (t => (UnityEngine.Object) t.destinationState == (UnityEngine.Object) stateA));
    }

    internal AnimatorStateMachine FindParent(AnimatorStateMachine stateMachine)
    {
      if (((IEnumerable<ChildAnimatorStateMachine>) this.stateMachines).Any<ChildAnimatorStateMachine>((Func<ChildAnimatorStateMachine, bool>) (childSM => (UnityEngine.Object) childSM.stateMachine == (UnityEngine.Object) stateMachine)))
        return this;
      return this.stateMachinesRecursive.Find((Predicate<ChildAnimatorStateMachine>) (sm => ((IEnumerable<ChildAnimatorStateMachine>) sm.stateMachine.stateMachines).Any<ChildAnimatorStateMachine>((Func<ChildAnimatorStateMachine, bool>) (childSM => (UnityEngine.Object) childSM.stateMachine == (UnityEngine.Object) stateMachine)))).stateMachine;
    }

    internal AnimatorStateMachine FindStateMachine(string path)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimatorStateMachine.\u003CFindStateMachine\u003Ec__AnonStoreyB machineCAnonStoreyB = new AnimatorStateMachine.\u003CFindStateMachine\u003Ec__AnonStoreyB();
      // ISSUE: reference to a compiler-generated field
      machineCAnonStoreyB.smNames = path.Split('.');
      AnimatorStateMachine parent = this;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimatorStateMachine.\u003CFindStateMachine\u003Ec__AnonStoreyC machineCAnonStoreyC = new AnimatorStateMachine.\u003CFindStateMachine\u003Ec__AnonStoreyC();
      // ISSUE: reference to a compiler-generated field
      machineCAnonStoreyC.\u003C\u003Ef__ref\u002411 = machineCAnonStoreyB;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (machineCAnonStoreyC.i = 1; machineCAnonStoreyC.i < machineCAnonStoreyB.smNames.Length - 1 && (UnityEngine.Object) parent != (UnityEngine.Object) null; ++machineCAnonStoreyC.i)
      {
        ChildAnimatorStateMachine[] childStateMachines = AnimatorStateMachine.StateMachineCache.GetChildStateMachines(parent);
        // ISSUE: reference to a compiler-generated method
        int index = Array.FindIndex<ChildAnimatorStateMachine>(childStateMachines, new Predicate<ChildAnimatorStateMachine>(machineCAnonStoreyC.\u003C\u003Em__0));
        parent = index < 0 ? (AnimatorStateMachine) null : childStateMachines[index].stateMachine;
      }
      return !((UnityEngine.Object) parent == (UnityEngine.Object) null) ? parent : this;
    }

    internal AnimatorStateMachine FindStateMachine(AnimatorState state)
    {
      if (this.HasState(state, false))
        return this;
      List<ChildAnimatorStateMachine> machinesRecursive = this.stateMachinesRecursive;
      int index = machinesRecursive.FindIndex((Predicate<ChildAnimatorStateMachine>) (sm => sm.stateMachine.HasState(state, false)));
      return index < 0 ? (AnimatorStateMachine) null : machinesRecursive[index].stateMachine;
    }

    internal AnimatorStateTransition FindTransition(AnimatorState destinationState)
    {
      return new List<AnimatorStateTransition>((IEnumerable<AnimatorStateTransition>) this.anyStateTransitions).Find((Predicate<AnimatorStateTransition>) (t => (UnityEngine.Object) t.destinationState == (UnityEngine.Object) destinationState));
    }

    [Obsolete("stateCount is obsolete. Use .states.Length  instead.", true)]
    private int stateCount
    {
      get
      {
        return 0;
      }
    }

    [Obsolete("stateMachineCount is obsolete. Use .stateMachines.Length instead.", true)]
    private int stateMachineCount
    {
      get
      {
        return 0;
      }
    }

    [Obsolete("GetTransitionsFromState is obsolete. Use AnimatorState.transitions instead.", true)]
    private AnimatorState GetTransitionsFromState(AnimatorState state)
    {
      return (AnimatorState) null;
    }

    [Obsolete("uniqueNameHash does not exist anymore.", true)]
    private int uniqueNameHash
    {
      get
      {
        return -1;
      }
    }

    internal class StateMachineCache
    {
      private static Dictionary<AnimatorStateMachine, ChildAnimatorStateMachine[]> m_ChildStateMachines;
      private static bool m_Initialized;

      private static void Init()
      {
        if (AnimatorStateMachine.StateMachineCache.m_Initialized)
          return;
        AnimatorStateMachine.StateMachineCache.m_ChildStateMachines = new Dictionary<AnimatorStateMachine, ChildAnimatorStateMachine[]>();
        AnimatorStateMachine.StateMachineCache.m_Initialized = true;
      }

      public static void Clear()
      {
        AnimatorStateMachine.StateMachineCache.Init();
        AnimatorStateMachine.StateMachineCache.m_ChildStateMachines.Clear();
      }

      public static ChildAnimatorStateMachine[] GetChildStateMachines(AnimatorStateMachine parent)
      {
        AnimatorStateMachine.StateMachineCache.Init();
        ChildAnimatorStateMachine[] stateMachines;
        if (!AnimatorStateMachine.StateMachineCache.m_ChildStateMachines.TryGetValue(parent, out stateMachines))
        {
          stateMachines = parent.stateMachines;
          AnimatorStateMachine.StateMachineCache.m_ChildStateMachines.Add(parent, stateMachines);
        }
        return stateMachines;
      }
    }
  }
}
