// Decompiled with JetBrains decompiler
// Type: UnityEngine.AtomicSafetyHandle
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  [NativeType(Header = "Runtime/Jobs/AtomicSafetyHandle.h")]
  internal struct AtomicSafetyHandle
  {
    internal IntPtr versionNode;
    internal AtomicSafetyHandleVersionMask version;

    public static AtomicSafetyHandle Create()
    {
      AtomicSafetyHandle ret;
      AtomicSafetyHandle.Create_Injected(out ret);
      return ret;
    }

    public static void Release(AtomicSafetyHandle handle)
    {
      AtomicSafetyHandle.Release_Injected(ref handle);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void PrepareUndisposable(ref AtomicSafetyHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void UseSecondaryVersion(ref AtomicSafetyHandle handle);

    public static void CheckWriteAndBumpSecondaryVersion(AtomicSafetyHandle handle)
    {
      AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion_Injected(ref handle);
    }

    public static void EnforceAllBufferJobsHaveCompletedAndRelease(AtomicSafetyHandle handle)
    {
      AtomicSafetyHandle.EnforceAllBufferJobsHaveCompletedAndRelease_Injected(ref handle);
    }

    internal static void CheckReadAndThrowNoEarlyOut(AtomicSafetyHandle handle)
    {
      AtomicSafetyHandle.CheckReadAndThrowNoEarlyOut_Injected(ref handle);
    }

    internal static void CheckWriteAndThrowNoEarlyOut(AtomicSafetyHandle handle)
    {
      AtomicSafetyHandle.CheckWriteAndThrowNoEarlyOut_Injected(ref handle);
    }

    public static void CheckDeallocateAndThrow(AtomicSafetyHandle handle)
    {
      AtomicSafetyHandle.CheckDeallocateAndThrow_Injected(ref handle);
    }

    public static void CheckGetSecondaryDataPointerAndThrow(AtomicSafetyHandle handle)
    {
      AtomicSafetyHandle.CheckGetSecondaryDataPointerAndThrow_Injected(ref handle);
    }

    public static unsafe void CheckReadAndThrow(AtomicSafetyHandle handle)
    {
      AtomicSafetyHandleVersionMask* versionNode = (AtomicSafetyHandleVersionMask*) (void*) handle.versionNode;
      if ((handle.version & AtomicSafetyHandleVersionMask.Read) != ~(AtomicSafetyHandleVersionMask.WriteInv | AtomicSafetyHandleVersionMask.Write) || handle.version == (*versionNode & AtomicSafetyHandleVersionMask.WriteInv))
        return;
      AtomicSafetyHandle.CheckReadAndThrowNoEarlyOut(handle);
    }

    public static unsafe void CheckWriteAndThrow(AtomicSafetyHandle handle)
    {
      AtomicSafetyHandleVersionMask* versionNode = (AtomicSafetyHandleVersionMask*) (void*) handle.versionNode;
      if ((handle.version & AtomicSafetyHandleVersionMask.Write) != ~(AtomicSafetyHandleVersionMask.WriteInv | AtomicSafetyHandleVersionMask.Write) || handle.version == (*versionNode & AtomicSafetyHandleVersionMask.ReadInv))
        return;
      AtomicSafetyHandle.CheckWriteAndThrowNoEarlyOut(handle);
    }

    public static unsafe void CheckExistsAndThrow(AtomicSafetyHandle handle)
    {
      AtomicSafetyHandleVersionMask* versionNode = (AtomicSafetyHandleVersionMask*) (void*) handle.versionNode;
      if ((handle.version & AtomicSafetyHandleVersionMask.ReadWriteAndDisposeInv) != (*versionNode & AtomicSafetyHandleVersionMask.ReadWriteAndDisposeInv))
        throw new InvalidOperationException("The NativeArray has been deallocated, it is not allowed to access it");
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Create_Injected(out AtomicSafetyHandle ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Release_Injected(ref AtomicSafetyHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void CheckWriteAndBumpSecondaryVersion_Injected(ref AtomicSafetyHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void EnforceAllBufferJobsHaveCompletedAndRelease_Injected(ref AtomicSafetyHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void CheckReadAndThrowNoEarlyOut_Injected(ref AtomicSafetyHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void CheckWriteAndThrowNoEarlyOut_Injected(ref AtomicSafetyHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void CheckDeallocateAndThrow_Injected(ref AtomicSafetyHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void CheckGetSecondaryDataPointerAndThrow_Injected(ref AtomicSafetyHandle handle);
  }
}
