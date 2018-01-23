// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.MultipartFormFileSection
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Text;

namespace UnityEngine.Networking
{
  public class MultipartFormFileSection : IMultipartFormSection
  {
    private string name;
    private byte[] data;
    private string file;
    private string content;

    public MultipartFormFileSection(string name, byte[] data, string fileName, string contentType)
    {
      if (data == null || data.Length < 1)
        throw new ArgumentException("Cannot create a multipart form file section without body data");
      if (string.IsNullOrEmpty(fileName))
        fileName = "file.dat";
      if (string.IsNullOrEmpty(contentType))
        contentType = "application/octet-stream";
      this.Init(name, data, fileName, contentType);
    }

    public MultipartFormFileSection(byte[] data)
      : this((string) null, data, (string) null, (string) null)
    {
    }

    public MultipartFormFileSection(string fileName, byte[] data)
      : this((string) null, data, fileName, (string) null)
    {
    }

    public MultipartFormFileSection(string name, string data, Encoding dataEncoding, string fileName)
    {
      if (data == null || data.Length < 1)
        throw new ArgumentException("Cannot create a multipart form file section without body data");
      if (dataEncoding == null)
        dataEncoding = Encoding.UTF8;
      byte[] bytes = dataEncoding.GetBytes(data);
      if (string.IsNullOrEmpty(fileName))
        fileName = "file.txt";
      if (string.IsNullOrEmpty(this.content))
        this.content = "text/plain; charset=" + dataEncoding.WebName;
      this.Init(name, bytes, fileName, this.content);
    }

    public MultipartFormFileSection(string data, Encoding dataEncoding, string fileName)
      : this((string) null, data, dataEncoding, fileName)
    {
    }

    public MultipartFormFileSection(string data, string fileName)
      : this(data, (Encoding) null, fileName)
    {
    }

    private void Init(string name, byte[] data, string fileName, string contentType)
    {
      this.name = name;
      this.data = data;
      this.file = fileName;
      this.content = contentType;
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
        return this.file;
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
