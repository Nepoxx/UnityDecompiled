// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.Capabilities
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Experimental.UIElements.GraphView
{
  [System.Flags]
  internal enum Capabilities
  {
    Normal = 1,
    Selectable = 2,
    DoesNotCollapse = 4,
    Floating = 8,
    Resizable = 16, // 0x00000010
    Movable = 32, // 0x00000020
    Deletable = 64, // 0x00000040
    Droppable = 128, // 0x00000080
  }
}
