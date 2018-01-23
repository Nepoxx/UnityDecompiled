// Decompiled with JetBrains decompiler
// Type: UnityEngine.JointSpring
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>JointSpring is used add a spring force to HingeJoint and PhysicMaterial.</para>
  /// </summary>
  public struct JointSpring
  {
    /// <summary>
    ///   <para>The spring forces used to reach the target position.</para>
    /// </summary>
    public float spring;
    /// <summary>
    ///   <para>The damper force uses to dampen the spring.</para>
    /// </summary>
    public float damper;
    /// <summary>
    ///   <para>The target position the joint attempts to reach.</para>
    /// </summary>
    public float targetPosition;
  }
}
