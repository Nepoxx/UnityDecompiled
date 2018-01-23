// Decompiled with JetBrains decompiler
// Type: UnityEditor.VisualStudioIntegration.UnityVSSupport
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.Utils;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.VisualStudioIntegration
{
  internal class UnityVSSupport
  {
    private static bool m_ShouldUnityVSBeActive;
    public static string s_UnityVSBridgeToLoad;
    private static bool? s_IsUnityVSEnabled;
    private static string s_AboutLabel;

    [RequiredByNativeCode]
    public static void InitializeUnityVSSupport()
    {
      UnityVSSupport.Initialize((string) null);
    }

    public static void Initialize()
    {
      UnityVSSupport.Initialize((string) null);
    }

    public static void Initialize(string editorPath)
    {
      string externalEditor = editorPath ?? ScriptEditorUtility.GetExternalScriptEditor();
      if (Application.platform == RuntimePlatform.OSXEditor)
      {
        UnityVSSupport.InitializeVSForMac(externalEditor);
      }
      else
      {
        if (Application.platform != RuntimePlatform.WindowsEditor)
          return;
        UnityVSSupport.InitializeVisualStudio(externalEditor);
      }
    }

    private static void InitializeVSForMac(string externalEditor)
    {
      Version vsfmVersion;
      if (!UnityVSSupport.IsVSForMac(externalEditor, out vsfmVersion))
        return;
      UnityVSSupport.m_ShouldUnityVSBeActive = true;
      string macBridgeAssembly = UnityVSSupport.GetVSForMacBridgeAssembly(externalEditor, vsfmVersion);
      if (string.IsNullOrEmpty(macBridgeAssembly) || !File.Exists(macBridgeAssembly))
      {
        Console.WriteLine("Unable to find Tools for Unity bridge dll for Visual Studio for Mac " + externalEditor);
      }
      else
      {
        UnityVSSupport.s_UnityVSBridgeToLoad = macBridgeAssembly;
        InternalEditorUtility.RegisterPrecompiledAssembly(Path.GetFileNameWithoutExtension(macBridgeAssembly), macBridgeAssembly);
      }
    }

    private static bool IsVSForMac(string externalEditor, out Version vsfmVersion)
    {
      vsfmVersion = (Version) null;
      if (!externalEditor.ToLower().EndsWith("visual studio.app"))
        return false;
      try
      {
        return UnityVSSupport.GetVSForMacVersion(externalEditor, out vsfmVersion);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Failed to read Visual Studio for Mac information: {0}", (object) ex);
        return false;
      }
    }

    private static bool GetVSForMacVersion(string externalEditor, out Version vsfmVersion)
    {
      vsfmVersion = (Version) null;
      string path = Path.Combine(externalEditor, "Contents/Info.plist");
      if (!File.Exists(path))
        return false;
      System.Text.RegularExpressions.Group group = Regex.Match(File.ReadAllText(path), "\\<key\\>CFBundleShortVersionString\\</key\\>\\s+\\<string\\>(?<version>\\d+\\.\\d+\\.\\d+\\.\\d+?)\\</string\\>").Groups["version"];
      if (!group.Success)
        return false;
      vsfmVersion = new Version(group.Value);
      return true;
    }

    private static string GetVSForMacBridgeAssembly(string externalEditor, Version vsfmVersion)
    {
      string environmentVariable = Environment.GetEnvironmentVariable("VSTUM_BRIDGE");
      if (!string.IsNullOrEmpty(environmentVariable) && File.Exists(environmentVariable))
        return environmentVariable;
      string path1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Application Support/VisualStudio/" + (object) vsfmVersion.Major + ".0/LocalInstall/Addins");
      if (Directory.Exists(path1))
      {
        foreach (string directory in Directory.GetDirectories(path1, "MonoDevelop.Unity*", SearchOption.TopDirectoryOnly))
        {
          string path2 = Path.Combine(directory, "Editor/SyntaxTree.VisualStudio.Unity.Bridge.dll");
          if (File.Exists(path2))
            return path2;
        }
      }
      string path3 = Path.Combine(externalEditor, "Contents/Resources/lib/monodevelop/AddIns/MonoDevelop.Unity/Editor/SyntaxTree.VisualStudio.Unity.Bridge.dll");
      if (File.Exists(path3))
        return path3;
      return (string) null;
    }

    private static void InitializeVisualStudio(string externalEditor)
    {
      if (externalEditor.EndsWith("UnityVS.OpenFile.exe"))
      {
        externalEditor = SyncVS.FindBestVisualStudio();
        if (externalEditor != null)
          ScriptEditorUtility.SetExternalScriptEditor(externalEditor);
      }
      VisualStudioVersion vsVersion;
      if (!UnityVSSupport.IsVisualStudio(externalEditor, out vsVersion))
        return;
      UnityVSSupport.m_ShouldUnityVSBeActive = true;
      string vstuBridgeAssembly = UnityVSSupport.GetVstuBridgeAssembly(vsVersion);
      if (vstuBridgeAssembly == null)
        Console.WriteLine("Unable to find bridge dll in registry for Microsoft Visual Studio Tools for Unity for " + externalEditor);
      else if (!File.Exists(vstuBridgeAssembly))
      {
        Console.WriteLine("Unable to find bridge dll on disk for Microsoft Visual Studio Tools for Unity for " + vstuBridgeAssembly);
      }
      else
      {
        UnityVSSupport.s_UnityVSBridgeToLoad = vstuBridgeAssembly;
        InternalEditorUtility.RegisterPrecompiledAssembly(Path.GetFileNameWithoutExtension(vstuBridgeAssembly), vstuBridgeAssembly);
      }
    }

    private static bool IsVisualStudio(string externalEditor, out VisualStudioVersion vsVersion)
    {
      if (string.IsNullOrEmpty(externalEditor))
      {
        vsVersion = VisualStudioVersion.Invalid;
        return false;
      }
      KeyValuePair<VisualStudioVersion, VisualStudioPath[]>[] array = SyncVS.InstalledVisualStudios.Where<KeyValuePair<VisualStudioVersion, VisualStudioPath[]>>((Func<KeyValuePair<VisualStudioVersion, VisualStudioPath[]>, bool>) (kvp => ((IEnumerable<VisualStudioPath>) kvp.Value).Any<VisualStudioPath>((Func<VisualStudioPath, bool>) (v => Paths.AreEqual(v.Path, externalEditor, true))))).ToArray<KeyValuePair<VisualStudioVersion, VisualStudioPath[]>>();
      if (array.Length > 0)
      {
        vsVersion = array[0].Key;
        return true;
      }
      if (externalEditor.EndsWith("devenv.exe", StringComparison.OrdinalIgnoreCase) && UnityVSSupport.TryGetVisualStudioVersion(externalEditor, out vsVersion))
        return true;
      vsVersion = VisualStudioVersion.Invalid;
      return false;
    }

    private static bool TryGetVisualStudioVersion(string externalEditor, out VisualStudioVersion vsVersion)
    {
      switch (UnityVSSupport.ProductVersion(externalEditor).Major)
      {
        case 9:
          vsVersion = VisualStudioVersion.VisualStudio2008;
          return true;
        case 10:
          vsVersion = VisualStudioVersion.VisualStudio2010;
          return true;
        case 11:
          vsVersion = VisualStudioVersion.VisualStudio2012;
          return true;
        case 12:
          vsVersion = VisualStudioVersion.VisualStudio2013;
          return true;
        case 14:
          vsVersion = VisualStudioVersion.VisualStudio2015;
          return true;
        case 15:
          vsVersion = VisualStudioVersion.VisualStudio2017;
          return true;
        default:
          vsVersion = VisualStudioVersion.Invalid;
          return false;
      }
    }

    private static Version ProductVersion(string externalEditor)
    {
      try
      {
        return new Version(FileVersionInfo.GetVersionInfo(externalEditor).ProductVersion);
      }
      catch (Exception ex)
      {
        return new Version(0, 0);
      }
    }

    public static bool ShouldUnityVSBeActive()
    {
      return UnityVSSupport.m_ShouldUnityVSBeActive;
    }

    private static string GetAssemblyLocation(Assembly a)
    {
      try
      {
        return a.Location;
      }
      catch (NotSupportedException ex)
      {
        return (string) null;
      }
    }

    [RequiredByNativeCode]
    public static bool IsUnityVSEnabled()
    {
      if (!UnityVSSupport.s_IsUnityVSEnabled.HasValue)
        UnityVSSupport.s_IsUnityVSEnabled = new bool?(UnityVSSupport.m_ShouldUnityVSBeActive && ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Any<Assembly>((Func<Assembly, bool>) (a => UnityVSSupport.GetAssemblyLocation(a) == UnityVSSupport.s_UnityVSBridgeToLoad)));
      return UnityVSSupport.s_IsUnityVSEnabled.Value;
    }

    private static string GetVstuBridgeAssembly(VisualStudioVersion version)
    {
      try
      {
        string vsVersion = string.Empty;
        switch (version)
        {
          case VisualStudioVersion.VisualStudio2010:
            vsVersion = "2010";
            break;
          case VisualStudioVersion.VisualStudio2012:
            vsVersion = "2012";
            break;
          case VisualStudioVersion.VisualStudio2013:
            vsVersion = "2013";
            break;
          case VisualStudioVersion.VisualStudio2015:
            vsVersion = "2015";
            break;
          case VisualStudioVersion.VisualStudio2017:
            vsVersion = "15.0";
            break;
        }
        return UnityVSSupport.GetVstuBridgePathFromRegistry(vsVersion, true) ?? UnityVSSupport.GetVstuBridgePathFromRegistry(vsVersion, false);
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }

    private static string GetVstuBridgePathFromRegistry(string vsVersion, bool currentUser)
    {
      return (string) Registry.GetValue(string.Format("{0}\\Software\\Microsoft\\Microsoft Visual Studio {1} Tools for Unity", !currentUser ? (object) "HKEY_LOCAL_MACHINE" : (object) "HKEY_CURRENT_USER", (object) vsVersion), "UnityExtensionPath", (object) null);
    }

    public static void ScriptEditorChanged(string editorPath)
    {
      if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        return;
      UnityVSSupport.Initialize(editorPath);
      InternalEditorUtility.RequestScriptReload();
    }

    public static string GetAboutWindowLabel()
    {
      if (UnityVSSupport.s_AboutLabel != null)
        return UnityVSSupport.s_AboutLabel;
      UnityVSSupport.s_AboutLabel = UnityVSSupport.CalculateAboutWindowLabel();
      return UnityVSSupport.s_AboutLabel;
    }

    private static string CalculateAboutWindowLabel()
    {
      if (!UnityVSSupport.IsUnityVSEnabled())
        return "";
      Assembly assembly = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => UnityVSSupport.GetAssemblyLocation(a) == UnityVSSupport.s_UnityVSBridgeToLoad));
      if (assembly == null)
        return "";
      StringBuilder stringBuilder = new StringBuilder("Microsoft Visual Studio Tools for Unity ");
      stringBuilder.Append((object) assembly.GetName().Version);
      stringBuilder.Append(" enabled");
      return stringBuilder.ToString();
    }
  }
}
