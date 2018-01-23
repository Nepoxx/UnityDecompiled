// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilers
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Scripting.Compilers;

namespace UnityEditor.Scripting
{
  internal static class ScriptCompilers
  {
    internal static readonly List<SupportedLanguage> SupportedLanguages = new List<SupportedLanguage>();
    internal static readonly SupportedLanguage CSharpSupportedLanguage;

    static ScriptCompilers()
    {
      foreach (System.Type type in new List<System.Type>() { typeof (CSharpLanguage), typeof (BooLanguage), typeof (UnityScriptLanguage) })
        ScriptCompilers.SupportedLanguages.Add((SupportedLanguage) Activator.CreateInstance(type));
      ScriptCompilers.CSharpSupportedLanguage = ScriptCompilers.SupportedLanguages.Single<SupportedLanguage>((Func<SupportedLanguage, bool>) (l => l.GetType() == typeof (CSharpLanguage)));
    }

    internal static SupportedLanguageStruct[] GetSupportedLanguageStructs()
    {
      return ScriptCompilers.SupportedLanguages.Select<SupportedLanguage, SupportedLanguageStruct>((Func<SupportedLanguage, SupportedLanguageStruct>) (lang => new SupportedLanguageStruct() { extension = lang.GetExtensionICanCompile(), languageName = lang.GetLanguageName() })).ToArray<SupportedLanguageStruct>();
    }

    internal static string GetNamespace(string file, string definedSymbols)
    {
      if (string.IsNullOrEmpty(file))
        throw new ArgumentException("Invalid file");
      string extensionOfSourceFile = ScriptCompilers.GetExtensionOfSourceFile(file);
      foreach (SupportedLanguage supportedLanguage in ScriptCompilers.SupportedLanguages)
      {
        if (supportedLanguage.GetExtensionICanCompile() == extensionOfSourceFile)
          return supportedLanguage.GetNamespace(file, definedSymbols);
      }
      throw new ApplicationException("Unable to find a suitable compiler");
    }

    internal static SupportedLanguage GetLanguageFromName(string name)
    {
      foreach (SupportedLanguage supportedLanguage in ScriptCompilers.SupportedLanguages)
      {
        if (string.Equals(name, supportedLanguage.GetLanguageName(), StringComparison.OrdinalIgnoreCase))
          return supportedLanguage;
      }
      throw new ApplicationException(string.Format("Script language '{0}' is not supported", (object) name));
    }

    internal static SupportedLanguage GetLanguageFromExtension(string extension)
    {
      foreach (SupportedLanguage supportedLanguage in ScriptCompilers.SupportedLanguages)
      {
        if (string.Equals(extension, supportedLanguage.GetExtensionICanCompile(), StringComparison.OrdinalIgnoreCase))
          return supportedLanguage;
      }
      throw new ApplicationException(string.Format("Script file extension '{0}' is not supported", (object) extension));
    }

    internal static ScriptCompilerBase CreateCompilerInstance(MonoIsland island, bool buildingForEditor, BuildTarget targetPlatform, bool runUpdater)
    {
      if (island._files.Length == 0)
        throw new ArgumentException("Cannot compile MonoIsland with no files");
      foreach (SupportedLanguage supportedLanguage in ScriptCompilers.SupportedLanguages)
      {
        if (supportedLanguage.GetExtensionICanCompile() == island.GetExtensionOfSourceFiles())
          return supportedLanguage.CreateCompiler(island, buildingForEditor, targetPlatform, runUpdater);
      }
      throw new ApplicationException(string.Format("Unable to find a suitable compiler for sources with extension '{0}' (Output assembly: {1})", (object) island.GetExtensionOfSourceFiles(), (object) island._output));
    }

    public static string GetExtensionOfSourceFile(string file)
    {
      return Path.GetExtension(file).ToLower().Substring(1);
    }
  }
}
