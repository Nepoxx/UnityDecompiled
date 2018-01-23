// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.Il2CppNativeCodeBuilder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace UnityEditorInternal
{
  public abstract class Il2CppNativeCodeBuilder
  {
    public abstract string CompilerPlatform { get; }

    public abstract string CompilerArchitecture { get; }

    public virtual string CompilerFlags
    {
      get
      {
        return string.Empty;
      }
    }

    public virtual string LinkerFlags
    {
      get
      {
        return string.Empty;
      }
    }

    public virtual bool SetsUpEnvironment
    {
      get
      {
        return false;
      }
    }

    public virtual string CacheDirectory
    {
      get
      {
        return string.Empty;
      }
    }

    public virtual string PluginPath
    {
      get
      {
        return string.Empty;
      }
    }

    public virtual IEnumerable<string> AdditionalIl2CPPArguments
    {
      get
      {
        return (IEnumerable<string>) new string[0];
      }
    }

    public virtual bool LinkLibIl2CppStatically
    {
      get
      {
        return true;
      }
    }

    public virtual IEnumerable<string> ConvertIncludesToFullPaths(IEnumerable<string> relativeIncludePaths)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return relativeIncludePaths.Select<string, string>(new Func<string, string>(new Il2CppNativeCodeBuilder.\u003CConvertIncludesToFullPaths\u003Ec__AnonStorey0() { workingDirectory = Directory.GetCurrentDirectory() }.\u003C\u003Em__0));
    }

    public virtual string ConvertOutputFileToFullPath(string outputFileRelativePath)
    {
      return Path.Combine(Directory.GetCurrentDirectory(), outputFileRelativePath);
    }

    public void SetupStartInfo(ProcessStartInfo startInfo)
    {
      if (!this.SetsUpEnvironment)
        return;
      this.SetupEnvironment(startInfo);
    }

    protected virtual void SetupEnvironment(ProcessStartInfo startInfo)
    {
    }
  }
}
