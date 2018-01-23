// Decompiled with JetBrains decompiler
// Type: UnityEditor.DeploymentTargets.DeploymentTargetManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.BuildReporting;
using UnityEditor.Modules;

namespace UnityEditor.DeploymentTargets
{
  internal class DeploymentTargetManager
  {
    private const string kExtensionErrorMessage = "Platform does not implement DeploymentTargetsExtension";

    private static IDeploymentTargetsExtension GetExtension(BuildTargetGroup targetGroup, BuildTarget buildTarget)
    {
      IDeploymentTargetsExtension targetsExtension = ModuleManager.GetDeploymentTargetsExtension(targetGroup, buildTarget);
      if (targetsExtension == null)
        throw new NotSupportedException("Platform does not implement DeploymentTargetsExtension");
      return targetsExtension;
    }

    public static IDeploymentTargetInfo GetTargetInfo(BuildTargetGroup targetGroup, BuildTarget buildTarget, DeploymentTargetId targetId)
    {
      return DeploymentTargetManager.GetExtension(targetGroup, buildTarget).GetTargetInfo(targetId, (ProgressHandler) null);
    }

    public static bool SupportsLaunchBuild(IDeploymentTargetInfo info, BuildReport buildReport)
    {
      return info.GetSupportFlags().HasFlags(DeploymentTargetSupportFlags.Launch) && info.CheckBuild(buildReport).Passed();
    }

    public static void LaunchBuildOnTarget(BuildTargetGroup targetGroup, BuildReport buildReport, DeploymentTargetId targetId, ProgressHandler progressHandler = null)
    {
      DeploymentTargetManager.GetExtension(targetGroup, buildReport.buildTarget).LaunchBuildOnTarget(buildReport, targetId, progressHandler);
    }

    public static List<DeploymentTargetIdAndStatus> GetKnownTargets(BuildTargetGroup targetGroup, BuildTarget buildTarget)
    {
      return DeploymentTargetManager.GetExtension(targetGroup, buildTarget).GetKnownTargets((ProgressHandler) null);
    }

    public static List<DeploymentTargetId> FindValidTargetsForLaunchBuild(BuildTargetGroup targetGroup, BuildReport buildReport)
    {
      IDeploymentTargetsExtension extension = DeploymentTargetManager.GetExtension(targetGroup, buildReport.buildTarget);
      List<DeploymentTargetId> deploymentTargetIdList = new List<DeploymentTargetId>();
      foreach (DeploymentTargetIdAndStatus knownTarget in extension.GetKnownTargets((ProgressHandler) null))
      {
        if (knownTarget.status == DeploymentTargetStatus.Ready && DeploymentTargetManager.SupportsLaunchBuild(extension.GetTargetInfo(knownTarget.id, (ProgressHandler) null), buildReport))
          deploymentTargetIdList.Add(knownTarget.id);
      }
      return deploymentTargetIdList;
    }
  }
}
