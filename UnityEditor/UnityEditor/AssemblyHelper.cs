// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssemblyHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor.Modules;
using UnityEditor.Scripting.ScriptCompilation;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEditor
{
  internal class AssemblyHelper
  {
    private const int kDefaultDepth = 10;

    public static void CheckForAssemblyFileNameMismatch(string assemblyPath)
    {
      string withoutExtension = Path.GetFileNameWithoutExtension(assemblyPath);
      string internalAssemblyName = AssemblyHelper.ExtractInternalAssemblyName(assemblyPath);
      if (string.IsNullOrEmpty(internalAssemblyName) || !(withoutExtension != internalAssemblyName))
        return;
      UnityEngine.Debug.LogWarning((object) ("Assembly '" + internalAssemblyName + "' has non matching file name: '" + Path.GetFileName(assemblyPath) + "'. This can cause build issues on some platforms."));
    }

    public static string[] GetNamesOfAssembliesLoadedInCurrentDomain()
    {
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      List<string> stringList = new List<string>();
      foreach (Assembly assembly in assemblies)
      {
        try
        {
          stringList.Add(assembly.Location);
        }
        catch (NotSupportedException ex)
        {
        }
      }
      return stringList.ToArray();
    }

    public static Assembly FindLoadedAssemblyWithName(string s)
    {
      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        try
        {
          if (s == Path.GetFileNameWithoutExtension(assembly.Location))
            return assembly;
        }
        catch (NotSupportedException ex)
        {
        }
      }
      return (Assembly) null;
    }

    public static string ExtractInternalAssemblyName(string path)
    {
      try
      {
        return AssemblyDefinition.ReadAssembly(path).Name.Name;
      }
      catch
      {
        return "";
      }
    }

    private static AssemblyDefinition GetAssemblyDefinitionCached(string path, Dictionary<string, AssemblyDefinition> cache)
    {
      if (cache.ContainsKey(path))
        return cache[path];
      AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(path);
      cache[path] = assemblyDefinition;
      return assemblyDefinition;
    }

    private static bool IgnoreAssembly(string assemblyPath, BuildTarget target)
    {
      switch (target)
      {
        case BuildTarget.WSAPlayer:
          if (assemblyPath.IndexOf("mscorlib.dll") != -1 || assemblyPath.IndexOf("System.") != -1 || (assemblyPath.IndexOf("Windows.dll") != -1 || assemblyPath.IndexOf("Microsoft.") != -1) || (assemblyPath.IndexOf("Windows.") != -1 || assemblyPath.IndexOf("WinRTLegacy.dll") != -1 || assemblyPath.IndexOf("platform.dll") != -1))
            return true;
          break;
        case BuildTarget.XboxOne:
          if (PlayerSettings.GetApiCompatibilityLevel(BuildTargetGroup.XboxOne) != ApiCompatibilityLevel.NET_4_6)
            break;
          goto case BuildTarget.WSAPlayer;
      }
      return AssemblyHelper.IsInternalAssembly(assemblyPath);
    }

    private static void AddReferencedAssembliesRecurse(string assemblyPath, List<string> alreadyFoundAssemblies, string[] allAssemblyPaths, string[] foldersToSearch, Dictionary<string, AssemblyDefinition> cache, BuildTarget target)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssemblyHelper.\u003CAddReferencedAssembliesRecurse\u003Ec__AnonStorey1 recurseCAnonStorey1 = new AssemblyHelper.\u003CAddReferencedAssembliesRecurse\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      recurseCAnonStorey1.target = target;
      // ISSUE: reference to a compiler-generated field
      if (AssemblyHelper.IgnoreAssembly(assemblyPath, recurseCAnonStorey1.target) || !File.Exists(assemblyPath))
        return;
      AssemblyDefinition definitionCached = AssemblyHelper.GetAssemblyDefinitionCached(assemblyPath, cache);
      if (definitionCached == null)
        throw new ArgumentException("Referenced Assembly " + Path.GetFileName(assemblyPath) + " could not be found!");
      if (alreadyFoundAssemblies.IndexOf(assemblyPath) != -1)
        return;
      alreadyFoundAssemblies.Add(assemblyPath);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      IEnumerable<string> source = ((IEnumerable<PluginImporter>) PluginImporter.GetImporters(recurseCAnonStorey1.target)).Where<PluginImporter>(new Func<PluginImporter, bool>(recurseCAnonStorey1.\u003C\u003Em__0)).Select<PluginImporter, string>((Func<PluginImporter, string>) (i => Path.GetFileName(i.assetPath))).Distinct<string>();
      foreach (AssemblyNameReference assemblyReference in definitionCached.MainModule.AssemblyReferences)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AssemblyHelper.\u003CAddReferencedAssembliesRecurse\u003Ec__AnonStorey2 recurseCAnonStorey2 = new AssemblyHelper.\u003CAddReferencedAssembliesRecurse\u003Ec__AnonStorey2();
        // ISSUE: reference to a compiler-generated field
        recurseCAnonStorey2.referencedAssembly = assemblyReference;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!(recurseCAnonStorey2.referencedAssembly.Name == "BridgeInterface") && !(recurseCAnonStorey2.referencedAssembly.Name == "WinRTBridge") && (!(recurseCAnonStorey2.referencedAssembly.Name == "UnityEngineProxy") && !AssemblyHelper.IgnoreAssembly(recurseCAnonStorey2.referencedAssembly.Name + ".dll", recurseCAnonStorey1.target)))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          string assemblyName = AssemblyHelper.FindAssemblyName(recurseCAnonStorey2.referencedAssembly.FullName, recurseCAnonStorey2.referencedAssembly.Name, allAssemblyPaths, foldersToSearch, cache);
          if (assemblyName == "")
          {
            bool flag = false;
            string[] strArray = new string[2]{ ".dll", ".winmd" };
            foreach (string str in strArray)
            {
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: reference to a compiler-generated method
              if (source.Any<string>(new Func<string, bool>(new AssemblyHelper.\u003CAddReferencedAssembliesRecurse\u003Ec__AnonStorey3() { \u003C\u003Ef__ref\u00242 = recurseCAnonStorey2, extension = str }.\u003C\u003Em__0)))
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              // ISSUE: reference to a compiler-generated field
              throw new ArgumentException(string.Format("The Assembly {0} is referenced by {1} ('{2}'). But the dll is not allowed to be included or could not be found.", (object) recurseCAnonStorey2.referencedAssembly.Name, (object) definitionCached.MainModule.Assembly.Name.Name, (object) assemblyPath));
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            AssemblyHelper.AddReferencedAssembliesRecurse(assemblyName, alreadyFoundAssemblies, allAssemblyPaths, foldersToSearch, cache, recurseCAnonStorey1.target);
          }
        }
      }
    }

    private static string FindAssemblyName(string fullName, string name, string[] allAssemblyPaths, string[] foldersToSearch, Dictionary<string, AssemblyDefinition> cache)
    {
      for (int index = 0; index < allAssemblyPaths.Length; ++index)
      {
        if (File.Exists(allAssemblyPaths[index]) && AssemblyHelper.GetAssemblyDefinitionCached(allAssemblyPaths[index], cache).MainModule.Assembly.Name.Name == name)
          return allAssemblyPaths[index];
      }
      foreach (string path1 in foldersToSearch)
      {
        string path = Path.Combine(path1, name + ".dll");
        if (File.Exists(path))
          return path;
      }
      return "";
    }

    public static string[] FindAssembliesReferencedBy(string[] paths, string[] foldersToSearch, BuildTarget target)
    {
      List<string> alreadyFoundAssemblies = new List<string>();
      string[] allAssemblyPaths = paths;
      Dictionary<string, AssemblyDefinition> cache = new Dictionary<string, AssemblyDefinition>();
      for (int index = 0; index < paths.Length; ++index)
        AssemblyHelper.AddReferencedAssembliesRecurse(paths[index], alreadyFoundAssemblies, allAssemblyPaths, foldersToSearch, cache, target);
      for (int index = 0; index < paths.Length; ++index)
        alreadyFoundAssemblies.Remove(paths[index]);
      return alreadyFoundAssemblies.ToArray();
    }

    public static string[] FindAssembliesReferencedBy(string path, string[] foldersToSearch, BuildTarget target)
    {
      return AssemblyHelper.FindAssembliesReferencedBy(new string[1]{ path }, foldersToSearch, target);
    }

    public static bool IsUnityEngineModule(string assemblyName)
    {
      return assemblyName.EndsWith("Module") && assemblyName.StartsWith("UnityEngine.");
    }

    private static bool IsTypeAUserExtendedScript(AssemblyDefinition assembly, TypeReference type)
    {
      if (type == null || type.FullName == "System.Object")
        return false;
      Assembly assembly1 = (Assembly) null;
      if (type.Scope.Name == "UnityEngine" || type.Scope.Name == "UnityEngine.CoreModule")
        assembly1 = typeof (MonoBehaviour).Assembly;
      else if (type.Scope.Name == "UnityEditor")
        assembly1 = typeof (EditorWindow).Assembly;
      else if (type.Scope.Name == "UnityEngine.UI")
        assembly1 = AssemblyHelper.FindLoadedAssemblyWithName("UnityEngine.UI");
      if (assembly1 != null)
      {
        string name = !type.IsGenericInstance ? type.FullName : type.Namespace + "." + type.Name;
        System.Type type1 = assembly1.GetType(name);
        if (type1 != null && (type1 == typeof (MonoBehaviour) || type1.IsSubclassOf(typeof (MonoBehaviour)) || (type1 == typeof (ScriptableObject) || type1.IsSubclassOf(typeof (ScriptableObject))) || (type1 == typeof (ScriptedImporter) || type1.IsSubclassOf(typeof (ScriptedImporter)))))
          return true;
      }
      TypeDefinition typeDefinition = (TypeDefinition) null;
      try
      {
        typeDefinition = type.Resolve();
      }
      catch (AssemblyResolutionException ex)
      {
      }
      if (typeDefinition != null)
        return AssemblyHelper.IsTypeAUserExtendedScript(assembly, typeDefinition.BaseType);
      return false;
    }

    public static void ExtractAllClassesThatAreUserExtendedScripts(string path, out string[] classNamesArray, out string[] classNameSpacesArray, out string[] originalClassNameSpacesArray)
    {
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      List<string> stringList3 = new List<string>();
      ReaderParameters parameters = new ReaderParameters();
      DefaultAssemblyResolver assemblyResolver = new DefaultAssemblyResolver();
      assemblyResolver.AddSearchDirectory(Path.GetDirectoryName(path));
      PrecompiledAssembly[] precompiledAssemblies = InternalEditorUtility.GetPrecompiledAssemblies(true, EditorUserBuildSettings.activeBuildTargetGroup, EditorUserBuildSettings.activeBuildTarget);
      HashSet<string> stringSet = new HashSet<string>();
      foreach (PrecompiledAssembly precompiledAssembly in precompiledAssemblies)
        stringSet.Add(Path.GetDirectoryName(precompiledAssembly.Path));
      foreach (string directory in stringSet)
        assemblyResolver.AddSearchDirectory(directory);
      parameters.AssemblyResolver = (IAssemblyResolver) assemblyResolver;
      AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(path, parameters);
      foreach (ModuleDefinition module in assembly.Modules)
      {
        foreach (TypeDefinition type in module.Types)
        {
          TypeReference baseType = type.BaseType;
          try
          {
            if (AssemblyHelper.IsTypeAUserExtendedScript(assembly, baseType))
            {
              stringList1.Add(type.Name);
              stringList2.Add(type.Namespace);
              string empty = string.Empty;
              Mono.Cecil.CustomAttribute customAttribute = type.CustomAttributes.SingleOrDefault<Mono.Cecil.CustomAttribute>((Func<Mono.Cecil.CustomAttribute, bool>) (a => a.AttributeType.FullName == typeof (MovedFromAttribute).FullName));
              if (customAttribute != null)
                empty = (string) customAttribute.ConstructorArguments[0].Value;
              stringList3.Add(empty);
            }
          }
          catch (Exception ex)
          {
            UnityEngine.Debug.LogError((object) ("Failed to extract " + type.FullName + " class of base type " + baseType.FullName + " when inspecting " + path));
          }
        }
      }
      classNamesArray = stringList1.ToArray();
      classNameSpacesArray = stringList2.ToArray();
      originalClassNameSpacesArray = stringList3.ToArray();
    }

    public static AssemblyTypeInfoGenerator.ClassInfo[] ExtractAssemblyTypeInfo(BuildTarget targetPlatform, bool isEditor, string assemblyPathName, string[] searchDirs)
    {
      try
      {
        ICompilationExtension compilationExtension = ModuleManager.GetCompilationExtension(ModuleManager.GetTargetStringFromBuildTarget(targetPlatform));
        string[] extraAssemblyPaths = compilationExtension.GetCompilerExtraAssemblyPaths(isEditor, assemblyPathName);
        if (extraAssemblyPaths != null && extraAssemblyPaths.Length > 0)
        {
          List<string> stringList = new List<string>((IEnumerable<string>) searchDirs);
          stringList.AddRange((IEnumerable<string>) extraAssemblyPaths);
          searchDirs = stringList.ToArray();
        }
        IAssemblyResolver assemblyResolver = compilationExtension.GetAssemblyResolver(isEditor, assemblyPathName, searchDirs);
        return (assemblyResolver != null ? new AssemblyTypeInfoGenerator(assemblyPathName, assemblyResolver) : new AssemblyTypeInfoGenerator(assemblyPathName, searchDirs)).GatherClassInfo();
      }
      catch (Exception ex)
      {
        throw new Exception("ExtractAssemblyTypeInfo: Failed to process " + assemblyPathName + ", " + (object) ex);
      }
    }

    internal static System.Type[] GetTypesFromAssembly(Assembly assembly)
    {
      if (assembly == null)
        return new System.Type[0];
      try
      {
        return assembly.GetTypes();
      }
      catch (ReflectionTypeLoadException ex)
      {
        return new System.Type[0];
      }
    }

    [DebuggerHidden]
    internal static IEnumerable<T> FindImplementors<T>(Assembly assembly) where T : class
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssemblyHelper.\u003CFindImplementors\u003Ec__Iterator0<T> implementorsCIterator0 = new AssemblyHelper.\u003CFindImplementors\u003Ec__Iterator0<T>() { assembly = assembly };
      // ISSUE: reference to a compiler-generated field
      implementorsCIterator0.\u0024PC = -2;
      return (IEnumerable<T>) implementorsCIterator0;
    }

    public static bool IsManagedAssembly(string file)
    {
      DllType dllType = InternalEditorUtility.DetectDotNetDll(file);
      return dllType != DllType.Unknown && dllType != DllType.Native;
    }

    public static bool IsInternalAssembly(string file)
    {
      return ModuleManager.IsRegisteredModule(file) || ((IEnumerable<string>) ModuleUtils.GetAdditionalReferencesForUserScripts()).Any<string>((Func<string, bool>) (p => p.Equals(file)));
    }

    internal static ICollection<string> FindAssemblies(string basePath)
    {
      return AssemblyHelper.FindAssemblies(basePath, 10);
    }

    internal static ICollection<string> FindAssemblies(string basePath, int maxDepth)
    {
      List<string> stringList = new List<string>();
      if (maxDepth == 0)
        return (ICollection<string>) stringList;
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(basePath);
        stringList.AddRange(((IEnumerable<FileInfo>) directoryInfo.GetFiles()).Where<FileInfo>((Func<FileInfo, bool>) (file => AssemblyHelper.IsManagedAssembly(file.FullName))).Select<FileInfo, string>((Func<FileInfo, string>) (file => file.FullName)));
        foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
          stringList.AddRange((IEnumerable<string>) AssemblyHelper.FindAssemblies(directory.FullName, maxDepth - 1));
      }
      catch (Exception ex)
      {
      }
      return (ICollection<string>) stringList;
    }
  }
}
