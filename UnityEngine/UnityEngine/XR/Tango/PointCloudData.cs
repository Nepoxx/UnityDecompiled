// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.Tango.PointCloudData
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;

namespace UnityEngine.XR.Tango
{
  internal struct PointCloudData
  {
    public uint version;
    public double timestamp;
    public List<Vector4> points;
  }
}
