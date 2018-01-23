// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.BaseIl2CppPlatformProvider
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.BuildReporting;
using UnityEditor.Modules;
using UnityEditor.Scripting.Compilers;

namespace UnityEditorInternal
{
  internal class BaseIl2CppPlatformProvider : IIl2CppPlatformProvider
  {
    public BaseIl2CppPlatformProvider(BuildTarget target, string libraryFolder)
    {
      this.target = target;
      this.libraryFolder = libraryFolder;
    }

    public virtual BuildTarget target { get; private set; }

    public virtual string libraryFolder { get; private set; }

    public virtual bool developmentMode
    {
      get
      {
        return false;
      }
    }

    public virtual bool emitNullChecks
    {
      get
      {
        return true;
      }
    }

    public virtual bool enableStackTraces
    {
      get
      {
        return true;
      }
    }

    public virtual bool enableArrayBoundsCheck
    {
      get
      {
        return true;
      }
    }

    public virtual bool enableDivideByZeroCheck
    {
      get
      {
        return false;
      }
    }

    public virtual bool supportsEngineStripping
    {
      get
      {
        return false;
      }
    }

    public virtual BuildReport buildReport
    {
      get
      {
        return (BuildReport) null;
      }
    }

    public virtual string[] includePaths
    {
      get
      {
        return new string[2]{ this.GetFolderInPackageOrDefault("bdwgc/include"), this.GetFolderInPackageOrDefault("libil2cpp/include") };
      }
    }

    public virtual string[] libraryPaths
    {
      get
      {
        return new string[2]{ this.GetFileInPackageOrDefault("bdwgc/lib/bdwgc." + this.staticLibraryExtension), this.GetFileInPackageOrDefault("libil2cpp/lib/libil2cpp." + this.staticLibraryExtension) };
      }
    }

    public virtual string nativeLibraryFileName
    {
      get
      {
        return (string) null;
      }
    }

    public virtual string staticLibraryExtension
    {
      get
      {
        return "a";
      }
    }

    public virtual string il2CppFolder
    {
      get
      {
        Unity.DataContract.PackageInfo il2CppPackage = BaseIl2CppPlatformProvider.FindIl2CppPackage();
        if (il2CppPackage == (Unity.DataContract.PackageInfo) null)
          return Path.GetFullPath(Path.Combine(EditorApplication.applicationContentsPath, "il2cpp"));
        return il2CppPackage.basePath;
      }
    }

    public virtual string moduleStrippingInformationFolder
    {
      get
      {
        return Path.Combine(BuildPipeline.GetPlaybackEngineDirectory(EditorUserBuildSettings.activeBuildTarget, BuildOptions.None), "Whitelists");
      }
    }

    public virtual INativeCompiler CreateNativeCompiler()
    {
      return (INativeCompiler) null;
    }

    public virtual Il2CppNativeCodeBuilder CreateIl2CppNativeCodeBuilder()
    {
      return (Il2CppNativeCodeBuilder) null;
    }

    public virtual CompilerOutputParserBase CreateIl2CppOutputParser()
    {
      return (CompilerOutputParserBase) null;
    }

    protected string GetFolderInPackageOrDefault(string path)
    {
      Unity.DataContract.PackageInfo il2CppPackage = BaseIl2CppPlatformProvider.FindIl2CppPackage();
      if (il2CppPackage == (Unity.DataContract.PackageInfo) null)
        return Path.Combine(this.libraryFolder, path);
      string path1 = Path.Combine(il2CppPackage.basePath, path);
      return Directory.Exists(path1) ? path1 : Path.Combine(this.libraryFolder, path);
    }

    protected string GetFileInPackageOrDefault(string path)
    {
      Unity.DataContract.PackageInfo il2CppPackage = BaseIl2CppPlatformProvider.FindIl2CppPackage();
      if (il2CppPackage == (Unity.DataContract.PackageInfo) null)
        return Path.Combine(this.libraryFolder, path);
      string path1 = Path.Combine(il2CppPackage.basePath, path);
      return File.Exists(path1) ? path1 : Path.Combine(this.libraryFolder, path);
    }

    private static Unity.DataContract.PackageInfo FindIl2CppPackage()
    {
      return ModuleManager.packageManager.unityExtensions.FirstOrDefault<Unity.DataContract.PackageInfo>((Func<Unity.DataContract.PackageInfo, bool>) (e => e.name == "IL2CPP"));
    }
  }
}
