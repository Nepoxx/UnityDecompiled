// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.IBuildWindowExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Modules
{
  internal interface IBuildWindowExtension
  {
    void ShowPlatformBuildOptions();

    void ShowInternalPlatformBuildOptions();

    bool EnabledBuildButton();

    bool EnabledBuildAndRunButton();

    bool ShouldDrawScriptDebuggingCheckbox();

    bool ShouldDrawProfilerCheckbox();

    bool ShouldDrawDevelopmentPlayerCheckbox();

    bool ShouldDrawExplicitNullCheckbox();

    bool ShouldDrawExplicitDivideByZeroCheckbox();

    bool ShouldDrawForceOptimizeScriptsCheckbox();
  }
}
