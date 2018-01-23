// Decompiled with JetBrains decompiler
// Type: UnityEditor.LocalCacheServer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEditor.Utils;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal class LocalCacheServer : ScriptableSingleton<LocalCacheServer>
  {
    [SerializeField]
    public int pid = -1;
    [SerializeField]
    public string path;
    [SerializeField]
    public int port;
    [SerializeField]
    public ulong size;
    [SerializeField]
    public string time;
    public const string SizeKey = "LocalCacheServerSize";
    public const string PathKey = "LocalCacheServerPath";
    public const string CustomPathKey = "LocalCacheServerCustomPath";

    public static string GetCacheLocation()
    {
      string str1 = EditorPrefs.GetString("LocalCacheServerPath");
      bool flag = EditorPrefs.GetBool("LocalCacheServerCustomPath");
      string str2 = str1;
      if (!flag || string.IsNullOrEmpty(str1))
        str2 = Paths.Combine(OSUtil.GetDefaultCachePath(), "CacheServer");
      return str2;
    }

    public static void CreateCacheDirectory()
    {
      string cacheLocation = LocalCacheServer.GetCacheLocation();
      if (Directory.Exists(cacheLocation))
        return;
      Directory.CreateDirectory(cacheLocation);
    }

    private void Create(int _port, ulong _size)
    {
      string str1 = Paths.Combine(EditorApplication.applicationContentsPath, "Tools", "nodejs");
      string fileName;
      if (Application.platform == RuntimePlatform.WindowsEditor)
        fileName = Paths.Combine(str1, "node.exe");
      else
        fileName = Paths.Combine(str1, "bin", "node");
      LocalCacheServer.CreateCacheDirectory();
      this.path = LocalCacheServer.GetCacheLocation();
      string str2 = Paths.Combine(EditorApplication.applicationContentsPath, "Tools", "CacheServer", "main.js");
      ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName) { Arguments = "\"" + str2 + "\" --port " + (object) _port + " --path \"" + this.path + "\" --nolegacy --monitor-parent-process " + (object) Process.GetCurrentProcess().Id + " --silent --size " + (object) _size, UseShellExecute = false, CreateNoWindow = true };
      Process process = new Process();
      process.StartInfo = processStartInfo;
      process.Start();
      this.port = _port;
      this.pid = process.Id;
      this.size = _size;
      this.time = process.StartTime.ToString();
      this.Save(true);
    }

    public static int GetRandomUnusedPort()
    {
      TcpListener tcpListener = new TcpListener(IPAddress.Any, 0);
      tcpListener.Start();
      int port = ((IPEndPoint) tcpListener.LocalEndpoint).Port;
      tcpListener.Stop();
      return port;
    }

    public static bool PingHost(string host, int port, int timeout)
    {
      try
      {
        using (TcpClient tcpClient = new TcpClient())
        {
          tcpClient.BeginConnect(host, port, (AsyncCallback) null, (object) null).AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds((double) timeout));
          return tcpClient.Connected;
        }
      }
      catch
      {
        return false;
      }
    }

    public static bool WaitForServerToComeAlive(int port)
    {
      DateTime now = DateTime.Now;
      DateTime dateTime = now.AddSeconds(5.0);
      while (DateTime.Now < dateTime)
      {
        if (LocalCacheServer.PingHost("localhost", port, 10))
        {
          Console.WriteLine("Server Came alive after {0} ms", (object) (DateTime.Now - now).TotalMilliseconds);
          return true;
        }
      }
      return false;
    }

    public static void Kill()
    {
      if (ScriptableSingleton<LocalCacheServer>.instance.pid == -1)
        return;
      try
      {
        Process.GetProcessById(ScriptableSingleton<LocalCacheServer>.instance.pid).Kill();
        ScriptableSingleton<LocalCacheServer>.instance.pid = -1;
      }
      catch
      {
      }
    }

    public static void CreateIfNeeded()
    {
      Process process = (Process) null;
      try
      {
        process = Process.GetProcessById(ScriptableSingleton<LocalCacheServer>.instance.pid);
      }
      catch
      {
      }
      ulong _size = (ulong) ((long) EditorPrefs.GetInt("LocalCacheServerSize", 10) * 1024L * 1024L) * 1024UL;
      if (process != null && process.StartTime.ToString() == ScriptableSingleton<LocalCacheServer>.instance.time)
      {
        if ((long) ScriptableSingleton<LocalCacheServer>.instance.size == (long) _size && ScriptableSingleton<LocalCacheServer>.instance.path == LocalCacheServer.GetCacheLocation())
        {
          LocalCacheServer.CreateCacheDirectory();
          return;
        }
        LocalCacheServer.Kill();
      }
      ScriptableSingleton<LocalCacheServer>.instance.Create(LocalCacheServer.GetRandomUnusedPort(), _size);
      LocalCacheServer.WaitForServerToComeAlive(ScriptableSingleton<LocalCacheServer>.instance.port);
    }

    public static void Setup()
    {
      if (EditorPrefs.GetInt("CacheServerMode") == 0)
        LocalCacheServer.CreateIfNeeded();
      else
        LocalCacheServer.Kill();
    }

    [UsedByNativeCode]
    public static int GetLocalCacheServerPort()
    {
      LocalCacheServer.Setup();
      return ScriptableSingleton<LocalCacheServer>.instance.port;
    }

    public static void Clear()
    {
      LocalCacheServer.Kill();
      string cacheLocation = LocalCacheServer.GetCacheLocation();
      if (!Directory.Exists(cacheLocation))
        return;
      Directory.Delete(cacheLocation, true);
    }

    public static bool CheckCacheLocationExists()
    {
      return Directory.Exists(LocalCacheServer.GetCacheLocation());
    }

    public static bool CheckValidCacheLocation(string path)
    {
      if (Directory.Exists(path))
      {
        foreach (string fileSystemEntry in Directory.GetFileSystemEntries(path))
        {
          string lower = Path.GetFileName(fileSystemEntry).ToLower();
          if (lower.Length != 2 && !(lower == "temp") && (!(lower == ".ds_store") && !(lower == "desktop.ini")))
            return false;
        }
      }
      return true;
    }
  }
}
