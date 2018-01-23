// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.WebCam.CameraParameters
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Linq;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.WebCam
{
  /// <summary>
  ///   <para>When calling PhotoCapture.StartPhotoModeAsync, you must pass in a CameraParameters object that contains the various settings that the web camera will use.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA.WebCam")]
  public struct CameraParameters
  {
    private float m_HologramOpacity;
    private float m_FrameRate;
    private int m_CameraResolutionWidth;
    private int m_CameraResolutionHeight;
    private CapturePixelFormat m_PixelFormat;

    public CameraParameters(WebCamMode webCamMode)
    {
      this.m_HologramOpacity = 1f;
      this.m_PixelFormat = CapturePixelFormat.BGRA32;
      this.m_FrameRate = 0.0f;
      this.m_CameraResolutionWidth = 0;
      this.m_CameraResolutionHeight = 0;
      switch (webCamMode)
      {
        case WebCamMode.PhotoMode:
          Resolution resolution1 = PhotoCapture.SupportedResolutions.OrderByDescending<Resolution, int>((Func<Resolution, int>) (res => res.width * res.height)).First<Resolution>();
          this.m_CameraResolutionWidth = resolution1.width;
          this.m_CameraResolutionHeight = resolution1.height;
          break;
        case WebCamMode.VideoMode:
          Resolution resolution2 = VideoCapture.SupportedResolutions.OrderByDescending<Resolution, int>((Func<Resolution, int>) (res => res.width * res.height)).First<Resolution>();
          float num = VideoCapture.GetSupportedFrameRatesForResolution(resolution2).OrderByDescending<float, float>((Func<float, float>) (fps => fps)).First<float>();
          this.m_CameraResolutionWidth = resolution2.width;
          this.m_CameraResolutionHeight = resolution2.height;
          this.m_FrameRate = num;
          break;
      }
    }

    /// <summary>
    ///   <para>The opacity of captured holograms.</para>
    /// </summary>
    public float hologramOpacity
    {
      get
      {
        return this.m_HologramOpacity;
      }
      set
      {
        this.m_HologramOpacity = value;
      }
    }

    /// <summary>
    ///   <para>The framerate at which to capture video.  This is only for use with VideoCapture.</para>
    /// </summary>
    public float frameRate
    {
      get
      {
        return this.m_FrameRate;
      }
      set
      {
        this.m_FrameRate = value;
      }
    }

    /// <summary>
    ///   <para>A valid width resolution for use with the web camera.</para>
    /// </summary>
    public int cameraResolutionWidth
    {
      get
      {
        return this.m_CameraResolutionWidth;
      }
      set
      {
        this.m_CameraResolutionWidth = value;
      }
    }

    /// <summary>
    ///   <para>A valid height resolution for use with the web camera.</para>
    /// </summary>
    public int cameraResolutionHeight
    {
      get
      {
        return this.m_CameraResolutionHeight;
      }
      set
      {
        this.m_CameraResolutionHeight = value;
      }
    }

    /// <summary>
    ///   <para>The pixel format used to capture and record your image data.</para>
    /// </summary>
    public CapturePixelFormat pixelFormat
    {
      get
      {
        return this.m_PixelFormat;
      }
      set
      {
        this.m_PixelFormat = value;
      }
    }
  }
}
