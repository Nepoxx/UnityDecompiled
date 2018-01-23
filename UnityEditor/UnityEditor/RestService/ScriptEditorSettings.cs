// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.ScriptEditorSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.RestService
{
  internal class ScriptEditorSettings
  {
    static ScriptEditorSettings()
    {
      ScriptEditorSettings.OpenDocuments = new List<string>();
      ScriptEditorSettings.Clear();
    }

    public static string Name { get; set; }

    public static string ServerURL { get; set; }

    public static int ProcessId { get; set; }

    public static List<string> OpenDocuments { get; set; }

    private static string FilePath
    {
      get
      {
        return Application.dataPath + "/../Library/UnityScriptEditorSettings.json";
      }
    }

    private static void Clear()
    {
      ScriptEditorSettings.Name = (string) null;
      ScriptEditorSettings.ServerURL = (string) null;
      ScriptEditorSettings.ProcessId = -1;
    }

    public static void Save()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("{{\n\t\"name\" : \"{0}\",\n\t\"serverurl\" : \"{1}\",\n\t\"processid\" : {2},\n\t", (object) ScriptEditorSettings.Name, (object) ScriptEditorSettings.ServerURL, (object) ScriptEditorSettings.ProcessId);
      stringBuilder.AppendFormat("\"opendocuments\" : [{0}]\n}}", (object) string.Join(",", ScriptEditorSettings.OpenDocuments.Select<string, string>((Func<string, string>) (d => "\"" + d + "\"")).ToArray<string>()));
      File.WriteAllText(ScriptEditorSettings.FilePath, stringBuilder.ToString());
    }

    public static void Load()
    {
      try
      {
        JSONValue jsonValue = new JSONParser(File.ReadAllText(ScriptEditorSettings.FilePath)).Parse();
        ScriptEditorSettings.Name = !jsonValue.ContainsKey("name") ? (string) null : jsonValue["name"].AsString();
        ScriptEditorSettings.ServerURL = !jsonValue.ContainsKey("serverurl") ? (string) null : jsonValue["serverurl"].AsString();
        ScriptEditorSettings.ProcessId = !jsonValue.ContainsKey("processid") ? -1 : (int) jsonValue["processid"].AsFloat();
        ScriptEditorSettings.OpenDocuments = !jsonValue.ContainsKey("opendocuments") ? new List<string>() : jsonValue["opendocuments"].AsList().Select<JSONValue, string>((Func<JSONValue, string>) (d => d.AsString())).ToList<string>();
        if (ScriptEditorSettings.ProcessId < 0)
          return;
        Process.GetProcessById(ScriptEditorSettings.ProcessId);
      }
      catch (FileNotFoundException ex)
      {
        ScriptEditorSettings.Clear();
        ScriptEditorSettings.Save();
      }
      catch (Exception ex)
      {
        Logger.Log(ex);
        ScriptEditorSettings.Clear();
        ScriptEditorSettings.Save();
      }
    }
  }
}
