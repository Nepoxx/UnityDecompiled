// Decompiled with JetBrains decompiler
// Type: UnityEngine.JointProjectionMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Determines how to snap physics joints back to its constrained position when it drifts off too much.</para>
  /// </summary>
  public enum JointProjectionMode
  {
    None,
    PositionAndRotation,
    [Obsolete("JointProjectionMode.PositionOnly is no longer supported", true)] PositionOnly,
  }
}
