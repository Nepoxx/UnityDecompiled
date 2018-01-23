// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.IBuildPostprocessor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Modules
{
  internal interface IBuildPostprocessor
  {
    void LaunchPlayer(BuildLaunchPlayerArgs args);

    void PostProcess(BuildPostProcessArgs args);

    bool SupportsInstallInBuildFolder();

    bool SupportsLz4Compression();

    void PostProcessScriptsOnly(BuildPostProcessArgs args);

    bool SupportsScriptsOnlyBuild();

    string PrepareForBuild(BuildOptions options, BuildTarget target);

    void UpdateBootConfig(BuildTarget target, BootConfigData config, BuildOptions options);

    string GetExtension(BuildTarget target, BuildOptions options);
  }
}
