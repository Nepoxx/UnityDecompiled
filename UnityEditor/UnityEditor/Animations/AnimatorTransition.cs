// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.AnimatorTransition
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>Transitions define when and how the state machine switch from on state to another. AnimatorTransition always originate from a StateMachine or a StateMachine entry. They do not define timing parameters.</para>
  /// </summary>
  public sealed class AnimatorTransition : AnimatorTransitionBase
  {
    /// <summary>
    ///   <para>Creates a new animator transition.</para>
    /// </summary>
    public AnimatorTransition()
    {
      AnimatorTransition.Internal_Create(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create(AnimatorTransition mono);
  }
}
