// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.DownloadHandlerFile
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Networking
{
  [StructLayout(LayoutKind.Sequential)]
  public sealed class DownloadHandlerFile : DownloadHandler
  {
    public DownloadHandlerFile(string path)
    {
      this.InternalCreateVFS(path);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern IntPtr Create(DownloadHandlerFile obj, string path);

    private void InternalCreateVFS(string path)
    {
      this.m_Ptr = DownloadHandlerFile.Create(this, path);
    }

    protected override byte[] GetData()
    {
      throw new NotSupportedException("Raw data access is not supported");
    }

    protected override string GetText()
    {
      throw new NotSupportedException("String access is not supported");
    }

    public extern bool removeFileOnAbort { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
