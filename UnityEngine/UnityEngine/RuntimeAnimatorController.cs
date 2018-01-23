// Decompiled with JetBrains decompiler
// Type: UnityEngine.RuntimeAnimatorController
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Runtime representation of the AnimatorController. It can be used to change the Animator's controller during runtime.</para>
  /// </summary>
  public class RuntimeAnimatorController : Object
  {
    /// <summary>
    ///   <para>Retrieves all AnimationClip used by the controller.</para>
    /// </summary>
    public extern AnimationClip[] animationClips { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
