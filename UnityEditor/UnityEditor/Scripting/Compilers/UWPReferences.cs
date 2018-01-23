// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.UWPReferences
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEditor.Utils;
using UnityEditorInternal;

namespace UnityEditor.Scripting.Compilers
{
  internal static class UWPReferences
  {
    public static string[] GetReferences(Version sdkVersion)
    {
      string windowsKit10 = UWPReferences.GetWindowsKit10();
      if (string.IsNullOrEmpty(windowsKit10))
        return new string[0];
      string version = UWPReferences.SdkVersionToString(sdkVersion);
      HashSet<string> source = new HashSet<string>((IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
      string path = UWPReferences.CombinePaths(windowsKit10, "UnionMetadata", version, "Facade", "Windows.winmd");
      if (!File.Exists(path))
        path = UWPReferences.CombinePaths(windowsKit10, "UnionMetadata", "Facade", "Windows.winmd");
      source.Add(path);
      foreach (string str in UWPReferences.GetPlatform(windowsKit10, version))
        source.Add(str);
      foreach (UWPReferences.UWPExtension extension in UWPReferences.GetExtensions(windowsKit10, version))
      {
        foreach (string reference in extension.References)
          source.Add(reference);
      }
      return source.ToArray<string>();
    }

    public static IEnumerable<UWPExtensionSDK> GetExtensionSDKs(Version sdkVersion)
    {
      string windowsKit10 = UWPReferences.GetWindowsKit10();
      if (string.IsNullOrEmpty(windowsKit10))
        return (IEnumerable<UWPExtensionSDK>) new UWPExtensionSDK[0];
      return UWPReferences.GetExtensionSDKs(windowsKit10, UWPReferences.SdkVersionToString(sdkVersion));
    }

    private static string SdkVersionToString(Version version)
    {
      string str = version.ToString();
      if (version.Minor == -1)
        str += ".0";
      if (version.Build == -1)
        str += ".0";
      if (version.Revision == -1)
        str += ".0";
      return str;
    }

    public static IEnumerable<UWPSDK> GetInstalledSDKs()
    {
      string windowsKit10 = UWPReferences.GetWindowsKit10();
      if (string.IsNullOrEmpty(windowsKit10))
        return Enumerable.Empty<UWPSDK>();
      string path = UWPReferences.CombinePaths(windowsKit10, "Platforms", "UAP");
      if (!Directory.Exists(path))
        return Enumerable.Empty<UWPSDK>();
      List<UWPSDK> uwpsdkList = new List<UWPSDK>();
      foreach (string uri in ((IEnumerable<string>) Directory.GetFiles(path, "*", SearchOption.AllDirectories)).Where<string>((Func<string, bool>) (f => string.Equals("Platform.xml", Path.GetFileName(f), StringComparison.OrdinalIgnoreCase))))
      {
        XDocument xdocument;
        try
        {
          xdocument = XDocument.Load(uri);
        }
        catch
        {
          continue;
        }
        foreach (XElement element in xdocument.Elements((XName) "ApplicationPlatform"))
        {
          Version version;
          if (UWPReferences.FindVersionInNode(element, out version))
          {
            string s = element.Elements((XName) "MinimumVisualStudioVersion").Select<XElement, string>((Func<XElement, string>) (e => e.Value)).FirstOrDefault<string>();
            uwpsdkList.Add(new UWPSDK(version, UWPReferences.TryParseVersion(s)));
          }
        }
      }
      return (IEnumerable<UWPSDK>) uwpsdkList;
    }

    private static Version TryParseVersion(string s)
    {
      if (!string.IsNullOrEmpty(s))
      {
        try
        {
          return new Version(s);
        }
        catch
        {
        }
      }
      return (Version) null;
    }

    private static bool FindVersionInNode(XElement node, out Version version)
    {
      for (XAttribute xattribute = node.FirstAttribute; xattribute != null; xattribute = xattribute.NextAttribute)
      {
        if (string.Equals(xattribute.Name.LocalName, nameof (version), StringComparison.OrdinalIgnoreCase))
        {
          version = UWPReferences.TryParseVersion(xattribute.Value);
          if (version != (Version) null)
            return true;
        }
      }
      version = (Version) null;
      return false;
    }

    private static string[] GetPlatform(string folder, string version)
    {
      string str = UWPReferences.CombinePaths(folder, "Platforms\\UAP", version, "Platform.xml");
      if (!File.Exists(str))
        return new string[0];
      XElement xelement = XDocument.Load(str).Element((XName) "ApplicationPlatform");
      if (xelement.Attribute((XName) "name").Value != "UAP")
        throw new Exception(string.Format("Invalid platform manifest at \"{0}\".", (object) str));
      XElement containedApiContractsElement = xelement.Element((XName) "ContainedApiContracts");
      return UWPReferences.GetReferences(folder, version, containedApiContractsElement);
    }

    private static string CombinePaths(params string[] paths)
    {
      return Paths.Combine(paths);
    }

    private static IEnumerable<UWPExtensionSDK> GetExtensionSDKs(string sdkFolder, string sdkVersion)
    {
      List<UWPExtensionSDK> uwpExtensionSdkList = new List<UWPExtensionSDK>();
      string path = Path.Combine(sdkFolder, "Extension SDKs");
      if (!Directory.Exists(path))
        return (IEnumerable<UWPExtensionSDK>) new UWPExtensionSDK[0];
      foreach (string directory in Directory.GetDirectories(path))
      {
        string str1 = UWPReferences.CombinePaths(directory, sdkVersion, "SDKManifest.xml");
        string fileName = Path.GetFileName(directory);
        if (File.Exists(str1))
          uwpExtensionSdkList.Add(new UWPExtensionSDK(fileName, sdkVersion, str1));
        else if (fileName == "XboxLive")
        {
          string str2 = UWPReferences.CombinePaths(directory, "1.0", "SDKManifest.xml");
          if (File.Exists(str2))
            uwpExtensionSdkList.Add(new UWPExtensionSDK(fileName, "1.0", str2));
        }
      }
      return (IEnumerable<UWPExtensionSDK>) uwpExtensionSdkList;
    }

    private static UWPReferences.UWPExtension[] GetExtensions(string windowsKitsFolder, string version)
    {
      List<UWPReferences.UWPExtension> uwpExtensionList = new List<UWPReferences.UWPExtension>();
      foreach (UWPExtensionSDK extensionSdK in UWPReferences.GetExtensionSDKs(windowsKitsFolder, version))
      {
        try
        {
          UWPReferences.UWPExtension uwpExtension = new UWPReferences.UWPExtension(extensionSdK.ManifestPath, windowsKitsFolder, version);
          uwpExtensionList.Add(uwpExtension);
        }
        catch
        {
        }
      }
      return uwpExtensionList.ToArray();
    }

    private static string[] GetReferences(string windowsKitsFolder, string sdkVersion, XElement containedApiContractsElement)
    {
      List<string> stringList = new List<string>();
      foreach (XElement element in containedApiContractsElement.Elements((XName) "ApiContract"))
      {
        string str1 = element.Attribute((XName) "name").Value;
        string str2 = element.Attribute((XName) "version").Value;
        string path = UWPReferences.CombinePaths(windowsKitsFolder, "References", sdkVersion, str1, str2, str1 + ".winmd");
        if (!File.Exists(path))
        {
          path = UWPReferences.CombinePaths(windowsKitsFolder, "References", str1, str2, str1 + ".winmd");
          if (!File.Exists(path))
            continue;
        }
        stringList.Add(path);
      }
      return stringList.ToArray();
    }

    private static string GetWindowsKit10()
    {
      string str = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "Windows Kits\\10\\");
      try
      {
        str = RegistryUtil.GetRegistryStringValue("SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows\\v10.0", "InstallationFolder", str, RegistryView._32);
      }
      catch
      {
      }
      if (!Directory.Exists(str))
        return string.Empty;
      return str;
    }

    private sealed class UWPExtension
    {
      public UWPExtension(string manifest, string windowsKitsFolder, string sdkVersion)
      {
        XElement xelement = XDocument.Load(manifest).Element((XName) "FileList");
        if (xelement.Attribute((XName) "TargetPlatform").Value != "UAP")
          throw new Exception(string.Format("Invalid extension manifest at \"{0}\".", (object) manifest));
        this.Name = xelement.Attribute((XName) "DisplayName").Value;
        XElement containedApiContractsElement = xelement.Element((XName) "ContainedApiContracts");
        this.References = UWPReferences.GetReferences(windowsKitsFolder, sdkVersion, containedApiContractsElement);
      }

      public string Name { get; private set; }

      public string[] References { get; private set; }
    }
  }
}
