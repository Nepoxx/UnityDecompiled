// Decompiled with JetBrains decompiler
// Type: UnityEngine.LocationService
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Interface into location functionality.</para>
  /// </summary>
  public sealed class LocationService
  {
    /// <summary>
    ///   <para>Specifies whether location service is enabled in user settings.</para>
    /// </summary>
    public extern bool isEnabledByUser { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns location service status.</para>
    /// </summary>
    public extern LocationServiceStatus status { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Last measured device geographical location.</para>
    /// </summary>
    public extern LocationInfo lastData { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Starts location service updates.  Last location coordinates could be.</para>
    /// </summary>
    /// <param name="desiredAccuracyInMeters"></param>
    /// <param name="updateDistanceInMeters"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Start([DefaultValue("10f")] float desiredAccuracyInMeters, [DefaultValue("10f")] float updateDistanceInMeters);

    /// <summary>
    ///   <para>Starts location service updates.  Last location coordinates could be.</para>
    /// </summary>
    /// <param name="desiredAccuracyInMeters"></param>
    /// <param name="updateDistanceInMeters"></param>
    [ExcludeFromDocs]
    public void Start(float desiredAccuracyInMeters)
    {
      float updateDistanceInMeters = 10f;
      this.Start(desiredAccuracyInMeters, updateDistanceInMeters);
    }

    [ExcludeFromDocs]
    public void Start()
    {
      this.Start(10f, 10f);
    }

    /// <summary>
    ///   <para>Stops location service updates. This could be useful for saving battery life.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Stop();
  }
}
