// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilation.EditorScriptCompilationOptions
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Scripting.ScriptCompilation
{
  [System.Flags]
  internal enum EditorScriptCompilationOptions
  {
    BuildingEmpty = 0,
    BuildingDevelopmentBuild = 1,
    BuildingForEditor = 2,
    BuildingEditorOnlyAssembly = 4,
    BuildingForIl2Cpp = 8,
    BuildingWithAsserts = 16, // 0x00000010
  }
}
