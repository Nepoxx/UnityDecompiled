// Decompiled with JetBrains decompiler
// Type: UnityEngine.ParticleSystemShapeType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The emission shape (Shuriken).</para>
  /// </summary>
  public enum ParticleSystemShapeType
  {
    Sphere,
    [Obsolete("SphereShell is deprecated and does nothing. Please use ShapeModule.radiusThickness instead, to control edge emission.")] SphereShell,
    Hemisphere,
    [Obsolete("HemisphereShell is deprecated and does nothing. Please use ShapeModule.radiusThickness instead, to control edge emission.")] HemisphereShell,
    Cone,
    Box,
    Mesh,
    [Obsolete("ConeShell is deprecated and does nothing. Please use ShapeModule.radiusThickness instead, to control edge emission.")] ConeShell,
    ConeVolume,
    [Obsolete("ConeVolumeShell is deprecated and does nothing. Please use ShapeModule.radiusThickness instead, to control edge emission.")] ConeVolumeShell,
    Circle,
    [Obsolete("CircleEdge is deprecated and does nothing. Please use ShapeModule.radiusThickness instead, to control edge emission.")] CircleEdge,
    SingleSidedEdge,
    MeshRenderer,
    SkinnedMeshRenderer,
    BoxShell,
    BoxEdge,
    Donut,
  }
}
