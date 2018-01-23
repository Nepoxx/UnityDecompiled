// Decompiled with JetBrains decompiler
// Type: UnityEditor.CrashReporting.CrashReporting
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using UnityEditor.Connect;
using UnityEditor.Utils;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.CrashReporting
{
  internal class CrashReporting
  {
    public static string ServiceBaseUrl
    {
      get
      {
        string configurationUrl = UnityConnect.instance.GetConfigurationURL(CloudConfigUrl.CloudPerfEvents);
        if (!string.IsNullOrEmpty(configurationUrl))
          return configurationUrl;
        return string.Empty;
      }
    }

    public static string NativeCrashSubmissionUrl
    {
      get
      {
        if (!string.IsNullOrEmpty(UnityEditor.CrashReporting.CrashReporting.ServiceBaseUrl))
          return new Uri(new Uri(UnityEditor.CrashReporting.CrashReporting.ServiceBaseUrl), "symbolicate").ToString();
        return string.Empty;
      }
    }

    public static string SignedUrlSourceUrl
    {
      get
      {
        if (!string.IsNullOrEmpty(UnityEditor.CrashReporting.CrashReporting.ServiceBaseUrl))
          return new Uri(new Uri(UnityEditor.CrashReporting.CrashReporting.ServiceBaseUrl), "url").ToString();
        return string.Empty;
      }
    }

    public static string ServiceTokenUrl
    {
      get
      {
        if (!string.IsNullOrEmpty(UnityEditor.CrashReporting.CrashReporting.ServiceBaseUrl) && !string.IsNullOrEmpty(PlayerSettings.cloudProjectId))
          return new Uri(new Uri(UnityEditor.CrashReporting.CrashReporting.ServiceBaseUrl), string.Format("token/{0}", (object) PlayerSettings.cloudProjectId)).ToString();
        return string.Empty;
      }
    }

    public static string GetUsymUploadAuthToken()
    {
      string str = string.Empty;
      RemoteCertificateValidationCallback validationCallback = ServicePointManager.ServerCertificateValidationCallback;
      try
      {
        string environmentVariable = Environment.GetEnvironmentVariable("USYM_UPLOAD_AUTH_TOKEN");
        if (!string.IsNullOrEmpty(environmentVariable))
          return environmentVariable;
        string accessToken = UnityConnect.instance.GetAccessToken();
        if (string.IsNullOrEmpty(accessToken))
          return string.Empty;
        string cloudProjectId = PlayerSettings.cloudProjectId;
        if (Application.platform != RuntimePlatform.OSXEditor)
          ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback) ((a, b, c, d) => true);
        WebRequest webRequest = WebRequest.Create(UnityEditor.CrashReporting.CrashReporting.ServiceTokenUrl);
        webRequest.Timeout = 15000;
        webRequest.Headers.Add("Authorization", string.Format("Bearer {0}", (object) accessToken));
        HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse;
        string jsondata = string.Empty;
        using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
          jsondata = streamReader.ReadToEnd();
        JSONValue jsonValue = JSONParser.SimpleParse(jsondata);
        if (jsonValue.ContainsKey("AuthToken"))
          str = jsonValue["AuthToken"].AsString();
      }
      catch (Exception ex)
      {
        UnityEngine.Debug.LogException(ex);
      }
      ServicePointManager.ServerCertificateValidationCallback = validationCallback;
      return str;
    }

    private static UnityEditor.CrashReporting.CrashReporting.UploadPlatformConfig GetUploadPlatformConfig()
    {
      UnityEditor.CrashReporting.CrashReporting.UploadPlatformConfig uploadPlatformConfig = new UnityEditor.CrashReporting.CrashReporting.UploadPlatformConfig();
      switch (Application.platform)
      {
        case RuntimePlatform.OSXEditor:
          uploadPlatformConfig.UsymtoolPath = Paths.Combine(EditorApplication.applicationContentsPath, "Tools", "macosx", "usymtool");
          uploadPlatformConfig.LzmaPath = Paths.Combine(EditorApplication.applicationContentsPath, "Tools", "lzma");
          uploadPlatformConfig.LogFilePath = Paths.Combine(Environment.GetEnvironmentVariable("HOME"), "Library", "Logs", "Unity", "symbol_upload.log");
          break;
        case RuntimePlatform.WindowsEditor:
          uploadPlatformConfig.UsymtoolPath = Paths.Combine(EditorApplication.applicationContentsPath, "Tools", "usymtool.exe");
          uploadPlatformConfig.LzmaPath = Paths.Combine(EditorApplication.applicationContentsPath, "Tools", "lzma.exe");
          uploadPlatformConfig.LogFilePath = Paths.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Unity", "Editor", "symbol_upload.log");
          break;
        case RuntimePlatform.LinuxEditor:
          uploadPlatformConfig.UsymtoolPath = Paths.Combine(EditorApplication.applicationContentsPath, "Tools", "usymtool");
          uploadPlatformConfig.LzmaPath = Paths.Combine(EditorApplication.applicationContentsPath, "Tools", "lzma-linux32");
          uploadPlatformConfig.LogFilePath = Paths.Combine(Environment.GetEnvironmentVariable("HOME"), ".config", "unity3d", "symbol_upload.log");
          break;
      }
      return uploadPlatformConfig;
    }

    public static void UploadSymbolsInPath(string authToken, string symbolPath, string includeFilter, string excludeFilter, bool waitForExit)
    {
      UnityEditor.CrashReporting.CrashReporting.UploadPlatformConfig uploadPlatformConfig = UnityEditor.CrashReporting.CrashReporting.GetUploadPlatformConfig();
      string str = string.Format("-symbolPath \"{0}\" -log \"{1}\" -filter \"{2}\" -excludeFilter \"{3}\"", (object) symbolPath, (object) uploadPlatformConfig.LogFilePath, (object) includeFilter, (object) excludeFilter);
      ProcessStartInfo processStartInfo = new ProcessStartInfo() { Arguments = str, CreateNoWindow = true, FileName = uploadPlatformConfig.UsymtoolPath, WorkingDirectory = Directory.GetParent(Application.dataPath).FullName, UseShellExecute = false };
      processStartInfo.EnvironmentVariables.Add("USYM_UPLOAD_AUTH_TOKEN", authToken);
      processStartInfo.EnvironmentVariables.Add("USYM_UPLOAD_URL_SOURCE", UnityEditor.CrashReporting.CrashReporting.SignedUrlSourceUrl);
      processStartInfo.EnvironmentVariables.Add("LZMA_PATH", uploadPlatformConfig.LzmaPath);
      Process process = new Process();
      process.StartInfo = processStartInfo;
      process.Start();
      if (!waitForExit)
        return;
      process.WaitForExit();
    }

    private class UploadPlatformConfig
    {
      public string UsymtoolPath;
      public string LzmaPath;
      public string LogFilePath;
    }
  }
}
