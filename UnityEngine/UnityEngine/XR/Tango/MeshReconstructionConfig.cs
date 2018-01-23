// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.Tango.MeshReconstructionConfig
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine.XR.Tango
{
  [UsedByNativeCode]
  internal struct MeshReconstructionConfig
  {
    public double resolution;
    public double minDepth;
    public double maxDepth;
    public int minNumVertices;
    public bool useParallelIntegration;
    public bool generateColor;
    public bool useSpaceClearing;
    public UpdateMethod updateMethod;

    public static MeshReconstructionConfig GetDefault()
    {
      return new MeshReconstructionConfig() { resolution = 0.03, minDepth = 0.6, maxDepth = 3.5, useParallelIntegration = false, generateColor = true, useSpaceClearing = false, minNumVertices = 1, updateMethod = UpdateMethod.Traversal };
    }
  }
}
