// Decompiled with JetBrains decompiler
// Type: UnityEngine.SharedBetweenAnimatorsAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>SharedBetweenAnimatorsAttribute is an attribute that specify that this StateMachineBehaviour should be instantiate only once and shared among all Animator instance. This attribute reduce the memory footprint for each controller instance.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  [RequiredByNativeCode]
  public sealed class SharedBetweenAnimatorsAttribute : Attribute
  {
  }
}
