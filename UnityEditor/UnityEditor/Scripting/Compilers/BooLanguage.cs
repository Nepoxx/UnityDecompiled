// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.BooLanguage
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using Boo.Lang.Compiler.Ast;
using Boo.Lang.Parser;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.Scripting.Compilers
{
  internal class BooLanguage : SupportedLanguage
  {
    public override string GetExtensionICanCompile()
    {
      return "boo";
    }

    public override string GetLanguageName()
    {
      return "Boo";
    }

    public override ScriptCompilerBase CreateCompiler(MonoIsland island, bool buildingForEditor, BuildTarget targetPlatform, bool runUpdater)
    {
      return (ScriptCompilerBase) new BooCompiler(island, runUpdater);
    }

    public override string GetNamespace(string fileName, string definedSymbols)
    {
      try
      {
        return ((IEnumerable<Module>) BooParser.ParseFile(fileName).get_Modules()).First<Module>().get_Namespace().get_Name();
      }
      catch
      {
      }
      return base.GetNamespace(fileName, definedSymbols);
    }
  }
}
