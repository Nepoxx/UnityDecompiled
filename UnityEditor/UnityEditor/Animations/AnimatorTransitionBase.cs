// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.AnimatorTransitionBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>Base class for animator transitions. Transitions define when and how the state machine switches from one state to another.</para>
  /// </summary>
  public class AnimatorTransitionBase : Object
  {
    private PushUndoIfNeeded undoHandler = new PushUndoIfNeeded(true);

    public string GetDisplayName(Object source)
    {
      return !(source is AnimatorState) ? this.GetDisplayNameStateMachineSource(source as AnimatorStateMachine) : this.GetDisplayNameStateSource(source as AnimatorState);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern string GetDisplayNameStateSource(AnimatorState source);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern string GetDisplayNameStateMachineSource(AnimatorStateMachine source);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string BuildTransitionName(string source, string destination);

    /// <summary>
    ///   <para>Mutes all other transitions in the source state.</para>
    /// </summary>
    public extern bool solo { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Mutes the transition. The transition will never occur.</para>
    /// </summary>
    public extern bool mute { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is the transition destination the exit of the current state machine.</para>
    /// </summary>
    public extern bool isExit { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The destination state machine of the transition.</para>
    /// </summary>
    public extern AnimatorStateMachine destinationStateMachine { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The destination state of the transition.</para>
    /// </summary>
    public extern AnimatorState destinationState { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Animations.AnimatorCondition conditions that need to be met for a transition to happen.</para>
    /// </summary>
    public extern AnimatorCondition[] conditions { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal bool pushUndo
    {
      set
      {
        this.undoHandler.pushUndo = value;
      }
    }

    /// <summary>
    ///   <para>Utility function to add a condition to a transition.</para>
    /// </summary>
    /// <param name="mode">The Animations.AnimatorCondition mode of the condition.</param>
    /// <param name="threshold">The threshold value of the condition.</param>
    /// <param name="parameter">The name of the parameter.</param>
    public void AddCondition(AnimatorConditionMode mode, float threshold, string parameter)
    {
      this.undoHandler.DoUndo((Object) this, "Condition added");
      AnimatorCondition[] conditions = this.conditions;
      ArrayUtility.Add<AnimatorCondition>(ref conditions, new AnimatorCondition()
      {
        mode = mode,
        parameter = parameter,
        threshold = threshold
      });
      this.conditions = conditions;
    }

    /// <summary>
    ///   <para>Utility function to remove a condition from the transition.</para>
    /// </summary>
    /// <param name="condition">The condition to remove.</param>
    public void RemoveCondition(AnimatorCondition condition)
    {
      this.undoHandler.DoUndo((Object) this, "Condition removed");
      AnimatorCondition[] conditions = this.conditions;
      ArrayUtility.Remove<AnimatorCondition>(ref conditions, condition);
      this.conditions = conditions;
    }
  }
}
