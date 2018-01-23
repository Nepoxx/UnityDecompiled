// Decompiled with JetBrains decompiler
// Type: UnityEditor.Utils.MonoInstallationFinder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.IO;
using UnityEngine;

namespace UnityEditor.Utils
{
  internal class MonoInstallationFinder
  {
    public const string MonoInstallation = "Mono";
    public const string MonoBleedingEdgeInstallation = "MonoBleedingEdge";

    public static string GetFrameWorksFolder()
    {
      string str = FileUtil.NiceWinPath(EditorApplication.applicationPath);
      if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform != RuntimePlatform.OSXEditor)
        return Path.Combine(Path.GetDirectoryName(str), "Data");
      return Path.Combine(str, "Contents");
    }

    public static string GetProfileDirectory(string profile)
    {
      return Path.Combine(MonoInstallationFinder.GetMonoInstallation(), Path.Combine("lib", Path.Combine("mono", profile)));
    }

    public static string GetProfileDirectory(string profile, string monoInstallation)
    {
      return Path.Combine(MonoInstallationFinder.GetMonoInstallation(monoInstallation), Path.Combine("lib", Path.Combine("mono", profile)));
    }

    public static string GetProfilesDirectory(string monoInstallation)
    {
      return Path.Combine(MonoInstallationFinder.GetMonoInstallation(monoInstallation), Path.Combine("lib", "mono"));
    }

    public static string GetEtcDirectory(string monoInstallation)
    {
      return Path.Combine(MonoInstallationFinder.GetMonoInstallation(monoInstallation), Path.Combine("etc", "mono"));
    }

    public static string GetMonoInstallation()
    {
      return MonoInstallationFinder.GetMonoInstallation("Mono");
    }

    public static string GetMonoBleedingEdgeInstallation()
    {
      return MonoInstallationFinder.GetMonoInstallation("MonoBleedingEdge");
    }

    public static string GetMonoInstallation(string monoName)
    {
      return Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), monoName);
    }
  }
}
