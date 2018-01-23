// Decompiled with JetBrains decompiler
// Type: UnityEngine.ClothSkinningCoefficient
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The ClothSkinningCoefficient struct is used to set up how a Cloth component is allowed to move with respect to the SkinnedMeshRenderer it is attached to.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct ClothSkinningCoefficient
  {
    /// <summary>
    ///   <para>Distance a vertex is allowed to travel from the skinned mesh vertex position.</para>
    /// </summary>
    public float maxDistance;
    /// <summary>
    ///   <para>Definition of a sphere a vertex is not allowed to enter. This allows collision against the animated cloth.</para>
    /// </summary>
    public float collisionSphereDistance;
  }
}
