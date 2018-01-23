// Decompiled with JetBrains decompiler
// Type: UnityEngine.AI.NavMeshDataInstance
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.AI
{
  /// <summary>
  ///   <para>The instance is returned when adding NavMesh data.</para>
  /// </summary>
  public struct NavMeshDataInstance
  {
    private int m_Handle;

    /// <summary>
    ///   <para>True if the NavMesh data is added to the navigation system - otherwise false (Read Only).</para>
    /// </summary>
    public bool valid
    {
      get
      {
        return this.m_Handle != 0 && NavMesh.IsValidNavMeshDataHandle(this.m_Handle);
      }
    }

    internal int id
    {
      get
      {
        return this.m_Handle;
      }
      set
      {
        this.m_Handle = value;
      }
    }

    /// <summary>
    ///   <para>Removes this instance from the NavMesh system.</para>
    /// </summary>
    public void Remove()
    {
      NavMesh.RemoveNavMeshDataInternal(this.id);
    }

    /// <summary>
    ///   <para>Get or set the owning Object.</para>
    /// </summary>
    public Object owner
    {
      get
      {
        return NavMesh.InternalGetOwner(this.id);
      }
      set
      {
        if (NavMesh.InternalSetOwner(this.id, !(value != (Object) null) ? 0 : value.GetInstanceID()))
          return;
        Debug.LogError((object) "Cannot set 'owner' on an invalid NavMeshDataInstance");
      }
    }
  }
}
