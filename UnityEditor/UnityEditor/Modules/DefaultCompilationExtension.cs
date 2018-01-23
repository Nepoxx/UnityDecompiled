// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.DefaultCompilationExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using Mono.Cecil;
using System.Collections.Generic;

namespace UnityEditor.Modules
{
  internal class DefaultCompilationExtension : ICompilationExtension
  {
    public virtual CSharpCompiler GetCsCompiler(bool buildingForEditor, string assemblyName)
    {
      return CSharpCompiler.Mono;
    }

    public virtual string[] GetCompilerExtraAssemblyPaths(bool isEditor, string assemblyPathName)
    {
      return new string[0];
    }

    public virtual IAssemblyResolver GetAssemblyResolver(bool buildingForEditor, string assemblyPath, string[] searchDirectories)
    {
      return (IAssemblyResolver) null;
    }

    public virtual IEnumerable<string> GetWindowsMetadataReferences()
    {
      return (IEnumerable<string>) new string[0];
    }

    public virtual IEnumerable<string> GetAdditionalAssemblyReferences()
    {
      return (IEnumerable<string>) new string[0];
    }

    public virtual IEnumerable<string> GetAdditionalDefines()
    {
      return (IEnumerable<string>) new string[0];
    }

    public virtual IEnumerable<string> GetAdditionalSourceFiles()
    {
      return (IEnumerable<string>) new string[0];
    }
  }
}
