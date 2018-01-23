// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.PluginsHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEditor.Modules;

namespace UnityEditorInternal
{
  internal class PluginsHelper
  {
    public static bool CheckFileCollisions(BuildTarget buildTarget)
    {
      IPluginImporterExtension importerExtension = (IPluginImporterExtension) null;
      if (ModuleManager.IsPlatformSupported(buildTarget))
        importerExtension = ModuleManager.GetPluginImporterExtension(buildTarget);
      if (importerExtension == null)
        importerExtension = BuildPipeline.GetBuildTargetGroup(buildTarget) != BuildTargetGroup.Standalone ? (IPluginImporterExtension) new DefaultPluginImporterExtension((DefaultPluginImporterExtension.Property[]) null) : (IPluginImporterExtension) new DesktopPluginImporterExtension();
      return importerExtension.CheckFileCollisions(BuildPipeline.GetBuildTargetName(buildTarget));
    }
  }
}
