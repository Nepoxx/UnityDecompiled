// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.Cil2AsOutputParser
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace UnityEditor.Scripting.Compilers
{
  internal class Cil2AsOutputParser : UnityScriptCompilerOutputParser
  {
    [DebuggerHidden]
    public override IEnumerable<CompilerMessage> Parse(string[] errorOutput, string[] standardOutput, bool compilationHadFailure)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Cil2AsOutputParser.\u003CParse\u003Ec__Iterator0 parseCIterator0 = new Cil2AsOutputParser.\u003CParse\u003Ec__Iterator0() { errorOutput = errorOutput };
      // ISSUE: reference to a compiler-generated field
      parseCIterator0.\u0024PC = -2;
      return (IEnumerable<CompilerMessage>) parseCIterator0;
    }

    private static CompilerMessage CompilerErrorFor(StringBuilder currentErrorBuffer)
    {
      return new CompilerMessage() { type = CompilerMessageType.Error, message = currentErrorBuffer.ToString() };
    }
  }
}
