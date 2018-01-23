// Decompiled with JetBrains decompiler
// Type: UnityEngine.WSA.Cursor
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.WSA
{
  /// <summary>
  ///   <para>Cursor API for Windows Store Apps.</para>
  /// </summary>
  public sealed class Cursor
  {
    /// <summary>
    ///   <para>Set a custom cursor.</para>
    /// </summary>
    /// <param name="id">The cursor resource id.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetCustomCursor(uint id);
  }
}
