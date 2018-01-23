// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimationClipPair
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>This class defines a pair of clips used by AnimatorOverrideController.</para>
  /// </summary>
  [Obsolete("This class is not used anymore.  See AnimatorOverrideController.GetOverrides() and AnimatorOverrideController.ApplyOverrides()")]
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class AnimationClipPair
  {
    /// <summary>
    ///   <para>The original clip from the controller.</para>
    /// </summary>
    public AnimationClip originalClip;
    /// <summary>
    ///   <para>The override animation clip.</para>
    /// </summary>
    public AnimationClip overrideClip;
  }
}
