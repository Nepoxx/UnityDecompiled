// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.ModuleUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor.Modules
{
  internal static class ModuleUtils
  {
    internal static string[] GetAdditionalReferencesForUserScripts()
    {
      List<string> stringList = new List<string>();
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModulesDontRegister)
        stringList.AddRange((IEnumerable<string>) platformSupportModule.AssemblyReferencesForUserScripts);
      return stringList.ToArray();
    }

    internal static string[] GetAdditionalReferencesForEditorCsharpProject()
    {
      List<string> stringList = new List<string>();
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModulesDontRegister)
        stringList.AddRange((IEnumerable<string>) platformSupportModule.AssemblyReferencesForEditorCsharpProject);
      return stringList.ToArray();
    }
  }
}
