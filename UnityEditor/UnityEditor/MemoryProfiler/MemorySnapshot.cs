// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryProfiler.MemorySnapshot
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditorInternal;

namespace UnityEditor.MemoryProfiler
{
  /// <summary>
  ///   <para>MemorySnapshot is a profiling tool to help diagnose memory usage.</para>
  /// </summary>
  public static class MemorySnapshot
  {
    public static event Action<PackedMemorySnapshot> OnSnapshotReceived;

    /// <summary>
    ///   <para>Requests a new snapshot from the currently connected target of the profiler. Currently only il2cpp-based players are able to provide memory snapshots.</para>
    /// </summary>
    public static void RequestNewSnapshot()
    {
      ProfilerDriver.RequestMemorySnapshot();
    }

    private static void DispatchSnapshot(PackedMemorySnapshot snapshot)
    {
      // ISSUE: reference to a compiler-generated field
      Action<PackedMemorySnapshot> snapshotReceived = MemorySnapshot.OnSnapshotReceived;
      if (snapshotReceived == null)
        return;
      snapshotReceived(snapshot);
    }
  }
}
