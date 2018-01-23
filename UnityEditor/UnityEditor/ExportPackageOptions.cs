// Decompiled with JetBrains decompiler
// Type: UnityEditor.ExportPackageOptions
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Export package option. Multiple options can be combined together using the | operator.</para>
  /// </summary>
  [System.Flags]
  public enum ExportPackageOptions
  {
    Default = 0,
    Interactive = 1,
    Recurse = 2,
    IncludeDependencies = 4,
    IncludeLibraryAssets = 8,
  }
}
