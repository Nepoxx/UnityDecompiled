// Decompiled with JetBrains decompiler
// Type: UnityEditor.CrossCompileOptions
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  [System.Flags]
  internal enum CrossCompileOptions
  {
    Dynamic = 0,
    FastICall = 1,
    Static = 2,
    Debugging = 4,
    ExplicitNullChecks = 8,
    LoadSymbols = 16, // 0x00000010
  }
}
