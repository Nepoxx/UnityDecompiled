// Decompiled with JetBrains decompiler
// Type: UnityEditor.PresetLibraryLocations
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal static class PresetLibraryLocations
  {
    public static string defaultLibraryLocation
    {
      get
      {
        return PresetLibraryLocations.GetDefaultFilePathForFileLocation(PresetFileLocation.PreferencesFolder);
      }
    }

    public static string defaultPresetLibraryPath
    {
      get
      {
        return Path.Combine(PresetLibraryLocations.defaultLibraryLocation, PresetLibraryLocations.defaultLibraryName);
      }
    }

    public static string defaultLibraryName
    {
      get
      {
        return "Default";
      }
    }

    public static List<string> GetAvailableFilesWithExtensionOnTheHDD(PresetFileLocation fileLocation, string fileExtensionWithoutDot)
    {
      List<string> exentionFromFolders = PresetLibraryLocations.GetFilesWithExentionFromFolders(PresetLibraryLocations.GetDirectoryPaths(fileLocation), fileExtensionWithoutDot);
      for (int index = 0; index < exentionFromFolders.Count; ++index)
        exentionFromFolders[index] = PresetLibraryLocations.ConvertToUnitySeperators(exentionFromFolders[index]);
      return exentionFromFolders;
    }

    public static string GetDefaultFilePathForFileLocation(PresetFileLocation fileLocation)
    {
      if (fileLocation == PresetFileLocation.PreferencesFolder)
        return InternalEditorUtility.unityPreferencesFolder + "/Presets/";
      if (fileLocation == PresetFileLocation.ProjectFolder)
        return "Assets/Editor/";
      Debug.LogError((object) "Enum not handled!");
      return "";
    }

    private static List<string> GetDirectoryPaths(PresetFileLocation fileLocation)
    {
      List<string> stringList = new List<string>();
      switch (fileLocation)
      {
        case PresetFileLocation.PreferencesFolder:
          stringList.Add(PresetLibraryLocations.GetDefaultFilePathForFileLocation(PresetFileLocation.PreferencesFolder));
          break;
        case PresetFileLocation.ProjectFolder:
          string[] directories = Directory.GetDirectories("Assets/", "Editor", SearchOption.AllDirectories);
          stringList.AddRange((IEnumerable<string>) directories);
          break;
        default:
          Debug.LogError((object) "Enum not handled!");
          break;
      }
      return stringList;
    }

    private static List<string> GetFilesWithExentionFromFolders(List<string> folderPaths, string fileExtensionWithoutDot)
    {
      List<string> stringList = new List<string>();
      foreach (string folderPath in folderPaths)
      {
        string[] files = Directory.GetFiles(folderPath, "*." + fileExtensionWithoutDot);
        stringList.AddRange((IEnumerable<string>) files);
      }
      return stringList;
    }

    public static PresetFileLocation GetFileLocationFromPath(string path)
    {
      if (path.Contains(InternalEditorUtility.unityPreferencesFolder))
        return PresetFileLocation.PreferencesFolder;
      if (path.Contains("Assets/"))
        return PresetFileLocation.ProjectFolder;
      Debug.LogError((object) ("Could not determine preset file location type " + path));
      return PresetFileLocation.ProjectFolder;
    }

    private static string ConvertToUnitySeperators(string path)
    {
      return path.Replace('\\', '/');
    }

    public static string GetParticleCurveLibraryExtension(bool singleCurve, bool signedRange)
    {
      string str1 = "particle";
      string str2 = !singleCurve ? str1 + "DoubleCurves" : str1 + "Curves";
      return !signedRange ? str2 + "" : str2 + "Signed";
    }

    public static string GetCurveLibraryExtension(bool normalized)
    {
      return normalized ? "curvesNormalized" : "curves";
    }
  }
}
