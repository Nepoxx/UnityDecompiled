// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.MonoCSharpCompilerOutputParser
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Text.RegularExpressions;

namespace UnityEditor.Scripting.Compilers
{
  internal class MonoCSharpCompilerOutputParser : CompilerOutputParserBase
  {
    private static Regex sCompilerOutput = new Regex("\\s*(?<filename>.*)\\((?<line>\\d+),(?<column>\\d+)(\\+{1})?\\):\\s*(?<type>warning|error)\\s*(?<id>[^:]*):\\s*(?<message>.*)", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    private static Regex sInternalErrorCompilerOutput = new Regex("\\s*(?<message>Internal compiler (?<type>error)) at\\s*(?<filename>.*)\\((?<line>\\d+),(?<column>\\d+)\\):\\s*(?<id>.*)", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    private static Regex sMissingMember = new Regex("[^`]*`(?<type_name>[^']+)'[^`]+`(?<member_name>[^']+)'", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    private static Regex sMissingType = new Regex("[^`]*`(?<type_name>[^']+)'[^`]+`(?<namespace>[^']+)'", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    private static Regex sUnknownTypeOrNamespace = new Regex("[^`]*`(?<type_name>[^']+)'.*", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    protected override Regex GetOutputRegex()
    {
      return MonoCSharpCompilerOutputParser.sCompilerOutput;
    }

    protected override Regex GetInternalErrorOutputRegex()
    {
      return MonoCSharpCompilerOutputParser.sInternalErrorCompilerOutput;
    }

    protected override string GetErrorIdentifier()
    {
      return "error";
    }

    protected override NormalizedCompilerStatus NormalizedStatusFor(Match match)
    {
      Match match1 = match;
      string idToCheck1 = "CS0117";
      Regex sMissingMember = MonoCSharpCompilerOutputParser.sMissingMember;
      // ISSUE: reference to a compiler-generated field
      if (MonoCSharpCompilerOutputParser.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MonoCSharpCompilerOutputParser.\u003C\u003Ef__mg\u0024cache0 = new Func<Match, Regex, NormalizedCompilerStatus>(CompilerOutputParserBase.NormalizeMemberNotFoundError);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Match, Regex, NormalizedCompilerStatus> fMgCache0 = MonoCSharpCompilerOutputParser.\u003C\u003Ef__mg\u0024cache0;
      NormalizedCompilerStatus normalizedCompilerStatus = CompilerOutputParserBase.TryNormalizeCompilerStatus(match1, idToCheck1, sMissingMember, fMgCache0);
      if (normalizedCompilerStatus.code != NormalizedCompilerStatusCode.NotNormalized)
        return normalizedCompilerStatus;
      Match match2 = match;
      string idToCheck2 = "CS0246";
      Regex unknownTypeOrNamespace1 = MonoCSharpCompilerOutputParser.sUnknownTypeOrNamespace;
      // ISSUE: reference to a compiler-generated field
      if (MonoCSharpCompilerOutputParser.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MonoCSharpCompilerOutputParser.\u003C\u003Ef__mg\u0024cache1 = new Func<Match, Regex, NormalizedCompilerStatus>(CompilerOutputParserBase.NormalizeSimpleUnknownTypeOfNamespaceError);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Match, Regex, NormalizedCompilerStatus> fMgCache1 = MonoCSharpCompilerOutputParser.\u003C\u003Ef__mg\u0024cache1;
      normalizedCompilerStatus = CompilerOutputParserBase.TryNormalizeCompilerStatus(match2, idToCheck2, unknownTypeOrNamespace1, fMgCache1);
      if (normalizedCompilerStatus.code != NormalizedCompilerStatusCode.NotNormalized)
        return normalizedCompilerStatus;
      Match match3 = match;
      string idToCheck3 = "CS0234";
      Regex sMissingType = MonoCSharpCompilerOutputParser.sMissingType;
      // ISSUE: reference to a compiler-generated field
      if (MonoCSharpCompilerOutputParser.\u003C\u003Ef__mg\u0024cache2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MonoCSharpCompilerOutputParser.\u003C\u003Ef__mg\u0024cache2 = new Func<Match, Regex, NormalizedCompilerStatus>(CompilerOutputParserBase.NormalizeUnknownTypeMemberOfNamespaceError);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Match, Regex, NormalizedCompilerStatus> fMgCache2 = MonoCSharpCompilerOutputParser.\u003C\u003Ef__mg\u0024cache2;
      normalizedCompilerStatus = CompilerOutputParserBase.TryNormalizeCompilerStatus(match3, idToCheck3, sMissingType, fMgCache2);
      if (normalizedCompilerStatus.code != NormalizedCompilerStatusCode.NotNormalized)
        return normalizedCompilerStatus;
      Match match4 = match;
      string idToCheck4 = "CS0103";
      Regex unknownTypeOrNamespace2 = MonoCSharpCompilerOutputParser.sUnknownTypeOrNamespace;
      // ISSUE: reference to a compiler-generated field
      if (MonoCSharpCompilerOutputParser.\u003C\u003Ef__mg\u0024cache3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MonoCSharpCompilerOutputParser.\u003C\u003Ef__mg\u0024cache3 = new Func<Match, Regex, NormalizedCompilerStatus>(CompilerOutputParserBase.NormalizeSimpleUnknownTypeOfNamespaceError);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Match, Regex, NormalizedCompilerStatus> fMgCache3 = MonoCSharpCompilerOutputParser.\u003C\u003Ef__mg\u0024cache3;
      return CompilerOutputParserBase.TryNormalizeCompilerStatus(match4, idToCheck4, unknownTypeOrNamespace2, fMgCache3);
    }
  }
}
