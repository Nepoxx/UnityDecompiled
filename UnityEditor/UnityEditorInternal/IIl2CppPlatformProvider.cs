// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.IIl2CppPlatformProvider
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEditor.BuildReporting;
using UnityEditor.Scripting.Compilers;

namespace UnityEditorInternal
{
  internal interface IIl2CppPlatformProvider
  {
    BuildTarget target { get; }

    bool emitNullChecks { get; }

    bool enableStackTraces { get; }

    bool enableArrayBoundsCheck { get; }

    bool enableDivideByZeroCheck { get; }

    string nativeLibraryFileName { get; }

    string il2CppFolder { get; }

    bool developmentMode { get; }

    string moduleStrippingInformationFolder { get; }

    bool supportsEngineStripping { get; }

    BuildReport buildReport { get; }

    string[] includePaths { get; }

    string[] libraryPaths { get; }

    INativeCompiler CreateNativeCompiler();

    Il2CppNativeCodeBuilder CreateIl2CppNativeCodeBuilder();

    CompilerOutputParserBase CreateIl2CppOutputParser();
  }
}
