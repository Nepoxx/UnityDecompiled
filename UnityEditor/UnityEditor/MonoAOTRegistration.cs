// Decompiled with JetBrains decompiler
// Type: UnityEditor.MonoAOTRegistration
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Utils;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class MonoAOTRegistration
  {
    private static void ExtractNativeMethodsFromTypes(ICollection<TypeDefinition> types, ArrayList res)
    {
      foreach (TypeDefinition type in (IEnumerable<TypeDefinition>) types)
      {
        foreach (MethodDefinition method in type.Methods)
        {
          if (method.IsStatic && method.IsPInvokeImpl && method.PInvokeInfo.Module.Name.Equals("__Internal"))
          {
            if (res.Contains((object) method.Name))
              throw new SystemException("Duplicate native method found : " + method.Name + ". Please check your source carefully.");
            res.Add((object) method.Name);
          }
        }
        if (type.HasNestedTypes)
          MonoAOTRegistration.ExtractNativeMethodsFromTypes((ICollection<TypeDefinition>) type.NestedTypes, res);
      }
    }

    private static ArrayList BuildNativeMethodList(AssemblyDefinition[] assemblies)
    {
      ArrayList res = new ArrayList();
      foreach (AssemblyDefinition assembly in assemblies)
      {
        if (!"System".Equals(assembly.Name.Name))
          MonoAOTRegistration.ExtractNativeMethodsFromTypes((ICollection<TypeDefinition>) assembly.MainModule.Types, res);
      }
      return res;
    }

    public static HashSet<string> BuildReferencedTypeList(AssemblyDefinition[] assemblies)
    {
      HashSet<string> stringSet = new HashSet<string>();
      foreach (AssemblyDefinition assembly in assemblies)
      {
        if (!assembly.Name.Name.StartsWith("System") && !assembly.Name.Name.Equals("UnityEngine"))
        {
          foreach (TypeReference typeReference in assembly.MainModule.GetTypeReferences())
            stringSet.Add(typeReference.FullName);
        }
      }
      return stringSet;
    }

    public static void WriteCPlusPlusFileForStaticAOTModuleRegistration(BuildTarget buildTarget, string file, CrossCompileOptions crossCompileOptions, bool advancedLic, string targetDevice, bool stripping, RuntimeClassRegistry usedClassRegistry, AssemblyReferenceChecker checker, string stagingAreaDataManaged)
    {
      MonoAOTRegistration.WriteCPlusPlusFileForStaticAOTModuleRegistration(buildTarget, file, crossCompileOptions, advancedLic, targetDevice, stripping, usedClassRegistry, checker, stagingAreaDataManaged, (IIl2CppPlatformProvider) null);
    }

    public static void WriteCPlusPlusFileForStaticAOTModuleRegistration(BuildTarget buildTarget, string file, CrossCompileOptions crossCompileOptions, bool advancedLic, string targetDevice, bool stripping, RuntimeClassRegistry usedClassRegistry, AssemblyReferenceChecker checker, string stagingAreaDataManaged, IIl2CppPlatformProvider platformProvider)
    {
      string icallsListFile = Path.Combine(stagingAreaDataManaged, "ICallSummary.txt");
      Runner.RunManagedProgram(Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Tools/InternalCallRegistrationWriter/InternalCallRegistrationWriter.exe"), string.Format("-assembly=\"{0}\" -summary=\"{1}\"", (object) ((IEnumerable<string>) Directory.GetFiles(stagingAreaDataManaged, "UnityEngine.*Module.dll")).Concat<string>((IEnumerable<string>) new string[1]
      {
        Path.Combine(stagingAreaDataManaged, "UnityEngine.dll")
      }).Aggregate<string>((Func<string, string, string>) ((dllArg, next) => dllArg + ";" + next)), (object) icallsListFile));
      HashSet<UnityType> nativeClasses;
      HashSet<string> nativeModules;
      CodeStrippingUtils.GenerateDependencies(Path.GetDirectoryName(stagingAreaDataManaged), icallsListFile, usedClassRegistry, stripping, out nativeClasses, out nativeModules, platformProvider);
      using (TextWriter output = (TextWriter) new StreamWriter(file))
      {
        string[] assemblyFileNames = checker.GetAssemblyFileNames();
        AssemblyDefinition[] assemblyDefinitions = checker.GetAssemblyDefinitions();
        bool flag = (crossCompileOptions & CrossCompileOptions.FastICall) != CrossCompileOptions.Dynamic;
        ArrayList arrayList = MonoAOTRegistration.BuildNativeMethodList(assemblyDefinitions);
        if (buildTarget == BuildTarget.iOS)
        {
          output.WriteLine("#include \"RegisterMonoModules.h\"");
          output.WriteLine("#include <stdio.h>");
        }
        output.WriteLine("");
        output.WriteLine("#if defined(TARGET_IPHONE_SIMULATOR) && TARGET_IPHONE_SIMULATOR");
        output.WriteLine("    #define DECL_USER_FUNC(f) void f() __attribute__((weak_import))");
        output.WriteLine("    #define REGISTER_USER_FUNC(f)\\");
        output.WriteLine("        do {\\");
        output.WriteLine("        if(f != NULL)\\");
        output.WriteLine("            mono_dl_register_symbol(#f, (void*)f);\\");
        output.WriteLine("        else\\");
        output.WriteLine("            ::printf_console(\"Symbol '%s' not found. Maybe missing implementation for Simulator?\\n\", #f);\\");
        output.WriteLine("        }while(0)");
        output.WriteLine("#else");
        output.WriteLine("    #define DECL_USER_FUNC(f) void f() ");
        output.WriteLine("    #if !defined(__arm64__)");
        output.WriteLine("    #define REGISTER_USER_FUNC(f) mono_dl_register_symbol(#f, (void*)&f)");
        output.WriteLine("    #else");
        output.WriteLine("        #define REGISTER_USER_FUNC(f)");
        output.WriteLine("    #endif");
        output.WriteLine("#endif");
        output.WriteLine("extern \"C\"\n{");
        output.WriteLine("    typedef void* gpointer;");
        output.WriteLine("    typedef int gboolean;");
        if (buildTarget == BuildTarget.iOS)
        {
          output.WriteLine("    const char*         UnityIPhoneRuntimeVersion = \"{0}\";", (object) Application.unityVersion);
          output.WriteLine("    void                mono_dl_register_symbol (const char* name, void *addr);");
          output.WriteLine("#if !defined(__arm64__)");
          output.WriteLine("    extern int          mono_ficall_flag;");
          output.WriteLine("#endif");
        }
        output.WriteLine("    void                mono_aot_register_module(gpointer *aot_info);");
        output.WriteLine("#if __ORBIS__ || SN_TARGET_PSP2");
        output.WriteLine("#define DLL_EXPORT __declspec(dllexport)");
        output.WriteLine("#else");
        output.WriteLine("#define DLL_EXPORT");
        output.WriteLine("#endif");
        output.WriteLine("#if !(TARGET_IPHONE_SIMULATOR)");
        output.WriteLine("    extern gboolean     mono_aot_only;");
        for (int index = 0; index < assemblyFileNames.Length; ++index)
        {
          string str1 = assemblyFileNames[index];
          string str2 = assemblyDefinitions[index].Name.Name.Replace(".", "_").Replace("-", "_").Replace(" ", "_");
          output.WriteLine("    extern gpointer*    mono_aot_module_{0}_info; // {1}", (object) str2, (object) str1);
        }
        output.WriteLine("#endif // !(TARGET_IPHONE_SIMULATOR)");
        IEnumerator enumerator1 = arrayList.GetEnumerator();
        try
        {
          while (enumerator1.MoveNext())
          {
            string current = (string) enumerator1.Current;
            output.WriteLine("    DECL_USER_FUNC({0});", (object) current);
          }
        }
        finally
        {
          IDisposable disposable;
          if ((disposable = enumerator1 as IDisposable) != null)
            disposable.Dispose();
        }
        output.WriteLine("}");
        output.WriteLine("DLL_EXPORT void RegisterMonoModules()");
        output.WriteLine("{");
        output.WriteLine("#if !(TARGET_IPHONE_SIMULATOR) && !defined(__arm64__)");
        output.WriteLine("    mono_aot_only = true;");
        if (buildTarget == BuildTarget.iOS)
          output.WriteLine("    mono_ficall_flag = {0};", !flag ? (object) "false" : (object) "true");
        foreach (AssemblyDefinition assemblyDefinition in assemblyDefinitions)
        {
          string str = assemblyDefinition.Name.Name.Replace(".", "_").Replace("-", "_").Replace(" ", "_");
          output.WriteLine("    mono_aot_register_module(mono_aot_module_{0}_info);", (object) str);
        }
        output.WriteLine("#endif // !(TARGET_IPHONE_SIMULATOR) && !defined(__arm64__)");
        output.WriteLine("");
        if (buildTarget == BuildTarget.iOS)
        {
          IEnumerator enumerator2 = arrayList.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              string current = (string) enumerator2.Current;
              output.WriteLine("    REGISTER_USER_FUNC({0});", (object) current);
            }
          }
          finally
          {
            IDisposable disposable;
            if ((disposable = enumerator2 as IDisposable) != null)
              disposable.Dispose();
          }
        }
        output.WriteLine("}");
        output.WriteLine("");
        if (buildTarget == BuildTarget.iOS)
        {
          List<AssemblyDefinition> assemblyDefinitionList = new List<AssemblyDefinition>();
          for (int index = 0; index < assemblyDefinitions.Length; ++index)
          {
            if (AssemblyHelper.IsUnityEngineModule(assemblyDefinitions[index].Name.Name))
              assemblyDefinitionList.Add(assemblyDefinitions[index]);
          }
          MonoAOTRegistration.GenerateRegisterInternalCalls(assemblyDefinitionList.ToArray(), output);
          MonoAOTRegistration.GenerateRegisterModules(nativeClasses, nativeModules, output, stripping);
          if (stripping && usedClassRegistry != null)
            MonoAOTRegistration.GenerateRegisterClassesForStripping(nativeClasses, output);
          else
            MonoAOTRegistration.GenerateRegisterClasses(nativeClasses, output);
        }
        output.Close();
      }
    }

    public static void GenerateRegisterModules(HashSet<UnityType> nativeClasses, HashSet<string> nativeModules, TextWriter output, bool strippingEnabled)
    {
      output.WriteLine("void InvokeRegisterStaticallyLinkedModuleClasses()");
      output.WriteLine("{");
      if (nativeClasses == null)
      {
        output.WriteLine("\tvoid RegisterStaticallyLinkedModuleClasses();");
        output.WriteLine("\tRegisterStaticallyLinkedModuleClasses();");
      }
      else
        output.WriteLine("\t// Do nothing (we're in stripping mode)");
      output.WriteLine("}");
      output.WriteLine();
      output.WriteLine("void RegisterStaticallyLinkedModulesGranular()");
      output.WriteLine("{");
      foreach (string nativeModule in nativeModules)
      {
        output.WriteLine("\tvoid RegisterModule_" + nativeModule + "();");
        output.WriteLine("\tRegisterModule_" + nativeModule + "();");
        output.WriteLine();
      }
      output.WriteLine("}\n");
    }

    public static void GenerateRegisterClassesForStripping(HashSet<UnityType> nativeClassesAndBaseClasses, TextWriter output)
    {
      output.WriteLine("template <typename T> void RegisterClass();");
      output.WriteLine("template <typename T> void RegisterStrippedType(int, const char*, const char*);");
      output.WriteLine();
      foreach (UnityType type in UnityType.GetTypes())
      {
        if (type.baseClass != null && !type.isEditorOnly)
        {
          if (!type.hasNativeNamespace)
            output.WriteLine("class {0};", (object) type.name);
          else
            output.WriteLine("namespace {0} {{ class {1}; }}", (object) type.nativeNamespace, (object) type.name);
          output.WriteLine();
        }
      }
      output.Write("void RegisterAllClasses() \n{\n");
      output.WriteLine("\tvoid RegisterBuiltinTypes();");
      output.WriteLine("\tRegisterBuiltinTypes();");
      output.WriteLine("\t// {0} Non stripped classes\n", (object) nativeClassesAndBaseClasses.Count);
      int num = 1;
      foreach (UnityType type in UnityType.GetTypes())
      {
        if (type.baseClass != null && !type.isEditorOnly && nativeClassesAndBaseClasses.Contains(type))
        {
          output.WriteLine("\t// {0}. {1}", (object) num++, (object) type.qualifiedName);
          output.WriteLine("\tRegisterClass<{0}>();\n", (object) type.qualifiedName);
        }
      }
      output.WriteLine();
      output.Write("\n}\n");
    }

    public static void GenerateRegisterClasses(HashSet<UnityType> allClasses, TextWriter output)
    {
      output.WriteLine("void RegisterAllClasses() \n{");
      output.WriteLine("\tvoid RegisterAllClassesGranular();");
      output.WriteLine("\tRegisterAllClassesGranular();");
      output.WriteLine("}");
    }

    public static void GenerateRegisterInternalCalls(AssemblyDefinition[] assemblies, TextWriter output)
    {
      output.Write("void RegisterAllStrippedInternalCalls ()\n{\n");
      foreach (AssemblyDefinition assembly in assemblies)
        MonoAOTRegistration.GenerateRegisterInternalCallsForTypes((IEnumerable<TypeDefinition>) assembly.MainModule.Types, output);
      output.Write("}\n\n");
    }

    private static void GenerateRegisterInternalCallsForTypes(IEnumerable<TypeDefinition> types, TextWriter output)
    {
      foreach (TypeDefinition type in types)
      {
        foreach (MethodDefinition method in type.Methods)
          MonoAOTRegistration.GenerateInternalCallMethod(type, method, output);
        MonoAOTRegistration.GenerateRegisterInternalCallsForTypes((IEnumerable<TypeDefinition>) type.NestedTypes, output);
      }
    }

    private static void GenerateInternalCallMethod(TypeDefinition typeDefinition, MethodDefinition method, TextWriter output)
    {
      if (!method.IsInternalCall)
        return;
      string str = (typeDefinition.FullName + "_" + method.Name).Replace('/', '_').Replace('.', '_');
      if (str.Contains("UnityEngine_Serialization"))
        return;
      output.WriteLine("\tvoid Register_{0} ();", (object) str);
      output.WriteLine("\tRegister_{0} ();", (object) str);
    }
  }
}
