// Decompiled with JetBrains decompiler
// Type: UnityEditor.CodeStrippingUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Analytics;
using UnityEditor.BuildReporting;
using UnityEditor.Scripting.ScriptCompilation;
using UnityEditor.Utils;
using UnityEditorInternal;
using UnityEditorInternal.VR;

namespace UnityEditor
{
  internal class CodeStrippingUtils
  {
    private static UnityType s_GameManagerTypeInfo = (UnityType) null;
    private static string[] s_blackListNativeClassNames = new string[14]{ "Behaviour", "PreloadData", "Material", "Cubemap", "Texture3D", "Texture2DArray", "RenderTexture", "Mesh", "MeshFilter", "MeshRenderer", "Sprite", "LowerResBlitTexture", "Transform", "RectTransform" };
    private static readonly Dictionary<string, string> s_blackListNativeClassesDependencyNames = new Dictionary<string, string>() { { "ParticleSystemRenderer", "ParticleSystem" } };
    private static readonly string[] s_TreatedAsUserAssemblies = new string[1]{ "UnityEngine.Analytics.dll" };
    private static UnityType[] s_blackListNativeClasses;
    private static Dictionary<UnityType, UnityType> s_blackListNativeClassesDependency;

    private static UnityType GameManagerTypeInfo
    {
      get
      {
        if (CodeStrippingUtils.s_GameManagerTypeInfo == null)
          CodeStrippingUtils.s_GameManagerTypeInfo = CodeStrippingUtils.FindTypeByNameChecked("GameManager", "initializing code stripping utils");
        return CodeStrippingUtils.s_GameManagerTypeInfo;
      }
    }

    public static UnityType[] BlackListNativeClasses
    {
      get
      {
        if (CodeStrippingUtils.s_blackListNativeClasses == null)
          CodeStrippingUtils.s_blackListNativeClasses = ((IEnumerable<string>) CodeStrippingUtils.s_blackListNativeClassNames).Select<string, UnityType>((Func<string, UnityType>) (typeName => CodeStrippingUtils.FindTypeByNameChecked(typeName, "code stripping blacklist native class"))).ToArray<UnityType>();
        return CodeStrippingUtils.s_blackListNativeClasses;
      }
    }

    public static Dictionary<UnityType, UnityType> BlackListNativeClassesDependency
    {
      get
      {
        if (CodeStrippingUtils.s_blackListNativeClassesDependency == null)
        {
          CodeStrippingUtils.s_blackListNativeClassesDependency = new Dictionary<UnityType, UnityType>();
          foreach (KeyValuePair<string, string> classesDependencyName in CodeStrippingUtils.s_blackListNativeClassesDependencyNames)
            CodeStrippingUtils.BlackListNativeClassesDependency.Add(CodeStrippingUtils.FindTypeByNameChecked(classesDependencyName.Key, "code stripping blacklist native class dependency key"), CodeStrippingUtils.FindTypeByNameChecked(classesDependencyName.Value, "code stripping blacklist native class dependency value"));
        }
        return CodeStrippingUtils.s_blackListNativeClassesDependency;
      }
    }

    private static UnityType FindTypeByNameChecked(string name, string msg)
    {
      UnityType typeByName = UnityType.FindTypeByName(name);
      if (typeByName == null)
        throw new ArgumentException(string.Format("Could not map typename '{0}' to type info ({1})", (object) name, (object) (msg ?? "no context")));
      return typeByName;
    }

    public static HashSet<string> GetModulesFromICalls(string icallsListFile)
    {
      string[] strArray = File.ReadAllLines(icallsListFile);
      HashSet<string> stringSet = new HashSet<string>();
      foreach (string icall in strArray)
      {
        string icallModule = ModuleMetadata.GetICallModule(icall);
        if (!string.IsNullOrEmpty(icallModule))
          stringSet.Add(icallModule);
      }
      return stringSet;
    }

    public static void InjectCustomDependencies(BuildTarget target, StrippingInfo strippingInfo, HashSet<UnityType> nativeClasses, HashSet<string> nativeModules)
    {
      UnityType typeByName1 = UnityType.FindTypeByName("UnityConnectSettings");
      UnityType typeByName2 = UnityType.FindTypeByName("CloudWebServicesManager");
      if ((nativeClasses.Contains(typeByName1) || nativeClasses.Contains(typeByName2)) && PlayerSettings.submitAnalytics)
      {
        strippingInfo.RegisterDependency("UnityConnectSettings", "Required by HW Statistics (See Player Settings)");
        strippingInfo.RegisterDependency("CloudWebServicesManager", "Required by HW Statistics (See Player Settings)");
        strippingInfo.SetIcon("Required by HW Statistics (See Player Settings)", "class/PlayerSettings");
      }
      UnityType typeByName3 = UnityType.FindTypeByName("UnityAnalyticsManager");
      if (nativeClasses.Contains(typeByName3) && AnalyticsSettings.enabled)
      {
        strippingInfo.RegisterDependency("UnityAnalyticsManager", "Required by Unity Analytics (See Services Window)");
        strippingInfo.SetIcon("Required by Unity Analytics (See Services Window)", "class/PlayerSettings");
      }
      if (!VRModule.ShouldInjectVRDependenciesForBuildTarget(target))
        return;
      nativeModules.Add("VR");
      strippingInfo.RegisterDependency("VR", "Required by Scripts");
      strippingInfo.SetIcon("Required because VR is enabled in PlayerSettings", "class/PlayerSettings");
    }

    public static void GenerateDependencies(string strippedAssemblyDir, string icallsListFile, RuntimeClassRegistry rcr, bool doStripping, out HashSet<UnityType> nativeClasses, out HashSet<string> nativeModules, IIl2CppPlatformProvider platformProvider)
    {
      StrippingInfo strippingInfo = platformProvider != null ? StrippingInfo.GetBuildReportData(platformProvider.buildReport) : (StrippingInfo) null;
      string[] userAssemblies = CodeStrippingUtils.GetUserAssemblies(strippedAssemblyDir);
      nativeClasses = !doStripping ? (HashSet<UnityType>) null : CodeStrippingUtils.GenerateNativeClassList(rcr, strippedAssemblyDir, userAssemblies, strippingInfo);
      if (nativeClasses != null)
        CodeStrippingUtils.ExcludeModuleManagers(ref nativeClasses);
      nativeModules = CodeStrippingUtils.GetNativeModulesToRegister(nativeClasses, strippingInfo);
      if (nativeClasses != null && icallsListFile != null)
      {
        HashSet<string> modulesFromIcalls = CodeStrippingUtils.GetModulesFromICalls(icallsListFile);
        foreach (string str in modulesFromIcalls)
        {
          if (!nativeModules.Contains(str) && (UnityEngine.Object) strippingInfo != (UnityEngine.Object) null)
            strippingInfo.RegisterDependency(StrippingInfo.ModuleName(str), "Required by Scripts");
          foreach (UnityType moduleType in ModuleMetadata.GetModuleTypes(str))
          {
            if (moduleType.IsDerivedFrom(CodeStrippingUtils.GameManagerTypeInfo))
              nativeClasses.Add(moduleType);
          }
        }
        nativeModules.UnionWith((IEnumerable<string>) modulesFromIcalls);
      }
      CodeStrippingUtils.ApplyManualStrippingOverrides(nativeClasses, nativeModules, strippingInfo);
      bool flag = true;
      if (platformProvider != null)
      {
        while (flag)
        {
          flag = false;
          foreach (string str in nativeModules.ToList<string>())
          {
            string[] moduleDependencies = ModuleMetadata.GetModuleDependencies(str);
            if (moduleDependencies != null)
            {
              foreach (string module in moduleDependencies)
              {
                if (!nativeModules.Contains(module))
                {
                  nativeModules.Add(module);
                  flag = true;
                }
                if ((UnityEngine.Object) strippingInfo != (UnityEngine.Object) null)
                {
                  string key = StrippingInfo.ModuleName(str);
                  strippingInfo.RegisterDependency(StrippingInfo.ModuleName(module), "Required by " + key);
                  if (strippingInfo.icons.ContainsKey(key))
                    strippingInfo.SetIcon("Required by " + key, strippingInfo.icons[key]);
                }
              }
            }
          }
        }
      }
      new AssemblyReferenceChecker().CollectReferencesFromRoots(strippedAssemblyDir, (IEnumerable<string>) userAssemblies, true, 0.0f, true);
      if ((UnityEngine.Object) strippingInfo != (UnityEngine.Object) null)
      {
        foreach (string module in nativeModules)
          strippingInfo.AddModule(StrippingInfo.ModuleName(module));
        strippingInfo.AddModule(StrippingInfo.ModuleName("Core"));
      }
      if (nativeClasses == null || !((UnityEngine.Object) strippingInfo != (UnityEngine.Object) null))
        return;
      CodeStrippingUtils.InjectCustomDependencies(platformProvider.target, strippingInfo, nativeClasses, nativeModules);
    }

    public static void ApplyManualStrippingOverrides(HashSet<UnityType> nativeClasses, HashSet<string> nativeModules, StrippingInfo strippingInfo)
    {
      foreach (string moduleName in ModuleMetadata.GetModuleNames())
      {
        switch (ModuleMetadata.GetModuleIncludeSettingForModule(moduleName))
        {
          case ModuleIncludeSetting.ForceExclude:
            if (nativeModules.Contains(moduleName))
            {
              nativeModules.Remove(moduleName);
              foreach (UnityType moduleType in ModuleMetadata.GetModuleTypes(moduleName))
              {
                if (nativeClasses.Contains(moduleType))
                  nativeClasses.Remove(moduleType);
              }
              if ((UnityEngine.Object) strippingInfo != (UnityEngine.Object) null)
                strippingInfo.modules.Remove(StrippingInfo.ModuleName(moduleName));
            }
            break;
          case ModuleIncludeSetting.ForceInclude:
            nativeModules.Add(moduleName);
            foreach (UnityType moduleType in ModuleMetadata.GetModuleTypes(moduleName))
            {
              nativeClasses.Add(moduleType);
              if ((UnityEngine.Object) strippingInfo != (UnityEngine.Object) null)
              {
                strippingInfo.RegisterDependency(moduleType.name, "Force included module");
                strippingInfo.RegisterDependency(StrippingInfo.ModuleName(moduleName), moduleType.name);
              }
            }
            if ((UnityEngine.Object) strippingInfo != (UnityEngine.Object) null)
            {
              strippingInfo.RegisterDependency(StrippingInfo.ModuleName(moduleName), "Force included module");
              break;
            }
            break;
        }
      }
    }

    public static string GetModuleWhitelist(string module, string moduleStrippingInformationFolder)
    {
      return Paths.Combine(moduleStrippingInformationFolder, module + ".xml");
    }

    public static void WriteModuleAndClassRegistrationFile(string strippedAssemblyDir, string icallsListFile, string outputDir, RuntimeClassRegistry rcr, IEnumerable<UnityType> classesToSkip, IIl2CppPlatformProvider platformProvider)
    {
      bool stripEngineCode = PlayerSettings.stripEngineCode;
      HashSet<UnityType> nativeClasses;
      HashSet<string> nativeModules;
      CodeStrippingUtils.GenerateDependencies(strippedAssemblyDir, icallsListFile, rcr, stripEngineCode, out nativeClasses, out nativeModules, platformProvider);
      CodeStrippingUtils.WriteModuleAndClassRegistrationFile(Path.Combine(outputDir, "UnityClassRegistration.cpp"), nativeModules, nativeClasses, new HashSet<UnityType>(classesToSkip));
    }

    public static HashSet<string> GetNativeModulesToRegister(HashSet<UnityType> nativeClasses, StrippingInfo strippingInfo)
    {
      return nativeClasses != null ? CodeStrippingUtils.GetRequiredStrippableModules(nativeClasses, strippingInfo) : CodeStrippingUtils.GetAllStrippableModules();
    }

    private static HashSet<string> GetAllStrippableModules()
    {
      HashSet<string> stringSet = new HashSet<string>();
      foreach (string moduleName in ModuleMetadata.GetModuleNames())
      {
        if (ModuleMetadata.GetModuleStrippable(moduleName))
          stringSet.Add(moduleName);
      }
      return stringSet;
    }

    private static HashSet<string> GetRequiredStrippableModules(HashSet<UnityType> nativeClasses, StrippingInfo strippingInfo)
    {
      HashSet<UnityType> unityTypeSet1 = new HashSet<UnityType>();
      HashSet<string> stringSet = new HashSet<string>();
      foreach (string moduleName in ModuleMetadata.GetModuleNames())
      {
        if (ModuleMetadata.GetModuleStrippable(moduleName))
        {
          HashSet<UnityType> unityTypeSet2 = new HashSet<UnityType>((IEnumerable<UnityType>) ModuleMetadata.GetModuleTypes(moduleName));
          if (nativeClasses.Overlaps((IEnumerable<UnityType>) unityTypeSet2))
          {
            stringSet.Add(moduleName);
            if ((UnityEngine.Object) strippingInfo != (UnityEngine.Object) null)
            {
              foreach (UnityType unityType in unityTypeSet2)
              {
                if (nativeClasses.Contains(unityType))
                {
                  strippingInfo.RegisterDependency(StrippingInfo.ModuleName(moduleName), unityType.name);
                  unityTypeSet1.Add(unityType);
                }
              }
            }
          }
        }
      }
      if ((UnityEngine.Object) strippingInfo != (UnityEngine.Object) null)
      {
        foreach (UnityType nativeClass in nativeClasses)
        {
          if (!unityTypeSet1.Contains(nativeClass))
            strippingInfo.RegisterDependency(StrippingInfo.ModuleName("Core"), nativeClass.name);
        }
      }
      return stringSet;
    }

    private static void ExcludeModuleManagers(ref HashSet<UnityType> nativeClasses)
    {
      foreach (string moduleName in ModuleMetadata.GetModuleNames())
      {
        if (ModuleMetadata.GetModuleStrippable(moduleName))
        {
          UnityType[] moduleTypes = ModuleMetadata.GetModuleTypes(moduleName);
          HashSet<UnityType> unityTypeSet1 = new HashSet<UnityType>();
          HashSet<UnityType> unityTypeSet2 = new HashSet<UnityType>();
          foreach (UnityType unityType in moduleTypes)
          {
            if (unityType.IsDerivedFrom(CodeStrippingUtils.GameManagerTypeInfo))
              unityTypeSet1.Add(unityType);
            else
              unityTypeSet2.Add(unityType);
          }
          if (unityTypeSet2.Count != 0)
          {
            if (!nativeClasses.Overlaps((IEnumerable<UnityType>) unityTypeSet2))
            {
              foreach (UnityType unityType in unityTypeSet1)
                nativeClasses.Remove(unityType);
            }
            else
            {
              foreach (UnityType unityType in unityTypeSet1)
                nativeClasses.Add(unityType);
            }
          }
        }
      }
    }

    private static HashSet<UnityType> GenerateNativeClassList(RuntimeClassRegistry rcr, string directory, string[] rootAssemblies, StrippingInfo strippingInfo)
    {
      HashSet<UnityType> unityTypeSet1 = CodeStrippingUtils.CollectNativeClassListFromRoots(directory, rootAssemblies, strippingInfo);
      foreach (UnityType blackListNativeClass in CodeStrippingUtils.BlackListNativeClasses)
        unityTypeSet1.Add(blackListNativeClass);
      foreach (UnityType key in CodeStrippingUtils.BlackListNativeClassesDependency.Keys)
      {
        if (unityTypeSet1.Contains(key))
        {
          UnityType unityType = CodeStrippingUtils.BlackListNativeClassesDependency[key];
          unityTypeSet1.Add(unityType);
        }
      }
      foreach (string name in rcr.GetAllNativeClassesIncludingManagersAsString())
      {
        UnityType typeByName = UnityType.FindTypeByName(name);
        if (typeByName != null && typeByName.baseClass != null)
        {
          unityTypeSet1.Add(typeByName);
          if ((UnityEngine.Object) strippingInfo != (UnityEngine.Object) null && !typeByName.IsDerivedFrom(CodeStrippingUtils.GameManagerTypeInfo))
          {
            List<string> scenesForClass = rcr.GetScenesForClass(typeByName.persistentTypeID);
            if (scenesForClass != null)
            {
              foreach (string str in scenesForClass)
              {
                strippingInfo.RegisterDependency(name, str);
                if (str.EndsWith(".unity"))
                  strippingInfo.SetIcon(str, "class/SceneAsset");
                else
                  strippingInfo.SetIcon(str, "class/AssetBundle");
              }
            }
          }
        }
      }
      HashSet<UnityType> unityTypeSet2 = new HashSet<UnityType>();
      foreach (UnityType unityType1 in unityTypeSet1)
      {
        for (UnityType unityType2 = unityType1; unityType2.baseClass != null; unityType2 = unityType2.baseClass)
          unityTypeSet2.Add(unityType2);
      }
      return unityTypeSet2;
    }

    private static HashSet<UnityType> CollectNativeClassListFromRoots(string directory, string[] rootAssemblies, StrippingInfo strippingInfo)
    {
      return new HashSet<UnityType>(CodeStrippingUtils.CollectManagedTypeReferencesFromRoots(directory, rootAssemblies, strippingInfo).Select<string, UnityType>((Func<string, UnityType>) (name => UnityType.FindTypeByName(name))).Where<UnityType>((Func<UnityType, bool>) (klass => klass != null && klass.baseClass != null)));
    }

    private static HashSet<string> CollectManagedTypeReferencesFromRoots(string directory, string[] rootAssemblies, StrippingInfo strippingInfo)
    {
      HashSet<string> stringSet = new HashSet<string>();
      AssemblyReferenceChecker referenceChecker = new AssemblyReferenceChecker();
      bool collectMethods = false;
      bool ignoreSystemDlls = false;
      referenceChecker.CollectReferencesFromRoots(directory, (IEnumerable<string>) rootAssemblies, collectMethods, 0.0f, ignoreSystemDlls);
      string[] assemblyFileNames = referenceChecker.GetAssemblyFileNames();
      AssemblyDefinition[] assemblyDefinitions = referenceChecker.GetAssemblyDefinitions();
      foreach (AssemblyDefinition assemblyDefinition in assemblyDefinitions)
      {
        foreach (TypeDefinition type in assemblyDefinition.MainModule.Types)
        {
          if (type.Namespace.StartsWith("UnityEngine") && (type.Fields.Count > 0 || type.Methods.Count > 0 || type.Properties.Count > 0))
          {
            string name = type.Name;
            stringSet.Add(name);
            if ((UnityEngine.Object) strippingInfo != (UnityEngine.Object) null && !AssemblyReferenceChecker.IsIgnoredSystemDll(assemblyDefinition.Name.Name))
              strippingInfo.RegisterDependency(name, "Required by Scripts");
          }
        }
      }
      AssemblyDefinition assemblyDefinition1 = (AssemblyDefinition) null;
      AssemblyDefinition assemblyDefinition2 = (AssemblyDefinition) null;
      for (int index = 0; index < assemblyFileNames.Length; ++index)
      {
        if (assemblyFileNames[index] == "UnityEngine.dll")
          assemblyDefinition1 = assemblyDefinitions[index];
        if (assemblyFileNames[index] == "UnityEngine.UI.dll")
          assemblyDefinition2 = assemblyDefinitions[index];
      }
      foreach (AssemblyDefinition assemblyDefinition3 in assemblyDefinitions)
      {
        if (assemblyDefinition3 != assemblyDefinition1 && assemblyDefinition3 != assemblyDefinition2)
        {
          foreach (TypeReference typeReference in assemblyDefinition3.MainModule.GetTypeReferences())
          {
            if (typeReference.Namespace.StartsWith("UnityEngine"))
            {
              string name = typeReference.Name;
              stringSet.Add(name);
              if ((UnityEngine.Object) strippingInfo != (UnityEngine.Object) null && !AssemblyReferenceChecker.IsIgnoredSystemDll(assemblyDefinition3.Name.Name))
                strippingInfo.RegisterDependency(name, "Required by Scripts");
            }
          }
        }
      }
      return stringSet;
    }

    private static void WriteStaticallyLinkedModuleRegistration(TextWriter w, HashSet<string> nativeModules, HashSet<UnityType> nativeClasses)
    {
      w.WriteLine("void InvokeRegisterStaticallyLinkedModuleClasses()");
      w.WriteLine("{");
      if (nativeClasses == null)
      {
        w.WriteLine("\tvoid RegisterStaticallyLinkedModuleClasses();");
        w.WriteLine("\tRegisterStaticallyLinkedModuleClasses();");
      }
      else
        w.WriteLine("\t// Do nothing (we're in stripping mode)");
      w.WriteLine("}");
      w.WriteLine();
      w.WriteLine("void RegisterStaticallyLinkedModulesGranular()");
      w.WriteLine("{");
      foreach (string nativeModule in nativeModules)
      {
        w.WriteLine("\tvoid RegisterModule_" + nativeModule + "();");
        w.WriteLine("\tRegisterModule_" + nativeModule + "();");
        w.WriteLine();
      }
      w.WriteLine("}");
    }

    private static void WriteModuleAndClassRegistrationFile(string file, HashSet<string> nativeModules, HashSet<UnityType> nativeClasses, HashSet<UnityType> classesToSkip)
    {
      using (TextWriter w = (TextWriter) new StreamWriter(file))
      {
        w.WriteLine("template <typename T> void RegisterClass();");
        w.WriteLine("template <typename T> void RegisterStrippedType(int, const char*, const char*);");
        w.WriteLine();
        CodeStrippingUtils.WriteStaticallyLinkedModuleRegistration(w, nativeModules, nativeClasses);
        w.WriteLine();
        if (nativeClasses != null)
        {
          foreach (UnityType type in UnityType.GetTypes())
          {
            if (type.baseClass != null && !type.isEditorOnly && !classesToSkip.Contains(type))
            {
              if (type.hasNativeNamespace)
                w.Write("namespace {0} {{ class {1}; }} ", (object) type.nativeNamespace, (object) type.name);
              else
                w.Write("class {0}; ", (object) type.name);
              if (nativeClasses.Contains(type))
                w.WriteLine("template <> void RegisterClass<{0}>();", (object) type.qualifiedName);
              else
                w.WriteLine();
            }
          }
          w.WriteLine();
        }
        w.WriteLine("void RegisterAllClasses()");
        w.WriteLine("{");
        if (nativeClasses == null)
        {
          w.WriteLine("\tvoid RegisterAllClassesGranular();");
          w.WriteLine("\tRegisterAllClassesGranular();");
        }
        else
        {
          w.WriteLine("void RegisterBuiltinTypes();");
          w.WriteLine("RegisterBuiltinTypes();");
          w.WriteLine("\t//Total: {0} non stripped classes", (object) nativeClasses.Count);
          int num = 0;
          foreach (UnityType nativeClass in nativeClasses)
          {
            w.WriteLine("\t//{0}. {1}", (object) num, (object) nativeClass.qualifiedName);
            if (classesToSkip.Contains(nativeClass))
              w.WriteLine("\t//Skipping {0}", (object) nativeClass.qualifiedName);
            else
              w.WriteLine("\tRegisterClass<{0}>();", (object) nativeClass.qualifiedName);
            ++num;
          }
          w.WriteLine();
        }
        w.WriteLine("}");
        w.Close();
      }
    }

    public static string[] UserAssemblies
    {
      get
      {
        EditorCompilation.TargetAssemblyInfo[] targetAssemblies = EditorCompilationInterface.GetTargetAssemblies();
        string[] strArray = new string[targetAssemblies.Length + CodeStrippingUtils.s_TreatedAsUserAssemblies.Length];
        for (int index = 0; index < targetAssemblies.Length; ++index)
          strArray[index] = targetAssemblies[index].Name;
        for (int index = 0; index < CodeStrippingUtils.s_TreatedAsUserAssemblies.Length; ++index)
          strArray[targetAssemblies.Length + index] = CodeStrippingUtils.s_TreatedAsUserAssemblies[index];
        return strArray;
      }
    }

    private static string[] GetUserAssemblies(string strippedAssemblyDir)
    {
      List<string> stringList = new List<string>();
      foreach (string userAssembly in CodeStrippingUtils.UserAssemblies)
      {
        string[] files = Directory.GetFiles(strippedAssemblyDir, userAssembly, SearchOption.TopDirectoryOnly);
        stringList.AddRange(((IEnumerable<string>) files).Select<string, string>((Func<string, string>) (f => Path.GetFileName(f))));
      }
      return stringList.ToArray();
    }
  }
}
