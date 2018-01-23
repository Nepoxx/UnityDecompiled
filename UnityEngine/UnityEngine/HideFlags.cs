// Decompiled with JetBrains decompiler
// Type: UnityEngine.HideFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Bit mask that controls object destruction, saving and visibility in inspectors.</para>
  /// </summary>
  [Flags]
  public enum HideFlags
  {
    None = 0,
    HideInHierarchy = 1,
    HideInInspector = 2,
    DontSaveInEditor = 4,
    NotEditable = 8,
    DontSaveInBuild = 16, // 0x00000010
    DontUnloadUnusedAsset = 32, // 0x00000020
    DontSave = DontUnloadUnusedAsset | DontSaveInBuild | DontSaveInEditor, // 0x00000034
    HideAndDontSave = DontSave | NotEditable | HideInHierarchy, // 0x0000003D
  }
}
