// Decompiled with JetBrains decompiler
// Type: UnityEngine.Cursor
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Cursor API for setting the cursor (mouse pointer).</para>
  /// </summary>
  public sealed class Cursor
  {
    private static void SetCursor(Texture2D texture, CursorMode cursorMode)
    {
      Cursor.SetCursor(texture, Vector2.zero, cursorMode);
    }

    /// <summary>
    ///   <para>Specify a custom cursor that you wish to use as a cursor.</para>
    /// </summary>
    /// <param name="texture">The texture to use for the cursor or null to set the default cursor. Note that a texture needs to be imported with "Read/Write enabled" in the texture importer (or using the "Cursor" defaults), in order to be used as a cursor.</param>
    /// <param name="hotspot">The offset from the top left of the texture to use as the target point (must be within the bounds of the cursor).</param>
    /// <param name="cursorMode">Allow this cursor to render as a hardware cursor on supported platforms, or force software cursor.</param>
    public static void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
    {
      Cursor.INTERNAL_CALL_SetCursor(texture, ref hotspot, cursorMode);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetCursor(Texture2D texture, ref Vector2 hotspot, CursorMode cursorMode);

    /// <summary>
    ///   <para>Determines whether the hardware pointer is visible or not.</para>
    /// </summary>
    public static extern bool visible { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Determines whether the hardware pointer is locked to the center of the view, constrained to the window, or not constrained at all.</para>
    /// </summary>
    public static extern CursorLockMode lockState { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
