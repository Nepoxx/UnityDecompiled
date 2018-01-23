// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimationInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Information about what animation clips is played and its weight.</para>
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Use AnimatorClipInfo instead (UnityUpgradable) -> AnimatorClipInfo", true)]
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct AnimationInfo
  {
    /// <summary>
    ///   <para>Animation clip that is played.</para>
    /// </summary>
    public AnimationClip clip
    {
      get
      {
        return (AnimationClip) null;
      }
    }

    /// <summary>
    ///   <para>The weight of the animation clip.</para>
    /// </summary>
    public float weight
    {
      get
      {
        return 0.0f;
      }
    }
  }
}
