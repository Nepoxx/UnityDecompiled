// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Types of UnityGUI input and processing events.</para>
  /// </summary>
  public enum EventType
  {
    MouseDown = 0,
    [Obsolete("Use MouseDown instead (UnityUpgradable) -> MouseDown", true)] mouseDown = 0,
    MouseUp = 1,
    [Obsolete("Use MouseUp instead (UnityUpgradable) -> MouseUp", true)] mouseUp = 1,
    MouseMove = 2,
    [Obsolete("Use MouseMove instead (UnityUpgradable) -> MouseMove", true)] mouseMove = 2,
    MouseDrag = 3,
    [Obsolete("Use MouseDrag instead (UnityUpgradable) -> MouseDrag", true)] mouseDrag = 3,
    KeyDown = 4,
    [Obsolete("Use KeyDown instead (UnityUpgradable) -> KeyDown", true)] keyDown = 4,
    KeyUp = 5,
    [Obsolete("Use KeyUp instead (UnityUpgradable) -> KeyUp", true)] keyUp = 5,
    ScrollWheel = 6,
    [Obsolete("Use ScrollWheel instead (UnityUpgradable) -> ScrollWheel", true)] scrollWheel = 6,
    Repaint = 7,
    [Obsolete("Use Repaint instead (UnityUpgradable) -> Repaint", true)] repaint = 7,
    Layout = 8,
    [Obsolete("Use Layout instead (UnityUpgradable) -> Layout", true)] layout = 8,
    DragUpdated = 9,
    [Obsolete("Use DragUpdated instead (UnityUpgradable) -> DragUpdated", true)] dragUpdated = 9,
    DragPerform = 10, // 0x0000000A
    [Obsolete("Use DragPerform instead (UnityUpgradable) -> DragPerform", true)] dragPerform = 10, // 0x0000000A
    Ignore = 11, // 0x0000000B
    [Obsolete("Use Ignore instead (UnityUpgradable) -> Ignore", true)] ignore = 11, // 0x0000000B
    Used = 12, // 0x0000000C
    [Obsolete("Use Used instead (UnityUpgradable) -> Used", true)] used = 12, // 0x0000000C
    ValidateCommand = 13, // 0x0000000D
    ExecuteCommand = 14, // 0x0000000E
    DragExited = 15, // 0x0000000F
    ContextClick = 16, // 0x00000010
    MouseEnterWindow = 20, // 0x00000014
    MouseLeaveWindow = 21, // 0x00000015
  }
}
