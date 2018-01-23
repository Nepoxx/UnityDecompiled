// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.UnityWebRequest
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngineInternal;

namespace UnityEngine.Networking
{
  [StructLayout(LayoutKind.Sequential)]
  public class UnityWebRequest : IDisposable
  {
    [NonSerialized]
    internal IntPtr m_Ptr;
    [NonSerialized]
    internal DownloadHandler m_DownloadHandler;
    [NonSerialized]
    internal UploadHandler m_UploadHandler;
    public const string kHttpVerbGET = "GET";
    public const string kHttpVerbHEAD = "HEAD";
    public const string kHttpVerbPOST = "POST";
    public const string kHttpVerbPUT = "PUT";
    public const string kHttpVerbCREATE = "CREATE";
    public const string kHttpVerbDELETE = "DELETE";

    public UnityWebRequest()
    {
      this.m_Ptr = UnityWebRequest.Create();
      this.InternalSetDefaults();
    }

    public UnityWebRequest(string url)
    {
      this.m_Ptr = UnityWebRequest.Create();
      this.InternalSetDefaults();
      this.url = url;
    }

    public UnityWebRequest(string url, string method)
    {
      this.m_Ptr = UnityWebRequest.Create();
      this.InternalSetDefaults();
      this.url = url;
      this.method = method;
    }

    public UnityWebRequest(string url, string method, DownloadHandler downloadHandler, UploadHandler uploadHandler)
    {
      this.m_Ptr = UnityWebRequest.Create();
      this.InternalSetDefaults();
      this.url = url;
      this.method = method;
      this.downloadHandler = downloadHandler;
      this.uploadHandler = uploadHandler;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetWebErrorString(UnityWebRequest.UnityWebRequestError err);

    public bool disposeDownloadHandlerOnDispose { get; set; }

    public bool disposeUploadHandlerOnDispose { get; set; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern IntPtr Create();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Release();

    internal void InternalDestroy()
    {
      if (!(this.m_Ptr != IntPtr.Zero))
        return;
      this.Abort();
      this.Release();
      this.m_Ptr = IntPtr.Zero;
    }

    private void InternalSetDefaults()
    {
      this.disposeDownloadHandlerOnDispose = true;
      this.disposeUploadHandlerOnDispose = true;
    }

    ~UnityWebRequest()
    {
      this.DisposeHandlers();
      this.InternalDestroy();
    }

    public void Dispose()
    {
      this.DisposeHandlers();
      this.InternalDestroy();
      GC.SuppressFinalize((object) this);
    }

    private void DisposeHandlers()
    {
      if (this.disposeDownloadHandlerOnDispose)
      {
        DownloadHandler downloadHandler = this.downloadHandler;
        if (downloadHandler != null)
          downloadHandler.Dispose();
      }
      if (!this.disposeUploadHandlerOnDispose)
        return;
      UploadHandler uploadHandler = this.uploadHandler;
      if (uploadHandler != null)
        uploadHandler.Dispose();
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern UnityWebRequestAsyncOperation BeginWebRequest();

    [Obsolete("Use SendWebRequest.  It returns a UnityWebRequestAsyncOperation which contains a reference to the WebRequest object.", false)]
    public AsyncOperation Send()
    {
      return (AsyncOperation) this.SendWebRequest();
    }

    public UnityWebRequestAsyncOperation SendWebRequest()
    {
      UnityWebRequestAsyncOperation requestAsyncOperation = this.BeginWebRequest();
      requestAsyncOperation.webRequest = this;
      return requestAsyncOperation;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Abort();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetMethod(UnityWebRequest.UnityWebRequestMethod methodType);

    internal void InternalSetMethod(UnityWebRequest.UnityWebRequestMethod methodType)
    {
      if (!this.isModifiable)
        throw new InvalidOperationException("UnityWebRequest has already been sent and its request method can no longer be altered");
      UnityWebRequest.UnityWebRequestError err = this.SetMethod(methodType);
      if (err != UnityWebRequest.UnityWebRequestError.OK)
        throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetCustomMethod(string customMethodName);

    internal void InternalSetCustomMethod(string customMethodName)
    {
      if (!this.isModifiable)
        throw new InvalidOperationException("UnityWebRequest has already been sent and its request method can no longer be altered");
      UnityWebRequest.UnityWebRequestError err = this.SetCustomMethod(customMethodName);
      if (err != UnityWebRequest.UnityWebRequestError.OK)
        throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern UnityWebRequest.UnityWebRequestMethod GetMethod();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern string GetCustomMethod();

    public string method
    {
      get
      {
        switch (this.GetMethod())
        {
          case UnityWebRequest.UnityWebRequestMethod.Get:
            return "GET";
          case UnityWebRequest.UnityWebRequestMethod.Post:
            return "POST";
          case UnityWebRequest.UnityWebRequestMethod.Put:
            return "PUT";
          case UnityWebRequest.UnityWebRequestMethod.Head:
            return "HEAD";
          default:
            return this.GetCustomMethod();
        }
      }
      set
      {
        if (string.IsNullOrEmpty(value))
          throw new ArgumentException("Cannot set a UnityWebRequest's method to an empty or null string");
        switch (value.ToUpper())
        {
          case "GET":
            this.InternalSetMethod(UnityWebRequest.UnityWebRequestMethod.Get);
            break;
          case "POST":
            this.InternalSetMethod(UnityWebRequest.UnityWebRequestMethod.Post);
            break;
          case "PUT":
            this.InternalSetMethod(UnityWebRequest.UnityWebRequestMethod.Put);
            break;
          case "HEAD":
            this.InternalSetMethod(UnityWebRequest.UnityWebRequestMethod.Head);
            break;
          default:
            this.InternalSetCustomMethod(value.ToUpper());
            break;
        }
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError GetError();

    public string error
    {
      get
      {
        if (!this.isNetworkError && !this.isHttpError)
          return (string) null;
        return UnityWebRequest.GetWebErrorString(this.GetError());
      }
    }

    private extern bool use100Continue { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public bool useHttpContinue
    {
      get
      {
        return this.use100Continue;
      }
      set
      {
        if (!this.isModifiable)
          throw new InvalidOperationException("UnityWebRequest has already been sent and its 100-Continue setting cannot be altered");
        this.use100Continue = value;
      }
    }

    public string url
    {
      get
      {
        return this.GetUrl();
      }
      set
      {
        string localUrl = "http://localhost/";
        this.InternalSetUrl(WebRequestUtils.MakeInitialUrl(value, localUrl));
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern string GetUrl();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetUrl(string url);

    private void InternalSetUrl(string url)
    {
      if (!this.isModifiable)
        throw new InvalidOperationException("UnityWebRequest has already been sent and its URL cannot be altered");
      UnityWebRequest.UnityWebRequestError err = this.SetUrl(url);
      if (err != UnityWebRequest.UnityWebRequestError.OK)
        throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
    }

    public extern long responseCode { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern float GetUploadProgress();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool IsExecuting();

    public float uploadProgress
    {
      get
      {
        if (!this.IsExecuting() && !this.isDone)
          return -1f;
        return this.GetUploadProgress();
      }
    }

    public extern bool isModifiable { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern bool isDone { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern bool isNetworkError { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern bool isHttpError { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern float GetDownloadProgress();

    public float downloadProgress
    {
      get
      {
        if (!this.IsExecuting() && !this.isDone)
          return -1f;
        return this.GetDownloadProgress();
      }
    }

    public extern ulong uploadedBytes { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern ulong downloadedBytes { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int GetRedirectLimit();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetRedirectLimitFromScripting(int limit);

    public int redirectLimit
    {
      get
      {
        return this.GetRedirectLimit();
      }
      set
      {
        this.SetRedirectLimitFromScripting(value);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool GetChunked();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetChunked(bool chunked);

    public bool chunkedTransfer
    {
      get
      {
        return this.GetChunked();
      }
      set
      {
        if (!this.isModifiable)
          throw new InvalidOperationException("UnityWebRequest has already been sent and its chunked transfer encoding setting cannot be altered");
        UnityWebRequest.UnityWebRequestError err = this.SetChunked(value);
        if (err != UnityWebRequest.UnityWebRequestError.OK)
          throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetRequestHeader(string name);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern UnityWebRequest.UnityWebRequestError InternalSetRequestHeader(string name, string value);

    public void SetRequestHeader(string name, string value)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException("Cannot set a Request Header with a null or empty name");
      if (value == null)
        throw new ArgumentException("Cannot set a Request header with a null");
      if (!this.isModifiable)
        throw new InvalidOperationException("UnityWebRequest has already been sent and its request headers cannot be altered");
      UnityWebRequest.UnityWebRequestError err = this.InternalSetRequestHeader(name, value);
      if (err != UnityWebRequest.UnityWebRequestError.OK)
        throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetResponseHeader(string name);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern string[] GetResponseHeaderKeys();

    public Dictionary<string, string> GetResponseHeaders()
    {
      string[] responseHeaderKeys = this.GetResponseHeaderKeys();
      if (responseHeaderKeys == null || responseHeaderKeys.Length == 0)
        return (Dictionary<string, string>) null;
      Dictionary<string, string> dictionary = new Dictionary<string, string>(responseHeaderKeys.Length, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      for (int index = 0; index < responseHeaderKeys.Length; ++index)
      {
        string responseHeader = this.GetResponseHeader(responseHeaderKeys[index]);
        dictionary.Add(responseHeaderKeys[index], responseHeader);
      }
      return dictionary;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetUploadHandler(UploadHandler uh);

    public UploadHandler uploadHandler
    {
      get
      {
        return this.m_UploadHandler;
      }
      set
      {
        if (!this.isModifiable)
          throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the upload handler");
        UnityWebRequest.UnityWebRequestError err = this.SetUploadHandler(value);
        if (err != UnityWebRequest.UnityWebRequestError.OK)
          throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
        this.m_UploadHandler = value;
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetDownloadHandler(DownloadHandler dh);

    public DownloadHandler downloadHandler
    {
      get
      {
        return this.m_DownloadHandler;
      }
      set
      {
        if (!this.isModifiable)
          throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the download handler");
        UnityWebRequest.UnityWebRequestError err = this.SetDownloadHandler(value);
        if (err != UnityWebRequest.UnityWebRequestError.OK)
          throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
        this.m_DownloadHandler = value;
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int GetTimeoutMsec();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetTimeoutMsec(int timeout);

    public int timeout
    {
      get
      {
        return this.GetTimeoutMsec() / 1000;
      }
      set
      {
        if (!this.isModifiable)
          throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the timeout");
        value = Math.Max(value, 0);
        UnityWebRequest.UnityWebRequestError err = this.SetTimeoutMsec(value * 1000);
        if (err != UnityWebRequest.UnityWebRequestError.OK)
          throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
      }
    }

    [Obsolete("UnityWebRequest.isError has been renamed to isNetworkError for clarity. (UnityUpgradable) -> isNetworkError", false)]
    public bool isError
    {
      get
      {
        return this.isNetworkError;
      }
    }

    public static UnityWebRequest Get(string uri)
    {
      return new UnityWebRequest(uri, "GET", (DownloadHandler) new DownloadHandlerBuffer(), (UploadHandler) null);
    }

    public static UnityWebRequest Delete(string uri)
    {
      return new UnityWebRequest(uri, "DELETE");
    }

    public static UnityWebRequest Head(string uri)
    {
      return new UnityWebRequest(uri, "HEAD");
    }

    [Obsolete("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestTexture.GetTexture(*)", true)]
    public static UnityWebRequest GetTexture(string uri)
    {
      throw new NotSupportedException("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead.");
    }

    [Obsolete("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestTexture.GetTexture(*)", true)]
    public static UnityWebRequest GetTexture(string uri, bool nonReadable)
    {
      throw new NotSupportedException("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead.");
    }

    [Obsolete("UnityWebRequest.GetAudioClip is obsolete. Use UnityWebRequestMultimedia.GetAudioClip instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestMultimedia.GetAudioClip(*)", true)]
    public static UnityWebRequest GetAudioClip(string uri, AudioType audioType)
    {
      return (UnityWebRequest) null;
    }

    public static UnityWebRequest GetAssetBundle(string uri)
    {
      return UnityWebRequest.GetAssetBundle(uri, 0U);
    }

    public static UnityWebRequest GetAssetBundle(string uri, uint crc)
    {
      return new UnityWebRequest(uri, "GET", (DownloadHandler) new DownloadHandlerAssetBundle(uri, crc), (UploadHandler) null);
    }

    public static UnityWebRequest GetAssetBundle(string uri, uint version, uint crc)
    {
      return new UnityWebRequest(uri, "GET", (DownloadHandler) new DownloadHandlerAssetBundle(uri, version, crc), (UploadHandler) null);
    }

    public static UnityWebRequest GetAssetBundle(string uri, Hash128 hash, uint crc)
    {
      return new UnityWebRequest(uri, "GET", (DownloadHandler) new DownloadHandlerAssetBundle(uri, hash, crc), (UploadHandler) null);
    }

    public static UnityWebRequest GetAssetBundle(string uri, CachedAssetBundle cachedAssetBundle, uint crc)
    {
      return new UnityWebRequest(uri, "GET", (DownloadHandler) new DownloadHandlerAssetBundle(uri, cachedAssetBundle.name, cachedAssetBundle.hash, crc), (UploadHandler) null);
    }

    public static UnityWebRequest Put(string uri, byte[] bodyData)
    {
      return new UnityWebRequest(uri, "PUT", (DownloadHandler) new DownloadHandlerBuffer(), (UploadHandler) new UploadHandlerRaw(bodyData));
    }

    public static UnityWebRequest Put(string uri, string bodyData)
    {
      return new UnityWebRequest(uri, "PUT", (DownloadHandler) new DownloadHandlerBuffer(), (UploadHandler) new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyData)));
    }

    public static UnityWebRequest Post(string uri, string postData)
    {
      UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
      byte[] data = (byte[]) null;
      if (!string.IsNullOrEmpty(postData))
        data = Encoding.UTF8.GetBytes(WWWTranscoder.DataEncode(postData, Encoding.UTF8));
      unityWebRequest.uploadHandler = (UploadHandler) new UploadHandlerRaw(data);
      unityWebRequest.uploadHandler.contentType = "application/x-www-form-urlencoded";
      unityWebRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
      return unityWebRequest;
    }

    public static UnityWebRequest Post(string uri, WWWForm formData)
    {
      UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
      byte[] data = (byte[]) null;
      if (formData != null)
      {
        data = formData.data;
        if (data.Length == 0)
          data = (byte[]) null;
      }
      unityWebRequest.uploadHandler = (UploadHandler) new UploadHandlerRaw(data);
      unityWebRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
      if (formData != null)
      {
        foreach (KeyValuePair<string, string> header in formData.headers)
          unityWebRequest.SetRequestHeader(header.Key, header.Value);
      }
      return unityWebRequest;
    }

    public static UnityWebRequest Post(string uri, List<IMultipartFormSection> multipartFormSections)
    {
      byte[] boundary = UnityWebRequest.GenerateBoundary();
      return UnityWebRequest.Post(uri, multipartFormSections, boundary);
    }

    public static UnityWebRequest Post(string uri, List<IMultipartFormSection> multipartFormSections, byte[] boundary)
    {
      UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
      byte[] data = (byte[]) null;
      if (multipartFormSections != null && multipartFormSections.Count != 0)
        data = UnityWebRequest.SerializeFormSections(multipartFormSections, boundary);
      UploadHandler uploadHandler = (UploadHandler) new UploadHandlerRaw(data);
      uploadHandler.contentType = "multipart/form-data; boundary=" + Encoding.UTF8.GetString(boundary, 0, boundary.Length);
      unityWebRequest.uploadHandler = uploadHandler;
      unityWebRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
      return unityWebRequest;
    }

    public static UnityWebRequest Post(string uri, Dictionary<string, string> formFields)
    {
      UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
      byte[] data = (byte[]) null;
      if (formFields != null && formFields.Count != 0)
        data = UnityWebRequest.SerializeSimpleForm(formFields);
      UploadHandler uploadHandler = (UploadHandler) new UploadHandlerRaw(data);
      uploadHandler.contentType = "application/x-www-form-urlencoded";
      unityWebRequest.uploadHandler = uploadHandler;
      unityWebRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
      return unityWebRequest;
    }

    public static string EscapeURL(string s)
    {
      return UnityWebRequest.EscapeURL(s, Encoding.UTF8);
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
      return UnityWebRequest.UnEscapeURL(s, Encoding.UTF8);
    }

    public static string UnEscapeURL(string s, Encoding e)
    {
      if (s == null)
        return (string) null;
      if (s.IndexOf('%') == -1 && s.IndexOf('+') == -1)
        return s;
      return WWWTranscoder.URLDecode(s, e);
    }

    public static byte[] SerializeFormSections(List<IMultipartFormSection> multipartFormSections, byte[] boundary)
    {
      byte[] bytes1 = Encoding.UTF8.GetBytes("\r\n");
      byte[] bytes2 = WWWForm.DefaultEncoding.GetBytes("--");
      int capacity = 0;
      foreach (IMultipartFormSection multipartFormSection in multipartFormSections)
        capacity += 64 + multipartFormSection.sectionData.Length;
      List<byte> byteList = new List<byte>(capacity);
      foreach (IMultipartFormSection multipartFormSection in multipartFormSections)
      {
        string str1 = "form-data";
        string sectionName = multipartFormSection.sectionName;
        string fileName = multipartFormSection.fileName;
        if (!string.IsNullOrEmpty(fileName))
          str1 = "file";
        string str2 = "Content-Disposition: " + str1;
        if (!string.IsNullOrEmpty(sectionName))
          str2 = str2 + "; name=\"" + sectionName + "\"";
        if (!string.IsNullOrEmpty(fileName))
          str2 = str2 + "; filename=\"" + fileName + "\"";
        string s = str2 + "\r\n";
        string contentType = multipartFormSection.contentType;
        if (!string.IsNullOrEmpty(contentType))
          s = s + "Content-Type: " + contentType + "\r\n";
        byteList.AddRange((IEnumerable<byte>) bytes1);
        byteList.AddRange((IEnumerable<byte>) bytes2);
        byteList.AddRange((IEnumerable<byte>) boundary);
        byteList.AddRange((IEnumerable<byte>) bytes1);
        byteList.AddRange((IEnumerable<byte>) Encoding.UTF8.GetBytes(s));
        byteList.AddRange((IEnumerable<byte>) bytes1);
        byteList.AddRange((IEnumerable<byte>) multipartFormSection.sectionData);
      }
      byteList.TrimExcess();
      return byteList.ToArray();
    }

    public static byte[] GenerateBoundary()
    {
      byte[] numArray = new byte[40];
      for (int index = 0; index < 40; ++index)
      {
        int num = UnityEngine.Random.Range(48, 110);
        if (num > 57)
          num += 7;
        if (num > 90)
          num += 6;
        numArray[index] = (byte) num;
      }
      return numArray;
    }

    public static byte[] SerializeSimpleForm(Dictionary<string, string> formFields)
    {
      string s = "";
      foreach (KeyValuePair<string, string> formField in formFields)
      {
        if (s.Length > 0)
          s += "&";
        s = s + WWWTranscoder.DataEncode(formField.Key) + "=" + WWWTranscoder.DataEncode(formField.Value);
      }
      return Encoding.UTF8.GetBytes(s);
    }

    internal enum UnityWebRequestMethod
    {
      Get,
      Post,
      Put,
      Head,
      Custom,
    }

    internal enum UnityWebRequestError
    {
      OK,
      Unknown,
      SDKError,
      UnsupportedProtocol,
      MalformattedUrl,
      CannotResolveProxy,
      CannotResolveHost,
      CannotConnectToHost,
      AccessDenied,
      GenericHttpError,
      WriteError,
      ReadError,
      OutOfMemory,
      Timeout,
      HTTPPostError,
      SSLCannotConnect,
      Aborted,
      TooManyRedirects,
      ReceivedNoData,
      SSLNotSupported,
      FailedToSendData,
      FailedToReceiveData,
      SSLCertificateError,
      SSLCipherNotAvailable,
      SSLCACertError,
      UnrecognizedContentEncoding,
      LoginFailed,
      SSLShutdownFailed,
      NoInternetConnection,
    }
  }
}
