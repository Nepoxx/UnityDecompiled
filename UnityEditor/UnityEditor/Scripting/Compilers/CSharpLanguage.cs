// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.CSharpLanguage
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor.Modules;

namespace UnityEditor.Scripting.Compilers
{
  internal class CSharpLanguage : SupportedLanguage
  {
    private static Regex _crOnlyRegex = new Regex("\r(?!\n)", RegexOptions.Compiled);
    private static Regex _lfOnlyRegex = new Regex("(?<!\r)\n", RegexOptions.Compiled);

    public override string GetExtensionICanCompile()
    {
      return "cs";
    }

    public override string GetLanguageName()
    {
      return "CSharp";
    }

    internal static CSharpCompiler GetCSharpCompiler(BuildTarget targetPlatform, bool buildingForEditor, string assemblyName)
    {
      return ModuleManager.GetCompilationExtension(ModuleManager.GetTargetStringFromBuildTarget(targetPlatform)).GetCsCompiler(buildingForEditor, assemblyName);
    }

    public override ScriptCompilerBase CreateCompiler(MonoIsland island, bool buildingForEditor, BuildTarget targetPlatform, bool runUpdater)
    {
      switch (CSharpLanguage.GetCSharpCompiler(targetPlatform, buildingForEditor, island._output))
      {
        case CSharpCompiler.Microsoft:
          return (ScriptCompilerBase) new MicrosoftCSharpCompiler(island, runUpdater);
        default:
          return (ScriptCompilerBase) new MonoCSharpCompiler(island, runUpdater);
      }
    }

    public override string GetNamespace(string fileName, string definedSymbols)
    {
      using (IParser parser = ParserFactory.CreateParser(ICSharpCode.NRefactory.SupportedLanguage.CSharp, (TextReader) CSharpLanguage.ReadAndConverteNewLines(fileName)))
      {
        string str = definedSymbols;
        char[] separator = new char[1]{ ',' };
        int num = 1;
        foreach (string key in new HashSet<string>((IEnumerable<string>) str.Split(separator, (StringSplitOptions) num)))
          parser.Lexer.ConditionalCompilationSymbols.Add(key, (object) string.Empty);
        parser.Lexer.EvaluateConditionalCompilation = true;
        parser.Parse();
        try
        {
          CSharpLanguage.NamespaceVisitor namespaceVisitor = new CSharpLanguage.NamespaceVisitor();
          CSharpLanguage.VisitorData visitorData = new CSharpLanguage.VisitorData() { TargetClassName = Path.GetFileNameWithoutExtension(fileName) };
          parser.CompilationUnit.AcceptVisitor((IAstVisitor) namespaceVisitor, (object) visitorData);
          return !string.IsNullOrEmpty(visitorData.DiscoveredNamespace) ? visitorData.DiscoveredNamespace : string.Empty;
        }
        catch
        {
        }
      }
      return string.Empty;
    }

    private static StringReader ReadAndConverteNewLines(string filePath)
    {
      string input1 = File.ReadAllText(filePath);
      string input2 = CSharpLanguage._crOnlyRegex.Replace(input1, "\r\n");
      return new StringReader(CSharpLanguage._lfOnlyRegex.Replace(input2, "\r\n"));
    }

    private class VisitorData
    {
      public string TargetClassName;
      public Stack<string> CurrentNamespaces;
      public string DiscoveredNamespace;

      public VisitorData()
      {
        this.CurrentNamespaces = new Stack<string>();
      }
    }

    private class NamespaceVisitor : AbstractAstVisitor
    {
      public override object VisitNamespaceDeclaration(NamespaceDeclaration namespaceDeclaration, object data)
      {
        CSharpLanguage.VisitorData visitorData = (CSharpLanguage.VisitorData) data;
        visitorData.CurrentNamespaces.Push(namespaceDeclaration.Name);
        namespaceDeclaration.AcceptChildren((IAstVisitor) this, (object) visitorData);
        visitorData.CurrentNamespaces.Pop();
        return (object) null;
      }

      public override object VisitTypeDeclaration(TypeDeclaration typeDeclaration, object data)
      {
        CSharpLanguage.VisitorData visitorData = (CSharpLanguage.VisitorData) data;
        if (typeDeclaration.Name == visitorData.TargetClassName)
        {
          string str = string.Empty;
          foreach (string currentNamespace in visitorData.CurrentNamespaces)
            str = !(str == string.Empty) ? currentNamespace + "." + str : currentNamespace;
          visitorData.DiscoveredNamespace = str;
        }
        return (object) null;
      }
    }
  }
}
