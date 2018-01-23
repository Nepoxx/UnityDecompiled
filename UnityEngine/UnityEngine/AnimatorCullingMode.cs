// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimatorCullingMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Culling mode for the Animator.</para>
  /// </summary>
  public enum AnimatorCullingMode
  {
    AlwaysAnimate = 0,
    [Obsolete("Enum member AnimatorCullingMode.BasedOnRenderers has been deprecated. Use AnimatorCullingMode.CullUpdateTransforms instead (UnityUpgradable) -> CullUpdateTransforms", true)] BasedOnRenderers = 1,
    CullUpdateTransforms = 1,
    CullCompletely = 2,
  }
}
