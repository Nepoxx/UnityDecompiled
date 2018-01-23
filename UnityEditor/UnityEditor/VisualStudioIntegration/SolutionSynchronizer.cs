// Decompiled with JetBrains decompiler
// Type: UnityEditor.VisualStudioIntegration.SolutionSynchronizer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEditor.Compilation;
using UnityEditor.Modules;
using UnityEditor.Scripting;
using UnityEditor.Scripting.Compilers;
using UnityEditor.Scripting.ScriptCompilation;
using UnityEditorInternal;

namespace UnityEditor.VisualStudioIntegration
{
  internal class SolutionSynchronizer
  {
    public static readonly ISolutionSynchronizationSettings DefaultSynchronizationSettings = (ISolutionSynchronizationSettings) new DefaultSolutionSynchronizationSettings();
    private static readonly string WindowsNewline = "\r\n";
    internal static readonly Dictionary<string, ScriptingLanguage> BuiltinSupportedExtensions = new Dictionary<string, ScriptingLanguage>() { { "cs", ScriptingLanguage.CSharp }, { "js", ScriptingLanguage.UnityScript }, { "boo", ScriptingLanguage.Boo }, { "shader", ScriptingLanguage.None }, { "compute", ScriptingLanguage.None }, { "cginc", ScriptingLanguage.None }, { "hlsl", ScriptingLanguage.None }, { "glslinc", ScriptingLanguage.None } };
    private static readonly Dictionary<ScriptingLanguage, string> ProjectExtensions = new Dictionary<ScriptingLanguage, string>() { { ScriptingLanguage.Boo, ".booproj" }, { ScriptingLanguage.CSharp, ".csproj" }, { ScriptingLanguage.UnityScript, ".unityproj" }, { ScriptingLanguage.None, ".csproj" } };
    private static readonly Regex _MonoDevelopPropertyHeader = new Regex("^\\s*GlobalSection\\(MonoDevelopProperties.*\\)");
    public static readonly string MSBuildNamespaceUri = "http://schemas.microsoft.com/developer/msbuild/2003";
    private static readonly string DefaultMonoDevelopSolutionProperties = string.Join("\r\n", new string[3]{ "    GlobalSection(MonoDevelopProperties) = preSolution", "        StartupItem = Assembly-CSharp.csproj", "    EndGlobalSection" }).Replace("    ", "\t");
    public static readonly Regex scriptReferenceExpression = new Regex("^Library.ScriptAssemblies.(?<dllname>(?<project>.*)\\.dll$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private string[] ProjectSupportedExtensions = new string[0];
    private readonly string _projectDirectory;
    private readonly ISolutionSynchronizationSettings _settings;
    private readonly string _projectName;

    public SolutionSynchronizer(string projectDirectory, ISolutionSynchronizationSettings settings)
    {
      this._projectDirectory = projectDirectory;
      this._settings = settings;
      this._projectName = Path.GetFileName(this._projectDirectory);
    }

    public SolutionSynchronizer(string projectDirectory)
      : this(projectDirectory, SolutionSynchronizer.DefaultSynchronizationSettings)
    {
    }

    private void SetupProjectSupportedExtensions()
    {
      this.ProjectSupportedExtensions = EditorSettings.projectGenerationUserExtensions;
    }

    public bool ShouldFileBePartOfSolution(string file)
    {
      string extension = Path.GetExtension(file);
      if (AssetDatabase.IsPackagedAssetPath(file))
        return false;
      if (extension == ".dll" || file.ToLower().EndsWith(".asmdef"))
        return true;
      return this.IsSupportedExtension(extension);
    }

    private bool IsSupportedExtension(string extension)
    {
      extension = extension.TrimStart('.');
      return SolutionSynchronizer.BuiltinSupportedExtensions.ContainsKey(extension) || ((IEnumerable<string>) this.ProjectSupportedExtensions).Contains<string>(extension);
    }

    private static ScriptingLanguage ScriptingLanguageFor(MonoIsland island)
    {
      return SolutionSynchronizer.ScriptingLanguageFor(island.GetExtensionOfSourceFiles());
    }

    private static ScriptingLanguage ScriptingLanguageFor(string extension)
    {
      ScriptingLanguage scriptingLanguage;
      if (SolutionSynchronizer.BuiltinSupportedExtensions.TryGetValue(extension.TrimStart('.'), out scriptingLanguage))
        return scriptingLanguage;
      return ScriptingLanguage.None;
    }

    public bool ProjectExists(MonoIsland island)
    {
      return File.Exists(this.ProjectFile(island));
    }

    public bool SolutionExists()
    {
      return File.Exists(this.SolutionFile());
    }

    private static void DumpIsland(MonoIsland island)
    {
      Console.WriteLine("{0} ({1})", (object) island._output, (object) island._api_compatibility_level);
      Console.WriteLine("Files: ");
      Console.WriteLine(string.Join("\n", island._files));
      Console.WriteLine("References: ");
      Console.WriteLine(string.Join("\n", island._references));
      Console.WriteLine("");
    }

    public bool SyncIfNeeded(IEnumerable<string> affectedFiles)
    {
      this.SetupProjectSupportedExtensions();
      if (!this.SolutionExists() || !affectedFiles.Any<string>(new Func<string, bool>(this.ShouldFileBePartOfSolution)))
        return false;
      this.Sync();
      return true;
    }

    public void Sync()
    {
      this.SetupProjectSupportedExtensions();
      if (!AssetPostprocessingInternal.OnPreGeneratingCSProjectFiles())
      {
        IEnumerable<MonoIsland> islands = ((IEnumerable<MonoIsland>) EditorCompilationInterface.GetAllMonoIslands()).Where<MonoIsland>((Func<MonoIsland, bool>) (i => 0 < i._files.Length && ((IEnumerable<string>) i._files).Any<string>((Func<string, bool>) (f => this.ShouldFileBePartOfSolution(f)))));
        Dictionary<string, string> assetProjectParts = this.GenerateAllAssetProjectParts();
        string[] fileDefinesFromFile = ScriptCompilerBase.GetResponseFileDefinesFromFile(MonoCSharpCompiler.ReponseFilename);
        this.SyncSolution(islands);
        List<MonoIsland> list = SolutionSynchronizer.RelevantIslandsForMode(islands, SolutionSynchronizer.ModeForCurrentExternalEditor()).ToList<MonoIsland>();
        foreach (MonoIsland island in list)
          this.SyncProject(island, assetProjectParts, fileDefinesFromFile, list);
        if (ScriptEditorUtility.GetScriptEditorFromPreferences() == ScriptEditorUtility.ScriptEditor.VisualStudioCode)
          this.WriteVSCodeSettingsFiles();
      }
      AssetPostprocessingInternal.CallOnGeneratedCSProjectFiles();
    }

    private Dictionary<string, string> GenerateAllAssetProjectParts()
    {
      Dictionary<string, StringBuilder> dictionary1 = new Dictionary<string, StringBuilder>();
      foreach (string allAssetPath in AssetDatabase.GetAllAssetPaths())
      {
        string extension = Path.GetExtension(allAssetPath);
        if (this.IsSupportedExtension(extension) && SolutionSynchronizer.ScriptingLanguageFor(extension) == ScriptingLanguage.None)
        {
          string withoutExtension = Path.GetFileNameWithoutExtension((CompilationPipeline.GetAssemblyNameFromScriptPath(allAssetPath + ".cs") ?? CompilationPipeline.GetAssemblyNameFromScriptPath(allAssetPath + ".js")) ?? CompilationPipeline.GetAssemblyNameFromScriptPath(allAssetPath + ".boo"));
          StringBuilder stringBuilder = (StringBuilder) null;
          if (!dictionary1.TryGetValue(withoutExtension, out stringBuilder))
          {
            stringBuilder = new StringBuilder();
            dictionary1[withoutExtension] = stringBuilder;
          }
          stringBuilder.AppendFormat("     <None Include=\"{0}\" />{1}", (object) this.EscapedRelativePathFor(allAssetPath), (object) SolutionSynchronizer.WindowsNewline);
        }
      }
      Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
      foreach (KeyValuePair<string, StringBuilder> keyValuePair in dictionary1)
        dictionary2[keyValuePair.Key] = keyValuePair.Value.ToString();
      return dictionary2;
    }

    private void SyncProject(MonoIsland island, Dictionary<string, string> allAssetsProjectParts, string[] additionalDefines, List<MonoIsland> allProjectIslands)
    {
      SolutionSynchronizer.SyncFileIfNotChanged(this.ProjectFile(island), this.ProjectText(island, SolutionSynchronizer.ModeForCurrentExternalEditor(), allAssetsProjectParts, additionalDefines, allProjectIslands));
    }

    private static void SyncFileIfNotChanged(string filename, string newContents)
    {
      if (File.Exists(filename) && newContents == File.ReadAllText(filename))
        return;
      File.WriteAllText(filename, newContents, Encoding.UTF8);
    }

    private void WriteVSCodeSettingsFiles()
    {
      string str = Path.Combine(this._projectDirectory, ".vscode");
      if (!Directory.Exists(str))
        Directory.CreateDirectory(str);
      string path = Path.Combine(str, "settings.json");
      if (File.Exists(path))
        return;
      File.WriteAllText(path, VSCodeTemplates.SettingsJson);
    }

    private static bool IsAdditionalInternalAssemblyReference(bool isBuildingEditorProject, string reference)
    {
      if (isBuildingEditorProject)
        return ((IEnumerable<string>) ModuleUtils.GetAdditionalReferencesForEditorCsharpProject()).Contains<string>(reference);
      return false;
    }

    private string ProjectText(MonoIsland island, SolutionSynchronizer.Mode mode, Dictionary<string, string> allAssetsProjectParts, string[] additionalDefines, List<MonoIsland> allProjectIslands)
    {
      StringBuilder stringBuilder = new StringBuilder(this.ProjectHeader(island, additionalDefines));
      List<string> first = new List<string>();
      List<Match> matchList = new List<Match>();
      bool isBuildingEditorProject = island._output.EndsWith("-Editor.dll");
      foreach (string file1 in island._files)
      {
        if (this.ShouldFileBePartOfSolution(file1))
        {
          string lower = Path.GetExtension(file1).ToLower();
          string file2 = !Path.IsPathRooted(file1) ? Path.Combine(this._projectDirectory, file1) : file1;
          if (".dll" != lower)
          {
            string str = "Compile";
            stringBuilder.AppendFormat("     <{0} Include=\"{1}\" />{2}", (object) str, (object) this.EscapedRelativePathFor(file2), (object) SolutionSynchronizer.WindowsNewline);
          }
          else
            first.Add(file2);
        }
      }
      string withoutExtension = Path.GetFileNameWithoutExtension(island._output);
      string str1;
      if (allAssetsProjectParts.TryGetValue(withoutExtension, out str1))
        stringBuilder.Append(str1);
      List<string> stringList = new List<string>();
      foreach (string str2 in first.Union<string>((IEnumerable<string>) island._references))
      {
        if (!str2.EndsWith("/UnityEditor.dll") && !str2.EndsWith("/UnityEngine.dll") && (!str2.EndsWith("\\UnityEditor.dll") && !str2.EndsWith("\\UnityEngine.dll")) && !AssemblyHelper.IsUnityEngineModule(Path.GetFileNameWithoutExtension(str2)))
        {
          Match match = SolutionSynchronizer.scriptReferenceExpression.Match(str2);
          if (match.Success)
          {
            ScriptingLanguage scriptingLanguage = (ScriptingLanguage) Enum.Parse(typeof (ScriptingLanguage), ScriptCompilers.GetLanguageFromExtension(island.GetExtensionOfSourceFiles()).GetLanguageName(), true);
            if (mode == SolutionSynchronizer.Mode.UnityScriptAsUnityProj || scriptingLanguage == ScriptingLanguage.CSharp)
            {
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: reference to a compiler-generated method
              if (allProjectIslands.Any<MonoIsland>(new Func<MonoIsland, bool>(new SolutionSynchronizer.\u003CProjectText\u003Ec__AnonStorey0() { dllName = match.Groups["dllname"].Value }.\u003C\u003Em__0)))
              {
                matchList.Add(match);
                continue;
              }
            }
          }
          string str3 = !Path.IsPathRooted(str2) ? Path.Combine(this._projectDirectory, str2) : str2;
          if (AssemblyHelper.IsManagedAssembly(str3))
          {
            if (AssemblyHelper.IsInternalAssembly(str3))
            {
              if (SolutionSynchronizer.IsAdditionalInternalAssemblyReference(isBuildingEditorProject, str3))
              {
                string fileName = Path.GetFileName(str3);
                if (!stringList.Contains(fileName))
                  stringList.Add(fileName);
                else
                  continue;
              }
              else
                continue;
            }
            string path = str3.Replace("\\", "/").Replace("\\\\", "/");
            stringBuilder.AppendFormat(" <Reference Include=\"{0}\">{1}", (object) Path.GetFileNameWithoutExtension(path), (object) SolutionSynchronizer.WindowsNewline);
            stringBuilder.AppendFormat(" <HintPath>{0}</HintPath>{1}", (object) path, (object) SolutionSynchronizer.WindowsNewline);
            stringBuilder.AppendFormat(" </Reference>{0}", (object) SolutionSynchronizer.WindowsNewline);
          }
        }
      }
      if (0 < matchList.Count)
      {
        stringBuilder.AppendLine("  </ItemGroup>");
        stringBuilder.AppendLine("  <ItemGroup>");
        foreach (Match match in matchList)
        {
          EditorBuildRules.TargetAssembly targetAssemblyDetails = EditorCompilationInterface.Instance.GetTargetAssemblyDetails(match.Groups["dllname"].Value);
          ScriptingLanguage language = ScriptingLanguage.None;
          if (targetAssemblyDetails != null)
            language = (ScriptingLanguage) Enum.Parse(typeof (ScriptingLanguage), targetAssemblyDetails.Language.GetLanguageName(), true);
          string str2 = match.Groups["project"].Value;
          stringBuilder.AppendFormat("    <ProjectReference Include=\"{0}{1}\">{2}", (object) str2, (object) SolutionSynchronizer.GetProjectExtension(language), (object) SolutionSynchronizer.WindowsNewline);
          stringBuilder.AppendFormat("      <Project>{{{0}}}</Project>", (object) this.ProjectGuid(Path.Combine("Temp", match.Groups["project"].Value + ".dll")), (object) SolutionSynchronizer.WindowsNewline);
          stringBuilder.AppendFormat("      <Name>{0}</Name>", (object) str2, (object) SolutionSynchronizer.WindowsNewline);
          stringBuilder.AppendLine("    </ProjectReference>");
        }
      }
      stringBuilder.Append(this.ProjectFooter(island));
      return stringBuilder.ToString();
    }

    public string ProjectFile(MonoIsland island)
    {
      ScriptingLanguage index = SolutionSynchronizer.ScriptingLanguageFor(island);
      return Path.Combine(this._projectDirectory, string.Format("{0}{1}", (object) Path.GetFileNameWithoutExtension(island._output), (object) SolutionSynchronizer.ProjectExtensions[index]));
    }

    internal string SolutionFile()
    {
      return Path.Combine(this._projectDirectory, string.Format("{0}.sln", (object) this._projectName));
    }

    private string ProjectHeader(MonoIsland island, string[] additionalDefines)
    {
      string str1 = "v3.5";
      string str2 = "4";
      string str3 = "4.0";
      string str4 = "10.0.20506";
      ScriptingLanguage language = SolutionSynchronizer.ScriptingLanguageFor(island);
      if (island._api_compatibility_level == ApiCompatibilityLevel.NET_4_6)
      {
        str1 = "v4.6";
        str2 = "6";
      }
      else if (ScriptEditorUtility.GetScriptEditorFromPreferences() == ScriptEditorUtility.ScriptEditor.Rider)
        str1 = "v4.5";
      else if (this._settings.VisualStudioVersion == 9)
      {
        str3 = "3.5";
        str4 = "9.0.21022";
      }
      object[] objArray = new object[11]{ (object) str3, (object) str4, (object) this.ProjectGuid(island._output), (object) this._settings.EngineAssemblyPath, (object) this._settings.EditorAssemblyPath, (object) string.Join(";", ((IEnumerable<string>) new string[2]{ "DEBUG", "TRACE" }).Concat<string>((IEnumerable<string>) this._settings.Defines).Concat<string>((IEnumerable<string>) island._defines).Concat<string>((IEnumerable<string>) additionalDefines).Distinct<string>().ToArray<string>()), (object) SolutionSynchronizer.MSBuildNamespaceUri, (object) Path.GetFileNameWithoutExtension(island._output), (object) EditorSettings.projectGenerationRootNamespace, (object) str1, (object) str2 };
      try
      {
        return string.Format(this._settings.GetProjectHeaderTemplate(language), objArray);
      }
      catch (Exception ex)
      {
        throw new NotSupportedException("Failed creating c# project because the c# project header did not have the correct amount of arguments, which is " + (object) objArray.Length);
      }
    }

    private void SyncSolution(IEnumerable<MonoIsland> islands)
    {
      SolutionSynchronizer.SyncFileIfNotChanged(this.SolutionFile(), this.SolutionText(islands, SolutionSynchronizer.ModeForCurrentExternalEditor()));
    }

    private static SolutionSynchronizer.Mode ModeForCurrentExternalEditor()
    {
      switch (ScriptEditorUtility.GetScriptEditorFromPreferences())
      {
        case ScriptEditorUtility.ScriptEditor.Internal:
          return SolutionSynchronizer.Mode.UnityScriptAsUnityProj;
        case ScriptEditorUtility.ScriptEditor.VisualStudio:
        case ScriptEditorUtility.ScriptEditor.VisualStudioExpress:
        case ScriptEditorUtility.ScriptEditor.VisualStudioCode:
          return SolutionSynchronizer.Mode.UnityScriptAsPrecompiledAssembly;
        default:
          return !EditorPrefs.GetBool("kExternalEditorSupportsUnityProj", false) ? SolutionSynchronizer.Mode.UnityScriptAsPrecompiledAssembly : SolutionSynchronizer.Mode.UnityScriptAsUnityProj;
      }
    }

    private string SolutionText(IEnumerable<MonoIsland> islands, SolutionSynchronizer.Mode mode)
    {
      string str1 = "11.00";
      string str2 = "2010";
      if (this._settings.VisualStudioVersion == 9)
      {
        str1 = "10.00";
        str2 = "2008";
      }
      IEnumerable<MonoIsland> monoIslands = SolutionSynchronizer.RelevantIslandsForMode(islands, mode);
      string projectEntries = this.GetProjectEntries(monoIslands);
      string str3 = string.Join(SolutionSynchronizer.WindowsNewline, monoIslands.Select<MonoIsland, string>((Func<MonoIsland, string>) (i => this.GetProjectActiveConfigurations(this.ProjectGuid(i._output)))).ToArray<string>());
      return string.Format(this._settings.SolutionTemplate, (object) str1, (object) str2, (object) projectEntries, (object) str3, (object) this.ReadExistingMonoDevelopSolutionProperties());
    }

    private static IEnumerable<MonoIsland> RelevantIslandsForMode(IEnumerable<MonoIsland> islands, SolutionSynchronizer.Mode mode)
    {
      return islands.Where<MonoIsland>((Func<MonoIsland, bool>) (i => mode == SolutionSynchronizer.Mode.UnityScriptAsUnityProj || ScriptingLanguage.CSharp == SolutionSynchronizer.ScriptingLanguageFor(i)));
    }

    private string GetProjectEntries(IEnumerable<MonoIsland> islands)
    {
      IEnumerable<string> source = islands.Select<MonoIsland, string>((Func<MonoIsland, string>) (i => string.Format(SolutionSynchronizer.DefaultSynchronizationSettings.SolutionProjectEntryTemplate, (object) this.SolutionGuid(i), (object) this._projectName, (object) Path.GetFileName(this.ProjectFile(i)), (object) this.ProjectGuid(i._output))));
      return string.Join(SolutionSynchronizer.WindowsNewline, source.ToArray<string>());
    }

    private string GetProjectActiveConfigurations(string projectGuid)
    {
      return string.Format(SolutionSynchronizer.DefaultSynchronizationSettings.SolutionProjectConfigurationTemplate, (object) projectGuid);
    }

    private string EscapedRelativePathFor(string file)
    {
      string str = this._projectDirectory.Replace("/", "\\");
      file = file.Replace("/", "\\");
      return SecurityElement.Escape(!file.StartsWith(str) ? file : file.Substring(this._projectDirectory.Length + 1));
    }

    private string ProjectGuid(string assembly)
    {
      return SolutionGuidGenerator.GuidForProject(this._projectName + Path.GetFileNameWithoutExtension(assembly));
    }

    private string SolutionGuid(MonoIsland island)
    {
      return SolutionGuidGenerator.GuidForSolution(this._projectName, island.GetExtensionOfSourceFiles());
    }

    private string ProjectFooter(MonoIsland island)
    {
      return string.Format(this._settings.GetProjectFooterTemplate(SolutionSynchronizer.ScriptingLanguageFor(island)), (object) this.ReadExistingMonoDevelopProjectProperties(island));
    }

    private string ReadExistingMonoDevelopSolutionProperties()
    {
      if (!this.SolutionExists())
        return SolutionSynchronizer.DefaultMonoDevelopSolutionProperties;
      string[] strArray;
      try
      {
        strArray = File.ReadAllLines(this.SolutionFile());
      }
      catch (IOException ex)
      {
        return SolutionSynchronizer.DefaultMonoDevelopSolutionProperties;
      }
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      foreach (string input in strArray)
      {
        if (SolutionSynchronizer._MonoDevelopPropertyHeader.IsMatch(input))
          flag = true;
        if (flag)
        {
          if (input.Contains("EndGlobalSection"))
          {
            stringBuilder.Append(input);
            flag = false;
          }
          else
            stringBuilder.AppendFormat("{0}{1}", (object) input, (object) SolutionSynchronizer.WindowsNewline);
        }
      }
      if (0 < stringBuilder.Length)
        return stringBuilder.ToString();
      return SolutionSynchronizer.DefaultMonoDevelopSolutionProperties;
    }

    private string ReadExistingMonoDevelopProjectProperties(MonoIsland island)
    {
      if (!this.ProjectExists(island))
        return string.Empty;
      XmlDocument xmlDocument = new XmlDocument();
      XmlNamespaceManager nsmgr;
      try
      {
        xmlDocument.Load(this.ProjectFile(island));
        nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
        nsmgr.AddNamespace("msb", SolutionSynchronizer.MSBuildNamespaceUri);
      }
      catch (Exception ex)
      {
        if (ex is IOException || ex is XmlException)
          return string.Empty;
        throw;
      }
      XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/msb:Project/msb:ProjectExtensions", nsmgr);
      if (xmlNodeList.Count == 0)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      IEnumerator enumerator = xmlNodeList.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          XmlNode current = (XmlNode) enumerator.Current;
          stringBuilder.AppendLine(current.OuterXml);
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      return stringBuilder.ToString();
    }

    [Obsolete("Use AssemblyHelper.IsManagedAssembly")]
    public static bool IsManagedAssembly(string file)
    {
      return AssemblyHelper.IsManagedAssembly(file);
    }

    public static string GetProjectExtension(ScriptingLanguage language)
    {
      if (!SolutionSynchronizer.ProjectExtensions.ContainsKey(language))
        throw new ArgumentException("Unsupported language", nameof (language));
      return SolutionSynchronizer.ProjectExtensions[language];
    }

    private enum Mode
    {
      UnityScriptAsUnityProj,
      UnityScriptAsPrecompiledAssembly,
    }
  }
}
