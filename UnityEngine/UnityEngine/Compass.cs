// Decompiled with JetBrains decompiler
// Type: UnityEngine.Compass
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Interface into compass functionality.</para>
  /// </summary>
  public sealed class Compass
  {
    /// <summary>
    ///   <para>The heading in degrees relative to the magnetic North Pole. (Read Only)</para>
    /// </summary>
    public extern float magneticHeading { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The heading in degrees relative to the geographic North Pole. (Read Only)</para>
    /// </summary>
    public extern float trueHeading { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Accuracy of heading reading in degrees.</para>
    /// </summary>
    public extern float headingAccuracy { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The raw geomagnetic data measured in microteslas. (Read Only)</para>
    /// </summary>
    public Vector3 rawVector
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_rawVector(out vector3);
        return vector3;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_rawVector(out Vector3 value);

    /// <summary>
    ///   <para>Timestamp (in seconds since 1970) when the heading was last time updated. (Read Only)</para>
    /// </summary>
    public extern double timestamp { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Used to enable or disable compass. Note, that if you want Input.compass.trueHeading property to contain a valid value, you must also enable location updates by calling Input.location.Start().</para>
    /// </summary>
    public extern bool enabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
