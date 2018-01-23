// Decompiled with JetBrains decompiler
// Type: UnityEditor.Compilation.CompilationPipeline
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Scripting.ScriptCompilation;
using UnityEngine;

namespace UnityEditor.Compilation
{
  /// <summary>
  ///   <para>Methods and properties for script compilation pipeline.</para>
  /// </summary>
  public static class CompilationPipeline
  {
    private static AssemblyDefinitionPlatform[] assemblyDefinitionPlatforms;

    static CompilationPipeline()
    {
      CompilationPipeline.SubscribeToEvents(EditorCompilationInterface.Instance);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ClearEditorCompilationErrors();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void LogEditorCompilationError(string message, int instanceID);

    public static event Action<string> assemblyCompilationStarted;

    public static event Action<string, CompilerMessage[]> assemblyCompilationFinished;

    internal static void SubscribeToEvents(EditorCompilation editorCompilation)
    {
      editorCompilation.assemblyCompilationStarted += (Action<string>) (assemblyPath =>
      {
        try
        {
          // ISSUE: reference to a compiler-generated field
          if (CompilationPipeline.assemblyCompilationStarted == null)
            return;
          // ISSUE: reference to a compiler-generated field
          CompilationPipeline.assemblyCompilationStarted(assemblyPath);
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
        }
      });
      editorCompilation.assemblyCompilationFinished += (Action<string, CompilerMessage[]>) ((assemblyPath, messages) =>
      {
        try
        {
          // ISSUE: reference to a compiler-generated field
          if (CompilationPipeline.assemblyCompilationFinished == null)
            return;
          // ISSUE: reference to a compiler-generated field
          CompilationPipeline.assemblyCompilationFinished(assemblyPath, messages);
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
        }
      });
    }

    /// <summary>
    ///   <para>Get all script assemblies compiled by Unity.</para>
    /// </summary>
    /// <returns>
    ///   <para>Array of script assemblies compoiled by Unity.</para>
    /// </returns>
    public static Assembly[] GetAssemblies()
    {
      return CompilationPipeline.GetAssemblies(EditorCompilationInterface.Instance);
    }

    /// <summary>
    ///   <para>Returns the assembly name for a source (script) path. Returns null if there is no assembly name for the given script path.</para>
    /// </summary>
    /// <param name="sourceFilePath">Source (script) path.</param>
    /// <returns>
    ///   <para>Assembly name.</para>
    /// </returns>
    public static string GetAssemblyNameFromScriptPath(string sourceFilePath)
    {
      return CompilationPipeline.GetAssemblyNameFromScriptPath(EditorCompilationInterface.Instance, sourceFilePath);
    }

    /// <summary>
    ///   <para>Returns the assembly definition file path for a source (script) path. Returns null if there is no assembly definition file for the given script path.</para>
    /// </summary>
    /// <param name="sourceFilePath">Source (script) file path.</param>
    /// <returns>
    ///   <para>File path of assembly definition file.</para>
    /// </returns>
    public static string GetAssemblyDefinitionFilePathFromScriptPath(string sourceFilePath)
    {
      return CompilationPipeline.GetAssemblyDefinitionFilePathFromScriptPath(EditorCompilationInterface.Instance, sourceFilePath);
    }

    /// <summary>
    ///   <para>Returns the assembly definition file path from an assembly name. Returns null if there is no assembly definition file for the given assembly name.</para>
    /// </summary>
    /// <param name="assemblyName">Assembly name.</param>
    /// <returns>
    ///   <para>File path of assembly definition file.</para>
    /// </returns>
    public static string GetAssemblyDefinitionFilePathFromAssemblyName(string assemblyName)
    {
      return CompilationPipeline.GetAssemblyDefinitionFilePathFromAssemblyName(EditorCompilationInterface.Instance, assemblyName);
    }

    /// <summary>
    ///         <para>Returns all the platforms supported by assembly definition files.
    /// 
    /// See Also: AssemblyDefinitionPlatform.</para>
    ///       </summary>
    /// <returns>
    ///   <para>Platforms supported by assembly definition files.</para>
    /// </returns>
    public static AssemblyDefinitionPlatform[] GetAssemblyDefinitionPlatforms()
    {
      if (CompilationPipeline.assemblyDefinitionPlatforms == null)
      {
        CompilationPipeline.assemblyDefinitionPlatforms = ((IEnumerable<CustomScriptAssemblyPlatform>) CustomScriptAssembly.Platforms).Select<CustomScriptAssemblyPlatform, AssemblyDefinitionPlatform>((Func<CustomScriptAssemblyPlatform, AssemblyDefinitionPlatform>) (p => new AssemblyDefinitionPlatform(p.Name, p.DisplayName, p.BuildTarget))).ToArray<AssemblyDefinitionPlatform>();
        AssemblyDefinitionPlatform[] definitionPlatforms = CompilationPipeline.assemblyDefinitionPlatforms;
        // ISSUE: reference to a compiler-generated field
        if (CompilationPipeline.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CompilationPipeline.\u003C\u003Ef__mg\u0024cache0 = new Comparison<AssemblyDefinitionPlatform>(CompilationPipeline.CompareAssemblyDefinitionPlatformByDisplayName);
        }
        // ISSUE: reference to a compiler-generated field
        Comparison<AssemblyDefinitionPlatform> fMgCache0 = CompilationPipeline.\u003C\u003Ef__mg\u0024cache0;
        Array.Sort<AssemblyDefinitionPlatform>(definitionPlatforms, fMgCache0);
      }
      return CompilationPipeline.assemblyDefinitionPlatforms;
    }

    internal static Assembly[] GetAssemblies(EditorCompilation editorCompilation)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CompilationPipeline.\u003CGetAssemblies\u003Ec__AnonStorey0 assembliesCAnonStorey0 = new CompilationPipeline.\u003CGetAssemblies\u003Ec__AnonStorey0();
      ScriptAssembly[] scriptAssemblies = editorCompilation.GetAllEditorScriptAssemblies();
      Assembly[] assemblyArray = new Assembly[scriptAssemblies.Length];
      // ISSUE: reference to a compiler-generated field
      assembliesCAnonStorey0.scriptAssemblyToAssembly = new Dictionary<ScriptAssembly, Assembly>();
      for (int index = 0; index < scriptAssemblies.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        assembliesCAnonStorey0.scriptAssemblyToAssembly.Add(scriptAssemblies[index], assemblyArray[index]);
      }
      for (int index = 0; index < scriptAssemblies.Length; ++index)
      {
        ScriptAssembly scriptAssembly = scriptAssemblies[index];
        string withoutExtension = AssetPath.GetAssemblyNameWithoutExtension(scriptAssembly.Filename);
        string fullPath = scriptAssembly.FullPath;
        string[] files = scriptAssembly.Files;
        string[] defines = scriptAssembly.Defines;
        string[] references = scriptAssembly.References;
        // ISSUE: reference to a compiler-generated method
        Assembly[] array = ((IEnumerable<ScriptAssembly>) scriptAssembly.ScriptAssemblyReferences).Select<ScriptAssembly, Assembly>(new Func<ScriptAssembly, Assembly>(assembliesCAnonStorey0.\u003C\u003Em__0)).ToArray<Assembly>();
        AssemblyFlags flags = AssemblyFlags.None;
        if ((scriptAssembly.Flags & UnityEditor.Scripting.ScriptCompilation.AssemblyFlags.EditorOnly) == UnityEditor.Scripting.ScriptCompilation.AssemblyFlags.EditorOnly)
          flags |= AssemblyFlags.EditorAssembly;
        assemblyArray[index] = new Assembly(withoutExtension, fullPath, files, defines, array, references, flags);
      }
      return assemblyArray;
    }

    private static int CompareAssemblyDefinitionPlatformByDisplayName(AssemblyDefinitionPlatform p1, AssemblyDefinitionPlatform p2)
    {
      return string.Compare(p1.DisplayName, p2.DisplayName, StringComparison.OrdinalIgnoreCase);
    }

    internal static string GetAssemblyNameFromScriptPath(EditorCompilation editorCompilation, string sourceFilePath)
    {
      try
      {
        return editorCompilation.GetTargetAssembly(sourceFilePath).Name;
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }

    internal static string GetAssemblyDefinitionFilePathFromAssemblyName(EditorCompilation editorCompilation, string assemblyName)
    {
      try
      {
        return editorCompilation.FindCustomScriptAssemblyFromAssemblyName(assemblyName).FilePath;
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }

    internal static string GetAssemblyDefinitionFilePathFromScriptPath(EditorCompilation editorCompilation, string sourceFilePath)
    {
      try
      {
        CustomScriptAssembly assemblyFromScriptPath = editorCompilation.FindCustomScriptAssemblyFromScriptPath(sourceFilePath);
        return assemblyFromScriptPath == null ? (string) null : assemblyFromScriptPath.FilePath;
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }
  }
}
