// Decompiled with JetBrains decompiler
// Type: UnityEditor.SyncVS
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Build;
using UnityEditor.Utils;
using UnityEditor.VisualStudioIntegration;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [InitializeOnLoad]
  internal class SyncVS : AssetPostprocessor
  {
    private static readonly SolutionSynchronizer Synchronizer = new SolutionSynchronizer(Directory.GetParent(Application.dataPath).FullName, (ISolutionSynchronizationSettings) new SyncVS.SolutionSynchronizationSettings());
    private static bool s_AlreadySyncedThisDomainReload;

    static SyncVS()
    {
      try
      {
        SyncVS.InstalledVisualStudios = SyncVS.GetInstalledVisualStudios() as Dictionary<VisualStudioVersion, VisualStudioPath[]>;
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error detecting Visual Studio installations: {0}{1}{2}", (object) ex.Message, (object) Environment.NewLine, (object) ex.StackTrace);
        SyncVS.InstalledVisualStudios = new Dictionary<VisualStudioVersion, VisualStudioPath[]>();
      }
      SyncVS.SetVisualStudioAsEditorIfNoEditorWasSet();
      UnityVSSupport.Initialize();
    }

    private static void SetVisualStudioAsEditorIfNoEditorWasSet()
    {
      string str = EditorPrefs.GetString("kScriptsDefaultApp");
      string bestVisualStudio = SyncVS.FindBestVisualStudio();
      if (!(str == "") || bestVisualStudio == null)
        return;
      EditorPrefs.SetString("kScriptsDefaultApp", bestVisualStudio);
    }

    public static string FindBestVisualStudio()
    {
      VisualStudioPath[] visualStudioPathArray = SyncVS.InstalledVisualStudios.OrderByDescending<KeyValuePair<VisualStudioVersion, VisualStudioPath[]>, VisualStudioVersion>((Func<KeyValuePair<VisualStudioVersion, VisualStudioPath[]>, VisualStudioVersion>) (kvp => kvp.Key)).Select<KeyValuePair<VisualStudioVersion, VisualStudioPath[]>, VisualStudioPath[]>((Func<KeyValuePair<VisualStudioVersion, VisualStudioPath[]>, VisualStudioPath[]>) (kvp2 => kvp2.Value)).FirstOrDefault<VisualStudioPath[]>();
      return visualStudioPathArray != null ? ((IEnumerable<VisualStudioPath>) visualStudioPathArray).Last<VisualStudioPath>().Path : (string) null;
    }

    internal static Dictionary<VisualStudioVersion, VisualStudioPath[]> InstalledVisualStudios { get; private set; }

    public static bool ProjectExists()
    {
      return SyncVS.Synchronizer.SolutionExists();
    }

    public static void CreateIfDoesntExist()
    {
      if (SyncVS.Synchronizer.SolutionExists())
        return;
      SyncVS.Synchronizer.Sync();
    }

    public static void SyncVisualStudioProjectIfItAlreadyExists()
    {
      if (!SyncVS.Synchronizer.SolutionExists())
        return;
      SyncVS.Synchronizer.Sync();
    }

    public static void PostprocessSyncProject(string[] importedAssets, string[] addedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
      SyncVS.Synchronizer.SyncIfNeeded(((IEnumerable<string>) addedAssets).Union<string>(((IEnumerable<string>) deletedAssets).Union<string>(((IEnumerable<string>) movedAssets).Union<string>((IEnumerable<string>) movedFromAssetPaths))));
    }

    [MenuItem("Assets/Open C# Project")]
    private static void SyncAndOpenSolution()
    {
      SyncVS.SyncSolution();
      SyncVS.OpenProjectFileUnlessInBatchMode();
    }

    public static void SyncSolution()
    {
      AssetDatabase.Refresh();
      SyncVS.Synchronizer.Sync();
    }

    public static void SyncIfFirstFileOpenSinceDomainLoad()
    {
      if (SyncVS.s_AlreadySyncedThisDomainReload)
        return;
      SyncVS.s_AlreadySyncedThisDomainReload = true;
      SyncVS.Synchronizer.Sync();
    }

    private static void OpenProjectFileUnlessInBatchMode()
    {
      if (InternalEditorUtility.inBatchMode)
        return;
      InternalEditorUtility.OpenFileAtLineExternal("", -1);
    }

    private static IDictionary<VisualStudioVersion, VisualStudioPath[]> GetInstalledVisualStudios()
    {
      Dictionary<VisualStudioVersion, VisualStudioPath[]> dictionary = new Dictionary<VisualStudioVersion, VisualStudioPath[]>();
      if (SyncVS.SolutionSynchronizationSettings.IsWindows)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SyncVS.\u003CGetInstalledVisualStudios\u003Ec__AnonStorey0 studiosCAnonStorey0 = new SyncVS.\u003CGetInstalledVisualStudios\u003Ec__AnonStorey0();
        IEnumerator enumerator = Enum.GetValues(typeof (VisualStudioVersion)).GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            VisualStudioVersion current = (VisualStudioVersion) enumerator.Current;
            if (current <= VisualStudioVersion.VisualStudio2015)
            {
              try
              {
                string environmentVariable = Environment.GetEnvironmentVariable(string.Format("VS{0}0COMNTOOLS", (object) current));
                if (!string.IsNullOrEmpty(environmentVariable))
                {
                  string path = Paths.Combine(environmentVariable, "..", "IDE", "devenv.exe");
                  if (File.Exists(path))
                  {
                    dictionary[current] = new VisualStudioPath[1]
                    {
                      new VisualStudioPath(path, "")
                    };
                    continue;
                  }
                }
                string registryValue1 = SyncVS.GetRegistryValue(string.Format("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\VisualStudio\\{0}.0", (object) current), "InstallDir");
                if (string.IsNullOrEmpty(registryValue1))
                  registryValue1 = SyncVS.GetRegistryValue(string.Format("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Microsoft\\VisualStudio\\{0}.0", (object) current), "InstallDir");
                if (!string.IsNullOrEmpty(registryValue1))
                {
                  string path = Paths.Combine(registryValue1, "devenv.exe");
                  if (File.Exists(path))
                  {
                    dictionary[current] = new VisualStudioPath[1]
                    {
                      new VisualStudioPath(path, "")
                    };
                    continue;
                  }
                }
                string registryValue2 = SyncVS.GetRegistryValue(string.Format("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\VisualStudio\\{0}.0\\Debugger", (object) current), "FEQARuntimeImplDll");
                if (!string.IsNullOrEmpty(registryValue2))
                {
                  string path = SyncVS.DeriveVisualStudioPath(registryValue2);
                  if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    dictionary[current] = new VisualStudioPath[1]
                    {
                      new VisualStudioPath(SyncVS.DeriveVisualStudioPath(registryValue2), "")
                    };
                }
              }
              catch
              {
              }
            }
          }
        }
        finally
        {
          IDisposable disposable;
          if ((disposable = enumerator as IDisposable) != null)
            disposable.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        studiosCAnonStorey0.requiredWorkloads = new string[1]
        {
          "Microsoft.VisualStudio.Workload.ManagedGame"
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        VisualStudioPath[] array = VisualStudioUtil.ParseRawDevEnvPaths(VisualStudioUtil.FindVisualStudioDevEnvPaths(15, studiosCAnonStorey0.requiredWorkloads)).Where<VisualStudioUtil.VisualStudio>(new Func<VisualStudioUtil.VisualStudio, bool>(studiosCAnonStorey0.\u003C\u003Em__0)).Select<VisualStudioUtil.VisualStudio, VisualStudioPath>((Func<VisualStudioUtil.VisualStudio, VisualStudioPath>) (vs => new VisualStudioPath(vs.DevEnvPath, vs.Edition))).ToArray<VisualStudioPath>();
        if (array.Length != 0)
          dictionary[VisualStudioVersion.VisualStudio2017] = array;
      }
      return (IDictionary<VisualStudioVersion, VisualStudioPath[]>) dictionary;
    }

    private static string GetRegistryValue(string path, string key)
    {
      try
      {
        return Registry.GetValue(path, key, (object) null) as string;
      }
      catch (Exception ex)
      {
        return "";
      }
    }

    private static string DeriveVisualStudioPath(string debuggerPath)
    {
      string a1 = SyncVS.DeriveProgramFilesSentinel();
      string a2 = "Common7";
      bool flag = false;
      string[] strArray = debuggerPath.Split(new char[2]{ Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
      string path1 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
      foreach (string str in strArray)
      {
        if (!flag && string.Equals(a1, str, StringComparison.OrdinalIgnoreCase))
          flag = true;
        else if (flag)
        {
          path1 = Path.Combine(path1, str);
          if (string.Equals(a2, str, StringComparison.OrdinalIgnoreCase))
            break;
        }
      }
      return Paths.Combine(path1, "IDE", "devenv.exe");
    }

    private static string DeriveProgramFilesSentinel()
    {
      string str = ((IEnumerable<string>) Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Split(new char[2]{ Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar })).LastOrDefault<string>();
      if (string.IsNullOrEmpty(str))
        return "Program Files";
      int startIndex = str.LastIndexOf("(x86)");
      if (0 <= startIndex)
        str = str.Remove(startIndex);
      return str.TrimEnd();
    }

    private static bool PathsAreEquivalent(string aPath, string zPath)
    {
      if (aPath == null && zPath == null)
        return true;
      if (string.IsNullOrEmpty(aPath) || string.IsNullOrEmpty(zPath))
        return false;
      aPath = Path.GetFullPath(aPath);
      zPath = Path.GetFullPath(zPath);
      StringComparison comparisonType = StringComparison.OrdinalIgnoreCase;
      if (!SyncVS.SolutionSynchronizationSettings.IsOSX && !SyncVS.SolutionSynchronizationSettings.IsWindows)
        comparisonType = StringComparison.Ordinal;
      aPath = aPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
      zPath = zPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
      return string.Equals(aPath, zPath, comparisonType);
    }

    internal static bool CheckVisualStudioVersion(int major, int minor, int build)
    {
      int num = -1;
      if (major != 11)
        return false;
      RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\DevDiv\\vc\\Servicing");
      if (registryKey1 == null)
        return false;
      foreach (string subKeyName in registryKey1.GetSubKeyNames())
      {
        if (subKeyName.StartsWith("11."))
        {
          if (subKeyName.Length > 3)
          {
            try
            {
              int int32 = Convert.ToInt32(subKeyName.Substring(3));
              if (int32 > num)
                num = int32;
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      if (num < 0)
        return false;
      RegistryKey registryKey2 = registryKey1.OpenSubKey(string.Format("11.{0}\\RuntimeDebug", (object) num));
      if (registryKey2 == null)
        return false;
      string str = registryKey2.GetValue("Version", (object) null) as string;
      if (str == null)
        return false;
      string[] strArray = str.Split('.');
      if (strArray != null)
      {
        if (strArray.Length >= 3)
        {
          int int32;
          try
          {
            int32 = Convert.ToInt32(strArray[2]);
          }
          catch (Exception ex)
          {
            return false;
          }
          return num > minor || num == minor && int32 >= build;
        }
      }
      return false;
    }

    private class SolutionSynchronizationSettings : DefaultSolutionSynchronizationSettings
    {
      public override int VisualStudioVersion
      {
        get
        {
          string externalScriptEditor = ScriptEditorUtility.GetExternalScriptEditor();
          return SyncVS.InstalledVisualStudios.ContainsKey(VisualStudioVersion.VisualStudio2008) && externalScriptEditor != string.Empty && SyncVS.PathsAreEquivalent(((IEnumerable<VisualStudioPath>) SyncVS.InstalledVisualStudios[VisualStudioVersion.VisualStudio2008]).Last<VisualStudioPath>().Path, externalScriptEditor) ? 9 : 10;
        }
      }

      public override string SolutionTemplate
      {
        get
        {
          return EditorPrefs.GetString("VSSolutionText", base.SolutionTemplate);
        }
      }

      public override string GetProjectHeaderTemplate(ScriptingLanguage language)
      {
        return EditorPrefs.GetString("VSProjectHeader", base.GetProjectHeaderTemplate(language));
      }

      public override string GetProjectFooterTemplate(ScriptingLanguage language)
      {
        return EditorPrefs.GetString("VSProjectFooter", base.GetProjectFooterTemplate(language));
      }

      public override string EditorAssemblyPath
      {
        get
        {
          return InternalEditorUtility.GetEditorAssemblyPath();
        }
      }

      public override string EngineAssemblyPath
      {
        get
        {
          return InternalEditorUtility.GetMonolithicEngineAssemblyPath();
        }
      }

      public override string[] Defines
      {
        get
        {
          return EditorUserBuildSettings.activeScriptCompilationDefines;
        }
      }

      protected override string FrameworksPath()
      {
        return EditorApplication.applicationContentsPath;
      }

      internal static bool IsOSX
      {
        get
        {
          return Environment.OSVersion.Platform == PlatformID.Unix;
        }
      }

      internal static bool IsWindows
      {
        get
        {
          return !SyncVS.SolutionSynchronizationSettings.IsOSX && (int) Path.DirectorySeparatorChar == 92 && Environment.NewLine == "\r\n";
        }
      }
    }

    private class BuildTargetChangedHandler : IActiveBuildTargetChanged, IOrderedCallback
    {
      public int callbackOrder
      {
        get
        {
          return 0;
        }
      }

      public void OnActiveBuildTargetChanged(BuildTarget oldTarget, BuildTarget newTarget)
      {
        SyncVS.SyncVisualStudioProjectIfItAlreadyExists();
      }
    }
  }
}
