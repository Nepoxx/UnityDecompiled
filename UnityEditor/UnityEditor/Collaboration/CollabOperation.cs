// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.CollabOperation
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Collaboration
{
  [System.Flags]
  internal enum CollabOperation : ulong
  {
    Noop = 0,
    Publish = 1,
    Update = 2,
    Revert = 4,
    GoBack = 8,
    Restore = 16, // 0x0000000000000010
    Diff = 32, // 0x0000000000000020
    ConflictDiff = 64, // 0x0000000000000040
    Exclude = 128, // 0x0000000000000080
    Include = 256, // 0x0000000000000100
    ChooseMine = 512, // 0x0000000000000200
    ChooseTheirs = 1024, // 0x0000000000000400
    ExternalMerge = 2048, // 0x0000000000000800
  }
}
