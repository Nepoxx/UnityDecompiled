// Decompiled with JetBrains decompiler
// Type: UnityEngine.AI.NavMeshBuildDebugSettings
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.AI
{
  /// <summary>
  ///   <para>Specify which of the temporary data generated while building the NavMesh should be retained in memory after the process has completed.</para>
  /// </summary>
  public struct NavMeshBuildDebugSettings
  {
    private byte m_Flags;

    /// <summary>
    ///   <para>Specify which types of debug data to collect when building the NavMesh.</para>
    /// </summary>
    public NavMeshBuildDebugFlags flags
    {
      get
      {
        return (NavMeshBuildDebugFlags) this.m_Flags;
      }
      set
      {
        this.m_Flags = (byte) value;
      }
    }
  }
}
