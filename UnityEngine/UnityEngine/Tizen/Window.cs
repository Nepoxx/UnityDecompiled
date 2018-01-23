// Decompiled with JetBrains decompiler
// Type: UnityEngine.Tizen.Window
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Tizen
{
  /// <summary>
  ///   <para>Interface into Tizen specific functionality.</para>
  /// </summary>
  public sealed class Window
  {
    /// <summary>
    ///   <para>Get pointer to the native window handle.</para>
    /// </summary>
    public static IntPtr windowHandle
    {
      get
      {
        IntPtr num;
        Window.INTERNAL_get_windowHandle(out num);
        return num;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_windowHandle(out IntPtr value);

    /// <summary>
    ///   <para>Get pointer to the Tizen EvasGL object..</para>
    /// </summary>
    public static IntPtr evasGL
    {
      get
      {
        IntPtr num;
        Window.INTERNAL_get_evasGL(out num);
        return num;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_evasGL(out IntPtr value);
  }
}
