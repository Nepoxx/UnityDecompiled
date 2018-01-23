// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilation.EditorCompilationInterface
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.Compilation;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Scripting.ScriptCompilation
{
  internal static class EditorCompilationInterface
  {
    private static EditorCompilation editorCompilation;

    public static EditorCompilation Instance
    {
      get
      {
        if (EditorCompilationInterface.editorCompilation == null)
        {
          EditorCompilationInterface.editorCompilation = new EditorCompilation();
          EditorCompilation editorCompilation = EditorCompilationInterface.editorCompilation;
          Action<EditorCompilation.CompilationSetupErrorFlags> errorFlagsChanged = editorCompilation.setupErrorFlagsChanged;
          if (EditorCompilationInterface.\u003C\u003Ef__mg\u0024cache0 == null)
            EditorCompilationInterface.\u003C\u003Ef__mg\u0024cache0 = new Action<EditorCompilation.CompilationSetupErrorFlags>(EditorCompilationInterface.ClearErrors);
          Action<EditorCompilation.CompilationSetupErrorFlags> fMgCache0 = EditorCompilationInterface.\u003C\u003Ef__mg\u0024cache0;
          editorCompilation.setupErrorFlagsChanged = errorFlagsChanged + fMgCache0;
          CompilationPipeline.ClearEditorCompilationErrors();
        }
        return EditorCompilationInterface.editorCompilation;
      }
    }

    private static void ClearErrors(EditorCompilation.CompilationSetupErrorFlags flags)
    {
      if (flags != EditorCompilation.CompilationSetupErrorFlags.none)
        return;
      CompilationPipeline.ClearEditorCompilationErrors();
    }

    private static void LogException(Exception exception)
    {
      AssemblyDefinitionException definitionException = exception as AssemblyDefinitionException;
      if (definitionException != null && definitionException.filePaths.Length > 0)
      {
        foreach (string filePath in definitionException.filePaths)
          CompilationPipeline.LogEditorCompilationError(string.Format("{0} ({1})", (object) exception.Message, (object) filePath), AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(filePath).GetInstanceID());
      }
      else
        Debug.LogException(exception);
    }

    private static void EmitExceptionAsError(Action action)
    {
      try
      {
        action();
      }
      catch (Exception ex)
      {
        EditorCompilationInterface.LogException(ex);
      }
    }

    private static T EmitExceptionAsError<T>(Func<T> func, T returnValue)
    {
      try
      {
        return func();
      }
      catch (Exception ex)
      {
        EditorCompilationInterface.LogException(ex);
        return returnValue;
      }
    }

    [RequiredByNativeCode]
    public static void SetAssemblySuffix(string suffix)
    {
      EditorCompilationInterface.Instance.SetAssemblySuffix(suffix);
    }

    [RequiredByNativeCode]
    public static void SetAllScripts(string[] allScripts)
    {
      EditorCompilationInterface.Instance.SetAllScripts(allScripts);
    }

    [RequiredByNativeCode]
    public static bool IsExtensionSupportedByCompiler(string extension)
    {
      return EditorCompilationInterface.Instance.IsExtensionSupportedByCompiler(extension);
    }

    [RequiredByNativeCode]
    public static void DirtyAllScripts()
    {
      EditorCompilationInterface.Instance.DirtyAllScripts();
    }

    [RequiredByNativeCode]
    public static void DirtyScript(string path)
    {
      EditorCompilationInterface.Instance.DirtyScript(path);
    }

    [RequiredByNativeCode]
    public static void RunScriptUpdaterOnAssembly(string assemblyFilename)
    {
      EditorCompilationInterface.Instance.RunScriptUpdaterOnAssembly(assemblyFilename);
    }

    [RequiredByNativeCode]
    public static void SetAllPrecompiledAssemblies(PrecompiledAssembly[] precompiledAssemblies)
    {
      EditorCompilationInterface.Instance.SetAllPrecompiledAssemblies(precompiledAssemblies);
    }

    [RequiredByNativeCode]
    public static void SetAllUnityAssemblies(PrecompiledAssembly[] unityAssemblies)
    {
      EditorCompilationInterface.Instance.SetAllUnityAssemblies(unityAssemblies);
    }

    [RequiredByNativeCode]
    public static void SetCompileScriptsOutputDirectory(string directory)
    {
      EditorCompilationInterface.Instance.SetCompileScriptsOutputDirectory(directory);
    }

    [RequiredByNativeCode]
    public static string GetCompileScriptsOutputDirectory()
    {
      return EditorCompilationInterface.EmitExceptionAsError<string>((Func<string>) (() => EditorCompilationInterface.Instance.GetCompileScriptsOutputDirectory()), string.Empty);
    }

    [RequiredByNativeCode]
    public static void SetAllCustomScriptAssemblyJsons(string[] allAssemblyJsons)
    {
      EditorCompilationInterface.EmitExceptionAsError((Action) (() => EditorCompilationInterface.Instance.SetAllCustomScriptAssemblyJsons(allAssemblyJsons)));
    }

    [RequiredByNativeCode]
    public static void SetAllPackageAssemblies(EditorCompilation.PackageAssembly[] packageAssemblies)
    {
      EditorCompilationInterface.EmitExceptionAsError((Action) (() => EditorCompilationInterface.Instance.SetAllPackageAssemblies(packageAssemblies)));
    }

    [RequiredByNativeCode]
    public static EditorCompilation.TargetAssemblyInfo[] GetAllCompiledAndResolvedCustomTargetAssemblies()
    {
      return EditorCompilationInterface.EmitExceptionAsError<EditorCompilation.TargetAssemblyInfo[]>((Func<EditorCompilation.TargetAssemblyInfo[]>) (() => EditorCompilationInterface.Instance.GetAllCompiledAndResolvedCustomTargetAssemblies()), new EditorCompilation.TargetAssemblyInfo[0]);
    }

    [RequiredByNativeCode]
    public static bool HaveSetupErrors()
    {
      return EditorCompilationInterface.Instance.HaveSetupErrors();
    }

    [RequiredByNativeCode]
    public static void DeleteUnusedAssemblies()
    {
      EditorCompilationInterface.EmitExceptionAsError((Action) (() => EditorCompilationInterface.Instance.DeleteUnusedAssemblies()));
    }

    [RequiredByNativeCode]
    public static bool CompileScripts(EditorScriptCompilationOptions definesOptions, BuildTargetGroup platformGroup, BuildTarget platform)
    {
      return EditorCompilationInterface.EmitExceptionAsError<bool>((Func<bool>) (() => EditorCompilationInterface.Instance.CompileScripts(definesOptions, platformGroup, platform)), false);
    }

    [RequiredByNativeCode]
    public static bool DoesProjectFolderHaveAnyDirtyScripts()
    {
      return EditorCompilationInterface.Instance.DoesProjectFolderHaveAnyDirtyScripts();
    }

    [RequiredByNativeCode]
    public static bool DoesProjectFolderHaveAnyScripts()
    {
      return EditorCompilationInterface.Instance.DoesProjectFolderHaveAnyScripts();
    }

    [RequiredByNativeCode]
    public static EditorCompilation.AssemblyCompilerMessages[] GetCompileMessages()
    {
      return EditorCompilationInterface.Instance.GetCompileMessages();
    }

    [RequiredByNativeCode]
    public static bool IsCompilationPending()
    {
      return EditorCompilationInterface.Instance.IsCompilationPending();
    }

    [RequiredByNativeCode]
    public static bool IsCompiling()
    {
      return EditorCompilationInterface.Instance.IsCompiling();
    }

    [RequiredByNativeCode]
    public static void StopAllCompilation()
    {
      EditorCompilationInterface.Instance.StopAllCompilation();
    }

    [RequiredByNativeCode]
    public static EditorCompilation.CompileStatus TickCompilationPipeline(EditorScriptCompilationOptions options, BuildTargetGroup platformGroup, BuildTarget platform)
    {
      return EditorCompilationInterface.EmitExceptionAsError<EditorCompilation.CompileStatus>((Func<EditorCompilation.CompileStatus>) (() => EditorCompilationInterface.Instance.TickCompilationPipeline(options, platformGroup, platform)), EditorCompilation.CompileStatus.Idle);
    }

    [RequiredByNativeCode]
    public static EditorCompilation.TargetAssemblyInfo[] GetTargetAssemblies()
    {
      return EditorCompilationInterface.Instance.GetTargetAssemblies();
    }

    [RequiredByNativeCode]
    public static EditorCompilation.TargetAssemblyInfo GetTargetAssembly(string scriptPath)
    {
      return EditorCompilationInterface.Instance.GetTargetAssembly(scriptPath);
    }

    [RequiredByNativeCode]
    public static MonoIsland[] GetAllMonoIslands()
    {
      return EditorCompilationInterface.Instance.GetAllMonoIslands();
    }
  }
}
