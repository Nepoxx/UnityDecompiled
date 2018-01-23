// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.DownloadHandlerBuffer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Networking
{
  [StructLayout(LayoutKind.Sequential)]
  public sealed class DownloadHandlerBuffer : DownloadHandler
  {
    public DownloadHandlerBuffer()
    {
      this.InternalCreateBuffer();
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern IntPtr Create(DownloadHandlerBuffer obj);

    private void InternalCreateBuffer()
    {
      this.m_Ptr = DownloadHandlerBuffer.Create(this);
    }

    protected override byte[] GetData()
    {
      return this.InternalGetData();
    }

    private byte[] InternalGetData()
    {
      return DownloadHandler.InternalGetByteArray((DownloadHandler) this);
    }

    public static string GetContent(UnityWebRequest www)
    {
      return DownloadHandler.GetCheckedDownloader<DownloadHandlerBuffer>(www).text;
    }
  }
}
