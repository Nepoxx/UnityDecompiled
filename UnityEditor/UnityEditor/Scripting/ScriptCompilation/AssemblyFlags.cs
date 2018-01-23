// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilation.AssemblyFlags
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Scripting.ScriptCompilation
{
  [System.Flags]
  internal enum AssemblyFlags
  {
    None = 0,
    EditorOnly = 1,
    UseForMono = 2,
    UseForDotNet = 4,
    FirstPass = 8,
    ExcludedForRuntimeCode = 16, // 0x00000010
  }
}
