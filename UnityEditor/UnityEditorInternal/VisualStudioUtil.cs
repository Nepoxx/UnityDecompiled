// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VisualStudioUtil
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditorInternal
{
  internal static class VisualStudioUtil
  {
    [DebuggerHidden]
    public static IEnumerable<VisualStudioUtil.VisualStudio> ParseRawDevEnvPaths(string[] rawDevEnvPaths)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      VisualStudioUtil.\u003CParseRawDevEnvPaths\u003Ec__Iterator0 envPathsCIterator0 = new VisualStudioUtil.\u003CParseRawDevEnvPaths\u003Ec__Iterator0() { rawDevEnvPaths = rawDevEnvPaths };
      // ISSUE: reference to a compiler-generated field
      envPathsCIterator0.\u0024PC = -2;
      return (IEnumerable<VisualStudioUtil.VisualStudio>) envPathsCIterator0;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] FindVisualStudioDevEnvPaths(int visualStudioVersion, string[] requiredWorkloads);

    public class VisualStudio
    {
      public readonly string DevEnvPath;
      public readonly string Edition;
      public readonly Version Version;
      public readonly string[] Workloads;

      internal VisualStudio(string devEnvPath, string edition, Version version, string[] workloads)
      {
        this.DevEnvPath = devEnvPath;
        this.Edition = edition;
        this.Version = version;
        this.Workloads = workloads;
      }
    }
  }
}
