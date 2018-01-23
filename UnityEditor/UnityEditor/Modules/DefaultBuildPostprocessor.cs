// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.DefaultBuildPostprocessor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.Modules
{
  internal abstract class DefaultBuildPostprocessor : IBuildPostprocessor
  {
    public virtual void LaunchPlayer(BuildLaunchPlayerArgs args)
    {
      throw new NotSupportedException();
    }

    public virtual void PostProcess(BuildPostProcessArgs args)
    {
    }

    public virtual bool SupportsInstallInBuildFolder()
    {
      return false;
    }

    public virtual bool SupportsLz4Compression()
    {
      return false;
    }

    public virtual void PostProcessScriptsOnly(BuildPostProcessArgs args)
    {
      if (!this.SupportsScriptsOnlyBuild())
        throw new NotSupportedException();
    }

    public virtual bool SupportsScriptsOnlyBuild()
    {
      return true;
    }

    public virtual string PrepareForBuild(BuildOptions options, BuildTarget target)
    {
      return (string) null;
    }

    public virtual void UpdateBootConfig(BuildTarget target, BootConfigData config, BuildOptions options)
    {
      config.Set("wait-for-native-debugger", "0");
      if (!(config.Get("player-connection-debug") == "1"))
        return;
      if (EditorUserBuildSettings.GetPlatformSettings(BuildPipeline.GetBuildTargetName(target), "WaitForManagedDebugger") == "true")
        config.Set("wait-for-managed-debugger", "1");
      else
        config.Set("wait-for-managed-debugger", "0");
    }

    public virtual string GetExtension(BuildTarget target, BuildOptions options)
    {
      return string.Empty;
    }
  }
}
