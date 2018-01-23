// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.DownloadHandlerAudioClip
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Networking
{
  [StructLayout(LayoutKind.Sequential)]
  public sealed class DownloadHandlerAudioClip : DownloadHandler
  {
    public DownloadHandlerAudioClip(string url, AudioType audioType)
    {
      this.InternalCreateAudioClip(url, audioType);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern IntPtr Create(DownloadHandlerAudioClip obj, string url, AudioType audioType);

    private void InternalCreateAudioClip(string url, AudioType audioType)
    {
      this.m_Ptr = DownloadHandlerAudioClip.Create(this, url, audioType);
    }

    protected override byte[] GetData()
    {
      return DownloadHandler.InternalGetByteArray((DownloadHandler) this);
    }

    protected override string GetText()
    {
      throw new NotSupportedException("String access is not supported for audio clips");
    }

    public extern AudioClip audioClip { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static AudioClip GetContent(UnityWebRequest www)
    {
      return DownloadHandler.GetCheckedDownloader<DownloadHandlerAudioClip>(www).audioClip;
    }
  }
}
