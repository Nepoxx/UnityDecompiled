// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.ICompilationExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using Mono.Cecil;
using System.Collections.Generic;

namespace UnityEditor.Modules
{
  internal interface ICompilationExtension
  {
    CSharpCompiler GetCsCompiler(bool buildingForEditor, string assemblyName);

    string[] GetCompilerExtraAssemblyPaths(bool isEditor, string assemblyPathName);

    IAssemblyResolver GetAssemblyResolver(bool buildingForEditor, string assemblyPath, string[] searchDirectories);

    IEnumerable<string> GetWindowsMetadataReferences();

    IEnumerable<string> GetAdditionalAssemblyReferences();

    IEnumerable<string> GetAdditionalDefines();

    IEnumerable<string> GetAdditionalSourceFiles();
  }
}
