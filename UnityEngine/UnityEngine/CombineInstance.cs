// Decompiled with JetBrains decompiler
// Type: UnityEngine.CombineInstance
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  public struct CombineInstance
  {
    private int m_MeshInstanceID;
    private int m_SubMeshIndex;
    private Matrix4x4 m_Transform;
    private Vector4 m_LightmapScaleOffset;
    private Vector4 m_RealtimeLightmapScaleOffset;

    public Mesh mesh
    {
      get
      {
        return Mesh.FromInstanceID(this.m_MeshInstanceID);
      }
      set
      {
        this.m_MeshInstanceID = !((Object) value != (Object) null) ? 0 : value.GetInstanceID();
      }
    }

    public int subMeshIndex
    {
      get
      {
        return this.m_SubMeshIndex;
      }
      set
      {
        this.m_SubMeshIndex = value;
      }
    }

    public Matrix4x4 transform
    {
      get
      {
        return this.m_Transform;
      }
      set
      {
        this.m_Transform = value;
      }
    }

    public Vector4 lightmapScaleOffset
    {
      get
      {
        return this.m_LightmapScaleOffset;
      }
      set
      {
        this.m_LightmapScaleOffset = value;
      }
    }

    public Vector4 realtimeLightmapScaleOffset
    {
      get
      {
        return this.m_RealtimeLightmapScaleOffset;
      }
      set
      {
        this.m_RealtimeLightmapScaleOffset = value;
      }
    }
  }
}
