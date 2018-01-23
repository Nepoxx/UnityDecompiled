// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.VRModule
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal.VR
{
  public class VRModule
  {
    private static bool IsTargetingCardboardOnIOS(BuildTarget target)
    {
      return PlayerSettings.virtualRealitySupported && target == BuildTarget.iOS && VREditor.IsVRDeviceEnabledForBuildTarget(target, "cardboard");
    }

    public static void SetupBuildSettings(BuildTarget target, int osVerMajor)
    {
      if (!VRModule.IsTargetingCardboardOnIOS(target) || osVerMajor >= 8)
        return;
      Debug.LogWarning((object) string.Format("Deployment target version is set to {0}, but Cardboard supports only versions starting from 8.0.", (object) osVerMajor));
    }

    public static bool ShouldInjectVRDependenciesForBuildTarget(BuildTarget target)
    {
      if (!PlayerSettings.virtualRealitySupported)
        return false;
      return BuildPipeline.GetBuildTargetGroup(target) == BuildTargetGroup.iPhone && VREditor.IsVRDeviceEnabledForBuildTarget(target, "cardboard");
    }
  }
}
