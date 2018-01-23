// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilation.CompilationTask
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Scripting.Compilers;

namespace UnityEditor.Scripting.ScriptCompilation
{
  internal class CompilationTask
  {
    private Dictionary<ScriptAssembly, CompilerMessage[]> processedAssemblies = new Dictionary<ScriptAssembly, CompilerMessage[]>();
    private Dictionary<ScriptAssembly, ScriptCompilerBase> compilerTasks = new Dictionary<ScriptAssembly, ScriptCompilerBase>();
    private int compilePhase = 0;
    private HashSet<ScriptAssembly> pendingAssemblies;
    private string buildOutputDirectory;
    private EditorScriptCompilationOptions options;
    private int maxConcurrentCompilers;

    public CompilationTask(ScriptAssembly[] scriptAssemblies, string buildOutputDirectory, EditorScriptCompilationOptions options, int maxConcurrentCompilers)
    {
      this.pendingAssemblies = new HashSet<ScriptAssembly>((IEnumerable<ScriptAssembly>) scriptAssemblies);
      this.CompileErrors = false;
      this.buildOutputDirectory = buildOutputDirectory;
      this.options = options;
      this.maxConcurrentCompilers = maxConcurrentCompilers;
    }

    public event Action<ScriptAssembly, int> OnCompilationStarted;

    public event Action<ScriptAssembly, List<CompilerMessage>> OnCompilationFinished;

    public bool Stopped { get; private set; }

    public bool CompileErrors { get; private set; }

    ~CompilationTask()
    {
      this.Stop();
    }

    public bool IsCompiling
    {
      get
      {
        return this.pendingAssemblies.Count > 0 || this.compilerTasks.Count > 0;
      }
    }

    public Dictionary<ScriptAssembly, CompilerMessage[]> CompilerMessages
    {
      get
      {
        return this.processedAssemblies;
      }
    }

    public void Stop()
    {
      if (this.Stopped)
        return;
      foreach (KeyValuePair<ScriptAssembly, ScriptCompilerBase> compilerTask in this.compilerTasks)
        compilerTask.Value.Dispose();
      this.compilerTasks.Clear();
      this.Stopped = true;
    }

    public bool Poll()
    {
      if (this.Stopped)
        return true;
      Dictionary<ScriptAssembly, ScriptCompilerBase> dictionary = (Dictionary<ScriptAssembly, ScriptCompilerBase>) null;
      foreach (KeyValuePair<ScriptAssembly, ScriptCompilerBase> compilerTask in this.compilerTasks)
      {
        ScriptCompilerBase scriptCompilerBase = compilerTask.Value;
        if (scriptCompilerBase.Poll())
        {
          if (dictionary == null)
            dictionary = new Dictionary<ScriptAssembly, ScriptCompilerBase>();
          ScriptAssembly key = compilerTask.Key;
          dictionary.Add(key, scriptCompilerBase);
        }
      }
      if (dictionary != null)
      {
        foreach (KeyValuePair<ScriptAssembly, ScriptCompilerBase> keyValuePair in dictionary)
        {
          ScriptAssembly key = keyValuePair.Key;
          ScriptCompilerBase scriptCompilerBase = keyValuePair.Value;
          CompilerMessage[] compilerMessages = scriptCompilerBase.GetCompilerMessages();
          List<CompilerMessage> list = ((IEnumerable<CompilerMessage>) compilerMessages).ToList<CompilerMessage>();
          // ISSUE: reference to a compiler-generated field
          if (this.OnCompilationFinished != null)
          {
            // ISSUE: reference to a compiler-generated field
            this.OnCompilationFinished(key, list);
          }
          this.processedAssemblies.Add(key, list.ToArray());
          if (!this.CompileErrors)
            this.CompileErrors = ((IEnumerable<CompilerMessage>) compilerMessages).Any<CompilerMessage>((Func<CompilerMessage, bool>) (m => m.type == CompilerMessageType.Error));
          this.compilerTasks.Remove(key);
          scriptCompilerBase.Dispose();
        }
      }
      if (this.CompileErrors)
      {
        if (this.pendingAssemblies.Count > 0)
        {
          foreach (ScriptAssembly pendingAssembly in this.pendingAssemblies)
            this.processedAssemblies.Add(pendingAssembly, new CompilerMessage[0]);
          this.pendingAssemblies.Clear();
        }
        return this.compilerTasks.Count == 0;
      }
      if (this.compilerTasks.Count == 0 || dictionary != null && dictionary.Count > 0)
        this.QueuePendingAssemblies();
      return this.pendingAssemblies.Count == 0 && this.compilerTasks.Count == 0;
    }

    private void QueuePendingAssemblies()
    {
      if (this.pendingAssemblies.Count == 0)
        return;
      List<ScriptAssembly> scriptAssemblyList = (List<ScriptAssembly>) null;
      foreach (ScriptAssembly pendingAssembly in this.pendingAssemblies)
      {
        bool flag = true;
        foreach (ScriptAssembly assemblyReference in pendingAssembly.ScriptAssemblyReferences)
        {
          if (!this.processedAssemblies.ContainsKey(assemblyReference))
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          if (scriptAssemblyList == null)
            scriptAssemblyList = new List<ScriptAssembly>();
          scriptAssemblyList.Add(pendingAssembly);
        }
      }
      if (scriptAssemblyList == null)
        return;
      bool buildingForEditor = (this.options & EditorScriptCompilationOptions.BuildingForEditor) == EditorScriptCompilationOptions.BuildingForEditor;
      foreach (ScriptAssembly key in scriptAssemblyList)
      {
        this.pendingAssemblies.Remove(key);
        MonoIsland monoIsland = key.ToMonoIsland(this.options, this.buildOutputDirectory);
        ScriptCompilerBase compilerInstance = ScriptCompilers.CreateCompilerInstance(monoIsland, buildingForEditor, monoIsland._target, key.RunUpdater);
        this.compilerTasks.Add(key, compilerInstance);
        compilerInstance.BeginCompiling();
        // ISSUE: reference to a compiler-generated field
        if (this.OnCompilationStarted != null)
        {
          // ISSUE: reference to a compiler-generated field
          this.OnCompilationStarted(key, this.compilePhase);
        }
        if (this.compilerTasks.Count == this.maxConcurrentCompilers)
          break;
      }
      ++this.compilePhase;
    }
  }
}
