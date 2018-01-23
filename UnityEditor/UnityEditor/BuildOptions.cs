// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildOptions
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Building options. Multiple options can be combined together.</para>
  /// </summary>
  [System.Flags]
  public enum BuildOptions
  {
    None = 0,
    Development = 1,
    AutoRunPlayer = 4,
    ShowBuiltPlayer = 8,
    BuildAdditionalStreamedScenes = 16, // 0x00000010
    AcceptExternalModificationsToPlayer = 32, // 0x00000020
    InstallInBuildFolder = 64, // 0x00000040
    [Obsolete("WebPlayer has been removed in 5.4", true)] WebPlayerOfflineDeployment = 128, // 0x00000080
    ConnectWithProfiler = 256, // 0x00000100
    AllowDebugging = 512, // 0x00000200
    SymlinkLibraries = 1024, // 0x00000400
    UncompressedAssetBundle = 2048, // 0x00000800
    [Obsolete("Use BuildOptions.Development instead")] StripDebugSymbols = 0,
    [Obsolete("Texture Compression is now always enabled")] CompressTextures = 0,
    ConnectToHost = 4096, // 0x00001000
    EnableHeadlessMode = 16384, // 0x00004000
    BuildScriptsOnly = 32768, // 0x00008000
    Il2CPP = 65536, // 0x00010000
    ForceEnableAssertions = 131072, // 0x00020000
    CompressWithLz4 = 262144, // 0x00040000
    CompressWithLz4HC = 524288, // 0x00080000
    [Obsolete("Specify IL2CPP optimization level in Player Settings.")] ForceOptimizeScriptCompilation = 0,
    ComputeCRC = 1048576, // 0x00100000
    StrictMode = 2097152, // 0x00200000
  }
}
