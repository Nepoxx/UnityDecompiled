// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildReporting.BuildReportHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Modules;
using UnityEngine.Scripting;

namespace UnityEditor.BuildReporting
{
  internal static class BuildReportHelper
  {
    private static IBuildAnalyzer m_CachedAnalyzer;
    private static BuildTarget m_CachedAnalyzerTarget;

    private static IBuildAnalyzer GetAnalyzerForTarget(BuildTarget target)
    {
      if (BuildReportHelper.m_CachedAnalyzerTarget == target)
        return BuildReportHelper.m_CachedAnalyzer;
      BuildReportHelper.m_CachedAnalyzer = ModuleManager.GetBuildAnalyzer(target);
      BuildReportHelper.m_CachedAnalyzerTarget = target;
      return BuildReportHelper.m_CachedAnalyzer;
    }

    [RequiredByNativeCode]
    public static void OnAddedExecutable(BuildReport report, int fileIndex)
    {
      IBuildAnalyzer analyzerForTarget = BuildReportHelper.GetAnalyzerForTarget(report.buildTarget);
      if (analyzerForTarget == null)
        return;
      analyzerForTarget.OnAddedExecutable(report, fileIndex);
    }
  }
}
