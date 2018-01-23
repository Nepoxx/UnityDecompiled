// Decompiled with JetBrains decompiler
// Type: UnityEditor.Build.BuildDefines
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UnityEditor.Build
{
  [RequiredByNativeCode]
  internal class BuildDefines
  {
    public static event GetScriptCompilationDefinesDelegate getScriptCompilationDefinesDelegates;

    [RequiredByNativeCode]
    public static string[] GetScriptCompilationDefines(BuildTarget target, string[] defines)
    {
      HashSet<string> defines1 = new HashSet<string>((IEnumerable<string>) defines);
      // ISSUE: reference to a compiler-generated field
      if (BuildDefines.getScriptCompilationDefinesDelegates != null)
      {
        // ISSUE: reference to a compiler-generated field
        BuildDefines.getScriptCompilationDefinesDelegates(target, defines1);
      }
      string[] array = new string[defines1.Count];
      defines1.CopyTo(array);
      return array;
    }
  }
}
