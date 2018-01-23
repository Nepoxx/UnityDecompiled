// Decompiled with JetBrains decompiler
// Type: UnityEngine.WaitWhile
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Suspends the coroutine execution until the supplied delegate evaluates to false.</para>
  /// </summary>
  public sealed class WaitWhile : CustomYieldInstruction
  {
    private Func<bool> m_Predicate;

    public WaitWhile(Func<bool> predicate)
    {
      this.m_Predicate = predicate;
    }

    public override bool keepWaiting
    {
      get
      {
        return this.m_Predicate();
      }
    }
  }
}
