// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.StateMachineBehaviourContext
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>This class contains all the owner's information for this State Machine Behaviour.</para>
  /// </summary>
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class StateMachineBehaviourContext
  {
    /// <summary>
    ///   <para>The Animations.AnimatorController that owns this state machine behaviour.</para>
    /// </summary>
    public AnimatorController animatorController;
    /// <summary>
    ///   <para>The object that owns this state machine behaviour. Could be an Animations.AnimatorState or Animations.AnimatorStateMachine.</para>
    /// </summary>
    public UnityEngine.Object animatorObject;
    /// <summary>
    ///   <para>The animator's layer index that owns this state machine behaviour.</para>
    /// </summary>
    public int layerIndex;
  }
}
