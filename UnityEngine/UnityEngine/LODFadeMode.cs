// Decompiled with JetBrains decompiler
// Type: UnityEngine.LODFadeMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>The LOD fade modes. Modes other than LODFadeMode.None will result in Unity calculating a blend factor for blending/interpolating between two neighbouring LODs and pass it to your shader.</para>
  /// </summary>
  public enum LODFadeMode
  {
    None,
    CrossFade,
    SpeedTree,
  }
}
