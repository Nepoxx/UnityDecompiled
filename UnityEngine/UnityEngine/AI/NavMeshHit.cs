// Decompiled with JetBrains decompiler
// Type: UnityEngine.AI.NavMeshHit
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI
{
  /// <summary>
  ///   <para>Result information for NavMesh queries.</para>
  /// </summary>
  [MovedFrom("UnityEngine")]
  public struct NavMeshHit
  {
    private Vector3 m_Position;
    private Vector3 m_Normal;
    private float m_Distance;
    private int m_Mask;
    private int m_Hit;

    /// <summary>
    ///   <para>Position of hit.</para>
    /// </summary>
    public Vector3 position
    {
      get
      {
        return this.m_Position;
      }
      set
      {
        this.m_Position = value;
      }
    }

    /// <summary>
    ///   <para>Normal at the point of hit.</para>
    /// </summary>
    public Vector3 normal
    {
      get
      {
        return this.m_Normal;
      }
      set
      {
        this.m_Normal = value;
      }
    }

    /// <summary>
    ///   <para>Distance to the point of hit.</para>
    /// </summary>
    public float distance
    {
      get
      {
        return this.m_Distance;
      }
      set
      {
        this.m_Distance = value;
      }
    }

    /// <summary>
    ///   <para>Mask specifying NavMesh area at point of hit.</para>
    /// </summary>
    public int mask
    {
      get
      {
        return this.m_Mask;
      }
      set
      {
        this.m_Mask = value;
      }
    }

    /// <summary>
    ///   <para>Flag set when hit.</para>
    /// </summary>
    public bool hit
    {
      get
      {
        return this.m_Hit != 0;
      }
      set
      {
        this.m_Hit = !value ? 0 : 1;
      }
    }
  }
}
