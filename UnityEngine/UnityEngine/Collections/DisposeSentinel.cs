// Decompiled with JetBrains decompiler
// Type: UnityEngine.Collections.DisposeSentinel
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace UnityEngine.Collections
{
  [StructLayout(LayoutKind.Sequential)]
  internal class DisposeSentinel
  {
    private IntPtr m_Ptr;
    private DisposeSentinel.DeallocateDelegate m_DeallocateDelegate;
    private Allocator m_Label;
    private AtomicSafetyHandle m_Safety;
    private string m_FileName;
    private int m_LineNumber;

    public static void Dispose(AtomicSafetyHandle safety, ref DisposeSentinel sentinel)
    {
      AtomicSafetyHandle.CheckDeallocateAndThrow(safety);
      AtomicSafetyHandle.Release(safety);
      DisposeSentinel.Clear(ref sentinel);
    }

    public static void Create(IntPtr ptr, Allocator label, out AtomicSafetyHandle safety, out DisposeSentinel sentinel, int callSiteStackDepth, DisposeSentinel.DeallocateDelegate deallocateDelegate = null)
    {
      safety = AtomicSafetyHandle.Create();
      if (NativeLeakDetection.Mode == NativeLeakDetectionMode.Enabled)
      {
        sentinel = new DisposeSentinel();
        StackFrame stackFrame = new StackFrame(callSiteStackDepth + 2, true);
        sentinel.m_FileName = stackFrame.GetFileName();
        sentinel.m_LineNumber = stackFrame.GetFileLineNumber();
        sentinel.m_Ptr = ptr;
        sentinel.m_Label = label;
        sentinel.m_DeallocateDelegate = deallocateDelegate;
        sentinel.m_Safety = safety;
      }
      else
        sentinel = (DisposeSentinel) null;
    }

    ~DisposeSentinel()
    {
      if (!(this.m_Ptr != IntPtr.Zero))
        return;
      UnsafeUtility.LogError(string.Format("A Native Collection created with Allocator.{2} has not been disposed, resulting in a memory leak. It was allocated at {0}:{1}.", (object) this.m_FileName, (object) this.m_LineNumber, (object) this.m_Label), this.m_FileName, this.m_LineNumber);
      AtomicSafetyHandle.EnforceAllBufferJobsHaveCompletedAndRelease(this.m_Safety);
      if (this.m_DeallocateDelegate != null)
        this.m_DeallocateDelegate(this.m_Ptr, this.m_Label);
      else
        UnsafeUtility.Free(this.m_Ptr, this.m_Label);
    }

    public static void Clear(ref DisposeSentinel sentinel)
    {
      if (sentinel == null)
        return;
      sentinel.m_Ptr = IntPtr.Zero;
      sentinel = (DisposeSentinel) null;
    }

    public delegate void DeallocateDelegate(IntPtr buffer, Allocator allocator);
  }
}
