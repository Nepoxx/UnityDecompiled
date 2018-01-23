// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.MicrosoftCSharpCompilerOutputParser
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Text.RegularExpressions;

namespace UnityEditor.Scripting.Compilers
{
  internal class MicrosoftCSharpCompilerOutputParser : CompilerOutputParserBase
  {
    private static Regex sCompilerOutput = new Regex("\\s*(?<filename>.*)\\((?<line>\\d+),(?<column>\\d+)\\):\\s*(?<type>warning|error)\\s*(?<id>[^:]*):\\s*(?<message>.*)", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    protected override Regex GetOutputRegex()
    {
      return MicrosoftCSharpCompilerOutputParser.sCompilerOutput;
    }

    protected override string GetErrorIdentifier()
    {
      return "error";
    }
  }
}
