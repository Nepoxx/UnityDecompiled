// Decompiled with JetBrains decompiler
// Type: UnityEngine.MeshSubsetCombineUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;

namespace UnityEngine
{
  internal class MeshSubsetCombineUtility
  {
    public struct MeshInstance
    {
      public int meshInstanceID;
      public int rendererInstanceID;
      public int additionalVertexStreamsMeshInstanceID;
      public Matrix4x4 transform;
      public Vector4 lightmapScaleOffset;
      public Vector4 realtimeLightmapScaleOffset;
    }

    public struct SubMeshInstance
    {
      public int meshInstanceID;
      public int vertexOffset;
      public int gameObjectInstanceID;
      public int subMeshIndex;
      public Matrix4x4 transform;
    }

    public struct MeshContainer
    {
      public GameObject gameObject;
      public MeshSubsetCombineUtility.MeshInstance instance;
      public List<MeshSubsetCombineUtility.SubMeshInstance> subMeshInstances;
    }
  }
}
