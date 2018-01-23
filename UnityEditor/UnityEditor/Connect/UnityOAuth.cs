// Decompiled with JetBrains decompiler
// Type: UnityEditor.Connect.UnityOAuth
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.InteropServices;
using UnityEditorInternal;

namespace UnityEditor.Connect
{
  public static class UnityOAuth
  {
    public static event Action UserLoggedIn;

    public static event Action UserLoggedOut;

    public static void GetAuthorizationCodeAsync(string clientId, Action<UnityOAuth.AuthCodeResponse> callback)
    {
      if (string.IsNullOrEmpty(clientId))
        throw new ArgumentException("clientId is null or empty.", nameof (clientId));
      if (callback == null)
        throw new ArgumentNullException(nameof (callback));
      if (string.IsNullOrEmpty(UnityConnect.instance.GetAccessToken()))
        throw new InvalidOperationException("User is not logged in or user status invalid.");
      new AsyncHTTPClient(string.Format("{0}/v1/oauth2/authorize", (object) UnityConnect.instance.GetConfigurationURL(CloudConfigUrl.CloudIdentity)))
      {
        postData = string.Format("client_id={0}&response_type=code&format=json&access_token={1}&prompt=none", (object) clientId, (object) UnityConnect.instance.GetAccessToken()),
        doneCallback = ((AsyncHTTPClient.DoneCallback) (c =>
        {
          UnityOAuth.AuthCodeResponse authCodeResponse = new UnityOAuth.AuthCodeResponse();
          if (!c.IsSuccess())
          {
            authCodeResponse.Exception = (Exception) new InvalidOperationException("Failed to call Unity ID to get auth code.");
          }
          else
          {
            try
            {
              JSONValue jsonValue = new JSONParser(c.text).Parse();
              if (jsonValue.ContainsKey("code") && !jsonValue["code"].IsNull())
                authCodeResponse.AuthCode = jsonValue["code"].AsString();
              else if (jsonValue.ContainsKey("message"))
                authCodeResponse.Exception = (Exception) new InvalidOperationException(string.Format("Error from server: {0}", (object) jsonValue["message"].AsString()));
              else if (jsonValue.ContainsKey("location") && !jsonValue["location"].IsNull())
              {
                UnityConnectConsentView connectConsentView = UnityConnectConsentView.ShowUnityConnectConsentView(jsonValue["location"].AsString());
                if (!string.IsNullOrEmpty(connectConsentView.Code))
                  authCodeResponse.AuthCode = connectConsentView.Code;
                else
                  authCodeResponse.Exception = string.IsNullOrEmpty(connectConsentView.Error) ? (Exception) new InvalidOperationException("Consent Windows was closed unexpected.") : (Exception) new InvalidOperationException(string.Format("Error from server: {0}", (object) connectConsentView.Error));
              }
              else
                authCodeResponse.Exception = (Exception) new InvalidOperationException("Unexpected response from server.");
            }
            catch (JSONParseException ex)
            {
              authCodeResponse.Exception = (Exception) new InvalidOperationException("Unexpected response from server: Failed to parse JSON.");
            }
          }
          callback(authCodeResponse);
        }))
      }.Begin();
    }

    private static void OnUserLoggedIn()
    {
      // ISSUE: reference to a compiler-generated field
      if (UnityOAuth.UserLoggedIn == null)
        return;
      // ISSUE: reference to a compiler-generated field
      UnityOAuth.UserLoggedIn();
    }

    private static void OnUserLoggedOut()
    {
      // ISSUE: reference to a compiler-generated field
      if (UnityOAuth.UserLoggedOut == null)
        return;
      // ISSUE: reference to a compiler-generated field
      UnityOAuth.UserLoggedOut();
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct AuthCodeResponse
    {
      public string AuthCode { get; set; }

      public Exception Exception { get; set; }
    }
  }
}
