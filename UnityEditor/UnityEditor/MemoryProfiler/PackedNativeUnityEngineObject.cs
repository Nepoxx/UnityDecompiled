// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryProfiler.PackedNativeUnityEngineObject
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.MemoryProfiler
{
  /// <summary>
  ///   <para>Description of a C++ unity object in memory.</para>
  /// </summary>
  [Serializable]
  public struct PackedNativeUnityEngineObject
  {
    [SerializeField]
    internal string m_Name;
    [SerializeField]
    internal int m_InstanceId;
    [SerializeField]
    internal int m_Size;
    [SerializeField]
    internal int m_NativeTypeArrayIndex;
    [SerializeField]
    internal HideFlags m_HideFlags;
    [SerializeField]
    internal PackedNativeUnityEngineObject.ObjectFlags m_Flags;
    [SerializeField]
    internal long m_NativeObjectAddress;

    /// <summary>
    ///   <para>Is this object persistent? (Assets are persistent, objects stored in scenes are persistent,  dynamically created objects are not)</para>
    /// </summary>
    public bool isPersistent
    {
      get
      {
        return (this.m_Flags & PackedNativeUnityEngineObject.ObjectFlags.IsPersistent) != (PackedNativeUnityEngineObject.ObjectFlags) 0;
      }
    }

    /// <summary>
    ///   <para>Has this object has been marked as DontDestroyOnLoad?</para>
    /// </summary>
    public bool isDontDestroyOnLoad
    {
      get
      {
        return (this.m_Flags & PackedNativeUnityEngineObject.ObjectFlags.IsDontDestroyOnLoad) != (PackedNativeUnityEngineObject.ObjectFlags) 0;
      }
    }

    /// <summary>
    ///   <para>Is this native object an internal Unity manager object?</para>
    /// </summary>
    public bool isManager
    {
      get
      {
        return (this.m_Flags & PackedNativeUnityEngineObject.ObjectFlags.IsManager) != (PackedNativeUnityEngineObject.ObjectFlags) 0;
      }
    }

    /// <summary>
    ///   <para>Name of this object.</para>
    /// </summary>
    public string name
    {
      get
      {
        return this.m_Name;
      }
    }

    /// <summary>
    ///   <para>InstanceId of this object.</para>
    /// </summary>
    public int instanceId
    {
      get
      {
        return this.m_InstanceId;
      }
    }

    /// <summary>
    ///   <para>Size in bytes of this object.</para>
    /// </summary>
    public int size
    {
      get
      {
        return this.m_Size;
      }
    }

    [Obsolete("PackedNativeUnityEngineObject.classId is obsolete. Use PackedNativeUnityEngineObject.nativeTypeArrayIndex instead (UnityUpgradable) -> nativeTypeArrayIndex")]
    public int classId
    {
      get
      {
        return this.m_NativeTypeArrayIndex;
      }
    }

    /// <summary>
    ///   <para>The index used to obtain the native C++ type description from the PackedMemorySnapshot.nativeTypes array.</para>
    /// </summary>
    public int nativeTypeArrayIndex
    {
      get
      {
        return this.m_NativeTypeArrayIndex;
      }
    }

    /// <summary>
    ///   <para>The hideFlags this native object has.</para>
    /// </summary>
    public HideFlags hideFlags
    {
      get
      {
        return this.m_HideFlags;
      }
    }

    /// <summary>
    ///   <para>The memory address of the native C++ object. This matches the "m_CachedPtr" field of UnityEngine.Object.</para>
    /// </summary>
    public long nativeObjectAddress
    {
      get
      {
        return this.m_NativeObjectAddress;
      }
    }

    internal enum ObjectFlags
    {
      IsDontDestroyOnLoad = 1,
      IsPersistent = 2,
      IsManager = 4,
    }
  }
}
