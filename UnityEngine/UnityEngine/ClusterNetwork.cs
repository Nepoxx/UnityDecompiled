// Decompiled with JetBrains decompiler
// Type: UnityEngine.ClusterNetwork
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A helper class that contains static method to inquire status of Unity Cluster.</para>
  /// </summary>
  public sealed class ClusterNetwork
  {
    /// <summary>
    ///   <para>Check whether the current instance is a master node in the cluster network.</para>
    /// </summary>
    public static extern bool isMasterOfCluster { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Check whether the current instance is disconnected from the cluster network.</para>
    /// </summary>
    public static extern bool isDisconnected { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>To acquire or set the node index of the current machine from the cluster network.</para>
    /// </summary>
    public static extern int nodeIndex { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
