// Decompiled with JetBrains decompiler
// Type: UnityEngine.HumanPose
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Retargetable humanoid pose.</para>
  /// </summary>
  public struct HumanPose
  {
    /// <summary>
    ///   <para>The human body position for that pose.</para>
    /// </summary>
    public Vector3 bodyPosition;
    /// <summary>
    ///   <para>The human body orientation for that pose.</para>
    /// </summary>
    public Quaternion bodyRotation;
    /// <summary>
    ///   <para>The array of muscle values for that pose.</para>
    /// </summary>
    public float[] muscles;

    internal void Init()
    {
      if (this.muscles != null && this.muscles.Length != HumanTrait.MuscleCount)
        throw new ArgumentException("Bad array size for HumanPose.muscles. Size must equal HumanTrait.MuscleCount");
      if (this.muscles != null)
        return;
      this.muscles = new float[HumanTrait.MuscleCount];
      if ((double) this.bodyRotation.x == 0.0 && (double) this.bodyRotation.y == 0.0 && ((double) this.bodyRotation.z == 0.0 && (double) this.bodyRotation.w == 0.0))
        this.bodyRotation.w = 1f;
    }
  }
}
