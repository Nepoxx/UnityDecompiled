// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.WebCam.CapturePixelFormat
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.WebCam
{
  /// <summary>
  ///   <para>The encoded image or video pixel format to use for PhotoCapture and VideoCapture.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA.WebCam")]
  public enum CapturePixelFormat
  {
    BGRA32,
    NV12,
    JPEG,
    PNG,
  }
}
