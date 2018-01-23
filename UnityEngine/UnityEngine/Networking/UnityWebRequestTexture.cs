// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.UnityWebRequestTexture
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Networking
{
  public static class UnityWebRequestTexture
  {
    public static UnityWebRequest GetTexture(string uri)
    {
      return UnityWebRequestTexture.GetTexture(uri, false);
    }

    public static UnityWebRequest GetTexture(string uri, bool nonReadable)
    {
      return new UnityWebRequest(uri, "GET", (DownloadHandler) new DownloadHandlerTexture(!nonReadable), (UploadHandler) null);
    }
  }
}
