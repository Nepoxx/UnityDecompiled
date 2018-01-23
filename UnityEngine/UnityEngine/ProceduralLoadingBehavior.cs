// Decompiled with JetBrains decompiler
// Type: UnityEngine.ProceduralLoadingBehavior
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>ProceduralMaterial loading behavior.</para>
  /// </summary>
  [Obsolete("Built-in support for Substance Designer materials has been deprecated and will be removed in Unity 2018.1. To continue using Substance Designer materials in Unity 2018.1, you will need to install a suitable third-party external importer from the Asset Store.", false)]
  public enum ProceduralLoadingBehavior
  {
    DoNothing,
    Generate,
    BakeAndKeep,
    BakeAndDiscard,
    Cache,
    DoNothingAndCache,
  }
}
