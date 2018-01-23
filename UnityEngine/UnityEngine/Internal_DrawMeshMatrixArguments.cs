// Decompiled with JetBrains decompiler
// Type: UnityEngine.Internal_DrawMeshMatrixArguments
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  internal struct Internal_DrawMeshMatrixArguments
  {
    public int layer;
    public int submeshIndex;
    public Matrix4x4 matrix;
    public int castShadows;
    public int receiveShadows;
    public int reflectionProbeAnchorInstanceID;
    public bool useLightProbes;
  }
}
