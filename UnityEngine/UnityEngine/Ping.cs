// Decompiled with JetBrains decompiler
// Type: UnityEngine.Ping
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Ping any given IP address (given in dot notation).</para>
  /// </summary>
  public sealed class Ping
  {
    internal IntPtr m_Ptr;

    /// <summary>
    ///   <para>Perform a ping to the supplied target IP address.</para>
    /// </summary>
    /// <param name="address"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Ping(string address);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void DestroyPing();

    ~Ping()
    {
      this.DestroyPing();
    }

    /// <summary>
    ///   <para>Has the ping function completed?</para>
    /// </summary>
    public extern bool isDone { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>This property contains the ping time result after isDone returns true.</para>
    /// </summary>
    public extern int time { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The IP target of the ping.</para>
    /// </summary>
    public extern string ip { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
