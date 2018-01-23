// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.DefaultPlatformSupportModule
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.DeploymentTargets;
using UnityEngine;

namespace UnityEditor.Modules
{
  internal abstract class DefaultPlatformSupportModule : IPlatformSupportModule
  {
    protected ICompilationExtension compilationExtension;
    protected ITextureImportSettingsExtension textureSettingsExtension;

    public abstract string TargetName { get; }

    public abstract string JamTarget { get; }

    public virtual string ExtensionVersion
    {
      get
      {
        return (string) null;
      }
    }

    public virtual GUIContent[] GetDisplayNames()
    {
      return (GUIContent[]) null;
    }

    public virtual string[] NativeLibraries
    {
      get
      {
        return new string[0];
      }
    }

    public virtual string[] AssemblyReferencesForUserScripts
    {
      get
      {
        return new string[0];
      }
    }

    public virtual string[] AssemblyReferencesForEditorCsharpProject
    {
      get
      {
        return new string[0];
      }
    }

    public virtual IBuildAnalyzer CreateBuildAnalyzer()
    {
      return (IBuildAnalyzer) null;
    }

    public abstract IBuildPostprocessor CreateBuildPostprocessor();

    public virtual IScriptingImplementations CreateScriptingImplementations()
    {
      return (IScriptingImplementations) null;
    }

    public virtual ISettingEditorExtension CreateSettingsEditorExtension()
    {
      return (ISettingEditorExtension) null;
    }

    public virtual IPreferenceWindowExtension CreatePreferenceWindowExtension()
    {
      return (IPreferenceWindowExtension) null;
    }

    public virtual ITextureImportSettingsExtension CreateTextureImportSettingsExtension()
    {
      return this.textureSettingsExtension == null ? (this.textureSettingsExtension = (ITextureImportSettingsExtension) new DefaultTextureImportSettingsExtension()) : this.textureSettingsExtension;
    }

    public virtual IBuildWindowExtension CreateBuildWindowExtension()
    {
      return (IBuildWindowExtension) null;
    }

    public virtual ICompilationExtension CreateCompilationExtension()
    {
      return this.compilationExtension == null ? (this.compilationExtension = (ICompilationExtension) new DefaultCompilationExtension()) : this.compilationExtension;
    }

    public virtual IPluginImporterExtension CreatePluginImporterExtension()
    {
      return (IPluginImporterExtension) null;
    }

    public virtual IUserAssembliesValidator CreateUserAssembliesValidatorExtension()
    {
      return (IUserAssembliesValidator) null;
    }

    public virtual IProjectGeneratorExtension CreateProjectGeneratorExtension()
    {
      return (IProjectGeneratorExtension) null;
    }

    public virtual IDeploymentTargetsExtension CreateDeploymentTargetsExtension()
    {
      return (IDeploymentTargetsExtension) null;
    }

    public virtual void RegisterAdditionalUnityExtensions()
    {
    }

    public virtual IDevice CreateDevice(string id)
    {
      throw new NotSupportedException();
    }

    public virtual void OnActivate()
    {
    }

    public virtual void OnDeactivate()
    {
    }

    public virtual void OnLoad()
    {
    }

    public virtual void OnUnload()
    {
    }
  }
}
