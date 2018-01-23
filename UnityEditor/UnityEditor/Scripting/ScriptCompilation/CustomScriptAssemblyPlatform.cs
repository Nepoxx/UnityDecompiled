// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilation.CustomScriptAssemblyPlatform
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.InteropServices;

namespace UnityEditor.Scripting.ScriptCompilation
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  internal struct CustomScriptAssemblyPlatform
  {
    public CustomScriptAssemblyPlatform(string name, string displayName, BuildTarget buildTarget)
    {
      this = new CustomScriptAssemblyPlatform();
      this.Name = name;
      this.DisplayName = displayName;
      this.BuildTarget = buildTarget;
    }

    public CustomScriptAssemblyPlatform(string name, BuildTarget buildTarget)
    {
      this = new CustomScriptAssemblyPlatform(name, name, buildTarget);
    }

    public string Name { get; private set; }

    public string DisplayName { get; private set; }

    public BuildTarget BuildTarget { get; private set; }
  }
}
