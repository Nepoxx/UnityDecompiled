// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.UploadHandler
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Networking
{
  [StructLayout(LayoutKind.Sequential)]
  public class UploadHandler : IDisposable
  {
    [NonSerialized]
    internal IntPtr m_Ptr;

    internal UploadHandler()
    {
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Release();

    ~UploadHandler()
    {
      this.Dispose();
    }

    public void Dispose()
    {
      if (!(this.m_Ptr != IntPtr.Zero))
        return;
      this.Release();
      this.m_Ptr = IntPtr.Zero;
    }

    public byte[] data
    {
      get
      {
        return this.GetData();
      }
    }

    public string contentType
    {
      get
      {
        return this.GetContentType();
      }
      set
      {
        this.SetContentType(value);
      }
    }

    public float progress
    {
      get
      {
        return this.GetProgress();
      }
    }

    internal virtual byte[] GetData()
    {
      return (byte[]) null;
    }

    internal virtual string GetContentType()
    {
      return "text/plain";
    }

    internal virtual void SetContentType(string newContentType)
    {
    }

    internal virtual float GetProgress()
    {
      return 0.5f;
    }
  }
}
