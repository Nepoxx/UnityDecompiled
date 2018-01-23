// Decompiled with JetBrains decompiler
// Type: UnityEngine.JointDriveMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The ConfigurableJoint attempts to attain position / velocity targets based on this flag.</para>
  /// </summary>
  [Flags]
  [Obsolete("JointDriveMode is no longer supported")]
  public enum JointDriveMode
  {
    [Obsolete("JointDriveMode.None is no longer supported")] None = 0,
    [Obsolete("JointDriveMode.Position is no longer supported")] Position = 1,
    [Obsolete("JointDriveMode.Velocity is no longer supported")] Velocity = 2,
    [Obsolete("JointDriveMode.PositionAndvelocity is no longer supported")] PositionAndVelocity = Velocity | Position, // 0x00000003
  }
}
