// Decompiled with JetBrains decompiler
// Type: UnityEngine.AI.NavMeshLinkInstance
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.AI
{
  /// <summary>
  ///   <para>An instance representing a link available for pathfinding.</para>
  /// </summary>
  public struct NavMeshLinkInstance
  {
    private int m_Handle;

    /// <summary>
    ///   <para>True if the NavMesh link is added to the navigation system - otherwise false (Read Only).</para>
    /// </summary>
    public bool valid
    {
      get
      {
        return this.m_Handle != 0 && NavMesh.IsValidLinkHandle(this.m_Handle);
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
    ///   <para>Removes this instance from the game.</para>
    /// </summary>
    public void Remove()
    {
      NavMesh.RemoveLinkInternal(this.id);
    }

    /// <summary>
    ///   <para>Get or set the owning Object.</para>
    /// </summary>
    public Object owner
    {
      get
      {
        return NavMesh.InternalGetLinkOwner(this.id);
      }
      set
      {
        if (NavMesh.InternalSetLinkOwner(this.id, !(value != (Object) null) ? 0 : value.GetInstanceID()))
          return;
        Debug.LogError((object) "Cannot set 'owner' on an invalid NavMeshLinkInstance");
      }
    }
  }
}
