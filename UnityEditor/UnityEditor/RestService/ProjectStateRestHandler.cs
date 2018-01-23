// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.ProjectStateRestHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Scripting;
using UnityEditor.Scripting.ScriptCompilation;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.RestService
{
  internal class ProjectStateRestHandler : JSONHandler
  {
    protected override JSONValue HandleGet(Request request, JSONValue payload)
    {
      AssetDatabase.Refresh();
      return ProjectStateRestHandler.JsonForProject();
    }

    private static JSONValue JsonForProject()
    {
      List<ProjectStateRestHandler.Island> list = ((IEnumerable<MonoIsland>) EditorCompilationInterface.GetAllMonoIslands()).Select<MonoIsland, ProjectStateRestHandler.Island>((Func<MonoIsland, ProjectStateRestHandler.Island>) (i => new ProjectStateRestHandler.Island() { MonoIsland = i, Name = Path.GetFileNameWithoutExtension(i._output), References = ((IEnumerable<string>) i._references).ToList<string>() })).ToList<ProjectStateRestHandler.Island>();
      foreach (ProjectStateRestHandler.Island island in list)
      {
        List<string> stringList1 = new List<string>();
        List<string> stringList2 = new List<string>();
        foreach (string reference in island.References)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ProjectStateRestHandler.\u003CJsonForProject\u003Ec__AnonStorey0 projectCAnonStorey0 = new ProjectStateRestHandler.\u003CJsonForProject\u003Ec__AnonStorey0();
          // ISSUE: reference to a compiler-generated field
          projectCAnonStorey0.refName = Path.GetFileNameWithoutExtension(reference);
          // ISSUE: reference to a compiler-generated method
          if (reference.StartsWith("Library/") && list.Any<ProjectStateRestHandler.Island>(new Func<ProjectStateRestHandler.Island, bool>(projectCAnonStorey0.\u003C\u003Em__0)))
          {
            // ISSUE: reference to a compiler-generated field
            stringList1.Add(projectCAnonStorey0.refName);
            stringList2.Add(reference);
          }
          if (reference.EndsWith("/UnityEditor.dll") || reference.EndsWith("/UnityEngine.dll") || (reference.EndsWith("\\UnityEditor.dll") || reference.EndsWith("\\UnityEngine.dll")))
            stringList2.Add(reference);
        }
        island.References.Add(InternalEditorUtility.GetEditorAssemblyPath());
        island.References.Add(InternalEditorUtility.GetEngineAssemblyPath());
        foreach (string str in stringList1)
          island.References.Add(str);
        foreach (string str in stringList2)
          island.References.Remove(str);
      }
      string[] array = list.SelectMany<ProjectStateRestHandler.Island, string>((Func<ProjectStateRestHandler.Island, IEnumerable<string>>) (i => (IEnumerable<string>) i.MonoIsland._files)).Concat<string>(ProjectStateRestHandler.GetAllSupportedFiles()).Distinct<string>().ToArray<string>();
      string[] projectPath = ProjectStateRestHandler.RelativeToProjectPath(ProjectStateRestHandler.FindEmptyDirectories(ProjectStateRestHandler.AssetsPath, array));
      JSONValue jsonValue1 = new JSONValue();
      jsonValue1["islands"] = new JSONValue((object) list.Select<ProjectStateRestHandler.Island, JSONValue>((Func<ProjectStateRestHandler.Island, JSONValue>) (i => ProjectStateRestHandler.JsonForIsland(i))).Where<JSONValue>((Func<JSONValue, bool>) (i2 => !i2.IsNull())).ToList<JSONValue>());
      jsonValue1["basedirectory"] = (JSONValue) ProjectStateRestHandler.ProjectPath;
      JSONValue jsonValue2 = new JSONValue();
      jsonValue2["files"] = JSONHandler.ToJSON((IEnumerable<string>) array);
      jsonValue2["emptydirectories"] = JSONHandler.ToJSON((IEnumerable<string>) projectPath);
      jsonValue1["assetdatabase"] = jsonValue2;
      return jsonValue1;
    }

    private static bool IsSupportedExtension(string extension)
    {
      if (extension.StartsWith("."))
        extension = extension.Substring(1);
      return ((IEnumerable<string>) EditorSettings.projectGenerationBuiltinExtensions).Concat<string>((IEnumerable<string>) EditorSettings.projectGenerationUserExtensions).Any<string>((Func<string, bool>) (s => string.Equals(s, extension, StringComparison.InvariantCultureIgnoreCase)));
    }

    private static IEnumerable<string> GetAllSupportedFiles()
    {
      return ((IEnumerable<string>) AssetDatabase.GetAllAssetPaths()).Where<string>((Func<string, bool>) (asset => ProjectStateRestHandler.IsSupportedExtension(Path.GetExtension(asset))));
    }

    private static JSONValue JsonForIsland(ProjectStateRestHandler.Island island)
    {
      if (island.Name == "UnityEngine" || island.Name == "UnityEditor")
        return (JSONValue) ((string) null);
      JSONValue jsonValue = new JSONValue();
      jsonValue["name"] = (JSONValue) island.Name;
      jsonValue["language"] = (JSONValue) (!island.Name.Contains("Boo") ? (!island.Name.Contains("UnityScript") ? "C#" : "UnityScript") : "Boo");
      jsonValue["files"] = JSONHandler.ToJSON((IEnumerable<string>) island.MonoIsland._files);
      jsonValue["defines"] = JSONHandler.ToJSON((IEnumerable<string>) island.MonoIsland._defines);
      jsonValue["references"] = JSONHandler.ToJSON((IEnumerable<string>) island.MonoIsland._references);
      jsonValue["basedirectory"] = (JSONValue) ProjectStateRestHandler.ProjectPath;
      return jsonValue;
    }

    private static void FindPotentialEmptyDirectories(string path, ICollection<string> result)
    {
      string[] directories = Directory.GetDirectories(path);
      if (directories.Length == 0)
      {
        result.Add(path.Replace('\\', '/'));
      }
      else
      {
        foreach (string path1 in directories)
          ProjectStateRestHandler.FindPotentialEmptyDirectories(path1, result);
      }
    }

    private static IEnumerable<string> FindPotentialEmptyDirectories(string path)
    {
      List<string> stringList = new List<string>();
      ProjectStateRestHandler.FindPotentialEmptyDirectories(path, (ICollection<string>) stringList);
      return (IEnumerable<string>) stringList;
    }

    private static string[] FindEmptyDirectories(string path, string[] files)
    {
      return ProjectStateRestHandler.FindPotentialEmptyDirectories(path).Where<string>((Func<string, bool>) (d => !((IEnumerable<string>) files).Any<string>((Func<string, bool>) (f => f.StartsWith(d))))).ToArray<string>();
    }

    private static string[] RelativeToProjectPath(string[] paths)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return ((IEnumerable<string>) paths).Select<string, string>(new Func<string, string>(new ProjectStateRestHandler.\u003CRelativeToProjectPath\u003Ec__AnonStorey4() { projectPath = !ProjectStateRestHandler.ProjectPath.EndsWith("/") ? ProjectStateRestHandler.ProjectPath + "/" : ProjectStateRestHandler.ProjectPath }.\u003C\u003Em__0)).ToArray<string>();
    }

    private static string ProjectPath
    {
      get
      {
        return Path.GetDirectoryName(Application.dataPath);
      }
    }

    private static string AssetsPath
    {
      get
      {
        return ProjectStateRestHandler.ProjectPath + "/Assets";
      }
    }

    internal static void Register()
    {
      Router.RegisterHandler("/unity/projectstate", (Handler) new ProjectStateRestHandler());
    }

    public class Island
    {
      public MonoIsland MonoIsland { get; set; }

      public string Name { get; set; }

      public List<string> References { get; set; }
    }
  }
}
