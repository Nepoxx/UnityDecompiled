// Decompiled with JetBrains decompiler
// Type: UnityEditor.Analytics.CoreStats
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor.Analytics
{
  internal static class CoreStats
  {
    public static event CoreStats.RequireInBuildDelegate OnRequireInBuildHandler = null;

    [RequiredByNativeCode]
    public static bool RequiresCoreStatsInBuild()
    {
      // ISSUE: reference to a compiler-generated field
      if (CoreStats.OnRequireInBuildHandler != null)
      {
        // ISSUE: reference to a compiler-generated field
        foreach (CoreStats.RequireInBuildDelegate invocation in CoreStats.OnRequireInBuildHandler.GetInvocationList())
        {
          if (invocation())
            return true;
        }
      }
      return false;
    }

    public delegate bool RequireInBuildDelegate();
  }
}
