// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.GPUFence
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
  /// <summary>
  ///   <para>Used to manage synchronisation between tasks on async compute queues and the graphics queue.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct GPUFence
  {
    internal IntPtr m_Ptr;
    internal int m_Version;

    /// <summary>
    ///         <para>Has the GPUFence passed?
    /// 
    /// Allows for CPU determination of whether the GPU has passed the point in its processing represented by the GPUFence.</para>
    ///       </summary>
    public bool passed
    {
      get
      {
        this.Validate();
        if (!SystemInfo.supportsGPUFence)
          throw new NotSupportedException("Cannot determine if this GPUFence has passed as this platform has not implemented GPUFences.");
        if (!this.IsFencePending())
          return true;
        return this.HasFencePassed_Internal(this.m_Ptr);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool HasFencePassed_Internal(IntPtr fencePtr);

    internal void InitPostAllocation()
    {
      if (this.m_Ptr == IntPtr.Zero)
      {
        if (SystemInfo.supportsGPUFence)
          throw new NullReferenceException("The internal fence ptr is null, this should not be possible for fences that have been correctly constructed using Graphics.CreateGPUFence() or CommandBuffer.CreateGPUFence()");
        this.m_Version = this.GetPlatformNotSupportedVersion();
      }
      else
        this.m_Version = this.GetVersionNumber(this.m_Ptr);
    }

    internal bool IsFencePending()
    {
      if (this.m_Ptr == IntPtr.Zero)
        return false;
      return this.m_Version == this.GetVersionNumber(this.m_Ptr);
    }

    internal void Validate()
    {
      if (this.m_Version == 0 || SystemInfo.supportsGPUFence && this.m_Version == this.GetPlatformNotSupportedVersion())
        throw new InvalidOperationException("This GPUFence object has not been correctly constructed see Graphics.CreateGPUFence() or CommandBuffer.CreateGPUFence()");
    }

    private int GetPlatformNotSupportedVersion()
    {
      return -1;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int GetVersionNumber(IntPtr fencePtr);
  }
}
