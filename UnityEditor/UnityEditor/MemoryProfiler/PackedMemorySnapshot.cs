// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryProfiler.PackedMemorySnapshot
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.MemoryProfiler
{
  /// <summary>
  ///   <para>PackedMemorySnapshot is a compact representation of a memory snapshot that a player has sent through the profiler connection.</para>
  /// </summary>
  [Serializable]
  public class PackedMemorySnapshot
  {
    [SerializeField]
    internal PackedNativeType[] m_NativeTypes = (PackedNativeType[]) null;
    [SerializeField]
    internal PackedNativeUnityEngineObject[] m_NativeObjects = (PackedNativeUnityEngineObject[]) null;
    [SerializeField]
    internal PackedGCHandle[] m_GcHandles = (PackedGCHandle[]) null;
    [SerializeField]
    internal Connection[] m_Connections = (Connection[]) null;
    [SerializeField]
    internal MemorySection[] m_ManagedHeapSections = (MemorySection[]) null;
    [SerializeField]
    internal MemorySection[] m_ManagedStacks = (MemorySection[]) null;
    [SerializeField]
    internal TypeDescription[] m_TypeDescriptions = (TypeDescription[]) null;
    [SerializeField]
    internal VirtualMachineInformation m_VirtualMachineInformation = new VirtualMachineInformation();

    internal PackedMemorySnapshot()
    {
    }

    /// <summary>
    ///   <para>Descriptions of all the C++ unity types the profiled player knows about.</para>
    /// </summary>
    public PackedNativeType[] nativeTypes
    {
      get
      {
        return this.m_NativeTypes;
      }
    }

    /// <summary>
    ///   <para>All native C++ objects that were loaded at time of the snapshot.</para>
    /// </summary>
    public PackedNativeUnityEngineObject[] nativeObjects
    {
      get
      {
        return this.m_NativeObjects;
      }
    }

    /// <summary>
    ///   <para>All GC handles in use in the memorysnapshot.</para>
    /// </summary>
    public PackedGCHandle[] gcHandles
    {
      get
      {
        return this.m_GcHandles;
      }
    }

    /// <summary>
    ///   <para>Connections is an array of from,to pairs that describe which things are keeping which other things alive.</para>
    /// </summary>
    public Connection[] connections
    {
      get
      {
        return this.m_Connections;
      }
    }

    /// <summary>
    ///   <para>Array of actual managed heap memory sections.</para>
    /// </summary>
    public MemorySection[] managedHeapSections
    {
      get
      {
        return this.m_ManagedHeapSections;
      }
    }

    /// <summary>
    ///   <para>Descriptions of all the managed types that were known to the virtual machine when the snapshot was taken.</para>
    /// </summary>
    public TypeDescription[] typeDescriptions
    {
      get
      {
        return this.m_TypeDescriptions;
      }
    }

    /// <summary>
    ///   <para>Information about the virtual machine running executing the managade code inside the player.</para>
    /// </summary>
    public VirtualMachineInformation virtualMachineInformation
    {
      get
      {
        return this.m_VirtualMachineInformation;
      }
    }
  }
}
