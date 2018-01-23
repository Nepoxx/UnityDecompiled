// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventModifiers
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Types of modifier key that can be active during a keystroke event.</para>
  /// </summary>
  [Flags]
  public enum EventModifiers
  {
    None = 0,
    Shift = 1,
    Control = 2,
    Alt = 4,
    Command = 8,
    Numeric = 16, // 0x00000010
    CapsLock = 32, // 0x00000020
    FunctionKey = 64, // 0x00000040
  }
}
