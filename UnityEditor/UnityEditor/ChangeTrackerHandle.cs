// Decompiled with JetBrains decompiler
// Type: UnityEditor.ChangeTrackerHandle
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [RequiredByNativeCode]
  internal struct ChangeTrackerHandle
  {
    private IntPtr m_Handle;

    internal static ChangeTrackerHandle AcquireTracker(UnityEngine.Object obj)
    {
      if (obj == (UnityEngine.Object) null)
        throw new ArgumentNullException("Not a valid unity engine object");
      return ChangeTrackerHandle.Internal_AcquireTracker(obj);
    }

    private static ChangeTrackerHandle Internal_AcquireTracker(UnityEngine.Object o)
    {
      ChangeTrackerHandle changeTrackerHandle;
      ChangeTrackerHandle.INTERNAL_CALL_Internal_AcquireTracker(o, out changeTrackerHandle);
      return changeTrackerHandle;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_AcquireTracker(UnityEngine.Object o, out ChangeTrackerHandle value);

    internal void ReleaseTracker()
    {
      if (this.m_Handle == IntPtr.Zero)
        throw new ArgumentNullException("Not a valid handle, has it been released already?");
      ChangeTrackerHandle.Internal_ReleaseTracker(this);
      this.m_Handle = IntPtr.Zero;
    }

    private static void Internal_ReleaseTracker(ChangeTrackerHandle h)
    {
      ChangeTrackerHandle.INTERNAL_CALL_Internal_ReleaseTracker(ref h);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_ReleaseTracker(ref ChangeTrackerHandle h);

    internal bool PollForChanges()
    {
      if (this.m_Handle == IntPtr.Zero)
        throw new ArgumentNullException("Not a valid handle, has it been released already?");
      return this.Internal_PollChanges();
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool Internal_PollChanges();

    internal void ForceDirtyNextPoll()
    {
      if (this.m_Handle == IntPtr.Zero)
        throw new ArgumentNullException("Not a valid handle, has it been released already?");
      this.Internal_ForceUpdate();
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_ForceUpdate();
  }
}
