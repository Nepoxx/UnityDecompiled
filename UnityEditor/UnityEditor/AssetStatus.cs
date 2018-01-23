// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStatus
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  [Obsolete("AssetStatus enum is not used anymore (Asset Server has been removed)")]
  public enum AssetStatus
  {
    Calculating = -1,
    ClientOnly = 0,
    ServerOnly = 1,
    Unchanged = 2,
    Conflict = 3,
    Same = 4,
    NewVersionAvailable = 5,
    NewLocalVersion = 6,
    RestoredFromTrash = 7,
    Ignored = 8,
    BadState = 9,
  }
}
