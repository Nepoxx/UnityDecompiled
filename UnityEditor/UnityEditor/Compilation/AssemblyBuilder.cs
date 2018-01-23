// Decompiled with JetBrains decompiler
// Type: UnityEditor.Compilation.AssemblyBuilder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.Scripting.ScriptCompilation;
using UnityEngine;

namespace UnityEditor.Compilation
{
  /// <summary>
  ///   <para>Compiles scripts outside the Assets folder into a managed assembly that can be used inside the Assets folder.</para>
  /// </summary>
  public class AssemblyBuilder
  {
    private CompilationTask compilationTask;

    /// <summary>
    ///   <para>AssemblyBuilder constructor.</para>
    /// </summary>
    /// <param name="assemblyPath">Path of the output assembly. Relative to project root.</param>
    /// <param name="scriptPaths">Array of script paths to be compiled into the output assembly. Relative to project root.</param>
    public AssemblyBuilder(string assemblyPath, params string[] scriptPaths)
    {
      if (string.IsNullOrEmpty(assemblyPath))
        throw new ArgumentException("assemblyPath cannot be null or empty");
      if (scriptPaths == null || scriptPaths.Length == 0)
        throw new ArgumentException("scriptPaths cannot be null or empty");
      this.scriptPaths = scriptPaths;
      this.assemblyPath = assemblyPath;
      this.flags = AssemblyBuilderFlags.None;
      this.buildTargetGroup = EditorUserBuildSettings.activeBuildTargetGroup;
      this.buildTarget = EditorUserBuildSettings.activeBuildTarget;
    }

    public event Action<string> buildStarted;

    public event Action<string, CompilerMessage[]> buildFinished;

    /// <summary>
    ///   <para>Array of script paths used as input for assembly build. (Read Only)</para>
    /// </summary>
    public string[] scriptPaths { get; private set; }

    /// <summary>
    ///   <para>Output path of the assembly to build. (Read Only)</para>
    /// </summary>
    public string assemblyPath { get; private set; }

    /// <summary>
    ///   <para>Additional #define directives passed to compilation of the assembly.</para>
    /// </summary>
    public string[] additionalDefines { get; set; }

    /// <summary>
    ///   <para>Additional assembly references passed to compilation of the assembly.</para>
    /// </summary>
    public string[] additionalReferences { get; set; }

    /// <summary>
    ///   <para>References to exclude when compiling the assembly.</para>
    /// </summary>
    public string[] excludeReferences { get; set; }

    /// <summary>
    ///   <para>Flags to control the assembly build.</para>
    /// </summary>
    public AssemblyBuilderFlags flags { get; set; }

    /// <summary>
    ///   <para>BuildTargetGroup for the assembly build.</para>
    /// </summary>
    public BuildTargetGroup buildTargetGroup { get; set; }

    /// <summary>
    ///   <para>BuildTarget for the assembly build.</para>
    /// </summary>
    public BuildTarget buildTarget { get; set; }

    /// <summary>
    ///         <para>Starts the build of the assembly.
    /// 
    /// While building, the small progress icon in the lower right corner of Unity's main window will spin and EditorApplication.isCompiling will return true.</para>
    ///       </summary>
    /// <returns>
    ///   <para>Returns true if build was started. Returns false if the build was not started due to the editor currently compiling scripts in the Assets folder.</para>
    /// </returns>
    public bool Build()
    {
      return this.Build(EditorCompilationInterface.Instance);
    }

    internal bool Build(EditorCompilation editorCompilation)
    {
      if (editorCompilation.IsCompilationTaskCompiling())
        return false;
      if (this.status != AssemblyBuilderStatus.NotStarted)
        throw new Exception(string.Format("Cannot start AssemblyBuilder with status {0}. Expected {1}", (object) this.status, (object) AssemblyBuilderStatus.NotStarted));
      ScriptAssembly scriptAssembly = editorCompilation.CreateScriptAssembly(this);
      this.compilationTask = new CompilationTask(new ScriptAssembly[1]
      {
        scriptAssembly
      }, scriptAssembly.OutputDirectory, EditorScriptCompilationOptions.BuildingEmpty, 1);
      this.compilationTask.OnCompilationStarted += (Action<ScriptAssembly, int>) ((assembly, phase) =>
      {
        editorCompilation.InvokeAssemblyCompilationStarted(this.assemblyPath);
        this.OnCompilationStarted(assembly, phase);
      });
      this.compilationTask.OnCompilationFinished += (Action<ScriptAssembly, List<UnityEditor.Scripting.Compilers.CompilerMessage>>) ((assembly, messages) =>
      {
        editorCompilation.InvokeAssemblyCompilationFinished(this.assemblyPath, messages);
        this.OnCompilationFinished(assembly, messages);
      });
      this.compilationTask.Poll();
      editorCompilation.AddAssemblyBuilder(this);
      return true;
    }

    /// <summary>
    ///   <para>Current status of assembly build. (Read Only)</para>
    /// </summary>
    public AssemblyBuilderStatus status
    {
      get
      {
        if (this.compilationTask == null)
          return AssemblyBuilderStatus.NotStarted;
        if (this.compilationTask.IsCompiling)
          return !this.compilationTask.Poll() ? AssemblyBuilderStatus.IsCompiling : AssemblyBuilderStatus.Finished;
        return AssemblyBuilderStatus.Finished;
      }
    }

    private void OnCompilationStarted(ScriptAssembly assembly, int phase)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.buildStarted == null)
        return;
      try
      {
        // ISSUE: reference to a compiler-generated field
        this.buildStarted(this.assemblyPath);
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
      }
    }

    private void OnCompilationFinished(ScriptAssembly assembly, List<UnityEditor.Scripting.Compilers.CompilerMessage> messages)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.buildFinished == null)
        return;
      CompilerMessage[] compilerMessageArray = EditorCompilation.ConvertCompilerMessages(messages);
      try
      {
        // ISSUE: reference to a compiler-generated field
        this.buildFinished(this.assemblyPath, compilerMessageArray);
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
      }
    }
  }
}
