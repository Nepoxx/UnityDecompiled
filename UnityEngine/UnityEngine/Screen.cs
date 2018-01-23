// Decompiled with JetBrains decompiler
// Type: UnityEngine.Screen
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Access to display information.</para>
  /// </summary>
  public sealed class Screen
  {
    /// <summary>
    ///   <para>All fullscreen resolutions supported by the monitor (Read Only).</para>
    /// </summary>
    public static extern Resolution[] resolutions { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The current screen resolution (Read Only).</para>
    /// </summary>
    public static Resolution currentResolution
    {
      get
      {
        Resolution resolution;
        Screen.INTERNAL_get_currentResolution(out resolution);
        return resolution;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_currentResolution(out Resolution value);

    /// <summary>
    ///   <para>Switches the screen resolution.</para>
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="fullscreen"></param>
    /// <param name="preferredRefreshRate"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetResolution(int width, int height, bool fullscreen, [DefaultValue("0")] int preferredRefreshRate);

    /// <summary>
    ///   <para>Switches the screen resolution.</para>
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="fullscreen"></param>
    /// <param name="preferredRefreshRate"></param>
    [ExcludeFromDocs]
    public static void SetResolution(int width, int height, bool fullscreen)
    {
      int preferredRefreshRate = 0;
      Screen.SetResolution(width, height, fullscreen, preferredRefreshRate);
    }

    /// <summary>
    ///   <para>Is the game running fullscreen?</para>
    /// </summary>
    public static extern bool fullScreen { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The current width of the screen window in pixels (Read Only).</para>
    /// </summary>
    public static extern int width { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The current height of the screen window in pixels (Read Only).</para>
    /// </summary>
    public static extern int height { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The current DPI of the screen / device (Read Only).</para>
    /// </summary>
    public static extern float dpi { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Specifies logical orientation of the screen.</para>
    /// </summary>
    public static extern ScreenOrientation orientation { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A power saving setting, allowing the screen to dim some time after the last active user interaction.</para>
    /// </summary>
    public static extern int sleepTimeout { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool IsOrientationEnabled(EnabledOrientation orient);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetOrientationEnabled(EnabledOrientation orient, bool enabled);

    /// <summary>
    ///   <para>Allow auto-rotation to portrait?</para>
    /// </summary>
    public static bool autorotateToPortrait
    {
      get
      {
        return Screen.IsOrientationEnabled(EnabledOrientation.kAutorotateToPortrait);
      }
      set
      {
        Screen.SetOrientationEnabled(EnabledOrientation.kAutorotateToPortrait, value);
      }
    }

    /// <summary>
    ///   <para>Allow auto-rotation to portrait, upside down?</para>
    /// </summary>
    public static bool autorotateToPortraitUpsideDown
    {
      get
      {
        return Screen.IsOrientationEnabled(EnabledOrientation.kAutorotateToPortraitUpsideDown);
      }
      set
      {
        Screen.SetOrientationEnabled(EnabledOrientation.kAutorotateToPortraitUpsideDown, value);
      }
    }

    /// <summary>
    ///   <para>Allow auto-rotation to landscape left?</para>
    /// </summary>
    public static bool autorotateToLandscapeLeft
    {
      get
      {
        return Screen.IsOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeLeft);
      }
      set
      {
        Screen.SetOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeLeft, value);
      }
    }

    /// <summary>
    ///   <para>Allow auto-rotation to landscape right?</para>
    /// </summary>
    public static bool autorotateToLandscapeRight
    {
      get
      {
        return Screen.IsOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeRight);
      }
      set
      {
        Screen.SetOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeRight, value);
      }
    }

    /// <summary>
    ///   <para>Returns the safe area of the screen in pixels (Read Only).</para>
    /// </summary>
    public static Rect safeArea
    {
      get
      {
        Rect ret;
        Screen.get_safeArea_Injected(out ret);
        return ret;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property GetResolution has been deprecated. Use resolutions instead (UnityUpgradable) -> resolutions", true)]
    public static Resolution[] GetResolution
    {
      get
      {
        return (Resolution[]) null;
      }
    }

    /// <summary>
    ///   <para>Should the cursor be visible?</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property showCursor has been deprecated. Use Cursor.visible instead (UnityUpgradable) -> UnityEngine.Cursor.visible", true)]
    public static bool showCursor { get; set; }

    /// <summary>
    ///   <para>Should the cursor be locked?</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use Cursor.lockState and Cursor.visible instead.", false)]
    public static bool lockCursor
    {
      get
      {
        return CursorLockMode.Locked == Cursor.lockState;
      }
      set
      {
        if (value)
        {
          Cursor.visible = false;
          Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
          Cursor.lockState = CursorLockMode.None;
          Cursor.visible = true;
        }
      }
    }

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void get_safeArea_Injected(out Rect ret);
  }
}
