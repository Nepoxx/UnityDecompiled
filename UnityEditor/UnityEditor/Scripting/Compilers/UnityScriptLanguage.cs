// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.UnityScriptLanguage
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Scripting.Compilers
{
  internal class UnityScriptLanguage : SupportedLanguage
  {
    public override string GetExtensionICanCompile()
    {
      return "js";
    }

    public override string GetLanguageName()
    {
      return "UnityScript";
    }

    public override ScriptCompilerBase CreateCompiler(MonoIsland island, bool buildingForEditor, BuildTarget targetPlatform, bool runUpdater)
    {
      return (ScriptCompilerBase) new UnityScriptCompiler(island, runUpdater);
    }
  }
}
