// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.CompilerOutputParserBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnityEditor.Scripting.Compilers
{
  internal abstract class CompilerOutputParserBase
  {
    protected static CompilerMessage CreateInternalCompilerErrorMessage(string[] compileroutput)
    {
      CompilerMessage compilerMessage;
      compilerMessage.file = "";
      compilerMessage.message = string.Join("\n", compileroutput);
      compilerMessage.type = CompilerMessageType.Error;
      compilerMessage.line = 0;
      compilerMessage.column = 0;
      compilerMessage.normalizedStatus = new NormalizedCompilerStatus();
      return compilerMessage;
    }

    protected internal static CompilerMessage CreateCompilerMessageFromMatchedRegex(string line, Match m, string erroridentifier)
    {
      CompilerMessage compilerMessage;
      compilerMessage.file = m.Groups["filename"].Value;
      compilerMessage.message = line;
      compilerMessage.line = int.Parse(m.Groups[nameof (line)].Value);
      compilerMessage.column = int.Parse(m.Groups["column"].Value);
      compilerMessage.type = !(m.Groups["type"].Value == erroridentifier) ? CompilerMessageType.Warning : CompilerMessageType.Error;
      compilerMessage.normalizedStatus = new NormalizedCompilerStatus();
      return compilerMessage;
    }

    public virtual IEnumerable<CompilerMessage> Parse(string[] errorOutput, bool compilationHadFailure)
    {
      return this.Parse(errorOutput, new string[0], compilationHadFailure);
    }

    public virtual IEnumerable<CompilerMessage> Parse(string[] errorOutput, string[] standardOutput, bool compilationHadFailure)
    {
      bool flag = false;
      List<CompilerMessage> compilerMessageList = new List<CompilerMessage>();
      Regex outputRegex = this.GetOutputRegex();
      Regex errorOutputRegex = this.GetInternalErrorOutputRegex();
      foreach (string line in errorOutput)
      {
        string input = line.Length <= 1000 ? line : line.Substring(0, 100);
        Match match = outputRegex.Match(input);
        if (!match.Success)
        {
          if (errorOutputRegex != null)
            match = errorOutputRegex.Match(input);
          if (!match.Success)
            continue;
        }
        CompilerMessage fromMatchedRegex = CompilerOutputParserBase.CreateCompilerMessageFromMatchedRegex(line, match, this.GetErrorIdentifier());
        fromMatchedRegex.normalizedStatus = this.NormalizedStatusFor(match);
        if (fromMatchedRegex.type == CompilerMessageType.Error)
          flag = true;
        compilerMessageList.Add(fromMatchedRegex);
      }
      if (compilationHadFailure && !flag)
        compilerMessageList.Add(CompilerOutputParserBase.CreateInternalCompilerErrorMessage(errorOutput));
      return (IEnumerable<CompilerMessage>) compilerMessageList;
    }

    protected virtual NormalizedCompilerStatus NormalizedStatusFor(Match match)
    {
      return new NormalizedCompilerStatus();
    }

    protected abstract string GetErrorIdentifier();

    protected abstract Regex GetOutputRegex();

    protected virtual Regex GetInternalErrorOutputRegex()
    {
      return (Regex) null;
    }

    protected static NormalizedCompilerStatus TryNormalizeCompilerStatus(Match match, string idToCheck, Regex messageParser, Func<Match, Regex, NormalizedCompilerStatus> normalizer)
    {
      string str = match.Groups["id"].Value;
      NormalizedCompilerStatus normalizedCompilerStatus = new NormalizedCompilerStatus();
      if (str != idToCheck)
        return normalizedCompilerStatus;
      return normalizer(match, messageParser);
    }

    protected static NormalizedCompilerStatus NormalizeMemberNotFoundError(Match outputMatch, Regex messageParser)
    {
      NormalizedCompilerStatus normalizedCompilerStatus;
      normalizedCompilerStatus.code = NormalizedCompilerStatusCode.MemberNotFound;
      Match match = messageParser.Match(outputMatch.Groups["message"].Value);
      normalizedCompilerStatus.details = match.Groups["type_name"].Value + "%" + match.Groups["member_name"].Value;
      return normalizedCompilerStatus;
    }

    protected static NormalizedCompilerStatus NormalizeSimpleUnknownTypeOfNamespaceError(Match outputMatch, Regex messageParser)
    {
      NormalizedCompilerStatus normalizedCompilerStatus;
      normalizedCompilerStatus.code = NormalizedCompilerStatusCode.UnknownTypeOrNamespace;
      Match match = messageParser.Match(outputMatch.Groups["message"].Value);
      normalizedCompilerStatus.details = "EntityName=" + match.Groups["type_name"].Value + "\nScript=" + outputMatch.Groups["filename"].Value + "\nLine=" + outputMatch.Groups["line"].Value + "\nColumn=" + outputMatch.Groups["column"].Value;
      return normalizedCompilerStatus;
    }

    protected static NormalizedCompilerStatus NormalizeUnknownTypeMemberOfNamespaceError(Match outputMatch, Regex messageParser)
    {
      NormalizedCompilerStatus normalizedCompilerStatus;
      normalizedCompilerStatus.code = NormalizedCompilerStatusCode.UnknownTypeOrNamespace;
      Match match = messageParser.Match(outputMatch.Groups["message"].Value);
      normalizedCompilerStatus.details = "EntityName=" + match.Groups["namespace"].Value + "." + match.Groups["type_name"].Value + "\nScript=" + outputMatch.Groups["filename"].Value + "\nLine=" + outputMatch.Groups["line"].Value + "\nColumn=" + outputMatch.Groups["column"].Value;
      return normalizedCompilerStatus;
    }
  }
}
