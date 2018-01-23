// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Serialization.Weaver
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor.Modules;
using UnityEditor.Scripting.ScriptCompilation;
using UnityEditor.Utils;

namespace UnityEditor.Scripting.Serialization
{
  internal static class Weaver
  {
    private static ManagedProgram SerializationWeaverProgramWith(string arguments, string playerPackage)
    {
      return Weaver.ManagedProgramFor(playerPackage + "/SerializationWeaver/SerializationWeaver.exe", arguments);
    }

    private static ManagedProgram ManagedProgramFor(string exe, string arguments)
    {
      return new ManagedProgram(MonoInstallationFinder.GetMonoInstallation("MonoBleedingEdge"), (string) null, exe, arguments, false, (Action<ProcessStartInfo>) null);
    }

    private static ICompilationExtension GetCompilationExtension()
    {
      return ModuleManager.GetCompilationExtension(ModuleManager.GetTargetStringFromBuildTarget(EditorUserBuildSettings.activeBuildTarget));
    }

    private static void QueryAssemblyPathsAndResolver(ICompilationExtension compilationExtension, string file, bool editor, out string[] assemblyPaths, out IAssemblyResolver assemblyResolver)
    {
      assemblyResolver = compilationExtension.GetAssemblyResolver(editor, file, (string[]) null);
      assemblyPaths = ((IEnumerable<string>) compilationExtension.GetCompilerExtraAssemblyPaths(editor, file)).ToArray<string>();
    }

    public static bool WeaveUnetFromEditor(ScriptAssembly assembly, string assemblyDirectory, string outputDirectory, string unityEngine, string unityUNet, bool buildingForEditor)
    {
      if ((assembly.Flags & AssemblyFlags.EditorOnly) == AssemblyFlags.EditorOnly)
        return true;
      string str = Path.Combine(assemblyDirectory, assembly.Filename);
      string[] assemblyPaths;
      IAssemblyResolver assemblyResolver;
      Weaver.QueryAssemblyPathsAndResolver(Weaver.GetCompilationExtension(), str, buildingForEditor, out assemblyPaths, out assemblyResolver);
      return Weaver.WeaveInto(assembly, str, outputDirectory, unityEngine, unityUNet, assemblyPaths, assemblyResolver);
    }

    private static bool WeaveInto(ScriptAssembly assembly, string assemblyPath, string outputDirectory, string unityEngine, string unityUNet, string[] extraAssemblyPaths, IAssemblyResolver assemblyResolver)
    {
      string[] allReferences = assembly.GetAllReferences();
      string[] strArray = new string[((IEnumerable<string>) allReferences).Count<string>() + (extraAssemblyPaths == null ? 0 : extraAssemblyPaths.Length)];
      int index = 0;
      foreach (string path in allReferences)
        strArray[index++] = Path.GetDirectoryName(path);
      if (extraAssemblyPaths != null)
        extraAssemblyPaths.CopyTo((Array) strArray, index);
      try
      {
        string unityEngine1 = unityEngine;
        string unetDLL = unityUNet;
        string outputDirectory1 = outputDirectory;
        string[] assemblies = new string[1]{ assemblyPath };
        string[] extraAssemblyPaths1 = strArray;
        IAssemblyResolver assemblyResolver1 = assemblyResolver;
        // ISSUE: reference to a compiler-generated field
        if (Weaver.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Weaver.\u003C\u003Ef__mg\u0024cache0 = new Action<string>(UnityEngine.Debug.LogWarning);
        }
        // ISSUE: reference to a compiler-generated field
        Action<string> fMgCache0 = Weaver.\u003C\u003Ef__mg\u0024cache0;
        // ISSUE: reference to a compiler-generated field
        if (Weaver.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Weaver.\u003C\u003Ef__mg\u0024cache1 = new Action<string>(UnityEngine.Debug.LogError);
        }
        // ISSUE: reference to a compiler-generated field
        Action<string> fMgCache1 = Weaver.\u003C\u003Ef__mg\u0024cache1;
        if (!Unity.UNetWeaver.Program.Process(unityEngine1, unetDLL, outputDirectory1, assemblies, extraAssemblyPaths1, assemblyResolver1, fMgCache0, fMgCache1))
        {
          UnityEngine.Debug.LogError((object) "Failure generating network code.");
          return false;
        }
      }
      catch (Exception ex)
      {
        UnityEngine.Debug.LogError((object) ("Exception generating network code: " + ex.ToString() + " " + ex.StackTrace));
      }
      return true;
    }
  }
}
