// Decompiled with JetBrains decompiler
// Type: UnityEngine.Display
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Provides access to a display / screen for rendering operations.</para>
  /// </summary>
  [UsedByNativeCode]
  public sealed class Display
  {
    /// <summary>
    ///   <para>The list of currently connected Displays. Contains at least one (main) display.</para>
    /// </summary>
    public static Display[] displays = new Display[1]{ new Display() };
    private static Display _mainDisplay = Display.displays[0];
    internal IntPtr nativeDisplay;

    internal Display()
    {
      this.nativeDisplay = new IntPtr(0);
    }

    internal Display(IntPtr nativeDisplay)
    {
      this.nativeDisplay = nativeDisplay;
    }

    /// <summary>
    ///   <para>Horizontal resolution that the display is rendering at.</para>
    /// </summary>
    public int renderingWidth
    {
      get
      {
        int w = 0;
        int h = 0;
        Display.GetRenderingExtImpl(this.nativeDisplay, out w, out h);
        return w;
      }
    }

    /// <summary>
    ///   <para>Vertical resolution that the display is rendering at.</para>
    /// </summary>
    public int renderingHeight
    {
      get
      {
        int w = 0;
        int h = 0;
        Display.GetRenderingExtImpl(this.nativeDisplay, out w, out h);
        return h;
      }
    }

    /// <summary>
    ///   <para>Horizontal native display resolution.</para>
    /// </summary>
    public int systemWidth
    {
      get
      {
        int w = 0;
        int h = 0;
        Display.GetSystemExtImpl(this.nativeDisplay, out w, out h);
        return w;
      }
    }

    /// <summary>
    ///   <para>Vertical native display resolution.</para>
    /// </summary>
    public int systemHeight
    {
      get
      {
        int w = 0;
        int h = 0;
        Display.GetSystemExtImpl(this.nativeDisplay, out w, out h);
        return h;
      }
    }

    /// <summary>
    ///   <para>Color RenderBuffer.</para>
    /// </summary>
    public RenderBuffer colorBuffer
    {
      get
      {
        RenderBuffer color;
        RenderBuffer depth;
        Display.GetRenderingBuffersImpl(this.nativeDisplay, out color, out depth);
        return color;
      }
    }

    /// <summary>
    ///   <para>Depth RenderBuffer.</para>
    /// </summary>
    public RenderBuffer depthBuffer
    {
      get
      {
        RenderBuffer color;
        RenderBuffer depth;
        Display.GetRenderingBuffersImpl(this.nativeDisplay, out color, out depth);
        return depth;
      }
    }

    /// <summary>
    ///   <para>Gets the state of the display and returns true if the display is active and false if otherwise.</para>
    /// </summary>
    public bool active
    {
      get
      {
        return Display.GetActiveImp(this.nativeDisplay);
      }
    }

    /// <summary>
    ///   <para>Activate an external display. Eg. Secondary Monitors connected to the System.</para>
    /// </summary>
    public void Activate()
    {
      Display.ActivateDisplayImpl(this.nativeDisplay, 0, 0, 60);
    }

    /// <summary>
    ///   <para>This overloaded function available for Windows allows specifying desired Window Width, Height and Refresh Rate.</para>
    /// </summary>
    /// <param name="width">Desired Width of the Window (for Windows only. On Linux and Mac uses Screen Width).</param>
    /// <param name="height">Desired Height of the Window (for Windows only. On Linux and Mac uses Screen Height).</param>
    /// <param name="refreshRate">Desired Refresh Rate.</param>
    public void Activate(int width, int height, int refreshRate)
    {
      Display.ActivateDisplayImpl(this.nativeDisplay, width, height, refreshRate);
    }

    /// <summary>
    ///   <para>Set rendering size and position on screen (Windows only).</para>
    /// </summary>
    /// <param name="width">Change Window Width (Windows Only).</param>
    /// <param name="height">Change Window Height (Windows Only).</param>
    /// <param name="x">Change Window Position X (Windows Only).</param>
    /// <param name="y">Change Window Position Y (Windows Only).</param>
    public void SetParams(int width, int height, int x, int y)
    {
      Display.SetParamsImpl(this.nativeDisplay, width, height, x, y);
    }

    /// <summary>
    ///   <para>Sets rendering resolution for the display.</para>
    /// </summary>
    /// <param name="w">Rendering width in pixels.</param>
    /// <param name="h">Rendering height in pixels.</param>
    public void SetRenderingResolution(int w, int h)
    {
      Display.SetRenderingResolutionImpl(this.nativeDisplay, w, h);
    }

    [Obsolete("MultiDisplayLicense has been deprecated.", false)]
    public static bool MultiDisplayLicense()
    {
      return true;
    }

    /// <summary>
    ///   <para>Query relative mouse coordinates.</para>
    /// </summary>
    /// <param name="inputMouseCoordinates">Mouse Input Position as Coordinates.</param>
    public static Vector3 RelativeMouseAt(Vector3 inputMouseCoordinates)
    {
      int rx = 0;
      int ry = 0;
      int x = (int) inputMouseCoordinates.x;
      int y = (int) inputMouseCoordinates.y;
      Vector3 vector3;
      vector3.z = (float) Display.RelativeMouseAtImpl(x, y, out rx, out ry);
      vector3.x = (float) rx;
      vector3.y = (float) ry;
      return vector3;
    }

    /// <summary>
    ///   <para>Main Display.</para>
    /// </summary>
    public static Display main
    {
      get
      {
        return Display._mainDisplay;
      }
    }

    [RequiredByNativeCode]
    private static void RecreateDisplayList(IntPtr[] nativeDisplay)
    {
      Display.displays = new Display[nativeDisplay.Length];
      for (int index = 0; index < nativeDisplay.Length; ++index)
        Display.displays[index] = new Display(nativeDisplay[index]);
      Display._mainDisplay = Display.displays[0];
    }

    [RequiredByNativeCode]
    private static void FireDisplaysUpdated()
    {
      // ISSUE: reference to a compiler-generated field
      if (Display.onDisplaysUpdated == null)
        return;
      // ISSUE: reference to a compiler-generated field
      Display.onDisplaysUpdated();
    }

    public static event Display.DisplaysUpdatedDelegate onDisplaysUpdated = null;

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetSystemExtImpl(IntPtr nativeDisplay, out int w, out int h);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetRenderingExtImpl(IntPtr nativeDisplay, out int w, out int h);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetRenderingBuffersImpl(IntPtr nativeDisplay, out RenderBuffer color, out RenderBuffer depth);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetRenderingResolutionImpl(IntPtr nativeDisplay, int w, int h);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ActivateDisplayImpl(IntPtr nativeDisplay, int width, int height, int refreshRate);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetParamsImpl(IntPtr nativeDisplay, int width, int height, int x, int y);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int RelativeMouseAtImpl(int x, int y, out int rx, out int ry);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool GetActiveImp(IntPtr nativeDisplay);

    public delegate void DisplaysUpdatedDelegate();
  }
}
