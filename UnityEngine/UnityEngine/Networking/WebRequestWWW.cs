// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.WebRequestWWW
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine.Networking
{
  internal static class WebRequestWWW
  {
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern AudioClip InternalCreateAudioClipUsingDH(DownloadHandler dh, string url, bool stream, bool compressed, AudioType audioType);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern MovieTexture InternalCreateMovieTextureUsingDH(DownloadHandler dh);
  }
}
