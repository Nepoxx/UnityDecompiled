// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.GendarmeOutputParser
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace UnityEditor.Scripting.Compilers
{
  internal class GendarmeOutputParser : UnityScriptCompilerOutputParser
  {
    public override IEnumerable<CompilerMessage> Parse(string[] errorOutput, bool compilationHadFailure)
    {
      throw new ArgumentException("Gendarme Output Parser needs standard out");
    }

    [DebuggerHidden]
    public override IEnumerable<CompilerMessage> Parse(string[] errorOutput, string[] standardOutput, bool compilationHadFailure)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GendarmeOutputParser.\u003CParse\u003Ec__Iterator0 parseCIterator0 = new GendarmeOutputParser.\u003CParse\u003Ec__Iterator0() { standardOutput = standardOutput };
      // ISSUE: reference to a compiler-generated field
      parseCIterator0.\u0024PC = -2;
      return (IEnumerable<CompilerMessage>) parseCIterator0;
    }

    private static CompilerMessage CompilerErrorFor(GendarmeRuleData gendarmeRuleData)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(gendarmeRuleData.Problem);
      stringBuilder.AppendLine(gendarmeRuleData.Details);
      stringBuilder.AppendLine(string.IsNullOrEmpty(gendarmeRuleData.Location) ? string.Format("{0} at line : {1}", (object) gendarmeRuleData.Source, (object) gendarmeRuleData.Line) : gendarmeRuleData.Location);
      string str = stringBuilder.ToString();
      return new CompilerMessage() { type = CompilerMessageType.Error, message = str, file = gendarmeRuleData.File, line = gendarmeRuleData.Line, column = 1 };
    }

    private static GendarmeRuleData GetGendarmeRuleDataFor(IList<string> output, int index)
    {
      GendarmeRuleData gendarmeRuleData = new GendarmeRuleData();
      for (int index1 = index; index1 < output.Count; ++index1)
      {
        string currentLine = output[index1];
        if (currentLine.StartsWith("Problem:"))
          gendarmeRuleData.Problem = currentLine.Substring(currentLine.LastIndexOf("Problem:", StringComparison.Ordinal) + "Problem:".Length);
        else if (currentLine.StartsWith("* Details"))
          gendarmeRuleData.Details = currentLine;
        else if (currentLine.StartsWith("* Source"))
        {
          gendarmeRuleData.IsAssemblyError = false;
          gendarmeRuleData.Source = currentLine;
          gendarmeRuleData.Line = GendarmeOutputParser.GetLineNumberFrom(currentLine);
          gendarmeRuleData.File = GendarmeOutputParser.GetFileNameFrome(currentLine);
        }
        else if (currentLine.StartsWith("* Severity"))
          gendarmeRuleData.Severity = currentLine;
        else if (currentLine.StartsWith("* Location"))
        {
          gendarmeRuleData.IsAssemblyError = true;
          gendarmeRuleData.Location = currentLine;
        }
        else if (currentLine.StartsWith("* Target"))
        {
          gendarmeRuleData.Target = currentLine;
        }
        else
        {
          gendarmeRuleData.LastIndex = index1;
          break;
        }
      }
      return gendarmeRuleData;
    }

    private static string GetFileNameFrome(string currentLine)
    {
      int startIndex = currentLine.LastIndexOf("* Source:") + "* Source:".Length;
      int num = currentLine.IndexOf("(");
      if (startIndex != -1 && num != -1)
        return currentLine.Substring(startIndex, num - startIndex).Trim();
      return "";
    }

    private static int GetLineNumberFrom(string currentLine)
    {
      int startIndex = currentLine.IndexOf("(") + 2;
      int num = currentLine.IndexOf(")");
      if (startIndex != -1 && num != -1)
        return int.Parse(currentLine.Substring(startIndex, num - startIndex));
      return 0;
    }
  }
}
