// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.BooCompilerOutputParser
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Text.RegularExpressions;

namespace UnityEditor.Scripting.Compilers
{
  internal class BooCompilerOutputParser : CompilerOutputParserBase
  {
    private static Regex sCompilerOutput = new Regex("\\s*(?<filename>.*)\\((?<line>\\d+),(?<column>\\d+)\\):\\s*[BU]C(?<type>W|E)(?<id>[^:]*):\\s*(?<message>.*)", RegexOptions.ExplicitCapture);
    private static Regex sMissingMember = new Regex("[^']*'(?<member_name>[^']+)'[^']+'(?<type_name>[^']+)'", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    private static Regex sUnknownTypeOrNamespace = new Regex("[^']*'(?<type_name>[^']+)'.*", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    protected override string GetErrorIdentifier()
    {
      return "E";
    }

    protected override Regex GetOutputRegex()
    {
      return BooCompilerOutputParser.sCompilerOutput;
    }

    protected override NormalizedCompilerStatus NormalizedStatusFor(Match match)
    {
      Match match1 = match;
      string idToCheck1 = "0019";
      Regex sMissingMember = BooCompilerOutputParser.sMissingMember;
      // ISSUE: reference to a compiler-generated field
      if (BooCompilerOutputParser.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        BooCompilerOutputParser.\u003C\u003Ef__mg\u0024cache0 = new Func<Match, Regex, NormalizedCompilerStatus>(CompilerOutputParserBase.NormalizeMemberNotFoundError);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Match, Regex, NormalizedCompilerStatus> fMgCache0 = BooCompilerOutputParser.\u003C\u003Ef__mg\u0024cache0;
      NormalizedCompilerStatus normalizedCompilerStatus = CompilerOutputParserBase.TryNormalizeCompilerStatus(match1, idToCheck1, sMissingMember, fMgCache0);
      if (normalizedCompilerStatus.code != NormalizedCompilerStatusCode.NotNormalized)
        return normalizedCompilerStatus;
      Match match2 = match;
      string idToCheck2 = "0018";
      Regex unknownTypeOrNamespace = BooCompilerOutputParser.sUnknownTypeOrNamespace;
      // ISSUE: reference to a compiler-generated field
      if (BooCompilerOutputParser.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        BooCompilerOutputParser.\u003C\u003Ef__mg\u0024cache1 = new Func<Match, Regex, NormalizedCompilerStatus>(CompilerOutputParserBase.NormalizeSimpleUnknownTypeOfNamespaceError);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Match, Regex, NormalizedCompilerStatus> fMgCache1 = BooCompilerOutputParser.\u003C\u003Ef__mg\u0024cache1;
      return CompilerOutputParserBase.TryNormalizeCompilerStatus(match2, idToCheck2, unknownTypeOrNamespace, fMgCache1);
    }
  }
}
