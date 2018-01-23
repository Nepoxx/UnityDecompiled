// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.UnityScriptCompilerOutputParser
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Text.RegularExpressions;

namespace UnityEditor.Scripting.Compilers
{
  internal class UnityScriptCompilerOutputParser : CompilerOutputParserBase
  {
    private static Regex sCompilerOutput = new Regex("\\s*(?<filename>.*)\\((?<line>\\d+),(?<column>\\d+)\\):\\s*[BU]C(?<type>W|E)(?<id>[^:]*):\\s*(?<message>.*)", RegexOptions.ExplicitCapture);
    private static Regex sUnknownTypeOrNamespace = new Regex("[^']*'(?<type_name>[^']+)'.*", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    protected override string GetErrorIdentifier()
    {
      return "E";
    }

    protected override Regex GetOutputRegex()
    {
      return UnityScriptCompilerOutputParser.sCompilerOutput;
    }

    protected override NormalizedCompilerStatus NormalizedStatusFor(Match match)
    {
      Match match1 = match;
      string idToCheck1 = "0018";
      Regex unknownTypeOrNamespace1 = UnityScriptCompilerOutputParser.sUnknownTypeOrNamespace;
      // ISSUE: reference to a compiler-generated field
      if (UnityScriptCompilerOutputParser.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        UnityScriptCompilerOutputParser.\u003C\u003Ef__mg\u0024cache0 = new Func<Match, Regex, NormalizedCompilerStatus>(CompilerOutputParserBase.NormalizeSimpleUnknownTypeOfNamespaceError);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Match, Regex, NormalizedCompilerStatus> fMgCache0 = UnityScriptCompilerOutputParser.\u003C\u003Ef__mg\u0024cache0;
      NormalizedCompilerStatus normalizedCompilerStatus = CompilerOutputParserBase.TryNormalizeCompilerStatus(match1, idToCheck1, unknownTypeOrNamespace1, fMgCache0);
      if (normalizedCompilerStatus.code != NormalizedCompilerStatusCode.NotNormalized)
        return normalizedCompilerStatus;
      Match match2 = match;
      string idToCheck2 = "0005";
      Regex unknownTypeOrNamespace2 = UnityScriptCompilerOutputParser.sUnknownTypeOrNamespace;
      // ISSUE: reference to a compiler-generated field
      if (UnityScriptCompilerOutputParser.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        UnityScriptCompilerOutputParser.\u003C\u003Ef__mg\u0024cache1 = new Func<Match, Regex, NormalizedCompilerStatus>(CompilerOutputParserBase.NormalizeSimpleUnknownTypeOfNamespaceError);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Match, Regex, NormalizedCompilerStatus> fMgCache1 = UnityScriptCompilerOutputParser.\u003C\u003Ef__mg\u0024cache1;
      return CompilerOutputParserBase.TryNormalizeCompilerStatus(match2, idToCheck2, unknownTypeOrNamespace2, fMgCache1);
    }
  }
}
