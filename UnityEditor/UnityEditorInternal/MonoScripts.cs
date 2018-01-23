// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.MonoScripts
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;

namespace UnityEditorInternal
{
  public static class MonoScripts
  {
    public static MonoScript CreateMonoScript(string scriptContents, string className, string nameSpace, string assemblyName, bool isEditorScript)
    {
      MonoScript monoScript = new MonoScript();
      monoScript.Init(scriptContents, className, nameSpace, assemblyName, isEditorScript);
      return monoScript;
    }
  }
}
