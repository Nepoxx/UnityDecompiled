// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.ChildAnimatorState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>Structure that represents a state in the context of its parent state machine.</para>
  /// </summary>
  [RequiredByNativeCode]
  public struct ChildAnimatorState
  {
    private AnimatorState m_State;
    private Vector3 m_Position;

    /// <summary>
    ///   <para>The state.</para>
    /// </summary>
    public AnimatorState state
    {
      get
      {
        return this.m_State;
      }
      set
      {
        this.m_State = value;
      }
    }

    /// <summary>
    ///   <para>The position the the state node in the context of its parent state machine.</para>
    /// </summary>
    public Vector3 position
    {
      get
      {
        return this.m_Position;
      }
      set
      {
        this.m_Position = value;
      }
    }
  }
}
