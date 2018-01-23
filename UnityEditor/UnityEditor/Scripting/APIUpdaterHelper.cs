// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.APIUpdaterHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using ICSharpCode.NRefactory;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Unity.DataContract;
using UnityEditor.Modules;
using UnityEditor.Scripting.Compilers;
using UnityEditor.Utils;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEditor.Scripting
{
  internal class APIUpdaterHelper
  {
    private static string[] _ignoredAssemblies = new string[3]{ "^UnityScript$", "^System\\..*", "^mscorlib$" };

    public static bool IsReferenceToMissingObsoleteMember(string namespaceName, string className)
    {
      try
      {
        return APIUpdaterHelper.FindTypeInLoadedAssemblies((Func<System.Type, bool>) (t => t.Name == className && t.Namespace == namespaceName && APIUpdaterHelper.IsUpdateable(t))) != null;
      }
      catch (ReflectionTypeLoadException ex)
      {
        throw new Exception(ex.Message + ((IEnumerable<Exception>) ex.LoaderExceptions).Aggregate<Exception, string>("", (Func<string, Exception, string>) ((acc, curr) => acc + "\r\n\t" + curr.Message)));
      }
    }

    public static bool IsReferenceToTypeWithChangedNamespace(string normalizedErrorMessage)
    {
      try
      {
        string[] strArray = normalizedErrorMessage.Split('\n');
        return (APIUpdaterHelper.FindExactTypeMatchingMovedType(APIUpdaterHelper.GetValueFromNormalizedMessage((IEnumerable<string>) strArray, "EntityName=")) ?? APIUpdaterHelper.FindTypeMatchingMovedTypeBasedOnNamespaceFromError((IEnumerable<string>) strArray)) != null;
      }
      catch (ReflectionTypeLoadException ex)
      {
        throw new Exception(ex.Message + ((IEnumerable<Exception>) ex.LoaderExceptions).Aggregate<Exception, string>("", (Func<string, Exception, string>) ((acc, curr) => acc + "\r\n\t" + curr.Message)));
      }
    }

    public static void Run(string commaSeparatedListOfAssemblies)
    {
      string[] strArray = commaSeparatedListOfAssemblies.Split(new char[1]{ ',' }, StringSplitOptions.RemoveEmptyEntries);
      APIUpdaterLogger.WriteToFile("Started to update {0} assemblie(s)", (object) ((IEnumerable<string>) strArray).Count<string>());
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      foreach (string str1 in strArray)
      {
        if (AssemblyHelper.IsManagedAssembly(str1))
        {
          string str2 = APIUpdaterHelper.ResolveAssemblyPath(str1);
          string stdOut;
          string stdErr;
          int exitCode = APIUpdaterHelper.RunUpdatingProgram("AssemblyUpdater.exe", "-u -a " + str2 + APIUpdaterHelper.APIVersionArgument() + APIUpdaterHelper.AssemblySearchPathArgument() + APIUpdaterHelper.ConfigurationProviderAssembliesPathArgument(), out stdOut, out stdErr);
          if (stdOut.Length > 0)
            APIUpdaterLogger.WriteToFile("Assembly update output ({0})\r\n{1}", (object) str2, (object) stdOut);
          if (APIUpdaterHelper.IsError(exitCode))
            APIUpdaterLogger.WriteErrorToConsole("Error {0} running AssemblyUpdater. Its output is: `{1}`", (object) exitCode, (object) stdErr);
        }
      }
      APIUpdaterLogger.WriteToFile("Update finished in {0}s", (object) stopwatch.Elapsed.TotalSeconds);
    }

    private static bool IsError(int exitCode)
    {
      return (exitCode & 128) != 0;
    }

    private static string ResolveAssemblyPath(string assemblyPath)
    {
      return CommandLineFormatter.PrepareFileName(assemblyPath);
    }

    public static bool DoesAssemblyRequireUpgrade(string assemblyFullPath)
    {
      if (!File.Exists(assemblyFullPath) || !AssemblyHelper.IsManagedAssembly(assemblyFullPath) || !APIUpdaterHelper.MayContainUpdatableReferences(assemblyFullPath))
        return false;
      string stdOut;
      string stdErr;
      int num = APIUpdaterHelper.RunUpdatingProgram("AssemblyUpdater.exe", APIUpdaterHelper.TimeStampArgument() + APIUpdaterHelper.APIVersionArgument() + "--check-update-required -a " + CommandLineFormatter.PrepareFileName(assemblyFullPath) + APIUpdaterHelper.AssemblySearchPathArgument() + APIUpdaterHelper.ConfigurationProviderAssembliesPathArgument(), out stdOut, out stdErr);
      Console.WriteLine("{0}{1}", (object) stdOut, (object) stdErr);
      switch (num)
      {
        case 0:
        case 1:
          return false;
        case 2:
          return true;
        default:
          UnityEngine.Debug.LogError((object) (stdOut + Environment.NewLine + stdErr));
          return false;
      }
    }

    private static string AssemblySearchPathArgument()
    {
      return " -s \"" + (Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Managed") + ",+" + Path.Combine(EditorApplication.applicationContentsPath, "UnityExtensions/Unity") + ",+" + Application.dataPath) + "\"";
    }

    private static string ConfigurationProviderAssembliesPathArgument()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Unity.DataContract.PackageInfo unityExtension in ModuleManager.packageManager.unityExtensions)
      {
        foreach (string path2 in unityExtension.files.Where<KeyValuePair<string, PackageFileData>>((Func<KeyValuePair<string, PackageFileData>, bool>) (f => f.Value.type == PackageFileType.Dll)).Select<KeyValuePair<string, PackageFileData>, string>((Func<KeyValuePair<string, PackageFileData>, string>) (pi => pi.Key)))
          stringBuilder.AppendFormat(" {0}", (object) CommandLineFormatter.PrepareFileName(Path.Combine(unityExtension.basePath, path2)));
      }
      string editorManagedPath = APIUpdaterHelper.GetUnityEditorManagedPath();
      stringBuilder.AppendFormat(" {0}", (object) CommandLineFormatter.PrepareFileName(Path.Combine(editorManagedPath, "UnityEngine.dll")));
      stringBuilder.AppendFormat(" {0}", (object) CommandLineFormatter.PrepareFileName(Path.Combine(editorManagedPath, "UnityEditor.dll")));
      return stringBuilder.ToString();
    }

    private static string GetUnityEditorManagedPath()
    {
      return Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Managed");
    }

    private static int RunUpdatingProgram(string executable, string arguments, out string stdOut, out string stdErr)
    {
      ManagedProgram managedProgram = new ManagedProgram(MonoInstallationFinder.GetMonoInstallation("MonoBleedingEdge"), (string) null, EditorApplication.applicationContentsPath + "/Tools/ScriptUpdater/" + executable, arguments, false, (Action<ProcessStartInfo>) null);
      managedProgram.LogProcessStartInfo();
      managedProgram.Start();
      managedProgram.WaitForExit();
      stdOut = managedProgram.GetStandardOutputAsString();
      stdErr = string.Join("\r\n", managedProgram.GetErrorOutput());
      return managedProgram.ExitCode;
    }

    private static string APIVersionArgument()
    {
      return " --api-version " + Application.unityVersion + " ";
    }

    private static string TimeStampArgument()
    {
      return " --timestamp " + (object) DateTime.Now.Ticks + " ";
    }

    private static bool IsUpdateable(System.Type type)
    {
      object[] customAttributes = type.GetCustomAttributes(typeof (ObsoleteAttribute), false);
      if (customAttributes.Length != 1)
        return false;
      return ((ObsoleteAttribute) customAttributes[0]).Message.Contains("UnityUpgradable");
    }

    private static bool NamespaceHasChanged(System.Type type, string namespaceName)
    {
      object[] customAttributes = type.GetCustomAttributes(typeof (MovedFromAttribute), false);
      if (customAttributes.Length != 1)
        return false;
      if (string.IsNullOrEmpty(namespaceName))
        return true;
      return ((MovedFromAttribute) customAttributes[0]).Namespace == namespaceName;
    }

    private static System.Type FindTypeInLoadedAssemblies(Func<System.Type, bool> predicate)
    {
      return ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>) (assembly => !APIUpdaterHelper.IsIgnoredAssembly(assembly.GetName()))).SelectMany<Assembly, System.Type>((Func<Assembly, IEnumerable<System.Type>>) (a => APIUpdaterHelper.GetValidTypesIn(a))).FirstOrDefault<System.Type>(predicate);
    }

    private static IEnumerable<System.Type> GetValidTypesIn(Assembly a)
    {
      System.Type[] types;
      try
      {
        types = a.GetTypes();
      }
      catch (ReflectionTypeLoadException ex)
      {
        types = ex.Types;
      }
      return ((IEnumerable<System.Type>) types).Where<System.Type>((Func<System.Type, bool>) (t => t != null));
    }

    private static bool IsIgnoredAssembly(AssemblyName assemblyName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return ((IEnumerable<string>) APIUpdaterHelper._ignoredAssemblies).Any<string>(new Func<string, bool>(new APIUpdaterHelper.\u003CIsIgnoredAssembly\u003Ec__AnonStorey1() { name = assemblyName.Name }.\u003C\u003Em__0));
    }

    internal static bool MayContainUpdatableReferences(string assemblyPath)
    {
      using (AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(assemblyPath))
      {
        if (assembly.Name.IsWindowsRuntime)
          return false;
        if (!APIUpdaterHelper.IsTargetFrameworkValidOnCurrentOS(assembly))
          return false;
      }
      return true;
    }

    private static bool IsTargetFrameworkValidOnCurrentOS(AssemblyDefinition assembly)
    {
      return Environment.OSVersion.Platform == PlatformID.Win32NT || (!assembly.HasCustomAttributes ? 0 : (assembly.CustomAttributes.Any<Mono.Cecil.CustomAttribute>((Func<Mono.Cecil.CustomAttribute, bool>) (attr => APIUpdaterHelper.TargetsWindowsSpecificFramework(attr))) ? 1 : 0)) == 0;
    }

    private static bool TargetsWindowsSpecificFramework(Mono.Cecil.CustomAttribute targetFrameworkAttr)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      APIUpdaterHelper.\u003CTargetsWindowsSpecificFramework\u003Ec__AnonStorey2 frameworkCAnonStorey2 = new APIUpdaterHelper.\u003CTargetsWindowsSpecificFramework\u003Ec__AnonStorey2();
      if (!targetFrameworkAttr.AttributeType.FullName.Contains("System.Runtime.Versioning.TargetFrameworkAttribute"))
        return false;
      // ISSUE: reference to a compiler-generated field
      frameworkCAnonStorey2.regex = new Regex("\\.NETCore|\\.NETPortable");
      // ISSUE: reference to a compiler-generated method
      return targetFrameworkAttr.ConstructorArguments.Any<CustomAttributeArgument>(new Func<CustomAttributeArgument, bool>(frameworkCAnonStorey2.\u003C\u003Em__0));
    }

    private static System.Type FindExactTypeMatchingMovedType(string simpleOrQualifiedName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      APIUpdaterHelper.\u003CFindExactTypeMatchingMovedType\u003Ec__AnonStorey3 typeCAnonStorey3 = new APIUpdaterHelper.\u003CFindExactTypeMatchingMovedType\u003Ec__AnonStorey3();
      Match match = Regex.Match(simpleOrQualifiedName, "^(?:(?<namespace>.*)(?=\\.)\\.)?(?<typename>[a-zA-Z_0-9]+)$");
      if (!match.Success)
        return (System.Type) null;
      // ISSUE: reference to a compiler-generated field
      typeCAnonStorey3.typename = match.Groups["typename"].Value;
      // ISSUE: reference to a compiler-generated field
      typeCAnonStorey3.namespaceName = match.Groups["namespace"].Value;
      // ISSUE: reference to a compiler-generated method
      return APIUpdaterHelper.FindTypeInLoadedAssemblies(new Func<System.Type, bool>(typeCAnonStorey3.\u003C\u003Em__0));
    }

    private static System.Type FindTypeMatchingMovedTypeBasedOnNamespaceFromError(IEnumerable<string> lines)
    {
      string normalizedMessage1 = APIUpdaterHelper.GetValueFromNormalizedMessage(lines, "Line=");
      int line = normalizedMessage1 == null ? -1 : int.Parse(normalizedMessage1);
      string normalizedMessage2 = APIUpdaterHelper.GetValueFromNormalizedMessage(lines, "Column=");
      int column = normalizedMessage2 == null ? -1 : int.Parse(normalizedMessage2);
      string normalizedMessage3 = APIUpdaterHelper.GetValueFromNormalizedMessage(lines, "Script=");
      if (line != -1 && column != -1)
      {
        if (normalizedMessage3 != null)
        {
          try
          {
            using (FileStream fileStream = File.Open(normalizedMessage3, System.IO.FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
              IParser parser = ParserFactory.CreateParser(ICSharpCode.NRefactory.SupportedLanguage.CSharp, (TextReader) new StreamReader((Stream) fileStream));
              parser.Lexer.EvaluateConditionalCompilation = false;
              parser.Parse();
              string namespaceError = InvalidTypeOrNamespaceErrorTypeMapper.IsTypeMovedToNamespaceError(parser.CompilationUnit, line, column);
              if (namespaceError == null)
                return (System.Type) null;
              return APIUpdaterHelper.FindExactTypeMatchingMovedType(namespaceError);
            }
          }
          catch (FileNotFoundException ex)
          {
            return (System.Type) null;
          }
        }
      }
      return (System.Type) null;
    }

    private static string GetValueFromNormalizedMessage(IEnumerable<string> lines, string marker)
    {
      string str1 = (string) null;
      string str2 = lines.FirstOrDefault<string>((Func<string, bool>) (l => l.StartsWith(marker)));
      if (str2 != null)
        str1 = str2.Substring(marker.Length).Trim();
      return str1;
    }
  }
}
