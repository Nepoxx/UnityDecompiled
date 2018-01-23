// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilation.EditorBuildRules
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Scripting.Compilers;
using UnityEngine;

namespace UnityEditor.Scripting.ScriptCompilation
{
  internal static class EditorBuildRules
  {
    private static readonly EditorBuildRules.TargetAssembly[] predefinedTargetAssemblies = EditorBuildRules.CreatePredefinedTargetAssemblies();

    public static EditorBuildRules.TargetAssembly[] GetPredefinedTargetAssemblies()
    {
      return EditorBuildRules.predefinedTargetAssemblies;
    }

    public static PrecompiledAssembly CreateUserCompiledAssembly(string path)
    {
      AssemblyFlags assemblyFlags = AssemblyFlags.None;
      string lower = path.ToLower();
      if (lower.Contains("/editor/") || lower.Contains("\\editor\\"))
        assemblyFlags |= AssemblyFlags.EditorOnly;
      return new PrecompiledAssembly() { Path = path, Flags = assemblyFlags };
    }

    public static PrecompiledAssembly CreateEditorCompiledAssembly(string path)
    {
      return new PrecompiledAssembly() { Path = path, Flags = AssemblyFlags.EditorOnly };
    }

    public static EditorBuildRules.TargetAssembly[] CreateTargetAssemblies(IEnumerable<CustomScriptAssembly> customScriptAssemblies)
    {
      if (customScriptAssemblies == null)
        return (EditorBuildRules.TargetAssembly[]) null;
      foreach (CustomScriptAssembly customScriptAssembly in customScriptAssemblies)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EditorBuildRules.\u003CCreateTargetAssemblies\u003Ec__AnonStorey0 assembliesCAnonStorey0 = new EditorBuildRules.\u003CCreateTargetAssemblies\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        assembliesCAnonStorey0.customAssembly = customScriptAssembly;
        // ISSUE: reference to a compiler-generated method
        if (((IEnumerable<EditorBuildRules.TargetAssembly>) EditorBuildRules.predefinedTargetAssemblies).Any<EditorBuildRules.TargetAssembly>(new Func<EditorBuildRules.TargetAssembly, bool>(assembliesCAnonStorey0.\u003C\u003Em__0)))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          throw new Exception(string.Format("Assembly cannot be have reserved name '{0}'. Defined in '{1}'", (object) assembliesCAnonStorey0.customAssembly.Name, (object) assembliesCAnonStorey0.customAssembly.FilePath));
        }
      }
      List<EditorBuildRules.TargetAssembly> targetAssemblyList = new List<EditorBuildRules.TargetAssembly>();
      Dictionary<string, EditorBuildRules.TargetAssembly> dictionary = new Dictionary<string, EditorBuildRules.TargetAssembly>();
      foreach (CustomScriptAssembly customScriptAssembly in customScriptAssemblies)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EditorBuildRules.\u003CCreateTargetAssemblies\u003Ec__AnonStorey2 assembliesCAnonStorey2 = new EditorBuildRules.\u003CCreateTargetAssemblies\u003Ec__AnonStorey2();
        // ISSUE: reference to a compiler-generated field
        assembliesCAnonStorey2.customAssembly = customScriptAssembly;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EditorBuildRules.\u003CCreateTargetAssemblies\u003Ec__AnonStorey1 assembliesCAnonStorey1 = new EditorBuildRules.\u003CCreateTargetAssemblies\u003Ec__AnonStorey1();
        // ISSUE: reference to a compiler-generated field
        assembliesCAnonStorey1.\u003C\u003Ef__ref\u00242 = assembliesCAnonStorey2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        assembliesCAnonStorey1.pathPrefixLowerCase = assembliesCAnonStorey2.customAssembly.PathPrefix.ToLower();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        EditorBuildRules.TargetAssembly targetAssembly = new EditorBuildRules.TargetAssembly(assembliesCAnonStorey2.customAssembly.Name + ".dll", (SupportedLanguage) null, assembliesCAnonStorey2.customAssembly.AssemblyFlags, EditorBuildRules.TargetAssemblyType.Custom, new Func<string, int>(assembliesCAnonStorey1.\u003C\u003Em__0), new Func<BuildTarget, EditorScriptCompilationOptions, bool>(assembliesCAnonStorey1.\u003C\u003Em__1));
        targetAssemblyList.Add(targetAssembly);
        // ISSUE: reference to a compiler-generated field
        dictionary[assembliesCAnonStorey2.customAssembly.Name] = targetAssembly;
      }
      List<EditorBuildRules.TargetAssembly>.Enumerator enumerator = targetAssemblyList.GetEnumerator();
      foreach (CustomScriptAssembly customScriptAssembly in customScriptAssemblies)
      {
        enumerator.MoveNext();
        EditorBuildRules.TargetAssembly current = enumerator.Current;
        if (customScriptAssembly.References != null)
        {
          foreach (string reference in customScriptAssembly.References)
          {
            EditorBuildRules.TargetAssembly targetAssembly = (EditorBuildRules.TargetAssembly) null;
            if (!dictionary.TryGetValue(reference, out targetAssembly))
              Debug.LogWarning((object) string.Format("Could not find reference '{0}' for assembly '{1}'", (object) reference, (object) customScriptAssembly.Name));
            else
              current.References.Add(targetAssembly);
          }
        }
      }
      return targetAssemblyList.ToArray();
    }

    public static ScriptAssembly[] GetAllScriptAssemblies(IEnumerable<string> allSourceFiles, string projectDirectory, ScriptAssemblySettings settings, EditorBuildRules.CompilationAssemblies assemblies)
    {
      if (allSourceFiles == null || allSourceFiles.Count<string>() == 0)
        return new ScriptAssembly[0];
      Dictionary<EditorBuildRules.TargetAssembly, HashSet<string>> dictionary = new Dictionary<EditorBuildRules.TargetAssembly, HashSet<string>>();
      foreach (string allSourceFile in allSourceFiles)
      {
        EditorBuildRules.TargetAssembly targetAssembly = EditorBuildRules.GetTargetAssembly(allSourceFile, projectDirectory, assemblies.CustomTargetAssemblies);
        if (EditorBuildRules.IsCompatibleWithPlatform(targetAssembly, settings))
        {
          HashSet<string> stringSet;
          if (!dictionary.TryGetValue(targetAssembly, out stringSet))
          {
            stringSet = new HashSet<string>();
            dictionary[targetAssembly] = stringSet;
          }
          stringSet.Add(AssetPath.Combine(projectDirectory, allSourceFile));
        }
      }
      return EditorBuildRules.ToScriptAssemblies((IDictionary<EditorBuildRules.TargetAssembly, HashSet<string>>) dictionary, settings, assemblies, (HashSet<string>) null);
    }

    public static ScriptAssembly[] GenerateChangedScriptAssemblies(EditorBuildRules.GenerateChangedScriptAssembliesArgs args)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EditorBuildRules.\u003CGenerateChangedScriptAssemblies\u003Ec__AnonStorey3 assembliesCAnonStorey3 = new EditorBuildRules.\u003CGenerateChangedScriptAssemblies\u003Ec__AnonStorey3();
      // ISSUE: reference to a compiler-generated field
      assembliesCAnonStorey3.args = args;
      Dictionary<EditorBuildRules.TargetAssembly, HashSet<string>> source = new Dictionary<EditorBuildRules.TargetAssembly, HashSet<string>>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EditorBuildRules.TargetAssembly[] targetAssemblyArray = assembliesCAnonStorey3.args.Assemblies.CustomTargetAssemblies != null ? ((IEnumerable<EditorBuildRules.TargetAssembly>) EditorBuildRules.predefinedTargetAssemblies).Concat<EditorBuildRules.TargetAssembly>((IEnumerable<EditorBuildRules.TargetAssembly>) assembliesCAnonStorey3.args.Assemblies.CustomTargetAssemblies).ToArray<EditorBuildRules.TargetAssembly>() : EditorBuildRules.predefinedTargetAssemblies;
      // ISSUE: reference to a compiler-generated field
      if (assembliesCAnonStorey3.args.RunUpdaterAssemblies != null)
      {
        // ISSUE: reference to a compiler-generated field
        foreach (string runUpdaterAssembly in assembliesCAnonStorey3.args.RunUpdaterAssemblies)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated method
          EditorBuildRules.TargetAssembly index = ((IEnumerable<EditorBuildRules.TargetAssembly>) targetAssemblyArray).First<EditorBuildRules.TargetAssembly>(new Func<EditorBuildRules.TargetAssembly, bool>(new EditorBuildRules.\u003CGenerateChangedScriptAssemblies\u003Ec__AnonStorey4() { \u003C\u003Ef__ref\u00243 = assembliesCAnonStorey3, assemblyFilename = runUpdaterAssembly }.\u003C\u003Em__0));
          source[index] = new HashSet<string>();
        }
      }
      // ISSUE: reference to a compiler-generated field
      foreach (string dirtySourceFile in assembliesCAnonStorey3.args.DirtySourceFiles)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EditorBuildRules.TargetAssembly targetAssembly = EditorBuildRules.GetTargetAssembly(dirtySourceFile, assembliesCAnonStorey3.args.ProjectDirectory, assembliesCAnonStorey3.args.Assemblies.CustomTargetAssemblies);
        // ISSUE: reference to a compiler-generated field
        if (EditorBuildRules.IsCompatibleWithPlatform(targetAssembly, assembliesCAnonStorey3.args.Settings))
        {
          SupportedLanguage languageFromExtension = ScriptCompilers.GetLanguageFromExtension(ScriptCompilers.GetExtensionOfSourceFile(dirtySourceFile));
          HashSet<string> stringSet;
          if (!source.TryGetValue(targetAssembly, out stringSet))
          {
            stringSet = new HashSet<string>();
            source[targetAssembly] = stringSet;
            if (targetAssembly.Type == EditorBuildRules.TargetAssemblyType.Custom)
              targetAssembly.Language = languageFromExtension;
          }
          // ISSUE: reference to a compiler-generated field
          stringSet.Add(AssetPath.Combine(assembliesCAnonStorey3.args.ProjectDirectory, dirtySourceFile));
          if (languageFromExtension != targetAssembly.Language)
          {
            // ISSUE: reference to a compiler-generated field
            assembliesCAnonStorey3.args.NotCompiledTargetAssemblies.Add(targetAssembly);
          }
        }
      }
      if (source.Any<KeyValuePair<EditorBuildRules.TargetAssembly, HashSet<string>>>((Func<KeyValuePair<EditorBuildRules.TargetAssembly, HashSet<string>>, bool>) (entry => entry.Key.Type == EditorBuildRules.TargetAssemblyType.Custom)))
      {
        foreach (EditorBuildRules.TargetAssembly predefinedTargetAssembly in EditorBuildRules.predefinedTargetAssemblies)
        {
          // ISSUE: reference to a compiler-generated field
          if (EditorBuildRules.IsCompatibleWithPlatform(predefinedTargetAssembly, assembliesCAnonStorey3.args.Settings) && !source.ContainsKey(predefinedTargetAssembly))
            source[predefinedTargetAssembly] = new HashSet<string>();
        }
      }
      int num;
      do
      {
        num = 0;
        foreach (EditorBuildRules.TargetAssembly index in targetAssemblyArray)
        {
          // ISSUE: reference to a compiler-generated field
          if (EditorBuildRules.IsCompatibleWithPlatform(index, assembliesCAnonStorey3.args.Settings) && !source.ContainsKey(index))
          {
            foreach (EditorBuildRules.TargetAssembly reference in index.References)
            {
              if (source.ContainsKey(reference))
              {
                source[index] = new HashSet<string>();
                ++num;
                break;
              }
            }
          }
        }
      }
      while (num > 0);
      // ISSUE: reference to a compiler-generated field
      foreach (string allSourceFile in assembliesCAnonStorey3.args.AllSourceFiles)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EditorBuildRules.TargetAssembly targetAssembly = EditorBuildRules.GetTargetAssembly(allSourceFile, assembliesCAnonStorey3.args.ProjectDirectory, assembliesCAnonStorey3.args.Assemblies.CustomTargetAssemblies);
        // ISSUE: reference to a compiler-generated field
        if (EditorBuildRules.IsCompatibleWithPlatform(targetAssembly, assembliesCAnonStorey3.args.Settings))
        {
          SupportedLanguage languageFromExtension = ScriptCompilers.GetLanguageFromExtension(ScriptCompilers.GetExtensionOfSourceFile(allSourceFile));
          if (targetAssembly.Language == null && targetAssembly.Type == EditorBuildRules.TargetAssemblyType.Custom)
            targetAssembly.Language = languageFromExtension;
          if (languageFromExtension != targetAssembly.Language)
          {
            // ISSUE: reference to a compiler-generated field
            assembliesCAnonStorey3.args.NotCompiledTargetAssemblies.Add(targetAssembly);
          }
          HashSet<string> stringSet;
          if (source.TryGetValue(targetAssembly, out stringSet))
          {
            // ISSUE: reference to a compiler-generated field
            stringSet.Add(AssetPath.Combine(assembliesCAnonStorey3.args.ProjectDirectory, allSourceFile));
          }
        }
      }
      Dictionary<EditorBuildRules.TargetAssembly, HashSet<string>> dictionary = source.Where<KeyValuePair<EditorBuildRules.TargetAssembly, HashSet<string>>>((Func<KeyValuePair<EditorBuildRules.TargetAssembly, HashSet<string>>, bool>) (e => e.Value.Count > 0)).ToDictionary<KeyValuePair<EditorBuildRules.TargetAssembly, HashSet<string>>, EditorBuildRules.TargetAssembly, HashSet<string>>((Func<KeyValuePair<EditorBuildRules.TargetAssembly, HashSet<string>>, EditorBuildRules.TargetAssembly>) (e => e.Key), (Func<KeyValuePair<EditorBuildRules.TargetAssembly, HashSet<string>>, HashSet<string>>) (e => e.Value));
      // ISSUE: reference to a compiler-generated field
      foreach (EditorBuildRules.TargetAssembly compiledTargetAssembly in assembliesCAnonStorey3.args.NotCompiledTargetAssemblies)
        dictionary.Remove(compiledTargetAssembly);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return EditorBuildRules.ToScriptAssemblies((IDictionary<EditorBuildRules.TargetAssembly, HashSet<string>>) dictionary, assembliesCAnonStorey3.args.Settings, assembliesCAnonStorey3.args.Assemblies, assembliesCAnonStorey3.args.RunUpdaterAssemblies);
    }

    internal static ScriptAssembly[] ToScriptAssemblies(IDictionary<EditorBuildRules.TargetAssembly, HashSet<string>> targetAssemblies, ScriptAssemblySettings settings, EditorBuildRules.CompilationAssemblies assemblies, HashSet<string> runUpdaterAssemblies)
    {
      ScriptAssembly[] scriptAssemblyArray = new ScriptAssembly[targetAssemblies.Count];
      Dictionary<EditorBuildRules.TargetAssembly, ScriptAssembly> dictionary = new Dictionary<EditorBuildRules.TargetAssembly, ScriptAssembly>();
      int index = 0;
      bool buildingForEditor = settings.BuildingForEditor;
      foreach (KeyValuePair<EditorBuildRules.TargetAssembly, HashSet<string>> targetAssembly in (IEnumerable<KeyValuePair<EditorBuildRules.TargetAssembly, HashSet<string>>>) targetAssemblies)
      {
        EditorBuildRules.TargetAssembly key = targetAssembly.Key;
        HashSet<string> source = targetAssembly.Value;
        ScriptAssembly scriptAssembly = new ScriptAssembly();
        scriptAssemblyArray[index] = scriptAssembly;
        dictionary[key] = scriptAssemblyArray[index++];
        scriptAssembly.Flags = key.Flags;
        scriptAssembly.BuildTarget = settings.BuildTarget;
        scriptAssembly.Language = key.Language;
        scriptAssembly.ApiCompatibilityLevel = (key.Flags & AssemblyFlags.EditorOnly) == AssemblyFlags.EditorOnly || buildingForEditor && settings.ApiCompatibilityLevel == ApiCompatibilityLevel.NET_4_6 ? (EditorApplication.scriptingRuntimeVersion != ScriptingRuntimeVersion.Latest ? ApiCompatibilityLevel.NET_2_0 : ApiCompatibilityLevel.NET_4_6) : settings.ApiCompatibilityLevel;
        if (!string.IsNullOrEmpty(settings.FilenameSuffix))
        {
          string withoutExtension = AssetPath.GetAssemblyNameWithoutExtension(key.Filename);
          scriptAssembly.Filename = withoutExtension + settings.FilenameSuffix + ".dll";
        }
        else
          scriptAssembly.Filename = key.Filename;
        if (runUpdaterAssemblies != null && runUpdaterAssemblies.Contains(scriptAssembly.Filename))
          scriptAssembly.RunUpdater = true;
        scriptAssembly.OutputDirectory = settings.OutputDirectory;
        scriptAssembly.Defines = settings.Defines;
        scriptAssembly.Files = source.ToArray<string>();
        Array.Sort<string>(scriptAssembly.Files);
      }
      int num = 0;
      foreach (KeyValuePair<EditorBuildRules.TargetAssembly, HashSet<string>> targetAssembly in (IEnumerable<KeyValuePair<EditorBuildRules.TargetAssembly, HashSet<string>>>) targetAssemblies)
        EditorBuildRules.AddScriptAssemblyReferences(ref scriptAssemblyArray[num++], targetAssembly.Key, settings, assemblies, (IDictionary<EditorBuildRules.TargetAssembly, ScriptAssembly>) dictionary, settings.FilenameSuffix);
      return scriptAssemblyArray;
    }

    private static bool IsPrecompiledAssemblyCompatibleWithScriptAssembly(PrecompiledAssembly compiledAssembly, ScriptAssembly scriptAssembly)
    {
      if (WSAHelpers.UseDotNetCore(scriptAssembly))
        return (compiledAssembly.Flags & AssemblyFlags.UseForDotNet) == AssemblyFlags.UseForDotNet;
      return (compiledAssembly.Flags & AssemblyFlags.UseForMono) == AssemblyFlags.UseForMono;
    }

    internal static void AddScriptAssemblyReferences(ref ScriptAssembly scriptAssembly, EditorBuildRules.TargetAssembly targetAssembly, ScriptAssemblySettings settings, EditorBuildRules.CompilationAssemblies assemblies, IDictionary<EditorBuildRules.TargetAssembly, ScriptAssembly> targetToScriptAssembly, string filenameSuffix)
    {
      List<ScriptAssembly> scriptAssemblyList = new List<ScriptAssembly>();
      List<string> stringList = new List<string>();
      bool buildingForEditor = settings.BuildingForEditor;
      List<string> unityReferences = EditorBuildRules.GetUnityReferences(scriptAssembly, assemblies.UnityAssemblies, settings.CompilationOptions);
      stringList.AddRange((IEnumerable<string>) unityReferences);
      foreach (EditorBuildRules.TargetAssembly reference in targetAssembly.References)
      {
        ScriptAssembly scriptAssembly1;
        if (targetToScriptAssembly.TryGetValue(reference, out scriptAssembly1))
        {
          scriptAssemblyList.Add(scriptAssembly1);
        }
        else
        {
          string path = reference.FullPath(settings.OutputDirectory, filenameSuffix);
          if (File.Exists(path))
            stringList.Add(path);
        }
      }
      if (assemblies.CustomTargetAssemblies != null && (targetAssembly.Type & EditorBuildRules.TargetAssemblyType.Predefined) == EditorBuildRules.TargetAssemblyType.Predefined)
      {
        foreach (EditorBuildRules.TargetAssembly customTargetAssembly in assemblies.CustomTargetAssemblies)
        {
          ScriptAssembly scriptAssembly1;
          if (targetToScriptAssembly.TryGetValue(customTargetAssembly, out scriptAssembly1))
          {
            scriptAssemblyList.Add(scriptAssembly1);
          }
          else
          {
            string path = customTargetAssembly.FullPath(settings.OutputDirectory, filenameSuffix);
            if (File.Exists(path))
              stringList.Add(path);
          }
        }
      }
      List<string> precompiledReferences = EditorBuildRules.GetPrecompiledReferences(scriptAssembly, settings.CompilationOptions, targetAssembly.editorCompatibility, assemblies.PrecompiledAssemblies);
      stringList.AddRange((IEnumerable<string>) precompiledReferences);
      if (buildingForEditor && assemblies.EditorAssemblyReferences != null)
        stringList.AddRange((IEnumerable<string>) assemblies.EditorAssemblyReferences);
      scriptAssembly.ScriptAssemblyReferences = scriptAssemblyList.ToArray();
      scriptAssembly.References = stringList.ToArray();
    }

    public static List<string> GetUnityReferences(ScriptAssembly scriptAssembly, PrecompiledAssembly[] unityAssemblies, EditorScriptCompilationOptions options)
    {
      List<string> stringList = new List<string>();
      bool flag1 = (scriptAssembly.Flags & AssemblyFlags.EditorOnly) == AssemblyFlags.EditorOnly;
      bool flag2 = (options & EditorScriptCompilationOptions.BuildingForEditor) == EditorScriptCompilationOptions.BuildingForEditor;
      if (unityAssemblies != null)
      {
        foreach (PrecompiledAssembly unityAssembly in unityAssemblies)
        {
          bool flag3 = (unityAssembly.Flags & AssemblyFlags.ExcludedForRuntimeCode) == AssemblyFlags.ExcludedForRuntimeCode;
          if (flag2 && !flag3 || flag1)
          {
            if ((unityAssembly.Flags & AssemblyFlags.UseForMono) != AssemblyFlags.None)
              stringList.Add(unityAssembly.Path);
          }
          else if ((unityAssembly.Flags & AssemblyFlags.EditorOnly) != AssemblyFlags.EditorOnly && !flag3 && EditorBuildRules.IsPrecompiledAssemblyCompatibleWithScriptAssembly(unityAssembly, scriptAssembly))
            stringList.Add(unityAssembly.Path);
        }
      }
      return stringList;
    }

    public static List<string> GetPrecompiledReferences(ScriptAssembly scriptAssembly, EditorScriptCompilationOptions options, EditorBuildRules.EditorCompatibility editorCompatibility, PrecompiledAssembly[] precompiledAssemblies)
    {
      List<string> stringList = new List<string>();
      bool flag1 = (options & EditorScriptCompilationOptions.BuildingForEditor) == EditorScriptCompilationOptions.BuildingForEditor;
      bool flag2 = (scriptAssembly.Flags & AssemblyFlags.EditorOnly) == AssemblyFlags.EditorOnly;
      if (precompiledAssemblies != null)
      {
        foreach (PrecompiledAssembly precompiledAssembly in precompiledAssemblies)
        {
          if (((precompiledAssembly.Flags & AssemblyFlags.EditorOnly) != AssemblyFlags.EditorOnly || flag2 || flag1 && editorCompatibility == EditorBuildRules.EditorCompatibility.CompatibleWithEditor) && EditorBuildRules.IsPrecompiledAssemblyCompatibleWithScriptAssembly(precompiledAssembly, scriptAssembly))
            stringList.Add(precompiledAssembly.Path);
        }
      }
      return stringList;
    }

    public static List<string> GetCompiledCustomAssembliesReferences(ScriptAssembly scriptAssembly, EditorBuildRules.TargetAssembly[] customTargetAssemblies, string outputDirectory, string filenameSuffix)
    {
      List<string> stringList = new List<string>();
      if (customTargetAssemblies != null)
      {
        foreach (EditorBuildRules.TargetAssembly customTargetAssembly in customTargetAssemblies)
        {
          string path = customTargetAssembly.FullPath(outputDirectory, filenameSuffix);
          if (File.Exists(path))
            stringList.Add(path);
        }
      }
      return stringList;
    }

    private static bool IsCompatibleWithEditor(BuildTarget buildTarget, EditorScriptCompilationOptions options)
    {
      return (options & EditorScriptCompilationOptions.BuildingForEditor) == EditorScriptCompilationOptions.BuildingForEditor;
    }

    private static bool IsCompatibleWithPlatform(EditorBuildRules.TargetAssembly assembly, ScriptAssemblySettings settings)
    {
      return assembly.IsCompatibleFunc == null || assembly.IsCompatibleFunc(settings.BuildTarget, settings.CompilationOptions);
    }

    internal static EditorBuildRules.TargetAssembly[] CreatePredefinedTargetAssemblies()
    {
      List<EditorBuildRules.TargetAssembly> targetAssemblyList1 = new List<EditorBuildRules.TargetAssembly>();
      List<EditorBuildRules.TargetAssembly> targetAssemblyList2 = new List<EditorBuildRules.TargetAssembly>();
      List<EditorBuildRules.TargetAssembly> targetAssemblyList3 = new List<EditorBuildRules.TargetAssembly>();
      List<EditorBuildRules.TargetAssembly> targetAssemblyList4 = new List<EditorBuildRules.TargetAssembly>();
      List<SupportedLanguage> supportedLanguages = ScriptCompilers.SupportedLanguages;
      List<EditorBuildRules.TargetAssembly> targetAssemblyList5 = new List<EditorBuildRules.TargetAssembly>();
      foreach (SupportedLanguage language1 in supportedLanguages)
      {
        string languageName = language1.GetLanguageName();
        string name1 = "Assembly-" + languageName + "-firstpass.dll";
        SupportedLanguage language2 = language1;
        int num1 = 8;
        int num2 = 1;
        // ISSUE: reference to a compiler-generated field
        if (EditorBuildRules.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EditorBuildRules.\u003C\u003Ef__mg\u0024cache0 = new Func<string, int>(EditorBuildRules.FilterAssemblyInFirstpassFolder);
        }
        // ISSUE: reference to a compiler-generated field
        Func<string, int> fMgCache0 = EditorBuildRules.\u003C\u003Ef__mg\u0024cache0;
        // ISSUE: variable of the null type
        __Null local = null;
        EditorBuildRules.TargetAssembly targetAssembly1 = new EditorBuildRules.TargetAssembly(name1, language2, (AssemblyFlags) num1, (EditorBuildRules.TargetAssemblyType) num2, fMgCache0, (Func<BuildTarget, EditorScriptCompilationOptions, bool>) local);
        EditorBuildRules.TargetAssembly targetAssembly2 = new EditorBuildRules.TargetAssembly("Assembly-" + languageName + ".dll", language1, AssemblyFlags.None, EditorBuildRules.TargetAssemblyType.Predefined);
        string name2 = "Assembly-" + languageName + "-Editor-firstpass.dll";
        SupportedLanguage language3 = language1;
        int num3 = 9;
        int num4 = 1;
        // ISSUE: reference to a compiler-generated field
        if (EditorBuildRules.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EditorBuildRules.\u003C\u003Ef__mg\u0024cache1 = new Func<string, int>(EditorBuildRules.FilterAssemblyInFirstpassEditorFolder);
        }
        // ISSUE: reference to a compiler-generated field
        Func<string, int> fMgCache1 = EditorBuildRules.\u003C\u003Ef__mg\u0024cache1;
        // ISSUE: reference to a compiler-generated field
        if (EditorBuildRules.\u003C\u003Ef__mg\u0024cache2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EditorBuildRules.\u003C\u003Ef__mg\u0024cache2 = new Func<BuildTarget, EditorScriptCompilationOptions, bool>(EditorBuildRules.IsCompatibleWithEditor);
        }
        // ISSUE: reference to a compiler-generated field
        Func<BuildTarget, EditorScriptCompilationOptions, bool> fMgCache2 = EditorBuildRules.\u003C\u003Ef__mg\u0024cache2;
        EditorBuildRules.TargetAssembly targetAssembly3 = new EditorBuildRules.TargetAssembly(name2, language3, (AssemblyFlags) num3, (EditorBuildRules.TargetAssemblyType) num4, fMgCache1, fMgCache2);
        string name3 = "Assembly-" + languageName + "-Editor.dll";
        SupportedLanguage language4 = language1;
        int num5 = 1;
        int num6 = 1;
        // ISSUE: reference to a compiler-generated field
        if (EditorBuildRules.\u003C\u003Ef__mg\u0024cache3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EditorBuildRules.\u003C\u003Ef__mg\u0024cache3 = new Func<string, int>(EditorBuildRules.FilterAssemblyInEditorFolder);
        }
        // ISSUE: reference to a compiler-generated field
        Func<string, int> fMgCache3 = EditorBuildRules.\u003C\u003Ef__mg\u0024cache3;
        // ISSUE: reference to a compiler-generated field
        if (EditorBuildRules.\u003C\u003Ef__mg\u0024cache4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EditorBuildRules.\u003C\u003Ef__mg\u0024cache4 = new Func<BuildTarget, EditorScriptCompilationOptions, bool>(EditorBuildRules.IsCompatibleWithEditor);
        }
        // ISSUE: reference to a compiler-generated field
        Func<BuildTarget, EditorScriptCompilationOptions, bool> fMgCache4 = EditorBuildRules.\u003C\u003Ef__mg\u0024cache4;
        EditorBuildRules.TargetAssembly targetAssembly4 = new EditorBuildRules.TargetAssembly(name3, language4, (AssemblyFlags) num5, (EditorBuildRules.TargetAssemblyType) num6, fMgCache3, fMgCache4);
        targetAssemblyList1.Add(targetAssembly1);
        targetAssemblyList2.Add(targetAssembly2);
        targetAssemblyList3.Add(targetAssembly3);
        targetAssemblyList4.Add(targetAssembly4);
        targetAssemblyList5.Add(targetAssembly1);
        targetAssemblyList5.Add(targetAssembly2);
        targetAssemblyList5.Add(targetAssembly3);
        targetAssemblyList5.Add(targetAssembly4);
      }
      foreach (EditorBuildRules.TargetAssembly targetAssembly in targetAssemblyList2)
        targetAssembly.References.AddRange((IEnumerable<EditorBuildRules.TargetAssembly>) targetAssemblyList1);
      foreach (EditorBuildRules.TargetAssembly targetAssembly in targetAssemblyList3)
        targetAssembly.References.AddRange((IEnumerable<EditorBuildRules.TargetAssembly>) targetAssemblyList1);
      foreach (EditorBuildRules.TargetAssembly targetAssembly in targetAssemblyList4)
      {
        targetAssembly.References.AddRange((IEnumerable<EditorBuildRules.TargetAssembly>) targetAssemblyList1);
        targetAssembly.References.AddRange((IEnumerable<EditorBuildRules.TargetAssembly>) targetAssemblyList2);
        targetAssembly.References.AddRange((IEnumerable<EditorBuildRules.TargetAssembly>) targetAssemblyList3);
      }
      return targetAssemblyList5.ToArray();
    }

    internal static EditorBuildRules.TargetAssembly GetTargetAssembly(string scriptPath, string projectDirectory, EditorBuildRules.TargetAssembly[] customTargetAssemblies)
    {
      return EditorBuildRules.GetCustomTargetAssembly(scriptPath, projectDirectory, customTargetAssemblies) ?? EditorBuildRules.GetPredefinedTargetAssembly(scriptPath);
    }

    internal static EditorBuildRules.TargetAssembly GetPredefinedTargetAssembly(string scriptPath)
    {
      EditorBuildRules.TargetAssembly targetAssembly = (EditorBuildRules.TargetAssembly) null;
      string lower = AssetPath.GetExtension(scriptPath).Substring(1).ToLower();
      string str = "/" + scriptPath.ToLower();
      int num1 = -1;
      foreach (EditorBuildRules.TargetAssembly predefinedTargetAssembly in EditorBuildRules.predefinedTargetAssemblies)
      {
        if (!(lower != predefinedTargetAssembly.Language.GetExtensionICanCompile()))
        {
          Func<string, int> pathFilter = predefinedTargetAssembly.PathFilter;
          int num2 = pathFilter != null ? pathFilter(str) : 0;
          if (num2 > num1)
          {
            targetAssembly = predefinedTargetAssembly;
            num1 = num2;
          }
        }
      }
      return targetAssembly;
    }

    internal static EditorBuildRules.TargetAssembly GetCustomTargetAssembly(string scriptPath, string projectDirectory, EditorBuildRules.TargetAssembly[] customTargetAssemblies)
    {
      if (customTargetAssemblies == null)
        return (EditorBuildRules.TargetAssembly) null;
      int num1 = -1;
      EditorBuildRules.TargetAssembly targetAssembly = (EditorBuildRules.TargetAssembly) null;
      string str = !AssetPath.IsPathRooted(scriptPath) ? AssetPath.Combine(projectDirectory, scriptPath).ToLower() : AssetPath.GetFullPath(scriptPath).ToLower();
      foreach (EditorBuildRules.TargetAssembly customTargetAssembly in customTargetAssemblies)
      {
        int num2 = customTargetAssembly.PathFilter(str);
        if (num2 > num1)
        {
          targetAssembly = customTargetAssembly;
          num1 = num2;
        }
      }
      return targetAssembly;
    }

    private static int FilterAssemblyInFirstpassFolder(string pathName)
    {
      int num1 = EditorBuildRules.FilterAssemblyPathBeginsWith(pathName, "/assets/plugins/");
      if (num1 >= 0)
        return num1;
      int num2 = EditorBuildRules.FilterAssemblyPathBeginsWith(pathName, "/assets/standard assets/");
      if (num2 >= 0)
        return num2;
      int num3 = EditorBuildRules.FilterAssemblyPathBeginsWith(pathName, "/assets/pro standard assets/");
      if (num3 >= 0)
        return num3;
      int num4 = EditorBuildRules.FilterAssemblyPathBeginsWith(pathName, "/assets/iphone standard assets/");
      if (num4 >= 0)
        return num4;
      return -1;
    }

    private static int FilterAssemblyInFirstpassEditorFolder(string pathName)
    {
      if (EditorBuildRules.FilterAssemblyInFirstpassFolder(pathName) == -1)
        return -1;
      return EditorBuildRules.FilterAssemblyInEditorFolder(pathName);
    }

    private static int FilterAssemblyInEditorFolder(string pathName)
    {
      int num = pathName.IndexOf("/editor/");
      if (num == -1)
        return -1;
      return num + "/editor/".Length;
    }

    private static int FilterAssemblyPathBeginsWith(string pathName, string prefix)
    {
      return !pathName.StartsWith(prefix) ? -1 : prefix.Length;
    }

    [System.Flags]
    internal enum TargetAssemblyType
    {
      Undefined = 0,
      Predefined = 1,
      Custom = 2,
    }

    internal enum EditorCompatibility
    {
      NotCompatibleWithEditor,
      CompatibleWithEditor,
    }

    internal class TargetAssembly
    {
      public TargetAssembly()
      {
        this.References = new List<EditorBuildRules.TargetAssembly>();
      }

      public TargetAssembly(string name, SupportedLanguage language, AssemblyFlags flags, EditorBuildRules.TargetAssemblyType type)
        : this(name, language, flags, type, (Func<string, int>) null, (Func<BuildTarget, EditorScriptCompilationOptions, bool>) null)
      {
      }

      public TargetAssembly(string name, SupportedLanguage language, AssemblyFlags flags, EditorBuildRules.TargetAssemblyType type, Func<string, int> pathFilter, Func<BuildTarget, EditorScriptCompilationOptions, bool> compatFunc)
        : this()
      {
        this.Language = language;
        this.Filename = name;
        this.Flags = flags;
        this.PathFilter = pathFilter;
        this.IsCompatibleFunc = compatFunc;
        this.Type = type;
      }

      public string Filename { get; private set; }

      public SupportedLanguage Language { get; set; }

      public AssemblyFlags Flags { get; private set; }

      public Func<string, int> PathFilter { get; private set; }

      public Func<BuildTarget, EditorScriptCompilationOptions, bool> IsCompatibleFunc { get; private set; }

      public List<EditorBuildRules.TargetAssembly> References { get; private set; }

      public EditorBuildRules.TargetAssemblyType Type { get; private set; }

      public string FilenameWithSuffix(string filenameSuffix)
      {
        if (!string.IsNullOrEmpty(filenameSuffix))
          return this.Filename.Replace(".dll", filenameSuffix + ".dll");
        return this.Filename;
      }

      public string FullPath(string outputDirectory, string filenameSuffix)
      {
        return AssetPath.Combine(outputDirectory, this.FilenameWithSuffix(filenameSuffix));
      }

      public EditorBuildRules.EditorCompatibility editorCompatibility
      {
        get
        {
          return this.IsCompatibleFunc != null && !this.IsCompatibleFunc(BuildTarget.NoTarget, EditorScriptCompilationOptions.BuildingForEditor) ? EditorBuildRules.EditorCompatibility.NotCompatibleWithEditor : EditorBuildRules.EditorCompatibility.CompatibleWithEditor;
        }
      }
    }

    public class CompilationAssemblies
    {
      public PrecompiledAssembly[] UnityAssemblies { get; set; }

      public PrecompiledAssembly[] PrecompiledAssemblies { get; set; }

      public EditorBuildRules.TargetAssembly[] CustomTargetAssemblies { get; set; }

      public string[] EditorAssemblyReferences { get; set; }
    }

    public class GenerateChangedScriptAssembliesArgs
    {
      public GenerateChangedScriptAssembliesArgs()
      {
        this.NotCompiledTargetAssemblies = new HashSet<EditorBuildRules.TargetAssembly>();
      }

      public IEnumerable<string> AllSourceFiles { get; set; }

      public IEnumerable<string> DirtySourceFiles { get; set; }

      public string ProjectDirectory { get; set; }

      public ScriptAssemblySettings Settings { get; set; }

      public EditorBuildRules.CompilationAssemblies Assemblies { get; set; }

      public HashSet<string> RunUpdaterAssemblies { get; set; }

      public HashSet<EditorBuildRules.TargetAssembly> NotCompiledTargetAssemblies { get; set; }
    }
  }
}
