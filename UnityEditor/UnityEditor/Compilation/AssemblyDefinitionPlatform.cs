// Decompiled with JetBrains decompiler
// Type: UnityEditor.Compilation.AssemblyDefinitionPlatform
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.InteropServices;

namespace UnityEditor.Compilation
{
  /// <summary>
  ///   <para>Contains information about a platform supported by the assembly definition files.</para>
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct AssemblyDefinitionPlatform
  {
    internal AssemblyDefinitionPlatform(string name, string displayName, BuildTarget buildTarget)
    {
      this = new AssemblyDefinitionPlatform();
      this.Name = name;
      this.DisplayName = displayName;
      this.BuildTarget = buildTarget;
    }

    /// <summary>
    ///   <para>Name used in assembly definition files.</para>
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///   <para>Display name for the platform.</para>
    /// </summary>
    public string DisplayName { get; private set; }

    /// <summary>
    ///   <para>BuildTarget for the AssemblyDefinitionPlatform.</para>
    /// </summary>
    public BuildTarget BuildTarget { get; private set; }
  }
}
