// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.RestRequestException
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor.RestService
{
  internal class RestRequestException : Exception
  {
    public RestRequestException()
    {
    }

    public RestRequestException(HttpStatusCode httpStatusCode, string restErrorString)
      : this(httpStatusCode, restErrorString, (string) null)
    {
    }

    public RestRequestException(HttpStatusCode httpStatusCode, string restErrorString, string restErrorDescription)
    {
      this.HttpStatusCode = httpStatusCode;
      this.RestErrorString = restErrorString;
      this.RestErrorDescription = restErrorDescription;
    }

    public string RestErrorString { get; set; }

    public HttpStatusCode HttpStatusCode { get; set; }

    public string RestErrorDescription { get; set; }
  }
}
