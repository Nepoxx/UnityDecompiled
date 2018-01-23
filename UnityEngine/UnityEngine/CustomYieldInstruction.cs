// Decompiled with JetBrains decompiler
// Type: UnityEngine.CustomYieldInstruction
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Base class for custom yield instructions to suspend coroutines.</para>
  /// </summary>
  public abstract class CustomYieldInstruction : IEnumerator
  {
    /// <summary>
    ///   <para>Indicates if coroutine should be kept suspended.</para>
    /// </summary>
    public abstract bool keepWaiting { get; }

    public object Current
    {
      get
      {
        return (object) null;
      }
    }

    public bool MoveNext()
    {
      return this.keepWaiting;
    }

    public void Reset()
    {
    }
  }
}
