// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.PseudoStates
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  [Flags]
  internal enum PseudoStates
  {
    Active = 1,
    Hover = 2,
    Checked = 8,
    Selected = 16, // 0x00000010
    Disabled = 32, // 0x00000020
    Focus = 64, // 0x00000040
    Invisible = -2147483648, // -0x80000000
  }
}
