// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.UWPExtensionSDK
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Scripting.Compilers
{
  internal struct UWPExtensionSDK
  {
    public readonly string Name;
    public readonly string Version;
    public readonly string ManifestPath;

    public UWPExtensionSDK(string name, string version, string manifestPath)
    {
      this.Name = name;
      this.Version = version;
      this.ManifestPath = manifestPath;
    }
  }
}
