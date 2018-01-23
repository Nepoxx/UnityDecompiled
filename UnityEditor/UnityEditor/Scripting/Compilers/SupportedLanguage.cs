// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.SupportedLanguage
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Scripting.Compilers
{
  internal abstract class SupportedLanguage
  {
    public abstract string GetExtensionICanCompile();

    public abstract string GetLanguageName();

    public abstract ScriptCompilerBase CreateCompiler(MonoIsland island, bool buildingForEditor, BuildTarget targetPlatform, bool runUpdater);

    public virtual string GetNamespace(string fileName, string definedSymbols)
    {
      return string.Empty;
    }
  }
}
