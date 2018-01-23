// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.InstrumentedAssemblyTypes
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditorInternal
{
  [Flags]
  public enum InstrumentedAssemblyTypes
  {
    None = 0,
    System = 1,
    Unity = 2,
    Plugins = 4,
    Script = 8,
    All = 2147483647, // 0x7FFFFFFF
  }
}
