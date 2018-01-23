// Decompiled with JetBrains decompiler
// Type: UnityEditor.MonoAssemblyStripping
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor.Utils;

namespace UnityEditor
{
  internal class MonoAssemblyStripping
  {
    private static void ReplaceFile(string src, string dst)
    {
      if (File.Exists(dst))
        FileUtil.DeleteFileOrDirectory(dst);
      FileUtil.CopyFileOrDirectory(src, dst);
    }

    public static void MonoCilStrip(BuildTarget buildTarget, string managedLibrariesDirectory, string[] fileNames)
    {
      string str = Path.Combine(MonoInstallationFinder.GetProfileDirectory(BuildPipeline.CompatibilityProfileToClassLibFolder(ApiCompatibilityLevel.NET_4_6), "MonoBleedingEdge"), "mono-cil-strip.exe");
      foreach (string fileName in fileNames)
      {
        Process process = MonoProcessUtility.PrepareMonoProcessBleedingEdge(managedLibrariesDirectory);
        string path2 = fileName + ".out";
        process.StartInfo.Arguments = "\"" + str + "\"";
        ProcessStartInfo startInfo = process.StartInfo;
        startInfo.Arguments = startInfo.Arguments + " \"" + fileName + "\" \"" + fileName + ".out\"";
        MonoProcessUtility.RunMonoProcess(process, "byte code stripper", Path.Combine(managedLibrariesDirectory, path2));
        MonoAssemblyStripping.ReplaceFile(managedLibrariesDirectory + "/" + path2, managedLibrariesDirectory + "/" + fileName);
        File.Delete(managedLibrariesDirectory + "/" + path2);
      }
    }

    public static string GenerateLinkXmlToPreserveDerivedTypes(string stagingArea, string librariesFolder, RuntimeClassRegistry usedClasses)
    {
      string fullPath = Path.GetFullPath(Path.Combine(stagingArea, "preserved_derived_types.xml"));
      using (TextWriter textWriter = (TextWriter) new StreamWriter(fullPath))
      {
        textWriter.WriteLine("<linker>");
        foreach (AssemblyDefinition collectAllAssembly in MonoAssemblyStripping.CollectAllAssemblies(librariesFolder, usedClasses))
        {
          if (!AssemblyHelper.IsUnityEngineModule(collectAllAssembly.Name.Name))
          {
            HashSet<TypeDefinition> typesToPreserve = new HashSet<TypeDefinition>();
            MonoAssemblyStripping.CollectBlackListTypes(typesToPreserve, (IList<TypeDefinition>) collectAllAssembly.MainModule.Types, usedClasses.GetAllManagedBaseClassesAsString());
            if (typesToPreserve.Count != 0)
            {
              textWriter.WriteLine("<assembly fullname=\"{0}\">", (object) collectAllAssembly.Name.Name);
              foreach (TypeDefinition typeDefinition in typesToPreserve)
                textWriter.WriteLine("<type fullname=\"{0}\" preserve=\"all\"/>", (object) typeDefinition.FullName);
              textWriter.WriteLine("</assembly>");
            }
          }
        }
        textWriter.WriteLine("</linker>");
      }
      return fullPath;
    }

    public static IEnumerable<AssemblyDefinition> CollectAllAssemblies(string librariesFolder, RuntimeClassRegistry usedClasses)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MonoAssemblyStripping.\u003CCollectAllAssemblies\u003Ec__AnonStorey0 assembliesCAnonStorey0 = new MonoAssemblyStripping.\u003CCollectAllAssemblies\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      assembliesCAnonStorey0.usedClasses = usedClasses;
      // ISSUE: reference to a compiler-generated field
      assembliesCAnonStorey0.resolver = new DefaultAssemblyResolver();
      // ISSUE: reference to a compiler-generated field
      assembliesCAnonStorey0.resolver.RemoveSearchDirectory(".");
      // ISSUE: reference to a compiler-generated field
      assembliesCAnonStorey0.resolver.RemoveSearchDirectory("bin");
      // ISSUE: reference to a compiler-generated field
      assembliesCAnonStorey0.resolver.AddSearchDirectory(librariesFolder);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return (IEnumerable<AssemblyDefinition>) MonoAssemblyStripping.CollectAssembliesRecursive(((IEnumerable<string>) assembliesCAnonStorey0.usedClasses.GetUserAssemblies()).Where<string>(new Func<string, bool>(assembliesCAnonStorey0.\u003C\u003Em__0)).Select<string, AssemblyNameReference>((Func<string, AssemblyNameReference>) (file => AssemblyNameReference.Parse(Path.GetFileNameWithoutExtension(file)))).Select<AssemblyNameReference, AssemblyDefinition>(new Func<AssemblyNameReference, AssemblyDefinition>(assembliesCAnonStorey0.\u003C\u003Em__1)).Where<AssemblyDefinition>((Func<AssemblyDefinition, bool>) (a => a != null)));
    }

    private static HashSet<AssemblyDefinition> CollectAssembliesRecursive(IEnumerable<AssemblyDefinition> assemblies)
    {
      HashSet<AssemblyDefinition> source = new HashSet<AssemblyDefinition>(assemblies, (IEqualityComparer<AssemblyDefinition>) new MonoAssemblyStripping.AssemblyDefinitionComparer());
      int num = 0;
      while (source.Count > num)
      {
        num = source.Count;
        source.UnionWith(((IEnumerable<AssemblyDefinition>) source.ToArray<AssemblyDefinition>()).SelectMany<AssemblyDefinition, AssemblyDefinition>((Func<AssemblyDefinition, IEnumerable<AssemblyDefinition>>) (a => MonoAssemblyStripping.ResolveAssemblyReferences(a))));
      }
      return source;
    }

    public static IEnumerable<AssemblyDefinition> ResolveAssemblyReferences(AssemblyDefinition assembly)
    {
      return MonoAssemblyStripping.ResolveAssemblyReferences(assembly.MainModule.AssemblyResolver, (IEnumerable<AssemblyNameReference>) assembly.MainModule.AssemblyReferences);
    }

    public static IEnumerable<AssemblyDefinition> ResolveAssemblyReferences(IAssemblyResolver resolver, IEnumerable<AssemblyNameReference> assemblyReferences)
    {
      return assemblyReferences.Select<AssemblyNameReference, AssemblyDefinition>((Func<AssemblyNameReference, AssemblyDefinition>) (reference => MonoAssemblyStripping.ResolveAssemblyReference(resolver, reference))).Where<AssemblyDefinition>((Func<AssemblyDefinition, bool>) (a => a != null));
    }

    public static AssemblyDefinition ResolveAssemblyReference(IAssemblyResolver resolver, AssemblyNameReference assemblyName)
    {
      try
      {
        return resolver.Resolve(assemblyName, new ReaderParameters() { AssemblyResolver = resolver, ApplyWindowsRuntimeProjections = true });
      }
      catch (AssemblyResolutionException ex)
      {
        if (AssemblyHelper.IsUnityEngineModule(assemblyName.Name) || ex.AssemblyReference.IsWindowsRuntime)
          return (AssemblyDefinition) null;
        throw;
      }
    }

    private static void CollectBlackListTypes(HashSet<TypeDefinition> typesToPreserve, IList<TypeDefinition> types, List<string> baseTypes)
    {
      if (types == null)
        return;
      foreach (TypeDefinition type in (IEnumerable<TypeDefinition>) types)
      {
        if (type != null)
        {
          foreach (string baseType in baseTypes)
          {
            if (MonoAssemblyStripping.DoesTypeEnheritFrom((TypeReference) type, baseType))
            {
              typesToPreserve.Add(type);
              break;
            }
          }
          MonoAssemblyStripping.CollectBlackListTypes(typesToPreserve, (IList<TypeDefinition>) type.NestedTypes, baseTypes);
        }
      }
    }

    private static bool DoesTypeEnheritFrom(TypeReference type, string typeName)
    {
      TypeDefinition typeDefinition;
      for (; type != null; type = typeDefinition.BaseType)
      {
        if (type.FullName == typeName)
          return true;
        typeDefinition = type.Resolve();
        if (typeDefinition == null)
          return false;
      }
      return false;
    }

    private class AssemblyDefinitionComparer : IEqualityComparer<AssemblyDefinition>
    {
      public bool Equals(AssemblyDefinition x, AssemblyDefinition y)
      {
        return x.FullName == y.FullName;
      }

      public int GetHashCode(AssemblyDefinition obj)
      {
        return obj.FullName.GetHashCode();
      }
    }
  }
}
