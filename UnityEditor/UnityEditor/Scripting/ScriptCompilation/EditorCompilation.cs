// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilation.EditorCompilation
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Compilation;
using UnityEditor.Modules;
using UnityEditor.Scripting.Compilers;
using UnityEditor.Scripting.Serialization;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Scripting.ScriptCompilation
{
  internal class EditorCompilation
  {
    private string projectDirectory = string.Empty;
    private string assemblySuffix = string.Empty;
    private HashSet<string> allScripts = new HashSet<string>();
    private HashSet<string> dirtyScripts = new HashSet<string>();
    private HashSet<string> runScriptUpdaterAssemblies = new HashSet<string>();
    private EditorCompilation.CompilationSetupErrorFlags setupErrorFlags = EditorCompilation.CompilationSetupErrorFlags.none;
    private List<AssemblyBuilder> assemblyBuilders = new List<AssemblyBuilder>();
    private static readonly string EditorTempPath = "Temp";
    private bool areAllScriptsDirty;
    private PrecompiledAssembly[] precompiledAssemblies;
    private CustomScriptAssembly[] customScriptAssemblies;
    private CustomScriptAssembly[] packageCustomScriptAssemblies;
    private EditorBuildRules.TargetAssembly[] customTargetAssemblies;
    private PrecompiledAssembly[] unityAssemblies;
    private CompilationTask compilationTask;
    private string outputDirectory;
    public Action<EditorCompilation.CompilationSetupErrorFlags> setupErrorFlagsChanged;

    public event Action<string> assemblyCompilationStarted;

    public event Action<string, UnityEditor.Compilation.CompilerMessage[]> assemblyCompilationFinished;

    internal string GetAssemblyTimestampPath(string editorAssemblyPath)
    {
      return AssetPath.Combine(editorAssemblyPath, "BuiltinAssemblies.stamp");
    }

    internal void SetProjectDirectory(string projectDirectory)
    {
      this.projectDirectory = projectDirectory;
    }

    internal void SetAssemblySuffix(string assemblySuffix)
    {
      this.assemblySuffix = assemblySuffix;
    }

    public void SetAllScripts(string[] allScripts)
    {
      this.allScripts = new HashSet<string>((IEnumerable<string>) allScripts);
      foreach (string dirtyScript in this.dirtyScripts)
        this.allScripts.Add(dirtyScript);
    }

    public bool IsExtensionSupportedByCompiler(string extension)
    {
      return ScriptCompilers.SupportedLanguages.Count<SupportedLanguage>((Func<SupportedLanguage, bool>) (l => l.GetExtensionICanCompile() == extension)) > 0;
    }

    public void DirtyAllScripts()
    {
      this.areAllScriptsDirty = true;
    }

    public void DirtyScript(string path)
    {
      this.allScripts.Add(path);
      this.dirtyScripts.Add(path);
    }

    public void RunScriptUpdaterOnAssembly(string assemblyFilename)
    {
      this.runScriptUpdaterAssemblies.Add(assemblyFilename);
    }

    public void SetAllUnityAssemblies(PrecompiledAssembly[] unityAssemblies)
    {
      this.unityAssemblies = unityAssemblies;
    }

    public void SetCompileScriptsOutputDirectory(string directory)
    {
      this.outputDirectory = directory;
    }

    public string GetCompileScriptsOutputDirectory()
    {
      if (string.IsNullOrEmpty(this.outputDirectory))
        throw new Exception("Must set an output directory through SetCompileScriptsOutputDirectory before compiling");
      return this.outputDirectory;
    }

    public void SetCompilationSetupErrorFlags(EditorCompilation.CompilationSetupErrorFlags flags)
    {
      EditorCompilation.CompilationSetupErrorFlags compilationSetupErrorFlags = this.setupErrorFlags | flags;
      if (compilationSetupErrorFlags == this.setupErrorFlags)
        return;
      this.setupErrorFlags = compilationSetupErrorFlags;
      if (this.setupErrorFlagsChanged != null)
        this.setupErrorFlagsChanged(this.setupErrorFlags);
    }

    public void ClearCompilationSetupErrorFlags(EditorCompilation.CompilationSetupErrorFlags flags)
    {
      EditorCompilation.CompilationSetupErrorFlags compilationSetupErrorFlags = this.setupErrorFlags & ~flags;
      if (compilationSetupErrorFlags == this.setupErrorFlags)
        return;
      this.setupErrorFlags = compilationSetupErrorFlags;
      if (this.setupErrorFlagsChanged != null)
        this.setupErrorFlagsChanged(this.setupErrorFlags);
    }

    public bool HaveSetupErrors()
    {
      return this.setupErrorFlags != EditorCompilation.CompilationSetupErrorFlags.none;
    }

    public void SetAllPrecompiledAssemblies(PrecompiledAssembly[] precompiledAssemblies)
    {
      this.precompiledAssemblies = precompiledAssemblies;
    }

    public PrecompiledAssembly[] GetAllPrecompiledAssemblies()
    {
      return this.precompiledAssemblies;
    }

    public EditorCompilation.TargetAssemblyInfo[] GetAllCompiledAndResolvedCustomTargetAssemblies()
    {
      if (this.customTargetAssemblies == null)
        return new EditorCompilation.TargetAssemblyInfo[0];
      Dictionary<EditorBuildRules.TargetAssembly, string> dictionary = new Dictionary<EditorBuildRules.TargetAssembly, string>();
      foreach (EditorBuildRules.TargetAssembly customTargetAssembly in this.customTargetAssemblies)
      {
        string path = customTargetAssembly.FullPath(this.outputDirectory, this.assemblySuffix);
        if (File.Exists(path))
          dictionary.Add(customTargetAssembly, path);
      }
      bool flag;
      do
      {
        flag = false;
        if (dictionary.Count > 0)
        {
          foreach (EditorBuildRules.TargetAssembly customTargetAssembly in this.customTargetAssemblies)
          {
            if (dictionary.ContainsKey(customTargetAssembly))
            {
              foreach (EditorBuildRules.TargetAssembly reference in customTargetAssembly.References)
              {
                if (!dictionary.ContainsKey(reference))
                {
                  dictionary.Remove(customTargetAssembly);
                  flag = true;
                  break;
                }
              }
            }
          }
        }
      }
      while (flag);
      int count = dictionary.Count;
      EditorCompilation.TargetAssemblyInfo[] targetAssemblyInfoArray = new EditorCompilation.TargetAssemblyInfo[dictionary.Count];
      int num = 0;
      foreach (KeyValuePair<EditorBuildRules.TargetAssembly, string> keyValuePair in dictionary)
      {
        EditorBuildRules.TargetAssembly key = keyValuePair.Key;
        targetAssemblyInfoArray[num++] = this.ToTargetAssemblyInfo(key);
      }
      return targetAssemblyInfoArray;
    }

    private static CustomScriptAssembly LoadCustomScriptAssemblyFromJson(string path)
    {
      string json = File.ReadAllText(path);
      try
      {
        CustomScriptAssemblyData customScriptAssemblyData = CustomScriptAssemblyData.FromJson(json);
        return CustomScriptAssembly.FromCustomScriptAssemblyData(path, customScriptAssemblyData);
      }
      catch (Exception ex)
      {
        throw new AssemblyDefinitionException(ex.Message, new string[1]{ path });
      }
    }

    private string[] CustomTargetAssembliesToFilePaths(IEnumerable<EditorBuildRules.TargetAssembly> targetAssemblies)
    {
      return targetAssemblies.Select<EditorBuildRules.TargetAssembly, CustomScriptAssembly>((Func<EditorBuildRules.TargetAssembly, CustomScriptAssembly>) (a => this.FindCustomTargetAssemblyFromTargetAssembly(a))).Select<CustomScriptAssembly, string>((Func<CustomScriptAssembly, string>) (a => a.FilePath)).ToArray<string>();
    }

    private string CustomTargetAssembliesToFilePaths(EditorBuildRules.TargetAssembly targetAssembly)
    {
      return this.FindCustomTargetAssemblyFromTargetAssembly(targetAssembly).FilePath;
    }

    private void CheckCyclicAssemblyReferencesDFS(EditorBuildRules.TargetAssembly visitAssembly, HashSet<EditorBuildRules.TargetAssembly> visited)
    {
      if (visited.Contains(visitAssembly))
        throw new AssemblyDefinitionException("Assembly with cyclic references detected", this.CustomTargetAssembliesToFilePaths((IEnumerable<EditorBuildRules.TargetAssembly>) visited));
      visited.Add(visitAssembly);
      foreach (EditorBuildRules.TargetAssembly reference in visitAssembly.References)
      {
        if (reference.Filename == visitAssembly.Filename)
          throw new AssemblyDefinitionException("Assembly contains a references to itself", new string[1]{ this.CustomTargetAssembliesToFilePaths(visitAssembly) });
        this.CheckCyclicAssemblyReferencesDFS(reference, visited);
      }
      visited.Remove(visitAssembly);
    }

    private void CheckCyclicAssemblyReferences()
    {
      if (this.customTargetAssemblies == null || this.customTargetAssemblies.Length < 1)
        return;
      HashSet<EditorBuildRules.TargetAssembly> visited = new HashSet<EditorBuildRules.TargetAssembly>();
      try
      {
        foreach (EditorBuildRules.TargetAssembly customTargetAssembly in this.customTargetAssemblies)
          this.CheckCyclicAssemblyReferencesDFS(customTargetAssembly, visited);
      }
      catch (Exception ex)
      {
        this.SetCompilationSetupErrorFlags(EditorCompilation.CompilationSetupErrorFlags.cyclicReferences);
        throw ex;
      }
    }

    private void UpdateCustomTargetAssemblies()
    {
      List<CustomScriptAssembly> source = new List<CustomScriptAssembly>();
      if (this.customScriptAssemblies != null)
        source.AddRange((IEnumerable<CustomScriptAssembly>) this.customScriptAssemblies);
      if (this.packageCustomScriptAssemblies != null)
      {
        if (this.customScriptAssemblies == null)
        {
          source.AddRange(((IEnumerable<CustomScriptAssembly>) this.packageCustomScriptAssemblies).Select<CustomScriptAssembly, CustomScriptAssembly>((Func<CustomScriptAssembly, CustomScriptAssembly>) (a => CustomScriptAssembly.Create(a.Name, a.FilePath))));
        }
        else
        {
          foreach (CustomScriptAssembly customScriptAssembly in this.packageCustomScriptAssemblies)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            EditorCompilation.\u003CUpdateCustomTargetAssemblies\u003Ec__AnonStorey1 assembliesCAnonStorey1 = new EditorCompilation.\u003CUpdateCustomTargetAssemblies\u003Ec__AnonStorey1();
            // ISSUE: reference to a compiler-generated field
            assembliesCAnonStorey1.pathPrefix = customScriptAssembly.PathPrefix.ToLower();
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            if (!((IEnumerable<CustomScriptAssembly>) this.customScriptAssemblies).Any<CustomScriptAssembly>(new Func<CustomScriptAssembly, bool>(assembliesCAnonStorey1.\u003C\u003Em__0)) && !((IEnumerable<CustomScriptAssembly>) this.customScriptAssemblies).Any<CustomScriptAssembly>(new Func<CustomScriptAssembly, bool>(assembliesCAnonStorey1.\u003C\u003Em__1)))
              source.Add(CustomScriptAssembly.Create(customScriptAssembly.Name, customScriptAssembly.FilePath));
          }
        }
      }
      foreach (CustomScriptAssembly customScriptAssembly in source)
      {
        try
        {
          foreach (string reference in customScriptAssembly.References)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            EditorCompilation.\u003CUpdateCustomTargetAssemblies\u003Ec__AnonStorey2 assembliesCAnonStorey2 = new EditorCompilation.\u003CUpdateCustomTargetAssemblies\u003Ec__AnonStorey2();
            // ISSUE: reference to a compiler-generated field
            assembliesCAnonStorey2.reference = reference;
            // ISSUE: reference to a compiler-generated method
            if (!source.Any<CustomScriptAssembly>(new Func<CustomScriptAssembly, bool>(assembliesCAnonStorey2.\u003C\u003Em__0)))
            {
              // ISSUE: reference to a compiler-generated field
              throw new AssemblyDefinitionException(string.Format("Assembly has reference to non-existent assembly '{0}'", (object) assembliesCAnonStorey2.reference), new string[1]{ customScriptAssembly.FilePath });
            }
          }
        }
        catch (Exception ex)
        {
          this.SetCompilationSetupErrorFlags(EditorCompilation.CompilationSetupErrorFlags.cyclicReferences);
          throw ex;
        }
      }
      this.customTargetAssemblies = EditorBuildRules.CreateTargetAssemblies((IEnumerable<CustomScriptAssembly>) source);
      this.ClearCompilationSetupErrorFlags(EditorCompilation.CompilationSetupErrorFlags.cyclicReferences);
    }

    public void SetAllCustomScriptAssemblyJsons(string[] paths)
    {
      List<CustomScriptAssembly> source1 = new List<CustomScriptAssembly>();
      this.ClearCompilationSetupErrorFlags(EditorCompilation.CompilationSetupErrorFlags.cyclicReferences);
      foreach (string path1 in paths)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EditorCompilation.\u003CSetAllCustomScriptAssemblyJsons\u003Ec__AnonStorey3 jsonsCAnonStorey3 = new EditorCompilation.\u003CSetAllCustomScriptAssemblyJsons\u003Ec__AnonStorey3();
        string path2 = !AssetPath.IsPathRooted(path1) ? AssetPath.Combine(this.projectDirectory, path1) : AssetPath.GetFullPath(path1);
        // ISSUE: reference to a compiler-generated field
        jsonsCAnonStorey3.loadedCustomScriptAssembly = (CustomScriptAssembly) null;
        try
        {
          // ISSUE: reference to a compiler-generated field
          jsonsCAnonStorey3.loadedCustomScriptAssembly = EditorCompilation.LoadCustomScriptAssemblyFromJson(path2);
          // ISSUE: reference to a compiler-generated method
          IEnumerable<CustomScriptAssembly> source2 = source1.Where<CustomScriptAssembly>(new Func<CustomScriptAssembly, bool>(jsonsCAnonStorey3.\u003C\u003Em__0));
          if (source2.Any<CustomScriptAssembly>())
          {
            List<string> stringList = new List<string>();
            // ISSUE: reference to a compiler-generated field
            stringList.Add(jsonsCAnonStorey3.loadedCustomScriptAssembly.FilePath);
            stringList.AddRange(source2.Select<CustomScriptAssembly, string>((Func<CustomScriptAssembly, string>) (a => a.FilePath)));
            // ISSUE: reference to a compiler-generated field
            throw new AssemblyDefinitionException(string.Format("Assembly with name '{0}' already exists", (object) jsonsCAnonStorey3.loadedCustomScriptAssembly.Name), stringList.ToArray());
          }
          // ISSUE: reference to a compiler-generated method
          IEnumerable<CustomScriptAssembly> source3 = source1.Where<CustomScriptAssembly>(new Func<CustomScriptAssembly, bool>(jsonsCAnonStorey3.\u003C\u003Em__1));
          if (source3.Any<CustomScriptAssembly>())
          {
            List<string> stringList = new List<string>();
            // ISSUE: reference to a compiler-generated field
            stringList.Add(jsonsCAnonStorey3.loadedCustomScriptAssembly.FilePath);
            stringList.AddRange(source3.Select<CustomScriptAssembly, string>((Func<CustomScriptAssembly, string>) (a => a.FilePath)));
            // ISSUE: reference to a compiler-generated field
            throw new AssemblyDefinitionException(string.Format("Folder '{0}' contains multiple assembly definition files", (object) jsonsCAnonStorey3.loadedCustomScriptAssembly.PathPrefix), stringList.ToArray());
          }
          // ISSUE: reference to a compiler-generated field
          if (jsonsCAnonStorey3.loadedCustomScriptAssembly.References == null)
          {
            // ISSUE: reference to a compiler-generated field
            jsonsCAnonStorey3.loadedCustomScriptAssembly.References = new string[0];
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (jsonsCAnonStorey3.loadedCustomScriptAssembly.References.Length != ((IEnumerable<string>) jsonsCAnonStorey3.loadedCustomScriptAssembly.References).Distinct<string>().Count<string>())
          {
            // ISSUE: reference to a compiler-generated field
            throw new AssemblyDefinitionException("Assembly has duplicate references", new string[1]{ jsonsCAnonStorey3.loadedCustomScriptAssembly.FilePath });
          }
        }
        catch (Exception ex)
        {
          this.SetCompilationSetupErrorFlags(EditorCompilation.CompilationSetupErrorFlags.cyclicReferences);
          throw ex;
        }
        // ISSUE: reference to a compiler-generated field
        source1.Add(jsonsCAnonStorey3.loadedCustomScriptAssembly);
      }
      this.customScriptAssemblies = source1.ToArray();
      this.UpdateCustomTargetAssemblies();
    }

    public void SetAllPackageAssemblies(EditorCompilation.PackageAssembly[] packageAssemblies)
    {
      this.packageCustomScriptAssemblies = ((IEnumerable<EditorCompilation.PackageAssembly>) packageAssemblies).Select<EditorCompilation.PackageAssembly, CustomScriptAssembly>((Func<EditorCompilation.PackageAssembly, CustomScriptAssembly>) (a => CustomScriptAssembly.Create(a.Name, AssetPath.GetFullPath(a.DirectoryPath)))).ToArray<CustomScriptAssembly>();
      this.UpdateCustomTargetAssemblies();
    }

    public void DeleteUnusedAssemblies()
    {
      string str = AssetPath.Combine(AssetPath.GetDirectoryName(Application.dataPath), this.GetCompileScriptsOutputDirectory());
      if (!Directory.Exists(str))
        return;
      List<string> list = ((IEnumerable<string>) Directory.GetFiles(str)).Select<string, string>((Func<string, string>) (f => AssetPath.ReplaceSeparators(f))).ToList<string>();
      string assemblyTimestampPath = this.GetAssemblyTimestampPath(this.GetCompileScriptsOutputDirectory());
      list.Remove(AssetPath.Combine(AssetPath.GetDirectoryName(Application.dataPath), assemblyTimestampPath));
      foreach (ScriptAssembly allScriptAssembly in this.GetAllScriptAssemblies(EditorScriptCompilationOptions.BuildingForEditor))
      {
        if (allScriptAssembly.Files.Length > 0)
        {
          string dllPath = AssetPath.Combine(str, allScriptAssembly.Filename);
          list.Remove(dllPath);
          list.Remove(EditorCompilation.MDBPath(dllPath));
          list.Remove(EditorCompilation.PDBPath(dllPath));
        }
      }
      foreach (string path in list)
        EditorCompilation.DeleteFile(path, EditorCompilation.DeleteFileOptions.LogError);
    }

    public void CleanScriptAssemblies()
    {
      string path = AssetPath.Combine(AssetPath.GetDirectoryName(Application.dataPath), this.GetCompileScriptsOutputDirectory());
      if (!Directory.Exists(path))
        return;
      foreach (string file in Directory.GetFiles(path))
        EditorCompilation.DeleteFile(file, EditorCompilation.DeleteFileOptions.LogError);
    }

    private static void DeleteFile(string path, EditorCompilation.DeleteFileOptions fileOptions = EditorCompilation.DeleteFileOptions.LogError)
    {
      try
      {
        File.Delete(path);
      }
      catch (Exception ex)
      {
        if (fileOptions != EditorCompilation.DeleteFileOptions.LogError)
          return;
        Debug.LogErrorFormat("Could not delete file '{0}'\n", (object) path);
      }
    }

    private static bool MoveOrReplaceFile(string sourcePath, string destinationPath)
    {
      bool flag = true;
      try
      {
        File.Move(sourcePath, destinationPath);
      }
      catch (IOException ex)
      {
        flag = false;
      }
      if (!flag)
      {
        flag = true;
        string str = destinationPath + ".bak";
        EditorCompilation.DeleteFile(str, EditorCompilation.DeleteFileOptions.NoLogError);
        try
        {
          File.Replace(sourcePath, destinationPath, str, true);
        }
        catch (IOException ex)
        {
          flag = false;
        }
        EditorCompilation.DeleteFile(str, EditorCompilation.DeleteFileOptions.NoLogError);
      }
      return flag;
    }

    private static string PDBPath(string dllPath)
    {
      return dllPath.Replace(".dll", ".pdb");
    }

    private static string MDBPath(string dllPath)
    {
      return dllPath + ".mdb";
    }

    private static bool CopyAssembly(string sourcePath, string destinationPath)
    {
      if (!EditorCompilation.MoveOrReplaceFile(sourcePath, destinationPath))
        return false;
      string str1 = EditorCompilation.MDBPath(sourcePath);
      string str2 = EditorCompilation.MDBPath(destinationPath);
      if (File.Exists(str1))
        EditorCompilation.MoveOrReplaceFile(str1, str2);
      else if (File.Exists(str2))
        EditorCompilation.DeleteFile(str2, EditorCompilation.DeleteFileOptions.LogError);
      string str3 = EditorCompilation.PDBPath(sourcePath);
      string str4 = EditorCompilation.PDBPath(destinationPath);
      if (File.Exists(str3))
        EditorCompilation.MoveOrReplaceFile(str3, str4);
      else if (File.Exists(str4))
        EditorCompilation.DeleteFile(str4, EditorCompilation.DeleteFileOptions.LogError);
      return true;
    }

    public CustomScriptAssembly FindCustomScriptAssemblyFromAssemblyName(string assemblyName)
    {
      List<CustomScriptAssembly> source = new List<CustomScriptAssembly>();
      if (this.customScriptAssemblies != null)
        source.AddRange((IEnumerable<CustomScriptAssembly>) this.customScriptAssemblies);
      if (this.packageCustomScriptAssemblies != null)
        source.AddRange((IEnumerable<CustomScriptAssembly>) this.packageCustomScriptAssemblies);
      return source.Single<CustomScriptAssembly>((Func<CustomScriptAssembly, bool>) (a => a.Name == AssetPath.GetAssemblyNameWithoutExtension(assemblyName)));
    }

    internal CustomScriptAssembly FindCustomScriptAssemblyFromScriptPath(string scriptPath)
    {
      EditorBuildRules.TargetAssembly customTargetAssembly = EditorBuildRules.GetCustomTargetAssembly(scriptPath, this.projectDirectory, this.customTargetAssemblies);
      return customTargetAssembly == null ? (CustomScriptAssembly) null : this.FindCustomScriptAssemblyFromAssemblyName(customTargetAssembly.Filename);
    }

    internal CustomScriptAssembly FindCustomTargetAssemblyFromTargetAssembly(EditorBuildRules.TargetAssembly assembly)
    {
      return this.FindCustomScriptAssemblyFromAssemblyName(AssetPath.GetAssemblyNameWithoutExtension(assembly.Filename));
    }

    public bool CompileScripts(EditorScriptCompilationOptions options, BuildTargetGroup platformGroup, BuildTarget platform)
    {
      ScriptAssemblySettings assemblySettings = this.CreateScriptAssemblySettings(platformGroup, platform, options);
      EditorBuildRules.TargetAssembly[] notCompiledTargetAssemblies = (EditorBuildRules.TargetAssembly[]) null;
      bool flag = this.CompileScripts(assemblySettings, EditorCompilation.EditorTempPath, options, ref notCompiledTargetAssemblies);
      if (notCompiledTargetAssemblies != null)
      {
        foreach (EditorBuildRules.TargetAssembly targetAssembly in notCompiledTargetAssemblies)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          EditorCompilation.\u003CCompileScripts\u003Ec__AnonStorey5 scriptsCAnonStorey5 = new EditorCompilation.\u003CCompileScripts\u003Ec__AnonStorey5();
          // ISSUE: reference to a compiler-generated field
          scriptsCAnonStorey5.targetAssembly = targetAssembly;
          // ISSUE: reference to a compiler-generated method
          string str = ((IEnumerable<CustomScriptAssembly>) this.customScriptAssemblies).Single<CustomScriptAssembly>(new Func<CustomScriptAssembly, bool>(scriptsCAnonStorey5.\u003C\u003Em__0)).FilePath;
          if (str.StartsWith(this.projectDirectory))
            str = str.Substring(this.projectDirectory.Length);
          // ISSUE: reference to a compiler-generated field
          Debug.LogWarning((object) string.Format("Script assembly '{0}' has not been compiled. Folder containing assembly definition file '{1}' contains script files for different script languages. Folder must only contain script files for one script language.", (object) scriptsCAnonStorey5.targetAssembly.Filename, (object) str));
        }
      }
      return flag;
    }

    internal bool CompileScripts(ScriptAssemblySettings scriptAssemblySettings, string tempBuildDirectory, EditorScriptCompilationOptions options, ref EditorBuildRules.TargetAssembly[] notCompiledTargetAssemblies)
    {
      this.StopAllCompilation();
      if (this.setupErrorFlags != EditorCompilation.CompilationSetupErrorFlags.none)
        return false;
      this.CheckCyclicAssemblyReferences();
      this.DeleteUnusedAssemblies();
      if (!Directory.Exists(scriptAssemblySettings.OutputDirectory))
        Directory.CreateDirectory(scriptAssemblySettings.OutputDirectory);
      if (!Directory.Exists(tempBuildDirectory))
        Directory.CreateDirectory(tempBuildDirectory);
      IEnumerable<string> source = !this.areAllScriptsDirty ? (IEnumerable<string>) this.dirtyScripts.ToArray<string>() : (IEnumerable<string>) this.allScripts.ToArray<string>();
      this.areAllScriptsDirty = false;
      this.dirtyScripts.Clear();
      if (!source.Any<string>() && this.runScriptUpdaterAssemblies.Count == 0)
        return false;
      EditorBuildRules.CompilationAssemblies compilationAssemblies = new EditorBuildRules.CompilationAssemblies() { UnityAssemblies = this.unityAssemblies, PrecompiledAssemblies = this.precompiledAssemblies, CustomTargetAssemblies = this.customTargetAssemblies, EditorAssemblyReferences = ModuleUtils.GetAdditionalReferencesForUserScripts() };
      EditorBuildRules.GenerateChangedScriptAssembliesArgs args = new EditorBuildRules.GenerateChangedScriptAssembliesArgs() { AllSourceFiles = (IEnumerable<string>) this.allScripts, DirtySourceFiles = source, ProjectDirectory = this.projectDirectory, Settings = scriptAssemblySettings, Assemblies = compilationAssemblies, RunUpdaterAssemblies = this.runScriptUpdaterAssemblies };
      ScriptAssembly[] scriptAssemblies = EditorBuildRules.GenerateChangedScriptAssemblies(args);
      notCompiledTargetAssemblies = args.NotCompiledTargetAssemblies.ToArray<EditorBuildRules.TargetAssembly>();
      if (!((IEnumerable<ScriptAssembly>) scriptAssemblies).Any<ScriptAssembly>())
        return false;
      this.compilationTask = new CompilationTask(scriptAssemblies, tempBuildDirectory, options, SystemInfo.processorCount);
      this.compilationTask.OnCompilationStarted += (Action<ScriptAssembly, int>) ((assembly, phase) =>
      {
        string assemblyOutputPath = AssetPath.Combine(scriptAssemblySettings.OutputDirectory, assembly.Filename);
        Console.WriteLine("- Starting compile {0}", (object) assemblyOutputPath);
        this.InvokeAssemblyCompilationStarted(assemblyOutputPath);
      });
      this.compilationTask.OnCompilationFinished += (Action<ScriptAssembly, List<UnityEditor.Scripting.Compilers.CompilerMessage>>) ((assembly, messages) =>
      {
        string assemblyOutputPath = AssetPath.Combine(scriptAssemblySettings.OutputDirectory, assembly.Filename);
        Console.WriteLine("- Finished compile {0}", (object) assemblyOutputPath);
        if (this.runScriptUpdaterAssemblies.Contains(assembly.Filename))
          this.runScriptUpdaterAssemblies.Remove(assembly.Filename);
        if (messages.Any<UnityEditor.Scripting.Compilers.CompilerMessage>((Func<UnityEditor.Scripting.Compilers.CompilerMessage, bool>) (m => m.type == UnityEditor.Scripting.Compilers.CompilerMessageType.Error)))
        {
          this.InvokeAssemblyCompilationFinished(assemblyOutputPath, messages);
        }
        else
        {
          bool buildingForEditor = scriptAssemblySettings.BuildingForEditor;
          string moduleAssemblyPath = InternalEditorUtility.GetEngineCoreModuleAssemblyPath();
          string unityUNet = EditorApplication.applicationContentsPath + "/UnityExtensions/Unity/Networking/UnityEngine.Networking.dll";
          if (!Weaver.WeaveUnetFromEditor(assembly, tempBuildDirectory, tempBuildDirectory, moduleAssemblyPath, unityUNet, buildingForEditor))
          {
            messages.Add(new UnityEditor.Scripting.Compilers.CompilerMessage()
            {
              message = "UNet Weaver failed",
              type = UnityEditor.Scripting.Compilers.CompilerMessageType.Error,
              file = assembly.FullPath,
              line = -1,
              column = -1
            });
            this.StopAllCompilation();
            this.InvokeAssemblyCompilationFinished(assemblyOutputPath, messages);
          }
          else if (!EditorCompilation.CopyAssembly(AssetPath.Combine(tempBuildDirectory, assembly.Filename), assembly.FullPath))
          {
            messages.Add(new UnityEditor.Scripting.Compilers.CompilerMessage()
            {
              message = string.Format("Copying assembly from directory {0} to {1} failed", (object) tempBuildDirectory, (object) assembly.OutputDirectory),
              type = UnityEditor.Scripting.Compilers.CompilerMessageType.Error,
              file = assembly.FullPath,
              line = -1,
              column = -1
            });
            this.StopAllCompilation();
            this.InvokeAssemblyCompilationFinished(assemblyOutputPath, messages);
          }
          else
            this.InvokeAssemblyCompilationFinished(assemblyOutputPath, messages);
        }
      });
      this.compilationTask.Poll();
      return true;
    }

    public void InvokeAssemblyCompilationStarted(string assemblyOutputPath)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.assemblyCompilationStarted == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.assemblyCompilationStarted(assemblyOutputPath);
    }

    public void InvokeAssemblyCompilationFinished(string assemblyOutputPath, List<UnityEditor.Scripting.Compilers.CompilerMessage> messages)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.assemblyCompilationFinished == null)
        return;
      UnityEditor.Compilation.CompilerMessage[] compilerMessageArray = EditorCompilation.ConvertCompilerMessages(messages);
      // ISSUE: reference to a compiler-generated field
      this.assemblyCompilationFinished(assemblyOutputPath, compilerMessageArray);
    }

    public bool DoesProjectFolderHaveAnyDirtyScripts()
    {
      return this.areAllScriptsDirty && this.allScripts.Count > 0 || this.dirtyScripts.Count > 0;
    }

    public bool DoesProjectFolderHaveAnyScripts()
    {
      return this.allScripts != null && this.allScripts.Count > 0;
    }

    private ScriptAssemblySettings CreateScriptAssemblySettings(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, EditorScriptCompilationOptions options)
    {
      string[] compilationDefines = InternalEditorUtility.GetCompilationDefines(options, buildTargetGroup, buildTarget);
      return new ScriptAssemblySettings() { BuildTarget = buildTarget, BuildTargetGroup = buildTargetGroup, OutputDirectory = this.GetCompileScriptsOutputDirectory(), Defines = compilationDefines, ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup), CompilationOptions = options, FilenameSuffix = this.assemblySuffix };
    }

    private ScriptAssemblySettings CreateEditorScriptAssemblySettings(EditorScriptCompilationOptions options)
    {
      return this.CreateScriptAssemblySettings(EditorUserBuildSettings.activeBuildTargetGroup, EditorUserBuildSettings.activeBuildTarget, options);
    }

    public EditorCompilation.AssemblyCompilerMessages[] GetCompileMessages()
    {
      if (this.compilationTask == null)
        return (EditorCompilation.AssemblyCompilerMessages[]) null;
      EditorCompilation.AssemblyCompilerMessages[] array = new EditorCompilation.AssemblyCompilerMessages[this.compilationTask.CompilerMessages.Count];
      int num = 0;
      foreach (KeyValuePair<ScriptAssembly, UnityEditor.Scripting.Compilers.CompilerMessage[]> compilerMessage in this.compilationTask.CompilerMessages)
      {
        ScriptAssembly key = compilerMessage.Key;
        UnityEditor.Scripting.Compilers.CompilerMessage[] compilerMessageArray = compilerMessage.Value;
        array[num++] = new EditorCompilation.AssemblyCompilerMessages()
        {
          assemblyFilename = key.Filename,
          messages = compilerMessageArray
        };
      }
      Array.Sort<EditorCompilation.AssemblyCompilerMessages>(array, (Comparison<EditorCompilation.AssemblyCompilerMessages>) ((m1, m2) => string.Compare(m1.assemblyFilename, m2.assemblyFilename)));
      return array;
    }

    public bool IsCompilationPending()
    {
      if (this.setupErrorFlags != EditorCompilation.CompilationSetupErrorFlags.none)
        return false;
      return this.DoesProjectFolderHaveAnyDirtyScripts() || this.runScriptUpdaterAssemblies.Count<string>() > 0;
    }

    public bool IsAnyAssemblyBuilderCompiling()
    {
      if (this.assemblyBuilders.Count <= 0)
        return false;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EditorCompilation.\u003CIsAnyAssemblyBuilderCompiling\u003Ec__AnonStorey7 compilingCAnonStorey7 = new EditorCompilation.\u003CIsAnyAssemblyBuilderCompiling\u003Ec__AnonStorey7();
      bool flag = false;
      // ISSUE: reference to a compiler-generated field
      compilingCAnonStorey7.removeAssemblyBuilders = new List<AssemblyBuilder>();
      foreach (AssemblyBuilder assemblyBuilder in this.assemblyBuilders)
      {
        switch (assemblyBuilder.status)
        {
          case AssemblyBuilderStatus.IsCompiling:
            flag = true;
            break;
          case AssemblyBuilderStatus.Finished:
            // ISSUE: reference to a compiler-generated field
            compilingCAnonStorey7.removeAssemblyBuilders.Add(assemblyBuilder);
            break;
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (compilingCAnonStorey7.removeAssemblyBuilders.Count > 0)
      {
        // ISSUE: reference to a compiler-generated method
        this.assemblyBuilders.RemoveAll(new Predicate<AssemblyBuilder>(compilingCAnonStorey7.\u003C\u003Em__0));
      }
      return flag;
    }

    public bool IsCompiling()
    {
      return this.IsCompilationTaskCompiling() || this.IsCompilationPending() || this.IsAnyAssemblyBuilderCompiling();
    }

    public bool IsCompilationTaskCompiling()
    {
      return this.compilationTask != null && this.compilationTask.IsCompiling;
    }

    public void StopAllCompilation()
    {
      if (this.compilationTask == null)
        return;
      this.compilationTask.Stop();
      this.compilationTask = (CompilationTask) null;
    }

    public EditorCompilation.CompileStatus TickCompilationPipeline(EditorScriptCompilationOptions options, BuildTargetGroup platformGroup, BuildTarget platform)
    {
      if (this.IsAnyAssemblyBuilderCompiling())
        return EditorCompilation.CompileStatus.Compiling;
      if (!this.IsCompilationTaskCompiling() && this.IsCompilationPending() && this.CompileScripts(options, platformGroup, platform))
        return EditorCompilation.CompileStatus.CompilationStarted;
      if (!this.IsCompilationTaskCompiling())
        return EditorCompilation.CompileStatus.Idle;
      if (this.compilationTask.Poll())
        return this.compilationTask == null || this.compilationTask.CompileErrors ? EditorCompilation.CompileStatus.CompilationFailed : EditorCompilation.CompileStatus.CompilationComplete;
      return EditorCompilation.CompileStatus.Compiling;
    }

    private string AssemblyFilenameWithSuffix(string assemblyFilename)
    {
      if (!string.IsNullOrEmpty(this.assemblySuffix))
        return AssetPath.GetAssemblyNameWithoutExtension(assemblyFilename) + this.assemblySuffix + ".dll";
      return assemblyFilename;
    }

    public EditorCompilation.TargetAssemblyInfo[] GetTargetAssemblies()
    {
      EditorBuildRules.TargetAssembly[] targetAssemblies = EditorBuildRules.GetPredefinedTargetAssemblies();
      EditorCompilation.TargetAssemblyInfo[] targetAssemblyInfoArray = new EditorCompilation.TargetAssemblyInfo[targetAssemblies.Length + (this.customTargetAssemblies == null ? 0 : ((IEnumerable<EditorBuildRules.TargetAssembly>) this.customTargetAssemblies).Count<EditorBuildRules.TargetAssembly>())];
      for (int index = 0; index < targetAssemblies.Length; ++index)
        targetAssemblyInfoArray[index] = this.ToTargetAssemblyInfo(targetAssemblies[index]);
      if (this.customTargetAssemblies != null)
      {
        for (int index1 = 0; index1 < ((IEnumerable<EditorBuildRules.TargetAssembly>) this.customTargetAssemblies).Count<EditorBuildRules.TargetAssembly>(); ++index1)
        {
          int index2 = targetAssemblies.Length + index1;
          targetAssemblyInfoArray[index2] = this.ToTargetAssemblyInfo(this.customTargetAssemblies[index1]);
        }
      }
      return targetAssemblyInfoArray;
    }

    public ScriptAssembly[] GetAllScriptAssembliesForLanguage<T>() where T : SupportedLanguage
    {
      return ((IEnumerable<ScriptAssembly>) this.GetAllScriptAssemblies(EditorScriptCompilationOptions.BuildingForEditor)).Where<ScriptAssembly>((Func<ScriptAssembly, bool>) (a => a.Language.GetType() == typeof (T))).ToArray<ScriptAssembly>();
    }

    public ScriptAssembly GetScriptAssemblyForLanguage<T>(string assemblyNameOrPath) where T : SupportedLanguage
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return ((IEnumerable<ScriptAssembly>) this.GetAllScriptAssembliesForLanguage<T>()).SingleOrDefault<ScriptAssembly>(new Func<ScriptAssembly, bool>(new EditorCompilation.\u003CGetScriptAssemblyForLanguage\u003Ec__AnonStorey8<T>() { assemblyName = AssetPath.GetAssemblyNameWithoutExtension(assemblyNameOrPath) }.\u003C\u003Em__0));
    }

    public EditorBuildRules.TargetAssembly[] GetCustomTargetAssemblies()
    {
      return this.customTargetAssemblies;
    }

    public PrecompiledAssembly[] GetUnityAssemblies()
    {
      return this.unityAssemblies;
    }

    public EditorCompilation.TargetAssemblyInfo GetTargetAssembly(string scriptPath)
    {
      return this.ToTargetAssemblyInfo(EditorBuildRules.GetTargetAssembly(scriptPath, this.projectDirectory, this.customTargetAssemblies));
    }

    public EditorBuildRules.TargetAssembly GetTargetAssemblyDetails(string scriptPath)
    {
      return EditorBuildRules.GetTargetAssembly(scriptPath, this.projectDirectory, this.customTargetAssemblies);
    }

    public ScriptAssembly[] GetAllEditorScriptAssemblies()
    {
      return this.GetAllScriptAssemblies(EditorScriptCompilationOptions.BuildingForEditor);
    }

    private ScriptAssembly[] GetAllScriptAssemblies(EditorScriptCompilationOptions options)
    {
      return this.GetAllScriptAssemblies(options, this.unityAssemblies, this.precompiledAssemblies);
    }

    private ScriptAssembly[] GetAllScriptAssemblies(EditorScriptCompilationOptions options, PrecompiledAssembly[] unityAssembliesArg, PrecompiledAssembly[] precompiledAssembliesArg)
    {
      EditorBuildRules.CompilationAssemblies assemblies = new EditorBuildRules.CompilationAssemblies() { UnityAssemblies = unityAssembliesArg, PrecompiledAssemblies = precompiledAssembliesArg, CustomTargetAssemblies = this.customTargetAssemblies, EditorAssemblyReferences = ModuleUtils.GetAdditionalReferencesForUserScripts() };
      return EditorBuildRules.GetAllScriptAssemblies((IEnumerable<string>) this.allScripts, this.projectDirectory, this.CreateEditorScriptAssemblySettings(options), assemblies);
    }

    public MonoIsland[] GetAllMonoIslands()
    {
      return this.GetAllMonoIslands(this.unityAssemblies, this.precompiledAssemblies, EditorScriptCompilationOptions.BuildingForEditor);
    }

    public MonoIsland[] GetAllMonoIslands(PrecompiledAssembly[] unityAssembliesArg, PrecompiledAssembly[] precompiledAssembliesArg, EditorScriptCompilationOptions options)
    {
      ScriptAssembly[] scriptAssemblies = this.GetAllScriptAssemblies(options, unityAssembliesArg, precompiledAssembliesArg);
      MonoIsland[] monoIslandArray = new MonoIsland[scriptAssemblies.Length];
      for (int index = 0; index < scriptAssemblies.Length; ++index)
        monoIslandArray[index] = scriptAssemblies[index].ToMonoIsland(EditorScriptCompilationOptions.BuildingForEditor, EditorCompilation.EditorTempPath);
      return monoIslandArray;
    }

    public bool IsRuntimeScriptAssembly(string assemblyNameOrPath)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EditorCompilation.\u003CIsRuntimeScriptAssembly\u003Ec__AnonStorey9 assemblyCAnonStorey9 = new EditorCompilation.\u003CIsRuntimeScriptAssembly\u003Ec__AnonStorey9();
      // ISSUE: reference to a compiler-generated field
      assemblyCAnonStorey9.assemblyFilename = AssetPath.GetFileName(assemblyNameOrPath);
      // ISSUE: reference to a compiler-generated field
      if (!assemblyCAnonStorey9.assemblyFilename.EndsWith(".dll"))
      {
        // ISSUE: reference to a compiler-generated field
        assemblyCAnonStorey9.assemblyFilename += ".dll";
      }
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return ((IEnumerable<EditorBuildRules.TargetAssembly>) EditorBuildRules.GetPredefinedTargetAssemblies()).Any<EditorBuildRules.TargetAssembly>(new Func<EditorBuildRules.TargetAssembly, bool>(assemblyCAnonStorey9.\u003C\u003Em__0)) || this.customTargetAssemblies != null && ((IEnumerable<EditorBuildRules.TargetAssembly>) this.customTargetAssemblies).Any<EditorBuildRules.TargetAssembly>(new Func<EditorBuildRules.TargetAssembly, bool>(assemblyCAnonStorey9.\u003C\u003Em__1));
    }

    private EditorCompilation.TargetAssemblyInfo ToTargetAssemblyInfo(EditorBuildRules.TargetAssembly targetAssembly)
    {
      return new EditorCompilation.TargetAssemblyInfo() { Name = this.AssemblyFilenameWithSuffix(targetAssembly.Filename), Flags = targetAssembly.Flags };
    }

    public ScriptAssembly CreateScriptAssembly(AssemblyBuilder assemblyBuilder)
    {
      EditorScriptCompilationOptions options = EditorScriptCompilationOptions.BuildingEmpty;
      AssemblyFlags assemblyFlags = AssemblyFlags.None;
      bool flag = false;
      if ((assemblyBuilder.flags & AssemblyBuilderFlags.DevelopmentBuild) == AssemblyBuilderFlags.DevelopmentBuild)
        options |= EditorScriptCompilationOptions.BuildingDevelopmentBuild;
      if ((assemblyBuilder.flags & AssemblyBuilderFlags.EditorAssembly) == AssemblyBuilderFlags.EditorAssembly)
      {
        options |= EditorScriptCompilationOptions.BuildingForEditor;
        assemblyFlags |= AssemblyFlags.EditorOnly;
        flag = true;
      }
      string[] array = ((IEnumerable<string>) assemblyBuilder.scriptPaths).Select<string, string>((Func<string, string>) (p => AssetPath.Combine(this.projectDirectory, p))).ToArray<string>();
      string path = AssetPath.Combine(this.projectDirectory, assemblyBuilder.assemblyPath);
      string[] strArray = InternalEditorUtility.GetCompilationDefines(options, assemblyBuilder.buildTargetGroup, assemblyBuilder.buildTarget);
      if (assemblyBuilder.additionalDefines != null)
        strArray = ((IEnumerable<string>) strArray).Concat<string>((IEnumerable<string>) assemblyBuilder.additionalDefines).ToArray<string>();
      ScriptAssembly scriptAssembly = new ScriptAssembly();
      scriptAssembly.Flags = assemblyFlags;
      scriptAssembly.BuildTarget = assemblyBuilder.buildTarget;
      scriptAssembly.ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(assemblyBuilder.buildTargetGroup);
      scriptAssembly.Language = ScriptCompilers.GetLanguageFromExtension(ScriptCompilers.GetExtensionOfSourceFile(assemblyBuilder.scriptPaths[0]));
      scriptAssembly.Files = array;
      scriptAssembly.Filename = AssetPath.GetFileName(path);
      scriptAssembly.OutputDirectory = AssetPath.GetDirectoryName(path);
      scriptAssembly.Defines = strArray;
      scriptAssembly.ScriptAssemblyReferences = new ScriptAssembly[0];
      IEnumerable<string> strings = EditorBuildRules.GetUnityReferences(scriptAssembly, this.unityAssemblies, options).Concat<string>((IEnumerable<string>) EditorBuildRules.GetCompiledCustomAssembliesReferences(scriptAssembly, this.customTargetAssemblies, this.GetCompileScriptsOutputDirectory(), this.assemblySuffix)).Concat<string>((IEnumerable<string>) EditorBuildRules.GetPrecompiledReferences(scriptAssembly, options, EditorBuildRules.EditorCompatibility.CompatibleWithEditor, this.precompiledAssemblies)).Concat<string>(!flag ? (IEnumerable<string>) new string[0] : (IEnumerable<string>) ModuleUtils.GetAdditionalReferencesForUserScripts());
      if (assemblyBuilder.additionalReferences != null && assemblyBuilder.additionalReferences.Length > 0)
        strings = strings.Concat<string>((IEnumerable<string>) assemblyBuilder.additionalReferences);
      if (assemblyBuilder.excludeReferences != null && assemblyBuilder.excludeReferences.Length > 0)
        strings = strings.Where<string>((Func<string, bool>) (r => !((IEnumerable<string>) assemblyBuilder.excludeReferences).Contains<string>(r)));
      scriptAssembly.References = strings.ToArray<string>();
      return scriptAssembly;
    }

    public void AddAssemblyBuilder(AssemblyBuilder assemblyBuilder)
    {
      this.assemblyBuilders.Add(assemblyBuilder);
    }

    public static UnityEditor.Compilation.CompilerMessage[] ConvertCompilerMessages(List<UnityEditor.Scripting.Compilers.CompilerMessage> messages)
    {
      UnityEditor.Compilation.CompilerMessage[] compilerMessageArray = new UnityEditor.Compilation.CompilerMessage[messages.Count];
      int num = 0;
      foreach (UnityEditor.Scripting.Compilers.CompilerMessage message in messages)
      {
        UnityEditor.Compilation.CompilerMessage compilerMessage = new UnityEditor.Compilation.CompilerMessage();
        compilerMessage.message = message.message;
        compilerMessage.file = message.file;
        compilerMessage.line = message.line;
        compilerMessage.column = message.column;
        switch (message.type)
        {
          case UnityEditor.Scripting.Compilers.CompilerMessageType.Error:
            compilerMessage.type = UnityEditor.Compilation.CompilerMessageType.Error;
            break;
          case UnityEditor.Scripting.Compilers.CompilerMessageType.Warning:
            compilerMessage.type = UnityEditor.Compilation.CompilerMessageType.Warning;
            break;
        }
        compilerMessageArray[num++] = compilerMessage;
      }
      return compilerMessageArray;
    }

    public enum CompileStatus
    {
      Idle,
      Compiling,
      CompilationStarted,
      CompilationFailed,
      CompilationComplete,
    }

    public enum DeleteFileOptions
    {
      NoLogError,
      LogError,
    }

    public struct TargetAssemblyInfo
    {
      public string Name;
      public AssemblyFlags Flags;
    }

    public struct AssemblyCompilerMessages
    {
      public string assemblyFilename;
      public UnityEditor.Scripting.Compilers.CompilerMessage[] messages;
    }

    public struct PackageAssembly
    {
      public string DirectoryPath;
      public string Name;
    }

    [System.Flags]
    public enum CompilationSetupErrorFlags
    {
      none = 0,
      cyclicReferences = 1,
      loadError = cyclicReferences, // 0x00000001
    }
  }
}
