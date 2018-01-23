// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.RestRequest
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.IO;
using System.Net;
using System.Text;

namespace UnityEditor.RestService
{
  internal class RestRequest
  {
    public static bool Send(string endpoint, string payload, int timeout)
    {
      if (ScriptEditorSettings.ServerURL == null)
        return false;
      byte[] bytes = Encoding.UTF8.GetBytes(payload);
      WebRequest webRequest1 = WebRequest.Create(ScriptEditorSettings.ServerURL + endpoint);
      webRequest1.Timeout = timeout;
      webRequest1.Method = "POST";
      webRequest1.ContentType = "application/json";
      webRequest1.ContentLength = (long) bytes.Length;
      try
      {
        Stream requestStream = webRequest1.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
      }
      catch (Exception ex)
      {
        Logger.Log(ex);
        return false;
      }
      try
      {
        WebRequest webRequest2 = webRequest1;
        // ISSUE: reference to a compiler-generated field
        if (RestRequest.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          RestRequest.\u003C\u003Ef__mg\u0024cache0 = new AsyncCallback(RestRequest.GetResponseCallback);
        }
        // ISSUE: reference to a compiler-generated field
        AsyncCallback fMgCache0 = RestRequest.\u003C\u003Ef__mg\u0024cache0;
        WebRequest webRequest3 = webRequest1;
        webRequest2.BeginGetResponse(fMgCache0, (object) webRequest3);
      }
      catch (Exception ex)
      {
        Logger.Log(ex);
        return false;
      }
      return true;
    }

    private static void GetResponseCallback(IAsyncResult asynchronousResult)
    {
      WebResponse response = ((WebRequest) asynchronousResult.AsyncState).EndGetResponse(asynchronousResult);
      try
      {
        Stream responseStream = response.GetResponseStream();
        StreamReader streamReader = new StreamReader(responseStream);
        streamReader.ReadToEnd();
        streamReader.Close();
        responseStream.Close();
      }
      finally
      {
        response.Close();
      }
    }
  }
}
