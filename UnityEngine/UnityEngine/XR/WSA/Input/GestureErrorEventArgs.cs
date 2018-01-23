// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Input.GestureErrorEventArgs
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
  /// <summary>
  ///   <para>Contains fields that are relevant during an error event.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA.Input")]
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct GestureErrorEventArgs
  {
    public GestureErrorEventArgs(string error, int hresult)
    {
      this = new GestureErrorEventArgs();
      this.error = error;
      this.hresult = hresult;
    }

    /// <summary>
    ///   <para>A readable error string (when possible).</para>
    /// </summary>
    public string error { get; private set; }

    /// <summary>
    ///   <para>The HRESULT code from the platform.</para>
    /// </summary>
    public int hresult { get; private set; }
  }
}
