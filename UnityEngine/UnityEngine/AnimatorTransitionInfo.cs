// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimatorTransitionInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Information about the current transition.</para>
  /// </summary>
  [RequiredByNativeCode]
  public struct AnimatorTransitionInfo
  {
    private int m_FullPath;
    private int m_UserName;
    private int m_Name;
    private bool m_HasFixedDuration;
    private float m_Duration;
    private float m_NormalizedTime;
    private bool m_AnyState;
    private int m_TransitionType;

    /// <summary>
    ///   <para>Does name match the name of the active Transition.</para>
    /// </summary>
    /// <param name="name"></param>
    public bool IsName(string name)
    {
      return Animator.StringToHash(name) == this.m_Name || Animator.StringToHash(name) == this.m_FullPath;
    }

    /// <summary>
    ///   <para>Does userName match the name of the active Transition.</para>
    /// </summary>
    /// <param name="name"></param>
    public bool IsUserName(string name)
    {
      return Animator.StringToHash(name) == this.m_UserName;
    }

    /// <summary>
    ///   <para>The hash name of the Transition.</para>
    /// </summary>
    public int fullPathHash
    {
      get
      {
        return this.m_FullPath;
      }
    }

    /// <summary>
    ///   <para>The simplified name of the Transition.</para>
    /// </summary>
    public int nameHash
    {
      get
      {
        return this.m_Name;
      }
    }

    /// <summary>
    ///   <para>The user-specified name of the Transition.</para>
    /// </summary>
    public int userNameHash
    {
      get
      {
        return this.m_UserName;
      }
    }

    /// <summary>
    ///   <para>The unit of the transition duration.</para>
    /// </summary>
    public DurationUnit durationUnit
    {
      get
      {
        return !this.m_HasFixedDuration ? DurationUnit.Normalized : DurationUnit.Fixed;
      }
    }

    /// <summary>
    ///   <para>Duration of the transition.</para>
    /// </summary>
    public float duration
    {
      get
      {
        return this.m_Duration;
      }
    }

    /// <summary>
    ///   <para>Normalized time of the Transition.</para>
    /// </summary>
    public float normalizedTime
    {
      get
      {
        return this.m_NormalizedTime;
      }
    }

    /// <summary>
    ///   <para>Returns true if the transition is from an AnyState node, or from Animator.CrossFade().</para>
    /// </summary>
    public bool anyState
    {
      get
      {
        return this.m_AnyState;
      }
    }

    internal bool entry
    {
      get
      {
        return (this.m_TransitionType & 2) != 0;
      }
    }

    internal bool exit
    {
      get
      {
        return (this.m_TransitionType & 4) != 0;
      }
    }
  }
}
