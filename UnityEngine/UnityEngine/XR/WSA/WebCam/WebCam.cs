// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.WebCam.WebCam
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.WebCam
{
  /// <summary>
  ///   <para>Contains general information about the current state of the web camera.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA.WebCam")]
  public static class WebCam
  {
    /// <summary>
    ///   <para>Specifies what mode the Web Camera is currently in.</para>
    /// </summary>
    public static WebCamMode Mode
    {
      get
      {
        return (WebCamMode) UnityEngine.XR.WSA.WebCam.WebCam.GetWebCamModeState_Internal();
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetWebCamModeState_Internal();
  }
}
