// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.CommandLineFormatter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityEditor.Scripting.Compilers
{
  internal static class CommandLineFormatter
  {
    private static readonly Regex UnsafeCharsWindows = new Regex("[^A-Za-z0-9_\\-\\.\\:\\,\\/\\@\\\\]");
    private static readonly Regex UnescapeableChars = new Regex("[\\x00-\\x08\\x10-\\x1a\\x1c-\\x1f\\x7f\\xff]");
    private static readonly Regex Quotes = new Regex("\"");

    public static string EscapeCharsQuote(string input)
    {
      if (input.IndexOf('\'') == -1)
        return "'" + input + "'";
      if (input.IndexOf('"') == -1)
        return "\"" + input + "\"";
      return (string) null;
    }

    public static string PrepareFileName(string input)
    {
      input = FileUtil.ResolveSymlinks(input);
      if (Application.platform == RuntimePlatform.WindowsEditor)
        return CommandLineFormatter.EscapeCharsWindows(input);
      return CommandLineFormatter.EscapeCharsQuote(input);
    }

    public static string EscapeCharsWindows(string input)
    {
      if (input.Length == 0)
        return "\"\"";
      if (CommandLineFormatter.UnescapeableChars.IsMatch(input))
      {
        Debug.LogWarning((object) "Cannot escape control characters in string");
        return "\"\"";
      }
      if (CommandLineFormatter.UnsafeCharsWindows.IsMatch(input))
        return "\"" + CommandLineFormatter.Quotes.Replace(input, "\"\"") + "\"";
      return input;
    }

    internal static string GenerateResponseFile(IEnumerable<string> arguments)
    {
      string tempPathInProject = FileUtil.GetUniqueTempPathInProject();
      using (StreamWriter streamWriter = new StreamWriter(tempPathInProject))
      {
        foreach (string str in arguments.Where<string>((Func<string, bool>) (a => a != null)))
          streamWriter.WriteLine(str);
      }
      return tempPathInProject;
    }
  }
}
