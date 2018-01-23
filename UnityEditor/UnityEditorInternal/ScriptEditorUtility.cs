// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ScriptEditorUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditorInternal
{
  public class ScriptEditorUtility
  {
    public static ScriptEditorUtility.ScriptEditor GetScriptEditorFromPath(string path)
    {
      string lower = path.ToLower();
      if (lower == "internal")
        return ScriptEditorUtility.ScriptEditor.Internal;
      if (lower.Contains("monodevelop") || lower.Contains("xamarinstudio") || lower.Contains("xamarin studio"))
        return ScriptEditorUtility.ScriptEditor.MonoDevelop;
      if (lower.EndsWith("devenv.exe"))
        return ScriptEditorUtility.ScriptEditor.VisualStudio;
      if (lower.EndsWith("vcsexpress.exe"))
        return ScriptEditorUtility.ScriptEditor.VisualStudioExpress;
      string str = Path.GetFileName(Paths.UnifyDirectorySeparator(lower)).Replace(" ", "");
      if (str == "visualstudio.app")
        return ScriptEditorUtility.ScriptEditor.MonoDevelop;
      if (str == "code.exe" || str == "visualstudiocode.app" || (str == "vscode.app" || str == "code.app") || str == "code")
        return ScriptEditorUtility.ScriptEditor.VisualStudioCode;
      return str == "rider.exe" || str == "rider64.exe" || (str == "rider32.exe" || str == "ridereap.app") || (str == "rider.app" || str == "rider.sh") ? ScriptEditorUtility.ScriptEditor.Rider : ScriptEditorUtility.ScriptEditor.Other;
    }

    public static bool IsScriptEditorSpecial(string path)
    {
      return ScriptEditorUtility.GetScriptEditorFromPath(path) != ScriptEditorUtility.ScriptEditor.Other;
    }

    public static string GetExternalScriptEditor()
    {
      return EditorPrefs.GetString("kScriptsDefaultApp");
    }

    public static void SetExternalScriptEditor(string path)
    {
      EditorPrefs.SetString("kScriptsDefaultApp", path);
    }

    private static string GetScriptEditorArgsKey(string path)
    {
      if (Application.platform == RuntimePlatform.OSXEditor)
        return "kScriptEditorArgs_" + path;
      return "kScriptEditorArgs" + path;
    }

    private static string GetDefaultStringEditorArgs()
    {
      return Application.platform == RuntimePlatform.OSXEditor ? "" : "\"$(File)\"";
    }

    public static string GetExternalScriptEditorArgs()
    {
      string externalScriptEditor = ScriptEditorUtility.GetExternalScriptEditor();
      if (ScriptEditorUtility.IsScriptEditorSpecial(externalScriptEditor))
        return "";
      return EditorPrefs.GetString(ScriptEditorUtility.GetScriptEditorArgsKey(externalScriptEditor), ScriptEditorUtility.GetDefaultStringEditorArgs());
    }

    public static void SetExternalScriptEditorArgs(string args)
    {
      EditorPrefs.SetString(ScriptEditorUtility.GetScriptEditorArgsKey(ScriptEditorUtility.GetExternalScriptEditor()), args);
    }

    public static ScriptEditorUtility.ScriptEditor GetScriptEditorFromPreferences()
    {
      return ScriptEditorUtility.GetScriptEditorFromPath(ScriptEditorUtility.GetExternalScriptEditor());
    }

    public static string[] GetFoundScriptEditorPaths(RuntimePlatform platform)
    {
      List<string> list = new List<string>();
      if (platform == RuntimePlatform.OSXEditor)
        ScriptEditorUtility.AddIfDirectoryExists("/Applications/Visual Studio.app", list);
      return list.ToArray();
    }

    private static void AddIfDirectoryExists(string path, List<string> list)
    {
      if (!Directory.Exists(path))
        return;
      list.Add(path);
    }

    public enum ScriptEditor
    {
      Internal = 0,
      MonoDevelop = 1,
      VisualStudio = 2,
      VisualStudioExpress = 3,
      VisualStudioCode = 4,
      Rider = 5,
      Other = 32, // 0x00000020
    }
  }
}
