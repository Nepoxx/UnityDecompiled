// Decompiled with JetBrains decompiler
// Type: UnityEngine.iOS.OnDemandResourcesRequest
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.iOS
{
  [UsedByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class OnDemandResourcesRequest : AsyncOperation, IDisposable
  {
    internal OnDemandResourcesRequest()
    {
    }

    public extern string error { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern float loadingPriority { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetResourcePath(string resourceName);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void DestroyFromScript(IntPtr ptr);

    public void Dispose()
    {
      if (this.m_Ptr != IntPtr.Zero)
      {
        OnDemandResourcesRequest.DestroyFromScript(this.m_Ptr);
        this.m_Ptr = IntPtr.Zero;
      }
      GC.SuppressFinalize((object) this);
    }

    ~OnDemandResourcesRequest()
    {
      this.Dispose();
    }
  }
}
