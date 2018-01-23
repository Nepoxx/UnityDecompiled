// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.UnityWebRequestMultimedia
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Networking
{
  public static class UnityWebRequestMultimedia
  {
    public static UnityWebRequest GetAudioClip(string uri, AudioType audioType)
    {
      return new UnityWebRequest(uri, "GET", (DownloadHandler) new DownloadHandlerAudioClip(uri, audioType), (UploadHandler) null);
    }

    public static UnityWebRequest GetMovieTexture(string uri)
    {
      return new UnityWebRequest(uri, "GET", (DownloadHandler) new DownloadHandlerMovieTexture(), (UploadHandler) null);
    }
  }
}
