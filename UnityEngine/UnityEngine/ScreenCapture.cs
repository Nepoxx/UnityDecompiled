// Decompiled with JetBrains decompiler
// Type: UnityEngine.ScreenCapture
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
  public static class ScreenCapture
  {
    public static void CaptureScreenshot(string filename)
    {
      ScreenCapture.CaptureScreenshot(filename, 1);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CaptureScreenshot(string filename, [DefaultValue("1")] int superSize);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Texture2D CaptureScreenshotAsTexture(int superSize = 1);
  }
}
