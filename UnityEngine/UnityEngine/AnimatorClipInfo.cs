// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimatorClipInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Information about clip being played and blended by the Animator.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct AnimatorClipInfo
  {
    private int m_ClipInstanceID;
    private float m_Weight;

    /// <summary>
    ///   <para>Returns the animation clip played by the Animator.</para>
    /// </summary>
    public AnimationClip clip
    {
      get
      {
        return this.m_ClipInstanceID == 0 ? (AnimationClip) null : AnimatorClipInfo.ClipInstanceToScriptingObject(this.m_ClipInstanceID);
      }
    }

    /// <summary>
    ///   <para>Returns the blending weight used by the Animator to blend this clip.</para>
    /// </summary>
    public float weight
    {
      get
      {
        return this.m_Weight;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AnimationClip ClipInstanceToScriptingObject(int instanceID);
  }
}
