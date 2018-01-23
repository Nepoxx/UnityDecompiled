// Decompiled with JetBrains decompiler
// Type: UnityEngine.AndroidInput
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>AndroidInput provides support for off-screen touch input, such as a touchpad.</para>
  /// </summary>
  public sealed class AndroidInput
  {
    private AndroidInput()
    {
    }

    /// <summary>
    ///   <para>Returns object representing status of a specific touch on a secondary touchpad (Does not allocate temporary variables).</para>
    /// </summary>
    /// <param name="index"></param>
    public static Touch GetSecondaryTouch(int index)
    {
      Touch touch;
      AndroidInput.INTERNAL_CALL_GetSecondaryTouch(index, out touch);
      return touch;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetSecondaryTouch(int index, out Touch value);

    /// <summary>
    ///   <para>Number of secondary touches. Guaranteed not to change throughout the frame. (Read Only).</para>
    /// </summary>
    public static extern int touchCountSecondary { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Property indicating whether the system provides secondary touch input.</para>
    /// </summary>
    public static extern bool secondaryTouchEnabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Property indicating the width of the secondary touchpad.</para>
    /// </summary>
    public static extern int secondaryTouchWidth { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Property indicating the height of the secondary touchpad.</para>
    /// </summary>
    public static extern int secondaryTouchHeight { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
