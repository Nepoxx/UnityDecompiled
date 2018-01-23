// Decompiled with JetBrains decompiler
// Type: UnityEditor.Compilation.Assembly
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.Compilation
{
  /// <summary>
  ///   <para>Class that represents an assembly compiled by Unity.</para>
  /// </summary>
  public class Assembly
  {
    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    /// <param name="name">Assembly name.</param>
    /// <param name="outputPath">Assembly output.</param>
    /// <param name="sourceFiles">Assembliy source files.</param>
    /// <param name="defines">Assembly defines.</param>
    /// <param name="assemblyReferences">Assembly references.</param>
    /// <param name="compiledAssemblyReferences">Compiled assembly references.</param>
    /// <param name="flags">Assembly flags.</param>
    public Assembly(string name, string outputPath, string[] sourceFiles, string[] defines, Assembly[] assemblyReferences, string[] compiledAssemblyReferences, AssemblyFlags flags)
    {
      this.name = name;
      this.outputPath = outputPath;
      this.sourceFiles = sourceFiles;
      this.defines = defines;
      this.assemblyReferences = assemblyReferences;
      this.compiledAssemblyReferences = compiledAssemblyReferences;
      this.flags = flags;
    }

    /// <summary>
    ///   <para>The name of the assembly.</para>
    /// </summary>
    public string name { get; private set; }

    /// <summary>
    ///   <para>The full output file path of this assembly.</para>
    /// </summary>
    public string outputPath { get; private set; }

    /// <summary>
    ///   <para>All the souce files used to compile this assembly.</para>
    /// </summary>
    public string[] sourceFiles { get; private set; }

    /// <summary>
    ///   <para>The defines used to compile this assembly.</para>
    /// </summary>
    public string[] defines { get; private set; }

    /// <summary>
    ///         <para>Assembly references used to build this assembly.
    /// 
    /// The references are also assemblies built as part of the Unity project.
    /// 
    /// See Also: Assembly.compiledAssemblyReferences and Assembly.allReferences.</para>
    ///       </summary>
    public Assembly[] assemblyReferences { get; private set; }

    /// <summary>
    ///         <para>Assembly references to pre-compiled assemblies that used to build this assembly.
    /// 
    /// See Also: Assembly.assemblyReferences and Assembly.allReferences.</para>
    ///       </summary>
    public string[] compiledAssemblyReferences { get; private set; }

    /// <summary>
    ///         <para>Flags for the assembly.
    /// 
    /// See Also: AssemblyFlags.</para>
    ///       </summary>
    public AssemblyFlags flags { get; private set; }

    /// <summary>
    ///         <para>Returns Assembly.assemblyReferences and Assembly.compiledAssemblyReferences combined.
    /// 
    /// This returns all assemblies that are passed to the compiler when building this assembly,.</para>
    ///       </summary>
    public string[] allReferences
    {
      get
      {
        return ((IEnumerable<Assembly>) this.assemblyReferences).Select<Assembly, string>((Func<Assembly, string>) (a => a.outputPath)).Concat<string>((IEnumerable<string>) this.compiledAssemblyReferences).ToArray<string>();
      }
    }
  }
}
