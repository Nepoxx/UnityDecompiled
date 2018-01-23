// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.DownloadHandler
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
  [StructLayout(LayoutKind.Sequential)]
  public class DownloadHandler : IDisposable
  {
    [NonSerialized]
    internal IntPtr m_Ptr;

    internal DownloadHandler()
    {
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Release();

    ~DownloadHandler()
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

    public bool isDone
    {
      get
      {
        return this.IsDone();
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool IsDone();

    public byte[] data
    {
      get
      {
        return this.GetData();
      }
    }

    public string text
    {
      get
      {
        return this.GetText();
      }
    }

    protected virtual byte[] GetData()
    {
      return (byte[]) null;
    }

    protected virtual string GetText()
    {
      byte[] data = this.GetData();
      if (data != null && data.Length > 0)
        return this.GetTextEncoder().GetString(data, 0, data.Length);
      return "";
    }

    private Encoding GetTextEncoder()
    {
      string contentType = this.GetContentType();
      if (!string.IsNullOrEmpty(contentType))
      {
        int startIndex = contentType.IndexOf("charset", StringComparison.OrdinalIgnoreCase);
        if (startIndex > -1)
        {
          int num = contentType.IndexOf('=', startIndex);
          if (num > -1)
          {
            string name = contentType.Substring(num + 1).Trim().Trim('\'', '"').Trim();
            int length = name.IndexOf(';');
            if (length > -1)
              name = name.Substring(0, length);
            try
            {
              return Encoding.GetEncoding(name);
            }
            catch (ArgumentException ex)
            {
              Debug.LogWarning((object) string.Format("Unsupported encoding '{0}': {1}", (object) name, (object) ex.Message));
            }
          }
        }
      }
      return Encoding.UTF8;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern string GetContentType();

    [UsedByNativeCode]
    protected virtual bool ReceiveData(byte[] data, int dataLength)
    {
      return true;
    }

    [UsedByNativeCode]
    protected virtual void ReceiveContentLength(int contentLength)
    {
    }

    [UsedByNativeCode]
    protected virtual void CompleteContent()
    {
    }

    [UsedByNativeCode]
    protected virtual float GetProgress()
    {
      return 0.0f;
    }

    protected static T GetCheckedDownloader<T>(UnityWebRequest www) where T : DownloadHandler
    {
      if (www == null)
        throw new NullReferenceException("Cannot get content from a null UnityWebRequest object");
      if (!www.isDone)
        throw new InvalidOperationException("Cannot get content from an unfinished UnityWebRequest object");
      if (www.isNetworkError)
        throw new InvalidOperationException(www.error);
      return (T) www.downloadHandler;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern byte[] InternalGetByteArray(DownloadHandler dh);
  }
}
