// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.Tango.NativeImage
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.XR.Tango
{
  internal struct NativeImage
  {
    public uint width;
    public uint height;
    public int format;
    public long timestampNs;
    public IntPtr planeData;
    public IntPtr nativePtr;
    public List<ImageData.PlaneInfo> planeInfos;
    public ImageData.CameraMetadata metadata;
  }
}
