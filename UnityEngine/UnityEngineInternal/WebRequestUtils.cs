// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.WebRequestUtils
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEngineInternal
{
  internal static class WebRequestUtils
  {
    private static Regex domainRegex = new Regex("^\\s*\\w+(?:\\.\\w+)+(\\/.*)?$");

    [RequiredByNativeCode]
    internal static string RedirectTo(string baseUri, string redirectUri)
    {
      Uri relativeUri = (int) redirectUri[0] != 47 ? new Uri(redirectUri, UriKind.RelativeOrAbsolute) : new Uri(redirectUri, UriKind.Relative);
      if (relativeUri.IsAbsoluteUri)
        return relativeUri.AbsoluteUri;
      return new Uri(new Uri(baseUri, UriKind.Absolute), relativeUri).AbsoluteUri;
    }

    internal static string MakeInitialUrl(string targetUrl, string localUrl)
    {
      if (targetUrl.StartsWith("jar:file://") || targetUrl.StartsWith("blob:http"))
        return targetUrl;
      Uri baseUri = new Uri(localUrl);
      Uri uri = (Uri) null;
      if ((int) targetUrl[0] == 47)
        uri = new Uri(baseUri, targetUrl);
      if (uri == (Uri) null && WebRequestUtils.domainRegex.IsMatch(targetUrl))
        targetUrl = baseUri.Scheme + "://" + targetUrl;
      FormatException formatException = (FormatException) null;
      try
      {
        if (uri == (Uri) null)
        {
          if ((int) targetUrl[0] != 46)
            uri = new Uri(targetUrl);
        }
      }
      catch (FormatException ex)
      {
        formatException = ex;
      }
      if (uri == (Uri) null)
      {
        try
        {
          uri = new Uri(baseUri, targetUrl);
        }
        catch (FormatException ex)
        {
          throw formatException;
        }
      }
      if (targetUrl.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
        return !targetUrl.Contains("%") ? targetUrl : WWWTranscoder.URLDecode(targetUrl, Encoding.UTF8);
      return !targetUrl.Contains("%") ? uri.AbsoluteUri : uri.OriginalString;
    }
  }
}
