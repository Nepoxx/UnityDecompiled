// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Input.InteractionSourceDetectedEventArgs
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;

namespace UnityEngine.XR.WSA.Input
{
  /// <summary>
  ///   <para>Contains fields that are relevent when an interaction source is detected.</para>
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct InteractionSourceDetectedEventArgs
  {
    public InteractionSourceDetectedEventArgs(InteractionSourceState state)
    {
      this = new InteractionSourceDetectedEventArgs();
      this.state = state;
    }

    /// <summary>
    ///   <para>The current state of the reported interaction source that was just detected.</para>
    /// </summary>
    public InteractionSourceState state { get; private set; }
  }
}
