// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.Il2CppOutputParser
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace UnityEditor.Scripting.Compilers
{
  internal class Il2CppOutputParser : CompilerOutputParserBase
  {
    private static readonly Regex sErrorRegexWithSourceInformation = new Regex("\\s*(?<message>.*) in (?<filename>.*):(?<line>\\d+)");
    private const string _errorIdentifier = "IL2CPP error";

    public override IEnumerable<CompilerMessage> Parse(string[] errorOutput, string[] standardOutput, bool compilationHadFailure)
    {
      List<CompilerMessage> compilerMessageList = new List<CompilerMessage>();
      for (int index = 0; index < standardOutput.Length; ++index)
      {
        string input = standardOutput[index];
        if (input.StartsWith("IL2CPP error"))
        {
          string empty = string.Empty;
          int num = 0;
          StringBuilder stringBuilder = new StringBuilder();
          Match match = Il2CppOutputParser.sErrorRegexWithSourceInformation.Match(input);
          if (match.Success)
          {
            empty = match.Groups["filename"].Value;
            num = int.Parse(match.Groups["line"].Value);
            stringBuilder.AppendFormat("{0} in {1}:{2}", (object) match.Groups["message"].Value, (object) Path.GetFileName(empty), (object) num);
          }
          else
            stringBuilder.Append(input);
          if (index + 1 < standardOutput.Length && standardOutput[index + 1].StartsWith("Additional information:"))
          {
            stringBuilder.AppendFormat("{0}{1}", (object) Environment.NewLine, (object) standardOutput[index + 1]);
            ++index;
          }
          compilerMessageList.Add(new CompilerMessage()
          {
            file = empty,
            line = num,
            message = stringBuilder.ToString(),
            type = CompilerMessageType.Error
          });
        }
      }
      return (IEnumerable<CompilerMessage>) compilerMessageList;
    }

    protected override string GetErrorIdentifier()
    {
      return "IL2CPP error";
    }

    protected override Regex GetOutputRegex()
    {
      return Il2CppOutputParser.sErrorRegexWithSourceInformation;
    }
  }
}
