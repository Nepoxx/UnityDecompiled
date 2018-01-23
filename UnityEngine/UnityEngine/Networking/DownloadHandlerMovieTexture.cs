// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.DownloadHandlerMovieTexture
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Networking
{
  [StructLayout(LayoutKind.Sequential)]
  public sealed class DownloadHandlerMovieTexture : DownloadHandler
  {
    public DownloadHandlerMovieTexture()
    {
      this.InternalCreateDHMovieTexture();
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern IntPtr Create(DownloadHandlerMovieTexture obj);

    private void InternalCreateDHMovieTexture()
    {
      this.m_Ptr = DownloadHandlerMovieTexture.Create(this);
    }

    protected override byte[] GetData()
    {
      return DownloadHandler.InternalGetByteArray((DownloadHandler) this);
    }

    protected override string GetText()
    {
      throw new NotSupportedException("String access is not supported for movies");
    }

    public extern MovieTexture movieTexture { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static MovieTexture GetContent(UnityWebRequest uwr)
    {
      return DownloadHandler.GetCheckedDownloader<DownloadHandlerMovieTexture>(uwr).movieTexture;
    }
  }
}
