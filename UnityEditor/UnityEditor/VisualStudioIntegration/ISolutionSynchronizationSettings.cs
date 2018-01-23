// Decompiled with JetBrains decompiler
// Type: UnityEditor.VisualStudioIntegration.ISolutionSynchronizationSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.VisualStudioIntegration
{
  internal interface ISolutionSynchronizationSettings
  {
    int VisualStudioVersion { get; }

    string SolutionTemplate { get; }

    string SolutionProjectEntryTemplate { get; }

    string SolutionProjectConfigurationTemplate { get; }

    string EditorAssemblyPath { get; }

    string EngineAssemblyPath { get; }

    string MonoLibFolder { get; }

    string[] Defines { get; }

    string GetProjectHeaderTemplate(ScriptingLanguage language);

    string GetProjectFooterTemplate(ScriptingLanguage language);
  }
}
