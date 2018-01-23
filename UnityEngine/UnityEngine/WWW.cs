// Decompiled with JetBrains decompiler
// Type: UnityEngine.WWW
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine.Networking;

namespace UnityEngine
{
  public class WWW : CustomYieldInstruction, IDisposable
  {
    private UnityWebRequest _uwr;
    private AssetBundle _assetBundle;
    private Dictionary<string, string> _responseHeaders;

    public WWW(string url)
    {
      this._uwr = UnityWebRequest.Get(url);
      this._uwr.SendWebRequest();
    }

    public WWW(string url, WWWForm form)
    {
      this._uwr = UnityWebRequest.Post(url, form);
      this._uwr.SendWebRequest();
    }

    public WWW(string url, byte[] postData)
    {
      this._uwr = new UnityWebRequest(url, "POST");
      UploadHandler uploadHandler = (UploadHandler) new UploadHandlerRaw(postData);
      uploadHandler.contentType = "application/x-www-form-urlencoded";
      this._uwr.uploadHandler = uploadHandler;
      this._uwr.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
      this._uwr.SendWebRequest();
    }

    [Obsolete("This overload is deprecated. Use UnityEngine.WWW.WWW(string, byte[], System.Collections.Generic.Dictionary<string, string>) instead.")]
    public WWW(string url, byte[] postData, Hashtable headers)
    {
      string method = postData != null ? "POST" : "GET";
      this._uwr = new UnityWebRequest(url, method);
      UploadHandler uploadHandler = (UploadHandler) new UploadHandlerRaw(postData);
      uploadHandler.contentType = "application/x-www-form-urlencoded";
      this._uwr.uploadHandler = uploadHandler;
      this._uwr.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
      IEnumerator enumerator = headers.Keys.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          object current = enumerator.Current;
          this._uwr.SetRequestHeader((string) current, (string) headers[current]);
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      this._uwr.SendWebRequest();
    }

    public WWW(string url, byte[] postData, Dictionary<string, string> headers)
    {
      string method = postData != null ? "POST" : "GET";
      this._uwr = new UnityWebRequest(url, method);
      UploadHandler uploadHandler = (UploadHandler) new UploadHandlerRaw(postData);
      uploadHandler.contentType = "application/x-www-form-urlencoded";
      this._uwr.uploadHandler = uploadHandler;
      this._uwr.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
      foreach (KeyValuePair<string, string> header in headers)
        this._uwr.SetRequestHeader(header.Key, header.Value);
      this._uwr.SendWebRequest();
    }

    internal WWW(string url, string name, Hash128 hash, uint crc)
    {
      this._uwr = UnityWebRequest.GetAssetBundle(url, new CachedAssetBundle(name, hash), crc);
      this._uwr.SendWebRequest();
    }

    public static string EscapeURL(string s)
    {
      return WWW.EscapeURL(s, Encoding.UTF8);
    }

    public static string EscapeURL(string s, Encoding e)
    {
      if (s == null)
        return (string) null;
      if (s == "")
        return "";
      if (e == null)
        return (string) null;
      return WWWTranscoder.URLEncode(s, e);
    }

    public static string UnEscapeURL(string s)
    {
      return WWW.UnEscapeURL(s, Encoding.UTF8);
    }

    public static string UnEscapeURL(string s, Encoding e)
    {
      if (s == null)
        return (string) null;
      if (s.IndexOf('%') == -1 && s.IndexOf('+') == -1)
        return s;
      return WWWTranscoder.URLDecode(s, e);
    }

    public static WWW LoadFromCacheOrDownload(string url, int version)
    {
      return WWW.LoadFromCacheOrDownload(url, version, 0U);
    }

    public static WWW LoadFromCacheOrDownload(string url, int version, uint crc)
    {
      Hash128 hash = new Hash128(0U, 0U, 0U, (uint) version);
      return WWW.LoadFromCacheOrDownload(url, hash, crc);
    }

    public static WWW LoadFromCacheOrDownload(string url, Hash128 hash)
    {
      return WWW.LoadFromCacheOrDownload(url, hash, 0U);
    }

    public static WWW LoadFromCacheOrDownload(string url, Hash128 hash, uint crc)
    {
      return new WWW(url, "", hash, crc);
    }

    public static WWW LoadFromCacheOrDownload(string url, CachedAssetBundle cachedBundle, uint crc = 0)
    {
      return new WWW(url, cachedBundle.name, cachedBundle.hash, crc);
    }

    public AssetBundle assetBundle
    {
      get
      {
        if ((Object) this._assetBundle == (Object) null)
        {
          if (!this.WaitUntilDoneIfPossible() || this._uwr.isNetworkError)
            return (AssetBundle) null;
          DownloadHandlerAssetBundle downloadHandler = this._uwr.downloadHandler as DownloadHandlerAssetBundle;
          if (downloadHandler != null)
          {
            this._assetBundle = downloadHandler.assetBundle;
          }
          else
          {
            byte[] bytes = this.bytes;
            if (bytes == null)
              return (AssetBundle) null;
            this._assetBundle = AssetBundle.LoadFromMemory(bytes);
          }
        }
        return this._assetBundle;
      }
    }

    [Obsolete("Obsolete msg (UnityUpgradable) -> * UnityEngine.WWW.GetAudioClip()", true)]
    public Object audioClip
    {
      get
      {
        return (Object) null;
      }
    }

    public byte[] bytes
    {
      get
      {
        if (!this.WaitUntilDoneIfPossible())
          return new byte[0];
        if (this._uwr.isNetworkError)
          return new byte[0];
        DownloadHandler downloadHandler = this._uwr.downloadHandler;
        if (downloadHandler == null)
          return new byte[0];
        return downloadHandler.data;
      }
    }

    [Obsolete("Obsolete msg (UnityUpgradable) -> * UnityEngine.WWW.GetMovieTexture()", true)]
    public Object movie
    {
      get
      {
        return (Object) null;
      }
    }

    [Obsolete("WWW.size is obsolete. Please use WWW.bytesDownloaded instead")]
    public int size
    {
      get
      {
        return this.bytesDownloaded;
      }
    }

    public int bytesDownloaded
    {
      get
      {
        return (int) this._uwr.downloadedBytes;
      }
    }

    public string error
    {
      get
      {
        if (!this._uwr.isDone)
          return (string) null;
        if (this._uwr.isNetworkError)
          return this._uwr.error;
        if (this._uwr.responseCode >= 400L)
          return string.Format("{0} {1}", (object) this._uwr.responseCode, (object) this.GetStatusCodeName(this._uwr.responseCode));
        return (string) null;
      }
    }

    public bool isDone
    {
      get
      {
        return this._uwr.isDone;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Obsolete msg (UnityUpgradable) -> * UnityEngine.WWW.GetAudioClip()", true)]
    public Object oggVorbis
    {
      get
      {
        return (Object) null;
      }
    }

    public float progress
    {
      get
      {
        float num = this._uwr.downloadProgress;
        if ((double) num < 0.0)
          num = 0.0f;
        return num;
      }
    }

    public Dictionary<string, string> responseHeaders
    {
      get
      {
        if (!this.isDone)
          return new Dictionary<string, string>();
        if (this._responseHeaders == null)
        {
          this._responseHeaders = this._uwr.GetResponseHeaders();
          if (this._responseHeaders != null)
            this._responseHeaders["STATUS"] = string.Format("HTTP/1.1 {0} {1}", (object) this._uwr.responseCode, (object) this.GetStatusCodeName(this._uwr.responseCode));
          else
            this._responseHeaders = new Dictionary<string, string>();
        }
        return this._responseHeaders;
      }
    }

    [Obsolete("Please use WWW.text instead. (UnityUpgradable) -> text", true)]
    public string data
    {
      get
      {
        return this.text;
      }
    }

    public string text
    {
      get
      {
        if (!this.WaitUntilDoneIfPossible() || this._uwr.isNetworkError)
          return "";
        DownloadHandler downloadHandler = this._uwr.downloadHandler;
        if (downloadHandler == null)
          return "";
        return downloadHandler.text;
      }
    }

    private Texture2D CreateTextureFromDownloadedData(bool markNonReadable)
    {
      if (!this.WaitUntilDoneIfPossible())
        return new Texture2D(2, 2);
      if (this._uwr.isNetworkError)
        return (Texture2D) null;
      DownloadHandler downloadHandler = this._uwr.downloadHandler;
      if (downloadHandler == null)
        return (Texture2D) null;
      Texture2D tex = new Texture2D(2, 2);
      tex.LoadImage(downloadHandler.data, markNonReadable);
      return tex;
    }

    public Texture2D texture
    {
      get
      {
        return this.CreateTextureFromDownloadedData(false);
      }
    }

    public Texture2D textureNonReadable
    {
      get
      {
        return this.CreateTextureFromDownloadedData(true);
      }
    }

    public void LoadImageIntoTexture(Texture2D texture)
    {
      if (!this.WaitUntilDoneIfPossible())
        return;
      if (this._uwr.isNetworkError)
      {
        Debug.LogError((object) "Cannot load image: download failed");
      }
      else
      {
        DownloadHandler downloadHandler = this._uwr.downloadHandler;
        if (downloadHandler == null)
          Debug.LogError((object) "Cannot load image: internal error");
        else
          texture.LoadImage(downloadHandler.data, false);
      }
    }

    public ThreadPriority threadPriority { get; set; }

    public float uploadProgress
    {
      get
      {
        float num = this._uwr.uploadProgress;
        if ((double) num < 0.0)
          num = 0.0f;
        return num;
      }
    }

    public string url
    {
      get
      {
        return this._uwr.url;
      }
    }

    public override bool keepWaiting
    {
      get
      {
        return !this._uwr.isDone;
      }
    }

    public void Dispose()
    {
      this._uwr.Dispose();
    }

    internal Object GetAudioClipInternal(bool threeD, bool stream, bool compressed, AudioType audioType)
    {
      return (Object) WebRequestWWW.InternalCreateAudioClipUsingDH(this._uwr.downloadHandler, this._uwr.url, stream, compressed, audioType);
    }

    internal object GetMovieTextureInternal()
    {
      return (object) WebRequestWWW.InternalCreateMovieTextureUsingDH(this._uwr.downloadHandler);
    }

    public AudioClip GetAudioClip()
    {
      return this.GetAudioClip(true, false, AudioType.UNKNOWN);
    }

    public AudioClip GetAudioClip(bool threeD)
    {
      return this.GetAudioClip(threeD, false, AudioType.UNKNOWN);
    }

    public AudioClip GetAudioClip(bool threeD, bool stream)
    {
      return this.GetAudioClip(threeD, stream, AudioType.UNKNOWN);
    }

    public AudioClip GetAudioClip(bool threeD, bool stream, AudioType audioType)
    {
      return (AudioClip) this.GetAudioClipInternal(threeD, stream, false, audioType);
    }

    public AudioClip GetAudioClipCompressed()
    {
      return this.GetAudioClipCompressed(false, AudioType.UNKNOWN);
    }

    public AudioClip GetAudioClipCompressed(bool threeD)
    {
      return this.GetAudioClipCompressed(threeD, AudioType.UNKNOWN);
    }

    public AudioClip GetAudioClipCompressed(bool threeD, AudioType audioType)
    {
      return (AudioClip) this.GetAudioClipInternal(threeD, false, true, audioType);
    }

    public MovieTexture GetMovieTexture()
    {
      return (MovieTexture) this.GetMovieTextureInternal();
    }

    private bool WaitUntilDoneIfPossible()
    {
      if (this._uwr.isDone)
        return true;
      if (this.url.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
      {
        do
          ;
        while (!this._uwr.isDone);
        return true;
      }
      Debug.LogError((object) "You are trying to load data from a www stream which has not completed the download yet.\nYou need to yield the download or wait until isDone returns true.");
      return false;
    }

    private string GetStatusCodeName(long statusCode)
    {
      if (statusCode >= 400L && statusCode <= 416L)
      {
        switch (statusCode)
        {
          case 400:
            return "Bad Request";
          case 401:
            return "Unauthorized";
          case 402:
            return "Payment Required";
          case 403:
            return "Forbidden";
          case 404:
            return "Not Found";
          case 405:
            return "Method Not Allowed";
          case 406:
            return "Not Acceptable";
          case 407:
            return "Proxy Authentication Required";
          case 408:
            return "Request Timeout";
          case 409:
            return "Conflict";
          case 410:
            return "Gone";
          case 411:
            return "Length Required";
          case 412:
            return "Precondition Failed";
          case 413:
            return "Request Entity Too Large";
          case 414:
            return "Request-URI Too Long";
          case 415:
            return "Unsupported Media Type";
          case 416:
            return "Requested Range Not Satisfiable";
        }
      }
      if (statusCode >= 200L && statusCode <= 206L)
      {
        switch (statusCode)
        {
          case 200:
            return "OK";
          case 201:
            return "Created";
          case 202:
            return "Accepted";
          case 203:
            return "Non-Authoritative Information";
          case 204:
            return "No Content";
          case 205:
            return "Reset Content";
          case 206:
            return "Partial Content";
        }
      }
      if (statusCode >= 300L && statusCode <= 307L)
      {
        switch (statusCode)
        {
          case 300:
            return "Multiple Choices";
          case 301:
            return "Moved Permanently";
          case 302:
            return "Found";
          case 303:
            return "See Other";
          case 304:
            return "Not Modified";
          case 305:
            return "Use Proxy";
          case 307:
            return "Temporary Redirect";
        }
      }
      if (statusCode >= 500L && statusCode <= 505L)
      {
        switch (statusCode)
        {
          case 500:
            return "Internal Server Error";
          case 501:
            return "Not Implemented";
          case 502:
            return "Bad Gateway";
          case 503:
            return "Service Unavailable";
          case 504:
            return "Gateway Timeout";
          case 505:
            return "HTTP Version Not Supported";
        }
      }
      return statusCode == 41L ? "Expectation Failed" : "";
    }
  }
}
