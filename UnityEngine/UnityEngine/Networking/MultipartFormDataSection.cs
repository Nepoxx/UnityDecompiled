// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.MultipartFormDataSection
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Text;

namespace UnityEngine.Networking
{
  public class MultipartFormDataSection : IMultipartFormSection
  {
    private string name;
    private byte[] data;
    private string content;

    public MultipartFormDataSection(string name, byte[] data, string contentType)
    {
      if (data == null || data.Length < 1)
        throw new ArgumentException("Cannot create a multipart form data section without body data");
      this.name = name;
      this.data = data;
      this.content = contentType;
    }

    public MultipartFormDataSection(string name, byte[] data)
      : this(name, data, (string) null)
    {
    }

    public MultipartFormDataSection(byte[] data)
      : this((string) null, data)
    {
    }

    public MultipartFormDataSection(string name, string data, Encoding encoding, string contentType)
    {
      if (data == null || data.Length < 1)
        throw new ArgumentException("Cannot create a multipart form data section without body data");
      byte[] bytes = encoding.GetBytes(data);
      this.name = name;
      this.data = bytes;
      if (contentType != null && !contentType.Contains("encoding="))
        contentType = contentType.Trim() + "; encoding=" + encoding.WebName;
      this.content = contentType;
    }

    public MultipartFormDataSection(string name, string data, string contentType)
      : this(name, data, Encoding.UTF8, contentType)
    {
    }

    public MultipartFormDataSection(string name, string data)
      : this(name, data, "text/plain")
    {
    }

    public MultipartFormDataSection(string data)
      : this((string) null, data)
    {
    }

    public string sectionName
    {
      get
      {
        return this.name;
      }
    }

    public byte[] sectionData
    {
      get
      {
        return this.data;
      }
    }

    public string fileName
    {
      get
      {
        return (string) null;
      }
    }

    public string contentType
    {
      get
      {
        return this.content;
      }
    }
  }
}
