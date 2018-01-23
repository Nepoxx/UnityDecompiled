// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.DefaultProjectGeneratorExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor.Modules
{
  internal abstract class DefaultProjectGeneratorExtension : IProjectGeneratorExtension
  {
    public virtual void GenerateCSharpProject(CSharpProject project, string assemblyName, IEnumerable<string> sourceFiles, IEnumerable<string> defines, IEnumerable<CSharpProject> additionalProjectReferences)
    {
    }
  }
}
