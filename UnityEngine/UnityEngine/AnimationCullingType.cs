// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimationCullingType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>This enum controlls culling of Animation component.</para>
  /// </summary>
  public enum AnimationCullingType
  {
    AlwaysAnimate,
    BasedOnRenderers,
    [Obsolete("Enum member AnimatorCullingMode.BasedOnClipBounds has been deprecated. Use AnimationCullingType.AlwaysAnimate or AnimationCullingType.BasedOnRenderers instead")] BasedOnClipBounds,
    [Obsolete("Enum member AnimatorCullingMode.BasedOnUserBounds has been deprecated. Use AnimationCullingType.AlwaysAnimate or AnimationCullingType.BasedOnRenderers instead")] BasedOnUserBounds,
  }
}
